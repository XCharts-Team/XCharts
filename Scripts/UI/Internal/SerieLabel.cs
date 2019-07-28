using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    /// <summary>
    /// Text label of chart, to explain some data information about graphic item like value, name and so on.
    /// 图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
    /// </summary>
    public class SerieLabel
    {
        /// <summary>
        /// The position of label.标签的位置。
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// Outside of sectors of pie chart, which relates to corresponding sector through visual guide line.
            /// 饼图扇区外侧，通过视觉引导线连到相应的扇区。
            /// </summary>
            Outside,
            /// <summary>
            /// Inside the sectors of pie chart.饼图扇区内部。
            /// </summary>
            Inside,
            /// <summary>
            /// In the center of pie chart.在饼图中心位置。
            Center,
            Top,
            Left,
            Right,
            Bottom,
        }
        [SerializeField] private bool m_Show = false;
        [SerializeField] Position m_Position;
        [SerializeField] private float m_Distance;
        [SerializeField] private float m_Rotate;
        [SerializeField] private Color m_Color;
        [SerializeField] private int m_FontSize = 18;
        [SerializeField] private FontStyle m_FontStyle = FontStyle.Normal;
        [SerializeField] private bool m_Line = true;
        [SerializeField] private float m_LineWidth = 1.0f;
        [SerializeField] private float m_LineLength1 = 25f;
        [SerializeField] private float m_LineLength2 = 15f;
        /// <summary>
        /// Whether the label is showed.是否显示文本标签。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// The position of label.标签的位置。
        /// </summary>
        public Position position { get { return m_Position; } set { m_Position = value; } }
        /// <summary>
        /// Distance to the host graphic element. Works when position is Top,Left,Right,Bottom.
        /// 距离图形元素的距离，当position为Top，Left，Right，Bottom时有效。
        /// </summary>
        public float distance { get { return m_Distance; } set { m_Distance = value; } }
        /// <summary>
        /// Text color,If set as default ,the color will assigned as series color.
        /// 自定义文字颜色，默认和系列的颜色一致。
        /// </summary>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// Rotate label.标签旋转。
        /// </summary>
        public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
        /// <summary>
        /// font size.文字的字体大小。
        /// </summary>
        public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
        /// <summary>
        /// font style.文字的字体风格。
        /// </summary>
        public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }
        public bool line { get { return m_Line; } set { m_Line = value; } }
        public float lineWidth { get { return m_LineWidth; } set { m_LineWidth = value; } }
        public float lineLength1 { get { return m_LineLength1; } set { m_LineLength1 = value; } }
        public float lineLength2 { get { return m_LineLength2; } set { m_LineLength2 = value; } }
    }
}
