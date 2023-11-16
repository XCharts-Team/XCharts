using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    /// <summary>
    /// Theme.
    /// ||主题相关配置。
    /// </summary>
    [Serializable]
    public class Theme : ScriptableObject
    {
        [SerializeField] private ThemeType m_ThemeType = ThemeType.Default;
        [SerializeField] private string m_ThemeName = ThemeType.Default.ToString();
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
        [SerializeField] private TooltipTheme m_Tooltip;
        [SerializeField] private DataZoomTheme m_DataZoom;
        [SerializeField] private VisualMapTheme m_VisualMap;
        [SerializeField] private SerieTheme m_Serie;

        /// <summary>
        /// the theme of chart.
        /// ||主题类型。
        /// </summary>
        public ThemeType themeType
        {
            get { return m_ThemeType; }
            set { PropertyUtil.SetStruct(ref m_ThemeType, value); }
        }
        /// <summary>
        /// the name of theme.
        /// ||主题名称。
        /// </summary>
        public string themeName
        {
            get { return m_ThemeName; }
            set { PropertyUtil.SetClass(ref m_ThemeName, value); }
        }

        /// <summary>
        /// the contrast color of chart.
        /// ||对比色。
        /// </summary>
        public Color32 contrastColor
        {
            get { return m_ContrastColor; }
            set { PropertyUtil.SetColor(ref m_ContrastColor, value); }
        }
        /// <summary>
        /// the background color of chart.
        /// ||背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get { return m_BackgroundColor; }
            set { PropertyUtil.SetColor(ref m_BackgroundColor, value); }
        }

        /// <summary>
        /// The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.
        /// ||调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。
        /// </summary>
        public List<Color32> colorPalette { get { return m_ColorPalette; } set { m_ColorPalette = value; } }
        public ComponentTheme common { get { return m_Common; } set { m_Common = value; } }
        public TitleTheme title { get { return m_Title; } set { m_Title = value; } }
        public SubTitleTheme subTitle { get { return m_SubTitle; } set { m_SubTitle = value; } }
        public LegendTheme legend { get { return m_Legend; } set { m_Legend = value; } }
        public AxisTheme axis { get { return m_Axis; } set { m_Axis = value; } }
        public TooltipTheme tooltip { get { return m_Tooltip; } set { m_Tooltip = value; } }
        public DataZoomTheme dataZoom { get { return m_DataZoom; } set { m_DataZoom = value; } }
        public VisualMapTheme visualMap { get { return m_VisualMap; } set { m_VisualMap = value; } }
        public SerieTheme serie { get { return m_Serie; } set { m_Serie = value; } }
#if dUI_TextMeshPro
        /// <summary>
        /// the font of chart text。
        /// ||主题字体。
        /// </summary>
        public TMP_FontAsset tmpFont
        {
            get { return m_TMPFont; }
            set
            {
                m_TMPFont = value;
                SyncTMPFontToSubComponent();
            }
        }
#endif
        /// <summary>
        /// the font of chart text。
        /// ||主题字体。
        /// </summary>
        public Font font
        {
            get { return m_Font; }
            set
            {
                m_Font = value;
                SyncFontToSubComponent();
            }
        }

        // void OnEnable()
        // {
        // }

        // void OnDisable()
        // {
        // }

        public void SetDefaultFont()
        {
#if dUI_TextMeshPro
            tmpFont = XCSettings.tmpFont;
            SyncTMPFontToSubComponent();
#else
            font = XCSettings.font;
            SyncFontToSubComponent();
#endif
        }

        /// <summary>
        /// Gets the color of the specified index from the palette.
        /// ||获得调色盘对应系列索引的颜色值。
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
        /// ||获得指定索引的十六进制颜色值字符串。
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

        public bool CopyTheme(ThemeType theme)
        {
            switch (theme)
            {
                case ThemeType.Dark:
                    ResetToDarkTheme(this);
                    return true;
                case ThemeType.Default:
                    ResetToDefaultTheme(this);
                    return true;
            }
            return false;
        }

        /// <summary>
        /// copy all configurations from theme.
        /// ||复制主题的所有配置。
        /// </summary>
        /// <param name="theme"></param>
        public void CopyTheme(Theme theme)
        {
            m_ThemeType = theme.themeType;
            m_ThemeName = theme.themeName;
#if dUI_TextMeshPro
            tmpFont = theme.tmpFont;
#endif
            font = theme.font;
            m_BackgroundColor = theme.backgroundColor;
            m_Common.Copy(theme.common);
            m_Legend.Copy(theme.legend);
            m_Title.Copy(theme.title);
            m_SubTitle.Copy(theme.subTitle);
            m_Axis.Copy(theme.axis);
            m_Tooltip.Copy(theme.tooltip);
            m_DataZoom.Copy(theme.dataZoom);
            m_VisualMap.Copy(theme.visualMap);
            m_Serie.Copy(theme.serie);
            ChartHelper.CopyList(m_ColorPalette, theme.colorPalette);
        }

        /// <summary>
        /// Clear all custom configurations.
        /// ||重置，清除所有自定义配置。
        /// </summary>
        public bool ResetTheme()
        {
            switch (m_ThemeType)
            {
                case ThemeType.Default:
                    ResetToDefaultTheme(this);
                    return true;
                case ThemeType.Dark:
                    ResetToDarkTheme(this);
                    return true;
                case ThemeType.Custom:
                    return false;
            }
            return false;
        }

        /// <summary>
        /// 克隆主题。
        /// </summary>
        /// <returns></returns>
        public Theme CloneTheme()
        {
            var theme = ScriptableObject.CreateInstance<Theme>();
            InitChartComponentTheme(theme);
            theme.CopyTheme(this);
            return theme;
        }

        /// <summary>
        /// default theme.
        /// ||默认主题。
        /// </summary>
        public static void ResetToDefaultTheme(Theme theme)
        {
            theme.themeType = ThemeType.Default;
            theme.themeName = ThemeType.Default.ToString();
            theme.backgroundColor = new Color32(255, 255, 255, 255);
            theme.colorPalette = new List<Color32>
            {
                ColorUtil.GetColor("#5470c6"),
                ColorUtil.GetColor("#91cc75"),
                ColorUtil.GetColor("#fac858"),
                ColorUtil.GetColor("#ee6666"),
                ColorUtil.GetColor("#73c0de"),
                ColorUtil.GetColor("#3ba272"),
                ColorUtil.GetColor("#fc8452"),
                ColorUtil.GetColor("#9a60b4"),
                ColorUtil.GetColor("#ea7ccc"),

            };
            InitChartComponentTheme(theme);
        }

        /// <summary>
        /// dark theme.
        /// ||暗主题。
        /// </summary>
        public static void ResetToDarkTheme(Theme theme)
        {
            theme.themeType = ThemeType.Dark;
            theme.themeName = ThemeType.Dark.ToString();
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
        }

        public static Theme EmptyTheme
        {
            get
            {
                var theme = ScriptableObject.CreateInstance<Theme>();
                theme.themeType = ThemeType.Custom;
                theme.themeName = ThemeType.Custom.ToString();
                theme.backgroundColor = Color.clear;
                theme.colorPalette = new List<Color32>();
                InitChartComponentTheme(theme);
                return theme;
            }
        }

        public void SyncFontToSubComponent()
        {
            common.font = font;
            title.font = font;
            subTitle.font = font;
            legend.font = font;
            axis.font = font;
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
            tooltip.tmpFont = tmpFont;
            dataZoom.tmpFont = tmpFont;
            visualMap.tmpFont = tmpFont;
        }
#endif

        private static void InitChartComponentTheme(Theme theme)
        {
            theme.common = new ComponentTheme(theme.themeType);
            theme.title = new TitleTheme(theme.themeType);
            theme.subTitle = new SubTitleTheme(theme.themeType);
            theme.legend = new LegendTheme(theme.themeType);
            theme.axis = new AxisTheme(theme.themeType);
            theme.tooltip = new TooltipTheme(theme.themeType);
            theme.dataZoom = new DataZoomTheme(theme.themeType);
            theme.visualMap = new VisualMapTheme(theme.themeType);
            theme.serie = new SerieTheme(theme.themeType);
            theme.SetDefaultFont();
        }

        /// <summary>
        /// Convert the html string to color.
        /// ||将字符串颜色值转成Color。
        /// </summary>
        /// <param name="hexColorStr"></param>
        /// <returns></returns>
        public static Color32 GetColor(string hexColorStr)
        {
            Color color;
            ColorUtility.TryParseHtmlString(hexColorStr, out color);
            return (Color32) color;
        }

        public void SetColorPalette(List<string> hexColorStringList)
        {
            m_ColorPalette.Clear();
            foreach (var hexColor in hexColorStringList)
                m_ColorPalette.Add(ColorUtil.GetColor(hexColor));

        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}