using System;
using System.Collections.Generic;
using System.Reflection;
using CupheadOnline.Net;
using HarmonyLib;
using UnityEngine;

namespace CupheadOnline.Sync
{
    public sealed class ExtraParticipantDeathBubbleTag : MonoBehaviour
    {
        public byte ParticipantId;
        public bool IsChalice;
        public bool IsMugman;
        public bool ReviveTriggered;
    }

    public static class ExtraParticipantReviveVisuals
    {
        static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        static readonly Dictionary<byte, PlayerDeathEffect> ActiveEffects =
            new Dictionary<byte, PlayerDeathEffect>(4);
        static readonly List<byte> ScratchParticipants =
            new List<byte>(6);
        static readonly List<byte> MissingParticipants =
            new List<byte>(6);

        static readonly FieldInfo CupheadField =
            typeof(PlayerDeathEffect).GetField("cuphead", AnyInstance);
        static readonly FieldInfo MugmanField =
            typeof(PlayerDeathEffect).GetField("mugman", AnyInstance);
        static readonly FieldInfo ChaliceField =
            typeof(PlayerDeathEffect).GetField("chalice", AnyInstance);
        static readonly FieldInfo ParrySwitchField =
            typeof(PlayerDeathEffect).GetField("parrySwitch", AnyInstance);
        static readonly FieldInfo EffectField =
            typeof(PlayerDeathEffect).GetField("effect", AnyInstance);
        static readonly FieldInfo ChaliceEffectField =
            typeof(PlayerDeathEffect).GetField("chaliceEffect", AnyInstance);
        static readonly FieldInfo SpriteRendererField =
            typeof(PlayerDeathEffect).GetField("spriteRenderer", AnyInstance);
        static readonly FieldInfo DeathEffectPlayerIdField =
            typeof(PlayerDeathEffect).GetField("playerId", AnyInstance);
        static readonly FieldInfo DeathEffectExitingField =
            typeof(PlayerDeathEffect).GetField("exiting", AnyInstance);

        static PlayerDeathEffect _prefab;

        static ExtraParticipantReviveVisuals()
        {
            MultiplayerSession.OnSessionEnded += Reset;
        }

        public static void Update()
        {
            if (!MultiplayerSession.IsActive)
                return;

            ScratchParticipants.Clear();
            ParticipantStatusTracker.AppendKnownParticipants(ScratchParticipants, includeBuiltIn: false);

            MissingParticipants.Clear();
            foreach (var kvp in ActiveEffects)
                MissingParticipants.Add(kvp.Key);

            for (int i = 0; i < ScratchParticipants.Count; i++)
            {
                byte participantId = ScratchParticipants[i];
                MissingParticipants.Remove(participantId);

                ParticipantStatusTracker.ParticipantStatus status;
                if (!ParticipantStatusTracker.TryGet(participantId, out status) || !status.IsKnown)
                {
                    RemoveEffect(participantId);
                    continue;
                }

                if (status.IsDead)
                    EnsureEffect(participantId, status);
                else
                    RemoveEffect(participantId);
            }

            for (int i = 0; i < MissingParticipants.Count; i++)
                RemoveEffect(MissingParticipants[i]);
        }

        public static void Reset()
        {
            var ids = new List<byte>(ActiveEffects.Keys);
            for (int i = 0; i < ids.Count; i++)
                RemoveEffect(ids[i]);

            ActiveEffects.Clear();
        }

        public static bool IsExtraBubble(PlayerDeathEffect effect, out ExtraParticipantDeathBubbleTag tag)
        {
            tag = effect == null ? null : effect.GetComponent<ExtraParticipantDeathBubbleTag>();
            return tag != null;
        }

        public static void OnEffectStarted(PlayerDeathEffect effect)
        {
            ExtraParticipantDeathBubbleTag tag;
            if (!IsExtraBubble(effect, out tag))
                return;

            effect.StopAllCoroutines();
        }

        public static bool HandleParrySwitch(PlayerDeathEffect effect)
        {
            ExtraParticipantDeathBubbleTag tag;
            if (!IsExtraBubble(effect, out tag))
                return false;
            if (tag.ReviveTriggered)
                return true;

            tag.ReviveTriggered = true;
            if (DeathEffectExitingField != null)
                DeathEffectExitingField.SetValue(effect, true);

            effect.StopAllCoroutines();

            var parrySwitch = ParrySwitchField == null ? null : ParrySwitchField.GetValue(effect) as PlayerDeathParrySwitch;
            if (parrySwitch != null && parrySwitch.gameObject != null)
                parrySwitch.gameObject.SetActive(false);

            AudioManager.Play("player_revive");
            AudioManager.Play(tag.IsChalice ? "player_revive_thank_you_chalice" : "player_revive_thank_you");
            if (effect.animator != null)
                effect.animator.SetTrigger("OnParry");

            if (Plugin.Net != null && Plugin.Net.IsConnected)
            {
                var request = new ReviveRequestPacket
                {
                    PosX = effect.transform.position.x,
                    PosY = effect.transform.position.y,
                    Tick = MultiplayerSession.Tick,
                };

                if (MultiplayerSession.IsHost)
                {
                    ParticipantReviveController.ResolveHostReviveRequest(
                        (byte)MultiplayerSession.LocalId,
                        effect.transform.position,
                        request.Tick,
                        Plugin.Net);
                }
                else
                {
                    Plugin.Net.SendReviveRequest(ref request);
                }
            }

            return true;
        }

        public static bool HandleParryAnimComplete(PlayerDeathEffect effect)
        {
            ExtraParticipantDeathBubbleTag tag;
            if (!IsExtraBubble(effect, out tag))
                return false;

            RemoveEffect(tag.ParticipantId);
            return true;
        }

        static void EnsureEffect(byte participantId, ParticipantStatusTracker.ParticipantStatus status)
        {
            PlayerDeathEffect existing;
            if (ActiveEffects.TryGetValue(participantId, out existing) && existing != null)
            {
                var existingTag = existing.GetComponent<ExtraParticipantDeathBubbleTag>();
                if (existingTag != null)
                {
                    existingTag.IsChalice = status.IsChalice;
                    existingTag.IsMugman = status.IsMugman;
                }
                return;
            }

            var prefab = GetPrefab();
            if (prefab == null)
                return;

            var effect = UnityEngine.Object.Instantiate(prefab);
            if (effect == null)
                return;

            effect.name = "NetworkParticipantDeath_" + participantId;
            effect.gameObject.hideFlags = HideFlags.HideAndDontSave;

            var tag = effect.gameObject.GetComponent<ExtraParticipantDeathBubbleTag>();
            if (tag == null)
                tag = effect.gameObject.AddComponent<ExtraParticipantDeathBubbleTag>();
            tag.ParticipantId = participantId;
            tag.IsChalice = status.IsChalice;
            tag.IsMugman = status.IsMugman;
            tag.ReviveTriggered = false;

            Vector2 position;
            if (!ParticipantStatusTracker.TryGetPosition(participantId, out position))
                position = Vector2.zero;

            effect.transform.position = position;
            if (DeathEffectPlayerIdField != null)
                DeathEffectPlayerIdField.SetValue(effect, PlayerId.PlayerOne);
            if (DeathEffectExitingField != null)
                DeathEffectExitingField.SetValue(effect, false);

            ConfigureAppearance(effect, status);

            if (effect.animator != null)
            {
                effect.animator.SetInteger("Mode", (int)PlayerMode.Level);
                effect.animator.SetBool("CanParry", true);
                effect.animator.Play("Level_Start", 0, 0f);
                effect.animator.Update(0f);
            }

            var parrySwitch = ParrySwitchField == null ? null : ParrySwitchField.GetValue(effect) as PlayerDeathParrySwitch;
            if (parrySwitch != null)
            {
                parrySwitch.enabled = true;
                if (parrySwitch.gameObject != null)
                    parrySwitch.gameObject.SetActive(true);
            }

            ActiveEffects[participantId] = effect;
        }

        static void ConfigureAppearance(PlayerDeathEffect effect, ParticipantStatusTracker.ParticipantStatus status)
        {
            var cuphead = CupheadField == null ? null : CupheadField.GetValue(effect) as SpriteRenderer;
            var mugman = MugmanField == null ? null : MugmanField.GetValue(effect) as SpriteRenderer;
            var chalice = ChaliceField == null ? null : ChaliceField.GetValue(effect) as SpriteRenderer;
            var effectRenderer = EffectField == null ? null : EffectField.GetValue(effect) as SpriteRenderer;
            var chaliceEffectRenderer = ChaliceEffectField == null ? null : ChaliceEffectField.GetValue(effect) as SpriteRenderer;

            if (cuphead != null) cuphead.gameObject.SetActive(false);
            if (mugman != null) mugman.gameObject.SetActive(false);
            if (chalice != null) chalice.gameObject.SetActive(false);

            SpriteRenderer active = cuphead;
            if (status.IsChalice)
                active = chalice ?? cuphead;
            else if (status.IsMugman)
                active = mugman ?? cuphead;

            if (active != null)
            {
                active.gameObject.SetActive(true);
                if (SpriteRendererField != null)
                    SpriteRendererField.SetValue(effect, active);
            }

            if (effectRenderer != null)
                effectRenderer.enabled = !status.IsChalice;
            if (chaliceEffectRenderer != null)
                chaliceEffectRenderer.enabled = status.IsChalice;
        }

        static PlayerDeathEffect GetPrefab()
        {
            if (_prefab != null)
                return _prefab;

            _prefab = Resources.Load<PlayerDeathEffect>("Player/Player_Death");
            if (_prefab != null)
                return _prefab;

            var local = MultiplayerSession.GetLocalController();
            if (local != null)
            {
                var fi = AccessTools.Field(typeof(LevelPlayerController), "deathEffect");
                if (fi != null)
                    _prefab = fi.GetValue(local) as PlayerDeathEffect;
            }

            if (_prefab == null)
                Plugin.Log.LogWarning("[ReviveVisuals] Could not resolve PlayerDeathEffect prefab.");

            return _prefab;
        }

        static void RemoveEffect(byte participantId)
        {
            PlayerDeathEffect effect;
            if (!ActiveEffects.TryGetValue(participantId, out effect))
                return;

            if (effect != null)
                UnityEngine.Object.Destroy(effect.gameObject);

            ActiveEffects.Remove(participantId);
        }
    }
}
