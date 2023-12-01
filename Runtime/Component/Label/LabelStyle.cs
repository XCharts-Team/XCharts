using System;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Text label of chart, to explain some data information about graphic item like value, name and so on.
    /// ||图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
    /// </summary>
    [System.Serializable]
    public class LabelStyle : ChildComponent, ISerieComponent, ISerieDataComponent
    {
        /// <summary>
        /// The position of label.
        /// ||标签的位置。
        /// </summary>
        public enum Position
        {
            Default,
            /// <summary>
            /// Outside of sectors of pie chart, which relates to corresponding sector through visual guide line.
            /// ||饼图扇区外侧，通过视觉引导线连到相应的扇区。
            /// </summary>
            Outside,
            /// <summary>
            /// Inside the sectors of pie chart.
            /// ||饼图扇区内部。
            /// </summary>
            Inside,
            /// <summary>
            /// In the center of pie chart.
            /// ||在饼图中心位置。
            /// </summary>
            Center,
            /// <summary>
            /// top of symbol.
            /// ||图形标志的顶部。
            /// </summary>
            Top,
            /// <summary>
            /// the bottom of symbol.
            /// ||图形标志的底部。
            /// </summary>
            Bottom,
            /// <summary>
            /// the left of symbol.
            /// ||图形标志的左边。
            /// </summary>
            Left,
            /// <summary>
            /// the right of symbol.
            /// ||图形标志的右边。
            /// </summary>
            Right,
            /// <summary>
            /// the start of line.
            /// ||线的起始点。
            /// </summary>
            Start,
            /// <summary>
            /// the middle of line.
            /// ||线的中点。
            /// </summary>
            Middle,
            /// <summary>
            /// the end of line.
            /// ||线的结束点。
            /// </summary>
            End
        }

        [SerializeField] protected bool m_Show = true;
        [SerializeField] Position m_Position = Position.Default;
        [SerializeField] protected bool m_AutoOffset = false;
        [SerializeField] protected Vector3 m_Offset;
        [SerializeField] protected float m_Rotate;
        [SerializeField][Since("v3.6.0")] protected bool m_AutoRotate = false;
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
        /// ||是否显示文本标签。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The position of label.
        /// ||标签的位置。
        /// </summary>
        public Position position
        {
            get { return m_Position; }
            set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetAllDirty(); }
        }
        /// <summary>
        /// label content string template formatter. \n line wrapping is supported. Formatters for some components will not take effect. <br />
        /// Template placeholder have the following, some of which apply only to fixed components: <br />
        /// `{.}` : indicates the dot mark. <br />
        /// `{a}` : indicates the series name. <br />
        /// `{b}` : category value or data name. <br />
        /// `{c}` : data value. <br />
        /// `{d}` : percentage. <br />
        /// `{e}` : indicates the data name. <br />
        /// `{f}` : data sum. <br />
        /// `{g}` : indicates the total number of data. <br />
        /// `{h}` : hexadecimal color value. <br />
        /// `{value}` : The value of the axis or legend. <br />
        /// The following placeholder apply to `UITable` components: <br />
        /// `{name}` : indicates the row name of the table. <br />
        /// `{index}` : indicates the row number of the table. <br />
        /// The following placeholder apply to `UIStatistc` components: <br />
        /// `{title}` : title text. <br />
        /// `{dd}` : day. <br />
        /// `{hh}` : hours. <br />
        /// `{mm}` : minutes. <br />
        /// `{ss}` : second. <br />
        /// `{fff}` : milliseconds. <br />
        /// `{d}` : day. <br />
        /// `{h}` : hours. <br />
        /// `{m}` : minutes. <br />
        /// `{s}` : second. <br />
        /// `{f}` : milliseconds. <br />
        /// Example :{b}:{c}<br />
        /// ||标签内容字符串模版格式器。支持用 \n 换行。部分组件的格式器会不生效。<br/>
        /// 模板通配符有以下这些，部分只适用于固定的组件：<br/>
        /// `{.}`：圆点标记。<br/>
        /// `{a}`：系列名。<br/>
        /// `{b}`：类目值或数据名。<br/>
        /// `{c}`：数据值。<br/>
        /// `{d}`：百分比。<br/>
        /// `{e}`：数据名。<br/>
        /// `{f}`：数据和。<br/>
        /// `{g}`：数据总个数。<br/>
        /// `{h}`：十六进制颜色值。<br/> 
        /// `{value}`：坐标轴或图例的值。<br/>
        /// 以下通配符适用UITable组件：<br/>
        /// `{name}`： 表格的行名。<br/>
        /// `{index}`：表格的行号。<br/>
        /// 以下通配符适用UIStatistc组件：<br/>
        /// `{title}`：标题文本。<br/>
        /// `{dd}`：天。<br/>
        /// `{hh}`：小时。<br/>
        /// `{mm}`：分钟。<br/>
        /// `{ss}`：秒。<br/>
        /// `{fff}`：毫秒。<br/>
        /// `{d}`：天。<br/>
        /// `{h}`：小时。<br/>
        /// `{m}`：分钟。<br/>
        /// `{s}`：秒。<br/>
        /// `{f}`：毫秒。<br/>
        /// 示例：“{b}:{c}”
        /// </summary>
        public string formatter
        {
            get { return m_Formatter; }
            set { if (PropertyUtil.SetClass(ref m_Formatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Standard number and date format string. Used to format a Double value or a DateTime date as a string. numericFormatter is used as an argument to either `Double.ToString ()` or `DateTime.ToString()`. <br />
        /// The number format uses the Axx format: A is a single-character format specifier that supports C currency, D decimal, E exponent, F fixed-point number, G regular, N digit, P percentage, R round trip, and X hexadecimal. xx is precision specification, from 0-99. E.g. F1, E2<br />
        /// Date format Common date formats are: yyyy year, MM month, dd day, HH hour, mm minute, ss second, fff millisecond. For example: yyyy-MM-dd HH:mm:ss<br />
        /// number format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/>
        /// date format reference: https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings<br/>
        /// ||标准数字和日期格式字符串。用于将Double数值或DateTime日期格式化显示为字符串。numericFormatter用来作为Double.ToString()或DateTime.ToString()的参数。<br/>
        /// 数字格式使用Axx的形式：A是格式说明符的单字符，支持C货币、D十进制、E指数、F定点数、G常规、N数字、P百分比、R往返、X十六进制的。xx是精度说明，从0-99。如：F1, E2<br/>
        /// 日期格式常见的格式：yyyy年，MM月，dd日，HH时，mm分，ss秒，fff毫秒。如：yyyy-MM-dd HH:mm:ss<br/>
        /// 数值格式化参考：https://docs.microsoft.com/zh-cn/dotnet/standard/base-types/standard-numeric-format-strings <br/>
        /// 日期格式化参考：https://learn.microsoft.com/zh-cn/dotnet/standard/base-types/standard-date-and-time-format-strings
        /// </summary>
        public string numericFormatter
        {
            get { return m_NumericFormatter; }
            set { if (PropertyUtil.SetClass(ref m_NumericFormatter, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// offset to the host graphic element.
        /// ||距离图形元素的偏移
        /// </summary>
        public Vector3 offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Rotation of label.
        /// ||文本的旋转。
        /// </summary>
        public float rotate
        {
            get { return m_Rotate; }
            set { if (PropertyUtil.SetStruct(ref m_Rotate, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// auto rotate of label.
        /// ||是否自动旋转。
        /// </summary>
        public bool autoRotate
        {
            get { return m_AutoRotate; }
            set { if (PropertyUtil.SetStruct(ref m_AutoRotate, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the distance of label to axis line.
        /// ||距离轴线的距离。
        /// </summary>
        public float distance
        {
            get { return m_Distance; }
            set { if (PropertyUtil.SetStruct(ref m_Distance, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the width of label. If set as default value 0, it means than the label width auto set as the text width.
        /// ||标签的宽度。一般不用指定，不指定时则自动是文字的宽度。
        /// </summary>
        public float width
        {
            get { return m_Width; }
            set { if (PropertyUtil.SetStruct(ref m_Width, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the height of label. If set as default value 0, it means than the label height auto set as the text height.
        /// ||标签的高度。一般不用指定，不指定时则自动是文字的高度。
        /// </summary>
        public float height
        {
            get { return m_Height; }
            set { if (PropertyUtil.SetStruct(ref m_Height, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the text padding of label. 
        /// ||文本的边距。
        /// </summary>
        public TextPadding textPadding
        {
            get { return m_TextPadding; }
            set { if (PropertyUtil.SetClass(ref m_TextPadding, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Whether to automatically offset. When turned on, the Y offset will automatically determine the opening of the curve to determine whether to offset up or down.
        /// ||是否开启自动偏移。当开启时，Y的偏移会自动判断曲线的开口来决定向上还是向下偏移。
        /// </summary>
        public bool autoOffset
        {
            get { return m_AutoOffset; }
            set { if (PropertyUtil.SetStruct(ref m_AutoOffset, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the sytle of background.
        /// ||背景图样式。
        /// </summary>
        public ImageStyle background
        {
            get { return m_Background; }
            set { if (PropertyUtil.SetClass(ref m_Background, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the sytle of icon.
        /// ||图标样式。
        /// </summary>
        public IconStyle icon
        {
            get { return m_Icon; }
            set { if (PropertyUtil.SetClass(ref m_Icon, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the sytle of text.
        /// ||文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the formatter function of label, which supports string template and callback function.
        /// ||标签的文本格式化函数，支持字符串模版和回调函数。
        /// </summary>
        public LabelFormatterFunction formatterFunction
        {
            get { return m_FormatterFunction; }
            set { m_FormatterFunction = value; }
        }
        /// <summary>
        /// whether the label is inside.
        /// ||是否在内部。
        /// </summary>
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

        public virtual string GetFormatterContent(int labelIndex, string category)
        {
            if (string.IsNullOrEmpty(category))
                return GetFormatterFunctionContent(labelIndex, category, category);

            if (string.IsNullOrEmpty(m_Formatter))
            {
                return GetFormatterFunctionContent(labelIndex, category, category);
            }
            else
            {
                var content = m_Formatter;
                FormatterHelper.ReplaceAxisLabelContent(ref content, category);
                return GetFormatterFunctionContent(labelIndex, category, category);
            }
        }

        public virtual string GetFormatterContent(int labelIndex, double value, double minValue, double maxValue, bool isLog = false)
        {
            var newNumericFormatter = numericFormatter;
            if (value == 0)
            {
                newNumericFormatter = "f0";
            }
            else if (string.IsNullOrEmpty(newNumericFormatter) && !isLog)
            {
                if (Math.Abs(maxValue) >= Math.Abs(minValue))
                {
                    newNumericFormatter = MathUtil.IsInteger(maxValue) ? "0.#" : "f" + MathUtil.GetPrecision(maxValue);
                }
                else
                {
                    newNumericFormatter = MathUtil.IsInteger(minValue) ? "0.#" : "f" + MathUtil.GetPrecision(minValue);
                }
            }
            if (string.IsNullOrEmpty(m_Formatter))
            {
                if (isLog)
                {
                    return GetFormatterFunctionContent(labelIndex, value, ChartCached.NumberToStr(value, newNumericFormatter));
                }
                if (minValue >= -1 && minValue <= 1 && maxValue >= -1 && maxValue <= 1)
                {
                    int minAcc = MathUtil.GetPrecision(minValue);
                    int maxAcc = MathUtil.GetPrecision(maxValue);
                    int curAcc = MathUtil.GetPrecision(value);
                    int acc = Mathf.Max(Mathf.Max(minAcc, maxAcc), curAcc);
                    return GetFormatterFunctionContent(labelIndex, value, ChartCached.FloatToStr(value, newNumericFormatter, acc));
                }
                return GetFormatterFunctionContent(labelIndex, value, ChartCached.NumberToStr(value, newNumericFormatter));
            }
            else
            {
                var content = m_Formatter;
                FormatterHelper.ReplaceAxisLabelContent(ref content, newNumericFormatter, value);
                return GetFormatterFunctionContent(labelIndex, value, content);
            }
        }

        public string GetFormatterDateTime(int labelIndex, double value, double minValue, double maxValue)
        {
            var timestamp = (int)value;
            var dateTime = DateTimeUtil.GetDateTime(timestamp);
            var dateString = string.Empty;
            if (string.IsNullOrEmpty(numericFormatter) || numericFormatter.Equals("f2"))
            {
                dateString = DateTimeUtil.GetDateTimeFormatString(dateTime, maxValue - minValue);
            }
            else
            {
                try
                {
                    dateString = dateTime.ToString(numericFormatter);
                }
                catch
                {
                    XLog.Warning("not support datetime formatter:" + numericFormatter);
                }
            }
            if (!string.IsNullOrEmpty(m_Formatter))
            {
                var content = m_Formatter;
                FormatterHelper.ReplaceAxisLabelContent(ref content, dateString);
                return GetFormatterFunctionContent(labelIndex, value, content);
            }
            else
            {
                return GetFormatterFunctionContent(labelIndex, value, dateString);
            }
        }

        protected string GetFormatterFunctionContent(int labelIndex, string category, string currentContent)
        {
            return m_FormatterFunction == null ? currentContent :
                m_FormatterFunction(labelIndex, labelIndex, category, currentContent);
        }

        protected string GetFormatterFunctionContent(int labelIndex, double value, string currentContent)
        {
            return m_FormatterFunction == null ? currentContent :
                m_FormatterFunction(labelIndex, value, null, currentContent);
        }
    }
}