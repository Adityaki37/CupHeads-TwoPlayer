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

        private static readonly Color GoodColour         = new Color(0.55f, 0.92f, 0.62f, 1f);
        private static readonly Color OkayColour         = new Color(0.95f, 0.85f, 0.40f, 1f);
        private static readonly Color PoorColour         = new Color(1.00f, 0.55f, 0.10f, 1f);
        private static readonly Color DisconnectedColour = new Color(0.90f, 0.20f, 0.15f, 1f);
        private static readonly Color TextColour         = new Color(0.96f, 0.91f, 0.77f, 1f);
        private static readonly Color BgGoodColour       = new Color(0.05f, 0.03f, 0.02f, 0.82f);
        private static readonly Color BgOkayColour       = new Color(0.18f, 0.12f, 0.03f, 0.86f);
        private static readonly Color BgPoorColour       = new Color(0.28f, 0.09f, 0.02f, 0.88f);

        private Text  _titleLabel;
        private Text  _pingLabel;
        private Text  _statusLabel;
        private Image _bgImage;

        // ──────────────────────────────────────────────────────────────────────
        //  Show / hide / update
        // ──────────────────────────────────────────────────────────────────────

        public static void Show()
        {
            if (!Plugin.ShowConnectionHud)
            {
                Hide();
                return;
            }
            if (Instance != null) return;
            var go = new GameObject("CupheadOnline_HUD");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<ConnectionHUD>();
        }

        public static void Show(string status)
        {
            if (!Plugin.ShowConnectionHud)
            {
                Hide();
                return;
            }
            Show();
            if (Instance != null)
                Instance.SetConnectedStatus(status);
        }

        public static void Hide()
        {
            if (Instance == null) return;
            Destroy(Instance.gameObject);
            Instance = null;
        }

        public static void UpdatePing(int ms)
        {
            if (!Plugin.ShowConnectionHud)
            {
                Hide();
                return;
            }
            Show();
            if (Instance != null)
                Instance.ApplyPing(ms);
        }

        public static void ShowDisconnected(string reason)
        {
            if (!Plugin.ShowConnectionHud)
            {
                Hide();
                return;
            }
            Show();
            if (Instance != null)
                Instance.ApplyDisconnected(reason);
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
            bgRT.sizeDelta = new Vector2(270f, 78f);
            _bgImage       = bg.AddComponent<Image>();
            _bgImage.color = BgGoodColour;

            _titleLabel = MakeLabel(bg, "CUPHEAD ONLINE", 13, OkayColour, new Vector2(0, 20), new Vector2(250f, 20f));

            _pingLabel   = MakeLabel(bg, "PING ---", 13, OkayColour, new Vector2(0, 0), new Vector2(250f, 20f));

            _statusLabel = MakeLabel(bg, "Waiting for peer...", 10, TextColour, new Vector2(0, -22), new Vector2(250f, 28f));
            _statusLabel.horizontalOverflow = HorizontalWrapMode.Wrap;
            _statusLabel.verticalOverflow = VerticalWrapMode.Overflow;
        }

        void OnDestroy() { if (Instance == this) Instance = null; }

        void SetConnectedStatus(string status)
        {
            if (_titleLabel != null) _titleLabel.color = OkayColour;
            if (_pingLabel != null && string.IsNullOrEmpty(_pingLabel.text))
                _pingLabel.text = "PING ---";
            if (_statusLabel != null)
            {
                _statusLabel.text = string.IsNullOrEmpty(status) ? "Steam P2P connected." : status;
                _statusLabel.color = TextColour;
            }
            if (_bgImage != null)
                _bgImage.color = BgGoodColour;
        }

        void ApplyPing(int ms)
        {
            string quality;
            Color accent;
            Color bg;

            if (ms < 80)
            {
                quality = "GOOD";
                accent = GoodColour;
                bg = BgGoodColour;
            }
            else if (ms < 150)
            {
                quality = "OKAY";
                accent = OkayColour;
                bg = BgOkayColour;
            }
            else
            {
                quality = "POOR";
                accent = PoorColour;
                bg = BgPoorColour;
            }

            if (_titleLabel != null) _titleLabel.color = accent;
            if (_pingLabel != null)
            {
                _pingLabel.text  = $"PING {ms}ms - {quality}";
                _pingLabel.color = accent;
            }
            if (_statusLabel != null && string.IsNullOrEmpty(_statusLabel.text))
                _statusLabel.text = "Steam P2P connected.";
            if (_bgImage != null)
                _bgImage.color = bg;
        }

        void ApplyDisconnected(string reason)
        {
            if (_titleLabel != null)
            {
                _titleLabel.text = "CUPHEAD ONLINE";
                _titleLabel.color = DisconnectedColour;
            }
            if (_pingLabel != null)
            {
                _pingLabel.text = "DISCONNECTED";
                _pingLabel.color = DisconnectedColour;
            }
            if (_statusLabel != null)
            {
                _statusLabel.text = string.IsNullOrEmpty(reason) ? "Connection closed." : reason;
                _statusLabel.color = new Color(1f, 0.78f, 0.72f, 1f);
            }
            if (_bgImage != null)
                _bgImage.color = new Color(0.30f, 0.04f, 0.02f, 0.90f);
        }

        static Text MakeLabel(GameObject parent, string text, int size, Color colour, Vector2 offset, Vector2 sizeDelta)
        {
            var go = new GameObject("L_" + text);
            go.transform.SetParent(parent.transform, false);
            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = offset;
            rt.sizeDelta = sizeDelta;
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
