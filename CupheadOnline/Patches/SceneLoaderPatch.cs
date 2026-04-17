using HarmonyLib;
using CupheadOnline.Net;
using CupheadOnline.Sync;

namespace CupheadOnline.Patches
{
    /// <summary>
    /// Intercepts scene transitions so the host can drag the client into the same level.
    /// Also resets per-level sync state on both sides.
    ///
    /// SceneLoader.LoadLevel(Levels, Transition, ...) is the game's level-load API.
    /// Patching this is sufficient since all level transitions go through it.
    /// </summary>
    [HarmonyPatch(typeof(SceneLoader), "LoadLevel",
        typeof(Levels), typeof(SceneLoader.Transition), typeof(SceneLoader.Icon), typeof(SceneLoader.Context))]
    public static class SceneLoaderLevelsPatch
    {
        static void Prefix(Levels level)
        {
            if (!MultiplayerSession.IsActive) return;

            // Reset per-level systems before new scene loads
            EnemySyncManager.Reset();
            EnemyRegistry.Clear();
            RemotePlayer.Reset();

            if (!MultiplayerSession.IsHost) return; // only host drives scene changes

            var pkt = new SceneChangePacket
            {
                LevelEnum = (int)level,
                RngSeed   = RngSync.NextSeed(),
            };
            Plugin.Net.SendSceneChange(ref pkt);
        }
    }
}
