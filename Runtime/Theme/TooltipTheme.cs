using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [Serializable]
    public class TooltipTheme : ComponentTheme
    {

        [SerializeField] protected LineStyle.Type m_LineType = LineStyle.Type.Solid;
        [SerializeField] protected float m_LineWidth = 1f;
        [SerializeField] protected Color32 m_LineColor;
        [SerializeField] protected Color32 m_AreaColor;
        [SerializeField] protected Color32 m_LabelTextColor;
        [SerializeField] protected Color32 m_LabelBackgroundColor;

        /// <summary>
        /// the type of line.
        /// ||坐标轴线类型。
        /// </summary>
        public LineStyle.Type lineType
        {
            get { return m_LineType; }
            set { if (PropertyUtil.SetStruct(ref m_LineType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of line.
        /// ||指示线线宽。
        /// </summary>
        public float lineWidth
        {
            get { return m_LineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_LineWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of line.
        /// ||指示线颜色。
        /// </summary>
        public Color32 lineColor
        {
            get { return m_LineColor; }
            set { if (PropertyUtil.SetColor(ref m_LineColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the color of line.
        /// ||区域指示的颜色。
        /// </summary>
        public Color32 areaColor
        {
            get { return m_AreaColor; }
            set { if (PropertyUtil.SetColor(ref m_AreaColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the text color of tooltip cross indicator's axis label.
        /// ||十字指示器坐标轴标签的文本颜色。
        /// </summary>
        public Color32 labelTextColor
        {
            get { return m_LabelTextColor; }
            set { if (PropertyUtil.SetColor(ref m_LabelTextColor, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the background color of tooltip cross indicator's axis label.
        /// ||十字指示器坐标轴标签的背景颜色。
        /// </summary>
        public Color32 labelBackgroundColor
        {
            get { return m_LabelBackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_LabelBackgroundColor, value)) SetComponentDirty(); }
        }

        public TooltipTheme(ThemeType theme) : base(theme)
        {
            m_LineType = LineStyle.Type.Solid;
            m_LineWidth = XCSettings.tootipLineWidth;
            switch (theme)
            {
                case ThemeType.Default:
                    m_TextBackgroundColor = ColorUtil.GetColor("#FFFFFFFF");
                    m_TextColor = ColorUtil.GetColor("#000000FF");
                    m_AreaColor = ColorUtil.GetColor("#51515120");
                    m_LabelTextColor = ColorUtil.GetColor("#FFFFFFFF");
                    m_LabelBackgroundColor = ColorUtil.GetColor("#292929FF");
                    m_LineColor = ColorUtil.GetColor("#29292964");
                    break;
                case ThemeType.Light:
                    m_TextBackgroundColor = ColorUtil.GetColor("#FFFFFFFF");
                    m_TextColor = ColorUtil.GetColor("#000000FF");
                    m_AreaColor = ColorUtil.GetColor("#51515120");
                    m_LabelTextColor = ColorUtil.GetColor("#FFFFFFFF");
                    m_LabelBackgroundColor = ColorUtil.GetColor("#292929FF");
                    m_LineColor = ColorUtil.GetColor("#29292964");
                    break;
                case ThemeType.Dark:
                    m_TextBackgroundColor = ColorUtil.GetColor("#FFFFFFFF");
                    m_TextColor = ColorUtil.GetColor("#000000FF");
                    m_AreaColor = ColorUtil.GetColor("#51515120");
                    m_LabelTextColor = ColorUtil.GetColor("#FFFFFFFF");
                    m_LabelBackgroundColor = ColorUtil.GetColor("#292929FF");
                    m_LineColor = ColorUtil.GetColor("#29292964");
                    break;
            }
        }

        public void Copy(TooltipTheme theme)
        {
            base.Copy(theme);
            m_LineType = theme.lineType;
            m_LineWidth = theme.lineWidth;
            m_LineColor = theme.lineColor;
            m_AreaColor = theme.areaColor;
            m_LabelTextColor = theme.labelTextColor;
            m_LabelBackgroundColor = theme.labelBackgroundColor;
        }
    }
}