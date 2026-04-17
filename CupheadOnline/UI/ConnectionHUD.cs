using UnityEngine;
using UnityEngine.UI;

namespace CupheadOnline.UI
{
    /// <summary>
    /// Minimal in-game overlay: connection status and ping.
    /// Top-right corner, standard UnityEngine.UI.Text (no TMPro required).
    /// </summary>
    public class ConnectionHUD : MonoBehaviour
    {
        public static ConnectionHUD Instance { get; private set; }

        private static readonly Color ActiveColour       = new Color(0.95f, 0.85f, 0.40f, 1f);
        private static readonly Color WarningColour      = new Color(1.00f, 0.55f, 0.10f, 1f);
        private static readonly Color DisconnectedColour = new Color(0.90f, 0.20f, 0.15f, 1f);
        private static readonly Color BgColour           = new Color(0.05f, 0.03f, 0.02f, 0.80f);

        private Text  _pingLabel;
        private Text  _statusLabel;
        private Image _bgImage;

        // ──────────────────────────────────────────────────────────────────────
        //  Show / hide / update
        // ──────────────────────────────────────────────────────────────────────

        public static void Show()
        {
            if (Instance != null) return;
            var go = new GameObject("CupheadOnline_HUD");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<ConnectionHUD>();
        }

        public static void Show(string status)
        {
            Show();
            if (Instance != null && Instance._statusLabel != null)
                Instance._statusLabel.text = status;
        }

        public static void Hide()
        {
            if (Instance == null) return;
            Destroy(Instance.gameObject);
            Instance = null;
        }

        public static void UpdatePing(int ms)
        {
            if (Instance == null) return;
            Instance._pingLabel.text  = $"{ms}ms";
            Instance._pingLabel.color = ms < 80 ? ActiveColour : WarningColour;
        }

        public static void ShowDisconnected(string reason)
        {
            if (Instance == null) return;
            Instance._statusLabel.text  = $"DISCONNECTED: {reason}";
            Instance._statusLabel.color = DisconnectedColour;
            Instance._bgImage.color     = new Color(0.3f, 0.04f, 0.02f, 0.90f);
        }

        // ──────────────────────────────────────────────────────────────────────
        //  Build
        // ──────────────────────────────────────────────────────────────────────

        void Awake()
        {
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode   = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 150;
            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode        = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            gameObject.AddComponent<GraphicRaycaster>();

            // Background pill — top-right corner
            var bg   = new GameObject("HUD_BG");
            bg.transform.SetParent(gameObject.transform, false);
            var bgRT = bg.AddComponent<RectTransform>();
            bgRT.anchorMin = bgRT.anchorMax = new Vector2(1f, 1f);
            bgRT.pivot     = new Vector2(1f, 1f);
            bgRT.anchoredPosition = new Vector2(-12f, -12f);
            bgRT.sizeDelta = new Vector2(200f, 60f);
            _bgImage       = bg.AddComponent<Image>();
            _bgImage.color = BgColour;

            // Role indicator
            MakeLabel(bg, "P1 --- P2", 13, ActiveColour, new Vector2(0, 12));

            // Ping
            _pingLabel   = MakeLabel(bg, "---ms", 13, ActiveColour, new Vector2(0, -6));

            // Status (shown on disconnect)
            _statusLabel = MakeLabel(bg, "", 10, ActiveColour, new Vector2(0, -22));
            _statusLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
        }

        void OnDestroy() { if (Instance == this) Instance = null; }

        static Text MakeLabel(GameObject parent, string text, int size, Color colour, Vector2 offset)
        {
            var go = new GameObject("L_" + text);
            go.transform.SetParent(parent.transform, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = offset;
            rt.sizeDelta = new Vector2(190f, 20f);
            var t = go.AddComponent<Text>();
            t.text      = text;
            t.fontSize  = size;
            t.color     = colour;
            t.alignment = TextAnchor.MiddleCenter;
            t.font      = Resources.GetBuiltinResource<Font>("Arial.ttf");
            return t;
        }
    }
}
