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
            DrawBarSerie(vh, serie, serie.context.colorIndex);
        }

        private void UpdateSerieContext()
        {
            if (!chart.isPointerInChart) return;
            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;
            foreach (var serieData in serie.data)
            {
                if (serieData.context.rect.Contains(chart.pointerPos))
                {
                    serie.context.pointerItemDataIndex = serieData.index;
                    serie.context.pointerEnter = true;
                    serieData.context.highlight = true;
                    chart.RefreshTopPainter();
                }
                else
                {
                    serieData.context.highlight = false;
                }
            }
        }

        private void DrawBarSerie(VertexHelper vh, Bar serie, int colorIndex)
        {
            if (!serie.show || serie.animation.HasFadeOut())
                return;

            var isY = ComponentHelper.IsAnyCategoryOfYAxis(chart.components);

            Axis axis;
            Axis relativedAxis;
            GridCoord grid;

            if (isY)
            {
                axis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
                relativedAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
            }
            else
            {
                axis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
                relativedAxis = chart.GetChartComponent<YAxis>(serie.yAxisIndex);
            }
            grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);

            if (axis == null)
                return;
            if (relativedAxis == null)
                return;
            if (grid == null)
                return;

            var dataZoom = chart.GetDataZoomOfAxis(axis);
            var showData = serie.GetDataList(dataZoom);

            if (showData.Count <= 0)
                return;

            var axisLength = isY ? grid.context.height : grid.context.width;
            var axisXY = isY ? grid.context.y : grid.context.x;

            var isStack = SeriesHelper.IsStack<Bar>(chart.series, serie.stack);
            if (isStack)
                SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);

            float categoryWidth = AxisHelper.GetDataWidth(axis, axisLength, showData.Count, dataZoom);
            float barGap = chart.GetSerieBarGap<Bar>();
            float totalBarWidth = chart.GetSerieTotalWidth<Bar>(categoryWidth, barGap);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float offset = (categoryWidth - totalBarWidth) * 0.5f;
            float barGapWidth = barWidth + barWidth * barGap;
            float space = serie.barGap == -1 ? offset : offset + chart.GetSerieIndexIfStack<Bar>(serie) * barGapWidth;
            int maxCount = serie.maxShow > 0
                ? (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;

            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series, serie.stack);
            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double yMinValue = relativedAxis.context.minValue;
            double yMaxValue = relativedAxis.context.maxValue;
            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;
            serie.animation.InitProgress(axisXY, axisXY + axisLength);
            for (int i = serie.minShow; i < maxCount; i++)
            {
                var serieData = showData[i];
                if (!serieData.show || serie.IsIgnoreValue(serieData))
                {
                    serie.context.dataPoints.Add(Vector3.zero);
                    continue;
                }

                if (serieData.IsDataChanged())
                    dataChanging = true;

                var highlight = serieData.context.highlight || serie.highlight;
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, highlight);
                var value = axis.IsCategory() ? i : serieData.GetData(0, axis.inverse);
                var relativedValue = serieData.GetCurrData(1, dataChangeDuration, relativedAxis.inverse, yMinValue, yMaxValue);
                var borderWidth = relativedValue == 0 ? 0 : itemStyle.runtimeBorderWidth;

                var pX = 0f;
                var pY = 0f;
                UpdateXYPosition(grid, isY, axis, relativedAxis, i, categoryWidth, barWidth, isStack, value, ref pX, ref pY);

                var barHig = 0f;
                if (isPercentStack)
                {
                    var valueTotal = chart.GetSerieSameStackTotalValue<Bar>(serie.stack, i);
                    barHig = valueTotal != 0 ? (float)(relativedValue / valueTotal * axisLength) : 0;
                }
                else
                {
                    barHig = AxisHelper.GetAxisValueLength(grid, relativedAxis, categoryWidth, relativedValue);
                }

                float currHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, barHig);

                Vector3 plb, plt, prt, prb, top;
                UpdateRectPosition(grid, isY, relativedValue, pX, pY, space, borderWidth, barWidth, currHig,
                    out plb, out plt, out prt, out prb, out top);
                serieData.context.stackHeight = barHig;
                serieData.context.position = top;
                serieData.context.rect = Rect.MinMaxRect(plb.x, plb.y, prb.x, prt.y);
                serie.context.dataPoints.Add(top);
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

                if (serie.animation.CheckDetailBreak(top, isY))
                {
                    break;
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void UpdateXYPosition(GridCoord grid, bool isY, Axis axis, Axis relativedAxis, int i, float categoryWidth, float barWidth, bool isStack,
            double value, ref float pX, ref float pY)
        {
            if (isY)
            {
                if (axis.IsCategory())
                {
                    pY = grid.context.y + i * categoryWidth + (axis.boundaryGap ? 0 : -categoryWidth * 0.5f);
                }
                else
                {
                    if (axis.context.minMaxRange <= 0) pY = grid.context.y;
                    else pY = grid.context.y + (float)((value - axis.context.minValue) / axis.context.minMaxRange) * (grid.context.height - barWidth);
                }
                pX = AxisHelper.GetAxisPosition(grid, relativedAxis, categoryWidth, 0);
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        pX += m_StackSerieData[n][i].context.stackHeight;
                }
            }
            else
            {
                if (axis.IsCategory())
                {
                    pX = grid.context.x + i * categoryWidth + (axis.boundaryGap ? 0 : -categoryWidth * 0.5f);
                }
                else
                {
                    if (axis.context.minMaxRange <= 0) pX = grid.context.x;
                    else pX = grid.context.x + (float)((value - axis.context.minValue) / axis.context.minMaxRange) * (grid.context.width - barWidth);
                }
                pY = AxisHelper.GetAxisPosition(grid, relativedAxis, categoryWidth, 0);
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                        pY += m_StackSerieData[n][i].context.stackHeight;
                }
            }
        }

        private void UpdateRectPosition(GridCoord grid, bool isY, double yValue, float pX, float pY, float space, float borderWidth,
            float barWidth, float currHig,
            out Vector3 plb, out Vector3 plt, out Vector3 prt, out Vector3 prb, out Vector3 top)
        {
            if (isY)
            {
                if (yValue < 0)
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
            }
            else
            {
                if (yValue < 0)
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
            }
            if (serie.clip)
            {
                plb = chart.ClampInGrid(grid, plb);
                plt = chart.ClampInGrid(grid, plt);
                prt = chart.ClampInGrid(grid, prt);
                prb = chart.ClampInGrid(grid, prb);
                top = chart.ClampInGrid(grid, top);
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