using System.Text;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Serialization;

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
        Dark
    }

    [Serializable]
    /// <summary>
    /// Theme.
    /// 主题相关配置。
    /// </summary>
    public class ThemeInfo : MainComponent
    {
        [SerializeField] private Theme m_Theme = Theme.Default;
        [SerializeField] private Font m_Font;
        [SerializeField] private Color32 m_BackgroundColor;
        [FormerlySerializedAs("m_TextColor")]
        [SerializeField] private Color32 m_TitleTextColor;
        [SerializeField] private Color32 m_TitleSubTextColor;
        [SerializeField] private Color32 m_LegendTextColor;
        [SerializeField] private Color32 m_LegendUnableColor;
        [SerializeField] private Color32 m_AxisTextColor;
        [SerializeField] private Color32 m_AxisLineColor;
        [SerializeField] private Color32 m_AxisSplitLineColor;
        [SerializeField] private Color32 m_TooltipBackgroundColor;
        [SerializeField] private Color32 m_TooltipFlagAreaColor;
        [SerializeField] private Color32 m_TooltipTextColor;
        [SerializeField] private Color32 m_TooltipLabelColor;
        [SerializeField] private Color32 m_TooltipLineColor;
        [SerializeField] private Color32 m_DataZoomTextColor;
        [SerializeField] private Color32 m_DataZoomLineColor;
        [SerializeField] private Color32 m_DataZoomSelectedColor;
        [SerializeField] private Color32 m_VisualMapBackgroundColor;
        [SerializeField] private Color32 m_VisualMapBorderColor;
        [SerializeField] private Color32[] m_ColorPalette;

        [SerializeField] private Font m_CustomFont;
        [SerializeField] private Color32 m_CustomBackgroundColor;
        [FormerlySerializedAs("m_CustomTextColor")]
        [SerializeField] private Color32 m_CustomTitleTextColor;
        [SerializeField] private Color32 m_CustomTitleSubTextColor;
        [SerializeField] private Color32 m_CustomLegendTextColor;
        [SerializeField] private Color32 m_CustomLegendUnableColor;
        [SerializeField] private Color32 m_CustomAxisTextColor;
        [SerializeField] private Color32 m_CustomAxisLineColor;
        [SerializeField] private Color32 m_CustomAxisSplitLineColor;
        [SerializeField] private Color32 m_CustomTooltipBackgroundColor;
        [SerializeField] private Color32 m_CustomTooltipFlagAreaColor;
        [SerializeField] private Color32 m_CustomTooltipTextColor;
        [SerializeField] private Color32 m_CustomTooltipLabelColor;
        [SerializeField] private Color32 m_CustomTooltipLineColor;
        [SerializeField] private Color32 m_CustomDataZoomTextColor;
        [SerializeField] private Color32 m_CustomDataZoomLineColor;
        [SerializeField] private Color32 m_CustomDataZoomSelectedColor;
        [SerializeField] private Color32 m_CustomVisualMapBackgroundColor;
        [SerializeField] private Color32 m_CustomVisualMapBorderColor;
        [SerializeField] private List<Color32> m_CustomColorPalette = new List<Color32>(13);
        /// <summary>
        /// the theme of chart.
        /// 主题类型。
        /// </summary>
        public Theme theme
        {
            get { return m_Theme; }
            set { if (PropertyUtility.SetStruct(ref m_Theme, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the font of chart text。
        /// 字体。
        /// </summary>
        public Font font
        {
            get { return m_CustomFont != null ? m_CustomFont : m_Font; }
            set { if (PropertyUtility.SetClass(ref m_CustomFont, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the background color of chart.
        /// 背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomBackgroundColor) ? m_CustomBackgroundColor : m_BackgroundColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomBackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the main title text color.
        /// 主标题颜色。
        /// </summary>
        public Color32 titleTextColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomTitleTextColor) ? m_CustomTitleTextColor : m_TitleTextColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomTitleTextColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the subtitie text color.
        /// 副标题颜色。
        /// </summary>
        public Color32 titleSubTextColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomTitleSubTextColor) ? m_CustomTitleSubTextColor : m_TitleSubTextColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomTitleSubTextColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the legend text color.
        /// 图例文字的颜色。
        /// </summary>
        public Color32 legendTextColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomLegendTextColor) ? m_CustomLegendTextColor : m_LegendTextColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomLegendTextColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the legend unable text color.
        /// 图例变为不可用时的按钮颜色。
        /// </summary>
        public Color32 legendUnableColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomLegendUnableColor) ? m_CustomLegendUnableColor : m_LegendUnableColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomLegendUnableColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the axis text color.
        /// 坐标轴上标签的颜色。
        /// </summary>
        public Color32 axisTextColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomAxisTextColor) ? m_CustomAxisTextColor : m_AxisTextColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomAxisTextColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color of axis line.
        /// 坐标轴轴线的颜色。
        /// </summary>
        public Color32 axisLineColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomAxisLineColor) ? m_CustomAxisLineColor : m_AxisLineColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomAxisLineColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of axis split line.
        /// 分割线的颜色，默认和坐标轴轴线颜色一致。
        /// </summary>
        public Color32 axisSplitLineColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomAxisSplitLineColor) ? m_CustomAxisSplitLineColor : m_AxisSplitLineColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomAxisSplitLineColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the tooltip background color.
        /// 提示框背景颜色。
        /// </summary>
        public Color32 tooltipBackgroundColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomTooltipBackgroundColor) ? m_CustomTooltipBackgroundColor : m_TooltipBackgroundColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomTooltipBackgroundColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color of tooltip shadow crosshair indicator. 
        /// 提示框阴影指示器的颜色。
        /// </summary>
        public Color32 tooltipFlagAreaColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomTooltipFlagAreaColor) ? m_CustomTooltipFlagAreaColor : m_TooltipFlagAreaColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomTooltipFlagAreaColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of tooltip text.
        /// 提示框文字颜色。
        /// </summary>
        public Color32 tooltipTextColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomTooltipTextColor) ? m_CustomTooltipTextColor : m_TooltipTextColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomTooltipTextColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the background color of tooltip cross indicator's axis label.
        /// 提示框的十字指示器坐标轴标签的背景颜色。
        /// </summary>
        public Color32 tooltipLabelColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomTooltipLabelColor) ? m_CustomTooltipLabelColor : m_TooltipLabelColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomTooltipLabelColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color tooltip indicator line.
        /// 提示框的指示线的颜色。
        /// </summary>
        public Color32 tooltipLineColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomTooltipLineColor) ? m_CustomTooltipLineColor : m_TooltipLineColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomTooltipLineColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of datazoom text.
        /// 区域缩放的文字颜色。
        /// </summary>
        public Color32 dataZoomTextColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomDataZoomTextColor) ? m_CustomDataZoomTextColor : m_DataZoomTextColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomDataZoomTextColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color of datazoom line.
        /// 区域缩放的线条颜色。
        /// </summary>
        public Color32 dataZoomLineColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomDataZoomLineColor) ? m_CustomDataZoomLineColor : m_DataZoomLineColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomDataZoomLineColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of datazoom selected area.
        /// 区域缩放的选中区域颜色。
        /// </summary>
        public Color32 dataZoomSelectedColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomDataZoomSelectedColor) ? m_CustomDataZoomSelectedColor : m_DataZoomSelectedColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomDataZoomSelectedColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// 视觉映射组件的背景色。
        /// </summary>
        public Color32 visualMapBackgroundColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomVisualMapBackgroundColor) ? m_CustomVisualMapBackgroundColor : m_VisualMapBackgroundColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomVisualMapBackgroundColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// 视觉映射的边框色。
        /// </summary>
        public Color32 visualMapBorderColor
        {
            get { return !ChartHelper.IsClearColor(m_CustomVisualMapBorderColor) ? m_CustomVisualMapBorderColor : m_VisualMapBorderColor; }
            set { if (PropertyUtility.SetColor(ref m_CustomVisualMapBorderColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.
        /// 调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。
        /// </summary>
        public List<Color32> colorPalette { set { m_CustomColorPalette = value; SetVerticesDirty(); } }

        /// <summary>
        /// Gets the color of the specified index from the palette. 
        /// 获得调色盘对应系列索引的颜色值。
        /// </summary>
        /// <param name="index">编号索引</param>
        /// <returns>the color,or Color.clear when failed.颜色值，失败时返回Color.clear</returns>
        public Color32 GetColor(int index)
        {
            if (index < 0) index = 0;
            if (m_CustomColorPalette.Count > 0)
            {
                var customIndex = index < m_CustomColorPalette.Count ? index : index % m_CustomColorPalette.Count;
                if (customIndex < m_CustomColorPalette.Count
                && !ChartHelper.IsClearColor(m_CustomColorPalette[customIndex]))
                {
                    return m_CustomColorPalette[customIndex];
                }
            }
            var newIndex = index < m_ColorPalette.Length ? index : index % m_ColorPalette.Length;
            if (newIndex < m_ColorPalette.Length)
                return m_ColorPalette[newIndex];
            else return Color.clear;
        }

        public void CheckWarning(StringBuilder sb)
        {
            if (m_Font == null && m_CustomFont == null)
            {
                sb.AppendFormat("warning:theme->font is null\n");
            }
            if (m_ColorPalette.Length == 0 && m_CustomColorPalette.Count == 0)
            {
                sb.AppendFormat("warning:theme->colorPalette is empty\n");
            }
            for (int i = 0; i < m_ColorPalette.Length; i++)
            {
                if (!ChartHelper.IsClearColor(m_ColorPalette[i]) && m_ColorPalette[i].a == 0)
                    sb.AppendFormat("warning:theme->colorPalette[{0}] alpha = 0\n", i);
            }
            for (int i = 0; i < m_CustomColorPalette.Count; i++)
            {
                if (!ChartHelper.IsClearColor(m_CustomColorPalette[i]) && m_CustomColorPalette[i].a == 0)
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
            index = index % m_ColorPalette.Length;
            if (_colorDic.ContainsKey(index)) return _colorDic[index];
            else
            {
                _colorDic[index] = ColorUtility.ToHtmlStringRGBA(GetColor(index));
                return _colorDic[index];
            }
        }

        public void Copy(Theme theme)
        {
            switch (theme)
            {
                case Theme.Dark:
                    Copy(ThemeInfo.Dark);
                    break;
                case Theme.Default:
                    Copy(ThemeInfo.Default);
                    break;
                case Theme.Light:
                    Copy(ThemeInfo.Light);
                    break;
            }
        }

        /// <summary>
        /// copy all configurations from theme. 
        /// 复制主题的所有配置。
        /// </summary>
        /// <param name="theme"></param>
        public void Copy(ThemeInfo theme)
        {
            m_Theme = theme.theme;
            m_Font = theme.m_Font;
            m_BackgroundColor = theme.m_BackgroundColor;
            m_LegendUnableColor = theme.m_LegendUnableColor;
            m_TitleTextColor = theme.m_TitleTextColor;
            m_TitleSubTextColor = theme.m_TitleSubTextColor;
            m_LegendTextColor = theme.m_LegendTextColor;
            m_AxisTextColor = theme.m_AxisTextColor;
            m_AxisLineColor = theme.m_AxisLineColor;
            m_AxisSplitLineColor = theme.m_AxisSplitLineColor;
            m_TooltipBackgroundColor = theme.m_TooltipBackgroundColor;
            m_TooltipTextColor = theme.m_TooltipTextColor;
            m_TooltipLabelColor = theme.m_TooltipLabelColor;
            m_TooltipLineColor = theme.m_TooltipLineColor;
            m_DataZoomLineColor = theme.m_DataZoomLineColor;
            m_DataZoomSelectedColor = theme.m_DataZoomSelectedColor;
            m_DataZoomTextColor = theme.m_DataZoomTextColor;
            m_VisualMapBackgroundColor = theme.m_VisualMapBackgroundColor;
            m_VisualMapBorderColor = theme.m_VisualMapBorderColor;
            m_ColorPalette = new Color32[theme.m_ColorPalette.Length];
            for (int i = 0; i < theme.m_ColorPalette.Length; i++)
            {
                m_ColorPalette[i] = theme.m_ColorPalette[i];
            }
        }

        /// <summary>
        /// Clear all custom configurations. 
        /// 重置，清除所有自定义配置。
        /// </summary>
        public void Reset()
        {
            m_Theme = Theme.Default;
            m_Font = null;
            m_BackgroundColor = Color.clear;
            m_LegendUnableColor = Color.clear;
            m_TitleTextColor = Color.clear;
            m_TitleSubTextColor = Color.clear;
            m_LegendTextColor = Color.clear;
            m_AxisTextColor = Color.clear;
            m_AxisLineColor = Color.clear;
            m_AxisSplitLineColor = Color.clear;
            m_TooltipBackgroundColor = Color.clear;
            m_TooltipTextColor = Color.clear;
            m_TooltipLabelColor = Color.clear;
            m_TooltipLineColor = Color.clear;
            m_DataZoomLineColor = Color.clear;
            m_DataZoomSelectedColor = Color.clear;
            m_DataZoomTextColor = Color.clear;
            m_VisualMapBackgroundColor = Color.clear;
            m_VisualMapBorderColor = Color.clear;
            for (int i = 0; i < m_CustomColorPalette.Count; i++)
            {
                m_CustomColorPalette[i] = Color.clear;
            }
        }

        /// <summary>
        /// default theme. 
        /// 默认主题。
        /// </summary>
        /// <value></value>
        public static ThemeInfo Default
        {
            get
            {
                return new ThemeInfo()
                {
                    m_Theme = Theme.Default,
                    m_Font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    m_BackgroundColor = new Color32(255, 255, 255, 255),
                    m_LegendUnableColor = GetColor("#cccccc"),
                    m_TitleTextColor = GetColor("#514D4D"),
                    m_TitleSubTextColor = GetColor("#514D4D"),
                    m_LegendTextColor = GetColor("#514D4D"),
                    m_AxisTextColor = GetColor("#514D4D"),
                    m_AxisLineColor = GetColor("#514D4D"),
                    m_AxisSplitLineColor = GetColor("#51515120"),
                    m_TooltipBackgroundColor = GetColor("#515151C8"),
                    m_TooltipTextColor = GetColor("#FFFFFFFF"),
                    m_TooltipFlagAreaColor = GetColor("#51515120"),
                    m_TooltipLabelColor = GetColor("#292929FF"),
                    m_TooltipLineColor = GetColor("#29292964"),
                    m_DataZoomLineColor = GetColor("#51515120"),
                    m_DataZoomSelectedColor = GetColor("#51515120"),
                    m_DataZoomTextColor = GetColor("#514D4D"),
                    m_VisualMapBackgroundColor = GetColor("#51515120"),
                    m_VisualMapBorderColor = GetColor("#cccccc"),
                    m_ColorPalette = new Color32[]
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
                    },
                    m_CustomColorPalette = new List<Color32>{
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear
                    }
                };
            }
        }

        /// <summary>
        /// light theme. 
        /// 亮主题。
        /// </summary>
        /// <value></value>
        public static ThemeInfo Light
        {
            get
            {
                return new ThemeInfo()
                {
                    m_Theme = Theme.Light,
                    m_Font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    m_BackgroundColor = new Color32(255, 255, 255, 255),
                    m_LegendUnableColor = GetColor("#cccccc"),
                    m_TitleTextColor = GetColor("#514D4D"),
                    m_TitleSubTextColor = GetColor("#514D4D"),
                    m_LegendTextColor = GetColor("#514D4D"),
                    m_AxisTextColor = GetColor("#514D4D"),
                    m_AxisLineColor = GetColor("#514D4D"),
                    m_AxisSplitLineColor = GetColor("#51515120"),
                    m_TooltipBackgroundColor = GetColor("#515151C8"),
                    m_TooltipTextColor = GetColor("#FFFFFFFF"),
                    m_TooltipFlagAreaColor = GetColor("#51515120"),
                    m_TooltipLabelColor = GetColor("#292929FF"),
                    m_TooltipLineColor = GetColor("#29292964"),
                    m_DataZoomLineColor = GetColor("#51515120"),
                    m_DataZoomSelectedColor = GetColor("#51515120"),
                    m_DataZoomTextColor = GetColor("#514D4D"),
                    m_VisualMapBackgroundColor = GetColor("#51515120"),
                    m_VisualMapBorderColor = GetColor("#cccccc"),
                    m_ColorPalette = new Color32[]
                    {
                        new Color32(55, 162, 218, 255),
                        new Color32(255, 159, 127, 255),
                        new Color32(50, 197, 233, 255),
                        new Color32(251, 114, 147, 255),
                        new Color32(103, 224, 227, 255),
                        new Color32(224, 98, 174, 255),
                        new Color32(159, 230, 184, 255),
                        new Color32(230, 144, 209, 255),
                        new Color32(255, 219, 92, 255),
                        new Color32(230, 188, 243, 255),
                        new Color32(157, 150, 245, 255),
                        new Color32(131, 120, 234, 255),
                        new Color32(150, 191, 255, 255)
                    },
                    m_CustomColorPalette = new List<Color32>{
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear
                    }
                };
            }
        }

        /// <summary>
        /// dark theme. 
        /// 暗主题。
        /// </summary>
        /// <value></value>
        public static ThemeInfo Dark
        {
            get
            {
                return new ThemeInfo()
                {
                    m_Theme = Theme.Dark,
                    m_Font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    m_LegendUnableColor = GetColor("#cccccc"),
                    m_BackgroundColor = new Color32(34, 34, 34, 255),
                    m_TitleTextColor = GetColor("#eee"),
                    m_TitleSubTextColor = GetColor("#eee"),
                    m_LegendTextColor = GetColor("#eee"),
                    m_AxisTextColor = GetColor("#eee"),
                    m_AxisLineColor = GetColor("#eee"),
                    m_AxisSplitLineColor = GetColor("#aaa"),
                    m_TooltipBackgroundColor = GetColor("#515151C8"),
                    m_TooltipTextColor = GetColor("#FFFFFFFF"),
                    m_TooltipFlagAreaColor = GetColor("#51515120"),
                    m_TooltipLabelColor = GetColor("#A7A7A7FF"),
                    m_TooltipLineColor = GetColor("#eee"),
                    m_DataZoomLineColor = GetColor("#FFFFFF45"),
                    m_DataZoomSelectedColor = GetColor("#D0D0D03D"),
                    m_DataZoomTextColor = GetColor("#FFFFFFFF"),
                    m_VisualMapBackgroundColor = GetColor("#aaa"),
                    m_VisualMapBorderColor = GetColor("#cccccc"),
                    m_ColorPalette = new Color32[]
                    {
                        new Color32(221, 107, 102, 255),
                        new Color32(117, 154, 160, 255),
                        new Color32(230, 157, 135, 255),
                        new Color32(141, 193, 169, 255),
                        new Color32(234, 126, 83, 255),
                        new Color32(238, 221, 120, 255),
                        new Color32(115, 163, 115, 255),
                        new Color32(115, 185, 188, 255),
                        new Color32(114, 137, 171, 255),
                        new Color32(145, 202, 140, 255),
                        new Color32(244, 159, 66, 255)
                    },
                    m_CustomColorPalette = new List<Color32>{
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear,
                        Color.clear
                    }
                };
            }
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