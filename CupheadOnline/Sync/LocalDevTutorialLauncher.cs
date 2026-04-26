using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// Disabled-by-default launcher for a fresh-save tutorial smoke test in a
    /// test copy. It requires redirected saves so it cannot clear real slots.
    /// </summary>
    internal static class LocalDevTutorialLauncher
    {
        enum Stage
        {
            Idle,
            LoadSlotSelect,
            WaitSlotSelect,
            WaitIntroOrHouse,
            ActivateTutorial,
            WaitTutorial,
            Done,
            Failed,
        }

        static readonly BindingFlags InstanceAny =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        static readonly FieldInfo SlotSelectionField =
            typeof(SlotSelectScreen).GetField("_slotSelection", InstanceAny);
        static readonly FieldInfo SlotsField =
            typeof(SlotSelectScreen).GetField("slots", InstanceAny);
        static readonly MethodInfo EnterGameMethod =
            typeof(SlotSelectScreen).GetMethod("EnterGame", InstanceAny);
        static readonly MethodInfo HouseTutorialActivateMethod =
            typeof(HouseLevelTutorial).GetMethod("Activate", InstanceAny);

        static Stage _stage = Stage.Idle;
        static float _stageStartedAt;
        static int _saveSlot = -1;
        static bool _forcedHouseLoad;
        static bool _forcedTutorialLoad;

        public static void Update()
        {
            if (!Plugin.AutoRunLocalDevTutorial)
                return;

            try
            {
                switch (_stage)
                {
                    case Stage.Idle:
                        Begin();
                        break;
                    case Stage.LoadSlotSelect:
                        LoadSlotSelect();
                        break;
                    case Stage.WaitSlotSelect:
                        WaitSlotSelect();
                        break;
                    case Stage.WaitIntroOrHouse:
                        WaitIntroOrHouse();
                        break;
                    case Stage.ActivateTutorial:
                        ActivateTutorial();
                        break;
                    case Stage.WaitTutorial:
                        WaitTutorial();
                        break;
                }
            }
            catch (Exception ex)
            {
                Fail("Exception: " + ex);
            }
        }

        static void Begin()
        {
            if (!Plugin.UseSeparateSavePath)
            {
                Fail("Refusing to create a fresh tutorial save without UseSeparateSavePath enabled.");
                return;
            }

            if (Plugin.Net != null && Plugin.Net.IsConnected)
            {
                Fail("Refusing to run while a real Steam session is connected.");
                return;
            }

            string message;
            if (!LocalDevSession.IsActive && !LocalDevSession.Start(out message))
            {
                Fail("Could not start local dev session: " + message);
                return;
            }

            Log("Starting fresh-save tutorial launcher. SavePath=" + Plugin.SeparateSavePath
                + "; PacketLoopback=" + (Plugin.Net != null && Plugin.Net.IsLocalLoopbackTestActive) + ".");
            SetStage(Stage.LoadSlotSelect, "Loading slot select.");
        }

        static void LoadSlotSelect()
        {
            if (SceneManager.GetActiveScene().name != "scene_slot_select")
            {
                SceneLoader.LoadScene(
                    Scenes.scene_slot_select,
                    SceneLoader.Transition.Iris,
                    SceneLoader.Transition.Iris,
                    SceneLoader.Icon.Hourglass,
                    null);
            }

            SetStage(Stage.WaitSlotSelect, "Waiting for save slots.");
        }

        static void WaitSlotSelect()
        {
            var screen = UnityEngine.Object.FindObjectOfType<SlotSelectScreen>();
            if (screen == null)
            {
                if (TimedOut(15f)) Fail("Slot select screen did not appear.");
                return;
            }

            var slots = SlotsField == null ? null : SlotsField.GetValue(screen) as SlotSelectScreenSlot[];
            if (slots == null || slots.Length == 0)
            {
                if (TimedOut(15f)) Fail("Slot select slots were not available.");
                return;
            }

            int slotIndex = FindEmptySlot(slots);
            if (slotIndex < 0)
            {
                slotIndex = 0;
                Log("No empty slots in the redirected save path; resetting test slot 0 only.");
                PlayerData.ClearSlot(slotIndex);
            }

            _saveSlot = slotIndex;
            PlayerData.CurrentSaveFileIndex = slotIndex;
            if (SlotSelectionField != null)
                SlotSelectionField.SetValue(screen, slotIndex);
            if (slots[slotIndex] != null)
                slots[slotIndex].Init(slotIndex);

            Log("Starting fresh save slot " + slotIndex + " through SlotSelectScreen.EnterGame.");
            if (EnterGameMethod == null)
            {
                Fail("Could not find SlotSelectScreen.EnterGame.");
                return;
            }

            EnterGameMethod.Invoke(screen, null);
            SetStage(Stage.WaitIntroOrHouse, "Waiting for intro/house transition.");
        }

        static int FindEmptySlot(SlotSelectScreenSlot[] slots)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;
                slots[i].Init(i);
                if (slots[i].IsEmpty)
                    return i;
            }
            return -1;
        }

        static void WaitIntroOrHouse()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "scene_level_tutorial")
            {
                SetStage(Stage.WaitTutorial, "Tutorial already loaded.");
                return;
            }

            if (sceneName == "scene_level_house_elder_kettle")
            {
                SetStage(Stage.ActivateTutorial, "House loaded; starting tutorial interaction.");
                return;
            }

            if (!_forcedHouseLoad && Time.unscaledTime - _stageStartedAt > 6f)
            {
                _forcedHouseLoad = true;
                ForceLoadHouse(sceneName);
                return;
            }

            if (TimedOut(30f))
                Fail("House did not load; current scene is " + sceneName + ".");
        }

        static void ForceLoadHouse(string currentScene)
        {
            if (_saveSlot < 0)
            {
                Fail("Cannot load house without a selected save slot.");
                return;
            }

            PlayerData.CurrentSaveFileIndex = _saveSlot;
            PlayerData.Data.CurrentMap = Scenes.scene_map_world_1;
            PlayerData.Data.GetMapData(Scenes.scene_map_world_1).sessionStarted = false;
            PlayerData.inGame = true;
            Log("New-save intro did not reach house from " + currentScene + "; loading Elder Kettle house.");
            SceneLoader.LoadScene(
                Scenes.scene_level_house_elder_kettle,
                SceneLoader.Transition.Iris,
                SceneLoader.Transition.Iris,
                SceneLoader.Icon.Hourglass,
                null);
        }

        static void ActivateTutorial()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName == "scene_level_tutorial")
            {
                SetStage(Stage.WaitTutorial, "Tutorial loaded.");
                return;
            }

            var tutorial = UnityEngine.Object.FindObjectOfType<HouseLevelTutorial>();
            if (tutorial != null && HouseTutorialActivateMethod != null)
            {
                Log("Activating Elder Kettle tutorial object.");
                HouseTutorialActivateMethod.Invoke(tutorial, null);
                SetStage(Stage.WaitTutorial, "Waiting for tutorial scene.");
                return;
            }

            if (!_forcedTutorialLoad && Time.unscaledTime - _stageStartedAt > 3f)
            {
                _forcedTutorialLoad = true;
                Log("Tutorial object was not available; loading tutorial scene directly.");
                SceneLoader.LoadScene(
                    Scenes.scene_level_tutorial,
                    SceneLoader.Transition.Iris,
                    SceneLoader.Transition.Iris,
                    SceneLoader.Icon.Hourglass,
                    null);
                SetStage(Stage.WaitTutorial, "Waiting for tutorial scene.");
                return;
            }

            if (TimedOut(12f))
                Fail("Could not find or activate HouseLevelTutorial in " + sceneName + ".");
        }

        static void WaitTutorial()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName != "scene_level_tutorial")
            {
                if (TimedOut(20f))
                    Fail("Tutorial did not load; current scene is " + sceneName + ".");
                return;
            }

            var p1 = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
            var p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController;
            if (p1 == null || p2 == null || p1.stats == null || p2.stats == null)
            {
                if (TimedOut(20f))
                    Fail("Tutorial scene loaded but both level players were not available.");
                return;
            }

            Log("Tutorial loaded from fresh save slot " + _saveSlot
                + "; P1=" + DescribeLevelPlayer(p1)
                + "; P2=" + DescribeLevelPlayer(p2)
                + "; P2 networkControlled=" + MultiplayerSession.IsNetworkControlledPlayer(PlayerId.PlayerTwo) + ".");

            SetStage(Stage.Done, "PASS.");
            Plugin.Log.LogInfo("[LocalDevTutorial] PASS");
        }

        static bool TimedOut(float seconds)
        {
            return Time.unscaledTime - _stageStartedAt > seconds;
        }

        static void SetStage(Stage stage, string message)
        {
            _stage = stage;
            _stageStartedAt = Time.unscaledTime;
            if (stage == Stage.LoadSlotSelect)
            {
                _saveSlot = -1;
                _forcedHouseLoad = false;
                _forcedTutorialLoad = false;
            }
            Log(message);
        }

        static void Fail(string message)
        {
            _stage = Stage.Failed;
            Plugin.Log.LogError("[LocalDevTutorial] FAIL: " + message);
        }

        static void Log(string message)
        {
            Plugin.Log.LogInfo("[LocalDevTutorial] " + message);
        }

        static string DescribeLevelPlayer(LevelPlayerController player)
        {
            if (player == null)
                return "missing";
            Vector3 pos = player.transform.position;
            string health = player.stats == null ? "no-stats" : player.stats.Health + "/" + player.stats.HealthMax;
            return player.id + " dead=" + player.IsDead + " hp=" + health + " pos=(" + pos.x.ToString("0.00") + "," + pos.y.ToString("0.00") + ")";
        }
    }
}
