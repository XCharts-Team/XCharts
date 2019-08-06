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
    public class ThemeInfo : IEquatable<ThemeInfo>
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
        [SerializeField] private List<Color32> m_CustomColorPalette = new List<Color32>(13);
        /// <summary>
        /// the theme of chart.
        /// 主题类型。
        /// </summary>
        public Theme theme { get { return m_Theme; } set { m_Theme = value; } }
        /// <summary>
        /// the font of chart text。
        /// 字体。
        /// </summary>
        public Font font
        {
            get { return m_CustomFont != null ? m_CustomFont : m_Font; }
            set { m_CustomFont = value; }
        }
        /// <summary>
        /// the background color of chart.
        /// 背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get { return m_CustomBackgroundColor != Color.clear ? m_CustomBackgroundColor : m_BackgroundColor; }
            set { m_CustomBackgroundColor = value; }
        }
        /// <summary>
        /// the main title text color.
        /// 主标题颜色。
        /// </summary>
        public Color32 titleTextColor
        {
            get { return m_CustomTitleTextColor != Color.clear ? m_CustomTitleTextColor : m_TitleTextColor; }
            set { m_CustomTitleTextColor = value; }
        }
        /// <summary>
        /// the subtitie text color.
        /// 副标题颜色。
        /// </summary>
        public Color32 titleSubTextColor
        {
            get { return m_CustomTitleSubTextColor != Color.clear ? m_CustomTitleSubTextColor : m_TitleSubTextColor; }
            set { m_CustomTitleSubTextColor = value; }
        }
        /// <summary>
        /// the legend text color.
        /// 图例文字的颜色。
        /// </summary>
        public Color32 legendTextColor
        {
            get { return m_CustomLegendTextColor != Color.clear ? m_CustomLegendTextColor : m_LegendTextColor; }
            set { m_CustomLegendTextColor = value; }
        }
        /// <summary>
        /// the legend unable text color.
        /// 图例变为不可用时的按钮颜色。
        /// </summary>
        public Color32 legendUnableColor
        {
            get { return m_CustomLegendUnableColor != Color.clear ? m_CustomLegendUnableColor : m_LegendUnableColor; }
            set { m_CustomLegendUnableColor = value; }
        }
        /// <summary>
        /// the axis text color.
        /// 坐标轴上标签的颜色。
        /// </summary>
        public Color32 axisTextColor
        {
            get { return m_CustomAxisTextColor != Color.clear ? m_CustomAxisTextColor : m_AxisTextColor; }
            set { m_CustomAxisTextColor = value; }
        }
        /// <summary>
        /// the color of axis line.
        /// 坐标轴轴线的颜色。
        /// </summary>
        public Color32 axisLineColor
        {
            get { return m_CustomAxisLineColor != Color.clear ? m_CustomAxisLineColor : m_AxisLineColor; }
            set { m_CustomAxisLineColor = value; }
        }
        /// <summary>
        /// the color of axis split line.
        /// 分割线的颜色，默认和坐标轴轴线颜色一致。
        /// </summary>
        public Color32 axisSplitLineColor
        {
            get { return m_CustomAxisSplitLineColor != Color.clear ? m_CustomAxisSplitLineColor : m_AxisSplitLineColor; }
            set { m_CustomAxisSplitLineColor = value; }
        }
        /// <summary>
        /// the tooltip background color.
        /// 提示框背景颜色。
        /// </summary>
        public Color32 tooltipBackgroundColor
        {
            get { return m_CustomTooltipBackgroundColor != Color.clear ? m_CustomTooltipBackgroundColor : m_TooltipBackgroundColor; }
            set { m_CustomTooltipBackgroundColor = value; }
        }
        /// <summary>
        /// the color of tooltip shadow crosshair indicator. 
        /// 提示框阴影指示器的颜色。
        /// </summary>
        public Color32 tooltipFlagAreaColor
        {
            get { return m_CustomTooltipFlagAreaColor != Color.clear ? m_CustomTooltipFlagAreaColor : m_TooltipFlagAreaColor; }
            set { m_CustomTooltipFlagAreaColor = value; }
        }
        /// <summary>
        /// the color of tooltip text.
        /// 提示框文字颜色。
        /// </summary>
        public Color32 tooltipTextColor
        {
            get { return m_CustomTooltipTextColor != Color.clear ? m_CustomTooltipTextColor : m_TooltipTextColor; }
            set { m_CustomTooltipTextColor = value; }
        }
        /// <summary>
        /// the background color of tooltip cross indicator's axis label.
        /// 提示框的十字指示器坐标轴标签的背景颜色。
        /// </summary>
        public Color32 tooltipLabelColor
        {
            get { return m_CustomTooltipLabelColor != Color.clear ? m_CustomTooltipLabelColor : m_TooltipLabelColor; }
            set { m_CustomTooltipLabelColor = value; }
        }
        /// <summary>
        /// the color tooltip indicator line.
        /// 提示框的指示线的颜色。
        /// </summary>
        public Color32 tooltipLineColor
        {
            get { return m_CustomTooltipLineColor != Color.clear ? m_CustomTooltipLineColor : m_TooltipLineColor; }
            set { m_CustomTooltipLineColor = value; }
        }
        /// <summary>
        /// the color of datazoom text.
        /// 区域缩放的文字颜色。
        /// </summary>
        public Color32 dataZoomTextColor
        {
            get { return m_CustomDataZoomTextColor != Color.clear ? m_CustomDataZoomTextColor : m_DataZoomTextColor; }
            set { m_CustomDataZoomTextColor = value; }
        }
        /// <summary>
        /// the color of datazoom line.
        /// 区域缩放的线条颜色。
        /// </summary>
        public Color32 dataZoomLineColor
        {
            get { return m_CustomDataZoomLineColor != Color.clear ? m_CustomDataZoomLineColor : m_DataZoomLineColor; }
            set { m_CustomDataZoomLineColor = value; }
        }
        /// <summary>
        /// the color of datazoom selected area.
        /// 区域缩放的选中区域颜色。
        /// </summary>
        public Color32 dataZoomSelectedColor
        {
            get { return m_CustomDataZoomSelectedColor != Color.clear ? m_CustomDataZoomSelectedColor : m_DataZoomSelectedColor; }
            set { m_CustomDataZoomSelectedColor = value; }
        }
        /// <summary>
        /// The color list of palette. If no color is set in series, the colors would be adopted sequentially and circularly from this list as the colors of series.
        /// 调色盘颜色列表。如果系列没有设置颜色，则会依次循环从该列表中取颜色作为系列颜色。
        /// </summary>
        public List<Color32> colorPalette { set { m_CustomColorPalette = value; } }

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
                if (customIndex < m_CustomColorPalette.Count && m_CustomColorPalette[customIndex] != Color.clear)
                {
                    return m_CustomColorPalette[customIndex];
                }
            }
            var newIndex = index < m_ColorPalette.Length ? index : index % m_ColorPalette.Length;
            if (newIndex < m_ColorPalette.Length)
                return m_ColorPalette[newIndex];
            else return Color.clear;
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
                    m_LegendTextColor = GetColor("#eee"),
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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is ThemeInfo)
            {
                return Equals((ThemeInfo)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(ThemeInfo other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return m_Font == other.m_Font &&
                ChartHelper.IsValueEqualsColor(m_LegendUnableColor, other.m_LegendUnableColor) &&
                ChartHelper.IsValueEqualsColor(m_BackgroundColor, other.m_BackgroundColor) &&
                ChartHelper.IsValueEqualsColor(m_TitleTextColor, other.m_TitleTextColor) &&
                ChartHelper.IsValueEqualsColor(m_TitleSubTextColor, other.m_TitleSubTextColor) &&
                ChartHelper.IsValueEqualsColor(m_AxisTextColor, other.m_AxisTextColor) &&
                ChartHelper.IsValueEqualsColor(m_AxisLineColor, other.m_AxisLineColor) &&
                ChartHelper.IsValueEqualsColor(m_AxisSplitLineColor, other.m_AxisSplitLineColor) &&
                ChartHelper.IsValueEqualsColor(m_TooltipBackgroundColor, other.m_TooltipBackgroundColor) &&
                ChartHelper.IsValueEqualsColor(m_AxisSplitLineColor, other.m_AxisSplitLineColor) &&
                ChartHelper.IsValueEqualsColor(m_TooltipTextColor, other.m_TooltipTextColor) &&
                ChartHelper.IsValueEqualsColor(m_TooltipFlagAreaColor, other.m_TooltipFlagAreaColor) &&
                ChartHelper.IsValueEqualsColor(m_DataZoomLineColor, other.m_DataZoomLineColor) &&
                ChartHelper.IsValueEqualsColor(m_DataZoomSelectedColor, other.m_DataZoomSelectedColor) &&
                ChartHelper.IsValueEqualsColor(m_DataZoomTextColor, other.m_DataZoomTextColor) &&
                m_ColorPalette.Length == other.m_ColorPalette.Length;
        }

        public static bool operator ==(ThemeInfo left, ThemeInfo right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }
            else if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }
            return Equals(left, right);
        }

        public static bool operator !=(ThemeInfo left, ThemeInfo right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}