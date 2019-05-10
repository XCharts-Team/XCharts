using System;
using System.Collections.Generic;
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

        [SerializeField] private bool m_Show = true;
        [SerializeField] private SerieType m_Type;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Stack;
        [SerializeField] private List<float> m_Data = new List<float>();
        [SerializeField] private bool m_Flodout;

        public bool show { get { return m_Show; }set { m_Show = value; } }
        public SerieType type { get { return m_Type; } set { m_Type = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public string stack { get { return m_Stack; } set { m_Stack = value; } }
        public List<float> data { get { return m_Data; } }

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

        public void AddData(float value, int maxDataNumber)
        {
            if (maxDataNumber > 0)
            {
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_Data.Add(value);
        }

        public float GetData(int index)
        {
            if (index >= 0 && index <= data.Count - 1)
            {
                return data[index];
            }
            return 0;
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
