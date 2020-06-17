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
        [NonSerialized] private bool m_LabelDirty;

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
        public bool labelDirty { get { return m_LabelDirty; } set { m_LabelDirty = value; } }

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

        public override bool vertsDirty
        {
            get
            {
                if (m_VertsDirty) return true;
                foreach (var serie in m_Series)
                {
                    if (serie.vertsDirty) return true;
                }
                return false;
            }
        }

        public void SetLabelDirty()
        {
            m_LabelDirty = true;
        }

        internal override void ClearVerticesDirty()
        {
            base.ClearVerticesDirty();
            foreach (var serie in m_Series)
            {
                serie.ClearVerticesDirty();
            }
        }

        internal void ClearLabelDirty()
        {
            m_LabelDirty = false;
            foreach (var serie in m_Series)
            {
                serie.label.ClearVerticesDirty();
            }
        }

        public override void SetAllDirty()
        {
            base.SetAllDirty();
            SetLabelDirty();
        }

        public override void ClearDirty()
        {
            base.ClearDirty();
            ClearLabelDirty();
            SeriesHelper.ClearNameDirty(this);
        }

        /// <summary>
        /// 清空所有系列的数据
        /// </summary>
        public void ClearData()
        {
            AnimationFadeIn();
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

        public float GetCurrData(int serieIndex, int dataIndex)
        {
            if (serieIndex >= 0 && serieIndex < Count)
            {
                return m_Series[serieIndex].GetYCurrData(dataIndex);
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
            AnimationFadeIn();
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
                serie.symbol.show = true;
                serie.symbol.type = SerieSymbolType.Circle;
                serie.symbol.size = 20f;
                serie.symbol.selectedSize = 30f;
            }
            else if (type == SerieType.Line)
            {
                serie.symbol.show = true;
                serie.symbol.type = SerieSymbolType.EmptyCircle;
                serie.symbol.size = 2.5f;
                serie.symbol.selectedSize = 5f;
            }
            else
            {
                serie.symbol.show = false;
            }
            serie.animation.Restart();
            m_Series.Add(serie);
            SetVerticesDirty();
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
        public bool UpdateData(string serieName, int dataIndex, float value)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.UpdateYData(dataIndex, value);
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列的数据项名称
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dataName"></param>
        public bool UpdateDataName(string serieName, int dataIndex, string dataName)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.UpdateDataName(dataIndex, dataName);
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列的数据项名称
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dataName"></param>
        public bool UpdateDataName(int serieIndex, int dataIndex, string dataName)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                return serie.UpdateDataName(dataIndex, dataName);
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列的维度Y数据项的值
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="value"></param>
        public bool UpdateData(int serieIndex, int dataIndex, float value)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                return serie.UpdateYData(dataIndex, value);
            }
            return false;
        }

        public bool UpdateData(string serieName, int dataIndex, List<float> values)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.UpdateData(dataIndex, values);
            }
            return false;
        }
        public bool UpdateData(int serieIndex, int dataIndex, List<float> values)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                return serie.UpdateData(dataIndex, values);
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列指定数据项指定维度的数据值
        /// </summary>
        /// <param name="serieIndex">系列</param>
        /// <param name="dataIndex">数据项</param>
        /// <param name="dimension">数据维数，从0开始</param>
        /// <param name="value">值</param>
        public bool UpdateData(int serieIndex, int dataIndex, int dimension, float value)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                return serie.UpdateData(dataIndex, dimension, value);
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列指定数据项指定维度的数据值
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dimension">数据维数，从0开始</param>
        /// <param name="value"></param>
        public bool UpdateData(string serieName, int dataIndex, int dimension, float value)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.UpdateData(dataIndex, dimension, value);
            }
            return false;
        }


        /// <summary>
        /// 更新指定系列的维度X和维度Y数据
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public bool UpdateXYData(string serieName, int dataIndex, float xValue, float yValue)
        {
            var serie = GetSerie(serieName);
            if (serie != null)
            {
                return serie.UpdateXYData(dataIndex, xValue, yValue);
            }
            return false;
        }

        /// <summary>
        /// 更新指定系列的维度X和维度Y数据
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataIndex"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public bool UpdateXYData(int serieIndex, int dataIndex, float xValue, float yValue)
        {
            var serie = GetSerie(serieIndex);
            if (serie != null)
            {
                return serie.UpdateXYData(dataIndex, xValue, yValue);
            }
            return false;
        }

        /// <summary>
        /// dataZoom由变化是更新系列的缓存数据
        /// </summary>
        /// <param name="dataZoom"></param>
        internal void UpdateFilterData(DataZoom dataZoom)
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
                serie.animation.Reset();
                if (active) serie.animation.FadeIn();
            }
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
        /// 渐入动画
        /// </summary>
        public void AnimationFadeIn()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable)
                {
                    serie.animation.FadeIn();
                }
            }
        }

        /// <summary>
        /// 渐出动画
        /// </summary>
        public void AnimationFadeOut()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable) serie.animation.FadeOut();
            }
        }

        /// <summary>
        /// 暂停动画
        /// </summary>
        public void AnimationPause()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable) serie.animation.Pause();
            }
        }

        /// <summary>
        /// 继续动画
        /// </summary>
        public void AnimationResume()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable) serie.animation.Resume();
            }
        }

        /// <summary>
        /// 重置动画
        /// </summary>
        public void AnimationReset()
        {
            foreach (var serie in m_Series)
            {
                if (serie.animation.enable) serie.animation.Reset();
            }
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
