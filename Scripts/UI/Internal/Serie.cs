using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace XCharts
{
    public enum SerieType
    {
        Line,
        Bar,
        Pie,
        Radar,
        Scatter,
        EffectScatter
    }

/// <summary>
/// Whether to show as Nightingale chart, which distinguishs data through radius. 
/// 是否展示成南丁格尔图，通过半径区分数据大小。
/// </summary>
    public enum RoseType
    {
        /// <summary>
        /// Don't show as Nightingale chart.不展示成南丁格尔玫瑰图
        /// </summary>
        None,
        /// <summary>
        /// Use central angle to show the percentage of data, radius to show data size.
        /// 扇区圆心角展现数据的百分比，半径展现数据的大小。
        /// </summary>
        Radius,
        /// <summary>
        /// All the sectors will share the same central angle, the data size is shown only through radiuses.
        /// 所有扇区圆心角相同，仅通过半径展现数据大小。
        /// </summary>
        Area
    }

    [System.Serializable]
    public class Serie : JsonDataSupport
    {
        [SerializeField] [DefaultValue("true")] private bool m_Show = true;
        [SerializeField] private SerieType m_Type;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Stack;
        [SerializeField] [Range(0, 1)] private int m_AxisIndex;
        [SerializeField] private SerieSymbol m_Symbol = new SerieSymbol();
        #region PieChart
        [SerializeField] private bool m_ClickOffset = true;
        [SerializeField] private RoseType m_RoseType = RoseType.None;
        [SerializeField] private float m_Space;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private float[] m_Radius = new float[2] { 0, 80 };
        #endregion
        [SerializeField] private SerieLabel m_Label = new SerieLabel();
        [SerializeField] private SerieLabel m_HighlightLabel = new SerieLabel();
        [SerializeField] [Range(1, 6)] private int m_ShowDataDimension;
        [SerializeField] private bool m_ShowDataName;
        [FormerlySerializedAs("m_Data")]
        [SerializeField] private List<float> m_YData = new List<float>();
        [SerializeField] private List<float> m_XData = new List<float>();
        [SerializeField] private List<SerieData> m_Data = new List<SerieData>();

        public bool show { get { return m_Show; } set { m_Show = value; } }
        public SerieType type { get { return m_Type; } set { m_Type = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public string stack { get { return m_Stack; } set { m_Stack = value; } }
        public int axisIndex { get { return m_AxisIndex; } set { m_AxisIndex = value; } }
        public SerieSymbol symbol { get { return m_Symbol; } set { m_Symbol = value; } }
        public bool clickOffset { get { return m_ClickOffset; } set { m_ClickOffset = value; } }
        /// <summary>
        /// Whether to show as Nightingale chart.
        /// 是否展示成南丁格尔图，通过半径区分数据大小。
        /// </summary>
        public RoseType roseType { get { return m_RoseType; } set { m_RoseType = value; } }
        public float space { get { return m_Space; } set { m_Space = value; } }
        public float[] center { get { return m_Center; } set { m_Center = value; } }
        public float[] radius { get { return m_Radius; } set { m_Radius = value; } }
        /// <summary>
        /// Text label of graphic element,to explain some data information about graphic item like value, name and so on. 
        /// 图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
        /// </summary>
        public SerieLabel label { get { return m_Label; } set { m_Label = value; } }
        public SerieLabel highlightLabel { get { return m_HighlightLabel; } set { m_HighlightLabel = value; } }
        public List<float> yData { get { return m_YData; } }
        public List<float> xData { get { return m_XData; } }
        public List<SerieData> data { get { return m_Data; } }

        /// <summary>
        /// The index of serie,start at 0.
        /// 系列的索引，从0开始。
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// Whether the serie is highlighted.
        /// 该系列是否高亮，一般由图例悬停触发。
        /// </summary>
        public bool highlighted { get; set; }
        public int dataCount { get { return m_Data.Count; } }
        public int filterStart { get; set; }
        public int filterEnd { get; set; }

        private List<float> yFilterData { get; set; }
        private List<float> xFilterData { get; set; }
        private List<SerieData> filterData { get; set; }

        public float yMax
        {
            get
            {
                float max = int.MinValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[1] > max)
                    {
                        max = sdata.data[1];
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
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[0] > max)
                    {
                        max = sdata.data[0];
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
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[1] < min)
                    {
                        min = sdata.data[1];
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
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[0] < min)
                    {
                        min = sdata.data[0];
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
                foreach (var sdata in data)
                {
                    if (sdata.show)
                        total += sdata.data[1];
                }
                return total;
            }
        }

        public float xTotal
        {
            get
            {
                float total = 0;
                foreach (var sdata in data)
                {
                    if (sdata.show)
                        total += sdata.data[0];
                }
                return total;
            }
        }

        public void ClearData()
        {
            m_XData.Clear();
            m_YData.Clear();
            m_Data.Clear();
        }

        public void RemoveData(int index)
        {
            m_XData.RemoveAt(index);
            m_YData.RemoveAt(index);
            m_Data.RemoveAt(index);
        }

        public void AddYData(float value, string dataName = null, int maxDataNumber = 0)
        {
            if (maxDataNumber > 0)
            {
                while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            int xValue = m_XData.Count;
            m_XData.Add(xValue);
            m_YData.Add(value);
            m_Data.Add(new SerieData() { data = new List<float>() { xValue, value }, name = dataName });
        }

        public void AddXYData(float xValue, float yValue, string dataName = null, int maxDataNumber = 0)
        {
            if (maxDataNumber > 0)
            {
                while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_XData.Add(xValue);
            m_YData.Add(yValue);
            m_Data.Add(new SerieData() { data = new List<float>() { xValue, yValue }, name = dataName });
        }

        public void AddData(List<float> valueList, string dataName = null, int maxDataNumber = 0)
        {
            if (valueList == null || valueList.Count == 0) return;
            if (valueList.Count == 1)
            {
                AddYData(valueList[0], dataName, maxDataNumber);
            }
            else if (valueList.Count == 2)
            {
                AddXYData(valueList[0], valueList[1], dataName, maxDataNumber);
            }
            else
            {
                if (maxDataNumber > 0)
                {
                    while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                    while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
                    while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
                }
                var serieData = new SerieData();
                serieData.name = dataName;
                for (int i = 0; i < valueList.Count; i++)
                {
                    if (i == 0) m_XData.Add(valueList[i]);
                    else if (i == 1) m_YData.Add(valueList[i]);
                    serieData.data.Add(valueList[0]);
                }
                m_Data.Add(serieData);
            }
        }

        public float GetYData(int index, DataZoom dataZoom = null)
        {
            if (index < 0) return 0;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                return serieData[index].data[1];
            }
            return 0;
        }

        public void GetYData(int index, out float yData, out string dataName, DataZoom dataZoom = null)
        {
            yData = 0;
            dataName = null;
            if (index < 0) return;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                yData = serieData[index].data[1];
                dataName = serieData[index].name;
            }
        }

        public SerieData GetSerieData(int index, DataZoom dataZoom = null)
        {
            var data = GetDataList(dataZoom);
            if (index >= 0 && index <= data.Count - 1)
            {
                return data[index];
            }
            return null;
        }

        public void GetXYData(int index, DataZoom dataZoom, out float xValue, out float yVlaue)
        {
            xValue = 0;
            yVlaue = 0;
            if (index < 0) return;
            var showData = GetDataList(dataZoom);
            if (index < showData.Count)
            {
                var serieData = showData[index];
                xValue = serieData.data[0];
                yVlaue = serieData.data[1];
            }
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

        public List<SerieData> GetDataList(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((m_Data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((m_Data.Count - 1) * dataZoom.end / 100);
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
                var startIndex = (int)((yData.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((yData.Count - 1) * dataZoom.end / 100);
                if (startIndex != filterStart || endIndex != filterEnd)
                {
                    filterStart = startIndex;
                    filterEnd = endIndex;
                    var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                    if (m_YData.Count > 0)
                    {
                        yFilterData = m_YData.GetRange(startIndex, count);
                    }
                    else
                    {
                        yFilterData = m_YData;
                    }
                    if (m_XData.Count > 0)
                    {
                        xFilterData = m_XData.GetRange(startIndex, count);
                    }
                    else
                    {
                        xFilterData = m_XData;
                    }
                    if (m_Data.Count > 0)
                    {
                        filterData = m_Data.GetRange(startIndex, count);
                    }
                    else
                    {
                        filterData = m_Data;
                    }
                }
                else if (endIndex == 0)
                {
                    yFilterData = new List<float>();
                    xFilterData = new List<float>();
                    filterData = new List<SerieData>();
                }
            }
        }

        public void UpdateYData(int index, float value)
        {
            UpdateData(index, 2, value);
        }

        public void UpdateXYData(int index, float xValue, float yValue)
        {
            UpdateData(index, 1, xValue);
            UpdateData(index, 2, yValue);
        }

        public void UpdateData(int index, int dimension, float value)
        {
            if (index < 0) return;
            if (dimension == 1)
            {
                if (index < m_XData.Count) m_XData[index] = value;
            }
            else if (dimension == 2)
            {
                if (index < m_YData.Count) m_YData[index] = value;
            }
            if (index < m_Data.Count && dimension < m_Data[index].data.Count)
            {
                m_Data[index].data[dimension] = value;
            }
        }

        public void ClearHighlight()
        {
            highlighted = false;
            foreach (var sd in m_Data)
            {
                sd.highlighted = false;
            }
        }

        public void SetHighlight(int index)
        {
            if (index <= 0) return;
            for (int i = 0; i < m_Data.Count; i++)
            {
                m_Data[i].highlighted = index == i;
            }
        }

        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            jsonData = jsonData.Replace("\r\n", "");
            jsonData = jsonData.Replace(" ", "");
            jsonData = jsonData.Replace("\n", "");
            int startIndex = jsonData.IndexOf("[");
            int endIndex = jsonData.LastIndexOf("]");
            if (startIndex == -1 || endIndex == -1)
            {
                Debug.LogError("json data need include in [ ]");
                return;
            }
            ClearData();
            string temp = jsonData.Substring(startIndex + 1, endIndex - startIndex - 1);
            if (temp.IndexOf("],") > -1 || temp.IndexOf("] ,") > -1)
            {
                string[] datas = temp.Split(new string[] { "],", "] ," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var data = datas[i].Split(new char[] { '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var serieData = new SerieData();
                    for (int j = 0; j < data.Length; j++)
                    {
                        var txt = data[j].Trim().Replace("]", "");
                        float value;
                        var flag = float.TryParse(txt, out value);
                        if (flag)
                        {
                            serieData.data.Add(value);
                            if (j == 0) m_XData.Add(value);
                            else if (j == 1) m_YData.Add(value);
                        }
                        else serieData.name = txt.Replace("\"", "").Trim();
                    }
                    m_Data.Add(serieData);
                }
            }
            else if (temp.IndexOf("value") > -1 && temp.IndexOf("name") > -1)
            {
                string[] datas = temp.Split(new string[] { "},", "} ,", "}" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var arr = datas[i].Replace("{", "").Split(',');
                    var serieData = new SerieData();
                    foreach (var a in arr)
                    {
                        if (a.StartsWith("value:"))
                        {
                            float value = float.Parse(a.Substring(6, a.Length - 6));
                            serieData.data = new List<float>() { i, value };
                        }
                        else if (a.StartsWith("name:"))
                        {
                            string name = a.Substring(6, a.Length - 6 - 1);
                            serieData.name = name;
                        }
                        else if (a.StartsWith("selected:"))
                        {
                            string selected = a.Substring(9, a.Length - 9);
                            serieData.selected = bool.Parse(selected);
                        }
                    }
                    m_Data.Add(serieData);
                }
            }
            else
            {
                string[] datas = temp.Split(',');
                for (int i = 0; i < datas.Length; i++)
                {
                    float value;
                    var flag = float.TryParse(datas[i].Trim(), out value);
                    if (flag)
                    {
                        var serieData = new SerieData();
                        serieData.data = new List<float>() { i, value };
                        m_Data.Add(serieData);
                    }
                }
            }
        }
    }
}
