using System;
using System.Collections.Generic;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    [Serializable]
    public class BaseAxisTheme : ComponentTheme
    {
        [SerializeField] protected LineStyle.Type m_LineType = LineStyle.Type.Solid;
        [SerializeField] protected float m_LineWidth = 1f;
        [SerializeField] protected float m_LineLength = 0f;
        [SerializeField] protected Color32 m_LineColor;
        [SerializeField] protected LineStyle.Type m_SplitLineType = LineStyle.Type.Dashed;
        [SerializeField] protected float m_SplitLineWidth = 1f;
        [SerializeField] protected float m_SplitLineLength = 0f;
        [SerializeField] protected Color32 m_SplitLineColor;
        [SerializeField] protected float m_TickWidth = 1f;
        [SerializeField] protected float m_TickLength = 5f;
        [SerializeField] protected Color32 m_TickColor;
        [SerializeField] protected List<Color32> m_SplitAreaColors = new List<Color32>();

        /// <summary>
        /// the type of line.
        /// |坐标轴线类型。
        /// </summary>
        public LineStyle.Type lineType
        {
            get { return m_LineType; }
            set { if (PropertyUtil.SetStruct(ref m_LineType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of line.
        /// |坐标轴线宽。
        /// </summary>
        public float lineWidth
        {
            get { return m_LineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_LineWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the length of line.
        /// |坐标轴线长。
        /// </summary>
        public float lineLength
        {
            get { return m_LineLength; }
            set { if (PropertyUtil.SetStruct(ref m_LineLength, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of line.
        /// |坐标轴线颜色。
        /// </summary>
        public Color32 lineColor
        {
            get { return m_LineColor; }
            set { if (PropertyUtil.SetColor(ref m_LineColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the type of split line.
        /// |分割线线类型。
        /// </summary>
        public LineStyle.Type splitLineType
        {
            get { return m_SplitLineType; }
            set { if (PropertyUtil.SetStruct(ref m_SplitLineType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of split line.
        /// |分割线线宽。
        /// </summary>
        public float splitLineWidth
        {
            get { return m_SplitLineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_SplitLineWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the length of split line.
        /// |分割线线长。
        /// </summary>
        public float splitLineLength
        {
            get { return m_SplitLineLength; }
            set { if (PropertyUtil.SetStruct(ref m_SplitLineLength, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of line.
        /// |分割线线颜色。
        /// </summary>
        public Color32 splitLineColor
        {
            get { return m_SplitLineColor; }
            set { if (PropertyUtil.SetColor(ref m_SplitLineColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the length of tick.
        /// |刻度线线长。
        /// </summary>
        public float tickLength
        {
            get { return m_TickLength; }
            set { if (PropertyUtil.SetStruct(ref m_TickLength, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of tick.
        /// |刻度线线宽。
        /// </summary>
        public float tickWidth
        {
            get { return m_TickWidth; }
            set { if (PropertyUtil.SetStruct(ref m_TickWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of tick.
        /// |坐标轴线颜色。
        /// </summary>
        public Color32 tickColor
        {
            get { return m_TickColor; }
            set { if (PropertyUtil.SetColor(ref m_TickColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the colors of split area.
        /// |坐标轴分隔区域的颜色。
        /// </summary>
        public List<Color32> splitAreaColors
        {
            get { return m_SplitAreaColors; }
            set { if (value != null) { m_SplitAreaColors = value; SetVerticesDirty(); } }
        }

        public BaseAxisTheme(ThemeType theme) : base(theme)
        {
            m_FontSize = XCSettings.fontSizeLv4;
            m_LineType = XCSettings.axisLineType;
            m_LineWidth = XCSettings.axisLineWidth;
            m_LineLength = 0;
            m_SplitLineType = XCSettings.axisSplitLineType;
            m_SplitLineWidth = XCSettings.axisSplitLineWidth;
            m_SplitLineLength = 0;
            m_TickWidth = XCSettings.axisTickWidth;
            m_TickLength = XCSettings.axisTickLength;
            switch (theme)
            {
                case ThemeType.Default:
                    m_LineColor = ColorUtil.GetColor("#514D4D");
                    m_TickColor = ColorUtil.GetColor("#514D4D");
                    m_SplitLineColor = ColorUtil.GetColor("#51515120");
                    m_SplitAreaColors = new List<Color32>
                    {
                        new Color32(250, 250, 250, 77),
                        new Color32(200, 200, 200, 77)
                    };
                    break;
                case ThemeType.Light:
                    m_LineColor = ColorUtil.GetColor("#514D4D");
                    m_TickColor = ColorUtil.GetColor("#514D4D");
                    m_SplitLineColor = ColorUtil.GetColor("#51515120");
                    m_SplitAreaColors = new List<Color32>
                    {
                        new Color32(250, 250, 250, 77),
                        new Color32(200, 200, 200, 77)
                    };
                    break;
                case ThemeType.Dark:
                    m_LineColor = ColorUtil.GetColor("#B9B8CE");
                    m_TickColor = ColorUtil.GetColor("#B9B8CE");
                    m_SplitLineColor = ColorUtil.GetColor("#484753");
                    m_SplitAreaColors = new List<Color32>
                    {
                        new Color32(255, 255, 255, (byte) (0.02f * 255)),
                        new Color32(255, 255, 255, (byte) (0.05f * 255))
                    };
                    break;
            }
        }

        public void Copy(BaseAxisTheme theme)
        {
            base.Copy(theme);
            m_LineType = theme.lineType;
            m_LineWidth = theme.lineWidth;
            m_LineLength = theme.lineLength;
            m_LineColor = theme.lineColor;
            m_SplitLineType = theme.splitLineType;
            m_SplitLineWidth = theme.splitLineWidth;
            m_SplitLineLength = theme.splitLineLength;
            m_SplitLineColor = theme.splitLineColor;
            m_TickWidth = theme.tickWidth;
            m_TickLength = theme.tickLength;
            m_TickColor = theme.tickColor;
            ChartHelper.CopyList(m_SplitAreaColors, theme.splitAreaColors);
        }
    }

    [Serializable]
    public class AxisTheme : BaseAxisTheme
    {
        public AxisTheme(ThemeType theme) : base(theme)
        { }
    }

    [Serializable]
    public class RadiusAxisTheme : BaseAxisTheme
    {
        public RadiusAxisTheme(ThemeType theme) : base(theme)
        { }
    }

    [Serializable]
    public class AngleAxisTheme : BaseAxisTheme
    {
        public AngleAxisTheme(ThemeType theme) : base(theme)
        { }
    }

    [Serializable]
    public class PolarAxisTheme : BaseAxisTheme
    {
        public PolarAxisTheme(ThemeType theme) : base(theme)
        { }
    }

    [Serializable]
    public class RadarAxisTheme : BaseAxisTheme
    {
        public RadarAxisTheme(ThemeType theme) : base(theme)
        {
            m_SplitAreaColors.Clear();
            switch (theme)
            {
                case ThemeType.Dark:
                    m_SplitAreaColors.Add(ThemeStyle.GetColor("#6f6f6f"));
                    m_SplitAreaColors.Add(ThemeStyle.GetColor("#606060"));
                    break;
                case ThemeType.Default:
                    m_SplitAreaColors.Add(ThemeStyle.GetColor("#f6f6f6"));
                    m_SplitAreaColors.Add(ThemeStyle.GetColor("#e7e7e7"));
                    break;
                case ThemeType.Light:
                    m_SplitAreaColors.Add(ThemeStyle.GetColor("#f6f6f6"));
                    m_SplitAreaColors.Add(ThemeStyle.GetColor("#e7e7e7"));
                    break;
            }
        }
    }
}