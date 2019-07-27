using UnityEngine;
using System.Collections.Generic;

namespace XCharts
{
    public partial class BaseChart
    {
        public Title title { get { return m_Title; } }
        public Legend legend { get { return m_Legend; } }
        public Tooltip tooltip { get { return m_Tooltip; } }
        public Series series { get { return m_Series; } }

        public float chartWidth { get { return m_ChartWidth; } }
        public float chartHeight { get { return m_ChartHeight; } }

        /// <summary>
        /// The min number of data to show in chart.
        /// </summary>
        public int minShowDataNumber
        {
            get { return m_MinShowDataNumber; }
            set { m_MinShowDataNumber = value; if (m_MinShowDataNumber < 0) m_MinShowDataNumber = 0; }
        }

        /// <summary>
        /// The max number of data to show in chart.
        /// </summary>
        public int maxShowDataNumber
        {
            get { return m_MaxShowDataNumber; }
            set { m_MaxShowDataNumber = value; if (m_MaxShowDataNumber < 0) m_MaxShowDataNumber = 0; }
        }

        /// <summary>
        /// The max number of serie and axis data cache.
        /// The first data will be remove when the size of serie and axis data is larger then maxCacheDataNumber.
        /// default:0,unlimited.
        /// </summary>
        public int maxCacheDataNumber
        {
            get { return m_MaxCacheDataNumber; }
            set { m_MaxCacheDataNumber = value; if (m_MaxCacheDataNumber < 0) m_MaxCacheDataNumber = 0; }
        }

        /// <summary>
        /// Set the size of chart.
        /// </summary>
        /// <param name="width">width</param>
        /// <param name="height">height</param>
        public virtual void SetSize(float width, float height)
        {
            m_ChartWidth = width;
            m_ChartHeight = height;
            m_CheckWidth = width;
            m_CheckHeight = height;
            rectTransform.sizeDelta = new Vector2(m_ChartWidth, m_ChartHeight);
            OnSizeChanged();
        }

        /// <summary>
        /// Remove all series and legend data.
        /// It just emptying all of serie's data without emptying the list of series.
        /// </summary>
        public virtual void ClearData()
        {
            m_Series.ClearData();
            m_Legend.ClearData();
            RefreshChart();
        }

        /// <summary>
        /// Remove legend and serie by name.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        public virtual void RemoveData(string serieName)
        {
            m_Series.Remove(serieName);
            m_Legend.RemoveData(serieName);
            RefreshChart();
        }

        /// <summary>
        /// Remove all data from series and legend.
        /// The series list is also cleared.
        /// </summary>
        public virtual void RemoveData()
        {
            m_Legend.ClearData();
            m_Series.RemoveAll();
            RefreshChart();
        }

        /// <summary>
        /// Add a serie to serie list.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="type">the type of serie</param>
        /// <param name="show">whether to show this serie</param>
        /// <returns>the added serie</returns>
        public virtual Serie AddSerie(string serieName, SerieType type, bool show = true)
        {
            m_Legend.AddData(serieName);
            return m_Series.AddSerie(serieName, type);
        }

        /// <summary>
        /// Add a data to serie.
        /// If serieName doesn't exist in legend,will be add to legend.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="data">the data to add</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual bool AddData(string serieName, float data, string dataName = null)
        {
            m_Legend.AddData(serieName);
            var success = m_Series.AddData(serieName, data, dataName, m_MaxCacheDataNumber);
            if (success) RefreshChart();
            return success;
        }

        /// <summary>
        /// Add a data to serie.
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="data">the data to add</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual bool AddData(int serieIndex, float data, string dataName = null)
        {
            var success = m_Series.AddData(serieIndex, data, dataName, m_MaxCacheDataNumber);
            if (success) RefreshChart();
            return success;
        }

        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual bool AddData(string serieName, List<float> multidimensionalData, string dataName = null)
        {
            var success = m_Series.AddData(serieName, multidimensionalData, dataName, m_MaxCacheDataNumber);
            if (success) RefreshChart();
            return success;
        }

        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// </summary>
        /// <param name="serieIndex">the index of serie,index starts at 0</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual bool AddData(int serieIndex, List<float> multidimensionalData, string dataName = null)
        {
            var success = m_Series.AddData(serieIndex, multidimensionalData, dataName, m_MaxCacheDataNumber);
            if (success) RefreshChart();
            return success;
        }

        /// <summary>
        /// Add a (x,y) data to serie.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="xValue">x data</param>
        /// <param name="yValue">y data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual bool AddData(string serieName, float xValue, float yValue, string dataName)
        {
            var success = m_Series.AddXYData(serieName, xValue, yValue, dataName, m_MaxCacheDataNumber);
            if (success) RefreshChart();
            return true;
        }

        /// <summary>
        /// Add a (x,y) data to serie.
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="xValue">x data</param>
        /// <param name="yValue">y data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual bool AddData(int serieIndex, float xValue, float yValue, string dataName = null)
        {
            var success = m_Series.AddXYData(serieIndex, xValue, yValue, dataName, m_MaxCacheDataNumber);
            if (success) RefreshChart();
            return success;
        }

        /// <summary>
        /// Update serie data by serie name.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="value">the data will be update</param>
        /// <param name="dataIndex">the index of data</param>
        public virtual void UpdateData(string serieName, float value, int dataIndex = 0)
        {
            m_Series.UpdateData(serieName, value, dataIndex);
            RefreshChart();
        }

        /// <summary>
        /// Update serie data by serie index.
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="value">the data will be update</param>
        /// <param name="dataIndex">the index of data</param>
        public virtual void UpdateData(int serieIndex, float value, int dataIndex = 0)
        {
            m_Series.UpdateData(serieIndex, value, dataIndex);
            RefreshChart();
        }

        /// <summary>
        /// Whether to show serie and legend.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="active">Active or not</param>
        public virtual void SetActive(string serieName, bool active)
        {
            var serie = m_Series.GetSerie(serieName);
            if (serie != null)
            {
                SetActive(serie.index, active);
            }
        }

        /// <summary>
        /// Whether to show serie and legend.
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="active">Active or not</param>
        public virtual void SetActive(int serieIndex, bool active)
        {
            m_Series.SetActive(serieIndex, active);
            var serie = m_Series.GetSerie(serieIndex);
            if (serie != null && !string.IsNullOrEmpty(serie.name))
            {
                var bgColor1 = active ? m_ThemeInfo.GetColor(serie.index) : m_ThemeInfo.legendUnableColor;
                m_Legend.UpdateButtonColor(serie.name, bgColor1);
            }
        }

        /// <summary>
        /// Whether serie is activated.
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <returns>True when activated</returns>
        public virtual bool IsActive(string serieName)
        {
            return m_Series.IsActive(serieName);
        }

        /// <summary>
        /// Whether serie is activated.
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <returns>True when activated</returns>
        public virtual bool IsActive(int serieIndex)
        {
            return m_Series.IsActive(serieIndex);
        }

        public virtual bool IsLegendActive(string legendName)
        {
            return IsActive(legendName);
        }

        /// <summary>
        /// Redraw chart next frame.
        /// </summary>
        public void RefreshChart()
        {
            m_RefreshChart = true;
        }

        /// <summary>
        /// Update chart theme
        /// </summary>
        /// <param name="theme">theme</param>
        public void UpdateTheme(Theme theme)
        {
            m_ThemeInfo.theme = theme;
            OnThemeChanged();
            RefreshChart();
        }
    }
}
