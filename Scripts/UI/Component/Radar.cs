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
    public class Radar : JsonDataSupport, IEquatable<Radar>
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
        /// Indicator of radar chart, which is used to assign multiple variables(dimensions) in radar chart. 
        /// 雷达图的指示器，用来指定雷达图中的多个变量（维度）。
        /// </summary>
        [System.Serializable]
        public class Indicator : IEquatable<Indicator>
        {
            [SerializeField] private string m_Name;
            [SerializeField] private float m_Max;
            [SerializeField] private float m_Min;
            [SerializeField] private Color m_Color;
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
            /// Specfy a color the the indicator.
            /// 标签特定的颜色。默认取自主题的axisTextColor。
            /// </summary>
            public Color color { get { return m_Color; } set { m_Color = value; } }
            /// <summary>
            /// the text conponent of indicator.
            /// 指示器的文本组件。
            /// </summary>
            public Text text { get; set; }

            public Indicator Clone()
            {
                return new Indicator()
                {
                    m_Name = name,
                    m_Max = max,
                    m_Min = min,
                    m_Color = color
                };
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj))
                {
                    return false;
                }
                else if (obj is Indicator)
                {
                    return Equals((Indicator)obj);
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(Indicator other)
            {
                if (ReferenceEquals(null, other))
                {
                    return false;
                }
                return m_Name.Equals(other.name) &&
                    ChartHelper.IsValueEqualsColor(m_Color, other.color);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
        [SerializeField] private Shape m_Shape;
        [SerializeField] private float m_Radius = 100;
        [SerializeField] private int m_SplitNumber = 5;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private LineStyle m_LineStyle = new LineStyle();
        [SerializeField] private AxisSplitArea m_SplitArea = AxisSplitArea.defaultSplitArea;
        [SerializeField] private bool m_Indicator = true;
        [SerializeField] private List<Indicator> m_IndicatorList = new List<Indicator>();
        /// <summary>
        /// Radar render type, in which 'Polygon' and 'Circle' are supported.
        /// 雷达图绘制类型，支持 'Polygon' 和 'Circle'。
        /// </summary>
        /// <value></value>
        public Shape shape { get { return m_Shape; } set { m_Shape = value; } }
        /// <summary>
        /// the radius of radar.
        /// 雷达图的半径。
        /// </summary>
        public float radius { get { return m_Radius; } set { m_Radius = value; } }
        /// <summary>
        /// Segments of indicator axis.
        /// 指示器轴的分割段数。
        /// </summary>
        public int splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }
        /// <summary>
        /// the center of radar chart.
        /// 雷达图的中心点。数组的第一项是横坐标，第二项是纵坐标。
        /// 当值为0-1之间时表示百分比，设置成百分比时第一项是相对于容器宽度，第二项是相对于容器高度。
        /// </summary>
        public float[] center { get { return m_Center; } set { m_Center = value; } }
        /// <summary>
        /// the line style of radar.
        /// 线条样式。
        /// </summary>
        public LineStyle lineStyle { get { return m_LineStyle; } set { m_LineStyle = value; } }
        /// <summary>
        /// Split area of axis in grid area.
        /// 分割区域。
        /// </summary>
        public AxisSplitArea splitArea { get { return m_SplitArea; } set { m_SplitArea = value; } }
        /// <summary>
        /// Whether to show indicator.
        /// 是否显示指示器。
        /// </summary>
        public bool indicator { get { return m_Indicator; } set { m_Indicator = value; } }
        /// <summary>
        /// the indicator list.
        /// 指示器列表。
        /// </summary>
        public List<Indicator> indicatorList { get { return m_IndicatorList; } }

        /// <summary>
        /// the center position of radar in container.
        /// 雷达图在容器中的具体中心点。
        /// </summary>
        /// <value></value>
        public Vector2 centerPos { get; set; }
        /// <summary>
        /// the true radius of radar.
        /// 雷达图的运行时实际半径。
        /// </summary>
        /// <value></value>
        public float actualRadius { get; set; }
        /// <summary>
        /// the data position list of radar.
        /// 雷达图的所有数据坐标点列表。
        /// </summary>
        /// <returns></returns>
        public Dictionary<int,List<Vector3>> dataPosList = new Dictionary<int,List<Vector3>>();

        public static Radar defaultRadar
        {
            get
            {
                var radar = new Radar
                {
                    m_Shape = Shape.Polygon,
                    m_Radius = 0.4f,
                    m_SplitNumber = 5,
                    m_Indicator = true,
                    m_IndicatorList = new List<Indicator>(5){
                        new Indicator(){name="indicator1",max = 100},
                        new Indicator(){name="indicator2",max = 100},
                        new Indicator(){name="indicator3",max = 100},
                        new Indicator(){name="indicator4",max = 100},
                        new Indicator(){name="indicator5",max = 100},
                    }
                };
                radar.center[0] = 0.5f;
                radar.center[1] = 0.45f;
                radar.splitArea.show = true;
                radar.lineStyle.width = 0.3f;
                return radar;
            }
        }

        public void Copy(Radar other)
        {
            m_Shape = other.shape;
            m_Radius = other.radius;
            m_SplitNumber = other.splitNumber;
            m_Center[0] = other.center[0];
            m_Center[1] = other.center[1];
            m_Indicator = other.indicator;
            indicatorList.Clear();
            foreach (var d in other.indicatorList) indicatorList.Add(d.Clone());
        }

        public Radar Clone()
        {
            var radar = new Radar();
            radar.shape = shape;
            radar.radius = radius;
            radar.splitNumber = splitNumber;
            radar.center[0] = center[0];
            radar.center[1] = center[1];
            radar.indicatorList.Clear();
            radar.indicator = indicator;
            foreach (var d in indicatorList) radar.indicatorList.Add(d.Clone());
            return radar;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Radar)
            {
                return Equals((Radar)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Radar other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return radius == other.radius &&
                shape == other.shape &&
                splitNumber == other.splitNumber &&
                center[0] == other.center[0] &&
                center[1] == other.center[1] &&
                indicator == other.indicator &&
                IsEqualsIndicatorList(indicatorList, other.indicatorList);
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

        public static bool operator ==(Radar left, Radar right)
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

        public static bool operator !=(Radar left, Radar right)
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

        public void UpdateRadarCenter(float chartWidth, float chartHeight)
        {
            if (center.Length < 2) return;
            var centerX = center[0] <= 1 ? chartWidth * center[0] : center[0];
            var centerY = center[1] <= 1 ? chartHeight * center[1] : center[1];
            centerPos = new Vector2(centerX, centerY);
            if (radius <= 0)
            {
                actualRadius = 0;
            }
            else if (radius <= 1)
            {
                actualRadius = Mathf.Min(chartWidth, chartHeight) * radius;
            }
            else
            {
                actualRadius = radius;
            }
        }

        public Vector3 GetIndicatorPosition(int index)
        {
            int indicatorNum = indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum * index;
            var x = centerPos.x + actualRadius * Mathf.Sin(angle);
            var y = centerPos.y + actualRadius * Mathf.Cos(angle);
            return new Vector3(x, y);
        }
    }
}