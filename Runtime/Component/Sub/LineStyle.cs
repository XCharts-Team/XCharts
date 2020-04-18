/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/


using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// The style of line.
    /// 线条样式。
    /// 注： 修改 lineStyle 中的颜色不会影响图例颜色，如果需要图例颜色和折线图颜色一致，需修改 itemStyle.color，线条颜色默认也会取改颜色。
    /// </summary>
    [System.Serializable]
    public class LineStyle : SubComponent
    {
        /// <summary>
        /// 线的类型。
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// 实线
            /// </summary>
            Solid,
            /// <summary>
            /// 虚线
            /// </summary>
            Dashed,
            /// <summary>
            /// 点线
            /// </summary>
            Dotted,
            /// <summary>
            /// 点划线
            /// </summary>
            DashDot,
            /// <summary>
            /// 双点划线
            /// </summary>
            DashDotDot,
        }
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Type m_Type = Type.Solid;
        [SerializeField] private Color m_Color;
        [SerializeField] private float m_Width = 0.8f;
        [SerializeField] [Range(0, 1)] private float m_Opacity = 1;

        /// <summary>
        /// Whether show line.
        /// 是否显示线条。当作为子组件，它的父组件有参数控制是否显示时，改参数无效。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the type of line.
        /// 线的类型。
        /// </summary>
        public Type type
        {
            get { return m_Type; }
            set { if (PropertyUtility.SetStruct(ref m_Type, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of line, default use serie color.
        /// 线的颜色。
        /// </summary>
        public Color color
        {
            get { return m_Color; }
            set { if (PropertyUtility.SetColor(ref m_Color, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of line.
        /// 线宽。
        /// /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtility.SetStruct(ref m_Width, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Opacity of the line. Supports value from 0 to 1, and the line will not be drawn when set to 0.
        /// 线的透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
        /// </summary>
        public float opacity
        {
            get { return m_Opacity; }
            set { if (PropertyUtility.SetStruct(ref m_Opacity, value)) SetVerticesDirty(); }
        }

        public LineStyle()
        {
        }

        public LineStyle(float width)
        {
            this.width = width;
        }

        public LineStyle(LineStyle.Type type, float width)
        {
            this.type = type;
            this.width = width;
        }

        public LineStyle Clone()
        {
            var lineStyle = new LineStyle();
            lineStyle.show = show;
            lineStyle.type = type;
            lineStyle.color = color;
            lineStyle.width = width;
            lineStyle.opacity = opacity;
            return lineStyle;
        }

        public void Copy(LineStyle lineStyle)
        {
            show = lineStyle.show;
            type = lineStyle.type;
            color = lineStyle.color;
            width = lineStyle.width;
            opacity = lineStyle.opacity;
        }

        public Color GetColor()
        {
            var color = m_Color;
            color.a *= m_Opacity;
            return color;
        }
    }
}