using HarmonyLib;
using CupheadOnline.Sync;

namespace CupheadOnline.Patches
{
    /// <summary>
    /// Replaces Cuphead's built-in random with a seeded deterministic PRNG when a
    /// network session is active.  Both host and client are seeded identically via
    /// SceneChangePacket, ensuring boss patterns, enemy spawns, and RNG-dependent
    /// events unfold in exactly the same order on both machines.
    /// </summary>
    [HarmonyPatch(typeof(Rand), "GetValue", typeof(float), typeof(float))]
    public static class RandPatch
    {
        static bool Prefix(float min, float max, ref float __result)
        {
            if (!MultiplayerSession.IsActive) return true;
            if (!RngSync.IsSeeded) return true;

            __result = RngSync.NextFloat(min, max);
            return false; // skip original
        }
    }

    [HarmonyPatch(typeof(Rand), "GetValue", typeof(int), typeof(int))]
    public static class RandIntPatch
    {
        static bool Prefix(int min, int max, ref int __result)
        {
            if (!MultiplayerSession.IsActive) return true;
            if (!RngSync.IsSeeded) return true;

            __result = RngSync.NextInt(min, max);
            return false;
        }
    }
}
