using System;
using CupheadOnline.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupheadOnline.Sync
{
    internal static class LevelStartSync
    {
        const float HostTimeout = 12f;
        const float ClientTimeout = 18f;

        static bool _hostGateActive;
        static bool _hostSceneLoaded;
        static bool _hostGuestLoaded;
        static bool _hostReleaseSent;
        static Levels _hostLevel;
        static float _hostGateStartedAt;
        static float _hostLocalReleaseAt;
        static float _hostReleaseDelaySeconds;

        static bool _clientGateActive;
        static bool _clientLoadedSent;
        static bool _clientReleaseReceived;
        static Levels _clientLevel;
        static float _clientGateStartedAt;

        static bool _holdingTimeScale;
        static float _lastLogAt;

        public static bool IsHostWaitingForGuestStart => _hostGateActive;
        public static bool IsClientWaitingForStartRelease => _clientGateActive && !_clientReleaseReceived;
        public static float LastHostReleaseDelaySeconds => _hostReleaseDelaySeconds;

        public static void BeginHostLevelLoad(Levels level)
        {
            if (!MultiplayerSession.IsActive || !MultiplayerSession.IsHost || Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            _hostGateActive = true;
            _hostSceneLoaded = false;
            _hostGuestLoaded = false;
            _hostReleaseSent = false;
            _hostLevel = level;
            _hostGateStartedAt = Time.unscaledTime;
            _hostLocalReleaseAt = 0f;
            _hostReleaseDelaySeconds = 0f;
            _lastLogAt = -100f;
            Plugin.Log.LogInfo("[LevelStartSync] Host waiting for guest to load " + level + ".");
        }

        public static void BeginClientLevelLoad(Levels level)
        {
            if (!MultiplayerSession.IsActive || MultiplayerSession.IsHost || Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            _clientGateActive = true;
            _clientLoadedSent = false;
            _clientReleaseReceived = false;
            _clientLevel = level;
            _clientGateStartedAt = Time.unscaledTime;
            _lastLogAt = -100f;
            Plugin.Log.LogInfo("[LevelStartSync] Client waiting for host start release for " + level + ".");
        }

        public static void HandleRemoteLevelLoaded(ushort levelToken)
        {
            if (!MultiplayerSession.IsHost || !_hostGateActive)
                return;

            if (levelToken != ToToken(_hostLevel))
            {
                Plugin.Log.LogInfo("[LevelStartSync] Ignored loaded signal for stale level token " + levelToken + ".");
                return;
            }

            _hostGuestLoaded = true;
            Plugin.Log.LogInfo("[LevelStartSync] Guest loaded " + _hostLevel + ".");
        }

        public static void HandleRemoteLevelStartRelease(ushort levelToken)
        {
            if (MultiplayerSession.IsHost || !_clientGateActive)
                return;

            if (levelToken != ToToken(_clientLevel))
            {
                Plugin.Log.LogInfo("[LevelStartSync] Ignored release signal for stale level token " + levelToken + ".");
                return;
            }

            _clientReleaseReceived = true;
            Plugin.Log.LogInfo("[LevelStartSync] Host released level start for " + _clientLevel + ".");
        }

        public static void NotifyLocalTransitionInComplete(Level level)
        {
            if (level == null || !MultiplayerSession.IsActive || Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            ushort levelToken = ToToken(level.CurrentLevel);

            if (MultiplayerSession.IsHost)
            {
                if (!_hostGateActive || levelToken != ToToken(_hostLevel))
                    return;

                _hostSceneLoaded = true;
                HoldLevelStart("host-transition");
                Plugin.Log.LogInfo("[LevelStartSync] Host transition complete for " + _hostLevel + ".");
                return;
            }

            if (!_clientGateActive || levelToken != ToToken(_clientLevel))
                return;

            HoldLevelStart("client-transition");
            SendClientLoadedIfNeeded();
            Plugin.Log.LogInfo("[LevelStartSync] Client transition complete for " + _clientLevel + ".");
        }

        public static void Update()
        {
            if (!MultiplayerSession.IsActive || Plugin.Net == null || !Plugin.Net.IsConnected)
            {
                ClearAll();
                return;
            }

            if (MultiplayerSession.IsHost)
                UpdateHost();
            else
                UpdateClient();
        }

        static void UpdateHost()
        {
            if (!_hostGateActive)
                return;

            if (_hostSceneLoaded)
                HoldLevelStart("host");

            if (_hostReleaseSent)
            {
                if (Time.unscaledTime >= _hostLocalReleaseAt)
                {
                    ReleaseLocal("host-latency-aligned");
                    _hostGateActive = false;
                    return;
                }

                LogWaiting("Host release sent; holding local start for "
                    + Mathf.Max(0f, _hostLocalReleaseAt - Time.unscaledTime).ToString("0.00")
                    + "s so the guest receives the release.");
                return;
            }

            if (_hostSceneLoaded && _hostGuestLoaded)
            {
                ReleaseHostAndGuest();
                return;
            }

            if (Time.unscaledTime - _hostGateStartedAt > HostTimeout)
            {
                Plugin.Log.LogWarning("[LevelStartSync] Guest did not report level load before timeout; releasing host.");
                ReleaseHostAndGuest();
            }
            else
            {
                LogWaiting("Host level start is paused until the guest finishes loading.");
            }
        }

        static void UpdateClient()
        {
            if (!_clientGateActive)
                return;

            if (IsInLevelScene() && _clientLoadedSent)
            {
                HoldLevelStart("client");
            }

            if (_clientReleaseReceived)
            {
                ReleaseLocal("client");
                _clientGateActive = false;
                return;
            }

            if (Time.unscaledTime - _clientGateStartedAt > ClientTimeout)
            {
                Plugin.Log.LogWarning("[LevelStartSync] Host release did not arrive before timeout; releasing client.");
                ReleaseLocal("client-timeout");
                _clientGateActive = false;
            }
            else
            {
                LogWaiting("Client level start is paused until the host releases both players.");
            }
        }

        static void SendClientLoadedIfNeeded()
        {
            if (_clientLoadedSent)
                return;

            SendSignal(SessionSignalKind.LevelLoaded, ToToken(_clientLevel));
            _clientLoadedSent = true;
            Plugin.Log.LogInfo("[LevelStartSync] Client reported transition-ready for " + _clientLevel + ".");
        }

        static void ReleaseHostAndGuest()
        {
            if (!_hostReleaseSent)
            {
                SendSignal(SessionSignalKind.LevelStartRelease, ToToken(_hostLevel));
                _hostReleaseSent = true;
                _hostReleaseDelaySeconds = EstimateOneWayReleaseDelaySeconds();
                _hostLocalReleaseAt = Time.unscaledTime + _hostReleaseDelaySeconds;
                Plugin.Log.LogInfo("[LevelStartSync] Released level start for "
                    + _hostLevel
                    + "; delaying host local release by "
                    + _hostReleaseDelaySeconds.ToString("0.000")
                    + "s for latency alignment.");
            }

            if (_hostReleaseDelaySeconds <= 0.001f)
            {
                ReleaseLocal("host");
                _hostGateActive = false;
            }
        }

        static void HoldLevelStart(string role)
        {
            if (PauseManager.state == PauseManager.State.Paused)
                return;

            if (Time.timeScale != 0f)
                Time.timeScale = 0f;
            _holdingTimeScale = true;
        }

        static void ReleaseLocal(string role)
        {
            if (_holdingTimeScale && PauseManager.state != PauseManager.State.Paused)
                Time.timeScale = 1f;
            _holdingTimeScale = false;
            Plugin.Log.LogInfo("[LevelStartSync] Local level start gate released on " + role + ".");
        }

        static void ClearAll()
        {
            if (_holdingTimeScale && PauseManager.state != PauseManager.State.Paused)
                Time.timeScale = 1f;
            _holdingTimeScale = false;
            _hostGateActive = false;
            _clientGateActive = false;
        }

        static void SendSignal(SessionSignalKind kind, ushort levelToken)
        {
            if (Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            var pkt = new SessionSignalPacket
            {
                Signal = (byte)kind,
                SaveRevision = levelToken,
            };
            Plugin.Net.SendSessionSignal(ref pkt);
        }

        public static float EstimateOneWayReleaseDelaySeconds()
        {
            float delay = 0f;

            if (Plugin.Net != null && Plugin.Net.Latency > 0)
                delay = Mathf.Max(delay, Plugin.Net.Latency * 0.0005f);

            if (Plugin.LanArtificialLatencyMs > 0)
                delay = Mathf.Max(delay, Plugin.LanArtificialLatencyMs / 1000f);

            return Mathf.Clamp(delay, 0f, 2.5f);
        }

        static ushort ToToken(Levels level)
        {
            return unchecked((ushort)((int)level & 0xffff));
        }

        static bool IsInLevelScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return !string.IsNullOrEmpty(sceneName)
                && sceneName.StartsWith("scene_level_", StringComparison.Ordinal);
        }

        static void LogWaiting(string message)
        {
            if (Time.unscaledTime - _lastLogAt <= 2f)
                return;

            _lastLogAt = Time.unscaledTime;
            Plugin.Log.LogInfo("[LevelStartSync] " + message);
        }
    }
}
