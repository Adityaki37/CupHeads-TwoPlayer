using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using CupheadOnline.Net;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// Manages the visual representation of the remote player.
    ///
    /// Flow:
    ///   1. PacketDispatcher calls OnStateReceived() with each incoming PlayerStatePacket.
    ///   2. Packets are buffered (jitter buffer) to absorb network variability.
    ///   3. PlayerMotorPatch calls GetNextSnapshot() each FixedUpdate to consume one frame.
    ///   4. The patch applies position + direction + fires animation events.
    ///
    /// Jitter buffer target: 2 frames (≈ 33 ms at 60 Hz).
    /// If the buffer drains (starvation), we extrapolate from the last snapshot.
    /// If the buffer grows beyond 4 frames, we consume extras to catch up.
    /// </summary>
    public static class RemotePlayer
    {
        // ──────────────────────────────────────────────────────────────────────
        //  Jitter buffer
        // ──────────────────────────────────────────────────────────────────────
        private const int TARGET_BUFFER   = 2;
        private const int MAX_BUFFER      = 6;

        private static readonly Queue<PlayerStatePacket> _buffer = new Queue<PlayerStatePacket>(MAX_BUFFER);
        private static PlayerStatePacket _last;
        private static bool              _hasLast;

        // Previous flags — detect state transitions to fire animation events
        private static byte _prevFlags;

        // Reflection cache for raising VB.NET event backing delegates on LevelPlayerMotor
        // VB compiles `Public Event Foo As Action` → private field named `FooEvent`
        private static FieldInfo _fiGrounded;
        private static FieldInfo _fiDashStart;
        private static FieldInfo _fiDashEnd;
        private static FieldInfo _fiHit;

        static RemotePlayer()
        {
            var t = typeof(LevelPlayerMotor);
            const BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance;
            // Try VB naming convention (Event suffix) first, then plain name
            _fiGrounded  = t.GetField("OnGroundedEventEvent",  bf) ?? t.GetField("OnGroundedEvent",  bf);
            _fiDashStart = t.GetField("OnDashStartEventEvent", bf) ?? t.GetField("OnDashStartEvent", bf);
            _fiDashEnd   = t.GetField("OnDashEndEventEvent",   bf) ?? t.GetField("OnDashEndEvent",   bf);
            _fiHit       = t.GetField("OnHitEventEvent",       bf) ?? t.GetField("OnHitEvent",       bf);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  Public API
        // ──────────────────────────────────────────────────────────────────────

        public static void OnStateReceived(PlayerStatePacket pkt)
        {
            // Only buffer packets for the remote player
            if (pkt.PlayerId != (byte)MultiplayerSession.RemoteId) return;

            if (_buffer.Count >= MAX_BUFFER)
                _buffer.Dequeue(); // drop oldest to prevent runaway lag

            _buffer.Enqueue(pkt);
        }

        /// <summary>
        /// Called by PlayerMotorPatch every FixedUpdate for the remote motor.
        /// Returns null if buffer is empty (caller should extrapolate).
        /// </summary>
        public static PlayerStatePacket? GetNextSnapshot()
        {
            // Catch-up: if buffer is oversized, consume multiple frames silently
            while (_buffer.Count > TARGET_BUFFER + 2 && _buffer.Count > 1)
                _last = _buffer.Dequeue();

            if (_buffer.Count > 0)
            {
                _last    = _buffer.Dequeue();
                _hasLast = true;
                return _last;
            }

            // Starvation: extrapolate (return last known with same flags)
            return _hasLast ? (PlayerStatePacket?)_last : null;
        }

        /// <summary>
        /// Detects transitions in the remote player's motor flags and raises the
        /// corresponding events on their LevelPlayerMotor so that
        /// LevelPlayerAnimationController reacts normally (grounded landing, dash FX, etc.)
        /// </summary>
        public static void UpdateStateTransitions(LevelPlayerMotor motor, PlayerStatePacket s)
        {
            byte prev = _prevFlags;
            byte cur  = s.Flags;

            bool wasGrounded = (prev & 1)  != 0;
            bool nowGrounded = (cur  & 1)  != 0;
            bool wasDashing  = (prev & 2)  != 0;
            bool nowDashing  = (cur  & 2)  != 0;
            bool wasHit      = (prev & 16) != 0;
            bool nowHit      = (cur  & 16) != 0;

            if (!wasGrounded && nowGrounded) RaiseEvent(motor, _fiGrounded);
            if (!wasDashing  && nowDashing)  RaiseEvent(motor, _fiDashStart);
            if (wasDashing   && !nowDashing) RaiseEvent(motor, _fiDashEnd);
            if (!wasHit      && nowHit)      RaiseEvent(motor, _fiHit);

            _prevFlags = cur;
        }

        public static void Reset()
        {
            _buffer.Clear();
            _hasLast   = false;
            _prevFlags = 0;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  Helpers
        // ──────────────────────────────────────────────────────────────────────

        static void RaiseEvent(LevelPlayerMotor motor, FieldInfo fi)
        {
            if (fi == null) return;
            try
            {
                var del = fi.GetValue(motor) as Action;
                del?.Invoke();
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning($"[RemotePlayer] Event raise failed: {ex.Message}");
            }
        }
    }
}
