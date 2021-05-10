/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using System;

namespace XCharts
{
    /// <summary>
    /// Global parameter setting component. The default value can be used in general, and can be adjusted when necessary.
    /// 全局参数设置组件。一般情况下可使用默认值，当有需要时可进行调整。
    /// </summary>
    [Serializable]
    public class Settings : MainComponent
    {
        [SerializeField] [Range(1, 20)] protected int m_MaxPainter = 10;
        [SerializeField] protected bool m_ReversePainter = false;
        [SerializeField] protected Material m_BasePainterMaterial;
        [SerializeField] protected Material m_SeriePainterMaterial;
        [SerializeField] protected Material m_TopPainterMaterial;
        [SerializeField] [Range(1, 10)] protected float m_LineSmoothStyle = 3f;
        [SerializeField] [Range(1f, 20)] protected float m_LineSmoothness = 2f;
        [SerializeField] [Range(0.5f, 20)] protected float m_LineSegmentDistance = 3f;
        [SerializeField] [Range(1, 10)] protected float m_CicleSmoothness = 2f;
        [SerializeField] protected float m_LegendIconLineWidth = 2;
        [SerializeField] private float[] m_LegendIconCornerRadius = new float[] { 0.25f, 0.25f, 0.25f, 0.25f };

        /// <summary>
        /// max painter.
        /// 设定的painter数量。
        /// </summary>
        public int maxPainter
        {
            get { return m_MaxPainter; }
            set { if (PropertyUtil.SetStruct(ref m_MaxPainter, value < 0 ? 1 : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Painter是否逆序。逆序时index大的serie最先绘制。
        /// </summary>
        public bool reversePainter
        {
            get { return m_ReversePainter; }
            set { if (PropertyUtil.SetStruct(ref m_ReversePainter, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Base Pointer 材质球，设置后会影响Axis等。
        /// </summary>
        public Material basePainterMaterial
        {
            get { return m_BasePainterMaterial; }
            set { if (PropertyUtil.SetClass(ref m_BasePainterMaterial, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Serie Pointer 材质球，设置后会影响所有Serie。
        /// </summary>
        public Material seriePainterMaterial
        {
            get { return m_SeriePainterMaterial; }
            set { if (PropertyUtil.SetClass(ref m_SeriePainterMaterial, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Top Pointer 材质球，设置后会影响Tooltip等。
        /// </summary>
        public Material topPainterMaterial
        {
            get { return m_TopPainterMaterial; }
            set { if (PropertyUtil.SetClass(ref m_TopPainterMaterial, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Curve smoothing factor. By adjusting the smoothing coefficient, the curvature of the curve can be changed, 
        /// and different curves with slightly different appearance can be obtained.
        /// 曲线平滑系数。通过调整平滑系数可以改变曲线的曲率，得到外观稍微有变化的不同曲线。
        /// </summary>
        public float lineSmoothStyle
        {
            get { return m_LineSmoothStyle; }
            set { if (PropertyUtil.SetStruct(ref m_LineSmoothStyle, value < 0 ? 1f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Smoothness of curve. The smaller the value, the smoother the curve, but the number of vertices will increase. 
        /// When the area with gradient is filled, the larger the value, the worse the transition effect.
        /// 曲线平滑度。值越小曲线越平滑，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。
        /// </summary>
        /// <value></value>
        public float lineSmoothness
        {
            get { return m_LineSmoothness; }
            set { if (PropertyUtil.SetStruct(ref m_LineSmoothStyle, value < 0 ? 1f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The partition distance of a line segment. A line in a normal line chart is made up of many segments, 
        /// the number of which is determined by the change in value. The smaller the number of segments, 
        /// the higher the number of vertices. When the area with gradient is filled, the larger the value, the worse the transition effect.
        /// 线段的分割距离。普通折线图的线是由很多线段组成，段数由该数值决定。值越小段数越多，但顶点数也会随之增加。当开启有渐变的区域填充时，数值越大渐变过渡效果越差。
        /// </summary>
        /// <value></value>
        public float lineSegmentDistance
        {
            get { return m_LineSegmentDistance; }
            set { if (PropertyUtil.SetStruct(ref m_LineSegmentDistance, value < 0 ? 1f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the smoothess of cricle.
        /// 圆形的平滑度。数越小圆越平滑，但顶点数也会随之增加。
        /// </summary>
        public float cicleSmoothness
        {
            get { return m_CicleSmoothness; }
            set { if (PropertyUtil.SetStruct(ref m_CicleSmoothness, value < 0 ? 1f : value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the width of line serie legend.
        /// Line类型图例图标的线条宽度。
        /// </summary>
        public float legendIconLineWidth
        {
            get { return m_LegendIconLineWidth; }
            set { if (PropertyUtil.SetStruct(ref m_LegendIconLineWidth, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// The radius of rounded corner. Its unit is px. Use array to respectively specify the 4 corner radiuses((clockwise upper left, upper right, bottom right and bottom left)).
        /// 图例圆角半径。用数组分别指定4个圆角半径（顺时针左上，右上，右下，左下）。
        /// </summary>
        public float[] legendIconCornerRadius
        {
            get { return m_LegendIconCornerRadius; }
            set { if (PropertyUtil.SetClass(ref m_LegendIconCornerRadius, value, true)) SetVerticesDirty(); }
        }

        public void Copy(Settings settings)
        {
            m_ReversePainter = settings.reversePainter;
            m_MaxPainter = settings.maxPainter;
            m_BasePainterMaterial = settings.basePainterMaterial;
            m_SeriePainterMaterial = settings.seriePainterMaterial;
            m_TopPainterMaterial = settings.topPainterMaterial;
            m_LineSmoothStyle = settings.lineSmoothStyle;
            m_LineSmoothness = settings.lineSmoothness;
            m_LineSegmentDistance = settings.lineSegmentDistance;
            m_CicleSmoothness = settings.cicleSmoothness;
            m_LegendIconLineWidth = settings.legendIconLineWidth;
            ChartHelper.CopyArray(m_LegendIconCornerRadius, settings.legendIconCornerRadius);
        }

        public void Reset()
        {
            Copy(DefaultSettings);
        }

        public static Settings DefaultSettings
        {
            get
            {
                return new Settings()
                {
                    m_ReversePainter = false,
                    m_MaxPainter = XChartsSettings.maxPainter,
                    m_LineSmoothStyle = XChartsSettings.lineSmoothStyle,
                    m_LineSmoothness = XChartsSettings.lineSmoothness,
                    m_LineSegmentDistance = XChartsSettings.lineSegmentDistance,
                    m_CicleSmoothness = XChartsSettings.cicleSmoothness,
                    m_LegendIconLineWidth = 2,
                    m_LegendIconCornerRadius = new float[] { 0.25f, 0.25f, 0.25f, 0.25f }
                };
            }
        }
    }
}