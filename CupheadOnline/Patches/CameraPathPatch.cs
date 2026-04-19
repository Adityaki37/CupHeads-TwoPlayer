using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using CupheadOnline.Sync;
using UnityEngine;

namespace CupheadOnline.Patches
{
    [HarmonyPatch(typeof(CupheadLevelCamera), "UpdatePath")]
    public static class CupheadLevelCameraPathPatch
    {
        static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        static readonly FieldInfo StabilizeYField =
            typeof(CupheadLevelCamera).GetField("stabilizeY", AnyInstance);
        static readonly FieldInfo StabilizePaddingTopField =
            typeof(CupheadLevelCamera).GetField("stabilizePaddingTop", AnyInstance);
        static readonly FieldInfo StabilizePaddingBottomField =
            typeof(CupheadLevelCamera).GetField("stabilizePaddingBottom", AnyInstance);
        static readonly FieldInfo MoveXField =
            typeof(CupheadLevelCamera).GetField("moveX", AnyInstance);
        static readonly FieldInfo MoveYField =
            typeof(CupheadLevelCamera).GetField("moveY", AnyInstance);
        static readonly FieldInfo LeftOffsetField =
            typeof(CupheadLevelCamera).GetField("leftOffset", AnyInstance);
        static readonly FieldInfo PathMovesOnlyForwardField =
            typeof(CupheadLevelCamera).GetField("pathMovesOnlyForward", AnyInstance);
        static readonly FieldInfo PathField =
            typeof(CupheadLevelCamera).GetField("path", AnyInstance);
        static readonly FieldInfo TargetPosField =
            typeof(CupheadLevelCamera).GetField("targetPos", AnyInstance);
        static readonly FieldInfo MinPathValueField =
            typeof(CupheadLevelCamera).GetField("_minPathValue", AnyInstance);
        static readonly FieldInfo SpeedLastFrameField =
            typeof(CupheadLevelCamera).GetField("_speedLastFrame", AnyInstance);
        static readonly FieldInfo PositionField =
            typeof(AbstractCupheadGameCamera).GetField("_position", AnyInstance);

        static readonly List<Vector2> ScratchCenters =
            new List<Vector2>(6);

        static bool Prefix(CupheadLevelCamera __instance)
        {
            if (!ShouldOverride(__instance))
                return true;

            var path = PathField.GetValue(__instance) as VectorPath;
            if (path == null)
                return true;

            Vector3 currentPosition = GetPosition(__instance);
            bool moveX = GetBool(MoveXField, __instance);
            bool moveY = GetBool(MoveYField, __instance);
            bool pathMovesOnlyForward = GetBool(PathMovesOnlyForwardField, __instance);

            Vector2 target = BuildStabilizedTarget(__instance, currentPosition.y);
            Vector3 targetPos;
            if (__instance.cameraOffset)
            {
                float offsetX = GetBool(LeftOffsetField, __instance) ? 500f : -500f;
                targetPos = new Vector3(target.x + offsetX, target.y, currentPosition.z);
            }
            else
            {
                targetPos = new Vector3(target.x, target.y, currentPosition.z);
            }

            TargetPosField.SetValue(__instance, targetPos);

            float delta = Mathf.Max(CupheadTime.Delta, 0.0001f);
            Vector3 closestPoint = path.GetClosestPoint(currentPosition, targetPos, moveX, moveY);
            float requestedSpeed = (closestPoint - currentPosition).magnitude / delta;
            float lastSpeed = GetFloat(SpeedLastFrameField, __instance);
            float maxSpeed = Mathf.Max(lastSpeed + 5000f * delta, 1000f);
            if (requestedSpeed > maxSpeed)
            {
                closestPoint = currentPosition
                    + (closestPoint - currentPosition).normalized * maxSpeed * delta;
            }

            SpeedLastFrameField.SetValue(__instance, Mathf.Min(requestedSpeed, maxSpeed));

            if (pathMovesOnlyForward)
            {
                float minPathValue = GetFloat(MinPathValueField, __instance);
                float closestNormalizedPoint =
                    path.GetClosestNormalizedPoint(currentPosition, closestPoint, moveX, moveY);
                if (closestNormalizedPoint < minPathValue)
                    return false;
            }

            currentPosition.x = closestPoint.x;
            currentPosition.y = closestPoint.y;

            Vector3 updatedPosition = GetPosition(__instance);
            if (!__instance.cameraLocked)
            {
                if (!__instance.autoScrolling)
                {
                    updatedPosition = Vector3.Lerp(updatedPosition, currentPosition, delta * 15f);
                }
                else
                {
                    Vector3 scrollTarget = new Vector3(
                        __instance.transform.position.x + 500f,
                        __instance.transform.position.y,
                        updatedPosition.z);
                    Vector3 nextPoint = path.GetClosestPoint(updatedPosition, scrollTarget, moveX, moveY);
                    float autoSpeed = 200f * __instance.autoScrollSpeedMultiplier;
                    updatedPosition = Vector3.MoveTowards(updatedPosition, nextPoint, delta * autoSpeed);
                }

                SetPosition(__instance, updatedPosition);
            }

            if (pathMovesOnlyForward)
            {
                Vector3 finalPosition = GetPosition(__instance);
                float nextMinPathValue = path.GetClosestNormalizedPoint(
                    finalPosition,
                    finalPosition,
                    moveX,
                    moveY);
                MinPathValueField.SetValue(__instance, nextMinPathValue);
            }

            return false;
        }

        static bool ShouldOverride(CupheadLevelCamera instance)
        {
            if (instance == null
             || !MultiplayerSession.IsActive
             || ExtraParticipantTracker.LiveCount <= 0)
            {
                return false;
            }

            return GetBool(StabilizeYField, instance)
                && MoveXField != null
                && MoveYField != null
                && LeftOffsetField != null
                && PathMovesOnlyForwardField != null
                && PathField != null
                && TargetPosField != null
                && MinPathValueField != null
                && SpeedLastFrameField != null
                && PositionField != null;
        }

        static Vector2 BuildStabilizedTarget(CupheadLevelCamera instance, float currentY)
        {
            ScratchCenters.Clear();
            AppendBuiltIn(PlayerId.PlayerOne);
            AppendBuiltIn(PlayerId.PlayerTwo);
            ExtraParticipantTracker.AppendCameraCenters(ScratchCenters);

            if (ScratchCenters.Count <= 0)
                return PlayerManager.Center;

            float paddingTop = GetFloat(StabilizePaddingTopField, instance);
            float paddingBottom = GetFloat(StabilizePaddingBottomField, instance);
            float sumX = 0f;
            float sumY = 0f;

            for (int i = 0; i < ScratchCenters.Count; i++)
            {
                Vector2 center = ScratchCenters[i];
                sumX += center.x;
                sumY += StabilizeY(center.y, currentY, paddingTop, paddingBottom);
            }

            return new Vector2(sumX / ScratchCenters.Count, sumY / ScratchCenters.Count);
        }

        static void AppendBuiltIn(PlayerId playerId)
        {
            var player = PlayerManager.GetPlayer(playerId);
            if (player == null || player.IsDead || !player.gameObject.activeInHierarchy)
                return;

            ScratchCenters.Add(player.center);
        }

        static float StabilizeY(float sourceY, float currentY, float paddingTop, float paddingBottom)
        {
            if (sourceY > currentY + paddingTop)
                return sourceY - paddingTop;

            if (sourceY < currentY - paddingBottom)
                return sourceY + paddingBottom;

            return currentY;
        }

        static bool GetBool(FieldInfo field, object target)
        {
            if (field == null || target == null)
                return false;

            object value = field.GetValue(target);
            return value is bool && (bool)value;
        }

        static float GetFloat(FieldInfo field, object target)
        {
            if (field == null || target == null)
                return 0f;

            object value = field.GetValue(target);
            return value is float ? (float)value : 0f;
        }

        static Vector3 GetPosition(CupheadLevelCamera instance)
        {
            if (PositionField == null || instance == null)
                return Vector3.zero;

            object value = PositionField.GetValue(instance);
            return value is Vector3 ? (Vector3)value : Vector3.zero;
        }

        static void SetPosition(CupheadLevelCamera instance, Vector3 value)
        {
            if (PositionField != null && instance != null)
                PositionField.SetValue(instance, value);
        }
    }
}
