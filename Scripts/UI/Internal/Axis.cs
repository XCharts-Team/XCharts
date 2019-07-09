using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class Axis : JsonDataSupport, IEquatable<Axis>
    {
        public enum AxisType
        {
            Value,
            Category,
            //Time,
            //Log
        }

        public enum AxisMinMaxType
        {
            Default,
            MinMax,
            Custom
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

            public bool show { get { return m_Show; } set { m_Show = value; } }
            public bool alignWithLabel { get { return m_AlignWithLabel; } set { m_AlignWithLabel = value; } }
            public bool inside { get { return m_Inside; } set { m_Inside = value; } }
            public float length { get { return m_Length; } set { m_Length = value; } }

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

        [System.Serializable]
        public class AxisLine
        {
            [SerializeField] private bool m_Show;
            [SerializeField] private bool m_Symbol;
            [SerializeField] private float m_SymbolWidth;
            [SerializeField] private float m_SymbolHeight;
            [SerializeField] private float m_SymbolOffset;
            [SerializeField] private float m_SymbolDent;

            public bool show { get { return m_Show; } set { m_Show = value; } }
            public bool symbol { get { return m_Symbol; } set { m_Symbol = value; } }
            public float symbolWidth { get { return m_SymbolWidth; } set { m_SymbolWidth = value; } }
            public float symbolHeight { get { return m_SymbolHeight; } set { m_SymbolHeight = value; } }
            public float symbolOffset { get { return m_SymbolOffset; } set { m_SymbolOffset = value; } }
            public float symbolDent { get { return m_SymbolDent; } set { m_SymbolDent = value; } }

            public static AxisLine defaultAxisLine
            {
                get
                {
                    var axisLine = new AxisLine
                    {
                        m_Show = true,
                        m_Symbol = false,
                        m_SymbolWidth = 10,
                        m_SymbolHeight = 15,
                        m_SymbolOffset = 0,
                        m_SymbolDent = 3,
                    };
                    return axisLine;
                }
            }
        }

        [Serializable]
        public class AxisName
        {
            [Serializable]
            public enum Location
            {
                Start,
                Middle,
                End
            }
            [SerializeField] private bool m_Show;
            [SerializeField] private string m_Name;
            [SerializeField] private Location m_Location;
            [SerializeField] private float m_Gap;
            [SerializeField] private float m_Rotate;
            [SerializeField] private Color m_Color;
            [SerializeField] private int m_FontSize;
            [SerializeField] private FontStyle m_FontStyle;

            public bool show { get { return m_Show; } set { m_Show = value; } }
            public string name { get { return m_Name; } set { m_Name = value; } }
            public Location location { get { return m_Location; } set { m_Location = value; } }
            public float gap { get { return m_Gap; } set { m_Gap = value; } }
            public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
            public Color color { get { return m_Color; } set { m_Color = value; } }
            public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
            public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }

            public static AxisName defaultAxisName
            {
                get
                {
                    return new AxisName()
                    {
                        m_Show = false,
                        m_Name = "axisName",
                        m_Location = Location.End,
                        m_Gap = 5,
                        m_Rotate = 0,
                        m_Color = Color.clear,
                        m_FontSize = 18,
                        m_FontStyle = FontStyle.Normal
                    };
                }
            }

            public void Copy(AxisName other)
            {
                m_Show = other.show;
                m_Name = other.name;
                m_Location = other.location;
                m_Gap = other.gap;
                m_Rotate = other.rotate;
                m_Color = other.color;
                m_FontSize = other.fontSize;
                m_FontStyle = other.fontStyle;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                var other = (AxisName)obj;
                return m_Show == other.show &&
                    m_Name.Equals(other.name) &&
                    m_Location == other.location &&
                    m_Gap == other.gap &&
                    m_Rotate == other.rotate &&
                    m_Color == other.color &&
                    m_FontSize == other.fontSize &&
                    m_FontStyle == other.fontStyle;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        /// <summary>
        /// Split area of axis in grid area, not shown by default.
        /// </summary>
        [Serializable]
        public class SplitArea
        {
            [SerializeField] private bool m_Show;
            [SerializeField] private List<Color> m_Color;

            /// <summary>
            /// Set this to true to show the splitArea.
            /// </summary>
            /// <value>false</value>
            public bool show { get { return m_Show; } set { m_Show = value; } }
            /// <summary>
            /// Color of split area. SplitArea color could also be set in color array,
            /// which the split lines would take as their colors in turns. 
            /// Dark and light colors in turns are used by default.
            /// </summary>
            /// <value>['rgba(250,250,250,0.3)','rgba(200,200,200,0.3)']</value>
            public List<Color> color { get { return m_Color; } set { m_Color = value; } }

            public static SplitArea defaultSplitArea
            {
                get
                {
                    return new SplitArea()
                    {
                        m_Show = false,
                        m_Color = new List<Color>(){
                            new Color32(250,250,250,77),
                            new Color32(200,200,200,77)
                        }
                    };
                }
            }

            public Color getColor(int index)
            {
                var i = index % color.Count;
                return color[i];
            }
        }

        [Serializable]
        public class AxisLabel
        {
            [SerializeField] private bool m_Show;
            [SerializeField] private int m_Interval;
            [SerializeField] private bool m_Inside;
            [SerializeField] private float m_Rotate;
            [SerializeField] private float m_Margin;
            [SerializeField] private Color m_Color;
            [SerializeField] private int m_FontSize;
            [SerializeField] private FontStyle m_FontStyle;

            public bool show { get { return m_Show; } set { m_Show = value; } }
            public int interval { get { return m_Interval; } set { m_Interval = value; } }
            public bool inside { get { return m_Inside; } set { m_Inside = value; } }
            public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
            public float margin { get { return m_Margin; } set { m_Margin = value; } }
            public Color color { get { return m_Color; } set { m_Color = value; } }
            public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
            public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }

            public static AxisLabel defaultAxisLabel
            {
                get
                {
                    return new AxisLabel()
                    {
                        m_Show = true,
                        m_Interval = 0,
                        m_Inside = false,
                        m_Rotate = 0,
                        m_Margin = 8,
                        m_Color = Color.clear,
                        m_FontSize = 18,
                        m_FontStyle = FontStyle.Normal
                    };
                }
            }
            public void Copy(AxisLabel other)
            {
                m_Show = other.show;
                m_Interval = other.interval;
                m_Inside = other.inside;
                m_Rotate = other.rotate;
                m_Margin = other.margin;
                m_Color = other.color;
                m_FontSize = other.fontSize;
                m_FontStyle = other.fontStyle;
            }

            public override bool Equals(object obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                var other = (AxisLabel)obj;
                return m_Show == other.show &&
                    m_Interval.Equals(other.interval) &&
                    m_Inside == other.inside &&
                    m_Rotate == other.rotate &&
                    m_Margin == other.margin &&
                    m_Color == other.color &&
                    m_FontSize == other.fontSize &&
                    m_FontStyle == other.fontStyle;
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        [SerializeField] protected bool m_Show = true;
        [SerializeField] protected AxisType m_Type;
        [SerializeField] protected AxisMinMaxType m_MinMaxType;
        [SerializeField] protected int m_Min;
        [SerializeField] protected int m_Max;
        [SerializeField] protected int m_SplitNumber = 5;
        [SerializeField] protected bool m_ShowSplitLine = false;
        [SerializeField] protected SplitLineType m_SplitLineType = SplitLineType.Dashed;
        [SerializeField] protected bool m_BoundaryGap = true;
        [SerializeField] protected List<string> m_Data = new List<string>();
        [SerializeField] protected AxisLine m_AxisLine = AxisLine.defaultAxisLine;
        [SerializeField] protected AxisName m_AxisName = AxisName.defaultAxisName;
        [SerializeField] protected AxisTick m_AxisTick = AxisTick.defaultTick;
        [SerializeField] protected AxisLabel m_AxisLabel = AxisLabel.defaultAxisLabel;
        [SerializeField] protected SplitArea m_SplitArea = SplitArea.defaultSplitArea;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public AxisType type { get { return m_Type; } set { m_Type = value; } }
        public AxisMinMaxType minMaxType { get { return m_MinMaxType; } set { m_MinMaxType = value; } }
        public int min { get { return m_Min; } set { m_Min = value; } }
        public int max { get { return m_Max; } set { m_Max = value; } }
        public int splitNumber { get { return m_SplitNumber; } set { m_SplitNumber = value; } }
        public bool showSplitLine { get { return m_ShowSplitLine; } set { m_ShowSplitLine = value; } }
        public SplitLineType splitLineType { get { return m_SplitLineType; } set { m_SplitLineType = value; } }
        public bool boundaryGap { get { return m_BoundaryGap; } set { m_BoundaryGap = value; } }
        public List<string> data { get { return m_Data; } }

        public AxisLine axisLine { get { return m_AxisLine; } set { m_AxisLine = value; } }
        public AxisName axisName { get { return m_AxisName; } set { m_AxisName = value; } }
        public AxisTick axisTick { get { return m_AxisTick; } set { m_AxisTick = value; } }
        public AxisLabel axisLabel { get { return m_AxisLabel; } set { m_AxisLabel = value; } }
        public SplitArea splitArea { get { return m_SplitArea; } set { m_SplitArea = value; } }

        public int filterStart { get; set; }
        public int filterEnd { get; set; }
        public List<string> filterData { get; set; }

        public void Copy(Axis other)
        {
            m_Show = other.show;
            m_Type = other.type;
            m_Min = other.min;
            m_Max = other.max;
            m_SplitNumber = other.splitNumber;

            m_ShowSplitLine = other.showSplitLine;
            m_SplitLineType = other.splitLineType;
            m_BoundaryGap = other.boundaryGap;
            m_AxisName.Copy(other.axisName);
            m_AxisLabel.Copy(other.axisLabel);
            m_Data.Clear();
            foreach (var d in other.data) m_Data.Add(d);
        }

        public void ClearData()
        {
            m_Data.Clear();
        }

        public void AddData(string category, int maxDataNumber)
        {
            if (maxDataNumber > 0)
            {
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_Data.Add(category);
        }

        public string GetData(int index, DataZoom dataZoom)
        {
            var showData = GetData(dataZoom);
            if (index >= 0 && index < showData.Count)
                return showData[index];
            else
                return "";
        }

        public List<string> GetData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
                var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                if (filterData == null || filterData.Count != count)
                {
                    UpdateFilterData(dataZoom);
                }
                return filterData;
            }
            else
            {
                return m_Data;
            }
        }

        public void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
                if (startIndex != filterStart || endIndex != filterEnd)
                {
                    filterStart = startIndex;
                    filterEnd = endIndex;
                    if (m_Data.Count > 0)
                    {
                        var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                        filterData = m_Data.GetRange(startIndex, count);
                    }
                    else
                    {
                        filterData = m_Data;
                    }
                }
                else if (endIndex == 0)
                {
                    filterData = new List<string>();
                }
            }
        }

        public int GetSplitNumber(DataZoom dataZoom)
        {
            if (type == AxisType.Value) return m_SplitNumber;
            int dataCount = GetData(dataZoom).Count;
            if (dataCount > 2 * m_SplitNumber || dataCount <= 0)
                return m_SplitNumber;
            else
                return dataCount;
        }

        public float GetSplitWidth(float coordinateWidth, DataZoom dataZoom)
        {
            return coordinateWidth / (m_BoundaryGap ? GetSplitNumber(dataZoom) : GetSplitNumber(dataZoom) - 1);
        }

        public int GetDataNumber(DataZoom dataZoom)
        {
            return GetData(dataZoom).Count;
        }

        public float GetDataWidth(float coordinateWidth, DataZoom dataZoom)
        {
            var dataCount = GetDataNumber(dataZoom);
            return coordinateWidth / (m_BoundaryGap ? dataCount : dataCount - 1);
        }

        public string GetScaleName(int index, float minValue, float maxValue, DataZoom dataZoom)
        {
            if (m_Type == AxisType.Value)
            {
                float value = (minValue + (maxValue - minValue) * index / (GetSplitNumber(dataZoom) - 1));
                if (value - (int)value == 0)
                    return (value).ToString();
                else
                    return (value).ToString("f1");
            }
            var showData = GetData(dataZoom);
            int dataCount = showData.Count;
            if (dataCount <= 0) return "";

            if (index == GetSplitNumber(dataZoom) - 1 && !m_BoundaryGap)
            {
                return showData[dataCount - 1];
            }
            else
            {
                float rate = dataCount / GetSplitNumber(dataZoom);
                if (rate < 1) rate = 1;
                int offset = m_BoundaryGap ? (int)(rate / 2) : 0;
                int newIndex = (int)(index * rate >= dataCount - 1 ?
                    dataCount - 1 : offset + index * rate);
                return showData[newIndex];
            }
        }

        public int GetScaleNumber(DataZoom dataZoom)
        {
            if (type == AxisType.Value)
            {
                return m_BoundaryGap ? m_SplitNumber + 1 : m_SplitNumber;
            }
            else
            {
                var showData = GetData(dataZoom);
                int dataCount = showData.Count;
                if (dataCount > 2 * splitNumber || dataCount <= 0)
                    return m_BoundaryGap ? m_SplitNumber + 1 : m_SplitNumber;
                else
                    return m_BoundaryGap ? dataCount + 1 : dataCount;
            }
        }

        public float GetScaleWidth(float coordinateWidth, DataZoom dataZoom)
        {
            int num = GetScaleNumber(dataZoom) - 1;
            if (num <= 0) num = 1;
            return coordinateWidth / num;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is Axis)
            {
                return Equals((Axis)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Axis other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return show == other.show &&
                type == other.type &&
                min == other.min &&
                max == other.max &&
                splitNumber == other.splitNumber &&
                showSplitLine == other.showSplitLine &&
                m_AxisLabel.Equals(other.axisLabel) &&
                splitLineType == other.splitLineType &&
                boundaryGap == other.boundaryGap &&
                axisName.Equals(other.axisName) &&
                ChartHelper.IsValueEqualsList<string>(m_Data, other.data);
        }

        public static bool operator ==(Axis left, Axis right)
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

        public static bool operator !=(Axis left, Axis right)
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
                    m_Min = 0,
                    m_Max = 0,
                    m_SplitNumber = 5,
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
                    m_Min = 0,
                    m_Max = 0,
                    m_SplitNumber = 5,
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