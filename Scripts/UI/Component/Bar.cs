using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// bar component.global setting for bar chart.
    /// 柱状图的全局配置组件。
    /// </summary>
    [System.Serializable]
    public class Bar
    {
        [SerializeField] private bool m_InSameBar = false;
        [SerializeField] private float m_BarWidth = 0.7f;
        [SerializeField] private float m_Space = 10;

        /// <summary>
        /// Whether to draw all bar in the same bar,but not stacked.
        /// 非堆叠同柱。多序列绘制在同一bar上，但不堆叠，而是覆盖绘制。
        /// </summary>
        public bool inSameBar { get { return m_InSameBar; } set { m_InSameBar = value; } }
        /// <summary>
        /// the width of bar.
        /// 状态的宽度。
        /// </summary>
        public float barWidth { get { return m_BarWidth; } set { m_BarWidth = value; } }
        /// <summary>
        /// the space of bars.
        /// 多柱状间的间距。
        /// </summary>
        public float space { get { return m_Space; } set { m_Space = value; } }

        public static Bar defaultBar
        {
            get
            {
                return new Bar()
                {
                    m_InSameBar = false,
                    m_BarWidth = 0.6f,
                    m_Space = 10
                };
            }
        }
    }
}