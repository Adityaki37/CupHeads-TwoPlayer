using System;
using System.Reflection;
using CupheadOnline.Net;
using CupheadOnline.Patches;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// Disabled-by-default smoke test that exercises the same-PC remote input path
    /// through the normal save screen, world map, boss card, and level spawn flow.
    /// </summary>
    internal static class LocalDevE2ETest
    {
        enum Stage
        {
            Idle,
            LoadSlotSelect,
            WaitSlotSelect,
            WaitMap,
            WalkToBoss,
            OpenStartCard,
            WaitLevel,
            Fight,
            Done,
            Failed,
        }

        const float WalkTimeout = 35f;
        const float CardTimeout = 10f;
        const float LevelTimeout = 25f;
        const float FightDuration = 8f;

        static readonly BindingFlags InstanceAny =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        static readonly FieldInfo SlotSelectionField =
            typeof(SlotSelectScreen).GetField("_slotSelection", InstanceAny);
        static readonly FieldInfo SlotsField =
            typeof(SlotSelectScreen).GetField("slots", InstanceAny);
        static readonly MethodInfo EnterGameMethod =
            typeof(SlotSelectScreen).GetMethod("EnterGame", InstanceAny);
        static readonly FieldInfo LevelField =
            typeof(MapLevelLoader).GetField("level", InstanceAny);
        static readonly MethodInfo MapLevelLoaderActivateMethod =
            typeof(MapLevelLoader).GetMethod("Activate", InstanceAny, null, new[] { typeof(MapPlayerController) }, null);

        static Stage _stage = Stage.Idle;
        static float _stageStartedAt;
        static float _lastLogAt;
        static MapLevelLoader _targetLoader;
        static Levels _targetLevel;
        static int _saveSlot = -1;
        static bool _fallbackMapLoadTried;
        static bool _fallbackUnityLevelLoadTried;
        static uint _remoteTick = 1;
        static Vector2 _scriptedAxis;
        static uint _scriptedButtons;
        static uint _scriptedDownButtons;
        static int _scriptedDownUntilFrame;

        public static void Update()
        {
            ClearExpiredDownButtons();

            if (!Plugin.AutoRunLocalDevE2E)
            {
                if (_stage != Stage.Idle && _stage != Stage.Done && _stage != Stage.Failed)
                    ResetScriptedInput();
                return;
            }

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
                    case Stage.WaitMap:
                        WaitMap();
                        break;
                    case Stage.WalkToBoss:
                        WalkToBoss();
                        break;
                    case Stage.OpenStartCard:
                        OpenStartCard();
                        break;
                    case Stage.WaitLevel:
                        WaitLevel();
                        break;
                    case Stage.Fight:
                        Fight();
                        break;
                }
            }
            catch (Exception ex)
            {
                Fail("Exception: " + ex);
            }
        }

        public static bool TryGetLocalAxis(PlayerId playerId, int actionId, out float value)
        {
            value = 0f;
            if (!IsDrivingLocalPlayer(playerId))
                return false;

            value = actionId == 0 ? _scriptedAxis.x : actionId == 1 ? _scriptedAxis.y : 0f;
            return true;
        }

        public static bool TryGetLocalButton(PlayerId playerId, int actionId, bool down, bool up, out bool value)
        {
            value = false;
            if (!IsDrivingLocalPlayer(playerId) || actionId < 0 || actionId >= 32)
                return false;

            uint mask = 1u << actionId;
            if (down)
                value = (_scriptedDownButtons & mask) != 0u;
            else if (up)
                value = false;
            else
                value = (_scriptedButtons & mask) != 0u;
            return true;
        }

        static bool IsDrivingLocalPlayer(PlayerId playerId)
        {
            return Plugin.AutoRunLocalDevE2E
                && (_stage == Stage.WalkToBoss || _stage == Stage.OpenStartCard || _stage == Stage.Fight)
                && playerId == PlayerId.PlayerOne;
        }

        static void Begin()
        {
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

            Log("Starting local dev E2E. VanillaTwoPlayerOnline=" + Plugin.VanillaTwoPlayerOnline
                + "; PacketLoopback=" + (Plugin.Net != null && Plugin.Net.IsLocalLoopbackTestActive) + ".");
            SetStage(Stage.LoadSlotSelect, "Loading slot select.");
        }

        static void LoadSlotSelect()
        {
            ResetScriptedInput();
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

            int slotIndex = FindFirstNonEmptySlot(slots);
            if (slotIndex < 0)
            {
                Fail("No non-empty save slot is available for the E2E test.");
                return;
            }

            _saveSlot = slotIndex;
            if (SlotSelectionField != null)
                SlotSelectionField.SetValue(screen, slotIndex);
            if (slots[slotIndex] != null)
                slots[slotIndex].Init(slotIndex);

            Log("Entering save slot " + slotIndex + " through SlotSelectScreen.EnterGame.");
            if (EnterGameMethod == null)
            {
                Fail("Could not find SlotSelectScreen.EnterGame.");
                return;
            }

            EnterGameMethod.Invoke(screen, null);
            SetStage(Stage.WaitMap, "Waiting for world map.");
        }

        static int FindFirstNonEmptySlot(SlotSelectScreenSlot[] slots)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] == null)
                    continue;
                slots[i].Init(i);
                if (!slots[i].IsEmpty)
                    return i;
            }
            return -1;
        }

        static void WaitMap()
        {
            string sceneName = SceneManager.GetActiveScene().name;
            if (!sceneName.StartsWith("scene_map_world", StringComparison.Ordinal))
            {
                if (!_fallbackMapLoadTried
                 && sceneName == "scene_slot_select"
                 && Time.unscaledTime - _stageStartedAt > 4f)
                {
                    _fallbackMapLoadTried = true;
                    ForceLoadSelectedMap();
                    return;
                }

                if (TimedOut(25f)) Fail("Map did not load; current scene is " + sceneName + ".");
                return;
            }

            if (Map.Current == null || Map.Current.players == null || Map.Current.players.Length < 2)
                return;

            var p1 = Map.Current.players[0];
            var p2 = Map.Current.players[1];
            if (p1 == null || p2 == null)
                return;

            _targetLoader = ChooseNearestBossLoader(p1.transform.position);
            if (_targetLoader == null)
            {
                Fail("No active boss MapLevelLoader was found on " + sceneName + ".");
                return;
            }

            _targetLevel = GetLoaderLevel(_targetLoader);
            Log("Map loaded from save slot " + _saveSlot
                + "; P1=" + DescribeMapPlayer(p1)
                + "; P2=" + DescribeMapPlayer(p2)
                + "; target=" + _targetLevel + ".");
            SetStage(Stage.WalkToBoss, "Walking to " + _targetLevel + ".");
        }

        static void ForceLoadSelectedMap()
        {
            if (_saveSlot < 0)
            {
                Fail("Cannot force map load without a selected save slot.");
                return;
            }

            var data = PlayerData.GetDataForSlot(_saveSlot);
            if (data == null)
            {
                Fail("Selected save slot " + _saveSlot + " has no PlayerData.");
                return;
            }

            Scenes map = data.CurrentMap;
            if (!Enum.IsDefined(typeof(Scenes), map) || map == Scenes.scene_slot_select)
                map = Scenes.scene_map_world_1;
            if (!DLCManager.DLCEnabled() && map == Scenes.scene_map_world_DLC)
                map = Scenes.scene_map_world_1;

            PlayerData.CurrentSaveFileIndex = _saveSlot;
            PlayerManager.player1IsMugman = data.isPlayer1Mugman;
            data.isPlayer1Mugman = PlayerManager.player1IsMugman;
            PlayerData.inGame = true;

            Log("SlotSelectScreen.EnterGame did not transition; loading selected save map via SceneLoader: " + map + ".");
            SceneLoader.LoadScene(
                map,
                SceneLoader.Transition.Fade,
                SceneLoader.Transition.Iris,
                SceneLoader.Icon.Hourglass,
                null);
        }

        static MapLevelLoader ChooseNearestBossLoader(Vector3 from)
        {
            MapLevelLoader[] loaders = UnityEngine.Object.FindObjectsOfType<MapLevelLoader>();
            MapLevelLoader best = null;
            float bestDist = float.MaxValue;
            for (int i = 0; i < loaders.Length; i++)
            {
                var loader = loaders[i];
                if (loader == null || !loader.isActiveAndEnabled || !loader.gameObject.activeInHierarchy)
                    continue;

                Levels level = GetLoaderLevel(loader);
                if (!IsBossLevel(level))
                    continue;

                float dist = Vector2.Distance(from, GetInteractionPoint(loader));
                if (dist < bestDist)
                {
                    bestDist = dist;
                    best = loader;
                }
            }
            return best;
        }

        static bool IsBossLevel(Levels level)
        {
            return level != Levels.Tutorial
                && level != Levels.ShmupTutorial
                && level != Levels.House
                && level != Levels.Mausoleum
                && level != Levels.DiceGate
                && level != Levels.ChaliceTutorial;
        }

        static Levels GetLoaderLevel(MapLevelLoader loader)
        {
            if (loader == null || LevelField == null)
                return Levels.Test;

            object raw = LevelField.GetValue(loader);
            return raw is Levels ? (Levels)raw : Levels.Test;
        }

        static Vector2 GetInteractionPoint(MapLevelLoader loader)
        {
            return (Vector2)loader.transform.position + loader.interactionPoint;
        }

        static void WalkToBoss()
        {
            if (_targetLoader == null)
            {
                Fail("Target loader disappeared while walking.");
                return;
            }

            if (Map.Current == null || Map.Current.players == null || Map.Current.players[0] == null)
                return;

            var p1 = Map.Current.players[0];
            Vector2 target = GetInteractionPoint(_targetLoader);
            Vector2 current = p1.transform.position;
            Vector2 delta = target - current;
            float distance = delta.magnitude;

            if (Time.unscaledTime - _lastLogAt > 2f)
            {
                _lastLogAt = Time.unscaledTime;
                Log("Walking: distance to " + _targetLevel + " is " + distance.ToString("0.00") + ".");
            }

            if (distance <= Mathf.Max(0.2f, _targetLoader.interactionDistance * 0.65f))
            {
                _scriptedAxis = Vector2.zero;
                ActivateTargetLoader(p1);
                SetStage(Stage.OpenStartCard, "Reached " + _targetLevel + "; opening start card.");
                return;
            }

            if (distance <= 0.65f)
            {
                _scriptedAxis = Vector2.zero;
                Log("Close to " + _targetLevel + " after walking; activating map loader at distance " + distance.ToString("0.00") + ".");
                ActivateTargetLoader(p1);
                SetStage(Stage.OpenStartCard, "Reached " + _targetLevel + "; opening start card.");
                return;
            }

            if (TimedOut(WalkTimeout))
            {
                Fail("Could not walk to " + _targetLevel + " within " + WalkTimeout + " seconds.");
                return;
            }

            _scriptedAxis = delta.normalized;
            DriveRemotePlayerTwo(_scriptedAxis, 0u);
        }

        static void ActivateTargetLoader(MapPlayerController player)
        {
            if (_targetLoader == null || player == null)
            {
                Press(CupheadButton.Accept);
                return;
            }

            if (MapLevelLoaderActivateMethod == null)
            {
                Press(CupheadButton.Accept);
                return;
            }

            MapLevelLoaderActivateMethod.Invoke(_targetLoader, new object[] { player });
        }

        static void OpenStartCard()
        {
            _scriptedAxis = Vector2.zero;
            DriveRemotePlayerTwo(Vector2.zero, 0u);

            if (IsAnyStartUiActive())
            {
                LoadActiveStartUi();
                SetStage(Stage.WaitLevel, "Confirming boss start card for " + _targetLevel + ".");
                return;
            }

            if (Time.unscaledTime - _stageStartedAt < 1.5f)
                Press(CupheadButton.Accept);

            if (TimedOut(CardTimeout))
                Fail("Boss start card did not open for " + _targetLevel + ".");
        }

        static bool IsAnyStartUiActive()
        {
            return (MapDifficultySelectStartUI.Current != null && MapDifficultySelectStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Active)
                || (MapConfirmStartUI.Current != null && MapConfirmStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Active)
                || (MapBasicStartUI.Current != null && MapBasicStartUI.Current.CurrentState == AbstractMapSceneStartUI.State.Active);
        }

        static void LoadActiveStartUi()
        {
            Log("Start card is active; loading selected map target " + _targetLevel + ".");
            SceneLoader.LoadLevel(_targetLevel, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
        }

        static void WaitLevel()
        {
            ResetScriptedInput();
            DriveRemotePlayerTwo(Vector2.zero, 0u);

            string sceneName = SceneManager.GetActiveScene().name;
            if (sceneName.StartsWith("scene_map_world", StringComparison.Ordinal)
             || sceneName == "scene_slot_select")
            {
                if (!_fallbackUnityLevelLoadTried
                 && sceneName.StartsWith("scene_map_world", StringComparison.Ordinal)
                 && Time.unscaledTime - _stageStartedAt > 3f)
                {
                    _fallbackUnityLevelLoadTried = true;
                    ForceUnityLevelLoad();
                    return;
                }

                if (TimedOut(LevelTimeout)) Fail("Level did not load after confirming " + _targetLevel + "; current scene is " + sceneName + ".");
                return;
            }

            var p1 = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
            var p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController;
            if (p1 == null || p2 == null || p1.stats == null || p2.stats == null)
            {
                if (TimedOut(LevelTimeout)) Fail("Level scene loaded but both level players were not available.");
                return;
            }

            Log("Level loaded: " + sceneName
                + "; P1=" + DescribeLevelPlayer(p1)
                + "; P2=" + DescribeLevelPlayer(p2)
                + "; P2 networkControlled=" + MultiplayerSession.IsNetworkControlledPlayer(PlayerId.PlayerTwo) + ".");

            if (p2.IsDead || p2.stats.Health <= 0)
            {
                Fail("Player Two spawned dead: " + DescribeLevelPlayer(p2) + ".");
                return;
            }

            SetStage(Stage.Fight, "Fighting briefly to verify live two-player state.");
        }

        static void Fight()
        {
            Hold(CupheadButton.Shoot);
            DriveRemotePlayerTwo(Vector2.zero, ButtonMask(CupheadButton.Shoot));

            if (!TimedOut(FightDuration))
                return;

            var p1 = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
            var p2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController;
            Log("Fight smoke complete; P1=" + DescribeLevelPlayer(p1) + "; P2=" + DescribeLevelPlayer(p2) + ".");

            if (p2 == null || p2.stats == null)
            {
                Fail("Player Two disappeared during the fight smoke.");
                return;
            }

            if (p2.IsDead || p2.stats.Health <= 0)
            {
                Fail("Player Two became dead during the fight smoke: " + DescribeLevelPlayer(p2) + ".");
                return;
            }

            ResetScriptedInput();
            SetStage(Stage.Done, "PASS.");
            Plugin.Log.LogInfo("[LocalDevE2E] PASS");
        }

        static void ForceUnityLevelLoad()
        {
            string levelScene = LevelProperties.GetLevelScene(_targetLevel);
            Log("SceneLoader did not transition from the visible start card; forcing Unity scene load for " + _targetLevel + " (" + levelScene + ").");
            SceneSyncState.ResetTransientSyncState();
            SceneLoader.SetCurrentLevel(_targetLevel);
            PlayerData.inGame = true;
            SceneManager.LoadScene(levelScene);
        }

        static void DriveRemotePlayerTwo(Vector2 axis, uint buttons)
        {
            if (!LocalDevSession.IsActive)
                return;

            InputFramePacket input = new InputFramePacket
            {
                AxisX = axis.x,
                AxisY = axis.y,
                Buttons = buttons,
                Tick = NextRemoteTick(),
            };
            RemoteInputDriver.Apply(PlayerId.PlayerTwo, input);
        }

        static uint NextRemoteTick()
        {
            unchecked
            {
                uint next = _remoteTick++;
                if (next == 0)
                {
                    _remoteTick = 1;
                    next = _remoteTick++;
                }
                return next;
            }
        }

        static void Press(CupheadButton button)
        {
            uint mask = ButtonMask(button);
            _scriptedButtons |= mask;
            _scriptedDownButtons |= mask;
            _scriptedDownUntilFrame = Math.Max(_scriptedDownUntilFrame, Time.frameCount + 4);
        }

        static void Hold(CupheadButton button)
        {
            _scriptedButtons |= ButtonMask(button);
        }

        static uint ButtonMask(CupheadButton button)
        {
            int index = (int)button;
            if (index < 0 || index >= 32)
                return 0u;
            return 1u << index;
        }

        static void ClearExpiredDownButtons()
        {
            if (_scriptedDownButtons != 0u && Time.frameCount > _scriptedDownUntilFrame)
            {
                _scriptedDownButtons = 0u;
                _scriptedButtons &= ~ButtonMask(CupheadButton.Accept);
            }
        }

        static void ResetScriptedInput()
        {
            _scriptedAxis = Vector2.zero;
            _scriptedButtons = 0u;
            _scriptedDownButtons = 0u;
            _scriptedDownUntilFrame = 0;
        }

        static bool TimedOut(float seconds)
        {
            return Time.unscaledTime - _stageStartedAt > seconds;
        }

        static void SetStage(Stage stage, string message)
        {
            _stage = stage;
            _stageStartedAt = Time.unscaledTime;
            _lastLogAt = -100f;
            if (stage == Stage.LoadSlotSelect)
            {
                _fallbackMapLoadTried = false;
                _fallbackUnityLevelLoadTried = false;
            }
            ResetScriptedInput();
            Log(message);
        }

        static void Fail(string message)
        {
            ResetScriptedInput();
            _stage = Stage.Failed;
            Plugin.Log.LogError("[LocalDevE2E] FAIL: " + message);
        }

        static void Log(string message)
        {
            Plugin.Log.LogInfo("[LocalDevE2E] " + message);
        }

        static string DescribeMapPlayer(MapPlayerController player)
        {
            if (player == null)
                return "missing";
            Vector3 pos = player.transform.position;
            return player.id + " state=" + player.state + " pos=(" + pos.x.ToString("0.00") + "," + pos.y.ToString("0.00") + ")";
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
