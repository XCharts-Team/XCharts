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
    /// 主题
    /// </summary>
    public enum ThemeType
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
    /// ||主题相关配置。
    /// </summary>
    public class ThemeStyle : ChildComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Theme m_SharedTheme;
        [SerializeField] private bool m_TransparentBackground = false;
        [SerializeField] private bool m_EnableCustomTheme = false;
        [SerializeField] private Font m_CustomFont;
        [SerializeField] private Color32 m_CustomBackgroundColor;
#if UNITY_2020_2
        [NonReorderable]
#endif
        [SerializeField] private List<Color32> m_CustomColorPalette = new List<Color32>(13);

        public bool show { get { return m_Show; } }
        /// <summary>
        /// the theme of chart.
        /// ||主题类型。
        /// </summary>
        public ThemeType themeType
        {
            get { return sharedTheme.themeType; }
        }
        /// <summary>
        /// theme name.
        /// ||主题名字。
        /// </summary>
        public string themeName
        {
            get { return sharedTheme.themeName; }
        }
        /// <summary>
        /// the asset of theme.
        /// ||主题配置。
        /// </summary>
        public Theme sharedTheme
        {
            get { return m_SharedTheme; }
            set { m_SharedTheme = value; SetAllDirty(); }
        }
        /// <summary>
        /// the contrast color of chart.
        /// ||对比色。
        /// </summary>
        public Color32 contrastColor
        {
            get { return sharedTheme.contrastColor; }
        }
        /// <summary>
        /// the background color of chart.
        /// ||背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get
            {
                if (m_TransparentBackground) return ColorUtil.clearColor32;
                else return m_EnableCustomTheme ? m_CustomBackgroundColor : sharedTheme.backgroundColor;
            }
        }
        /// <summary>
        /// Whether the background color is transparent. When true, the background color is not drawn.
        /// ||是否透明背景颜色。当设置为true时，不绘制背景颜色。
        /// </summary>
        public bool transparentBackground
        {
            get { return m_TransparentBackground; }
            set { m_TransparentBackground = value; SetAllDirty(); }
        }
        /// <summary>
        /// Whether to customize theme colors. When set to true, 
        /// you can use 'sync color to custom' to synchronize the theme color to the custom color. It can also be set manually.
        /// ||是否自定义主题颜色。当设置为true时，可以用‘sync color to custom’同步主题的颜色到自定义颜色。也可以手动设置。
        /// </summary>
        public bool enableCustomTheme
        {
            get { return m_EnableCustomTheme; }
            set { m_EnableCustomTheme = value; _colorDic.Clear(); SetAllDirty(); }
        }
        /// <summary>
        /// the custom background color of chart.
        /// ||自定义的背景颜色。
        /// </summary>
        public Color32 customBackgroundColor
        {
            get { return m_CustomBackgroundColor; }
            set { m_CustomBackgroundColor = value; SetAllDirty(); }
        }

        /// <summary>
        /// The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.
        /// ||调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。
        /// </summary>
        public List<Color32> colorPalette
        {
            get { return m_EnableCustomTheme ? m_CustomColorPalette : sharedTheme.colorPalette; }
        }
        public List<Color32> customColorPalette { get { return m_CustomColorPalette; } set { m_CustomColorPalette = value; SetVerticesDirty(); } }
        public ComponentTheme common { get { return sharedTheme.common; } }
        public TitleTheme title { get { return sharedTheme.title; } }
        public SubTitleTheme subTitle { get { return sharedTheme.subTitle; } }
        public LegendTheme legend { get { return sharedTheme.legend; } }
        public AxisTheme axis { get { return sharedTheme.axis; } }
        public TooltipTheme tooltip { get { return sharedTheme.tooltip; } }
        public DataZoomTheme dataZoom { get { return sharedTheme.dataZoom; } }
        public VisualMapTheme visualMap { get { return sharedTheme.visualMap; } }
        public SerieTheme serie { get { return sharedTheme.serie; } }

        /// <summary>
        /// Gets the color of the specified index from the palette.
        /// ||获得调色盘对应系列索引的颜色值。
        /// </summary>
        /// <param name="index">编号索引</param>
        /// <returns>the color,or Color.clear when failed.颜色值，失败时返回Color.clear</returns>
        public Color32 GetColor(int index)
        {
            if (colorPalette.Count <= 0) return Color.clear;
            if (index < 0) index = 0;
            var newIndex = index < colorPalette.Count ? index : index % colorPalette.Count;
            if (newIndex < colorPalette.Count)
                return colorPalette[newIndex];
            else return Color.clear;
        }

        public Color32 GetBackgroundColor(Background background)
        {
            if (background != null && background.show && !background.autoColor)
                return background.imageColor;
            else
                return backgroundColor;
        }

        public void SyncSharedThemeColorToCustom()
        {
            m_CustomBackgroundColor = sharedTheme.backgroundColor;
            m_CustomColorPalette.Clear();
            foreach (var color in sharedTheme.colorPalette)
            {
                m_CustomColorPalette.Add(color);
            }
            SetAllDirty();
        }

        public void CheckWarning(StringBuilder sb)
        {
#if dUI_TextMeshPro
            if (sharedTheme.tmpFont == null)
            {
                sb.AppendFormat("warning:theme->tmpFont is null\n");
            }
#else
            if (sharedTheme.font == null)
            {
                sb.AppendFormat("warning:theme->font is null\n");
            }
#endif
            if (sharedTheme.colorPalette.Count == 0)
            {
                sb.AppendFormat("warning:theme->colorPalette is empty\n");
            }
            for (int i = 0; i < sharedTheme.colorPalette.Count; i++)
            {
                if (!ChartHelper.IsClearColor(sharedTheme.colorPalette[i]) && sharedTheme.colorPalette[i].a == 0)
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
            index = index % colorPalette.Count;
            if (_colorDic.ContainsKey(index)) return _colorDic[index];
            else
            {
                _colorDic[index] = ColorUtility.ToHtmlStringRGBA(GetColor(index));
                return _colorDic[index];
            }
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

    }
}