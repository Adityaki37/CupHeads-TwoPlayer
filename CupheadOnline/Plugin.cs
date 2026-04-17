using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using CupheadOnline.Net;
using CupheadOnline.UI;
using CupheadOnline.Patches;

namespace CupheadOnline
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
    [BepInProcess("Cuphead.exe")]
    public sealed class Plugin : BaseUnityPlugin
    {
        // ──────────────────────────────────────────────────────────────────────
        //  Singleton
        // ──────────────────────────────────────────────────────────────────────
        public static Plugin           Instance { get; private set; }
        public static ManualLogSource  Log      { get; private set; }
        public static SteamNetManager  Net      { get; private set; }

        // ──────────────────────────────────────────────────────────────────────
        //  Unity lifecycle
        // ──────────────────────────────────────────────────────────────────────
        void Awake()
        {
            Instance = this;
            Log      = Logger;
            Log.LogInfo("CupheadOnline " + PluginInfo.VERSION + " loading\u2026");

            // Networking manager — Steam P2P transport (lobby + invite flow)
            Net = new SteamNetManager();
            Net.OnStatusChanged += msg =>
            {
                // Animate dots on "waiting" messages; plain display otherwise
                bool animate = msg.IndexOf("Waiting", StringComparison.OrdinalIgnoreCase) >= 0
                            || msg.IndexOf("Connecting", StringComparison.OrdinalIgnoreCase) >= 0
                            || msg.IndexOf("Creating", StringComparison.OrdinalIgnoreCase) >= 0;
                MpMenuState.SetStatus(msg, animate);
                Log.LogInfo("[Net] " + msg);
            };
            Net.TryInitializeSteam();

            // ── Diagnostic: scan our own assembly types and expose any failures ──
            try
            {
                var types = Assembly.GetExecutingAssembly().GetTypes();
                Log.LogInfo("[Plugin] Assembly type scan OK — " + types.Length + " types.");
            }
            catch (ReflectionTypeLoadException rtle)
            {
                Log.LogError("[Plugin] === ASSEMBLY TYPE SCAN FAILURES ===");
                foreach (var le in rtle.LoaderExceptions)
                    if (le != null)
                        Log.LogError("[Plugin]   " + le.GetType().Name + ": " + le.Message);
                Log.LogError("[Plugin] === END TYPE SCAN FAILURES ===");
            }
            catch (Exception ex)
            {
                Log.LogError("[Plugin] GetTypes() threw: " + ex);
            }

            // ── Apply patches one-by-one so a single failure does not block all ─
            var harmony = new Harmony(PluginInfo.GUID);

            // Core UI — SlotSelect patches inject the native MULTIPLAYER menu item
            PatchSafe(harmony, typeof(SlotSelectAwakePatch));
            PatchSafe(harmony, typeof(SlotSelectUpdatePatch));

            // Player lifecycle
            PatchSafe(harmony, typeof(PlayerManagerAwakePatch));
            PatchSafe(harmony, typeof(PlayerLevelInitPatch));
            PatchSafe(harmony, typeof(StatsLevelInitPatch));
            PatchSafe(harmony, typeof(LevelStartPatch));

            // Movement / input sync
            PatchSafe(harmony, typeof(PlayerMotorPatch));
            PatchSafe(harmony, typeof(PlayerInputAxisPatch));
            PatchSafe(harmony, typeof(PlayerInputAxisIntPatch));
            PatchSafe(harmony, typeof(PlayerInputButtonPatch));
            PatchSafe(harmony, typeof(ParryPatch));

            // Damage authority
            PatchSafe(harmony, typeof(PlayerDamagePatch));

            // Scene transitions
            PatchSafe(harmony, typeof(SceneLoaderLevelsPatch));

            Log.LogInfo("[Plugin] Patch pass complete.");
        }

        static void PatchSafe(Harmony harmony, Type patchType)
        {
            try
            {
                harmony.CreateClassProcessor(patchType).Patch();
                Log.LogInfo("[Plugin] OK: " + patchType.Name);
            }
            catch (Exception ex)
            {
                Log.LogWarning("[Plugin] SKIP " + patchType.Name + ": " + ex.Message);
            }
        }

        void Update()
        {
            MainThreadQueue.Drain();
            Net?.Poll();
        }

        void OnLevelWasLoaded(int level)
        {
            // Reset the injection flag so re-entering the title screen injects again
            UI.MultiplayerMenuInjector.ResetOnSceneChange();
        }

        void OnDestroy()
        {
            Net?.Dispose();
            MultiplayerSession.End();
        }
    }

    internal static class PluginInfo
    {
        public const string GUID    = "com.cupheadonline.mod";
        public const string NAME    = "CupheadOnline";
        public const string VERSION = "1.0.0";
    }
}
