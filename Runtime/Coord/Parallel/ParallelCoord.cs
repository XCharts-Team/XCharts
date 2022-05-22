using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Grid component.
    /// |Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.
    /// |网格组件。
    /// 直角坐标系内绘图网格。可以在网格上绘制折线图，柱状图，散点图。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(ParallelCoordHandler), true)]
    public class ParallelCoord : CoordSystem, IUpdateRuntimeData, ISerieContainer
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] protected Orient m_Orient = Orient.Vertical;
        [SerializeField] private float m_Left = 0.1f;
        [SerializeField] private float m_Right = 0.08f;
        [SerializeField] private float m_Top = 0.22f;
        [SerializeField] private float m_Bottom = 0.12f;
        [SerializeField] private Color m_BackgroundColor;

        public ParallelCoordContext context = new ParallelCoordContext();

        /// <summary>
        /// Whether to show the grid in rectangular coordinate.
        /// |是否显示直角坐标系网格。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Orientation of the axis. By default, it's 'Vertical'. You can set it to be 'Horizonal' to make a vertical axis.
        /// |坐标轴朝向。默认为垂直朝向。
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the left side of the container.
        /// |grid 组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the right side of the container.
        /// |grid 组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the top side of the container.
        /// |grid 组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the bottom side of the container.
        /// |grid 组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Background color of grid, which is transparent by default.
        /// |网格背景色，默认透明。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }

        public bool IsPointerEnter()
        {
            return context.runtimeIsPointerEnter;
        }

        public void UpdateRuntimeData(float chartX, float chartY, float chartWidth, float chartHeight)
        {
            context.left = left <= 1 ? left * chartWidth : left;
            context.bottom = bottom <= 1 ? bottom * chartHeight : bottom;
            context.top = top <= 1 ? top * chartHeight : top;
            context.right = right <= 1 ? right * chartWidth : right;
            context.x = chartX + context.left;
            context.y = chartY + context.bottom;
            context.width = chartWidth - context.left - context.right;
            context.height = chartHeight - context.top - context.bottom;
            context.position = new Vector3(context.x, context.y);
        }

        public bool Contains(Vector3 pos)
        {
            return Contains(pos.x, pos.y);
        }

        public bool Contains(float x, float y)
        {
            if (x < context.x - 1 || x > context.x + context.width + 1 ||
                y < context.y - 1 || y > context.y + context.height + 1)
            {
                return false;
            }
            return true;
        }
    }
}