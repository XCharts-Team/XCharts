using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Title component, including main title and subtitle.
    /// ||标题组件，包含主标题和副标题。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(TitleHander), true)]
    public class Title : MainComponent, IPropertyChanged
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_Text = "Chart Title";
        [SerializeField] private string m_SubText = "";
        [SerializeField] private LabelStyle m_LabelStyle = new LabelStyle();
        [SerializeField] private LabelStyle m_SubLabelStyle = new LabelStyle();
        [SerializeField] private float m_ItemGap = 0;
        [SerializeField] private Location m_Location = Location.defaultTop;

        /// <summary>
        /// [default:true]
        /// Set this to false to prevent the title from showing.
        /// ||是否显示标题组件。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); } }
        /// <summary>
        /// The main title text, supporting \n for newlines.
        /// ||主标题文本，支持使用 \n 换行。
        /// </summary>
        public string text { get { return m_Text; } set { if (PropertyUtil.SetClass(ref m_Text, value)) SetComponentDirty(); } }
        /// <summary>
        /// The text style of main title.
        /// ||主标题文本样式。
        /// </summary>
        public LabelStyle labelStyle
        {
            get { return m_LabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_LabelStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Subtitle text, supporting for \n for newlines.
        /// ||副标题文本，支持使用 \n 换行。
        /// </summary>
        public string subText
        {
            get { return m_SubText; }
            set { if (PropertyUtil.SetClass(ref m_SubText, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The text style of sub title.
        /// ||副标题文本样式。
        /// </summary>
        public LabelStyle subLabelStyle
        {
            get { return m_SubLabelStyle; }
            set { if (PropertyUtil.SetClass(ref m_SubLabelStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// [default:8]
        /// The gap between the main title and subtitle.
        /// ||主副标题之间的间距。
        /// </summary>
        public float itemGap
        {
            get { return m_ItemGap; }
            set { if (PropertyUtil.SetStruct(ref m_ItemGap, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The location of title component.
        /// ||标题显示位置。
        /// </summary>
        public Location location
        {
            get { return m_Location; }
            set { if (PropertyUtil.SetClass(ref m_Location, value)) SetComponentDirty(); }
        }

        public override bool vertsDirty { get { return false; } }
        public override bool componentDirty
        {
            get
            {
                return m_ComponentDirty ||
                    location.componentDirty ||
                    m_LabelStyle.componentDirty ||
                    m_SubLabelStyle.componentDirty;
            }
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            location.ClearComponentDirty();
            m_LabelStyle.ClearComponentDirty();
            m_SubLabelStyle.ClearComponentDirty();
        }

        public void OnChanged()
        {
            m_Location.OnChanged();
        }
    }
}