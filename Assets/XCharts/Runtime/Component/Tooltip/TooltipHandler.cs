/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class TooltipHandler : MainComponentHandler<Tooltip>
    {
        private static StringBuilder s_ContentBuilder = new StringBuilder(200);
        private List<ChartLabel> m_IndicatorLabels = new List<ChartLabel>();
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
        }

        public override void DrawTop(VertexHelper vh)
        {
            DrawTooltipIndicator(vh, component);
        }

        private void InitTooltip(Tooltip tooltip)
        {
            tooltip.painter = chart.m_PainterTop;
            tooltip.refreshComponent = delegate ()
            {
                var objName = ChartCached.GetComponentObjectName(tooltip);
                tooltip.gameObject = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                var tooltipObject = tooltip.gameObject;
                tooltipObject.transform.localPosition = Vector3.zero;
                tooltipObject.hideFlags = chart.chartHideFlags;
                var parent = tooltipObject.transform;
                var textStyle = tooltip.textStyle;
                ChartHelper.HideAllObject(tooltipObject.transform);
                GameObject content = ChartHelper.AddTooltipContent("content", parent, textStyle, chart.theme);
                tooltip.SetObj(tooltipObject);
                tooltip.SetContentObj(content);
                tooltip.SetContentBackgroundColor(TooltipHelper.GetTexBackgroundColor(tooltip, chart.theme.tooltip));
                tooltip.SetContentTextColor(TooltipHelper.GetTexColor(tooltip, chart.theme.tooltip));
                tooltip.SetActive(false);

                m_LabelRoot = ChartHelper.AddObject("label", tooltip.gameObject.transform, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                ChartHelper.HideAllObject(m_LabelRoot);
                m_IndicatorLabels.Clear();
                for (int i = 0; i < 2; i++)
                {
                    var labelName = "label_" + i;
                    var item = ChartHelper.AddTooltipLabel(component, labelName, m_LabelRoot.transform, chart.theme, new Vector2(0.5f, 0.5f));
                    m_IndicatorLabels.Add(item);
                }
            };
            tooltip.refreshComponent();
        }

        private ChartLabel GetIndicatorLabel(int index)
        {
            if (m_LabelRoot == null) return null;
            if (index < m_IndicatorLabels.Count) return m_IndicatorLabels[index];
            else
            {
                var labelName = "label_" + index;
                var item = ChartHelper.AddTooltipLabel(component, labelName, m_LabelRoot.transform, chart.theme, new Vector2(0.5f, 0.5f));
                m_IndicatorLabels.Add(item);
                return item;
            }
        }

        private void UpdateTooltip(Tooltip tooltip)
        {
            if (tooltip.trigger == Tooltip.Trigger.None) return;
            if (!chart.isPointerInChart || !tooltip.show || !tooltip.runtimeInited)
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
                if (tooltip.type == Tooltip.Type.Corss && (m_PointerContainer == null || m_PointerContainer.IsPointerEnter()))
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
                chart.RefreshTopPainter();
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
                    var labelCount = 0;
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
                                    var label = GetIndicatorLabel(labelCount++);
                                    if (label == null) continue;
                                    label.SetActive(true);
                                    label.SetPosition(axis.context.pointerLabelPosition);
                                    if (axis.IsCategory())
                                        label.SetText(axis.GetData((int)axis.context.pointerValue));
                                    else
                                        label.SetText(axis.context.pointerValue.ToString("f2"));
                                    var textColor = axis.axisLabel.textStyle.GetColor(chart.theme.axis.textColor);
                                    label.labelBackground.color = textColor;
                                }
                            }
                        }
                    }
                }
            }
        }

        private ISerieContainer GetPointerContainerAndSeries(Tooltip tooltip, List<Serie> list)
        {
            list.Clear();
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
                            if (serie is INeedSerieContainer
                                && (serie as INeedSerieContainer).containterInstanceId == component.instanceId)
                            {
                                if (tooltip.IsTriggerAxis())
                                {
                                    if (container is GridCoord)
                                    {
                                        var xAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
                                        var yAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
                                        serie.context.pointerEnter = true;
                                        UpdateAxisPointerDataIndex(serie, xAxis, yAxis, container as GridCoord);
                                    }
                                    list.Add(serie);
                                }
                                else if (serie.context.pointerEnter)
                                {
                                    list.Add(serie);
                                    return component as ISerieContainer;
                                }
                            }
                        }
                        return component as ISerieContainer;
                    }
                }
            }
            return null;
        }

        private void UpdateAxisPointerDataIndex(Serie serie, XAxis xAxis, YAxis yAxis, GridCoord grid)
        {
            serie.context.pointerAxisDataIndexs.Clear();
            if (yAxis.IsCategory())
            {
                serie.context.pointerAxisDataIndexs.Add((int)yAxis.context.pointerValue);
                yAxis.context.axisTooltipValue = yAxis.context.pointerValue;
            }
            else if (yAxis.IsTime())
            {
                GetSerieDataIndexByValue(serie, yAxis, grid);
            }
            else if (xAxis.IsCategory())
            {
                serie.context.pointerAxisDataIndexs.Add((int)xAxis.context.pointerValue);
                xAxis.context.axisTooltipValue = xAxis.context.pointerValue;
            }
            else// if (xAxis.IsTime())
            {
                GetSerieDataIndexByValue(serie, xAxis, grid);
            }
        }

        private void GetSerieDataIndexByValue(Serie serie, Axis axis, GridCoord grid, int dimension = 0)
        {
            var currValue = 0d;
            var lastValue = 0d;
            var nextValue = 0d;
            var axisValue = axis.context.pointerValue;
            var isTimeAxis = axis.IsTime();
            var dataCount = serie.dataCount;
            var themeSymbolSize = chart.theme.serie.scatterSymbolSize;
            serie.context.pointerAxisDataIndexs.Clear();
            var axisValueDistance = axis.GetDistance(axisValue, grid.context.width);
            for (int i = 0; i < dataCount; i++)
            {
                var serieData = serie.data[i];
                currValue = serieData.GetData(dimension);
                if (isTimeAxis)
                {
                    if (i == 0)
                    {
                        nextValue = serie.GetSerieData(i + 1).GetData(dimension);
                        if (axisValue <= currValue + (nextValue - currValue) / 2)
                        {
                            serie.context.pointerAxisDataIndexs.Add(i);
                            break;
                        }
                    }
                    else if (i == dataCount - 1)
                    {
                        if (axisValue > lastValue + (currValue - lastValue) / 2)
                        {
                            serie.context.pointerAxisDataIndexs.Add(i);
                            break;
                        }
                    }
                    else
                    {
                        nextValue = serie.GetSerieData(i + 1).GetData(dimension);
                        if (axisValue > (currValue - (currValue - lastValue) / 2) && axisValue <= currValue + (nextValue - currValue) / 2)
                        {
                            serie.context.pointerAxisDataIndexs.Add(i);
                            break;
                        }
                    }
                    lastValue = currValue;
                }
                else
                {
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                    var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);
                    var dist = axis.GetDistance(currValue, grid.context.width) - axisValueDistance;
                    if (System.Math.Abs(dist) <= symbolSize)
                    {
                        serie.context.pointerAxisDataIndexs.Add(i);
                        serieData.highlighted = true;
                    }
                    else
                    {
                        serieData.highlighted = false;
                    }
                }
            }
            if (serie.context.pointerAxisDataIndexs.Count > 0)
            {
                var index = serie.context.pointerAxisDataIndexs[0];
                axis.context.axisTooltipValue = serie.GetSerieData(index).GetData(0);
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
            s_ContentBuilder.Length = 0;
            serie.handler.SetDefaultTooltipContent(tooltip, s_ContentBuilder);
            tooltip.SetActive(true);
            tooltip.UpdateContentPos(chart.pointerPos + tooltip.offset);
            TooltipHelper.SetContentAndPosition(tooltip, s_ContentBuilder.ToString(), chart.chartRect);
            return true;
        }

        private bool SetSerieTooltip(Tooltip tooltip, List<Serie> series)
        {
            if (tooltip.trigger == Tooltip.Trigger.None) return false;
            s_ContentBuilder.Length = 0;
            var showContent = false;
            if (m_PointerContainer is GridCoord)
            {
                if (tooltip.trigger == Tooltip.Trigger.Axis)
                {
                    var grid = m_PointerContainer as GridCoord;
                    var category = GetAxisCategory(grid.index);
                    if (!string.IsNullOrEmpty(category))
                        s_ContentBuilder.Append(category).Append(FormatterHelper.PH_NN);
                }
                for (int i = 0; i < series.Count; i++)
                {
                    var serie = series[i];
                    if (serie.handler.SetDefaultTooltipContent(tooltip, s_ContentBuilder))
                    {
                        showContent = true;
                        if (i != series.Count - 1)
                            s_ContentBuilder.Append(FormatterHelper.PH_NN);
                    }
                }
                if (showContent)
                {
                    tooltip.SetActive(true);
                    tooltip.SetContentActive(true);
                    tooltip.UpdateContentPos(chart.pointerPos + tooltip.offset);
                    TooltipHelper.SetContentAndPosition(tooltip, s_ContentBuilder.ToString(), chart.chartRect);
                }
            }
            return showContent;
        }

        private string GetAxisCategory(int gridIndex)
        {
            foreach (var component in chart.components)
            {
                if (component is Axis)
                {
                    var axis = component as Axis;
                    if (axis.gridIndex == gridIndex && axis.IsCategory())
                    {
                        return axis.GetData((int)axis.context.pointerValue);
                    }
                }
            }
            return null;
        }

        private void DrawTooltipIndicator(VertexHelper vh, Tooltip tooltip)
        {
            if (!tooltip.show) return;
            if (tooltip.type == Tooltip.Type.None) return;
            if (m_PointerContainer is GridCoord)
            {
                var grid = m_PointerContainer as GridCoord;
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
                    var dataZoom = chart.GetDataZoomOfAxis(xAxis);
                    int dataCount = chart.series.Count > 0 ? chart.series[0].GetDataList(dataZoom).Count : 0;
                    float splitWidth = AxisHelper.GetDataWidth(xAxis, grid.context.width, dataCount, dataZoom);
                    switch (tooltip.type)
                    {
                        case Tooltip.Type.Corss:
                        case Tooltip.Type.Line:
                            float pX = grid.context.x;
                            pX += xAxis.IsCategory()
                                ? (float)(xAxis.context.pointerValue * splitWidth + (xAxis.boundaryGap ? splitWidth / 2 : 0))
                                : xAxis.GetDistance(xAxis.context.axisTooltipValue, grid.context.width);
                            //if (xAxis.IsValue()) pX = chart.pointerPos.x;
                            Vector2 sp = new Vector2(pX, grid.context.y);
                            Vector2 ep = new Vector2(pX, grid.context.y + grid.context.height);
                            var lineColor = TooltipHelper.GetLineColor(tooltip, chart.theme);
                            ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                            if (tooltip.type == Tooltip.Type.Corss)
                            {
                                sp = new Vector2(grid.context.x, chart.pointerPos.y);
                                ep = new Vector2(grid.context.x + grid.context.width, chart.pointerPos.y);
                                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, sp, ep, lineColor);
                            }
                            break;
                        case Tooltip.Type.Shadow:
                            if (xAxis.IsCategory())
                            {
                                float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                                pX = (float)(grid.context.x + splitWidth * xAxis.context.pointerValue -
                                    (xAxis.boundaryGap ? 0 : splitWidth / 2));
                                float pY = grid.context.y + grid.context.height;
                                Vector3 p1 = new Vector3(pX, grid.context.y);
                                Vector3 p2 = new Vector3(pX, pY);
                                Vector3 p3 = new Vector3(pX + tooltipSplitWid, pY);
                                Vector3 p4 = new Vector3(pX + tooltipSplitWid, grid.context.y);
                                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, chart.theme.tooltip.areaColor);
                            }
                            break;
                    }
                }
            }
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
                    var dataZoom = chart.GetDataZoomOfAxis(yAxis);
                    int dataCount = chart.series.Count > 0 ? chart.series[0].GetDataList(dataZoom).Count : 0;
                    float splitWidth = AxisHelper.GetDataWidth(yAxis, grid.context.height, dataCount, dataZoom);
                    switch (tooltip.type)
                    {
                        case Tooltip.Type.Corss:
                        case Tooltip.Type.Line:
                            float pY = (float)(grid.context.y + yAxis.context.pointerValue * splitWidth
                                + (yAxis.boundaryGap ? splitWidth / 2 : 0));
                            Vector2 sp = new Vector2(grid.context.x, pY);
                            Vector2 ep = new Vector2(grid.context.x + grid.context.width, pY);
                            var lineColor = TooltipHelper.GetLineColor(tooltip, chart.theme);
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
                                pY = (float)(grid.context.y + splitWidth * yAxis.context.pointerValue -
                                    (yAxis.boundaryGap ? 0 : splitWidth / 2));
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
            if (tooltip.runtimeAngle < 0) return;
            var theme = chart.theme;
            var m_AngleAxis = ComponentHelper.GetAngleAxis(chart.components, m_Polar.index);
            var lineColor = TooltipHelper.GetLineColor(tooltip, theme);
            var lineType = tooltip.lineStyle.GetType(theme.tooltip.lineType);
            var lineWidth = tooltip.lineStyle.GetWidth(theme.tooltip.lineWidth);
            var cenPos = m_Polar.context.center;
            var radius = m_Polar.context.radius;
            var sp = m_Polar.context.center;
            var tooltipAngle = tooltip.runtimeAngle + m_AngleAxis.startAngle;
            var ep = ChartHelper.GetPos(sp, radius, tooltipAngle, true);

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