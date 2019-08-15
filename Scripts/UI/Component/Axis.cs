using System.Net.Mime;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// The axis in rectangular coordinate.
    /// 直角坐标系的坐标轴组件。
    /// </summary>
    [System.Serializable]
    public class Axis : JsonDataSupport, IEquatable<Axis>
    {
        /// <summary>
        /// the type of axis.
        /// 坐标轴类型。
        /// </summary>
        public enum AxisType
        {
            /// <summary>
            /// Numerical axis, suitable for continuous data.
            /// 数值轴，适用于连续数据。
            /// </summary>
            Value,
            /// <summary>
            /// Category axis, suitable for discrete category data. Data should only be set via data for this type.
            /// 类目轴，适用于离散的类目数据，为该类型时必须通过 data 设置类目数据。
            /// </summary>
            Category
        }

        /// <summary>
        /// the type of axis min and max value.
        /// 坐标轴最大最小刻度显示类型。
        /// </summary>
        public enum AxisMinMaxType
        {
            /// <summary>
            /// 0 - maximum.
            /// 0-最大值。
            /// </summary>
            Default,
            /// <summary>
            /// minimum - maximum.
            /// 最小值-最大值。
            /// </summary>
            MinMax,
            /// <summary>
            /// Customize the minimum and maximum.
            /// 自定义最小值最大值。
            /// </summary>
            Custom
        }

        /// <summary>
        /// the type of split line. 
        /// 分割线类型
        /// </summary>
        public enum SplitLineType
        {
            /// <summary>
            /// 不显示分割线
            /// </summary>
            None,
            /// <summary>
            /// 实线
            /// </summary>
            Solid,
            /// <summary>
            /// 破折线
            /// </summary>
            Dashed,
            /// <summary>
            /// 虚线
            /// </summary>
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

        /// <summary>
        /// Set this to false to prevent the axis from showing.
        /// 是否显示坐标轴。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// the type of axis. 
        /// 坐标轴类型。
        /// </summary>
        public AxisType type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// the type of axis minmax.
        /// 坐标轴刻度最大最小值显示类型。
        /// </summary>
        public AxisMinMaxType minMaxType { get { return m_MinMaxType; } set { m_MinMaxType = value; } }
        /// <summary>
        /// The minimun value of axis.
        /// 设定的坐标轴刻度最小值，当minMaxType为Custom时有效。
        /// </summary>
        public int min { get { return m_Min; } set { m_Min = value; } }
        /// <summary>
        /// The maximum value of axis.
        /// 设定的坐标轴刻度最大值，当minMaxType为Custom时有效。
        /// </summary>
        public int max { get { return m_Max; } set { m_Max = value; } }
        /// <summary>
        /// Number of segments that the axis is split into.
        /// 坐标轴的分割段数。
        /// </summary>
        public int splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }
        /// <summary>
        /// showSplitLineSet this to false to prevent the splitLine from showing. value type axes are shown by default, while category type axes are hidden.
        /// 是否显示分隔线。默认数值轴显示，类目轴不显示。
        /// </summary>
        public bool showSplitLine { get { return m_ShowSplitLine; } set { m_ShowSplitLine = value; } }
        /// <summary>
        /// the type of split line. 
        /// 分割线类型。
        /// </summary>
        public SplitLineType splitLineType { get { return m_SplitLineType; } set { m_SplitLineType = value; } }
        /// <summary>
        /// The boundary gap on both sides of a coordinate axis. 
        /// 坐标轴两边是否留白。
        /// </summary>
        public bool boundaryGap { get { return m_BoundaryGap; } set { m_BoundaryGap = value; } }
        /// <summary>
        /// Category data, available in type: 'Category' axis.
        /// 类目数据，在类目轴（type: 'category'）中有效。
        /// </summary>
        public List<string> data { get { return m_Data; } }
        /// <summary>
        /// axis Line.
        /// 坐标轴轴线。
        /// </summary>
        public AxisLine axisLine { get { return m_AxisLine; } set { m_AxisLine = value; } }
        /// <summary>
        /// axis name.
        /// 坐标轴名称。
        /// </summary>
        public AxisName axisName { get { return m_AxisName; } set { m_AxisName = value; } }
        /// <summary>
        /// axis tick.
        /// 坐标轴刻度。
        /// </summary>
        public AxisTick axisTick { get { return m_AxisTick; } set { m_AxisTick = value; } }
        /// <summary>
        /// axis label.
        /// 坐标轴刻度标签。
        /// </summary>
        public AxisLabel axisLabel { get { return m_AxisLabel; } set { m_AxisLabel = value; } }
        /// <summary>
        /// axis split area.
        /// 坐标轴分割区域。
        /// </summary>
        public AxisSplitArea splitArea { get { return m_SplitArea; } set { m_SplitArea = value; } }
        /// <summary>
        /// the axis label text list. 
        /// 坐标轴刻度标签的Text列表。
        /// </summary>
        public List<Text> axisLabelTextList { get { return m_AxisLabelTextList; } set { m_AxisLabelTextList = value; } }
        /// <summary>
        /// the current minimun value.
        /// 当前最小值。
        /// </summary>
        public float minValue { get; set; }
        /// <summary>
        /// the current maximum value.
        /// 当前最大值。
        /// </summary>
        public float maxValue { get; set; }
        /// <summary>
        /// the x offset of zero position.
        /// 坐标轴原点在X轴的偏移。
        /// </summary>
        public float zeroXOffset { get; set; }
        /// <summary>
        /// the y offset of zero position.
        /// 坐标轴原点在Y轴的偏移。
        /// </summary>
        public float zeroYOffset { get; set; }

        private int filterStart;
        private int filterEnd;
        private List<string> filterData;
        private List<Text> m_AxisLabelTextList = new List<Text>();
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
            m_Data.Capacity = m_Data.Count;
            foreach (var d in other.data) m_Data.Add(d);
        }

        /// <summary>
        /// 清空类目数据
        /// </summary>
        public void ClearData()
        {
            m_Data.Clear();
        }

        /// <summary>
        /// 当前坐标轴是否时类目轴
        /// </summary>
        /// <returns></returns>
        public bool IsCategory()
        {
            return type == AxisType.Category;
        }

        /// <summary>
        /// 当前坐标轴是否时数值轴
        /// </summary>
        /// <returns></returns>
        public bool IsValue()
        {
            return type == AxisType.Value;
        }

        /// <summary>
        /// 添加一个类目到类目数据列表
        /// </summary>
        /// <param name="category"></param>
        /// <param name="maxDataNumber"></param>
        public void AddData(string category, int maxDataNumber)
        {
            if (maxDataNumber > 0)
            {
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_Data.Add(category);
        }

        /// <summary>
        /// 获得在dataZoom范围内指定索引的类目数据
        /// </summary>
        /// <param name="index">类目数据索引</param>
        /// <param name="dataZoom">区域缩放</param>
        /// <returns></returns>
        public string GetData(int index, DataZoom dataZoom)
        {
            var showData = GetDataList(dataZoom);
            if (index >= 0 && index < showData.Count)
                return showData[index];
            else
                return "";
        }

        /// <summary>
        /// 获得指定区域缩放的类目数据列表
        /// </summary>
        /// <param name="dataZoom">区域缩放</param>
        /// <returns></returns>
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

        /// <summary>
        /// 更新dataZoom对应的类目数据列表
        /// </summary>
        /// <param name="dataZoom"></param>
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

        /// <summary>
        /// 获得分割段数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public int GetSplitNumber(DataZoom dataZoom)
        {
            if (type == AxisType.Value) return m_SplitNumber;
            int dataCount = GetDataList(dataZoom).Count;
            if (dataCount > 2 * m_SplitNumber || dataCount <= 0)
                return m_SplitNumber;
            else
                return dataCount;
        }

        /// <summary>
        /// 获得分割段的宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public float GetSplitWidth(float coordinateWidth, DataZoom dataZoom)
        {
            return coordinateWidth / (m_BoundaryGap ? GetSplitNumber(dataZoom) : GetSplitNumber(dataZoom) - 1);
        }

        /// <summary>
        /// 获得类目数据个数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public int GetDataNumber(DataZoom dataZoom)
        {
            return GetDataList(dataZoom).Count;
        }

        /// <summary>
        /// 获得一个类目数据在坐标系中代表的宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public float GetDataWidth(float coordinateWidth, DataZoom dataZoom)
        {
            var dataCount = GetDataNumber(dataZoom);
            return coordinateWidth / (m_BoundaryGap ? dataCount : dataCount - 1);
        }

        private Dictionary<float, string> _cacheValue2str = new Dictionary<float, string>();
        /// <summary>
        /// 获得标签显示的名称
        /// </summary>
        /// <param name="index"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public string GetLabelName(int index, float minValue, float maxValue, DataZoom dataZoom)
        {
            if (m_Type == AxisType.Value)
            {
                float value = (minValue + (maxValue - minValue) * index / (GetSplitNumber(dataZoom) - 1));
                if (_cacheValue2str.ContainsKey(value)) return _cacheValue2str[value];
                else
                {
                    if (value - (int)value == 0)
                        _cacheValue2str[value] = (value).ToString();
                    else
                        _cacheValue2str[value] = (value).ToString("f1");
                    return _cacheValue2str[value];
                }
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

        /// <summary>
        /// 获得分割线条数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获得分割段宽度
        /// </summary>
        /// <param name="coordinateWidth"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public float GetScaleWidth(float coordinateWidth, DataZoom dataZoom)
        {
            int num = GetScaleNumber(dataZoom) - 1;
            if (num <= 0) num = 1;
            return coordinateWidth / num;
        }

        /// <summary>
        /// 更新刻度标签文字
        /// </summary>
        /// <param name="dataZoom"></param>
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

        /// <summary>
        /// 调整最大最小值
        /// </summary>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
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

    /// <summary>
    /// The x axis in cartesian(rectangular) coordinate. a grid component can place at most 2 x axis, 
    /// one on the bottom and another on the top.
    /// <para>直角坐标系 grid 中的 x 轴，单个 grid 组件最多只能放上下两个 x 轴。</para>
    /// </summary>
    [System.Serializable]
    public class XAxis : Axis
    {
        public XAxis Clone()
        {
            var axis = XAxisPool.Get();
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
            if (axis.data.Capacity < data.Count) axis.data.Capacity = data.Count;
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

    /// <summary>
    /// The x axis in cartesian(rectangular) coordinate. a grid component can place at most 2 x axis, 
    /// one on the bottom and another on the top.
    /// <para>直角坐标系 grid 中的 y 轴，单个 grid 组件最多只能放左右两个 y 轴</para>
    /// </summary>
    [System.Serializable]
    public class YAxis : Axis
    {
        public YAxis Clone()
        {
            var axis = YAxisPool.Get();
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
            if (axis.data.Capacity < data.Count) axis.data.Capacity = data.Count;
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
                    m_ShowSplitLine = true,
                    m_SplitLineType = SplitLineType.Dashed,
                    m_BoundaryGap = false,
                    m_Data = new List<string>(5),
                };
                return axis;
            }
        }
    }
}