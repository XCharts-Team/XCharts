using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    public class Serie : JsonDataSupport
    {
        public enum SerieType
        {
            Line,
            Bar
        }

        [SerializeField][DefaultValue("true")] private bool m_Show;
        [SerializeField] private SerieType m_Type;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Stack;
        [SerializeField] private int m_AxisIndex;
        [SerializeField] private List<float> m_Data = new List<float>();
        [SerializeField] private bool m_Flodout;

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public SerieType type { get { return m_Type; } set { m_Type = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public string stack { get { return m_Stack; } set { m_Stack = value; } }
        public int axisIndex { get { return m_AxisIndex; } set { m_AxisIndex = value; } }
        public List<float> data { get { return m_Data; } set { m_Data = value; } }

        public int filterStart { get; set; }
        public int filterEnd { get; set; }
        public List<float> filterData { get; set; }

        public float Max
        {
            get
            {
                float max = int.MinValue;
                foreach (var data in data)
                {
                    if (data > max)
                    {
                        max = data;
                    }
                }
                return max;
            }
        }

        public float Min
        {
            get
            {
                float min = int.MaxValue;
                foreach (var data in data)
                {
                    if (data < min)
                    {
                        min = data;
                    }
                }
                return min;
            }
        }

        public float Total
        {
            get
            {
                float total = 0;
                foreach (var data in data)
                {
                    total += data;
                }
                return total;
            }
        }

        public void ClearData()
        {
            m_Data.Clear();
        }

        public void RemoveData(int index)
        {
            m_Data.RemoveAt(index);
        }

        public void AddData(float value, int maxDataNumber = 0)
        {
            if (maxDataNumber > 0)
            {
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_Data.Add(value);
        }

        public float GetData(int index, DataZoom dataZoom = null)
        {
            var showData = GetData(dataZoom);
            if (index >= 0 && index <= showData.Count - 1)
            {
                return showData[index];
            }
            return 0;
        }

        public List<float> GetData(DataZoom dataZoom)
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
                    filterData = new List<float>();
                }
            }
        }

        public void UpdateData(int index, float value)
        {
            if (index >= 0 && index <= m_Data.Count - 1)
            {
                m_Data[index] = value;
            }
        }

        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            m_Data = ChartHelper.ParseFloatFromString(jsonData);
        }
    }
}
