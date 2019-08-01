
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Settings related to axis tick.
    /// 坐标轴刻度相关设置。
    /// </summary>
    [System.Serializable]
    public class AxisTick
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private bool m_AlignWithLabel;
        [SerializeField] private bool m_Inside;
        [SerializeField] private float m_Length;

        /// <summary>
        /// Set this to false to prevent the axis tick from showing.
        /// 是否显示坐标轴刻度。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Align axis tick with label, which is available only when boundaryGap is set to be true in category axis.
        /// 类目轴中在 boundaryGap 为 true 的时候有效，可以保证刻度线和标签对齐。
        /// </summary>
        public bool alignWithLabel { get { return m_AlignWithLabel; } set { m_AlignWithLabel = value; } }
        /// <summary>
        /// Set this to true so the axis labels face the inside direction.
        /// 坐标轴刻度是否朝内，默认朝外。
        /// </summary>
        public bool inside { get { return m_Inside; } set { m_Inside = value; } }
        /// <summary>
        /// The length of the axis tick.
        /// 坐标轴刻度的长度。
        /// </summary>
        public float length { get { return m_Length; } set { m_Length = value; } }

        public static AxisTick defaultTick
        {
            get
            {
                var tick = new AxisTick
                {
                    m_Show = true,
                    m_AlignWithLabel = false,
                    m_Inside = false,
                    m_Length = 5f
                };
                return tick;
            }
        }
    }
}