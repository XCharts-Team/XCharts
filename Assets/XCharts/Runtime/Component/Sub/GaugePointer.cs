/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
        /// 是否显示指针。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 指针长度。可以是绝对值，也可以是相对于半径的百分比（0-1的浮点数）
        /// </summary>
        /// <value></value>
        public float length
        {
            get { return m_Length; }
            set { if (PropertyUtility.SetStruct(ref m_Length, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 指针宽度。
        /// </summary>
        /// <value></value>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtility.SetStruct(ref m_Width, value)) SetVerticesDirty(); }
        }
    }
}