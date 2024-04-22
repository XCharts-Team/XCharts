using System;
using System.Collections.Generic;
using UnityEngine;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// View control component in 3D coordinate system.
    /// ||3D视角控制组件。
    /// </summary>
    [Since("v3.11.0")]
    [Serializable]
    public class ViewControl : ChildComponent
    {
        [SerializeField][Range(-90, 180)] private float m_Alpha = 90f;
        [SerializeField][Range(-90, 90)] private float m_Beta = 55f;

        /// <summary>
        /// The angle of the view in the x-z plane.
        /// ||视角在x-z平面的角度。
        /// </summary>
        public float alpha
        {
            get { return m_Alpha; }
            set { if (PropertyUtil.SetStruct(ref m_Alpha, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// The angle of the view in the y-z plane.
        /// ||视角在y-z平面的角度。
        /// </summary>
        public float beta
        {
            get { return m_Beta; }
            set { if (PropertyUtil.SetStruct(ref m_Beta, value)) SetVerticesDirty(); }
        }
    }

    /// <summary>
    /// Grid component.
    /// ||Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.
    /// ||3D网格组件。
    /// 3D直角坐标系内绘图网格。可以在网格上绘制3D折线图，3D柱状图，3D散点图。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(GridCoord3DHandler), true)]
    public class GridCoord3D : CoordSystem, IUpdateRuntimeData, ISerieContainer
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private float m_Left = 0.15f;
        [SerializeField] private float m_Right = 0.2f;
        [SerializeField] private float m_Top = 0.3f;
        [SerializeField] private float m_Bottom = 0.15f;
        [SerializeField] private bool m_ShowBorder = false;
        [SerializeField] private float m_BoxWidth = 0.55f;
        [SerializeField] private float m_BoxHeight = 0.4f;
        [SerializeField] private float m_BoxDepth = 0.2f;
        [SerializeField] private bool m_XYExchanged = false;
        [SerializeField] private ViewControl m_ViewControl = new ViewControl();

        public GridCoord3DContext context = new GridCoord3DContext();

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
        ///  Whether to show the grid border.
        /// ||是否显示网格边框。
        /// </summary>
        public bool showBorder
        {
            get { return m_ShowBorder; }
            set { if (PropertyUtil.SetStruct(ref m_ShowBorder, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The width of the box in the coordinate system.
        /// ||坐标系的宽度。 
        /// </summary>
        public float boxWidth
        {
            get { return m_BoxWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BoxWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The height of the box in the coordinate system.
        /// ||坐标系的高度。
        /// </summary>
        public float boxHeight
        {
            get { return m_BoxHeight; }
            set { if (PropertyUtil.SetStruct(ref m_BoxHeight, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The depth of the box in the coordinate system.
        /// ||坐标系的深度。
        /// </summary>
        public float boxDepth
        {
            get { return m_BoxDepth; }
            set { if (PropertyUtil.SetStruct(ref m_BoxDepth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to exchange the x and y axes.
        /// ||是否交换x和y轴。
        /// </summary>
        public bool xyExchanged
        {
            get { return m_XYExchanged; }
            set { if (PropertyUtil.SetStruct(ref m_XYExchanged, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// View control component in 3D coordinate system.
        /// ||3D视角控制组件。
        /// </summary>
        public ViewControl viewControl
        {
            get { return m_ViewControl; }
            //set { if (PropertyUtil.SetClass(ref m_ViewControl, value)) SetVerticesDirty(); }
        }

        public void UpdateRuntimeData(BaseChart chart)
        {
            var chartX = chart.chartX;
            var chartY = chart.chartY;
            var chartWidth = chart.chartWidth;
            var chartHeight = chart.chartHeight;
            var actualLeft = left <= 1 ? left * chartWidth : left;
            var actualBottom = bottom <= 1 ? bottom * chartHeight : bottom;
            var actualBoxWidth = m_BoxWidth <= 1 ? m_BoxWidth * chartWidth : m_BoxWidth;
            var actualBoxHeight = m_BoxHeight <= 1 ? m_BoxHeight * chartHeight : m_BoxHeight;
            var actualBoxDepth = m_BoxDepth <= 1 ? m_BoxDepth * chartWidth : m_BoxDepth;
            context.x = chartX + actualLeft;
            context.y = chartY + actualBottom;
            context.pointA.x = context.x;
            context.pointA.y = context.y;

            var angle = m_ViewControl.alpha * Mathf.Deg2Rad;
            context.pointD.x = context.x + actualBoxWidth * Mathf.Sin(angle);
            context.pointD.y = context.y - actualBoxWidth * Mathf.Cos(angle);

            angle = (90 - m_ViewControl.beta) * Mathf.Deg2Rad;
            context.pointB.x = context.x + actualBoxDepth * Mathf.Cos(angle);
            context.pointB.y = context.y + actualBoxDepth * Mathf.Sin(angle);

            context.pointC = context.pointB + (context.pointD - context.pointA);

            context.pointE.x = context.pointA.x;
            context.pointE.y = context.pointA.y + actualBoxHeight;

            var diff = context.pointE - context.pointA;
            context.pointF = context.pointB + diff;
            context.pointG = context.pointC + diff;
            context.pointH = context.pointD + diff;

            var minX = Mathf.Min(context.pointA.x, context.pointB.x, context.pointC.x, context.pointD.x, context.pointE.x, context.pointF.x, context.pointG.x, context.pointH.x);
            var minY = Mathf.Min(context.pointA.y, context.pointB.y, context.pointC.y, context.pointD.y, context.pointE.y, context.pointF.y, context.pointG.y, context.pointH.y);
            var maxX = Mathf.Max(context.pointA.x, context.pointB.x, context.pointC.x, context.pointD.x, context.pointE.x, context.pointF.x, context.pointG.x, context.pointH.x);
            var maxY = Mathf.Max(context.pointA.y, context.pointB.y, context.pointC.y, context.pointD.y, context.pointE.y, context.pointF.y, context.pointG.y, context.pointH.y);

            context.maxRect.x = minX;
            context.maxRect.y = minY;
            context.maxRect.width = maxX - minX;
            context.maxRect.height = maxY - minY;
        }

        /// <summary>
        /// The opening of the coordinate system faces to the left.
        /// 坐标系开口朝向左边。
        /// </summary>
        /// <returns></returns>
        public bool IsLeft()
        {
            return context.pointB.x < context.pointA.x;
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
            if (!context.maxRect.Contains(pos)) return false;
            if (UGLHelper.IsPointInPolygon(pos, context.pointA, context.pointB, context.pointC, context.pointD)) return true;
            if (UGLHelper.IsPointInPolygon(pos, context.pointB, context.pointF, context.pointG, context.pointC)) return true;
            if (IsLeft())
                if (UGLHelper.IsPointInPolygon(pos, context.pointC, context.pointG, context.pointH, context.pointD)) return true;
                else
                if (UGLHelper.IsPointInPolygon(pos, context.pointA, context.pointE, context.pointF, context.pointB)) return true;
            return false;
        }

        /// <summary>
        /// Clamp the position of pos to the grid.
        /// ||将位置限制在网格内。
        /// </summary>
        /// <param name="pos"></param>
        public void Clamp(ref Vector3 pos)
        {
            //TODO:
        }

        /// <summary>
        /// Determines whether a given line segment will not intersect the Grid boundary at all.
        /// ||判断给定的线段是否与Grid边界是否完全不会相交。
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="ep"></param>
        /// <returns></returns>
        public bool NotAnyIntersect(Vector3 sp, Vector3 ep)
        {
            //TODO:
            return false;
        }
    }
}