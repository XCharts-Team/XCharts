using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Grid layout component. Used to manage the layout of multiple `GridCoord`, and the number of rows and columns of the grid can be controlled by `row` and `column`.
    /// ||网格布局组件。用于管理多个`GridCoord`的布局，可以通过`row`和`column`来控制网格的行列数。
    /// </summary>
    [Since("v3.8.0")]
    [Serializable]
    [ComponentHandler(typeof(GridLayoutHandler), true)]
    public class GridLayout : MainComponent, IUpdateRuntimeData
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private float m_Left = 0.1f;
        [SerializeField] private float m_Right = 0.08f;
        [SerializeField] private float m_Top = 0.22f;
        [SerializeField] private float m_Bottom = 0.12f;
        [SerializeField] private int m_Row = 2;
        [SerializeField] private int m_Column = 2;
        [SerializeField] private Vector2 m_Spacing = Vector2.zero;
        [SerializeField] protected bool m_Inverse = false;

        public GridLayoutContext context = new GridLayoutContext();

        /// <summary>
        /// Whether to show the grid in rectangular coordinate.
        /// ||是否显示直角坐标系网格。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the left side of the container.
        /// ||grid 组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the right side of the container.
        /// ||grid 组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the top side of the container.
        /// ||grid 组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the bottom side of the container.
        /// ||grid 组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the row count of grid layout.
        /// ||网格布局的行数。
        /// </summary>
        public int row
        {
            get { return m_Row; }
            set { if (PropertyUtil.SetStruct(ref m_Row, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the column count of grid layout.
        /// ||网格布局的列数。
        /// </summary>
        public int column
        {
            get { return m_Column; }
            set { if (PropertyUtil.SetStruct(ref m_Column, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the spacing of grid layout.
        /// ||网格布局的间距。
        /// </summary>
        public Vector2 spacing
        {
            get { return m_Spacing; }
            set { if (PropertyUtil.SetStruct(ref m_Spacing, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether to inverse the grid layout.
        /// ||是否反转网格布局。
        /// </summary>
        public bool inverse
        {
            get { return m_Inverse; }
            set { if (PropertyUtil.SetStruct(ref m_Inverse, value)) SetAllDirty(); }
        }

        public void UpdateRuntimeData(BaseChart chart)
        {
            var chartX = chart.chartX;
            var chartY = chart.chartY;
            var chartWidth = chart.chartWidth;
            var chartHeight = chart.chartHeight;
            var actualLeft = left <= 1 ? left * chartWidth : left;
            var actualBottom = bottom <= 1 ? bottom * chartHeight : bottom;
            var actualTop = top <= 1 ? top * chartHeight : top;
            var actualRight = right <= 1 ? right * chartWidth : right;
            context.x = chartX + actualLeft;
            context.y = chartY + actualBottom;
            context.width = chartWidth - actualLeft - actualRight;
            context.height = chartHeight - actualTop - actualBottom;
            context.eachWidth = (context.width - spacing.x * (column - 1)) / column;
            context.eachHeight = (context.height - spacing.y * (row - 1)) / row;
        }

        internal void UpdateGridContext(int index, ref float x, ref float y, ref float width, ref float height)
        {
            var row = index / m_Column;
            var column = index % m_Column;

            x = context.x + column * (context.eachWidth + spacing.x);
            if(m_Inverse)
                y = context.y + row * (context.eachHeight + spacing.y);
            else
                y = context.y + context.height - (row + 1) * context.eachHeight - row * spacing.y;
            width = context.eachWidth;
            height = context.eachHeight;
        }

        internal void UpdateGridContext(int index, ref Vector3 position, ref float width, ref float height)
        {
            float x = 0, y = 0;
            UpdateGridContext(index, ref x, ref y, ref width, ref height);
            position = new Vector3(x, y);
        }
    }
}