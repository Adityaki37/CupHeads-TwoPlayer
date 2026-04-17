using System.Reflection;
using UnityEngine;
using CupheadOnline.Net;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// HOST: broadcasts enemy / boss state at 20 Hz (every 3rd FixedUpdate at 60 Hz).
    /// CLIENT: receives EnemyStatePacket and snaps enemy positions + HP + animation.
    ///
    /// The boss AI continues to run independently on the client; we correct it every
    /// ~50 ms so minor divergences are continuously repaired without hard-snapping
    /// which would cause visual pops.
    ///
    /// HP reflection cache: The game does not expose a public HP getter on enemies.
    /// We locate the first 'float hp' (or 'HP', 'health') field on the DamageReceiver
    /// and use it.  If the field is not found, HP sync is silently skipped.
    /// </summary>
    public static class EnemySyncManager
    {
        private static int   _broadcastCounter;
        private const  int   BROADCAST_EVERY = 3; // frames (= 20 Hz at 60 Hz FixedUpdate)

        // Reflection cache for enemy HP field
        private static FieldInfo _hpField;
        private static bool      _hpFieldSearched;

        // ──────────────────────────────────────────────────────────────────────
        //  HOST side: called every FixedUpdate from Plugin.Update indirectly via
        //  a MonoBehaviour we attach, or from the PlayerMotorPatch tick.
        //  We call it from Plugin.Update to keep it off the physics thread.
        // ──────────────────────────────────────────────────────────────────────

        public static void HostTick()
        {
            if (!MultiplayerSession.IsHost || !Plugin.Net.IsConnected) return;

            _broadcastCounter++;
            if (_broadcastCounter < BROADCAST_EVERY) return;
            _broadcastCounter = 0;

            foreach (var dr in Object.FindObjectsOfType<DamageReceiver>())
            {
                if (dr.type != DamageReceiver.Type.Enemy) continue;
                var go = dr.gameObject;

                float hp   = GetEnemyHp(dr);
                byte  phase = GetEnemyPhase(dr);
                int   hash  = 0;
                var   anim  = go.GetComponentInChildren<Animator>();
                if (anim != null)
                    hash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;

                var pkt = new EnemyStatePacket
                {
                    InstanceId = go.GetInstanceID(),
                    PosX       = go.transform.position.x,
                    PosY       = go.transform.position.y,
                    Hp         = hp,
                    Phase      = phase,
                    AnimHash   = hash,
                    Tick       = MultiplayerSession.Tick,
                };
                Plugin.Net.SendEnemyState(ref pkt);
            }
        }

        // ──────────────────────────────────────────────────────────────────────
        //  CLIENT side: correct enemy state
        // ──────────────────────────────────────────────────────────────────────

        public static void OnEnemyStateReceived(EnemyStatePacket pkt)
        {
            if (!EnemyRegistry.TryGet(pkt.InstanceId, out var dr))
            {
                EnemyRegistry.MarkDirty(); // trigger a rescan next query
                if (!EnemyRegistry.TryGet(pkt.InstanceId, out dr)) return;
            }

            var go = dr.gameObject;

            // ── Position: gentle lerp to avoid visual snap ────────────────────
            go.transform.position = Vector3.Lerp(
                go.transform.position,
                new Vector3(pkt.PosX, pkt.PosY, go.transform.position.z),
                0.3f);

            // ── HP correction ─────────────────────────────────────────────────
            SetEnemyHp(dr, pkt.Hp);

            // ── Animation: play the host's animator state ─────────────────────
            var anim = go.GetComponentInChildren<Animator>();
            if (anim != null && pkt.AnimHash != 0)
            {
                // Only force the state if there's a significant divergence;
                // avoid overriding transition logic every single frame.
                int localHash = anim.GetCurrentAnimatorStateInfo(0).fullPathHash;
                if (localHash != pkt.AnimHash)
                    anim.Play(pkt.AnimHash, 0, -1f);
            }
        }

        public static void Reset()
        {
            _broadcastCounter = 0;
        }

        // ──────────────────────────────────────────────────────────────────────
        //  HP reflection
        // ──────────────────────────────────────────────────────────────────────

        static float GetEnemyHp(DamageReceiver dr)
        {
            var fi = FindHpField(dr);
            if (fi == null) return -1f;
            try { return (float)fi.GetValue(dr); }
            catch { return -1f; }
        }

        static void SetEnemyHp(DamageReceiver dr, float hp)
        {
            if (hp < 0f) return;
            var fi = FindHpField(dr);
            if (fi == null) return;
            try { fi.SetValue(dr, hp); }
            catch { /* field type mismatch — silently skip */ }
        }

        static FieldInfo FindHpField(DamageReceiver dr)
        {
            if (_hpFieldSearched) return _hpField;
            _hpFieldSearched = true;

            var t = dr.GetType();
            const BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            foreach (var name in new[] { "hp", "HP", "health", "_hp", "currentHp", "currentHealth" })
            {
                var fi = t.GetField(name, bf);
                if (fi != null && fi.FieldType == typeof(float))
                {
                    _hpField = fi;
                    return fi;
                }
            }
            Plugin.Log.LogWarning("[EnemySync] Could not find HP field on DamageReceiver — HP sync disabled.");
            return null;
        }

        static byte GetEnemyPhase(DamageReceiver dr)
        {
            // Try to read a common 'phase' or 'currentPhase' int field
            var t  = dr.GetType();
            const BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
            foreach (var name in new[] { "phase", "currentPhase", "_phase", "Phase" })
            {
                var fi = t.GetField(name, bf);
                if (fi != null && fi.FieldType == typeof(int))
                {
                    try { return (byte)(int)fi.GetValue(dr); }
                    catch { }
                }
            }
            return 0;
        }
    }
}
