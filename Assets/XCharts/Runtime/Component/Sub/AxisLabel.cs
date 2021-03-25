/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// Settings related to axis label.
    /// 坐标轴刻度标签的相关设置。
    /// </summary>
    [Serializable]
    public class AxisLabel : SubComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_Formatter;
        [SerializeField] private int m_Interval = 0;
        [SerializeField] private bool m_Inside = false;
        [SerializeField] private float m_Margin;
        [SerializeField] private string m_NumericFormatter = "";
        [SerializeField] private bool m_ShowAsPositiveNumber = false;
        [SerializeField] private bool m_OnZero = false;
        [SerializeField] private TextLimit m_TextLimit = new TextLimit();
        [SerializeField] private TextStyle m_TextStyle = new TextStyle();

        /// <summary>
        /// Set this to false to prevent the axis label from appearing.
        /// 是否显示刻度标签。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The display interval of the axis label.
        /// 坐标轴刻度标签的显示间隔，在类目轴中有效。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。
        /// </summary>
        public int interval
        {
            get { return m_Interval; }
            set { if (PropertyUtil.SetStruct(ref m_Interval, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Set this to true so the axis labels face the inside direction.
        /// 刻度标签是否朝内，默认朝外。
        /// </summary>
        public bool inside
        {
            get { return m_Inside; }
            set { if (PropertyUtil.SetStruct(ref m_Inside, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The margin between the axis label and the axis line.
        /// 刻度标签与轴线之间的距离。
        /// </summary>
        public float margin
        {
            get { return m_Margin; }
            set { if (PropertyUtil.SetStruct(ref m_Margin, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 图例内容字符串模版格式器。支持用 \n 换行。
        /// 模板变量为图例名称 {value}。
        /// </summary>
        public string formatter
        {
            get { return m_Formatter; }
            set { if (PropertyUtil.SetClass(ref m_Formatter, value)) SetComponentDirty(); }
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
        /// Show negative number as positive number.
        /// 将负数数值显示为正数。一般和`Serie`的`showAsPositiveNumber`配合使用。
        /// </summary>
        public bool showAsPositiveNumber
        {
            get { return m_ShowAsPositiveNumber; }
            set { if (PropertyUtil.SetStruct(ref m_ShowAsPositiveNumber, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// 刻度标签显示在0刻度上。
        /// </summary>
        public bool onZero
        {
            get { return m_OnZero; }
            set { if (PropertyUtil.SetStruct(ref m_OnZero, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// 文本限制。
        /// </summary>
        public TextLimit textLimit
        {
            get { return m_TextLimit; }
            set { if (value != null) { m_TextLimit = value; SetComponentDirty(); } }
        }

        /// <summary>
        /// The text style of axis name.
        /// 文本样式。
        /// </summary>
        public TextStyle textStyle
        {
            get { return m_TextStyle; }
            set { if (PropertyUtil.SetClass(ref m_TextStyle, value)) SetComponentDirty(); }
        }

        public override bool componentDirty { get { return m_ComponentDirty || m_TextLimit.componentDirty; } }
        internal override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            textLimit.ClearComponentDirty();
        }

        public static AxisLabel defaultAxisLabel
        {
            get
            {
                return new AxisLabel()
                {
                    m_Show = true,
                    m_Interval = 0,
                    m_Inside = false,
                    m_Margin = 8,
                    m_TextStyle = new TextStyle(),
                };
            }
        }

        public AxisLabel Clone()
        {
            var axisLable = new AxisLabel();
            axisLable.show = show;
            axisLable.formatter = formatter;
            axisLable.interval = interval;
            axisLable.inside = inside;
            axisLable.margin = margin;
            axisLable.numericFormatter = numericFormatter;
            axisLable.textLimit = textLimit.Clone();
            axisLable.textStyle.Copy(textStyle);
            return axisLable;
        }

        public void Copy(AxisLabel axisLable)
        {
            show = axisLable.show;
            formatter = axisLable.formatter;
            interval = axisLable.interval;
            inside = axisLable.inside;
            margin = axisLable.margin;
            numericFormatter = axisLable.numericFormatter;
            textLimit.Copy(axisLable.textLimit);
            textStyle.Copy(axisLable.textStyle);
        }

        public void SetRelatedText(ChartText txt, float labelWidth)
        {
            m_TextLimit.SetRelatedText(txt, labelWidth);
        }

        public string GetFormatterContent(string category)
        {
            if (string.IsNullOrEmpty(category)) return category;
            if (string.IsNullOrEmpty(m_Formatter))
            {
                return m_TextLimit.GetLimitContent(category);
            }
            else
            {
                var content = m_Formatter;
                FormatterHelper.ReplaceAxisLabelContent(ref content, category);
                return m_TextLimit.GetLimitContent(content);
            }
        }

        public string GetFormatterContent(float value, float minValue, float maxValue, bool isLog = false)
        {
            if (showAsPositiveNumber && value < 0)
            {
                value = Mathf.Abs(value);
            }
            if (string.IsNullOrEmpty(m_Formatter))
            {
                if (isLog)
                {
                    return ChartCached.NumberToStr(value, numericFormatter);
                }
                if (minValue >= -1 && minValue <= 1 && maxValue >= -1 && maxValue <= 1)
                {
                    int minAcc = ChartHelper.GetFloatAccuracy(minValue);
                    int maxAcc = ChartHelper.GetFloatAccuracy(maxValue);
                    int curAcc = ChartHelper.GetFloatAccuracy(value);
                    int acc = Mathf.Max(Mathf.Max(minAcc, maxAcc), curAcc);
                    return ChartCached.FloatToStr(value, numericFormatter, acc);
                }
                return ChartCached.NumberToStr(value, numericFormatter);
            }
            else
            {
                var content = m_Formatter;
                FormatterHelper.ReplaceAxisLabelContent(ref content, numericFormatter, value);
                return content;
            }
        }

        public string GetFormatterDateTime(DateTime dateTime)
        {
            var format = string.IsNullOrEmpty(numericFormatter) ? "yyyy/M/d" : numericFormatter;
            if (!string.IsNullOrEmpty(m_Formatter))
            {
                var content = m_Formatter;
                FormatterHelper.ReplaceAxisLabelContent(ref content, dateTime.ToString(format));
                return m_TextLimit.GetLimitContent(content);
            }
            else
            {
                var content = dateTime.ToString(format);
                return m_TextLimit.GetLimitContent(content);
            }
        }
    }
}