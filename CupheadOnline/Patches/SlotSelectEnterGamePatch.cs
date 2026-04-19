using System.Reflection;
using HarmonyLib;
using CupheadOnline.Net;
using CupheadOnline.UI;
using UnityEngine;

namespace CupheadOnline.Patches
{
    [HarmonyPatch(typeof(SlotSelectScreen), "EnterGame")]
    public static class SlotSelectEnterGamePatch
    {
        static readonly BindingFlags BF = BindingFlags.NonPublic | BindingFlags.Instance;
        static readonly FieldInfo SlotSelectionField =
            typeof(SlotSelectScreen).GetField("_slotSelection", BF);
        static readonly FieldInfo SlotsField =
            typeof(SlotSelectScreen).GetField("slots", BF);
        static readonly MethodInfo EnterGameMethod =
            typeof(SlotSelectScreen).GetMethod("EnterGame", BF);

        internal static bool TryBroadcastLobbySelection(
            SlotSelectScreen screen,
            int slotIndex,
            bool player1IsMugman,
            out SaveSlotSyncPacket packet,
            out string reason)
        {
            packet = default(SaveSlotSyncPacket);
            reason = string.Empty;

            if (screen == null)
            {
                reason = "The lobby screen is no longer available.";
                return false;
            }
            if (Plugin.Net == null || !Plugin.Net.IsConnected || !MultiplayerSession.IsHost)
            {
                reason = "Connect a guest before choosing a multiplayer save.";
                return false;
            }
            if (SlotSelectionField == null || SlotsField == null)
            {
                reason = "The save-slot reflection hooks are missing.";
                return false;
            }

            var slots = SlotsField.GetValue(screen) as SlotSelectScreenSlot[];
            if (slots == null || slots.Length == 0)
            {
                reason = "Cuphead's save slots are not available yet.";
                return false;
            }

            slotIndex = Mathf.Clamp(slotIndex, 0, slots.Length - 1);
            var slot = slots[slotIndex];
            if (slot == null)
            {
                reason = "That save slot could not be loaded.";
                return false;
            }

            var data = PlayerData.GetDataForSlot(slotIndex);
            if (data != null)
                data.isPlayer1Mugman = player1IsMugman;

            SlotSelectionField.SetValue(screen, slotIndex);
            slot.Init(slotIndex);

            bool isEmpty = slot.IsEmpty;
            Scenes currentMap = Scenes.scene_map_world_1;
            if (!isEmpty)
            {
                if (data != null)
                    currentMap = data.CurrentMap;

                if (!DLCManager.DLCEnabled() && currentMap == Scenes.scene_map_world_DLC)
                    currentMap = Scenes.scene_map_world_1;
            }

            packet = new SaveSlotSyncPacket
            {
                SlotIndex = (byte)slotIndex,
                Flags = (byte)((isEmpty ? 1 : 0) | (player1IsMugman ? 2 : 0)),
                SaveRevision = 0,
                CurrentMapScene = (int)currentMap,
            };

            Sync.SessionSync.RecordSelectedSave(ref packet);
            Plugin.Net.SendSaveSlotSync(ref packet);
            Sync.SessionSync.BroadcastSelectedSaveProfile();
            Sync.SessionSync.BroadcastSessionSnapshot(true);
            return true;
        }

        internal static bool TryStartFromLobbySelection(
            SlotSelectScreen screen,
            int slotIndex,
            bool player1IsMugman,
            out string reason)
        {
            reason = string.Empty;

            SaveSlotSyncPacket packet;
            if (!TryBroadcastLobbySelection(screen, slotIndex, player1IsMugman, out packet, out reason))
                return false;

            if (!Sync.SessionSync.CanHostStartRun(out reason))
                return false;

            if (EnterGameMethod == null)
            {
                reason = "Could not reach Cuphead's start-game method.";
                return false;
            }

            bool suppressVanillaSceneBroadcast = TrySendImmediateLaunchScene(ref packet);
            if (suppressVanillaSceneBroadcast)
                SceneSyncState.SuppressMenuSceneBroadcast = true;

            try
            {
                EnterGameMethod.Invoke(screen, null);
            }
            finally
            {
                if (suppressVanillaSceneBroadcast)
                    SceneSyncState.SuppressMenuSceneBroadcast = false;
            }
            return true;
        }

        static bool TrySendImmediateLaunchScene(ref SaveSlotSyncPacket packet)
        {
            if (Plugin.Net == null || !Plugin.Net.IsConnected || !MultiplayerSession.IsHost)
                return false;
            if (packet.IsEmpty)
                return false;
            if (!System.Enum.IsDefined(typeof(Scenes), packet.CurrentMapScene))
                return false;

            var pkt = new MenuSceneChangePacket
            {
                SceneEnum = packet.CurrentMapScene,
                TransitionStart = (byte)SceneLoader.Transition.Fade,
                TransitionEnd = (byte)SceneLoader.Transition.Iris,
                Icon = (byte)SceneLoader.Icon.Hourglass,
                RngSeed = Sync.RngSync.NextSeed(),
            };
            Plugin.Net.SendMenuSceneChange(ref pkt);
            Sync.SessionSync.BroadcastSessionSnapshot(true);
            Plugin.Log.LogInfo("[SaveSync] Sent immediate lobby launch scene " + ((Scenes)packet.CurrentMapScene) + ".");
            return true;
        }

        static bool Prefix(SlotSelectScreen __instance)
        {
            if (!MultiplayerSession.IsActive || !MultiplayerSession.IsHost) return true;
            if (Plugin.Net == null || !Plugin.Net.IsConnected) return true;
            if (SlotSelectionField == null || SlotsField == null) return true;

            var slots = SlotsField.GetValue(__instance) as SlotSelectScreenSlot[];
            if (slots == null || slots.Length == 0) return true;

            int slotIndex = (int)SlotSelectionField.GetValue(__instance);
            if (slotIndex < 0 || slotIndex >= slots.Length) return true;

            var slot = slots[slotIndex];
            if (slot == null) return true;

            bool isEmpty = slot.IsEmpty;
            Scenes currentMap = Scenes.scene_map_world_1;
            if (!isEmpty)
            {
                var data = PlayerData.GetDataForSlot(slotIndex);
                if (data != null)
                    currentMap = data.CurrentMap;

                if (!DLCManager.DLCEnabled() && currentMap == Scenes.scene_map_world_DLC)
                    currentMap = Scenes.scene_map_world_1;
            }

            var pkt = new SaveSlotSyncPacket
            {
                SlotIndex       = (byte)slotIndex,
                Flags           = (byte)((isEmpty ? 1 : 0) | (slot.isPlayer1Mugman ? 2 : 0)),
                SaveRevision    = 0,
                CurrentMapScene = (int)currentMap,
            };
            Sync.SessionSync.RecordSelectedSave(ref pkt);
            Plugin.Net.SendSaveSlotSync(ref pkt);
            Sync.SessionSync.BroadcastSelectedSaveProfile();
            Sync.SessionSync.BroadcastSessionSnapshot(true);

            string gateReason;
            if (!Sync.SessionSync.CanHostStartRun(out gateReason))
            {
                ConnectionHUD.Show(gateReason);
                Plugin.Log.LogInfo("[SaveSync] Host start blocked: " + gateReason);
                return false;
            }

            Plugin.Log.LogInfo(
                "[SaveSync] Broadcast host slot "
                + slotIndex
                + " (map="
                + currentMap
                + ", empty="
                + isEmpty
                + ", rev="
                + pkt.SaveRevision
                + ", mugman="
                + slot.isPlayer1Mugman
                + ").");
            return true;
        }
    }
}
