using System.Collections.Generic;
using UnityEngine;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// Maps Unity instance IDs → DamageReceiver components for the active level's enemies.
    /// Used by EnemySyncManager to look up enemies by the numeric ID sent in EnemyStatePacket.
    ///
    /// Populated lazily: the first time an EnemyStatePacket arrives for an unknown ID,
    /// we scan all active DamageReceivers and rebuild the map.
    /// </summary>
    public static class EnemyRegistry
    {
        private static readonly Dictionary<int, DamageReceiver> _map
            = new Dictionary<int, DamageReceiver>(32);

        private static bool _dirty = true;

        public static void MarkDirty() => _dirty = true;
        public static void Clear()
        {
            _map.Clear();
            _dirty = true;
        }

        public static bool TryGet(int instanceId, out DamageReceiver dr)
        {
            if (_dirty) Rebuild();
            return _map.TryGetValue(instanceId, out dr);
        }

        static void Rebuild()
        {
            _map.Clear();
            foreach (var dr in Object.FindObjectsOfType<DamageReceiver>())
            {
                if (dr.type == DamageReceiver.Type.Enemy)
                    _map[dr.gameObject.GetInstanceID()] = dr;
            }
            _dirty = false;
        }
    }
}
