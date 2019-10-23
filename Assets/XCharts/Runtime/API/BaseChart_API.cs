/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// The base class of all charts.
    /// 所有Chart的基类，不可直接使用。
    /// </summary>
    public partial class BaseChart
    {
        /// <summary>
        /// The title setting of chart.
        /// 标题组件
        /// </summary>
        public Title title { get { return m_Title; } }
        /// <summary>
        /// The legend setting of chart.
        /// 图例组件
        /// </summary>
        public Legend legend { get { return m_Legend; } }
        /// <summary>
        /// The tooltip setting of chart.
        /// 提示框组件
        /// </summary>
        public Tooltip tooltip { get { return m_Tooltip; } }
        /// <summary>
        /// The series setting of chart.
        /// 系列列表
        /// </summary>
        public Series series { get { return m_Series; } }
        /// <summary>
        /// Global parameter setting component.
        /// 全局设置组件。
        /// </summary>
        public Settings settings { get { return m_Settings; } }
        /// <summary>
        /// The width of chart. 
        /// 图表的宽
        /// </summary>
        public float chartWidth { get { return m_ChartWidth; } }
        /// <summary>
        /// The height of chart. 
        /// 图表的高
        /// </summary>
        public float chartHeight { get { return m_ChartHeight; } }
        /// <summary>
        /// The postion of pointer.
        /// 鼠标位置
        /// </summary>
        public Vector2 pointerPos { get; protected set; }
        /// <summary>
        /// 自定义绘制回调。
        /// </summary>
        public Action<VertexHelper> customDrawCallback { set { m_CustomDrawCallback = value; } }
        /// <summary>
        /// Set the size of chart.
        /// 设置图表的大小。
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
        /// 清除所有数据，系列中只是移除数据，列表会保留。
        /// </summary>
        public virtual void ClearData()
        {
            m_Series.ClearData();
            m_Legend.ClearData();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Remove all data from series and legend.
        /// The series list is also cleared.
        /// 清除所有系列和图例数据，系列的列表也会被清除。
        /// </summary>
        public virtual void RemoveData()
        {
            m_Legend.ClearData();
            m_Series.RemoveAll();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Remove legend and serie by name.
        /// 清除指定系列名称的数据。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        public virtual void RemoveData(string serieName)
        {
            m_Series.Remove(serieName);
            m_Legend.RemoveData(serieName);
            RefreshChart();
        }

        /// <summary>
        /// Add a serie to serie list.
        /// 添加一个系列到系列列表中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="type">the type of serie</param>
        /// <param name="show">whether to show this serie</param>
        /// <returns>the added serie</returns>
        public virtual Serie AddSerie(SerieType type, string serieName = null, bool show = true)
        {
            return m_Series.AddSerie(type, serieName);
        }

        /// <summary>
        /// Add a data to serie.
        /// If serieName doesn't exist in legend,will be add to legend.
        /// 添加一个数据到指定的系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="data">the data to add</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual SerieData AddData(string serieName, float data, string dataName = null)
        {
            var serieData = m_Series.AddData(serieName, data, dataName);
            if (serieData != null)
            {
                RefreshChart();
                RefreshLabel();
            }
            return serieData;
        }

        /// <summary>
        /// Add a data to serie.
        /// 添加一个数据到指定的系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="data">the data to add</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual SerieData AddData(int serieIndex, float data, string dataName = null)
        {
            var serieData = m_Series.AddData(serieIndex, data, dataName);
            if (serieData != null)
            {
                RefreshChart();
                RefreshLabel();
            }
            return serieData;
        }

        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// 添加多维数据（x,y,z...）到指定的系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual SerieData AddData(string serieName, List<float> multidimensionalData, string dataName = null)
        {
            var serieData = m_Series.AddData(serieName, multidimensionalData, dataName);
            if (serieData != null)
            {
                RefreshChart();
                RefreshLabel();
            }
            return serieData;
        }

        /// <summary>
        /// Add an arbitray dimension data to serie,such as (x,y,z,...).
        /// 添加多维数据（x,y,z...）到指定的系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie,index starts at 0</param>
        /// <param name="multidimensionalData">the (x,y,z,...) data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual SerieData AddData(int serieIndex, List<float> multidimensionalData, string dataName = null)
        {
            var serieData = m_Series.AddData(serieIndex, multidimensionalData, dataName);
            if (serieData != null)
            {
                RefreshChart();
                RefreshLabel();
            }
            return serieData;
        }

        /// <summary>
        /// Add a (x,y) data to serie.
        /// 添加（x,y）数据到指定系列中。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="xValue">x data</param>
        /// <param name="yValue">y data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual SerieData AddData(string serieName, float xValue, float yValue, string dataName)
        {
            var serieData = m_Series.AddXYData(serieName, xValue, yValue, dataName);
            if (serieData != null)
            {
                RefreshChart();
                RefreshLabel();
            }
            return serieData;
        }

        /// <summary>
        /// Add a (x,y) data to serie.
        /// 添加（x,y）数据到指定系列中。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="xValue">x data</param>
        /// <param name="yValue">y data</param>
        /// <param name="dataName">the name of data</param>
        /// <returns>Returns True on success</returns>
        public virtual SerieData AddData(int serieIndex, float xValue, float yValue, string dataName = null)
        {
            var serieData = m_Series.AddXYData(serieIndex, xValue, yValue, dataName);
            if (serieData != null)
            {
                RefreshChart();
                RefreshLabel();
            }
            return serieData;
        }

        /// <summary>
        /// Update serie data by serie name.
        /// 更新指定系列中的指定索引数据。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <param name="dataIndex">the index of data</param>
        /// <param name="value">the data will be update</param>
        public virtual void UpdateData(string serieName, int dataIndex, float value)
        {
            m_Series.UpdateData(serieName, dataIndex, value);
            RefreshChart();
        }

        /// <summary>
        /// Update serie data by serie index.
        /// 更新指定系列中的指定索引数据。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="dataIndex">the index of data</param>
        /// <param name="value">the data will be update</param>
        public virtual void UpdateData(int serieIndex, int dataIndex, float value)
        {
            m_Series.UpdateData(serieIndex, dataIndex, value);
            RefreshChart();
        }

        /// <summary>
        /// Update serie data name.
        /// 更新指定系列中的指定索引数据名称。
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dataName"></param>
        public virtual void UpdateDataName(string serieName, int dataIndex, string dataName)
        {
            m_Series.UpdateDataName(serieName, dataIndex, dataName);
        }

        /// <summary>
        /// Update serie data name.
        /// 更新指定系列中的指定索引数据名称。
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataName"></param>
        /// <param name="dataIndex"></param>
        public virtual void UpdateDataName(int serieIndex, int dataIndex, string dataName)
        {
            m_Series.UpdateDataName(serieIndex, dataIndex, dataName);
        }

        /// <summary>
        /// Whether to show serie.
        /// 设置指定系列是否显示。
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
        /// Whether to show serie.
        /// 设置指定系列是否显示。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="active">Active or not</param>
        public virtual void SetActive(int serieIndex, bool active)
        {
            m_Series.SetActive(serieIndex, active);
            var serie = m_Series.GetSerie(serieIndex);
            if (serie != null && !string.IsNullOrEmpty(serie.name))
            {
                var legendIndex = m_LegendRealShowName.IndexOf(serie.name);
                var bgColor1 = active ? m_ThemeInfo.GetColor(legendIndex) : m_ThemeInfo.legendUnableColor;
                m_Legend.UpdateButtonColor(serie.name, bgColor1);
            }
        }

        /// <summary>
        /// Whether serie is activated.
        /// 获取指定系列是否显示。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        /// <returns>True when activated</returns>
        public virtual bool IsActive(string serieName)
        {
            return m_Series.IsActive(serieName);
        }

        /// <summary>
        /// Whether serie is activated.
        /// 获取指定系列是否显示。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <returns>True when activated</returns>
        public virtual bool IsActive(int serieIndex)
        {
            return m_Series.IsActive(serieIndex);
        }

        /// <summary>
        /// Whether serie is activated.
        /// 获得指定图例名字的系列是否显示。
        /// </summary>
        /// <param name="legendName"></param>
        /// <returns></returns>
        public virtual bool IsActiveByLegend(string legendName)
        {
            foreach (var serie in m_Series.list)
            {
                if (serie.show && legendName.Equals(serie.name))
                {
                    return true;
                }
                else
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.show && legendName.Equals(serieData.name))
                        {
                            return true;
                        }
                    }
                }

            }
            return false;
        }

        /// <summary>
        /// Redraw chart in next frame.
        /// 在下一帧刷新图表。
        /// </summary>
        public void RefreshChart()
        {
            m_RefreshChart = true;
        }

        [Obsolete("Use BaseChart.RefreshLabel() instead.", true)]
        public void ReinitChartLabel()
        {
            RefreshLabel();
        }

        /// <summary>
        /// 刷新文本标签Label，重新初始化，当有改动Label参数时手动调用改接口
        /// </summary>
        public void RefreshLabel()
        {
            m_ReinitLabel = true;
        }

        /// <summary>
        /// Update chart theme.
        /// 切换图表主题。
        /// </summary>
        /// <param name="theme">theme</param>
        public void UpdateTheme(Theme theme)
        {
            m_ThemeInfo.theme = theme;
            OnThemeChanged();
            RefreshChart();
        }

        /// <summary>
        /// Whether series animation enabel.
        /// 启用或关闭起始动画。
        /// </summary>
        /// <param name="flag"></param>
        public void AnimationEnable(bool flag)
        {
            m_Series.AnimationEnable(flag);
        }

        /// <summary>
        /// Start play animation.
        /// 开始初始动画。
        /// </summary>
        public void AnimationStart()
        {
            m_Series.AnimationStart();
        }

        /// <summary>
        /// Stop play animation.
        /// 停止初始化动画。
        /// </summary>
        public void AnimationStop()
        {
            m_CheckAnimation = false;
            m_Series.AnimationStop();
        }

        /// <summary>
        /// Reset animation to play.
        /// 重置初始动画，重新播放。
        /// </summary>
        public void AnimationReset()
        {
            m_CheckAnimation = false;
            m_Series.AnimationReset();
            RefreshChart();
        }

        /// <summary>
        /// 点击图例按钮
        /// </summary>
        /// <param name="legendIndex">图例按钮索引</param>
        /// <param name="legendName">图例按钮名称</param>
        /// <param name="show">显示还是隐藏</param>
        public void ClickLegendButton(int legendIndex, string legendName, bool show)
        {
            OnLegendButtonClick(legendIndex, legendName, show);
            RefreshChart();
        }
    }
}
