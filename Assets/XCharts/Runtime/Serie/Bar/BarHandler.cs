/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class BarHandler : SerieHandler<Bar>
    {
        List<List<SerieData>> m_StackSerieData = new List<List<SerieData>>();

        public override void Update()
        {
            base.Update();
            UpdateSerieContext();
        }

        public override bool SetDefaultTooltipContent(Tooltip tooltip, StringBuilder sb)
        {
            var dataIndex = serie.context.pointerItemDataIndex;
            if (!serie.context.pointerEnter || dataIndex < 0) return false;
            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null) return false;
            double xValue, yValue;
            serie.GetXYData(dataIndex, null, out xValue, out yValue);
            var isIngore = serie.IsIgnorePoint(dataIndex);
            var key = serieData.name;
            var numericFormatter = TooltipHelper.GetItemNumericFormatter(tooltip, serie, serieData);
            var valueTxt = isIngore ? tooltip.ignoreDataDefaultContent :
                ChartCached.FloatToStr(yValue, numericFormatter);
            switch (tooltip.trigger)
            {
                case Tooltip.Trigger.Item:
                    var category = string.Empty;
                    var xAxis = chart.GetChartComponent<XAxis>();
                    var yAxis = chart.GetChartComponent<YAxis>();
                    if (yAxis.IsCategory())
                        category = yAxis.GetData((int)yAxis.context.pointerValue);
                    else
                        category = xAxis.IsCategory() ? xAxis.GetData((int)xAxis.context.pointerValue) :
                            ChartCached.FloatToStr(xAxis.context.pointerValue, "F", 2);
                    if (!string.IsNullOrEmpty(serie.serieName))
                        sb.Append(serie.serieName).Append(FormatterHelper.PH_NN);
                    sb.Append("<color=#").Append(chart.theme.GetColorStr(serie.index)).Append(">● </color>");
                    if (!string.IsNullOrEmpty(category))
                        sb.Append(category).Append(":");
                    sb.Append(valueTxt);
                    break;
                case Tooltip.Trigger.Axis:
                    sb.Append("<color=#").Append(chart.theme.GetColorStr(serie.index)).Append(">● </color>");
                    if (!string.IsNullOrEmpty(serie.serieName))
                        sb.Append(serie.serieName).Append(":");
                    sb.Append(valueTxt);
                    break;
            }
            return true;
        }

        public override void DrawSerie(VertexHelper vh)
        {
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);
            var yCategory = ComponentHelper.IsAnyCategoryOfYAxis(chart.components);
            serie.dataPoints.Clear();
            if (yCategory) DrawYBarSerie(vh, serie, colorIndex);
            else DrawXBarSerie(vh, serie, colorIndex);
        }

        private void UpdateSerieContext()
        {
            if (!chart.isPointerInChart) return;
            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;
            foreach (var serieData in serie.data)
            {
                if (serieData.runtimeRect.Contains(chart.pointerPos))
                {
                    serie.context.pointerItemDataIndex = serieData.index;
                    serie.context.pointerEnter = true;
                    serieData.highlighted = true;
                    chart.RefreshTopPainter();
                }
                else
                {
                    serieData.highlighted = false;
                }
            }
        }

        private void DrawYBarSerie(VertexHelper vh, Bar serie, int colorIndex)
        {
            if (!serie.show) return;
            if (serie.animation.HasFadeOut()) return;
            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex)) return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex)) return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex)) return;
            var dataZoom = chart.GetDataZoomOfAxis(yAxis);
            var showData = serie.GetDataList(dataZoom);
            float categoryWidth = AxisHelper.GetDataWidth(yAxis, grid.context.height, showData.Count, dataZoom);
            float barGap = chart.GetSerieBarGap<Bar>();
            float totalBarWidth = chart.GetSerieTotalWidth<Bar>(categoryWidth, barGap);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float offset = (categoryWidth - totalBarWidth) / 2;
            float barGapWidth = barWidth + barWidth * barGap;
            float space = serie.barGap == -1 ? offset : offset + chart.GetSerieIndexIfStack<Bar>(serie) * barGapWidth;
            var isStack = SeriesHelper.IsStack<Bar>(chart.series, serie.stack);
            m_StackSerieData.Clear();
            if (isStack) SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);

            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series, serie.stack);
            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double xMinValue = xAxis.context.minValue;
            double xMaxValue = xAxis.context.maxValue;
            var isAllBarEnd = true;
            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;
            for (int i = serie.minShow; i < maxCount; i++)
            {
                var serieData = showData[i];
                if (!serieData.show || serie.IsIgnoreValue(serieData))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    continue;
                }
                var highlight = serie.data[i].highlighted
                    || serie.highlighted;
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, highlight);

                serieData.canShowLabel = true;
                double value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse, xMinValue, xMaxValue);
                float borderWidth = value == 0 ? 0 : itemStyle.runtimeBorderWidth;
                if (showData[i].IsDataChanged()) dataChanging = true;
                float axisLineWidth = value == 0 ? 0
                    : ((value < 0 ? -1 : 1) * yAxis.axisLine.GetWidth(chart.theme.axis.lineWidth));

                float pY = grid.context.y + i * categoryWidth;
                if (!yAxis.boundaryGap) pY -= categoryWidth / 2;
                float pX = grid.context.x + xAxis.context.offset + axisLineWidth;
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pX += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }

                var barHig = 0f;
                double valueTotal = 0f;
                if (isPercentStack)
                {
                    valueTotal = chart.GetSerieSameStackTotalValue<Bar>(serie.stack, i);
                    barHig = valueTotal != 0 ? (float)((value / valueTotal * grid.context.width)) : 0;
                }
                else
                {
                    if (yAxis.IsLog())
                    {
                        int minIndex = xAxis.GetLogMinIndex();
                        float nowIndex = xAxis.GetLogValue(value);
                        barHig = (nowIndex - minIndex) / xAxis.splitNumber * grid.context.width;
                    }
                    else
                    {
                        valueTotal = xMaxValue - xMinValue;
                        if (valueTotal != 0)
                            barHig = (float)((xMinValue > 0 ? value - xMinValue : value)
                                / valueTotal * grid.context.width);
                    }
                }
                serieData.runtimeStackHig = barHig;
                var isBarEnd = false;
                float currHig = chart.CheckSerieBarAnimation(serie, i, barHig, out isBarEnd);
                if (!isBarEnd) isAllBarEnd = false;
                Vector3 plt, prt, prb, plb, top;
                if (value < 0)
                {
                    plt = new Vector3(pX - borderWidth, pY + space + barWidth - borderWidth);
                    prt = new Vector3(pX + currHig + borderWidth, pY + space + barWidth - borderWidth);
                    prb = new Vector3(pX + currHig + borderWidth, pY + space + borderWidth);
                    plb = new Vector3(pX - borderWidth, pY + space + borderWidth);
                }
                else
                {
                    plt = new Vector3(pX + borderWidth, pY + space + barWidth - borderWidth);
                    prt = new Vector3(pX + currHig - borderWidth, pY + space + barWidth - borderWidth);
                    prb = new Vector3(pX + currHig - borderWidth, pY + space + borderWidth);
                    plb = new Vector3(pX + borderWidth, pY + space + borderWidth);
                }
                top = new Vector3(pX + currHig - borderWidth, pY + space + barWidth / 2);
                if (serie.clip)
                {
                    plt = chart.ClampInGrid(grid, plt);
                    prt = chart.ClampInGrid(grid, prt);
                    prb = chart.ClampInGrid(grid, prb);
                    plb = chart.ClampInGrid(grid, plb);
                    top = chart.ClampInGrid(grid, top);
                }
                serieData.runtimeRect = Rect.MinMaxRect(plb.x, plb.y, prb.x, prt.y);
                serie.dataPoints.Add(top);
                if (serie.show)
                {
                    switch (serie.barType)
                    {
                        case BarType.Normal:
                            DrawNormalBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, true, grid);
                            break;
                        case BarType.Zebra:
                            DrawZebraBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, true, grid);
                            break;
                        case BarType.Capsule:
                            DrawCapsuleBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, true, grid);
                            break;
                    }
                }
            }
            if (isAllBarEnd) serie.animation.AllBarEnd();
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }



        private void DrawXBarSerie(VertexHelper vh, Bar serie, int colorIndex)
        {
            if (!serie.show) return;
            if (serie.animation.HasFadeOut()) return;
            var yAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
            var xAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
            var grid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
            var dataZoom = chart.GetDataZoomOfAxis(xAxis);
            var showData = serie.GetDataList(dataZoom);
            var isStack = SeriesHelper.IsStack<Bar>(chart.series, serie.stack);
            m_StackSerieData.Clear();
            if (isStack) SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);
            float categoryWidth = AxisHelper.GetDataWidth(xAxis, grid.context.width, showData.Count, dataZoom);
            float barGap = chart.GetSerieBarGap<Bar>();
            float totalBarWidth = chart.GetSerieTotalWidth<Bar>(categoryWidth, barGap);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float offset = (categoryWidth - totalBarWidth) / 2;
            float barGapWidth = barWidth + barWidth * barGap;
            float space = serie.barGap == -1 ? offset : offset + chart.GetSerieIndexIfStack<Bar>(serie) * barGapWidth;
            int maxCount = serie.maxShow > 0
                ? (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;

            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series, serie.stack);
            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double xMinValue = xAxis.context.minValue;
            double xMaxValue = xAxis.context.maxValue;
            double yMinValue = yAxis.context.minValue;
            double yMaxValue = yAxis.context.maxValue;
            var isAllBarEnd = true;
            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;
            for (int i = serie.minShow; i < maxCount; i++)
            {
                var serieData = showData[i];
                if (!serieData.show || serie.IsIgnoreValue(serieData))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    continue;
                }
                var highlight = serie.data[i].highlighted
                    || serie.highlighted;
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, highlight);
                double value = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                float borderWidth = value == 0 ? 0 : itemStyle.runtimeBorderWidth;
                if (serieData.IsDataChanged()) dataChanging = true;
                float pX = grid.context.x + i * categoryWidth;
                if (xAxis.IsValue() || xAxis.IsTime())
                {
                    space = 0;
                    if ((xMaxValue - xMinValue) <= 0) pX = grid.context.x;
                    else pX = grid.context.x + (float)((serieData.GetData(0) - xMinValue) / (xMaxValue - xMinValue)) * (grid.context.width - barWidth);
                    //if (xAxis.boundaryGap) pX += barWidth / 2;
                }
                else
                {
                    if (!xAxis.boundaryGap) pX -= categoryWidth / 2;
                }
                float zeroY = grid.context.y + yAxis.context.offset;
                float axisLineWidth = value == 0 ? 0 :
                     ((value < 0 ? -1 : 1) * xAxis.axisLine.GetWidth(chart.theme.axis.lineWidth));
                float pY = zeroY + axisLineWidth;
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pY += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }

                var barHig = 0f;
                double valueTotal = 0f;
                if (isPercentStack)
                {
                    valueTotal = chart.GetSerieSameStackTotalValue<Bar>(serie.stack, i);
                    barHig = valueTotal != 0 ? (float)(value / valueTotal * grid.context.height) : 0;
                }
                else
                {
                    valueTotal = (double)(yMaxValue - yMinValue);
                    if (valueTotal != 0)
                    {
                        if (yAxis.IsLog())
                        {
                            int minIndex = yAxis.GetLogMinIndex();
                            var nowIndex = yAxis.GetLogValue(value);
                            barHig = (nowIndex - minIndex) / yAxis.splitNumber * grid.context.height;
                        }
                        else
                        {
                            barHig = (float)((yMinValue > 0 ? value - yMinValue : value) / valueTotal * grid.context.height);
                        }
                    }
                }
                serieData.runtimeStackHig = barHig;
                var isBarEnd = false;
                float currHig = chart.CheckSerieBarAnimation(serie, i, barHig, out isBarEnd);
                if (!isBarEnd) isAllBarEnd = false;
                Vector3 plb, plt, prt, prb, top;
                if (value < 0)
                {
                    plb = new Vector3(pX + space + borderWidth, pY - borderWidth);
                    plt = new Vector3(pX + space + borderWidth, pY + currHig + borderWidth);
                    prt = new Vector3(pX + space + barWidth - borderWidth, pY + currHig + borderWidth);
                    prb = new Vector3(pX + space + barWidth - borderWidth, pY - borderWidth);
                }
                else
                {
                    plb = new Vector3(pX + space + borderWidth, pY + borderWidth);
                    plt = new Vector3(pX + space + borderWidth, pY + currHig - borderWidth);
                    prt = new Vector3(pX + space + barWidth - borderWidth, pY + currHig - borderWidth);
                    prb = new Vector3(pX + space + barWidth - borderWidth, pY + borderWidth);
                }
                top = new Vector3(pX + space + barWidth / 2, pY + currHig - borderWidth);
                if (serie.clip)
                {
                    plb = chart.ClampInGrid(grid, plb);
                    plt = chart.ClampInGrid(grid, plt);
                    prt = chart.ClampInGrid(grid, prt);
                    prb = chart.ClampInGrid(grid, prb);
                    top = chart.ClampInGrid(grid, top);
                }
                serieData.runtimeRect = Rect.MinMaxRect(plb.x, plb.y, prb.x, prt.y);
                serie.dataPoints.Add(top);
                if (serie.show && currHig != 0)
                {
                    switch (serie.barType)
                    {
                        case BarType.Normal:
                            DrawNormalBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, false, grid);
                            break;
                        case BarType.Zebra:
                            DrawZebraBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, false, grid);
                            break;
                        case BarType.Capsule:
                            DrawCapsuleBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                               pX, pY, plb, plt, prt, prb, false, grid);
                            break;
                    }
                }
            }
            if (isAllBarEnd)
            {
                serie.animation.AllBarEnd();
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void DrawNormalBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
            bool highlight, float space, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis, GridCoord grid)
        {
            var areaColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, colorIndex, highlight);
            var areaToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, colorIndex, highlight);
            DrawBarBackground(vh, serie, serieData, itemStyle, colorIndex, highlight, pX, pY, space, barWidth, isYAxis, grid);
            var borderWidth = itemStyle.runtimeBorderWidth;
            if (isYAxis)
            {
                if (serie.clip)
                {
                    prb = chart.ClampInGrid(grid, prb);
                    plb = chart.ClampInGrid(grid, plb);
                    plt = chart.ClampInGrid(grid, plt);
                    prt = chart.ClampInGrid(grid, prt);
                }
                var itemWidth = Mathf.Abs(prb.x - plt.x);
                var itemHeight = Mathf.Abs(prt.y - plb.y);
                var center = new Vector3((plt.x + prb.x) / 2, (prt.y + plb.y) / 2);
                if (itemWidth > 0 && itemHeight > 0)
                {
                    var invert = center.x < plb.x;
                    if (ItemStyleHelper.IsNeedCorner(itemStyle))
                    {
                        UGL.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaToColor, 0,
                            itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                    }
                    else
                    {
                        chart.DrawClipPolygon(vh, plb, plt, prt, prb, areaColor, areaToColor, serie.clip, grid);
                    }
                    UGL.DrawBorder(vh, center, itemWidth, itemHeight, borderWidth, itemStyle.borderColor,
                        itemStyle.borderToColor, 0, itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                }
            }
            else
            {
                if (serie.clip)
                {
                    prb = chart.ClampInGrid(grid, prb);
                    plb = chart.ClampInGrid(grid, plb);
                    plt = chart.ClampInGrid(grid, plt);
                    prt = chart.ClampInGrid(grid, prt);
                }
                var itemWidth = Mathf.Abs(prt.x - plb.x);
                var itemHeight = Mathf.Abs(plt.y - prb.y);
                var center = new Vector3((plb.x + prt.x) / 2, (plt.y + prb.y) / 2);
                if (itemWidth > 0 && itemHeight > 0)
                {
                    var invert = center.y < plb.y;
                    if (ItemStyleHelper.IsNeedCorner(itemStyle))
                    {
                        UGL.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaToColor, 0,
                            itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                    }
                    else
                    {
                        chart.DrawClipPolygon(vh, ref prb, ref plb, ref plt, ref prt, areaColor, areaToColor,
                            serie.clip, grid);
                    }
                    UGL.DrawBorder(vh, center, itemWidth, itemHeight, borderWidth, itemStyle.borderColor,
                        itemStyle.borderToColor, 0, itemStyle.cornerRadius, isYAxis, chart.settings.cicleSmoothness, invert);
                }
            }
        }

        private void DrawZebraBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
            bool highlight, float space, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis, GridCoord grid)
        {
            var barColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, colorIndex, highlight);
            var barToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, colorIndex, highlight);
            DrawBarBackground(vh, serie, serieData, itemStyle, colorIndex, highlight, pX, pY, space, barWidth, isYAxis, grid);
            if (isYAxis)
            {
                plt = (plb + plt) / 2;
                prt = (prt + prb) / 2;
                chart.DrawClipZebraLine(vh, plt, prt, barWidth / 2, serie.barZebraWidth, serie.barZebraGap,
                    barColor, barToColor, serie.clip, grid);
            }
            else
            {
                plb = (prb + plb) / 2;
                plt = (plt + prt) / 2;
                chart.DrawClipZebraLine(vh, plb, plt, barWidth / 2, serie.barZebraWidth, serie.barZebraGap,
                    barColor, barToColor, serie.clip, grid);
            }
        }

        private void DrawCapsuleBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
            bool highlight, float space, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis, GridCoord grid)
        {
            var areaColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, colorIndex, highlight);
            var areaToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, colorIndex, highlight);
            DrawBarBackground(vh, serie, serieData, itemStyle, colorIndex, highlight, pX, pY, space, barWidth, isYAxis, grid);
            var borderWidth = itemStyle.runtimeBorderWidth;
            var radius = barWidth / 2 - borderWidth;
            var isGradient = !ChartHelper.IsValueEqualsColor(areaColor, areaToColor);
            if (isYAxis)
            {
                var diff = Vector3.right * radius;
                if (plt.x < prt.x)
                {
                    var pcl = (plt + plb) / 2 + diff;
                    var pcr = (prt + prb) / 2 - diff;
                    if (pcr.x > pcl.x)
                    {
                        if (isGradient)
                        {
                            var barLen = prt.x - plt.x;
                            var rectStartColor = Color32.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color32.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            chart.DrawClipPolygon(vh, plb + diff, plt + diff, prt - diff, prb - diff, rectStartColor,
                                rectEndColor, serie.clip, grid);
                            UGL.DrawSector(vh, pcl, radius, areaColor, rectStartColor, 180, 360, 1, isYAxis);
                            UGL.DrawSector(vh, pcr, radius, rectEndColor, areaToColor, 0, 180, 1, isYAxis);
                        }
                        else
                        {
                            chart.DrawClipPolygon(vh, plb + diff, plt + diff, prt - diff, prb - diff, areaColor,
                                areaToColor, serie.clip, grid);
                            UGL.DrawSector(vh, pcl, radius, areaColor, 180, 360);
                            UGL.DrawSector(vh, pcr, radius, areaToColor, 0, 180);
                        }
                    }
                }
                else if (plt.x > prt.x)
                {
                    var pcl = (plt + plb) / 2 - diff;
                    var pcr = (prt + prb) / 2 + diff;
                    if (pcr.x < pcl.x)
                    {
                        if (isGradient)
                        {
                            var barLen = plt.x - prt.x;
                            var rectStartColor = Color32.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color32.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            chart.DrawClipPolygon(vh, plb - diff, plt - diff, prt + diff, prb + diff, rectStartColor,
                                rectEndColor, serie.clip, grid);
                            UGL.DrawSector(vh, pcl, radius, rectStartColor, areaColor, 0, 180, 1, isYAxis);
                            UGL.DrawSector(vh, pcr, radius, areaToColor, rectEndColor, 180, 360, 1, isYAxis);
                        }
                        else
                        {
                            chart.DrawClipPolygon(vh, plb - diff, plt - diff, prt + diff, prb + diff, areaColor,
                                areaToColor, serie.clip, grid);
                            UGL.DrawSector(vh, pcl, radius, areaColor, 0, 180);
                            UGL.DrawSector(vh, pcr, radius, areaToColor, 180, 360);
                        }
                    }
                }
            }
            else
            {
                var diff = Vector3.up * radius;
                if (plt.y > plb.y)
                {
                    var pct = (plt + prt) / 2 - diff;
                    var pcb = (plb + prb) / 2 + diff;
                    if (pct.y > pcb.y)
                    {
                        if (isGradient)
                        {
                            var barLen = plt.y - plb.y;
                            var rectStartColor = Color32.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color32.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            chart.DrawClipPolygon(vh, prb + diff, plb + diff, plt - diff, prt - diff, rectStartColor,
                                rectEndColor, serie.clip, grid);
                            UGL.DrawSector(vh, pct, radius, rectEndColor, areaToColor, 270, 450, 1, isYAxis);
                            UGL.DrawSector(vh, pcb, radius, rectStartColor, areaColor, 90, 270, 1, isYAxis);
                        }
                        else
                        {
                            chart.DrawClipPolygon(vh, prb + diff, plb + diff, plt - diff, prt - diff, areaColor,
                                areaToColor, serie.clip, grid);
                            UGL.DrawSector(vh, pct, radius, areaToColor, 270, 450);
                            UGL.DrawSector(vh, pcb, radius, areaColor, 90, 270);
                        }
                    }
                }
                else if (plt.y < plb.y)
                {
                    var pct = (plt + prt) / 2 + diff;
                    var pcb = (plb + prb) / 2 - diff;
                    if (pct.y < pcb.y)
                    {
                        if (isGradient)
                        {
                            var barLen = plb.y - plt.y;
                            var rectStartColor = Color32.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color32.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            chart.DrawClipPolygon(vh, prb - diff, plb - diff, plt + diff, prt + diff, rectStartColor,
                                rectEndColor, serie.clip, grid);
                            UGL.DrawSector(vh, pct, radius, rectEndColor, areaToColor, 90, 270, 1, isYAxis);
                            UGL.DrawSector(vh, pcb, radius, rectStartColor, areaColor, 270, 450, 1, isYAxis);
                        }
                        else
                        {
                            chart.DrawClipPolygon(vh, prb - diff, plb - diff, plt + diff, prt + diff, areaColor,
                                areaToColor, serie.clip, grid);
                            UGL.DrawSector(vh, pct, radius, areaToColor, 90, 270);
                            UGL.DrawSector(vh, pcb, radius, areaColor, 270, 450);
                        }
                    }
                }
            }
        }

        private void DrawBarBackground(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle,
            int colorIndex, bool highlight, float pX, float pY, float space, float barWidth, bool isYAxis, GridCoord grid)
        {
            var color = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, colorIndex, highlight, false);
            if (ChartHelper.IsClearColor(color)) return;
            if (isYAxis)
            {
                var axis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
                var axisWidth = axis.axisLine.GetWidth(chart.theme.axis.lineWidth);
                Vector3 plt = new Vector3(grid.context.x + axisWidth, pY + space + barWidth);
                Vector3 prt = new Vector3(grid.context.x + axisWidth + grid.context.width, pY + space + barWidth);
                Vector3 prb = new Vector3(grid.context.x + axisWidth + grid.context.width, pY + space);
                Vector3 plb = new Vector3(grid.context.x + axisWidth, pY + space);
                if (serie.barType == BarType.Capsule)
                {
                    var radius = barWidth / 2;
                    var diff = Vector3.right * radius;
                    var pcl = (plt + plb) / 2 + diff;
                    var pcr = (prt + prb) / 2 - diff;
                    chart.DrawClipPolygon(vh, plb + diff, plt + diff, prt - diff, prb - diff, color, color, serie.clip, grid);
                    UGL.DrawSector(vh, pcl, radius, color, 180, 360);
                    UGL.DrawSector(vh, pcr, radius, color, 0, 180);
                    if (itemStyle.NeedShowBorder())
                    {
                        var borderWidth = itemStyle.borderWidth;
                        var borderColor = itemStyle.borderColor;
                        var smoothness = chart.settings.cicleSmoothness;
                        var inRadius = radius - borderWidth;
                        var outRadius = radius;
                        var p1 = plb + diff + Vector3.up * borderWidth / 2;
                        var p2 = prb - diff + Vector3.up * borderWidth / 2;
                        var p3 = plt + diff - Vector3.up * borderWidth / 2;
                        var p4 = prt - diff - Vector3.up * borderWidth / 2;
                        UGL.DrawLine(vh, p1, p2, borderWidth / 2, borderColor);
                        UGL.DrawLine(vh, p3, p4, borderWidth / 2, borderColor);
                        UGL.DrawDoughnut(vh, pcl, inRadius, outRadius, borderColor, ChartConst.clearColor32,
                            180, 360, smoothness);
                        UGL.DrawDoughnut(vh, pcr, inRadius, outRadius, borderColor, ChartConst.clearColor32,
                            0, 180, smoothness);
                    }
                }
                else
                {
                    chart.DrawClipPolygon(vh, ref plb, ref plt, ref prt, ref prb, color, color, serie.clip, grid);
                }
            }
            else
            {
                var axis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
                var axisWidth = axis.axisLine.GetWidth(chart.theme.axis.lineWidth);
                Vector3 plb = new Vector3(pX + space, grid.context.y + axisWidth);
                Vector3 plt = new Vector3(pX + space, grid.context.y + grid.context.height + axisWidth);
                Vector3 prt = new Vector3(pX + space + barWidth, grid.context.y + grid.context.height + axisWidth);
                Vector3 prb = new Vector3(pX + space + barWidth, grid.context.y + axisWidth);
                if (serie.barType == BarType.Capsule)
                {
                    var radius = barWidth / 2;
                    var diff = Vector3.up * radius;
                    var pct = (plt + prt) / 2 - diff;
                    var pcb = (plb + prb) / 2 + diff;
                    chart.DrawClipPolygon(vh, prb + diff, plb + diff, plt - diff, prt - diff, color, color,
                        serie.clip, grid);
                    UGL.DrawSector(vh, pct, radius, color, 270, 450);
                    UGL.DrawSector(vh, pcb, radius, color, 90, 270);
                    if (itemStyle.NeedShowBorder())
                    {
                        var borderWidth = itemStyle.borderWidth;
                        var borderColor = itemStyle.borderColor;
                        var smoothness = chart.settings.cicleSmoothness;
                        var inRadius = radius - borderWidth;
                        var outRadius = radius;
                        var p1 = plb + diff + Vector3.right * borderWidth / 2;
                        var p2 = plt - diff + Vector3.right * borderWidth / 2;
                        var p3 = prb + diff - Vector3.right * borderWidth / 2;
                        var p4 = prt - diff - Vector3.right * borderWidth / 2;
                        UGL.DrawLine(vh, p1, p2, borderWidth / 2, borderColor);
                        UGL.DrawLine(vh, p3, p4, borderWidth / 2, borderColor);
                        UGL.DrawDoughnut(vh, pct, inRadius, outRadius, borderColor, ChartConst.clearColor32,
                            270, 450, smoothness);
                        UGL.DrawDoughnut(vh, pcb, inRadius, outRadius, borderColor, ChartConst.clearColor32,
                            90, 270, smoothness);
                    }
                }
                else
                {
                    chart.DrawClipPolygon(vh, ref prb, ref plb, ref plt, ref prt, color, color, serie.clip, grid);
                }
            }
        }
    }
}