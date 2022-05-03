/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

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
        /// The theme.
        /// </summary>
        public ChartTheme theme { get { return m_Theme; } set { m_Theme = value; } }
        /// <summary>
        /// The title setting of chart.
        /// 标题组件
        /// </summary>
        public Title title { get { return m_Titles.Count > 0 ? m_Titles[0] : null; } }
        public List<Title> titles { get { return m_Titles; } }
        /// <summary>
        /// The legend setting of chart.
        /// 图例组件
        /// </summary>
        public Legend legend { get { return m_Legends.Count > 0 ? m_Legends[0] : null; } }
        public List<Legend> legends { get { return m_Legends; } }
        /// <summary>
        /// The tooltip setting of chart.
        /// 提示框组件
        /// </summary>
        public Tooltip tooltip { get { return m_Tooltips.Count > 0 ? m_Tooltips[0] : null; } }
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
        /// dataZoom component.
        /// 区域缩放组件。
        /// </summary>
        public DataZoom dataZoom { get { return m_DataZooms.Count > 0 ? m_DataZooms[0] : null; } }
        public List<DataZoom> dataZooms { get { return m_DataZooms; } }
        /// <summary>
        /// visualMap component.
        /// 视觉映射组件。
        /// </summary>
        public VisualMap visualMap { get { return m_VisualMaps.Count > 0 ? m_VisualMaps[0] : null; } }
        public List<VisualMap> visualMaps { get { return m_VisualMaps; } }
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
        public Vector2 chartMinAnchor { get { return m_ChartMinAnchor; } }
        public Vector2 chartMaxAnchor { get { return m_ChartMaxAnchor; } }
        public Vector2 chartPivot { get { return m_ChartPivot; } }
        public Vector2 chartSizeDelta { get { return m_ChartSizeDelta; } }
        /// <summary>
        /// The position of chart.
        /// 图表的左下角起始坐标。
        /// </summary>
        public Vector3 chartPosition { get { return m_ChartPosition; } }
        public Rect chartRect { get { return m_ChartRect; } }
        /// <summary>
        /// 自定义绘制回调。在绘制Serie前调用。
        /// </summary>
        public Action<VertexHelper> onCustomDraw { set { m_OnCustomDrawBaseCallback = value; } }
        /// <summary>
        /// 自定义Serie绘制回调。在每个Serie绘制完前调用。
        /// </summary>
        public Action<VertexHelper, Serie> onCustomDrawBeforeSerie { set { m_OnCustomDrawSerieBeforeCallback = value; } }
        /// <summary>
        /// 自定义Serie绘制回调。在每个Serie绘制完后调用。
        /// </summary>
        public Action<VertexHelper, Serie> onCustomDrawAfterSerie { set { m_OnCustomDrawSerieAfterCallback = value; } }
        /// <summary>
        /// 自定义Top绘制回调。在绘制Tooltip前调用。
        /// </summary>
        public Action<VertexHelper> onCustomDrawTop { set { m_OnCustomDrawTopCallback = value; } }
        /// <summary>
        /// the callback function of click pie area.
        /// 点击饼图区域回调。参数：PointerEventData，SerieIndex，SerieDataIndex
        /// </summary>
        public Action<PointerEventData, int, int> onPointerClickPie { set { m_OnPointerClickPie = value; m_ForceOpenRaycastTarget = true; } get { return m_OnPointerClickPie; } }
        /// <summary>
        /// the callback function of click legend.
        /// 点击图例按钮回调。参数：legendIndex, legendName, show
        /// </summary>
        public Action<int, string, bool> onLegendClick { set { m_OnLegendClick = value; } }
        /// <summary>
        /// the callback function of enter legend.
        /// 鼠标进入图例回调。参数：legendIndex, legendName
        /// </summary>
        public Action<int, string> onLegendEnter { set { m_OnLegendEnter = value; } }
        /// <summary>
        /// the callback function of exit legend.
        /// 鼠标退出图例回调。参数：legendIndex, legendName
        /// </summary>
        public Action<int, string> onLegendExit { set { m_OnLegendExit = value; } }
        /// <summary>
        /// Redraw chart in next frame.
        /// 在下一帧刷新图表。
        /// </summary>
        public void RefreshChart()
        {
            m_RefreshChart = true;
            if (m_Painter) m_Painter.Refresh();
        }

        /// <summary>
        /// Remove all series and legend data.
        /// It just emptying all of serie's data without emptying the list of series.
        /// 清除所有数据，系列中只是移除数据，列表会保留。
        /// </summary>
        public virtual void ClearData()
        {
            m_Series.ClearData();
            foreach (var legend in m_Legends) legend.ClearData();
            tooltip.ClearValue();
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
            foreach (var legend in m_Legends) legend.ClearData();
            foreach (var radar in m_Radars) radar.indicatorList.Clear();
            m_Series.RemoveAll();
            tooltip.ClearValue();
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
            m_Series.RemoveSerie(serieName);
            foreach (var legend in m_Legends) legend.RemoveData(serieName);
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
        public virtual Serie AddSerie(SerieType type, string serieName = null, bool show = true, bool addToHead = false)
        {
            return m_Series.AddSerie(type, serieName, show, addToHead);
        }

        /// <summary>
        /// Add a serie to serie list.
        /// 通过字符串类型的serieType添加一个系列到系列列表中。如果serieType不是已定义的SerieType类型，则设置为Custom类型。
        /// </summary>
        /// <param name="serieType"></param>
        /// <param name="serieName"></param>
        /// <param name="show"></param>
        /// <returns></returns>
        public virtual Serie AddSerie(string serieType, string serieName = null, bool show = true, bool addToHead = false)
        {
            var type = SerieType.Custom;
            var list = Enum.GetNames(typeof(SerieType));
            foreach (var t in list)
            {
                if (t.Equals(serieType)) type = (SerieType)Enum.Parse(typeof(SerieType), t);
            }
            return AddSerie(type, serieName, show, addToHead);
        }

        public virtual Serie InsertSerie(int index, SerieType serieType, string serieName = null, bool show = true)
        {
            return m_Series.InsertSerie(index, serieType, serieName, show);
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
        public virtual SerieData AddData(string serieName, double data, string dataName = null)
        {
            var serieData = m_Series.AddData(serieName, data, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieName);
                if (SerieHelper.GetSerieLabel(serie, serieData).show)
                {
                    RefreshLabel();
                }
                RefreshPainter(serie);
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
        public virtual SerieData AddData(int serieIndex, double data, string dataName = null)
        {
            var serieData = m_Series.AddData(serieIndex, data, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieIndex);
                if (SerieHelper.GetSerieLabel(serie, serieData).show)
                {
                    RefreshLabel();
                }
                RefreshPainter(serie);
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
        public virtual SerieData AddData(string serieName, List<double> multidimensionalData, string dataName = null)
        {
            var serieData = m_Series.AddData(serieName, multidimensionalData, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieName);
                if (SerieHelper.GetSerieLabel(serie, serieData).show)
                {
                    RefreshLabel();
                }
                RefreshPainter(serie);
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
        public virtual SerieData AddData(int serieIndex, List<double> multidimensionalData, string dataName = null)
        {
            var serieData = m_Series.AddData(serieIndex, multidimensionalData, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieIndex);
                if (SerieHelper.GetSerieLabel(serie, serieData).show)
                {
                    RefreshLabel();
                }
                RefreshPainter(serie);
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
        public virtual SerieData AddData(string serieName, double xValue, double yValue, string dataName = null)
        {
            var serieData = m_Series.AddXYData(serieName, xValue, yValue, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieName);
                if (SerieHelper.GetSerieLabel(serie, serieData).show)
                {
                    RefreshLabel();
                }
                RefreshPainter(serie);
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
        public virtual SerieData AddData(int serieIndex, double xValue, double yValue, string dataName = null)
        {
            var serieData = m_Series.AddXYData(serieIndex, xValue, yValue, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieIndex);
                if (SerieHelper.GetSerieLabel(serie, serieData).show)
                {
                    RefreshLabel();
                }
                RefreshPainter(serie);
            }
            return serieData;
        }
        public virtual SerieData AddData(int serieIndex, double open, double close, double lowest, double heighest, string dataName = null)
        {
            var serieData = m_Series.AddData(serieIndex, open, close, lowest, heighest, dataName);
            if (serieData != null)
            {
                var serie = m_Series.GetSerie(serieIndex);
                if (SerieHelper.GetSerieLabel(serie, serieData).show)
                {
                    RefreshLabel();
                }
                RefreshPainter(serie);
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
        public virtual bool UpdateData(string serieName, int dataIndex, double value)
        {
            if (m_Series.UpdateData(serieName, dataIndex, value))
            {
                RefreshPainter(m_Series.GetSerie(serieName));
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
        public virtual bool UpdateData(int serieIndex, int dataIndex, double value)
        {
            if (m_Series.UpdateData(serieIndex, dataIndex, value))
            {
                RefreshPainter(m_Series.GetSerie(serieIndex));
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
        public virtual bool UpdateData(string serieName, int dataIndex, List<double> multidimensionalData)
        {
            if (m_Series.UpdateData(serieName, dataIndex, multidimensionalData))
            {
                RefreshPainter(m_Series.GetSerie(serieName));
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
        public virtual bool UpdateData(int serieIndex, int dataIndex, List<double> multidimensionalData)
        {
            if (m_Series.UpdateData(serieIndex, dataIndex, multidimensionalData))
            {
                RefreshPainter(m_Series.GetSerie(serieIndex));
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
        public virtual bool UpdateData(string serieName, int dataIndex, int dimension, double value)
        {
            if (m_Series.UpdateData(serieName, dataIndex, dimension, value))
            {
                RefreshPainter(m_Series.GetSerie(serieName));
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
        public virtual bool UpdateData(int serieIndex, int dataIndex, int dimension, double value)
        {
            if (m_Series.UpdateData(serieIndex, dataIndex, dimension, value))
            {
                RefreshPainter(m_Series.GetSerie(serieIndex));
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

        public virtual void UpdateLegendColor(string legendName, bool active)
        {
            var legendIndex = m_LegendRealShowName.IndexOf(legendName);
            if (legendIndex >= 0)
            {
                foreach (var legend in m_Legends)
                {
                    var iconColor = LegendHelper.GetIconColor(this, legendIndex, legendName, active);
                    var contentColor = LegendHelper.GetContentColor(legendIndex, legend, m_Theme, active);
                    legend.UpdateButtonColor(legendName, iconColor);
                    legend.UpdateContentColor(legendName, contentColor);
                }
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
        /// 切换内置主题。
        /// </summary>
        /// <param name="theme">theme</param>
        public bool UpdateTheme(Theme theme)
        {
            if (theme == Theme.Custom)
            {
                Debug.LogError("UpdateTheme: not support switch to Custom theme.");
                return false;
            }
            m_Theme.theme = theme;
            return true;
        }

        /// <summary>
        /// Update chart theme info.
        /// 切换图表主题。
        /// </summary>
        /// <param name="theme">theme</param>
        public void UpdateTheme(ChartTheme theme)
        {
            m_Theme.CopyTheme(theme);
            SetAllComponentDirty();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Whether series animation enabel.
        /// 启用或关闭起始动画。
        /// </summary>
        /// <param name="flag"></param>
        public void AnimationEnable(bool flag)
        {
            foreach (var serie in m_Series.list) serie.AnimationEnable(flag);
        }

        /// <summary>
        /// fadeIn animation.
        /// 开始渐入动画。
        /// </summary>
        public void AnimationFadeIn()
        {
            foreach (var serie in m_Series.list) serie.AnimationFadeIn();
        }

        /// <summary>
        /// fadeIn animation.
        /// 开始渐出动画。
        /// </summary>
        public void AnimationFadeOut()
        {
            foreach (var serie in m_Series.list) serie.AnimationFadeOut();
        }

        /// <summary>
        /// Pause animation.
        /// 暂停动画。
        /// </summary>
        public void AnimationPause()
        {
            foreach (var serie in m_Series.list) serie.AnimationPause();
        }

        /// <summary>
        /// Stop play animation.
        /// 继续动画。
        /// </summary>
        public void AnimationResume()
        {
            foreach (var serie in m_Series.list) serie.AnimationResume();
        }

        /// <summary>
        /// Reset animation.
        /// 重置动画。
        /// </summary>
        public void AnimationReset()
        {
            foreach (var serie in m_Series.list) serie.AnimationReset();
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

        public Vector3 GetTitlePosition(Title title)
        {
            return chartPosition + title.location.GetPosition(chartWidth, chartHeight);
        }

        public bool ContainsSerie(SerieType serieType)
        {
            return SeriesHelper.ContainsSerie(m_Series, serieType);
        }

        public virtual bool AddDefaultCustomSerie(string serieName, int dataCount = 5)
        {
            return false;
        }

        public virtual string[] GetCustomSerieInspectorShowFileds()
        {
            return null;
        }
        public virtual string[][] GetCustomSerieInspectorCustomFileds()
        {
            return null;
        }
        public virtual string[] GetCustomChartInspectorShowFileds()
        {
            return null;
        }

        public virtual string GetCustomSerieTypeName()
        {
            return null;
        }

        public virtual bool GetCustomSerieDataNameForColor()
        {
            return false;
        }

        public int GetLegendRealShowNameIndex(string name)
        {
            return m_LegendRealShowName.IndexOf(name);
        }

        public virtual void InitCustomSerieTooltip(ref StringBuilder stringBuilder, Serie serie, int index)
        {
        }

        /// <summary>
        /// 设置Base Painter的材质球
        /// </summary>
        /// <param name="material"></param>
        public void SetBasePainterMaterial(Material material)
        {
            settings.basePainterMaterial = material;
            if (m_Painter != null)
            {
                m_Painter.material = material;
            }
        }

        /// <summary>
        /// 设置Serie Painter的材质球
        /// </summary>
        /// <param name="material"></param>
        public void SetSeriePainterMaterial(Material material)
        {
            settings.basePainterMaterial = material;
            if (m_PainterList != null)
            {
                foreach (var painter in m_PainterList)
                    painter.material = material;
            }
        }

        /// <summary>
        /// 设置Top Painter的材质球
        /// </summary>
        /// <param name="material"></param>
        public void SetTopPainterMaterial(Material material)
        {
            settings.topPainterMaterial = material;
            if (m_PainterTop != null)
            {
                m_PainterTop.material = material;
            }
        }
    }
}
