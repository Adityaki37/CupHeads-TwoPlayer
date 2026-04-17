using System.Reflection;
using HarmonyLib;
using CupheadOnline.Net;

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

        static void Prefix(SlotSelectScreen __instance)
        {
            if (!MultiplayerSession.IsActive || !MultiplayerSession.IsHost) return;
            if (Plugin.Net == null || !Plugin.Net.IsConnected) return;
            if (SlotSelectionField == null || SlotsField == null) return;

            var slots = SlotsField.GetValue(__instance) as SlotSelectScreenSlot[];
            if (slots == null || slots.Length == 0) return;

            int slotIndex = (int)SlotSelectionField.GetValue(__instance);
            if (slotIndex < 0 || slotIndex >= slots.Length) return;

            var slot = slots[slotIndex];
            if (slot == null) return;

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
                CurrentMapScene = (int)currentMap,
            };
            Sync.SessionSync.RecordSelectedSave(ref pkt);
            Plugin.Net.SendSaveSlotSync(ref pkt);
            Sync.SessionSync.BroadcastSelectedSaveProfile();
            Sync.SessionSync.BroadcastSessionSnapshot(true);
            Plugin.Log.LogInfo(
                "[SaveSync] Broadcast host slot "
                + slotIndex
                + " (map="
                + currentMap
                + ", empty="
                + isEmpty
                + ", mugman="
                + slot.isPlayer1Mugman
                + ").");
        }
    }
}
