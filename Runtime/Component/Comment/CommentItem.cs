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
        [SerializeField] private Rect m_MarkRect;
        [SerializeField] private CommentMarkStyle m_MarkStyle = new CommentMarkStyle() { show = false };
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle() { show = false };
        [SerializeField] [Since("v3.5.0")]private Location m_Location = new Location() { align = Location.Align.TopLeft, top = 0.125f };


        /// <summary>
        /// Set this to false to prevent this comment item from showing.
        /// |是否显示当前注解项。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); } }
        /// <summary>
        /// content of comment.
        /// |注解的文本内容。支持模板参数，可以参考Tooltip的itemFormatter。
        /// </summary>
        public string content { get { return m_Content; } set { if (PropertyUtil.SetClass(ref m_Content, value)) SetComponentDirty(); } }
        /// <summary>
        /// the mark rect of comment.
        /// |注解区域。
        /// </summary>
        public Rect markRect { get { return m_MarkRect; } set { if (PropertyUtil.SetStruct(ref m_MarkRect, value)) SetVerticesDirty(); } }
        /// <summary>
        /// the mark rect style.
        /// |注解标记区域样式。
        /// </summary>
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
        /// <summary>
        /// The location of comment.
        /// |Comment显示的位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtil.SetClass(ref m_Location, value)) SetComponentDirty(); }
        }
    }
}