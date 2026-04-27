using System;
using System.Reflection;
using HarmonyLib;
using CupheadOnline.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CupheadOnline.Sync
{
    internal static class MapDialogueSync
    {
        static readonly MethodInfo StartSpeechBubbleMethod =
            AccessTools.Method(typeof(MapDialogueInteraction), "StartSpeechBubble");

        static bool _applyingRemote;
        static uint _tick;
        static int _remoteStartCount;
        static int _remoteContinueCount;
        static bool _localDialogueActive;
        static int _remoteAllowedStartDialogueId = -1;
        static float _remoteAllowedStartUntil = -1f;

        public static bool IsApplyingRemote => _applyingRemote;
        public static int RemoteStartCount => _remoteStartCount;
        public static int RemoteContinueCount => _remoteContinueCount;

        public static void Reset()
        {
            _applyingRemote = false;
            _tick = 0;
            _remoteStartCount = 0;
            _remoteContinueCount = 0;
            _localDialogueActive = false;
            _remoteAllowedStartDialogueId = -1;
            _remoteAllowedStartUntil = -1f;
        }

        public static bool ShouldBlockLocalClientMapDialogue()
        {
            return MultiplayerSession.IsActive
                && MultiplayerSession.IsClient
                && !_applyingRemote
                && IsMapScene();
        }

        public static void BroadcastStart(int dialogueId)
        {
            if (_localDialogueActive)
                return;

            Broadcast(MapDialogueAction.Start, dialogueId, 0, hostOnly: true);
            if (CanSendMapDialogue(hostOnly: true))
                _localDialogueActive = true;
        }

        public static void BroadcastContinue(int choice)
        {
            if (!_localDialogueActive)
                return;

            Broadcast(MapDialogueAction.Continue, -1, choice, hostOnly: false);
        }

        public static void BroadcastEnd()
        {
            Broadcast(MapDialogueAction.End, -1, 0, hostOnly: false);
            _localDialogueActive = false;
        }

        public static void Apply(MapDialoguePacket pkt)
        {
            if (!MultiplayerSession.IsActive)
                return;

            _applyingRemote = true;
            try
            {
                switch (pkt.Kind)
                {
                    case MapDialogueAction.Start:
                        if (MultiplayerSession.IsHost)
                            break;
                        if (pkt.DialogueId >= 0)
                        {
                            ApplyStart(pkt.DialogueId);
                            _remoteStartCount++;
                            Plugin.Log.LogInfo("[MapDialogueSync] Applied remote dialogue start " + pkt.DialogueId + ".");
                        }
                        break;
                    case MapDialogueAction.Continue:
                        Dialoguer.ContinueDialogue(pkt.Choice);
                        _remoteContinueCount++;
                        Plugin.Log.LogInfo("[MapDialogueSync] Applied remote dialogue continue choice " + pkt.Choice + ".");
                        break;
                    case MapDialogueAction.End:
                        Dialoguer.EndDialogue();
                        _localDialogueActive = false;
                        Plugin.Log.LogInfo("[MapDialogueSync] Applied remote dialogue end.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning("[MapDialogueSync] Could not apply remote dialogue packet: " + ex.Message);
            }
            finally
            {
                _applyingRemote = false;
            }
        }

        public static bool ShouldAllowDialoguerStart(int dialogueId)
        {
            if (_applyingRemote)
                return true;
            if (!MultiplayerSession.IsActive || !MultiplayerSession.IsClient)
                return false;
            if (_remoteAllowedStartDialogueId != dialogueId)
                return false;
            if (Time.unscaledTime > _remoteAllowedStartUntil)
                return false;

            _remoteAllowedStartDialogueId = -1;
            _remoteAllowedStartUntil = -1f;
            return true;
        }

        public static bool ShouldAllowLocalDialogueProgress()
        {
            if (!ShouldBlockLocalClientMapDialogue())
                return true;

            return _localDialogueActive;
        }

        static void ApplyStart(int dialogueId)
        {
            _localDialogueActive = true;
            Dialoguer.Initialize();
            var interaction = FindDialogueInteraction(dialogueId);
            if (interaction == null || StartSpeechBubbleMethod == null)
            {
                Dialoguer.StartDialogue(dialogueId);
                return;
            }

            _remoteAllowedStartDialogueId = dialogueId;
            _remoteAllowedStartUntil = Time.unscaledTime + 2f;
            StartSpeechBubbleMethod.Invoke(interaction, null);
        }

        static MapDialogueInteraction FindDialogueInteraction(int dialogueId)
        {
            var interactions = UnityEngine.Object.FindObjectsOfType<MapDialogueInteraction>();
            MapDialogueInteraction best = null;
            float bestDistance = float.MaxValue;
            Vector3 reference = Vector3.zero;
            if (Map.Current != null && Map.Current.players != null && Map.Current.players.Length > 0 && Map.Current.players[0] != null)
                reference = Map.Current.players[0].transform.position;

            for (int i = 0; i < interactions.Length; i++)
            {
                var interaction = interactions[i];
                if (interaction == null
                 || !interaction.isActiveAndEnabled
                 || !interaction.gameObject.activeInHierarchy
                 || (int)interaction.dialogueInteraction != dialogueId)
                {
                    continue;
                }

                float distance = Vector2.Distance(reference, interaction.transform.position);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    best = interaction;
                }
            }

            return best;
        }

        static void Broadcast(MapDialogueAction action, int dialogueId, int choice, bool hostOnly)
        {
            if (!CanSendMapDialogue(hostOnly))
                return;

            var pkt = new MapDialoguePacket
            {
                Action = (byte)action,
                DialogueId = dialogueId,
                Choice = choice,
                Tick = ++_tick,
            };
            Plugin.Net.SendMapDialogue(ref pkt);
            Plugin.Log.LogInfo("[MapDialogueSync] Broadcast " + action + " dialogue=" + dialogueId + " choice=" + choice + ".");
        }

        static bool CanSendMapDialogue(bool hostOnly)
        {
            return !_applyingRemote
                && MultiplayerSession.IsActive
                && (!hostOnly || MultiplayerSession.IsHost)
                && Plugin.Net != null
                && Plugin.Net.IsConnected
                && IsMapScene();
        }

        static bool IsMapScene()
        {
            try
            {
                var scene = SceneManager.GetActiveScene();
                return scene.IsValid()
                    && !string.IsNullOrEmpty(scene.name)
                    && scene.name.StartsWith("scene_map_world", StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }
    }
}
