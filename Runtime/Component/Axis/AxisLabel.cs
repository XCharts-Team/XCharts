using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// Settings related to axis label.
    /// |坐标轴刻度标签的相关设置。
    /// </summary>
    [Serializable]
    public class AxisLabel : LabelStyle
    {
        [SerializeField] private int m_Interval = 0;
        [SerializeField] private bool m_Inside = false;
        [SerializeField] private bool m_ShowAsPositiveNumber = false;
        [SerializeField] private bool m_OnZero = false;
        [SerializeField] private bool m_ShowStartLabel = true;
        [SerializeField] private bool m_ShowEndLabel = true;
        [SerializeField] private TextLimit m_TextLimit = new TextLimit();

        /// <summary>
        /// The display interval of the axis label.
        /// |坐标轴刻度标签的显示间隔，在类目轴中有效。0表示显示所有标签，1表示隔一个隔显示一个标签，以此类推。
        /// </summary>
        public int interval
        {
            get { return m_Interval; }
            set { if (PropertyUtil.SetStruct(ref m_Interval, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Set this to true so the axis labels face the inside direction.
        /// |刻度标签是否朝内，默认朝外。
        /// </summary>
        public bool inside
        {
            get { return m_Inside; }
            set { if (PropertyUtil.SetStruct(ref m_Inside, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Show negative number as positive number.
        /// |将负数数值显示为正数。一般和`Serie`的`showAsPositiveNumber`配合使用。
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
        /// Whether to display the first label.
        /// |是否显示第一个文本。
        /// </summary>
        public bool showStartLabel
        {
            get { return m_ShowStartLabel; }
            set { if (PropertyUtil.SetStruct(ref m_ShowStartLabel, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Whether to display the last label.
        /// |是否显示最后一个文本。
        /// </summary>
        public bool showEndLabel
        {
            get { return m_ShowEndLabel; }
            set { if (PropertyUtil.SetStruct(ref m_ShowEndLabel, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 文本限制。
        /// </summary>
        public TextLimit textLimit
        {
            get { return m_TextLimit; }
            set { if (value != null) { m_TextLimit = value; SetComponentDirty(); } }
        }

        public override bool componentDirty { get { return m_ComponentDirty || m_TextLimit.componentDirty; } }
        public override void ClearComponentDirty()
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
                    m_Distance = 8,
                    m_TextStyle = new TextStyle(),
                };
            }
        }

        public new AxisLabel Clone()
        {
            var axisLabel = new AxisLabel();
            axisLabel.show = show;
            axisLabel.formatter = formatter;
            axisLabel.interval = interval;
            axisLabel.inside = inside;
            axisLabel.distance = distance;
            axisLabel.numericFormatter = numericFormatter;
            axisLabel.width = width;
            axisLabel.height = height;
            axisLabel.showStartLabel = showStartLabel;
            axisLabel.showEndLabel = showEndLabel;
            axisLabel.textLimit = textLimit.Clone();
            axisLabel.textStyle.Copy(textStyle);
            return axisLabel;
        }

        public void Copy(AxisLabel axisLabel)
        {
            show = axisLabel.show;
            formatter = axisLabel.formatter;
            interval = axisLabel.interval;
            inside = axisLabel.inside;
            distance = axisLabel.distance;
            numericFormatter = axisLabel.numericFormatter;
            width = axisLabel.width;
            height = axisLabel.height;
            showStartLabel = axisLabel.showStartLabel;
            showEndLabel = axisLabel.showEndLabel;
            textLimit.Copy(axisLabel.textLimit);
            textStyle.Copy(axisLabel.textStyle);
        }

        public void SetRelatedText(ChartText txt, float labelWidth)
        {
            m_TextLimit.SetRelatedText(txt, labelWidth);
        }

        public override string GetFormatterContent(int labelIndex, string category)
        {
            if (string.IsNullOrEmpty(category))
                return GetFormatterFunctionContent(labelIndex, category, category);

            if (string.IsNullOrEmpty(m_Formatter))
            {
                return GetFormatterFunctionContent(labelIndex, category, m_TextLimit.GetLimitContent(category));
            }
            else
            {
                var content = m_Formatter;
                FormatterHelper.ReplaceAxisLabelContent(ref content, category);
                return GetFormatterFunctionContent(labelIndex, category, m_TextLimit.GetLimitContent(content));
            }
        }

        public override string GetFormatterContent(int labelIndex, double value, double minValue, double maxValue, bool isLog = false)
        {
            if (showAsPositiveNumber && value < 0)
            {
                value = Math.Abs(value);
            }
            return base.GetFormatterContent(labelIndex, value, minValue, maxValue, isLog);
        }

        public bool IsNeedShowLabel(int index, int total)
        {
            var labelShow = show && (interval == 0 || index % (interval + 1) == 0);
            if (labelShow)
            {
                if (!showStartLabel && index == 0) labelShow = false;
                else if (!showEndLabel && index == total - 1) labelShow = false;
            }
            return labelShow;
        }
    }
}