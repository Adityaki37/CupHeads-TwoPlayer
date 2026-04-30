using System;
using System.Collections.Generic;
using CupheadOnline.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupheadOnline.Sync
{
    internal static class HighLatencyInputSync
    {
        const float EnableThresholdSeconds = 0.15f;
        const float SafetyMarginSeconds = 0.40f;
        const int MaxQueuedFrames = 2048;

        struct DelayedFrame
        {
            public InputFramePacket Packet;
            public float DueAt;
        }

        sealed class LocalInputState
        {
            public readonly Queue<DelayedFrame> Queue = new Queue<DelayedFrame>(MaxQueuedFrames);
            public InputFramePacket Current;
            public InputFramePacket Previous;
            public bool HasCurrent;
            public uint LastRawButtonsForLog;
            public uint DownEdges;
            public uint UpEdges;
            public readonly int[] DownServedFrames = CreateServedFrameBuffer();
            public readonly int[] UpServedFrames = CreateServedFrameBuffer();
        }

        static readonly Dictionary<byte, LocalInputState> _localStates =
            new Dictionary<byte, LocalInputState>(2);

        static float _levelClockStartedAt = -1f;
        static float _lastModeLogAt = -1f;

        static HighLatencyInputSync()
        {
            MultiplayerSession.OnSessionEnded += Reset;
        }

        public static bool IsEnabled
        {
            get
            {
                return MultiplayerSession.IsActive
                    && Plugin.VanillaTwoPlayerOnline
                    && IsInLevelScene()
                    && GetDelaySeconds() >= EnableThresholdSeconds;
            }
        }

        public static bool ShouldSimulateBuiltInRemotePlayers()
        {
            return IsEnabled;
        }

        public static bool ShouldDelayNetworkGameplayInput(byte participantId)
        {
            return IsEnabled && participantId <= (byte)PlayerId.PlayerTwo;
        }

        public static bool ShouldDropUntimedNetworkGameplayInput(byte participantId, InputFramePacket pkt)
        {
            return IsEnabled
                && participantId <= (byte)PlayerId.PlayerTwo
                && pkt.InputTime < 0f;
        }

        public static bool ShouldUseHostAuthorityForLocalBuiltInPlayer(PlayerId playerId)
        {
            return false;
        }

        public static void Reset()
        {
            _localStates.Clear();
            _levelClockStartedAt = -1f;
            _lastModeLogAt = -1f;
        }

        public static void ResetLevelClock()
        {
            _localStates.Clear();
            ResetRemoteBuiltInInputs();
            _levelClockStartedAt = -1f;
            _lastModeLogAt = -1f;
        }

        public static void ResetLocalPlayerInput(PlayerId playerId)
        {
            _localStates.Remove((byte)playerId);
        }

        public static void NotifyLevelStartReleased(float elapsedSinceSharedRelease)
        {
            _levelClockStartedAt = Time.unscaledTime - Mathf.Clamp(elapsedSinceSharedRelease, 0f, 0.75f);
            _localStates.Clear();
            ResetRemoteBuiltInInputs();
            LogModeIfNeeded(true);
        }

        static void ResetRemoteBuiltInInputs()
        {
            RemoteInputDriver.Reset((byte)PlayerId.PlayerOne);
            RemoteInputDriver.Reset((byte)PlayerId.PlayerTwo);
        }

        public static float PacketTimeNow()
        {
            return _levelClockStartedAt >= 0f
                ? Mathf.Max(0f, Time.unscaledTime - _levelClockStartedAt)
                : -1f;
        }

        public static float PlayoutTimeNow()
        {
            return _levelClockStartedAt >= 0f
                ? Mathf.Max(0f, Time.unscaledTime - _levelClockStartedAt)
                : Time.unscaledTime;
        }

        public static float GetDelaySeconds()
        {
            float delay = EstimateOneWaySeconds();

            if (delay > 0f)
                delay += Mathf.Max(0, Plugin.LanArtificialJitterMs) / 1000f + SafetyMarginSeconds;

            return Mathf.Clamp(delay, 0f, 2.5f);
        }

        public static float EstimateOneWaySeconds()
        {
            if (Plugin.LanArtificialLatencyMs > 0)
                return Mathf.Clamp(Plugin.LanArtificialLatencyMs / 1000f, 0f, 2.5f);

            float delay = 0f;

            if (Plugin.Net != null && Plugin.Net.Latency > 0)
                delay = Mathf.Max(delay, Plugin.Net.Latency * 0.0005f);

            return Mathf.Clamp(delay, 0f, 2.5f);
        }

        public static float GetDueTime(InputFramePacket pkt)
        {
            float inputTime = pkt.InputTime >= 0f ? pkt.InputTime : PlayoutTimeNow();
            return inputTime + GetDelaySeconds();
        }

        public static void RecordLocalFrame(PlayerId playerId, InputFramePacket pkt)
        {
            if (!ShouldDelayLocalPlayer(playerId))
            {
                _localStates.Remove((byte)playerId);
                return;
            }

            if (pkt.InputTime < 0f)
                pkt.InputTime = PacketTimeNow();

            if (HasGameplayInput(pkt))
                ParticipantReviveController.CancelRecentBuiltInReviveCorrection(playerId);

            var state = GetOrCreateLocalState(playerId);
            float dueAt = GetDueTime(pkt);
            LogJumpDelayIfNeeded("local", (byte)playerId, pkt, dueAt, state.LastRawButtonsForLog);
            state.LastRawButtonsForLog = pkt.Buttons;
            state.Queue.Enqueue(new DelayedFrame
            {
                Packet = pkt,
                DueAt = dueAt,
            });

            while (state.Queue.Count > MaxQueuedFrames)
                PromoteLocal(state, state.Queue.Dequeue());

            AdvanceLocal(state);
            LogModeIfNeeded(false);
        }

        public static bool TryGetDelayedAxis(PlayerId playerId, int actionId, out float value)
        {
            value = 0f;
            if (!ShouldDelayLocalPlayer(playerId))
                return false;

            var state = GetOrCreateLocalState(playerId);
            AdvanceLocal(state);
            if (!state.HasCurrent)
                return true;

            value = actionId == 0 ? state.Current.AxisX : actionId == 1 ? state.Current.AxisY : 0f;
            return true;
        }

        public static bool TryGetDelayedButton(PlayerId playerId, int actionId, bool down, bool up, out bool value)
        {
            value = false;
            if (actionId < 0 || actionId >= 32)
                return false;
            if (!IsDelayedGameplayButton(actionId))
                return false;
            if (!ShouldDelayLocalPlayer(playerId))
                return false;

            var state = GetOrCreateLocalState(playerId);
            AdvanceLocal(state);

            if (down)
                value = ConsumeEdgeForFrame(state.DownEdges, state.DownServedFrames, actionId);
            else if (up)
                value = ConsumeEdgeForFrame(state.UpEdges, state.UpServedFrames, actionId);
            else
                value = state.HasCurrent && ((state.Current.Buttons & (1u << actionId)) != 0u);

            return true;
        }

        public static bool PeekPressedThisFrame(PlayerId playerId, CupheadButton button)
        {
            int actionId = (int)button;
            if (actionId < 0 || actionId >= 32)
                return false;
            if (!ShouldDelayLocalPlayer(playerId))
                return false;

            var state = GetOrCreateLocalState(playerId);
            AdvanceLocal(state);
            return (state.DownEdges & (1u << actionId)) != 0u;
        }

        static bool ShouldDelayLocalPlayer(PlayerId playerId)
        {
            return IsEnabled
                && playerId == MultiplayerSession.LocalId
                && playerId <= PlayerId.PlayerTwo;
        }

        static bool IsDelayedGameplayButton(int actionId)
        {
            var button = (CupheadButton)actionId;
            switch (button)
            {
                case CupheadButton.Jump:
                case CupheadButton.Shoot:
                case CupheadButton.Super:
                case CupheadButton.SwitchWeapon:
                case CupheadButton.Lock:
                case CupheadButton.Dash:
                    return true;
                default:
                    return false;
            }
        }

        static bool HasGameplayInput(InputFramePacket pkt)
        {
            if (Mathf.Abs(pkt.AxisX) > 0.15f || Mathf.Abs(pkt.AxisY) > 0.15f)
                return true;

            uint gameplayButtons = 0u;
            gameplayButtons |= 1u << (int)CupheadButton.Jump;
            gameplayButtons |= 1u << (int)CupheadButton.Shoot;
            gameplayButtons |= 1u << (int)CupheadButton.Super;
            gameplayButtons |= 1u << (int)CupheadButton.SwitchWeapon;
            gameplayButtons |= 1u << (int)CupheadButton.Lock;
            gameplayButtons |= 1u << (int)CupheadButton.Dash;
            return (pkt.Buttons & gameplayButtons) != 0u;
        }

        static LocalInputState GetOrCreateLocalState(PlayerId playerId)
        {
            var key = (byte)playerId;
            LocalInputState state;
            if (!_localStates.TryGetValue(key, out state))
            {
                state = new LocalInputState();
                _localStates[key] = state;
            }
            return state;
        }

        static void AdvanceLocal(LocalInputState state)
        {
            float now = PlayoutTimeNow();
            bool promotedAny = false;
            while (state.Queue.Count > 0)
            {
                var next = state.Queue.Peek();
                if (next.DueAt > now)
                    break;

                if (!promotedAny)
                {
                    state.DownEdges = 0u;
                    state.UpEdges = 0u;
                    ResetServedFrames(state.DownServedFrames);
                    ResetServedFrames(state.UpServedFrames);
                }

                state.Queue.Dequeue();
                PromoteLocal(state, next);
                promotedAny = true;
            }
        }

        static void PromoteLocal(LocalInputState state, DelayedFrame frame)
        {
            var previous = state.HasCurrent ? state.Current : default(InputFramePacket);
            state.Previous = previous;
            state.Current = frame.Packet;
            state.HasCurrent = true;
            state.DownEdges |= state.Current.Buttons & ~previous.Buttons;
            state.UpEdges |= previous.Buttons & ~state.Current.Buttons;
        }

        static bool IsInLevelScene()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            return !string.IsNullOrEmpty(sceneName)
                && sceneName.StartsWith("scene_level_", System.StringComparison.Ordinal);
        }

        static void LogModeIfNeeded(bool force)
        {
            if (!IsEnabled)
                return;

            float now = Time.unscaledTime;
            if (!force && _lastModeLogAt > 0f && now - _lastModeLogAt < 5f)
                return;

            _lastModeLogAt = now;
            Plugin.Log.LogInfo("[SyncClock] High-latency presentation clock active: estimated one-way+jitter="
                + GetDelaySeconds().ToString("0.000")
                + "s, packetTime="
                + PacketTimeNow().ToString("0.000")
                + "s.");
        }

        static bool ConsumeEdgeForFrame(uint edges, int[] servedFrames, int buttonIndex)
        {
            uint mask = 1u << buttonIndex;
            if ((edges & mask) == 0u)
                return false;

            if (!Time.inFixedTimeStep)
                return true;

            int frame = Time.frameCount;
            if (servedFrames[buttonIndex] == frame)
                return true;
            if (servedFrames[buttonIndex] >= 0)
                return false;

            servedFrames[buttonIndex] = frame;
            return true;
        }

        static int[] CreateServedFrameBuffer()
        {
            var frames = new int[32];
            ResetServedFrames(frames);
            return frames;
        }

        static void LogJumpDelayIfNeeded(string side, byte participantId, InputFramePacket pkt, float dueAt, uint previousButtons)
        {
            if (!Plugin.AutoRunLanSteamE2E)
                return;

            uint jumpMask = 1u << (int)CupheadButton.Jump;
            if ((pkt.Buttons & jumpMask) == 0u || (previousButtons & jumpMask) != 0u)
                return;

            Plugin.Log.LogInfo("[SyncClock] " + side
                + " jump down p" + participantId
                + " inputTime=" + pkt.InputTime.ToString("0.000")
                + " due=" + dueAt.ToString("0.000")
                + " now=" + PlayoutTimeNow().ToString("0.000")
                + " delay=" + GetDelaySeconds().ToString("0.000")
                + ".");
        }

        static void ResetServedFrames(int[] frames)
        {
            if (frames == null)
                return;

            for (int i = 0; i < frames.Length; i++)
                frames[i] = -1;
        }
    }
}
