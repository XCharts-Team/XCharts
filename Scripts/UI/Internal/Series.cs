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
            foreach(var serie in m_Series)
            {
                serie.ClearData();
            }
        }

        public float GetData(int serieIndex,int dataIndex)
        {
            if(serieIndex >= 0 && serieIndex < Count)
            {
                return m_Series[serieIndex].GetData(dataIndex);
            }
            else
            {
                return 0;
            }
        }

        public void AddData(string legend, float value, int maxDataNumber = 0)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (m_Series[i].name.Equals(legend))
                {
                    m_Series[i].AddData(value, maxDataNumber);
                    break;
                }
            }
        }

        public void AddData(int legend, float value, int maxDataNumber = 0)
        {
            if (legend >= 0 && legend < Count)
            {
                m_Series[legend].AddData(value, maxDataNumber);
            }
        }

        public void UpdateData(string legend, float value, int dataIndex = 0)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (m_Series[i].name.Equals(legend))
                {
                    m_Series[i].UpdateData(dataIndex, value);
                    break;
                }
            }
        }

        public void UpdateData(int legendIndex, float value, int dataIndex = 0)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                if (i == legendIndex)
                {
                    m_Series[i].UpdateData(dataIndex, value);
                    break;
                }
            }
        }

        public float GetMaxValue(Legend legend)
        {
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
                    float tmax = 0;
                    foreach (var tt in seriesTotalValue)
                    {
                        if (tt.Value > tmax) tmax = tt.Value;
                    }
                    if (tmax > max) max = tmax;
                }
            }
            else
            {
                for (int i = 0; i < m_Series.Count; i++)
                {
                    if (legend.IsShowSeries(i) && m_Series[i].Max > max) max = m_Series[i].Max;
                }
            }
            if (max == int.MinValue) return 100;
            if (max < 1 && max > -1) return max;
            int bigger = (int)Mathf.Abs(max);
            int n = 1;
            while (bigger / (Mathf.Pow(10, n)) > 10)
            {
                n++;
            }
            float mm = bigger < 10 ? bigger : ((bigger - bigger % (Mathf.Pow(10, n))) + Mathf.Pow(10, n));
            if (max < 0) return -mm;
            else return mm;
        }

        public float GetMaxValue(int index,int splitNumber = 0)
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
            int bigger = (int)Mathf.Abs(max);
            int n = 1;
            while (bigger / (Mathf.Pow(10, n)) > 10)
            {
                n++;
            }
            float mm = bigger < 10 ? bigger : ((bigger - bigger % (Mathf.Pow(10, n))) + Mathf.Pow(10, n));
            if (max < 1 && max > -1) return max;
            else if (max < 0) return -mm;
            else return mm;
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
