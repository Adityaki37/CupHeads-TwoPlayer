using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace CupheadOnline.UI
{
    /// <summary>
    /// Optional battle overlay that reads Cuphead's boss property objects when
    /// available, then falls back to boss-like DamageReceiver HP fields.
    /// </summary>
    public sealed class BossHealthBarOverlay : MonoBehaviour
    {
        private sealed class BarView
        {
            public GameObject Root;
            public Text Name;
            public Text Value;
            public Image Fill;
            public Image LagFill;
            public float DisplayedRatio;
            public float LagRatio;
        }

        private struct BossSnapshot
        {
            public int Key;
            public string Name;
            public float Current;
            public float Total;
        }

        private sealed class PropertyHandle
        {
            public int Key;
            public object Owner;
            public string Name;
            public FieldInfo TotalField;
            public PropertyInfo CurrentProperty;
        }

        public static BossHealthBarOverlay Instance { get; private set; }

        private const int MaxBars = 3;
        private const float ScanInterval = 0.45f;
        private const float DefeatedHoldSeconds = 1.35f;
        private const float MinInterestingHealth = 8f;

        private static readonly BindingFlags AnyInstance =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        private static readonly Color Cream = new Color(0.96f, 0.90f, 0.72f, 1f);
        private static readonly Color MutedCream = new Color(0.84f, 0.75f, 0.55f, 0.95f);
        private static readonly Color Backing = new Color(0.08f, 0.045f, 0.025f, 0.78f);
        private static readonly Color BarBack = new Color(0.18f, 0.08f, 0.04f, 0.90f);
        private static readonly Color BarLag = new Color(0.95f, 0.58f, 0.28f, 0.58f);
        private static readonly Color BarFill = new Color(0.82f, 0.13f, 0.10f, 0.96f);
        private static readonly Color BarFillLow = new Color(0.98f, 0.80f, 0.28f, 0.98f);
        private static readonly Color Border = new Color(0.82f, 0.62f, 0.30f, 0.72f);

        private static readonly string[] BossKeywords =
        {
            "boss",
            "baroness",
            "dragon",
            "robot",
            "saltbaker",
            "dice",
            "devil",
            "pirate",
            "train",
            "genie",
            "clown",
            "flower",
            "blimp",
            "bee",
            "queen",
            "king",
            "knight",
            "rook",
            "bishop",
            "pawn",
            "moon",
            "mermaid",
            "briney",
            "howling",
            "glumstone",
            "mortimer",
            "esther",
            "chef",
            "salt",
        };

        private static readonly Dictionary<Type, FieldInfo[]> PropertyFieldCache =
            new Dictionary<Type, FieldInfo[]>(128);
        private static readonly Dictionary<Type, FieldInfo> HpFieldCache =
            new Dictionary<Type, FieldInfo>(64);
        private static readonly List<BossSnapshot> ScratchSnapshots =
            new List<BossSnapshot>(16);
        private static readonly List<PropertyHandle> ScratchProperties =
            new List<PropertyHandle>(16);
        private static readonly Dictionary<int, float> FallbackMaxHealth =
            new Dictionary<int, float>(32);
        private static readonly Dictionary<int, float> DefeatedUntil =
            new Dictionary<int, float>(32);

        private readonly BarView[] _bars = new BarView[MaxBars];
        private RectTransform _panel;
        private float _nextScanAt;

        public static void Tick()
        {
            if (!Plugin.ShowBossHealthBars || !IsBattleActive())
            {
                Hide();
                return;
            }

            Ensure();
            if (Instance != null)
                Instance.Refresh();
        }

        public static void Reset()
        {
            FallbackMaxHealth.Clear();
            DefeatedUntil.Clear();
            if (Instance != null)
                Instance._nextScanAt = 0f;
        }

        public static bool TryGetPrimaryBossHealth(out string name, out float current, out float total)
        {
            name = string.Empty;
            current = 0f;
            total = 0f;

            ScratchSnapshots.Clear();
            ScanBosses(ScratchSnapshots);
            if (ScratchSnapshots.Count == 0)
                return false;

            var snapshot = ScratchSnapshots[0];
            name = snapshot.Name;
            current = snapshot.Current;
            total = snapshot.Total;
            return true;
        }

        public static string GetDebugSummary()
        {
            ScratchSnapshots.Clear();
            ScanBosses(ScratchSnapshots);
            if (ScratchSnapshots.Count == 0)
                return "none";

            var parts = new string[Mathf.Min(ScratchSnapshots.Count, MaxBars)];
            for (int i = 0; i < parts.Length; i++)
            {
                var snapshot = ScratchSnapshots[i];
                parts[i] = snapshot.Name
                    + " "
                    + Mathf.CeilToInt(Mathf.Max(0f, snapshot.Current))
                    + "/"
                    + Mathf.CeilToInt(Mathf.Max(1f, snapshot.Total));
            }
            return string.Join(", ", parts);
        }

        public static void Hide()
        {
            if (Instance == null)
                return;

            Destroy(Instance.gameObject);
            Instance = null;
        }

        private static void Ensure()
        {
            if (Instance != null)
                return;

            var go = new GameObject("CupHeads_BossHealthBars");
            DontDestroyOnLoad(go);
            Instance = go.AddComponent<BossHealthBarOverlay>();
        }

        private void Awake()
        {
            var canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 138;

            var scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);

            gameObject.AddComponent<GraphicRaycaster>();

            var panelGo = new GameObject("BossHealthPanel");
            panelGo.transform.SetParent(transform, false);
            _panel = panelGo.AddComponent<RectTransform>();
            _panel.anchorMin = _panel.anchorMax = new Vector2(0.5f, 1f);
            _panel.pivot = new Vector2(0.5f, 1f);
            _panel.anchoredPosition = new Vector2(0f, -22f);
            _panel.sizeDelta = new Vector2(720f, 136f);

            for (int i = 0; i < _bars.Length; i++)
                _bars[i] = CreateBar(i);
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        private void Refresh()
        {
            if (Time.unscaledTime >= _nextScanAt)
            {
                ScanBosses(ScratchSnapshots);
                _nextScanAt = Time.unscaledTime + ScanInterval;
            }

            int count = Mathf.Min(MaxBars, ScratchSnapshots.Count);
            for (int i = 0; i < _bars.Length; i++)
            {
                if (i >= count)
                {
                    _bars[i].Root.SetActive(false);
                    continue;
                }

                ApplyBar(_bars[i], ScratchSnapshots[i]);
            }
        }

        private BarView CreateBar(int index)
        {
            var root = new GameObject("BossBar_" + index);
            root.transform.SetParent(_panel, false);
            var rootRt = root.AddComponent<RectTransform>();
            rootRt.anchorMin = rootRt.anchorMax = new Vector2(0.5f, 1f);
            rootRt.pivot = new Vector2(0.5f, 1f);
            rootRt.anchoredPosition = new Vector2(0f, -index * 42f);
            rootRt.sizeDelta = new Vector2(700f, 34f);

            var bg = root.AddComponent<Image>();
            bg.color = Backing;
            var outline = root.AddComponent<Outline>();
            outline.effectColor = Border;
            outline.effectDistance = new Vector2(1f, -1f);

            var barBgGo = new GameObject("Track");
            barBgGo.transform.SetParent(root.transform, false);
            var barBgRt = barBgGo.AddComponent<RectTransform>();
            barBgRt.anchorMin = barBgRt.anchorMax = new Vector2(0.5f, 0.5f);
            barBgRt.pivot = new Vector2(0.5f, 0.5f);
            barBgRt.anchoredPosition = new Vector2(0f, -6f);
            barBgRt.sizeDelta = new Vector2(664f, 12f);
            var barBg = barBgGo.AddComponent<Image>();
            barBg.color = BarBack;

            var lagGo = new GameObject("LagFill");
            lagGo.transform.SetParent(barBgGo.transform, false);
            var lagRt = lagGo.AddComponent<RectTransform>();
            lagRt.anchorMin = new Vector2(0f, 0f);
            lagRt.anchorMax = new Vector2(1f, 1f);
            lagRt.pivot = new Vector2(0f, 0.5f);
            lagRt.offsetMin = Vector2.zero;
            lagRt.offsetMax = Vector2.zero;
            var lagFill = lagGo.AddComponent<Image>();
            lagFill.type = Image.Type.Filled;
            lagFill.fillMethod = Image.FillMethod.Horizontal;
            lagFill.fillOrigin = 0;
            lagFill.color = BarLag;

            var fillGo = new GameObject("Fill");
            fillGo.transform.SetParent(barBgGo.transform, false);
            var fillRt = fillGo.AddComponent<RectTransform>();
            fillRt.anchorMin = new Vector2(0f, 0f);
            fillRt.anchorMax = new Vector2(1f, 1f);
            fillRt.pivot = new Vector2(0f, 0.5f);
            fillRt.offsetMin = Vector2.zero;
            fillRt.offsetMax = Vector2.zero;
            var fill = fillGo.AddComponent<Image>();
            fill.type = Image.Type.Filled;
            fill.fillMethod = Image.FillMethod.Horizontal;
            fill.fillOrigin = 0;
            fill.color = BarFill;

            var name = MakeText(root, "Name", "BOSS", 13, Cream, new Vector2(-326f, 8f), new Vector2(450f, 20f), TextAnchor.MiddleLeft);
            var value = MakeText(root, "Value", "", 11, MutedCream, new Vector2(266f, 8f), new Vector2(130f, 20f), TextAnchor.MiddleRight);

            root.SetActive(false);
            return new BarView
            {
                Root = root,
                Name = name,
                Value = value,
                Fill = fill,
                LagFill = lagFill,
                DisplayedRatio = 1f,
                LagRatio = 1f,
            };
        }

        private static Text MakeText(
            GameObject parent,
            string name,
            string content,
            int size,
            Color color,
            Vector2 position,
            Vector2 sizeDelta,
            TextAnchor anchor)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent.transform, false);

            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = position;
            rt.sizeDelta = sizeDelta;

            var text = go.AddComponent<Text>();
            text.text = content;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = size;
            text.color = color;
            text.alignment = anchor;
            text.horizontalOverflow = HorizontalWrapMode.Overflow;
            text.verticalOverflow = VerticalWrapMode.Truncate;
            return text;
        }

        private static void ApplyBar(BarView bar, BossSnapshot snapshot)
        {
            float ratio = snapshot.Total <= 0f ? 0f : Mathf.Clamp01(snapshot.Current / snapshot.Total);
            if (!bar.Root.activeSelf)
            {
                bar.DisplayedRatio = ratio;
                bar.LagRatio = ratio;
                bar.Root.SetActive(true);
            }

            bar.DisplayedRatio = Mathf.Lerp(bar.DisplayedRatio, ratio, Mathf.Min(1f, 18f * Time.unscaledDeltaTime));
            bar.LagRatio = Mathf.Lerp(bar.LagRatio, ratio, Mathf.Min(1f, 5f * Time.unscaledDeltaTime));
            if (ratio > bar.LagRatio)
                bar.LagRatio = ratio;

            bar.Name.text = snapshot.Name;
            bar.Value.text = Mathf.CeilToInt(Mathf.Max(0f, snapshot.Current)).ToString()
                + " / "
                + Mathf.CeilToInt(Mathf.Max(1f, snapshot.Total)).ToString();
            bar.Fill.fillAmount = bar.DisplayedRatio;
            bar.LagFill.fillAmount = Mathf.Max(bar.DisplayedRatio, bar.LagRatio);
            bar.Fill.color = ratio <= 0.25f ? BarFillLow : BarFill;
        }

        private static void ScanBosses(List<BossSnapshot> target)
        {
            target.Clear();
            ScanPropertyBosses(target);
            if (target.Count == 0)
                ScanDamageReceiverBosses(target);

            target.Sort(CompareBossSnapshots);
        }

        private static void ScanPropertyBosses(List<BossSnapshot> target)
        {
            ScratchProperties.Clear();

            var behaviours = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();
            for (int i = 0; i < behaviours.Length; i++)
            {
                var behaviour = behaviours[i];
                if (behaviour == null || behaviour.gameObject == null || !behaviour.gameObject.activeInHierarchy)
                    continue;

                var fields = GetPropertyFields(behaviour.GetType());
                for (int f = 0; f < fields.Length; f++)
                {
                    object properties = null;
                    try { properties = fields[f].GetValue(behaviour); }
                    catch { }
                    if (properties == null)
                        continue;

                    var handle = BuildPropertyHandle(behaviour, fields[f], properties);
                    if (handle == null)
                        continue;

                    BossSnapshot snapshot;
                    if (!TryReadPropertySnapshot(handle, out snapshot))
                        continue;
                    if (!ShouldShowSnapshot(snapshot, handle.Name))
                        continue;

                    target.Add(snapshot);
                }
            }
        }

        private static void ScanDamageReceiverBosses(List<BossSnapshot> target)
        {
            var receivers = UnityEngine.Object.FindObjectsOfType<DamageReceiver>();
            for (int i = 0; i < receivers.Length; i++)
            {
                var receiver = receivers[i];
                if (receiver == null
                 || receiver.type != DamageReceiver.Type.Enemy
                 || receiver.gameObject == null
                 || !receiver.gameObject.activeInHierarchy)
                {
                    continue;
                }

                if (!IsBossLike(receiver.gameObject.name))
                    continue;

                FieldInfo hpField = FindHpField(receiver.GetType());
                if (hpField == null)
                    continue;

                float hp;
                try { hp = (float)hpField.GetValue(receiver); }
                catch { continue; }

                int key = receiver.gameObject.GetInstanceID();
                float max;
                if (!FallbackMaxHealth.TryGetValue(key, out max) || hp > max)
                {
                    max = Mathf.Max(1f, hp);
                    FallbackMaxHealth[key] = max;
                }

                var snapshot = new BossSnapshot
                {
                    Key = key,
                    Name = CleanName(receiver.gameObject.name),
                    Current = Mathf.Max(0f, hp),
                    Total = Mathf.Max(1f, max),
                };

                if (ShouldShowSnapshot(snapshot, snapshot.Name))
                    target.Add(snapshot);
            }
        }

        private static PropertyHandle BuildPropertyHandle(MonoBehaviour owner, FieldInfo field, object properties)
        {
            Type type = properties.GetType();
            var totalField = type.GetField("TotalHealth", AnyInstance);
            var currentProperty = type.GetProperty("CurrentHealth", AnyInstance);
            if (totalField == null
             || totalField.FieldType != typeof(float)
             || currentProperty == null
             || currentProperty.PropertyType != typeof(float)
             || !currentProperty.CanRead)
            {
                return null;
            }

            string descriptor = (owner.GetType().FullName ?? string.Empty)
                + " "
                + owner.gameObject.name
                + " "
                + field.Name
                + " "
                + (type.FullName ?? string.Empty);

            float total;
            try { total = (float)totalField.GetValue(properties); }
            catch { return null; }

            if (total < MinInterestingHealth && !IsBossLike(descriptor))
                return null;

            return new PropertyHandle
            {
                Key = RuntimeHelpers.GetHashCode(properties),
                Owner = properties,
                Name = BuildDisplayName(owner, field, type),
                TotalField = totalField,
                CurrentProperty = currentProperty,
            };
        }

        private static bool TryReadPropertySnapshot(PropertyHandle handle, out BossSnapshot snapshot)
        {
            snapshot = new BossSnapshot();
            try
            {
                float total = (float)handle.TotalField.GetValue(handle.Owner);
                float current = (float)handle.CurrentProperty.GetValue(handle.Owner, null);
                if (total <= 0f)
                    return false;

                snapshot = new BossSnapshot
                {
                    Key = handle.Key,
                    Name = handle.Name,
                    Current = Mathf.Max(0f, current),
                    Total = Mathf.Max(1f, total),
                };
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool ShouldShowSnapshot(BossSnapshot snapshot, string descriptor)
        {
            if (snapshot.Total <= 0f)
                return false;

            if (snapshot.Current > 0f)
            {
                if (DefeatedUntil.ContainsKey(snapshot.Key))
                    DefeatedUntil.Remove(snapshot.Key);
                return snapshot.Total >= MinInterestingHealth || IsBossLike(descriptor);
            }

            float until;
            if (!DefeatedUntil.TryGetValue(snapshot.Key, out until))
            {
                until = Time.unscaledTime + DefeatedHoldSeconds;
                DefeatedUntil[snapshot.Key] = until;
            }

            return Time.unscaledTime <= until;
        }

        private static FieldInfo[] GetPropertyFields(Type type)
        {
            FieldInfo[] cached;
            if (PropertyFieldCache.TryGetValue(type, out cached))
                return cached;

            var found = new List<FieldInfo>();
            var fields = type.GetFields(AnyInstance);
            for (int i = 0; i < fields.Length; i++)
            {
                var field = fields[i];
                if (field != null && IsLevelPropertiesType(field.FieldType))
                    found.Add(field);
            }

            cached = found.ToArray();
            PropertyFieldCache[type] = cached;
            return cached;
        }

        private static bool IsLevelPropertiesType(Type type)
        {
            while (type != null)
            {
                if (type.IsGenericType
                 && type.GetGenericTypeDefinition() == typeof(AbstractLevelProperties<,,>))
                {
                    return true;
                }

                type = type.BaseType;
            }

            return false;
        }

        private static FieldInfo FindHpField(Type type)
        {
            FieldInfo cached;
            if (HpFieldCache.TryGetValue(type, out cached))
                return cached;

            string[] names = { "hp", "HP", "health", "_hp", "currentHp", "currentHealth" };
            for (int i = 0; i < names.Length; i++)
            {
                var field = type.GetField(names[i], AnyInstance);
                if (field != null && field.FieldType == typeof(float))
                {
                    HpFieldCache[type] = field;
                    return field;
                }
            }

            HpFieldCache[type] = null;
            return null;
        }

        private static bool IsBattleActive()
        {
            try
            {
                return Level.Current != null && Level.Current.LevelType == Level.Type.Battle;
            }
            catch
            {
                return false;
            }
        }

        private static int CompareBossSnapshots(BossSnapshot left, BossSnapshot right)
        {
            int total = right.Total.CompareTo(left.Total);
            if (total != 0)
                return total;
            return string.Compare(left.Name, right.Name, StringComparison.OrdinalIgnoreCase);
        }

        private static string BuildDisplayName(MonoBehaviour owner, FieldInfo field, Type propertyType)
        {
            string candidate = field.Name;
            if (string.Equals(candidate, "properties", StringComparison.OrdinalIgnoreCase)
             || string.Equals(candidate, "_properties", StringComparison.OrdinalIgnoreCase)
             || candidate.Length <= 2)
            {
                candidate = owner.gameObject != null ? owner.gameObject.name : owner.GetType().Name;
            }

            if (string.IsNullOrEmpty(candidate) || candidate == "GameObject")
                candidate = propertyType.Name;

            string cleaned = CleanName(candidate);
            if (IsGenericBossDisplayName(cleaned))
                return CurrentLevelDisplayName(cleaned);

            return cleaned;
        }

        private static string CleanName(string value)
        {
            if (string.IsNullOrEmpty(value))
                return "BOSS";

            string text = value.Replace("(Clone)", string.Empty)
                .Replace("_", " ")
                .Replace("-", " ")
                .Trim();

            while (text.Contains("  "))
                text = text.Replace("  ", " ");

            if (text.Length > 34)
                text = text.Substring(0, 31) + "...";

            return text.ToUpperInvariant();
        }

        private static string CurrentLevelDisplayName(string fallback)
        {
            try
            {
                if (Level.Current != null)
                {
                    string levelName = CleanLevelName(Level.Current.CurrentLevel.ToString(), fallback);
                    if (!IsGenericBossDisplayName(levelName))
                        return levelName;
                }
            }
            catch
            {
            }

            return fallback;
        }

        private static string CleanLevelName(string value, string fallback)
        {
            if (string.IsNullOrEmpty(value))
                return fallback;

            string text = value.Replace("level_", string.Empty)
                .Replace("scene_", string.Empty)
                .Replace("_", " ")
                .Trim();

            while (text.Contains("  "))
                text = text.Replace("  ", " ");

            if (string.IsNullOrEmpty(text))
                return fallback;

            if (text.Length > 34)
                text = text.Substring(0, 31) + "...";

            return text.ToUpperInvariant();
        }

        private static bool IsGenericBossDisplayName(string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;

            string text = value.Trim().ToUpperInvariant();
            return text == "BOSS"
                || text == "LEVEL"
                || text == "GAMEOBJECT"
                || text == "PROPERTIES"
                || text == "PROPERTY";
        }

        private static bool IsBossLike(string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            string name = value.ToLowerInvariant();
            for (int i = 0; i < BossKeywords.Length; i++)
            {
                if (name.Contains(BossKeywords[i]))
                    return true;
            }

            return false;
        }
    }
}
