/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/


using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Settings related to axis tick.
    /// 坐标轴刻度相关设置。
    /// </summary>
    [System.Serializable]
    public class AxisTick : SubComponent
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private bool m_AlignWithLabel;
        [SerializeField] private bool m_Inside;
        [SerializeField] private float m_Length;
        [SerializeField] private float m_Width;

        /// <summary>
        /// Set this to false to prevent the axis tick from showing.
        /// 是否显示坐标轴刻度。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Align axis tick with label, which is available only when boundaryGap is set to be true in category axis.
        /// 类目轴中在 boundaryGap 为 true 的时候有效，可以保证刻度线和标签对齐。
        /// </summary>
        public bool alignWithLabel
        {
            get { return m_AlignWithLabel; }
            set { if (PropertyUtility.SetStruct(ref m_AlignWithLabel, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Set this to true so the axis labels face the inside direction.
        /// 坐标轴刻度是否朝内，默认朝外。
        /// </summary>
        public bool inside
        {
            get { return m_Inside; }
            set { if (PropertyUtility.SetStruct(ref m_Inside, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The length of the axis tick.
        /// 坐标轴刻度的长度。
        /// </summary>
        public float length
        {
            get { return m_Length; }
            set { if (PropertyUtility.SetStruct(ref m_Length, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The width of the axis tick.Keep the same width with axis line when default 0.
        /// 坐标轴刻度的宽度。默认为0时宽度和坐标轴一致。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtility.SetStruct(ref m_Width, value)) SetVerticesDirty(); }
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
                    m_Width = 0f,
                    m_Length = 5f
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
            axisTick.length = length;
            axisTick.width = width;
            return axisTick;
        }

        public void Copy(AxisTick axisTick)
        {
            show = axisTick.show;
            alignWithLabel = axisTick.alignWithLabel;
            inside = axisTick.inside;
            length = axisTick.length;
            width = axisTick.width;
        }
    }
}