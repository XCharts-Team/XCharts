using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Whether to show as Nightingale chart, which distinguishs data through radius. 
    /// 是否展示成南丁格尔图，通过半径区分数据大小。
    /// </summary>
    public enum RoseType
    {
        /// <summary>
        /// Don't show as Nightingale chart.
        /// |不展示成南丁格尔玫瑰图。
        /// </summary>
        None,
        /// <summary>
        /// Use central angle to show the percentage of data, radius to show data size.
        /// |扇区圆心角展现数据的百分比，半径展现数据的大小。
        /// </summary>
        Radius,
        /// <summary>
        /// All the sectors will share the same central angle, the data size is shown only through radiuses.
        /// |所有扇区圆心角相同，仅通过半径展现数据大小。
        /// </summary>
        Area
    }

    /// <summary>
    /// the type of line chart.
    /// |折线图样式类型
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// the normal line chart，
        /// |普通折线图。
        /// </summary>
        Normal,
        /// <summary>
        /// the smooth line chart，
        /// |平滑曲线。
        /// </summary>
        Smooth,
        /// <summary>
        /// step line.
        /// |阶梯线图：当前点。
        /// </summary>
        StepStart,
        /// <summary>
        /// step line.
        /// |阶梯线图：当前点和下一个点的中间。
        /// </summary>
        StepMiddle,
        /// <summary>
        /// step line.
        /// |阶梯线图：下一个拐点。
        /// </summary>
        StepEnd
    }

    /// <summary>
    /// the type of bar. |柱状图类型。
    /// </summary>
    public enum BarType
    {
        /// <summary>
        /// normal bar.
        /// |普通柱形图。
        /// </summary>
        Normal,
        /// <summary>
        /// zebra bar.
        /// |斑马柱形图。
        /// </summary>
        Zebra,
        /// <summary>
        /// capsule bar.
        /// |胶囊柱形图。
        /// </summary>
        Capsule
    }

    /// <summary>
    /// the type of radar. |雷达图类型。
    /// </summary>
    public enum RadarType
    {
        /// <summary>
        /// multiple radar.
        /// |多圈雷达图。此时可一个雷达里绘制多个圈，一个serieData就可组成一个圈（多维数据）。
        /// </summary>
        Multiple,
        /// <summary>
        /// single radar.
        /// |单圈雷达图。此时一个雷达只能绘制一个圈，多个serieData组成一个圈，数据取自`data[1]`。
        /// </summary>
        Single
    }

    /// <summary>
    /// 采样类型
    /// </summary>
    public enum SampleType
    {
        /// <summary>
        /// Take a peak. When the average value of the filter point is greater than or equal to 'sampleAverage', 
        /// take the maximum value; If you do it the other way around, you get the minimum.
        /// |取峰值。
        /// </summary>
        Peak,
        /// <summary>
        /// Take the average of the filter points.
        /// |取过滤点的平均值。
        /// </summary>
        Average,
        /// <summary>
        /// Take the maximum value of the filter point.
        /// |取过滤点的最大值。
        /// </summary>
        Max,
        /// <summary>
        /// Take the minimum value of the filter point.
        /// |取过滤点的最小值。
        /// </summary>
        Min,
        /// <summary>
        /// Take the sum of the filter points.
        /// |取过滤点的和。
        /// </summary>
        Sum
    }

    /// <summary>
    /// 数据排序方式
    /// </summary>
    public enum SerieDataSortType
    {
        /// <summary>
        /// 按 data 的顺序
        /// </summary>
        None,
        /// <summary>
        /// 升序
        /// </summary>
        Ascending,
        /// <summary>
        /// 降序
        /// </summary>
        Descending,
    }

    /// <summary>
    /// 对齐方式
    /// </summary>
    public enum Align
    {
        Center,
        Left,
        Right
    }

    /// <summary>
    /// Serie state. Supports normal, emphasis, blur, and select states.
    /// |Serie状态。支持正常、高亮、淡出、选中四种状态。
    /// </summary>
    public enum SerieState
    {
        /// <summary>
        /// Normal state.
        /// |正常状态。
        /// </summary>
        Normal,
        /// <summary>
        /// Emphasis state.
        /// |高亮状态。
        /// </summary>
        Emphasis,
        /// <summary>
        /// Blur state.
        /// |淡出状态。
        /// </summary>
        Blur,
        /// <summary>
        /// Select state.
        /// |选中状态。
        /// </summary>
        Select,
        /// <summary>
        /// Auto state.
        /// |自动保持和父节点一致。一般用在SerieData。
        /// </summary>
        Auto
    }

    /// <summary>
    /// The policy to take color from theme.
    /// |从主题中取色策略。
    /// </summary>
    public enum SerieColorBy
    {
        /// <summary>
        /// Select state.
        /// |默认策略。每种Serie都有自己的默认的取颜色策略。比如Line默认是Series策略，Pie默认是Data策略
        /// </summary>
        Default,
        /// <summary>
        /// assigns the colors in the palette by serie, so that all data in the same series are in the same color;.
        /// |按照系列分配调色盘中的颜色，同一系列中的所有数据都是用相同的颜色。
        /// </summary>
        Serie,
        /// <summary>
        /// assigns colors in the palette according to data items, with each data item using a different color..
        /// |按照数据项分配调色盘中的颜色，每个数据项都使用不同的颜色。
        /// </summary>
        Data
    }

    /// <summary>
    /// 系列。
    /// </summary>
    [System.Serializable]
    public partial class Serie : BaseSerie, IComparable
    {
        [SerializeField] private int m_Index;
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_CoordSystem = "GridCoord";
        [SerializeField] private string m_SerieType = "";
        [SerializeField] private string m_SerieName;
        [SerializeField][Since("v3.2.0")] private SerieState m_State = SerieState.Normal;
        [SerializeField][Since("v3.2.0")] private SerieColorBy m_ColorBy = SerieColorBy.Default;
        [SerializeField] private string m_Stack;
        [SerializeField] private int m_XAxisIndex = 0;
        [SerializeField] private int m_YAxisIndex = 0;
        [SerializeField] private int m_RadarIndex = 0;
        [SerializeField] private int m_VesselIndex = 0;
        [SerializeField] private int m_PolarIndex = 0;
        [SerializeField] private int m_SingleAxisIndex = 0;
        [SerializeField] private int m_ParallelIndex = 0;
        [SerializeField] protected int m_MinShow;
        [SerializeField] protected int m_MaxShow;
        [SerializeField] protected int m_MaxCache;

        [SerializeField] private float m_SampleDist = 0;
        [SerializeField] private SampleType m_SampleType = SampleType.Average;
        [SerializeField] private float m_SampleAverage = 0;

        [SerializeField] private LineType m_LineType = LineType.Normal;
        [SerializeField] private BarType m_BarType = BarType.Normal;
        [SerializeField] private bool m_BarPercentStack = false;
        [SerializeField] private float m_BarWidth = 0;
        [SerializeField] private float m_BarGap = 0.1f;
        [SerializeField] private float m_BarZebraWidth = 4f;
        [SerializeField] private float m_BarZebraGap = 2f;

        [SerializeField] private float m_Min;
        [SerializeField] private float m_Max;
        [SerializeField] private float m_MinSize = 0f;
        [SerializeField] private float m_MaxSize = 1f;
        [SerializeField] private float m_StartAngle;
        [SerializeField] private float m_EndAngle;
        [SerializeField] private float m_MinAngle;
        [SerializeField] private bool m_Clockwise = true;
        [SerializeField] private bool m_RoundCap;
        [SerializeField] private int m_SplitNumber;
        [SerializeField] private bool m_ClickOffset = true;
        [SerializeField] private RoseType m_RoseType = RoseType.None;
        [SerializeField] private float m_Gap;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.48f };
        [SerializeField] private float[] m_Radius = new float[2] { 0, 0.28f };

        [SerializeField][Range(2, 10)] private int m_ShowDataDimension;
        [SerializeField] private bool m_ShowDataName;
        [SerializeField] private bool m_Clip = false;
        [SerializeField] private bool m_Ignore = false;
        [SerializeField] private double m_IgnoreValue = 0;
        [SerializeField] private bool m_IgnoreLineBreak = false;
        [SerializeField] private bool m_ShowAsPositiveNumber = false;
        [SerializeField] private bool m_Large = true;
        [SerializeField] private int m_LargeThreshold = 200;
        [SerializeField] private bool m_AvoidLabelOverlap = false;
        [SerializeField] private RadarType m_RadarType = RadarType.Multiple;
        [SerializeField] private bool m_PlaceHolder = false;

        [SerializeField] private SerieDataSortType m_DataSortType = SerieDataSortType.Descending;
        [SerializeField] private Orient m_Orient = Orient.Vertical;
        [SerializeField] private Align m_Align = Align.Center;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;
        [SerializeField] private bool m_InsertDataToHead;

        [SerializeField] private LineStyle m_LineStyle = new LineStyle();
        [SerializeField] private SerieSymbol m_Symbol = new SerieSymbol();
        [SerializeField] private AnimationStyle m_Animation = new AnimationStyle();
        [SerializeField] private ItemStyle m_ItemStyle = new ItemStyle();
        [SerializeField] private List<SerieData> m_Data = new List<SerieData>();

        [NonSerialized] internal int m_FilterStart;
        [NonSerialized] internal int m_FilterEnd;
        [NonSerialized] internal double m_FilterStartValue;
        [NonSerialized] internal double m_FilterEndValue;
        [NonSerialized] internal int m_FilterMinShow;
        [NonSerialized] internal bool m_NeedUpdateFilterData;
        [NonSerialized] public List<SerieData> m_FilterData = new List<SerieData>();
        [NonSerialized] private bool m_NameDirty;

        /// <summary>
        /// The index of serie.
        /// |系列索引。
        /// </summary>
        public int index { get { return m_Index; } internal set { m_Index = value; } }
        /// <summary>
        /// Whether to show serie in chart.
        /// |系列是否显示在图表上。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) { SetVerticesDirty(); SetSerieNameDirty(); } }
        }
        /// <summary>
        /// the chart coord system of serie.
        /// |使用的坐标系。
        /// </summary>
        public string coordSystem
        {
            get { return m_CoordSystem; }
            set { if (PropertyUtil.SetClass(ref m_CoordSystem, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the type of serie.
        /// |系列类型。
        /// </summary>
        public string serieType
        {
            get { return m_SerieType; }
            set { if (PropertyUtil.SetClass(ref m_SerieType, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Series name used for displaying in tooltip and filtering with legend.
        /// |系列名称，用于 tooltip 的显示，legend 的图例筛选。
        /// </summary>
        public string serieName
        {
            get { return m_SerieName; }
            set { if (PropertyUtil.SetClass(ref m_SerieName, value)) { SetVerticesDirty(); SetSerieNameDirty(); } }
        }
        /// <summary>
        /// Legend name. When the serie name is not empty, the legend name is the series name; Otherwise, it is index.
        /// |图例名称。当系列名称不为空时，图例名称即为系列名称；反之则为索引index。
        /// </summary>
        public string legendName { get { return string.IsNullOrEmpty(serieName) ? ChartCached.IntToStr(index) : serieName; } }
        /// <summary>
        /// The default state of a serie.
        /// |系列的默认状态。
        /// </summary>
        public SerieState state
        {
            get { return m_State; }
            set { if (PropertyUtil.SetStruct(ref m_State, value)) { SetAllDirty(); } }
        }
        /// <summary>
        /// The policy to take color from theme.
        /// |从主题中取色的策略。
        /// </summary>
        public SerieColorBy colorBy
        {
            //get { return m_ColorBy; }
            get { return m_ColorBy == SerieColorBy.Default?defaultColorBy : m_ColorBy; }
            set { if (PropertyUtil.SetStruct(ref m_ColorBy, value)) { SetAllDirty(); } }
        }
        /// <summary>
        /// If stack the value. On the same category axis, the series with the same stack name would be put on top of each other.
        /// |数据堆叠，同个类目轴上系列配置相同的stack值后，后一个系列的值会在前一个系列的值上相加。
        /// </summary>
        public string stack
        {
            get { return m_Stack; }
            set { if (PropertyUtil.SetClass(ref m_Stack, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the index of XAxis.
        /// |使用X轴的index。
        /// </summary>
        public int xAxisIndex
        {
            get { return m_XAxisIndex; }
            set { if (PropertyUtil.SetStruct(ref m_XAxisIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the index of YAxis.
        /// |使用Y轴的index。
        /// </summary>
        public int yAxisIndex
        {
            get { return m_YAxisIndex; }
            set { if (PropertyUtil.SetStruct(ref m_YAxisIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Index of radar component that radar chart uses.
        /// |雷达图所使用的 radar 组件的 index。
        /// </summary>
        public int radarIndex
        {
            get { return m_RadarIndex; }
            set { if (PropertyUtil.SetStruct(ref m_RadarIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Index of vesel component that liquid chart uses.
        /// |水位图所使用的 vessel 组件的 index。
        /// </summary>
        public int vesselIndex
        {
            get { return m_VesselIndex; }
            set { if (PropertyUtil.SetStruct(ref m_VesselIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Index of polar component that serie uses.
        /// |所使用的 polar 组件的 index。
        /// </summary>
        public int polarIndex
        {
            get { return m_PolarIndex; }
            set { if (PropertyUtil.SetStruct(ref m_PolarIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>s
        /// Index of single axis component that serie uses.
        /// |所使用的 singleAxis 组件的 index。
        /// </summary>
        public int singleAxisIndex
        {
            get { return m_SingleAxisIndex; }
            set { if (PropertyUtil.SetStruct(ref m_SingleAxisIndex, value)) SetAllDirty(); }
        }
        /// <summary>s
        /// Index of parallel coord component that serie uses.
        /// |所使用的 parallel coord 组件的 index。
        /// </summary>
        public int parallelIndex
        {
            get { return m_ParallelIndex; }
            set { if (PropertyUtil.SetStruct(ref m_ParallelIndex, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The min number of data to show in chart.
        /// |系列所显示数据的最小索引
        /// </summary>
        public int minShow
        {
            get { return m_MinShow; }
            set { if (PropertyUtil.SetStruct(ref m_MinShow, value < 0 ? 0 : value)) { SetVerticesDirty(); } }
        }
        /// <summary>
        /// The max number of data to show in chart.
        /// |系列所显示数据的最大索引
        /// </summary>
        public int maxShow
        {
            get { return m_MaxShow; }
            set { if (PropertyUtil.SetStruct(ref m_MaxShow, value < 0 ? 0 : value)) { SetVerticesDirty(); } }
        }
        /// <summary>
        /// The max number of serie data cache.
        /// The first data will be remove when the size of serie data is larger then maxCache.
        /// |系列中可缓存的最大数据量。默认为0没有限制，大于0时超过指定值会移除旧数据再插入新数据。
        /// </summary>
        public int maxCache
        {
            get { return m_MaxCache; }
            set { if (PropertyUtil.SetStruct(ref m_MaxCache, value < 0 ? 0 : value)) { SetVerticesDirty(); } }
        }

        /// <summary>
        /// the symbol of serie data item.
        /// |标记的图形。
        /// </summary>
        public SerieSymbol symbol
        {
            get { return m_Symbol; }
            set { if (PropertyUtil.SetClass(ref m_Symbol, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The type of line chart.
        /// |折线图样式类型。
        /// </summary>
        public LineType lineType
        {
            get { return m_LineType; }
            set { if (PropertyUtil.SetStruct(ref m_LineType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the min pixel dist of sample.
        /// |采样的最小像素距离，默认为0时不采样。当两个数据点间的水平距离小于改值时，开启采样，保证两点间的水平距离不小于改值。
        /// </summary>
        public float sampleDist
        {
            get { return m_SampleDist; }
            set { if (PropertyUtil.SetStruct(ref m_SampleDist, value < 0 ? 0 : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the type of sample.
        /// |采样类型。当sampleDist大于0时有效。
        /// </summary>
        public SampleType sampleType
        {
            get { return m_SampleType; }
            set { if (PropertyUtil.SetStruct(ref m_SampleType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 设定的采样平均值。当sampleType 为 Peak 时，用于和过滤数据的平均值做对比是取最大值还是最小值。默认为0时会实时计算所有数据的平均值。
        /// </summary>
        public float sampleAverage
        {
            get { return m_SampleAverage; }
            set { if (PropertyUtil.SetStruct(ref m_SampleAverage, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The style of line.
        /// |线条样式。
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (PropertyUtil.SetClass(ref m_LineStyle, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 柱形图类型。
        /// </summary>
        public BarType barType
        {
            get { return m_BarType; }
            set { if (PropertyUtil.SetStruct(ref m_BarType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 柱形图是否为百分比堆积。相同stack的serie只要有一个barPercentStack为true，则就显示成百分比堆叠柱状图。
        /// </summary>
        public bool barPercentStack
        {
            get { return m_BarPercentStack; }
            set { if (PropertyUtil.SetStruct(ref m_BarPercentStack, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The width of the bar. Adaptive when default 0.
        /// |柱条的宽度，不设时自适应。支持设置成相对于类目宽度的百分比。
        /// </summary>
        public float barWidth
        {
            get { return m_BarWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BarWidth, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The gap between bars between different series, is a percent value like '0.3f' , which means 30% of the bar width, can be set as a fixed value.
        /// Set barGap as '-1' can overlap bars that belong to different series, which is useful when making a series of bar be background.
        /// In a single coodinate system, this attribute is shared by multiple 'bar' series.
        /// This attribute should be set on the last 'bar' series in the coodinate system, 
        /// then it will be adopted by all 'bar' series in the coordinate system.
        /// |不同系列的柱间距离。为百分比（如 '0.3f'，表示柱子宽度的 30%）
        /// 如果想要两个系列的柱子重叠，可以设置 barGap 为 '-1f'。这在用柱子做背景的时候有用。
        /// 在同一坐标系上，此属性会被多个 'bar' 系列共享。此属性应设置于此坐标系中最后一个 'bar' 系列上才会生效，并且是对此坐标系中所有 'bar' 系列生效。
        /// </summary>
        public float barGap
        {
            get { return m_BarGap; }
            set { if (PropertyUtil.SetStruct(ref m_BarGap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 斑马线的粗细。
        /// </summary>
        public float barZebraWidth
        {
            get { return m_BarZebraWidth; }
            set { if (PropertyUtil.SetStruct(ref m_BarZebraWidth, value < 0 ? 0 : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 斑马线的间距。
        /// </summary>
        public float barZebraGap
        {
            get { return m_BarZebraGap; }
            set { if (PropertyUtil.SetStruct(ref m_BarZebraGap, value < 0 ? 0 : value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// Whether offset when mouse click pie chart item.
        /// |鼠标点击时是否开启偏移，一般用在PieChart图表中。
        /// </summary>
        public bool pieClickOffset
        {
            get { return m_ClickOffset; }
            set { if (PropertyUtil.SetStruct(ref m_ClickOffset, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Whether to show as Nightingale chart.
        /// |是否展示成南丁格尔图，通过半径区分数据大小。
        /// </summary>
        public RoseType pieRoseType
        {
            get { return m_RoseType; }
            set { if (PropertyUtil.SetStruct(ref m_RoseType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// gap of item.
        /// |间距。
        /// </summary>
        public float gap
        {
            get { return m_Gap; }
            set { if (PropertyUtil.SetStruct(ref m_Gap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// the center of chart.
        /// |中心点。
        /// </summary>
        public float[] center
        {
            get { return m_Center; }
            set { if (value != null && value.Length == 2) { m_Center = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// the radius of chart.
        /// |半径。radius[0]表示内径，radius[1]表示外径。
        /// </summary>
        public float[] radius
        {
            get { return m_Radius; }
            set { if (value != null && value.Length == 2) { m_Radius = value; SetVerticesDirty(); } }
        }
        /// <summary>
        /// 最小值。
        /// </summary>
        public float min
        {
            get { return m_Min; }
            set { if (PropertyUtil.SetStruct(ref m_Min, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 最大值。
        /// </summary>
        public float max
        {
            get { return m_Max; }
            set { if (PropertyUtil.SetStruct(ref m_Max, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据最小值 min 映射的宽度。
        /// </summary>
        public float minSize
        {
            get { return m_MinSize; }
            set { if (PropertyUtil.SetStruct(ref m_MinSize, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据最大值 max 映射的宽度。
        /// </summary>
        public float maxSize
        {
            get { return m_MaxSize; }
            set { if (PropertyUtil.SetStruct(ref m_MaxSize, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 起始角度。和时钟一样，12点钟位置是0度，顺时针到360度。
        /// </summary>
        public float startAngle
        {
            get { return m_StartAngle; }
            set { if (PropertyUtil.SetStruct(ref m_StartAngle, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 结束角度。和时钟一样，12点钟位置是0度，顺时针到360度。
        /// </summary>
        public float endAngle
        {
            get { return m_EndAngle; }
            set { if (PropertyUtil.SetStruct(ref m_EndAngle, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The minimum angle of sector(0-360). It prevents some sector from being too small when value is small.
        /// |最小的扇区角度（0-360）。用于防止某个值过小导致扇区太小影响交互。
        /// </summary>
        public float minAngle
        {
            get { return m_MinAngle; }
            set { if (PropertyUtil.SetStruct(ref m_MinAngle, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否顺时针。
        /// </summary>
        public bool clockwise
        {
            get { return m_Clockwise; }
            set { if (PropertyUtil.SetStruct(ref m_Clockwise, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 刻度分割段数。最大可设置36。
        /// </summary>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtil.SetStruct(ref m_SplitNumber, value > 36 ? 36 : value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否开启圆弧效果。
        /// </summary>
        public bool roundCap
        {
            get { return m_RoundCap; }
            set { if (PropertyUtil.SetStruct(ref m_RoundCap, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 是否开启忽略数据。当为 true 时，数据值为 ignoreValue 时不进行绘制。
        /// </summary>
        public bool ignore
        {
            get { return m_Ignore; }
            set { if (PropertyUtil.SetStruct(ref m_Ignore, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 忽略数据的默认值。当ignore为true才有效。
        /// </summary>
        public double ignoreValue
        {
            get { return m_IgnoreValue; }
            set { if (PropertyUtil.SetStruct(ref m_IgnoreValue, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 忽略数据时折线是断开还是连接。默认false为连接。
        /// </summary>
        /// <value></value>
        public bool ignoreLineBreak
        {
            get { return m_IgnoreLineBreak; }
            set { if (PropertyUtil.SetStruct(ref m_IgnoreLineBreak, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 雷达图类型。
        /// </summary>
        public RadarType radarType
        {
            get { return m_RadarType; }
            set { if (PropertyUtil.SetStruct(ref m_RadarType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The start animation.
        /// |起始动画。
        /// </summary>
        public AnimationStyle animation
        {
            get { return m_Animation; }
            set { if (PropertyUtil.SetClass(ref m_Animation, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The style of data item.
        /// |图形样式。
        /// </summary>
        public ItemStyle itemStyle
        {
            get { return m_ItemStyle; }
            set { if (PropertyUtil.SetClass(ref m_ItemStyle, value, true)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 数据项里的数据维数。
        /// </summary>
        public int showDataDimension { get { return m_ShowDataDimension; } set { m_ShowDataDimension = Mathf.Clamp(2, 10, value); } }
        /// <summary>
        /// 在Editor的inpsector上是否显示name参数
        /// </summary>
        public bool showDataName { get { return m_ShowDataName; } set { m_ShowDataName = value; } }
        /// <summary>
        /// If clip the overflow on the coordinate system.
        /// |是否裁剪超出坐标系部分的图形。
        /// </summary>
        public bool clip
        {
            get { return m_Clip; }
            set { if (PropertyUtil.SetStruct(ref m_Clip, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Show negative number as positive number.
        /// |将负数数值显示为正数。一般和`AxisLabel`的`showAsPositiveNumber`配合使用。仅在折线图和柱状图中有效。
        /// </summary>
        public bool showAsPositiveNumber
        {
            get { return m_ShowAsPositiveNumber; }
            set { if (PropertyUtil.SetStruct(ref m_ShowAsPositiveNumber, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 是否开启大数据量优化，在数据图形特别多而出现卡顿时候可以开启。
        /// 开启后配合 largeThreshold 在数据量大于指定阈值的时候对绘制进行优化。
        /// 缺点：优化后不能自定义设置单个数据项的样式，不能显示Label。
        /// </summary>
        public bool large
        {
            get { return m_Large; }
            set { if (PropertyUtil.SetStruct(ref m_Large, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 开启大数量优化的阈值。只有当开启了large并且数据量大于该阀值时才进入性能模式。
        /// </summary>
        public int largeThreshold
        {
            get { return m_LargeThreshold; }
            set { if (PropertyUtil.SetStruct(ref m_LargeThreshold, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 在饼图且标签外部显示的情况下，是否启用防止标签重叠策略，默认关闭，在标签拥挤重叠的情况下会挪动各个标签的位置，防止标签间的重叠。
        /// </summary>
        public bool avoidLabelOverlap
        {
            get { return m_AvoidLabelOverlap; }
            set { if (PropertyUtil.SetStruct(ref m_AvoidLabelOverlap, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// Distance between component and the left side of the container.
        /// |组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtil.SetStruct(ref m_Left, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between component and the right side of the container.
        /// |组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtil.SetStruct(ref m_Right, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between component and the top side of the container.
        /// |组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtil.SetStruct(ref m_Top, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Distance between component and the bottom side of the container.
        /// |组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtil.SetStruct(ref m_Bottom, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether to add new data at the head or at the end of the list.
        /// |添加新数据时是在列表的头部还是尾部加入。
        /// </summary>
        public bool insertDataToHead
        {
            get { return m_InsertDataToHead; }
            set { if (PropertyUtil.SetStruct(ref m_InsertDataToHead, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 组件的数据排序。
        /// </summary>
        public SerieDataSortType dataSortType
        {
            get { return m_DataSortType; }
            set { if (PropertyUtil.SetStruct(ref m_DataSortType, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 组件的朝向。
        /// </summary>
        public Orient orient
        {
            get { return m_Orient; }
            set { if (PropertyUtil.SetStruct(ref m_Orient, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 组件水平方向对齐方式。
        /// </summary>
        public Align align
        {
            get { return m_Align; }
            set { if (PropertyUtil.SetStruct(ref m_Align, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 占位模式。占位模式时，数据有效但不参与渲染和显示。
        /// </summary>
        public bool placeHolder
        {
            get { return m_PlaceHolder; }
            set { if (PropertyUtil.SetStruct(ref m_PlaceHolder, value)) SetAllDirty(); }
        }
        /// <summary>
        /// 系列中的数据内容数组。SerieData可以设置1到n维数据。
        /// </summary>
        public List<SerieData> data { get { return m_Data; } }
        /// <summary>
        /// 取色策略是否为按数据项分配。
        /// </summary>
        public bool colorByData { get { return colorBy == SerieColorBy.Data; } }
        public override bool vertsDirty
        {
            get
            {
                return m_VertsDirty ||
                    symbol.vertsDirty ||
                    lineStyle.vertsDirty ||
                    itemStyle.vertsDirty ||
                    IsVertsDirty(lineArrow) ||
                    IsVertsDirty(areaStyle) ||
                    IsVertsDirty(label) ||
                    IsVertsDirty(labelLine) ||
                    IsVertsDirty(titleStyle) ||
                    IsVertsDirty(emphasisStyle) ||
                    IsVertsDirty(blurStyle) ||
                    IsVertsDirty(selectStyle) ||
                    AnySerieDataVerticesDirty();
            }
        }

        public override bool componentDirty
        {
            get
            {
                return m_ComponentDirty ||
                    symbol.componentDirty ||
                    IsComponentDirty(titleStyle) ||
                    IsComponentDirty(label) ||
                    IsComponentDirty(labelLine) ||
                    IsComponentDirty(emphasisStyle) ||
                    IsComponentDirty(blurStyle) ||
                    IsComponentDirty(selectStyle);
            }
        }
        public override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            if (!IsPerformanceMode())
            {
                foreach (var serieData in m_Data)
                    serieData.ClearVerticesDirty();
            }
            symbol.ClearVerticesDirty();
            lineStyle.ClearVerticesDirty();
            itemStyle.ClearVerticesDirty();
            ClearVerticesDirty(areaStyle);
            ClearVerticesDirty(label);
            ClearVerticesDirty(emphasisStyle);
            ClearVerticesDirty(blurStyle);
            ClearVerticesDirty(selectStyle);
            ClearVerticesDirty(lineArrow);
            ClearVerticesDirty(titleStyle);
        }

        public override void ClearComponentDirty()
        {
            base.ClearComponentDirty();
            if (!IsPerformanceMode())
            {
                foreach (var serieData in m_Data)
                    serieData.ClearComponentDirty();
            }
            symbol.ClearComponentDirty();
            lineStyle.ClearComponentDirty();
            itemStyle.ClearComponentDirty();
            ClearComponentDirty(areaStyle);
            ClearComponentDirty(label);
            ClearComponentDirty(emphasisStyle);
            ClearComponentDirty(blurStyle);
            ClearComponentDirty(selectStyle);
            ClearComponentDirty(lineArrow);
            ClearComponentDirty(titleStyle);
        }

        public override void SetAllDirty()
        {
            base.SetAllDirty();
            labelDirty = true;
            titleDirty = true;
        }

        private bool AnySerieDataVerticesDirty()
        {
            if (IsPerformanceMode())
                return false;
            if (this is ISimplifiedSerie)
                return false;
            foreach (var serieData in m_Data)
                if (serieData.vertsDirty) return true;
            return false;
        }

        private bool AnySerieDataComponentDirty()
        {
            if (IsPerformanceMode())
                return false;
            if (this is ISimplifiedSerie)
                return false;
            foreach (var serieData in m_Data)
                if (serieData.componentDirty) return true;
            return false;
        }
        /// <summary>
        /// Whether the serie is highlighted.
        /// |该系列是否高亮，一般由图例悬停触发。
        /// </summary>
        public bool highlight { get; internal set; }
        /// <summary>
        /// the count of data list.
        /// |数据项个数。
        /// </summary>
        public int dataCount { get { return m_Data.Count; } }
        public bool nameDirty { get { return m_NameDirty; } }
        public bool labelDirty { get; set; }
        public bool titleDirty { get; set; }
        public bool dataDirty { get; set; }

        private void SetSerieNameDirty()
        {
            m_NameDirty = true;
        }

        public void ClearSerieNameDirty()
        {
            m_NameDirty = false;
        }

        public override void ClearDirty()
        {
            base.ClearDirty();
        }

        /// <summary>
        /// 维度Y对应数据中最大值。
        /// </summary>
        public double yMax
        {
            get
            {
                var max = double.MinValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && !IsIgnoreValue(sdata.data[1]) && sdata.data[1] > max)
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
        public double xMax
        {
            get
            {
                var max = double.MinValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && !IsIgnoreValue(sdata.data[0]) && sdata.data[0] > max)
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
        public double yMin
        {
            get
            {
                var min = double.MaxValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && !IsIgnoreValue(sdata.data[1]) && sdata.data[1] < min)
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
        public double xMin
        {
            get
            {
                var min = double.MaxValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && !IsIgnoreValue(sdata.data[0]) && sdata.data[0] < min)
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
        public double yTotal
        {
            get
            {
                double total = 0;
                if (IsPerformanceMode())
                {
                    foreach (var sdata in data)
                    {
                        if (sdata.show && !IsIgnoreValue(sdata.data[1]))
                            total += sdata.data[1];
                    }
                }
                else
                {
                    var duration = animation.GetUpdateAnimationDuration();
                    foreach (var sdata in data)
                    {
                        if (sdata.show && !IsIgnoreValue(sdata.data[1]))
                            total += sdata.GetCurrData(1, duration);
                    }
                }
                return total;
            }
        }

        /// <summary>
        /// 维度X数据的总和。
        /// </summary>
        public double xTotal
        {
            get
            {
                double total = 0;
                foreach (var sdata in data)
                {
                    if (sdata.show && !IsIgnoreValue(sdata.data[1]))
                        total += sdata.data[0];
                }
                return total;
            }
        }

        public void ResetInteract()
        {
            interact.Reset();
            foreach (var serieData in m_Data)
                serieData.interact.Reset();
        }

        /// <summary>
        /// 重置数据项索引。避免部分数据项的索引异常。
        /// </summary>
        public bool ResetDataIndex()
        {
            var flag = false;
            for (int i = 0; i < m_Data.Count; i++)
            {
                if (m_Data[i].index != i)
                {
                    m_Data[i].index = i;
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public override void ClearData()
        {
            while (m_Data.Count > 0)
            {
                RemoveData(0);
            }
            m_Data.Clear();
            m_NeedUpdateFilterData = true;
            dataDirty = true;
            SetVerticesDirty();
        }

        /// <summary>
        /// 移除指定索引的数据
        /// </summary>
        /// <param name="index"></param>
        public void RemoveData(int index)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                if (!string.IsNullOrEmpty(m_Data[index].name))
                {
                    SetSerieNameDirty();
                }
                SetVerticesDirty();
                var serieData = m_Data[index];
                SerieDataPool.Release(serieData);
                if (serieData.labelObject != null)
                {
                    SerieLabelPool.Release(serieData.labelObject.gameObject);
                }
                m_Data.RemoveAt(index);
                m_NeedUpdateFilterData = true;
                labelDirty = true;
                dataDirty = true;
            }
        }

        /// <summary>
        /// 添加一个数据到维度Y（此时维度X对应的数据是索引）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataName"></param>
        /// <param name="dataId">the unique id of data</param>
        public SerieData AddYData(double value, string dataName = null, string dataId = null)
        {
            CheckMaxCache();
            int xValue = m_Data.Count;
            var serieData = SerieDataPool.Get();
            serieData.data.Add(xValue);
            serieData.data.Add(value);
            serieData.name = dataName;
            serieData.index = xValue;
            serieData.id = dataId;
            AddSerieData(serieData);
            m_ShowDataDimension = 2;
            SetVerticesDirty();
            CheckDataName(dataName);
            labelDirty = true;
            dataDirty = true;
            return serieData;
        }

        public void AddSerieData(SerieData serieData)
        {
            if (m_InsertDataToHead)
                m_Data.Insert(0, serieData);
            else
                m_Data.Add(serieData);
            SetVerticesDirty();
            dataDirty = true;
            m_NeedUpdateFilterData = true;
        }

        private void CheckDataName(string dataName)
        {
            if (string.IsNullOrEmpty(dataName))
                SetSerieNameDirty();
            else
                m_ShowDataName = true;
        }

        /// <summary>
        /// 添加（x，y）数据到维度X和维度Y
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        /// <param name="dataName"></param>
        /// <param name="dataId">the unique id of data</param>
        public SerieData AddXYData(double xValue, double yValue, string dataName = null, string dataId = null)
        {
            CheckMaxCache();
            var serieData = SerieDataPool.Get();
            serieData.data.Clear();
            serieData.data.Add(xValue);
            serieData.data.Add(yValue);
            serieData.name = dataName;
            serieData.index = m_Data.Count;
            serieData.id = dataId;
            AddSerieData(serieData);
            m_ShowDataDimension = 2;
            SetVerticesDirty();
            CheckDataName(dataName);
            labelDirty = true;
            return serieData;
        }

        /// <summary>
        /// 添加 (open, close, lowest, heighest) 数据
        /// </summary>
        /// <param name="open"></param>
        /// <param name="close"></param>
        /// <param name="lowest"></param>
        /// <param name="heighest"></param>
        /// <param name="dataName"></param>
        /// <param name="dataId">the unique id of data</param>
        /// <returns></returns>
        public SerieData AddData(double open, double close, double lowest, double heighest, string dataName = null, string dataId = null)
        {
            CheckMaxCache();
            var serieData = SerieDataPool.Get();
            serieData.data.Clear();
            serieData.data.Add(open);
            serieData.data.Add(close);
            serieData.data.Add(lowest);
            serieData.data.Add(heighest);
            serieData.name = dataName;
            serieData.index = m_Data.Count;
            serieData.id = dataId;
            AddSerieData(serieData);
            m_ShowDataDimension = 4;
            SetVerticesDirty();
            CheckDataName(dataName);
            labelDirty = true;
            return serieData;
        }

        /// <summary>
        /// 将一组数据添加到系列中。
        /// 如果数据只有一个，默认添加到维度Y中。
        /// </summary>
        /// <param name="valueList"></param>
        /// <param name="dataName"></param>
        /// <param name="dataId">the unique id of data</param>
        public SerieData AddData(List<double> valueList, string dataName = null, string dataId = null)
        {
            if (valueList == null || valueList.Count == 0) return null;
            if (valueList.Count == 1)
                return AddYData(valueList[0], dataName, dataId);
            else if (valueList.Count == 2)
                return AddXYData(valueList[0], valueList[1], dataName, dataId);
            else
            {
                CheckMaxCache();
                m_ShowDataDimension = valueList.Count;
                var serieData = SerieDataPool.Get();
                serieData.name = dataName;
                serieData.index = m_Data.Count;
                serieData.id = dataId;
                for (int i = 0; i < valueList.Count; i++)
                {
                    serieData.data.Add(valueList[i]);
                }
                AddSerieData(serieData);
                SetVerticesDirty();
                CheckDataName(dataName);
                labelDirty = true;
                return serieData;
            }
        }

        public SerieData AddChildData(SerieData parent, double value, string name, string id)
        {
            var serieData = new SerieData();
            serieData.name = name;
            serieData.index = m_Data.Count;
            serieData.id = id;
            serieData.data = new List<double>() { m_Data.Count, value };
            AddChildData(parent, serieData);
            return serieData;
        }

        public SerieData AddChildData(SerieData parent, List<double> value, string name, string id)
        {
            var serieData = new SerieData();
            serieData.name = name;
            serieData.index = m_Data.Count;
            serieData.id = id;
            serieData.data = new List<double>(value);
            AddChildData(parent, serieData);
            return serieData;
        }

        public void AddChildData(SerieData parent, SerieData serieData)
        {
            serieData.parentId = parent.id;
            serieData.context.parent = parent;

            if (!m_Data.Contains(serieData))
                AddSerieData(serieData);

            if (!parent.context.children.Contains(serieData))
            {
                parent.context.children.Add(serieData);
            }
        }

        private void CheckMaxCache()
        {
            if (m_MaxCache <= 0) return;
            while (m_Data.Count >= m_MaxCache)
            {
                m_NeedUpdateFilterData = true;
                if (m_InsertDataToHead) RemoveData(m_Data.Count - 1);
                else RemoveData(0);
            }
        }

        /// <summary>
        /// 获得指定index指定维数的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dimension"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public double GetData(int index, int dimension, DataZoom dataZoom = null)
        {
            if (index < 0 || dimension < 0) return 0;
            var serieData = GetSerieData(index, dataZoom);
            if (serieData != null && dimension < serieData.data.Count)
            {
                var value = serieData.GetData(dimension);
                if (showAsPositiveNumber)
                    value = Math.Abs(value);
                return value;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获得维度Y索引对应的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public double GetYData(int index, DataZoom dataZoom = null)
        {
            if (index < 0) return 0;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                var value = serieData[index].data[1];
                if (showAsPositiveNumber)
                    value = Math.Abs(value);
                return value;
            }
            return 0;
        }

        public double GetYCurrData(int index, DataZoom dataZoom = null)
        {
            if (index < 0) return 0;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                var value = serieData[index].GetCurrData(1, animation.GetUpdateAnimationDuration());
                if (showAsPositiveNumber)
                    value = Math.Abs(value);
                return value;
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
        public void GetYData(int index, out double yData, out string dataName, DataZoom dataZoom = null)
        {
            yData = 0;
            dataName = null;
            if (index < 0) return;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                yData = serieData[index].data[1];
                if (showAsPositiveNumber)
                    yData = Math.Abs(yData);
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
                return data[index];
            return null;
        }

        public SerieData GetSerieData(string id, DataZoom dataZoom = null)
        {
            var data = GetDataList(dataZoom);
            foreach (var serieData in data)
            {
                var target = GetSerieData(serieData, id);
                if (target != null) return target;
            }
            return null;
        }

        public SerieData GetSerieData(SerieData parent, string id)
        {
            if (id.Equals(parent.id)) return parent;
            foreach (var child in parent.context.children)
            {
                var data = GetSerieData(child, id);
                if (data != null)
                {
                    return data;
                }
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
        public void GetXYData(int index, DataZoom dataZoom, out double xValue, out double yVlaue)
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
                if (showAsPositiveNumber)
                {
                    xValue = Math.Abs(xValue);
                    yVlaue = Math.Abs(yVlaue);
                }
            }
        }

        public virtual double GetDataTotal(int dimension, SerieData serieData = null)
        {
            if (m_Max > 0) return m_Max;

            double total = 0;
            foreach (var sdata in data)
            {
                if (sdata.show)
                    total += sdata.GetData(dimension);
            }
            return total;
        }

        /// <summary>
        /// 获得系列的数据列表
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public List<SerieData> GetDataList(DataZoom dataZoom = null)
        {
            if (dataZoom != null && dataZoom.enable &&
                (dataZoom.IsContainsXAxis(xAxisIndex) || dataZoom.IsContainsYAxis(yAxisIndex)))
            {
                SerieHelper.UpdateFilterData(this, dataZoom);
                return m_FilterData;
            }
            else
            {
                return useSortData && context.sortedData.Count > 0 ? context.sortedData : m_Data;
            }
        }

        /// <summary>
        /// 更新指定索引的维度Y数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public bool UpdateYData(int index, double value)
        {
            UpdateData(index, 1, value);
            return true;
        }

        /// <summary>
        /// 更新指定索引的维度X和维度Y的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public bool UpdateXYData(int index, float xValue, float yValue)
        {
            var flag1 = UpdateData(index, 0, xValue);
            var flag2 = UpdateData(index, 1, yValue);
            return flag1 || flag2;
        }

        /// <summary>
        /// 更新指定索引指定维数的数据
        /// </summary>
        /// <param name="index">要更新数据的索引</param>
        /// <param name="dimension">要更新数据的维数</param>
        /// <param name="value">新的数据值</param>
        public bool UpdateData(int index, int dimension, double value)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                var animationOpen = animation.enable;
                var animationDuration = animation.GetUpdateAnimationDuration();
                var flag = m_Data[index].UpdateData(dimension, value, animationOpen, animationDuration);
                if (flag)
                {
                    SetVerticesDirty();
                    dataDirty = true;
                }
                return flag;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新指定索引的数据项数据列表
        /// </summary>
        /// <param name="index"></param>
        /// <param name="values"></param>
        public bool UpdateData(int index, List<double> values)
        {
            if (index >= 0 && index < m_Data.Count && values != null)
            {
                var serieData = m_Data[index];
                var animationOpen = animation.enable;
                var animationDuration = animation.GetUpdateAnimationDuration();
                for (int i = 0; i < values.Count; i++)
                    serieData.UpdateData(i, values[i], animationOpen, animationDuration);
                SetVerticesDirty();
                dataDirty = true;
                return true;
            }
            return false;
        }

        public bool UpdateDataName(int index, string name)
        {
            if (index >= 0 && index < m_Data.Count)
            {
                var serieData = m_Data[index];
                serieData.name = name;
                SetSerieNameDirty();
                if (serieData.labelObject != null)
                {
                    serieData.labelObject.SetText(name == null ? "" : name);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 清除所有数据的高亮标志
        /// </summary>
        public void ClearHighlight()
        {
            highlight = false;
            foreach (var serieData in m_Data)
                serieData.context.highlight = false;
        }

        /// <summary>
        /// 设置指定索引的数据为高亮状态
        /// </summary>
        public void SetHighlight(int index, bool flag)
        {
            var serieData = GetSerieData(index);
            if (serieData != null)
                serieData.context.highlight = flag;
        }

        public float GetBarWidth(float categoryWidth, int barCount = 0)
        {
            if (m_BarWidth == 0)
            {
                var width = ChartHelper.GetActualValue(0.6f, categoryWidth);
                if (barCount == 0)
                    return width < 1 ? categoryWidth : width;
                else
                    return width / barCount;
            }
            else
                return ChartHelper.GetActualValue(m_BarWidth, categoryWidth);
        }

        public bool IsIgnoreIndex(int index, int dimension = 1)
        {
            var serieData = GetSerieData(index);
            if (serieData != null)
                return IsIgnoreValue(serieData, dimension);
            return false;
        }

        public bool IsIgnoreValue(SerieData serieData, int dimension = 1)
        {
            return serieData.ignore || IsIgnoreValue(serieData.GetData(dimension));
        }

        public bool IsIgnoreValue(double value)
        {
            return m_Ignore && MathUtil.Approximately(value, m_IgnoreValue);
        }

        public bool IsIgnorePoint(int index)
        {
            if (index >= 0 && index < dataCount)
            {
                return ChartHelper.IsIngore(data[index].context.position);
            }
            return false;
        }

        public bool IsSerie<T>() where T : Serie
        {
            return this is T;
        }

        public bool IsUseCoord<T>() where T : CoordSystem
        {
            return ChartCached.GetTypeName<T>().Equals(m_CoordSystem);
        }

        public bool SetCoord<T>() where T : CoordSystem
        {
            if (GetType().IsDefined(typeof(CoordOptionsAttribute), false))
            {
                var attribute = GetType().GetAttribute<CoordOptionsAttribute>();
                if (attribute.Contains<T>())
                {
                    m_CoordSystem = typeof(T).Name;
                    return true;
                }
            }
            Debug.LogError("not support coord system:" + typeof(T));
            return false;
        }

        /// <summary>
        /// 是否为性能模式。性能模式下不绘制Symbol，不刷新Label，不单独设置数据项配置。
        /// </summary>
        public bool IsPerformanceMode()
        {
            return m_Large && m_Data.Count >= m_LargeThreshold;
        }

        public bool IsLegendName(string legendName)
        {
            if (colorBy == SerieColorBy.Data)
            {
                return IsSerieDataLegendName(legendName) || IsSerieLegendName(legendName);
            }
            else
            {
                return IsSerieLegendName(legendName);
            }
        }

        public bool IsSerieLegendName(string legendName)
        {
            return legendName.Equals(this.legendName);
        }

        public bool IsSerieDataLegendName(string legendName)
        {
            foreach (var serieData in m_Data)
            {
                if (legendName.Equals(serieData.legendName))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 启用或取消初始动画
        /// </summary>
        public void AnimationEnable(bool flag)
        {
            if (animation.enable) animation.enable = flag;
            SetVerticesDirty();
        }

        /// <summary>
        /// 渐入动画
        /// </summary>
        public void AnimationFadeIn()
        {
            if (animation.enable) animation.FadeIn();
            SetVerticesDirty();
        }

        /// <summary>
        /// 渐出动画
        /// </summary>
        public void AnimationFadeOut()
        {
            if (animation.enable) animation.FadeOut();
            SetVerticesDirty();
        }

        /// <summary>
        /// 暂停动画
        /// </summary>
        public void AnimationPause()
        {
            if (animation.enable) animation.Pause();
            SetVerticesDirty();
        }

        /// <summary>
        /// 继续动画
        /// </summary>
        public void AnimationResume()
        {
            if (animation.enable) animation.Resume();
            SetVerticesDirty();
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        public void AnimationReset()
        {
            if (animation.enable) animation.Reset();
            SetVerticesDirty();
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        public void AnimationRestart()
        {
            if (animation.enable) animation.Restart();
            SetVerticesDirty();
        }

        public int CompareTo(object obj)
        {
            return index.CompareTo((obj as Serie).index);
        }

        public T Clone<T>() where T : Serie
        {
            var newSerie = Activator.CreateInstance<T>();
            SerieHelper.CopySerie(this, newSerie);
            return newSerie;
        }

        public Serie Clone()
        {
            var newSerie = Activator.CreateInstance(GetType()) as Serie;
            SerieHelper.CopySerie(this, newSerie);
            return newSerie;
        }
    }
}