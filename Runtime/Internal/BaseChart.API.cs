using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// The base class of all charts.
    /// ||所有Chart的基类。
    /// </summary>
    public partial class BaseChart
    {
        /// <summary>
        /// The name of chart.
        /// ||</summary>
        public string chartName
        {
            get { return m_ChartName; }
            set
            {
                if (!string.IsNullOrEmpty(value) && XChartsMgr.ContainsChart(value))
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
        /// ||</summary>
        public ThemeStyle theme { get { return m_Theme; } set { m_Theme = value; } }
        /// <summary>
        /// Global parameter setting component.
        /// ||全局设置组件。
        /// </summary>
        public Settings settings { get { return m_Settings; } }
        /// <summary>
        /// The x of chart.
        /// ||图表的X
        /// </summary>
        public float chartX { get { return m_ChartX; } }
        /// <summary>
        /// The y of chart.
        /// ||图表的Y
        /// </summary>
        public float chartY { get { return m_ChartY; } }
        /// <summary>
        /// The width of chart.
        /// ||图表的宽
        /// </summary>
        public float chartWidth { get { return m_ChartWidth; } }
        /// <summary>
        /// The height of chart.
        /// ||图表的高
        /// </summary>
        public float chartHeight { get { return m_ChartHeight; } }
        public Vector2 chartMinAnchor { get { return m_ChartMinAnchor; } }
        public Vector2 chartMaxAnchor { get { return m_ChartMaxAnchor; } }
        public Vector2 chartPivot { get { return m_ChartPivot; } }
        public Vector2 chartSizeDelta { get { return m_ChartSizeDelta; } }
        /// <summary>
        /// The position of chart.
        /// ||图表的左下角起始坐标。
        /// </summary>
        public Vector3 chartPosition { get { return m_ChartPosition; } }
        public Rect chartRect { get { return m_ChartRect; } }
        /// <summary>
        /// The callback function of chart init.
        /// ||图表的初始化完成回调。
        /// </summary>
        public Action onInit { set { m_OnInit = value; } }
        /// <summary>
        /// The callback function of chart update.
        /// ||图表的Update回调。
        /// </summary>
        public Action onUpdate { set { m_OnUpdate = value; } }
        /// <summary>
        /// 自定义绘制回调。在绘制Serie前调用。
        /// </summary>
        public Action<VertexHelper> onDraw { set { m_OnDrawBase = value; } }
        /// <summary>
        /// 自定义Serie绘制回调。在每个Serie绘制完前调用。
        /// </summary>
        public Action<VertexHelper, Serie> onDrawBeforeSerie { set { m_OnDrawSerieBefore = value; } }
        /// <summary>
        /// 自定义Serie绘制回调。在每个Serie绘制完后调用。
        /// </summary>
        public Action<VertexHelper, Serie> onDrawAfterSerie { set { m_OnDrawSerieAfter = value; } }
        /// <summary>
        /// 自定义Upper层绘制回调。在绘制Tooltip前调用。
        /// </summary>
        public Action<VertexHelper> onDrawUpper { set { m_OnDrawUpper = value; } }
        /// <summary>
        /// 自定义Top层绘制回调。在绘制Tooltip前调用。
        /// </summary>
        public Action<VertexHelper> onDrawTop { set { m_OnDrawTop = value; } }
        /// <summary>
        /// 自定义仪表盘指针绘制委托。
        /// </summary>
        public CustomDrawGaugePointerFunction customDrawGaugePointerFunction { set { m_CustomDrawGaugePointerFunction = value; } get { return m_CustomDrawGaugePointerFunction; } }
        /// <summary>
        /// the callback function of pointer click serie.
        /// ||鼠标点击Serie回调。
        /// </summary>
        [Since("v3.6.0")]
        public Action<SerieEventData> onSerieClick { set { m_OnSerieClick = value; m_ForceOpenRaycastTarget = true; } get { return m_OnSerieClick; } }
        /// <summary>
        /// the callback function of pointer down serie.
        /// ||鼠标按下Serie回调。
        /// </summary>
        [Since("v3.6.0")]
        public Action<SerieEventData> onSerieDown { set { m_OnSerieDown = value; m_ForceOpenRaycastTarget = true; } get { return m_OnSerieDown; } }
        /// <summary>
        /// the callback function of pointer enter serie.
        /// ||鼠标进入Serie回调。
        /// </summary>
        [Since("v3.6.0")]
        public Action<SerieEventData> onSerieEnter { set { m_OnSerieEnter = value; m_ForceOpenRaycastTarget = true; } get { return m_OnSerieEnter; } }
        /// <summary>
        /// the callback function of pointer exit serie.
        /// ||鼠标离开Serie回调。
        /// </summary>
        [Since("v3.6.0")]
        public Action<SerieEventData> onSerieExit { set { m_OnSerieExit = value; m_ForceOpenRaycastTarget = true; } get { return m_OnSerieExit; } }
        /// <summary>
        /// the callback function of pointer click pie area.
        /// ||点击饼图区域回调。参数：PointerEventData，SerieIndex，SerieDataIndex
        /// </summary>
        [Obsolete("Use \"onSerieClick\" instead", true)]
        public Action<PointerEventData, int, int> onPointerClickPie { get; set; }
        /// <summary>
        /// the callback function of pointer enter pie area.
        /// ||鼠标进入和离开饼图区域回调，SerieDataIndex为-1时表示离开。参数：PointerEventData，SerieIndex，SerieDataIndex
        /// </summary>
        [Since("v3.3.0")]
        [Obsolete("Use \"onSerieEnter\" instead", true)]
        public Action<int, int> onPointerEnterPie { set { m_OnPointerEnterPie = value; m_ForceOpenRaycastTarget = true; } get { return m_OnPointerEnterPie; } }
        /// <summary>
        /// the callback function of click bar.
        /// ||点击柱形图柱条回调。参数：eventData, dataIndex
        /// </summary>
        [Obsolete("Use \"onSerieClick\" instead", true)]
        public Action<PointerEventData, int> onPointerClickBar { get; set; }
        /// <summary>
        /// 坐标轴变更数据索引时回调。参数：axis, dataIndex/dataValue
        /// </summary>
        public Action<Axis, double> onAxisPointerValueChanged { set { m_OnAxisPointerValueChanged = value; } get { return m_OnAxisPointerValueChanged; } }
        /// <summary>
        /// the callback function of click legend.
        /// ||点击图例按钮回调。参数：legendIndex, legendName, show
        /// </summary>
        public Action<Legend, int, string, bool> onLegendClick { set { m_OnLegendClick = value; } internal get { return m_OnLegendClick; } }
        /// <summary>
        /// the callback function of enter legend.
        /// ||鼠标进入图例回调。参数：legendIndex, legendName
        /// </summary>
        public Action<Legend, int, string> onLegendEnter { set { m_OnLegendEnter = value; } internal get { return m_OnLegendEnter; } }
        /// <summary>
        /// the callback function of exit legend.
        /// ||鼠标退出图例回调。参数：legendIndex, legendName
        /// </summary>
        public Action<Legend, int, string> onLegendExit { set { m_OnLegendExit = value; } internal get { return m_OnLegendExit; } }

        public void Init(bool defaultChart = true)
        {
            if (defaultChart)
            {
                OnInit();
                DefaultChart();
            }
            else
            {
                OnBeforeSerialize();
            }
        }

        /// <summary>
        /// Redraw chart in next frame.
        /// ||在下一帧刷新整个图表。
        /// </summary>
        public void RefreshChart()
        {
            m_RefreshChart = true;
            if (m_Painter) m_Painter.Refresh();
            foreach (var painter in m_PainterList) painter.Refresh();
            if (m_PainterUpper) m_PainterUpper.Refresh();
            if (m_PainterTop) m_PainterTop.Refresh();
        }

        public override void RefreshGraph()
        {
            RefreshChart();
        }

        /// <summary>
        /// Redraw chart serie in next frame.
        /// ||在下一帧刷新图表的指定serie。
        /// </summary>
        public void RefreshChart(int serieIndex)
        {
            RefreshPainter(GetSerie(serieIndex));
        }

        /// <summary>
        /// Redraw chart serie in next frame.
        /// ||在下一帧刷新图表的指定serie。
        /// </summary>
        public void RefreshChart(Serie serie)
        {
            if (serie == null) return;
            // serie.ResetInteract();
            RefreshPainter(serie);
        }

        /// <summary>
        /// Clear all components and series data. Note: serie only empties the data and does not remove serie.
        /// ||清空所有组件和Serie的数据。注意：Serie只是清空数据，不会移除Serie。
        /// </summary>
        public virtual void ClearData()
        {
            ClearSerieData();
            ClearSerieLinks();
            ClearComponentData();
        }

        /// <summary>
        /// Clear the data of all series.
        /// ||清空所有serie的数据。
        /// </summary>
        [Since("v3.4.0")]
        public virtual void ClearSerieData()
        {
            foreach (var serie in m_Series)
                serie.ClearData();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Clear the link data of all series.
        /// ||清空所有serie的link数据。
        /// </summary>
        [Since("v3.10.0")]
        public virtual void ClearSerieLinks()
        {
            foreach (var serie in m_Series)
                serie.ClearLinks();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Clear the data of all components.
        /// ||清空所有组件的数据。
        /// </summary>
        [Since("v3.4.0")]
        public virtual void ClearComponentData()
        {
            foreach (var component in m_Components)
                component.ClearData();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Empty all component data and remove all series. Use the chart again and again to tell the truth.
        /// Note: The component only clears the data part, and the parameters are retained and not reset.
        /// ||清空所有组件数据，并移除所有Serie。一般在图表重新初始化时使用。
        /// 注意：组件只清空数据部分，参数会保留不会被重置。
        /// </summary>
        public virtual void RemoveData()
        {
            foreach (var component in m_Components)
                component.ClearData();
            m_Series.Clear();
            m_SerieHandlers.Clear();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Remove all of them Serie. This interface is used when Serie needs to be removed only, and RemoveData() is generally used in other cases.
        /// ||移除所有的Serie。当确认只需要移除Serie时使用该接口，其他情况下一般用RemoveData()。
        /// </summary>
        [Since("v3.2.0")]
        public virtual void RemoveAllSerie()
        {
            m_Series.Clear();
            m_SerieHandlers.Clear();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Remove legend and serie by name.
        /// ||清除指定系列名称的数据。
        /// </summary>
        /// <param name="serieName">the name of serie</param>
        public virtual void RemoveData(string serieName)
        {
            RemoveSerie(serieName);
            foreach (var component in m_Components)
            {
                if (component is Legend)
                {
                    var legend = component as Legend;
                    legend.RemoveData(serieName);
                }
            }
            RefreshChart();
        }

        public virtual void UpdateLegendColor(string legendName, bool active)
        {
            var legendIndex = m_LegendRealShowName.IndexOf(legendName);
            if (legendIndex >= 0)
            {
                foreach (var component in m_Components)
                {
                    if (component is Legend)
                    {
                        var legend = component as Legend;
                        var iconColor = LegendHelper.GetIconColor(this, legend, legendIndex, legendName, active);
                        var contentColor = LegendHelper.GetContentColor(this, legendIndex, legendName, legend, m_Theme, active);
                        legend.UpdateButtonColor(legendName, iconColor);
                        legend.UpdateContentColor(legendName, contentColor);
                    }
                }
            }
        }

        /// <summary>
        /// Whether serie is activated.
        /// ||获得指定图例名字的系列是否显示。
        /// </summary>
        /// <param name="legendName"></param>
        /// <returns></returns>
        public virtual bool IsActiveByLegend(string legendName)
        {
            foreach (var serie in m_Series)
            {
                if (serie.show && legendName.Equals(serie.serieName))
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
        /// Update chart theme.
        /// ||切换内置主题。
        /// </summary>
        /// <param name="theme">theme</param>
        public bool UpdateTheme(ThemeType theme)
        {
            if (theme == ThemeType.Custom)
            {
                Debug.LogError("UpdateTheme: not support switch to Custom theme.");
                return false;
            }
            if (m_Theme.sharedTheme == null)
                m_Theme.sharedTheme = XCThemeMgr.GetTheme(ThemeType.Default);
            m_Theme.sharedTheme.CopyTheme(theme);
            return true;
        }

        /// <summary>
        /// Update chart theme info.
        /// ||切换图表主题。
        /// </summary>
        /// <param name="theme">theme</param>
        public void UpdateTheme(Theme theme)
        {
            m_Theme.sharedTheme = theme;
            SetAllComponentDirty();
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>
        /// Whether enable serie animations.
        /// ||是否启用Serie动画。
        /// </summary>
        /// <param name="flag"></param>
        public void AnimationEnable(bool flag)
        {
            foreach (var serie in m_Series) serie.AnimationEnable(flag);
        }

        /// <summary>
        /// Start all serie fadein animations.
        /// ||开始所有Serie的渐入动画。
        /// </summary>
        /// <param name="reset">reset animation</param>
        public void AnimationFadeIn(bool reset = true)
        {
            if (reset) AnimationReset();
            foreach (var serie in m_Series) serie.AnimationFadeIn();
        }

        /// <summary>
        /// Start all serie fadeout animations.
        /// ||开始所有Serie的渐出动画。
        /// </summary>
        public void AnimationFadeOut()
        {
            foreach (var serie in m_Series) serie.AnimationFadeOut();
        }

        /// <summary>
        /// Pause all animations.
        /// ||暂停所有Serie的动画。
        /// </summary>
        public void AnimationPause()
        {
            foreach (var serie in m_Series) serie.AnimationPause();
        }

        /// <summary>
        /// Resume all animations.
        /// ||继续所有Serie的动画。
        /// </summary>
        public void AnimationResume()
        {
            foreach (var serie in m_Series) serie.AnimationResume();
        }

        /// <summary>
        /// Reset all animations.
        /// ||重置所有Serie的动画。
        /// </summary>
        public void AnimationReset()
        {
            foreach (var serie in m_Series) serie.AnimationReset();
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

        public Vector3 ClampInGrid(GridCoord grid, Vector3 pos)
        {
            if (grid.Contains(pos)) return pos;
            else
            {
                // var pos = new Vector3(pos.x, pos.y);
                if (pos.x < grid.context.x) pos.x = grid.context.x;
                if (pos.x > grid.context.x + grid.context.width) pos.x = grid.context.x + grid.context.width;
                if (pos.y < grid.context.y) pos.y = grid.context.y;
                if (pos.y > grid.context.y + grid.context.height) pos.y = grid.context.y + grid.context.height;
                return pos;
            }
        }

        /// <summary>
        /// 转换X轴和Y轴的配置
        /// </summary>
        /// <param name="index">坐标轴索引，0或1</param>
        public void ConvertXYAxis(int index)
        {
            List<MainComponent> m_XAxes;
            List<MainComponent> m_YAxes;
            m_ComponentMaps.TryGetValue(typeof(XAxis), out m_XAxes);
            m_ComponentMaps.TryGetValue(typeof(YAxis), out m_YAxes);
            if (index >= 0 && index <= 1)
            {
                var xAxis = m_XAxes[index] as XAxis;
                var yAxis = m_YAxes[index] as YAxis;
                var tempX = xAxis.Clone();
                xAxis.Copy(yAxis);
                yAxis.Copy(tempX);
                xAxis.context.offset = 0;
                yAxis.context.offset = 0;
                xAxis.context.minValue = 0;
                xAxis.context.maxValue = 0;
                yAxis.context.minValue = 0;
                yAxis.context.maxValue = 0;
                ResetChartStatus();
                RefreshChart();
            }
        }

        /// <summary>
        /// 在下一帧刷新DataZoom
        /// </summary>
        public void RefreshDataZoom()
        {
            foreach (var handler in m_ComponentHandlers)
            {
                if (handler is DataZoomHandler)
                {
                    (handler as DataZoomHandler).RefreshDataZoomLabel();
                }
            }
        }

        /// <summary>
        /// 设置可缓存的最大数据量。当数据量超过该值时，会自动删除第一个值再加入最新值。
        /// </summary>
        public void SetMaxCache(int maxCache)
        {
            foreach (var serie in m_Series)
                serie.maxCache = maxCache;
            foreach (var component in m_Components)
            {
                if (component is Axis)
                {
                    (component as Axis).maxCache = maxCache;
                }
            }
        }

        /// <summary>
        /// set insert data to head.
        /// ||设置数据插入到头部。
        /// </summary>
        /// <param name="insertDataToHead"></param>
        [Since("v3.11.0")]
        public void SetInsertDataToHead(bool insertDataToHead)
        {
            foreach (var serie in m_Series)
                serie.insertDataToHead = insertDataToHead;

            var coms = GetChartComponents<XAxis>();
            foreach (var com in coms)
            {
                var axis = com as XAxis;
                if (axis.type == Axis.AxisType.Category)
                    axis.insertDataToHead = insertDataToHead;
            }
        }

        public Vector3 GetTitlePosition(Title title)
        {
            return chartPosition + title.location.GetPosition(chartWidth, chartHeight);
        }

        public int GetLegendRealShowNameIndex(string name)
        {
            return m_LegendRealShowName.IndexOf(name);
        }

        public Color32 GetLegendRealShowNameColor(string name)
        {
            var index = GetLegendRealShowNameIndex(name);
            return theme.GetColor(index);
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
        /// 设置Upper Painter的材质球
        /// </summary>
        /// <param name="material"></param>
        public void SetUpperPainterMaterial(Material material)
        {
            settings.upperPainterMaterial = material;
            if (m_PainterUpper != null)
            {
                m_PainterUpper.material = material;
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

        public Color32 GetChartBackgroundColor()
        {
            var background = GetChartComponent<Background>();
            return theme.GetBackgroundColor(background);
        }

        /// <summary>
        /// 获得Serie的标识颜色。
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="serieData"></param>
        /// <returns></returns>
        [Since("v3.4.0")]
        public Color32 GetMarkColor(Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (ChartHelper.IsClearColor(itemStyle.markColor))
            {
                return GetItemColor(serie, serieData);
            }
            else
            {
                return itemStyle.markColor;
            }
        }

        public Color32 GetItemColor(Serie serie, SerieData serieData)
        {
            Color32 color, toColor;
            SerieHelper.GetItemColor(out color, out toColor, serie, serieData, m_Theme);
            return color;
        }

        public Color32 GetItemColor(Serie serie, SerieData serieData, int colorIndex)
        {
            Color32 color, toColor;
            SerieHelper.GetItemColor(out color, out toColor, serie, serieData, m_Theme, colorIndex);
            return color;
        }

        public Color32 GetItemColor(Serie serie)
        {
            Color32 color, toColor;
            SerieHelper.GetItemColor(out color, out toColor, serie, null, m_Theme);
            return color;
        }

        /// <summary>
        /// trigger tooltip by data index.
        /// ||尝试触发指定数据项的Tooltip.
        /// </summary>
        /// <param name="dataIndex">数据项索引</param>
        /// <param name="serieIndex">Serie索引，默认为第0个Serie</param>
        /// <returns></returns>
        [Since("v3.7.0")]
        public bool TriggerTooltip(int dataIndex, int serieIndex = 0)
        {
            var serie = GetSerie(serieIndex);
            if (serie == null) return false;
            var dataPoints = serie.context.dataPoints;
            var dataPoint = Vector3.zero;
            if (dataPoints.Count == 0)
            {
                if (serie.dataCount == 0) return false;
                dataIndex = dataIndex % serie.dataCount;
                var serieData = serie.GetSerieData(dataIndex);
                if (serieData == null) return false;
                dataPoint = serie.GetSerieData(dataIndex).context.position;
            }
            else
            {
                dataIndex = dataIndex % dataPoints.Count;
                dataPoint = dataPoints[dataIndex];
            }
            return TriggerTooltip(dataPoint);
        }

        /// <summary>
        /// trigger tooltip by chart local position.
        /// ||在指定的位置尝试触发Tooltip.
        /// </summary>
        /// <param name="localPosition"></param>
        /// <returns></returns>
        [Since("v3.7.0")]
        public bool TriggerTooltip(Vector3 localPosition)
        {
            var screenPoint = LocalPointToScreenPoint(localPosition);
            var eventData = new PointerEventData(EventSystem.current);
            eventData.position = screenPoint;
            OnPointerEnter(eventData);
            return true;
        }

        /// <summary>
        /// cancel tooltip.
        /// ||取消Tooltip.
        /// </summary>
        [Since("v3.7.0")]
        public void CancelTooltip()
        {
            pointerMoveEventData = null;
            pointerClickEventData = null;
            var tooltip = GetChartComponent<Tooltip>();
            if (tooltip != null)
            {
                tooltip.SetActive(false);
            }
        }

        /// <summary>
        /// reset chart status. When some parameters are set, due to the animation effect, the chart status may not be correct.
        /// ||重置图表状态。当设置某些参数后，由于动画影响，可能导致图表状态不正确，此时可以调用该接口重置图表状态。
        /// </summary>
        [Since("v3.10.0")]
        public void ResetChartStatus()
        {
            foreach (var component in m_Components) component.ResetStatus();
            foreach (var handler in m_SerieHandlers) handler.ForceUpdateSerieContext();
        }
    }
}