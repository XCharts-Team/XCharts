/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using System;

namespace XCharts
{
    /// <summary>
    /// Title component, including main title and subtitle.
    /// 标题组件，包含主标题和副标题。
    /// </summary>
    [Serializable]
    public class Title : MainComponent, IPropertyChanged
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_Text;
        [SerializeField] private TextStyle m_TextStyle = new TextStyle(16);
        [SerializeField] private string m_SubText;
        [SerializeField] private TextStyle m_SubTextStyle = new TextStyle(14);
        [SerializeField] private float m_ItemGap = 8;
        [SerializeField] private Location m_Location = Location.defaultTop;

        /// <summary>
        /// [default:true]
        /// Set this to false to prevent the title from showing.
        /// 是否显示标题组件。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetComponentDirty(); } }
        /// <summary>
        /// The main title text, supporting for \n for newlines.
        /// 主标题文本，支持使用 \n 换行。
        /// </summary>
        public string text { get { return m_Text; } set { if (PropertyUtility.SetClass(ref m_Text, value)) SetComponentDirty(); } }
        /// <summary>
        /// [default:16]
        /// main title font size.
        /// 主标题文字的字体大小。
        /// </summary>
        [Obsolete("use textStyle instead.", true)]
        public int textFontSize { get { return m_TextStyle.fontSize; } set { m_TextStyle.fontSize = value; } }
        /// <summary>
        /// 主标题文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtility.SetClass(ref m_TextStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Subtitle text, supporting for \n for newlines.
        /// 副标题文本，支持使用 \n 换行。
        /// </summary>
        public string subText
        {
            get { return m_SubText; }
            set { if (PropertyUtility.SetClass(ref m_SubText, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 副标题文本样式。
        /// </summary>
        public TextStyle subTextStyle
        {
            get { return m_SubTextStyle; }
            set { if (PropertyUtility.SetClass(ref m_SubTextStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// [default:14]
        /// subtitle font size.
        /// 副标题文字的字体大小。
        /// </summary>
        [Obsolete("use subTextStyle instead.", true)]
        public int subTextFontSize
        {
            get { return m_SubTextStyle.fontSize; }
            set { m_SubTextStyle.fontSize = value; }
        }
        /// <summary>
        /// [default:8]
        /// The gap between the main title and subtitle.
        /// 主副标题之间的间距。
        /// </summary>
        public float itemGap
        {
            get { return m_ItemGap; }
            set { if (PropertyUtility.SetStruct(ref m_ItemGap, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The location of title component.
        /// 标题显示位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtility.SetClass(ref m_Location, value)) SetComponentDirty(); }
        }

        public override bool vertsDirty { get { return false; } }
        public override bool componentDirty
        {
            get { return m_ComponentDirty || location.componentDirty || textStyle.componentDirty || subTextStyle.componentDirty; }
        }

        internal override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            location.ClearComponentDirty();
            textStyle.ClearComponentDirty();
            subTextStyle.ClearComponentDirty();
        }

        public static Title defaultTitle
        {
            get
            {
                var title = new Title
                {
                    m_Show = true,
                    m_Text = "Chart Title",
                    m_TextStyle = new TextStyle(16),
                    m_SubText = "",
                    m_SubTextStyle = new TextStyle(14),
                    m_ItemGap = 8,
                    m_Location = Location.defaultTop
                };
                return title;
            }
        }

        public void OnChanged()
        {
            m_Location.OnChanged();
        }
    }
}