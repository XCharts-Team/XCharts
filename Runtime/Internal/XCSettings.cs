using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XCharts.Runtime
{
    [Serializable]
#if UNITY_2018_3

    [ExcludeFromPresetAttribute]
#endif
    public class XCSettings : ScriptableObject
    {
        public readonly static string THEME_ASSET_NAME_PREFIX = "XCTheme-";
        public readonly static string THEME_ASSET_FOLDER = "Assets/XCharts/Resources";

        [SerializeField] private Lang m_Lang = null;
        [SerializeField] private Font m_Font = null;
#if dUI_TextMeshPro
        [SerializeField] private TMP_FontAsset m_TMPFont = null;
#endif
        [SerializeField][Range(1, 200)] private int m_FontSizeLv1 = 28;
        [SerializeField][Range(1, 200)] private int m_FontSizeLv2 = 24;
        [SerializeField][Range(1, 200)] private int m_FontSizeLv3 = 20;
        [SerializeField][Range(1, 200)] private int m_FontSizeLv4 = 18;
        [SerializeField] private LineStyle.Type m_AxisLineType = LineStyle.Type.Solid;
        [SerializeField][Range(0, 20)] private float m_AxisLineWidth = 0.8f;
        [SerializeField] private LineStyle.Type m_AxisSplitLineType = LineStyle.Type.Solid;
        [SerializeField][Range(0, 20)] private float m_AxisSplitLineWidth = 0.8f;
        [SerializeField][Range(0, 20)] private float m_AxisTickWidth = 0.8f;
        [SerializeField][Range(0, 20)] private float m_AxisTickLength = 5f;
        [SerializeField][Range(0, 200)] private float m_GaugeAxisLineWidth = 15f;
        [SerializeField][Range(0, 20)] private float m_GaugeAxisSplitLineWidth = 0.8f;
        [SerializeField][Range(0, 20)] private float m_GaugeAxisSplitLineLength = 15f;
        [SerializeField][Range(0, 20)] private float m_GaugeAxisTickWidth = 0.8f;
        [SerializeField][Range(0, 20)] private float m_GaugeAxisTickLength = 5f;
        [SerializeField][Range(0, 20)] private float m_TootipLineWidth = 0.8f;
        [SerializeField][Range(0, 20)] private float m_DataZoomBorderWidth = 0.5f;
        [SerializeField][Range(0, 20)] private float m_DataZoomDataLineWidth = 0.5f;
        [SerializeField][Range(0, 20)] private float m_VisualMapBorderWidth = 0f;

        [SerializeField][Range(0, 20)] private float m_SerieLineWidth = 1.8f;
        [SerializeField][Range(0, 200)] private float m_SerieLineSymbolSize = 5f;
        [SerializeField][Range(0, 200)] private float m_SerieScatterSymbolSize = 20f;
        [SerializeField][Range(0, 200)] private float m_SerieSelectedRate = 1.3f;
        [SerializeField][Range(0, 10)] private float m_SerieCandlestickBorderWidth = 1f;

        [SerializeField] private bool m_EditorShowAllListData = false;

        [SerializeField][Range(1, 20)] protected int m_MaxPainter = 10;
        [SerializeField][Range(1, 10)] protected float m_LineSmoothStyle = 3f;
        [SerializeField][Range(1f, 20)] protected float m_LineSmoothness = 2f;
        [SerializeField][Range(1f, 20)] protected float m_LineSegmentDistance = 3f;
        [SerializeField][Range(1, 10)] protected float m_CicleSmoothness = 2f;
        [SerializeField][Range(10, 50)] protected float m_VisualMapTriangeLen = 20f;
        [SerializeField] protected List<Theme> m_CustomThemes = new List<Theme>();

        public static Lang lang { get { return Instance.m_Lang; } }
        public static Font font { get { return Instance.m_Font; } }
#if dUI_TextMeshPro
        public static TMP_FontAsset tmpFont { get { return Instance.m_TMPFont; } }
#endif
        /// <summary>
        /// 一级字体大小。
        /// </summary>
        public static int fontSizeLv1 { get { return Instance.m_FontSizeLv1; } }
        public static int fontSizeLv2 { get { return Instance.m_FontSizeLv2; } }
        public static int fontSizeLv3 { get { return Instance.m_FontSizeLv3; } }
        public static int fontSizeLv4 { get { return Instance.m_FontSizeLv4; } }
        public static LineStyle.Type axisLineType { get { return Instance.m_AxisLineType; } }
        public static float axisLineWidth { get { return Instance.m_AxisLineWidth; } }
        public static LineStyle.Type axisSplitLineType { get { return Instance.m_AxisSplitLineType; } }
        public static float axisSplitLineWidth { get { return Instance.m_AxisSplitLineWidth; } }
        public static float axisTickWidth { get { return Instance.m_AxisTickWidth; } }
        public static float axisTickLength { get { return Instance.m_AxisTickLength; } }
        public static float gaugeAxisLineWidth { get { return Instance.m_GaugeAxisLineWidth; } }
        public static float gaugeAxisSplitLineWidth { get { return Instance.m_GaugeAxisSplitLineWidth; } }
        public static float gaugeAxisSplitLineLength { get { return Instance.m_GaugeAxisSplitLineLength; } }
        public static float gaugeAxisTickWidth { get { return Instance.m_GaugeAxisTickWidth; } }
        public static float gaugeAxisTickLength { get { return Instance.m_GaugeAxisTickLength; } }

        public static float tootipLineWidth { get { return Instance.m_TootipLineWidth; } }
        public static float dataZoomBorderWidth { get { return Instance.m_DataZoomBorderWidth; } }
        public static float dataZoomDataLineWidth { get { return Instance.m_DataZoomDataLineWidth; } }
        public static float visualMapBorderWidth { get { return Instance.m_VisualMapBorderWidth; } }

        #region serie
        public static float serieLineWidth { get { return Instance.m_SerieLineWidth; } }
        public static float serieLineSymbolSize { get { return Instance.m_SerieLineSymbolSize; } }
        public static float serieScatterSymbolSize { get { return Instance.m_SerieScatterSymbolSize; } }
        public static float serieSelectedRate { get { return Instance.m_SerieSelectedRate; } }
        public static float serieCandlestickBorderWidth { get { return Instance.m_SerieCandlestickBorderWidth; } }
        #endregion

        #region editor
        public static bool editorShowAllListData { get { return Instance.m_EditorShowAllListData; } }
        #endregion

        #region graphic
        public static int maxPainter { get { return Instance.m_MaxPainter; } }
        public static float lineSmoothStyle { get { return Instance.m_LineSmoothStyle; } }
        public static float lineSmoothness { get { return Instance.m_LineSmoothness; } }
        public static float lineSegmentDistance { get { return Instance.m_LineSegmentDistance; } }
        public static float cicleSmoothness { get { return Instance.m_CicleSmoothness; } }
        public static float visualMapTriangeLen { get { return Instance.m_VisualMapTriangeLen; } }
        #endregion

        public static List<Theme> customThemes { get { return Instance.m_CustomThemes; } }

        private static XCSettings s_Instance;
        public static XCSettings Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = Resources.Load<XCSettings>("XCSettings");
#if UNITY_EDITOR
                    if (s_Instance == null)
                    {
                        var assetPath = GetSettingAssetPath();
                        if (string.IsNullOrEmpty(assetPath))
                            XCResourceImporterWindow.ShowPackageImporterWindow();
                        else
                            s_Instance = AssetDatabase.LoadAssetAtPath<XCSettings>(assetPath);
                    }
                    else
                    {
                        if (s_Instance.m_Lang == null)
                            s_Instance.m_Lang = Resources.Load<Lang>("XCLang-EN");
                        if (s_Instance.m_Lang == null)
                            s_Instance.m_Lang = ScriptableObject.CreateInstance<Lang>();
                        if (s_Instance.m_Font == null)
                            s_Instance.m_Font = Resources.GetBuiltinResource<Font>("Arial.ttf");
#if dUI_TextMeshPro
                        if (s_Instance.m_TMPFont == null)
                            s_Instance.m_TMPFont = Resources.Load<TMP_FontAsset>("LiberationSans SDF");
#endif
                    }
#endif
                }
                return s_Instance;
            }
        }

#if UNITY_EDITOR
        public static bool ExistAssetFile()
        {
            return System.IO.File.Exists("Assets/XCharts/Resources/XCSettings.asset");
        }

        public static string GetSettingAssetPath()
        {
            var path = "Assets/XCharts/Resources/XCSettings.asset";
            if (File.Exists(path)) return path;
            var dir = Application.dataPath;
            string[] matchingPaths = Directory.GetDirectories(dir);
            foreach (var match in matchingPaths)
            {
                if (match.Contains("XCharts"))
                {
                    var jsonPath = string.Format("{0}/package.json", match);
                    if (File.Exists(jsonPath))
                    {
                        var jsonText = File.ReadAllText(jsonPath);
                        if (jsonText.Contains("\"displayName\": \"XCharts\""))
                        {
                            path = string.Format("{0}/Resources/XCSettings.asset", match.Replace('\\', '/'));
                            if (File.Exists(path))
                                return path.Substring(path.IndexOf("/Assets/") + 1);
                        }
                    }
                }
            }
            return null;
        }
#endif

        public static bool AddCustomTheme(Theme theme)
        {
            if (theme == null) return false;
            if (Instance == null || Instance.m_CustomThemes == null) return false;
            if (!Instance.m_CustomThemes.Contains(theme))
            {
                Instance.m_CustomThemes.Add(theme);
#if UNITY_EDITOR
                EditorUtility.SetDirty(Instance);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
#endif
                return true;
            }
            return false;
        }
    }
}