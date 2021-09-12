/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// Radar coordinate conponnet for radar charts. 
    /// 雷达图坐标系组件，只适用于雷达图。
    /// </summary>
    [System.Serializable]
    public class Radar : MainComponent
    {
        /// <summary>
        /// Radar render type, in which 'Polygon' and 'Circle' are supported.
        /// 雷达图绘制类型，支持 'Polygon' 和 'Circle'。
        /// </summary>
        public enum Shape
        {
            Polygon,
            Circle
        }
        /// <summary>
        /// The position type of radar.
        /// 显示位置。
        /// </summary>
        public enum PositionType
        {
            /// <summary>
            /// Display at the vertex.
            /// 显示在顶点处。
            /// </summary>
            Vertice,
            /// <summary>
            /// Display at the middle of line.
            /// 显示在两者之间。
            /// </summary>
            Between,
        }
        /// <summary>
        /// Indicator of radar chart, which is used to assign multiple variables(dimensions) in radar chart. 
        /// 雷达图的指示器，用来指定雷达图中的多个变量（维度）。
        /// </summary>
        [System.Serializable]
        public class Indicator
        {
            [SerializeField] private string m_Name;
            [SerializeField] private double m_Max;
            [SerializeField] private double m_Min;
            [SerializeField] private double[] m_Range = new double[2] { 0, 0 };
            [SerializeField] private TextStyle m_TextStyle = new TextStyle();

            /// <summary>
            /// The name of indicator.
            /// 指示器名称。
            /// </summary>
            public string name { get { return FormatterHelper.TrimAndReplaceLine(m_Name); } set { m_Name = value; } }
            /// <summary>
            /// The maximum value of indicator, with default value of 0, but we recommend to set it manually.
            /// 指示器的最大值，默认为 0 无限制。
            /// </summary>
            public double max { get { return m_Max; } set { m_Max = value; } }
            /// <summary>
            /// The minimum value of indicator, with default value of 0.
            /// 指示器的最小值，默认为 0 无限制。
            /// </summary>
            public double min { get { return m_Min; } set { m_Min = value; } }
            /// <summary>
            /// the style of text.
            /// 文本样式。
            /// </summary>
            public TextStyle textStyle { get { return m_TextStyle; } set { m_TextStyle = value; } }
            /// <summary>
            /// the text conponent of indicator.
            /// 指示器的文本组件。
            /// </summary>
            public Text text { get; set; }
            /// <summary>
            /// Normal range. When the value is outside this range, the display color is automatically changed.
            /// 正常值范围。当数值不在这个范围时，会自动变更显示颜色。
            /// </summary>
            public double[] range
            {
                get { return m_Range; }
                set { if (value != null && value.Length == 2) { m_Range = value; } }
            }

            public bool IsInRange(double value)
            {
                if (m_Range == null || m_Range.Length < 2) return true;
                if (m_Range[0] != 0 || m_Range[1] != 0)
                    return value >= m_Range[0] && value <= m_Range[1];
                else
                    return true;
            }
        }
        [SerializeField] private bool m_Show;
        [SerializeField] private Shape m_Shape;
        [SerializeField] private float m_Radius = 100;
        [SerializeField] private int m_SplitNumber = 5;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private AxisLine m_AxisLine = AxisLine.defaultAxisLine;
        [SerializeField] private AxisSplitLine m_SplitLine = AxisSplitLine.defaultSplitLine;
        [SerializeField] private AxisSplitArea m_SplitArea = AxisSplitArea.defaultSplitArea;
        [SerializeField] private bool m_Indicator = true;
        [SerializeField] private PositionType m_PositionType = PositionType.Vertice;
        [SerializeField] private float m_IndicatorGap = 10;
        [SerializeField] private int m_CeilRate = 0;
        [SerializeField] private bool m_IsAxisTooltip;
        [SerializeField] private Color32 m_OutRangeColor = Color.red;
        [SerializeField] private bool m_ConnectCenter = false;
        [SerializeField] private bool m_LineGradient = true;
        [SerializeField] private List<Indicator> m_IndicatorList = new List<Indicator>();
        /// <summary>
        /// [default:true]
        /// Set this to false to prevent the radar from showing.
        /// 是否显示雷达坐标系组件。
        /// </summary>
        public bool show { get { return m_Show; } set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); } }
        /// <summary>
        /// Radar render type, in which 'Polygon' and 'Circle' are supported.
        /// 雷达图绘制类型，支持 'Polygon' 和 'Circle'。
        /// </summary>
        /// <value></value>
        public Shape shape
        {
            get { return m_Shape; }
            set { if (PropertyUtil.SetStruct(ref m_Shape, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the radius of radar.
        /// 雷达图的半径。
        /// </summary>
        public float radius
        {
            get { return m_Radius; }
            set { if (PropertyUtil.SetStruct(ref m_Radius, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Segments of indicator axis.
        /// 指示器轴的分割段数。
        /// </summary>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtil.SetStruct(ref m_SplitNumber, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the center of radar chart.
        /// 雷达图的中心点。数组的第一项是横坐标，第二项是纵坐标。
        /// 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
        /// </summary>
        public float[] center
        {
            get { return m_Center; }
            set { if (value != null) { m_Center = value; SetAllDirty(); } }
        }
        /// <summary>
        /// axis line.
        /// 轴线。
        /// </summary>
        public AxisLine axisLine
        {
            get { return m_AxisLine; }
            set { if (PropertyUtil.SetClass(ref m_AxisLine, value, true)) SetAllDirty(); }
        }
        /// <summary>
        /// split line.
        /// 分割线。
        /// </summary>
        public AxisSplitLine splitLine
        {
            get { return m_SplitLine; }
            set { if (PropertyUtil.SetClass(ref m_SplitLine, value, true)) SetAllDirty(); }
        }
        /// <summary>
        /// Split area of axis in grid area.
        /// 分割区域。
        /// </summary>
        public AxisSplitArea splitArea
        {
            get { return m_SplitArea; }
            set { if (PropertyUtil.SetClass(ref m_SplitArea, value, true)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether to show indicator.
        /// 是否显示指示器。
        /// </summary>
        public bool indicator
        {
            get { return m_Indicator; }
            set { if (PropertyUtil.SetStruct(ref m_Indicator, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The gap of indicator and radar.
        /// 指示器和雷达的间距。
        /// </summary>
        public float indicatorGap
        {
            get { return m_IndicatorGap; }
            set { if (PropertyUtil.SetStruct(ref m_IndicatorGap, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// The ratio of maximum and minimum values rounded upward. The default is 0, which is automatically calculated.
        /// 最大最小值向上取整的倍率。默认为0时自动计算。
        /// </summary>
        public int ceilRate
        {
            get { return m_CeilRate; }
            set { if (PropertyUtil.SetStruct(ref m_CeilRate, value < 0 ? 0 : value)) SetAllDirty(); }
        }
        /// <summary>
        /// 是否Tooltip显示轴线上的所有数据。
        /// </summary>
        public bool isAxisTooltip
        {
            get { return m_IsAxisTooltip; }
            set { if (PropertyUtil.SetStruct(ref m_IsAxisTooltip, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The position type of indicator.
        /// 显示位置类型。
        /// </summary>
        public PositionType positionType
        {
            get { return m_PositionType; }
            set { if (PropertyUtil.SetStruct(ref m_PositionType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// The color displayed when data out of range.
        /// 数值超出范围时显示的颜色。
        /// </summary>
        public Color32 outRangeColor
        {
            get { return m_OutRangeColor; }
            set { if (PropertyUtil.SetStruct(ref m_OutRangeColor, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether serie data connect to radar center with line.
        /// 数值是否连线到中心点。
        /// </summary>
        public bool connectCenter
        {
            get { return m_ConnectCenter; }
            set { if (PropertyUtil.SetStruct(ref m_ConnectCenter, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether need gradient for data line.
        /// 数值线段是否需要渐变。
        /// </summary>
        public bool lineGradient
        {
            get { return m_LineGradient; }
            set { if (PropertyUtil.SetStruct(ref m_LineGradient, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the indicator list.
        /// 指示器列表。
        /// </summary>
        public List<Indicator> indicatorList { get { return m_IndicatorList; } }

        public int index { get; internal set; }
        /// <summary>
        /// the center position of radar in container.
        /// 雷达图在容器中的具体中心点。
        /// </summary>
        public Vector3 runtimeCenterPos { get; internal set; }
        /// <summary>
        /// the true radius of radar.
        /// 雷达图的运行时实际半径。
        /// </summary>
        public float runtimeRadius { get; internal set; }
        public float runtimeDataRadius { get; internal set; }
        /// <summary>
        /// the data position list of radar.
        /// 雷达图的所有数据坐标点列表。
        /// </summary>
        public Dictionary<int, List<Vector3>> runtimeDataPosList = new Dictionary<int, List<Vector3>>();

        public static Radar defaultRadar
        {
            get
            {
                var radar = new Radar
                {
                    m_Show = true,
                    m_Shape = Shape.Polygon,
                    m_Radius = 0.35f,
                    m_SplitNumber = 5,
                    m_Indicator = true,
                    m_IndicatorList = new List<Indicator>(5){
                        new Indicator(){name="indicator1",max = 0},
                        new Indicator(){name="indicator2",max = 0},
                        new Indicator(){name="indicator3",max = 0},
                        new Indicator(){name="indicator4",max = 0},
                        new Indicator(){name="indicator5",max = 0},
                    }
                };
                radar.center[0] = 0.5f;
                radar.center[1] = 0.4f;
                radar.splitLine.show = true;
                radar.splitArea.show = true;
                return radar;
            }
        }

        private bool IsEqualsIndicatorList(List<Indicator> indicators1, List<Indicator> indicators2)
        {
            if (indicators1.Count != indicators2.Count) return false;
            for (int i = 0; i < indicators1.Count; i++)
            {
                var indicator1 = indicators1[i];
                var indicator2 = indicators2[i];
                if (!indicator1.Equals(indicator2)) return false;
            }
            return true;
        }

        public bool IsInIndicatorRange(int index, double value)
        {
            var indicator = GetIndicator(index);
            return indicator == null ? true : indicator.IsInRange(value);
        }

        public double GetIndicatorMin(int index)
        {
            if (index >= 0 && index < m_IndicatorList.Count)
            {
                return m_IndicatorList[index].min;
            }
            return 0;
        }
        public double GetIndicatorMax(int index)
        {
            if (index >= 0 && index < m_IndicatorList.Count)
            {
                return m_IndicatorList[index].max;
            }
            return 0;
        }

        internal void UpdateRadarCenter(Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (center.Length < 2) return;
            var centerX = center[0] <= 1 ? chartWidth * center[0] : center[0];
            var centerY = center[1] <= 1 ? chartHeight * center[1] : center[1];
            runtimeCenterPos = chartPosition + new Vector3(centerX, centerY);
            if (radius <= 0)
            {
                runtimeRadius = 0;
            }
            else if (radius <= 1)
            {
                runtimeRadius = Mathf.Min(chartWidth, chartHeight) * radius;
            }
            else
            {
                runtimeRadius = radius;
            }
            if (shape == Radar.Shape.Polygon && positionType == PositionType.Between)
            {
                var angle = Mathf.PI / indicatorList.Count;
                runtimeDataRadius = runtimeRadius * Mathf.Cos(angle);
            }
            else
            {
                runtimeDataRadius = runtimeRadius;
            }
        }

        public Vector3 GetIndicatorPosition(int index)
        {
            int indicatorNum = indicatorList.Count;
            var angle = 0f;
            switch (positionType)
            {
                case PositionType.Vertice:
                    angle = 2 * Mathf.PI / indicatorNum * index;
                    break;
                case PositionType.Between:
                    angle = 2 * Mathf.PI / indicatorNum * (index + 0.5f);
                    break;
            }
            var x = runtimeCenterPos.x + (runtimeRadius + indicatorGap) * Mathf.Sin(angle);
            var y = runtimeCenterPos.y + (runtimeRadius + indicatorGap) * Mathf.Cos(angle);
            return new Vector3(x, y);
        }

        public void AddIndicator(Radar.Indicator indicator)
        {
            indicatorList.Add(indicator);
            SetAllDirty();
        }

        public Radar.Indicator AddIndicator(string name, float min, float max)
        {
            var indicator = new Radar.Indicator();
            indicator.name = name;
            indicator.min = min;
            indicator.max = max;
            indicatorList.Add(indicator);
            SetAllDirty();
            return indicator;
        }

        public bool UpdateIndicator(int indicatorIndex, string name, float min, float max)
        {
            var indicator = GetIndicator(indicatorIndex);
            if (indicator == null) return false;
            indicator.name = name;
            indicator.min = min;
            indicator.max = max;
            SetAllDirty();
            return true;
        }

        public Radar.Indicator GetIndicator(int indicatorIndex)
        {
            if (indicatorIndex < 0 || indicatorIndex > indicatorList.Count - 1) return null;
            return indicatorList[indicatorIndex];
        }
    }
}