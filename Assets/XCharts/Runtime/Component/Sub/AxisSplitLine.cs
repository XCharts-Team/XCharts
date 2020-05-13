/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Split line of axis in grid area.
    /// 坐标轴在 grid 区域中的分隔线。
    /// </summary>
    [Serializable]
    public class AxisSplitLine : SubComponent
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private int m_Interval;
        [SerializeField] private LineStyle m_LineStyle = new LineStyle(0.7f);

        /// <summary>
        /// Set this to true to show the split line.
        /// 是否显示分隔线。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        public int interval
        {
            get { return m_Interval; }
            set { if (PropertyUtility.SetStruct(ref m_Interval, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 线条样式
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (value != null) { m_LineStyle = value; SetVerticesDirty(); } }
        }

        public override bool vertsDirty { get { return m_VertsDirty || m_LineStyle.anyDirty; } }
        internal override void ClearVerticesDirty()
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
            axisSplitLine.lineStyle = lineStyle.Clone();
            return axisSplitLine;
        }

        public void Copy(AxisSplitLine splitLine)
        {
            show = splitLine.show;
            interval = splitLine.interval;
            lineStyle.Copy(splitLine.lineStyle);
        }

        internal Color GetColor(ThemeInfo theme)
        {
            if (!ChartHelper.IsClearColor(lineStyle.color))
            {
                var color = lineStyle.color;
                color.a *= lineStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.axisSplitLineColor;
                color.a *= lineStyle.opacity;
                return color;
            }
        }

        internal bool NeedShow(int index)
        {
            return show && (interval == 0 || index % (interval + 1) == 0);
        }
    }
}