using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using CupheadOnline.Sync;
using UnityEngine;

namespace CupheadOnline.Patches
{
    static class ExtraWorldInteractionBridge
    {
        static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        static readonly FieldInfo LevelCoinCollectedField =
            typeof(LevelCoin).GetField("_collected", AnyInstance);
        static readonly MethodInfo LevelCoinCollectMethod =
            typeof(LevelCoin).GetMethod("Collect", AnyInstance);

        static readonly FieldInfo PirateBarrelStateField =
            typeof(PirateLevelBarrel).GetField("state", AnyInstance);
        static readonly MethodInfo PirateBarrelPlayerFoundMethod =
            typeof(PirateLevelBarrel).GetMethod("PlayerFound", AnyInstance);

        static readonly List<AbstractPlayerController> ScratchPlayers =
            new List<AbstractPlayerController>(6);

        const float CoinCollectRange = 100f;
        const float PirateBarrelHalfRange = 60f;
        const int PirateBarrelMoveState = 1;

        internal static void HandleLevelCoin(LevelCoin instance)
        {
            if (!ShouldBridge() || instance == null)
                return;
            if (LevelCoinCollectedField == null || LevelCoinCollectMethod == null)
                return;

            object rawCollected = LevelCoinCollectedField.GetValue(instance);
            if (rawCollected is bool && (bool)rawCollected)
                return;

            ScratchPlayers.Clear();
            ExtraRemoteAvatarManager.AppendTargetableControllers(ScratchPlayers);
            if (ScratchPlayers.Count <= 0)
                return;

            Vector3 coinPos = instance.transform.position;
            for (int i = 0; i < ScratchPlayers.Count; i++)
            {
                var player = ScratchPlayers[i];
                if (player == null || player.IsDead || !player.gameObject.activeInHierarchy)
                    continue;

                if (Vector2.Distance(coinPos, player.center) >= CoinCollectRange)
                    continue;

                LevelCoinCollectMethod.Invoke(
                    instance,
                    new object[] { SanitizeCollectorId(player.id) });
                return;
            }
        }

        internal static void HandlePirateBarrel(PirateLevelBarrel instance)
        {
            if (!ShouldBridge() || instance == null)
                return;
            if (PirateBarrelStateField == null || PirateBarrelPlayerFoundMethod == null)
                return;

            object rawState = PirateBarrelStateField.GetValue(instance);
            if (rawState == null || Convert.ToInt32(rawState) != PirateBarrelMoveState)
                return;

            ScratchPlayers.Clear();
            ExtraRemoteAvatarManager.AppendTargetableControllers(ScratchPlayers);
            if (ScratchPlayers.Count <= 0)
                return;

            float minX = instance.transform.position.x - PirateBarrelHalfRange;
            float maxX = instance.transform.position.x + PirateBarrelHalfRange;
            for (int i = 0; i < ScratchPlayers.Count; i++)
            {
                var player = ScratchPlayers[i];
                if (player == null || player.IsDead || !player.gameObject.activeInHierarchy)
                    continue;

                float x = player.center.x;
                if (x <= minX || x >= maxX)
                    continue;

                PirateBarrelPlayerFoundMethod.Invoke(instance, null);
                return;
            }
        }

        static bool ShouldBridge()
        {
            return MultiplayerSession.IsActive && ExtraParticipantTracker.LiveCount > 0;
        }

        static PlayerId SanitizeCollectorId(PlayerId candidate)
        {
            if (candidate == PlayerId.PlayerOne || candidate == PlayerId.PlayerTwo)
                return candidate;

            return PlayerId.PlayerOne;
        }
    }

    [HarmonyPatch(typeof(LevelCoin), "Update")]
    public static class LevelCoinExtraCollectorPatch
    {
        static void Postfix(LevelCoin __instance)
        {
            ExtraWorldInteractionBridge.HandleLevelCoin(__instance);
        }
    }

    [HarmonyPatch(typeof(PirateLevelBarrel), "Update")]
    public static class PirateLevelBarrelExtraTriggerPatch
    {
        static void Postfix(PirateLevelBarrel __instance)
        {
            ExtraWorldInteractionBridge.HandlePirateBarrel(__instance);
        }
    }
}
