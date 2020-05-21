/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
    public class Grid : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;
        [SerializeField] private Color m_BackgroundColor;

        /// <summary>
        /// Whether to show the grid in rectangular coordinate.
        /// 是否显示直角坐标系网格。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the left side of the container.
        /// grid 组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtility.SetStruct(ref m_Left, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the right side of the container.
        /// grid 组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtility.SetStruct(ref m_Right, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the top side of the container.
        /// grid 组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtility.SetStruct(ref m_Top, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between grid component and the bottom side of the container.
        /// grid 组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtility.SetStruct(ref m_Bottom, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Background color of grid, which is transparent by default.
        /// 网格背景色，默认透明。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtility.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }

        public static Grid defaultGrid
        {
            get
            {
                var grid = new Grid
                {
                    m_Show = true,
                    m_Left = 50,
                    m_Right = 30,
                    m_Top = 50,
                    m_Bottom = 30
                };
                return grid;
            }
        }
    }
}