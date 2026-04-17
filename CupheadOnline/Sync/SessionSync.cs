using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using CupheadOnline.Net;

namespace CupheadOnline.Sync
{
    public enum SessionIssueSeverity
    {
        None = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
    }

    public static class SessionSync
    {
        private static SaveProfilePacket? _localSaveProfile;
        private static SaveProfilePacket? _remoteSaveProfile;
        private static SessionSnapshotPacket? _lastHostSnapshot;

        private static byte _trackedSaveSlot = byte.MaxValue;
        private static Scenes _trackedMapScene = Scenes.scene_map_world_1;
        private static bool _trackedSaveEmpty;
        private static bool _hasTrackedSave;
        private static bool _hasCompletedHandshake;

        private static float _nextHostSnapshotAt;
        private static float _lastHostSnapshotAt = -1f;
        private static int _sceneMismatchStreak;

        private static string _compatibilitySummary = "Compatibility: waiting for host save.";
        private static SessionIssueSeverity _compatibilitySeverity = SessionIssueSeverity.Info;

        private static string _desyncSummary = string.Empty;
        private static SessionIssueSeverity _desyncSeverity = SessionIssueSeverity.None;

        private static int _localDeaths;
        private static int _localRetries;
        private static int _localParries;

        static SessionSync()
        {
            MultiplayerSession.OnSessionStarted += HandleSessionStarted;
            MultiplayerSession.OnSessionEnded += HandleSessionEnded;
        }

        public static string CompatibilitySummary => _compatibilitySummary;
        public static SessionIssueSeverity CompatibilitySeverity => _compatibilitySeverity;
        public static string DesyncSummary => _desyncSummary;
        public static SessionIssueSeverity DesyncSeverity => _desyncSeverity;
        public static int LocalDeaths => _localDeaths;
        public static int LocalRetries => _localRetries;
        public static int LocalParries => _localParries;

        public static void Update()
        {
            if (!MultiplayerSession.IsActive || Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            if (MultiplayerSession.IsHost)
            {
                if (Time.unscaledTime >= _nextHostSnapshotAt)
                {
                    BroadcastSessionSnapshot(true);
                    _nextHostSnapshotAt = Time.unscaledTime + 1f;
                }
            }
            else
            {
                EvaluateDesync();
            }
        }

        public static void OnConnected(bool isHost)
        {
            _hasCompletedHandshake = true;
            _desyncSummary = string.Empty;
            _desyncSeverity = SessionIssueSeverity.None;
            _sceneMismatchStreak = 0;
            _nextHostSnapshotAt = Time.unscaledTime + 0.25f;

            if (isHost)
            {
                if (!_hasTrackedSave)
                {
                    string sceneName = GetActiveSceneName();
                    bool canRecoverContext = !string.Equals(sceneName, "scene_title", StringComparison.OrdinalIgnoreCase);
                    if (canRecoverContext)
                    {
                        _trackedSaveSlot = (byte)Mathf.Clamp(PlayerData.CurrentSaveFileIndex, 0, 2);
                        _trackedMapScene = PlayerData.Data != null ? PlayerData.Data.CurrentMap : Scenes.scene_map_world_1;
                        _trackedSaveEmpty = false;
                        _hasTrackedSave = true;
                    }
                }

                if (_hasTrackedSave)
                {
                    CaptureLocalSaveProfile(_trackedSaveSlot, _trackedSaveEmpty);
                    BroadcastSelectedSaveProfile();
                }

                BroadcastSessionSnapshot(true);
            }
            else if (_hasTrackedSave)
            {
                BroadcastGuestSaveProfile();
            }
            else
            {
                _compatibilitySummary = "Compatibility: waiting for host save.";
                _compatibilitySeverity = SessionIssueSeverity.Info;
            }
        }

        public static void RecordSelectedSave(ref SaveSlotSyncPacket pkt)
        {
            _trackedSaveSlot = pkt.SlotIndex;
            _trackedMapScene = (Scenes)pkt.CurrentMapScene;
            _trackedSaveEmpty = pkt.IsEmpty;
            _hasTrackedSave = true;

            CaptureLocalSaveProfile(pkt.SlotIndex, pkt.IsEmpty);
            EvaluateCompatibility();
        }

        public static void ApplyRemoteSaveSelection(ref SaveSlotSyncPacket pkt)
        {
            _trackedSaveSlot = pkt.SlotIndex;
            _trackedMapScene = (Scenes)pkt.CurrentMapScene;
            _trackedSaveEmpty = pkt.IsEmpty;
            _hasTrackedSave = true;

            CaptureLocalSaveProfile(pkt.SlotIndex, pkt.IsEmpty);
            BroadcastGuestSaveProfile();
            EvaluateCompatibility();
        }

        public static void ApplyRemoteSaveProfile(SaveProfilePacket pkt)
        {
            _remoteSaveProfile = pkt;
            if (!_hasTrackedSave)
            {
                _trackedSaveSlot = pkt.SlotIndex;
                _trackedMapScene = (Scenes)pkt.CurrentMapScene;
                _trackedSaveEmpty = pkt.IsEmpty;
                _hasTrackedSave = true;
            }

            EvaluateCompatibility();
        }

        public static void ApplyHostSnapshot(SessionSnapshotPacket pkt)
        {
            _lastHostSnapshot = pkt;
            _lastHostSnapshotAt = Time.unscaledTime;
            EvaluateDesync();
        }

        public static void RecordLocalDeath()
        {
            if (!MultiplayerSession.IsActive) return;
            _localDeaths++;
        }

        public static void RecordLocalRetry()
        {
            if (!MultiplayerSession.IsActive) return;
            _localRetries++;
        }

        public static void RecordLocalParry()
        {
            if (!MultiplayerSession.IsActive) return;
            _localParries++;
        }

        public static string GetFooterHint()
        {
            if (Plugin.Net == null)
                return string.Empty;

            if (Plugin.Net.IsConnected)
            {
                if (Plugin.Net.IsHost)
                {
                    if (!_hasTrackedSave)
                        return "Guest connected - press Start to choose a save";

                    return _compatibilitySeverity >= SessionIssueSeverity.Warning
                        ? "Save synced - review compatibility warning"
                        : "Save synced - start the run when ready";
                }

                if (!_hasTrackedSave)
                    return "Connected - waiting for host save";

                if (_desyncSeverity >= SessionIssueSeverity.Warning)
                    return _desyncSummary;

                return _compatibilitySeverity >= SessionIssueSeverity.Warning
                    ? _compatibilitySummary
                    : "Host save synced - following host";
            }

            if (Plugin.Net.IsInLobby)
                return Plugin.Net.IsHost
                    ? "Lobby ready - invite or share the lobby ID"
                    : "Lobby joined - finishing connection";

            return string.Empty;
        }

        public static string GetStageSummary()
        {
            if (Plugin.Net == null)
                return "OFFLINE";

            if (!Plugin.Net.IsConnected)
            {
                if (Plugin.Net.IsInLobby)
                    return Plugin.Net.IsHost ? "HOSTING LOBBY" : "JOINING HOST";
                return Plugin.Net.IsSteamReady ? "NOT CONNECTED" : "STEAM UNAVAILABLE";
            }

            if (Plugin.Net.IsHost)
                return _hasTrackedSave ? "HOST READY" : "WAITING FOR SAVE";

            return _hasTrackedSave ? "FOLLOWING HOST" : "WAITING FOR HOST SAVE";
        }

        public static string GetMenuPresenceSummary()
        {
            if (!MultiplayerSession.IsActive && (Plugin.Net == null || !Plugin.Net.IsConnected))
                return string.Empty;

            var sb = new StringBuilder();
            sb.AppendLine("Session: " + GetStageSummary());

            if (_hasTrackedSave)
            {
                sb.Append("Save: SLOT ");
                sb.Append(_trackedSaveSlot + 1);
                sb.Append(" | ");
                sb.Append(_trackedMapScene);
                if (_trackedSaveEmpty)
                    sb.Append(" | EMPTY");
                sb.AppendLine();
            }

            if (!string.IsNullOrEmpty(_compatibilitySummary))
            {
                sb.Append("Check: ");
                sb.AppendLine(_compatibilitySummary.Replace(Environment.NewLine, " "));
            }

            if (!string.IsNullOrEmpty(_desyncSummary))
            {
                sb.Append("Sync: ");
                sb.AppendLine(_desyncSummary.Replace(Environment.NewLine, " "));
            }

            return sb.ToString().TrimEnd();
        }

        public static string BuildPausePanelText()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Stage: " + GetStageSummary());

            if (_hasTrackedSave)
            {
                sb.Append("Save Slot: ");
                sb.Append(_trackedSaveSlot + 1);
                sb.Append(" | Map: ");
                sb.AppendLine(_trackedMapScene.ToString());
            }
            else
            {
                sb.AppendLine("Save Slot: waiting for host");
            }

            sb.Append("Save Check: ");
            sb.AppendLine(string.IsNullOrEmpty(_compatibilitySummary) ? "No save data yet." : _compatibilitySummary);

            sb.Append("Sync Check: ");
            sb.AppendLine(string.IsNullOrEmpty(_desyncSummary) ? "Following host." : _desyncSummary);

            sb.Append("Stats: ");
            sb.Append(_localDeaths);
            sb.Append(" deaths | ");
            sb.Append(_localRetries);
            sb.Append(" retries | ");
            sb.Append(_localParries);
            sb.Append(" parries");

            return sb.ToString();
        }

        public static string BuildDiagnosticsSection()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Stage: " + GetStageSummary());
            sb.AppendLine("Tracked Save: " + (_hasTrackedSave ? (_trackedSaveSlot + 1).ToString() : "(none)"));
            sb.AppendLine("Compatibility: " + _compatibilitySummary);
            sb.AppendLine("Compatibility Severity: " + _compatibilitySeverity);
            sb.AppendLine("Desync: " + (string.IsNullOrEmpty(_desyncSummary) ? "(none)" : _desyncSummary));
            sb.AppendLine("Desync Severity: " + _desyncSeverity);
            sb.AppendLine("Stats: deaths=" + _localDeaths + ", retries=" + _localRetries + ", parries=" + _localParries);

            if (_lastHostSnapshot.HasValue)
            {
                var snap = _lastHostSnapshot.Value;
                sb.AppendLine("Host Scene: " + snap.SceneName);
                sb.AppendLine("Host Level: " + snap.CurrentLevel);
                sb.AppendLine("Host Tick: " + snap.HostTick);
            }

            return sb.ToString().TrimEnd();
        }

        public static Color GetSeverityColor(SessionIssueSeverity severity)
        {
            switch (severity)
            {
                case SessionIssueSeverity.Error:
                    return new Color(0.95f, 0.35f, 0.30f, 1f);
                case SessionIssueSeverity.Warning:
                    return new Color(0.98f, 0.80f, 0.38f, 1f);
                case SessionIssueSeverity.Info:
                    return new Color(0.72f, 0.86f, 0.96f, 1f);
                default:
                    return new Color(0.78f, 0.90f, 0.70f, 1f);
            }
        }

        public static void BroadcastSelectedSaveProfile()
        {
            if (!_hasTrackedSave || Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            BroadcastSaveProfile(_trackedSaveSlot, _trackedMapScene, _trackedSaveEmpty);
        }

        public static void BroadcastGuestSaveProfile()
        {
            if (!_hasTrackedSave || Plugin.Net == null || !Plugin.Net.IsConnected || MultiplayerSession.IsHost)
                return;

            BroadcastSaveProfile(_trackedSaveSlot, _trackedMapScene, _trackedSaveEmpty);
        }

        public static void BroadcastSessionSnapshot(bool reliable)
        {
            if (Plugin.Net == null || !Plugin.Net.IsConnected || !MultiplayerSession.IsHost)
                return;

            var pkt = new SessionSnapshotPacket
            {
                SaveSlotIndex = _hasTrackedSave ? _trackedSaveSlot : byte.MaxValue,
                Flags = BuildSnapshotFlags(),
                CurrentLevel = Level.Current != null ? (int)Level.Current.CurrentLevel : -1,
                CurrentMapScene = PlayerData.Data != null ? (int)PlayerData.Data.CurrentMap : -1,
                HostTick = MultiplayerSession.Tick,
                SceneName = GetActiveSceneName(),
            };

            Plugin.Net.SendSessionSnapshot(ref pkt, reliable);
        }

        private static void BroadcastSaveProfile(byte slotIndex, Scenes mapScene, bool isEmpty)
        {
            if (Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            var pkt = BuildProfile(slotIndex, mapScene, isEmpty);
            _localSaveProfile = pkt;
            Plugin.Net.SendSaveProfile(ref pkt);
        }

        private static void CaptureLocalSaveProfile(byte slotIndex, bool isEmpty)
        {
            Scenes mapScene = PlayerData.Data != null ? PlayerData.Data.CurrentMap : Scenes.scene_map_world_1;
            if (_hasTrackedSave)
                mapScene = _trackedMapScene;

            _localSaveProfile = BuildProfile(slotIndex, mapScene, isEmpty);
        }

        private static SaveProfilePacket BuildProfile(byte slotIndex, Scenes mapScene, bool isEmpty)
        {
            int clampedSlot = Mathf.Clamp(slotIndex, 0, 2);
            var data = PlayerData.GetDataForSlot(clampedSlot);

            if (data != null && !isEmpty)
            {
                mapScene = data.CurrentMap;
                if (!DLCManager.DLCEnabled() && mapScene == Scenes.scene_map_world_DLC)
                    mapScene = Scenes.scene_map_world_1;
            }

            byte flags = 0;
            if (isEmpty) flags |= 1;
            if (DLCManager.DLCEnabled()) flags |= 2;
            if (data != null && data.isPlayer1Mugman) flags |= 4;

            byte weapon1 = 0;
            byte weapon2 = 0;
            byte super = 0;
            byte charm = 0;
            float baseCompletion = 0f;
            float dlcCompletion = 0f;
            ushort coins = 0;

            if (data != null)
            {
                baseCompletion = data.GetCompletionPercentage();
                dlcCompletion = DLCManager.DLCEnabled() ? data.GetCompletionPercentageDLC() : 0f;
                coins = (ushort)Math.Min((int)ushort.MaxValue, Math.Max(0, data.NumCoinsCollected));

                var loadouts = data.Loadouts;
                if (loadouts != null)
                {
                    var loadout = loadouts.GetPlayerLoadout(PlayerId.PlayerOne);
                    weapon1 = (byte)loadout.primaryWeapon;
                    weapon2 = (byte)loadout.secondaryWeapon;
                    super = (byte)loadout.super;
                    charm = (byte)loadout.charm;
                }
            }

            return new SaveProfilePacket
            {
                SlotIndex = (byte)clampedSlot,
                Flags = flags,
                CurrentMapScene = (int)mapScene,
                CompletionPct = baseCompletion,
                CompletionPctDlc = dlcCompletion,
                Coins = coins,
                Weapon1 = weapon1,
                Weapon2 = weapon2,
                Super = super,
                Charm = charm,
            };
        }

        private static void EvaluateCompatibility()
        {
            if (!_hasTrackedSave)
            {
                _compatibilitySummary = "Compatibility: waiting for host save.";
                _compatibilitySeverity = SessionIssueSeverity.Info;
                return;
            }

            if (!_remoteSaveProfile.HasValue)
            {
                _compatibilitySummary = MultiplayerSession.IsHost
                    ? "Compatibility: waiting for guest save data."
                    : "Compatibility: sending your save summary.";
                _compatibilitySeverity = SessionIssueSeverity.Info;
                return;
            }

            if (!_localSaveProfile.HasValue || _localSaveProfile.Value.SlotIndex != _trackedSaveSlot)
                CaptureLocalSaveProfile(_trackedSaveSlot, _trackedSaveEmpty);

            if (!_localSaveProfile.HasValue)
            {
                _compatibilitySummary = "Compatibility: local save data unavailable.";
                _compatibilitySeverity = SessionIssueSeverity.Warning;
                return;
            }

            var local = _localSaveProfile.Value;
            var remote = _remoteSaveProfile.Value;

            var issues = new List<string>();
            SessionIssueSeverity severity = SessionIssueSeverity.None;

            bool remoteNeedsDlc = (Scenes)remote.CurrentMapScene == Scenes.scene_map_world_DLC;
            if (remoteNeedsDlc && !DLCManager.DLCEnabled())
            {
                severity = SessionIssueSeverity.Error;
                issues.Add("DLC world selected but DLC is not enabled locally.");
            }

            if (!remote.IsEmpty && local.IsEmpty)
            {
                severity = MaxSeverity(severity, SessionIssueSeverity.Warning);
                issues.Add("Host save is populated, but your matching slot is empty.");
            }

            float completionDelta = Mathf.Abs(remote.CompletionPct - local.CompletionPct);
            if (completionDelta >= 20f)
            {
                severity = MaxSeverity(severity, SessionIssueSeverity.Warning);
                issues.Add("Base-game progression differs by " + completionDelta.ToString("0") + "%.");
            }

            float dlcDelta = Mathf.Abs(remote.CompletionPctDlc - local.CompletionPctDlc);
            if (dlcDelta >= 15f)
            {
                severity = MaxSeverity(severity, SessionIssueSeverity.Warning);
                issues.Add("DLC progression differs by " + dlcDelta.ToString("0") + "%.");
            }

            int coinDelta = Mathf.Abs(remote.Coins - local.Coins);
            if (coinDelta >= 10)
            {
                severity = MaxSeverity(severity, SessionIssueSeverity.Warning);
                issues.Add("Coin totals differ by " + coinDelta + ".");
            }

            if (remote.CurrentMapScene != local.CurrentMapScene && !remote.IsEmpty && !local.IsEmpty)
            {
                severity = MaxSeverity(severity, SessionIssueSeverity.Info);
                issues.Add("Current map progress does not match.");
            }

            if (issues.Count == 0)
            {
                _compatibilitySummary = "Compatibility looks good.";
                _compatibilitySeverity = SessionIssueSeverity.None;
            }
            else
            {
                _compatibilitySummary = issues[0];
                if (issues.Count > 1)
                    _compatibilitySummary += " +" + (issues.Count - 1) + " more";
                _compatibilitySeverity = severity;
            }
        }

        private static void EvaluateDesync()
        {
            if (!Plugin.Net.IsConnected || MultiplayerSession.IsHost)
            {
                _desyncSummary = string.Empty;
                _desyncSeverity = SessionIssueSeverity.None;
                return;
            }

            if (!_lastHostSnapshot.HasValue)
            {
                _desyncSummary = _hasCompletedHandshake
                    ? "Waiting for host snapshot..."
                    : string.Empty;
                _desyncSeverity = _hasCompletedHandshake ? SessionIssueSeverity.Info : SessionIssueSeverity.None;
                return;
            }

            var snapshot = _lastHostSnapshot.Value;
            string localScene = GetActiveSceneName();
            bool sceneMismatch = !string.IsNullOrEmpty(snapshot.SceneName)
                              && !string.IsNullOrEmpty(localScene)
                              && !string.Equals(snapshot.SceneName, localScene, StringComparison.OrdinalIgnoreCase);

            if (sceneMismatch)
                _sceneMismatchStreak++;
            else
                _sceneMismatchStreak = 0;

            if (_sceneMismatchStreak >= 2)
            {
                _desyncSummary = "Scene mismatch - host is in " + snapshot.SceneName + ".";
                _desyncSeverity = SessionIssueSeverity.Error;
                return;
            }

            uint localTick = MultiplayerSession.Tick;
            uint hostTick = snapshot.HostTick;
            uint tickDelta = localTick > hostTick ? localTick - hostTick : hostTick - localTick;
            if (snapshot.IsInLevel && tickDelta > 240)
            {
                _desyncSummary = "Simulation drift detected (" + tickDelta + " ticks).";
                _desyncSeverity = SessionIssueSeverity.Warning;
                return;
            }

            if (_lastHostSnapshotAt > 0f && Time.unscaledTime - _lastHostSnapshotAt > 4f)
            {
                _desyncSummary = "Host snapshots stalled - retry if gameplay diverges.";
                _desyncSeverity = SessionIssueSeverity.Warning;
                return;
            }

            _desyncSummary = string.Empty;
            _desyncSeverity = SessionIssueSeverity.None;
        }

        private static byte BuildSnapshotFlags()
        {
            byte flags = 0;
            if (_hasTrackedSave) flags |= 1;
            if (Level.Current != null) flags |= 2;
            if (PauseManager.state == PauseManager.State.Paused) flags |= 4;
            return flags;
        }

        private static string GetActiveSceneName()
        {
            try
            {
                var scene = SceneManager.GetActiveScene();
                return string.IsNullOrEmpty(scene.name) ? string.Empty : scene.name;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static SessionIssueSeverity MaxSeverity(SessionIssueSeverity left, SessionIssueSeverity right)
        {
            return left >= right ? left : right;
        }

        private static void HandleSessionStarted()
        {
            _localSaveProfile = null;
            _remoteSaveProfile = null;
            _lastHostSnapshot = null;
            _trackedSaveSlot = byte.MaxValue;
            _trackedMapScene = Scenes.scene_map_world_1;
            _trackedSaveEmpty = false;
            _hasTrackedSave = false;
            _hasCompletedHandshake = false;
            _nextHostSnapshotAt = 0f;
            _lastHostSnapshotAt = -1f;
            _sceneMismatchStreak = 0;
            _compatibilitySummary = "Compatibility: waiting for host save.";
            _compatibilitySeverity = SessionIssueSeverity.Info;
            _desyncSummary = string.Empty;
            _desyncSeverity = SessionIssueSeverity.None;
            _localDeaths = 0;
            _localRetries = 0;
            _localParries = 0;
        }

        private static void HandleSessionEnded()
        {
            _localSaveProfile = null;
            _remoteSaveProfile = null;
            _lastHostSnapshot = null;
            _trackedSaveSlot = byte.MaxValue;
            _trackedMapScene = Scenes.scene_map_world_1;
            _trackedSaveEmpty = false;
            _hasTrackedSave = false;
            _hasCompletedHandshake = false;
            _nextHostSnapshotAt = 0f;
            _lastHostSnapshotAt = -1f;
            _sceneMismatchStreak = 0;
            _compatibilitySummary = "Compatibility: waiting for host save.";
            _compatibilitySeverity = SessionIssueSeverity.Info;
            _desyncSummary = string.Empty;
            _desyncSeverity = SessionIssueSeverity.None;
            _localDeaths = 0;
            _localRetries = 0;
            _localParries = 0;
        }
    }
}
