using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace XCharts
{
    public enum SerieType
    {
        None,
        Line,
        Bar,
        Pie,
        Radar
    }

    [System.Serializable]
    public class Serie : JsonDataSupport
    {
        [SerializeField] [DefaultValue("true")] private bool m_Show;
        [SerializeField] private SerieType m_Type;
        [SerializeField] private bool m_Selected;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Stack;
        [SerializeField] private int m_AxisIndex;
        [SerializeField] private bool m_TwoDimensionData;
        [FormerlySerializedAs("m_Data")]
        [SerializeField] private List<float> m_YData = new List<float>();
        [SerializeField] private List<float> m_XData = new List<float>();

        public int index { get; set; }
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        public bool show { get { return m_Show; } set { m_Show = value; } }
        public SerieType type { get { return m_Type; } set { m_Type = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public string stack { get { return m_Stack; } set { m_Stack = value; } }
        public int axisIndex { get { return m_AxisIndex; } set { m_AxisIndex = value; } }
        public List<float> yData { get { return m_YData; } set { m_YData = value; } }
        public List<float> xData { get { return m_XData; } set { m_XData = value; } }

        public int filterStart { get; set; }
        public int filterEnd { get; set; }

        private List<float> yFilterData { get; set; }
        private List<float> xFilterData { get; set; }

        public float yMax
        {
            get
            {
                float max = int.MinValue;
                foreach (var data in yData)
                {
                    if (data > max)
                    {
                        max = data;
                    }
                }
                return max;
            }
        }

        public float xMax
        {
            get
            {
                float max = int.MinValue;
                foreach (var data in xData)
                {
                    if (data > max)
                    {
                        max = data;
                    }
                }
                return max;
            }
        }

        public float yMin
        {
            get
            {
                float min = int.MaxValue;
                foreach (var data in yData)
                {
                    if (data < min)
                    {
                        min = data;
                    }
                }
                return min;
            }
        }

        public float xMin
        {
            get
            {
                float min = int.MaxValue;
                foreach (var data in xData)
                {
                    if (data < min)
                    {
                        min = data;
                    }
                }
                return min;
            }
        }

        public float yTotal
        {
            get
            {
                float total = 0;
                foreach (var data in yData)
                {
                    total += data;
                }
                return total;
            }
        }

        public float xTotal
        {
            get
            {
                float total = 0;
                foreach (var data in xData)
                {
                    total += data;
                }
                return total;
            }
        }

        public void ClearData()
        {
            m_XData.Clear();
            m_YData.Clear();
        }

        public void RemoveData(int index)
        {
            m_XData.RemoveAt(index);
            m_YData.RemoveAt(index);
        }

        public void AddYData(float value, int maxDataNumber = 0)
        {
            if (maxDataNumber > 0)
            {
                while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
            }
            m_XData.Add(m_XData.Count);
            m_YData.Add(value);
        }

        public void AddXYData(float xValue, float yValue, int maxDataNumber = 0)
        {
            if (maxDataNumber > 0)
            {
                while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
            }
            m_XData.Add(xValue);
            m_YData.Add(yValue);
        }

        public float GetYData(int index, DataZoom dataZoom = null)
        {
            var showData = GetYDataList(dataZoom);
            if (index >= 0 && index <= showData.Count - 1)
            {
                return showData[index];
            }
            return 0;
        }

        public void GetXYData(int index, DataZoom dataZoom, out float xValue, out float yVlaue)
        {
            xValue = 0;
            yVlaue = 0;
            var xShowData = GetXDataList(dataZoom);
            if (index >= 0 && index < xShowData.Count) xValue = xShowData[index];
            var yShowData = GetYDataList(dataZoom);
            if (index >= 0 && index < yShowData.Count) yVlaue = yShowData[index];
        }

        public List<float> GetYDataList(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((yData.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((yData.Count - 1) * dataZoom.end / 100);
                var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                if (yFilterData == null || yFilterData.Count != count)
                {
                    UpdateFilterData(dataZoom);
                }
                return yFilterData;
            }
            else
            {
                return m_YData;
            }
        }

        public List<float> GetXDataList(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((xData.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((xData.Count - 1) * dataZoom.end / 100);
                var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                if (xFilterData == null || xFilterData.Count != count)
                {
                    UpdateFilterData(dataZoom);
                }
                return xFilterData;
            }
            else
            {
                return m_XData;
            }
        }

        public void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((yData.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((yData.Count - 1) * dataZoom.end / 100);
                if (startIndex != filterStart || endIndex != filterEnd)
                {
                    filterStart = startIndex;
                    filterEnd = endIndex;
                    if (m_YData.Count > 0)
                    {
                        var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                        yFilterData = m_YData.GetRange(startIndex, count);
                    }
                    else
                    {
                        yFilterData = m_YData;
                    }
                    if (m_XData.Count > 0)
                    {
                        var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                        xFilterData = m_XData.GetRange(startIndex, count);
                    }
                    else
                    {
                        xFilterData = m_XData;
                    }
                }
                else if (endIndex == 0)
                {
                    yFilterData = new List<float>();
                    xFilterData = new List<float>();
                }
            }
        }

        public void UpdateYData(int index, float value)
        {
            if (index >= 0 && index <= m_YData.Count - 1)
            {
                m_YData[index] = value;
            }
        }

        public void UpdateXYData(int index, float xValue, float yValue)
        {
            if (index >= 0 && index <= m_YData.Count - 1)
            {
                m_YData[index] = yValue;
            }
            if (index >= 0 && index <= m_XData.Count - 1)
            {
                m_XData[index] = xValue;
            }
        }

        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            m_YData = ChartHelper.ParseFloatFromString(jsonData);
        }
    }
}
