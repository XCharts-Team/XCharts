using UnityEngine;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace XCharts
{
    [System.Serializable]
    public class Radar : JsonDataSupport, IEquatable<Radar>
    {
        [System.Serializable]
        public class Indicator : IEquatable<Indicator>
        {
            [SerializeField] private string m_Name;
            [SerializeField] private float m_Max;

            public string name { get { return m_Name; } set { m_Name = value; } }
            public float max { get { return m_Max; } set { m_Max = value; } }

            public Indicator Clone()
            {
                return new Indicator()
                {
                    name = name,
                    max = max
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

        public bool cricle { get { return m_Cricle; } set { m_Cricle = value; } }
        public bool area { get { return m_Area; } set { m_Area = value; } }

        public float radius { get { return m_Radius; } set { m_Radius = value; } }
        public int splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }

        public float left { get { return m_Left; } set { m_Left = value; } }
        public float right { get { return m_Right; } set { m_Right = value; } }
        public float top { get { return m_Top; } set { m_Top = value; } }
        public float bottom { get { return m_Bottom; } set { m_Bottom = value; } }

        public float lineTickness { get { return m_LineTickness; } set { m_LineTickness = value; } }
        public float linePointSize { get { return m_LinePointSize; } set { m_LinePointSize = value; } }
        public Color lineColor { get { return m_LineColor; } set { m_LineColor = value; } }
        public int areaAipha { get { return m_AreaAlpha; } set { m_AreaAlpha = value; } }
        public List<Color> backgroundColorList { get { return m_BackgroundColorList; } }
        public bool indicator { get { return m_Indicator; } set { m_Indicator = value; } }
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
            m_AreaAlpha = other.areaAipha;
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