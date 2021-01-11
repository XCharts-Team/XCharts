/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Polar coordinate can be used in scatter and line chart. Every polar coordinate has an angleAxis and a radiusAxis.
    /// <para>
    /// 极坐标系组件。
    /// 极坐标系，可以用于散点图和折线图。每个极坐标系拥有一个角度轴和一个半径轴。
    /// </para>
    /// </summary>
    [Serializable]
    public class Polar : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private float m_Radius = 100;
        [SerializeField] private Color m_BackgroundColor;


        /// <summary>
        /// Whether to show the polor component.
        /// 是否显示极坐标。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// [default:[0.5f,0.45f]]The center of ploar. The center[0] is the x-coordinate, and the center[1] is the y-coordinate.
        /// When value between 0 and 1 represents a percentage  relative to the chart.
        /// 极坐标的中心点。数组的第一项是横坐标，第二项是纵坐标。
        /// 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
        /// </summary>
        public float[] center
        {
            get { return m_Center; }
            set { if (value != null) { m_Center = value; SetAllDirty(); } }
        }
        /// <summary>
        /// [default:0.35f]the radius of polar.
        /// 极坐标的半径。
        /// </summary>
        public float radius
        {
            get { return m_Radius; }
            set { if (PropertyUtil.SetStruct(ref m_Radius, value)) SetAllDirty(); }
        }
        /// <summary>
        /// [default:Color.clear]Background color of polar, which is transparent by default.
        /// 极坐标的背景色，默认透明。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        public int index { get; internal set; }
        /// <summary>
        /// the center position of polar in container.
        /// 极坐标在容器中的具体中心点。
        /// </summary>
        public Vector3 runtimeCenterPos { get; internal set; }
        /// <summary>
        /// the true radius of polar.
        /// 极坐标的运行时实际半径。
        /// </summary>
        public float runtimeRadius { get; internal set; }

        public static Polar defaultPolar
        {
            get
            {
                var polar = new Polar
                {
                    m_Show = true,
                    m_Radius = 0.35f,
                };
                polar.center[0] = 0.5f;
                polar.center[1] = 0.45f;
                return polar;
            }
        }
    }
}