using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class LineArrow
    {
        public enum Position
        {
            /// <summary>
            /// 末端箭头
            /// </summary>
            End,
            /// <summary>
            /// 头端箭头
            /// </summary>
            Start
        }
        [SerializeField] private bool m_Show;
        [SerializeField] Position m_Position;
        [SerializeField] private float m_Width = 10;
        [SerializeField] private float m_Height = 15;
        [SerializeField] private float m_Offset = 0;
        [SerializeField] private float m_Dent = 3;

        /// <summary>
        /// Whether to show the arrow.
        /// 是否显示箭头。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// The position of arrow.
        /// 箭头位置。
        /// </summary>
        public Position position { get { return m_Position; } set { m_Position = value; } }
        /// <summary>
        /// The widht of arrow.
        /// 箭头宽。
        /// </summary>
        public float width { get { return m_Width; } set { m_Width = value; } }
        /// <summary>
        /// The height of arrow.
        /// 箭头高。
        /// </summary>
        public float height { get { return m_Height; } set { m_Height = value; } }
        /// <summary>
        /// The offset of arrow.
        /// 箭头偏移。
        /// </summary>
        public float offset { get { return m_Offset; } set { m_Offset = value; } }
        /// <summary>
        /// The dent of arrow.
        /// 箭头的凹度。
        /// </summary>
        public float dent { get { return m_Dent; } set { m_Dent = value; } }
    }
}