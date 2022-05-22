using System;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    /// <summary>
    /// Location type. Quick to set the general location.
    /// |位置类型。通过Align快速设置大体位置，再通过left，right，top，bottom微调具体位置。
    /// </summary>
    [Serializable]
    public class Location : ChildComponent, IPropertyChanged
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

        private TextAnchor m_TextAlignment;
#if dUI_TextMeshPro
        private TextAlignmentOptions m_TMPTextAlignment;
#endif
        private Vector2 m_AnchorMin;
        private Vector2 m_AnchorMax;
        private Vector2 m_Pivot;

        /// <summary>
        /// 对齐方式。
        /// </summary>
        public Align align
        {
            get { return m_Align; }
            set { if (PropertyUtil.SetStruct(ref m_Align, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// |离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// |离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// |离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) { SetComponentDirty(); UpdateAlign(); } }
        }
        /// <summary>
        /// Distance between component and the left side of the container.
        /// |离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) { SetComponentDirty(); UpdateAlign(); } }
        }

        /// <summary>
        /// the anchor of text.
        /// |Location对应的Anchor锚点
        /// </summary>
        /// <value></value>
        public TextAnchor runtimeTextAlignment { get { return m_TextAlignment; } }

#if dUI_TextMeshPro
        public TextAlignmentOptions runtimeTMPTextAlignment { get { return m_TMPTextAlignment; } }
#endif
        /// <summary>
        /// the minimum achor.
        /// |Location对应的anchorMin。
        /// </summary>
        public Vector2 runtimeAnchorMin { get { return m_AnchorMin; } }
        /// <summary>
        /// the maximun achor.
        /// |Location对应的anchorMax.
        /// |</summary>
        public Vector2 runtimeAnchorMax { get { return m_AnchorMax; } }
        /// <summary>
        /// the povot.
        /// |Loation对应的中心点。
        /// </summary>
        public Vector2 runtimePivot { get { return m_Pivot; } }
        public float runtimeLeft { get; private set; }
        public float runtimeRight { get; private set; }
        public float runtimeBottom { get; private set; }
        public float runtimeTop { get; private set; }

        public static Location defaultLeft
        {
            get
            {
                return new Location()
                {
                    align = Align.CenterLeft,
                        left = 0.03f,
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
                        right = 0.03f,
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
                        top = 0.03f,
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
                        bottom = 0.03f
                };
            }
        }

        private void UpdateAlign()
        {
            switch (m_Align)
            {
                case Align.BottomCenter:
                    m_TextAlignment = TextAnchor.LowerCenter;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.Bottom;
#endif
                    m_AnchorMin = new Vector2(0.5f, 0);
                    m_AnchorMax = new Vector2(0.5f, 0);
                    m_Pivot = new Vector2(0.5f, 0);
                    break;
                case Align.BottomLeft:
                    m_TextAlignment = TextAnchor.LowerLeft;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.BottomLeft;
#endif
                    m_AnchorMin = new Vector2(0, 0);
                    m_AnchorMax = new Vector2(0, 0);
                    m_Pivot = new Vector2(0, 0);
                    break;
                case Align.BottomRight:
                    m_TextAlignment = TextAnchor.LowerRight;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.BottomRight;
#endif
                    m_AnchorMin = new Vector2(1, 0);
                    m_AnchorMax = new Vector2(1, 0);
                    m_Pivot = new Vector2(1, 0);
                    break;
                case Align.Center:
                    m_TextAlignment = TextAnchor.MiddleCenter;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.Center;
#endif
                    m_AnchorMin = new Vector2(0.5f, 0.5f);
                    m_AnchorMax = new Vector2(0.5f, 0.5f);
                    m_Pivot = new Vector2(0.5f, 0.5f);
                    break;
                case Align.CenterLeft:
                    m_TextAlignment = TextAnchor.MiddleLeft;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.Left;
#endif
                    m_AnchorMin = new Vector2(0, 0.5f);
                    m_AnchorMax = new Vector2(0, 0.5f);
                    m_Pivot = new Vector2(0, 0.5f);
                    break;
                case Align.CenterRight:
                    m_TextAlignment = TextAnchor.MiddleRight;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.Right;
#endif
                    m_AnchorMin = new Vector2(1, 0.5f);
                    m_AnchorMax = new Vector2(1, 0.5f);
                    m_Pivot = new Vector2(1, 0.5f);
                    break;
                case Align.TopCenter:
                    m_TextAlignment = TextAnchor.UpperCenter;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.Top;
#endif
                    m_AnchorMin = new Vector2(0.5f, 1);
                    m_AnchorMax = new Vector2(0.5f, 1);
                    m_Pivot = new Vector2(0.5f, 1);
                    break;
                case Align.TopLeft:
                    m_TextAlignment = TextAnchor.UpperLeft;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.TopLeft;
#endif
                    m_AnchorMin = new Vector2(0, 1);
                    m_AnchorMax = new Vector2(0, 1);
                    m_Pivot = new Vector2(0, 1);
                    break;
                case Align.TopRight:
                    m_TextAlignment = TextAnchor.UpperRight;
#if dUI_TextMeshPro
                    m_TMPTextAlignment = TextAlignmentOptions.TopRight;
#endif
                    m_AnchorMin = new Vector2(1, 1);
                    m_AnchorMax = new Vector2(1, 1);
                    m_Pivot = new Vector2(1, 1);
                    break;
                default:
                    break;
            }
        }

        public void UpdateRuntimeData(float chartWidth, float chartHeight)
        {
            runtimeLeft = left <= 1 ? left * chartWidth : left;
            runtimeRight = right <= 1 ? right * chartWidth : right;
            runtimeTop = top <= 1 ? top * chartHeight : top;
            runtimeBottom = bottom <= 1 ? bottom * chartHeight : bottom;
        }

        /// <summary>
        /// 返回在坐标系中的具体位置
        /// </summary>
        /// <param name="chartWidth"></param>
        /// <param name="chartHeight"></param>
        /// <returns></returns>
        public Vector3 GetPosition(float chartWidth, float chartHeight)
        {
            UpdateRuntimeData(chartWidth, chartHeight);
            switch (align)
            {
                case Align.BottomCenter:
                    return new Vector3(chartWidth / 2, runtimeBottom);
                case Align.BottomLeft:
                    return new Vector3(runtimeLeft, runtimeBottom);
                case Align.BottomRight:
                    return new Vector3(chartWidth - runtimeRight, runtimeBottom);
                case Align.Center:
                    return new Vector3(chartWidth / 2, chartHeight / 2);
                case Align.CenterLeft:
                    return new Vector3(runtimeLeft, chartHeight / 2);
                case Align.CenterRight:
                    return new Vector3(chartWidth - runtimeRight, chartHeight / 2);
                case Align.TopCenter:
                    return new Vector3(chartWidth / 2, chartHeight - runtimeTop);
                case Align.TopLeft:
                    return new Vector3(runtimeLeft, chartHeight - runtimeTop);
                case Align.TopRight:
                    return new Vector3(chartWidth - runtimeRight, chartHeight - runtimeTop);
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