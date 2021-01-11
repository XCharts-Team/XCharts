/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Settings related to axis line.
    /// 坐标轴轴线。
    /// </summary>
    [System.Serializable]
    public class AxisLine : BaseLine
    {
        [SerializeField] private bool m_OnZero;
        [SerializeField] private bool m_ShowArrow;
        [SerializeField] private Arrow m_Arrow = new Arrow();

        /// <summary>
        /// When mutiple axes exists, this option can be used to specify which axis can be "onZero" to.
        /// X 轴或者 Y 轴的轴线是否在另一个轴的 0 刻度上，只有在另一个轴为数值轴且包含 0 刻度时有效。
        /// </summary>
        public bool onZero
        {
            get { return m_OnZero; }
            set { if (PropertyUtil.SetStruct(ref m_OnZero, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show the arrow symbol of axis. 
        /// 是否显示箭头。
        /// </summary>
        public bool showArrow
        {
            get { return m_ShowArrow; }
            set { if (PropertyUtil.SetStruct(ref m_ShowArrow, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the arrow of line.
        /// 轴线箭头。
        /// </summary>
        public Arrow arrow
        {
            get { return m_Arrow; }
            set { if (PropertyUtil.SetClass(ref m_Arrow, value)) SetVerticesDirty(); }
        }
        public static AxisLine defaultAxisLine
        {
            get
            {
                var axisLine = new AxisLine
                {
                    m_Show = true,
                    m_OnZero = true,
                    m_ShowArrow = false,
                    m_Arrow = new Arrow(),
                    m_LineStyle = new LineStyle(LineStyle.Type.None),
                };
                return axisLine;
            }
        }

        public AxisLine Clone()
        {
            var axisLine = new AxisLine();
            axisLine.show = show;
            axisLine.onZero = onZero;
            axisLine.showArrow = showArrow;
            axisLine.arrow = arrow.Clone();
            return axisLine;
        }

        public void Copy(AxisLine axisLine)
        {
            base.Copy(axisLine);
            onZero = axisLine.onZero;
            showArrow = axisLine.showArrow;
            arrow.Copy(axisLine.arrow);
        }
    }
}