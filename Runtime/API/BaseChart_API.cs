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
    /// 所有Chart的基类。
    /// </summary>
    public partial class BaseChart
    {
        /// <summary>
        /// The name of chart.
        /// </summary>
        public string chartName
        {
            get { return m_ChartName; }
            set
            {
                if (!string.IsNullOrEmpty(value) && XChartsMgr.Instance.ContainsChart(value))
                {
                    Debug.LogError("chartName repeated:" + value);
                }
                else
                {
                    m_ChartName = value;
                }
            }
        }
        /// <summary>
        /// The theme info.
        /// </summary>
        public ThemeInfo themeInfo { get { return m_ThemeInfo; } set { m_ThemeInfo = value; } }
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
        /// The x of chart. 
        /// 图表的X
        /// </summary>
        public float chartX { get { return m_ChartX; } }
        /// <summary>
        /// The y of chart. 
        /// 图表的Y
        /// </summary>
        public float chartY { get { return m_ChartY; } }
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
        /// The position of chart.
        /// 图表的左下角起始坐标。
        /// </summary>
        public Vector3 chartPosition { get { return m_ChartPosition; } }
        public Rect chartRect { get { return m_ChartRect; } }

        /// <summary>
        /// 自定义绘制回调。
        /// </summary>
        public Action<VertexHelper> onCustomDraw { set { m_OnCustomDrawCallback = value; } }

        /// <summary>
        /// Redraw chart in next frame.
        /// 在下一帧刷新图表。
        /// </summary>
        public void RefreshChart()
        {
            m_RefreshChart = true;
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
            m_Tooltip.ClearValue();
            m_CheckAnimation = false;
            m_ReinitLabel = true;
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
            m_Tooltip.ClearValue();
            m_CheckAnimation = false;
            m_ReinitLabel = true;
            m_SerieLabelRoot = null;
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
            m_SerieLabelRoot = null;
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
                var serie = m_Series.GetSerie(serieName);
                AddSerieLabel(serie, serieData);
                RefreshChart();
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
                var serie = m_Series.GetSerie(serieIndex);
                AddSerieLabel(serie, serieData);
                RefreshChart();
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
                var serie = m_Series.GetSerie(serieName);
                AddSerieLabel(serie, serieData);
                RefreshChart();
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
                var serie = m_Series.GetSerie(serieIndex);
                AddSerieLabel(serie, serieData);
                RefreshChart();
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
        public virtual SerieData AddData(string serieName, float xValue, float yValue, string dataName = null)
        {
            var serieData = m_Series.AddXYData(serieName, xValue, yValue, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieName);
                AddSerieLabel(serie, serieData);
                RefreshChart();
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
                var serie = m_Series.GetSerie(serieIndex);
                AddSerieLabel(serie, serieData);
                RefreshChart();
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
        public virtual bool UpdateData(string serieName, int dataIndex, float value)
        {
            if (m_Series.UpdateData(serieName, dataIndex, value))
            {
                RefreshChart();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update serie data by serie index.
        /// 更新指定系列中的指定索引数据。
        /// </summary>
        /// <param name="serieIndex">the index of serie</param>
        /// <param name="dataIndex">the index of data</param>
        /// <param name="value">the data will be update</param>
        public virtual bool UpdateData(int serieIndex, int dataIndex, float value)
        {
            if (m_Series.UpdateData(serieIndex, dataIndex, value))
            {
                RefreshChart();
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
        public virtual bool UpdateData(string serieName, int dataIndex, List<float> multidimensionalData)
        {
            if (m_Series.UpdateData(serieName, dataIndex, multidimensionalData))
            {
                RefreshChart();
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
        public virtual bool UpdateData(int serieIndex, int dataIndex, List<float> multidimensionalData)
        {
            if (m_Series.UpdateData(serieIndex, dataIndex, multidimensionalData))
            {
                RefreshChart();
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
        public virtual bool UpdateData(string serieName, int dataIndex, int dimension, float value)
        {
            if (m_Series.UpdateData(serieName, dataIndex, dimension, value))
            {
                RefreshChart();
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
        public virtual bool UpdateData(int serieIndex, int dataIndex, int dimension, float value)
        {
            if (m_Series.UpdateData(serieIndex, dataIndex, dimension, value))
            {
                RefreshChart();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Update serie data name.
        /// 更新指定系列中的指定索引数据名称。
        /// </summary>
        /// <param name="serieName"></param>
        /// <param name="dataIndex"></param>
        /// <param name="dataName"></param>
        public virtual bool UpdateDataName(string serieName, int dataIndex, string dataName)
        {
            return m_Series.UpdateDataName(serieName, dataIndex, dataName);
        }

        /// <summary>
        /// Update serie data name.
        /// 更新指定系列中的指定索引数据名称。
        /// </summary>
        /// <param name="serieIndex"></param>
        /// <param name="dataName"></param>
        /// <param name="dataIndex"></param>
        public virtual bool UpdateDataName(int serieIndex, int dataIndex, string dataName)
        {
            return m_Series.UpdateDataName(serieIndex, dataIndex, dataName);
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
                UpdateLegendColor(serie.name, active);
            }
        }

        protected virtual void UpdateLegendColor(string legendName, bool active)
        {
            var legendIndex = m_LegendRealShowName.IndexOf(legendName);
            if (legendIndex >= 0)
            {
                var iconColor = LegendHelper.GetIconColor(legend, legendIndex, m_ThemeInfo, active);
                var contentColor = LegendHelper.GetContentColor(legend, m_ThemeInfo, active);
                m_Legend.UpdateButtonColor(legendName, iconColor);
                m_Legend.UpdateContentColor(legendName, contentColor);
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
        /// 刷新文本标签Label，重新初始化，当有改动Label参数时手动调用改接口
        /// </summary>
        public void RefreshLabel()
        {
            m_ReinitLabel = true;
            m_SerieLabelRoot = null;
        }

        /// <summary>
        /// 刷新Tooltip组件。
        /// </summary>
        public void RefreshTooltip()
        {
            InitTooltip();
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
        /// Update chart theme info.
        /// 切换图表主题。
        /// </summary>
        /// <param name="themeInfo">themeInfo</param>
        public void UpdateThemeInfo(ThemeInfo themeInfo)
        {
            m_ThemeInfo = themeInfo;
            UpdateTheme(m_ThemeInfo.theme);
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
        /// fadeIn animation.
        /// 开始渐入动画。
        /// </summary>
        public void AnimationFadeIn()
        {
            m_Series.AnimationFadeIn();
            RefreshChart();
        }

        /// <summary>
        /// fadeIn animation.
        /// 开始渐出动画。
        /// </summary>
        public void AnimationFadeOut()
        {
            m_Series.AnimationFadeOut();
            RefreshChart();
        }

        /// <summary>
        /// Pause animation.
        /// 暂停动画。
        /// </summary>
        public void AnimationPause()
        {
            m_Series.AnimationPause();
            RefreshChart();
        }

        /// <summary>
        /// Stop play animation.
        /// 继续动画。
        /// </summary>
        public void AnimationResume()
        {
            m_Series.AnimationResume();
            RefreshChart();
        }

        /// <summary>
        /// Reset animation.
        /// 重置动画。
        /// </summary>
        public void AnimationReset()
        {
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

        /// <summary>
        /// 坐标是否在图表范围内
        /// </summary>
        /// <param name="local"></param>
        /// <returns></returns>
        public bool IsInChart(Vector2 local)
        {
            return IsInChart(local.x, local.y);
        }

        public bool IsInChart(float x, float y)
        {
            if (x < m_ChartX || x > m_ChartX + m_ChartWidth ||
               y < m_ChartY || y > m_ChartY + m_ChartHeight)
            {
                return false;
            }
            return true;
        }

        public void ClampInChart(ref Vector3 pos)
        {
            if (!IsInChart(pos.x, pos.y))
            {
                if (pos.x < m_ChartX) pos.x = m_ChartX;
                if (pos.x > m_ChartX + m_ChartWidth) pos.x = m_ChartX + m_ChartWidth;
                if (pos.y < m_ChartY) pos.y = m_ChartY;
                if (pos.y > m_ChartY + m_ChartHeight) pos.y = m_ChartY + m_ChartHeight;
            }
        }

        /// <summary>
        /// 是否可以开启背景组件。背景组件在chart受上层布局控制时无法开启。
        /// </summary>
        /// <returns></returns>
        public bool CanShowBackgroundComponent()
        {
            return !m_IsControlledByLayout && m_Background.runtimeActive;
        }

        /// <summary>
        /// 开启背景组件。背景组件在chart受上层布局控制时不适用。
        /// </summary>
        /// <param name="flag"></param>
        public void EnableBackground(bool flag)
        {
            if (flag && !CanShowBackgroundComponent())
            {
                var msg = "The background component cannot be activated because chart is controlled by LayoutGroup,"
                + " or its parent have more than one child.";
                Debug.LogError(msg);
                return;
            }
            m_Background.show = flag;
        }

        public Vector3 GetTitlePosition()
        {
            return chartPosition + m_Title.location.GetPosition(chartWidth, chartHeight);
        }

        [Obsolete("Use BaseChart.RefreshLabel() instead.", true)]
        public void ReinitChartLabel() { }

        [Obsolete("Use BaseChart.AnimationFadeIn() instead.", true)]
        public void AnimationStart() { }

        [Obsolete("Use BaseChart.AnimationFadeOut() instead.", true)]
        public void MissAnimationStart() { }

        [Obsolete("Use onCustomDraw instead.", false)]
        public Action<VertexHelper> customDrawCallback { set { m_OnCustomDrawCallback = value; } }
    }
}
