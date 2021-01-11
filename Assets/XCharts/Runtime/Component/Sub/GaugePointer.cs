/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{

    /// <summary>
    /// Settings related to gauge pointer.
    /// 仪表盘指针相关设置。
    /// </summary>
    [System.Serializable]
    public class GaugePointer : SubComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private float m_Length = 0.8f;
        [SerializeField] private float m_Width = 15;

        /// <summary>
        /// Whether to display a pointer.
        /// 是否显示指针。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Pointer length. It can be an absolute value, or it can be a percentage relative to the radius (0-1). 
        /// 指针长度。可以是绝对值，也可以是相对于半径的百分比（0-1的浮点数）。
        /// </summary>
        public float length
        {
            get { return m_Length; }
            set { if (PropertyUtil.SetStruct(ref m_Length, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Pointer width.
        /// 指针宽度。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetVerticesDirty(); }
        }
    }
}