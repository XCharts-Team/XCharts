/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Split line of axis in grid area.
    /// 坐标轴在 grid 区域中的分隔线。
    /// </summary>
    [Serializable]
    public class AxisSplitLine : BaseLine
    {
        [SerializeField] private int m_Interval;

        public int interval
        {
            get { return m_Interval; }
            set { if (PropertyUtil.SetStruct(ref m_Interval, value)) SetVerticesDirty(); }
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
            axisSplitLine.lineStyle = lineStyle.Clone();
            return axisSplitLine;
        }

        public void Copy(AxisSplitLine splitLine)
        {
            base.Copy(splitLine);
            interval = splitLine.interval;
        }

        internal bool NeedShow(int index)
        {
            return show && (interval == 0 || index % (interval + 1) == 0);
        }
    }
}