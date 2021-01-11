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
    /// </summary>
    [Serializable]
    public class LineArrow : SubComponent
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
        [SerializeField]
        private Arrow m_Arrow = new Arrow()
        {
            width = 10,
            height = 15,
            offset = 0,
            dent = 3
        };

        /// <summary>
        /// Whether to show the arrow.
        /// 是否显示箭头。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The position of arrow.
        /// 箭头位置。
        /// </summary>
        public Position position
        {
            get { return m_Position; }
            set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the arrow of line.
        /// 箭头。
        /// </summary>
        public Arrow arrow
        {
            get { return m_Arrow; }
            set { if (PropertyUtil.SetClass(ref m_Arrow, value)) SetVerticesDirty(); }
        }
    }
}