using System;
using System.IO;
using System.Reflection;
using HarmonyLib;

namespace CupheadOnline.Patches
{
    /// <summary>
    /// Debug/test-copy only save redirection so automated new-save flows do not
    /// touch the user's normal Cuphead save folder.
    /// </summary>
    [HarmonyPatch]
    internal static class CloudSavePathPatch
    {
        static MethodBase TargetMethod()
        {
            return AccessTools.PropertyGetter(typeof(OnlineInterfaceSteam), "SavePath");
        }

        static void Postfix(ref string __result)
        {
            if (!Plugin.UseSeparateSavePath)
                return;

            string path = Plugin.SeparateSavePath;
            try
            {
                path = Path.GetFullPath(path);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                __result = path;
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning("[CloudSavePath] Could not use separate save path '" + path + "': " + ex.Message);
            }
        }
    }
}
