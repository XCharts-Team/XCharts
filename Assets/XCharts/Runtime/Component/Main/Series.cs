/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace XCharts
{
    /// <summary>
    /// the list of series.
    /// 系列列表。每个系列通过 type 决定自己的图表类型。
    /// </summary>
    [System.Serializable]
    public class Series : MainComponent
    {

        [SerializeField] protected List<Serie> m_Series;

        [Obsolete("Use Series.list instead.", true)]
        public List<Serie> series { get { return m_Series; } }

        /// <summary>
        /// the list of serie
        /// 系列列表。
        /// </summary>
        public List<Serie> list { get { return m_Series; } }
        /// <summary>
        /// the size of serie list.
        /// 系列个数。
        /// </summary>
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

        /// <summary>
        /// 清空所有系列的数据
        /// </summary>
        public void ClearData()
        {
            AnimationStop();
            foreach (var serie in m_Series)
            {
                serie.ClearData();
            }
        }

        /// <summary>
        /// 获得指定序列指定索引的数据值
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获得指定系列名的第一个系列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Serie GetSerie(string name)
        {
            for (int i = 0; i < m_Series.Count; i++)
            {
                bool match = false;
                if (string.IsNullOrEmpty(name))
                {
                    if (string.IsNullOrEmpty(m_Series[i].name)) match = true;
                }
                else if (name.Equals(m_Series[i].name))
                {
                    match = true;
                }
                if (match)
                {
                    m_Series[i].index = i;
                    return m_Series[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 获得指定系列名的所有系列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获得指定索引的系列
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Serie GetSerie(int index)
        {
            if (index >= 0 && index < m_Series.Count)
            {
                return m_Series[index];
            }
            return null;
        }

        /// <summary>
        /// 获得上一个同堆叠且显示的serie。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Serie GetLastStackSerie(int index)
        {
            var serie = GetSerie(index);
            return GetLastStackSerie(serie);
        }

        /// <summary>
        /// 同堆叠的serie是否有渐变色的。
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public bool IsAnyGradientSerie(string stack)
        {
            if (string.IsNullOrEmpty(stack)) return false;
            foreach (var serie in m_Series)
            {
                if (serie.show && serie.areaStyle.show && stack.Equals(serie.stack))
                {
                    if (serie.areaStyle.color != serie.areaStyle.toColor && serie.areaStyle.toColor != Color.clear) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得上一个同堆叠且显示的serie。
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public Serie GetLastStackSerie(Serie serie)
        {
            if (serie == null || string.IsNullOrEmpty(serie.stack)) return null;
            for (int i = serie.index - 1; i >= 0; i--)
            {
                var temp = m_Series[i];
                if (temp.show && serie.stack.Equals(temp.stack)) return temp;
            }
            return null;
        }

        /// <summary>
        /// 是否包含指定名字的系列
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
        /// 移除指定名字的系列。
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
        /// 移除所有系列。
        /// </summary>
        public void RemoveAll()
        {
            AnimationStop();
            m_Series.Clear();
        }

        /// <summary>
        /// 添加一个系列到列表中。
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="type"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        public Serie AddSerie(SerieType type, string serieName, bool show = true)
        {
            var serie = new Serie();
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
            serie.animation.Reset();
            m_Series.Add(serie);
            return serie;
        }

        /// <summary>
        /// 添加一个数据到指定系列的维度Y数据中
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="value"></param>
        /// <param name="dataName"></param>
        /// <returns>添加成功返回SerieData，否则返回null</returns>
        public SerieData AddData(string serieName, float value, string dataName = null)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.AddYData(value, dataName);
            }
            return null;
        }

        /// <summary>
        /// 添加一个数据到指定系列的维度Y中
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <param name="dataName"></param>
        /// <returns>添加成功返回SerieData，否则返回null</returns>
        public SerieData AddData(int index, float value, string dataName = null)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                return serie.AddYData(value, dataName);
            }
            return null;
        }

        /// <summary>
        /// 添加一组数据到指定的系列中
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="multidimensionalData"></param>
        /// <param name="dataName"></param>
        /// <returns>添加成功返回SerieData，否则返回null</returns>
        public SerieData AddData(string serieName, List<float> multidimensionalData, string dataName = null)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.AddData(multidimensionalData, dataName);
            }
            return null;
        }

        /// <summary>
        /// 添加一组数据到指定的系列中
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="multidimensionalData"></param>
        /// <param name="dataName"></param>
        /// <returns>添加成功返回SerieData，否则返回null</returns>
        public SerieData AddData(int serieIndex, List<float> multidimensionalData, string dataName = null)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                return serie.AddData(multidimensionalData, dataName);
            }
            return null;
        }

        /// <summary>
        /// 添加(x,y)数据到指定的系列中
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        /// <param name="dataName"></param>
        /// <returns>添加成功返回SerieData，否则返回null</returns>
        public SerieData AddXYData(string serieName, float xValue, float yValue, string dataName = null)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.AddXYData(xValue, yValue, dataName);
            }
            return null;
        }

        /// <summary>
        /// 添加(x,y)数据到指定的系列中
        /// </summary>
        /// <param name="index"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        /// <param name="dataName"></param>
        /// <returns>添加成功返回SerieData，否则返回null</returns>
        public SerieData AddXYData(int index, float xValue, float yValue, string dataName = null)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                return serie.AddXYData(xValue, yValue, dataName);
            }
            return null;
        }

        /// <summary>
        /// 更新指定系列的维度Y数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dataIndex"></param>
        public void UpdateData(string serieName, int dataIndex, float value)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.UpdateYData(dataIndex, value);
            }
        }

        /// <summary>
        /// 更新指定系列的数据项名称
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dataName"></param>
        public void UpdateDataName(string serieName, int dataIndex, string dataName)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.UpdateDataName(dataIndex, dataName);
            }
        }

        /// <summary>
        /// 更新指定系列的数据项名称
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dataName"></param>
        public void UpdateDataName(int serieIndex, int dataIndex, string dataName)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                serie.UpdateDataName(dataIndex, dataName);
            }
        }

        /// <summary>
        /// 更新指定系列的维度Y数据项的值
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="value"></param>
        public void UpdateData(int serieIndex, int dataIndex, float value)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                serie.UpdateYData(dataIndex, value);
            }
        }


        /// <summary>
        /// 更新指定系列的维度X和维度Y数据
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public void UpdateXYData(string serieName, int dataIndex, float xValue, float yValue)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.UpdateXYData(dataIndex, xValue, yValue);
            }
        }

        /// <summary>
        /// 更新指定系列的维度X和维度Y数据
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public void UpdateXYData(int serieIndex, int dataIndex, float xValue, float yValue)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                serie.UpdateXYData(dataIndex, xValue, yValue);
            }
        }

        /// <summary>
        /// dataZoom由变化是更新系列的缓存数据
        /// </summary>
        /// <param name="dataZoom"></param>
        public void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.enable)
            {
                for (int i = 0; i < m_Series.Count; i++)
                {
                    m_Series[i].UpdateFilterData(dataZoom);
                }
            }
        }

        /// <summary>
        /// 指定系列是否显示
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsActive(string name)
        {
            var serie = GetSerie(name);
            return serie == null ? false : serie.show;
        }

        /// <summary>
        /// 指定系列是否显示
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsActive(int index)
        {
            var serie = GetSerie(index);
            return serie == null ? false : serie.show;
        }

        /// <summary>
        /// 设置指定系列是否显示
        /// </summary>
        /// <param name="name"></param>
        /// <param name="active"></param>
        public void SetActive(string name, bool active)
        {
            var serie = GetSerie(name);
            if (serie != null)
            {
                serie.show = active;
            }
        }

        /// <summary>
        /// 设置指定系列是否显示
        /// </summary>
        /// <param name="index"></param>
        /// <param name="active"></param>
        public void SetActive(int index, bool active)
        {
            var serie = GetSerie(index);
            if (serie != null)
            {
                serie.show = active;
            }
        }

        /// <summary>
        /// 是否由系列在用指定索引的axis
        /// </summary>
        /// <param name="axisIndex"></param>
        /// <returns></returns>
        public bool IsUsedAxisIndex(int axisIndex)
        {
            foreach (var serie in list)
            {
                if (serie.axisIndex == axisIndex) return true;
            }
            return false;
        }

        /// <summary>
        /// 指定系列是否处于高亮选中状态
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <returns></returns>
        public bool IsHighlight(int serieIndex)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null) return serie.highlighted;
            else return false;
        }

        /// <summary>
        /// 获得维度X的最大最小值
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <param name="axisIndex"></param>
        /// <param name="minVaule"></param>
        /// <param name="maxValue"></param>
        public void GetXMinMaxValue(DataZoom dataZoom, int axisIndex, bool isValueAxis,
            out float minVaule, out float maxValue)
        {
            GetMinMaxValue(dataZoom, axisIndex, isValueAxis, false, out minVaule, out maxValue);
        }

        /// <summary>
        /// 获得维度Y的最大最小值
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <param name="axisIndex"></param>
        /// <param name="minVaule"></param>
        /// <param name="maxValue"></param>
        public void GetYMinMaxValue(DataZoom dataZoom, int axisIndex, bool isValueAxis,
            out float minVaule, out float maxValue)
        {
            GetMinMaxValue(dataZoom, axisIndex, isValueAxis, true, out minVaule, out maxValue);
        }

        private Dictionary<int, List<Serie>> _stackSeriesForMinMax = new Dictionary<int, List<Serie>>();
        private Dictionary<int, float> _serieTotalValueForMinMax = new Dictionary<int, float>();
        public void GetMinMaxValue(DataZoom dataZoom, int axisIndex, bool isValueAxis, bool yValue,
            out float minVaule, out float maxValue)
        {
            float min = int.MaxValue;
            float max = int.MinValue;
            var isPercentStack = IsPercentStack(SerieType.Bar);
            if (!IsStack() || (isValueAxis && !yValue))
            {
                for (int i = 0; i < m_Series.Count; i++)
                {
                    var serie = m_Series[i];
                    if (serie.axisIndex != axisIndex) continue;

                    if (IsActive(i))
                    {
                        if (isPercentStack && IsPercentStack(serie.name, SerieType.Bar))
                        {
                            if (100 > max) max = 100;
                            if (0 < min) min = 0;
                        }
                        else
                        {
                            var showData = m_Series[i].GetDataList(dataZoom);
                            foreach (var data in showData)
                            {
                                if (yValue)
                                {
                                    if (data.data[1] > max) max = data.data[1];
                                    if (data.data[1] < min) min = data.data[1];
                                }
                                else
                                {
                                    if (data.data[0] > max) max = data.data[0];
                                    if (data.data[0] < min) min = data.data[0];
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                GetStackSeries(ref _stackSeriesForMinMax);
                foreach (var ss in _stackSeriesForMinMax)
                {
                    _serieTotalValueForMinMax.Clear();
                    for (int i = 0; i < ss.Value.Count; i++)
                    {
                        var serie = ss.Value[i];
                        if (serie.axisIndex != axisIndex || !IsActive(i)) continue;
                        var showData = serie.GetDataList(dataZoom);
                        if (IsPercentStack(serie.stack, SerieType.Bar))
                        {
                            for (int j = 0; j < showData.Count; j++)
                            {
                                _serieTotalValueForMinMax[j] = 100;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < showData.Count; j++)
                            {
                                if (!_serieTotalValueForMinMax.ContainsKey(j))
                                    _serieTotalValueForMinMax[j] = 0;
                                _serieTotalValueForMinMax[j] = _serieTotalValueForMinMax[j] +
                                    (yValue ? showData[j].data[1] : showData[i].data[0]);
                            }
                        }

                    }
                    float tmax = int.MinValue;
                    float tmin = int.MaxValue;
                    foreach (var tt in _serieTotalValueForMinMax)
                    {
                        if (tt.Value > tmax) tmax = tt.Value;
                        if (tt.Value < tmin) tmin = tt.Value;
                    }
                    if (tmax > max) max = tmax;
                    if (tmin < min) min = tmin;
                }
            }
            if (max == int.MinValue && min == int.MaxValue)
            {
                minVaule = 0;
                maxValue = 0;
            }
            else
            {
                if (max > 1)
                {
                    minVaule = Mathf.FloorToInt(min);
                    maxValue = Mathf.CeilToInt(max);
                }else{
                    minVaule = min;
                    maxValue = max;
                }
            }
        }

        private HashSet<string> _setForStack = new HashSet<string>();
        /// <summary>
        /// 是否由数据堆叠
        /// </summary>
        /// <returns></returns>
        public bool IsStack()
        {
            _setForStack.Clear();
            foreach (var serie in m_Series)
            {
                if (string.IsNullOrEmpty(serie.stack)) continue;
                if (_setForStack.Contains(serie.stack)) return true;
                else
                {
                    _setForStack.Add(serie.stack);
                }
            }
            return false;
        }

        /// <summary>
        /// 是否堆叠
        /// </summary>
        /// <param name="stackName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsStack(string stackName, SerieType type)
        {
            if (string.IsNullOrEmpty(stackName)) return false;
            int count = 0;
            foreach (var serie in m_Series)
            {
                if (serie.show && serie.type == type)
                {
                    if (stackName.Equals(serie.stack)) count++;
                    if (count >= 2) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否时百分比堆叠
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsPercentStack(SerieType type)
        {
            int count = 0;
            bool isPercentStack = false;
            foreach (var serie in m_Series)
            {
                if (serie.show && serie.type == type)
                {
                    if (!string.IsNullOrEmpty(serie.stack))
                    {
                        count++;
                        if (serie.barPercentStack) isPercentStack = true;
                    }
                    if (count >= 2 && isPercentStack) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否时百分比堆叠
        /// </summary>
        /// <param name="stackName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool IsPercentStack(string stackName, SerieType type)
        {
            if (string.IsNullOrEmpty(stackName)) return false;
            int count = 0;
            bool isPercentStack = false;
            foreach (var serie in m_Series)
            {
                if (serie.show && serie.type == type)
                {
                    if (stackName.Equals(serie.stack))
                    {
                        count++;
                        if (serie.barPercentStack) isPercentStack = true;
                    }
                    if (count >= 2 && isPercentStack) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得堆叠系列列表
        /// </summary>
        /// <returns></returns>
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

        private Dictionary<string, int> sets = new Dictionary<string, int>();
        /// <summary>
        /// 获得堆叠系列列表
        /// </summary>
        /// <param name="Dictionary<int"></param>
        /// <param name="stackSeries"></param>
        public void GetStackSeries(ref Dictionary<int, List<Serie>> stackSeries)
        {
            int count = 0;
            sets.Clear();
            if (stackSeries == null)
            {
                stackSeries = new Dictionary<int, List<Serie>>(m_Series.Count);
            }
            else
            {
                foreach (var kv in stackSeries)
                {
                    kv.Value.Clear();
                }
            }
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                serie.index = i;
                if (string.IsNullOrEmpty(serie.stack))
                {
                    if (!stackSeries.ContainsKey(count))
                        stackSeries[count] = new List<Serie>(m_Series.Count);
                    stackSeries[count].Add(serie);
                    count++;
                }
                else
                {
                    if (!sets.ContainsKey(serie.stack))
                    {
                        sets.Add(serie.stack, count);
                        if (!stackSeries.ContainsKey(count))
                            stackSeries[count] = new List<Serie>(m_Series.Count);
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
        }

        private List<string> serieNameList = new List<string>();
        /// <summary>
        /// 获得所有系列名，不包含空名字。
        /// </summary>
        /// <returns></returns>
        public List<string> GetSerieNameList()
        {
            serieNameList.Clear();
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series[n];
                switch (serie.type)
                {
                    case SerieType.Pie:
                    case SerieType.Radar:
                        for (int i = 0; i < serie.data.Count; i++)
                        {
                            if (string.IsNullOrEmpty(serie.data[i].name))
                                serieNameList.Add(ChartCached.IntToStr(i));
                            else if (!serieNameList.Contains(serie.data[i].name))
                                serieNameList.Add(serie.data[i].name);
                        }
                        break;
                    default:
                        if (string.IsNullOrEmpty(serie.name))
                            serieNameList.Add(ChartCached.IntToStr(n));
                        else if (!serieNameList.Contains(serie.name))
                            serieNameList.Add(serie.name);
                        break;
                }
            }
            return serieNameList;
        }

        /// <summary>
        /// 设置获得标志图形大小的回调
        /// </summary>
        /// <param name="size"></param>
        /// <param name="selectedSize"></param>
        public void SetSerieSymbolSizeCallback(SymbolSizeCallback size, SymbolSizeCallback selectedSize)
        {
            foreach (var serie in m_Series)
            {
                serie.symbol.sizeCallback = size;
                serie.symbol.selectedSizeCallback = selectedSize;
            }
        }

        /// <summary>
        /// 启用或取消初始动画
        /// </summary>
        public void AnimationEnable(bool flag)
        {
            foreach (var serie in m_Series)
            {
                serie.animation.enable = flag;
            }
        }

        /// <summary>
        /// 开始初始动画
        /// </summary>
        public void AnimationStart()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable)
                {
                    serie.animation.Start();
                }
            }
        }

        /// <summary>
        /// 停止初始动画
        /// </summary>
        public void AnimationStop()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable) serie.animation.Stop();
            }
        }

        /// <summary>
        /// 重置初始动画
        /// </summary>
        public void AnimationReset()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable) serie.animation.Reset();
            }
        }

        public bool IsLegalLegendName(string name)
        {
            int numName = -1;
            if (int.TryParse(name, out numName))
            {
                if (numName >= 0 && numName < 100) return false;
            }
            return true;
        }

        /// <summary>
        /// 从json中解析数据
        /// </summary>
        /// <param name="jsonData"></param>
        public override void ParseJsonData(string jsonData)
        {
            //TODO:
        }
    }
}
