/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
        [SerializeField] [Range(1, 10)] protected float m_LineSmoothStyle = 3f;
        [SerializeField] [Range(1f, 20)] protected float m_LineSmoothness = 2f;
        [SerializeField] [Range(1f, 20)] protected float m_LineSegmentDistance = 3f;
        [SerializeField] [Range(1, 10)] protected float m_CicleSmoothness = 2f;
        [SerializeField] [Range(10, 50)] protected float m_VisualMapTriangeLen = 20f;
        [SerializeField] [Range(1, 20)] protected float m_PieTooltipExtraRadius = 8f;
        [SerializeField] [Range(1, 20)] protected float m_PieSelectedOffset = 8f;

        /// <summary>
        /// Curve smoothing factor. By adjusting the smoothing coefficient, the curvature of the curve can be changed, 
        /// and different curves with slightly different appearance can be obtained.
        /// 曲线平滑系数。通过调整平滑系数可以改变曲线的曲率，得到外观稍微有变化的不同曲线。
        /// </summary>
        public float lineSmoothStyle
        {
            get { return m_LineSmoothStyle; }
            set { if (PropertyUtility.SetStruct(ref m_LineSmoothStyle, value < 0 ? 1f : value)) SetVerticesDirty(); }
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
            set { if (PropertyUtility.SetStruct(ref m_LineSmoothStyle, value < 0 ? 1f : value)) SetVerticesDirty(); }
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
            set { if (PropertyUtility.SetStruct(ref m_LineSegmentDistance, value < 0 ? 1f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the smoothess of cricle.
        /// 圆形的平滑度。数越小圆越平滑，但顶点数也会随之增加。
        /// </summary>
        public float cicleSmoothness
        {
            get { return m_CicleSmoothness; }
            set { if (PropertyUtility.SetStruct(ref m_CicleSmoothness, value < 0 ? 1f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 可视化组件的调节三角形边长。
        /// </summary>
        /// <value></value>
        public float visualMapTriangeLen
        {
            get { return m_VisualMapTriangeLen; }
            set { if (PropertyUtility.SetStruct(ref m_VisualMapTriangeLen, value < 0 ? 1f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 饼图鼠标移到高亮时的额外半径
        /// </summary>
        public float pieTooltipExtraRadius
        {
            get { return m_PieTooltipExtraRadius; }
            set { if (PropertyUtility.SetStruct(ref m_PieTooltipExtraRadius, value < 0 ? 0f : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 饼图选中时的中心点偏移
        /// </summary>
        public float pieSelectedOffset
        {
            get { return m_PieSelectedOffset; }
            set { if (PropertyUtility.SetStruct(ref m_PieSelectedOffset, value < 0 ? 0f : value)) SetVerticesDirty(); }
        }
    }
}