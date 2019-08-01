using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// the global setting of pie chart.
    /// 饼图的全局设置。
    /// </summary>
    [System.Serializable]
    public class Pie
    {
        [SerializeField] private float m_TooltipExtraRadius;
        [SerializeField] private float m_SelectedOffset;
        /// <summary>
        /// the extra dadius of pie chart when the tooltip indicatored pie.
        /// 提示框指示时的额外半径。
        /// </summary>
        public float tooltipExtraRadius { get { return m_TooltipExtraRadius; } set { m_TooltipExtraRadius = value; } }
        /// <summary>
        /// the offset of pie when the pie item is selected.
        /// 饼图项被选中时的偏移。
        /// </summary>
        public float selectedOffset { get { return m_SelectedOffset; } set { m_SelectedOffset = value; } }

        public static Pie defaultPie
        {
            get
            {
                var pie = new Pie
                {
                    m_TooltipExtraRadius = 10f,
                    m_SelectedOffset = 10f,
                };
                return pie;
            }
        }
    }
}