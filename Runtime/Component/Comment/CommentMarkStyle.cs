using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// comment of chart.
    /// |注解项。
    /// </summary>
    [Serializable]
    public class CommentMarkStyle : ChildComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private LineStyle m_LineStyle;

        /// <summary>
        /// Set this to false to prevent this comment item from showing.
        /// |是否显示当前注解项。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); } }
        public LineStyle lineStyle { get { return m_LineStyle; } set { if (PropertyUtil.SetClass(ref m_LineStyle, value)) SetVerticesDirty(); } }
    }
}