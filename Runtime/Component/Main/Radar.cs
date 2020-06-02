/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// Coordinate for radar charts. 
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
        /// 显示位置。
        /// </summary>
        public enum PositionType
        {
            /// <summary>
            /// 显示在顶点处。
            /// </summary>
            Vertice,
            /// <summary>
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
            [SerializeField] private float m_Max;
            [SerializeField] private float m_Min;
            [SerializeField] private TextStyle m_TextStyle = new TextStyle();

            /// <summary>
            /// 指示器名称。
            /// </summary>
            public string name { get { return m_Name; } set { m_Name = value; } }
            /// <summary>
            /// The maximum value of indicator, with default value of 0, but we recommend to set it manually.
            /// 指示器的最大值，默认为 0 无限制。
            /// </summary>
            public float max { get { return m_Max; } set { m_Max = value; } }
            /// <summary>
            /// The minimum value of indicator, with default value of 0.
            /// 指示器的最小值，默认为 0 无限制。
            /// </summary>
            public float min { get { return m_Min; } set { m_Min = value; } }
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
        }
        [SerializeField] private Shape m_Shape;
        [SerializeField] private float m_Radius = 100;
        [SerializeField] private int m_SplitNumber = 5;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private AxisSplitLine m_SplitLine = AxisSplitLine.defaultSplitLine;
        [SerializeField] private AxisSplitArea m_SplitArea = AxisSplitArea.defaultSplitArea;
        [SerializeField] private bool m_Indicator = true;
        [SerializeField] private PositionType m_PositionType = PositionType.Vertice;
        [SerializeField] private float m_IndicatorGap = 10;
        [SerializeField] protected int m_CeilRate = 0;
        [SerializeField] private List<Indicator> m_IndicatorList = new List<Indicator>();
        /// <summary>
        /// Radar render type, in which 'Polygon' and 'Circle' are supported.
        /// 雷达图绘制类型，支持 'Polygon' 和 'Circle'。
        /// </summary>
        /// <value></value>
        public Shape shape
        {
            get { return m_Shape; }
            set { if (PropertyUtility.SetStruct(ref m_Shape, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the radius of radar.
        /// 雷达图的半径。
        /// </summary>
        public float radius
        {
            get { return m_Radius; }
            set { if (PropertyUtility.SetStruct(ref m_Radius, value)) SetAllDirty(); }
        }
        /// <summary>
        /// Segments of indicator axis.
        /// 指示器轴的分割段数。
        /// </summary>
        public int splitNumber
        {
            get { return m_SplitNumber; }
            set { if (PropertyUtility.SetStruct(ref m_SplitNumber, value)) SetAllDirty(); }
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
        /// split line.
        /// 分割线。
        /// </summary>
        public AxisSplitLine splitLine
        {
            get { return m_SplitLine; }
            set { if (PropertyUtility.SetClass(ref m_SplitLine, value, true)) SetAllDirty(); }
        }
        /// <summary>
        /// Split area of axis in grid area.
        /// 分割区域。
        /// </summary>
        public AxisSplitArea splitArea
        {
            get { return m_SplitArea; }
            set { if (PropertyUtility.SetClass(ref m_SplitArea, value, true)) SetAllDirty(); }
        }
        /// <summary>
        /// Whether to show indicator.
        /// 是否显示指示器。
        /// </summary>
        public bool indicator
        {
            get { return m_Indicator; }
            set { if (PropertyUtility.SetStruct(ref m_Indicator, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 指示器和雷达的间距。
        /// </summary>
        public float indicatorGap
        {
            get { return m_IndicatorGap; }
            set { if (PropertyUtility.SetStruct(ref m_IndicatorGap, value)) SetComponentDirty(); }
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
        /// /// 显示位置类型。
        /// </summary>
        public PositionType positionType
        {
            get { return m_PositionType; }
            set { if (PropertyUtility.SetStruct(ref m_PositionType, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the indicator list.
        /// 指示器列表。
        /// </summary>
        public List<Indicator> indicatorList { get { return m_IndicatorList; } }

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
                radar.splitLine.lineStyle.width = 0.6f;
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

        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            string pattern = "[\"|'](.*?)[\"|']";
            if (Regex.IsMatch(jsonData, pattern))
            {
                m_IndicatorList.Clear();
                MatchCollection m = Regex.Matches(jsonData, pattern);
                foreach (Match match in m)
                {
                    m_IndicatorList.Add(new Indicator()
                    {
                        name = match.Groups[1].Value
                    });
                }
            }
            pattern = "(\\d+)";
            if (Regex.IsMatch(jsonData, pattern))
            {
                MatchCollection m = Regex.Matches(jsonData, pattern);
                int index = 0;
                foreach (Match match in m)
                {
                    if (m_IndicatorList[index] != null)
                    {
                        m_IndicatorList[index].max = int.Parse(match.Groups[1].Value);
                    }
                    index++;
                }
            }
        }

        public float GetIndicatorMin(int index)
        {
            if (index >= 0 && index < m_IndicatorList.Count)
            {
                return m_IndicatorList[index].min;
            }
            return 0;
        }
        public float GetIndicatorMax(int index)
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