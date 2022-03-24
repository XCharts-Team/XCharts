
using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    public class LabelLine : ChildComponent, ISerieExtraComponent, ISerieDataComponent
    {
        /// <summary>
        /// 标签视觉引导线类型
        /// </summary>
        public enum LineType
        {
            /// <summary>
            /// 折线
            /// </summary>
            BrokenLine,
            /// <summary>
            /// 曲线
            /// </summary>
            Curves,
            /// <summary>
            /// 水平线
            /// </summary>
            HorizontalLine
        }

        [SerializeField] private bool m_Show = true;
        [SerializeField] private LineType m_LineType = LineType.BrokenLine;
        [SerializeField] private Color32 m_LineColor = ChartConst.clearColor32;
        [SerializeField] private float m_LineWidth = 1.0f;
        [SerializeField] private float m_LineGap = 1.0f;
        [SerializeField] private float m_LineLength1 = 25f;
        [SerializeField] private float m_LineLength2 = 15f;

        public void Reset()
        {
            m_Show = false;
            m_LineType = LineType.BrokenLine;
            m_LineColor = Color.clear;
            m_LineWidth = 1.0f;
            m_LineGap = 1.0f;
            m_LineLength1 = 25f;
            m_LineLength2 = 15f;
        }

        /// <summary>
        /// Whether the label line is showed.
        /// |是否显示视觉引导线。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the type of visual guide line.
        /// |视觉引导线类型。
        /// </summary>
        public LineType lineType
        {
            get { return m_LineType; }
            set { if (PropertyUtil.SetStruct(ref m_LineType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of visual guild line.
        /// |视觉引导线颜色。默认和serie一致取自调色板。
        /// </summary>
        public Color32 lineColor
        {
            get { return m_LineColor; }
            set { if (PropertyUtil.SetStruct(ref m_LineColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of visual guild line.
        /// |视觉引导线的宽度。
        /// </summary>
        public float lineWidth
        {
            get { return m_LineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_LineWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the gap of container and guild line.
        /// |视觉引导线和容器的间距。
        /// </summary>
        public float lineGap
        {
            get { return m_LineGap; }
            set { if (PropertyUtil.SetStruct(ref m_LineGap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The length of the first segment of visual guide line.
        /// |视觉引导线第一段的长度。
        /// </summary>
        public float lineLength1
        {
            get { return m_LineLength1; }
            set { if (PropertyUtil.SetStruct(ref m_LineLength1, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The length of the second segment of visual guide line.
        /// |视觉引导线第二段的长度。
        /// </summary>
        public float lineLength2
        {
            get { return m_LineLength2; }
            set { if (PropertyUtil.SetStruct(ref m_LineLength2, value)) SetVerticesDirty(); }
        }
    }
}
