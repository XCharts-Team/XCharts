using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

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
                    m_Series = new List<Serie>()
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
                return m_Series[serieIndex].GetData(dataIndex);
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
                    return m_Series[i];
                }
            }
            return null;
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

        public void RemoveData(string name)
        {
            var serie = GetSerie(name);
            if (serie != null)
            {
                m_Series.Remove(serie);
            }
        }

        public Serie AddData(string name, float value, int maxDataNumber = 0)
        {
            if (m_Series == null)
            {
                m_Series = new List<Serie>();
            }
            var serie = GetSerie(name);
            if (serie == null)
            {
                serie = new Serie();
                serie.name = name;
                serie.data = new List<float>();
                m_Series.Add(serie);
            }
            serie.AddData(value, maxDataNumber);
            return serie;
        }

        public Serie AddData(int index, float value, int maxDataNumber = 0)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.AddData(value, maxDataNumber);
            }
            return serie;
        }

        public void UpdateData(string name, float value, int dataIndex = 0)
        {
            var serie = GetSerie(name);
            if (serie != null)
            {
                serie.UpdateData(dataIndex, value);
            }
        }

        public void UpdateData(int index, float value, int dataIndex = 0)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.UpdateData(dataIndex, value);
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

        public void GetMinMaxValue(Legend legend, out int minVaule, out int maxValue)
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
                        for (int j = 0; j < serie.data.Count; j++)
                        {
                            if (!seriesTotalValue.ContainsKey(j))
                                seriesTotalValue[j] = 0;
                            seriesTotalValue[j] = seriesTotalValue[j] + serie.data[j];
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
                    if (legend.IsActive(i))
                    {
                        if (m_Series[i].Max > max) max = m_Series[i].Max;
                        if (m_Series[i].Min < min) min = m_Series[i].Min;
                    }
                }
            }
            if (max == int.MinValue && min == int.MaxValue)
            {
                minVaule = 0;
                maxValue = 100;
            }
            else if (max > 0 && min > 0)
            {
                minVaule = 0;
                maxValue = ChartHelper.GetMaxDivisibleValue(max);
            }
            else if (min < 0 && max < 0)
            {
                minVaule = ChartHelper.GetMaxDivisibleValue(min);
                maxValue = 0;
            }
            else
            {
                minVaule = ChartHelper.GetMaxDivisibleValue(min);
                maxValue = ChartHelper.GetMaxDivisibleValue(max);
            }
        }

        public float GetMaxValue(int index, int splitNumber = 0)
        {
            float max = int.MinValue;
            float min = int.MaxValue;
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (m_Series[i].data[index] > max)
                {
                    max = Mathf.Ceil(m_Series[i].data[index]);
                }
                if (m_Series[i].data[index] < min)
                {
                    min = Mathf.Ceil(m_Series[i].data[index]);
                }
            }
            if (max < 1 && max > -1) return max;
            if (max < 0 && min < 0) max = min;
            return ChartHelper.GetMaxDivisibleValue(max);
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
            foreach (var serie in m_Series)
            {
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

        public override void ParseJsonData(string jsonData)
        {
            //TODO:
        }
    }
}
