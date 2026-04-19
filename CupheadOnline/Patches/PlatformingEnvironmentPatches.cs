using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using CupheadOnline.Sync;
using UnityEngine;

namespace CupheadOnline.Patches
{
    static class ExtraPlatformingEnvironmentBridge
    {
        static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        static readonly BindingFlags AnyStatic =
            BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        static readonly FieldInfo ElevatorTriggerPointField =
            typeof(MountainPlatformingLevelElevatorHandler).GetField("triggerPoint", AnyInstance);
        static readonly FieldInfo ElevatorTriggerPoint2Field =
            typeof(MountainPlatformingLevelElevatorHandler).GetField("triggerPoint2", AnyInstance);
        static readonly FieldInfo ElevatorInvisibleWallField =
            typeof(MountainPlatformingLevelElevatorHandler).GetField("invisibleWall", AnyInstance);
        static readonly FieldInfo ElevatorCameraLockRoutineField =
            typeof(MountainPlatformingLevelElevatorHandler).GetField("cameraLockRoutine", AnyInstance);
        static readonly MethodInfo ElevatorMoveMethod =
            typeof(MountainPlatformingLevelElevatorHandler).GetMethod("move_cr", AnyInstance);

        static readonly FieldInfo TrampolineBoundsField =
            typeof(CircusPlatformingLevelTrampoline).GetField("bounds", AnyInstance);
        static readonly FieldInfo TrampolineAwakeningZoneField =
            typeof(CircusPlatformingLevelTrampoline).GetField("AwakeningZone", AnyInstance);
        static readonly FieldInfo TrampolineStartPosField =
            typeof(CircusPlatformingLevelTrampoline).GetField("startPos", AnyInstance);

        static readonly FieldInfo ScaleSpeedField =
            typeof(MountainPlatformingLevelScale).GetField("scaleSpeed", AnyInstance);
        static readonly FieldInfo ScaleChangeAmountField =
            typeof(MountainPlatformingLevelScale).GetField("scaleChangeAmount", AnyInstance);
        static readonly FieldInfo ScaleLeftField =
            typeof(MountainPlatformingLevelScale).GetField("ScaleLeft", AnyInstance);
        static readonly FieldInfo ScaleRightField =
            typeof(MountainPlatformingLevelScale).GetField("ScaleRight", AnyInstance);
        static readonly FieldInfo ScaleLeftStartField =
            typeof(MountainPlatformingLevelScale).GetField("scaleLeftStart", AnyInstance);
        static readonly FieldInfo ScaleRightStartField =
            typeof(MountainPlatformingLevelScale).GetField("scaleRightStart", AnyInstance);
        static readonly MethodInfo ScaleChangeStateMethod =
            typeof(MountainPlatformingLevelScale).GetMethod("ChangeState", AnyInstance);

        static readonly FieldInfo SnowDownDistField =
            typeof(SnowCultLevelPlatform).GetField("downDist", AnyInstance);
        static readonly FieldInfo SnowBounceSpeedField =
            typeof(SnowCultLevelPlatform).GetField("bounceSpeed", AnyInstance);

        static readonly FieldInfo ExitActivatedField =
            typeof(PlatformingLevelExit).GetField("_activated", AnyInstance);
        static readonly FieldInfo ExitExitedField =
            typeof(PlatformingLevelExit).GetField("_exited", AnyInstance);
        static readonly FieldInfo ExitDistanceField =
            typeof(PlatformingLevelExit).GetField("_exitDistance", AnyInstance);
        static readonly FieldInfo ExitWaitTimeField =
            typeof(PlatformingLevelExit).GetField("onCompleteWaitTime", AnyInstance);
        static readonly FieldInfo ExitOnWinStartEventField =
            typeof(PlatformingLevelExit).GetField("OnWinStartEvent", AnyStatic);
        static readonly FieldInfo ExitOnWinCompleteEventField =
            typeof(PlatformingLevelExit).GetField("OnWinCompleteEvent", AnyStatic);

        static readonly List<AbstractPlayerController> ScratchPlayers =
            new List<AbstractPlayerController>(6);
        static readonly List<AbstractPlayerController> ScratchInBoundsPlayers =
            new List<AbstractPlayerController>(6);
        static readonly List<byte> ScratchParticipants =
            new List<byte>(6);

        internal static bool ShouldBridge()
        {
            return MultiplayerSession.IsActive;
        }

        internal static void RestartElevatorCoroutines(MountainPlatformingLevelElevatorHandler instance)
        {
            if (!ShouldBridge() || instance == null)
                return;
            if (ElevatorTriggerPointField == null
             || ElevatorTriggerPoint2Field == null
             || ElevatorInvisibleWallField == null
             || ElevatorCameraLockRoutineField == null
             || ElevatorMoveMethod == null)
            {
                return;
            }

            instance.StopAllCoroutines();
            var cameraLockRoutine = instance.StartCoroutine(LockCameraCoroutine(instance));
            ElevatorCameraLockRoutineField.SetValue(instance, cameraLockRoutine);
            instance.StartCoroutine(WaitForElevatorPartyCoroutine(instance));
        }

        internal static bool HandleTrampolineSleep(CircusPlatformingLevelTrampoline instance)
        {
            if (!ShouldBridge() || instance == null)
                return false;
            if (TrampolineBoundsField == null
             || TrampolineAwakeningZoneField == null
             || TrampolineStartPosField == null)
            {
                return false;
            }

            var animator = instance.GetComponent<Animator>();
            float bounds = GetFloat(TrampolineBoundsField, instance);
            float awakeningZone = GetFloat(TrampolineAwakeningZoneField, instance);
            Vector2 startPos = GetVector2(TrampolineStartPosField, instance);

            ScratchInBoundsPlayers.Clear();
            AppendPlayersWithinBounds(startPos.x, bounds, awakeningZone, ScratchInBoundsPlayers);

            if (ScratchInBoundsPlayers.Count > 0)
            {
                if (animator != null)
                    animator.SetBool("Sleep", false);

                if (ScratchInBoundsPlayers.Count == 1)
                    instance.TrackingPlayer = ScratchInBoundsPlayers[0];
                else
                    instance.TrackingPlayer = PlayerManager.GetNext();

                return true;
            }

            if (instance.transform.position.x >= startPos.x + bounds
             || instance.transform.position.x <= startPos.x - bounds)
            {
                if (animator != null)
                    animator.SetBool("Sleep", true);
            }

            instance.TrackingPlayer = PlayerManager.GetNext();
            return true;
        }

        internal static void RestartScaleCoroutine(MountainPlatformingLevelScale instance)
        {
            if (!ShouldBridge() || instance == null)
                return;
            if (ScaleSpeedField == null
             || ScaleChangeAmountField == null
             || ScaleLeftField == null
             || ScaleRightField == null
             || ScaleLeftStartField == null
             || ScaleRightStartField == null
             || ScaleChangeStateMethod == null)
            {
                return;
            }

            instance.StopAllCoroutines();
            instance.StartCoroutine(ScaleBalanceCoroutine(instance));
        }

        internal static void ApplySnowPlatformBounce(SnowCultLevelPlatform instance)
        {
            if (!ShouldBridge() || instance == null || ExtraParticipantTracker.LiveCount <= 0)
                return;
            if (SnowDownDistField == null || SnowBounceSpeedField == null)
                return;

            var collider = instance.GetComponent<Collider2D>();
            if (collider == null || !AnyAliveExtraStandingOn(collider, 90f))
                return;

            float downDist = GetFloat(SnowDownDistField, instance);
            float bounceSpeed = GetFloat(SnowBounceSpeedField, instance);
            instance.transform.localPosition = Vector3.Lerp(
                instance.transform.localPosition,
                new Vector3(0f, downDist),
                bounceSpeed * CupheadTime.FixedDelta);
        }

        internal static bool HandlePlatformingExit(PlatformingLevelExit instance)
        {
            if (!ShouldBridge()
             || instance == null
             || !MultiplayerSession.IsHost
             || ExtraParticipantTracker.LiveCount <= 0)
            {
                return false;
            }
            if (ExitActivatedField == null
             || ExitExitedField == null
             || ExitDistanceField == null
             || ExitWaitTimeField == null)
            {
                return false;
            }

            bool activated = GetBool(ExitActivatedField, instance);
            bool exited = GetBool(ExitExitedField, instance);
            float exitDistance = GetFloat(ExitDistanceField, instance);
            float triggerX = instance.transform.position.x;

            ScratchPlayers.Clear();
            AppendAlivePlayers(ScratchPlayers);

            if (activated)
            {
                if (!exited && AnyPlayerPastX(ScratchPlayers, triggerX + exitDistance))
                {
                    ExitExitedField.SetValue(instance, true);
                    InvokeAndClear(ExitOnWinCompleteEventField);
                }

                return true;
            }

            if (!AnyPlayerPastX(ScratchPlayers, triggerX))
                return true;

            ExitActivatedField.SetValue(instance, true);
            InvokeAndClear(ExitOnWinStartEventField);
            PlatformingLevelEnd.Win();
            instance.StartCoroutine(PlatformingExitCompleteCoroutine(instance));
            return true;
        }

        static IEnumerator LockCameraCoroutine(MountainPlatformingLevelElevatorHandler instance)
        {
            yield return CupheadTime.WaitForSeconds(instance, 1f);

            while (true)
            {
                var triggerPoint = ElevatorTriggerPointField.GetValue(instance) as Transform;
                if (triggerPoint == null || CupheadLevelCamera.Current == null)
                {
                    yield return null;
                    continue;
                }

                float triggerX = triggerPoint.position.x;
                CupheadLevelCamera.Current.LockCamera(
                    CupheadLevelCamera.Current.transform.position.x > triggerX);

                if (AllAlivePlayersLeftOf(triggerX))
                    CupheadLevelCamera.Current.LockCamera(false);

                yield return null;
            }
        }

        static IEnumerator WaitForElevatorPartyCoroutine(MountainPlatformingLevelElevatorHandler instance)
        {
            yield return CupheadTime.WaitForSeconds(instance, 1f);

            while (!ShouldStartElevator(instance))
                yield return null;

            var cameraLockRoutine = ElevatorCameraLockRoutineField.GetValue(instance) as Coroutine;
            if (cameraLockRoutine != null)
                instance.StopCoroutine(cameraLockRoutine);

            if (CupheadLevelCamera.Current != null)
                CupheadLevelCamera.Current.LockCamera(true);

            var invisibleWall = ElevatorInvisibleWallField.GetValue(instance) as GameObject;
            if (invisibleWall != null)
                invisibleWall.SetActive(true);

            AudioManager.Play("castle_lift_start");
            if (CupheadLevelCamera.Current != null)
                CupheadLevelCamera.Current.Shake(10f, 1f, false);

            yield return CupheadTime.WaitForSeconds(instance, 0.9f);

            var moveCoroutine = ElevatorMoveMethod.Invoke(instance, null) as IEnumerator;
            if (moveCoroutine != null)
                instance.StartCoroutine(moveCoroutine);
        }

        static IEnumerator ScaleBalanceCoroutine(MountainPlatformingLevelScale instance)
        {
            var wait = new WaitForFixedUpdate();

            while (instance != null)
            {
                var scaleLeft = ScaleLeftField.GetValue(instance) as MountainPlatformingLevelScalePart;
                var scaleRight = ScaleRightField.GetValue(instance) as MountainPlatformingLevelScalePart;
                float scaleSpeed = GetFloat(ScaleSpeedField, instance);
                float scaleChangeAmount = GetFloat(ScaleChangeAmountField, instance);
                Vector2 scaleLeftStart = GetVector2(ScaleLeftStartField, instance);
                Vector2 scaleRightStart = GetVector2(ScaleRightStartField, instance);
                bool scaleLeftStepped = IsScalePartStepped(scaleLeft);
                bool scaleRightStepped = IsScalePartStepped(scaleRight);
                bool partyHasMultiplePlayers = CountAlivePlayers() > 1;

                if (scaleRight != null)
                {
                    if (scaleRightStepped)
                    {
                        if (!scaleLeftStepped)
                        {
                            if (scaleRight.transform.position.y > scaleRightStart.y - scaleChangeAmount)
                            {
                                scaleRight.transform.AddPosition(0f, -scaleSpeed * CupheadTime.FixedDelta, 0f);
                                ChangeScaleState(instance, 0);
                            }

                            if (scaleLeft != null
                             && scaleLeft.transform.position.y < scaleLeftStart.y + scaleChangeAmount)
                            {
                                scaleLeft.transform.AddPosition(0f, scaleSpeed * CupheadTime.FixedDelta, 0f);
                            }
                        }
                        else if (partyHasMultiplePlayers)
                        {
                            if (scaleRight.transform.position.y < scaleRightStart.y)
                            {
                                scaleRight.transform.AddPosition(0f, scaleSpeed * CupheadTime.FixedDelta, 0f);
                                ChangeScaleState(instance, 1);
                            }
                            else if (scaleLeftStepped)
                            {
                                ChangeScaleState(instance, 2);
                            }

                            if (scaleLeft != null && scaleLeft.transform.position.y > scaleLeftStart.y)
                                scaleLeft.transform.AddPosition(0f, -scaleSpeed * CupheadTime.FixedDelta, 0f);
                        }
                    }
                    else
                    {
                        if (scaleRight.transform.position.y < scaleRightStart.y)
                        {
                            scaleRight.transform.AddPosition(0f, scaleSpeed * CupheadTime.FixedDelta, 0f);
                            ChangeScaleState(instance, 1);
                        }
                        else if (!scaleLeftStepped)
                        {
                            ChangeScaleState(instance, 2);
                        }

                        if (scaleLeft != null && scaleLeft.transform.position.y > scaleLeftStart.y)
                            scaleLeft.transform.AddPosition(0f, -scaleSpeed * CupheadTime.FixedDelta, 0f);
                    }
                }

                if (scaleLeft != null)
                {
                    if (scaleLeftStepped)
                    {
                        if (!scaleRightStepped)
                        {
                            if (scaleLeft.transform.position.y > scaleLeftStart.y - scaleChangeAmount)
                            {
                                scaleLeft.transform.AddPosition(0f, -scaleSpeed * CupheadTime.FixedDelta, 0f);
                                ChangeScaleState(instance, 1);
                            }

                            if (scaleRight != null
                             && scaleRight.transform.position.y < scaleRightStart.y + scaleChangeAmount)
                            {
                                scaleRight.transform.AddPosition(0f, scaleSpeed * CupheadTime.FixedDelta, 0f);
                            }
                        }
                        else if (partyHasMultiplePlayers)
                        {
                            if (scaleLeft.transform.position.y < scaleLeftStart.y)
                            {
                                scaleLeft.transform.AddPosition(0f, scaleSpeed * CupheadTime.FixedDelta, 0f);
                                ChangeScaleState(instance, 0);
                            }
                            else if (scaleRightStepped)
                            {
                                ChangeScaleState(instance, 2);
                            }

                            if (scaleRight != null && scaleRight.transform.position.y > scaleRightStart.y)
                                scaleRight.transform.AddPosition(0f, -scaleSpeed * CupheadTime.FixedDelta, 0f);
                        }
                    }
                    else
                    {
                        if (scaleLeft.transform.position.y < scaleLeftStart.y)
                        {
                            scaleLeft.transform.AddPosition(0f, scaleSpeed * CupheadTime.FixedDelta, 0f);
                            ChangeScaleState(instance, 0);
                        }
                        else if (!scaleRightStepped)
                        {
                            ChangeScaleState(instance, 2);
                        }

                        if (scaleRight != null && scaleRight.transform.position.y > scaleRightStart.y)
                            scaleRight.transform.AddPosition(0f, -scaleSpeed * CupheadTime.FixedDelta, 0f);
                    }
                }

                yield return wait;
            }
        }

        static IEnumerator PlatformingExitCompleteCoroutine(PlatformingLevelExit instance)
        {
            float waitTime = GetFloat(ExitWaitTimeField, instance);
            yield return CupheadTime.WaitForSeconds(instance, waitTime);
            InvokeAndClear(ExitOnWinCompleteEventField);
        }

        static bool ShouldStartElevator(MountainPlatformingLevelElevatorHandler instance)
        {
            var triggerPoint = ElevatorTriggerPointField.GetValue(instance) as Transform;
            var triggerPoint2 = ElevatorTriggerPoint2Field.GetValue(instance) as Transform;
            if (triggerPoint == null || triggerPoint2 == null)
                return false;

            ScratchPlayers.Clear();
            AppendAlivePlayers(ScratchPlayers);
            if (ScratchPlayers.Count <= 0)
                return false;

            if (ScratchPlayers.Count == 1)
                return ScratchPlayers[0].transform.position.x > triggerPoint.position.x;

            float minTrigger = Mathf.Min(triggerPoint.position.x, triggerPoint2.position.x);
            float maxTrigger = Mathf.Max(triggerPoint.position.x, triggerPoint2.position.x);
            float minX = float.MaxValue;
            float maxX = float.MinValue;

            for (int i = 0; i < ScratchPlayers.Count; i++)
            {
                var player = ScratchPlayers[i];
                if (player == null)
                    continue;

                float x = player.transform.position.x;
                if (x < minX)
                    minX = x;
                if (x > maxX)
                    maxX = x;
            }

            return minX > minTrigger && maxX > maxTrigger;
        }

        static bool AllAlivePlayersLeftOf(float thresholdX)
        {
            ScratchPlayers.Clear();
            AppendAlivePlayers(ScratchPlayers);
            if (ScratchPlayers.Count <= 0)
                return false;

            for (int i = 0; i < ScratchPlayers.Count; i++)
            {
                var player = ScratchPlayers[i];
                if (player == null)
                    continue;

                if (player.transform.position.x >= thresholdX)
                    return false;
            }

            return true;
        }

        static void AppendPlayersWithinBounds(
            float startX,
            float bounds,
            float awakeningZone,
            List<AbstractPlayerController> target)
        {
            if (target == null)
                return;

            AppendAlivePlayers(target);
            if (target.Count <= 0)
                return;

            float minX = startX - bounds - awakeningZone;
            float maxX = startX + bounds + awakeningZone;

            for (int i = target.Count - 1; i >= 0; i--)
            {
                var player = target[i];
                if (player == null
                 || player.transform == null
                 || player.transform.position.x <= minX
                 || player.transform.position.x >= maxX)
                {
                    target.RemoveAt(i);
                }
            }
        }

        static void AppendAlivePlayers(List<AbstractPlayerController> target)
        {
            if (target == null)
                return;

            AppendBuiltIn(target, PlayerId.PlayerOne);
            AppendBuiltIn(target, PlayerId.PlayerTwo);
            ExtraRemoteAvatarManager.AppendTargetableControllers(target);
        }

        static void AppendBuiltIn(List<AbstractPlayerController> target, PlayerId playerId)
        {
            var player = PlayerManager.GetPlayer(playerId);
            if (player == null || player.IsDead || !player.gameObject.activeInHierarchy)
                return;

            target.Add(player);
        }

        static int CountAlivePlayers()
        {
            ScratchPlayers.Clear();
            AppendAlivePlayers(ScratchPlayers);
            return ScratchPlayers.Count;
        }

        static bool IsScalePartStepped(MountainPlatformingLevelScalePart part)
        {
            if (part == null)
                return false;
            if (part.steppedOn)
                return true;

            return AnyAliveExtraStandingOn(part.GetComponent<Collider2D>(), 90f);
        }

        static bool AnyAliveExtraStandingOn(Collider2D collider, float verticalTolerance)
        {
            if (collider == null || ExtraParticipantTracker.LiveCount <= 0)
                return false;

            var platformBounds = collider.bounds;
            ScratchParticipants.Clear();
            ExtraRemoteAvatarManager.AppendParticipants(ScratchParticipants);

            for (int i = 0; i < ScratchParticipants.Count; i++)
            {
                Bounds hitbox;
                if (!ExtraRemoteAvatarManager.TryGetHitbox(ScratchParticipants[i], out hitbox))
                    continue;
                if (hitbox.max.x < platformBounds.min.x || hitbox.min.x > platformBounds.max.x)
                    continue;
                if (hitbox.center.y < platformBounds.center.y)
                    continue;

                float minY = hitbox.min.y;
                float platformTop = platformBounds.max.y;
                if (minY >= platformTop - verticalTolerance && minY <= platformTop + verticalTolerance)
                    return true;
            }

            return false;
        }

        static bool AnyPlayerPastX(List<AbstractPlayerController> players, float thresholdX)
        {
            if (players == null)
                return false;

            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];
                if (player == null || player.IsDead)
                    continue;
                if (player.center.x > thresholdX)
                    return true;
            }

            return false;
        }

        static void ChangeScaleState(MountainPlatformingLevelScale instance, int stateValue)
        {
            if (instance == null || ScaleChangeStateMethod == null)
                return;

            ScaleChangeStateMethod.Invoke(
                instance,
                new object[] { Enum.ToObject(typeof(MountainPlatformingLevelScale.ScaleState), stateValue) });
        }

        static float GetFloat(FieldInfo field, object target)
        {
            if (field == null || target == null)
                return 0f;

            object value = field.GetValue(target);
            return value is float ? (float)value : 0f;
        }

        static Vector2 GetVector2(FieldInfo field, object target)
        {
            if (field == null || target == null)
                return Vector2.zero;

            object value = field.GetValue(target);
            return value is Vector2 ? (Vector2)value : Vector2.zero;
        }

        static bool GetBool(FieldInfo field, object target)
        {
            if (field == null || target == null)
                return false;

            object value = field.GetValue(target);
            return value is bool && (bool)value;
        }

        static void InvokeAndClear(FieldInfo field)
        {
            if (field == null)
                return;

            var action = field.GetValue(null) as Action;
            field.SetValue(null, null);
            if (action != null)
                action();
        }
    }

    [HarmonyPatch(typeof(MountainPlatformingLevelElevatorHandler), "Start")]
    public static class MountainPlatformingLevelElevatorHandlerStartPatch
    {
        static void Postfix(MountainPlatformingLevelElevatorHandler __instance)
        {
            ExtraPlatformingEnvironmentBridge.RestartElevatorCoroutines(__instance);
        }
    }

    [HarmonyPatch(typeof(CircusPlatformingLevelTrampoline), "CheckIfShouldSleep")]
    public static class CircusPlatformingLevelTrampolineSleepPatch
    {
        static bool Prefix(CircusPlatformingLevelTrampoline __instance)
        {
            return !ExtraPlatformingEnvironmentBridge.HandleTrampolineSleep(__instance);
        }
    }

    [HarmonyPatch(typeof(MountainPlatformingLevelScale), "Start")]
    public static class MountainPlatformingLevelScaleStartPatch
    {
        static void Postfix(MountainPlatformingLevelScale __instance)
        {
            ExtraPlatformingEnvironmentBridge.RestartScaleCoroutine(__instance);
        }
    }

    [HarmonyPatch(typeof(SnowCultLevelPlatform), "FixedUpdate")]
    public static class SnowCultLevelPlatformExtraBouncePatch
    {
        static void Postfix(SnowCultLevelPlatform __instance)
        {
            ExtraPlatformingEnvironmentBridge.ApplySnowPlatformBounce(__instance);
        }
    }

    [HarmonyPatch(typeof(PlatformingLevelExit), "FixedUpdate")]
    public static class PlatformingLevelExitExtraParticipantPatch
    {
        static bool Prefix(PlatformingLevelExit __instance)
        {
            return !ExtraPlatformingEnvironmentBridge.HandlePlatformingExit(__instance);
        }
    }
}
