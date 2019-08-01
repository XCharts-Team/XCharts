using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// The global settings of line chart.
    /// LineChart的全局配置。
    /// </summary>
    [System.Serializable]
    public class Line
    {
        /// <summary>
        /// the type fo step line.
        /// 阶梯线图类型。
        /// </summary>
        public enum StepType
        {
            /// <summary>
            /// 当前点。
            /// </summary>
            Start,
            /// <summary>
            /// 当前点和下一个点的中间。
            /// </summary>
            Middle,
            /// <summary>
            /// 下一个拐点
            /// </summary>
            End
        }
        [SerializeField] private float m_Tickness;
        [SerializeField] private bool m_Smooth;
        [SerializeField] [Range(1f, 10f)] private float m_SmoothStyle;
        [SerializeField] private bool m_Area;
        [SerializeField] private bool m_Step;
        [SerializeField] private StepType m_StepType;

        /// <summary>
        /// the tickness of lines.
        /// 线条粗细。
        /// </summary>
        public float tickness { get { return m_Tickness; } set { m_Tickness = value; } }
        /// <summary>
        /// smoothness.
        /// 平滑风格。
        /// </summary>
        public float smoothStyle { get { return m_SmoothStyle; } set { m_SmoothStyle = value; } }
        /// <summary>
        /// Whether the lines are displayed smoothly.
        /// 是否平滑显示。
        /// </summary>
        public bool smooth { get { return m_Smooth; } set { m_Smooth = value; } }
        /// <summary>
        /// Whether to show area.
        /// 是否显示区域填充颜色。
        /// </summary>
        public bool area { get { return m_Area; } set { m_Area = value; } }
        /// <summary>
        /// Whether to show as a step line. 
        /// 是否是阶梯线图。
        /// </summary>
        public bool step { get { return m_Step; } set { m_Step = value; } }
        /// <summary>
        /// the type of step line.
        /// 阶梯线图类型。
        /// </summary>
        public StepType stepTpe { get { return m_StepType; } set { m_StepType = value; } }

        public static Line defaultLine
        {
            get
            {
                var line = new Line
                {
                    m_Tickness = 0.8f,
                    m_Smooth = false,
                    m_SmoothStyle = 2f,
                    m_Area = false,
                    m_Step = false,
                    m_StepType = StepType.Middle
                };
                return line;
            }
        }
    }
}