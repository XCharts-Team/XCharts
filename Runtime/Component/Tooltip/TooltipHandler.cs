using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class TooltipHandler : MainComponentHandler<Tooltip>
    {
        private Dictionary<string, ChartLabel> m_IndicatorLabels = new Dictionary<string, ChartLabel>();
        private GameObject m_LabelRoot;
        private ISerieContainer m_PointerContainer;

        public override void InitComponent()
        {
            InitTooltip(component);
        }

        public override void Update()
        {
            UpdateTooltip(component);
            UpdateTooltipIndicatorLabelText(component);
            if (component.view != null)
                component.view.Update();
        }

        public override void DrawUpper(VertexHelper vh)
        {
            DrawTooltipIndicator(vh, component);
        }

        private void InitTooltip(Tooltip tooltip)
        {
            tooltip.painter = chart.m_PainterUpper;
            tooltip.refreshComponent = delegate()
            {
                var objName = ChartCached.GetComponentObjectName(tooltip);
                tooltip.gameObject = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                var tooltipObject = tooltip.gameObject;
                tooltipObject.transform.localPosition = Vector3.zero;
                tooltipObject.hideFlags = chart.chartHideFlags;
                var parent = tooltipObject.transform;
                ChartHelper.HideAllObject(tooltipObject.transform);

                tooltip.view = TooltipView.CreateView(tooltip, chart.theme, parent);
                tooltip.SetActive(false);

                m_LabelRoot = ChartHelper.AddObject("label", tooltip.gameObject.transform, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                ChartHelper.HideAllObject(m_LabelRoot);
                m_IndicatorLabels.Clear();
                foreach (var com in chart.components)
                {
                    if (com is Axis)
                    {
                        var axis = com as Axis;
                        var labelName = ChartCached.GetComponentObjectName(axis);
                        var item = ChartHelper.AddTooltipIndicatorLabel(component, labelName, m_LabelRoot.transform,
                            chart.theme, axis.context.aligment, axis.indicatorLabel);
                        item.SetActive(false);
                        m_IndicatorLabels[labelName] = item;
                    }
                }
            };
            tooltip.refreshComponent();
        }

        private ChartLabel GetIndicatorLabel(Axis axis)
        {
            if (m_LabelRoot == null) return null;
            var key = ChartCached.GetComponentObjectName(axis);
            if (m_IndicatorLabels.ContainsKey(key))
            {
                return m_IndicatorLabels[key];
            }
            else
            {
                var item = ChartHelper.AddTooltipIndicatorLabel(component, key, m_LabelRoot.transform,
                    chart.theme, TextAnchor.MiddleCenter, axis.indicatorLabel);
                m_IndicatorLabels[key] = item;
                return item;
            }
        }

        private void UpdateTooltip(Tooltip tooltip)
        {
            if (tooltip.trigger == Tooltip.Trigger.None) return;
            if (!chart.isPointerInChart || !tooltip.show)
            {
                if (tooltip.IsActive())
                {
                    tooltip.ClearValue();
                    tooltip.SetActive(false);
                }
                return;
            }
            var showTooltip = false;
            for (int i = chart.series.Count - 1; i >= 0; i--)
            {
                var serie = chart.series[i];
                if (!(serie is INeedSerieContainer))
                {
                    if (SetSerieTooltip(tooltip, serie))
                    {
                        showTooltip = true;
                        chart.RefreshTopPainter();
                        return;
                    }
                }
            }
            var containerSeries = ListPool<Serie>.Get();
            m_PointerContainer = GetPointerContainerAndSeries(tooltip, containerSeries);
            if (containerSeries.Count > 0)
            {
                if (SetSerieTooltip(tooltip, containerSeries))
                    showTooltip = true;
            }
            ListPool<Serie>.Release(containerSeries);
            if (!showTooltip)
            {
                if (tooltip.type == Tooltip.Type.Corss && m_PointerContainer != null && m_PointerContainer.IsPointerEnter())
                {
                    tooltip.SetActive(true);
                    tooltip.SetContentActive(false);
                }
                else
                {
                    tooltip.SetActive(false);
                }
            }
            else
            {
                chart.RefreshUpperPainter();
            }
        }

        private void UpdateTooltipIndicatorLabelText(Tooltip tooltip)
        {
            if (!tooltip.show) return;
            if (tooltip.type == Tooltip.Type.None) return;
            if (m_PointerContainer != null)
            {
                if (tooltip.type == Tooltip.Type.Corss)
                {
                    if (m_PointerContainer is GridCoord)
                    {
                        var grid = m_PointerContainer as GridCoord;
                        ChartHelper.HideAllObject(m_LabelRoot);
                        foreach (var component in chart.components)
                        {
                            if (component is XAxis || component is YAxis)
                            {
                                var axis = component as Axis;
                                if (axis.gridIndex == grid.index)
                                {
                                    var label = GetIndicatorLabel(axis);
                                    SetTooltipIndicatorLabel(tooltip, axis, label);
                                }
                            }
                        }
                    }
                    else if (m_PointerContainer is PolarCoord)
                    {
                        var polar = m_PointerContainer as PolarCoord;
                        ChartHelper.HideAllObject(m_LabelRoot);
                        foreach (var component in chart.components)
                        {
                            if (component is AngleAxis || component is RadiusAxis)
                            {
                                var axis = component as Axis;
                                if (axis.polarIndex == polar.index)
                                {
                                    var label = GetIndicatorLabel(axis);
                                    SetTooltipIndicatorLabel(tooltip, axis, label);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SetTooltipIndicatorLabel(Tooltip tooltip, Axis axis, ChartLabel label)
        {
            if (label == null) return;
            if (double.IsNaN(axis.context.pointerValue)) return;
            label.SetActive(true);
            label.SetTextActive(true);
            label.SetPosition(axis.context.pointerLabelPosition + axis.indicatorLabel.offset);

            if (axis.IsCategory())
            {
                var index = (int) axis.context.pointerValue;
                var category = axis.GetData(index);
                label.SetText(axis.indicatorLabel.GetFormatterContent(index, category));
            }
            else if (axis.IsTime())
            {
                label.SetText(axis.indicatorLabel.GetFormatterDateTime(0, axis.context.pointerValue, axis.context.minValue, axis.context.maxValue));
            }
            else
            {
                label.SetText(axis.indicatorLabel.GetFormatterContent(0, axis.context.pointerValue, axis.context.minValue, axis.context.maxValue, axis.IsLog()));
            }
            var textColor = axis.axisLabel.textStyle.GetColor(chart.theme.axis.textColor);
            if (ChartHelper.IsClearColor(axis.indicatorLabel.background.color))
                label.color = textColor;
            else
                label.color = axis.indicatorLabel.background.color;
            label.SetTextColor(Color.white);
        }

        private ISerieContainer GetPointerContainerAndSeries(Tooltip tooltip, List<Serie> list)
        {
            list.Clear();
            ISerieContainer target = null;
            for (int i = chart.components.Count - 1; i >= 0; i--)
            {
                var component = chart.components[i];
                if (component is ISerieContainer)
                {
                    var container = component as ISerieContainer;
                    if (container.IsPointerEnter())
                    {
                        foreach (var serie in chart.series)
                        {
                            if (serie is INeedSerieContainer &&
                                (serie as INeedSerieContainer).containterInstanceId == component.instanceId &&
                                !serie.placeHolder)
                            {
                                var isTriggerAxis = tooltip.IsTriggerAxis();
                                if (container is GridCoord)
                                {
                                    var xAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
                                    var yAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
                                    UpdateAxisPointerDataIndex(serie, xAxis, yAxis, container as GridCoord, isTriggerAxis);
                                }
                                else if (container is PolarCoord)
                                {
                                    var m_AngleAxis = ComponentHelper.GetAngleAxis(chart.components, container.index);
                                    tooltip.context.angle = (float) m_AngleAxis.context.pointerValue;
                                }
                                list.Add(serie);
                                if (!isTriggerAxis)
                                    chart.RefreshTopPainter();
                            }
                        }
                        target = container;
                    }
                }
            }
            return target;
        }

        private void UpdateAxisPointerDataIndex(Serie serie, XAxis xAxis, YAxis yAxis, GridCoord grid, bool isTriggerAxis)
        {
            serie.context.pointerAxisDataIndexs.Clear();
            if (serie is Heatmap)
            {
                GetSerieDataByXYAxis(serie, xAxis, yAxis);
            }
            else if (yAxis.IsCategory())
            {
                if (isTriggerAxis)
                {
                    serie.context.pointerEnter = true;
                    serie.context.pointerAxisDataIndexs.Add((int) yAxis.context.pointerValue);
                    yAxis.context.axisTooltipValue = yAxis.context.pointerValue;
                }
            }
            else if (yAxis.IsTime())
            {
                serie.context.pointerEnter = true;
                if (isTriggerAxis)
                    GetSerieDataIndexByAxis(serie, yAxis, grid);
                else
                    GetSerieDataIndexByItem(serie, yAxis, grid);
            }
            else if (xAxis.IsCategory())
            {
                if (isTriggerAxis)
                {
                    var index = serie.context.dataZoomStartIndex + (int) xAxis.context.pointerValue;
                    serie.context.pointerEnter = true;
                    serie.context.pointerAxisDataIndexs.Add(index);
                    serie.context.pointerItemDataIndex = index;
                    xAxis.context.axisTooltipValue = xAxis.context.pointerValue;
                }
            }
            else
            {
                serie.context.pointerEnter = true;
                if (isTriggerAxis)
                    GetSerieDataIndexByAxis(serie, xAxis, grid);
                else
                    GetSerieDataIndexByItem(serie, xAxis, grid);
            }
        }

        private void GetSerieDataByXYAxis(Serie serie, Axis xAxis, Axis yAxis)
        {
            var xAxisIndex = AxisHelper.GetAxisValueSplitIndex(xAxis, xAxis.context.pointerValue);
            var yAxisIndex = AxisHelper.GetAxisValueSplitIndex(yAxis, yAxis.context.pointerValue);
            serie.context.pointerItemDataIndex = -1;
            if (serie is Heatmap)
            {
                var heatmap = serie as Heatmap;
                if (heatmap.heatmapType == HeatmapType.Count)
                {
                    serie.context.pointerItemDataIndex = HeatmapHandler.GetGridKey(xAxisIndex, yAxisIndex);
                    return;
                }
            }
            foreach (var serieData in serie.data)
            {
                var x = AxisHelper.GetAxisValueSplitIndex(xAxis, serieData.GetData(0));
                var y = AxisHelper.GetAxisValueSplitIndex(yAxis, serieData.GetData(1));
                if (xAxisIndex == x && y == yAxisIndex)
                {
                    serie.context.pointerItemDataIndex = serieData.index;
                    break;
                }
            }
        }

        private void GetSerieDataIndexByAxis(Serie serie, Axis axis, GridCoord grid, int dimension = 0)
        {
            var currValue = 0d;
            var lastValue = 0d;
            var nextValue = 0d;
            var axisValue = axis.context.pointerValue;
            var isTimeAxis = axis.IsTime();
            var dataCount = serie.dataCount;
            var themeSymbolSize = chart.theme.serie.scatterSymbolSize;
            var data = serie.data;
            if (!isTimeAxis)
            {
                serie.context.sortedData.Clear();
                for (int i = 0; i < dataCount; i++)
                {
                    var serieData = serie.data[i];
                    serie.context.sortedData.Add(serieData);
                }
                serie.context.sortedData.Sort(delegate(SerieData a, SerieData b)
                {
                    return a.GetData(dimension).CompareTo(b.GetData(dimension));
                });
                data = serie.context.sortedData;
            }
            serie.context.pointerAxisDataIndexs.Clear();
            for (int i = 0; i < dataCount; i++)
            {
                var serieData = data[i];
                currValue = serieData.GetData(dimension);
                if (i == 0)
                {
                    nextValue = data[i + 1].GetData(dimension);
                    if (axisValue <= currValue + (nextValue - currValue) / 2)
                    {
                        serie.context.pointerAxisDataIndexs.Add(serieData.index);
                        break;
                    }
                }
                else if (i == dataCount - 1)
                {
                    if (axisValue > lastValue + (currValue - lastValue) / 2)
                    {
                        serie.context.pointerAxisDataIndexs.Add(serieData.index);
                        break;
                    }
                }
                else
                {
                    nextValue = data[i + 1].GetData(dimension);
                    if (axisValue > (currValue - (currValue - lastValue) / 2) && axisValue <= currValue + (nextValue - currValue) / 2)
                    {
                        serie.context.pointerAxisDataIndexs.Add(serieData.index);
                        break;
                    }
                }
                lastValue = currValue;
            }
            if (serie.context.pointerAxisDataIndexs.Count > 0)
            {
                var index = serie.context.pointerAxisDataIndexs[0];
                serie.context.pointerItemDataIndex = index;
                axis.context.axisTooltipValue = serie.GetSerieData(index).GetData(dimension);
            }
            else
            {
                serie.context.pointerItemDataIndex = -1;
                axis.context.axisTooltipValue = 0;
            }
        }

        private void GetSerieDataIndexByItem(Serie serie, Axis axis, GridCoord grid, int dimension = 0)
        {
            if (serie.context.pointerItemDataIndex >= 0)
            {
                axis.context.axisTooltipValue = serie.GetSerieData(serie.context.pointerItemDataIndex).GetData(dimension);
            }
            else if (component.type == Tooltip.Type.Corss)
            {
                axis.context.axisTooltipValue = axis.context.pointerValue;
            }
            else
            {
                axis.context.axisTooltipValue = 0;
            }
        }

        private bool SetSerieTooltip(Tooltip tooltip, Serie serie)
        {
            if (tooltip.trigger == Tooltip.Trigger.None) return false;
            if (serie.context.pointerItemDataIndex < 0) return false;

            tooltip.context.data.param.Clear();
            tooltip.context.data.title = serie.serieName;
            tooltip.context.pointer = chart.pointerPos;

            serie.handler.UpdateTooltipSerieParams(serie.context.pointerItemDataIndex, false, null,
                tooltip.marker, tooltip.itemFormatter, tooltip.numericFormatter, tooltip.ignoreDataDefaultContent,
                ref tooltip.context.data.param,
                ref tooltip.context.data.title);
            TooltipHelper.ResetTooltipParamsByItemFormatter(tooltip, chart);

            tooltip.SetActive(true);
            tooltip.view.Refresh();
            TooltipHelper.LimitInRect(tooltip, chart.chartRect);
            return true;
        }

        private bool SetSerieTooltip(Tooltip tooltip, List<Serie> series)
        {
            if (tooltip.trigger == Tooltip.Trigger.None)
                return false;

            if (series.Count <= 0)
                return false;

            string category = null;
            var showCategory = false;
            var isTriggerByAxis = false;
            var dataIndex = -1;
            tooltip.context.data.param.Clear();
            tooltip.context.pointer = chart.pointerPos;
            if (m_PointerContainer is GridCoord)
            {
                if (tooltip.trigger == Tooltip.Trigger.Axis)
                {
                    isTriggerByAxis = true;
                    GetAxisCategory(m_PointerContainer.index, ref dataIndex, ref category);
                    if (series.Count <= 1)
                    {
                        showCategory = true;
                        tooltip.context.data.title = series[0].serieName;
                    }
                    else
                        tooltip.context.data.title = category;
                }
            }

            for (int i = 0; i < series.Count; i++)
            {
                var serie = series[i];
                if (!serie.show) continue;
                serie.context.isTriggerByAxis = isTriggerByAxis;
                if (isTriggerByAxis && dataIndex >= 0 && serie.context.pointerItemDataIndex < 0)
                    serie.context.pointerItemDataIndex = dataIndex;
                serie.handler.UpdateTooltipSerieParams(dataIndex, showCategory, category,
                    tooltip.marker, tooltip.itemFormatter, tooltip.numericFormatter,
                    tooltip.ignoreDataDefaultContent,
                    ref tooltip.context.data.param,
                    ref tooltip.context.data.title);
            }
            TooltipHelper.ResetTooltipParamsByItemFormatter(tooltip, chart);
            if (tooltip.context.data.param.Count > 0)
            {
                tooltip.SetActive(true);
                if (tooltip.view != null)
                    tooltip.view.Refresh();
                TooltipHelper.LimitInRect(tooltip, chart.chartRect);
                return true;
            }
            return false;
        }

        private bool GetAxisCategory(int gridIndex, ref int dataIndex, ref string category)
        {
            foreach (var component in chart.components)
            {
                if (component is Axis)
                {
                    var axis = component as Axis;
                    if (axis.gridIndex == gridIndex && axis.IsCategory())
                    {
                        dataIndex = axis.context.dataZoomStartIndex + (int) axis.context.pointerValue;
                        category = axis.GetData(dataIndex);
                        return true;
                    }
                }
            }
            return false;
        }

        private void DrawTooltipIndicator(VertexHelper vh, Tooltip tooltip)
        {
            if (!tooltip.show) return;
            if (tooltip.type == Tooltip.Type.None) return;
            if (!IsAnySerieNeedTooltip()) return;
            if (m_PointerContainer is GridCoord)
            {
                var grid = m_PointerContainer as GridCoord;
                if (!grid.context.isPointerEnter) return;
                if (IsYCategoryOfGrid(grid.index))
                    DrawYAxisIndicator(vh, tooltip, grid);
                else
                    DrawXAxisIndicator(vh, tooltip, grid);
            }
            else if (m_PointerContainer is PolarCoord)
            {
                DrawPolarIndicator(vh, tooltip, m_PointerContainer as PolarCoord);
            }
        }

        private bool IsYCategoryOfGrid(int gridIndex)
        {
            var yAxes = chart.GetChartComponents<YAxis>();
            foreach (var component in yAxes)
            {
                var yAxis = component as YAxis;
                if (yAxis.gridIndex == gridIndex && yAxis.IsCategory()) return true;
            }
            return false;
        }

        private void DrawXAxisIndicator(VertexHelper vh, Tooltip tooltip, GridCoord grid)
        {
            var xAxes = chart.GetChartComponents<XAxis>();
            var lineType = tooltip.lineStyle.GetType(chart.theme.tooltip.lineType);
            var lineWidth = tooltip.lineStyle.GetWidth(chart.theme.tooltip.lineWidth);
            foreach (var component in xAxes)
            {
                var xAxis = component as XAxis;
                if (xAxis.gridIndex == grid.index)
                {
                    if (double.IsInfinity(xAxis.context.pointerValue))
                        continue;
                    var dataZoom = chart.GetDataZoomOfAxis(xAxis);
                    int dataCount = chart.series.Count > 0 ? chart.series[0].GetDataList(dataZoom).Count : 0;
                    float splitWidth = AxisHelper.GetDataWidth(xAxis, grid.context.width, dataCount, dataZoom);
                    switch (tooltip.type)
                    {
                        case Tooltip.Type.Corss:
                        case Tooltip.Type.Line:
                            float pX = grid.context.x;
                            pX += xAxis.IsCategory() ?
                                (float) (xAxis.context.pointerValue * splitWidth + (xAxis.boundaryGap ? splitWidth / 2 : 0)) :
                                xAxis.GetDistance(xAxis.context.axisTooltipValue, grid.context.width);
                            if (pX < grid.context.x)
                                break;
                            Vector2 sp = new Vector2(pX, grid.context.y);
                            Vector2 ep = new Vector2(pX, grid.context.y + grid.context.height);
                            var lineColor = TooltipHelper.GetLineColor(tooltip, chart.theme.tooltip.lineColor);
                            ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                            if (tooltip.type == Tooltip.Type.Corss)
                            {
                                sp = new Vector2(grid.context.x, chart.pointerPos.y);
                                ep = new Vector2(grid.context.x + grid.context.width, chart.pointerPos.y);
                                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                            }
                            break;
                        case Tooltip.Type.Shadow:
                            if (xAxis.IsCategory() && !double.IsInfinity(xAxis.context.pointerValue))
                            {
                                float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                                pX = (float) (grid.context.x + splitWidth * xAxis.context.pointerValue -
                                    (xAxis.boundaryGap ? 0 : splitWidth / 2));
                                if (pX < grid.context.x)
                                    break;
                                float pY = grid.context.y + grid.context.height;
                                Vector3 p1 = chart.ClampInGrid(grid,new Vector3(pX, grid.context.y));
                                Vector3 p2 = chart.ClampInGrid(grid,new Vector3(pX, pY));
                                Vector3 p3 = chart.ClampInGrid(grid,new Vector3(pX + tooltipSplitWid, pY));
                                Vector3 p4 = chart.ClampInGrid(grid,new Vector3(pX + tooltipSplitWid, grid.context.y));
                                var areaColor = TooltipHelper.GetLineColor(tooltip, chart.theme.tooltip.areaColor);
                                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, areaColor);
                            }
                            break;
                    }
                }
            }
        }

        private bool IsAnySerieNeedTooltip()
        {
            foreach (var serie in chart.series)
            {
                if (serie.context.pointerEnter) return true;
            }
            return false;
        }
        private void DrawYAxisIndicator(VertexHelper vh, Tooltip tooltip, GridCoord grid)
        {
            var yAxes = chart.GetChartComponents<YAxis>();
            var lineType = tooltip.lineStyle.GetType(chart.theme.tooltip.lineType);
            var lineWidth = tooltip.lineStyle.GetWidth(chart.theme.tooltip.lineWidth);
            foreach (var component in yAxes)
            {
                var yAxis = component as YAxis;
                if (yAxis.gridIndex == grid.index)
                {
                    if (double.IsInfinity(yAxis.context.pointerValue))
                        continue;
                    var dataZoom = chart.GetDataZoomOfAxis(yAxis);
                    int dataCount = chart.series.Count > 0 ? chart.series[0].GetDataList(dataZoom).Count : 0;
                    float splitWidth = AxisHelper.GetDataWidth(yAxis, grid.context.height, dataCount, dataZoom);
                    switch (tooltip.type)
                    {
                        case Tooltip.Type.Corss:
                        case Tooltip.Type.Line:
                            float pY = (float) (grid.context.y + yAxis.context.pointerValue * splitWidth +
                                (yAxis.boundaryGap ? splitWidth / 2 : 0));
                            if (pY < grid.context.y)
                                break;
                            Vector2 sp = new Vector2(grid.context.x, pY);
                            Vector2 ep = new Vector2(grid.context.x + grid.context.width, pY);
                            var lineColor = TooltipHelper.GetLineColor(tooltip, chart.theme.tooltip.lineColor);
                            ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                            if (tooltip.type == Tooltip.Type.Corss)
                            {
                                sp = new Vector2(chart.pointerPos.x, grid.context.y);
                                ep = new Vector2(chart.pointerPos.x, grid.context.y + grid.context.height);
                                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                            }
                            break;
                        case Tooltip.Type.Shadow:
                            if (yAxis.IsCategory())
                            {
                                float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                                float pX = grid.context.x + grid.context.width;
                                pY = (float) (grid.context.y + splitWidth * yAxis.context.pointerValue -
                                    (yAxis.boundaryGap ? 0 : splitWidth / 2));
                                if (pY < grid.context.y)
                                    break;
                                Vector3 p1 = new Vector3(grid.context.x, pY);
                                Vector3 p2 = new Vector3(grid.context.x, pY + tooltipSplitWid);
                                Vector3 p3 = new Vector3(pX, pY + tooltipSplitWid);
                                Vector3 p4 = new Vector3(pX, pY);
                                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, chart.theme.tooltip.areaColor);
                            }
                            break;
                    }
                }
            }
        }

        private void DrawPolarIndicator(VertexHelper vh, Tooltip tooltip, PolarCoord m_Polar)
        {
            if (tooltip.context.angle < 0) return;
            var theme = chart.theme;
            var m_AngleAxis = ComponentHelper.GetAngleAxis(chart.components, m_Polar.index);
            var lineColor = TooltipHelper.GetLineColor(tooltip, theme.tooltip.lineColor);
            var lineType = tooltip.lineStyle.GetType(theme.tooltip.lineType);
            var lineWidth = tooltip.lineStyle.GetWidth(theme.tooltip.lineWidth);
            var cenPos = m_Polar.context.center;
            var radius = m_Polar.context.outsideRadius;
            var tooltipAngle = m_AngleAxis.GetValueAngle(tooltip.context.angle);

            var sp = ChartHelper.GetPos(m_Polar.context.center, m_Polar.context.insideRadius, tooltipAngle, true);
            var ep = ChartHelper.GetPos(m_Polar.context.center, m_Polar.context.outsideRadius, tooltipAngle, true);

            switch (tooltip.type)
            {
                case Tooltip.Type.Corss:
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                    var dist = Vector2.Distance(chart.pointerPos, cenPos);
                    if (dist > radius) dist = radius;
                    var outsideRaidus = dist + tooltip.lineStyle.GetWidth(theme.tooltip.lineWidth) * 2;
                    UGL.DrawDoughnut(vh, cenPos, dist, outsideRaidus, lineColor, Color.clear);
                    break;
                case Tooltip.Type.Line:
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                    break;
                case Tooltip.Type.Shadow:
                    UGL.DrawSector(vh, cenPos, radius, lineColor, tooltipAngle - 2, tooltipAngle + 2, chart.settings.cicleSmoothness);
                    break;
            }
        }
    }
}