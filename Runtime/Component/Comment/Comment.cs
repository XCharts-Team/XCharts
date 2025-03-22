using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// The layer of comment.
    /// ||注解的显示层级。
    /// </summary>
    [Since("v3.15.0")]
    public enum CommentLayer
    {
        /// <summary>
        /// The comment is display under the serie.
        /// ||注解在系列下方。
        /// </summary>
        Lower,
        /// <summary>
        /// The comment is display above the serie.
        /// ||注解在系列上方。
        /// </summary>
        Upper
    }
    /// <summary>
    /// comment of chart. Used to annotate special information in the chart.
    /// ||图表注解组件。用于标注图表中的特殊信息。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(CommentHander), true)]
    public class Comment : MainComponent, IPropertyChanged
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField][Since("v3.15.0")] private CommentLayer m_Layer = CommentLayer.Lower;
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle();
        [SerializeField] private CommentMarkStyle m_MarkStyle;
        [SerializeField] private List<CommentItem> m_Items = new List<CommentItem>() { new CommentItem() };

        /// <summary>
        /// Set this to false to prevent the comment from showing.
        /// ||是否显示注解组件。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); } }
        /// <summary>
        /// The layer of comment.
        /// ||注解的显示层级。
        /// </summary>
        public CommentLayer layer { get { return m_Layer; } set { if (PropertyUtil.SetStruct(ref m_Layer, value)) SetComponentDirty(); } }
        /// <summary>
        /// The items of comment.
        /// ||注解项。每个注解组件可以设置多个注解项。
        /// </summary>
        public List<CommentItem> items { get { return m_Items; } set { m_Items = value; SetComponentDirty(); } }
        /// <summary>
        /// The text style of all comments.
        /// ||所有组件的文本样式。
        /// </summary>
        public LabelStyle labelStyle
        {
            get { return m_LabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_LabelStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The text style of all comments.
        /// ||所有组件的文本样式。
        /// </summary>
        public CommentMarkStyle markStyle
        {
            get { return m_MarkStyle; }
            set { if (PropertyUtil.SetClass(ref m_MarkStyle, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Get the label style of comment item.
        /// ||获取注解项的文本样式。
        /// </summary>
        /// <param name="index">the index of item</param>
        /// <returns></returns> 
        public LabelStyle GetLabelStyle(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                var labelStyle = items[index].labelStyle;
                if (labelStyle.show) return labelStyle;
            }
            return m_LabelStyle;
        }
        /// <summary>
        /// Get the mark style of comment item.
        /// ||获取注解项的标记样式。
        /// </summary>
        /// <param name="index">the index of item</param>
        /// <returns></returns>
        public CommentMarkStyle GetMarkStyle(int index)
        {
            if (index >= 0 && index < items.Count)
            {
                var markStyle = items[index].markStyle;
                if (markStyle.show) return markStyle;
            }
            return m_MarkStyle;
        }

        /// <summary>
        /// Callback handling when parameters change.
        /// ||参数变更时的回调处理。
        /// </summary>
        public void OnChanged()
        {
            foreach (var item in items)
            {
                item.location.OnChanged();
            }
        }
    }
}