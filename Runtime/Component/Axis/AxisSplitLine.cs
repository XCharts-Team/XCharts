using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Split line of axis in grid area.
    /// ||坐标轴在 grid 区域中的分隔线。
    /// </summary>
    [Serializable]
    public class AxisSplitLine : BaseLine
    {
        [SerializeField] private int m_Interval;
        [SerializeField] private float m_Distance;
        [SerializeField] private bool m_AutoColor;
        [SerializeField][Since("v3.3.0")] private bool m_ShowStartLine = true;
        [SerializeField][Since("v3.3.0")] private bool m_ShowEndLine = true;
        [SerializeField][Since("v3.11.0")] private bool m_ShowZLine = true;

        /// <summary>
        /// The distance between the split line and axis line.
        /// ||刻度线与轴线的距离。
        /// </summary>
        public float distance { get { return m_Distance; } set { m_Distance = value; } }
        /// <summary>
        /// auto color.
        /// ||自动设置颜色。
        /// </summary>
        public bool autoColor { get { return m_AutoColor; } set { m_AutoColor = value; } }
        /// <summary>
        /// Interval of Axis splitLine.
        /// ||坐标轴分隔线的显示间隔。
        /// </summary>
        public int interval
        {
            get { return m_Interval; }
            set { if (PropertyUtil.SetStruct(ref m_Interval, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show the first split line.
        /// ||是否显示第一条分割线。
        /// </summary>
        public bool showStartLine
        {
            get { return m_ShowStartLine; }
            set { if (PropertyUtil.SetStruct(ref m_ShowStartLine, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show the last split line.
        /// ||是否显示最后一条分割线。
        /// </summary>
        public bool showEndLine
        {
            get { return m_ShowEndLine; }
            set { if (PropertyUtil.SetStruct(ref m_ShowEndLine, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show the Z axis part of the split line. Generally used for 3D coordinate systems.
        /// ||是否显示Z轴部分分割线。一般用于3D坐标系。
        /// </summary> 
        public bool showZLine
        {
            get { return m_ShowZLine; }
            set { if (PropertyUtil.SetStruct(ref m_ShowZLine, value)) SetVerticesDirty(); }
        }

        public override bool vertsDirty { get { return m_VertsDirty || m_LineStyle.anyDirty; } }
        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            m_LineStyle.ClearVerticesDirty();
        }
        public static AxisSplitLine defaultSplitLine
        {
            get
            {
                return new AxisSplitLine()
                {
                    m_Show = false,
                };
            }
        }

        public AxisSplitLine Clone()
        {
            var axisSplitLine = new AxisSplitLine();
            axisSplitLine.show = show;
            axisSplitLine.interval = interval;
            axisSplitLine.showStartLine = showStartLine;
            axisSplitLine.showEndLine = showEndLine;
            axisSplitLine.lineStyle = lineStyle.Clone();
            return axisSplitLine;
        }

        public void Copy(AxisSplitLine splitLine)
        {
            base.Copy(splitLine);
            interval = splitLine.interval;
            showStartLine = splitLine.showStartLine;
            showEndLine = splitLine.showEndLine;
        }

        internal bool NeedShow(int index, int total)
        {
            if (!show) return false;
            if (interval != 0 && index % (interval + 1) != 0) return false;
            if (!showStartLine && index == 0) return false;
            if (!showEndLine && index == total - 1) return false;
            return true;
        }
    }
}