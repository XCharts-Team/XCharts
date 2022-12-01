using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Marquee style. It can be used for the DataZoom component.
    /// 选取框样式。可用于DataZoom组件。
    /// </summary>
    [Since("v3.5.0")]
    [System.Serializable]
    public class MarqueeStyle : ChildComponent
    {
        [SerializeField][Since("v3.5.0")] private bool m_Apply = false;
        [SerializeField][Since("v3.5.0")] private bool m_RealRect = false;
        [SerializeField][Since("v3.5.0")] private AreaStyle m_AreaStyle = new AreaStyle();
        [SerializeField][Since("v3.5.0")] private LineStyle m_LineStyle = new LineStyle();

        protected Action<DataZoom> m_OnStart;
        protected Action<DataZoom> m_OnGoing;
        protected Action<DataZoom> m_OnEnd;

        /// <summary>
        /// The area style of marquee.
        /// |选取框区域填充样式。
        /// </summary>
        public AreaStyle areaStyle { get { return m_AreaStyle; } set { m_AreaStyle = value; } }
        /// <summary>
        /// The line style of marquee border.
        /// |选取框区域边框样式。
        /// </summary>
        public LineStyle lineStyle { get { return m_LineStyle; } set { m_LineStyle = value; } }
        /// <summary>
        /// Check whether the scope is applied to the DataZoom. 
        /// If this parameter is set to true, the range after the selection is complete is the DataZoom selection range.
        /// |选取框范围是否应用到DataZoom上。当为true时，框选结束后的范围即为DataZoom的选择范围。
        /// </summary>
        public bool apply { get { return m_Apply; } set { m_Apply = value; } }
        /// <summary>
        /// Whether to select the actual box selection area. When true, 
        /// the actual range between the mouse's actual point and the end point is used as the box selection area.
        /// |是否选取实际框选区域。当为true时，以鼠标的其实点和结束点间的实际范围作为框选区域。
        /// </summary>
        public bool realRect { get { return m_RealRect; } set { m_RealRect = value; } }
        /// <summary>
        /// Customize the callback to the start of the selection of the checkbox.
        /// |自定义选取框开始选取时的回调。
        /// </summary>
        public Action<DataZoom> onStart { set { m_OnStart = value; } get { return m_OnStart; } }
        /// <summary>
        /// Custom checkboxes select ongoing callbacks.
        /// |自定义选取框选取进行时的回调。
        /// </summary>
        public Action<DataZoom> onGoing { set { m_OnStart = value; } get { return m_OnStart; } }
        /// <summary>
        /// Customize the callback at the end of the selection.
        /// |自定义选取框结束选取时的回调。
        /// </summary>
        public Action<DataZoom> onEnd { set { m_OnEnd = value; } get { return m_OnEnd; } }
    }
}