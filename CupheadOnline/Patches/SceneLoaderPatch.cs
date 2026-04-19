using HarmonyLib;
using CupheadOnline.Net;
using CupheadOnline.Sync;

namespace CupheadOnline.Patches
{
    internal static class SceneSyncState
    {
        internal static bool SuppressMenuSceneBroadcast;

        internal static void ResetTransientSyncState()
        {
            EnemySyncManager.Reset();
            EnemyRegistry.Clear();
            RemotePlayer.Reset();
            RemoteInputDriver.Reset();
            ExtraParticipantTracker.Reset();
        }
    }

    [HarmonyPatch(typeof(SceneLoader), "LoadLevel",
        typeof(Levels), typeof(SceneLoader.Transition), typeof(SceneLoader.Icon), typeof(SceneLoader.Context))]
    public static class SceneLoaderLevelsPatch
    {
        static void Prefix(Levels level)
        {
            if (!MultiplayerSession.IsActive) return;

            SceneSyncState.ResetTransientSyncState();

            if (!MultiplayerSession.IsHost) return;

            SceneSyncState.SuppressMenuSceneBroadcast = true;

            var pkt = new SceneChangePacket
            {
                LevelEnum = (int)level,
                RngSeed   = RngSync.NextSeed(),
            };
            Plugin.Net.SendSceneChange(ref pkt);
        }

        static void Postfix()
        {
            SceneSyncState.SuppressMenuSceneBroadcast = false;
        }
    }

    [HarmonyPatch(typeof(SceneLoader), "LoadScene",
        typeof(Scenes), typeof(SceneLoader.Transition), typeof(SceneLoader.Transition), typeof(SceneLoader.Icon), typeof(SceneLoader.Context))]
    public static class SceneLoaderScenesPatch
    {
        static void Prefix(
            Scenes scene,
            SceneLoader.Transition transitionStart,
            SceneLoader.Transition transitionEnd,
            SceneLoader.Icon icon)
        {
            if (!MultiplayerSession.IsActive) return;

            SceneSyncState.ResetTransientSyncState();

            if (!MultiplayerSession.IsHost) return;
            if (SceneSyncState.SuppressMenuSceneBroadcast) return;

            var pkt = new MenuSceneChangePacket
            {
                SceneEnum       = (int)scene,
                TransitionStart = (byte)transitionStart,
                TransitionEnd   = (byte)transitionEnd,
                Icon            = (byte)icon,
                RngSeed         = RngSync.NextSeed(),
            };
            Plugin.Net.SendMenuSceneChange(ref pkt);
        }
    }
}
