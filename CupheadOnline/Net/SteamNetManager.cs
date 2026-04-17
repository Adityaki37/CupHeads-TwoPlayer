using System;
using System.IO;
using Steamworks;
using UnityEngine;
using CupheadOnline.Sync;
using CupheadOnline.UI;

namespace CupheadOnline.Net
{
    // ────────────────────────────────────────────────────────────────────────────
    //  HANDSHAKE STATE MACHINE
    //
    //  HOST side:
    //    Idle
    //    → CreatingLobby   (CreateLobby call in flight)
    //    → WaitingInLobby  (lobby ready, invite overlay shown, waiting for knock)
    //    → WaitingHello    (P2PSessionRequest accepted)
    //    → WaitingReady    (Welcome sent, waiting for Ready)
    //    → Connected       (Ready received, game can sync)
    //
    //  CLIENT side:
    //    Idle
    //    → JoiningLobby    (JoinLobby call in flight)
    //    → WaitingWelcome  (Hello sent, waiting for Welcome)
    //    → Connected       (Ready sent and first game-packet ACKed)
    //
    //  Any failure/timeout at any state → Error state, status text updated.
    //  Disconnect while Connected → back to WaitingInLobby (host) or Error (client).
    // ────────────────────────────────────────────────────────────────────────────

    public sealed class SteamNetManager
    {
        // ── Public ────────────────────────────────────────────────────────────
        public bool IsSteamReady => _steamReady;
        public bool IsConnected  => _state == NetState.Connected;
        public int  Latency      { get; private set; }
        public string SteamUnavailableStatus => _steamUnavailableStatus;

        /// <summary>True while a critical async operation is in flight (host/join pending).</summary>
        public bool IsInputLocked => _state == NetState.CreatingLobby
                                  || _state == NetState.WaitingInLobby
                                  || _state == NetState.JoiningLobby
                                  || _state == NetState.WaitingHello
                                  || _state == NetState.WaitingWelcome
                                  || _state == NetState.WaitingReady;

        /// <summary>True when waiting in lobby indefinitely (no handshake timeout applies).</summary>
        public bool IsWaitingIndefinitely => _state == NetState.WaitingInLobby;

        /// <summary>True when we are in a Steam lobby (host or guest).</summary>
        public bool IsInLobby => _lobbyId != CSteamID.Nil;

        /// <summary>When the current state was entered (Time.realtimeSinceStartup).</summary>
        public float StateEnteredTime => _stateEnteredTime;

        public bool ShouldForceUnlockUi(float now)
        {
            if (!_steamReady) return false;

            switch (_state)
            {
                case NetState.CreatingLobby:
                case NetState.JoiningLobby:
                    return now - _stateEnteredTime > LOBBY_TIMEOUT + 5f;

                case NetState.WaitingHello:
                case NetState.WaitingWelcome:
                case NetState.WaitingReady:
                    return now - _stateEnteredTime > HANDSHAKE_TIMEOUT + 5f;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns a multi-line string with lobby member names and connection state,
        /// suitable for the presence panel in the MP menu.
        /// Returns empty string when not in a lobby.
        /// </summary>
        public string GetLobbyPresence()
        {
            if (!_steamReady || _lobbyId == CSteamID.Nil) return string.Empty;
            int n = SteamMatchmaking.GetNumLobbyMembers(_lobbyId);
            if (n <= 0) return string.Empty;

            CSteamID localId = SteamUser.GetSteamID();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("LOBBY");
            for (int i = 0; i < n; i++)
            {
                CSteamID member = SteamMatchmaking.GetLobbyMemberByIndex(_lobbyId, i);
                string name = (member == localId)
                    ? "You"
                    : SteamFriends.GetFriendPersonaName(member);
                string role   = (i == 0) ? "Host" : "Guest";
                string suffix = string.Empty;

                // Annotate the remote peer with handshake progress
                if (member != localId && member == _peerId)
                {
                    switch (_state)
                    {
                        case NetState.WaitingHello:
                        case NetState.WaitingWelcome:
                        case NetState.WaitingReady:
                            suffix = " (Connecting\u2026)";
                            break;
                        case NetState.Connected:
                            suffix = " (Connected)";
                            break;
                    }
                }
                sb.AppendLine(role + ": " + name + suffix);
            }
            return sb.ToString().TrimEnd();
        }

        /// <summary>Fired on the main thread with human-readable status.</summary>
        public Action<string> OnStatusChanged;

        /// <summary>Fired on the main thread when the Steam overlay is closed by the user.</summary>
        public Action OnOverlayClosed;

        // ── State ─────────────────────────────────────────────────────────────
        enum NetState
        {
            Idle,
            CreatingLobby,
            WaitingInLobby,   // host: invite overlay shown
            JoiningLobby,     // client: JoinLobby call in flight
            WaitingHello,     // host: session accepted, waiting Hello
            WaitingWelcome,   // client: Hello sent, waiting Welcome
            WaitingReady,     // host: Welcome sent, waiting Ready
            Connected,
            Error,
        }

        NetState _state  = NetState.Idle;
        bool     _isHost;
        CSteamID _peerId  = CSteamID.Nil;
        CSteamID _lobbyId = CSteamID.Nil;

        // ── Callbacks (hold references — prevent GC) ──────────────────────────
        Callback<P2PSessionRequest_t>      _cbP2PReq;
        Callback<P2PSessionConnectFail_t>  _cbP2PFail;
        Callback<GameLobbyJoinRequested_t> _cbLobbyJoinReq;
        Callback<LobbyChatUpdate_t>        _cbLobbyChatUpd;
        Callback<GameOverlayActivated_t>   _cbOverlay;
        CallResult<LobbyCreated_t>         _crLobbyCreated;
        CallResult<LobbyEnter_t>           _crLobbyEntered;

        // ── Buffers ───────────────────────────────────────────────────────────
        readonly byte[]       _recvBuf    = new byte[65536];
        readonly MemoryStream _sendBuf    = new MemoryStream(256);
        readonly System.IO.BinaryWriter _sendWriter;

        // ── Timing ────────────────────────────────────────────────────────────
        const float HANDSHAKE_TIMEOUT = 12f;   // seconds
        const float LOBBY_TIMEOUT     = 20f;
        const int   PEER_TIMEOUT_MS   = 30_000;
        const float PING_INTERVAL     = 3f;

        float    _stateEnteredTime;            // Time.realtimeSinceStartup at state change
        DateTime _lastReceive  = DateTime.UtcNow;
        float    _nextPingTime;
        DateTime _pingSentAt;
        bool     _pingSentPending;
        bool     _steamInitAttempted;
        bool     _steamReady;
        string   _steamUnavailableStatus = "Steam is unavailable.\nLaunch Cuphead through Steam.";

        // ── Constructor ───────────────────────────────────────────────────────

        public SteamNetManager()
        {
            _sendWriter = new System.IO.BinaryWriter(_sendBuf);
            Plugin.Log.LogInfo("[SteamNet] Created.");
        }

        public bool TryInitializeSteam()
        {
            if (_steamReady) return true;
            if (_steamInitAttempted) return false;

            _steamInitAttempted     = true;
            _steamUnavailableStatus = BuildSteamUnavailableStatus();

            try
            {
                if (!SteamAPI.Init())
                {
                    Plugin.Log.LogWarning("[SteamNet] SteamAPI.Init() returned false.");
                    Plugin.Log.LogWarning("[SteamNet] " + _steamUnavailableStatus.Replace('\n', ' '));
                    return false;
                }

                _steamReady      = true;
                _cbP2PReq        = Callback<P2PSessionRequest_t>.Create(OnP2PSessionRequest);
                _cbP2PFail       = Callback<P2PSessionConnectFail_t>.Create(OnP2PConnectFailRaw);
                _cbLobbyJoinReq  = Callback<GameLobbyJoinRequested_t>.Create(OnLobbyJoinRequestedRaw);
                _cbLobbyChatUpd  = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdateRaw);
                _cbOverlay       = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
                Plugin.Log.LogInfo("[SteamNet] Steam initialized.");
                return true;
            }
            catch (DllNotFoundException ex)
            {
                Plugin.Log.LogError("[SteamNet] Steam DLL missing: " + ex.Message);
                Plugin.Log.LogWarning("[SteamNet] " + _steamUnavailableStatus.Replace('\n', ' '));
                return false;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogError("[SteamNet] Steam initialization failed: " + ex);
                Plugin.Log.LogWarning("[SteamNet] " + _steamUnavailableStatus.Replace('\n', ' '));
                return false;
            }
        }

        // ── Host flow ─────────────────────────────────────────────────────────

        public bool StartHost()
        {
            if (!EnsureSteamReady()) return false;

            Shutdown();
            _isHost = true;
            SteamNetworking.AllowP2PPacketRelay(true);
            MultiplayerSession.StartAsHost();

            SetState(NetState.CreatingLobby, "Creating lobby...");
            _crLobbyCreated = CallResult<LobbyCreated_t>.Create(OnLobbyCreated);
            _crLobbyCreated.Set(SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypeFriendsOnly, 2));
            return true;
        }

        void OnLobbyCreated(LobbyCreated_t r, bool ioFail)
        {
            if (!_steamReady) return;

            if (ioFail || r.m_eResult != EResult.k_EResultOK)
            {
                MultiplayerSession.End();
                SetState(NetState.Error,
                    "Could not create lobby.\nPlease press B and try again.");
                Plugin.Log.LogWarning("[SteamNet] Lobby creation failed: " + r.m_eResult);
                return;
            }
            _lobbyId = new CSteamID(r.m_ulSteamIDLobby);
            SteamMatchmaking.SetLobbyData(_lobbyId, "game", "CupheadOnline");

            SetState(NetState.WaitingInLobby,
                "Waiting for player...\n"
                + "Invite a friend using the Steam overlay.");
            Plugin.Log.LogInfo("[SteamNet] Lobby: " + _lobbyId);
            SteamFriends.ActivateGameOverlayInviteDialog(_lobbyId);
        }

        void OnP2PSessionRequest(P2PSessionRequest_t req)
        {
            if (!_steamReady) return;

            if (_lobbyId != CSteamID.Nil && !IsLobbyMember(req.m_steamIDRemote))
            {
                Plugin.Log.LogWarning("[SteamNet] Rejected P2P from non-lobby peer.");
                return;
            }
            SteamNetworking.AcceptP2PSessionWithUser(req.m_steamIDRemote);
            _peerId = req.m_steamIDRemote;
            SetState(NetState.WaitingHello,
                "Player connecting...");
            Plugin.Log.LogInfo("[SteamNet] P2P accepted from " + FriendName(_peerId));
        }

        // ── Client flow ───────────────────────────────────────────────────────

        public bool JoinLobby(CSteamID lobbyId)
        {
            if (!EnsureSteamReady()) return false;

            Shutdown();
            _isHost = false;
            SteamNetworking.AllowP2PPacketRelay(true);
            SetState(NetState.JoiningLobby, "Joining lobby...");
            _crLobbyEntered = CallResult<LobbyEnter_t>.Create(OnLobbyEntered);
            _crLobbyEntered.Set(SteamMatchmaking.JoinLobby(lobbyId));
            return true;
        }

        // Thread-safe wrapper — Steam callback may arrive on any thread
        void OnLobbyJoinRequestedRaw(GameLobbyJoinRequested_t req)
        {
            var id = req.m_steamIDLobby;
            MainThreadQueue.Enqueue(() =>
            {
                Plugin.Log.LogInfo("[SteamNet] Invite accepted — joining " + id);
                JoinLobby(id);
            });
        }

        void OnLobbyEntered(LobbyEnter_t r, bool ioFail)
        {
            if (!_steamReady) return;

            if (ioFail || r.m_EChatRoomEnterResponse
                       != (uint)EChatRoomEnterResponse.k_EChatRoomEnterResponseSuccess)
            {
                SetState(NetState.Error,
                    "Could not join lobby.\nPlease press B and try again.");
                return;
            }
            _lobbyId = new CSteamID(r.m_ulSteamIDLobby);
            _peerId  = SteamMatchmaking.GetLobbyOwner(_lobbyId);

            MultiplayerSession.StartAsClient();
            SetState(NetState.WaitingWelcome,
                "Connecting to " + FriendName(_peerId) + "...");

            // Knock: sending Hello triggers P2PSessionRequest on host side
            RawSend(new[] { (byte)PacketType.Hello }, reliable: true);
            Plugin.Log.LogInfo("[SteamNet] Hello sent to " + FriendName(_peerId));
        }

        // ── Handshake message handlers ────────────────────────────────────────

        void OnHelloReceived()
        {
            // host ← Hello
            RawSend(new[] { (byte)PacketType.Welcome }, reliable: true);
            SetState(NetState.WaitingReady,
                "Almost there\u2026");
            Plugin.Log.LogInfo("[SteamNet] Hello received, Welcome sent.");
        }

        void OnWelcomeReceived()
        {
            // client ← Welcome
            RawSend(new[] { (byte)PacketType.Ready }, reliable: true);
            FinishConnect();
            Plugin.Log.LogInfo("[SteamNet] Welcome received, Ready sent.");
        }

        void OnReadyReceived()
        {
            // host ← Ready
            FinishConnect();
            Plugin.Log.LogInfo("[SteamNet] Ready received — fully connected.");
        }

        void FinishConnect()
        {
            if (_isHost)
            {
                if (!MultiplayerSession.IsActive || !MultiplayerSession.IsHost)
                    MultiplayerSession.StartAsHost();
            }
            else if (!MultiplayerSession.IsActive || MultiplayerSession.IsHost)
            {
                MultiplayerSession.StartAsClient();
            }

            _lastReceive = DateTime.UtcNow;
            string name  = FriendName(_peerId);
            SetState(NetState.Connected, "Connected - " + name);

            string role = _isHost
                ? "Host: You\nGuest: " + name
                : "Host: " + name + "\nGuest: You";
            FireStatus(role);

            ConnectionHUD.Show("Connected - " + name);

            if (_isHost)
            {
                var pkt = new SessionStartPacket
                {
                    CurrentLevel = (int)SceneLoader.CurrentLevel,
                    CurrentTick  = MultiplayerSession.Tick,
                    RngSeed      = RngSync.CurrentSeed,
                };
                Send(PacketType.SessionStart, ref pkt, reliable: true);
            }
        }

        // ── Disconnect / failure ──────────────────────────────────────────────

        // Thread-safe wrapper
        void OnP2PConnectFailRaw(P2PSessionConnectFail_t cb)
        {
            var err = cb.m_eP2PSessionError;
            MainThreadQueue.Enqueue(() =>
            {
                Plugin.Log.LogWarning("[SteamNet] P2P fail, error=" + err);
                HandleDisconnect("Connection error (" + err + ")");
            });
        }

        // Thread-safe wrapper
        void OnLobbyChatUpdateRaw(LobbyChatUpdate_t cb)
        {
            bool left = (cb.m_rgfChatMemberStateChange &
                         (uint)EChatMemberStateChange.k_EChatMemberStateChangeLeft) != 0
                     || (cb.m_rgfChatMemberStateChange &
                         (uint)EChatMemberStateChange.k_EChatMemberStateChangeDisconnected) != 0;
            if (!left) return;

            var changed = new CSteamID(cb.m_ulSteamIDUserChanged);
            if (changed != _peerId) return;

            string name = FriendName(changed);
            MainThreadQueue.Enqueue(() =>
            {
                Plugin.Log.LogInfo("[SteamNet] " + name + " left the lobby.");
                HandleDisconnect(name + " disconnected.");
            });
        }

        // Thread-safe wrapper — fired when the Steam overlay opens or closes
        void OnGameOverlayActivated(GameOverlayActivated_t cb)
        {
            if (cb.m_bActive != 0) return;   // 0 = overlay closed
            MainThreadQueue.Enqueue(() => OnOverlayClosed?.Invoke());
        }

        void HandleDisconnect(string reason)
        {
            bool wasConnected = _state == NetState.Connected;
            if (_steamReady && _peerId != CSteamID.Nil)
                SteamNetworking.CloseP2PSessionWithUser(_peerId);
            _peerId          = CSteamID.Nil;
            _pingSentPending = false;
            MultiplayerSession.End();

            if (_isHost && _lobbyId != CSteamID.Nil)
            {
                // Host stays in lobby — reset to WaitingInLobby so another player can join
                SetState(NetState.WaitingInLobby,
                    reason + "\n\nWaiting for next player...\n"
                    + "Invite a friend using the Steam overlay.");
                if (wasConnected) ConnectionHUD.ShowDisconnected(reason);
            }
            else
            {
                SetState(NetState.Error,
                    "Could not connect to player.\nPress B and try again.");
                if (wasConnected) ConnectionHUD.ShowDisconnected(reason);
            }
        }

        // ── Poll (every frame from Plugin.Update) ─────────────────────────────

        public void Poll()
        {
            if (!_steamReady) return;

            try
            {
                SteamAPI.RunCallbacks();
                DrainP2PPackets();
            }
            catch (InvalidOperationException ex)
            {
                HandleSteamRuntimeFailure("polling", ex);
                return;
            }

            if (_state == NetState.Idle || _state == NetState.Error) return;

            float now     = Time.realtimeSinceStartup;
            float elapsed = now - _stateEnteredTime;

            // Handshake / lobby timeouts
            switch (_state)
            {
                case NetState.CreatingLobby:
                    if (elapsed > LOBBY_TIMEOUT)
                    {
                        MultiplayerSession.End();
                        SetState(NetState.Error,
                            "Could not create lobby.\nPlease press B and try again.");
                    }
                    return;

                case NetState.JoiningLobby:
                    if (elapsed > LOBBY_TIMEOUT)
                    {
                        MultiplayerSession.End();
                        SetState(NetState.Error,
                            "Could not connect to player.\nPress B and try again.");
                    }
                    return;

                case NetState.WaitingHello:
                case NetState.WaitingWelcome:
                case NetState.WaitingReady:
                    if (elapsed > HANDSHAKE_TIMEOUT)
                        HandleDisconnect("Handshake timed out.");
                    return;

                case NetState.WaitingInLobby:
                    return;   // no timeout — host waits indefinitely
            }

            // Connected: keepalive + peer timeout check
            if ((DateTime.UtcNow - _lastReceive).TotalMilliseconds > PEER_TIMEOUT_MS)
            {
                HandleDisconnect("Peer timed out.");
                return;
            }

            if (now > _nextPingTime)
            {
                _nextPingTime    = now + PING_INTERVAL;
                _pingSentAt      = DateTime.UtcNow;
                _pingSentPending = true;
                RawSend(new[] { (byte)PacketType.Ping }, reliable: false);
            }
        }

        void DrainP2PPackets()
        {
            if (!_steamReady) return;

            uint size;
            while (SteamNetworking.IsP2PPacketAvailable(out size))
            {
                if (size > (uint)_recvBuf.Length)
                {
                    uint d1; CSteamID d2;
                    SteamNetworking.ReadP2PPacket(_recvBuf, (uint)_recvBuf.Length, out d1, out d2);
                    continue;
                }
                uint     read;
                CSteamID sender;
                if (SteamNetworking.ReadP2PPacket(_recvBuf, size, out read, out sender) && read > 0)
                    ProcessPacket(_recvBuf, (int)read, sender);
            }
        }

        void ProcessPacket(byte[] buf, int length, CSteamID sender)
        {
            if (length == 0) return;
            _lastReceive = DateTime.UtcNow;

            byte type = buf[0];

            // ── Handshake ─────────────────────────────────────────────────────
            if (type == (byte)PacketType.Hello)
            {
                if (_isHost && (_state == NetState.WaitingHello || _state == NetState.WaitingInLobby))
                {
                    if (_peerId == CSteamID.Nil) _peerId = sender;
                    OnHelloReceived();
                }
                return;
            }
            if (type == (byte)PacketType.Welcome)
            {
                if (!_isHost && _state == NetState.WaitingWelcome) OnWelcomeReceived();
                return;
            }
            if (type == (byte)PacketType.Ready)
            {
                if (_isHost && _state == NetState.WaitingReady) OnReadyReceived();
                return;
            }

            // ── Ping / Pong ───────────────────────────────────────────────────
            if (type == (byte)PacketType.Ping)
            {
                RawSend(new[] { (byte)PacketType.Pong }, reliable: false);
                return;
            }
            if (type == (byte)PacketType.Pong && _pingSentPending)
            {
                _pingSentPending = false;
                Latency = (int)(DateTime.UtcNow - _pingSentAt).TotalMilliseconds;
                ConnectionHUD.UpdatePing(Latency);
                return;
            }

            // ── Graceful disconnect ───────────────────────────────────────────
            if (type == (byte)PacketType.Disconnect)
            {
                HandleDisconnect("Remote disconnected.");
                return;
            }

            // ── Game packets — only accepted when fully connected ─────────────
            if (_state != NetState.Connected) return;
            if (_peerId != CSteamID.Nil && sender != _peerId) return;

            using (var ms = new MemoryStream(buf, 1, length - 1, false))
            using (var r  = new BinaryReader(ms))
                PacketDispatcher.Dispatch((PacketType)type, r);
        }

        // ── Send helpers ──────────────────────────────────────────────────────

        public void SendPlayerState (ref PlayerStatePacket  p) => Send(PacketType.PlayerState,  ref p, false);
        public void SendInputFrame  (ref InputFramePacket   p) => Send(PacketType.InputFrame,   ref p, false);
        public void SendEnemyState  (ref EnemyStatePacket   p) => Send(PacketType.EnemyState,   ref p, false);
        public void SendWeaponEvent (ref WeaponEventPacket  p) => Send(PacketType.WeaponEvent,  ref p, true);
        public void SendDamageEvent (ref DamageEventPacket  p) => Send(PacketType.DamageEvent,  ref p, true);
        public void SendSceneChange (ref SceneChangePacket  p) => Send(PacketType.SceneChange,  ref p, true);
        public void SendLobbySync   (ref LobbySyncPacket    p) => Send(PacketType.LobbySync,    ref p, true);

        void Send<T>(PacketType type, ref T pkt, bool reliable) where T : struct, IPacket
        {
            if (!_steamReady) return;
            if (_state != NetState.Connected && type != PacketType.SessionStart) return;
            _sendBuf.SetLength(0);
            _sendBuf.Position = 0;
            _sendWriter.Write((byte)type);
            pkt.Write(_sendWriter);
            _sendWriter.Flush();
            int len  = (int)_sendBuf.Length;
            var data = new byte[len];
            Buffer.BlockCopy(_sendBuf.GetBuffer(), 0, data, 0, len);
            RawSend(data, reliable);
        }

        void RawSend(byte[] data, bool reliable)
        {
            if (!_steamReady) return;
            if (_peerId == CSteamID.Nil) return;
            var mode = reliable ? EP2PSend.k_EP2PSendReliable : EP2PSend.k_EP2PSendUnreliable;
            SteamNetworking.SendP2PPacket(_peerId, data, (uint)data.Length, mode);
        }

        // ── Shutdown ──────────────────────────────────────────────────────────

        public void Shutdown()
        {
            bool hadState = _state != NetState.Idle
                         || _peerId != CSteamID.Nil
                         || _lobbyId != CSteamID.Nil;

            if (_steamReady && _peerId != CSteamID.Nil)
            {
                try { SteamNetworking.SendP2PPacket(_peerId, new[] { (byte)PacketType.Disconnect }, 1, EP2PSend.k_EP2PSendReliable); } catch { }
                SteamNetworking.CloseP2PSessionWithUser(_peerId);
            }
            if (_steamReady && _lobbyId != CSteamID.Nil)
            {
                SteamMatchmaking.LeaveLobby(_lobbyId);
            }
            _peerId          = CSteamID.Nil;
            _lobbyId         = CSteamID.Nil;
            _pingSentPending = false;
            _isHost          = false;
            Latency          = 0;
            _state           = NetState.Idle;
            ConnectionHUD.Hide();
            MultiplayerSession.End();
            if (hadState)
                Plugin.Log.LogInfo("[SteamNet] Shutdown.");
        }

        public void Dispose()
        {
            Shutdown();
            if (!_steamReady) return;

            try
            {
                SteamAPI.Shutdown();
                Plugin.Log.LogInfo("[SteamNet] Steam shutdown.");
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning("[SteamNet] Steam shutdown failed: " + ex.Message);
            }
            finally
            {
                _steamReady      = false;
                _cbP2PReq        = null;
                _cbP2PFail       = null;
                _cbLobbyJoinReq  = null;
                _cbLobbyChatUpd  = null;
                _cbOverlay       = null;
                _crLobbyCreated  = null;
                _crLobbyEntered  = null;
            }
        }

        /// <summary>Kept for source compat — Steam flow uses JoinLobby/invite.</summary>
        public void Connect(string ip, int port = 0) =>
            Plugin.Log.LogWarning("[SteamNet] Connect(ip) ignored — use Steam invite.");

        // ── Helpers ──────────────────────────────────────────────────────────

        void SetState(NetState s, string status)
        {
            _state            = s;
            _stateEnteredTime = Time.realtimeSinceStartup;
            FireStatus(status);
            Plugin.Log.LogInfo("[SteamNet] → " + s + ": " + status);
        }

        void FireStatus(string msg) => OnStatusChanged?.Invoke(msg);

        string FriendName(CSteamID id)
        {
            if (!_steamReady || id == CSteamID.Nil) return "Unknown Player";
            return SteamFriends.GetFriendPersonaName(id);
        }

        bool IsLobbyMember(CSteamID id)
        {
            if (!_steamReady) return false;
            if (_lobbyId == CSteamID.Nil) return true;
            int n = SteamMatchmaking.GetNumLobbyMembers(_lobbyId);
            for (int i = 0; i < n; i++)
                if (SteamMatchmaking.GetLobbyMemberByIndex(_lobbyId, i) == id) return true;
            return false;
        }

        bool EnsureSteamReady()
        {
            if (_steamReady) return true;

            if (!_steamInitAttempted)
                TryInitializeSteam();

            if (_steamReady) return true;

            SetState(NetState.Error, _steamUnavailableStatus);
            return false;
        }

        string BuildSteamUnavailableStatus()
        {
            string status = "Steam is unavailable.\nLaunch Cuphead through Steam.";

            try
            {
                string gameRoot = Path.GetDirectoryName(Application.dataPath);
                if (!string.IsNullOrEmpty(gameRoot))
                {
                    string appIdPath = Path.Combine(gameRoot, "steam_appid.txt");
                    if (!File.Exists(appIdPath))
                        status += "\nIf testing outside Steam, add steam_appid.txt next to Cuphead.exe.";
                }
            }
            catch
            {
                // Best-effort hint only.
            }

            return status;
        }

        void HandleSteamRuntimeFailure(string context, Exception ex)
        {
            if (!_steamReady) return;

            _steamReady = false;
            _peerId = CSteamID.Nil;
            _lobbyId = CSteamID.Nil;
            _isHost = false;
            _pingSentPending = false;
            _steamUnavailableStatus = BuildSteamUnavailableStatus();
            Plugin.Log.LogError("[SteamNet] Steam runtime failure while " + context + ": " + ex.Message);
            ConnectionHUD.Hide();
            MultiplayerSession.End();

            if (_state != NetState.Idle && _state != NetState.Error)
                SetState(NetState.Error, _steamUnavailableStatus);
        }
    }
}
