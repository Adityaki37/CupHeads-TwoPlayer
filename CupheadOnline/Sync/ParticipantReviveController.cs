using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CupheadOnline.Net;
using HarmonyLib;
using UnityEngine;

namespace CupheadOnline.Sync
{
    public static class ParticipantReviveController
    {
        sealed class ScheduledHostBuiltInParry
        {
            public PlayerDeathEffect Effect;
            public float ExecuteAt;
        }

        sealed class ScheduledClientBuiltInParry
        {
            public ReviveVisualPacket Packet;
            public long Key;
        }

        static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        static readonly FieldInfo DeathEffectPlayerIdField =
            typeof(PlayerDeathEffect).GetField("playerId", AnyInstance);
        static readonly FieldInfo DeathEffectExitingField =
            typeof(PlayerDeathEffect).GetField("exiting", AnyInstance);
        static readonly FieldInfo DeathEffectParrySwitchField =
            typeof(PlayerDeathEffect).GetField("parrySwitch", AnyInstance);
        static readonly MethodInfo ParrySwitchOnParryPrePauseMethod =
            typeof(ParrySwitch).GetMethod("OnParryPrePause", AnyInstance);
        static readonly MethodInfo ParrySwitchOnParryPostPauseMethod =
            typeof(ParrySwitch).GetMethod("OnParryPostPause", AnyInstance);
        static readonly MethodInfo PlayerDeathEffectOnParrySwitchMethod =
            AccessTools.Method(typeof(PlayerDeathEffect), "OnParrySwitch");
        static readonly MethodInfo PlayerDeathEffectOnReviveParryAnimCompleteMethod =
            AccessTools.Method(typeof(PlayerDeathEffect), "OnReviveParryAnimComplete");
        static readonly MethodInfo PlayerStatsOnStatsDeathMethod =
            AccessTools.Method(typeof(PlayerStatsManager), "OnStatsDeath");
        static readonly MethodInfo LevelPlayerOnDeathMethod =
            AccessTools.Method(typeof(LevelPlayerController), "OnDeath");
        static readonly MethodInfo PlayerOnPreReviveMethod =
            AccessTools.Method(typeof(AbstractPlayerController), "OnPreRevive");
        static readonly MethodInfo PlayerOnReviveMethod =
            AccessTools.Method(typeof(AbstractPlayerController), "OnRevive");

        const float DeathHeartParryPauseSeconds = 0.185f;
        const float DeathHeartParryCatchUpCapSeconds = 0.3f;
        const float DeathHeartParryOffsetToleranceSeconds = 0.03f;
        const float BuiltInAnimCompleteFallbackSeconds = 0.035f;
        const float BuiltInReviveCorrectionSeconds = 5.0f;
        const float ScheduledParryLeadMinSeconds = 0.05f;
        const float ScheduledParryLeadMaxSeconds = 1.5f;
        const float BuiltInFinalStatusMinSettleSeconds = 0.45f;
        const float BuiltInFinalStatusMaxSettleSeconds = 1.25f;

        static readonly List<PlayerDeathEffect> ScratchDeathEffects =
            new List<PlayerDeathEffect>(4);
        static readonly HashSet<long> AppliedGrantKeys =
            new HashSet<long>();
        static readonly HashSet<int> BroadcastedBuiltInVisuals =
            new HashSet<int>();
        static readonly HashSet<int> HostAuthorizedBuiltInParryEffects =
            new HashSet<int>();
        static readonly HashSet<int> SuppressedClientLocalBuiltInParrySwitches =
            new HashSet<int>();
        static readonly HashSet<int> HostScheduledBuiltInParryEffects =
            new HashSet<int>();
        static readonly HashSet<int> HostExecutingScheduledBuiltInParryEffects =
            new HashSet<int>();
        static readonly HashSet<int> HostBuiltInAnimCompletionFallbacks =
            new HashSet<int>();
        static readonly HashSet<int> ClientBuiltInAnimCompletionFallbacks =
            new HashSet<int>();
        static readonly HashSet<long> ClientScheduledBuiltInParryKeys =
            new HashSet<long>();
        static readonly Dictionary<int, ScheduledHostBuiltInParry> HostScheduledBuiltInParries =
            new Dictionary<int, ScheduledHostBuiltInParry>();
        static readonly Dictionary<long, ScheduledClientBuiltInParry> ClientScheduledBuiltInParries =
            new Dictionary<long, ScheduledClientBuiltInParry>();
        static readonly List<int> ScratchScheduledHostParryIds =
            new List<int>(4);
        static readonly List<long> ScratchScheduledClientParryKeys =
            new List<long>(4);
        static readonly Dictionary<int, float> ClientRemoteBuiltInParryStartedAt =
            new Dictionary<int, float>();
        static readonly Dictionary<PlayerId, float> MirroredBuiltInParryVisualAt =
            new Dictionary<PlayerId, float>();
        static readonly Dictionary<PlayerId, uint> PendingHostBuiltInReviveTicks =
            new Dictionary<PlayerId, uint>();
        static readonly Dictionary<PlayerId, Vector2> PendingHostBuiltInRevivePositions =
            new Dictionary<PlayerId, Vector2>();
        static readonly Dictionary<PlayerId, float> RecentBuiltInRevives =
            new Dictionary<PlayerId, float>();
        static readonly Dictionary<PlayerId, float> RecentBuiltInReviveInputUnlocks =
            new Dictionary<PlayerId, float>();
        static readonly Dictionary<PlayerId, uint> PendingBuiltInFinalStatusTicks =
            new Dictionary<PlayerId, uint>();
        static readonly Dictionary<PlayerId, Dictionary<Renderer, bool>> SuppressedBuiltInBodyRenderers =
            new Dictionary<PlayerId, Dictionary<Renderer, bool>>();
        static float _revivePauseCatchUpUntil = -1f;
        static bool _revivePauseCatchUpActive;
        static bool _deferHostBuiltInReviveStatus;
        static int _hostBuiltInReviveStatusSuppressDepth;

        static ParticipantReviveController()
        {
            MultiplayerSession.OnSessionEnded += Reset;
        }

        public static void Reset()
        {
            RestoreAllSuppressedBuiltInBodies();
            AppliedGrantKeys.Clear();
            BroadcastedBuiltInVisuals.Clear();
            HostAuthorizedBuiltInParryEffects.Clear();
            SuppressedClientLocalBuiltInParrySwitches.Clear();
            HostScheduledBuiltInParryEffects.Clear();
            HostExecutingScheduledBuiltInParryEffects.Clear();
            HostBuiltInAnimCompletionFallbacks.Clear();
            ClientBuiltInAnimCompletionFallbacks.Clear();
            ClientScheduledBuiltInParryKeys.Clear();
            HostScheduledBuiltInParries.Clear();
            ClientScheduledBuiltInParries.Clear();
            ScratchScheduledHostParryIds.Clear();
            ScratchScheduledClientParryKeys.Clear();
            ClientRemoteBuiltInParryStartedAt.Clear();
            MirroredBuiltInParryVisualAt.Clear();
            PendingHostBuiltInReviveTicks.Clear();
            PendingHostBuiltInRevivePositions.Clear();
            PendingBuiltInFinalStatusTicks.Clear();
            RecentBuiltInRevives.Clear();
            RecentBuiltInReviveInputUnlocks.Clear();
            _revivePauseCatchUpUntil = -1f;
            _revivePauseCatchUpActive = false;
            _deferHostBuiltInReviveStatus = false;
            _hostBuiltInReviveStatusSuppressDepth = 0;
        }

        public static void Update()
        {
            ExecuteDueHostBuiltInParries();
            ExecuteDueClientBuiltInParries();
            ApplyRecentReviveInputUnlocks();
        }

        public static bool ShouldSuppressHostBuiltInImmediateReviveStatus(AbstractPlayerController player)
        {
            if (!MultiplayerSession.IsHost || player == null || player.id > PlayerId.PlayerTwo)
                return false;

            if (IsHostBuiltInReviveStatusSuppressed())
                return true;

            return PendingBuiltInFinalStatusTicks.ContainsKey(player.id)
                && player.stats != null
                && player.stats.Health > 0
                && !player.IsDead;
        }

        public static bool TryOverrideReviveOutOfFrame(PlayerDeathEffect effect)
        {
            if (!MultiplayerSession.IsActive || effect == null || Plugin.Net == null || !Plugin.Net.IsConnected)
                return false;
            if (Level.IsTowerOfPowerMain)
                return true;

            PlayerId localPlayerId;
            if (!TryGetDeathEffectPlayerId(effect, out localPlayerId))
                return false;

            if (localPlayerId != MultiplayerSession.LocalId)
                return false;

            var request = new ReviveRequestPacket
            {
                PosX = effect.transform.position.x,
                PosY = effect.transform.position.y,
                Tick = MultiplayerSession.Tick,
            };

            if (MultiplayerSession.IsHost)
            {
                ResolveHostReviveRequest((byte)localPlayerId, effect.transform.position, request.Tick, Plugin.Net);
                return true;
            }

            Plugin.Net.SendReviveRequest(ref request);
            return true;
        }

        public static void ResolveHostReviveRequest(
            byte requesterParticipantId,
            Vector2 requestPosition,
            uint tick,
            SteamNetManager net)
        {
            if (!MultiplayerSession.IsHost || net == null)
                return;

            ParticipantStatusTracker.ParticipantStatus requesterStatus;
            if (!ParticipantStatusTracker.TryGet(requesterParticipantId, out requesterStatus)
             || !requesterStatus.IsKnown)
            {
                return;
            }

            if (!requesterStatus.IsDead)
            {
                byte targetParticipantId;
                Vector2 revivePosition;
                if (!ParticipantStatusTracker.TryGetBestReviveTarget(
                        requesterParticipantId,
                        requestPosition,
                        out targetParticipantId,
                        out revivePosition))
                {
                    return;
                }

                if (IsHostLocalParticipant(targetParticipantId))
                    ApplyLocalRevive(MultiplayerSession.LocalId, revivePosition);
                else
                    SendGrantToRemoteOwner(
                        net,
                        targetParticipantId,
                        targetParticipantId,
                        requesterParticipantId,
                        revivePosition,
                        tick,
                        applyDonorCost: false,
                        applyRevive: true);
                return;
            }

            byte donorParticipantId;
            Vector2 donorPosition;
            if (!ParticipantStatusTracker.TryGetBestDonor(
                    requesterParticipantId,
                    requestPosition,
                    out donorParticipantId,
                    out donorPosition))
            {
                return;
            }

            if (IsHostLocalParticipant(donorParticipantId))
                ApplyLocalDonorCost(MultiplayerSession.LocalId);
            else
                SendGrantToRemoteOwner(net, donorParticipantId, requesterParticipantId, donorParticipantId, donorPosition, tick, applyDonorCost: true, applyRevive: false);

            if (IsHostLocalParticipant(requesterParticipantId))
                ApplyLocalRevive(MultiplayerSession.LocalId, donorPosition);
            else
                SendGrantToRemoteOwner(net, requesterParticipantId, requesterParticipantId, donorParticipantId, donorPosition, tick, applyDonorCost: false, applyRevive: true);
        }

        public static void ApplyGrant(ReviveGrantPacket pkt)
        {
            if (!MultiplayerSession.IsActive)
                return;
            if (!MarkGrantApplied(pkt))
                return;

            PlayerId localPlayerId = MultiplayerSession.LocalId;
            if (pkt.ApplyDonorCost)
                ApplyLocalDonorCost(localPlayerId);
            if (pkt.ApplyRevive)
            {
                if (ShouldWaitForHostBuiltInReviveStatus(localPlayerId))
                {
                    Plugin.Log.LogInfo(
                        "[ReviveSync] Deferred built-in revive grant for "
                        + localPlayerId
                        + "; waiting for host final revive status.");
                    return;
                }

                ApplyLocalRevive(localPlayerId, new Vector2(pkt.RevivePosX, pkt.RevivePosY));
            }
        }

        public static bool TryScheduleHostBuiltInParrySwitch(PlayerDeathEffect effect)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsHost
             || Plugin.Instance == null
             || Plugin.Net == null
             || !Plugin.Net.IsConnected
             || effect == null
             || !HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers()
             || PlayerDeathEffectOnParrySwitchMethod == null)
            {
                return false;
            }

            int effectId = effect.GetInstanceID();
            if (HostExecutingScheduledBuiltInParryEffects.Contains(effectId))
                return false;
            if (HostScheduledBuiltInParryEffects.Contains(effectId))
                return true;

            ExtraParticipantDeathBubbleTag extraTag;
            if (ExtraParticipantReviveVisuals.IsExtraBubble(effect, out extraTag))
                return false;

            PlayerId targetPlayerId;
            if (!TryGetDeathEffectPlayerId(effect, out targetPlayerId))
                return false;
            if (targetPlayerId != PlayerId.PlayerOne && targetPlayerId != PlayerId.PlayerTwo)
                return false;

            float now = HighLatencyInputSync.PacketTimeNow();
            if (now < 0f)
                return false;

            float delay = Mathf.Clamp(
                HighLatencyInputSync.GetDelaySeconds(),
                ScheduledParryLeadMinSeconds,
                ScheduledParryLeadMaxSeconds);
            if (delay <= ScheduledParryLeadMinSeconds)
                return false;

            float executeAt = now + delay;
            HostScheduledBuiltInParryEffects.Add(effectId);
            HostScheduledBuiltInParries[effectId] = new ScheduledHostBuiltInParry
            {
                Effect = effect,
                ExecuteAt = executeAt,
            };
            BroadcastBuiltInParryVisual(effect, targetPlayerId, executeAt);
            Plugin.Log.LogInfo(
                "[ReviveSync] Scheduled host built-in death-heart parry for "
                + targetPlayerId
                + " at battle clock "
                + executeAt.ToString("0.000")
                + "s.");
            return true;
        }

        public static bool IsHostBuiltInParrySwitchPending(PlayerDeathEffect effect)
        {
            return effect != null && HostScheduledBuiltInParryEffects.Contains(effect.GetInstanceID());
        }

        public static void NotifyBuiltInParrySwitch(PlayerDeathEffect effect)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsHost
             || Plugin.Net == null
             || !Plugin.Net.IsConnected
             || effect == null)
            {
                return;
            }

            ExtraParticipantDeathBubbleTag extraTag;
            if (ExtraParticipantReviveVisuals.IsExtraBubble(effect, out extraTag))
                return;

            PlayerId targetPlayerId;
            if (!TryGetDeathEffectPlayerId(effect, out targetPlayerId))
                return;
            if (targetPlayerId != PlayerId.PlayerOne && targetPlayerId != PlayerId.PlayerTwo)
                return;

            BroadcastBuiltInParryVisual(effect, targetPlayerId, GetCurrentHostBattleElapsed());
        }

        static void BroadcastBuiltInParryVisual(
            PlayerDeathEffect effect,
            PlayerId targetPlayerId,
            float hostBattleElapsed)
        {
            if (effect == null || Plugin.Net == null)
                return;

            int effectId = effect.GetInstanceID();
            if (!BroadcastedBuiltInVisuals.Add(effectId))
                return;

            var position = effect.transform.position;

            var pkt = new ReviveVisualPacket
            {
                TargetParticipantId = (byte)targetPlayerId,
                DonorParticipantId = (byte)(targetPlayerId == PlayerId.PlayerOne ? PlayerId.PlayerTwo : PlayerId.PlayerOne),
                Flags = 1,
                PosX = position.x,
                PosY = position.y,
                Tick = MultiplayerSession.Tick,
                HostBattleElapsed = hostBattleElapsed,
            };

            Plugin.Net.SendReviveVisual(ref pkt);
            Plugin.Log.LogInfo(
                "[ReviveSync] Broadcast built-in death-heart parry visual for "
                + targetPlayerId
                + " at ("
                + position.x.ToString("0.00")
                + ","
                + position.y.ToString("0.00")
                + ").");
        }

        static float GetCurrentHostBattleElapsed()
        {
            float now = HighLatencyInputSync.PacketTimeNow();
            if (now >= 0f)
                return now;

            float localElapsed;
            float hostElapsed;
            float offset;
            return SessionSync.TryGetBattleAssistTiming(out localElapsed, out hostElapsed, out offset)
                ? hostElapsed
                : -1f;
        }

        static void ExecuteDueHostBuiltInParries()
        {
            if (HostScheduledBuiltInParries.Count == 0)
                return;

            float now = HighLatencyInputSync.PacketTimeNow();
            if (now < 0f)
                return;

            ScratchScheduledHostParryIds.Clear();
            foreach (var entry in HostScheduledBuiltInParries)
            {
                if (entry.Value == null || entry.Value.ExecuteAt <= now)
                    ScratchScheduledHostParryIds.Add(entry.Key);
            }

            for (int i = 0; i < ScratchScheduledHostParryIds.Count; i++)
            {
                int effectId = ScratchScheduledHostParryIds[i];
                ScheduledHostBuiltInParry scheduled;
                if (!HostScheduledBuiltInParries.TryGetValue(effectId, out scheduled))
                    continue;

                HostScheduledBuiltInParries.Remove(effectId);
                HostScheduledBuiltInParryEffects.Remove(effectId);
                ExecuteScheduledHostBuiltInParrySwitch(
                    scheduled == null ? null : scheduled.Effect,
                    effectId);
            }
        }

        static void ExecuteScheduledHostBuiltInParrySwitch(PlayerDeathEffect effect, int effectId)
        {
            if (effect == null || PlayerDeathEffectOnParrySwitchMethod == null)
                return;

            HostExecutingScheduledBuiltInParryEffects.Add(effectId);
            try
            {
                PlayerDeathEffectOnParrySwitchMethod.Invoke(effect, null);
                TryScheduleHostBuiltInAnimComplete(effect, effectId);
            }
            finally
            {
                HostExecutingScheduledBuiltInParryEffects.Remove(effectId);
            }
        }

        static void TryScheduleHostBuiltInAnimComplete(PlayerDeathEffect effect, int effectId)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsHost
             || !HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers()
             || effect == null
             || Plugin.Instance == null
             || PlayerDeathEffectOnReviveParryAnimCompleteMethod == null)
            {
                return;
            }

            PlayerId targetPlayerId;
            if (!TryGetDeathEffectPlayerId(effect, out targetPlayerId))
                return;
            if (targetPlayerId != PlayerId.PlayerOne && targetPlayerId != PlayerId.PlayerTwo)
                return;
            if (!HostBuiltInAnimCompletionFallbacks.Add(effectId))
                return;

            Plugin.Instance.StartCoroutine(
                CompleteHostBuiltInParryAnimAfterScheduledVisual(effect, targetPlayerId, effectId));
        }

        static IEnumerator CompleteHostBuiltInParryAnimAfterScheduledVisual(
            PlayerDeathEffect effect,
            PlayerId targetPlayerId,
            int effectId)
        {
            float completeAt = Time.unscaledTime + BuiltInAnimCompleteFallbackSeconds;
            while (Time.unscaledTime < completeAt)
                yield return null;

            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsHost
             || effect == null)
            {
                yield break;
            }

            var currentEffect = FindLocalDeathEffect(targetPlayerId);
            if (currentEffect != effect)
                yield break;

            try
            {
                PlayerDeathEffectOnReviveParryAnimCompleteMethod.Invoke(effect, null);
                Plugin.Log.LogInfo(
                    "[ReviveSync] Completed host built-in revive animation after scheduled visual for "
                    + targetPlayerId
                    + ".");
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning(
                    "[ReviveSync] Could not complete host built-in revive animation for "
                    + targetPlayerId
                    + ": "
                    + ex.Message);
            }
        }

        public static bool BeginHostBuiltInParryAnimComplete(PlayerDeathEffect effect)
        {
            PlayerId targetPlayerId;
            if (!ShouldHandleHostBuiltInParryAnimComplete(effect, out targetPlayerId))
                return false;

            _hostBuiltInReviveStatusSuppressDepth++;
            return true;
        }

        public static void NotifyBuiltInParryAnimComplete(PlayerDeathEffect effect, bool hostSuppressionStarted)
        {
            try
            {
                PlayerId targetPlayerId;
                if (!ShouldHandleHostBuiltInParryAnimComplete(effect, out targetPlayerId))
                    return;

                bool localSuppressionStarted = !hostSuppressionStarted;
                if (localSuppressionStarted)
                    _hostBuiltInReviveStatusSuppressDepth++;

                try
                {
                    CompleteHostBuiltInParryAnim(effect, targetPlayerId);
                }
                finally
                {
                    if (localSuppressionStarted)
                        EndHostBuiltInReviveStatusSuppression();
                }
            }
            finally
            {
                if (hostSuppressionStarted)
                    EndHostBuiltInReviveStatusSuppression();
            }
        }

        static void CompleteHostBuiltInParryAnim(PlayerDeathEffect effect, PlayerId targetPlayerId)
        {
            var target = GetPlayerSafe(targetPlayerId);
            if (target == null || target.stats == null)
                return;

            bool alreadyRevived = !target.IsDead
                && target.stats.Health > 0
                && target.gameObject != null
                && target.gameObject.activeInHierarchy;

            if (!alreadyRevived)
            {
                PlayerId donorPlayerId = targetPlayerId == PlayerId.PlayerOne
                    ? PlayerId.PlayerTwo
                    : PlayerId.PlayerOne;
                Vector2 revivePosition = effect.transform.position;

                _deferHostBuiltInReviveStatus = true;
                try
                {
                    ResolveHostReviveRequest(
                        (byte)donorPlayerId,
                        revivePosition,
                        MultiplayerSession.Tick,
                        Plugin.Net);

                    if (target.IsDead
                     || target.stats.Health <= 0
                     || target.gameObject == null
                     || !target.gameObject.activeInHierarchy)
                    {
                        ApplyLocalRevive(targetPlayerId, revivePosition);
                    }
                }
                finally
                {
                    _deferHostBuiltInReviveStatus = false;
                }
            }

            QueueBuiltInFinalStatus(targetPlayerId);

            Plugin.Log.LogInfo(
                "[ReviveSync] Resolved host built-in revive for "
                + targetPlayerId
                + " after death-heart parry animation completed.");
        }

        public static bool TryPlayClientRemoteBuiltInParryVisualOnly(PlayerDeathEffect effect)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || effect == null)
            {
                return false;
            }

            PlayerId targetPlayerId;
            if (!TryGetDeathEffectPlayerId(effect, out targetPlayerId))
                return false;
            if (targetPlayerId != PlayerId.PlayerOne && targetPlayerId != PlayerId.PlayerTwo)
                return false;
            if (MultiplayerSession.IsAuthoritativePlayer(targetPlayerId))
                return false;
            if (DeathEffectParrySwitchField == null
             || ParrySwitchOnParryPrePauseMethod == null
             || ParrySwitchOnParryPostPauseMethod == null)
            {
                return false;
            }

            var parrySwitch = DeathEffectParrySwitchField.GetValue(effect) as PlayerDeathParrySwitch;
            var donor = GetPlayerSafe(MultiplayerSession.LocalId) as LevelPlayerController;
            if (parrySwitch == null || donor == null)
                return false;

            int effectId = effect.GetInstanceID();
            if (HostAuthorizedBuiltInParryEffects.Contains(effectId))
                return false;
            if (ClientRemoteBuiltInParryStartedAt.ContainsKey(effectId))
                return true;

            ClientRemoteBuiltInParryStartedAt[effectId] = Time.unscaledTime;

            try
            {
                ParrySwitchOnParryPrePauseMethod.Invoke(parrySwitch, new object[] { donor });
                if (Plugin.Instance != null)
                    Plugin.Instance.StartCoroutine(FinishMirroredParrySwitch(parrySwitch, donor));
                else
                    ParrySwitchOnParryPostPauseMethod.Invoke(parrySwitch, new object[] { donor });

                Plugin.Log.LogInfo(
                    "[ReviveSync] Playing client-local parry visual on remote-owned "
                    + targetPlayerId
                    + " death heart; host revive will authorize completion.");
                return true;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning(
                    "[ReviveSync] Could not play client-local parry visual for remote-owned "
                    + targetPlayerId
                    + ": "
                    + ex.Message);
                return false;
            }
        }

        public static bool TrySuppressClientBuiltInParrySwitchUntilHost(PlayerDeathEffect effect)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || effect == null
             || !HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers())
            {
                return false;
            }

            int effectId = effect.GetInstanceID();
            if (HostAuthorizedBuiltInParryEffects.Contains(effectId))
                return false;

            ExtraParticipantDeathBubbleTag extraTag;
            if (ExtraParticipantReviveVisuals.IsExtraBubble(effect, out extraTag))
                return false;

            PlayerId targetPlayerId;
            if (!TryGetDeathEffectPlayerId(effect, out targetPlayerId))
                return false;
            if (targetPlayerId != PlayerId.PlayerOne && targetPlayerId != PlayerId.PlayerTwo)
                return false;
            if (!MultiplayerSession.IsAuthoritativePlayer(targetPlayerId))
                return false;

            if (SuppressedClientLocalBuiltInParrySwitches.Add(effectId))
            {
                Plugin.Log.LogInfo(
                    "[ReviveSync] Suppressed client-local built-in death-heart parry for "
                    + targetPlayerId
                    + "; waiting for host-authorized revive visual.");
            }
            return true;
        }

        public static bool TrySuppressClientRemoteBuiltInParryAnimComplete(PlayerDeathEffect effect)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || effect == null)
            {
                return false;
            }

            PlayerId targetPlayerId;
            if (!TryGetDeathEffectPlayerId(effect, out targetPlayerId))
                return false;
            if (targetPlayerId != PlayerId.PlayerOne && targetPlayerId != PlayerId.PlayerTwo)
                return false;
            if (MultiplayerSession.IsAuthoritativePlayer(targetPlayerId))
            {
                if (ShouldWaitForHostBuiltInReviveStatus(targetPlayerId))
                {
                    Plugin.Log.LogInfo(
                        "[ReviveSync] Allowing local revive completion for authoritative "
                        + targetPlayerId
                        + " death heart; host final status will reconcile.");
                }

                return false;
            }

            if (HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers()
             && HostAuthorizedBuiltInParryEffects.Contains(effect.GetInstanceID()))
            {
                Plugin.Log.LogInfo(
                    "[ReviveSync] Allowing host-authorized proxy revive completion for remote-owned "
                    + targetPlayerId
                    + " death heart.");
                return false;
            }

            Plugin.Log.LogInfo(
                "[ReviveSync] Suppressed client-local revive completion for remote-owned "
                + targetPlayerId
                + " death heart; waiting for host status.");
            return true;
        }

        public static void ApplyReviveVisual(ReviveVisualPacket pkt)
        {
            ApplyReviveVisual(pkt, allowSchedule: true);
        }

        static void ApplyReviveVisual(ReviveVisualPacket pkt, bool allowSchedule)
        {
            if (!MultiplayerSession.IsActive || !MultiplayerSession.IsClient)
                return;
            if (!pkt.ParrySwitch || pkt.TargetParticipantId > (byte)PlayerId.PlayerTwo)
                return;

            if (allowSchedule && TryScheduleClientBuiltInParryVisual(pkt))
                return;

            var targetPlayerId = (PlayerId)pkt.TargetParticipantId;
            var effect = FindLocalDeathEffect(targetPlayerId);
            if (effect == null)
                return;

            bool alreadyExiting = IsDeathEffectExiting(effect);
            int effectId = effect.GetInstanceID();
            bool localVisualAlreadyStarted =
                ClientRemoteBuiltInParryStartedAt.ContainsKey(effectId)
                || SuppressedClientLocalBuiltInParrySwitches.Contains(effectId);
            if (!localVisualAlreadyStarted)
                effect.transform.position = new Vector3(pkt.PosX, pkt.PosY, effect.transform.position.z);
            if (PlayerDeathEffectOnParrySwitchMethod == null && !alreadyExiting && !localVisualAlreadyStarted)
                return;

            try
            {
                HostAuthorizedBuiltInParryEffects.Add(effectId);
                float localVisualAt;
                MirroredBuiltInParryVisualAt[targetPlayerId] =
                    ClientRemoteBuiltInParryStartedAt.TryGetValue(effectId, out localVisualAt)
                        ? localVisualAt
                        : Time.unscaledTime;

                if (alreadyExiting || localVisualAlreadyStarted)
                {
                    Plugin.Log.LogInfo(
                        "[ReviveSync] Host authorized existing client death-heart parry visual for "
                        + targetPlayerId
                        + " at tick "
                        + pkt.Tick
                        + ".");
                    TryScheduleClientBuiltInAnimComplete(effect, targetPlayerId);
                    return;
                }

                bool scheduledDirect = ShouldInvokeScheduledParryDirectly(pkt);
                if (scheduledDirect)
                {
                    PlayerDeathEffectOnParrySwitchMethod.Invoke(effect, null);
                }
                else if (!TryBeginMirroredParrySwitch(effect, pkt))
                {
                    PlayerDeathEffectOnParrySwitchMethod.Invoke(effect, null);
                }
                TryScheduleClientBuiltInAnimComplete(effect, targetPlayerId);
                if (!scheduledDirect)
                    _revivePauseCatchUpUntil = Time.unscaledTime + 2f;
                Plugin.Log.LogInfo(
                    "[ReviveSync] Mirrored built-in death-heart parry visual for "
                    + targetPlayerId
                    + " at tick "
                    + pkt.Tick
                    + ".");
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning(
                    "[ReviveSync] Could not mirror built-in death-heart parry visual for "
                    + targetPlayerId
                    + ": "
                    + ex.Message);
            }
        }

        static void TryScheduleClientBuiltInAnimComplete(PlayerDeathEffect effect, PlayerId targetPlayerId)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || !HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers()
             || effect == null
             || Plugin.Instance == null
             || PlayerDeathEffectOnReviveParryAnimCompleteMethod == null)
            {
                return;
            }

            int effectId = effect.GetInstanceID();
            if (!HostAuthorizedBuiltInParryEffects.Contains(effectId))
                return;
            if (!ClientBuiltInAnimCompletionFallbacks.Add(effectId))
                return;

            Plugin.Instance.StartCoroutine(
                CompleteClientBuiltInParryAnimAfterHostVisual(effect, targetPlayerId, effectId));
        }

        static IEnumerator CompleteClientBuiltInParryAnimAfterHostVisual(
            PlayerDeathEffect effect,
            PlayerId targetPlayerId,
            int effectId)
        {
            float completeAt = Time.unscaledTime + BuiltInAnimCompleteFallbackSeconds;
            while (Time.unscaledTime < completeAt)
                yield return null;

            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || !HostAuthorizedBuiltInParryEffects.Contains(effectId)
             || effect == null)
            {
                yield break;
            }

            var currentEffect = FindLocalDeathEffect(targetPlayerId);
            if (currentEffect != effect)
                yield break;

            try
            {
                PlayerDeathEffectOnReviveParryAnimCompleteMethod.Invoke(effect, null);
                RecentBuiltInRevives[targetPlayerId] =
                    Time.unscaledTime + BuiltInReviveCorrectionSeconds;
                CompleteAuthoritativeClientBuiltInReviveIfNeeded(effect, targetPlayerId);
                Plugin.Log.LogInfo(
                    "[ReviveSync] Completed client built-in revive animation after host-authorized visual for "
                    + targetPlayerId
                    + ".");
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning(
                    "[ReviveSync] Could not complete client-local built-in revive animation for "
                    + targetPlayerId
                    + ": "
                    + ex.Message);
            }
        }

        static void CompleteAuthoritativeClientBuiltInReviveIfNeeded(PlayerDeathEffect effect, PlayerId targetPlayerId)
        {
            if (!MultiplayerSession.IsClient
             || !MultiplayerSession.IsAuthoritativePlayer(targetPlayerId)
             || effect == null)
            {
                return;
            }

            var player = GetPlayerSafe(targetPlayerId);
            if (player == null || player.stats == null)
                return;
            if (!player.IsDead && player.stats.Health > 0 && player.gameObject != null && player.gameObject.activeInHierarchy)
                return;

            Vector3 revivePosition = effect.transform.position;
            ApplyLocalRevive(targetPlayerId, revivePosition, pushStatus: false);
            Plugin.Log.LogInfo(
                "[ReviveSync] Completed authoritative client revive fallback for "
                + targetPlayerId
                + " at ("
                + revivePosition.x.ToString("0.00")
                + ","
                + revivePosition.y.ToString("0.00")
                + ").");
        }

        public static bool ShouldCorrectRecentlyRevivedBuiltInPlayer(PlayerId playerId)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || !HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers()
             || playerId > PlayerId.PlayerTwo)
            {
                return false;
            }

            float until;
            if (!RecentBuiltInRevives.TryGetValue(playerId, out until))
                return false;

            if (Time.unscaledTime <= until)
                return true;

            RecentBuiltInRevives.Remove(playerId);
            return false;
        }

        public static void CancelRecentBuiltInReviveCorrection(PlayerId playerId, string reason = "after local gameplay input")
        {
            if (playerId > PlayerId.PlayerTwo)
                return;

            if (!RecentBuiltInRevives.Remove(playerId))
                return;

            RecentBuiltInReviveInputUnlocks[playerId] = Time.unscaledTime + 2f;
            ReleaseBuiltInReviveMotorForImmediateControl(GetPlayerSafe(playerId) as LevelPlayerController);

            if (Plugin.AutoRunLanSteamE2E)
            {
                Plugin.Log.LogInfo(
                    "[ReviveSync] Cancelled recent revive correction for "
                    + playerId
                    + " "
                    + reason
                    + ".");
            }
        }

        static bool ShouldInvokeScheduledParryDirectly(ReviveVisualPacket pkt)
        {
            return pkt.HasHostBattleElapsed
                && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers();
        }

        static bool TryScheduleClientBuiltInParryVisual(ReviveVisualPacket pkt)
        {
            if (!pkt.ParrySwitch
             || !pkt.HasHostBattleElapsed
             || Plugin.Instance == null
             || !HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers())
            {
                return false;
            }

            float now = HighLatencyInputSync.PlayoutTimeNow();
            float delay = pkt.HostBattleElapsed - now;
            if (delay <= ScheduledParryLeadMinSeconds || delay > ScheduledParryLeadMaxSeconds + 0.5f)
                return false;

            long key = ((long)pkt.Tick << 8) | pkt.TargetParticipantId;
            if (!ClientScheduledBuiltInParryKeys.Add(key))
                return true;

            ClientScheduledBuiltInParries[key] = new ScheduledClientBuiltInParry
            {
                Packet = pkt,
                Key = key,
            };
            Plugin.Log.LogInfo(
                "[ReviveSync] Scheduled client built-in death-heart parry for "
                + (PlayerId)pkt.TargetParticipantId
                + " at host battle clock "
                + pkt.HostBattleElapsed.ToString("0.000")
                + "s.");
            return true;
        }

        static void ExecuteDueClientBuiltInParries()
        {
            if (ClientScheduledBuiltInParries.Count == 0)
                return;

            float now = HighLatencyInputSync.PlayoutTimeNow();
            ScratchScheduledClientParryKeys.Clear();
            foreach (var entry in ClientScheduledBuiltInParries)
            {
                if (entry.Value == null || entry.Value.Packet.HostBattleElapsed <= now)
                    ScratchScheduledClientParryKeys.Add(entry.Key);
            }

            for (int i = 0; i < ScratchScheduledClientParryKeys.Count; i++)
            {
                long key = ScratchScheduledClientParryKeys[i];
                ScheduledClientBuiltInParry scheduled;
                if (!ClientScheduledBuiltInParries.TryGetValue(key, out scheduled))
                    continue;

                ClientScheduledBuiltInParries.Remove(key);
                ClientScheduledBuiltInParryKeys.Remove(key);
                if (scheduled != null)
                    ApplyReviveVisual(scheduled.Packet, allowSchedule: false);
            }
        }

        public static void TryCatchUpAfterHostTimingSnapshot(float localMinusHostSeconds)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || Plugin.Instance == null
             || _revivePauseCatchUpActive
             || _revivePauseCatchUpUntil < 0f
             || Time.unscaledTime > _revivePauseCatchUpUntil
             || localMinusHostSeconds <= DeathHeartParryOffsetToleranceSeconds)
            {
                return;
            }

            float holdSeconds = Mathf.Min(localMinusHostSeconds, DeathHeartParryCatchUpCapSeconds);
            Plugin.Instance.StartCoroutine(HoldGuestForHostPauseCorrection(holdSeconds));
        }

        static bool TryBeginMirroredParrySwitch(PlayerDeathEffect effect, ReviveVisualPacket pkt)
        {
            if (effect == null
             || DeathEffectParrySwitchField == null
             || ParrySwitchOnParryPrePauseMethod == null
             || ParrySwitchOnParryPostPauseMethod == null)
            {
                return false;
            }

            var parrySwitch = DeathEffectParrySwitchField.GetValue(effect) as PlayerDeathParrySwitch;
            if (parrySwitch == null)
                return false;

            PlayerId donorPlayerId;
            if (pkt.DonorParticipantId <= (byte)PlayerId.PlayerTwo)
                donorPlayerId = (PlayerId)pkt.DonorParticipantId;
            else
                donorPlayerId = pkt.TargetParticipantId == (byte)PlayerId.PlayerOne
                    ? PlayerId.PlayerTwo
                    : PlayerId.PlayerOne;

            var donor = GetPlayerSafe(donorPlayerId) as LevelPlayerController;
            if (donor == null)
                return false;

            ParrySwitchOnParryPrePauseMethod.Invoke(parrySwitch, new object[] { donor });
            if (!IsDeathEffectExiting(effect) && PlayerDeathEffectOnParrySwitchMethod != null)
                PlayerDeathEffectOnParrySwitchMethod.Invoke(effect, null);

            if (Plugin.Instance != null)
                Plugin.Instance.StartCoroutine(FinishMirroredParrySwitch(parrySwitch, donor));
            else
                ParrySwitchOnParryPostPauseMethod.Invoke(parrySwitch, new object[] { donor });
            return true;
        }

        static IEnumerator FinishMirroredParrySwitch(
            PlayerDeathParrySwitch parrySwitch,
            LevelPlayerController donor)
        {
            bool pausedByUs = false;
            try
            {
                if (PauseManager.state != PauseManager.State.Paused)
                {
                    PauseManager.Pause();
                    pausedByUs = true;
                }
            }
            catch
            {
                pausedByUs = false;
            }

            float endAt = Time.unscaledTime + DeathHeartParryPauseSeconds;
            while (Time.unscaledTime < endAt)
                yield return null;

            float catchUpEndAt = Time.unscaledTime + DeathHeartParryCatchUpCapSeconds;
            while (Time.unscaledTime < catchUpEndAt && IsGuestStillAheadOfHostTimer())
                yield return null;

            try
            {
                if (pausedByUs && PauseManager.state == PauseManager.State.Paused)
                    PauseManager.Unpause();
            }
            catch
            {
            }

            if (parrySwitch != null && donor != null && ParrySwitchOnParryPostPauseMethod != null)
                ParrySwitchOnParryPostPauseMethod.Invoke(parrySwitch, new object[] { donor });
        }

        static IEnumerator HoldGuestForHostPauseCorrection(float seconds)
        {
            _revivePauseCatchUpActive = true;
            bool pausedByUs = false;
            try
            {
                if (PauseManager.state != PauseManager.State.Paused)
                {
                    PauseManager.Pause();
                    pausedByUs = true;
                }

                float endAt = Time.unscaledTime + Mathf.Max(0f, seconds);
                while (Time.unscaledTime < endAt)
                    yield return null;
            }
            finally
            {
                try
                {
                    if (pausedByUs && PauseManager.state == PauseManager.State.Paused)
                        PauseManager.Unpause();
                }
                catch
                {
                }

                _revivePauseCatchUpActive = false;
            }
        }

        static bool IsGuestStillAheadOfHostTimer()
        {
            float localElapsed;
            float hostElapsed;
            float offset;
            if (!SessionSync.TryGetBattleAssistTiming(out localElapsed, out hostElapsed, out offset))
                return false;

            return offset > DeathHeartParryOffsetToleranceSeconds;
        }

        public static bool TryMirrorHostBuiltInRevive(PlayerStatusPacket pkt)
        {
            if (!MultiplayerSession.IsActive || !MultiplayerSession.IsClient)
                return false;
            if (pkt.ParticipantId > (byte)PlayerId.PlayerTwo)
                return false;
            if (pkt.IsDead || pkt.Health <= 0)
                return false;

            var playerId = (PlayerId)pkt.ParticipantId;
            if (MultiplayerSession.IsAuthoritativePlayer(playerId)
             && pkt.ParticipantId != (byte)MultiplayerSession.LocalId)
            {
                return false;
            }

            var player = GetPlayerSafe(playerId);
            if (player == null)
                return false;
            var pendingDeathEffect = FindLocalDeathEffect(playerId);
            if (!player.IsDead && pendingDeathEffect == null)
            {
                if (ShouldAcceptRecentHostBuiltInRevivePosition(pkt, fromRemote: true))
                    return SnapBuiltInPlayerToHostReviveStatus(playerId, pkt);

                return false;
            }

            Vector2 revivePosition;
            if (pkt.HasPosition)
                revivePosition = new Vector2(pkt.PosX, pkt.PosY);
            else if (!TryGetHostRevivePosition(playerId, out revivePosition))
                revivePosition = player.center;

            if (ShouldDeferHostBuiltInRevive(playerId, pendingDeathEffect))
            {
                QueueDeferredHostBuiltInRevive(playerId, revivePosition, pkt.Tick);
                return true;
            }

            ApplyLocalRevive(playerId, revivePosition, pushStatus: false);
            Plugin.Log.LogInfo(
                "[ReviveSync] Mirrored host revive for "
                + playerId
                + " from status tick "
                + pkt.Tick
                + " at ("
                + revivePosition.x.ToString("0.00")
                + ","
                + revivePosition.y.ToString("0.00")
                + ").");
            return true;
        }

        public static bool TryMirrorHostBuiltInDeath(PlayerStatusPacket pkt)
        {
            if (!MultiplayerSession.IsActive || !MultiplayerSession.IsClient)
                return false;
            if (pkt.ParticipantId > (byte)PlayerId.PlayerTwo)
                return false;
            if (!pkt.IsDead || pkt.Health > 0)
                return false;

            var playerId = (PlayerId)pkt.ParticipantId;
            if (MultiplayerSession.IsAuthoritativePlayer(playerId))
                return false;

            var player = GetPlayerSafe(playerId) as LevelPlayerController;
            if (player == null || player.stats == null)
                return false;

            var existing = FindLocalDeathEffect(playerId);
            if (player.IsDead && existing != null)
            {
                SuppressRemoteBuiltInBody(player);
                return true;
            }

            try
            {
                player.stats.SetHealth(0);

                if (PlayerStatsOnStatsDeathMethod != null)
                    PlayerStatsOnStatsDeathMethod.Invoke(player.stats, null);

                existing = FindLocalDeathEffect(playerId);
                if (existing == null && LevelPlayerOnDeathMethod != null)
                    LevelPlayerOnDeathMethod.Invoke(player, new object[] { player.id });

                existing = FindLocalDeathEffect(playerId);
                if (existing != null)
                    SuppressRemoteBuiltInBody(player);

            Plugin.Log.LogInfo(
                "[ReviveSync] Mirrored host death for "
                + playerId
                + " from status tick "
                    + pkt.Tick
                    + ".");
                return existing != null || player.IsDead;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning(
                    "[ReviveSync] Could not mirror host death for "
                    + playerId
                    + ": "
                    + ex.Message);
                return false;
            }
        }

        public static bool TrySuppressRemoteBuiltInDeadBody(LevelPlayerController player)
        {
            if (!ShouldSuppressRemoteBuiltInDeadBody(player))
            {
                RestoreSuppressedBuiltInBody(player);
                return false;
            }

            SuppressRemoteBuiltInBody(player);
            return true;
        }

        public static bool ShouldAcceptRecentHostBuiltInRevivePosition(PlayerStatusPacket pkt, bool fromRemote)
        {
            if (!fromRemote
             || !MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || pkt.ParticipantId > (byte)PlayerId.PlayerTwo
             || pkt.IsDead
             || pkt.Health <= 0
             || !pkt.HasPosition
             || pkt.PosY > -225f)
            {
                return false;
            }

            var playerId = (PlayerId)pkt.ParticipantId;
            return ShouldCorrectRecentlyRevivedBuiltInPlayer(playerId);
        }

        static bool SnapBuiltInPlayerToHostReviveStatus(PlayerId playerId, PlayerStatusPacket pkt)
        {
            if (!pkt.HasPosition)
                return false;

            var player = GetPlayerSafe(playerId);
            var levelPlayer = player as LevelPlayerController;
            if (player == null)
                return false;

            var target = new Vector3(pkt.PosX, pkt.PosY, player.transform.position.z);
            player.transform.position = target;
            if (levelPlayer != null && levelPlayer.motor != null)
            {
                levelPlayer.motor.transform.position =
                    new Vector3(pkt.PosX, pkt.PosY, levelPlayer.motor.transform.position.z);
                ReleaseBuiltInReviveMotorForImmediateControl(levelPlayer);
                if (pkt.PosY <= -225f)
                {
                    try { Traverse.Create(levelPlayer.motor).Property("Grounded").SetValue(true); }
                    catch { }
                }
            }

            if (player.gameObject != null && !player.gameObject.activeSelf)
                player.gameObject.SetActive(true);
            if (player.stats != null && player.stats.Health <= 0)
                player.stats.SetHealth(Mathf.Max(1, player.stats.HealthMax));

            RecentBuiltInRevives[playerId] =
                Time.unscaledTime + BuiltInReviveCorrectionSeconds;

            Plugin.Log.LogInfo(
                "[ReviveSync] Snapped recently revived "
                + playerId
                + " to host final revive status at ("
                + pkt.PosX.ToString("0.00")
                + ","
                + pkt.PosY.ToString("0.00")
                + ") tick "
                + pkt.Tick
                + ".");
            return true;
        }

        static bool ShouldWaitForHostBuiltInReviveStatus(PlayerId playerId)
        {
            return MultiplayerSession.IsClient
                && Plugin.VanillaTwoPlayerOnline
                && playerId <= PlayerId.PlayerTwo
                && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers();
        }

        public static bool ShouldSuppressRecentBuiltInReviveDeath(PlayerStatusPacket pkt, bool fromRemote)
        {
            if (!MultiplayerSession.IsActive || pkt.ParticipantId > (byte)PlayerId.PlayerTwo)
                return false;
            if (!pkt.IsDead && pkt.Health > 0)
                return false;

            var playerId = (PlayerId)pkt.ParticipantId;
            float suppressUntil;
            if (!RecentBuiltInRevives.TryGetValue(playerId, out suppressUntil))
                return false;
            if (Time.unscaledTime > suppressUntil)
            {
                RecentBuiltInRevives.Remove(playerId);
                return false;
            }

            var existingPlayer = GetPlayerSafe(playerId);
            if (existingPlayer == null
             || existingPlayer.IsDead
             || existingPlayer.stats == null
             || existingPlayer.stats.Health <= 0)
            {
                Vector2 revivePosition;
                if (!TryGetHostRevivePosition(playerId, out revivePosition))
                {
                    revivePosition = existingPlayer == null ? Vector2.zero : (Vector2)existingPlayer.center;
                }

                ApplyLocalRevive(playerId, revivePosition, pushStatus: false);
            }

            Plugin.Log.LogInfo(
                "[ReviveSync] Suppressed "
                + (fromRemote ? "remote" : "local")
                + " stale death status for recently revived "
                + playerId
                + " tick="
                + pkt.Tick
                + ".");
            return true;
        }

        public static bool IsUnsettledBuiltInReviveStatus(PlayerStatusPacket pkt)
        {
            return pkt.ParticipantId <= (byte)PlayerId.PlayerTwo
                && !pkt.IsDead
                && pkt.Health > 0
                && pkt.HasPosition
                && pkt.PosY > -225f;
        }

        public static void RestoreSuppressedBuiltInBody(LevelPlayerController player)
        {
            if (player == null)
                return;

            RestoreSuppressedBuiltInBody(player.id);
        }

        static bool ShouldSuppressRemoteBuiltInDeadBody(LevelPlayerController player)
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsClient
             || player == null
             || player.id > PlayerId.PlayerTwo)
            {
                return false;
            }
            if (MultiplayerSession.IsAuthoritativePlayer(player.id))
                return false;
            if (FindLocalDeathEffect(player.id) == null)
                return false;

            ParticipantStatusTracker.ParticipantStatus status;
            bool statusDead = ParticipantStatusTracker.TryGet((byte)player.id, out status) && status.IsDead;
            return player.IsDead || statusDead;
        }

        static void SuppressRemoteBuiltInBody(LevelPlayerController player)
        {
            if (player == null)
                return;

            Dictionary<Renderer, bool> originalStates;
            if (!SuppressedBuiltInBodyRenderers.TryGetValue(player.id, out originalStates))
            {
                originalStates = new Dictionary<Renderer, bool>();
                SuppressedBuiltInBodyRenderers[player.id] = originalStates;
            }

            var renderers = player.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null)
                    continue;

                if (!originalStates.ContainsKey(renderer))
                    originalStates[renderer] = renderer.enabled;

                renderer.enabled = false;
            }
        }

        static void RestoreSuppressedBuiltInBody(PlayerId playerId)
        {
            Dictionary<Renderer, bool> originalStates;
            if (!SuppressedBuiltInBodyRenderers.TryGetValue(playerId, out originalStates))
                return;

            foreach (var entry in originalStates)
            {
                if (entry.Key != null)
                    entry.Key.enabled = entry.Value;
            }

            SuppressedBuiltInBodyRenderers.Remove(playerId);
        }

        static void RestoreAllSuppressedBuiltInBodies()
        {
            var ids = new List<PlayerId>(SuppressedBuiltInBodyRenderers.Keys);
            for (int i = 0; i < ids.Count; i++)
                RestoreSuppressedBuiltInBody(ids[i]);
        }

        static bool ShouldDeferHostBuiltInRevive(PlayerId playerId, PlayerDeathEffect pendingDeathEffect)
        {
            if (!MultiplayerSession.IsClient || Plugin.Instance == null)
                return false;
            if (!MultiplayerSession.IsAuthoritativePlayer(playerId))
                return true;
            if (pendingDeathEffect == null)
                return false;

            float visualAt;
            return MirroredBuiltInParryVisualAt.TryGetValue(playerId, out visualAt)
                && Time.unscaledTime - visualAt < 1.5f;
        }

        static void QueueDeferredHostBuiltInRevive(PlayerId playerId, Vector2 revivePosition, uint tick)
        {
            uint existingTick;
            if (PendingHostBuiltInReviveTicks.TryGetValue(playerId, out existingTick)
             && !NetTick.IsOlder(existingTick, tick))
            {
                if (existingTick == tick)
                    PendingHostBuiltInRevivePositions[playerId] = revivePosition;
                return;
            }

            PendingHostBuiltInReviveTicks[playerId] = tick;
            PendingHostBuiltInRevivePositions[playerId] = revivePosition;
            Plugin.Instance.StartCoroutine(ApplyDeferredHostBuiltInRevive(playerId, revivePosition, tick));
        }

        static IEnumerator ApplyDeferredHostBuiltInRevive(PlayerId playerId, Vector2 revivePosition, uint tick)
        {
            float requestedAt = Time.unscaledTime;
            float hardDeadline = requestedAt + 0.6f;
            while (Time.unscaledTime < hardDeadline)
            {
                float visualAt;
                if (MirroredBuiltInParryVisualAt.TryGetValue(playerId, out visualAt))
                {
                    if (Time.unscaledTime < visualAt + DeathHeartParryPauseSeconds + 0.08f)
                    {
                        yield return null;
                        continue;
                    }

                    break;
                }

                if (Time.unscaledTime - requestedAt >= 0.12f)
                    break;

                yield return null;
            }

            float settleUntil = Time.unscaledTime + 0.2f;
            while (Time.unscaledTime < settleUntil)
                yield return null;

            uint latestTick;
            if (!PendingHostBuiltInReviveTicks.TryGetValue(playerId, out latestTick)
             || latestTick != tick)
            {
                yield break;
            }

            PendingHostBuiltInReviveTicks.Remove(playerId);
            Vector2 latestPosition;
            if (PendingHostBuiltInRevivePositions.TryGetValue(playerId, out latestPosition))
                revivePosition = latestPosition;
            PendingHostBuiltInRevivePositions.Remove(playerId);

            var player = GetPlayerSafe(playerId);
            if (player == null)
                yield break;

            var deathEffect = FindLocalDeathEffect(playerId);
            if (!player.IsDead && deathEffect == null)
                yield break;

            ApplyLocalRevive(playerId, revivePosition, pushStatus: false);
            Plugin.Log.LogInfo(
                "[ReviveSync] Applied deferred host revive for "
                + playerId
                + " from status tick "
                + tick
                + " at ("
                + revivePosition.x.ToString("0.00")
                + ","
                + revivePosition.y.ToString("0.00")
                + ").");
        }

        static void SendGrantToRemoteOwner(
            SteamNetManager net,
            byte ownerParticipantId,
            byte targetParticipantId,
            byte donorParticipantId,
            Vector2 revivePosition,
            uint tick,
            bool applyDonorCost,
            bool applyRevive)
        {
            var pkt = new ReviveGrantPacket
            {
                TargetParticipantId = targetParticipantId,
                DonorParticipantId = donorParticipantId,
                Flags = (byte)((applyDonorCost ? 1 : 0) | (applyRevive ? 2 : 0)),
                RevivePosX = revivePosition.x,
                RevivePosY = revivePosition.y,
                Tick = tick,
            };

            net.SendReviveGrantToParticipant(ownerParticipantId, ref pkt);
        }

        static bool IsHostLocalParticipant(byte participantId)
        {
            return participantId == (byte)PlayerId.PlayerOne;
        }

        static void ApplyLocalDonorCost(PlayerId localPlayerId)
        {
            var player = GetPlayerSafe(localPlayerId);
            if (player == null || player.stats == null || !player.stats.PartnerCanSteal)
                return;

            player.stats.OnPartnerStealHealth();
            ParticipantStatusTracker.PushLocalStatus(player);
        }

        static void ApplyLocalRevive(PlayerId localPlayerId, Vector2 revivePosition, bool pushStatus = true)
        {
            var player = GetPlayerSafe(localPlayerId);
            if (player == null)
                return;

            var deathEffect = FindLocalDeathEffect(localPlayerId);
            if (deathEffect != null && DeathEffectExitingField != null)
                DeathEffectExitingField.SetValue(deathEffect, true);

            if (PlayerOnPreReviveMethod != null)
                PlayerOnPreReviveMethod.Invoke(player, new object[] { (Vector3)revivePosition });
            if (PlayerOnReviveMethod != null)
                PlayerOnReviveMethod.Invoke(player, new object[] { (Vector3)revivePosition });

            var levelPlayer = player as LevelPlayerController;
            if (levelPlayer != null && player.stats != null && player.stats.isChalice)
                levelPlayer.motor.OnChaliceRevive();
            if (levelPlayer != null)
            {
                ResetBuiltInReviveVelocity(levelPlayer);
                RestoreSuppressedBuiltInBody(levelPlayer);
            }

            if (player.gameObject != null && !player.gameObject.activeSelf)
                player.gameObject.SetActive(true);
            if (player.stats != null && player.stats.Health <= 0)
                player.stats.SetHealth(Mathf.Max(1, player.stats.HealthMax));

            if (deathEffect != null)
                UnityEngine.Object.Destroy(deathEffect.gameObject);

            RecentBuiltInRevives[localPlayerId] =
                Time.unscaledTime + BuiltInReviveCorrectionSeconds;

            bool deferBuiltInStatus = IsHostBuiltInReviveStatusSuppressed()
                && MultiplayerSession.IsHost
                && localPlayerId <= PlayerId.PlayerTwo;

            if (pushStatus && !deferBuiltInStatus)
                ParticipantStatusTracker.PushLocalStatus(player);
        }

        static void ResetBuiltInReviveVelocity(LevelPlayerController player)
        {
            if (player == null || player.motor == null)
                return;

            ReleaseBuiltInReviveMotorForImmediateControl(player);
        }

        static void ApplyRecentReviveInputUnlocks()
        {
            if (RecentBuiltInReviveInputUnlocks.Count == 0)
                return;

            var expired = new List<PlayerId>();
            foreach (var entry in RecentBuiltInReviveInputUnlocks)
            {
                if (Time.unscaledTime > entry.Value)
                {
                    expired.Add(entry.Key);
                    continue;
                }

                ReleaseBuiltInReviveMotorForImmediateControl(GetPlayerSafe(entry.Key) as LevelPlayerController);
            }

            for (int i = 0; i < expired.Count; i++)
                RecentBuiltInReviveInputUnlocks.Remove(expired[i]);
        }

        public static void ReleaseBuiltInReviveMotorForImmediateControl(LevelPlayerController player)
        {
            if (player == null || player.motor == null)
                return;

            try { player.EnableInput(); }
            catch { }

            try
            {
                var motor = Traverse.Create(player.motor);
                motor.Property("velocity").SetValue(Vector2.zero);
                motor.Property("Locked").SetValue(false);
                motor.Property("Grounded").SetValue(true);
                motor.Property("Parrying").SetValue(false);
                motor.Field("allowInput").SetValue(true);
                motor.Field("allowJumping").SetValue(true);
                motor.Field("allowFalling").SetValue(true);
                motor.Field("forceLaunchUp").SetValue(false);
                motor.Field("timeSinceInputBuffered").SetValue(0.134f);
            }
            catch
            {
            }

            TryInvokeNestedMotorMethod(player.motor, "velocityManager", "Clear");
            TrySetNestedMotorField(player.motor, "velocityManager", "yAxisForce", false);

            TrySetNestedMotorEnum(player.motor, "hitManager", "state", 0);
            TrySetNestedMotorField(player.motor, "hitManager", "timer", 0f);
            TrySetNestedMotorField(player.motor, "hitManager", "direction", 0);

            TrySetNestedMotorEnum(player.motor, "jumpManager", "state", 0);
            TrySetNestedMotorField(player.motor, "jumpManager", "timer", 0f);
            TrySetNestedMotorField(player.motor, "jumpManager", "timeSinceDownJump", 1000f);
            TrySetNestedMotorField(player.motor, "jumpManager", "timeInAir", 0f);
            TrySetNestedMotorField(player.motor, "jumpManager", "doubleJumped", false);
            TrySetNestedMotorField(player.motor, "jumpManager", "ableToLand", true);

            TrySetNestedMotorEnum(player.motor, "dashManager", "state", 0);
            TrySetNestedMotorField(player.motor, "dashManager", "timer", 0f);
            TrySetNestedMotorField(player.motor, "dashManager", "timeSinceGroundDash", 1000f);
            TrySetNestedMotorField(player.motor, "dashManager", "chaliceParryCoolDown", false);

            TrySetNestedMotorEnum(player.motor, "parryManager", "state", 0);
            TrySetNestedMotorEnum(player.motor, "superManager", "state", 0);
        }

        static void TrySetNestedMotorEnum(LevelPlayerMotor motor, string ownerFieldName, string fieldName, int value)
        {
            object owner = GetNestedMotorObject(motor, ownerFieldName);
            if (owner == null)
                return;

            try
            {
                var field = owner.GetType().GetField(fieldName, AnyInstance);
                if (field != null && field.FieldType.IsEnum)
                    field.SetValue(owner, Enum.ToObject(field.FieldType, value));
            }
            catch
            {
            }
        }

        static void TrySetNestedMotorField(LevelPlayerMotor motor, string ownerFieldName, string fieldName, object value)
        {
            object owner = GetNestedMotorObject(motor, ownerFieldName);
            if (owner == null)
                return;

            try
            {
                var field = owner.GetType().GetField(fieldName, AnyInstance);
                if (field != null)
                    field.SetValue(owner, value);
            }
            catch
            {
            }
        }

        static void TryInvokeNestedMotorMethod(LevelPlayerMotor motor, string ownerFieldName, string methodName)
        {
            object owner = GetNestedMotorObject(motor, ownerFieldName);
            if (owner == null)
                return;

            try
            {
                var method = owner.GetType().GetMethod(methodName, AnyInstance, null, Type.EmptyTypes, null);
                if (method != null)
                    method.Invoke(owner, null);
            }
            catch
            {
            }
        }

        static object GetNestedMotorObject(LevelPlayerMotor motor, string fieldName)
        {
            if (motor == null || string.IsNullOrEmpty(fieldName))
                return null;

            try
            {
                var field = typeof(LevelPlayerMotor).GetField(fieldName, AnyInstance);
                return field == null ? null : field.GetValue(motor);
            }
            catch
            {
                return null;
            }
        }

        static void QueueBuiltInFinalStatus(PlayerId playerId)
        {
            if (!MultiplayerSession.IsHost || Plugin.Instance == null)
                return;
            if (playerId > PlayerId.PlayerTwo)
                return;

            uint tick = MultiplayerSession.Tick;
            PendingBuiltInFinalStatusTicks[playerId] = tick;
            Plugin.Instance.StartCoroutine(PushBuiltInFinalStatusAfterSettle(playerId, tick));
        }

        static IEnumerator PushBuiltInFinalStatusAfterSettle(PlayerId playerId, uint tick)
        {
            float minEndAt = Time.unscaledTime + BuiltInFinalStatusMinSettleSeconds;
            float maxEndAt = Time.unscaledTime + BuiltInFinalStatusMaxSettleSeconds;
            while (Time.unscaledTime < maxEndAt)
            {
                if (Time.unscaledTime >= minEndAt)
                {
                    var current = GetPlayerSafe(playerId);
                    if (current != null && IsBuiltInReviveSettled(current))
                        break;
                }

                yield return null;
            }

            uint latestTick;
            if (!PendingBuiltInFinalStatusTicks.TryGetValue(playerId, out latestTick)
             || latestTick != tick)
            {
                yield break;
            }

            PendingBuiltInFinalStatusTicks.Remove(playerId);
            var player = GetPlayerSafe(playerId);
            if (player != null)
                ParticipantStatusTracker.PushLocalStatus(player);
        }

        static bool IsHostBuiltInReviveStatusSuppressed()
        {
            return _deferHostBuiltInReviveStatus || _hostBuiltInReviveStatusSuppressDepth > 0;
        }

        static void EndHostBuiltInReviveStatusSuppression()
        {
            if (_hostBuiltInReviveStatusSuppressDepth > 0)
                _hostBuiltInReviveStatusSuppressDepth--;
        }

        static bool IsBuiltInReviveSettled(AbstractPlayerController player)
        {
            if (player == null)
                return false;

            var levelPlayer = player as LevelPlayerController;
            if (levelPlayer != null && levelPlayer.motor != null && levelPlayer.motor.Grounded)
                return true;

            return player.transform.position.y <= -225f;
        }

        static bool ShouldHandleHostBuiltInParryAnimComplete(PlayerDeathEffect effect, out PlayerId targetPlayerId)
        {
            targetPlayerId = PlayerId.PlayerOne;
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsHost
             || Plugin.Net == null
             || !Plugin.Net.IsConnected
             || effect == null)
            {
                return false;
            }

            ExtraParticipantDeathBubbleTag extraTag;
            if (ExtraParticipantReviveVisuals.IsExtraBubble(effect, out extraTag))
                return false;

            if (!TryGetDeathEffectPlayerId(effect, out targetPlayerId))
                return false;

            return targetPlayerId == PlayerId.PlayerOne || targetPlayerId == PlayerId.PlayerTwo;
        }

        static bool TryGetHostRevivePosition(PlayerId playerId, out Vector2 position)
        {
            var deathEffect = FindLocalDeathEffect(playerId);
            if (deathEffect != null)
            {
                position = deathEffect.transform.position;
                return true;
            }

            PlayerStatePacket snapshot;
            if (RemotePlayer.TryGetLocalAuthoritySnapshot(playerId, out snapshot))
            {
                position = new Vector2(snapshot.PosX, snapshot.PosY);
                return true;
            }

            position = Vector2.zero;
            return false;
        }

        static bool IsDeathEffectExiting(PlayerDeathEffect effect)
        {
            if (effect == null || DeathEffectExitingField == null)
                return false;

            object raw = DeathEffectExitingField.GetValue(effect);
            return raw is bool && (bool)raw;
        }

        static bool MarkGrantApplied(ReviveGrantPacket pkt)
        {
            if (AppliedGrantKeys.Count > 128)
                AppliedGrantKeys.Clear();

            long key = ((long)pkt.Tick << 24)
                     ^ ((long)pkt.TargetParticipantId << 16)
                     ^ ((long)pkt.DonorParticipantId << 8)
                     ^ pkt.Flags;
            return AppliedGrantKeys.Add(key);
        }

        static AbstractPlayerController GetPlayerSafe(PlayerId playerId)
        {
            try
            {
                return PlayerManager.GetPlayer(playerId);
            }
            catch
            {
                return null;
            }
        }

        static PlayerDeathEffect FindLocalDeathEffect(PlayerId localPlayerId)
        {
            ScratchDeathEffects.Clear();
            ScratchDeathEffects.AddRange(UnityEngine.Object.FindObjectsOfType<PlayerDeathEffect>());
            for (int i = 0; i < ScratchDeathEffects.Count; i++)
            {
                var effect = ScratchDeathEffects[i];
                if (effect == null)
                    continue;

                PlayerId effectPlayerId;
                if (!TryGetDeathEffectPlayerId(effect, out effectPlayerId))
                    continue;
                if (effectPlayerId == localPlayerId)
                    return effect;
            }

            return null;
        }

        static bool TryGetDeathEffectPlayerId(PlayerDeathEffect effect, out PlayerId playerId)
        {
            playerId = PlayerId.PlayerOne;
            if (effect == null || DeathEffectPlayerIdField == null)
                return false;

            object raw = DeathEffectPlayerIdField.GetValue(effect);
            if (!(raw is PlayerId))
                return false;

            playerId = (PlayerId)raw;
            return true;
        }
    }
}
