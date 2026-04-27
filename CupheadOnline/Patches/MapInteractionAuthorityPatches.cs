using HarmonyLib;

namespace CupheadOnline.Patches
{
    static class MapInteractionAuthority
    {
        internal static bool AllowHostPlayerOne(MapPlayerController player, string label)
        {
            if (!MultiplayerSession.IsActive)
                return true;

            if (MultiplayerSession.IsHost && player != null && player.id == PlayerId.PlayerOne)
                return true;

            string role = MultiplayerSession.IsHost ? "host" : "guest";
            string playerLabel = player == null ? "unknown" : player.id.ToString();
            Plugin.Log.LogInfo(
                "[MapAuthority] Blocked "
                + label
                + " activation by "
                + playerLabel
                + " on "
                + role
                + "; waiting for host PlayerOne.");
            return false;
        }
    }

    [HarmonyPatch(typeof(MapLevelLoader), "Activate", typeof(MapPlayerController))]
    public static class MapLevelLoaderAuthorityPatch
    {
        static bool Prefix(MapPlayerController player)
        {
            return MapInteractionAuthority.AllowHostPlayerOne(player, "level");
        }
    }

    [HarmonyPatch(typeof(MapSceneLoader), "Activate", typeof(MapPlayerController))]
    public static class MapSceneLoaderAuthorityPatch
    {
        static bool Prefix(MapPlayerController player)
        {
            return MapInteractionAuthority.AllowHostPlayerOne(player, "scene");
        }
    }

    [HarmonyPatch(typeof(MapShopLoader), "Activate", typeof(MapPlayerController))]
    public static class MapShopLoaderAuthorityPatch
    {
        static bool Prefix(MapPlayerController player)
        {
            return MapInteractionAuthority.AllowHostPlayerOne(player, "shop");
        }
    }

    [HarmonyPatch(typeof(MapTutorialLoader), "Activate", typeof(MapPlayerController))]
    public static class MapTutorialLoaderAuthorityPatch
    {
        static bool Prefix(MapPlayerController player)
        {
            return MapInteractionAuthority.AllowHostPlayerOne(player, "tutorial");
        }
    }

    [HarmonyPatch(typeof(MapBakeryLoader), "Activate", typeof(MapPlayerController))]
    public static class MapBakeryLoaderAuthorityPatch
    {
        static bool Prefix(MapPlayerController player)
        {
            return MapInteractionAuthority.AllowHostPlayerOne(player, "bakery");
        }
    }

    [HarmonyPatch(typeof(MapDiceGateSceneLoader), "Activate", typeof(MapPlayerController))]
    public static class MapDiceGateSceneLoaderAuthorityPatch
    {
        static bool Prefix(MapPlayerController player)
        {
            return MapInteractionAuthority.AllowHostPlayerOne(player, "dice-gate");
        }
    }
}
