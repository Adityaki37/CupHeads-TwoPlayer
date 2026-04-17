namespace CupheadOnline.UI
{
    /// <summary>
    /// Stub — the join flow now uses the Steam overlay invite system.
    /// Lobby state is shown via MpMenuState.StatusText inside the native menu.
    /// This class is kept only to satisfy call-site references that may exist
    /// elsewhere; all methods are no-ops.
    /// </summary>
    public static class LobbyScreen
    {
        public static object Instance => null;   // always null — no overlay
        public static void Hide()            { }
        public static void ShowJoinDialog()  { }
    }
}
