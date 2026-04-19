using System.Reflection;
using System.Collections.Generic;
using HarmonyLib;
using CupheadOnline.Sync;
using UnityEngine;

namespace CupheadOnline.Patches
{
    static class ExtraPlatformingTriggerBridge
    {
        static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        static readonly FieldInfo EnemySpawnerStartRectField =
            typeof(PlatformingLevelEnemySpawner).GetField("startRect", AnyInstance);
        static readonly FieldInfo EnemySpawnerStopRectField =
            typeof(PlatformingLevelEnemySpawner).GetField("stopRect", AnyInstance);
        static readonly MethodInfo EnemySpawnerOnStartTriggerHitMethod =
            typeof(PlatformingLevelEnemySpawner).GetMethod("OnStartTriggerHit", AnyInstance);
        static readonly MethodInfo EnemySpawnerOnStopTriggerHitMethod =
            typeof(PlatformingLevelEnemySpawner).GetMethod("OnStopTriggerHit", AnyInstance);

        static readonly FieldInfo PitMoveTriggerRectField =
            typeof(PlatformingLevelPitMoveTrigger).GetField("rect", AnyInstance);
        static readonly MethodInfo PitMoveTriggerHitMethod =
            typeof(PlatformingLevelPitMoveTrigger).GetMethod("OnTriggerHit", AnyInstance);

        static readonly FieldInfo ChomperSpawnerStartRectField =
            typeof(ForestPlatformingLevelChomperSpawner).GetField("startRect", AnyInstance);
        static readonly MethodInfo ChomperSpawnerOnStartTriggerHitMethod =
            typeof(ForestPlatformingLevelChomperSpawner).GetMethod("OnStartTriggerHit", AnyInstance);

        static readonly FieldInfo PlatformingEnemyStartedField =
            typeof(AbstractPlatformingLevelEnemy).GetField("_started", AnyInstance);
        static readonly FieldInfo PlatformingEnemyStartConditionField =
            typeof(AbstractPlatformingLevelEnemy).GetField("_startCondition", AnyInstance);
        static readonly FieldInfo PlatformingEnemyTriggerPositionField =
            typeof(AbstractPlatformingLevelEnemy).GetField("_triggerPosition", AnyInstance);
        static readonly FieldInfo PlatformingEnemyTriggerSizeField =
            typeof(AbstractPlatformingLevelEnemy).GetField("_triggerSize", AnyInstance);
        static readonly MethodInfo PlatformingEnemyStartWithConditionMethod =
            typeof(AbstractPlatformingLevelEnemy).GetMethod("StartWithCondition", AnyInstance);
        static readonly FieldInfo ShootingEnemyTriggerTypeField =
            typeof(PlatformingLevelShootingEnemy).GetField("_triggerType", AnyInstance);
        static readonly FieldInfo ShootingEnemyTriggerVolumesField =
            typeof(PlatformingLevelShootingEnemy).GetField("_triggerVolumes", AnyInstance);
        static readonly FieldInfo ShootingEnemyTargetField =
            typeof(PlatformingLevelShootingEnemy).GetField("_target", AnyInstance);
        static readonly List<byte> PitParticipants =
            new List<byte>(8);
        static readonly Dictionary<byte, float> NextPitDamageAt =
            new Dictionary<byte, float>(8);
        static readonly List<AbstractPlayerController> ExtraTargets =
            new List<AbstractPlayerController>(8);

        const float PitDamageCooldown = 2f;

        static ExtraPlatformingTriggerBridge()
        {
            MultiplayerSession.OnSessionEnded += ResetPitCooldowns;
        }

        internal static bool ShouldBridge()
        {
            return MultiplayerSession.IsActive
                && MultiplayerSession.IsHost
                && ExtraParticipantTracker.LiveCount > 0;
        }

        internal static void HandleEnemySpawner(PlatformingLevelEnemySpawner instance)
        {
            if (!ShouldBridge() || instance == null)
                return;

            if (TryGetRect(EnemySpawnerStartRectField, instance, out var startRect)
             && ExtraParticipantTracker.AnyInRect(startRect))
            {
                EnemySpawnerOnStartTriggerHitMethod?.Invoke(instance, null);
            }

            if (TryGetRect(EnemySpawnerStopRectField, instance, out var stopRect)
             && ExtraParticipantTracker.AnyInRect(stopRect))
            {
                EnemySpawnerOnStopTriggerHitMethod?.Invoke(instance, null);
            }
        }

        internal static void HandlePitMoveTrigger(PlatformingLevelPitMoveTrigger instance)
        {
            if (!ShouldBridge() || instance == null)
                return;

            if (TryGetRect(PitMoveTriggerRectField, instance, out var rect)
             && ExtraParticipantTracker.AnyInRect(rect))
            {
                PitMoveTriggerHitMethod?.Invoke(instance, null);
            }
        }

        internal static void HandleChomperSpawner(ForestPlatformingLevelChomperSpawner instance)
        {
            if (!ShouldBridge() || instance == null)
                return;

            if (TryGetRect(ChomperSpawnerStartRectField, instance, out var startRect)
             && ExtraParticipantTracker.AnyInRect(startRect))
            {
                ChomperSpawnerOnStartTriggerHitMethod?.Invoke(instance, null);
            }
        }

        internal static void HandlePlatformingEnemy(AbstractPlatformingLevelEnemy instance)
        {
            if (!ShouldBridge() || instance == null)
                return;

            if (PlatformingEnemyStartedField == null
             || PlatformingEnemyStartConditionField == null
             || PlatformingEnemyTriggerPositionField == null
             || PlatformingEnemyTriggerSizeField == null
             || PlatformingEnemyStartWithConditionMethod == null)
            {
                return;
            }

            object rawStarted = PlatformingEnemyStartedField.GetValue(instance);
            object rawStartCondition = PlatformingEnemyStartConditionField.GetValue(instance);
            object rawTriggerPosition = PlatformingEnemyTriggerPositionField.GetValue(instance);
            object rawTriggerSize = PlatformingEnemyTriggerSizeField.GetValue(instance);

            bool started = rawStarted is bool && (bool)rawStarted;
            if (started)
                return;

            if (!(rawStartCondition is AbstractPlatformingLevelEnemy.StartCondition)
             || (AbstractPlatformingLevelEnemy.StartCondition)rawStartCondition
                != AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume)
            {
                return;
            }

            if (!(rawTriggerPosition is Vector2) || !(rawTriggerSize is Vector2))
                return;

            Vector2 triggerPosition = (Vector2)rawTriggerPosition;
            Vector2 triggerSize = (Vector2)rawTriggerSize;
            Rect rect = RectUtils.NewFromCenter(
                triggerPosition.x,
                triggerPosition.y,
                triggerSize.x,
                triggerSize.y);

            if (ExtraParticipantTracker.AnyInRect(rect))
            {
                PlatformingEnemyStartWithConditionMethod.Invoke(
                    instance,
                    new object[] { AbstractPlatformingLevelEnemy.StartCondition.TriggerVolume });
            }
        }

        internal static void HandleLevelPit(LevelPit instance)
        {
            if (!ShouldBridge() || instance == null || Plugin.Net == null || !Plugin.Net.IsConnected)
                return;

            PitParticipants.Clear();
            ExtraRemoteAvatarManager.AppendParticipants(PitParticipants);
            if (PitParticipants.Count == 0)
                return;

            float thresholdY = instance.transform.position.y + instance.ExtraOffset;
            float now = Time.unscaledTime;

            for (int i = 0; i < PitParticipants.Count; i++)
            {
                byte participantId = PitParticipants[i];
                Bounds hitbox;
                if (!ExtraRemoteAvatarManager.TryGetHitbox(participantId, out hitbox))
                    continue;

                if (hitbox.min.y > thresholdY)
                    continue;

                float nextAllowedAt;
                if (NextPitDamageAt.TryGetValue(participantId, out nextAllowedAt) && now < nextAllowedAt)
                    continue;

                if (Plugin.Net.SendDamageEventForParticipant(
                    participantId,
                    1f,
                    (byte)DamageDealer.DamageSource.Pit,
                    MultiplayerSession.Tick))
                {
                    NextPitDamageAt[participantId] = now + PitDamageCooldown;
                }
            }
        }

        internal static void ResetPitCooldowns()
        {
            NextPitDamageAt.Clear();
        }

        internal static bool HasExtraTargetInRange(PlatformingLevelShootingEnemy instance)
        {
            AbstractPlayerController target;
            return TrySelectExtraRangeTarget(instance, out target);
        }

        internal static bool HasExtraTargetInVolumes(PlatformingLevelShootingEnemy instance)
        {
            AbstractPlayerController target;
            return TrySelectExtraVolumeTarget(instance, out target);
        }

        internal static void OverrideShootingEnemyTarget(PlatformingLevelShootingEnemy instance)
        {
            if (!ShouldBridge() || instance == null || ShootingEnemyTargetField == null)
                return;

            AbstractPlayerController target = null;
            object rawTriggerType = ShootingEnemyTriggerTypeField != null
                ? ShootingEnemyTriggerTypeField.GetValue(instance)
                : null;

            if (rawTriggerType is PlatformingLevelShootingEnemy.TriggerType)
            {
                var triggerType = (PlatformingLevelShootingEnemy.TriggerType)rawTriggerType;
                switch (triggerType)
                {
                    case PlatformingLevelShootingEnemy.TriggerType.Range:
                        TrySelectExtraRangeTarget(instance, out target);
                        break;

                    case PlatformingLevelShootingEnemy.TriggerType.TriggerVolumes:
                        TrySelectExtraVolumeTarget(instance, out target);
                        break;
                }
            }

            if (target != null)
                ShootingEnemyTargetField.SetValue(instance, target);
        }

        static bool TryGetRect(FieldInfo field, object target, out Rect rect)
        {
            rect = new Rect();
            if (field == null || target == null)
                return false;

            object value = field.GetValue(target);
            if (!(value is Rect))
                return false;

            rect = (Rect)value;
            return true;
        }

        static bool TrySelectExtraRangeTarget(
            PlatformingLevelShootingEnemy instance,
            out AbstractPlayerController target)
        {
            target = null;
            if (!ShouldBridge() || instance == null)
                return false;

            ExtraTargets.Clear();
            ExtraRemoteAvatarManager.AppendTargetableControllers(ExtraTargets);
            if (ExtraTargets.Count == 0)
                return false;

            float bestDistance = float.MaxValue;
            for (int i = 0; i < ExtraTargets.Count; i++)
            {
                var candidate = ExtraTargets[i];
                if (candidate == null || candidate.IsDead || !candidate.gameObject.activeInHierarchy)
                    continue;

                float distance = Vector2.Distance(instance.transform.position, candidate.center);
                if (distance > instance.triggerRange || distance >= bestDistance)
                    continue;

                bestDistance = distance;
                target = candidate;
            }

            return target != null;
        }

        static bool TrySelectExtraVolumeTarget(
            PlatformingLevelShootingEnemy instance,
            out AbstractPlayerController target)
        {
            target = null;
            if (!ShouldBridge() || instance == null || ShootingEnemyTriggerVolumesField == null)
                return false;

            var triggerVolumes = ShootingEnemyTriggerVolumesField.GetValue(instance)
                as List<PlatformingLevelShootingEnemy.TriggerVolumeProperties>;
            if (triggerVolumes == null || triggerVolumes.Count == 0)
                return false;

            ExtraTargets.Clear();
            ExtraRemoteAvatarManager.AppendTargetableControllers(ExtraTargets);
            if (ExtraTargets.Count == 0)
                return false;

            float bestDistance = float.MaxValue;
            for (int i = 0; i < ExtraTargets.Count; i++)
            {
                var candidate = ExtraTargets[i];
                if (candidate == null || candidate.IsDead || !candidate.gameObject.activeInHierarchy)
                    continue;
                if (!IsControllerInsideVolumes(instance, triggerVolumes, candidate))
                    continue;

                float distance = Vector2.Distance(instance.transform.position, candidate.center);
                if (distance >= bestDistance)
                    continue;

                bestDistance = distance;
                target = candidate;
            }

            return target != null;
        }

        static bool IsControllerInsideVolumes(
            PlatformingLevelShootingEnemy instance,
            List<PlatformingLevelShootingEnemy.TriggerVolumeProperties> triggerVolumes,
            AbstractPlayerController candidate)
        {
            if (instance == null || triggerVolumes == null || candidate == null)
                return false;

            Vector2 center = candidate.center;
            for (int i = 0; i < triggerVolumes.Count; i++)
            {
                var triggerVolume = triggerVolumes[i];
                if (triggerVolume == null)
                    continue;

                if (triggerVolume.shape == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.CircleCollider)
                {
                    Vector2 position = triggerVolume.position;
                    if (triggerVolume.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
                    {
                        position.x += instance.transform.position.x;
                        position.y += instance.transform.position.y;
                    }

                    if (MathUtils.CircleContains(position, triggerVolume.circleRadius, center))
                        return true;
                }
                else if (triggerVolume.shape == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.BoxCollider)
                {
                    Rect rect = RectUtils.NewFromCenter(
                        triggerVolume.position.x,
                        triggerVolume.position.y,
                        triggerVolume.boxSize.x,
                        triggerVolume.boxSize.y);
                    if (triggerVolume.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
                    {
                        rect.x += instance.transform.position.x;
                        rect.y += instance.transform.position.y;
                    }

                    if (rect.Contains(center))
                        return true;
                }
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(PlatformingLevelEnemySpawner), "Update")]
    public static class PlatformingLevelEnemySpawnerPatch
    {
        static void Postfix(PlatformingLevelEnemySpawner __instance)
        {
            ExtraPlatformingTriggerBridge.HandleEnemySpawner(__instance);
        }
    }

    [HarmonyPatch(typeof(PlatformingLevelPitMoveTrigger), "Update")]
    public static class PlatformingLevelPitMoveTriggerPatch
    {
        static void Postfix(PlatformingLevelPitMoveTrigger __instance)
        {
            ExtraPlatformingTriggerBridge.HandlePitMoveTrigger(__instance);
        }
    }

    [HarmonyPatch(typeof(ForestPlatformingLevelChomperSpawner), "Update")]
    public static class ForestPlatformingLevelChomperSpawnerPatch
    {
        static void Postfix(ForestPlatformingLevelChomperSpawner __instance)
        {
            ExtraPlatformingTriggerBridge.HandleChomperSpawner(__instance);
        }
    }

    [HarmonyPatch(typeof(AbstractPlatformingLevelEnemy), "Update")]
    public static class AbstractPlatformingLevelEnemyTriggerPatch
    {
        static void Postfix(AbstractPlatformingLevelEnemy __instance)
        {
            ExtraPlatformingTriggerBridge.HandlePlatformingEnemy(__instance);
        }
    }

    [HarmonyPatch(typeof(LevelPit), "FixedUpdate")]
    public static class LevelPitExtraParticipantPatch
    {
        static void Postfix(LevelPit __instance)
        {
            ExtraPlatformingTriggerBridge.HandleLevelPit(__instance);
        }
    }

    [HarmonyPatch(typeof(PlatformingLevelShootingEnemy), "IsPlayerInRange")]
    public static class PlatformingLevelShootingEnemyRangePatch
    {
        static void Postfix(PlatformingLevelShootingEnemy __instance, PlayerId player, ref bool __result)
        {
            if (__result || player != PlayerId.PlayerTwo)
                return;

            if (ExtraPlatformingTriggerBridge.HasExtraTargetInRange(__instance))
                __result = true;
        }
    }

    [HarmonyPatch(typeof(PlatformingLevelShootingEnemy), "IsPlayerInVolumes")]
    public static class PlatformingLevelShootingEnemyVolumesPatch
    {
        static void Postfix(PlatformingLevelShootingEnemy __instance, PlayerId player, ref bool __result)
        {
            if (__result || player != PlayerId.PlayerTwo)
                return;

            if (ExtraPlatformingTriggerBridge.HasExtraTargetInVolumes(__instance))
                __result = true;
        }
    }

    [HarmonyPatch(typeof(PlatformingLevelShootingEnemy), "Shoot")]
    public static class PlatformingLevelShootingEnemyShootPatch
    {
        static void Prefix(PlatformingLevelShootingEnemy __instance)
        {
            ExtraPlatformingTriggerBridge.OverrideShootingEnemyTarget(__instance);
        }
    }
}
