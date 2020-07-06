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
    /// Vessel component for liquid chart.
    /// <para>
    /// 容器组件。
    /// 一般用于LiquidChart。
    /// </para>
    /// </summary>
    [Serializable]
    public class Vessel : MainComponent
    {
        public enum Shape
        {
            /// <summary>
            /// 圆形
            /// </summary>
            Circle,
            /// <summary>
            /// 正方形。
            /// </summary>
            Rect,
            /// <summary>
            /// 三角形。
            /// </summary>
            Triangle,
            /// <summary>
            /// 菱形。
            /// </summary>
            Diamond,
            /// <summary>
            /// 不显示标记。
            /// </summary>
            None,
        }
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Shape m_Shape = Shape.Circle;
        [SerializeField] private float m_ShapeWidth = 5f;
        [SerializeField] private float m_Gap = 10f;
        [SerializeField] private Color m_Color;
        [SerializeField] private Color m_BackgroundColor;
        [SerializeField] private bool m_AutoColor = true;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private float m_Radius = 0.5f;
        [SerializeField] [Range(0.5f, 10f)] private float m_Smoothness = 1f;

        /// <summary>
        /// Whether to show the vessel.
        /// 是否显示容器组件。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The shape of vessel.
        /// 容器形状。
        /// </summary>
        public Shape shape
        {
            get { return m_Shape; }
            set { if (PropertyUtility.SetStruct(ref m_Shape, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 容器厚度。
        /// </summary>
        public float shapeWidth
        {
            get { return m_ShapeWidth; }
            set { if (PropertyUtility.SetStruct(ref m_ShapeWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 间隙。容器和液体的间隙。
        /// </summary>
        public float gap
        {
            get { return m_Gap; }
            set { if (PropertyUtility.SetStruct(ref m_Gap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 中心点。数组的第一项是横坐标，第二项是纵坐标。
        /// 当值为0-1之间时表示百分比，设置成百分比时表示图表宽高最小值的百分比。
        /// </summary>
        public float[] center
        {
            get { return m_Center; }
            set { if (value != null) { m_Center = value; SetAllDirty(); } }
        }
        /// <summary>
        /// The radius of vessel.
        /// 半径。
        /// </summary>
        public float radius
        {
            get { return m_Radius; }
            set { if (PropertyUtility.SetStruct(ref m_Radius, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The smoothness of wave.
        /// 水波平滑度。
        /// </summary>
        public float smoothness
        {
            get { return m_Smoothness; }
            set { if (PropertyUtility.SetStruct(ref m_Smoothness, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Background color of polar, which is transparent by default.
        /// 背景色，默认透明。
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtility.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 容器颜色。默认和serie一致。
        /// </summary>
        public Color color
        {
            get { return m_Color; }
            set { if (PropertyUtility.SetColor(ref m_Color, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否自动颜色。为true时颜色会和serie一致。
        /// </summary>
        public bool autoColor
        {
            get { return m_AutoColor; }
            set { if (PropertyUtility.SetStruct(ref m_AutoColor, value)) SetVerticesDirty(); }
        }
        public int index { get; internal set; }
        /// <summary>
        /// the runtime center position of vessel.
        /// 运行时中心点。
        /// </summary>
        public Vector3 runtimeCenterPos { get; internal set; }
        /// <summary>
        /// the runtime radius of vessel.
        /// 运行时半径。
        /// </summary>
        public float runtimeRadius { get; internal set; }
        /// <summary>
        /// 运行时内半径。
        /// </summary>
        public float runtimeInnerRadius { get; internal set; }
        public static Vessel defaultVessel
        {
            get
            {
                var vessel = new Vessel
                {
                    m_Show = true,
                    m_Shape = Shape.Circle,
                    m_ShapeWidth = 5,
                    m_Gap = 10,
                    m_Radius = 0.35f,
                    m_AutoColor = true,
                    m_Color = new Color32(70, 70, 240, 255),
                    m_Smoothness = 1
                };
                vessel.center[0] = 0.5f;
                vessel.center[1] = 0.45f;
                return vessel;
            }
        }
    }
}