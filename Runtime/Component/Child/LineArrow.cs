using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class LineArrow : ChildComponent, ISerieExtraComponent
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
        [SerializeField] private Position m_Position;
        [SerializeField]
        private ArrowStyle m_Arrow = new ArrowStyle()
        {
            width = 10,
            height = 15,
            offset = 0,
            dent = 3
        };

        /// <summary>
        /// Whether to show the arrow.
        /// |是否显示箭头。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The position of arrow.
        /// |箭头位置。
        /// </summary>
        public Position position
        {
            get { return m_Position; }
            set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the arrow of line.
        /// |箭头。
        /// </summary>
        public ArrowStyle arrow
        {
            get { return m_Arrow; }
            set { if (PropertyUtil.SetClass(ref m_Arrow, value)) SetVerticesDirty(); }
        }
    }
}