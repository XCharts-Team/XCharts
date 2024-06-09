using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// The axis in rectangular coordinate.
    /// ||直角坐标系的坐标轴组件。
    /// </summary>
    [System.Serializable]
    public class Axis : MainComponent
    {
        /// <summary>
        /// the type of axis.
        /// ||坐标轴类型。
        /// </summary>
        public enum AxisType
        {
            /// <summary>
            /// Numerical axis, suitable for continuous data.
            /// ||数值轴。适用于连续数据。
            /// </summary>
            Value,
            /// <summary>
            /// Category axis, suitable for discrete category data. Data should only be set via data for this type.
            /// ||类目轴。适用于离散的类目数据，为该类型时必须通过 data 设置类目数据。serie的数据第0维数据对应坐标轴data的index。
            /// </summary>
            Category,
            /// <summary>
            /// Log axis, suitable for log data.
            /// ||对数轴。适用于对数数据。
            /// </summary>
            Log,
            /// <summary>
            /// Time axis, suitable for continuous time series data.
            /// ||时间轴。适用于连续的时序数据。
            /// </summary>
            Time
        }

        /// <summary>
        /// the type of axis min and max value.
        /// ||坐标轴最大最小刻度显示类型。
        /// </summary>
        public enum AxisMinMaxType
        {
            /// <summary>
            /// 0 - maximum.
            /// ||0-最大值。
            /// </summary>
            Default,
            /// <summary>
            /// minimum - maximum.
            /// ||最小值-最大值。
            /// </summary>
            MinMax,
            /// <summary>
            /// Customize the minimum and maximum.
            /// ||自定义最小值最大值。
            /// </summary>
            Custom,
            /// <summary>
            /// [since("v3.7.0")]minimum - maximum, automatically calculate the appropriate values.
            /// ||[since("v3.7.0")]最小值-最大值。自动计算合适的值。
            /// </summary>
            MinMaxAuto,
        }
        /// <summary>
        /// the position of axis in grid.
        /// ||坐标轴在Grid中的位置
        /// </summary>
        public enum AxisPosition
        {
            Left,
            Right,
            Bottom,
            Top,
            Center
        }

        [SerializeField] protected bool m_Show = true;
        [SerializeField] protected Axis.AxisType m_Type;
        [SerializeField] protected Axis.AxisMinMaxType m_MinMaxType;
        [SerializeField] protected int m_GridIndex;
        [SerializeField] protected int m_PolarIndex;
        [SerializeField] protected int m_ParallelIndex;
        [SerializeField] protected Axis.AxisPosition m_Position;
        [SerializeField] protected float m_Offset;
        [SerializeField] protected double m_Min;
        [SerializeField] protected double m_Max;
        [SerializeField] protected int m_SplitNumber = 0;
        [SerializeField] protected double m_Interval = 0;
        [SerializeField] protected bool m_BoundaryGap = true;
        [SerializeField] protected int m_MaxCache = 0;
        [SerializeField] protected float m_LogBase = 10;
        [SerializeField] protected bool m_LogBaseE = false;
        [SerializeField] protected double m_CeilRate = 0;
        [SerializeField] protected bool m_Inverse = false;
        [SerializeField] private bool m_Clockwise = true;
        [SerializeField] private bool m_InsertDataToHead;
        [SerializeField][Since("v3.11.0")] private float m_MinCategorySpacing = 0;
        [SerializeField] protected List<Sprite> m_Icons = new List<Sprite>();
        [SerializeField] protected List<string> m_Data = new List<string>();
        [SerializeField] protected AxisLine m_AxisLine = AxisLine.defaultAxisLine;
        [SerializeField] protected AxisName m_AxisName = AxisName.defaultAxisName;
        [SerializeField] protected AxisTick m_AxisTick = AxisTick.defaultTick;
        [SerializeField] protected AxisLabel m_AxisLabel = AxisLabel.defaultAxisLabel;
        [SerializeField] protected AxisSplitLine m_SplitLine = AxisSplitLine.defaultSplitLine;
        [SerializeField] protected AxisSplitArea m_SplitArea = AxisSplitArea.defaultSplitArea;
        [SerializeField] protected AxisAnimation m_Animation = new AxisAnimation();
        [SerializeField][Since("v3.2.0")] protected AxisMinorTick m_MinorTick = AxisMinorTick.defaultMinorTick;
        [SerializeField][Since("v3.2.0")] protected AxisMinorSplitLine m_MinorSplitLine = AxisMinorSplitLine.defaultMinorSplitLine;
        [SerializeField][Since("v3.4.0")] protected LabelStyle m_IndicatorLabel = new LabelStyle() { numericFormatter = "f2" };

        public AxisContext context = new AxisContext();

        /// <summary>
        /// Whether to show axis.
        /// ||是否显示坐标轴。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the type of axis.
        /// ||坐标轴类型。
        /// </summary>
        public AxisType type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the type of axis minmax.
        /// ||坐标轴刻度最大最小值显示类型。
        /// </summary>
        public AxisMinMaxType minMaxType
        {
            get { return m_MinMaxType; }
            set { if (PropertyUtil.SetStruct(ref m_MinMaxType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The index of the grid on which the axis are located, by default, is in the first grid.
        /// ||坐标轴所在的 grid 的索引，默认位于第一个 grid。
        /// </summary>
        public int gridIndex
        {
            get { return m_GridIndex; }
            set { if (PropertyUtil.SetStruct(ref m_GridIndex, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The index of the polar on which the axis are located, by default, is in the first polar.
        /// ||坐标轴所在的 ploar 的索引，默认位于第一个 polar。
        /// </summary>
        public int polarIndex
        {
            get { return m_PolarIndex; }
            set { if (PropertyUtil.SetStruct(ref m_PolarIndex, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The index of the parallel on which the axis are located, by default, is in the first parallel.
        /// ||坐标轴所在的 parallel 的索引，默认位于第一个 parallel。
        /// </summary>
        public int parallelIndex
        {
            get { return m_ParallelIndex; }
            set { if (PropertyUtil.SetStruct(ref m_ParallelIndex, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the position of axis in grid.
        /// ||坐标轴在Grid中的位置。
        /// </summary>
        public AxisPosition position
        {
            get { return m_Position; }
            set { if (PropertyUtil.SetStruct(ref m_Position, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the offset of axis from the default position. Useful when the same position has multiple axes.
        /// ||坐标轴相对默认位置的偏移。在相同position有多个坐标轴时有用。
        /// </summary>
        public float offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The minimun value of axis.Valid when `minMaxType` is `Custom`
        /// ||设定的坐标轴刻度最小值，当minMaxType为Custom时有效。
        /// </summary>
        public double min
        {
            get { return m_Min; }
            set { if (PropertyUtil.SetStruct(ref m_Min, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The maximum value of axis.Valid when `minMaxType` is `Custom`
        /// ||设定的坐标轴刻度最大值，当minMaxType为Custom时有效。
        /// </summary>
        public double max
        {
            get { return m_Max; }
            set { if (PropertyUtil.SetStruct(ref m_Max, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Number of segments that the axis is split into.
        /// ||坐标轴的期望的分割段数。默认为0表示自动分割。 
        /// </summary>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtil.SetStruct(ref m_SplitNumber, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Compulsively set segmentation interval for axis.This is unavailable for category axis.
        /// ||强制设置坐标轴分割间隔。无法在类目轴中使用。
        /// </summary>
        public double interval
        {
            get { return m_Interval; }
            set { if (PropertyUtil.SetStruct(ref m_Interval, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The boundary gap on both sides of a coordinate axis, which is valid only for category axis with type: 'Category'.
        /// ||坐标轴两边是否留白。只对类目轴有效。
        /// </summary>
        public bool boundaryGap
        {
            get { return IsCategory() ? m_BoundaryGap : false; }
            set { if (PropertyUtil.SetStruct(ref m_BoundaryGap, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Base of logarithm, which is valid only for numeric axes with type: 'Log'.
        /// ||对数轴的底数，只在对数轴（type:'Log'）中有效。
        /// </summary>
        public float logBase
        {
            get { return m_LogBase; }
            set
            {
                if (value <= 0 || value == 1) value = 10;
                if (PropertyUtil.SetStruct(ref m_LogBase, value)) SetAllDirty();
            }
        }
        /// <summary>
        /// On the log axis, if base e is the natural number, and is true, logBase fails.
        /// ||对数轴是否以自然数 e 为底数，为 true 时 logBase 失效。
        /// </summary>
        public bool logBaseE
        {
            get { return m_LogBaseE; }
            set { if (PropertyUtil.SetStruct(ref m_LogBaseE, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The max number of axis data cache.
        /// ||The first data will be remove when the size of axis data is larger then maxCache.
        /// ||可缓存的最大数据量。默认为0没有限制，大于0时超过指定值会移除旧数据再插入新数据。
        /// </summary>
        public int maxCache
        {
            get { return m_MaxCache; }
            set { if (PropertyUtil.SetStruct(ref m_MaxCache, value < 0 ? 0 : value)) SetAllDirty(); }
        }
        /// <summary>
        /// The ratio of maximum and minimum values rounded upward. The default is 0, which is automatically calculated.
        /// ||最大最小值向上取整的倍率。默认为0时自动计算。
        /// </summary>
        public double ceilRate
        {
            get { return m_CeilRate; }
            set { if (PropertyUtil.SetStruct(ref m_CeilRate, value < 0 ? 0 : value)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether the axis are reversed or not. Invalid in `Category` axis.
        /// ||是否反向坐标轴。在类目轴中无效。
        /// </summary>
        public bool inverse
        {
            get { return m_Inverse; }
            set { if (m_Type == AxisType.Value && PropertyUtil.SetStruct(ref m_Inverse, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether the positive position of axis is in clockwise. True for clockwise by default.
        /// ||刻度增长是否按顺时针，默认顺时针。
        /// </summary>
        public bool clockwise
        {
            get { return m_Clockwise; }
            set { if (PropertyUtil.SetStruct(ref m_Clockwise, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Category data, available in type: 'Category' axis.
        /// ||类目数据，在类目轴（type: 'category'）中有效。
        /// </summary>
        public List<string> data
        {
            get { return m_Data; }
            set { if (value != null) { m_Data = value; SetAllDirty(); } }
        }
        /// <summary>
        /// 类目数据对应的图标。
        /// </summary>
        public List<Sprite> icons
        {
            get { return m_Icons; }
            set { if (value != null) { m_Icons = value; SetAllDirty(); } }
        }
        /// <summary>
        /// axis Line.
        /// ||坐标轴轴线。
        /// </summary>
        public AxisLine axisLine
        {
            get { return m_AxisLine; }
            set { if (value != null) { m_AxisLine = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis name.
        /// ||坐标轴名称。
        /// </summary>
        public AxisName axisName
        {
            get { return m_AxisName; }
            set { if (value != null) { m_AxisName = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// axis tick.
        /// ||坐标轴刻度。
        /// </summary>
        public AxisTick axisTick
        {
            get { return m_AxisTick; }
            set { if (value != null) { m_AxisTick = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis label.
        /// ||坐标轴刻度标签。
        /// </summary>
        public AxisLabel axisLabel
        {
            get { return m_AxisLabel; }
            set { if (value != null) { m_AxisLabel = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// axis split line.
        /// ||坐标轴分割线。
        /// </summary>
        public AxisSplitLine splitLine
        {
            get { return m_SplitLine; }
            set { if (value != null) { m_SplitLine = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis split area.
        /// ||坐标轴分割区域。
        /// </summary>
        public AxisSplitArea splitArea
        {
            get { return m_SplitArea; }
            set { if (value != null) { m_SplitArea = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis minor tick.
        /// ||坐标轴次刻度。
        /// </summary>
        public AxisMinorTick minorTick
        {
            get { return m_MinorTick; }
            set { if (value != null) { m_MinorTick = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// axis minor split line.
        /// ||坐标轴次分割线。
        /// </summary>
        public AxisMinorSplitLine minorSplitLine
        {
            get { return m_MinorSplitLine; }
            set { if (value != null) { m_MinorSplitLine = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// Style of axis tooltip indicator label. 
        /// ||指示器文本的样式。Tooltip为Cross时使用。
        /// </summary>
        public LabelStyle indicatorLabel
        {
            get { return m_IndicatorLabel; }
            set { if (value != null) { m_IndicatorLabel = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// animation of axis.
        /// ||坐标轴动画。
        /// </summary>
        public AxisAnimation animation
        {
            get { return m_Animation; }
            set { if (value != null) { m_Animation = value; SetComponentDirty(); } }
        }
        /// <summary>
        /// Whether to add new data at the head or at the end of the list.
        /// ||添加新数据时是在列表的头部还是尾部加入。
        /// </summary>
        public bool insertDataToHead
        {
            get { return m_InsertDataToHead; }
            set { if (PropertyUtil.SetStruct(ref m_InsertDataToHead, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The minimum spacing between categories.
        /// ||类目之间的最小间距。
        /// </summary>
        public float minCategorySpacing
        {
            get { return m_MinCategorySpacing; }
            set { if (PropertyUtil.SetStruct(ref m_MinCategorySpacing, value)) SetAllDirty(); }
        }

        public override bool vertsDirty
        {
            get
            {
                return m_VertsDirty ||
                    axisLine.anyDirty ||
                    axisTick.anyDirty ||
                    splitLine.anyDirty ||
                    splitArea.anyDirty ||
                    minorTick.anyDirty ||
                    minorSplitLine.anyDirty;
            }
        }

        public override bool componentDirty
        {
            get
            {
                return m_ComponentDirty ||
                    axisName.anyDirty ||
                    axisLabel.anyDirty ||
                    indicatorLabel.anyDirty;
            }
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            axisName.ClearComponentDirty();
            axisLabel.ClearComponentDirty();
            indicatorLabel.ClearComponentDirty();
        }

        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            axisLabel.ClearVerticesDirty();
            axisLine.ClearVerticesDirty();
            axisTick.ClearVerticesDirty();
            splitLine.ClearVerticesDirty();
            splitArea.ClearVerticesDirty();
            minorTick.ClearVerticesDirty();
            minorSplitLine.ClearVerticesDirty();
            indicatorLabel.ClearVerticesDirty();
        }

        public override void SetComponentDirty()
        {
            context.isNeedUpdateFilterData = true;
            base.SetComponentDirty();
        }

        /// <summary>
        /// 重置状态。
        /// </summary>
        public override void ResetStatus()
        {
            context.minValue = 0;
            context.maxValue = 0;
            context.destMinValue = 0;
            context.destMaxValue = 0;
        }

        public Axis Clone()
        {
            var axis = new Axis();
            axis.show = show;
            axis.type = type;
            axis.gridIndex = 0;
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
            axis.insertDataToHead = insertDataToHead;
            axis.axisLine = axisLine.Clone();
            axis.axisName = axisName.Clone();
            axis.axisTick = axisTick.Clone();
            axis.axisLabel = axisLabel.Clone();
            axis.splitLine = splitLine.Clone();
            axis.splitArea = splitArea.Clone();
            axis.minorTick = minorTick.Clone();
            axis.minorSplitLine = minorSplitLine.Clone();
            axis.indicatorLabel = indicatorLabel.Clone();
            axis.animation = animation.Clone();
            axis.icons = new List<Sprite>();
            axis.data = new List<string>();
            ChartHelper.CopyList(axis.data, data);
            return axis;
        }

        public void Copy(Axis axis)
        {
            show = axis.show;
            type = axis.type;
            minMaxType = axis.minMaxType;
            gridIndex = axis.gridIndex;
            min = axis.min;
            max = axis.max;
            splitNumber = axis.splitNumber;
            interval = axis.interval;
            boundaryGap = axis.boundaryGap;
            maxCache = axis.maxCache;
            logBase = axis.logBase;
            logBaseE = axis.logBaseE;
            ceilRate = axis.ceilRate;
            insertDataToHead = axis.insertDataToHead;
            axisLine.Copy(axis.axisLine);
            axisName.Copy(axis.axisName);
            axisTick.Copy(axis.axisTick);
            axisLabel.Copy(axis.axisLabel);
            splitLine.Copy(axis.splitLine);
            splitArea.Copy(axis.splitArea);
            minorTick.Copy(axis.minorTick);
            minorSplitLine.Copy(axis.minorSplitLine);
            indicatorLabel.Copy(axis.indicatorLabel);
            animation.Copy(axis.animation);
            ChartHelper.CopyList(data, axis.data);
            ChartHelper.CopyList<Sprite>(icons, axis.icons);
        }

        /// <summary>
        /// 清空类目数据
        /// </summary>
        public override void ClearData()
        {
            m_Data.Clear();
            m_Icons.Clear();
            context.Clear();
            SetAllDirty();
        }

        /// <summary>
        /// 是否为类目轴。
        /// </summary>
        /// <returns></returns>
        public bool IsCategory()
        {
            return m_Type == AxisType.Category;
        }

        /// <summary>
        /// 是否为数值轴。
        /// </summary>
        /// <returns></returns>
        public bool IsValue()
        {
            return m_Type == AxisType.Value;
        }

        /// <summary>
        /// 是否为对数轴。
        /// </summary>
        /// <returns></returns>
        public bool IsLog()
        {
            return m_Type == AxisType.Log;
        }

        /// <summary>
        /// 是否为时间轴。
        /// </summary>
        public bool IsTime()
        {
            return m_Type == AxisType.Time;
        }

        public bool IsLeft()
        {
            return m_Position == AxisPosition.Left;
        }

        public bool IsRight()
        {
            return m_Position == AxisPosition.Right;
        }

        public bool IsTop()
        {
            return m_Position == AxisPosition.Top;
        }

        public bool IsBottom()
        {
            return m_Position == AxisPosition.Bottom;
        }

        public bool IsNeedShowLabel(int index, int total = 0)
        {
            if (total == 0)
            {
                total = context.labelValueList.Count;
            }
            return axisLabel.IsNeedShowLabel(index, total);
        }

        public void SetNeedUpdateFilterData()
        {
            context.isNeedUpdateFilterData = true;
        }

        /// <summary>
        /// 添加一个类目到类目数据列表
        /// </summary>
        /// <param name="category"></param>
        public void AddData(string category)
        {
            if (maxCache > 0)
            {
                if (context.addedDataCount < m_Data.Count)
                    context.addedDataCount = m_Data.Count;
                while (m_Data.Count >= maxCache)
                {
                    RemoveData(m_InsertDataToHead ? m_Data.Count - 1 : 0);
                }
            }
            context.addedDataCount++;
            if (m_InsertDataToHead)
                m_Data.Insert(0, category);
            else
                m_Data.Add(category);

            SetAllDirty();
        }

        /// <summary>
        /// get the history data count.
        /// ||获得添加过的历史数据总数
        /// </summary>
        /// <returns></returns>
        public int GetAddedDataCount()
        {
            return context.addedDataCount < m_Data.Count ? m_Data.Count : context.addedDataCount;
        }

        public void RemoveData(int dataIndex)
        {
            context.isNeedUpdateFilterData = true;
            m_Data.RemoveAt(dataIndex);
        }

        /// <summary>
        /// 更新类目数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="category"></param>
        public void UpdateData(int index, string category)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                m_Data[index] = category;
                SetComponentDirty();
            }
        }

        /// <summary>
        /// 添加图标
        /// </summary>
        /// <param name="icon"></param>
        public void AddIcon(Sprite icon)
        {
            if (maxCache > 0)
            {
                while (m_Icons.Count > maxCache)
                {
                    m_Icons.RemoveAt(m_InsertDataToHead ? m_Icons.Count - 1 : 0);
                }
            }
            if (m_InsertDataToHead) m_Icons.Insert(0, icon);
            else m_Icons.Add(icon);
            SetAllDirty();
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        /// <param name="index"></param>
        /// <param name="icon"></param>
        public void UpdateIcon(int index, Sprite icon)
        {
            if (index >= 0 && index < m_Icons.Count)
            {
                m_Icons[index] = icon;
                SetComponentDirty();
            }
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

        public Sprite GetIcon(int index)
        {
            if (index >= 0 && index < m_Icons.Count)
                return m_Icons[index];
            else
                return null;
        }

        /// <summary>
        /// 获得值在坐标轴上的距离
        /// </summary>
        /// <param name="value"></param>
        /// <param name="axisLength"></param>
        /// <returns></returns>
        public float GetDistance(double value, float axisLength = 0)
        {
            if (context.minMaxRange == 0)
                return 0;
            if (axisLength == 0)
            {
                axisLength = context.length;
            }

            if (IsCategory() && boundaryGap)
            {
                var each = axisLength / data.Count;
                return (float)(each * (value + 0.5f));
            }
            else if (IsLog())
            {
                var logValue = GetLogValue(value);
                var logMin = GetLogValue(context.minValue);
                var logMax = GetLogValue(context.maxValue);
                return axisLength * (float)((logValue - logMin) / (logMax - logMin));
            }
            else
            {
                return axisLength * (float)((value - context.minValue) / context.minMaxRange);
            }
        }

        public float GetValueLength(double value, float axisLength)
        {
            if (context.minMaxRange > 0)
            {
                return axisLength * ((float)(value / context.minMaxRange));
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获得指定区域缩放的类目数据列表
        /// </summary>
        /// <param name="dataZoom">区域缩放</param>
        /// <returns></returns>
        internal List<string> GetDataList(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.enable && dataZoom.IsContainsAxis(this))
            {
                UpdateFilterData(dataZoom);
                return context.filterData;
            }
            else
            {
                return m_Data.Count > 0 ? m_Data : context.runtimeData;
            }
        }

        internal List<string> GetDataList()
        {
            return m_Data.Count > 0 ? m_Data : context.runtimeData;
        }

        /// <summary>
        /// 更新dataZoom对应的类目数据列表
        /// </summary>
        /// <param name="dataZoom"></param>
        internal void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.enable && dataZoom.IsContainsAxis(this))
            {
                var data = GetDataList();
                context.UpdateFilterData(data, dataZoom);
            }
        }

        /// <summary>
        /// 获得类目数据个数
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        internal int GetDataCount(DataZoom dataZoom)
        {
            return IsCategory() ? GetDataList(dataZoom).Count : 0;
        }

        /// <summary>
        /// 更新刻度标签文字
        /// </summary>
        /// <param name="dataZoom"></param>
        internal void UpdateLabelText(float coordinateWidth, DataZoom dataZoom, bool forcePercent)
        {
            for (int i = 0; i < context.labelObjectList.Count; i++)
            {
                if (context.labelObjectList[i] != null)
                {
                    var text = AxisHelper.GetLabelName(this, coordinateWidth, i, context.destMinValue, context.destMaxValue, dataZoom, forcePercent);
                    context.labelObjectList[i].SetText(text);
                }
            }
        }

        internal Vector3 GetLabelObjectPosition(int index)
        {
            if (context.labelObjectList != null && index < context.labelObjectList.Count)
                return context.labelObjectList[index].GetPosition();
            else
                return Vector3.zero;
        }

        internal void UpdateMinMaxValue(double minValue, double maxValue, bool needAnimation = false)
        {
            if (needAnimation)
            {
                if (context.lastMinValue == 0 && context.lastMaxValue == 0)
                {
                    context.minValue = minValue;
                    context.maxValue = maxValue;
                }
                context.lastMinValue = context.minValue;
                context.lastMaxValue = context.maxValue;
                context.destMinValue = minValue;
                context.destMaxValue = maxValue;
            }
            else
            {
                context.minValue = minValue;
                context.maxValue = maxValue;
                context.destMinValue = minValue;
                context.destMaxValue = maxValue;
            }
            double tempRange = maxValue - minValue;
            if (context.minMaxRange != tempRange)
            {
                context.minMaxRange = tempRange;
                if (type == Axis.AxisType.Value && interval > 0)
                {
                    SetComponentDirty();
                }
            }
        }

        public float GetLogValue(double value)
        {
            if (value <= 0 || value == 1)
                return 0;
            else
                return logBaseE ? (float)Math.Log(value) : (float)Math.Log(value, logBase);
        }

        public double GetLogMinIndex()
        {
            if (context.minValue <= 0 || context.minValue == 1)
                return 0;
            return logBaseE ?
                Math.Log(context.minValue) :
                Math.Log(context.minValue, logBase);
        }

        public double GetLogMaxIndex()
        {
            if (context.maxValue <= 0 || context.maxValue == 1)
                return 0;
            return logBaseE ?
                Math.Log(context.maxValue) :
                Math.Log(context.maxValue, logBase);
        }

        public double GetLabelValue(int index)
        {
            if (index < 0)
                return context.minValue;
            else if (index > context.labelValueList.Count - 1)
                return context.maxValue;
            else
                return context.labelValueList[index];
        }

        public double GetLastLabelValue()
        {
            if (context.labelValueList.Count > 0)
                return context.labelValueList[context.labelValueList.Count - 1];
            else
                return 0;
        }

        public void UpdateZeroOffset(float axisLength)
        {
            context.offset = context.minValue > 0 || context.minMaxRange == 0 ?
                0 :
                (context.maxValue < 0 ?
                    axisLength :
                    (float)(Math.Abs(context.minValue) * (axisLength / (Math.Abs(context.minValue) + Math.Abs(context.maxValue))))
                );
        }

        public Vector3 GetCategoryPosition(int categoryIndex, int dataCount = 0)
        {
            if (dataCount <= 0)
            {
                dataCount = data.Count;
            }
            if (IsCategory() && dataCount > 0)
            {
                Vector3 pos;
                if (boundaryGap)
                {
                    var each = context.length / dataCount;
                    pos = context.start + context.dire * (each * (categoryIndex + 0.5f));
                }
                else
                {
                    var each = context.length / (dataCount - 1);
                    pos = context.start + context.dire * (each * categoryIndex);
                }
                if (axisLabel.distance != 0)
                {
                    if (this is YAxis)
                    {
                        pos.x = GetLabelObjectPosition(0).x;
                    }
                    else
                    {
                        pos.y = GetLabelObjectPosition(0).y;
                    }
                }
                return pos;
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}