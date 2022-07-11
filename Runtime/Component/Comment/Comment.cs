using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// comment of chart.
    /// |图表注解组件。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(CommentHander), true)]
    public class Comment : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle();
        [SerializeField] private CommentMarkStyle m_MarkStyle;
        [SerializeField] private List<CommentItem> m_Items = new List<CommentItem>() { new CommentItem() };

        /// <summary>
        /// Set this to false to prevent the comment from showing.
        /// |是否显示注解组件。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); } }
        /// <summary>
        /// The items of comment.
        /// |注解项。每个注解组件可以设置多个注解项。
        /// </summary>
        public List<CommentItem> items { get { return m_Items; } set { m_Items = value; SetComponentDirty(); } }
        /// <summary>
        /// The text style of all comments.
        /// |所有组件的文本样式。
        /// </summary>
        public LabelStyle labelStyle
        {
            get { return m_LabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_LabelStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The text style of all comments.
        /// |所有组件的文本样式。
        /// </summary>
        public CommentMarkStyle markStyle
        {
            get { return m_MarkStyle; }
            set { if (PropertyUtil.SetClass(ref m_MarkStyle, value)) SetVerticesDirty(); }
        }

        public LabelStyle GetLabelStyle(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                var labelStyle = items[index].labelStyle;
                if (labelStyle.show) return labelStyle;
            }
            return m_LabelStyle;
        }

        public CommentMarkStyle GetMarkStyle(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                var markStyle = items[index].markStyle;
                if (markStyle.show) return markStyle;
            }
            return m_MarkStyle;
        }
    }
}