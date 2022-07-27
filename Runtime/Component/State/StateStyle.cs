using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the state style of serie.
    /// |Serie的状态样式。Serie的状态有正常，高亮，淡出，选中四种状态。
    /// </summary>
    [System.Serializable]
    [Since("v3.2.0")]
    public class StateStyle : ChildComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private LabelStyle m_Label = new LabelStyle();
        [SerializeField] private LabelLine m_LabelLine = new LabelLine();
        [SerializeField] private ItemStyle m_ItemStyle = new ItemStyle();
        [SerializeField] private LineStyle m_LineStyle = new LineStyle();
        [SerializeField] private AreaStyle m_AreaStyle = new AreaStyle();
        [SerializeField] private SerieSymbol m_Symbol = new SerieSymbol();

        public void Reset()
        {
            m_Show = false;
            m_Label.Reset();
            m_LabelLine.Reset();
            m_ItemStyle.Reset();
            m_Symbol.Reset();
        }

        /// <summary>
        /// 是否启用高亮样式。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { m_Show = value; }
        }
        /// <summary>
        /// 图形文本标签。
        /// </summary>
        public LabelStyle label
        {
            get { return m_Label; }
            set { if (PropertyUtil.SetClass(ref m_Label, value, true)) SetAllDirty(); }
        }
        /// <summary>
        /// 图形文本引导线样式。
        /// </summary>
        public LabelLine labelLine
        {
            get { return m_LabelLine; }
            set { if (PropertyUtil.SetClass(ref m_LabelLine, value, true)) SetAllDirty(); }
        }
        /// <summary>
        /// 图形样式。
        /// </summary>
        public ItemStyle itemStyle
        {
            get { return m_ItemStyle; }
            set { if (PropertyUtil.SetClass(ref m_ItemStyle, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 折线样式。
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (PropertyUtil.SetClass(ref m_LineStyle, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 区域样式。
        /// </summary>
        public AreaStyle areaStyle
        {
            get { return m_AreaStyle; }
            set { if (PropertyUtil.SetClass(ref m_AreaStyle, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 标记样式。
        /// </summary>
        public SerieSymbol symbol
        {
            get { return m_Symbol; }
            set { if (PropertyUtil.SetClass(ref m_Symbol, value, true)) SetVerticesDirty(); }
        }

        public override bool vertsDirty
        {
            get
            {
                return m_VertsDirty ||
                    m_Label.vertsDirty ||
                    m_ItemStyle.vertsDirty ||
                    m_LineStyle.vertsDirty ||
                    m_AreaStyle.vertsDirty ||
                    m_Symbol.vertsDirty;
            }
        }

        public override bool componentDirty
        {
            get
            {
                return m_ComponentDirty ||
                    m_Label.componentDirty;
            }
        }

        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            m_Label.ClearVerticesDirty();
            m_ItemStyle.ClearVerticesDirty();
            m_LineStyle.ClearVerticesDirty();
            m_AreaStyle.ClearVerticesDirty();
            m_Symbol.ClearVerticesDirty();
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            m_Label.ClearComponentDirty();
            m_Symbol.ClearComponentDirty();
        }
    }
}