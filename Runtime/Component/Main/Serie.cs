/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// the type of serie.
    /// 系列类型。
    /// </summary>
    public enum SerieType
    {
        /// <summary>
        /// 折线图。折线图是用折线将各个数据点标志连接起来的图表，用于展现数据的变化趋势。可用于直角坐标系和极坐标系上。
        /// </summary>
        Line,
        /// <summary>
        /// 柱状图。柱状/条形图 通过 柱形的高度/条形的宽度 来表现数据的大小，用于有至少一个类目轴或时间轴的直角坐标系上。
        /// </summary>
        Bar,
        /// <summary>
        /// 饼图。饼图主要用于表现不同类目的数据在总和中的占比。每个的弧度表示数据数量的比例。
        /// 饼图更适合表现数据相对于总数的百分比等关系。
        /// </summary>
        Pie,
        /// <summary>
        /// 雷达图。雷达图主要用于表现多变量的数据，例如球员的各个属性分析。依赖 radar 组件。
        /// </summary>
        Radar,
        /// <summary>
        /// 散点图。直角坐标系上的散点图可以用来展现数据的 x，y 之间的关系，如果数据项有多个维度，
        /// 其它维度的值可以通过不同大小的 symbol 展现成气泡图，也可以用颜色来表现。
        /// </summary>
        Scatter,
        /// <summary>
        /// 带有涟漪特效动画的散点图。利用动画特效可以将某些想要突出的数据进行视觉突出。
        /// </summary>
        EffectScatter,
        /// <summary>
        /// 热力图。主要通过颜色去表现数值的大小，必须要配合 visualMap 组件使用。
        /// </summary>
        Heatmap,
    }

    /// <summary>
    /// Whether to show as Nightingale chart, which distinguishs data through radius. 
    /// 是否展示成南丁格尔图，通过半径区分数据大小。
    /// </summary>
    public enum RoseType
    {
        /// <summary>
        /// Don't show as Nightingale chart.不展示成南丁格尔玫瑰图
        /// </summary>
        None,
        /// <summary>
        /// Use central angle to show the percentage of data, radius to show data size.
        /// 扇区圆心角展现数据的百分比，半径展现数据的大小。
        /// </summary>
        Radius,
        /// <summary>
        /// All the sectors will share the same central angle, the data size is shown only through radiuses.
        /// 所有扇区圆心角相同，仅通过半径展现数据大小。
        /// </summary>
        Area
    }

    /// <summary>
    /// the type of line chart.
    /// 折线图样式类型
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// the normal line chart，
        /// 普通折线图。
        /// </summary>
        Normal,
        /// <summary>
        /// the smooth line chart，
        /// 平滑曲线。
        /// </summary>
        Smooth,
        /// <summary>
        /// the smooth-dash line chart，
        /// 平滑虚线。
        /// </summary>
        SmoothDash,
        /// <summary>
        /// step line.
        /// 阶梯线图：当前点。
        /// </summary>
        StepStart,
        /// <summary>
        /// step line.
        /// 阶梯线图：当前点和下一个点的中间。
        /// </summary>
        StepMiddle,
        /// <summary>
        /// step line.
        /// 阶梯线图：下一个拐点。
        /// </summary>
        StepEnd,
        /// <summary>
        /// 虚线
        /// </summary>
        Dash,
        /// <summary>
        /// 点线
        /// </summary>
        Dot,
        /// <summary>
        /// 点划线
        /// </summary>
        DashDot,
        /// <summary>
        /// 双点划线
        /// </summary>
        DashDotDot
    }

    public enum BarType
    {
        /// <summary>
        /// 普通柱形图
        /// </summary>
        Normal,
        /// <summary>
        /// 斑马柱形图
        /// </summary>
        Zebra,
        /// <summary>
        /// 胶囊柱形图
        /// </summary>
        Capsule
    }

    /// <summary>
    /// 采样类型
    /// </summary>
    public enum SampleType
    {
        /// <summary>
        /// 取峰值。
        /// </summary>
        Peak,
        /// <summary>
        /// 取过滤点的平均值。
        /// </summary>
        Average,
        /// <summary>
        /// 取过滤点的最大值。
        /// </summary>
        Max,
        /// <summary>
        /// 取过滤点的最小值。
        /// </summary>
        Min,
        /// <summary>
        /// 取过滤点的和。
        /// </summary>
        Sum
    }

    /// <summary>
    /// 系列。每个系列通过 type 决定自己的图表类型。
    /// </summary>
    [System.Serializable]
    public class Serie : MainComponent
    {
        [SerializeField] [DefaultValue("true")] private bool m_Show = true;
        [SerializeField] private SerieType m_Type;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Stack;
        [SerializeField] [Range(0, 1)] private int m_AxisIndex = 0;
        [SerializeField] private int m_RadarIndex = 0;
        [SerializeField] protected int m_MinShow;
        [SerializeField] protected int m_MaxShow;
        [SerializeField] protected int m_MaxCache;
        [SerializeField] private AreaStyle m_AreaStyle = AreaStyle.defaultAreaStyle;
        [SerializeField] private SerieSymbol m_Symbol = new SerieSymbol();

        [SerializeField] private float m_SampleDist = 0;
        [SerializeField] private SampleType m_SampleType = SampleType.Average;
        [SerializeField] private float m_SampleAverage = 0;

        [SerializeField] private LineType m_LineType = LineType.Normal;
        [SerializeField] private LineStyle m_LineStyle = new LineStyle();

        [SerializeField] private BarType m_BarType = BarType.Normal;
        [SerializeField] private bool m_BarPercentStack = false;
        [SerializeField] private float m_BarWidth = 0.6f;
        [SerializeField] private float m_BarGap = 0.3f; // 30%
        [SerializeField] private float m_BarCategoryGap = 0.2f; // 20%
        [SerializeField] private float m_BarZebraWidth = 4f;
        [SerializeField] private float m_BarZebraGap = 2f;


        [SerializeField] private bool m_ClickOffset = true;
        [SerializeField] private RoseType m_RoseType = RoseType.None;
        [SerializeField] private float m_Space;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private float[] m_Radius = new float[2] { 0, 80 };
        [SerializeField] private SerieLabel m_Label = new SerieLabel();
        [SerializeField] private SerieAnimation m_Animation = new SerieAnimation();
        [SerializeField] private LineArrow m_LineArrow = new LineArrow();
        [SerializeField] private ItemStyle m_ItemStyle = new ItemStyle();
        [SerializeField] private Emphasis m_Emphasis = new Emphasis();
        [SerializeField] [Range(1, 10)] private int m_ShowDataDimension;
        [SerializeField] private bool m_ShowDataName;
        [SerializeField] private bool m_ShowDataIcon;

        [SerializeField] private List<SerieData> m_Data = new List<SerieData>();

        [NonSerialized] private int m_FilterStart;
        [NonSerialized] private int m_FilterEnd;
        [NonSerialized] private List<SerieData> m_FilterData;
        [NonSerialized] private Dictionary<int, List<Vector3>> m_UpSmoothPoints = new Dictionary<int, List<Vector3>>();
        [NonSerialized] private Dictionary<int, List<Vector3>> m_DownSmoothPoints = new Dictionary<int, List<Vector3>>();
        [NonSerialized] private List<Vector3> m_DataPoints = new List<Vector3>();
        [NonSerialized] private bool m_NeedUpdateFilterData;

        /// <summary>
        /// Whether to show serie in chart.
        /// 系列是否显示在图表上。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// the chart type of serie.
        /// 系列的图表类型。
        /// </summary>
        public SerieType type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// Series name used for displaying in tooltip and filtering with legend.
        /// 系列名称，用于 tooltip 的显示，legend 的图例筛选。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// 图例名称。当系列名称不为空时，图例名称即为系列名称；反之则为索引index。
        /// </summary>
        public string legendName { get { return string.IsNullOrEmpty(name) ? ChartCached.IntToStr(index) : name; } }
        /// <summary>
        /// If stack the value. On the same category axis, the series with the same stack name would be put on top of each other.
        /// 数据堆叠，同个类目轴上系列配置相同的stack值后，后一个系列的值会在前一个系列的值上相加。
        /// </summary>
        public string stack { get { return m_Stack; } set { m_Stack = value; } }
        /// <summary>
        /// Index of axis to combine with, which is useful for multiple x axes in one chart.
        /// 使用的坐标轴轴的 index，在单个图表实例中存在多个坐标轴轴的时候有用。
        /// </summary>
        public int axisIndex { get { return m_AxisIndex; } set { m_AxisIndex = value; } }
        /// <summary>
        /// Index of radar component that radar chart uses.
        /// 雷达图所使用的 radar 组件的 index。
        /// </summary>
        public int radarIndex { get { return m_RadarIndex; } set { m_RadarIndex = value; } }
        /// <summary>
        /// The min number of data to show in chart.
        /// 系列所显示数据的最小索引
        /// </summary>
        public int minShow
        {
            get { return m_MinShow; }
            set { m_MinShow = value; if (m_MinShow < 0) m_MinShow = 0; }
        }
        /// <summary>
        /// The max number of data to show in chart.
        /// 系列所显示数据的最大索引
        /// </summary>
        public int maxShow
        {
            get { return m_MaxShow; }
            set { m_MaxShow = value; if (m_MaxShow < 0) m_MaxShow = 0; }
        }
        /// <summary>
        /// The max number of serie data cache.
        /// The first data will be remove when the size of serie data is larger then maxCache.
        /// default:0,unlimited.
        /// 系列中可缓存的最大数据量。默认为0没有限制，大于0时超过指定值会移除旧数据再插入新数据。
        /// </summary>
        public int maxCache
        {
            get { return m_MaxCache; }
            set { m_MaxCache = value; if (m_MaxCache < 0) m_MaxCache = 0; }
        }
        /// <summary>
        /// The style of area.
        /// 区域填充样式。
        /// </summary>
        public AreaStyle areaStyle { get { return m_AreaStyle; } set { m_AreaStyle = value; } }
        /// <summary>
        /// the symbol of serie data item.
        /// 标记的图形。
        /// </summary>
        public SerieSymbol symbol { get { return m_Symbol; } set { m_Symbol = value; } }
        /// <summary>
        /// The type of line chart.
        /// 折线图样式类型。
        /// </summary>
        public LineType lineType { get { return m_LineType; } set { m_LineType = value; } }
        /// <summary>
        /// the min pixel dist of sample.
        /// 采样的最小像素距离，默认为0时不采样。当两个数据点间的水平距离小于改值时，开启采样，保证两点间的水平距离不小于改值。
        /// </summary>
        public float sampleDist { get { return m_SampleDist; } set { m_SampleDist = value < 0 ? 0 : value; } }
        /// <summary>
        /// the type of sample.
        /// 采样类型。当sampleDist大于0时有效。
        /// </summary>
        public SampleType sampleType { get { return m_SampleType; } set { m_SampleType = value; } }
        /// <summary>
        /// 设定的采样平均值。当sampleType 为 Peak 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为0时会实时计算所有数据的平均值。
        /// </summary>
        public float sampleAverage { get { return m_SampleAverage; } set { m_SampleAverage = value; } }
        /// <summary>
        /// The style of line.
        /// 线条样式。
        /// </summary>
        public LineStyle lineStyle { get { return m_LineStyle; } set { m_LineStyle = value; } }
        /// <summary>
        /// 柱形图类型。
        /// </summary>
        public BarType barType { get { return m_BarType; } set { m_BarType = value; } }
        /// <summary>
        /// 柱形图是否为百分比堆积。相同stack的serie只要有一个barPercentStack为true，则就显示成百分比堆叠柱状图。
        /// </summary>
        public bool barPercentStack { get { return m_BarPercentStack; } set { m_BarPercentStack = value; } }
        /// <summary>
        /// The width of the bar. Adaptive when default 0.
        /// 柱条的宽度，不设时自适应。支持设置成相对于类目宽度的百分比。
        /// </summary>
        public float barWidth { get { return m_BarWidth; } set { m_BarWidth = value; } }
        /// <summary>
        /// The gap between bars between different series, is a percent value like '0.3f' , which means 30% of the bar width, can be set as a fixed value.
        /// <para>Set barGap as '-1' can overlap bars that belong to different series, which is useful when making a series of bar be background.
        /// In a single coodinate system, this attribute is shared by multiple 'bar' series. 
        /// This attribute should be set on the last 'bar' series in the coodinate system, 
        /// then it will be adopted by all 'bar' series in the coordinate system.</para>
        /// 不同系列的柱间距离。为百分比（如 '0.3f'，表示柱子宽度的 30%）
        /// 如果想要两个系列的柱子重叠，可以设置 barGap 为 '-1f'。这在用柱子做背景的时候有用。
        /// 在同一坐标系上，此属性会被多个 'bar' 系列共享。此属性应设置于此坐标系中最后一个 'bar' 系列上才会生效，并且是对此坐标系中所有 'bar' 系列生效。
        /// </summary>
        /// <value></value>
        public float barGap { get { return m_BarGap; } set { m_BarGap = value; } }
        /// <summary>
        /// The bar gap of a single series, defaults to be 20% of the category gap, can be set as a fixed value.
        /// In a single coodinate system, this attribute is shared by multiple 'bar' series. 
        /// This attribute should be set on the last 'bar' series in the coodinate system, 
        /// then it will be adopted by all 'bar' series in the coordinate system.
        /// 同一系列的柱间距离，默认为类目间距的20%，可设固定值。
        /// 在同一坐标系上，此属性会被多个 'bar' 系列共享。此属性应设置于此坐标系中最后一个 'bar' 系列上才会生效，并且是对此坐标系中所有 'bar' 系列生效。
        /// </summary>
        public float barCategoryGap { get { return m_BarCategoryGap; } set { m_BarCategoryGap = value; } }
        /// <summary>
        /// 斑马线的粗细。
        /// </summary>
        public float barZebraWidth { get { return m_BarZebraWidth; } set { m_BarZebraWidth = value > 0 ? value : 0; } }
        /// <summary>
        /// 斑马线的间距。
        /// </summary>
        public float barZebraGap { get { return m_BarZebraGap; } set { m_BarZebraGap = value > 0 ? value : 0; } }

        /// <summary>
        /// Whether offset when mouse click pie chart item.
        /// 鼠标点击时是否开启偏移，一般用在PieChart图表中。
        /// </summary>
        public bool pieClickOffset { get { return m_ClickOffset; } set { m_ClickOffset = value; } }
        /// <summary>
        /// Whether to show as Nightingale chart.
        /// 是否展示成南丁格尔图，通过半径区分数据大小。
        /// </summary>
        public RoseType pieRoseType { get { return m_RoseType; } set { m_RoseType = value; } }
        /// <summary>
        /// the space of pie chart item.
        /// 饼图项间的空隙留白。
        /// </summary>
        public float pieSpace { get { return m_Space; } set { m_Space = value; } }
        /// <summary>
        /// the center of pie chart.
        /// 饼图的中心点。
        /// </summary>
        public float[] pieCenter { get { return m_Center; } set { m_Center = value; } }
        /// <summary>
        /// the radius of pie chart.
        /// 饼图的半径。radius[0]表示内径，radius[1]表示外径。
        /// </summary>
        public float[] pieRadius { get { return m_Radius; } set { m_Radius = value; } }
        /// <summary>
        /// Text label of graphic element,to explain some data information about graphic item like value, name and so on. 
        /// 图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
        /// </summary>
        public SerieLabel label { get { return m_Label; } set { m_Label = value; } }
        /// <summary>
        /// The start animation.
        /// 起始动画。
        /// </summary>
        public SerieAnimation animation { get { return m_Animation; } set { m_Animation = value; } }
        /// <summary>
        /// The arrow of line.
        /// 折线图的箭头。
        /// </summary>
        public LineArrow lineArrow { get { return m_LineArrow; } set { m_LineArrow = value; } }
        /// <summary>
        /// The style of data item.
        /// 图形样式。
        /// </summary>
        public ItemStyle itemStyle { get { return m_ItemStyle; } set { m_ItemStyle = value; } }
        /// <summary>
        /// 高亮的图形样式和文本标签样式。
        /// </summary>
        public Emphasis emphasis { get { return m_Emphasis; } set { m_Emphasis = value; } }

        /// <summary>
        /// 系列中的数据内容数组。SerieData可以设置1到n维数据。
        /// </summary>
        public List<SerieData> data { get { return m_Data; } }

        /// <summary>
        /// The index of serie,start at 0.
        /// 系列的索引，从0开始。
        /// </summary>
        public int index { get; internal set; }
        /// <summary>
        /// Whether the serie is highlighted.
        /// 该系列是否高亮，一般由图例悬停触发。
        /// </summary>
        public bool highlighted { get; internal set; }
        /// <summary>
        /// the count of data list.
        /// 数据项个数。
        /// </summary>
        public int dataCount { get { return m_Data.Count; } }
        /// <summary>
        /// 数据项位置坐标。
        /// </summary>
        public List<Vector3> dataPoints { get { return m_DataPoints; } }
        /// <summary>
        /// 饼图的中心点位置。
        /// </summary>
        public Vector3 runtimePieCenterPos { get; internal set; }
        /// <summary>
        /// 饼图的内径
        /// </summary>
        public float runtimePieInsideRadius { get; internal set; }
        /// <summary>
        /// 饼图的外径
        /// </summary>
        public float runtimePieOutsideRadius { get; internal set; }
        /// <summary>
        /// 饼图的数据项最大值
        /// </summary>
        public float runtimePieDataMax { get; internal set; }
        /// <summary>
        /// 饼图的数据项之和
        /// </summary>
        public float runtimePieDataTotal { get; internal set; }
        internal int runtimeLastCheckDataCount { get; set; }

        internal List<Vector3> GetUpSmoothList(int dataIndex, int size = 100)
        {
            if (m_UpSmoothPoints.ContainsKey(dataIndex))
            {
                return m_UpSmoothPoints[dataIndex];
            }
            else
            {
                var list = new List<Vector3>(size);
                m_UpSmoothPoints[dataIndex] = list;
                return list;
            }
        }

        internal List<Vector3> GetDownSmoothList(int dataIndex, int size = 100)
        {
            if (m_DownSmoothPoints.ContainsKey(dataIndex))
            {
                return m_DownSmoothPoints[dataIndex];
            }
            else
            {
                var list = new List<Vector3>(size);
                m_DownSmoothPoints[dataIndex] = list;
                return list;
            }
        }

        internal void ClearSmoothList(int dataIndex)
        {
            if (m_UpSmoothPoints.ContainsKey(dataIndex))
            {
                m_UpSmoothPoints[dataIndex].Clear();
            }
            if (m_DownSmoothPoints.ContainsKey(dataIndex))
            {
                m_DownSmoothPoints[dataIndex].Clear();
            }
        }

        /// <summary>
        /// 维度Y对应数据中最大值。
        /// </summary>
        public float yMax
        {
            get
            {
                float max = int.MinValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[1] > max)
                    {
                        max = sdata.data[1];
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// 维度X对应数据中的最大值。
        /// </summary>
        public float xMax
        {
            get
            {
                float max = int.MinValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[0] > max)
                    {
                        max = sdata.data[0];
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// 维度Y对应数据的最小值。
        /// </summary>
        public float yMin
        {
            get
            {
                float min = int.MaxValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[1] < min)
                    {
                        min = sdata.data[1];
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// 维度X对应数据的最小值。
        /// </summary>
        public float xMin
        {
            get
            {
                float min = int.MaxValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[0] < min)
                    {
                        min = sdata.data[0];
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// 维度Y数据的总和。
        /// </summary>
        public float yTotal
        {
            get
            {
                float total = 0;
                foreach (var sdata in data)
                {
                    if (sdata.show)
                        total += sdata.data[1];
                }
                return total;
            }
        }

        /// <summary>
        /// 维度X数据的总和。
        /// </summary>
        public float xTotal
        {
            get
            {
                float total = 0;
                foreach (var sdata in data)
                {
                    if (sdata.show)
                        total += sdata.data[0];
                }
                return total;
            }
        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void ClearData()
        {
            m_Data.Clear();
        }

        /// <summary>
        /// 移除指定索引的数据
        /// </summary>
        /// <param name="index"></param>
        public void RemoveData(int index)
        {
            m_Data.RemoveAt(index);
        }

        /// <summary>
        /// 添加一个数据到维度Y（此时维度X对应的数据是索引）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataName"></param>
        public SerieData AddYData(float value, string dataName = null)
        {
            if (m_MaxCache > 0)
            {
                while (m_Data.Count > m_MaxCache)
                {
                    m_NeedUpdateFilterData = true;
                    m_Data.RemoveAt(0);
                }
            }
            int xValue = m_Data.Count;
            var serieData = new SerieData() { data = new List<float>() { xValue, value }, name = dataName };
            serieData.index = xValue;
            m_Data.Add(serieData);
            return serieData;
        }

        /// <summary>
        /// 添加（x，y）数据到维度X和维度Y
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        /// <param name="dataName"></param>
        /// <param name="maxDataNumber"></param>
        public SerieData AddXYData(float xValue, float yValue, string dataName = null)
        {
            if (m_MaxCache > 0)
            {
                while (m_Data.Count > m_MaxCache)
                {
                    m_NeedUpdateFilterData = true;
                    m_Data.RemoveAt(0);
                }
            }
            var serieData = new SerieData() { data = new List<float>() { xValue, yValue }, name = dataName };
            serieData.index = m_Data.Count;
            m_Data.Add(serieData);
            return serieData;
        }

        /// <summary>
        /// 将一组数据添加到系列中。
        /// 如果数据只有一个，默认添加到维度Y中。
        /// </summary>
        /// <param name="valueList"></param>
        /// <param name="dataName"></param>
        /// <param name="maxDataNumber"></param>
        public SerieData AddData(List<float> valueList, string dataName = null)
        {
            if (valueList == null || valueList.Count == 0) return null;
            if (valueList.Count == 1)
            {
                return AddYData(valueList[0], dataName);
            }
            else if (valueList.Count == 2)
            {
                return AddXYData(valueList[0], valueList[1], dataName);
            }
            else
            {
                if (m_MaxCache > 0)
                {
                    while (m_Data.Count > m_MaxCache)
                    {
                        m_NeedUpdateFilterData = true;
                        m_Data.RemoveAt(0);
                    }
                }
                var serieData = new SerieData();
                serieData.name = dataName;
                serieData.index = m_Data.Count;
                for (int i = 0; i < valueList.Count; i++)
                {
                    serieData.data.Add(valueList[i]);
                }
                m_Data.Add(serieData);
                return serieData;
            }
        }

        /// <summary>
        /// 获得维度Y索引对应的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public float GetYData(int index, DataZoom dataZoom = null)
        {
            if (index < 0) return 0;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                return serieData[index].data[1];
            }
            return 0;
        }

        /// <summary>
        /// 获得维度Y索引对应的数据和数据名
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="yData">对应的数据值</param>
        /// <param name="dataName">对应的数据名</param>
        /// <param name="dataZoom">区域缩放</param>
        public void GetYData(int index, out float yData, out string dataName, DataZoom dataZoom = null)
        {
            yData = 0;
            dataName = null;
            if (index < 0) return;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                yData = serieData[index].data[1];
                dataName = serieData[index].name;
            }
        }

        /// <summary>
        /// 获得指定索引的数据项
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public SerieData GetSerieData(int index, DataZoom dataZoom = null)
        {
            var data = GetDataList(dataZoom);
            if (index >= 0 && index <= data.Count - 1)
            {
                return data[index];
            }
            return null;
        }

        /// <summary>
        /// 获得指定索引的维度X和维度Y的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataZoom"></param>
        /// <param name="xValue"></param>
        /// <param name="yVlaue"></param>
        public void GetXYData(int index, DataZoom dataZoom, out float xValue, out float yVlaue)
        {
            xValue = 0;
            yVlaue = 0;
            if (index < 0) return;
            var showData = GetDataList(dataZoom);
            if (index < showData.Count)
            {
                var serieData = showData[index];
                xValue = serieData.data[0];
                yVlaue = serieData.data[1];
            }
        }

        /// <summary>
        /// 获得系列的数据列表
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public List<SerieData> GetDataList(DataZoom dataZoom = null)
        {
            if (dataZoom != null && dataZoom.enable)
            {
                UpdateFilterData(dataZoom);
                return m_FilterData;
            }
            else
            {
                return m_Data;
            }
        }

        /// <summary>
        /// 获得指定维数的最大最小值
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public void GetMinMaxData(int dimension, out float minValue, out float maxValue, DataZoom dataZoom = null)
        {
            var dataList = GetDataList(dataZoom);
            float max = float.MinValue;
            float min = float.MaxValue;
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (value > max) max = value;
                    if (value < min) min = value;
                }
            }
            maxValue = max;
            minValue = min;
        }

        private List<SerieData> emptyFilter = new List<SerieData>();
        /// <summary>
        /// 根据dataZoom更新数据列表缓存
        /// </summary>
        /// <param name="dataZoom"></param>
        internal void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.enable)
            {
                var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
                if (startIndex != m_FilterStart || endIndex != m_FilterEnd || m_NeedUpdateFilterData)
                {
                    m_FilterStart = startIndex;
                    m_FilterEnd = endIndex;
                    m_NeedUpdateFilterData = false;
                    var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                    if (m_Data.Count > 0)
                    {
                        m_FilterData = m_Data.GetRange(startIndex, count);
                    }
                    else
                    {
                        m_FilterData = m_Data;
                    }
                }
                else if (endIndex == 0)
                {
                    m_FilterData = emptyFilter;
                }
            }
        }

        /// <summary>
        /// 更新指定索引的维度Y数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void UpdateYData(int index, float value)
        {
            UpdateData(index, 1, value);
        }

        /// <summary>
        /// 更新指定索引的维度X和维度Y的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public void UpdateXYData(int index, float xValue, float yValue)
        {
            UpdateData(index, 0, xValue);
            UpdateData(index, 1, yValue);
        }

        /// <summary>
        /// 更新指定索引指定维数的数据
        /// </summary>
        /// <param name="index">要更新数据的索引</param>
        /// <param name="dimension">要更新数据的维数</param>
        /// <param name="value">新的数据值</param>
        public void UpdateData(int index, int dimension, float value)
        {
            if (index < 0) return;
            if (index < m_Data.Count && dimension < m_Data[index].data.Count)
            {
                m_Data[index].data[dimension] = value;
            }
        }

        public void UpdateDataName(int index, string name)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                var serieData = m_Data[index];
                serieData.name = name;
                if (serieData.labelText != null)
                {
                    serieData.labelText.text = name == null ? "" : name;
                }
            }
        }

        /// <summary>
        /// 清除所有数据的高亮标志
        /// </summary>
        public void ClearHighlight()
        {
            highlighted = false;
            foreach (var sd in m_Data)
            {
                sd.highlighted = false;
            }
        }

        /// <summary>
        /// 设置指定索引的数据为高亮状态
        /// </summary>
        /// <param name="index"></param>
        public void SetHighlight(int index)
        {
            if (index <= 0) return;
            for (int i = 0; i < m_Data.Count; i++)
            {
                m_Data[i].highlighted = index == i;
            }
        }

        internal Color GetAreaColor(ThemeInfo theme, int index, bool highlight)
        {
            var color = areaStyle.color != Color.clear ? areaStyle.color : (Color)theme.GetColor(index);
            if (highlight)
            {
                if (areaStyle.highlightColor != Color.clear) color = areaStyle.highlightColor;
                else color *= color;
            }
            color.a *= areaStyle.opacity;
            return color;
        }

        internal Color GetAreaToColor(ThemeInfo theme, int index, bool highlight)
        {
            if (areaStyle.toColor != Color.clear)
            {
                var color = areaStyle.toColor;
                if (highlight)
                {
                    if (areaStyle.highlightToColor != Color.clear) color = areaStyle.highlightToColor;
                    else color *= color;
                }
                color.a *= areaStyle.opacity;
                return color;
            }
            else
            {
                return GetAreaColor(theme, index, highlight);
            }
        }

        internal Color GetLineColor(ThemeInfo theme, int index, bool highlight)
        {
            if (lineStyle.color != Color.clear)
            {
                var color = lineStyle.color;
                if (highlight) color *= color;
                color.a *= lineStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a *= lineStyle.opacity;
                return color;
            }
        }

        internal Color GetSymbolColor(ThemeInfo theme, int index, bool highlight)
        {
            if (symbol.color != Color.clear)
            {
                var color = symbol.color;
                if (highlight) color *= color;
                color.a *= symbol.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a *= symbol.opacity;
                return color;
            }
        }

        internal float GetBarWidth(float categoryWidth)
        {
            if (m_BarWidth > 1) return m_BarWidth;
            else return m_BarWidth * categoryWidth;
        }

        internal float GetBarGap(float categoryWidth)
        {
            if (m_BarGap == -1) return 0;
            else if (m_BarGap <= 1) return GetBarWidth(categoryWidth) * m_BarGap;
            else return m_BarGap;
        }

        /// <summary>
        /// 设置所有数据的图标是否显示
        /// </summary>
        /// <param name="flag"></param>
        public void SetDataIconActive(bool flag)
        {
            foreach (var data in m_Data)
            {
                data.iconStyle.show = flag;
            }
        }

        /// <summary>
        /// 设置指定index的数据图标是否显示
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="flag"></param>
        public void SetDataIconActive(int dataIndex, bool flag)
        {
            if (dataIndex >= 0 && dataIndex < m_Data.Count)
            {
                var data = m_Data[dataIndex];
                data.iconStyle.show = flag;
            }
        }

        /// <summary>
        /// 统一设置图标的尺寸
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetDataIconSize(float width, float height)
        {
            foreach (var data in m_Data)
            {
                data.iconStyle.width = width;
                data.iconStyle.height = height;
            }
        }

        /// <summary>
        /// 设置指定index的数据图标的图片
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="image"></param>
        public void SetDataIcon(int dataIndex, Sprite image)
        {
            if (dataIndex >= 0 && dataIndex < m_Data.Count)
            {
                var data = m_Data[dataIndex];
                data.iconStyle.sprite = image;
            }
        }

        internal bool IsNeedShowDataIcon()
        {
            foreach (var data in m_Data)
            {
                if (data.iconStyle.show) return true;
            }
            return false;
        }

        /// <summary>
        /// 设置指定index的数据图标的尺寸
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetDataIconSize(int dataIndex, float width, float height)
        {
            if (dataIndex >= 0 && dataIndex < m_Data.Count)
            {
                var data = m_Data[dataIndex];
                data.iconStyle.width = width;
                data.iconStyle.height = height;
            }
        }

        /// <summary>
        /// 设置指定index的数据图标的颜色
        /// </summary>
        /// <param name="dataIndex"></param>
        /// <param name="color"></param>
        public void SetDataIconColor(int dataIndex, Color color)
        {
            if (dataIndex >= 0 && dataIndex < m_Data.Count)
            {
                var data = m_Data[dataIndex];
                data.iconStyle.color = color;
            }
        }

        /// <summary>
        /// 从json中导入数据
        /// </summary>
        /// <param name="jsonData"></param>
        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            jsonData = jsonData.Replace("\r\n", "");
            jsonData = jsonData.Replace(" ", "");
            jsonData = jsonData.Replace("\n", "");
            int startIndex = jsonData.IndexOf("[");
            int endIndex = jsonData.LastIndexOf("]");
            if (startIndex == -1 || endIndex == -1)
            {
                Debug.LogError("json data need include in [ ]");
                return;
            }
            ClearData();
            string temp = jsonData.Substring(startIndex + 1, endIndex - startIndex - 1);
            if (temp.IndexOf("],") > -1 || temp.IndexOf("] ,") > -1)
            {
                string[] datas = temp.Split(new string[] { "],", "] ," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var data = datas[i].Split(new char[] { '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var serieData = new SerieData();
                    for (int j = 0; j < data.Length; j++)
                    {
                        var txt = data[j].Trim().Replace("]", "");
                        float value;
                        var flag = float.TryParse(txt, out value);
                        if (flag)
                        {
                            serieData.data.Add(value);
                        }
                        else serieData.name = txt.Replace("\"", "").Trim();
                    }
                    m_Data.Add(serieData);
                }
            }
            else if (temp.IndexOf("value") > -1 && temp.IndexOf("name") > -1)
            {
                string[] datas = temp.Split(new string[] { "},", "} ,", "}" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var arr = datas[i].Replace("{", "").Split(',');
                    var serieData = new SerieData();
                    foreach (var a in arr)
                    {
                        if (a.StartsWith("value:"))
                        {
                            float value = float.Parse(a.Substring(6, a.Length - 6));
                            serieData.data = new List<float>() { i, value };
                        }
                        else if (a.StartsWith("name:"))
                        {
                            string name = a.Substring(6, a.Length - 6 - 1);
                            serieData.name = name;
                        }
                        else if (a.StartsWith("selected:"))
                        {
                            string selected = a.Substring(9, a.Length - 9);
                            serieData.selected = bool.Parse(selected);
                        }
                    }
                    m_Data.Add(serieData);
                }
            }
            else
            {
                string[] datas = temp.Split(',');
                for (int i = 0; i < datas.Length; i++)
                {
                    float value;
                    var flag = float.TryParse(datas[i].Trim(), out value);
                    if (flag)
                    {
                        var serieData = new SerieData();
                        serieData.data = new List<float>() { i, value };
                        m_Data.Add(serieData);
                    }
                }
            }
        }
    }
}
