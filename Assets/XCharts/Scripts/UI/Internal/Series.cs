using UnityEngine;
using System.Collections.Generic;

namespace XCharts
{
    [System.Serializable]
    public class Series : JsonDataSupport
    {
        [SerializeField] protected List<Serie> m_Series;

        public List<Serie> series { get { return m_Series; } }

        public int Count { get { return m_Series.Count; } }

        public static Series defaultSeries
        {
            get
            {
                var series = new Series
                {
                    m_Series = new List<Serie>(){new Serie(){
                        show  = true,
                        name = "serie1",
                        index = 0
                    }}
                };
                return series;
            }
        }

        public void ClearData()
        {
            foreach (var serie in m_Series)
            {
                serie.ClearData();
            }
        }

        public float GetData(int serieIndex, int dataIndex)
        {
            if (serieIndex >= 0 && serieIndex < Count)
            {
                return m_Series[serieIndex].GetYData(dataIndex);
            }
            else
            {
                return 0;
            }
        }

        public Serie GetSerie(string name)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (name.Equals(m_Series[i].name))
                {
                    m_Series[i].index = i;
                    return m_Series[i];
                }
            }
            return null;
        }

        public List<Serie> GetSeries(string name)
        {
            var list = new List<Serie>();
            if (name == null) return list;
            foreach (var serie in m_Series)
            {
                if (name.Equals(serie.name)) list.Add(serie);
            }
            return list;
        }

        public Serie GetSerie(int index)
        {
            if (index >= 0 && index < m_Series.Count)
            {
                return m_Series[index];
            }
            return null;
        }

        public bool Contains(string name)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (name.Equals(m_Series[i].name))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Remove serie from series.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        public void Remove(string serieName)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                m_Series.Remove(serie);
            }
        }

        /// <summary>
        /// Remove all serie from series.
        /// </summary>
        public void RemoveAll()
        {
            m_Series.Clear();
        }

        public Serie AddSerie(string serieName, SerieType type, bool show = true)
        {
            var serie = GetSerie(serieName);
            if (serie == null)
            {
                serie = new Serie();
                serie.type = type;
                serie.show = show;
                serie.name = serieName;
                serie.index = m_Series.Count;

                if (type == SerieType.Scatter)
                {
                    serie.symbol.type = SerieSymbolType.Circle;
                    serie.symbol.size = 20f;
                    serie.symbol.selectedSize = 30f;
                }
                else if (type == SerieType.Line)
                {
                    serie.symbol.type = SerieSymbolType.EmptyCircle;
                    serie.symbol.size = 2.5f;
                    serie.symbol.selectedSize = 5f;
                }
                else
                {
                    serie.symbol.type = SerieSymbolType.None;
                }
                m_Series.Add(serie);
            }
            else
            {
                serie.show = show;
            }
            return serie;
        }

        public bool AddData(string serieName, float value, int maxDataNumber = 0)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.AddYData(value, maxDataNumber);
                return true;
            }
            return false;
        }

        public bool AddData(int index, float value, int maxDataNumber = 0)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.AddYData(value, maxDataNumber);
                return true;
            }
            return false;
        }

        public bool AddXYData(string serieName, float xValue, float yValue, int maxDataNumber = 0)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.AddXYData(xValue, yValue, maxDataNumber);
                return true;
            }
            return false;
        }

        public bool AddXYData(int index, float xValue, float yValue, int maxDataNumber = 0)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.AddXYData(xValue, yValue, maxDataNumber);
                return true;
            }
            return false;
        }

        public void UpdateData(string name, float value, int dataIndex = 0)
        {
            var serie = GetSerie(name);
            if (serie != null)
            {
                serie.UpdateYData(dataIndex, value);
            }
        }

        public void UpdateXYData(string name, float xValue, float yValue, int dataIndex = 0)
        {
            var serie = GetSerie(name);
            if (serie != null)
            {
                serie.UpdateXYData(dataIndex, xValue, yValue);
            }
        }

        public void UpdateData(int index, float value, int dataIndex = 0)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.UpdateYData(dataIndex, value);
            }
        }

        public void UpdateXYData(int index, float xValue, float yValue, int dataIndex = 0)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.UpdateXYData(dataIndex, xValue, yValue);
            }
        }

        public void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                for (int i = 0; i < m_Series.Count; i++)
                {
                    m_Series[i].UpdateFilterData(dataZoom);
                }
            }
        }

        public bool IsActive(string name)
        {
            var serie = GetSerie(name);
            return serie == null ? false : serie.show;
        }

        public bool IsActive(int index)
        {
            var serie = GetSerie(index);
            return serie == null ? false : serie.show;
        }

        public void SetActive(string name, bool active)
        {
            var serie = GetSerie(name);
            if (serie != null)
            {
                serie.show = active;
            }
        }

        public void SetActive(int index, bool active)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.show = active;
            }
        }

        public bool IsUsedAxisIndex(int axisIndex)
        {
            foreach (var serie in series)
            {
                if (serie.axisIndex == axisIndex) return true;
            }
            return false;
        }

        public bool IsTooltipSelected(int serieIndex)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null) return serie.selected;
            else return false;
        }

        public void GetXMinMaxValue(DataZoom dataZoom, int axisIndex, out int minVaule, out int maxValue)
        {
            GetMinMaxValue(dataZoom, axisIndex, false, out minVaule, out maxValue);
        }

        public void GetYMinMaxValue(DataZoom dataZoom, int axisIndex, out int minVaule, out int maxValue)
        {
            GetMinMaxValue(dataZoom, axisIndex, true, out minVaule, out maxValue);
        }

        public void GetMinMaxValue(DataZoom dataZoom, int axisIndex, bool yValue, out int minVaule, out int maxValue)
        {
            float min = int.MaxValue;
            float max = int.MinValue;
            if (IsStack())
            {
                var stackSeries = GetStackSeries();
                foreach (var ss in stackSeries)
                {
                    var seriesTotalValue = new Dictionary<int, float>();
                    for (int i = 0; i < ss.Value.Count; i++)
                    {
                        var serie = ss.Value[i];
                        if (serie.axisIndex != axisIndex) continue;
                        var showData = yValue ? serie.GetYDataList(dataZoom) : serie.GetXDataList(dataZoom);
                        for (int j = 0; j < showData.Count; j++)
                        {
                            if (!seriesTotalValue.ContainsKey(j))
                                seriesTotalValue[j] = 0;
                            seriesTotalValue[j] = seriesTotalValue[j] + showData[j];
                        }
                    }
                    float tmax = int.MinValue;
                    float tmin = int.MaxValue;
                    foreach (var tt in seriesTotalValue)
                    {
                        if (tt.Value > tmax) tmax = tt.Value;
                        if (tt.Value < tmin) tmin = tt.Value;
                    }
                    if (tmax > max) max = tmax;
                    if (tmin < min) min = tmin;
                }
            }
            else
            {
                for (int i = 0; i < m_Series.Count; i++)
                {
                    if (m_Series[i].axisIndex != axisIndex) continue;
                    if (IsActive(i))
                    {
                        var showData = yValue ? m_Series[i].GetYDataList(dataZoom) : m_Series[i].GetXDataList(dataZoom);
                        foreach (var data in showData)
                        {
                            if (data > max) max = data;
                            if (data < min) min = data;
                        }
                    }
                }
            }
            if (max == int.MinValue && min == int.MaxValue)
            {
                minVaule = 0;
                maxValue = 90;
            }
            else
            {
                minVaule = Mathf.FloorToInt(min);
                maxValue = Mathf.CeilToInt(max);
            }
        }

        public float GetMaxValue(int index)
        {
            float max = int.MinValue;
            float min = int.MaxValue;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var showData = m_Series[i].yData;
                if (showData[index] > max)
                {
                    max = Mathf.Ceil(showData[index]);
                }
                if (showData[index] < min)
                {
                    min = Mathf.Ceil(showData[index]);
                }
            }
            if (max < 1 && max > -1) return max;
            if (max < 0 && min < 0) max = min;
            return ChartHelper.GetMaxDivisibleValue(max);
        }

        public float GetMinValue(int index)
        {
            float max = int.MinValue;
            float min = int.MaxValue;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var showData = m_Series[i].yData;
                if (showData[index] > max)
                {
                    max = Mathf.Ceil(showData[index]);
                }
                if (showData[index] < min)
                {
                    min = Mathf.Ceil(showData[index]);
                }
            }
            if (min < 1 && min > -1) return min;
            if (min < 0 && max < 0) min = max;
            return ChartHelper.GetMinDivisibleValue(min);
        }

        public bool IsStack()
        {
            HashSet<string> sets = new HashSet<string>();
            foreach (var serie in m_Series)
            {
                if (string.IsNullOrEmpty(serie.stack)) continue;
                if (sets.Contains(serie.stack)) return true;
                else
                {
                    sets.Add(serie.stack);
                }
            }
            return false;
        }

        public Dictionary<int, List<Serie>> GetStackSeries()
        {
            int count = 0;
            Dictionary<string, int> sets = new Dictionary<string, int>();
            Dictionary<int, List<Serie>> stackSeries = new Dictionary<int, List<Serie>>();
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                serie.index = i;
                if (string.IsNullOrEmpty(serie.stack))
                {
                    stackSeries[count] = new List<Serie>();
                    stackSeries[count].Add(serie);
                    count++;
                }
                else
                {
                    if (!sets.ContainsKey(serie.stack))
                    {
                        sets.Add(serie.stack, count);
                        stackSeries[count] = new List<Serie>();
                        stackSeries[count].Add(serie);
                        count++;
                    }
                    else
                    {
                        int stackIndex = sets[serie.stack];
                        stackSeries[stackIndex].Add(serie);
                    }
                }
            }
            return stackSeries;
        }

        public List<string> GetSerieNameList()
        {
            var list = new List<string>();
            foreach (var serie in m_Series)
            {
                if (!string.IsNullOrEmpty(serie.name) && !list.Contains(serie.name))
                {
                    list.Add(serie.name);
                }
            }
            return list;
        }

        public void SetSerieSymbolSizeCallback(SymbolSizeCallback size, SymbolSizeCallback selectedSize)
        {
            foreach (var serie in m_Series)
            {
                serie.symbol.sizeCallback = size;
                serie.symbol.selectedSizeCallback = selectedSize;
            }
        }

        public override void ParseJsonData(string jsonData)
        {
            //TODO:
        }
    }
}
