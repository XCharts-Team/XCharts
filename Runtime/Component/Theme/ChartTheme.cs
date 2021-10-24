/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts
{
    /// <summary>
    /// 主题
    /// </summary>
    public enum Theme
    {
        /// <summary>
        /// 默认主题。
        /// </summary>
        Default,
        /// <summary>
        /// 亮主题。
        /// </summary>
        Light,
        /// <summary>
        /// 暗主题。
        /// </summary>
        Dark,
        /// <summary>
        /// 自定义主题。
        /// </summary>
        Custom,
    }

    [Serializable]
    /// <summary>
    /// Theme.
    /// 主题相关配置。
    /// </summary>
    public class ChartTheme : MainComponent
    {
        [SerializeField] private Theme m_Theme = Theme.Default;
        [SerializeField] private string m_ThemeName = Theme.Default.ToString();
        [SerializeField] private string m_FontName;
        [SerializeField] private string m_TMPFontName;
        [SerializeField] private int m_FontInstacneId;
        [SerializeField] private int m_TMPFontInstanceId;
        [SerializeField] private Font m_Font;
#if dUI_TextMeshPro
        [SerializeField] private TMP_FontAsset m_TMPFont;
#endif

        [SerializeField] private Color32 m_ContrastColor;
        [SerializeField] private Color32 m_BackgroundColor;

#if UNITY_2020_2
        [NonReorderable]
#endif
        [SerializeField] private List<Color32> m_ColorPalette = new List<Color32>(13);

        [SerializeField] private ComponentTheme m_Common;
        [SerializeField] private TitleTheme m_Title;
        [SerializeField] private SubTitleTheme m_SubTitle;
        [SerializeField] private LegendTheme m_Legend;
        [SerializeField] private AxisTheme m_Axis;
        [SerializeField] private RadiusAxisTheme m_RadiusAxis;
        [SerializeField] private AngleAxisTheme m_AngleAxis;
        [SerializeField] private PolarAxisTheme m_Polar;
        [SerializeField] private GaugeAxisTheme m_Gauge;
        [SerializeField] private RadarAxisTheme m_Radar;
        [SerializeField] private TooltipTheme m_Tooltip;
        [SerializeField] private DataZoomTheme m_DataZoom;
        [SerializeField] private VisualMapTheme m_VisualMap;
        [SerializeField] private SerieTheme m_Serie;

        private ChartTheme()
        {
            m_FontName = "Arial";
            m_TMPFontName = "LiberationSans SDF";
        }

        /// <summary>
        /// the theme of chart.
        /// 主题类型。
        /// </summary>
        public Theme theme
        {
            get { return m_Theme; }
            set { if (PropertyUtil.SetStruct(ref m_Theme, value)) SetComponentDirty(); }
        }
        public string themeName
        {
            get { return m_ThemeName; }
            set { if (PropertyUtil.SetClass(ref m_ThemeName, value)) SetComponentDirty(); }
        }


        /// <summary>
        /// the contrast color of chart.
        /// 对比色。
        /// </summary>
        public Color32 contrastColor
        {
            get { return m_ContrastColor; }
            set { if (PropertyUtil.SetColor(ref m_ContrastColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the background color of chart.
        /// 背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }

        public string fontName
        {
            get { return m_FontName; }
            set { if (PropertyUtil.SetClass(ref m_FontName, value)) SetComponentDirty(); }
        }
        public int fontInstanceId
        {
            get { return m_FontInstacneId; }
            set { if (PropertyUtil.SetStruct(ref m_FontInstacneId, value)) SetComponentDirty(); }
        }
        public string tmpFontName
        {
            get { return m_TMPFontName; }
            set { if (PropertyUtil.SetClass(ref m_TMPFontName, value)) SetComponentDirty(); }
        }
        public int tmpFontInstanceId
        {
            get { return m_TMPFontInstanceId; }
            set { if (PropertyUtil.SetStruct(ref m_TMPFontInstanceId, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.
        /// 调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。
        /// </summary>
        public List<Color32> colorPalette { get { return m_ColorPalette; } set { m_ColorPalette = value; SetVerticesDirty(); } }
        public ComponentTheme common { get { return m_Common; } set { m_Common = value; SetComponentDirty(); } }
        public TitleTheme title { get { return m_Title; } set { m_Title = value; SetComponentDirty(); } }
        public SubTitleTheme subTitle { get { return m_SubTitle; } set { m_SubTitle = value; SetComponentDirty(); } }
        public LegendTheme legend { get { return m_Legend; } set { m_Legend = value; SetComponentDirty(); } }
        public AxisTheme axis { get { return m_Axis; } set { m_Axis = value; SetAllDirty(); } }
        public RadiusAxisTheme radiusAxis { get { return m_RadiusAxis; } set { m_RadiusAxis = value; SetAllDirty(); } }
        public AngleAxisTheme angleAxis { get { return m_AngleAxis; } set { m_AngleAxis = value; SetAllDirty(); } }
        public PolarAxisTheme polar { get { return m_Polar; } set { m_Polar = value; SetAllDirty(); } }
        public GaugeAxisTheme gauge { get { return m_Gauge; } set { m_Gauge = value; SetAllDirty(); } }
        public RadarAxisTheme radar { get { return m_Radar; } set { m_Radar = value; SetAllDirty(); } }
        public TooltipTheme tooltip { get { return m_Tooltip; } set { m_Tooltip = value; SetAllDirty(); } }
        public DataZoomTheme dataZoom { get { return m_DataZoom; } set { m_DataZoom = value; SetAllDirty(); } }
        public VisualMapTheme visualMap { get { return m_VisualMap; } set { m_VisualMap = value; SetAllDirty(); } }
        public SerieTheme serie { get { return m_Serie; } set { m_Serie = value; SetVerticesDirty(); } }
#if dUI_TextMeshPro
        /// <summary>
        /// the font of chart text。
        /// 字体。
        /// </summary>
        public TMP_FontAsset tmpFont
        {
            get { return m_TMPFont; }
            set
            {
                m_TMPFont = value;
                if(value)
                {
                    m_TMPFontName = value.name;
                    m_TMPFontInstanceId = value.GetInstanceID();
                }
                SetComponentDirty();
                SyncTMPFontToSubComponent();
            }
        }
#endif
        /// <summary>
        /// the font of chart text。
        /// 字体。
        /// </summary>
        public Font font
        {
            get { return m_Font; }
            set
            {
                m_Font = value;
                if (value)
                {
                    m_FontName = value.name;
                    m_FontInstacneId = value.GetInstanceID();
                }
                SetComponentDirty();
                SyncFontToSubComponent();
            }
        }

        public void SetDefaultFont()
        {
#if dUI_TextMeshPro
            tmpFont = XChartsSettings.tmpFont;
            SyncTMPFontToSubComponent();
#else
            font = XChartsSettings.font;
            SyncFontToSubComponent();
#endif
        }

        /// <summary>
        /// Gets the color of the specified index from the palette. 
        /// 获得调色盘对应系列索引的颜色值。
        /// </summary>
        /// <param name="index">编号索引</param>
        /// <returns>the color,or Color.clear when failed.颜色值，失败时返回Color.clear</returns>
        public Color32 GetColor(int index)
        {
            if (index < 0) index = 0;
            var newIndex = index < m_ColorPalette.Count ? index : index % m_ColorPalette.Count;
            if (newIndex < m_ColorPalette.Count)
                return m_ColorPalette[newIndex];
            else return Color.clear;
        }

        public void SetColorPalette(List<Color32> colorList)
        {
            m_ColorPalette = colorList;
            SetVerticesDirty();
        }
        public void SetColorPalette(List<string> hexColorStringList)
        {
            m_ColorPalette.Clear();
            foreach (var hexColor in hexColorStringList)
                m_ColorPalette.Add(ColorUtil.GetColor(hexColor));
            SetVerticesDirty();
        }

        public void CheckWarning(StringBuilder sb)
        {
#if dUI_TextMeshPro
            if (m_TMPFont == null)
            {
                sb.AppendFormat("warning:theme->tmpFont is null\n");
            }
#else
            if (m_Font == null)
            {
                sb.AppendFormat("warning:theme->font is null\n");
            }
#endif
            if (m_ColorPalette.Count == 0)
            {
                sb.AppendFormat("warning:theme->colorPalette is empty\n");
            }
            for (int i = 0; i < m_ColorPalette.Count; i++)
            {
                if (!ChartHelper.IsClearColor(m_ColorPalette[i]) && m_ColorPalette[i].a == 0)
                    sb.AppendFormat("warning:theme->colorPalette[{0}] alpha = 0\n", i);
            }
        }

        Dictionary<int, string> _colorDic = new Dictionary<int, string>();
        /// <summary>
        /// Gets the hexadecimal color string of the specified index from the palette. 
        /// 获得指定索引的十六进制颜色值字符串。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetColorStr(int index)
        {
            if (index < 0)
            {
                index = 0;
            }
            index = index % m_ColorPalette.Count;
            if (_colorDic.ContainsKey(index)) return _colorDic[index];
            else
            {
                _colorDic[index] = ColorUtility.ToHtmlStringRGBA(GetColor(index));
                return _colorDic[index];
            }
        }

        public void CopyTheme(Theme theme)
        {
            switch (theme)
            {
                case Theme.Dark:
                    CopyTheme(ChartTheme.Dark);
                    break;
                case Theme.Default:
                    CopyTheme(ChartTheme.Default);
                    break;
                case Theme.Light:
                    CopyTheme(ChartTheme.Light);
                    break;
            }
        }

        /// <summary>
        /// copy all configurations from theme. 
        /// 复制主题的所有配置。
        /// </summary>
        /// <param name="theme"></param>
        public void CopyTheme(ChartTheme theme)
        {
            m_Theme = theme.theme;
            m_ThemeName = theme.themeName;
#if dUI_TextMeshPro
            tmpFont = theme.tmpFont;
#endif
            font = theme.font;
            m_FontName = theme.fontName;
            m_FontInstacneId = theme.fontInstanceId;
            m_TMPFontName = theme.tmpFontName;
            m_TMPFontInstanceId = theme.tmpFontInstanceId;
            m_ContrastColor = theme.contrastColor;
            m_BackgroundColor = theme.m_BackgroundColor;
            m_Common.Copy(theme.common);
            m_Legend.Copy(theme.m_Legend);
            m_Title.Copy(theme.m_Title);
            m_SubTitle.Copy(theme.m_SubTitle);
            m_Axis.Copy(theme.axis);
            m_RadiusAxis.Copy(theme.radiusAxis);
            m_AngleAxis.Copy(theme.angleAxis);
            m_Polar.Copy(theme.polar);
            m_Gauge.Copy(theme.gauge);
            m_Radar.Copy(theme.radar);
            m_Tooltip.Copy(theme.tooltip);
            m_DataZoom.Copy(theme.dataZoom);
            m_VisualMap.Copy(theme.visualMap);
            m_Serie.Copy(theme.serie);
            ChartHelper.CopyList(m_ColorPalette, theme.colorPalette);
            SetAllDirty();
        }

        /// <summary>
        /// Clear all custom configurations. 
        /// 重置，清除所有自定义配置。
        /// </summary>
        public void ResetTheme()
        {
            switch (m_Theme)
            {
                case Theme.Default: CopyTheme(Default); break;
                case Theme.Light: CopyTheme(Light); break;
                case Theme.Dark: CopyTheme(Dark); break;
                case Theme.Custom:
                    var sourTheme = XThemeMgr.GetTheme(themeName);
                    if (sourTheme != null)
                    {
                        CopyTheme(sourTheme);
                    }
                    else
                    {
                        Debug.LogWarning("ResetTheme:can't find theme:" + themeName + ", reset to Default theme");
                        CopyTheme(Default);
                    }
                    break;
            }
        }

        /// <summary>
        /// 克隆主题。
        /// </summary>
        /// <returns></returns>
        public ChartTheme CloneTheme()
        {
            var theme = new ChartTheme();
            InitChartComponentTheme(theme);
            theme.CopyTheme(this);
            return theme;
        }


        /// <summary>
        /// default theme. 
        /// 默认主题。
        /// </summary>
        /// <value></value>
        public static ChartTheme Default
        {
            get
            {
                var theme = new ChartTheme();
                theme.theme = Theme.Default;
                theme.themeName = Theme.Default.ToString();
                theme.contrastColor = ColorUtil.GetColor("#514D4D");
                theme.backgroundColor = new Color32(255, 255, 255, 255);
                theme.colorPalette = new List<Color32>
                    {
                        new Color32(194, 53, 49, 255),
                        new Color32(47, 69, 84, 255),
                        new Color32(97, 160, 168, 255),
                        new Color32(212, 130, 101, 255),
                        new Color32(145, 199, 174, 255),
                        new Color32(116, 159, 131, 255),
                        new Color32(202, 134, 34, 255),
                        new Color32(189, 162, 154, 255),
                        new Color32(110, 112, 116, 255),
                        new Color32(84, 101, 112, 255),
                        new Color32(196, 204, 211, 255)
                    };
                InitChartComponentTheme(theme);
                return theme;
            }
        }

        /// <summary>
        /// light theme. 
        /// 亮主题。
        /// </summary>
        /// <value></value>
        public static ChartTheme Light
        {
            get
            {
                var theme = new ChartTheme();
                theme.theme = Theme.Light;
                theme.themeName = Theme.Light.ToString();
                theme.contrastColor = ColorUtil.GetColor("#514D4D");
                theme.backgroundColor = new Color32(255, 255, 255, 255);
                theme.colorPalette = new List<Color32>
                    {
                        ColorUtil.GetColor("#37A2DA"),
                        ColorUtil.GetColor("#32C5E9"),
                        ColorUtil.GetColor("#67E0E3"),
                        ColorUtil.GetColor("#9FE6B8"),
                        ColorUtil.GetColor("#FFDB5C"),
                        ColorUtil.GetColor("#ff9f7f"),
                        ColorUtil.GetColor("#fb7293"),
                        ColorUtil.GetColor("#E062AE"),
                        ColorUtil.GetColor("#E690D1"),
                        ColorUtil.GetColor("#e7bcf3"),
                        ColorUtil.GetColor("#9d96f5"),
                        ColorUtil.GetColor("#8378EA"),
                        ColorUtil.GetColor("#96BFFF"),
                    };
                InitChartComponentTheme(theme);
                return theme;
            }
        }

        /// <summary>
        /// dark theme. 
        /// 暗主题。
        /// </summary>
        /// <value></value>
        public static ChartTheme Dark
        {
            get
            {
                var theme = new ChartTheme();
                theme.theme = Theme.Dark;
                theme.themeName = Theme.Dark.ToString();
                theme.contrastColor = ColorUtil.GetColor("#B9B8CE");
                theme.backgroundColor = ColorUtil.GetColor("#100C2A");
                theme.colorPalette = new List<Color32>
                    {
                        ColorUtil.GetColor("#4992ff"),
                        ColorUtil.GetColor("#7cffb2"),
                        ColorUtil.GetColor("#fddd60"),
                        ColorUtil.GetColor("#ff6e76"),
                        ColorUtil.GetColor("#58d9f9"),
                        ColorUtil.GetColor("#05c091"),
                        ColorUtil.GetColor("#ff8a45"),
                        ColorUtil.GetColor("#8d48e3"),
                        ColorUtil.GetColor("#dd79ff"),
                    };
                InitChartComponentTheme(theme);
                return theme;
            }
        }

        public static ChartTheme EmptyTheme
        {
            get
            {
                var theme = new ChartTheme();
                theme.theme = Theme.Custom;
                theme.themeName = Theme.Custom.ToString();
                theme.contrastColor = Color.clear;
                theme.backgroundColor = Color.clear;
                theme.colorPalette = new List<Color32>();
                InitChartComponentTheme(theme);
                return theme;
            }
        }

        public void SyncFontName()
        {
            if (font)
            {
                m_FontName = font.name;
                m_FontInstacneId = font.GetInstanceID();
            }
#if dUI_TextMeshPro
            if (tmpFont)
            {
                m_TMPFontName = tmpFont.name;
                m_TMPFontInstanceId = tmpFont.GetInstanceID();
            }
#endif
        }

        public void SyncFontToSubComponent()
        {
            common.font = font;
            title.font = font;
            subTitle.font = font;
            legend.font = font;
            axis.font = font;
            radiusAxis.font = font;
            angleAxis.font = font;
            polar.font = font;
            gauge.font = font;
            radar.font = font;
            tooltip.font = font;
            dataZoom.font = font;
            visualMap.font = font;
        }

#if dUI_TextMeshPro
        public void SyncTMPFontToSubComponent()
        {
            common.tmpFont = tmpFont;
            title.tmpFont = tmpFont;
            subTitle.tmpFont = tmpFont;
            legend.tmpFont = tmpFont;
            axis.tmpFont = tmpFont;
            radiusAxis.tmpFont = tmpFont;
            angleAxis.tmpFont = tmpFont;
            polar.tmpFont = tmpFont;
            gauge.tmpFont = tmpFont;
            radar.tmpFont = tmpFont;
            tooltip.tmpFont = tmpFont;
            dataZoom.tmpFont = tmpFont;
            visualMap.tmpFont = tmpFont;
        }
#endif

        private static void InitChartComponentTheme(ChartTheme theme)
        {
            theme.common = new ComponentTheme(theme.theme);
            theme.title = new TitleTheme(theme.theme);
            theme.subTitle = new SubTitleTheme(theme.theme);
            theme.legend = new LegendTheme(theme.theme);
            theme.axis = new AxisTheme(theme.theme);
            theme.radiusAxis = new RadiusAxisTheme(theme.theme);
            theme.angleAxis = new AngleAxisTheme(theme.theme);
            theme.polar = new PolarAxisTheme(theme.theme);
            theme.gauge = new GaugeAxisTheme(theme.theme);
            theme.radar = new RadarAxisTheme(theme.theme);
            theme.tooltip = new TooltipTheme(theme.theme);
            theme.dataZoom = new DataZoomTheme(theme.theme);
            theme.visualMap = new VisualMapTheme(theme.theme);
            theme.serie = new SerieTheme(theme.theme);
            theme.SetDefaultFont();
        }

        /// <summary>
        /// Convert the html string to color. 
        /// 将字符串颜色值转成Color。
        /// </summary>
        /// <param name="hexColorStr"></param>
        /// <returns></returns>
        public static Color32 GetColor(string hexColorStr)
        {
            Color color;
            ColorUtility.TryParseHtmlString(hexColorStr, out color);
            return (Color32)color;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}