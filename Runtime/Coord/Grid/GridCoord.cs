using System;
using UnityEngine;
using XUGL;

namespace XCharts.Runtime
{
    /// <summary>
    /// Grid component.
    /// |Drawing grid in rectangular coordinate. Line chart, bar chart, and scatter chart can be drawn in grid.
    /// |网格组件。
    /// 直角坐标系内绘图网格。可以在网格上绘制折线图，柱状图，散点图。
    /// </summary>
    [Serializable]
    [ComponentHandler(typeof(GridCoordHandler), true)]
    public class GridCoord : CoordSystem, IUpdateRuntimeData, ISerieContainer
    {
        [SerializeField] private bool m_Show = true;
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
        /// |是否显示直角坐标系网格。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
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
        public Color32 backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        ///  Whether to show the grid border.
        /// |是否显示网格边框。
        /// </summary>
        public bool showBorder
        {
            get { return m_ShowBorder; }
            set { if (PropertyUtil.SetStruct(ref m_ShowBorder, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Border width of grid.
        /// |网格边框宽。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The color of grid border.
        /// |网格边框颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetColor(ref m_BorderColor, value)) SetVerticesDirty(); }
        }

        public bool IsPointerEnter()
        {
            return context.isPointerEnter;
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
            context.center = new Vector3(context.x + context.width / 2, context.y + context.height / 2);
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

        /// <summary>
        /// 给定的线段和Grid边界的交点
        /// </summary>
        /// <param name="sp"></param>
        /// <param name="ep"></param>
        /// <returns></returns>
        public bool BoundaryPoint(Vector3 sp, Vector3 ep, ref Vector3 point)
        {
            if (Contains(sp) && Contains(ep))
            {
                point = ep;
                return false;
            }
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
    }
}