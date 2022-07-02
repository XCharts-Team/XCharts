using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Text label of chart, to explain some data information about graphic item like value, name and so on.
    /// |图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
    /// </summary>
    [System.Serializable]
    public class LabelStyle : ChildComponent, ISerieExtraComponent, ISerieDataComponent
    {
        /// <summary>
        /// The position of label.
        /// |标签的位置。
        /// </summary>
        public enum Position
        {
            Default,
            /// <summary>
            /// Outside of sectors of pie chart, which relates to corresponding sector through visual guide line.
            /// |饼图扇区外侧，通过视觉引导线连到相应的扇区。
            /// </summary>
            Outside,
            /// <summary>
            /// Inside the sectors of pie chart.
            /// |饼图扇区内部。
            /// </summary>
            Inside,
            /// <summary>
            /// In the center of pie chart.
            /// |在饼图中心位置。
            /// </summary>
            Center,
            /// <summary>
            /// top of symbol.
            /// |图形标志的顶部。
            /// </summary>
            Top,
            /// <summary>
            /// the bottom of symbol.
            /// |图形标志的底部。
            /// </summary>
            Bottom,
            /// <summary>
            /// the left of symbol.
            /// |图形标志的左边。
            /// </summary>
            Left,
            /// <summary>
            /// the right of symbol.
            /// |图形标志的右边。
            /// </summary>
            Right,
            /// <summary>
            /// the start of line.
            /// |线的起始点。
            /// </summary>
            Start,
            /// <summary>
            /// the middle of line.
            /// |线的中点。
            /// </summary>
            Middle,
            /// <summary>
            /// the end of line.
            /// |线的结束点。
            /// </summary>
            End
        }

        [SerializeField] protected bool m_Show = true;
        [SerializeField] Position m_Position = Position.Default;
        [SerializeField] protected bool m_AutoOffset = false;
        [SerializeField] protected Vector3 m_Offset;
        [SerializeField] protected float m_Rotate;
        [SerializeField] protected float m_Distance;
        [SerializeField] protected string m_Formatter;
        [SerializeField] protected string m_NumericFormatter = "";
        [SerializeField] protected float m_Width = 0;
        [SerializeField] protected float m_Height = 0;

        [SerializeField] protected IconStyle m_Icon = new IconStyle();
        [SerializeField] protected ImageStyle m_Background = new ImageStyle();
        [SerializeField] protected TextPadding m_TextPadding = new TextPadding();
        [SerializeField] protected TextStyle m_TextStyle = new TextStyle();
        protected LabelFormatterFunction m_FormatterFunction;

        public void Reset()
        {
            m_Show = false;
            m_Position = Position.Default;
            m_Offset = Vector3.zero;
            m_Distance = 0;
            m_Rotate = 0;
            m_Width = 0;
            m_Height = 0;
            m_NumericFormatter = "";
            m_AutoOffset = false;
        }

        /// <summary>
        /// Whether the label is showed.
        /// |是否显示文本标签。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The position of label.
        /// |标签的位置。
        /// </summary>
        public Position position
        {
            get { return m_Position; }
            set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetAllDirty(); }
        }
        /// <summary>
        /// formatter of label.
        /// |标签内容字符串模版格式器。支持用 \n 换行。
        /// 模板变量有：
        /// {.}：圆点标记。
        /// {a}：系列名。
        /// {a}：系列名。
        /// {b}：类目值或数据名。
        /// {c}：数据值。
        /// {d}：百分比。
        /// {e}：数据名。
        /// {f}：数据和。
        /// 示例：“{b}:{c}”
        /// </summary>
        public string formatter
        {
            get { return m_Formatter; }
            set { if (PropertyUtil.SetClass(ref m_Formatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// offset to the host graphic element.
        /// |距离图形元素的偏移
        /// </summary>
        public Vector3 offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Rotation of label.
        /// |文本的旋转。
        /// </summary>
        public float rotate
        {
            get { return m_Rotate; }
            set { if (PropertyUtil.SetStruct(ref m_Rotate, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 距离轴线的距离。
        /// </summary>
        public float distance
        {
            get { return m_Distance; }
            set { if (PropertyUtil.SetStruct(ref m_Distance, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the width of label. If set as default value 0, it means than the label width auto set as the text width.
        /// |标签的宽度。一般不用指定，不指定时则自动是文字的宽度。
        /// </summary>
        /// <value></value>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the height of label. If set as default value 0, it means than the label height auto set as the text height.
        /// |标签的高度。一般不用指定，不指定时则自动是文字的高度。
        /// </summary>
        /// <value></value>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtil.SetStruct(ref m_Height, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the text padding of label. 
        /// |文本的边距。
        /// </summary>
        public TextPadding textPadding
        {
            get { return m_TextPadding; }
            set { if (PropertyUtil.SetClass(ref m_TextPadding, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Standard numeric format strings.
        /// |标准数字格式字符串。用于将数值格式化显示为字符串。
        /// 使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。
        /// 参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings
        /// </summary>
        /// <value></value>
        public string numericFormatter
        {
            get { return m_NumericFormatter; }
            set { if (PropertyUtil.SetClass(ref m_NumericFormatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 是否开启自动偏移。当开启时，Y的偏移会自动判断曲线的开口来决定向上还是向下偏移。
        /// </summary>
        public bool autoOffset
        {
            get { return m_AutoOffset; }
            set { if (PropertyUtil.SetStruct(ref m_AutoOffset, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the sytle of background.
        /// |背景图样式。
        /// </summary>
        public ImageStyle background
        {
            get { return m_Background; }
            set { if (PropertyUtil.SetClass(ref m_Background, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the sytle of icon.
        /// |图标样式。
        /// </summary>
        public IconStyle icon
        {
            get { return m_Icon; }
            set { if (PropertyUtil.SetClass(ref m_Icon, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the sytle of text.
        /// |文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value)) SetAllDirty(); }
        }
        public LabelFormatterFunction formatterFunction
        {
            get { return m_FormatterFunction; }
            set { m_FormatterFunction = value; }
        }

        public bool IsInside()
        {
            return m_Position == Position.Inside || m_Position == Position.Center;
        }

        public bool IsDefaultPosition(Position position)
        {
            return m_Position == Position.Default || m_Position == position;
        }

        public bool IsAutoSize()
        {
            return width == 0 && height == 0;
        }

        public Vector3 GetOffset(float radius)
        {
            var x = ChartHelper.GetActualValue(m_Offset.x, radius);
            var y = ChartHelper.GetActualValue(m_Offset.y, radius);
            var z = ChartHelper.GetActualValue(m_Offset.z, radius);
            return new Vector3(x, y, z);
        }

        public Color GetColor(Color defaultColor)
        {
            if (ChartHelper.IsClearColor(textStyle.color))
            {
                return IsInside() ? Color.black : defaultColor;
            }
            else
            {
                return textStyle.color;
            }
        }

        public virtual LabelStyle Clone()
        {
            var label = new LabelStyle();
            label.m_Show = m_Show;
            label.m_Position = m_Position;
            label.m_Offset = m_Offset;
            label.m_Rotate = m_Rotate;
            label.m_Distance = m_Distance;
            label.m_Formatter = m_Formatter;
            label.m_Width = m_Width;
            label.m_Height = m_Height;
            label.m_NumericFormatter = m_NumericFormatter;
            label.m_AutoOffset = m_AutoOffset;
            label.m_Icon.Copy(m_Icon);
            label.m_Background.Copy(m_Background);
            label.m_TextPadding = m_TextPadding;
            label.m_TextStyle.Copy(m_TextStyle);
            return label;
        }

        public virtual void Copy(LabelStyle label)
        {
            m_Show = label.m_Show;
            m_Position = label.m_Position;
            m_Offset = label.m_Offset;
            m_Rotate = label.m_Rotate;
            m_Distance = label.m_Distance;
            m_Formatter = label.m_Formatter;
            m_Width = label.m_Width;
            m_Height = label.m_Height;
            m_NumericFormatter = label.m_NumericFormatter;
            m_AutoOffset = label.m_AutoOffset;
            m_Icon.Copy(label.m_Icon);
            m_Background.Copy(label.m_Background);
            m_TextPadding = label.m_TextPadding;
            m_TextStyle.Copy(label.m_TextStyle);
        }
    }
}