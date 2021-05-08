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
    /// Vessel component for liquid chart. There can be multiple vessels in a Chart, which can be matched by vesselIndex in Serie.
    /// <para>
    /// 容器组件。
    /// 一般用于LiquidChart。一个Chart中可以有多个Vessel，Serie中用vesselIndex来对应。
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
        [SerializeField] private float m_Gap = 5f;
        [SerializeField] private Color32 m_Color;
        [SerializeField] private Color32 m_BackgroundColor;
        [SerializeField] private bool m_AutoColor = true;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private float m_Radius = 0.35f;
        [SerializeField] [Range(0.5f, 10f)] private float m_Smoothness = 1f;
        [SerializeField] private float m_Width = 0.5f;
        [SerializeField] private float m_Height = 0.7f;
        [SerializeField] private float[] m_CornerRadius = new float[] { 0, 0, 0, 0 };

        /// <summary>
        /// Whether to show the vessel.
        /// 是否显示容器组件。
        /// [defaut: true]
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The shape of vessel.
        /// 容器形状。
        /// [default: Shape.Circle]
        /// </summary>
        public Shape shape
        {
            get { return m_Shape; }
            set { if (PropertyUtil.SetStruct(ref m_Shape, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Thickness of vessel.
        /// 容器厚度。
        /// [defaut: 5f]
        /// </summary>
        public float shapeWidth
        {
            get { return m_ShapeWidth; }
            set { if (PropertyUtil.SetStruct(ref m_ShapeWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The gap between the vessel and the liquid.
        /// 间隙。容器和液体的间隙。
        /// [defaut: 10f]
        /// </summary>
        public float gap
        {
            get { return m_Gap; }
            set { if (PropertyUtil.SetStruct(ref m_Gap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The center of vesselß. The center[0] is the x-coordinate, and the center[1] is the y-coordinate.
        /// When value between 0 and 1 represents a percentage  relative to the chart.
        /// 中心点。数组的第一项是横坐标，第二项是纵坐标。
        /// 当值为0-1之间时表示百分比，设置成百分比时表示图表宽高最小值的百分比。
        /// [default:[0.5f,0.45f]]
        /// </summary>
        public float[] center
        {
            get { return m_Center; }
            set { if (value != null) { m_Center = value; SetAllDirty(); } }
        }
        /// <summary>
        /// The radius of vessel.
        /// When value between 0 and 1 represents a percentage relative to the chart.
        /// 半径。
        /// [default: 0.35f]
        /// </summary>
        public float radius
        {
            get { return m_Radius; }
            set { if (PropertyUtil.SetStruct(ref m_Radius, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The width of vessel.
        /// When value between 0 and 1 represents a percentage relative to the chart.
        /// 容器的宽。shape为Rect时有效。
        /// [default: 0.35f]
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The height of vessel.
        /// When value between 0 and 1 represents a percentage relative to the chart.
        /// 容器的高。shape为Rect时有效。
        /// [default: 0.35f]
        /// </summary>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtil.SetStruct(ref m_Height, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The smoothness of wave.
        /// 水波平滑度。
        /// [default: 1f]
        /// </summary>
        public float smoothness
        {
            get { return m_Smoothness; }
            set { if (PropertyUtil.SetStruct(ref m_Smoothness, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Background color of polar, which is transparent by default.
        /// 背景色，默认透明。
        /// [default: `Color.clear`]
        /// </summary>
        public Color32 backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Vessel color. The default is consistent with Serie.
        /// 容器颜色。默认和serie一致。
        /// </summary>
        public Color32 color
        {
            get { return m_Color; }
            set { if (PropertyUtil.SetColor(ref m_Color, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether automatic color. If true, the color matches serie.
        /// 是否自动颜色。为true时颜色会和serie一致。
        /// [default: true]
        /// </summary>
        public bool autoColor
        {
            get { return m_AutoColor; }
            set { if (PropertyUtil.SetStruct(ref m_AutoColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The radius of rounded corner. Its unit is px. Use array to respectively specify the 4 corner radiuses((clockwise upper left, upper right, bottom right and bottom left)).
        /// 容器的圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。shape为Rect时有效。
        /// </summary>
        public float[] cornerRadius
        {
            get { return m_CornerRadius; }
            set { if (PropertyUtil.SetClass(ref m_CornerRadius, value, true)) SetVerticesDirty(); }
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
        /// The actual radius after deducting shapeWidth and gap.
        /// 运行时内半径。扣除厚度和间隙后的实际半径。
        /// </summary>
        public float runtimeInnerRadius { get; internal set; }
        public float runtimeWidth { get; set; }
        public float runtimeHeight { get; set; }
        public static Vessel defaultVessel
        {
            get
            {
                var vessel = new Vessel
                {
                    m_Show = true,
                    m_Shape = Shape.Circle,
                    m_ShapeWidth = 5,
                    m_Gap = 5,
                    m_Radius = 0.35f,
                    m_Width = 0.5f,
                    m_Height = 0.7f,
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