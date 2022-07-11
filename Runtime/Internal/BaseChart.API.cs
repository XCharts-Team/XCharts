using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// The base class of all charts.
    /// |所有Chart的基类。
    /// </summary>
    public partial class BaseChart
    {
        /// <summary>
        /// The name of chart.
        /// |</summary>
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
        /// |</summary>
        public ThemeStyle theme { get { return m_Theme; } set { m_Theme = value; } }
        /// <summary>
        /// Global parameter setting component.
        /// |全局设置组件。
        /// </summary>
        public Settings settings { get { return m_Settings; } }
        /// <summary>
        /// The x of chart.
        /// |图表的X
        /// </summary>
        public float chartX { get { return m_ChartX; } }
        /// <summary>
        /// The y of chart.
        /// |图表的Y
        /// </summary>
        public float chartY { get { return m_ChartY; } }
        /// <summary>
        /// The width of chart.
        /// |图表的宽
        /// </summary>
        public float chartWidth { get { return m_ChartWidth; } }
        /// <summary>
        /// The height of chart.
        /// |图表的高
        /// </summary>
        public float chartHeight { get { return m_ChartHeight; } }
        public Vector2 chartMinAnchor { get { return m_ChartMinAnchor; } }
        public Vector2 chartMaxAnchor { get { return m_ChartMaxAnchor; } }
        public Vector2 chartPivot { get { return m_ChartPivot; } }
        public Vector2 chartSizeDelta { get { return m_ChartSizeDelta; } }
        /// <summary>
        /// The position of chart.
        /// |图表的左下角起始坐标。
        /// </summary>
        public Vector3 chartPosition { get { return m_ChartPosition; } }
        public Rect chartRect { get { return m_ChartRect; } }
        public Action onInit { set { m_OnInit = value; } }
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
        /// the callback function of click pie area.
        /// |点击饼图区域回调。参数：PointerEventData，SerieIndex，SerieDataIndex
        /// </summary>
        public Action<PointerEventData, int, int> onPointerClickPie { set { m_OnPointerClickPie = value; m_ForceOpenRaycastTarget = true; } get { return m_OnPointerClickPie; } }
        /// <summary>
        /// the callback function of click bar.
        /// |点击柱形图柱条回调。参数：eventData, dataIndex
        /// </summary>
        public Action<PointerEventData, int> onPointerClickBar { set { m_OnPointerClickBar = value; m_ForceOpenRaycastTarget = true; } get { return m_OnPointerClickBar; } }
        /// <summary>
        /// 坐标轴变更数据索引时回调。参数：axis, dataIndex/dataValue
        /// </summary>
        public Action<Axis, double> onAxisPointerValueChanged { set { m_OnAxisPointerValueChanged = value; } get { return m_OnAxisPointerValueChanged; } }
        /// <summary>
        /// the callback function of click legend.
        /// |点击图例按钮回调。参数：legendIndex, legendName, show
        /// </summary>
        public Action<Legend, int, string, bool> onLegendClick { set { m_OnLegendClick = value; } internal get { return m_OnLegendClick; } }
        /// <summary>
        /// the callback function of enter legend.
        /// |鼠标进入图例回调。参数：legendIndex, legendName
        /// </summary>
        public Action<Legend, int, string> onLegendEnter { set { m_OnLegendEnter = value; } internal get { return m_OnLegendEnter; } }
        /// <summary>
        /// the callback function of exit legend.
        /// |鼠标退出图例回调。参数：legendIndex, legendName
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
        /// |在下一帧刷新整个图表。
        /// </summary>
        public void RefreshChart()
        {
            foreach (var serie in m_Series)
                serie.ResetInteract();
            m_RefreshChart = true;
            if (m_Painter) m_Painter.Refresh();
            foreach (var painter in m_PainterList) painter.Refresh();
            if (m_PainterUpper) m_PainterUpper.Refresh();
            if (m_PainterTop) m_PainterTop.Refresh();
        }

        /// <summary>
        /// Redraw chart serie in next frame.
        /// |在下一帧刷新图表的指定serie。
        /// </summary>
        public void RefreshChart(int serieIndex)
        {
            RefreshPainter(GetSerie(serieIndex));
        }

        /// <summary>
        /// Redraw chart serie in next frame.
        /// |在下一帧刷新图表的指定serie。
        /// </summary>
        public void RefreshChart(Serie serie)
        {
            if (serie == null) return;
            serie.ResetInteract();
            RefreshPainter(serie);
        }

        /// <summary>
        /// Remove all series and legend data.
        /// |It just emptying all of serie's data without emptying the list of series.
        /// |清除所有数据，系列中只是移除数据，列表会保留。
        /// </summary>
        public virtual void ClearData()
        {
            foreach (var serie in m_Series)
                serie.ClearData();
            foreach (var component in m_Components)
                component.ClearData();
            m_CheckAnimation = false;
            RefreshChart();
        }

        /// <summary>
        /// Remove all data from series and legend.
        /// |The series list is also cleared.
        /// |清除所有系列和图例数据，系列的列表也会被清除。
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
        /// Remove legend and serie by name.
        /// |清除指定系列名称的数据。
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
        /// |获得指定图例名字的系列是否显示。
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
        /// |切换内置主题。
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
        /// |切换图表主题。
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
        /// Whether series animation enabel.
        /// |启用或关闭起始动画。
        /// </summary>
        /// <param name="flag"></param>
        public void AnimationEnable(bool flag)
        {
            foreach (var serie in m_Series) serie.AnimationEnable(flag);
        }

        /// <summary>
        /// fadeIn animation.
        /// |开始渐入动画。
        /// </summary>
        public void AnimationFadeIn()
        {
            foreach (var serie in m_Series) serie.AnimationFadeIn();
        }

        /// <summary>
        /// fadeIn animation.
        /// |开始渐出动画。
        /// </summary>
        public void AnimationFadeOut()
        {
            foreach (var serie in m_Series) serie.AnimationFadeOut();
        }

        /// <summary>
        /// Pause animation.
        /// |暂停动画。
        /// </summary>
        public void AnimationPause()
        {
            foreach (var serie in m_Series) serie.AnimationPause();
        }

        /// <summary>
        /// Stop play animation.
        /// |继续动画。
        /// </summary>
        public void AnimationResume()
        {
            foreach (var serie in m_Series) serie.AnimationResume();
        }

        /// <summary>
        /// Reset animation.
        /// |重置动画。
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
        public void CovertXYAxis(int index)
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

        public Color32 GetItemColor(Serie serie, SerieData serieData, bool highlight = false)
        {
            var colorIndex = serieData == null || !serie.useDataNameForColor ?
                GetLegendRealShowNameIndex(serie.legendName) :
                GetLegendRealShowNameIndex(serieData.legendName);
            return SerieHelper.GetItemColor(serie, serieData, m_Theme, colorIndex, highlight);
        }

        public Color32 GetItemColor(Serie serie, bool highlight = false)
        {
            return SerieHelper.GetItemColor(serie, null, m_Theme, serie.context.colorIndex, highlight);
        }
    }
}