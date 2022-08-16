using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Configurations of emphasis state. 
    /// |高亮状态样式。
    /// </summary>
    [System.Serializable]
    [Since("v3.2.0")]
    public class EmphasisStyle : StateStyle, ISerieExtraComponent, ISerieDataComponent
    {
        /// <summary>
        /// focus type. 
        /// |聚焦类型。
        /// </summary>
        public enum FocusType
        {
            /// <summary>
            /// Do not fade out other data, it's by default.
            /// |不淡出其它图形，默认使用该配置。
            /// </summary>
            None,
            /// <summary>
            /// Only focus (not fade out) the element of the currently highlighted data.
            /// |只聚焦（不淡出）当前高亮的数据的图形。
            /// </summary>
            Self,
            /// <summary>
            /// Focus on all elements of the series which the currently highlighted data belongs to.
            /// |聚焦当前高亮的数据所在的系列的所有图形。
            /// </summary>
            Series
        }
        /// <summary>
        /// blur scope.
        /// |淡出范围。
        /// </summary>
        public enum BlurScope
        {
            /// <summary>
            /// coordinate system.
            /// |淡出范围为坐标系，默认使用该配置。
            /// </summary>
            GridCoord,
            /// <summary>
            /// series.
            /// |淡出范围为系列。
            /// </summary>
            Series,
            /// <summary>
            /// global.
            /// |淡出范围为全局。
            /// </summary>
            Global
        }

        [SerializeField] private float m_Scale = 1.1f;
        [SerializeField] private FocusType m_Focus = FocusType.None;
        [SerializeField] private BlurScope m_BlurScope = BlurScope.GridCoord;

        /// <summary>
        /// Whether to scale to highlight the data in emphasis state.
        /// |高亮时的缩放倍数。
        /// </summary>
        public float scale
        {
            get { return m_Scale; }
            set { if (PropertyUtil.SetStruct(ref m_Scale, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// When the data is highlighted, whether to fade out of other data to focus the highlighted. 
        /// |在高亮图形时，是否淡出其它数据的图形已达到聚焦的效果。
        /// </summary>
        public FocusType focus
        {
            get { return m_Focus; }
            set { if (PropertyUtil.SetStruct(ref m_Focus, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The range of fade out when focus is enabled.
        /// |在开启focus的时候，可以通过blurScope配置淡出的范围。
        /// </summary>
        public BlurScope blurScope
        {
            get { return m_BlurScope; }
            set { if (PropertyUtil.SetStruct(ref m_BlurScope, value)) SetVerticesDirty(); }
        }
    }
}