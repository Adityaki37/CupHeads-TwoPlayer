using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CupheadOnline.Sync
{
    /// <summary>
    /// Host-side approximation layer for extra gameplay participants whose
    /// avatars are visual-only and therefore do not collide with Cuphead's
    /// native enemy damage scripts.
    /// </summary>
    public static class ExtraParticipantDamageBridge
    {
        sealed class DamageSourceHandle
        {
            public int InstanceId;
            public MonoBehaviour Owner;
            public float Damage;
            public float Cooldown;
            public byte Source;
        }

        const float RescanInterval = 0.75f;
        const float MinDamageCooldown = 0.12f;
        const float PlayerBoundsPadding = 12f;
        const float SourceBoundsPadding = 8f;
        const float FallbackSourceSize = 24f;

        static readonly List<DamageSourceHandle> _sources =
            new List<DamageSourceHandle>(96);
        static readonly List<byte> _participantIds =
            new List<byte>(8);
        static readonly List<byte> _rawParticipantIds =
            new List<byte>(8);
        static readonly List<Bounds> _participantBounds =
            new List<Bounds>(8);
        static readonly List<long> _expiredKeys =
            new List<long>(32);
        static readonly Dictionary<long, float> _nextHitTimes =
            new Dictionary<long, float>(128);
        static readonly Dictionary<Type, MemberInfo> _dealerMembers =
            new Dictionary<Type, MemberInfo>(128);
        static readonly HashSet<Type> _typesWithoutDealer =
            new HashSet<Type>();

        static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        static readonly FieldInfo DamageDealerField =
            typeof(DamageDealer).GetField("damage", AnyInstance);
        static readonly FieldInfo DamageRateField =
            typeof(DamageDealer).GetField("damageRate", AnyInstance);
        static readonly FieldInfo DamageSourceField =
            typeof(DamageDealer).GetField("damageSource", AnyInstance);
        static readonly FieldInfo DamageTypesField =
            typeof(DamageDealer).GetField("damageTypes", AnyInstance);
        static readonly FieldInfo DamageTypesPlayerField =
            typeof(DamageDealer.DamageTypesManager).GetField("Player", AnyInstance);

        static float _nextRescanAt;

        static ExtraParticipantDamageBridge()
        {
            MultiplayerSession.OnSessionEnded += Reset;
        }

        public static void Update()
        {
            if (!MultiplayerSession.IsActive
             || !MultiplayerSession.IsHost
             || Plugin.Net == null
             || !Plugin.Net.IsConnected)
            {
                Reset();
                return;
            }

            if (PauseManager.state == PauseManager.State.Paused)
                return;

            float now = Time.unscaledTime;
            if (now >= _nextRescanAt)
            {
                RescanSources();
                _nextRescanAt = now + RescanInterval;
            }

            if (_sources.Count == 0)
                return;

            BuildParticipantBounds();
            if (_participantIds.Count == 0)
            {
                PruneExpiredCooldowns(now);
                return;
            }

            for (int i = _sources.Count - 1; i >= 0; i--)
            {
                var source = _sources[i];
                if (source == null || source.Owner == null || !source.Owner.isActiveAndEnabled)
                {
                    _sources.RemoveAt(i);
                    continue;
                }

                if (!TryGetSourceBounds(source.Owner, out var sourceBounds))
                    continue;

                sourceBounds.Expand(new Vector3(SourceBoundsPadding, SourceBoundsPadding, 0f));
                for (int p = 0; p < _participantIds.Count; p++)
                {
                    if (!_participantBounds[p].Intersects(sourceBounds))
                        continue;

                    byte participantId = _participantIds[p];
                    long key = MakeHitKey(source.InstanceId, participantId);
                    float nextAllowedAt;
                    if (_nextHitTimes.TryGetValue(key, out nextAllowedAt) && now < nextAllowedAt)
                        continue;

                    if (Plugin.Net.SendDamageEventForParticipant(
                        participantId,
                        source.Damage,
                        source.Source,
                        MultiplayerSession.Tick))
                    {
                        _nextHitTimes[key] = now + source.Cooldown;
                        Plugin.LogVerbose(
                            "[ExtraDamage] Bridged "
                            + source.Damage.ToString("0.##")
                            + " damage to participant #"
                            + participantId
                            + " from "
                            + source.Owner.GetType().Name
                            + ".");
                    }
                }
            }

            PruneExpiredCooldowns(now);
        }

        public static void Reset()
        {
            _sources.Clear();
            _participantIds.Clear();
            _rawParticipantIds.Clear();
            _participantBounds.Clear();
            _nextHitTimes.Clear();
            _expiredKeys.Clear();
            _nextRescanAt = 0f;
        }

        static void RescanSources()
        {
            _sources.Clear();

            var behaviours = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                var behaviour = behaviours[i];
                if (behaviour == null || !behaviour.isActiveAndEnabled)
                    continue;

                var go = behaviour.gameObject;
                if (go == null || !go.activeInHierarchy)
                    continue;
                if (go.CompareTag("Player") || go.CompareTag("PlayerProjectile"))
                    continue;

                DamageDealer dealer;
                if (!TryGetDamageDealer(behaviour, out dealer) || dealer == null)
                    continue;
                if (!CanDamagePlayers(dealer))
                    continue;

                float damage = GetDamage(dealer);
                if (damage <= 0f)
                    continue;

                _sources.Add(new DamageSourceHandle
                {
                    InstanceId = behaviour.GetInstanceID(),
                    Owner = behaviour,
                    Damage = damage,
                    Cooldown = GetDamageCooldown(dealer),
                    Source = GetDamageSource(dealer),
                });
            }
        }

        static void BuildParticipantBounds()
        {
            _participantIds.Clear();
            _rawParticipantIds.Clear();
            _participantBounds.Clear();

            ExtraRemoteAvatarManager.AppendParticipants(_rawParticipantIds);
            for (int i = 0; i < _rawParticipantIds.Count; i++)
            {
                Bounds bounds;
                if (!ExtraRemoteAvatarManager.TryGetHitbox(_rawParticipantIds[i], out bounds))
                    continue;

                bounds.Expand(new Vector3(PlayerBoundsPadding, PlayerBoundsPadding, 0f));
                _participantIds.Add(_rawParticipantIds[i]);
                _participantBounds.Add(bounds);
            }
        }

        static bool TryGetSourceBounds(MonoBehaviour owner, out Bounds bounds)
        {
            bounds = new Bounds();
            if (owner == null)
                return false;

            bool hasBounds = false;
            var colliders = owner.GetComponentsInChildren<Collider2D>(false);
            for (int i = 0; i < colliders.Length; i++)
            {
                var collider = colliders[i];
                if (collider == null || !collider.enabled)
                    continue;

                Bounds colliderBounds = collider.bounds;
                if (colliderBounds.size.sqrMagnitude <= 0.001f)
                    continue;

                if (!hasBounds)
                {
                    bounds = colliderBounds;
                    hasBounds = true;
                }
                else
                {
                    bounds.Encapsulate(colliderBounds);
                }
            }

            if (hasBounds)
                return true;

            var renderers = owner.GetComponentsInChildren<Renderer>(false);
            for (int i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null || !renderer.enabled)
                    continue;

                if (!hasBounds)
                {
                    bounds = renderer.bounds;
                    hasBounds = true;
                }
                else
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }

            if (hasBounds)
                return true;

            bounds = new Bounds(
                owner.transform.position,
                new Vector3(FallbackSourceSize, FallbackSourceSize, 1f));
            return true;
        }

        static bool TryGetDamageDealer(MonoBehaviour behaviour, out DamageDealer dealer)
        {
            dealer = null;
            if (behaviour == null)
                return false;

            var type = behaviour.GetType();
            MemberInfo member;
            if (_dealerMembers.TryGetValue(type, out member))
                return TryReadDealer(behaviour, member, out dealer);
            if (_typesWithoutDealer.Contains(type))
                return false;

            member = FindDamageDealerMember(type);
            if (member == null)
            {
                _typesWithoutDealer.Add(type);
                return false;
            }

            _dealerMembers[type] = member;
            return TryReadDealer(behaviour, member, out dealer);
        }

        static MemberInfo FindDamageDealerMember(Type type)
        {
            var namedField = type.GetField("damageDealer", AnyInstance);
            if (namedField != null && typeof(DamageDealer).IsAssignableFrom(namedField.FieldType))
                return namedField;

            var fields = type.GetFields(AnyInstance);
            for (int i = 0; i < fields.Length; i++)
            {
                if (typeof(DamageDealer).IsAssignableFrom(fields[i].FieldType))
                    return fields[i];
            }

            var namedProperty = type.GetProperty("damageDealer", AnyInstance);
            if (namedProperty != null && typeof(DamageDealer).IsAssignableFrom(namedProperty.PropertyType))
                return namedProperty;

            var properties = type.GetProperties(AnyInstance);
            for (int i = 0; i < properties.Length; i++)
            {
                if (typeof(DamageDealer).IsAssignableFrom(properties[i].PropertyType))
                    return properties[i];
            }

            return null;
        }

        static bool TryReadDealer(object owner, MemberInfo member, out DamageDealer dealer)
        {
            dealer = null;
            try
            {
                var field = member as FieldInfo;
                if (field != null)
                {
                    dealer = field.GetValue(owner) as DamageDealer;
                    return dealer != null;
                }

                var property = member as PropertyInfo;
                if (property != null)
                {
                    dealer = property.GetValue(owner, null) as DamageDealer;
                    return dealer != null;
                }
            }
            catch
            {
            }

            return false;
        }

        static bool CanDamagePlayers(DamageDealer dealer)
        {
            if (dealer == null || DamageTypesField == null || DamageTypesPlayerField == null)
                return false;

            try
            {
                var damageTypes = DamageTypesField.GetValue(dealer);
                if (damageTypes == null)
                    return false;

                var value = DamageTypesPlayerField.GetValue(damageTypes);
                return value is bool && (bool)value;
            }
            catch
            {
                return false;
            }
        }

        static float GetDamage(DamageDealer dealer)
        {
            if (dealer == null || DamageDealerField == null)
                return 0f;

            try
            {
                object raw = DamageDealerField.GetValue(dealer);
                if (!(raw is float))
                    return 0f;

                return Mathf.Max(0f, (float)raw * dealer.DamageMultiplier);
            }
            catch
            {
                return 0f;
            }
        }

        static float GetDamageCooldown(DamageDealer dealer)
        {
            if (dealer == null || DamageRateField == null)
                return MinDamageCooldown;

            try
            {
                object raw = DamageRateField.GetValue(dealer);
                if (raw is float)
                    return Mathf.Max(MinDamageCooldown, (float)raw);
            }
            catch
            {
            }

            return MinDamageCooldown;
        }

        static byte GetDamageSource(DamageDealer dealer)
        {
            if (dealer == null || DamageSourceField == null)
                return 0;

            try
            {
                object raw = DamageSourceField.GetValue(dealer);
                return Convert.ToByte(raw);
            }
            catch
            {
                return 0;
            }
        }

        static long MakeHitKey(int sourceInstanceId, byte participantId)
        {
            return ((long)sourceInstanceId << 8) | participantId;
        }

        static void PruneExpiredCooldowns(float now)
        {
            if (_nextHitTimes.Count == 0)
                return;

            _expiredKeys.Clear();
            foreach (var entry in _nextHitTimes)
            {
                if (entry.Value <= now)
                    _expiredKeys.Add(entry.Key);
            }

            for (int i = 0; i < _expiredKeys.Count; i++)
                _nextHitTimes.Remove(_expiredKeys[i]);
        }
    }
}
