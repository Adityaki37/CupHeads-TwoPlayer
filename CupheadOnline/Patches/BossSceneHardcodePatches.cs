using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using CupheadOnline.Sync;
using HarmonyLib;
using UnityEngine;

namespace CupheadOnline.Patches
{
    static class BossSceneTargetingMath
    {
        internal static readonly List<AbstractPlayerController> ScratchPlayers =
            new List<AbstractPlayerController>(8);

        internal static bool IsExtendedTargetingActive()
        {
            return MultiplayerSession.IsActive && ExtraParticipantTracker.TotalCount > 0;
        }

        internal static bool TryCollectAlivePlayers(List<AbstractPlayerController> target)
        {
            return PlayerSelectionMath.TryCollectPlayers(target);
        }

        internal static AbstractPlayerController PickNearest(Vector3 origin)
        {
            if (!TryCollectAlivePlayers(ScratchPlayers))
                return null;

            AbstractPlayerController best = null;
            float bestDistance = float.MaxValue;
            for (int i = 0; i < ScratchPlayers.Count; i++)
            {
                var player = ScratchPlayers[i];
                if (player == null || player.IsDead)
                    continue;

                float sqrDistance = (player.center - origin).sqrMagnitude;
                if (sqrDistance >= bestDistance)
                    continue;

                bestDistance = sqrDistance;
                best = player;
            }

            return best;
        }

        internal static AbstractPlayerController PickRandom()
        {
            if (!TryCollectAlivePlayers(ScratchPlayers))
                return null;
            return ScratchPlayers[Random.Range(0, ScratchPlayers.Count)];
        }

        internal static int CountAlive()
        {
            return TryCollectAlivePlayers(ScratchPlayers) ? ScratchPlayers.Count : 0;
        }

        internal static bool AnyPlayerWithinXDistance(Vector3 origin, float distance)
        {
            if (!TryCollectAlivePlayers(ScratchPlayers))
                return false;

            for (int i = 0; i < ScratchPlayers.Count; i++)
            {
                var player = ScratchPlayers[i];
                if (player == null || player.IsDead)
                    continue;
                if (Mathf.Abs(player.transform.position.x - origin.x) <= distance)
                    return true;
            }

            return false;
        }

        internal static bool PlayersStraddleX(Vector3 origin)
        {
            if (!TryCollectAlivePlayers(ScratchPlayers))
                return false;

            bool hasLeft = false;
            bool hasRight = false;
            for (int i = 0; i < ScratchPlayers.Count; i++)
            {
                var player = ScratchPlayers[i];
                if (player == null || player.IsDead)
                    continue;

                float delta = origin.x - player.transform.position.x;
                if (delta < 0f) hasRight = true;
                else if (delta > 0f) hasLeft = true;

                if (hasLeft && hasRight)
                    return true;
            }

            return false;
        }
    }

    [HarmonyPatch(typeof(RobotLevelRobotHead), "OnPrimaryAttack")]
    public static class RobotLevelRobotHeadPrimaryPatch
    {
        static readonly FieldInfo CurrentPlayerField =
            AccessTools.Field(typeof(RobotLevelRobotHead), "currentPlayer");

        static void Postfix(RobotLevelRobotHead __instance)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive() || CurrentPlayerField == null)
                return;

            var target = BossSceneTargetingMath.PickNearest(__instance.transform.position);
            if (target != null)
                CurrentPlayerField.SetValue(__instance, target);
        }
    }

    [HarmonyPatch(typeof(ChessKnightLevelKnight), "LevelInit")]
    public static class ChessKnightLevelInitPatch3P
    {
        static readonly FieldInfo TargetPlayerField =
            AccessTools.Field(typeof(ChessKnightLevelKnight), "targetPlayer");

        static void Postfix(ChessKnightLevelKnight __instance)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive() || TargetPlayerField == null)
                return;

            var target = BossSceneTargetingMath.PickRandom();
            if (target != null)
                TargetPlayerField.SetValue(__instance, target);
        }
    }

    [HarmonyPatch(typeof(ChessKnightLevelKnight), "CheckTaunt")]
    public static class ChessKnightCheckTauntPatch3P
    {
        static readonly FieldInfo PropertiesField =
            AccessTools.Field(typeof(LevelProperties.ChessKnight.Entity), "properties");
        static readonly FieldInfo TauntAttackCounterField =
            AccessTools.Field(typeof(ChessKnightLevelKnight), "tauntAttackCounter");
        static readonly MethodInfo LongCoroutineMethod =
            AccessTools.Method(typeof(ChessKnightLevelKnight), "long_cr");
        static readonly MethodInfo TauntCoroutineMethod =
            AccessTools.Method(typeof(ChessKnightLevelKnight), "taunt_cr");
        static readonly Traverse StateProperty =
            Traverse.Create(typeof(ChessKnightLevelKnight)).Property("state");

        static bool Prefix(ChessKnightLevelKnight __instance)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive()
             || PropertiesField == null
             || TauntAttackCounterField == null
             || LongCoroutineMethod == null
             || TauntCoroutineMethod == null)
            {
                return true;
            }

            var properties = PropertiesField.GetValue(__instance) as LevelProperties.ChessKnight;
            if (properties == null)
                return true;

            float tauntDistance = properties.CurrentState.taunt.tauntDistance;
            bool playerIsClose = BossSceneTargetingMath.AnyPlayerWithinXDistance(__instance.transform.position, tauntDistance);
            if (playerIsClose)
            {
                Traverse.Create(__instance).Property("state").SetValue(ChessKnightLevelKnight.State.Idle);
                return false;
            }

            int tauntCounter = (int)TauntAttackCounterField.GetValue(__instance);
            IEnumerator routine = tauntCounter <= 0
                ? (IEnumerator)LongCoroutineMethod.Invoke(__instance, null)
                : (IEnumerator)TauntCoroutineMethod.Invoke(__instance, null);
            if (routine != null)
                __instance.StartCoroutine(routine);
            return false;
        }
    }

    [HarmonyPatch(typeof(ChessKnightLevelKnight), "shouldBackDash")]
    public static class ChessKnightShouldBackDashPatch3P
    {
        static readonly MethodInfo ShouldFaceLeftMethod =
            AccessTools.Method(typeof(ChessKnightLevelKnight), "shouldFaceLeft");

        static bool Prefix(ChessKnightLevelKnight __instance, ref bool __result)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive() || ShouldFaceLeftMethod == null)
                return true;

            bool faceLeft = (bool)ShouldFaceLeftMethod.Invoke(__instance, null);
            float edgeTarget = faceLeft ? -640f : 640f;
            float edgeDistance = Mathf.Abs(edgeTarget - __instance.transform.position.x);
            bool straddled = BossSceneTargetingMath.PlayersStraddleX(__instance.transform.position);

            __result = edgeDistance < 640f && !straddled;
            return false;
        }
    }

    [HarmonyPatch(typeof(ChessBishopLevelBishop), "FixedUpdate")]
    public static class ChessBishopFixedUpdatePatch3P
    {
        static readonly FieldInfo PlayerMaskField =
            AccessTools.Field(typeof(ChessBishopLevelBishop), "playerMask");

        static void Postfix(ChessBishopLevelBishop __instance)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive() || PlayerMaskField == null)
                return;

            var masks = PlayerMaskField.GetValue(__instance) as Transform[];
            if (masks == null || masks.Length == 0)
                return;
            if (!BossSceneTargetingMath.TryCollectAlivePlayers(BossSceneTargetingMath.ScratchPlayers))
                return;

            for (int i = 0; i < masks.Length && i < BossSceneTargetingMath.ScratchPlayers.Count; i++)
            {
                var mask = masks[i];
                var player = BossSceneTargetingMath.ScratchPlayers[i];
                if (mask == null || player == null)
                    continue;

                mask.position = player.transform.position + Vector3.up * 50f;
            }
        }
    }

    [HarmonyPatch(typeof(ChessBishopLevelBishop), "findMoveVerticalInitialAngle")]
    public static class ChessBishopFindVerticalAnglePatch3P
    {
        static void Postfix(ref float __result, float minimumDistance, float value, bool invert, float speed, Transform pivotPoint, Vector3 pivotOffset)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive())
                return;
            if (!BossSceneTargetingMath.TryCollectAlivePlayers(BossSceneTargetingMath.ScratchPlayers))
                return;

            float minimumDistanceSqr = minimumDistance * minimumDistance;
            for (int attempt = 0; attempt < 20; attempt++)
            {
                float angle = Random.Range(0f, Mathf.PI * 2f);
                float candidateAngle = angle;
                float candidateValue = value;
                bool candidateInvert = invert;
                Vector3 candidate = CalculateVerticalPosition(
                    ref candidateAngle,
                    ref candidateValue,
                    ref candidateInvert,
                    speed,
                    pivotPoint,
                    pivotOffset);

                bool tooClose = false;
                for (int i = 0; i < BossSceneTargetingMath.ScratchPlayers.Count; i++)
                {
                    var player = BossSceneTargetingMath.ScratchPlayers[i];
                    if (player == null)
                        continue;
                    if ((candidate - player.center).sqrMagnitude < minimumDistanceSqr)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    __result = angle;
                    return;
                }
            }
        }

        static Vector3 CalculateVerticalPosition(ref float angle, ref float value, ref bool invert, float speed, Transform pivotPoint, Vector3 pivotOffset)
        {
            angle += speed * CupheadTime.FixedDelta;
            if (angle > Mathf.PI * 2f)
            {
                invert = !invert;
                angle -= Mathf.PI * 2f;
            }
            if (angle < 0f)
                angle += Mathf.PI * 2f;

            Vector3 pivot = invert ? pivotPoint.position + pivotOffset : pivotPoint.position;
            value = invert ? -1f : 1f;
            return pivot + new Vector3(-Mathf.Sin(angle) * 500f, Mathf.Cos(angle) * value * 150f, 0f);
        }
    }

    [HarmonyPatch(typeof(ChessBishopLevelBishop), "findMoveHorizontalInitialPosition")]
    public static class ChessBishopFindHorizontalPositionPatch3P
    {
        static void Postfix(ref Vector3 __result, float minimumDistance, float yPosition, float maxDistance, float xSpeed, float amplitude, float frequency)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive())
                return;
            if (!BossSceneTargetingMath.TryCollectAlivePlayers(BossSceneTargetingMath.ScratchPlayers))
                return;

            float minimumDistanceSqr = minimumDistance * minimumDistance;
            for (int attempt = 0; attempt < 20; attempt++)
            {
                Vector3 candidate = new Vector3(Random.Range(-maxDistance, maxDistance), yPosition, 0f);
                bool tooClose = false;
                for (int i = 0; i < BossSceneTargetingMath.ScratchPlayers.Count; i++)
                {
                    var player = BossSceneTargetingMath.ScratchPlayers[i];
                    if (player == null)
                        continue;
                    if ((candidate - player.center).sqrMagnitude < minimumDistanceSqr)
                    {
                        tooClose = true;
                        break;
                    }
                }

                if (!tooClose)
                {
                    __result = candidate;
                    return;
                }
            }
        }
    }

    [HarmonyPatch(typeof(SallyStagePlayLevelMeteor), "ParryStar")]
    public static class SallyMeteorParryPatch3P
    {
        static readonly FieldInfo ParryCounterField =
            AccessTools.Field(typeof(SallyStagePlayLevelMeteor), "parryCounter");
        static readonly FieldInfo StarField =
            AccessTools.Field(typeof(SallyStagePlayLevelMeteor), "star");

        static bool Prefix(SallyStagePlayLevelMeteor __instance)
        {
            if (!BossSceneTargetingMath.IsExtendedTargetingActive()
             || ParryCounterField == null
             || StarField == null)
            {
                return true;
            }

            int aliveCount = Mathf.Max(1, BossSceneTargetingMath.CountAlive());
            int requiredCounter = Mathf.Max(0, aliveCount - 1);
            int parryCounter = (int)ParryCounterField.GetValue(__instance);
            if (parryCounter < requiredCounter)
            {
                ParryCounterField.SetValue(__instance, parryCounter + 1);
                return false;
            }

            var animator = __instance.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("SpinStar");

            Traverse.Create(__instance).Property("state").SetValue(SallyStagePlayLevelMeteor.State.Leaving);
            __instance.StartCoroutine("leave_cr");
            var star = StarField.GetValue(__instance) as ParrySwitch;
            if (star != null)
                star.StartParryCooldown();

            return false;
        }
    }
}
