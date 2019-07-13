
using UnityEngine;
using System;

namespace XCharts
{
    public enum Theme
    {
        Default = 1,
        Light,
        Dark
    }

    [Serializable]
    public class ThemeInfo : IEquatable<ThemeInfo>
    {
        [SerializeField] private Font m_Font;
        [SerializeField] private Color32 m_BackgroundColor;
        [SerializeField] private Color32 m_TextColor;
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

        public Font font { get { return m_Font; } set { m_Font = value; } }
        public Color32 backgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; } }
        public Color32 textColor { get { return m_TextColor; } set { m_TextColor = value; } }
        public Color32 titleSubTextColor { get { return m_TitleSubTextColor; } set { m_TitleSubTextColor = value; } }
        public Color32 legendTextColor { get { return m_LegendTextColor; } set { m_LegendTextColor = value; } }
        public Color32 legendUnableColor { get { return m_LegendUnableColor; } set { m_LegendUnableColor = value; } }
        public Color32 axisTextColor { get { return m_AxisTextColor; } set { m_AxisTextColor = value; } }
        public Color32 axisLineColor { get { return m_AxisLineColor; } set { m_AxisLineColor = value; } }
        public Color32 axisSplitLineColor { get { return m_AxisSplitLineColor; } set { m_AxisSplitLineColor = value; } }
        public Color32 tooltipBackgroundColor { get { return m_TooltipBackgroundColor; } set { m_TooltipBackgroundColor = value; } }
        public Color32 tooltipFlagAreaColor { get { return m_TooltipFlagAreaColor; } set { m_TooltipFlagAreaColor = value; } }
        public Color32 tooltipTextColor { get { return m_TooltipTextColor; } set { m_TooltipTextColor = value; } }
        public Color32 tooltipLabelColor { get { return m_TooltipLabelColor; } set { m_TooltipLabelColor = value; } }
        public Color32 tooltipLineColor { get { return m_TooltipLineColor; } set { m_TooltipLineColor = value; } }
        public Color32 dataZoomTextColor { get { return m_DataZoomTextColor; } set { m_DataZoomTextColor = value; } }
        public Color32 dataZoomLineColor { get { return m_DataZoomLineColor; } set { m_DataZoomLineColor = value; } }
        public Color32 dataZoomSelectedColor { get { return m_DataZoomSelectedColor; } set { m_DataZoomSelectedColor = value; } }
        public Color32[] colorPalette { get { return m_ColorPalette; } set { m_ColorPalette = value; } }

        public Color32 GetColor(int index)
        {
            if (index < 0)
            {
                index = 0;
            }
            index = index % m_ColorPalette.Length;
            return m_ColorPalette[index];
        }

        public void Copy(ThemeInfo theme)
        {
            m_Font = theme.m_Font;
            m_BackgroundColor = theme.m_BackgroundColor;
            m_LegendUnableColor = theme.m_LegendUnableColor;
            m_TextColor = theme.m_TextColor;
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

        public static ThemeInfo Default
        {
            get
            {
                return new ThemeInfo()
                {
                    m_Font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    m_BackgroundColor = new Color32(255, 255, 255, 255),
                    m_LegendUnableColor = GetColor("#cccccc"),
                    m_TextColor = GetColor("#514D4D"),
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
                    }
                };
            }
        }

        public static ThemeInfo Light
        {
            get
            {
                return new ThemeInfo()
                {
                    m_Font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    m_BackgroundColor = new Color32(255, 255, 255, 255),
                    m_LegendUnableColor = GetColor("#cccccc"),
                    m_TextColor = GetColor("#514D4D"),
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
                    }
                };
            }
        }

        public static ThemeInfo Dark
        {
            get
            {
                return new ThemeInfo()
                {
                    m_Font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                    m_LegendUnableColor = GetColor("#cccccc"),
                    m_BackgroundColor = new Color32(34, 34, 34, 255),
                    m_TextColor = GetColor("#eee"),
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
                    }
                };
            }
        }

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
                ChartHelper.IsValueEqualsColor(m_TextColor, other.m_TextColor) &&
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