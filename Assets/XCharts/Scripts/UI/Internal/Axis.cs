using System.Net.Mime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    public class Axis : JsonDataSupport, IEquatable<Axis>
    {
        public enum AxisType
        {
            Value,
            Category,
            //Time,
            //Log
        }

        public enum AxisMinMaxType
        {
            Default,
            MinMax,
            Custom
        }

        public enum SplitLineType
        {
            None,
            Solid,
            Dashed,
            Dotted
        }

        [SerializeField] protected bool m_Show = true;
        [SerializeField] protected AxisType m_Type;
        [SerializeField] protected AxisMinMaxType m_MinMaxType;
        [SerializeField] protected int m_Min;
        [SerializeField] protected int m_Max;
        [SerializeField] protected int m_SplitNumber = 5;
        [SerializeField] protected bool m_ShowSplitLine = false;
        [SerializeField] protected SplitLineType m_SplitLineType = SplitLineType.Dashed;
        [SerializeField] protected bool m_BoundaryGap = true;
        [SerializeField] protected List<string> m_Data = new List<string>();
        [SerializeField] protected AxisLine m_AxisLine = AxisLine.defaultAxisLine;
        [SerializeField] protected AxisName m_AxisName = AxisName.defaultAxisName;
        [SerializeField] protected AxisTick m_AxisTick = AxisTick.defaultTick;
        [SerializeField] protected AxisLabel m_AxisLabel = AxisLabel.defaultAxisLabel;
        [SerializeField] protected AxisSplitArea m_SplitArea = AxisSplitArea.defaultSplitArea;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public AxisType type { get { return m_Type; } set { m_Type = value; } }
        public AxisMinMaxType minMaxType { get { return m_MinMaxType; } set { m_MinMaxType = value; } }
        public int min { get { return m_Min; } set { m_Min = value; } }
        public int max { get { return m_Max; } set { m_Max = value; } }
        public int splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }
        public bool showSplitLine { get { return m_ShowSplitLine; } set { m_ShowSplitLine = value; } }
        public SplitLineType splitLineType { get { return m_SplitLineType; } set { m_SplitLineType = value; } }
        public bool boundaryGap { get { return m_BoundaryGap; } set { m_BoundaryGap = value; } }
        public List<string> data { get { return m_Data; } }

        public AxisLine axisLine { get { return m_AxisLine; } set { m_AxisLine = value; } }
        public AxisName axisName { get { return m_AxisName; } set { m_AxisName = value; } }
        public AxisTick axisTick { get { return m_AxisTick; } set { m_AxisTick = value; } }
        public AxisLabel axisLabel { get { return m_AxisLabel; } set { m_AxisLabel = value; } }
        public AxisSplitArea splitArea { get { return m_SplitArea; } set { m_SplitArea = value; } }

        public int filterStart { get; set; }
        public int filterEnd { get; set; }
        public List<string> filterData { get; set; }

        public float minValue { get; set; }
        public float maxValue { get; set; }
        public float zeroXOffset { get; set; }
        public float zeroYOffset { get; set; }
        private List<Text> m_AxisLabelTextList = new List<Text>();
        public List<Text> axisLabelTextList { get { return m_AxisLabelTextList; } set { m_AxisLabelTextList = value; } }

        private GameObject m_TooltipLabel;
        private Text m_TooltipLabelText;
        private RectTransform m_TooltipLabelRect;

        public void Copy(Axis other)
        {
            m_Show = other.show;
            m_Type = other.type;
            m_Min = other.min;
            m_Max = other.max;
            m_SplitNumber = other.splitNumber;

            m_ShowSplitLine = other.showSplitLine;
            m_SplitLineType = other.splitLineType;
            m_BoundaryGap = other.boundaryGap;
            m_AxisName.Copy(other.axisName);
            m_AxisLabel.Copy(other.axisLabel);
            m_Data.Clear();
            foreach (var d in other.data) m_Data.Add(d);
        }

        public void ClearData()
        {
            m_Data.Clear();
        }

        public bool IsCategory()
        {
            return type == AxisType.Category;
        }

        public bool IsValue()
        {
            return type == AxisType.Value;
        }

        public void AddData(string category, int maxDataNumber)
        {
            if (maxDataNumber > 0)
            {
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_Data.Add(category);
        }

        public string GetData(int index, DataZoom dataZoom)
        {
            var showData = GetDataList(dataZoom);
            if (index >= 0 && index < showData.Count)
                return showData[index];
            else
                return "";
        }

        public List<string> GetDataList(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
                var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                if (filterData == null || filterData.Count != count)
                {
                    UpdateFilterData(dataZoom);
                }
                return filterData;
            }
            else
            {
                return m_Data;
            }
        }

        public void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
                if (startIndex != filterStart || endIndex != filterEnd)
                {
                    filterStart = startIndex;
                    filterEnd = endIndex;
                    if (m_Data.Count > 0)
                    {
                        var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                        filterData = m_Data.GetRange(startIndex, count);
                    }
                    else
                    {
                        filterData = m_Data;
                    }
                }
                else if (endIndex == 0)
                {
                    filterData = new List<string>();
                }
            }
        }

        public int GetSplitNumber(DataZoom dataZoom)
        {
            if (type == AxisType.Value) return m_SplitNumber;
            int dataCount = GetDataList(dataZoom).Count;
            if (dataCount > 2 * m_SplitNumber || dataCount <= 0)
                return m_SplitNumber;
            else
                return dataCount;
        }

        public float GetSplitWidth(float coordinateWidth, DataZoom dataZoom)
        {
            return coordinateWidth / (m_BoundaryGap ? GetSplitNumber(dataZoom) : GetSplitNumber(dataZoom) - 1);
        }

        public int GetDataNumber(DataZoom dataZoom)
        {
            return GetDataList(dataZoom).Count;
        }

        public float GetDataWidth(float coordinateWidth, DataZoom dataZoom)
        {
            var dataCount = GetDataNumber(dataZoom);
            return coordinateWidth / (m_BoundaryGap ? dataCount : dataCount - 1);
        }

        public string GetLabelName(int index, float minValue, float maxValue, DataZoom dataZoom)
        {
            if (m_Type == AxisType.Value)
            {
                float value = (minValue + (maxValue - minValue) * index / (GetSplitNumber(dataZoom) - 1));
                if (value - (int)value == 0)
                    return (value).ToString();
                else
                    return (value).ToString("f1");
            }
            var showData = GetDataList(dataZoom);
            int dataCount = showData.Count;
            if (dataCount <= 0) return "";

            if (index == GetSplitNumber(dataZoom) - 1 && !m_BoundaryGap)
            {
                return showData[dataCount - 1];
            }
            else
            {
                float rate = dataCount / GetSplitNumber(dataZoom);
                if (rate < 1) rate = 1;
                int offset = m_BoundaryGap ? (int)(rate / 2) : 0;
                int newIndex = (int)(index * rate >= dataCount - 1 ?
                    dataCount - 1 : offset + index * rate);
                return showData[newIndex];
            }
        }

        public int GetScaleNumber(DataZoom dataZoom)
        {
            if (type == AxisType.Value)
            {
                return m_BoundaryGap ? m_SplitNumber + 1 : m_SplitNumber;
            }
            else
            {
                var showData = GetDataList(dataZoom);
                int dataCount = showData.Count;
                if (dataCount > 2 * splitNumber || dataCount <= 0)
                    return m_BoundaryGap ? m_SplitNumber + 1 : m_SplitNumber;
                else
                    return m_BoundaryGap ? dataCount + 1 : dataCount;
            }
        }

        public float GetScaleWidth(float coordinateWidth, DataZoom dataZoom)
        {
            int num = GetScaleNumber(dataZoom) - 1;
            if (num <= 0) num = 1;
            return coordinateWidth / num;
        }

        public void UpdateLabelText(DataZoom dataZoom)
        {
            for (int i = 0; i < axisLabelTextList.Count; i++)
            {
                if (axisLabelTextList[i] != null)
                {
                    axisLabelTextList[i].text = GetLabelName(i, minValue, maxValue, dataZoom);
                }
            }
        }

        public void SetTooltipLabel(GameObject label)
        {
            m_TooltipLabel = label;
            m_TooltipLabelRect = label.GetComponent<RectTransform>();
            m_TooltipLabelText = label.GetComponentInChildren<Text>();
            m_TooltipLabel.SetActive(true);
        }

        public void SetTooltipLabelColor(Color bgColor, Color textColor)
        {
            m_TooltipLabel.GetComponent<Image>().color = bgColor;
            m_TooltipLabelText.color = textColor;
        }

        public void SetTooltipLabelActive(bool flag)
        {
            if (m_TooltipLabel && m_TooltipLabel.activeInHierarchy != flag)
            {
                m_TooltipLabel.SetActive(flag);
            }
        }

        public void UpdateTooptipLabelText(string text)
        {
            if (m_TooltipLabelText)
            {
                m_TooltipLabelText.text = text;
                m_TooltipLabelRect.sizeDelta = new Vector2(m_TooltipLabelText.preferredWidth + 8,
                    m_TooltipLabelText.preferredHeight + 8);
            }
        }

        public void UpdateTooltipLabelPos(Vector2 pos)
        {
            if (m_TooltipLabel)
            {
                m_TooltipLabel.transform.localPosition = pos;
            }
        }

        public void AdjustMinMaxValue(ref int minValue, ref int maxValue)
        {
            if (minMaxType == Axis.AxisMinMaxType.Custom)
            {
                if (min != 0 || max != 0)
                {
                    minValue = min;
                    maxValue = max;
                }
            }
            else
            {
                switch (minMaxType)
                {
                    case Axis.AxisMinMaxType.Default:
                        if (minValue > 0 && maxValue > 0)
                        {
                            minValue = 0;
                            maxValue = ChartHelper.GetMaxDivisibleValue(maxValue);
                        }
                        else if (minValue < 0 && maxValue < 0)
                        {
                            minValue = ChartHelper.GetMinDivisibleValue(minValue);
                            maxValue = 0;
                        }
                        else
                        {
                            minValue = ChartHelper.GetMinDivisibleValue(minValue);
                            maxValue = ChartHelper.GetMaxDivisibleValue(maxValue);
                        }
                        break;
                    case Axis.AxisMinMaxType.MinMax:
                        minValue = ChartHelper.GetMinDivisibleValue(minValue);
                        maxValue = ChartHelper.GetMaxDivisibleValue(maxValue);
                        break;
                }
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Axis)
            {
                return Equals((Axis)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Axis other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return show == other.show &&
                type == other.type &&
                min == other.min &&
                max == other.max &&
                splitNumber == other.splitNumber &&
                showSplitLine == other.showSplitLine &&
                m_AxisLabel.Equals(other.axisLabel) &&
                splitLineType == other.splitLineType &&
                boundaryGap == other.boundaryGap &&
                axisName.Equals(other.axisName) &&
                ChartHelper.IsValueEqualsList<string>(m_Data, other.data);
        }

        public static bool operator ==(Axis left, Axis right)
        {
            if (ReferenceEquals(left, null) && ReferenceEquals(right, null))
            {
                return true;
            }
            else if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return false;
            }
            return Equals(left, right);
        }

        public static bool operator !=(Axis left, Axis right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            m_Data = ChartHelper.ParseStringFromString(jsonData);
        }
    }

    [System.Serializable]
    public class XAxis : Axis
    {

        public XAxis Clone()
        {
            var axis = new XAxis();
            axis.show = show;
            axis.type = type;
            axis.min = min;
            axis.max = max;
            axis.splitNumber = splitNumber;

            axis.showSplitLine = showSplitLine;
            axis.splitLineType = splitLineType;
            axis.boundaryGap = boundaryGap;
            axis.axisName.Copy(axisName);
            axis.axisLabel.Copy(axisLabel);
            axis.data.Clear();
            foreach (var d in data) axis.data.Add(d);
            return axis;
        }

        public static XAxis defaultXAxis
        {
            get
            {
                var axis = new XAxis
                {
                    m_Show = true,
                    m_Type = AxisType.Category,
                    m_Min = 0,
                    m_Max = 0,
                    m_SplitNumber = 5,
                    m_ShowSplitLine = false,
                    m_SplitLineType = SplitLineType.Dashed,
                    m_BoundaryGap = true,
                    m_Data = new List<string>()
                    {
                        "x1","x2","x3","x4","x5"
                    }
                };
                return axis;
            }
        }
    }

    [System.Serializable]
    public class YAxis : Axis
    {
        public YAxis Clone()
        {
            var axis = new YAxis();
            axis.show = show;
            axis.type = type;
            axis.min = min;
            axis.max = max;
            axis.splitNumber = splitNumber;

            axis.showSplitLine = showSplitLine;
            axis.splitLineType = splitLineType;
            axis.boundaryGap = boundaryGap;
            axis.axisName.Copy(axisName);
            axis.axisLabel.Copy(axisLabel);
            axis.data.Clear();
            foreach (var d in data) axis.data.Add(d);
            return axis;
        }

        public static YAxis defaultYAxis
        {
            get
            {
                var axis = new YAxis
                {
                    m_Show = true,
                    m_Type = AxisType.Value,
                    m_Min = 0,
                    m_Max = 0,
                    m_SplitNumber = 5,
                    m_ShowSplitLine = false,
                    m_SplitLineType = SplitLineType.Dashed,
                    m_BoundaryGap = false,
                    m_Data = new List<string>(5),
                };
                return axis;
            }
        }
    }
}