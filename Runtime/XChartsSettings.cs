/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using System;
using System.Collections.Generic;
#if dUI_TextMeshPro
using TMPro;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XCharts
{
    [Serializable]
#if UNITY_2018_3

    [ExcludeFromPresetAttribute]
#endif
    public class XChartsSettings : ScriptableObject
    {
        public readonly static string THEME_ASSET_NAME_PREFIX = "XTheme-";
        public readonly static string THEME_ASSET_FOLDER = "Assets/XCharts/Resources";

        [SerializeField] private Font m_Font = null;
#if dUI_TextMeshPro
        [SerializeField] private TMP_FontAsset m_TMPFont = null;
#endif
        [SerializeField] [Range(1, 200)] private int m_FontSizeLv1 = 28;
        [SerializeField] [Range(1, 200)] private int m_FontSizeLv2 = 24;
        [SerializeField] [Range(1, 200)] private int m_FontSizeLv3 = 20;
        [SerializeField] [Range(1, 200)] private int m_FontSizeLv4 = 18;
        [SerializeField] private LineStyle.Type m_AxisLineType = LineStyle.Type.Solid;
        [SerializeField] [Range(0, 20)] private float m_AxisLineWidth = 0.8f;
        [SerializeField] private LineStyle.Type m_AxisSplitLineType = LineStyle.Type.Solid;
        [SerializeField] [Range(0, 20)] private float m_AxisSplitLineWidth = 0.8f;
        [SerializeField] [Range(0, 20)] private float m_AxisTickWidth = 0.8f;
        [SerializeField] [Range(0, 20)] private float m_AxisTickLength = 5f;
        [SerializeField] [Range(0, 200)] private float m_GaugeAxisLineWidth = 15f;
        [SerializeField] [Range(0, 20)] private float m_GaugeAxisSplitLineWidth = 0.8f;
        [SerializeField] [Range(0, 20)] private float m_GaugeAxisSplitLineLength = 15f;
        [SerializeField] [Range(0, 20)] private float m_GaugeAxisTickWidth = 0.8f;
        [SerializeField] [Range(0, 20)] private float m_GaugeAxisTickLength = 5f;
        [SerializeField] [Range(0, 20)] private float m_TootipLineWidth = 0.8f;
        [SerializeField] [Range(0, 20)] private float m_DataZoomBorderWidth = 0.5f;
        [SerializeField] [Range(0, 20)] private float m_DataZoomDataLineWidth = 0.5f;
        [SerializeField] [Range(0, 20)] private float m_VisualMapBorderWidth = 0f;

        [SerializeField] [Range(0, 20)] private float m_SerieLineWidth = 1.4f;
        [SerializeField] [Range(0, 200)] private float m_SerieLineSymbolSize = 4f;
        [SerializeField] [Range(0, 200)] private float m_SerieLineSymbolSelectedSize = 8f;
        [SerializeField] [Range(0, 200)] private float m_SerieScatterSymbolSize = 20f;
        [SerializeField] [Range(0, 200)] private float m_SerieScatterSymbolSelectedSize = 30f;
        [SerializeField] [Range(0, 10)] private float m_SerieCandlestickBorderWidth = 1f;

        [SerializeField] private bool m_EditorBlockEnable = true;
        [SerializeField] private bool m_EditorShowAllListData = false;

        [SerializeField] [Range(1, 20)] protected int m_MaxPainter = 10;
        [SerializeField] [Range(1, 10)] protected float m_LineSmoothStyle = 3f;
        [SerializeField] [Range(1f, 20)] protected float m_LineSmoothness = 2f;
        [SerializeField] [Range(1f, 20)] protected float m_LineSegmentDistance = 3f;
        [SerializeField] [Range(1, 10)] protected float m_CicleSmoothness = 2f;
        [SerializeField] [Range(10, 50)] protected float m_VisualMapTriangeLen = 20f;
        [SerializeField] [Range(1, 20)] protected float m_PieTooltipExtraRadius = 8f;
        [SerializeField] [Range(1, 20)] protected float m_PieSelectedOffset = 8f;
        [SerializeField] protected List<TextAsset> m_CustomThemes = new List<TextAsset>();

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
        public static float serieLineSymbolSelectedSize { get { return Instance.m_SerieLineSymbolSelectedSize; } }
        public static float serieScatterSymbolSize { get { return Instance.m_SerieScatterSymbolSize; } }
        public static float serieScatterSymbolSelectedSize { get { return Instance.m_SerieScatterSymbolSelectedSize; } }
        public static float serieCandlestickBorderWidth { get { return Instance.m_SerieCandlestickBorderWidth; } }
        #endregion

        #region editor
        public static bool editorBlockEnable { get { return Instance.m_EditorBlockEnable; } }
        public static bool editorShowAllListData { get { return Instance.m_EditorShowAllListData; } }
        #endregion

        #region graphic
        public static int maxPainter { get { return Instance.m_MaxPainter; } }
        public static float lineSmoothStyle { get { return Instance.m_LineSmoothStyle; } }
        public static float lineSmoothness { get { return Instance.m_LineSmoothness; } }
        public static float lineSegmentDistance { get { return Instance.m_LineSegmentDistance; } }
        public static float cicleSmoothness { get { return Instance.m_CicleSmoothness; } }
        public static float visualMapTriangeLen { get { return Instance.m_VisualMapTriangeLen; } }
        public static float pieTooltipExtraRadius { get { return Instance.m_PieTooltipExtraRadius; } }
        public static float pieSelectedOffset { get { return Instance.m_PieSelectedOffset; } }
        #endregion

        public static List<TextAsset> customThemes { get { return Instance.m_CustomThemes; } }

        private static XChartsSettings s_Instance;
        public static XChartsSettings Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = Resources.Load<XChartsSettings>("XChartsSettings");
#if UNITY_EDITOR
                    if (s_Instance == null)
                    {
                        XChartsPackageResourceImporterWindow.ShowPackageImporterWindow();
                    }
                    else
                    {
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

        public static bool AddJsonTheme(TextAsset theme)
        {
            if (theme == null || string.IsNullOrEmpty(theme.text)) return false;
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