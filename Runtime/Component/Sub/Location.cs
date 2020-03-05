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
    /// Location type. Quick to set the general location.
    /// 位置类型。通过Align快速设置大体位置，再通过left，right，top，bottom微调具体位置。
    /// </summary>
    [Serializable]
    public class Location : SubComponent, IPropertyChanged
    {
        /// <summary>
        /// 对齐方式
        /// </summary>
        public enum Align
        {
            TopLeft,
            TopRight,
            TopCenter,
            BottomLeft,
            BottomRight,
            BottomCenter,
            Center,
            CenterLeft,
            CenterRight
        }

        [SerializeField] private Align m_Align = Align.TopCenter;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;

        private TextAnchor m_TextAnchor;
        private Vector2 m_AnchorMin;
        private Vector2 m_AnchorMax;
        private Vector2 m_Pivot;

        /// <summary>
        /// 对齐方式。
        /// </summary>
        public Align align
        {
            get { return m_Align; }
            set { if (PropertyUtility.SetStruct(ref m_Align, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// 离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtility.SetStruct(ref m_Left, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// 离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtility.SetStruct(ref m_Right, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// 离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtility.SetStruct(ref m_Top, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// 离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtility.SetStruct(ref m_Bottom, value)) { SetComponentDirty(); UpdateAlign(); } }
        }

        /// <summary>
        /// the anchor of text.
        /// Location对应的Anchor锚点
        /// </summary>
        /// <value></value>
        public TextAnchor runtimeTextAnchor { get { return m_TextAnchor; } }
        /// <summary>
        /// the minimum achor.
        /// Location对应的anchorMin。
        /// </summary>
        /// <value></value>
        public Vector2 runtimeAnchorMin { get { return m_AnchorMin; } }
        /// <summary>
        /// the maximun achor.
        /// Location对应的anchorMax.
        /// </summary>
        /// <value></value>
        public Vector2 runtimeAnchorMax { get { return m_AnchorMax; } }
        /// <summary>
        /// the povot.
        /// Loation对应的中心点。
        /// </summary>
        /// <value></value>
        public Vector2 runtimePivot { get { return m_Pivot; } }

        public static Location defaultLeft
        {
            get
            {
                return new Location()
                {
                    align = Align.CenterLeft,
                    left = 5,
                    right = 0,
                    top = 0,
                    bottom = 0
                };
            }
        }

        public static Location defaultRight
        {
            get
            {
                return new Location()
                {
                    align = Align.CenterRight,
                    left = 0,
                    right = 5,
                    top = 0,
                    bottom = 0
                };
            }
        }

        public static Location defaultTop
        {
            get
            {
                return new Location()
                {
                    align = Align.TopCenter,
                    left = 0,
                    right = 0,
                    top = 5,
                    bottom = 0
                };
            }
        }

        public static Location defaultBottom
        {
            get
            {
                return new Location()
                {
                    align = Align.BottomCenter,
                    left = 0,
                    right = 0,
                    top = 0,
                    bottom = 5
                };
            }
        }

        private void UpdateAlign()
        {
            switch (m_Align)
            {
                case Align.BottomCenter:
                    m_TextAnchor = TextAnchor.LowerCenter;
                    m_AnchorMin = new Vector2(0.5f, 0);
                    m_AnchorMax = new Vector2(0.5f, 0);
                    m_Pivot = new Vector2(0.5f, 0);
                    break;
                case Align.BottomLeft:
                    m_TextAnchor = TextAnchor.LowerLeft;
                    m_AnchorMin = new Vector2(0, 0);
                    m_AnchorMax = new Vector2(0, 0);
                    m_Pivot = new Vector2(0, 0);
                    break;
                case Align.BottomRight:
                    m_TextAnchor = TextAnchor.LowerRight;
                    m_AnchorMin = new Vector2(1, 0);
                    m_AnchorMax = new Vector2(1, 0);
                    m_Pivot = new Vector2(1, 0);
                    break;
                case Align.Center:
                    m_TextAnchor = TextAnchor.MiddleCenter;
                    m_AnchorMin = new Vector2(0.5f, 0.5f);
                    m_AnchorMax = new Vector2(0.5f, 0.5f);
                    m_Pivot = new Vector2(0.5f, 0.5f);
                    break;
                case Align.CenterLeft:
                    m_TextAnchor = TextAnchor.MiddleLeft;
                    m_AnchorMin = new Vector2(0, 0.5f);
                    m_AnchorMax = new Vector2(0, 0.5f);
                    m_Pivot = new Vector2(0, 0.5f);
                    break;
                case Align.CenterRight:
                    m_TextAnchor = TextAnchor.MiddleRight;
                    m_AnchorMin = new Vector2(1, 0.5f);
                    m_AnchorMax = new Vector2(1, 0.5f);
                    m_Pivot = new Vector2(1, 0.5f);
                    break;
                case Align.TopCenter:
                    m_TextAnchor = TextAnchor.UpperCenter;
                    m_AnchorMin = new Vector2(0.5f, 1);
                    m_AnchorMax = new Vector2(0.5f, 1);
                    m_Pivot = new Vector2(0.5f, 1);
                    break;
                case Align.TopLeft:
                    m_TextAnchor = TextAnchor.UpperLeft;
                    m_AnchorMin = new Vector2(0, 1);
                    m_AnchorMax = new Vector2(0, 1);
                    m_Pivot = new Vector2(0, 1);
                    break;
                case Align.TopRight:
                    m_TextAnchor = TextAnchor.UpperRight;
                    m_AnchorMin = new Vector2(1, 1);
                    m_AnchorMax = new Vector2(1, 1);
                    m_Pivot = new Vector2(1, 1);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 返回在坐标系中的具体位置
        /// </summary>
        /// <param name="chartWidth"></param>
        /// <param name="chartHeight"></param>
        /// <returns></returns>
        public Vector3 GetPosition(float chartWidth, float chartHeight)
        {
            switch (align)
            {
                case Align.BottomCenter:
                    return new Vector3(chartWidth / 2, bottom);
                case Align.BottomLeft:
                    return new Vector3(left, bottom);
                case Align.BottomRight:
                    return new Vector3(chartWidth - right, bottom);
                case Align.Center:
                    return new Vector3(chartWidth / 2, chartHeight / 2);
                case Align.CenterLeft:
                    return new Vector3(left, chartHeight / 2);
                case Align.CenterRight:
                    return new Vector3(chartWidth - right, chartHeight / 2);
                case Align.TopCenter:
                    return new Vector3(chartWidth / 2, chartHeight - top);
                case Align.TopLeft:
                    return new Vector3(left, chartHeight - top);
                case Align.TopRight:
                    return new Vector3(chartWidth - right, chartHeight - top);
                default:
                    return Vector2.zero;
            }
        }

        /// <summary>
        /// 属性变更时更新textAnchor,minAnchor,maxAnchor,pivot
        /// </summary>
        public void OnChanged()
        {
            UpdateAlign();
        }
    }
}
