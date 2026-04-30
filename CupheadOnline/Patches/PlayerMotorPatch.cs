using HarmonyLib;
using System;
using System.Collections.Generic;
using UnityEngine;
using CupheadOnline.Net;
using CupheadOnline.Sync;

namespace CupheadOnline.Patches
{
    /// <summary>
    /// Patches LevelPlayerMotor.FixedUpdate.
    ///
    /// Host: both built-in gameplay slots run the real Cuphead motor so the host
    /// remains authoritative for movement, collisions, jump state, death, and
    /// animation transitions.
    ///
    /// Client: the host-controlled slot normally stays a proxy, but high-latency
    /// vanilla two-player mode runs both built-in motors from the delayed input
    /// stream so sudden collisions/revives are simulated at the same battle time
    /// on both peers instead of arriving as late position snapshots.
    /// </summary>
    [HarmonyPatch(typeof(LevelPlayerMotor), "FixedUpdate")]
    public static class PlayerMotorPatch
    {
        struct MotionSample
        {
            public bool HasSample;
            public Vector2 Position;
            public Vector2 Velocity;
            public float Time;
        }

        static readonly Dictionary<byte, MotionSample> LastBuiltMotion =
            new Dictionary<byte, MotionSample>(2);
        const float BuiltInReviveSettledY = -225f;

        static PlayerMotorPatch()
        {
            MultiplayerSession.OnSessionEnded += ResetMotionSamples;
        }

        static bool Prefix(LevelPlayerMotor __instance)
        {
            if (!MultiplayerSession.IsActive)
                return true;

            var player = __instance.player;
            if (player == null)
                return true;

            byte extraParticipantId;
            if (ExtraRemoteAvatarManager.TryGetAvatarParticipantId(__instance, out extraParticipantId))
            {
                RemoteInputDriver.Tick(extraParticipantId);
                ApplyRemoteState(__instance, extraParticipantId);
                return false;
            }

            if (MultiplayerSession.IsNetworkControlledPlayer(player.id))
            {
                RemoteInputDriver.Tick(player.id);

                if (player.id <= PlayerId.PlayerTwo
                 && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers())
                {
                    return true;
                }

                // The host simulates the guest with the real gameplay motor.
                if (MultiplayerSession.IsHost && player.id <= PlayerId.PlayerTwo)
                    return true;

                ApplyRemoteState(__instance, (byte)player.id);
                return false;
            }

            if (HighLatencyInputSync.ShouldUseHostAuthorityForLocalBuiltInPlayer(player.id))
            {
                RemotePlayer.TryApplyLocalAuthoritySnapshot(player.id);
                return false;
            }

            MultiplayerSession.IncrementTick();
            return true;
        }

        static void Postfix(LevelPlayerMotor __instance)
        {
            if (!MultiplayerSession.IsActive)
                return;
            if (Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            var player = __instance.player;
            if (player == null)
                return;

            bool authoritativeBuiltIn = MultiplayerSession.IsHost && player.id <= PlayerId.PlayerTwo;
            bool localPlayer = MultiplayerSession.IsLocalPlayer(player.id);
            bool clientSimulatedBuiltIn = MultiplayerSession.IsClient
                && localPlayer
                && player.id <= PlayerId.PlayerTwo
                && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers();

            if (clientSimulatedBuiltIn)
                ApplyClientBuiltInReviveCorrection(__instance, player.id);

            if (!authoritativeBuiltIn && !localPlayer)
                return;

            var pkt = BuildStatePacket(player, __instance);

            if (authoritativeBuiltIn)
                Plugin.Net.SendPlayerState(ref pkt);
            else if (localPlayer)
            {
                SendInputFrameAndState(__instance, player, ref pkt);
                if (MultiplayerSession.IsClient
                 && MultiplayerSession.IsLocalPlayer(player.id)
                 && !ShouldLetBuiltInMotorOwnHighLatencyPosition(player.id))
                {
                    ApplyAuthoritativeCorrection(__instance, player.id);
                }
            }
        }

        internal static byte BuildFlags(AbstractPlayerController player, LevelPlayerMotor m)
        {
            byte f = 0;
            if (m.Grounded) f |= 1;
            if (m.Dashing) f |= 2;
            if (m.Ducking) f |= 4;
            if (m.GravityReversed) f |= 8;
            if (m.IsHit) f |= 16;
            if (m.IsUsingSuperOrEx) f |= 32;
            if (player != null && player.IsDead) f |= 64;
            return f;
        }

        internal static PlayerStatePacket BuildStatePacket(LevelPlayerController player, LevelPlayerMotor motor)
        {
            int animHash;
            float animTime;
            GetAnimState(player, out animHash, out animTime);
            Vector2 position = motor.transform.position;
            Vector2 velocity = EstimateOutgoingVelocity((byte)player.id, position);

            return new PlayerStatePacket
            {
                PlayerId = (byte)player.id,
                PosX = position.x,
                PosY = position.y,
                LookX = (sbyte)motor.TrueLookDirection.x.Value,
                LookY = (sbyte)motor.TrueLookDirection.y.Value,
                Flags = BuildFlags(player, motor),
                AnimState = (byte)(animHash & 0xFF),
                Tick = MultiplayerSession.Tick,
                AnimHash = animHash,
                AnimNormalizedTime = animTime,
                StateTime = HighLatencyInputSync.PacketTimeNow(),
                VelX = velocity.x,
                VelY = velocity.y,
                StateUtcTicks = DateTime.UtcNow.Ticks,
            };
        }

        internal static byte GetAnimHash(LevelPlayerController player)
        {
            int animHash;
            float animTime;
            GetAnimState(player, out animHash, out animTime);
            return (byte)(animHash & 0xFF);
        }

        static void GetAnimState(LevelPlayerController player, out int animHash, out float normalizedTime)
        {
            animHash = 0;
            normalizedTime = 0f;

            var anim = player.animationController?.animator;
            if (anim == null)
                return;

            var state = anim.GetCurrentAnimatorStateInfo(0);
            animHash = state.fullPathHash;
            normalizedTime = Mathf.Repeat(state.normalizedTime, 1f);
        }

        static void SendInputFrameAndState(LevelPlayerMotor motor, LevelPlayerController player, ref PlayerStatePacket statePkt)
        {
            Plugin.Net.SendPlayerState(ref statePkt);
        }

        static void ApplyRemoteState(LevelPlayerMotor motor, byte participantId)
        {
            var snapshot = RemotePlayer.GetNextSnapshot(participantId, mapState: false);
            if (participantId <= (byte)PlayerId.PlayerTwo
             && ParticipantReviveController.TrySuppressRemoteBuiltInDeadBody(motor.player))
            {
                return;
            }

            if (!snapshot.HasValue)
                return;

            var s = snapshot.Value;
            var target = GetRawPosition(s, motor.transform.position.z);
            if (Plugin.VanillaTwoPlayerOnline && participantId <= (byte)PlayerId.PlayerTwo)
            {
                motor.transform.position = target;
            }
            else
            {
                motor.transform.position = Vector3.Lerp(
                    motor.transform.position,
                    target,
                    Mathf.Min(1f, 20f * Time.fixedDeltaTime));
            }

            var t = Traverse.Create(motor);
            t.Property("LookDirection").SetValue(new Trilean2(s.LookX, s.LookY));
            t.Property("TrueLookDirection").SetValue(new Trilean2(s.LookX, s.LookY));

            InputFramePacket input;
            if (RemoteInputDriver.TryGetCurrent(participantId, out input))
            {
                t.Property("MoveDirection").SetValue(new Trilean2(
                    input.AxisX > 0.38f ? 1 : input.AxisX < -0.38f ? -1 : 0,
                    input.AxisY > 0.38f ? 1 : input.AxisY < -0.38f ? -1 : 0));
                ApplyRemoteShootingFlag(motor.player, input.IsPressed(CupheadButton.Shoot));
            }
            else
            {
                t.Property("MoveDirection").SetValue(new Trilean2(0, 0));
                ApplyRemoteShootingFlag(motor.player, false);
            }

            t.Property("Grounded").SetValue(s.Grounded);
            t.Property("Dashing").SetValue(s.Dashing);
            t.Property("Ducking").SetValue(s.Ducking);
            t.Property("Locked").SetValue(false);
            t.Property("GravityReversed").SetValue(s.GravReversed);
            t.Property("IsHit").SetValue(s.IsHit);
            t.Property("IsUsingSuperOrEx").SetValue(s.IsSuper);

            RemotePlayer.UpdateStateTransitions(participantId, motor, s);
            ApplyRemoteAnimation(motor.player, s);
        }

        static void ApplyAuthoritativeCorrection(LevelPlayerMotor motor, PlayerId playerId)
        {
            if (ShouldLetBuiltInMotorOwnHighLatencyPosition(playerId))
                return;

            PlayerStatePacket snapshot;
            if (!TryGetHostAuthoritativeSnapshot(playerId, out snapshot))
                return;
            if (snapshot.IsMapState)
                return;

            if (snapshot.IsDead)
                return;

            bool highLatencyBuiltIn = Plugin.VanillaTwoPlayerOnline
                && MultiplayerSession.IsClient
                && playerId <= PlayerId.PlayerTwo
                && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers();
            var target = highLatencyBuiltIn
                ? GetPresentationPosition(snapshot, motor.transform.position.z)
                : GetRawPosition(snapshot, motor.transform.position.z);
            float distance = Vector2.Distance(motor.transform.position, target);
            bool tightBuiltInSync = Plugin.VanillaTwoPlayerOnline && playerId <= PlayerId.PlayerTwo;
            float deadZone = tightBuiltInSync ? 0.08f : Plugin.LatencyFriendlyDamage ? 0.85f : 0.35f;
            float snapDistance = tightBuiltInSync ? 1.25f : Plugin.LatencyFriendlyDamage ? 8f : 4f;
            float blend = tightBuiltInSync ? 0.65f : Plugin.LatencyFriendlyDamage ? 0.08f : 0.18f;

            if (highLatencyBuiltIn)
            {
                if (distance < 0.35f)
                {
                    ApplySnapshotMotorState(motor, snapshot);
                    return;
                }

                motor.transform.position = distance > 8f
                    ? target
                    : Vector3.Lerp(motor.transform.position, target, 0.85f);
                ApplySnapshotMotorState(motor, snapshot);
                return;
            }

            if (distance < deadZone)
                return;

            if (tightBuiltInSync && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers())
            {
                motor.transform.position = target;
                return;
            }

            motor.transform.position = distance > snapDistance
                ? target
                : Vector3.Lerp(motor.transform.position, target, blend);
        }

        static bool TryGetHostAuthoritativeSnapshot(PlayerId playerId, out PlayerStatePacket snapshot)
        {
            return RemotePlayer.TryGetLocalAuthoritySnapshot(playerId, out snapshot);
        }

        static void ApplyClientBuiltInReviveCorrection(LevelPlayerMotor motor, PlayerId playerId)
        {
            if (motor == null
             || !ParticipantReviveController.ShouldCorrectRecentlyRevivedBuiltInPlayer(playerId))
            {
                return;
            }

            PlayerStatePacket snapshot;
            if (!RemotePlayer.TryGetLocalAuthoritySnapshot(playerId, out snapshot)
             || snapshot.IsMapState)
            {
                return;
            }
            if (snapshot.IsDead)
                return;

            var target = GetRawPosition(snapshot, motor.transform.position.z);
            float distance = Vector2.Distance(motor.transform.position, target);
            if (snapshot.Grounded || snapshot.PosY <= BuiltInReviveSettledY)
            {
                motor.transform.position = target;
                ApplySnapshotMotorState(motor, snapshot);
                return;
            }

            if (distance < 0.5f)
            {
                ApplySnapshotMotorState(motor, snapshot);
                return;
            }

            motor.transform.position = distance > 12f
                ? target
                : Vector3.Lerp(motor.transform.position, target, 0.85f);
            ApplySnapshotMotorState(motor, snapshot);
        }

        static void ApplyHighLatencyRemoteBuiltInPosition(LevelPlayerMotor motor, PlayerId playerId)
        {
            if (!ShouldUseRemoteBuiltInPositionSnapshot(playerId))
                return;

            var snapshot = RemotePlayer.GetNextSnapshot(playerId, mapState: false);
            if (!snapshot.HasValue)
                return;

            var s = snapshot.Value;
            var target = GetPresentationPosition(s, motor.transform.position.z);
            float distance = Vector2.Distance(motor.transform.position, target);

            if (distance <= 0.08f)
                return;

            motor.transform.position = distance > 8f
                ? target
                : Vector3.Lerp(motor.transform.position, target, 0.75f);

            var t = Traverse.Create(motor);
            t.Property("LookDirection").SetValue(new Trilean2(s.LookX, s.LookY));
            t.Property("TrueLookDirection").SetValue(new Trilean2(s.LookX, s.LookY));
            t.Property("Grounded").SetValue(s.Grounded);
            t.Property("Dashing").SetValue(s.Dashing);
            t.Property("Ducking").SetValue(s.Ducking);
            t.Property("GravityReversed").SetValue(s.GravReversed);
            t.Property("IsHit").SetValue(s.IsHit);
            t.Property("IsUsingSuperOrEx").SetValue(s.IsSuper);

            RemotePlayer.UpdateStateTransitions(playerId, motor, s);
            ApplyRemoteAnimation(motor.player, s);
        }

        static void ApplySnapshotMotorState(LevelPlayerMotor motor, PlayerStatePacket snapshot)
        {
            if (motor == null)
                return;

            var t = Traverse.Create(motor);
            t.Property("LookDirection").SetValue(new Trilean2(snapshot.LookX, snapshot.LookY));
            t.Property("TrueLookDirection").SetValue(new Trilean2(snapshot.LookX, snapshot.LookY));
            t.Property("Grounded").SetValue(snapshot.Grounded);
            t.Property("Dashing").SetValue(snapshot.Dashing);
            t.Property("Ducking").SetValue(snapshot.Ducking);
            t.Property("GravityReversed").SetValue(snapshot.GravReversed);
            t.Property("IsHit").SetValue(snapshot.IsHit);
            t.Property("IsUsingSuperOrEx").SetValue(snapshot.IsSuper);
            var velocity = new Vector2(snapshot.VelX, snapshot.VelY);
            if (snapshot.Grounded || snapshot.PosY <= BuiltInReviveSettledY)
                velocity = Vector2.zero;

            TrySetMotorVelocity(motor, velocity);
            ApplyRemoteAnimation(motor.player, snapshot);
        }

        static void TrySetMotorVelocity(LevelPlayerMotor motor, Vector2 velocity)
        {
            try
            {
                Traverse.Create(motor).Property("velocity").SetValue(velocity);
            }
            catch
            {
            }
        }

        static Vector2 EstimateOutgoingVelocity(byte playerId, Vector2 position)
        {
            float now = Time.unscaledTime;
            MotionSample previous;
            Vector2 velocity = Vector2.zero;
            if (LastBuiltMotion.TryGetValue(playerId, out previous) && previous.HasSample)
            {
                float dt = now - previous.Time;
                if (dt > 0.001f && dt < 0.5f)
                {
                    var measured = (position - previous.Position) / dt;
                    if (measured.magnitude > 900f)
                        measured = previous.Velocity;
                    velocity = Vector2.Lerp(previous.Velocity, measured, 0.25f);
                    if (velocity.magnitude > 420f)
                        velocity = velocity.normalized * 420f;
                }
            }

            LastBuiltMotion[playerId] = new MotionSample
            {
                HasSample = true,
                Position = position,
                Velocity = velocity,
                Time = now,
            };
            return velocity;
        }

        static bool ShouldLetBuiltInMotorOwnHighLatencyPosition(PlayerId playerId)
        {
            return Plugin.VanillaTwoPlayerOnline
                && playerId <= PlayerId.PlayerTwo
                && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers();
        }

        static bool ShouldUseRemoteBuiltInPositionSnapshot(PlayerId playerId)
        {
            return Plugin.VanillaTwoPlayerOnline
                && MultiplayerSession.IsClient
                && MultiplayerSession.IsNetworkControlledPlayer(playerId)
                && playerId <= PlayerId.PlayerTwo
                && HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers();
        }

        static Vector3 GetRawPosition(PlayerStatePacket snapshot, float z)
        {
            return new Vector3(snapshot.PosX, snapshot.PosY, z);
        }

        static Vector3 GetPresentationPosition(PlayerStatePacket snapshot, float z)
        {
            var target = new Vector3(snapshot.PosX, snapshot.PosY, z);
            if (!Plugin.VanillaTwoPlayerOnline
             || snapshot.PlayerId > (byte)PlayerId.PlayerTwo
             || !HighLatencyInputSync.ShouldSimulateBuiltInRemotePlayers())
            {
                return target;
            }

            float age = 0f;
            if (snapshot.StateTime >= 0f)
            {
                float clockAge = HighLatencyInputSync.PlayoutTimeNow() - snapshot.StateTime;
                if (clockAge >= 0f && clockAge <= 2.5f)
                    age = clockAge;
            }
            else if (snapshot.StateUtcTicks > 0L)
            {
                double utcAge = (DateTime.UtcNow.Ticks - snapshot.StateUtcTicks) / (double)TimeSpan.TicksPerSecond;
                if (utcAge >= 0.0 && utcAge <= 2.5)
                    age = (float)utcAge;
            }

            age = Mathf.Clamp(age, 0f, 1.25f);
            var velocity = new Vector2(snapshot.VelX, snapshot.VelY);
            if (velocity.magnitude > 420f)
                velocity = velocity.normalized * 420f;
            var lead = velocity * age;
            float maxLead = Mathf.Max(20f, 90f * age);
            if (lead.magnitude > maxLead)
                lead = lead.normalized * maxLead;

            return new Vector3(snapshot.PosX + lead.x, snapshot.PosY + lead.y, z);
        }

        static void ResetMotionSamples()
        {
            LastBuiltMotion.Clear();
        }

        static void ApplyRemoteShootingFlag(LevelPlayerController player, bool shooting)
        {
            var anim = player?.animationController?.animator;
            if (anim == null)
                return;

            anim.SetBool("Shooting", shooting);
        }

        internal static void ApplyRemoteAnimation(LevelPlayerController player, PlayerStatePacket snapshot)
        {
            if (snapshot.PlayerId > (byte)PlayerId.PlayerTwo)
                return;

            var anim = player?.animationController?.animator;
            if (anim == null || snapshot.AnimHash == 0)
                return;

            var local = anim.GetCurrentAnimatorStateInfo(0);
            float localTime = Mathf.Repeat(local.normalizedTime, 1f);
            float remoteTime = Mathf.Repeat(snapshot.AnimNormalizedTime, 1f);
            float wrappedDelta = Mathf.Abs(Mathf.Repeat(localTime - remoteTime + 0.5f, 1f) - 0.5f);

            if (local.fullPathHash != snapshot.AnimHash || wrappedDelta > 0.22f)
                anim.Play(snapshot.AnimHash, 0, remoteTime);
        }
    }

    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.GetAxis))]
    public static class PlayerInputAxisPatch
    {
        static bool Prefix(PlayerInput __instance, PlayerInput.Axis axis, ref float __result)
        {
            float scripted;
            if (LocalDevE2ETest.TryGetLocalAxis(__instance.playerId, axis == PlayerInput.Axis.X ? 0 : 1, out scripted))
            {
                __result = scripted;
                return false;
            }

            float delayed;
            if (HighLatencyInputSync.TryGetDelayedAxis(__instance.playerId, axis == PlayerInput.Axis.X ? 0 : 1, out delayed))
            {
                __result = delayed;
                return false;
            }

            if (!MultiplayerSession.IsActive)
                return true;
            if (!MultiplayerSession.IsNetworkControlledPlayer(__instance.playerId))
                return true;

            InputFramePacket input;
            if (!RemoteInputDriver.TryGetCurrent(__instance.playerId, out input))
            {
                __result = 0f;
                return false;
            }

            __result = axis == PlayerInput.Axis.X ? input.AxisX : input.AxisY;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.GetAxisInt))]
    public static class PlayerInputAxisIntPatch
    {
        static bool Prefix(PlayerInput __instance, PlayerInput.Axis axis, ref int __result)
        {
            float scripted;
            if (LocalDevE2ETest.TryGetLocalAxis(__instance.playerId, axis == PlayerInput.Axis.X ? 0 : 1, out scripted))
            {
                __result = scripted > 0.38f ? 1 : scripted < -0.38f ? -1 : 0;
                return false;
            }

            float delayed;
            if (HighLatencyInputSync.TryGetDelayedAxis(__instance.playerId, axis == PlayerInput.Axis.X ? 0 : 1, out delayed))
            {
                __result = delayed > 0.38f ? 1 : delayed < -0.38f ? -1 : 0;
                return false;
            }

            if (!MultiplayerSession.IsActive)
                return true;
            if (!MultiplayerSession.IsNetworkControlledPlayer(__instance.playerId))
                return true;

            InputFramePacket input;
            if (!RemoteInputDriver.TryGetCurrent(__instance.playerId, out input))
            {
                __result = 0;
                return false;
            }

            float v = axis == PlayerInput.Axis.X ? input.AxisX : input.AxisY;
            __result = v > 0.38f ? 1 : v < -0.38f ? -1 : 0;
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerInput), nameof(PlayerInput.GetButton))]
    public static class PlayerInputButtonPatch
    {
        static bool Prefix(PlayerInput __instance, CupheadButton button, ref bool __result)
        {
            bool scripted;
            if (LocalDevE2ETest.TryGetLocalButton(__instance.playerId, (int)button, false, false, out scripted))
            {
                __result = scripted;
                return false;
            }

            bool delayed;
            if (HighLatencyInputSync.TryGetDelayedButton(__instance.playerId, (int)button, false, false, out delayed))
            {
                __result = delayed;
                return false;
            }

            if (!MultiplayerSession.IsActive)
                return true;
            if (!MultiplayerSession.IsNetworkControlledPlayer(__instance.playerId))
                return true;

            InputFramePacket input;
            if (!RemoteInputDriver.TryGetCurrent(__instance.playerId, out input))
            {
                __result = false;
                return false;
            }

            __result = input.IsPressed(button);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerInput), "GetButtonDown")]
    public static class PlayerInputButtonDownPatch
    {
        static bool Prefix(PlayerInput __instance, CupheadButton button, ref bool __result)
        {
            bool scripted;
            if (LocalDevE2ETest.TryGetLocalButton(__instance.playerId, (int)button, true, false, out scripted))
            {
                __result = scripted;
                return false;
            }

            bool delayed;
            if (HighLatencyInputSync.TryGetDelayedButton(__instance.playerId, (int)button, true, false, out delayed))
            {
                __result = delayed;
                return false;
            }

            if (!MultiplayerSession.IsActive)
                return true;
            if (!MultiplayerSession.IsNetworkControlledPlayer(__instance.playerId))
                return true;

            __result = RemoteInputDriver.WasPressedThisFrame((byte)__instance.playerId, button);
            return false;
        }
    }

    [HarmonyPatch(typeof(PlayerInput), "GetButtonUp")]
    public static class PlayerInputButtonUpPatch
    {
        static bool Prefix(PlayerInput __instance, CupheadButton button, ref bool __result)
        {
            bool scripted;
            if (LocalDevE2ETest.TryGetLocalButton(__instance.playerId, (int)button, false, true, out scripted))
            {
                __result = scripted;
                return false;
            }

            bool delayed;
            if (HighLatencyInputSync.TryGetDelayedButton(__instance.playerId, (int)button, false, true, out delayed))
            {
                __result = delayed;
                return false;
            }

            if (!MultiplayerSession.IsActive)
                return true;
            if (!MultiplayerSession.IsNetworkControlledPlayer(__instance.playerId))
                return true;

            __result = RemoteInputDriver.WasReleasedThisFrame((byte)__instance.playerId, button);
            return false;
        }
    }
}
