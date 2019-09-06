using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Text label of chart, to explain some data information about graphic item like value, name and so on.
    /// 图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
    /// </summary>
    [System.Serializable]
    public class SerieLabel
    {
        /// <summary>
        /// The position of label.
        /// 标签的位置。
        /// </summary>
        public enum Position
        {
            /// <summary>
            /// Outside of sectors of pie chart, which relates to corresponding sector through visual guide line.
            /// 饼图扇区外侧，通过视觉引导线连到相应的扇区。
            /// </summary>
            Outside,
            /// <summary>
            /// Inside the sectors of pie chart.
            /// 饼图扇区内部。
            /// </summary>
            Inside,
            /// <summary>
            /// In the center of pie chart.
            /// 在饼图中心位置。
            /// </summary>
            Center,
            /// <summary>
            /// top of symbol.
            /// 图形标志的顶部。
            /// </summary>
            Top,
            /// <summary>
            /// the left of symbol.
            /// 图形标志的左边。
            /// </summary>
            //Left,
            /// <summary>
            /// the right of symbol.
            /// 图形标志的右边。
            /// </summary>
            //Right,
            /// <summary>
            /// the bottom of symbol.
            /// 图形标志的底部。
            /// </summary>
            Bottom,
        }
        [SerializeField] private bool m_Show = false;
        [SerializeField] Position m_Position;
        [SerializeField] private float m_Distance = 0;
        [SerializeField] private float m_Rotate = 0;
        [SerializeField] private float m_PaddingLeftRight = 2f;
        [SerializeField] private float m_PaddingTopBottom = 2f;
        [SerializeField] private Color m_Color;
        [SerializeField] private Color m_BackgroundColor;
        [SerializeField] private float m_BackgroundWidth = 0;
        [SerializeField] private float m_BackgroundHeight = 0;
        [SerializeField] private int m_FontSize = 18;
        [SerializeField] private FontStyle m_FontStyle = FontStyle.Normal;
        [SerializeField] private bool m_Line = true;
        [SerializeField] private float m_LineWidth = 1.0f;
        [SerializeField] private float m_LineLength1 = 25f;
        [SerializeField] private float m_LineLength2 = 15f;
        [SerializeField] private bool m_Border = true;
        [SerializeField] private float m_BorderWidth = 0.5f;
        [SerializeField] private Color m_BorderColor = Color.grey;
        /// <summary>
        /// Whether the label is showed.
        /// 是否显示文本标签。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// The position of label.
        /// 标签的位置。
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
        /// the background color. If set as default, it means than don't show background.
        /// 标签的背景色，默认无颜色。
        /// </summary>
        public Color backgroundColor { get { return m_BackgroundColor; } set { m_BackgroundColor = value; } }
        /// <summary>
        /// the width of background. If set as default value 0, it means than the background width auto set as the text width.
        /// 标签的背景宽度。一般不用指定，不指定时则自动是文字的宽度。
        /// </summary>
        /// <value></value>
        public float backgroundWidth { get { return m_BackgroundWidth; } set { m_BackgroundWidth = value; } }
        /// <summary>
        /// the height of background. If set as default value 0, it means than the background height auto set as the text height.
        /// 标签的背景高度。一般不用指定，不指定时则自动是文字的高度。
        /// </summary>
        /// <value></value>
        public float backgroundHeight { get { return m_BackgroundHeight; } set { m_BackgroundHeight = value; } }
        /// <summary>
        /// Rotate label.
        /// 标签旋转。
        /// </summary>
        public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
        /// <summary>
        /// the text padding of left and right. defaut:2.
        /// 左右边距。
        /// </summary>
        public float paddingLeftRight { get { return m_PaddingLeftRight; } set { m_PaddingLeftRight = value; } }
        /// <summary>
        /// the text padding of top and bottom. defaut:2.
        /// 上下边距。
        /// </summary>
        public float paddingTopBottom { get { return m_PaddingTopBottom; } set { m_PaddingTopBottom = value; } }
        /// <summary>
        /// font size.
        /// 文字的字体大小。
        /// </summary>
        public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
        /// <summary>
        /// font style.
        /// 文字的字体风格。
        /// </summary>
        public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }
        /// <summary>
        /// Whether to show visual guide line.Will show when label position is set as 'outside'.
        /// 是否显示视觉引导线。在 label 位置 设置为'outside'的时候会显示视觉引导线。
        /// </summary>
        public bool line { get { return m_Line; } set { m_Line = value; } }
        /// <summary>
        /// the width of visual guild line.
        /// 视觉引导线的宽度。
        /// </summary>
        public float lineWidth { get { return m_LineWidth; } set { m_LineWidth = value; } }
        /// <summary>
        /// The length of the first segment of visual guide line.
        /// 视觉引导线第一段的长度。
        /// </summary>
        public float lineLength1 { get { return m_LineLength1; } set { m_LineLength1 = value; } }
        /// <summary>
        /// The length of the second segment of visual guide line.
        /// 视觉引导线第二段的长度。
        /// </summary>
        public float lineLength2 { get { return m_LineLength2; } set { m_LineLength2 = value; } }
        /// <summary>
        /// Whether to show border.
        /// 是否显示边框。
        /// </summary>
        public bool border { get { return m_Border; } set { m_Border = value; } }
        /// <summary>
        /// the width of border.
        /// 边框宽度。
        /// </summary>
        public float borderWidth { get { return m_BorderWidth; } set { m_BorderWidth = value; } }
        /// <summary>
        /// the color of border.
        /// 边框颜色。
        /// </summary>
        public Color borderColor { get { return m_BorderColor; } set { m_BorderColor = value; } }
    }
}
