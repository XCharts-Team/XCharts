using System;
using System.Collections.Generic;
using UnityEngine;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// Grid component.
    /// ||Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.
    /// ||网格组件。
    /// 直角坐标系内绘图网格。可以在网格上绘制折线图，柱状图，散点图。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(GridCoordHandler), true)]
    public class GridCoord : CoordSystem, IUpdateRuntimeData, ISerieContainer
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField][Since("v3.8.0")] private int m_LayoutIndex = -1;
        [SerializeField] private float m_Left = 0.1f;
        [SerializeField] private float m_Right = 0.08f;
        [SerializeField] private float m_Top = 0.22f;
        [SerializeField] private float m_Bottom = 0.12f;
        [SerializeField] private Color32 m_BackgroundColor;
        [SerializeField] private bool m_ShowBorder = false;
        [SerializeField] private float m_BorderWidth = 0f;
        [SerializeField] private Color32 m_BorderColor;

        public GridCoordContext context = new GridCoordContext();

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
        /// The index of the grid layout component to which the grid belongs. 
        /// The default is -1, which means that it does not belong to any grid layout component. 
        /// When this value is set, the left, right, top, and bottom properties will be invalid.
        /// ||网格所属的网格布局组件的索引。默认为-1，表示不属于任何网格布局组件。当设置了该值时，left、right、top、bottom属性将失效。
        /// </summary>
        public int layoutIndex
        {
            get { return m_LayoutIndex; }
            set { if (PropertyUtil.SetStruct(ref m_LayoutIndex, value)) SetVerticesDirty(); }
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
        /// Background color of grid, which is transparent by default.
        /// ||网格背景色，默认透明。
        /// </summary>
        public Color32 backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        ///  Whether to show the grid border.
        /// ||是否显示网格边框。
        /// </summary>
        public bool showBorder
        {
            get { return m_ShowBorder; }
            set { if (PropertyUtil.SetStruct(ref m_ShowBorder, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Border width of grid.
        /// ||网格边框宽。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The color of grid border.
        /// ||网格边框颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor, value)) SetVerticesDirty(); }
        }

        public void UpdateRuntimeData(BaseChart chart)
        {
            var chartX = chart.chartX;
            var chartY = chart.chartY;
            var chartWidth = chart.chartWidth;
            var chartHeight = chart.chartHeight;
            if (layoutIndex >= 0)
            {
                var layout = chart.GetChartComponent<GridLayout>(layoutIndex);
                if (layout != null)
                {
                    layout.UpdateRuntimeData(chart);
                    layout.UpdateGridContext(index, ref chartX, ref chartY, ref chartWidth, ref chartHeight);
                }
            }
            var actualLeft = left <= 1 ? left * chartWidth : left;
            var actualBottom = bottom <= 1 ? bottom * chartHeight : bottom;
            var actualTop = top <= 1 ? top * chartHeight : top;
            var actualRight = right <= 1 ? right * chartWidth : right;
            context.x = chartX + actualLeft;
            context.y = chartY + actualBottom;
            context.width = chartWidth - actualLeft - actualRight;
            context.height = chartHeight - actualTop - actualBottom;
            context.position = new Vector3(context.x, context.y);
            context.center = new Vector3(context.x + context.width / 2, context.y + context.height / 2);
        }

        /// <summary>
        /// Whether the pointer is in the grid.
        /// ||指针是否在网格内。
        /// </summary>
        /// <returns></returns>
        public bool IsPointerEnter()
        {
            return context.isPointerEnter;
        }

        /// <summary>
        /// Whether the given position is in the grid.
        /// ||给定的位置是否在网格内。
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool Contains(Vector3 pos)
        {
            return Contains(pos.x, pos.y);
        }

        /// <summary>
        /// Whether the given position is in the grid.
        /// ||给定的位置是否在网格内。
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="isYAxis"></param>
        /// <returns></returns>
        [Since("v3.7.0")]
        public bool Contains(Vector3 pos, bool isYAxis)
        {
            return isYAxis ? ContainsY(pos.y) : ContainsX(pos.x);
        }

        /// <summary>
        /// Whether the given position is in the grid.
        /// ||给定的位置是否在网格内。
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Contains(float x, float y)
        {
            return ContainsX(x) && ContainsY(y);
        }

        /// <summary>
        /// Whether the given x is in the grid.
        /// ||给定的x是否在网格内。
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        [Since("v3.7.0")]
        public bool ContainsX(float x)
        {
            return x >= context.x && x <= context.x + context.width;
        }

        /// <summary>
        /// Whether the given y is in the grid.
        /// ||给定的y是否在网格内。
        /// </summary>
        /// <param name="y"></param>
        /// <returns></returns>
        [Since("v3.7.0")]
        public bool ContainsY(float y)
        {
            return y >= context.y && y <= context.y + context.height;
        }

        /// <summary>
        /// Clamp the position of pos to the grid.
        /// ||将位置限制在网格内。
        /// </summary>
        /// <param name="pos"></param>
        [Since("v3.7.0")]
        public void Clamp(ref Vector3 pos)
        {
            ClampX(ref pos);
            ClampY(ref pos);
        }

        /// <summary>
        /// Clamp the x position of pos to the grid.
        /// ||将位置的X限制在网格内。
        /// </summary>
        /// <param name="pos"></param>
        [Since("v3.7.0")]
        public void ClampX(ref Vector3 pos)
        {
            if (pos.x < context.x) pos.x = context.x;
            else if (pos.x > context.x + context.width) pos.x = context.x + context.width;
        }

        /// <summary>
        /// Clamp the y position of pos to the grid.
        /// ||将位置的Y限制在网格内。
        /// </summary>
        /// <param name="pos"></param>
        [Since("v3.7.0")]
        public void ClampY(ref Vector3 pos)
        {
            if (pos.y < context.y) pos.y = context.y;
            else if (pos.y > context.y + context.height) pos.y = context.y + context.height;
        }

        /// <summary>
        /// 给定的线段和Grid边界的交点
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="ep"></param>
        /// <returns></returns>
        public bool BoundaryPoint(Vector3 sp, Vector3 ep, ref Vector3 point)
        {
            if (Contains(sp) && Contains(ep))
                return false;
            if (sp.x < context.x && ep.x < context.x)
                return false;
            if (sp.x > context.x + context.width && ep.x > context.x + context.width)
                return false;
            if (sp.y < context.y && ep.y < context.y)
                return false;
            if (sp.y > context.y + context.height && ep.y > context.y + context.height)
                return false;
            var lb = new Vector3(context.x, context.y);
            var lt = new Vector3(context.x, context.y + context.height);
            var rt = new Vector3(context.x + context.width, context.y + context.height);
            var rb = new Vector3(context.x + context.width, context.y);
            if (UGLHelper.GetIntersection(sp, ep, rb, rt, ref point))
                return true;
            if (UGLHelper.GetIntersection(sp, ep, lt, rt, ref point))
                return true;
            if (UGLHelper.GetIntersection(sp, ep, lb, rb, ref point))
                return true;
            if (UGLHelper.GetIntersection(sp, ep, lb, lt, ref point))
                return true;
            return false;
        }

        /// <summary>
        /// 给定的线段和Grid边界的交点
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="ep"></param>
        /// <returns></returns>
        public bool BoundaryPoint(Vector3 sp, Vector3 ep, ref List<Vector3> point)
        {
            if (Contains(sp) && Contains(ep))
                return false;
            var lb = new Vector3(context.x, context.y);
            var lt = new Vector3(context.x, context.y + context.height);
            var rt = new Vector3(context.x + context.width, context.y + context.height);
            var rb = new Vector3(context.x + context.width, context.y);
            var flag = false;
            if (UGLHelper.GetIntersection(sp, ep, lb, lt, ref point))
                flag = true;
            if (UGLHelper.GetIntersection(sp, ep, lt, rt, ref point))
                flag = true;
            if (UGLHelper.GetIntersection(sp, ep, lb, rb, ref point))
                flag = true;
            if (UGLHelper.GetIntersection(sp, ep, rb, rt, ref point))
                flag = true;
            return flag;
        }
    }
}