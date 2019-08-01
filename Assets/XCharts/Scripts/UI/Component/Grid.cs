using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Grid component.
    /// Drawing grid in rectangular coordinate. In a single grid, at most two X and Y axes each is allowed. 
    /// Line chart, bar chart, and scatter chart can be drawn in grid.
    /// There is only one single grid component at most in a single echarts instance.
    /// <para>
    /// 网格组件。
    /// 直角坐标系内绘图网格，单个 grid 内最多可以放置上下两个 X 轴，左右两个 Y 轴。可以在网格上绘制折线图，柱状图，散点图。
    /// 单个xcharts实例中只能存在一个grid组件。
    /// </para>
    /// </summary>
    [Serializable]
    public class Grid : IEquatable<Grid>
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;
        [SerializeField] private Color m_BackgroundColor;

        /// <summary>
        /// Whether to show the grid in rectangular coordinate.
        /// 是否显示直角坐标系网格。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// Distance between grid component and the left side of the container.
        /// grid 组件离容器左侧的距离。
        /// </summary>
        public float left { get { return m_Left; } set { m_Left = value; } }
        /// <summary>
        /// Distance between grid component and the right side of the container.
        /// grid 组件离容器右侧的距离。
        /// </summary>
        public float right { get { return m_Right; } set { m_Right = value; } }
        /// <summary>
        /// Distance between grid component and the top side of the container.
        /// grid 组件离容器上侧的距离。
        /// </summary>
        public float top { get { return m_Top; } set { m_Top = value; } }
        /// <summary>
        /// Distance between grid component and the bottom side of the container.
        /// grid 组件离容器下侧的距离。
        /// </summary>
        public float bottom { get { return m_Bottom; } set { m_Bottom = value; } }
        /// <summary>
        /// Background color of grid, which is transparent by default.
        /// 网格背景色，默认透明。
        /// </summary>
        public Color backgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; } }

        public static Grid defaultGrid
        {
            get
            {
                var coordinate = new Grid
                {
                    m_Show = false,
                    m_Left = 50,
                    m_Right = 30,
                    m_Top = 50,
                    m_Bottom = 30
                };
                return coordinate;
            }
        }
        public void Copy(Grid other)
        {
            m_Show = other.show;
            m_Left = other.left;
            m_Right = other.right;
            m_Top = other.top;
            m_Bottom = other.bottom;
            m_BackgroundColor = other.backgroundColor;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Grid)
            {
                return Equals((Grid)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Grid other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return m_Show == other.show &&
                m_Left == other.left &&
                m_Right == other.right &&
                m_Top == other.top &&
                m_Bottom == other.bottom &&
                ChartHelper.IsValueEqualsColor(m_BackgroundColor, other.backgroundColor);
        }

        public static bool operator ==(Grid left, Grid right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }
            else if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }
            return Equals(left, right);
        }

        public static bool operator !=(Grid left, Grid right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}