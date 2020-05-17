/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
    public class Axis : MainComponent
    {
        /// <summary>
        /// the type of axis.
        /// 坐标轴类型。
        /// </summary>
        public enum AxisType
        {
            /// <summary>
            /// Numerical axis, suitable for continuous data.
            /// 数值轴。适用于连续数据。
            /// </summary>
            Value,
            /// <summary>
            /// Category axis, suitable for discrete category data. Data should only be set via data for this type.
            /// 类目轴。适用于离散的类目数据，为该类型时必须通过 data 设置类目数据。
            /// </summary>
            Category,
            /// <summary>
            /// Log axis, suitable for log data.
            /// 对数轴。适用于对数数据。
            /// </summary>
            Log
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

        [SerializeField] protected bool m_Show = true;
        [SerializeField] protected AxisType m_Type;
        [SerializeField] protected AxisMinMaxType m_MinMaxType;
        [SerializeField] protected float m_Min;
        [SerializeField] protected float m_Max;
        [SerializeField] protected int m_SplitNumber = 5;
        [SerializeField] protected float m_Interval = 0;
        [SerializeField] protected bool m_BoundaryGap = true;
        [SerializeField] protected int m_MaxCache = 0;
        [SerializeField] protected float m_LogBase = 10;
        [SerializeField] protected bool m_LogBaseE = false;
        [SerializeField] protected int m_CeilRate = 0;
        [SerializeField] protected bool m_Inverse = false;
        [SerializeField] protected List<string> m_Data = new List<string>();
        [SerializeField] protected AxisLine m_AxisLine = AxisLine.defaultAxisLine;
        [SerializeField] protected AxisName m_AxisName = AxisName.defaultAxisName;
        [SerializeField] protected AxisTick m_AxisTick = AxisTick.defaultTick;
        [SerializeField] protected AxisLabel m_AxisLabel = AxisLabel.defaultAxisLabel;
        [SerializeField] protected AxisSplitLine m_SplitLine = AxisSplitLine.defaultSplitLine;
        [SerializeField] protected AxisSplitArea m_SplitArea = AxisSplitArea.defaultSplitArea;

        [NonSerialized] private float m_MinMaxValueRange;
        [NonSerialized] private bool m_NeedUpdateFilterData;

        /// <summary>
        /// Set this to false to prevent the axis from showing.
        /// 是否显示坐标轴。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the type of axis. 
        /// 坐标轴类型。
        /// </summary>
        public AxisType type
        {
            get { return m_Type; }
            set { if (PropertyUtility.SetStruct(ref m_Type, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the type of axis minmax.
        /// 坐标轴刻度最大最小值显示类型。
        /// </summary>
        public AxisMinMaxType minMaxType
        {
            get { return m_MinMaxType; }
            set { if (PropertyUtility.SetStruct(ref m_MinMaxType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The minimun value of axis.
        /// 设定的坐标轴刻度最小值，当minMaxType为Custom时有效。
        /// </summary>
        public float min
        {
            get { return m_Min; }
            set { if (PropertyUtility.SetStruct(ref m_Min, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The maximum value of axis.
        /// 设定的坐标轴刻度最大值，当minMaxType为Custom时有效。
        /// </summary>
        public float max
        {
            get { return m_Max; }
            set { if (PropertyUtility.SetStruct(ref m_Max, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Number of segments that the axis is split into.
        /// 坐标轴的分割段数。
        /// </summary>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtility.SetStruct(ref m_SplitNumber, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 强制设置坐标轴分割间隔。无法在类目轴中使用。
        /// Compulsively set segmentation interval for axis.This is unavailable for category axis.
        /// </summary>
        public float interval
        {
            get { return m_Interval; }
            set { if (PropertyUtility.SetStruct(ref m_Interval, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The boundary gap on both sides of a coordinate axis. 
        /// 坐标轴两边是否留白。
        /// </summary>
        public bool boundaryGap
        {
            get { return m_BoundaryGap; }
            set { if (PropertyUtility.SetStruct(ref m_BoundaryGap, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Base of logarithm, which is valid only for numeric axes with type: 'Log'.
        /// 对数轴的底数，只在对数轴（type:'Log'）中有效。
        /// </summary>
        public float logBase
        {
            get { return m_LogBase; }
            set { if (PropertyUtility.SetStruct(ref m_LogBase, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 对数轴是否以自然数 e 为底数，为 true 时 logBase 失效。
        /// </summary>
        public bool logBaseE
        {
            get { return m_LogBaseE; }
            set { if (PropertyUtility.SetStruct(ref m_LogBaseE, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The max number of axis data cache.
        /// The first data will be remove when the size of axis data is larger then maxCache.
        /// 可缓存的最大数据量。默认为0没有限制，大于0时超过指定值会移除旧数据再插入新数据。
        /// </summary>
        public int maxCache
        {
            get { return m_MaxCache; }
            set { if (PropertyUtility.SetStruct(ref m_MaxCache, value < 0 ? 0 : value)) SetAllDirty(); }
        }
        /// <summary>
        /// 最大最小值向上取整的倍率。默认为0时自动计算。
        /// </summary>
        public int ceilRate
        {
            get { return m_CeilRate; }
            set { if (PropertyUtility.SetStruct(ref m_CeilRate, value < 0 ? 0 : value)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether the axis are reversed or not. Invalid in `Category` axis.
        /// 是否反向坐标轴。在类目轴中无效。
        /// </summary>
        public bool inverse
        {
            get { return m_Inverse; }
            set { if (m_Type == AxisType.Value && PropertyUtility.SetStruct(ref m_Inverse, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Category data, available in type: 'Category' axis.
        /// 类目数据，在类目轴（type: 'category'）中有效。
        /// </summary>
        public List<string> data
        {
            get { return m_Data; }
            set { if (value != null) { m_Data = value; SetAllDirty(); } }
        }
        /// <summary>
        /// axis Line.
        /// 坐标轴轴线。
        /// /// </summary>
        public AxisLine axisLine
        {
            get { return m_AxisLine; }
            set { if (value != null) { m_AxisLine = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis name.
        /// 坐标轴名称。
        /// </summary>
        public AxisName axisName
        {
            get { return m_AxisName; }
            set { if (value != null) { m_AxisName = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// axis tick.
        /// 坐标轴刻度。
        /// </summary>
        public AxisTick axisTick
        {
            get { return m_AxisTick; }
            set { if (value != null) { m_AxisTick = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis label.
        /// 坐标轴刻度标签。
        /// </summary>
        public AxisLabel axisLabel
        {
            get { return m_AxisLabel; }
            set { if (value != null) { m_AxisLabel = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// axis split line.
        /// 坐标轴分割线。
        /// </summary>
        public AxisSplitLine splitLine
        {
            get { return m_SplitLine; }
            set { if (value != null) { m_SplitLine = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis split area.
        /// 坐标轴分割区域。
        /// </summary>
        public AxisSplitArea splitArea
        {
            get { return m_SplitArea; }
            set { if (value != null) { m_SplitArea = value; SetVerticesDirty(); } }
        }
        public override bool vertsDirty
        {
            get { return m_VertsDirty || axisLine.anyDirty || axisTick.anyDirty || splitLine.anyDirty || splitArea.anyDirty; }
        }
        public override bool componentDirty
        {
            get { return m_ComponentDirty || axisName.anyDirty || axisLabel.anyDirty; }
        }
        internal override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            axisName.ClearComponentDirty();
            axisLabel.ClearComponentDirty();
        }

        internal override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            axisLine.ClearVerticesDirty();
            axisTick.ClearVerticesDirty();
            splitLine.ClearVerticesDirty();
            splitArea.ClearVerticesDirty();
        }
        /// <summary>
        /// the axis label text list. 
        /// 坐标轴刻度标签的Text列表。
        /// </summary>
        public List<Text> axisLabelTextList { get { return m_AxisLabelTextList; } set { m_AxisLabelTextList = value; } }
        /// <summary>
        /// the current minimun value.
        /// 当前最小值。
        /// </summary>
        public float runtimeMinValue
        {
            get { return m_RuntimeMinValue; }
            internal set
            {
                m_RuntimeMinValue = value;
                m_RuntimeLastMinValue = value;
                m_RuntimeMinValueUpdateTime = Time.time;
                m_RuntimeMinValueChanged = true;
            }
        }
        /// <summary>
        /// the current maximum value.
        /// 当前最大值。
        /// </summary>
        public float runtimeMaxValue
        {
            get { return m_RuntimeMaxValue; }
            internal set
            {
                m_RuntimeMaxValue = value;
                m_RuntimeLastMaxValue = value;
                m_RuntimeMaxValueUpdateTime = Time.time;
                m_RuntimeMaxValueChanged = false;
            }
        }
        /// <summary>
        /// the x offset of zero position.
        /// 坐标轴原点在X轴的偏移。
        /// </summary>
        public float runtimeZeroXOffset { get; internal set; }
        /// <summary>
        /// the y offset of zero position.
        /// 坐标轴原点在Y轴的偏移。
        /// </summary>
        public float runtimeZeroYOffset { get; internal set; }
        public int runtimeMinLogIndex { get { return logBaseE ? (int)Mathf.Log(runtimeMinValue) : (int)Mathf.Log(runtimeMinValue, logBase); } }
        public int runtimeMaxLogIndex { get { return logBaseE ? (int)Mathf.Log(runtimeMaxValue) : (int)Mathf.Log(runtimeMaxValue, logBase); } }
        internal bool runtimeLastCheckInverse { get; set; }
        internal float runtimeMinMaxRange { get { return m_MinMaxValueRange; } set { m_MinMaxValueRange = value; } }
        private int filterStart;
        private int filterEnd;
        private int filterMinShow;
        private List<string> filterData;
        private List<Text> m_AxisLabelTextList = new List<Text>();
        private GameObject m_TooltipLabel;
        private Text m_TooltipLabelText;
        private RectTransform m_TooltipLabelRect;
        private float m_RuntimeMinValue;
        private float m_RuntimeLastMinValue;
        private bool m_RuntimeMinValueChanged;
        private float m_RuntimeMinValueUpdateTime;
        private float m_RuntimeMaxValue;
        private float m_RuntimeLastMaxValue;
        private bool m_RuntimeMaxValueChanged;
        private float m_RuntimeMaxValueUpdateTime;
        private bool m_RuntimeMinValueFirstChanged = true;
        private bool m_RuntimeMaxValueFirstChanged = true;

        public Axis Clone()
        {
            var axis = new Axis();
            axis.show = show;
            axis.type = type;
            axis.minMaxType = minMaxType;
            axis.min = min;
            axis.max = max;
            axis.splitNumber = splitNumber;
            axis.interval = interval;
            axis.boundaryGap = boundaryGap;
            axis.maxCache = maxCache;
            axis.logBase = logBase;
            axis.logBaseE = logBaseE;
            axis.ceilRate = ceilRate;
            axis.axisLine = axisLine.Clone();
            axis.axisName = axisName.Clone();
            axis.axisTick = axisTick.Clone();
            axis.axisLabel = axisLabel.Clone();
            axis.splitLine = splitLine.Clone();
            axis.splitArea = splitArea.Clone();
            axis.data = new List<string>();
            ChartHelper.CopyList(axis.data, data);
            return axis;
        }

        public void Copy(Axis axis)
        {
            show = axis.show;
            type = axis.type;
            minMaxType = axis.minMaxType;
            min = axis.min;
            max = axis.max;
            splitNumber = axis.splitNumber;
            interval = axis.interval;
            boundaryGap = axis.boundaryGap;
            maxCache = axis.maxCache;
            logBase = axis.logBase;
            logBaseE = axis.logBaseE;
            ceilRate = axis.ceilRate;
            axisLine.Copy(axis.axisLine);
            axisName.Copy(axis.axisName);
            axisTick.Copy(axis.axisTick);
            axisLabel.Copy(axis.axisLabel);
            splitLine.Copy(axis.splitLine);
            splitArea.Copy(axis.splitArea);
            ChartHelper.CopyList(data, axis.data);
        }

        /// <summary>
        /// 清空类目数据
        /// </summary>
        public void ClearData()
        {
            m_Data.Clear();
            SetAllDirty();
        }

        /// <summary>
        /// 是否为类目轴。
        /// </summary>
        /// <returns></returns>
        public bool IsCategory()
        {
            return type == AxisType.Category;
        }

        /// <summary>
        /// 是否为数值轴。
        /// </summary>
        /// <returns></returns>
        public bool IsValue()
        {
            return type == AxisType.Value;
        }

        /// <summary>
        /// 是否为对数轴。
        /// </summary>
        /// <returns></returns>
        public bool IsLog()
        {
            return type == AxisType.Log;
        }

        /// <summary>
        /// 添加一个类目到类目数据列表
        /// </summary>
        /// <param name="category"></param>
        public void AddData(string category)
        {
            if (maxCache > 0)
            {
                while (m_Data.Count > maxCache)
                {
                    m_NeedUpdateFilterData = true;
                    m_Data.RemoveAt(0);
                }
            }
            m_Data.Add(category);
            SetAllDirty();
        }

        /// <summary>
        /// 获得指定索引的类目数据
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetData(int index)
        {
            if (index >= 0 && index < m_Data.Count)
                return m_Data[index];
            else
                return null;
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
        internal List<string> GetDataList(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.enable)
            {
                UpdateFilterData(dataZoom);
                return filterData;
            }
            else
            {
                return m_Data;
            }
        }

        private List<string> emptyFliter = new List<string>();
        /// <summary>
        /// 更新dataZoom对应的类目数据列表
        /// </summary>
        /// <param name="dataZoom"></param>
        internal void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.enable)
            {
                var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
                if (endIndex < startIndex) endIndex = startIndex;
                if (startIndex != filterStart || endIndex != filterEnd || dataZoom.minShowNum != filterMinShow || m_NeedUpdateFilterData)
                {
                    filterStart = startIndex;
                    filterEnd = endIndex;
                    filterMinShow = dataZoom.minShowNum;
                    m_NeedUpdateFilterData = false;
                    if (m_Data.Count > 0)
                    {
                        var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                        if (count < dataZoom.minShowNum)
                        {
                            if (dataZoom.minShowNum > m_Data.Count) count = m_Data.Count;
                            else count = dataZoom.minShowNum;
                        }
                        if (startIndex + count > m_Data.Count)
                        {
                            int start = endIndex - count;
                            filterData = m_Data.GetRange(start < 0 ? 0 : start, count);
                        }
                        else filterData = m_Data.GetRange(startIndex, count);
                    }
                    else
                    {
                        filterData = m_Data;
                    }
                }
                else if (endIndex == 0)
                {
                    filterData = emptyFliter;
                }
            }
        }

        /// <summary>
        /// 获得类目数据个数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        internal int GetDataNumber(DataZoom dataZoom)
        {
            return GetDataList(dataZoom).Count;
        }

        /// <summary>
        /// 更新刻度标签文字
        /// </summary>
        /// <param name="dataZoom"></param>
        internal void UpdateLabelText(float coordinateWidth, DataZoom dataZoom, bool forcePercent, float duration)
        {
            var minValue = GetCurrMinValue(duration);
            var maxValue = GetCurrMaxValue(duration);
            for (int i = 0; i < axisLabelTextList.Count; i++)
            {
                if (axisLabelTextList[i] != null)
                {
                    var text = AxisHelper.GetLabelName(this, coordinateWidth, i, minValue, maxValue, dataZoom, forcePercent);
                    axisLabelTextList[i].text = text;
                }
            }
        }

        internal void SetTooltipLabel(GameObject label)
        {
            m_TooltipLabel = label;
            m_TooltipLabelRect = label.GetComponent<RectTransform>();
            m_TooltipLabelText = label.GetComponentInChildren<Text>();
            ChartHelper.SetActive(m_TooltipLabel, true);
        }

        internal void SetTooltipLabelColor(Color bgColor, Color textColor)
        {
            m_TooltipLabel.GetComponent<Image>().color = bgColor;
            m_TooltipLabelText.color = textColor;
        }

        internal void SetTooltipLabelActive(bool flag)
        {
            if (m_TooltipLabel && m_TooltipLabel.activeInHierarchy != flag)
            {
                ChartHelper.SetActive(m_TooltipLabel, flag);
            }
        }

        internal void UpdateTooptipLabelText(string text)
        {
            if (m_TooltipLabelText)
            {
                m_TooltipLabelText.text = text;
                m_TooltipLabelRect.sizeDelta = new Vector2(m_TooltipLabelText.preferredWidth + 8,
                    m_TooltipLabelText.preferredHeight + 8);
            }
        }

        internal void UpdateTooltipLabelPos(Vector2 pos)
        {
            if (m_TooltipLabel)
            {
                m_TooltipLabel.transform.localPosition = pos;
            }
        }

        internal void UpdateMinValue(float value, bool check)
        {
            if (value != m_RuntimeMaxValue)
            {
                if (check && Application.isPlaying)
                {
                    if (m_RuntimeMinValueFirstChanged)
                    {
                        m_RuntimeMinValueFirstChanged = false;
                    }
                    else
                    {
                        m_RuntimeLastMinValue = m_RuntimeMinValue;
                        m_RuntimeMinValueChanged = true;
                        m_RuntimeMinValueUpdateTime = Time.time;
                    }
                    m_RuntimeMinValue = value;
                }
                else
                {
                    m_RuntimeMinValue = value;
                    m_RuntimeLastMinValue = value;
                    m_RuntimeMinValueUpdateTime = Time.time;
                    m_RuntimeMinValueChanged = true;
                }
            }
        }

        internal void UpdateMaxValue(float value, bool check)
        {
            if (value != m_RuntimeMaxValue)
            {
                if (check && Application.isPlaying)
                {
                    if (m_RuntimeMaxValueFirstChanged)
                    {
                        m_RuntimeMaxValueFirstChanged = false;
                    }
                    else
                    {
                        m_RuntimeLastMaxValue = m_RuntimeMaxValue;
                        m_RuntimeMaxValueChanged = true;
                        m_RuntimeMaxValueUpdateTime = Time.time;
                    }
                    m_RuntimeMaxValue = value;
                }
                else
                {
                    m_RuntimeMaxValue = value;
                    m_RuntimeLastMaxValue = value;
                    m_RuntimeMaxValueUpdateTime = Time.time;
                    m_RuntimeMaxValueChanged = false;
                }
            }
        }

        internal float GetCurrMinValue(float duration)
        {
            if (!Application.isPlaying) return m_RuntimeMinValue;
            if (m_RuntimeMinValue == 0 && m_RuntimeMaxValue == 0) return 0;
            if (!m_RuntimeMinValueChanged) return m_RuntimeMinValue;
            var time = Time.time - m_RuntimeMinValueUpdateTime;
            if (time == 0) return m_RuntimeMinValue;
            var total = duration / 1000;
            if (duration > 0 && time <= total)
            {
                var curr = Mathf.Lerp(m_RuntimeLastMinValue, m_RuntimeMinValue, time / total);
                return curr;
            }
            else
            {
                m_RuntimeMinValueChanged = false;
                return m_RuntimeMinValue;
            }
        }

        internal float GetCurrMaxValue(float duration)
        {
            if (!Application.isPlaying) return m_RuntimeMaxValue;
            if (m_RuntimeMinValue == 0 && m_RuntimeMaxValue == 0) return 0;
            if (!m_RuntimeMaxValueChanged) return m_RuntimeMaxValue;
            var time = Time.time - m_RuntimeMaxValueUpdateTime;
            if (time == 0) return m_RuntimeMaxValue;
            var total = duration / 1000;
            if (duration > 0 && time < total)
            {
                var curr = Mathf.Lerp(m_RuntimeLastMaxValue, m_RuntimeMaxValue, time / total);
                return curr;
            }
            else
            {
                m_RuntimeMaxValueChanged = false;
                return m_RuntimeMaxValue;
            }
        }

        public bool IsValueChanging(float duration)
        {
            if (!Application.isPlaying) return false;
            if (GetCurrMinValue(duration) != m_RuntimeMinValue || GetCurrMaxValue(duration) != m_RuntimeMaxValue)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float GetLogValue(float value)
        {
            if (value <= 0) return 0;
            return logBaseE ? Mathf.Log(value) : Mathf.Log(value, logBase);
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
                    m_BoundaryGap = true,
                    m_Data = new List<string>()
                    {
                        "x1","x2","x3","x4","x5"
                    }
                };
                axis.splitLine.show = false;
                axis.splitLine.lineStyle.type = LineStyle.Type.Dashed;
                axis.axisLabel.textLimit.enable = true;
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
                    m_BoundaryGap = false,
                    m_Data = new List<string>(5),
                };
                axis.splitLine.show = true;
                axis.splitLine.lineStyle.type = LineStyle.Type.Dashed;
                axis.axisLabel.textLimit.enable = false;
                return axis;
            }
        }
    }
}