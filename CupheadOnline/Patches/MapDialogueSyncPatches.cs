using HarmonyLib;
using CupheadOnline.Sync;

namespace CupheadOnline.Patches
{
    [HarmonyPatch(typeof(Dialoguer), "StartDialogue", typeof(DialoguerDialogues))]
    public static class DialoguerStartEnumSyncPatch
    {
        static bool Prefix(DialoguerDialogues dialogue)
        {
            if (MapDialogueSync.ShouldAllowDialoguerStart((int)dialogue))
                return true;
            if (MapDialogueSync.ShouldBlockLocalClientMapDialogue())
                return false;

            MapDialogueSync.BroadcastStart((int)dialogue);
            return true;
        }
    }

    [HarmonyPatch(typeof(Dialoguer), "StartDialogue", typeof(int))]
    public static class DialoguerStartIntSyncPatch
    {
        static bool Prefix(int dialogueId)
        {
            if (MapDialogueSync.ShouldAllowDialoguerStart(dialogueId))
                return true;
            if (MapDialogueSync.ShouldBlockLocalClientMapDialogue())
                return false;

            MapDialogueSync.BroadcastStart(dialogueId);
            return true;
        }
    }

    [HarmonyPatch(typeof(Dialoguer), "StartDialogue", typeof(DialoguerDialogues), typeof(DialoguerCallback))]
    public static class DialoguerStartEnumCallbackSyncPatch
    {
        static bool Prefix(DialoguerDialogues dialogue)
        {
            if (MapDialogueSync.ShouldAllowDialoguerStart((int)dialogue))
                return true;
            if (MapDialogueSync.ShouldBlockLocalClientMapDialogue())
                return false;

            MapDialogueSync.BroadcastStart((int)dialogue);
            return true;
        }
    }

    [HarmonyPatch(typeof(Dialoguer), "StartDialogue", typeof(int), typeof(DialoguerCallback))]
    public static class DialoguerStartIntCallbackSyncPatch
    {
        static bool Prefix(int dialogueId)
        {
            if (MapDialogueSync.ShouldAllowDialoguerStart(dialogueId))
                return true;
            if (MapDialogueSync.ShouldBlockLocalClientMapDialogue())
                return false;

            MapDialogueSync.BroadcastStart(dialogueId);
            return true;
        }
    }

    [HarmonyPatch(typeof(Dialoguer), "ContinueDialogue", typeof(int))]
    public static class DialoguerContinueChoiceSyncPatch
    {
        static bool Prefix(int choice)
        {
            if (!MapDialogueSync.ShouldAllowLocalDialogueProgress())
                return false;

            MapDialogueSync.BroadcastContinue(choice);
            return true;
        }
    }

    [HarmonyPatch(typeof(Dialoguer), "ContinueDialogue", new System.Type[0])]
    public static class DialoguerContinueSyncPatch
    {
        static bool Prefix()
        {
            if (!MapDialogueSync.ShouldAllowLocalDialogueProgress())
                return false;

            MapDialogueSync.BroadcastContinue(0);
            return true;
        }
    }

    [HarmonyPatch(typeof(Dialoguer), "EndDialogue")]
    public static class DialoguerEndSyncPatch
    {
        static bool Prefix()
        {
            if (!MapDialogueSync.ShouldAllowLocalDialogueProgress())
                return false;

            MapDialogueSync.BroadcastEnd();
            return true;
        }
    }

    [HarmonyPatch(typeof(MapDialogueInteraction), "Activate", typeof(MapPlayerController))]
    public static class MapDialogueInteractionClientGuardPatch
    {
        static bool Prefix()
        {
            return !MapDialogueSync.ShouldBlockLocalClientMapDialogue();
        }
    }

    [HarmonyPatch(typeof(MapDialogueInteraction), "StartSpeechBubble")]
    public static class MapDialogueInteractionStartSpeechBubbleSyncPatch
    {
        static bool Prefix(MapDialogueInteraction __instance)
        {
            if (MapDialogueSync.ShouldBlockLocalClientMapDialogue())
                return false;

            if (__instance != null)
                MapDialogueSync.BroadcastStart((int)__instance.dialogueInteraction);
            return true;
        }
    }
}
