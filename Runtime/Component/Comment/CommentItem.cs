using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// comment of chart.
    /// |注解项。
    /// </summary>
    [Serializable]
    public class CommentItem : ChildComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_Content = "comment";
        [SerializeField] private Vector3 m_Position;
        [SerializeField] private Rect m_MarkRect;
        [SerializeField] private CommentMarkStyle m_MarkStyle = new CommentMarkStyle() { show = false };
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle() { show = false };

        /// <summary>
        /// Set this to false to prevent this comment item from showing.
        /// |是否显示当前注解项。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); } }
        /// <summary>
        /// position of comment.
        /// |注解项的位置坐标。
        /// </summary>
        public Vector3 position { get { return m_Position; } set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetComponentDirty(); } }
        /// <summary>
        /// content of comment.
        /// |注解的文本内容。
        /// </summary>
        public string content { get { return m_Content; } set { if (PropertyUtil.SetClass(ref m_Content, value)) SetComponentDirty(); } }
        public Rect markRect { get { return m_MarkRect; } set { if (PropertyUtil.SetStruct(ref m_MarkRect, value)) SetVerticesDirty(); } }
        public CommentMarkStyle markStyle { get { return m_MarkStyle; } set { if (PropertyUtil.SetClass(ref m_MarkStyle, value)) SetVerticesDirty(); } }
        /// <summary>
        /// The text style of all comments.
        /// |注解项的文本样式。
        /// </summary>
        public LabelStyle labelStyle
        {
            get { return m_LabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_LabelStyle, value)) SetComponentDirty(); }
        }
    }
}