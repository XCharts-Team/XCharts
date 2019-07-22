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

    public enum SerieSymbolType
    {
        EmptyCircle,
        Circle,
        Rect,
        Triangle,
        Diamond,
        None,
    }

    /// <summary>
    /// The way to get serie symbol size.
    /// <para> `Custom`:Specify constant for symbol size. </para>
    /// <para> `FromData`:Specify the dataIndex and dataScale to calculate symbol size,the formula:data[dataIndex]*dataScale. </para>
    /// <para> `Callback`:Specify callback function for symbol size. </para>
    /// </summary>
    public enum SerieSymbolSizeType
    {
        /// <summary>
        /// Specify constant for symbol size.
        /// </summary>
        Custom,
        /// <summary>
        /// Specify the dataIndex and dataScale to calculate symbol size
        /// </summary>
        FromData,
        /// <summary>
        /// Specify callback function for symbol size
        /// </summary>
        Callback,
    }

    [System.Serializable]
    public class SerieData
    {
        [SerializeField] private string m_Name;
        [SerializeField] private List<float> m_Data = new List<float>();

        public string name { get { return m_Name; } set { m_Name = value; } }
        public List<float> data { get { return m_Data; } set { m_Data = value; } }
    }

    public delegate float SymbolSizeCallback(List<float> data);

    [System.Serializable]
    public class SerieSymbol
    {
        [SerializeField] private SerieSymbolType m_Type = SerieSymbolType.EmptyCircle;
        [SerializeField] private SerieSymbolSizeType m_SizeType = SerieSymbolSizeType.Custom;
        [SerializeField] private float m_Size = 20f;
        [SerializeField] private float m_SelectedSize = 30f;
        [SerializeField] private int m_DataIndex = 1;
        [SerializeField] private float m_DataScale = 1;
        [SerializeField] private float m_SelectedDataScale = 1.5f;
        [SerializeField] private SymbolSizeCallback m_SizeCallback;
        [SerializeField] private SymbolSizeCallback m_SelectedSizeCallback;

        public SerieSymbolType type { get { return m_Type; } set { m_Type = value; } }
        public float size { get { return m_Size; } set { m_Size = value; } }
        public float selectedSize { get { return m_SelectedSize; } set { m_SelectedSize = value; } }
        public int dataIndex { get { return m_DataIndex; } set { m_DataIndex = value; } }
        public float dataScale { get { return m_DataScale; } set { m_DataScale = value; } }
        public float selectedDataScale { get { return m_SelectedDataScale; } set { m_SelectedDataScale = value; } }
        public SymbolSizeCallback sizeCallback { get { return m_SizeCallback; } set { m_SizeCallback = value; } }
        public SymbolSizeCallback selectedSizeCallback { get { return m_SelectedSizeCallback; } set { m_SelectedSizeCallback = value; } }

        private List<float> m_AnimationSize = new List<float>(){0,5,10};
        public List<float> animationSize { get{return m_AnimationSize;}}
        public Color animationColor { get; set; }

        public float GetSize(List<float> data)
        {
            if(data == null) return size;
            switch (m_SizeType)
            {
                case SerieSymbolSizeType.Custom:
                    return size;
                case SerieSymbolSizeType.FromData:
                    if (dataIndex >= 0 && dataIndex < data.Count)
                    {
                        return data[dataIndex] * m_DataScale;
                    }
                    else
                    {
                        return size;
                    }
                case SerieSymbolSizeType.Callback:
                    if (sizeCallback != null) return sizeCallback(data);
                    else return size;
                default: return size;
            }
        }

        public float GetSelectedSize(List<float> data)
        {
            if(data == null) return selectedSize;
            switch (m_SizeType)
            {
                case SerieSymbolSizeType.Custom:
                    return selectedSize;
                case SerieSymbolSizeType.FromData:
                    if (dataIndex >= 0 && dataIndex < data.Count)
                    {
                        return data[dataIndex] * m_SelectedDataScale;
                    }
                    else
                    {
                        return selectedSize;
                    }
                case SerieSymbolSizeType.Callback:
                    if (selectedSizeCallback != null) return selectedSizeCallback(data);
                    else return selectedSize;
                default: return selectedSize;
            }
        }
    }
    [System.Serializable]
    public class Serie : JsonDataSupport
    {
        [SerializeField] [DefaultValue("true")] private bool m_Show = true;
        [SerializeField] private SerieType m_Type;
        [SerializeField] private bool m_Selected;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Stack;
        [SerializeField] private int m_AxisIndex;
        [SerializeField] private SerieSymbol m_Symbol = new SerieSymbol();

        [SerializeField] private int m_ShowDataDimension;
        [SerializeField] private bool m_ShowDataName;
        [FormerlySerializedAs("m_Data")]
        [SerializeField] private List<float> m_YData = new List<float>();
        [SerializeField] private List<float> m_XData = new List<float>();
        [SerializeField] private List<SerieData> m_Data = new List<SerieData>();

        public int index { get; set; }
        public int dataCount { get { return m_Data.Count; } }
        public bool selected { get { return m_Selected; } set { m_Selected = value; } }
        public bool show { get { return m_Show; } set { m_Show = value; } }
        public SerieType type { get { return m_Type; } set { m_Type = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public string stack { get { return m_Stack; } set { m_Stack = value; } }
        public int axisIndex { get { return m_AxisIndex; } set { m_AxisIndex = value; } }
        public SerieSymbol symbol { get { return m_Symbol; } set { m_Symbol = value; } }
        public List<float> yData { get { return m_YData; } }
        public List<float> xData { get { return m_XData; } }
        public List<SerieData> data { get { return m_Data; } }

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
            m_Data.Clear();
        }

        public void RemoveData(int index)
        {
            m_XData.RemoveAt(index);
            m_YData.RemoveAt(index);
            m_Data.RemoveAt(index);
        }

        public void AddYData(float value, int maxDataNumber = 0, string dataName = null)
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

        public void AddXYData(float xValue, float yValue, int maxDataNumber = 0, string dataName = null)
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

        public float GetYData(int index, DataZoom dataZoom = null)
        {
            var showData = GetYDataList(dataZoom);
            if (index >= 0 && index <= showData.Count - 1)
            {
                return showData[index];
            }
            return 0;
        }

         public SerieData GetSerieData(int index,DataZoom dataZoom = null){
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
            ClearData();
            jsonData = jsonData.Replace("\r\n", "");
            jsonData = jsonData.Replace(" ", "");
            jsonData = jsonData.Replace("\n", "");
            int startIndex = jsonData.IndexOf("[");
            int endIndex = jsonData.LastIndexOf("]");
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

                        serieData.data.Add(value);
                        m_Data.Add(serieData);
                        m_XData.Add(value);
                    }
                }
            }
        }
    }
}
