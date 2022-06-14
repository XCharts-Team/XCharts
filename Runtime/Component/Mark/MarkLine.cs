using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 标线类型
    /// </summary>
    public enum MarkLineType
    {
        None,
        /// <summary>
        /// 最小值。
        /// </summary>
        Min,
        /// <summary>
        /// 最大值。
        /// </summary>
        Max,
        /// <summary>
        /// 平均值。
        /// </summary>
        Average,
        /// <summary>
        /// 中位数。
        /// </summary>
        Median
    }

    /// <summary>
    /// Use a line in the chart to illustrate.
    /// |图表标线。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(MarkLineHandler), true)]
    public class MarkLine : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private int m_SerieIndex = 0;
        [SerializeField] private AnimationStyle m_Animation = new AnimationStyle();
        [SerializeField] private List<MarkLineData> m_Data = new List<MarkLineData>();

        /// <summary>
        /// Whether to display the marking line.
        /// |是否显示标线。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The serie index of markLine.
        /// |标线影响的Serie索引。
        /// </summary>
        public int serieIndex
        {
            get { return m_SerieIndex; }
            set { if (PropertyUtil.SetStruct(ref m_SerieIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The animation of markline.
        /// |标线的动画样式。
        /// </summary>
        public AnimationStyle animation
        {
            get { return m_Animation; }
            set { if (PropertyUtil.SetClass(ref m_Animation, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// A list of marked data. When the group of data item is 0, each data item represents a line; 
        /// When the group is not 0, two data items of the same group represent the starting point and 
        /// the ending point of the line respectively to form a line. In this case, the relevant style 
        /// parameters of the line are the parameters of the starting point.
        /// |标线的数据列表。当数据项的group为0时，每个数据项表示一条标线；当group不为0时，相同group的两个数据项分别表
        /// 示标线的起始点和终止点来组成一条标线，此时标线的相关样式参数取起始点的参数。
        /// </summary>
        public List<MarkLineData> data
        {
            get { return m_Data; }
            set { if (PropertyUtil.SetClass(ref m_Data, value)) SetVerticesDirty(); }
        }

        public override void SetDefaultValue()
        {
            data.Clear();
            var item = new MarkLineData();
            item.name = "average";
            item.type = MarkLineType.Average;
            item.lineStyle.type = LineStyle.Type.Dashed;
            item.lineStyle.color = Color.clear;
            item.startSymbol.show = true;
            item.startSymbol.type = SymbolType.Circle;
            item.startSymbol.size = 4;
            item.endSymbol.show = true;
            item.endSymbol.type = SymbolType.Arrow;
            item.endSymbol.size = 5;
            item.label.show = true;
            item.label.numericFormatter = "f1";
            item.label.formatter = "{c}";
            data.Add(item);
        }
    }
    /// <summary>
    /// Data of marking line. 
    /// |图表标线的数据。
    /// </summary>
    [System.Serializable]
    public class MarkLineData : ChildComponent
    {
        [SerializeField] private MarkLineType m_Type = MarkLineType.None;
        [SerializeField] private string m_Name;
        [SerializeField] private int m_Dimension = 1;
        [SerializeField] private float m_XPosition;
        [SerializeField] private float m_YPosition;
        [SerializeField] private double m_XValue;
        [SerializeField] private double m_YValue;
        [SerializeField] private int m_Group = 0;
        [SerializeField] private bool m_ZeroPosition = false;

        [SerializeField] private SymbolStyle m_StartSymbol = new SymbolStyle();
        [SerializeField] private SymbolStyle m_EndSymbol = new SymbolStyle();
        [SerializeField] private LineStyle m_LineStyle = new LineStyle();
        [SerializeField] private LabelStyle m_Label = new LabelStyle();
        //[SerializeField] private Emphasis m_Emphasis = new Emphasis();

        public Vector3 runtimeStartPosition { get; internal set; }
        public Vector3 runtimeEndPosition { get; internal set; }
        public Vector3 runtimeCurrentEndPosition { get; internal set; }
        public ChartLabel runtimeLabel { get; internal set; }
        public double runtimeValue { get; internal set; }

        /// <summary>
        /// Name of the marker, which will display as a label.
        /// |标线名称，将会作为文字显示。label的formatter可通过{b}显示名称，通过{c}显示数值。
        /// </summary>
        public string name
        {
            get { return m_Name; }
            set { if (PropertyUtil.SetClass(ref m_Name, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Special label types, are used to label maximum value, minimum value and so on.
        /// |特殊的标线类型，用于标注最大值最小值等。
        /// </summary>
        public MarkLineType type
        {
            get { return m_Type; }
            set { if (PropertyUtil.SetStruct(ref m_Type, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// From which dimension of data to calculate the maximum and minimum value and so on.
        /// |从哪个维度的数据计算最大最小值等。
        /// </summary>
        public int dimension
        {
            get { return m_Dimension; }
            set { if (PropertyUtil.SetStruct(ref m_Dimension, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The x coordinate relative to the origin, in pixels.
        /// |相对原点的 x 坐标，单位像素。当type为None时有效。
        /// </summary>
        public float xPosition
        {
            get { return m_XPosition; }
            set { if (PropertyUtil.SetStruct(ref m_XPosition, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The y coordinate relative to the origin, in pixels.
        /// |相对原点的 y 坐标，单位像素。当type为None时有效。
        /// </summary>
        public float yPosition
        {
            get { return m_YPosition; }
            set { if (PropertyUtil.SetStruct(ref m_YPosition, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The value specified on the X-axis. A value specified when the X-axis is the category axis represents the index of the category axis data, otherwise a specific value.
        /// |X轴上的指定值。当X轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
        /// </summary>
        public double xValue
        {
            get { return m_XValue; }
            set { if (PropertyUtil.SetStruct(ref m_XValue, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// That's the value on the Y-axis. The value specified when the Y axis is the category axis represents the index of the category axis data, otherwise the specific value.
        /// |Y轴上的指定值。当Y轴为类目轴时指定值表示类目轴数据的索引，否则为具体的值。当type为None时有效。
        /// </summary>
        public double yValue
        {
            get { return m_YValue; }
            set { if (PropertyUtil.SetStruct(ref m_YValue, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Grouping. When the group is not 0, it means that this data is the starting point or end point of the marking line. Data consistent with the group form a marking line.
        /// |分组。当group不为0时，表示这个data是标线的起点或终点，group一致的data组成一条标线。
        /// </summary>
        public int group
        {
            get { return m_Group; }
            set { if (PropertyUtil.SetStruct(ref m_Group, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Is the origin of the coordinate system.
        /// |是否为坐标系原点。
        /// </summary>
        public bool zeroPosition
        {
            get { return m_ZeroPosition; }
            set { if (PropertyUtil.SetStruct(ref m_ZeroPosition, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The symbol of the start point of markline.
        /// |起始点的图形标记。
        /// </summary>
        public SymbolStyle startSymbol
        {
            get { return m_StartSymbol; }
            set { if (PropertyUtil.SetClass(ref m_StartSymbol, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The symbol of the end point of markline.
        /// |结束点的图形标记。
        /// </summary>
        public SymbolStyle endSymbol
        {
            get { return m_EndSymbol; }
            set { if (PropertyUtil.SetClass(ref m_EndSymbol, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The line style of markline.
        /// |标线样式。
        /// </summary>
        public LineStyle lineStyle
        {
            get { return m_LineStyle; }
            set { if (PropertyUtil.SetClass(ref m_LineStyle, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Text styles of label. You can set position to Start, Middle, and End to display text in different locations.
        /// |文本样式。可设置position为Start、Middle和End在不同的位置显示文本。
        /// </summary>
        public LabelStyle label
        {
            get { return m_Label; }
            set { if (PropertyUtil.SetClass(ref m_Label, value)) SetVerticesDirty(); }
        }
        // public Emphasis emphasis
        // {
        //     get { return m_Emphasis; }
        //     set { if (PropertyUtil.SetClass(ref m_Emphasis, value)) SetVerticesDirty(); }
        // }
    }
}