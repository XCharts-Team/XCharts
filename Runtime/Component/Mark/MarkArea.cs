using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// 标域类型
    /// </summary>
    public enum MarkAreaType
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
    /// Used to mark an area in chart. For example, mark a time interval.
    /// |图表标域，常用于标记图表中某个范围的数据。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(MarkAreaHandler), true)]
    public class MarkArea : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private string m_Text = "";
        [SerializeField] private int m_SerieIndex = 0;
        [SerializeField] private MarkAreaData m_Start = new MarkAreaData();
        [SerializeField] private MarkAreaData m_End = new MarkAreaData();
        [SerializeField] private ItemStyle m_ItemStyle = new ItemStyle();
        [SerializeField] private LabelStyle m_Label = new LabelStyle();
        public ChartLabel runtimeLabel { get; internal set; }
        public Vector3 runtimeLabelPosition { get; internal set; }
        public Rect runtimeRect { get; internal set; }
        /// <summary>
        /// 是否显示标域。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// The text of markArea.
        /// 标域显示的文本。
        /// </summary>
        public string text
        {
            get { return m_Text; }
            set { if (PropertyUtil.SetClass(ref m_Text, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Serie index of markArea.
        /// 标域影响的Serie索引。
        /// </summary>
        public int serieIndex
        {
            get { return m_SerieIndex; }
            set { if (PropertyUtil.SetStruct(ref m_SerieIndex, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 标域范围的起始数据。
        /// </summary>
        public MarkAreaData start
        {
            get { return m_Start; }
            set { if (PropertyUtil.SetClass(ref m_Start, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 标域范围的结束数据。
        /// </summary>
        public MarkAreaData end
        {
            get { return m_End; }
            set { if (PropertyUtil.SetClass(ref m_End, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 标域样式。
        /// </summary>
        public ItemStyle itemStyle
        {
            get { return m_ItemStyle; }
            set { if (PropertyUtil.SetClass(ref m_ItemStyle, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// 标域文本样式。
        /// </summary>
        public LabelStyle label
        {
            get { return m_Label; }
            set { if (PropertyUtil.SetClass(ref m_Label, value)) SetComponentDirty(); }
        }
        public override void SetDefaultValue()
        {
            m_ItemStyle = new ItemStyle();
            m_ItemStyle.opacity = 0.6f;
            m_Label = new LabelStyle();
            m_Label.show = true;
        }
    }

    /// <summary>
    /// 标域的数据。
    /// </summary>
    [System.Serializable]
    public class MarkAreaData : ChildComponent
    {
        [SerializeField] private MarkAreaType m_Type = MarkAreaType.None;
        [SerializeField] private string m_Name;
        [SerializeField] private int m_Dimension = 1;
        [SerializeField] private float m_XPosition;
        [SerializeField] private float m_YPosition;
        [SerializeField] private double m_XValue;
        [SerializeField] private double m_YValue;
        public double runtimeValue { get; internal set; }
        /// <summary>
        /// Name of the marker, which will display as a label.
        /// |标注名称。会作为文字显示。
        /// </summary>
        public string name
        {
            get { return m_Name; }
            set { if (PropertyUtil.SetClass(ref m_Name, value)) SetVerticesDirty(); }
        }
        /// <summary>
        /// Special markArea types, are used to label maximum value, minimum value and so on.
        /// |特殊的标域类型，用于标注最大值最小值等。
        /// </summary>
        public MarkAreaType type
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
    }
}