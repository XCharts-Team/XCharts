
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// The style of line.
    /// 线条样式。
    /// 注： 修改 lineStyle 中的颜色不会影响图例颜色，如果需要图例颜色和折线图颜色一致，需修改 itemStyle.color，线条颜色默认也会取改颜色。
    /// </summary>
    [System.Serializable]
    public class LineStyle
    {
        /// <summary>
        /// 线的类型。
        /// </summary>
        public enum Type
        {
            Solid,
            Dashed,
            Dotted
        }
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Type m_Type = Type.Solid;
        [SerializeField] private Color m_Color;
        [SerializeField] private float m_Width = 0.8f;
        [SerializeField] [Range(0, 1)] private float m_Opacity = 1;

        /// <summary>
        /// Set this to false to prevent the areafrom showing.
        /// 是否显示区域填充。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// the type of line.
        /// 线的类型。
        /// </summary>
        public Type type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// the color of line, default use serie color.
        /// 线的颜色。
        /// </summary>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// the width of line.
        /// 线宽。
        /// </summary>
        public float width { get { return m_Width; } set { m_Width = value; } }
        /// <summary>
        /// Opacity of the line. Supports value from 0 to 1, and the line will not be drawn when set to 0.
        /// 线的透明度。支持从 0 到 1 的数字，为 0 时不绘制该图形。
        /// </summary>
        public float opacity { get { return m_Opacity; } set { m_Opacity = value; } }
    }
}