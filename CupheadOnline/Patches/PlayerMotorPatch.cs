using HarmonyLib;
using UnityEngine;
using CupheadOnline.Net;
using CupheadOnline.Sync;

namespace CupheadOnline.Patches
{
    /// <summary>
    /// Patches LevelPlayerMotor.FixedUpdate — the heart of all movement physics.
    ///
    /// Strategy:
    ///   LOCAL player  → let original run, then broadcast PlayerStatePacket.
    ///   REMOTE player → skip original (Prefix returns false), apply interpolated
    ///                   network position + set look direction for animation.
    ///
    /// Why Prefix+Postfix rather than a single patch:
    ///   The Prefix decides skip/run based on session state; the Postfix only
    ///   executes when the original was NOT skipped (HarmonyLib contract).
    ///   For the remote player we do everything in the Prefix.
    /// </summary>
    [HarmonyPatch(typeof(LevelPlayerMotor), "FixedUpdate")]
    public static class PlayerMotorPatch
    {
        // ── Prefix ────────────────────────────────────────────────────────────

        static bool Prefix(LevelPlayerMotor __instance)
        {
            if (!MultiplayerSession.IsActive) return true; // singleplayer pass-through

            var player = __instance.player;
            if (player == null) return true;

            if (MultiplayerSession.IsRemotePlayer(player.id))
            {
                // ── REMOTE PLAYER: replace FixedUpdate entirely ───────────────
                ApplyRemoteState(__instance);
                return false; // skip original
            }

            // ── LOCAL PLAYER: increment tick before original runs ─────────────
            MultiplayerSession.IncrementTick();
            return true;
        }

        // ── Postfix (only runs when Prefix returned true — i.e. local player) ─

        static void Postfix(LevelPlayerMotor __instance)
        {
            if (!MultiplayerSession.IsActive) return;

            var player = __instance.player;
            if (player == null) return;
            if (!MultiplayerSession.IsLocalPlayer(player.id)) return;

            // Build and send state packet
            var flags = BuildFlags(__instance);
            var animHash = GetAnimHash(player);

            var pkt = new PlayerStatePacket
            {
                PlayerId  = (byte)player.id,
                PosX      = __instance.transform.position.x,
                PosY      = __instance.transform.position.y,
                LookX     = (sbyte)__instance.LookDirection.x.Value,
                LookY     = (sbyte)__instance.LookDirection.y.Value,
                Flags     = flags,
                AnimState = animHash,
                Tick      = MultiplayerSession.Tick,
            };

            if (MultiplayerSession.IsHost)
                Plugin.Net.SendPlayerState(ref pkt);
            else
                SendInputFrameAndState(__instance, player, ref pkt);
        }

        // ─────────────────────────────────────────────────────────────────────
        //  Helpers
        // ─────────────────────────────────────────────────────────────────────

        static byte BuildFlags(LevelPlayerMotor m)
        {
            byte f = 0;
            if (m.Grounded)          f |= 1;
            if (m.Dashing)           f |= 2;
            if (m.Ducking)           f |= 4;
            if (m.GravityReversed)   f |= 8;
            if (m.IsHit)             f |= 16;
            if (m.IsUsingSuperOrEx)  f |= 32;
            return f;
        }

        static byte GetAnimHash(LevelPlayerController player)
        {
            var anim = player.animationController?.animator;
            if (anim == null) return 0;
            return (byte)(anim.GetCurrentAnimatorStateInfo(0).fullPathHash & 0xFF);
        }

        /// <summary>
        /// Client-specific: also build and send the local input frame so the host
        /// can drive PlayerTwo's motor authoritatively.
        /// </summary>
        static void SendInputFrameAndState(LevelPlayerMotor motor, LevelPlayerController player,
                                           ref PlayerStatePacket statePkt)
        {
            Plugin.Net.SendPlayerState(ref statePkt);

            var input   = player.input;
            if (input == null) return;

            uint buttons = 0;
            // Pack the subset of buttons that affect gameplay
            TryPackButton(input, CupheadButton.Jump,         ref buttons);
            TryPackButton(input, CupheadButton.Shoot,        ref buttons);
            TryPackButton(input, CupheadButton.Super,        ref buttons);
            TryPackButton(input, CupheadButton.Dash,         ref buttons);
            TryPackButton(input, CupheadButton.Lock,         ref buttons);
            TryPackButton(input, CupheadButton.SwitchWeapon, ref buttons);

            var inPkt = new InputFramePacket
            {
                AxisX   = input.GetAxis(PlayerInput.Axis.X),
                AxisY   = input.GetAxis(PlayerInput.Axis.Y),
                Buttons = buttons,
                Tick    = MultiplayerSession.Tick,
            };
            Plugin.Net.SendInputFrame(ref inPkt);
        }

        static void TryPackButton(PlayerInput input, CupheadButton btn, ref uint bits)
        {
            if (input.GetButton(btn))
                bits |= 1u << (int)btn;
        }

        /// <summary>
        /// Called instead of FixedUpdate for the remote player.
        /// Applies interpolated position and syncs motor properties used by the animator.
        /// </summary>
        static void ApplyRemoteState(LevelPlayerMotor motor)
        {
            var snapshot = RemotePlayer.GetNextSnapshot();
            if (!snapshot.HasValue) return;

            var s = snapshot.Value;

            // ── Position ──────────────────────────────────────────────────────
            var target = new Vector3(s.PosX, s.PosY, motor.transform.position.z);
            // Smooth snap: lerp 80 % toward target per frame to hide jitter
            motor.transform.position = Vector3.Lerp(
                motor.transform.position,
                target,
                Mathf.Min(1f, 20f * Time.fixedDeltaTime));

            // ── Motor properties (drive animation without physics) ────────────
            // Property setters may be non-public in the compiled binary; use Traverse
            var t = HarmonyLib.Traverse.Create(motor);
            t.Property("LookDirection").SetValue(new Trilean2(s.LookX, s.LookY));
            t.Property("TrueLookDirection").SetValue(new Trilean2(s.LookX, s.LookY));
            t.Property("Grounded").SetValue(s.Grounded);
            t.Property("GravityReversed").SetValue(s.GravReversed);

            // Track state transitions to fire animation events
            RemotePlayer.UpdateStateTransitions(motor, s);
        }
    }

    // ──────────────────────────────────────────────────────────────────────────
    //  PlayerInput patches — intercept axis/button reads for the remote player
    //  so the motor feeds from received network data instead of controller.
    // ──────────────────────────────────────────────────────────────────────────

    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.GetAxis))]
    public static class PlayerInputAxisPatch
    {
        static bool Prefix(PlayerInput __instance, PlayerInput.Axis axis, ref float __result)
        {
            if (!MultiplayerSession.IsActive) return true;
            if (!MultiplayerSession.IsRemotePlayer(__instance.playerId)) return true;

            __result = axis == PlayerInput.Axis.X
                ? RemoteInputDriver.Current.AxisX
                : RemoteInputDriver.Current.AxisY;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.GetAxisInt))]
    public static class PlayerInputAxisIntPatch
    {
        static bool Prefix(PlayerInput __instance, PlayerInput.Axis axis, ref int __result)
        {
            if (!MultiplayerSession.IsActive) return true;
            if (!MultiplayerSession.IsRemotePlayer(__instance.playerId)) return true;

            float v = axis == PlayerInput.Axis.X
                ? RemoteInputDriver.Current.AxisX
                : RemoteInputDriver.Current.AxisY;
            __result = v > 0.38f ? 1 : v < -0.38f ? -1 : 0;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.GetButton))]
    public static class PlayerInputButtonPatch
    {
        static bool Prefix(PlayerInput __instance, CupheadButton button, ref bool __result)
        {
            if (!MultiplayerSession.IsActive) return true;
            if (!MultiplayerSession.IsRemotePlayer(__instance.playerId)) return true;

            __result = RemoteInputDriver.Current.IsPressed(button);
            return false;
        }
    }
}
