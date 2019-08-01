using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

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
            /// 标签特定的颜色。
            /// </summary>
            public Color color { get { return m_Color; } set { m_Color = value; } }

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

            public bool Equals(Indicator other)
            {
                return name.Equals(other.name);
            }
        }

        [SerializeField] private bool m_Cricle;
        [SerializeField] private bool m_Area;

        [SerializeField] private float m_Radius = 100;
        [SerializeField] private int m_SplitNumber = 5;

        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;

        [SerializeField] private float m_LineTickness = 1f;
        [SerializeField] private float m_LinePointSize = 5f;
        [SerializeField] private Color m_LineColor = Color.grey;
        [Range(0, 255)]
        [SerializeField] private int m_AreaAlpha;

        [SerializeField] private List<Color> m_BackgroundColorList = new List<Color>();
        [SerializeField] private bool m_Indicator = true;
        [SerializeField] private List<Indicator> m_IndicatorList = new List<Indicator>();

        /// <summary>
        /// True is render radar as cricle,otherwise render as polygon.
        ///雷达图是否绘制成圆形，true为圆形，false为多边形。
        /// </summary>
        public bool cricle { get { return m_Cricle; } set { m_Cricle = value; } }
        /// <summary>
        /// Whether to fill color in area.
        /// 是否区域填充颜色
        /// </summary>
        public bool area { get { return m_Area; } set { m_Area = value; } }
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
        /// Distance between radar component and the left side of the container.
        /// 雷达图离容器左侧的距离。
        /// </summary>
        public float left { get { return m_Left; } set { m_Left = value; } }
        /// <summary>
        /// Distance between radar component and the right side of the container.
        /// 雷达图离容器右侧的距离。
        /// </summary>
        public float right { get { return m_Right; } set { m_Right = value; } }
        /// <summary>
        /// Distance between radar component and the top side of the container.
        /// 雷达图离容器上侧的距离。
        /// </summary>
        public float top { get { return m_Top; } set { m_Top = value; } }
        /// <summary>
        /// Distance between radar component and the bottom side of the container.
        /// 雷达图离容器下侧的距离。
        /// </summary>
        public float bottom { get { return m_Bottom; } set { m_Bottom = value; } }
        /// <summary>
        /// the tickness of line.
        /// 线的粗细。
        /// </summary>
        public float lineTickness { get { return m_LineTickness; } set { m_LineTickness = value; } }
        /// <summary>
        /// the size of point.
        /// 圆点大小。
        /// </summary>
        public float linePointSize { get { return m_LinePointSize; } set { m_LinePointSize = value; } }
        /// <summary>
        /// the color of line.
        /// 线的颜色。
        /// </summary>
        public Color lineColor { get { return m_LineColor; } set { m_LineColor = value; } }
        /// <summary>
        /// the alpha of area color.
        /// 区域填充时的颜色alpha值
        /// </summary>
        public int areaAlpha { get { return m_AreaAlpha; } set { m_AreaAlpha = value; } }
        /// <summary>
        /// the color list of split area.
        /// 分割区域颜色列表。
        /// </summary>
        public List<Color> backgroundColorList { get { return m_BackgroundColorList; } }
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

        public static Radar defaultRadar
        {
            get
            {
                var radar = new Radar
                {
                    m_Cricle = false,
                    m_Area = false,
                    m_Radius = 100,
                    m_SplitNumber = 5,
                    m_Left = 0,
                    m_Right = 0,
                    m_Top = 0,
                    m_Bottom = 0,
                    m_LineTickness = 1f,
                    m_LinePointSize = 5f,
                    m_AreaAlpha = 150,
                    m_LineColor = Color.grey,
                    m_Indicator = true,
                    m_BackgroundColorList = new List<Color> {
                        new Color32(246, 246, 246, 255),
                        new Color32(231, 231, 231, 255)
                    },
                    m_IndicatorList = new List<Indicator>(5){
                        new Indicator(){name="radar1",max = 100},
                        new Indicator(){name="radar2",max = 100},
                        new Indicator(){name="radar3",max = 100},
                        new Indicator(){name="radar4",max = 100},
                        new Indicator(){name="radar5",max = 100},
                    }
                };
                return radar;
            }
        }

        public void Copy(Radar other)
        {
            m_Radius = other.radius;
            m_SplitNumber = other.splitNumber;
            m_Left = other.left;
            m_Right = other.right;
            m_Top = other.top;
            m_Bottom = other.bottom;
            m_Indicator = other.indicator;
            m_AreaAlpha = other.areaAlpha;
            indicatorList.Clear();
            foreach (var d in other.indicatorList) indicatorList.Add(d.Clone());
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
                splitNumber == other.splitNumber &&
                left == other.left &&
                right == other.right &&
                top == other.top &&
                bottom == other.bottom &&
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

        public float GetIndicatorMax(int index)
        {
            if (index >= 0 && index < m_IndicatorList.Count)
            {
                return m_IndicatorList[index].max;
            }
            return 0;
        }
    }
}