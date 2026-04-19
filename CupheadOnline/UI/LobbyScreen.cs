namespace CupheadOnline.UI
{
    /// <summary>
    /// Compatibility shim.
    /// The real lobby UI is injected into SlotSelectScreen via SlotSelectScreenPatch,
    /// but a few older call sites still reference LobbyScreen directly.
    /// </summary>
    public static class LobbyScreen
    {
        public static object Instance => null;
        public static void Hide()            { }
        public static void ShowJoinDialog()  { }
    }
}
