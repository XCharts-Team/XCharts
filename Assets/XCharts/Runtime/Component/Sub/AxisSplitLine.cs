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
        public bool show { get { return m_Show; } set { m_Show = value; } }
        public int interval { get { return m_Interval; } set { m_Interval = value; } }
        /// <summary>
        /// 线条样式
        /// </summary>
        public LineStyle lineStyle { get { return m_LineStyle; } set { if (value != null) m_LineStyle = value; } }

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

        public void Copy(AxisSplitLine other)
        {
            m_Show = other.show;
            m_Interval = other.interval;
            m_LineStyle.Copy(other.m_LineStyle);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (AxisSplitLine)obj;
            return m_Show == other.show &&
                m_Interval == other.interval &&
                m_LineStyle.Equals(other.lineStyle);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        internal Color GetColor(ThemeInfo theme)
        {
            if (lineStyle.color != Color.clear)
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
            return interval == 0 || index % (interval + 1) == 0;
        }
    }
}