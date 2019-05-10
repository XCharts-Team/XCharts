
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
        [SerializeField] private Color32 m_ContrastColor;
        [SerializeField] private Color32 m_TextColor;
        [SerializeField] private Color32 m_SubTextColor;
        [SerializeField] private Color32 m_LegendTextColor;
        [SerializeField] private Color32 m_UnableColor;
        [SerializeField] private Color32 m_AxisLineColor;
        [SerializeField] private Color32 m_AxisSplitLineColor;
        [SerializeField] private Color32 m_TooltipBackgroundColor;
        [SerializeField] private Color32 m_TooltipFlagAreaColor;
        [SerializeField] private Color32 m_TooltipTextColor;
        [SerializeField] private Color32[] m_ColorPalette;

        public Font font { get { return m_Font; } set { m_Font = value; } }
        public Color32 backgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; } }
        public Color32 contrastColor { get { return m_ContrastColor; } set { m_ContrastColor = value; } }
        public Color32 textColor { get { return m_TextColor; } set { m_TextColor = value; } }
        public Color32 subTextColor { get { return m_SubTextColor; } set { m_SubTextColor = value; } }
        public Color32 legendTextColor { get { return m_LegendTextColor; } set { m_LegendTextColor = value; } }
        public Color32 unableColor { get { return m_UnableColor; } set { m_UnableColor = value; } }
        public Color32 axisLineColor { get { return m_AxisLineColor; } set { m_AxisLineColor = value; } }
        public Color32 axisSplitLineColor { get { return m_AxisSplitLineColor; } set { m_AxisSplitLineColor = value; } }
        public Color32 tooltipBackgroundColor { get { return m_TooltipBackgroundColor; } set { m_TooltipBackgroundColor = value; } }
        public Color32 tooltipFlagAreaColor { get { return m_TooltipFlagAreaColor; } set { m_TooltipFlagAreaColor = value; } }
        public Color32 tooltipTextColor { get { return m_TooltipTextColor; } set { m_TooltipTextColor = value; } }
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
            m_ContrastColor = theme.m_ContrastColor;
            m_UnableColor = theme.m_UnableColor;
            m_TextColor = theme.m_TextColor;
            m_SubTextColor = theme.m_SubTextColor;
            m_LegendTextColor = theme.m_LegendTextColor;
            m_AxisLineColor = theme.m_AxisLineColor;
            m_AxisSplitLineColor = theme.m_AxisSplitLineColor;
            m_TooltipBackgroundColor = theme.m_TooltipBackgroundColor;
            m_TooltipTextColor = theme.m_TooltipTextColor;
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
                    m_ContrastColor = GetColor("#514D4D"),
                    m_UnableColor = GetColor("#cccccc"),
                    m_TextColor = GetColor("#514D4D"),
                    m_SubTextColor = GetColor("#514D4D"),
                    m_LegendTextColor = GetColor("#eee"),
                    m_AxisLineColor = GetColor("#514D4D"),
                    m_AxisSplitLineColor = GetColor("#51515120"),
                    m_TooltipBackgroundColor = GetColor("#515151B5"),
                    m_TooltipTextColor = GetColor("#FFFFFFFF"),
                    m_TooltipFlagAreaColor = GetColor("#51515120"),
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
                    m_ContrastColor = GetColor("#514D4D"),
                    m_UnableColor = GetColor("#cccccc"),
                    m_TextColor = GetColor("#514D4D"),
                    m_SubTextColor = GetColor("#514D4D"),
                    m_LegendTextColor = GetColor("#514D4D"),
                    m_AxisLineColor = GetColor("#514D4D"),
                    m_AxisSplitLineColor = GetColor("#51515120"),
                    m_TooltipBackgroundColor = GetColor("#515151B5"),
                    m_TooltipTextColor = GetColor("#FFFFFFFF"),
                    m_TooltipFlagAreaColor = GetColor("#51515120"),
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
                    m_UnableColor = GetColor("#cccccc"),
                    m_BackgroundColor = new Color32(34, 34, 34, 255),
                    m_ContrastColor = GetColor("#eee"),
                    m_TextColor = GetColor("#eee"),
                    m_SubTextColor = GetColor("#eee"),
                    m_LegendTextColor = GetColor("#eee"),
                    m_AxisLineColor = GetColor("#eee"),
                    m_AxisSplitLineColor = GetColor("#aaa"),
                    m_TooltipBackgroundColor = GetColor("#515151B5"),
                    m_TooltipTextColor = GetColor("#FFFFFFFF"),
                    m_TooltipFlagAreaColor = GetColor("#51515120"),
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
            if (!(obj is ThemeInfo))
                return false;

            return Equals((ThemeInfo)obj);
        }

        public bool Equals(ThemeInfo other)
        {
            return m_Font == other.m_Font &&
                ChartHelper.IsValueEqualsColor(m_UnableColor, other.m_UnableColor) &&
                ChartHelper.IsValueEqualsColor(m_BackgroundColor, other.m_BackgroundColor) &&
                ChartHelper.IsValueEqualsColor(m_ContrastColor, other.m_ContrastColor) &&
                ChartHelper.IsValueEqualsColor(m_TextColor, other.m_TextColor) &&
                ChartHelper.IsValueEqualsColor(m_SubTextColor, other.m_SubTextColor) &&
                ChartHelper.IsValueEqualsColor(m_AxisLineColor, other.m_AxisLineColor) &&
                ChartHelper.IsValueEqualsColor(m_AxisSplitLineColor, other.m_AxisSplitLineColor) &&
                ChartHelper.IsValueEqualsColor(m_TooltipBackgroundColor, other.m_TooltipBackgroundColor) &&
                ChartHelper.IsValueEqualsColor(m_AxisSplitLineColor, other.m_AxisSplitLineColor) &&
                ChartHelper.IsValueEqualsColor(m_TooltipTextColor, other.m_TooltipTextColor) &&
                ChartHelper.IsValueEqualsColor(m_TooltipFlagAreaColor, other.m_TooltipFlagAreaColor) &&
                m_ColorPalette.Length == other.m_ColorPalette.Length;
        }

        public static bool operator ==(ThemeInfo point1, ThemeInfo point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(ThemeInfo point1, ThemeInfo point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}