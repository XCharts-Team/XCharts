/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Text label of chart, to explain some data information about graphic item like value, name and so on.
    /// 图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
    /// </summary>
    [System.Serializable]
    public class LabelStyle : ChildComponent,ISerieDataComponent
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
            /// the bottom of symbol.
            /// 图形标志的底部。
            /// </summary>
            Bottom,
            /// <summary>
            /// the left of symbol.
            /// 图形标志的左边。
            /// </summary>
            Left,
            /// <summary>
            /// the right of symbol.
            /// 图形标志的右边。
            /// </summary>
            Right,
            /// <summary>
            /// the start of line.
            /// 线的起始点。
            /// </summary>
            Start,
            /// <summary>
            /// the middle of line.
            /// 线的中点。
            /// </summary>
            Middle,
            /// <summary>
            /// the end of line.
            /// 线的结束点。
            /// </summary>
            End
        }

        
        [SerializeField] private bool m_Show = false;
        [SerializeField] Position m_Position = Position.Outside;
        [SerializeField] private Vector3 m_Offset;
        [SerializeField] private float m_Margin;
        [SerializeField] private string m_Formatter;
        [SerializeField] private float m_PaddingLeftRight = 2f;
        [SerializeField] private float m_PaddingTopBottom = 2f;
        [SerializeField] private float m_BackgroundWidth = 0;
        [SerializeField] private float m_BackgroundHeight = 0;
        [SerializeField] private bool m_Border = false;
        [SerializeField] private float m_BorderWidth = 0.5f;
        [SerializeField] private Color32 m_BorderColor = ChartConst.greyColor32;
        [SerializeField] private string m_NumericFormatter = "";
        [SerializeField] private bool m_AutoOffset = false;
        [SerializeField] private TextStyle m_TextStyle = new TextStyle();
        private DelegateSerieLabelFormatter m_FormatterFunction;

        public void Reset()
        {
            m_Show = false;
            m_Position = Position.Outside;
            m_Offset = Vector3.zero;
            m_Margin = 0;
            m_PaddingLeftRight = 2f;
            m_PaddingTopBottom = 2f;
            m_BackgroundWidth = 0;
            m_BackgroundHeight = 0;
            m_Border = false;
            m_BorderWidth = 0.5f;
            m_BorderColor = Color.grey;
            m_NumericFormatter = "";
            m_AutoOffset = false;
        }

        /// <summary>
        /// Whether the label is showed.
        /// 是否显示文本标签。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The position of label.
        /// 标签的位置。
        /// </summary>
        public Position position
        {
            get { return m_Position; }
            set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 标签内容字符串模版格式器。支持用 \n 换行。
        /// 模板变量有：
        /// <list type="bullet">
        /// <item><description>{a}：系列名。</description></item>
        /// <item><description>{b}：数据名。</description></item>
        /// <item><description>{c}：数据值。</description></item>
        /// <item><description>{d}：百分比。</description></item>
        /// </list>
        /// </summary>
        /// <example>
        /// 示例：“{b}:{c}”
        /// </example>
        public string formatter
        {
            get { return m_Formatter; }
            set { if (PropertyUtil.SetClass(ref m_Formatter, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// offset to the host graphic element. 
        /// 距离图形元素的偏移
        /// </summary>
        public Vector3 offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 距离轴线的距离。
        /// </summary>
        public float margin
        {
            get { return m_Margin; }
            set { if (PropertyUtil.SetStruct(ref m_Margin, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of background. If set as default value 0, it means than the background width auto set as the text width.
        /// 标签的背景宽度。一般不用指定，不指定时则自动是文字的宽度。
        /// </summary>
        /// <value></value>
        public float backgroundWidth
        {
            get { return m_BackgroundWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BackgroundWidth, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the height of background. If set as default value 0, it means than the background height auto set as the text height.
        /// 标签的背景高度。一般不用指定，不指定时则自动是文字的高度。
        /// </summary>
        /// <value></value>
        public float backgroundHeight
        {
            get { return m_BackgroundHeight; }
            set { if (PropertyUtil.SetStruct(ref m_BackgroundHeight, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the text padding of left and right. defaut:2.
        /// 左右边距。
        /// </summary>
        public float paddingLeftRight
        {
            get { return m_PaddingLeftRight; }
            set { if (PropertyUtil.SetStruct(ref m_PaddingLeftRight, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the text padding of top and bottom. defaut:2.
        /// 上下边距。
        /// </summary>
        public float paddingTopBottom
        {
            get { return m_PaddingTopBottom; }
            set { if (PropertyUtil.SetStruct(ref m_PaddingTopBottom, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Whether to show border.
        /// 是否显示边框。
        /// </summary>
        public bool border
        {
            get { return m_Border; }
            set { if (PropertyUtil.SetStruct(ref m_Border, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the width of border.
        /// 边框宽度。
        /// </summary>
        public float borderWidth
        {
            get { return m_BorderWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BorderWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the color of border.
        /// 边框颜色。
        /// </summary>
        public Color32 borderColor
        {
            get { return m_BorderColor; }
            set { if (PropertyUtil.SetStruct(ref m_BorderColor, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Standard numeric format strings.
        /// 标准数字格式字符串。用于将数值格式化显示为字符串。
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
        /// the sytle of text.
        /// 文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value)) SetAllDirty(); }
        }

        public DelegateSerieLabelFormatter formatterFunction
        {
            get { return m_FormatterFunction; }
            set { m_FormatterFunction = value; }
        }

        public bool IsInside()
        {
            return position == Position.Inside || position == Position.Center;
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

        public TextAnchor GetAutoAlignment()
        {
            if (textStyle.autoAlign) return textStyle.alignment;
            else
            {
                switch (position)
                {
                    case LabelStyle.Position.Inside:
                    case LabelStyle.Position.Center:
                    case LabelStyle.Position.Top:
                    case LabelStyle.Position.Bottom:
                        return TextAnchor.MiddleCenter;
                    case LabelStyle.Position.Outside:
                    case LabelStyle.Position.Right:
                        return TextAnchor.MiddleLeft;
                    case LabelStyle.Position.Left:
                        return TextAnchor.MiddleRight;
                    default:
                        return TextAnchor.MiddleCenter;
                }
            }
        }
    }
}
