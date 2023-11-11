using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Polar coordinate can be used in scatter and line chart. Every polar coordinate has an angleAxis and a radiusAxis.
    /// ||极坐标系组件。
    /// 极坐标系，可以用于散点图和折线图。每个极坐标系拥有一个角度轴和一个半径轴。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(PolarCoordHandler), true)]
    public class PolarCoord : CoordSystem, ISerieContainer
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.45f };
        [SerializeField] private float[] m_Radius = new float[2] { 0, 0.35f };
        [SerializeField] private Color m_BackgroundColor;
        [SerializeField][Since("v3.8.0")] private float m_IndicatorLabelOffset = 30f;

        public PolarCoordContext context = new PolarCoordContext();

        /// <summary>
        /// Whether to show the polor component.
        /// ||是否显示极坐标。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The center of ploar. The center[0] is the x-coordinate, and the center[1] is the y-coordinate.
        /// When value between 0 and 1 represents a percentage  relative to the chart.
        /// ||极坐标的中心点。数组的第一项是横坐标，第二项是纵坐标。
        /// 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
        /// </summary>
        public float[] center
        {
            get { return m_Center; }
            set { if (value != null) { m_Center = value; SetAllDirty(); } }
        }
        /// <summary>
        /// the radius of polar.
        /// ||半径。radius[0]表示内径，radius[1]表示外径。
        /// </summary>
        public float[] radius
        {
            get { return m_Radius; }
            set { if (value != null && value.Length == 2) { m_Radius = value; SetAllDirty(); } }
        }
        /// <summary>
        /// Background color of polar, which is transparent by default.
        /// ||极坐标的背景色，默认透明。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// The offset of indicator label.
        /// ||指示器标签的偏移量。
        /// </summary>
        public float indicatorLabelOffset
        {
            get { return m_IndicatorLabelOffset; }
            set { if (PropertyUtil.SetStruct(ref m_IndicatorLabelOffset, value)) SetVerticesDirty(); }
        }

        public bool IsPointerEnter()
        {
            return context.isPointerEnter;
        }

        public bool Contains(Vector3 pos)
        {
            var dist = Vector3.Distance(pos, context.center);
            return dist >= context.insideRadius && dist <= context.outsideRadius;
        }
    }
}