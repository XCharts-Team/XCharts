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
    /// Settings related to axis tick.
    /// 坐标轴刻度相关设置。
    /// </summary>
    [System.Serializable]
    public class AxisTick : BaseLine
    {
        [SerializeField] private bool m_AlignWithLabel;
        [SerializeField] private bool m_Inside;
        [SerializeField] private bool m_ShowStartTick;
        [SerializeField] private bool m_ShowEndTick;

        /// <summary>
        /// Align axis tick with label, which is available only when boundaryGap is set to be true in category axis.
        /// 类目轴中在 boundaryGap 为 true 的时候有效，可以保证刻度线和标签对齐。
        /// </summary>
        public bool alignWithLabel
        {
            get { return m_AlignWithLabel; }
            set { if (PropertyUtil.SetStruct(ref m_AlignWithLabel, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Set this to true so the axis labels face the inside direction.
        /// 坐标轴刻度是否朝内，默认朝外。
        /// </summary>
        public bool inside
        {
            get { return m_Inside; }
            set { if (PropertyUtil.SetStruct(ref m_Inside, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to display the first tick.
        /// 是否显示第一个刻度。
        /// </summary>
        public bool showStartTick
        {
            get { return m_ShowStartTick; }
            set { if (PropertyUtil.SetStruct(ref m_ShowStartTick, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to display the last tick.
        /// 是否显示最后一个刻度。
        /// </summary>
        public bool showEndTick
        {
            get { return m_ShowEndTick; }
            set { if (PropertyUtil.SetStruct(ref m_ShowEndTick, value)) SetVerticesDirty(); }
        }

        public static AxisTick defaultTick
        {
            get
            {
                var tick = new AxisTick
                {
                    m_Show = true,
                    m_AlignWithLabel = false,
                    m_Inside = false,
                    m_ShowStartTick = false,
                    m_ShowEndTick = true
                };
                return tick;
            }
        }

        public AxisTick Clone()
        {
            var axisTick = new AxisTick();
            axisTick.show = show;
            axisTick.alignWithLabel = alignWithLabel;
            axisTick.inside = inside;
            axisTick.showStartTick = showStartTick;
            axisTick.showEndTick = showEndTick;
            axisTick.lineStyle = lineStyle.Clone();
            return axisTick;
        }

        public void Copy(AxisTick axisTick)
        {
            show = axisTick.show;
            alignWithLabel = axisTick.alignWithLabel;
            inside = axisTick.inside;
            showStartTick = axisTick.showStartTick;
            showEndTick = axisTick.showEndTick;
        }
    }
}