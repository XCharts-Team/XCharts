using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XCharts.Runtime
{
    public partial class BaseChart
    {
        public T AddSerie<T>(string serieName = null, bool show = true, bool addToHead = false) where T : Serie
        {
            if (!CanAddSerie<T>()) return null;
            var index = -1;
            var serie = InsertSerie(index, typeof(T), serieName, show, addToHead) as T;
            CreateSerieHandler(serie);
            return serie;
        }

        public T InsertSerie<T>(int index, string serieName = null, bool show = true) where T : Serie
        {
            if (!CanAddSerie<T>()) return null;
            var serie = InsertSerie(index, typeof(T), serieName, show) as T;
            InitSerieHandlers();
            return serie;
        }

        public void InsertSerie(Serie serie, int index = -1, bool addToHead = false)
        {
            serie.AnimationRestart();
            AnimationStyleHelper.UpdateSerieAnimation(serie);
            if (addToHead) m_Series.Insert(0, serie);
            else if (index >= 0) m_Series.Insert(index, serie);
            else m_Series.Add(serie);
            ResetSeriesIndex();
            SeriesHelper.UpdateSerieNameList(this, ref m_LegendRealShowName);
        }

        public bool MoveUpSerie(int serieIndex)
        {
            if (serieIndex < 0 || serieIndex > m_Series.Count - 1) return false;
            if (serieIndex == 0) return false;
            var up = GetSerie(serieIndex - 1);
            var temp = GetSerie(serieIndex);
            m_Series[serieIndex - 1] = temp;
            m_Series[serieIndex] = up;
            ResetSeriesIndex();
            InitSerieHandlers();
            RefreshChart();
            return true;
        }

        public bool MoveDownSerie(int serieIndex)
        {
            if (serieIndex < 0 || serieIndex > m_Series.Count - 1) return false;
            if (serieIndex == m_Series.Count - 1) return false;
            var down = GetSerie(serieIndex + 1);
            var temp = GetSerie(serieIndex);
            m_Series[serieIndex + 1] = temp;
            m_Series[serieIndex] = down;
            ResetSeriesIndex();
            InitSerieHandlers();
            RefreshChart();
            return true;
        }

        /// <summary>
        /// 重置serie的数据项索引。避免数据项索引异常。
        /// </summary>
        /// <param name="serieIndex"></param>
        public bool ResetDataIndex(int serieIndex)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
                return serie.ResetDataIndex();
            return false;
        }

        public bool CanAddSerie<T>() where T : Serie
        {
            return CanAddSerie(typeof(T));
        }

        public bool CanAddSerie(Type type)
        {
            return m_TypeListForSerie.ContainsKey(type);
        }

        public bool HasSerie<T>() where T : Serie
        {
            return HasSerie(typeof(T));
        }

        public bool HasSerie(Type type)
        {
            if (!type.IsSubclassOf(typeof(Serie))) return false;
            foreach (var serie in m_Series)
            {
                if (serie.GetType() == type)
                    return true;
            }
            return false;
        }

        public T GetSerie<T>() where T : Serie
        {
            foreach (var serie in m_Series)
            {
                if (serie is T) return serie as T;
            }
            return null;
        }

        public Serie GetSerie(string serieName)
        {
            foreach (var serie in m_Series)
            {
                if (string.IsNullOrEmpty(serie.serieName))
                {
                    if (string.IsNullOrEmpty(serieName)) return serie;
                }
                else if (serie.serieName.Equals(serieName))
                {
                    return serie;
                }
            }
            return null;
        }

        public Serie GetSerie(int serieIndex)
        {
            if (serieIndex < 0 || serieIndex > m_Series.Count - 1) return null;
            return m_Series[serieIndex];
        }

        public T GetSerie<T>(int serieIndex) where T : Serie
        {
            if (serieIndex < 0 || serieIndex > m_Series.Count - 1) return null;
            return m_Series[serieIndex] as T;
        }

        public void RemoveSerie(string serieName)
        {
            for (int i = m_Series.Count - 1; i >= 0; i--)
            {
                var serie = m_Series[i];
                if (string.IsNullOrEmpty(serieName))
                {
                    if (string.IsNullOrEmpty(serie.serieName))
                        RemoveSerie(serie);
                }
                else if (serieName.Equals(serie.serieName))
                {
                    RemoveSerie(serie);
                }
            }
        }

        public void RemoveSerie(int serieIndex)
        {
            if (serieIndex < 0 || serieIndex > m_Series.Count - 1) return;
            RemoveSerie(m_Series[serieIndex]);
        }

        public void RemoveSerie<T>() where T : Serie
        {
            for (int i = m_Series.Count - 1; i >= 0; i--)
            {
                var serie = m_Series[i];
                if (serie is T)
                    RemoveSerie(serie);
            }
        }

        public void RemoveSerie(Serie serie)
        {
            serie.OnRemove();
            m_SerieHandlers.Remove(serie.handler);
            m_Series.Remove(serie);
            RefreshChart();
        }

        public bool ConvertSerie<T>(Serie serie) where T : Serie
        {
            return ConvertSerie(serie, typeof(T));
        }

        public bool ConvertSerie(Serie serie, Type type)
        {
            try
            {
                var newSerie = type.InvokeMember("ConvertSerie",
                    BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null,
                    new object[] { serie }) as Serie;
                return ReplaceSerie(serie, newSerie);
            }
            catch
            {
                Debug.LogError(string.Format("ConvertSerie Failed: can't found {0}.ConvertSerie(Serie serie)", type.Name));
                return false;
            }
        }

        public bool ReplaceSerie(Serie oldSerie, Serie newSerie)
        {
            if (oldSerie == null || newSerie == null)
                return false;

            var index = m_Series.IndexOf(oldSerie);
            if (index < 0)
                return false;
            AnimationStyleHelper.UpdateSerieAnimation(newSerie);
            oldSerie.OnRemove();
            m_Series.RemoveAt(index);
            m_Series.Insert(index, newSerie);
            ResetSeriesIndex();
            InitSerieHandlers();
            RefreshAllComponent();
            RefreshChart();
            return true;
        }

        /// <summary>
        /// Add a data to serie.
        /// |If serieName doesn't exist in legend,will be add to legend.
        /// |添加一个数据到指定的系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="data">the data to add</param>
        /// <param name="dataName">the name of data</param>
        /// <param name="dataId">the unique id of data</param>
        /// <returns>Returns True on success</returns>
        public SerieData AddData(string serieName, double data, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                var serieData = serie.AddYData(data, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        /// <summary>
        /// Add a data to serie.
        /// |添加一个数据到指定的系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="data">the data to add</param>
        /// <param name="dataName">the name of data</param>
        /// <param name="dataId">the unique id of data</param>
        /// <returns>Returns True on success</returns>
        public SerieData AddData(int serieIndex, double data, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                var serieData = serie.AddYData(data, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// |添加多维数据（x,y,z...）到指定的系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <param name="dataName">the name of data</param>
        /// <param name="dataId">the unique id of data</param>
        /// <returns>Returns True on success</returns>
        public SerieData AddData(string serieName, List<double> multidimensionalData, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                var serieData = serie.AddData(multidimensionalData, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// |添加多维数据（x,y,z...）到指定的系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie,index starts at 0</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <param name="dataName">the name of data</param>
        /// <param name="dataId">the unique id of data</param>
        /// <returns>Returns True on success</returns>
        public SerieData AddData(int serieIndex, List<double> multidimensionalData, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                var serieData = serie.AddData(multidimensionalData, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        [Since("v3.4.0")]
        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// |添加多维数据（x,y,z...）到指定的系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <returns></returns>
        public SerieData AddData(int serieIndex, params double[] multidimensionalData)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                var serieData = serie.AddData(multidimensionalData);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        [Since("v3.4.0")]
        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// |添加多维数据（x,y,z...）到指定的系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <returns></returns>
        public SerieData AddData(string serieName, params double[] multidimensionalData)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                var serieData = serie.AddData(multidimensionalData);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        /// <summary>
        /// Add a (x,y) data to serie.
        /// |添加（x,y）数据到指定系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="xValue">x data</param>
        /// <param name="yValue">y data</param>
        /// <param name="dataName">the name of data</param>
        /// <param name="dataId">the unique id of data</param>
        /// <returns>Returns True on success</returns>
        public SerieData AddData(string serieName, double xValue, double yValue, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                var serieData = serie.AddXYData(xValue, yValue, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        /// <summary>
        /// Add a (x,y) data to serie.
        /// |添加（x,y）数据到指定系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="xValue">x data</param>
        /// <param name="yValue">y data</param>
        /// <param name="dataName">the name of data</param>
        /// <param name="dataId">the unique id of data</param>
        /// <returns>Returns True on success</returns>
        public SerieData AddData(int serieIndex, double xValue, double yValue, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                var serieData = serie.AddXYData(xValue, yValue, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }
        /// <summary>
        /// Add a (time,y) data to serie.
        /// |添加（time,y）数据到指定的系列中。
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="time"></param>
        /// <param name="yValue"></param>
        /// <param name="dataName"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public SerieData AddData(string serieName, DateTime time, double yValue, string dataName = null, string dataId = null)
        {
            var xValue = DateTimeUtil.GetTimestamp(time);
            return AddData(serieName, xValue, yValue, dataName, dataId);
        }

        /// <summary>
        /// Add a (time,y) data to serie.
        /// |添加（time,y）数据到指定的系列中。
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="time"></param>
        /// <param name="yValue"></param>
        /// <param name="dataName"></param>
        /// <param name="dataId"></param>
        /// <returns></returns>
        public SerieData AddData(int serieIndex, DateTime time, double yValue, string dataName = null, string dataId = null)
        {
            var xValue = DateTimeUtil.GetTimestamp(time);
            return AddData(serieIndex, xValue, yValue, dataName, dataId);
        }

        public SerieData AddData(int serieIndex, double indexOrTimestamp, double open, double close, double lowest, double heighest, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                var serieData = serie.AddData(indexOrTimestamp, open, close, lowest, heighest, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }
        public SerieData AddData(string serieName, double indexOrTimestamp, double open, double close, double lowest, double heighest, string dataName = null, string dataId = null)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                var serieData = serie.AddData(indexOrTimestamp, open, close, lowest, heighest, dataName, dataId);
                RefreshPainter(serie.painter);
                return serieData;
            }
            return null;
        }

        /// <summary>
        /// Update serie data by serie name.
        /// |更新指定系列中的指定索引数据。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="dataIndex">the index of data</param>
        /// <param name="value">the data will be update</param>
        public bool UpdateData(string serieName, int dataIndex, double value)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.UpdateYData(dataIndex, value);
                RefreshPainter(serie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update serie data by serie index.
        /// |更新指定系列中的指定索引数据。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="dataIndex">the index of data</param>
        /// <param name="value">the data will be update</param>
        public bool UpdateData(int serieIndex, int dataIndex, double value)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                serie.UpdateYData(dataIndex, value);
                RefreshPainter(serie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列指定索引的数据项的多维数据。
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="multidimensionalData">一个数据项的多维数据列表，而不是多个数据项的数据</param>
        public bool UpdateData(string serieName, int dataIndex, List<double> multidimensionalData)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.UpdateData(dataIndex, multidimensionalData);
                RefreshPainter(serie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列指定索引的数据项的多维数据。
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="multidimensionalData">一个数据项的多维数据列表，而不是多个数据项的数据</param>
        public bool UpdateData(int serieIndex, int dataIndex, List<double> multidimensionalData)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                serie.UpdateData(dataIndex, multidimensionalData);
                RefreshPainter(serie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列指定索引指定维数的数据。维数从0开始。
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dimension">指定维数，从0开始</param>
        /// <param name="value"></param>
        public bool UpdateData(string serieName, int dataIndex, int dimension, double value)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.UpdateData(dataIndex, dimension, value);
                RefreshPainter(serie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列指定索引指定维数的数据。维数从0开始。
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dimension">指定维数，从0开始</param>
        /// <param name="value"></param>
        public bool UpdateData(int serieIndex, int dataIndex, int dimension, double value)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                serie.UpdateData(dataIndex, dimension, value);
                RefreshPainter(serie);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update serie data name.
        /// |更新指定系列中的指定索引数据名称。
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dataName"></param>
        public bool UpdateDataName(string serieName, int dataIndex, string dataName)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                serie.UpdateDataName(dataIndex, dataName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update serie data name.
        /// |更新指定系列中的指定索引数据名称。
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataName"></param>
        /// <param name="dataIndex"></param>
        public bool UpdateDataName(int serieIndex, int dataIndex, string dataName)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                serie.UpdateDataName(dataIndex, dataName);
                return true;
            }
            return false;
        }

        public double GetData(string serieName, int dataIndex, int dimension = 1)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.GetData(dataIndex, dimension);
            }
            return 0;
        }

        public double GetData(int serieIndex, int dataIndex, int dimension = 1)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                return serie.GetData(dataIndex, dimension);
            }
            return 0;
        }

        public int GetAllSerieDataCount()
        {
            var total = 0;
            foreach (var serie in m_Series)
                total += serie.dataCount;
            return total;
        }

        /// <summary>
        /// Whether to show serie.
        /// |设置指定系列是否显示。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="active">Active or not</param>
        public void SetSerieActive(string serieName, bool active)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
                SetSerieActive(serie, active);
        }

        /// <summary>
        /// Whether to show serie.
        /// |设置指定系列是否显示。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="active">Active or not</param>
        public void SetSerieActive(int serieIndex, bool active)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
                SetSerieActive(serie, active);
        }

        public void SetSerieActive(Serie serie, bool active)
        {
            serie.show = active;
            serie.RefreshLabel();
            serie.AnimationReset();
            if (active) serie.AnimationFadeIn();
            UpdateLegendColor(serie.serieName, active);
        }

        /// <summary>
        /// Add a category data to xAxis.
        /// |添加一个类目数据到指定的x轴。
        /// </summary>
        /// <param name="category">the category data</param>
        /// <param name="xAxisIndex">which xAxis should category add to</param>
        public void AddXAxisData(string category, int xAxisIndex = 0)
        {
            var xAxis = GetChartComponent<XAxis>(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.AddData(category);
            }
        }

        /// <summary>
        /// Update category data.
        /// |更新X轴类目数据。
        /// </summary>
        /// <param name="index">the index of category data</param>
        /// <param name="category"></param>
        /// <param name="xAxisIndex">which xAxis index to update to</param>
        public void UpdateXAxisData(int index, string category, int xAxisIndex = 0)
        {
            var xAxis = GetChartComponent<XAxis>(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.UpdateData(index, category);
            }
        }

        /// <summary>
        /// Add an icon to xAxis.
        /// |添加一个图标到指定的x轴。
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="xAxisIndex"></param>
        public void AddXAxisIcon(Sprite icon, int xAxisIndex = 0)
        {
            var xAxis = GetChartComponent<XAxis>(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.AddIcon(icon);
            }
        }

        /// <summary>
        /// Update xAxis icon.
        /// |更新X轴图标。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="icon"></param>
        /// <param name="xAxisIndex"></param>
        public void UpdateXAxisIcon(int index, Sprite icon, int xAxisIndex = 0)
        {
            var xAxis = GetChartComponent<XAxis>(xAxisIndex);
            if (xAxis != null)
            {
                xAxis.UpdateIcon(index, icon);
            }
        }

        /// <summary>
        /// Add a category data to yAxis.
        /// |添加一个类目数据到指定的y轴。
        /// </summary>
        /// <param name="category">the category data</param>
        /// <param name="yAxisIndex">which yAxis should category add to</param>
        public void AddYAxisData(string category, int yAxisIndex = 0)
        {
            var yAxis = GetChartComponent<YAxis>(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.AddData(category);
            }
        }

        /// <summary>
        /// Update category data.
        /// |更新Y轴类目数据。
        /// </summary>
        /// <param name="index">the index of category data</param>
        /// <param name="category"></param>
        /// <param name="yAxisIndex">which yAxis index to update to</param>
        public void UpdateYAxisData(int index, string category, int yAxisIndex = 0)
        {
            var yAxis = GetChartComponent<YAxis>(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.UpdateData(index, category);
            }
        }

        /// <summary>
        /// Add an icon to yAxis.
        /// |添加一个图标到指定的y轴。
        /// </summary>
        /// <param name="icon"></param>
        /// <param name="yAxisIndex"></param>
        public void AddYAxisIcon(Sprite icon, int yAxisIndex = 0)
        {
            var yAxis = GetChartComponent<YAxis>(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.AddIcon(icon);
            }
        }

        /// <summary>
        /// 更新Y轴图标。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="icon"></param>
        /// <param name="yAxisIndex"></param>
        public void UpdateYAxisIcon(int index, Sprite icon, int yAxisIndex = 0)
        {
            var yAxis = GetChartComponent<YAxis>(yAxisIndex);
            if (yAxis != null)
            {
                yAxis.UpdateIcon(index, icon);
            }
        }

        public float GetSerieBarGap<T>() where T : Serie
        {
            float gap = 0f;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                if (serie is T)
                {
                    if (serie.barGap != 0)
                    {
                        gap = serie.barGap;
                    }
                }
            }
            return gap;
        }

        public double GetSerieSameStackTotalValue<T>(string stack, int dataIndex) where T : Serie
        {
            if (string.IsNullOrEmpty(stack)) return 0;
            double total = 0;
            foreach (var serie in m_Series)
            {
                if (serie is T)
                {
                    if (stack.Equals(serie.stack))
                    {
                        total += serie.data[dataIndex].data[1];
                    }
                }
            }
            return total;
        }

        public int GetSerieBarRealCount<T>() where T : Serie
        {
            var count = 0;
            barStackSet.Clear();
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                if (!serie.show) continue;
                if (serie is T)
                {
                    if (!string.IsNullOrEmpty(serie.stack))
                    {
                        if (barStackSet.Contains(serie.stack)) continue;
                        barStackSet.Add(serie.stack);
                    }
                    count++;

                }
            }
            return count;
        }

        private HashSet<string> barStackSet = new HashSet<string>();
        public float GetSerieTotalWidth<T>(float categoryWidth, float gap, int realBarCount) where T : Serie
        {
            float total = 0;
            float lastGap = 0;
            barStackSet.Clear();
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                if (!serie.show) continue;
                if (serie is T)
                {
                    if (!string.IsNullOrEmpty(serie.stack))
                    {
                        if (barStackSet.Contains(serie.stack)) continue;
                        barStackSet.Add(serie.stack);
                    }
                    var width = GetStackBarWidth<T>(categoryWidth, serie, realBarCount);
                    if (gap == -1)
                    {
                        if (width > total) total = width;
                    }
                    else
                    {
                        lastGap = ChartHelper.GetActualValue(gap, width);
                        total += width;
                        total += lastGap;
                    }
                }
            }
            if (total > 0 && gap != -1) total -= lastGap;
            return total;
        }

        public float GetSerieTotalGap<T>(float categoryWidth, float gap, int index) where T : Serie
        {
            if (index <= 0) return 0;
            var total = 0f;
            var count = 0;
            var totalRealBarCount = GetSerieBarRealCount<T>();
            barStackSet.Clear();
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                if (!serie.show) continue;
                if (serie is T)
                {
                    if (!string.IsNullOrEmpty(serie.stack))
                    {
                        if (barStackSet.Contains(serie.stack)) continue;
                        barStackSet.Add(serie.stack);
                    }
                    var width = GetStackBarWidth<T>(categoryWidth, serie, totalRealBarCount);
                    if (gap == -1)
                    {
                        if (width > total) total = width;
                    }
                    else
                    {
                        total += width + ChartHelper.GetActualValue(gap, width);
                    }
                    if (count + 1 >= index)
                        break;
                    else
                        count++;
                }
            }
            return total;
        }

        private float GetStackBarWidth<T>(float categoryWidth, Serie now, int realBarCount) where T : Serie
        {
            if (string.IsNullOrEmpty(now.stack)) return now.GetBarWidth(categoryWidth, realBarCount);
            float barWidth = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                if ((serie is T) &&
                    serie.show && now.stack.Equals(serie.stack))
                {
                    if (serie.barWidth > barWidth) barWidth = serie.barWidth;
                }
            }
            if (barWidth == 0)
            {
                var width = ChartHelper.GetActualValue(0.6f, categoryWidth);
                if (realBarCount == 0)
                    return width < 1 ? categoryWidth : width;
                else
                    return width / realBarCount;
            }
            else
                return ChartHelper.GetActualValue(barWidth, categoryWidth);
        }

        private List<string> tempList = new List<string>();
        public int GetSerieIndexIfStack<T>(Serie currSerie) where T : Serie
        {
            tempList.Clear();
            int index = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                if (!(serie is T)) continue;
                if (string.IsNullOrEmpty(serie.stack))
                {
                    if (serie.index == currSerie.index) return index;
                    tempList.Add(string.Empty);
                    index++;
                }
                else
                {
                    if (!tempList.Contains(serie.stack))
                    {
                        if (serie.index == currSerie.index) return index;
                        tempList.Add(serie.stack);
                        index++;
                    }
                    else
                    {
                        if (serie.index == currSerie.index) return tempList.IndexOf(serie.stack);
                    }
                }
            }
            return 0;
        }

        internal void InitSerieHandlers()
        {
            m_SerieHandlers.Clear();
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series[i];
                serie.index = i;
                CreateSerieHandler(serie);
            }
        }

        private void CreateSerieHandler(Serie serie)
        {
            if (serie == null)
                throw new ArgumentNullException("serie is null");

            if (!serie.GetType().IsDefined(typeof(SerieHandlerAttribute), false))
            {
                Debug.LogError("Serie no Handler:" + serie.GetType());
                return;
            }
            var attribute = serie.GetType().GetAttribute<SerieHandlerAttribute>();
            var handler = (SerieHandler) Activator.CreateInstance(attribute.handler);
            handler.attribute = attribute;
            handler.chart = this;
            handler.defaultDimension = 1;
            handler.SetSerie(serie);
            serie.handler = handler;
            m_SerieHandlers.Add(handler);
        }

        private Serie InsertSerie(int index, Type type, string serieName, bool show = true, bool addToHead = false)
        {
            CheckAddRequireChartComponent(type);
            var serie = Activator.CreateInstance(type) as Serie;
            serie.show = show;
            serie.serieName = serieName;
            serie.serieType = type.Name;
            serie.index = m_Series.Count;

            if (type == typeof(Scatter))
            {
                serie.symbol.show = true;
                serie.symbol.type = SymbolType.Circle;
            }
            else if (type == typeof(Line))
            {
                serie.symbol.show = true;
                serie.symbol.type = SymbolType.EmptyCircle;
            }
            else if (type == typeof(Heatmap))
            {
                serie.symbol.show = true;
                serie.symbol.type = SymbolType.Rect;
            }
            else
            {
                serie.symbol.show = false;
            }
            InsertSerie(serie, index, addToHead);
            return serie;
        }

        private void ResetSeriesIndex()
        {
#if UNITY_EDITOR && UNITY_2019_1_OR_NEWER
            UnityEditor.EditorUtility.SetDirty(this);
#endif
            for (int i = 0; i < m_Series.Count; i++)
            {
                m_Series[i].index = i;
            }
        }

        private void AddSerieAfterDeserialize(Serie serie)
        {
            serie.OnAfterDeserialize();
            m_Series.Add(serie);
        }

        public string GenerateDefaultSerieName()
        {
            return "serie" + m_Series.Count;
        }

        public bool IsSerieName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;
            foreach (var serie in m_Series)
            {
                if (name.Equals(serie.serieName))
                    return true;
            }
            return false;
        }
    }
}