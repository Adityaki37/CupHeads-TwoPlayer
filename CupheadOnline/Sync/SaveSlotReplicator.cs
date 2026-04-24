using UnityEngine;
using CupheadOnline.Net;

namespace CupheadOnline.Sync
{
    public static class SaveSlotReplicator
    {
        public static void Apply(SaveSlotSyncPacket pkt)
        {
            if (SessionSync.IsStaleSaveRevision(pkt.SaveRevision))
            {
                Plugin.Log.LogInfo("[SaveSync] Ignored stale save slot revision " + pkt.SaveRevision + ".");
                return;
            }

            if (!System.Enum.IsDefined(typeof(Scenes), pkt.CurrentMapScene))
            {
                Plugin.Log.LogWarning("[SaveSync] Ignored save slot with invalid map scene " + pkt.CurrentMapScene + ".");
                return;
            }

            int slot = Mathf.Clamp(pkt.SlotIndex, 0, 2);
            var mapScene = (Scenes)pkt.CurrentMapScene;

            PlayerData.CurrentSaveFileIndex = slot;
            PlayerManager.SetPlayerCanSwitch(PlayerId.PlayerOne, false);
            PlayerManager.player1IsMugman = pkt.Player1IsMugman;
            PlayerData.inGame = false;

            var data = PlayerData.GetDataForSlot(slot);
            if (data != null)
            {
                data.isPlayer1Mugman = pkt.Player1IsMugman;
                data.CurrentMap = mapScene;

                var mapData = data.GetMapData(mapScene);
                if (mapData != null && pkt.IsEmpty)
                    mapData.sessionStarted = false;
            }

            Plugin.Log.LogInfo(
                "[SaveSync] Applied host save slot "
                + slot
                + " (map="
                + mapScene
                + ", empty="
                + pkt.IsEmpty
                + ", mugman="
                    + pkt.Player1IsMugman
                    + ").");

            SessionSync.ApplyRemoteSaveSelection(ref pkt);
        }
    }
}
