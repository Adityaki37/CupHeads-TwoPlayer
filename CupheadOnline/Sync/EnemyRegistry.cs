using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// Maps stable enemy keys to DamageReceiver components for the active level.
    /// Unity instance IDs are process-local, so host/client processes need a key
    /// derived from the scene hierarchy instead.
    /// </summary>
    public static class EnemyRegistry
    {
        private static readonly Dictionary<int, DamageReceiver> _map
            = new Dictionary<int, DamageReceiver>(32);
        private static readonly Dictionary<DamageReceiver, int> _stableKeys
            = new Dictionary<DamageReceiver, int>(32);

        private static bool _dirty = true;

        public static void MarkDirty() => _dirty = true;

        public static void Clear()
        {
            _map.Clear();
            _stableKeys.Clear();
            _dirty = true;
        }

        public static bool TryGet(int enemyKey, out DamageReceiver dr)
        {
            if (_dirty)
                Rebuild();
            return _map.TryGetValue(enemyKey, out dr);
        }

        public static int GetStableKey(DamageReceiver dr)
        {
            if (dr == null)
                return 0;

            if (_dirty)
                Rebuild();

            int key;
            if (_stableKeys.TryGetValue(dr, out key))
                return key;

            key = BuildStableKey(dr);
            _stableKeys[dr] = key;
            if (!_map.ContainsKey(key))
                _map[key] = dr;
            return key;
        }

        static void Rebuild()
        {
            _map.Clear();
            _stableKeys.Clear();
            foreach (var dr in Object.FindObjectsOfType<DamageReceiver>())
            {
                if (dr == null || dr.type != DamageReceiver.Type.Enemy)
                    continue;

                int key = BuildUniqueStableKey(dr);
                _map[key] = dr;
                _stableKeys[dr] = key;

                // Keep the local instance ID as a fallback for same-process tests
                // and older packets that may still be in flight.
                _map[dr.gameObject.GetInstanceID()] = dr;
            }
            _dirty = false;
        }

        static int BuildUniqueStableKey(DamageReceiver dr)
        {
            int key = BuildStableKey(dr);
            int probe = key;
            int suffix = 1;
            while (_map.ContainsKey(probe))
            {
                probe = CombineHash(key, suffix);
                suffix++;
            }
            return probe;
        }

        static int BuildStableKey(DamageReceiver dr)
        {
            unchecked
            {
                int hash = 216613626;
                hash = AddString(hash, SceneManager.GetActiveScene().name);
                hash = AddString(hash, dr.GetType().FullName);

                var transform = dr.transform;
                var names = new List<string>(12);
                while (transform != null)
                {
                    names.Add(transform.name + "#" + transform.GetSiblingIndex());
                    transform = transform.parent;
                }

                for (int i = names.Count - 1; i >= 0; i--)
                    hash = AddString(hash, names[i]);

                return hash == 0 ? 1 : hash;
            }
        }

        static int AddString(int hash, string value)
        {
            unchecked
            {
                if (string.IsNullOrEmpty(value))
                    return CombineHash(hash, 0);

                for (int i = 0; i < value.Length; i++)
                {
                    hash ^= value[i];
                    hash *= 16777619;
                }
                return hash;
            }
        }

        static int CombineHash(int hash, int value)
        {
            unchecked
            {
                hash ^= value;
                hash *= 16777619;
                return hash == 0 ? 1 : hash;
            }
        }
    }
}
