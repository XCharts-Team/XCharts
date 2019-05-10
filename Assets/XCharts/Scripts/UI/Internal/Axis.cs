using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class Axis : JsonDataSupport,IEquatable<Axis>
    {
        public enum AxisType
        {
            Value,
            Category,
            Time,
            Log
        }

        public enum SplitLineType
        {
            None,
            Solid,
            Dashed,
            Dotted
        }

        [System.Serializable]
        public class AxisTick
        {
            [SerializeField] private bool m_Show;
            [SerializeField] private bool m_AlignWithLabel;
            [SerializeField] private bool m_Inside;
            [SerializeField] private float m_Length;

            public bool show { get { return m_Show; }set { m_Show = value; } }
            public bool alignWithLabel { get { return m_AlignWithLabel; } set { m_AlignWithLabel = value; } }
            public bool inside { get { return m_Inside; }set { m_Inside = value; } }
            public float length { get { return m_Length; }set { m_Length = value; } }

            public static AxisTick defaultTick
            {
                get
                {
                    var tick = new AxisTick
                    {
                        m_Show = true,
                        m_AlignWithLabel = false,
                        m_Inside = false,
                        m_Length = 5f
                    };
                    return tick;
                }
            }
        }

        [SerializeField] protected bool m_Show = true;
        [SerializeField] protected AxisType m_Type;
        [SerializeField] protected int m_SplitNumber = 5;
        [SerializeField] protected int m_TextRotation = 0;
        [SerializeField] protected bool m_ShowSplitLine = false;
        [SerializeField] protected SplitLineType m_SplitLineType = SplitLineType.Dashed;
        [SerializeField] protected bool m_BoundaryGap = true;
        [SerializeField] protected List<string> m_Data = new List<string>();
        [SerializeField] protected AxisTick m_AxisTick = AxisTick.defaultTick;

        public bool show { get { return m_Show; }set { m_Show = value; } }
        public AxisType type { get { return m_Type; } set { m_Type = value; } }
        public int splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }
        public int textRotation { get { return m_TextRotation; } set { m_TextRotation = value; } }
        public bool showSplitLine { get { return m_ShowSplitLine; } set { m_ShowSplitLine = value; } }
        public SplitLineType splitLineType { get { return m_SplitLineType; } set { m_SplitLineType = value; } }
        public bool boundaryGap { get { return m_BoundaryGap; } set { m_BoundaryGap = value; } }
        public List<string> data { get { return m_Data; } }
        public AxisTick axisTick { get { return m_AxisTick; }set { m_AxisTick = value; } }

        public void Copy(Axis other)
        {
            m_Show = other.show;
            m_Type = other.type;
            m_SplitNumber = other.splitNumber;
            m_TextRotation = other.textRotation;
            m_ShowSplitLine = other.showSplitLine;
            m_SplitLineType = other.splitLineType;
            m_BoundaryGap = other.boundaryGap;
            m_Data.Clear();
            foreach (var d in other.data) m_Data.Add(d);
        }

        public void ClearData()
        {
            m_Data.Clear();
        }

        public void AddData(string category,int maxDataNumber)
        {
            if (maxDataNumber > 0)
            {
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_Data.Add(category);
        }

        public string GetData(int index)
        {
            if (index >= 0 && index < data.Count)
                return data[index];
            else
                return "";
        }

        public int GetSplitNumber()
        {
            if (data.Count > 2 * m_SplitNumber || data.Count <= 0)
                return m_SplitNumber;
            else
                return data.Count;
        }

        public float GetSplitWidth(float coordinateWidth)
        {
            return coordinateWidth / (m_BoundaryGap ? GetSplitNumber() : GetSplitNumber() - 1);
        }

        public int GetDataNumber()
        {
            return data.Count;
        }

        public float GetDataWidth(float coordinateWidth)
        {
            return coordinateWidth / (m_BoundaryGap ? data.Count : data.Count - 1);
        }

        public string GetScaleName(int index, float maxData = 0)
        {
            if (m_Type == AxisType.Value)
            {
                return ((int)(maxData * index / (GetSplitNumber() -1))).ToString();
            }
            int dataCount = data.Count;
            if (dataCount <= 0) return "";
            
            if(index == GetSplitNumber() - 1 && !m_BoundaryGap)
            {
                return data[data.Count-1];
            }
            else
            {
                float rate = dataCount / GetSplitNumber();
                if (rate < 1) rate = 1;
                int offset = m_BoundaryGap ? (int)(rate / 2) : 0;
                int newIndex = (int)(index * rate >= dataCount - 1 ? dataCount - 1 : offset + index * rate);
                return data[newIndex];
            }
        }

        public int GetScaleNumber()
        {
            if (data.Count > 2 * splitNumber || data.Count <= 0)
                return m_BoundaryGap ? m_SplitNumber + 1 : m_SplitNumber;
            else
                return m_BoundaryGap ? data.Count + 1 : data.Count;
        }

        public float GetScaleWidth(float coordinateWidth)
        {
            int num = GetScaleNumber() - 1;
            if (num <= 0) num = 1;
            return coordinateWidth / num;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Axis)) return false;
            return Equals((Axis)obj);
        }

        public bool Equals(Axis other)
        {
            return show == other.show &&
                type == other.type &&
                splitNumber == other.splitNumber &&
                showSplitLine == other.showSplitLine &&
                textRotation == other.textRotation &&
                splitLineType == other.splitLineType &&
                boundaryGap == other.boundaryGap &&
                ChartHelper.IsValueEqualsList<string>(m_Data, other.data);
        }

        public static bool operator ==(Axis point1, Axis point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(Axis point1, Axis point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            m_Data = ChartHelper.ParseStringFromString(jsonData);
        }
    }

    [System.Serializable]
    public class XAxis : Axis
    {
        public static XAxis defaultXAxis
        {
            get
            {
                var axis = new XAxis
                {
                    m_Show = true,
                    m_Type = AxisType.Category,
                    m_SplitNumber = 5,
                    m_TextRotation = 0,
                    m_ShowSplitLine = false,
                    m_SplitLineType = SplitLineType.Dashed,
                    m_BoundaryGap = true,
                    m_Data = new List<string>()
                    {
                        "x1","x2","x3","x4","x5"
                    }
                };
                return axis;
            }
        }
    }

    [System.Serializable]
    public class YAxis : Axis
    {
        public static YAxis defaultYAxis
        {
            get
            {
                var axis = new YAxis
                {
                    m_Show = true,
                    m_Type = AxisType.Value,
                    m_SplitNumber = 5,
                    m_TextRotation = 0,
                    m_ShowSplitLine = false,
                    m_SplitLineType = SplitLineType.Dashed,
                    m_BoundaryGap = false,
                    m_Data = new List<string>(5),
                };
                return axis;
            }
        }
    }
}