
using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// the title of serie.
    /// 标题相关设置。
    /// </summary>
    [Serializable]
    public class TitleStyle : ChildComponent, ISerieDataComponent, ISerieExtraComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Vector2 m_OffsetCenter = new Vector2(0, -0.2f);
        [SerializeField] private TextStyle m_TextStyle = new TextStyle();

        /// <summary>
        /// Whether to show title.
        /// 是否显示标题。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The offset position relative to the center.
        /// 相对于中心的偏移位置。
        /// </summary>
        public Vector2 offsetCenter
        {
            get { return m_OffsetCenter; }
            set { if (PropertyUtil.SetStruct(ref m_OffsetCenter, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the color of text. 
        /// 文本的颜色。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value, true)) SetComponentDirty(); }
        }

        public override bool componentDirty { get { return m_ComponentDirty || textStyle.componentDirty; } }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            textStyle.ClearComponentDirty();
        }

        public Vector3 GetOffset(float radius)
        {
            var x = ChartHelper.GetActualValue(m_OffsetCenter.x, radius);
            var y = ChartHelper.GetActualValue(m_OffsetCenter.y, radius);
            return new Vector3(x, y, 0);
        }
    }
}