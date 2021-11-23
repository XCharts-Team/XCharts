/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    /// <summary>
    /// For grid coord
    /// </summary>
    [UnityEngine.Scripting.Preserve]
    internal sealed partial class LineHandler : SerieHandler<Line>
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
            if (!serie.context.pointerEnter || dataIndex < 0)
                return false;
            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return false;

            double xValue, yValue;
            serie.GetXYData(dataIndex, null, out xValue, out yValue);

            var isIngore = serie.IsIgnorePoint(dataIndex);
            var numericFormatter = TooltipHelper.GetItemNumericFormatter(tooltip, serie, serieData);
            var valueTxt = isIngore
                ? tooltip.ignoreDataDefaultContent
                : ChartCached.FloatToStr(yValue, numericFormatter);

            switch (tooltip.trigger)
            {
                case Tooltip.Trigger.Item:

                    var category = string.Empty;
                    var xAxis = chart.GetChartComponent<XAxis>();
                    var yAxis = chart.GetChartComponent<YAxis>();
                    if (yAxis.IsCategory())
                    {
                        category = yAxis.GetData((int)yAxis.context.pointerValue);
                    }
                    else
                    {
                        category = xAxis.IsCategory()
                            ? xAxis.GetData((int)xAxis.context.pointerValue)
                            : (isIngore
                                ? tooltip.ignoreDataDefaultContent
                                : ChartCached.FloatToStr(xValue, numericFormatter));
                    }
                    if (!string.IsNullOrEmpty(serie.serieName))
                        sb.Append(serie.serieName).Append(FormatterHelper.PH_NN);

                    sb.Append("<color=#")
                        .Append(chart.theme.GetColorStr(serie.index))
                        .Append(">● </color>");

                    if (!string.IsNullOrEmpty(category))
                        sb.Append(category).Append(":");
                    sb.Append(valueTxt);
                    break;

                case Tooltip.Trigger.Axis:

                    sb.Append("<color=#")
                        .Append(chart.theme.GetColorStr(serie.index))
                        .Append(">● </color>");

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

            if (serie.IsUseCoord<PolarCoord>())
            {
                DrawPolarLine(vh, serie);
                DrawPolarLineSymbol(vh);
            }
            else if (serie.IsUseCoord<GridCoord>())
            {
                if (yCategory)
                    DrawYLineSerie(vh, serie, colorIndex);
                else
                    DrawXLineSerie(vh, serie, colorIndex);

                if (!SeriesHelper.IsStack(chart.series))
                {
                    DrawLinePoint(vh, serie);
                    DrawLineArrow(vh, serie);
                }
            }
        }

        public override void DrawTop(VertexHelper vh)
        {
            if (serie.IsUseCoord<GridCoord>())
            {
                if (SeriesHelper.IsStack(chart.series))
                {
                    DrawLinePoint(vh, serie);
                    DrawLineArrow(vh, serie);
                }
            }
        }

        private void UpdateSerieContext()
        {
            if (!chart.isPointerInChart)
                return;

            var themeSymbolSize = chart.theme.serie.lineSymbolSize;

            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;

            foreach (var serieData in serie.data)
            {
                var dist = Vector3.Distance(chart.pointerPos, serieData.runtimePosition);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);

                if (dist <= symbolSize)
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

        private void DrawLinePoint(VertexHelper vh, Serie serie)
        {
            if (!serie.show || serie.IsPerformanceMode())
                return;

            var count = serie.dataPoints.Count;
            var clip = SeriesHelper.IsAnyClipSerie(chart.series);
            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;

            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex))
                return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex))
                return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex))
                return;

            var theme = chart.theme;
            for (int i = 0; i < count; i++)
            {
                var serieData = serie.GetSerieData(i);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);

                if (!symbol.show || !symbol.ShowSymbol(i, count))
                    continue;

                if (serie.lineArrow.show)
                {
                    if (serie.lineArrow.position == LineArrow.Position.Start && i == 0)
                        continue;
                    if (serie.lineArrow.position == LineArrow.Position.End && i == count - 1)
                        continue;
                }

                if (ChartHelper.IsIngore(serie.dataPoints[i]))
                    continue;

                var highlight = serie.data[i].highlighted || serie.highlighted;
                var symbolSize = highlight
                    ? symbol.GetSelectedSize(serie.data[i].data, theme.serie.lineSymbolSelectedSize)
                    : symbol.GetSize(serie.data[i].data, theme.serie.lineSymbolSize);
                var symbolColor = SerieHelper.GetItemColor(serie, serieData, theme, serie.index, highlight);
                var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, theme, serie.index, highlight);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);

                symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                chart.DrawClipSymbol(vh, symbol.type, symbolSize, symbolBorder, serie.dataPoints[i],
                    symbolColor, symbolToColor, symbol.gap, clip, cornerRadius, grid,
                    i > 0 ? serie.dataPoints[i - 1] : grid.context.position);
            }
        }

        private void DrawLineArrow(VertexHelper vh, Serie serie)
        {
            if (!serie.show || !serie.lineArrow.show)
                return;

            if (serie.dataPoints.Count < 2)
                return;

            var lineColor = SerieHelper.GetLineColor(serie, chart.theme, serie.index, false);
            var startPos = Vector3.zero;
            var arrowPos = Vector3.zero;
            var lineArrow = serie.lineArrow.arrow;

            switch (serie.lineArrow.position)
            {
                case LineArrow.Position.End:

                    var dataPoints = serie.GetUpSmoothList(serie.dataCount - 1);
                    if (dataPoints.Count < 3)
                    {
                        dataPoints = serie.dataPoints;
                        startPos = dataPoints[dataPoints.Count - 2];
                        arrowPos = dataPoints[dataPoints.Count - 1];
                    }
                    else
                    {
                        startPos = dataPoints[dataPoints.Count - 3];
                        arrowPos = dataPoints[dataPoints.Count - 2];
                    }
                    UGL.DrawArrow(vh, startPos, arrowPos, lineArrow.width, lineArrow.height,
                        lineArrow.offset, lineArrow.dent, lineArrow.GetColor(lineColor));

                    break;

                case LineArrow.Position.Start:

                    dataPoints = serie.GetUpSmoothList(1);
                    if (dataPoints.Count < 2) dataPoints = serie.dataPoints;
                    startPos = dataPoints[1];
                    arrowPos = dataPoints[0];
                    UGL.DrawArrow(vh, startPos, arrowPos, lineArrow.width, lineArrow.height,
                        lineArrow.offset, lineArrow.dent, lineArrow.GetColor(lineColor));

                    break;
            }
        }

        private void DrawXLineSerie(VertexHelper vh, Line serie, int colorIndex)
        {

            if (!serie.show)
                return;
            if (serie.animation.HasFadeOut())
                return;

            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;

            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex))
                return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex))
                return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex))
                return;

            var visualMap = chart.GetVisualMapOfSerie(serie);
            var tooltip = chart.GetChartComponent<Tooltip>();
            var dataZoom = chart.GetDataZoomOfAxis(xAxis);
            var showData = serie.GetDataList(dataZoom);

            if (showData.Count <= 0)
                return;

            var theme = chart.theme;
            var highlight = serie.highlighted || serie.context.pointerEnter;

            var lineColor = SerieHelper.GetLineColor(serie, theme, colorIndex, highlight);
            var srcAreaColor = SerieHelper.GetAreaColor(serie, theme, colorIndex, false);
            var srcAreaToColor = SerieHelper.GetAreaToColor(serie, theme, colorIndex, false);
            var highlightAreaColor = SerieHelper.GetAreaColor(serie, theme, colorIndex, true);
            var highlightAreaToColor = SerieHelper.GetAreaToColor(serie, theme, colorIndex, true);
            var zeroPos = new Vector3(grid.context.x, grid.context.y + yAxis.context.yOffset);
            var isStack = SeriesHelper.IsStack<Line>(chart.series, serie.stack);
            var scaleWid = AxisHelper.GetDataWidth(xAxis, grid.context.width, showData.Count, dataZoom);
            var startX = grid.context.x + (xAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = serie.maxShow > 0
                ? (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            int i;
            var includeLastData = false;
            int rate = LineHelper.GetDataAverageRate(serie, grid, maxCount, false);
            var totalAverage = serie.sampleAverage > 0
                ? serie.sampleAverage
                : DataHelper.DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();

            Color32 areaColor, areaToColor;
            Vector3 lp = Vector3.zero, np = Vector3.zero, llp = Vector3.zero, nnp = Vector3.zero;

            xAxis.context.scaleWidth = scaleWid;
            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;

            if (isStack)
                SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);

            for (i = serie.minShow; i < maxCount; i += rate)
            {
                if (i == maxCount - 1)
                    includeLastData = true;

                if (serie.IsIgnoreValue(showData[i]))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    showData[i].runtimeStackHig = 0;
                }
                else
                {
                    double yValue = DataHelper.SampleValue(ref showData, serie.sampleType, rate, serie.minShow,
                        maxCount, totalAverage, i, dataChangeDuration, ref dataChanging, yAxis);
                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, grid, showData, yValue, startX, i,
                        scaleWid, isStack, ref np, dataChangeDuration);

                    showData[i].runtimePosition = np;
                    serie.dataPoints.Add(np);
                }
            }

            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }

            if (!includeLastData)
            {
                i = maxCount - 1;
                if (serie.IsIgnoreValue(showData[i]))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    showData[i].runtimeStackHig = 0;
                }
                else
                {
                    double yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse,
                        yAxis.context.minValue, yAxis.context.maxValue);

                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, grid, showData, yValue, startX, i,
                        scaleWid, isStack, ref np, dataChangeDuration);

                    showData[i].runtimePosition = np;
                    serie.dataPoints.Add(np);
                }
            }

            if (serie.dataPoints.Count <= 0)
            {
                return;
            }

            var startIndex = 0;
            var endIndex = serie.dataPoints.Count;
            var startPos = LineHelper.GetStartPos(serie.dataPoints, ref startIndex, serie.ignoreLineBreak);
            var endPos = LineHelper.GetEndPos(serie.dataPoints, ref endIndex, serie.ignoreLineBreak);
            var firstLastPos = Vector3.zero;
            var lastNextPos = Vector3.zero;

            lp = startPos;
            stPos1 = stPos2 = lastDir = lastDnPos = Vector3.zero;
            smoothStartPosUp = smoothStartPosDn = Vector3.zero;

            if (serie.minShow > 0 && serie.minShow < showData.Count)
            {
                i = serie.minShow - 1;

                if (serie.IsIgnoreValue(showData[i]))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    showData[i].runtimeStackHig = 0;
                }
                else
                {
                    double yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse,
                        yAxis.context.minValue, yAxis.context.maxValue);
                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, grid, showData, yValue, startX, i,
                        scaleWid, isStack, ref firstLastPos, dataChangeDuration);
                }
            }
            else
            {
                firstLastPos = lp;
            }
            if (serie.maxShow > 0 && serie.maxShow < showData.Count)
            {
                i = serie.maxShow;

                if (serie.IsIgnoreValue(showData[i]))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    showData[i].runtimeStackHig = 0;
                }
                else
                {
                    double yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse,
                        yAxis.context.minValue, yAxis.context.maxValue);
                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, grid, showData, yValue, startX, i,
                        scaleWid, isStack, ref lastNextPos, dataChangeDuration);
                }
            }
            else
            {
                lastNextPos = endPos;
            }
            VisualMapHelper.AutoSetLineMinMax(visualMap, serie, xAxis, yAxis);

            float currDetailProgress = lp.x;
            float totalDetailProgress = endPos.x;
            if (serie.animation.alongWithLinePath)
            {
                currDetailProgress = 0;
                totalDetailProgress = 0;
                var tempLp = startPos;
                for (i = startIndex + 1; i < serie.dataPoints.Count; i++)
                {
                    np = serie.dataPoints[i];
                    if (np != Vector3.zero)
                    {
                        totalDetailProgress += Vector3.Distance(np, tempLp);
                        tempLp = np;
                    }
                }
                serie.animation.SetLinePathStartPos(startPos);
            }

            serie.animation.InitProgress(serie.dataPoints.Count, currDetailProgress, totalDetailProgress);
            serie.animation.SetDataFinish(startIndex);


            for (i = startIndex + 1; i < serie.dataPoints.Count; i++)
            {
                np = serie.dataPoints[i];
                serie.ClearSmoothList(i);
                var isIgnoreBreak = false;
                if (np == Vector3.zero)
                {
                    if (serie.ignoreLineBreak)
                        isIgnoreBreak = true;
                    else
                    {
                        serie.animation.SetDataFinish(i);
                        continue;
                    }
                }
                if (!serie.animation.NeedAnimation(i))
                    break;

                bool isFinish = true;
                if (serie.areaStyle.tooltipHighlight && tooltip.show && i <= tooltip.runtimeDataIndex[0])
                {
                    areaColor = highlightAreaColor;
                    areaToColor = highlightAreaToColor;
                }
                else
                {
                    areaColor = srcAreaColor;
                    areaToColor = srcAreaToColor;
                }
                switch (serie.lineType)
                {
                    case LineType.Normal:

                        lp = LineHelper.GetLastPos(serie.dataPoints, i, np, serie.ignoreLineBreak);
                        nnp = LineHelper.GetNNPos(serie.dataPoints, i, np, serie.ignoreLineBreak);
                        if (lp == Vector3.zero && serie.ignoreLineBreak) isIgnoreBreak = true;
                        isFinish = DrawNormalLine(vh, serie, xAxis, visualMap, lp, np, nnp, i,
                            isIgnoreBreak ? ColorUtil.clearColor32 : lineColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaToColor,
                            zeroPos, startIndex);

                        break;

                    case LineType.Smooth:
                    case LineType.SmoothDash:

                        llp = LineHelper.GetLLPos(serie.dataPoints, i, firstLastPos, serie.ignoreLineBreak);
                        nnp = LineHelper.GetNNPos(serie.dataPoints, i, lastNextPos, serie.ignoreLineBreak);
                        if (lp == Vector3.zero && serie.ignoreLineBreak) isIgnoreBreak = true;
                        isFinish = DrawSmoothLine(vh, serie, xAxis, grid, visualMap, lp, np, llp, nnp, i,
                            isIgnoreBreak ? ColorUtil.clearColor32 : lineColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaToColor,
                            isStack, zeroPos, startIndex);
                        break;

                    case LineType.StepStart:
                    case LineType.StepMiddle:
                    case LineType.StepEnd:

                        nnp = LineHelper.GetNNPos(serie.dataPoints, i, np, serie.ignoreLineBreak);
                        if (lp == Vector3.zero && serie.ignoreLineBreak) isIgnoreBreak = true;
                        isFinish = DrawStepLine(vh, serie, xAxis, lp, np, nnp, i,
                            isIgnoreBreak ? ColorUtil.clearColor32 : lineColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaToColor,
                            zeroPos);
                        break;

                    case LineType.Dash:
                    case LineType.Dot:
                    case LineType.DashDot:
                    case LineType.DashDotDot:

                        if (lp == Vector3.zero && serie.ignoreLineBreak) isIgnoreBreak = true;
                        DrawOtherLine(vh, serie, xAxis, lp, np, i,
                            isIgnoreBreak ? ColorUtil.clearColor32 : lineColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaToColor,
                            zeroPos);
                        break;
                }
                if (isFinish)
                    serie.animation.SetDataFinish(i);

                if (np != Vector3.zero || serie.ignoreLineBreak)
                    lp = np;
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress - currDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, theme.serie.lineSymbolSize));
                chart.m_IsPlayingAnimation = true;
                chart.RefreshPainter(serie);
            }
        }

        private float GetDataPoint(Axis xAxis, Axis yAxis, GridCoord grid, List<SerieData> showData,
            double yValue, float startX, int i, float scaleWid, bool isStack, ref Vector3 np, float duration,
            bool isIngoreValue = false)
        {
            if (isIngoreValue)
            {
                np = Vector3.zero;
                return 0;
            }
            float xDataHig, yDataHig;
            double xMinValue = xAxis.GetCurrMinValue(duration);
            double xMaxValue = xAxis.GetCurrMaxValue(duration);
            double yMinValue = yAxis.GetCurrMinValue(duration);
            double yMaxValue = yAxis.GetCurrMaxValue(duration);
            if (xAxis.IsValue() || xAxis.IsLog() || xAxis.IsTime())
            {
                var axisLineWidth = xAxis.axisLine.GetWidth(chart.theme.axis.lineWidth);
                double xValue = i > showData.Count - 1
                    ? 0
                    : showData[i].GetData(0, xAxis.inverse);
                float pX = grid.context.x;
                float pY = grid.context.y + axisLineWidth;

                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pY += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }

                if (xAxis.IsLog())
                {
                    int minIndex = xAxis.GetLogMinIndex();
                    float nowIndex = xAxis.GetLogValue(xValue);
                    xDataHig = (nowIndex - minIndex) / xAxis.splitNumber * grid.context.width;
                }
                else
                {
                    if ((xMaxValue - xMinValue) <= 0) xDataHig = 0;
                    else xDataHig = (float)((xValue - xMinValue) / (xMaxValue - xMinValue)) * (grid.context.width);
                }

                if (yAxis.IsLog())
                {
                    int minIndex = yAxis.GetLogMinIndex();
                    float nowIndex = yAxis.GetLogValue(yValue);
                    yDataHig = (nowIndex - minIndex) / yAxis.splitNumber * grid.context.height;
                }
                else
                {
                    double valueTotal = yMaxValue - yMinValue;
                    if (valueTotal <= 0) yDataHig = 0;
                    else yDataHig = (float)((yValue - yMinValue) / valueTotal) * grid.context.height;
                }

                np = new Vector3(pX + xDataHig, pY + yDataHig);
            }
            else
            {
                var axisLineWidth = yAxis.axisLine.GetWidth(chart.theme.axis.lineWidth);
                float pX = startX + i * scaleWid;
                float pY = grid.context.y + axisLineWidth;

                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pY += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }

                if (yAxis.IsLog())
                {
                    int minIndex = yAxis.GetLogMinIndex();
                    float nowIndex = yAxis.GetLogValue(yValue);
                    yDataHig = (nowIndex - minIndex) / yAxis.splitNumber * grid.context.height;
                }
                else
                {
                    double valueTotal = yMaxValue - yMinValue;
                    if (valueTotal <= 0) yDataHig = 0;
                    else yDataHig = (float)((yValue - yMinValue) / valueTotal * grid.context.height);
                }

                np = new Vector3(pX, pY + yDataHig);
            }
            return yDataHig;
        }


        private void DrawYLineSerie(VertexHelper vh, Line serie, int colorIndex)
        {
            if (!serie.show)
                return;
            if (serie.animation.HasFadeOut())
                return;

            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex))
                return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex))
                return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex))
                return;

            var theme = chart.theme;
            var visualMap = chart.GetVisualMapOfSerie(serie);
            var tooltip = chart.GetChartComponent<Tooltip>();
            var dataZoom = chart.GetDataZoomOfAxis(yAxis);
            var showData = serie.GetDataList(dataZoom);

            var zeroPos = new Vector3(grid.context.x + xAxis.context.xOffset, grid.context.y);
            var isStack = SeriesHelper.IsStack<Line>(chart.series, serie.stack);
            var lineColor = SerieHelper.GetLineColor(serie, theme, colorIndex, serie.highlighted);
            var lineWidth = serie.lineStyle.GetWidth(theme.serie.lineWidth);
            var srcAreaColor = SerieHelper.GetAreaColor(serie, theme, colorIndex, false);
            var srcAreaToColor = SerieHelper.GetAreaToColor(serie, theme, colorIndex, false);
            var highlightAreaColor = SerieHelper.GetAreaColor(serie, theme, colorIndex, true);
            var highlightAreaToColor = SerieHelper.GetAreaToColor(serie, theme, colorIndex, true);

            Color32 areaColor, areaToColor;
            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            Vector3 llp = Vector3.zero;
            Vector3 nnp = Vector3.zero;

            float scaleWid = AxisHelper.GetDataWidth(yAxis, grid.context.height, showData.Count, dataZoom);
            float startY = grid.context.y + (yAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = serie.maxShow > 0
                ? (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            int i = 0;
            var rate = LineHelper.GetDataAverageRate(serie, grid, maxCount, true);
            var dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double xMinValue = xAxis.GetCurrMinValue(dataChangeDuration);
            double xMaxValue = xAxis.GetCurrMaxValue(dataChangeDuration);

            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;

            if (isStack)
                SeriesHelper.UpdateStackDataList(chart.series, serie, dataZoom, m_StackSerieData);

            for (i = serie.minShow; i < maxCount; i += rate)
            {
                double value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse,
                    xAxis.context.minValue, xAxis.context.maxValue);

                float pY = startY + i * scaleWid;
                float pX = grid.context.x + yAxis.axisLine.GetWidth(theme.axis.lineWidth);

                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pX += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }

                float dataHig = 0;
                if (xAxis.IsLog())
                {
                    int minIndex = xAxis.GetLogMinIndex();
                    float nowIndex = xAxis.GetLogValue(value);
                    dataHig = (nowIndex - minIndex) / (xAxis.splitNumber - 1) * grid.context.width;
                }
                else
                {
                    dataHig = (float)((value - xMinValue) / (xMaxValue - xMinValue) * grid.context.width);
                }
                showData[i].runtimeStackHig = dataHig;
                np = new Vector3(pX + dataHig, pY);
                showData[i].runtimePosition = np;
                serie.dataPoints.Add(np);
                if (showData[i].IsDataChanged())
                    dataChanging = true;
            }

            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }

            if (maxCount % rate != 0)
            {
                i = maxCount - 1;
                double value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse,
                    xAxis.context.minValue, xAxis.context.maxValue);
                float pY = startY + i * scaleWid;
                float pX = grid.context.x + yAxis.axisLine.GetWidth(theme.axis.lineWidth);

                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pX += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }

                float dataHig = 0;
                if (xAxis.IsLog())
                {
                    int minIndex = xAxis.GetLogMinIndex();
                    float nowIndex = xAxis.GetLogValue(value);
                    dataHig = (nowIndex - minIndex) / xAxis.splitNumber * grid.context.width;
                }
                else
                {
                    dataHig = (float)((value - xMinValue) / (xMaxValue - xMinValue)) * grid.context.width;
                }
                showData[i].runtimeStackHig = dataHig;
                np = new Vector3(pX + dataHig, pY);
                showData[i].runtimePosition = np;
                serie.dataPoints.Add(np);
            }

            lp = serie.dataPoints[0];

            int dataCount = serie.dataPoints.Count;
            float currDetailProgress = lp.y;
            float totalDetailProgress = serie.dataPoints[dataCount - 1].y;

            serie.animation.InitProgress(dataCount, currDetailProgress, totalDetailProgress);
            for (i = 1; i < serie.dataPoints.Count; i++)
            {
                np = serie.dataPoints[i];
                serie.ClearSmoothList(i);
                if (!serie.animation.NeedAnimation(i))
                    break;
                bool isFinish = true;

                if (serie.areaStyle.tooltipHighlight && tooltip.show && i < tooltip.runtimeDataIndex[0])
                {
                    areaColor = highlightAreaColor;
                    areaToColor = highlightAreaToColor;
                }
                else
                {
                    areaColor = srcAreaColor;
                    areaToColor = srcAreaToColor;
                }

                switch (serie.lineType)
                {
                    case LineType.Normal:

                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawNormalLine(vh, serie, yAxis, visualMap, lp, np, nnp, i, lineColor,
                            areaColor, areaToColor, zeroPos, 0);
                        break;

                    case LineType.Smooth:
                    case LineType.SmoothDash:

                        llp = i > 1 ? serie.dataPoints[i - 2] : lp;
                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawSmoothLine(vh, serie, yAxis, grid, visualMap, lp, np, llp, nnp, i,
                            lineColor, areaColor, areaToColor, isStack, zeroPos);
                        break;

                    case LineType.StepStart:
                    case LineType.StepMiddle:
                    case LineType.StepEnd:

                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawStepLine(vh, serie, yAxis, lp, np, nnp, i, lineColor,
                            areaColor, areaToColor, zeroPos);
                        break;

                    case LineType.Dash:

                        UGL.DrawDashLine(vh, lp, np, lineWidth, lineColor, lineColor);
                        isFinish = true;
                        break;

                    case LineType.Dot:

                        UGL.DrawDotLine(vh, lp, np, lineWidth, lineColor, lineColor);
                        isFinish = true;
                        break;

                    case LineType.DashDot:

                        UGL.DrawDashDotLine(vh, lp, np, lineWidth, lineColor);
                        isFinish = true;
                        break;

                    case LineType.DashDotDot:

                        UGL.DrawDashDotDotLine(vh, lp, np, lineWidth, lineColor);
                        isFinish = true;
                        break;
                }
                if (isFinish)
                    serie.animation.SetDataFinish(i);

                lp = np;
            }
            if (!serie.animation.IsFinish())
            {
                var totalLineWidth = dataCount * lineWidth * 0.5f;
                var total = totalDetailProgress - currDetailProgress - totalLineWidth;

                serie.animation.CheckProgress(total);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, theme.serie.lineSymbolSize));
                chart.m_IsPlayingAnimation = true;
                chart.RefreshPainter(serie);
            }
        }

        private Vector3 stPos1, stPos2, lastDir, lastDnPos;
        private bool lastIsDown;
        internal bool DrawNormalLine(VertexHelper vh, Serie serie, Axis axis, VisualMap visualMap, Vector3 lp,
            Vector3 np, Vector3 nnp, int dataIndex, Color32 lineColor, Color32 areaColor, Color32 areaToColor,
            Vector3 zeroPos, int startIndex)
        {
            var defaultLineColor = lineColor;
            var isSecond = dataIndex == startIndex + 1;
            var isTheLastPos = np == nnp;
            var isYAxis = axis is YAxis;
            var isTurnBack = LineHelper.IsInRightOrUp(isYAxis, np, lp);
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            var theme = chart.theme;

            Vector3 dnPos, upPos1, upPos2, dir1v, dir2v;
            bool isDown;
            var dir1 = (np - lp).normalized;

            dir1v = Vector3.Cross(dir1, Vector3.forward).normalized * (isYAxis ? -1 : 1);

            if (np != nnp)
            {
                var dir2 = (nnp - np).normalized;
                var dir3 = (dir1 + dir2).normalized;
                var normal = Vector3.Cross(dir1, dir2);
                var angle = (180 - Vector3.Angle(dir1, dir2)) * Mathf.Deg2Rad / 2;
                var diff = lineWidth / Mathf.Sin(angle);
                var dirDp = Vector3.Cross(dir3, Vector3.forward).normalized * (isYAxis ? -1 : 1);

                isDown = isYAxis ? normal.z >= 0 : normal.z <= 0;
                dir2v = Vector3.Cross(dir2, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                dnPos = np + (isDown ? dirDp : -dirDp) * diff;
                upPos1 = np + (isDown ? -dir1v : dir1v) * lineWidth;
                upPos2 = np + (isDown ? -dir2v : dir2v) * lineWidth;
                lastDir = dir1;

                if (isDown)
                {
                    if (isYAxis && dnPos.x < lp.x && dnPos.x < nnp.x)
                        dnPos.x = lp.x;
                    if (!isYAxis && dnPos.y < lp.y && dnPos.y < nnp.y)
                        dnPos.y = lp.y;
                }
                else
                {
                    if (isYAxis && dnPos.x > lp.x && dnPos.x > nnp.x)
                        dnPos.x = lp.x;
                    if (!isYAxis && dnPos.y > lp.y && dnPos.y > nnp.y)
                        dnPos.y = lp.y;
                }
            }
            else
            {
                isDown = Vector3.Cross(dir1, lastDir).z <= 0;
                if (isYAxis)
                    isDown = !isDown;
                dir1v = Vector3.Cross(dir1, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                upPos1 = np - dir1v * lineWidth;
                upPos2 = np + dir1v * lineWidth;
                dnPos = isDown ? upPos2 : upPos1;
            }

            if (isSecond)
            {
                stPos1 = lp - dir1v * lineWidth;
                stPos2 = lp + dir1v * lineWidth;
            }

            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            var dist = Vector3.Distance(lp, np);
            var lastSmoothPoint = Vector3.zero;
            var lastSmoothDownPoint = Vector3.zero;

            int segment = (int)(dist / chart.settings.lineSegmentDistance);
            if (segment <= 3)
                segment = (int)(dist / lineWidth);
            if (segment < 2)
                segment = 2;

            if (dataIndex > startIndex)
            {
                lastSmoothPoint = ChartHelper.GetLastPoint(serie.GetUpSmoothList(dataIndex - 1));
                lastSmoothDownPoint = ChartHelper.GetLastPoint(serie.GetDownSmoothList(dataIndex - 1));
            }

            smoothPoints.Clear();
            smoothDownPoints.Clear();
            if (!TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, stPos1, false))
            {
                smoothPoints.Add(lastSmoothPoint);
            }
            if (!TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, stPos2, false))
            {
                smoothDownPoints.Add(lastSmoothDownPoint);
            }

            var start = lp;
            var ltp1 = stPos1;
            var ltp2 = stPos2;
            bool isBreak = false;
            bool isStart = false;
            bool isShort = false;

            for (int i = 1; i < segment; i++)
            {
                var isEndPos = i == segment - 1;
                var cp = lp + dir1 * (dist * i / segment);
                if (serie.animation.CheckDetailBreak(cp, isYAxis))
                    isBreak = true;

                var tp1 = cp - dir1v * lineWidth;
                var tp2 = cp + dir1v * lineWidth;

                if (ChartHelper.IsClearColor(serie.lineStyle.color))
                    CheckLineGradientColor(visualMap, cp, serie.lineStyle, axis, defaultLineColor, ref lineColor);

                if (isDown)
                {
                    if (!isBreak)
                    {
                        if (!isStart)
                        {
                            if (isEndPos)
                            {
                                isShort = true;
                                isStart = true;
                                chart.DrawClipPolygon(vh, stPos1, upPos1, upPos2, stPos2, lineColor, serie.clip, grid);
                                chart.DrawClipTriangle(vh, stPos2, upPos2, dnPos, lineColor, serie.clip, grid);
                                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, stPos1, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, upPos1, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, dnPos, isEndPos);
                            }
                            else if (isSecond || isTurnBack ||
                                (lastIsDown && LineHelper.IsInRightOrUp(isYAxis, lastDnPos, tp2)) ||
                                (!lastIsDown && LineHelper.IsInRightOrUp(isYAxis, lastDnPos, tp1)))
                            {
                                isStart = true;
                                chart.DrawClipPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor, serie.clip, grid);
                            }
                        }
                        else
                        {
                            if (isEndPos)
                            {
                                if (np != nnp)
                                {
                                    chart.DrawClipPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor, serie.clip, grid);
                                    chart.DrawClipTriangle(vh, upPos1, upPos2, dnPos, lineColor, serie.clip, grid);
                                }
                                else
                                {
                                    chart.DrawClipPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor, serie.clip, grid);
                                }
                            }
                            else
                            {
                                if (LineHelper.IsInRightOrUp(isYAxis, tp2, dnPos) || isTurnBack)
                                {
                                    chart.DrawClipLine(vh, start, cp, lineWidth,
                                        lineColor, serie.clip, grid);
                                }
                                else
                                {
                                    chart.DrawClipPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor, serie.clip, grid);
                                    chart.DrawClipTriangle(vh, upPos1, upPos2, dnPos, lineColor, serie.clip, grid);
                                    i = segment;
                                }
                            }

                        }
                    }
                    if (!isShort)
                    {
                        TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, tp1, isEndPos);
                        TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, tp2, isEndPos);
                    }
                }
                else
                {
                    if (!isBreak)
                    {
                        if (!isStart)
                        {
                            if (isEndPos)
                            {
                                isStart = true;
                                isShort = true;
                                if (np == nnp)
                                {
                                    chart.DrawClipPolygon(vh, stPos1, dnPos, upPos2, stPos2, lineColor, serie.clip, grid);
                                }
                                else
                                {
                                    chart.DrawClipPolygon(vh, stPos1, dnPos, upPos1, stPos2, lineColor, serie.clip, grid);
                                    chart.DrawClipTriangle(vh, dnPos, upPos1, upPos2, lineColor, serie.clip, grid);
                                }
                                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, dnPos, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, stPos2, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, upPos2, isEndPos);
                            }
                            else if (isSecond || isTurnBack ||
                                (lastIsDown && LineHelper.IsInRightOrUp(isYAxis, lastDnPos, tp2)) ||
                                (!lastIsDown && LineHelper.IsInRightOrUp(isYAxis, lastDnPos, tp1)))
                            {
                                isStart = true;
                                if (stPos2 != Vector3.zero)
                                {
                                    chart.DrawClipPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor, serie.clip, grid);
                                }
                            }
                        }
                        else
                        {
                            if (isEndPos)
                            {
                                if (np != nnp)
                                {
                                    chart.DrawClipPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor, serie.clip, grid);
                                    chart.DrawClipTriangle(vh, dnPos, upPos2, upPos1, lineColor, serie.clip, grid);
                                }
                                else
                                    chart.DrawClipPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor, serie.clip, grid);
                            }
                            else
                            {
                                if (LineHelper.IsInRightOrUp(isYAxis, tp1, dnPos) || isTurnBack)
                                {
                                    chart.DrawClipLine(vh, start, cp, lineWidth,
                                        lineColor, serie.clip, grid);
                                }
                                else
                                {
                                    chart.DrawClipPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor, serie.clip, grid);
                                    chart.DrawClipTriangle(vh, dnPos, upPos2, upPos1, lineColor, serie.clip, grid);
                                    i = segment;
                                }
                            }
                        }
                    }
                    if (!isShort)
                    {
                        TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, tp1, isEndPos);
                        TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, tp2, isEndPos);
                    }
                }
                start = cp;
                ltp1 = tp1;
                ltp2 = tp2;
            }

            if (!isBreak && !isShort)
            {
                if (isDown)
                {
                    TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, upPos1, true);
                    TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, upPos2, true);
                    TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, dnPos, true);
                }
                else
                {
                    TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, dnPos, true);
                    if (isYAxis)
                    {
                        TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, isTheLastPos ? upPos1 : upPos2, true);
                        TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, isTheLastPos ? upPos2 : upPos1, true);
                    }
                    else
                    {
                        if (isTheLastPos)
                        {
                            TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, upPos2, true);
                            TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, upPos1, true);
                        }
                        else
                        {
                            TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, upPos1, true);
                            TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, upPos2, true);
                        }
                    }
                }
            }
            if (serie.areaStyle.show)
            {
                var lastSerie = SeriesHelper.GetLastStackSerie(chart.series, serie);
                if (lastSerie != null)
                {
                    var lastSmoothPoints = lastSerie.GetUpSmoothList(dataIndex);

                    DrawStackArea(vh, serie, axis, smoothDownPoints, lastSmoothPoints, areaColor, areaToColor);
                }
                else
                {
                    var points = ((isYAxis && lp.x < zeroPos.x) || (!isYAxis && lp.y < zeroPos.y))
                        ? smoothPoints
                        : smoothDownPoints;

                    Vector3 aep = isYAxis
                        ? new Vector3(zeroPos.x, zeroPos.y + grid.context.height)
                        : new Vector3(zeroPos.x + grid.context.width, zeroPos.y);
                    var sindex = 0;
                    var eindex = 0;
                    var sp = LineHelper.GetStartPos(points, ref sindex, serie.ignoreLineBreak);
                    var ep = LineHelper.GetEndPos(points, ref eindex, serie.ignoreLineBreak);
                    var cross = ChartHelper.GetIntersection(lp, np, zeroPos, aep);

                    if (cross == Vector3.zero || smoothDownPoints.Count <= 3)
                    {
                        sp = points[sindex];
                        for (int i = sindex + 1; i <= eindex; i++)
                        {
                            ep = points[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                break;
                            DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                            sp = ep;
                        }
                    }
                    else
                    {
                        var sp1 = smoothDownPoints[0];
                        var ep1 = smoothDownPoints[smoothDownPoints.Count - 1];
                        var axisLineWidth = axis.axisLine.GetWidth(theme.axis.lineWidth);
                        var axisUpStart = zeroPos + (isYAxis ? Vector3.right : Vector3.up) * axisLineWidth;
                        var axisUpEnd = axisUpStart + (isYAxis ? Vector3.up * grid.context.height : Vector3.right * grid.context.width);
                        var axisDownStart = zeroPos - (isYAxis ? Vector3.right : Vector3.up) * axisLineWidth;
                        var axisDownEnd = axisDownStart + (isYAxis ? Vector3.up * grid.context.height : Vector3.right * grid.context.width);
                        var luPos = ChartHelper.GetIntersection(sp1, ep1, axisUpStart, axisUpEnd);
                        var ecount = smoothPoints.Count - 2;
                        if (ecount < 0) ecount = 0;

                        sp1 = smoothPoints[0];
                        ep1 = smoothPoints[ecount];
                        var rdPos = ChartHelper.GetIntersection(sp1, ep1, axisDownStart, axisDownEnd);

                        if ((isYAxis && lp.x >= zeroPos.x) || (!isYAxis && lp.y >= zeroPos.y))
                        {
                            sp = smoothDownPoints[0];
                            for (int i = 1; i < smoothDownPoints.Count; i++)
                            {
                                ep = smoothDownPoints[i];

                                if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                    break;

                                if (luPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }

                                if ((isYAxis && ep.y > luPos.y) || (!isYAxis && ep.x > luPos.x))
                                {
                                    var tp = isYAxis ? new Vector3(luPos.x, sp.y) : new Vector3(sp.x, luPos.y);
                                    chart.DrawClipTriangle(vh, sp, luPos, tp, areaColor, areaToColor, areaToColor, serie.clip, grid);
                                    break;
                                }

                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                                sp = ep;
                            }
                            sp = smoothPoints[0];
                            bool first = false;
                            for (int i = 1; i < smoothPoints.Count; i++)
                            {
                                ep = smoothPoints[i];
                                if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                    break;

                                if ((isYAxis && ep.y <= rdPos.y) || (!isYAxis && ep.x <= rdPos.x))
                                    continue;

                                if (rdPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }
                                if (!first)
                                {
                                    first = true;
                                    var tp = isYAxis ? new Vector3(rdPos.x, ep.y) : new Vector3(ep.x, rdPos.y);
                                    chart.DrawClipTriangle(vh, rdPos, tp, ep, areaToColor, areaToColor, areaColor, serie.clip, grid);
                                    sp = ep;
                                    continue;
                                }
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                                sp = ep;
                            }
                        }
                        else
                        {
                            sp = smoothPoints[0];
                            for (int i = 1; i < smoothPoints.Count; i++)
                            {
                                ep = smoothPoints[i];
                                if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                    break;

                                if (rdPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }

                                if ((isYAxis && ep.y > rdPos.y) || (!isYAxis && ep.x > rdPos.x))
                                {
                                    var tp = isYAxis ? new Vector3(rdPos.x, sp.y) : new Vector3(sp.x, rdPos.y);
                                    chart.DrawClipTriangle(vh, sp, rdPos, tp, areaColor, areaToColor, areaToColor, serie.clip, grid);
                                    break;
                                }
                                if (rdPos != Vector3.zero)
                                    DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);

                                sp = ep;
                            }
                            sp = smoothDownPoints[0];
                            bool first = false;
                            for (int i = 1; i < smoothDownPoints.Count; i++)
                            {
                                ep = smoothDownPoints[i];

                                if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                    break;

                                if ((isYAxis && ep.y < luPos.y) || (!isYAxis && ep.x < luPos.x))
                                    continue;

                                if (luPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }

                                if (!first)
                                {
                                    first = true;
                                    var tp = isYAxis ? new Vector3(luPos.x, ep.y) : new Vector3(ep.x, luPos.y);
                                    chart.DrawClipTriangle(vh, ep, luPos, tp, areaColor, areaToColor, areaToColor, serie.clip, grid);
                                    sp = ep;
                                    continue;
                                }

                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                                sp = ep;
                            }
                        }
                    }
                }
            }
            stPos1 = isDown ? upPos2 : dnPos;
            stPos2 = isDown ? dnPos : upPos2;
            lastDnPos = dnPos;
            lastIsDown = isDown;
            return !isBreak;
        }

        private static bool TryAddToList(bool isTurnBack, bool isYAxis, List<Vector3> list, Vector3 lastPos,
            Vector3 pos, bool ignoreClose = false)
        {
            if (ChartHelper.IsZeroVector(pos))
                return false;

            if (isTurnBack)
            {
                list.Add(pos);
                return true;
            }
            else if (!ChartHelper.IsZeroVector(lastPos) && LineHelper.IsInRightOrUpNotCheckZero(isYAxis, pos, lastPos))
            {
                return false;
            }
            else if (list.Count <= 0)
            {
                list.Add(pos);
                return true;
            }
            else
            {
                var end = list[list.Count - 1];
                if (LineHelper.IsInRightOrUpNotCheckZero(isYAxis, end, pos)
                    && (!ignoreClose || !LineHelper.WasTooClose(isYAxis, end, pos, ignoreClose)))
                {
                    list.Add(pos);
                    return true;
                }
            }
            return false;
        }

        private void CheckLineGradientColor(VisualMap visualMap, Vector3 cp, LineStyle lineStyle, Axis axis,
            Color32 defaultLineColor, ref Color32 lineColor)
        {
            if (VisualMapHelper.IsNeedGradient(visualMap))
                lineColor = VisualMapHelper.GetLineGradientColor(visualMap, cp, chart, axis, defaultLineColor);
            else if (lineStyle.IsNeedGradient())
                lineColor = VisualMapHelper.GetLineStyleGradientColor(lineStyle, cp, chart, axis, defaultLineColor);
        }

        private void DrawPolygonToZero(VertexHelper vh, Vector3 sp, Vector3 ep, Axis axis, Vector3 zeroPos,
            Color32 areaColor, Color32 areaToColor, Vector3 areaDiff, bool clip = false)
        {
            float diff = 0;
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            var lineWidth = axis.axisLine.GetWidth(chart.theme.axis.lineWidth);

            if (axis is YAxis)
            {
                var isLessthan0 = (sp.x < zeroPos.x || ep.x < zeroPos.x);
                diff = isLessthan0 ? -lineWidth : lineWidth;
                areaColor = chart.GetYLerpColor(areaColor, areaToColor, sp, grid);
                if (isLessthan0) areaDiff = -areaDiff;
                chart.DrawClipPolygon(vh, new Vector3(zeroPos.x + diff, sp.y), new Vector3(zeroPos.x + diff, ep.y),
                     ep + areaDiff, sp + areaDiff, areaToColor, areaColor, clip, grid);
            }
            else
            {
                var isLessthan0 = (sp.y < zeroPos.y || ep.y < zeroPos.y);
                diff = isLessthan0 ? -lineWidth : lineWidth;
                areaColor = chart.GetXLerpColor(areaColor, areaToColor, sp, grid);
                if (isLessthan0) areaDiff = -areaDiff;
                if (isLessthan0)
                {
                    chart.DrawClipPolygon(vh, ep + areaDiff, sp + areaDiff, new Vector3(sp.x, zeroPos.y + diff),
                        new Vector3(ep.x, zeroPos.y + diff), areaColor, areaToColor, clip, grid);
                }
                else
                {
                    chart.DrawClipPolygon(vh, sp + areaDiff, ep + areaDiff, new Vector3(ep.x, zeroPos.y + diff),
                         new Vector3(sp.x, zeroPos.y + diff), areaColor, areaToColor, clip, grid);
                }
            }
        }

        private List<Vector3> posList = new List<Vector3>();
        private bool DrawOtherLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp,
            Vector3 np, int dataIndex, Color32 lineColor, Color32 areaColor,
            Color32 areaToColor, Vector3 zeroPos)
        {
            bool isYAxis = axis is YAxis;
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);

            posList.Clear();

            switch (serie.lineType)
            {
                case LineType.Dash:
                    UGL.DrawDashLine(vh, lp, np, lineWidth, lineColor, lineColor, 0, 0, posList);
                    break;
                case LineType.Dot:
                    UGL.DrawDotLine(vh, lp, np, lineWidth, lineColor, lineColor, 0, 0, posList);
                    break;
                case LineType.DashDot:
                    UGL.DrawDashDotLine(vh, lp, np, lineWidth, lineColor, 0, 0, 0, posList);
                    break;
                case LineType.DashDotDot:
                    UGL.DrawDashDotDotLine(vh, lp, np, lineWidth, lineColor, 0, 0, 0, posList);
                    break;
            }

            if (serie.areaStyle.show && !isYAxis && posList.Count > 0)
            {
                lp = posList[0];
                var value = serie.GetSerieData(dataIndex).data[1];

                for (int i = 0; i < posList.Count; i++)
                {
                    np = posList[i];
                    var start = new Vector3(lp.x, value > 0 ? (lp.y - lineWidth) : (lp.y + lineWidth));
                    var end = new Vector3(np.x, value > 0 ? (np.y - lineWidth) : (np.y + lineWidth));
                    DrawPolygonToZero(vh, start, end, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                    lp = np;
                }
            }

            return true;
        }

        private List<Vector3> bezierPoints = new List<Vector3>();
        private Vector3 smoothStartPosUp, smoothStartPosDn;
        private bool DrawSmoothLine(VertexHelper vh, Serie serie, Axis xAxis, GridCoord grid, VisualMap visualMap,
            Vector3 lp, Vector3 np, Vector3 llp, Vector3 nnp, int dataIndex, Color32 lineColor, Color32 areaColor,
            Color32 areaToColor, bool isStack, Vector3 zeroPos, int startIndex = 0)
        {
            var defaultLineColor = lineColor;
            var defaultAreaColor = areaColor;
            var defaultAreaToColor = areaToColor;
            bool isYAxis = xAxis is YAxis;
            var isTurnBack = LineHelper.IsInRightOrUp(isYAxis, np, lp);
            var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            var lastSmoothPoint = Vector3.zero;
            var lastSmoothDownPoint = Vector3.zero;
            var settings = chart.settings;

            if (dataIndex > startIndex)
            {
                lastSmoothPoint = ChartHelper.GetLastPoint(serie.GetUpSmoothList(dataIndex - 1));
                lastSmoothDownPoint = ChartHelper.GetLastPoint(serie.GetDownSmoothList(dataIndex - 1));
                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, lastSmoothPoint, true);
                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, lastSmoothDownPoint, true);
            }

            if (isYAxis)
                ChartHelper.GetBezierListVertical(ref bezierPoints, lp, np, settings.lineSmoothness, settings.lineSmoothStyle);
            else
                ChartHelper.GetBezierList(ref bezierPoints, lp, np, llp, nnp, settings.lineSmoothness, settings.lineSmoothStyle);

            Vector3 start, to;
            if (serie.lineType == LineType.SmoothDash)
            {
                for (int i = 0; i < bezierPoints.Count - 2; i += 2)
                {
                    start = bezierPoints[i];
                    to = bezierPoints[i + 1];
                    CheckLineGradientColor(visualMap, start, serie.lineStyle, xAxis, defaultLineColor, ref lineColor);
                    chart.DrawClipLine(vh, start, to, lineWidth, lineColor, serie.clip, grid);
                }
                return true;
            }

            start = bezierPoints[0];

            var dir = bezierPoints[1] - start;
            var dir1v = Vector3.Cross(dir, Vector3.forward).normalized * (isYAxis ? -1 : 1);
            var diff = dir1v * lineWidth;
            var startUp = start - diff;
            var startDn = start + diff;
            var startAreaDn = Vector3.zero;
            var startAreaUp = Vector3.zero;
            Vector3 toUp, toDn;
            bool isFinish = true;

            if (dataIndex > startIndex + 1)
            {
                if (smoothStartPosDn != Vector3.zero && smoothStartPosUp != Vector3.zero)
                {
                    if (!serie.animation.IsInFadeOut())
                    {
                        if (ChartHelper.IsClearColor(serie.lineStyle.color))
                            CheckLineGradientColor(visualMap, lp, serie.lineStyle, xAxis, defaultLineColor, ref lineColor);
                        chart.DrawClipTriangle(vh, smoothStartPosUp, startUp, lp, lineColor, serie.clip, grid);
                        chart.DrawClipTriangle(vh, smoothStartPosDn, startDn, lp, lineColor, serie.clip, grid);
                        TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, smoothStartPosUp, false);
                        TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, smoothStartPosDn, false);
                    }
                }
            }
            else
            {
                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, startUp, false);
                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, startDn, false);
            }

            var bezierPointsCount = bezierPoints.Count;
            for (int k = 1; k < bezierPointsCount; k++)
            {
                var isEndPos = k == bezierPointsCount - 1;
                to = bezierPoints[k];
                if (serie.animation.CheckDetailBreak(to, isYAxis))
                {
                    isFinish = false;
                    break;
                }
                dir = to - start;
                dir1v = Vector3.Cross(dir, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                diff = dir1v * lineWidth;
                toUp = to - diff;
                toDn = to + diff;

                if (ChartHelper.IsClearColor(serie.lineStyle.color))
                    CheckLineGradientColor(visualMap, to, serie.lineStyle, xAxis, defaultLineColor, ref lineColor);

                if (isYAxis)
                    chart.DrawClipPolygon(vh, startDn, toDn, toUp, startUp, lineColor, serie.clip, grid);
                else
                    chart.DrawClipPolygon(vh, startUp, toUp, toDn, startDn, lineColor, serie.clip, grid);

                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, toUp, true);
                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, toDn, true);

                if (isEndPos)
                {
                    smoothStartPosUp = toUp;
                    smoothStartPosDn = toDn;
                }

                start = to;
                startUp = toUp;
                startDn = toDn;
            }
            if (serie.areaStyle.show && (serie.index == 0 || !isStack))
            {
                if (smoothDownPoints.Count > 0)
                {
                    start = smoothDownPoints[0];
                    for (int i = 1; i < smoothDownPoints.Count; i++)
                    {
                        to = smoothDownPoints[i];

                        if (LineHelper.IsInRightOrUp(!isYAxis, zeroPos, to))
                        {
                            if (ChartHelper.IsClearColor(serie.areaStyle.color))
                            {
                                CheckLineGradientColor(visualMap, to, serie.lineStyle, xAxis, defaultAreaColor, ref areaColor);
                                CheckLineGradientColor(visualMap, to, serie.lineStyle, xAxis, defaultAreaToColor, ref areaToColor);
                            }
                            DrawPolygonToZero(vh, start, to, xAxis, zeroPos, areaColor, areaToColor, Vector3.zero);
                        }
                        start = to;
                    }
                }

                if (smoothPoints.Count > 0)
                {
                    start = smoothPoints[smoothPoints.Count - 1];
                    for (int i = smoothPoints.Count - 1; i >= 0; i--)
                    {
                        to = smoothPoints[i];
                        if (!LineHelper.IsInRightOrUp(!isYAxis, zeroPos, to))
                        {
                            DrawPolygonToZero(vh, to, start, xAxis, zeroPos, areaColor, areaToColor, Vector3.zero);
                        }
                        start = to;
                    }
                }
            }

            if (serie.areaStyle.show)
            {
                var lastSerie = SeriesHelper.GetLastStackSerie(chart.series, serie);
                if (lastSerie != null)
                {
                    var lastSmoothPoints = lastSerie.GetUpSmoothList(dataIndex);
                    DrawStackArea(vh, serie, xAxis, smoothDownPoints, lastSmoothPoints, areaColor, areaToColor);
                }
            }
            return isFinish;
        }

        private void DrawStackArea(VertexHelper vh, Serie serie, Axis axis, List<Vector3> smoothPoints,
            List<Vector3> lastSmoothPoints, Color32 areaColor, Color32 areaToColor)
        {
            if (!serie.areaStyle.show || lastSmoothPoints.Count <= 0)
                return;

            Vector3 start, to;
            var isYAxis = axis is YAxis;

            var lastCount = 1;
            start = smoothPoints[0];
            var sourAreaColor = areaColor;
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);

            for (int k = 1; k < smoothPoints.Count; k++)
            {
                to = smoothPoints[k];
                if (!LineHelper.IsInRightOrUp(isYAxis, start, to))
                    continue;
                if (serie.animation.CheckDetailBreak(to, isYAxis))
                    break;

                Vector3 tnp, tlp;
                if (isYAxis)
                    areaColor = chart.GetYLerpColor(sourAreaColor, areaToColor, to, grid);
                else
                    areaColor = chart.GetXLerpColor(sourAreaColor, areaToColor, to, grid);

                if (k == smoothPoints.Count - 1)
                {
                    if (k < lastSmoothPoints.Count - 1)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        chart.DrawClipTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor, serie.clip, grid);
                        while (lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis))
                                break;

                            chart.DrawClipTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor, serie.clip, grid);
                            lastCount++;
                            tnp = tlp;
                        }
                        start = to;
                        continue;
                    }
                }
                if (lastCount >= lastSmoothPoints.Count)
                {
                    tlp = lastSmoothPoints[lastSmoothPoints.Count - 1];
                    if (serie.animation.CheckDetailBreak(tlp, isYAxis))
                        break;

                    chart.DrawClipTriangle(vh, to, start, tlp, areaColor, areaColor, areaToColor, serie.clip, grid);
                    start = to;
                    continue;
                }
                tnp = lastSmoothPoints[lastCount];
                var diff = isYAxis ? tnp.y - to.y : tnp.x - to.x;
                if (Math.Abs(diff) < 1)
                {
                    tlp = lastSmoothPoints[lastCount - 1];
                    if (serie.animation.CheckDetailBreak(tlp, isYAxis))
                        break;

                    chart.DrawClipPolygon(vh, start, to, tnp, tlp, areaColor, areaToColor, serie.clip, grid);
                    lastCount++;
                }
                else
                {
                    if (diff < 0)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        chart.DrawClipTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor, serie.clip, grid);
                        while (diff < 0 && lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis))
                                break;

                            chart.DrawClipTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor, serie.clip, grid);
                            lastCount++;
                            diff = isYAxis ? tlp.y - to.y : tlp.x - to.x;
                            tnp = tlp;
                        }
                    }
                    else
                    {
                        tlp = lastSmoothPoints[lastCount - 1];
                        if (serie.animation.CheckDetailBreak(tlp, isYAxis))
                            break;

                        chart.DrawClipTriangle(vh, start, to, tlp, areaColor, areaColor, areaToColor, serie.clip, grid);
                    }
                }
                start = to;
            }
            if (lastCount < lastSmoothPoints.Count)
            {
                var p1 = lastSmoothPoints[lastCount - 1];
                var p2 = lastSmoothPoints[lastSmoothPoints.Count - 1];
                if (!serie.animation.CheckDetailBreak(p1, isYAxis) && !serie.animation.CheckDetailBreak(p2, isYAxis))
                {
                    chart.DrawClipTriangle(vh, p1, start, p2, areaToColor, areaColor, areaToColor, serie.clip, grid);
                }
            }
        }

        private List<Vector3> linePointList = new List<Vector3>();
        private bool DrawStepLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp, Vector3 np,
            Vector3 nnp, int dataIndex, Color32 lineColor, Color32 areaColor, Color32 areaToColor, Vector3 zeroPos)
        {
            bool isYAxis = axis is YAxis;
            float lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
            Vector3 start, end, middle, middleZero, middle1, middle2;
            Vector3 sp, ep, diff1, diff2;
            var areaDiff = isYAxis ? Vector3.left * lineWidth : Vector3.down * lineWidth;
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            var settings = chart.settings;

            switch (serie.lineType)
            {
                case LineType.StepStart:

                    middle = isYAxis ? new Vector3(np.x, lp.y) : new Vector3(lp.x, np.y);
                    middleZero = isYAxis ? new Vector3(zeroPos.x, middle.y) : new Vector3(middle.x, zeroPos.y);
                    diff1 = (middle - lp).normalized * lineWidth;
                    diff2 = (np - middle).normalized * lineWidth;
                    start = dataIndex == 1 ? lp : lp + diff1;
                    end = nnp != np ? np - diff2 : np;

                    if (Vector3.Distance(lp, middle) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, start, middle - diff1, settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                return false;

                            chart.DrawClipLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            sp = ep;
                        }
                        chart.DrawClipPolygon(vh, middle, lineWidth, lineColor, serie.clip, true, grid);
                    }
                    else
                    {
                        if (dataIndex == 1)
                            chart.DrawClipPolygon(vh, lp, lineWidth, lineColor, serie.clip, true, grid);

                        chart.DrawClipLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor, serie.clip, grid);
                    }
                    if (serie.areaStyle.show)
                    {
                        if (Vector3.Dot(middle - lp, middleZero - middle) >= 0)
                        {
                            DrawPolygonToZero(vh, middle - diff2, middle + diff2, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                        else if (dataIndex == 1)
                        {
                            DrawPolygonToZero(vh, lp - diff2, lp + diff2, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                        }
                    }

                    ChartHelper.GetPointList(ref linePointList, middle + diff2, end, settings.lineSegmentDistance);

                    sp = linePointList[0];
                    for (int i = 1; i < linePointList.Count; i++)
                    {
                        ep = linePointList[i];
                        if (serie.animation.CheckDetailBreak(ep, isYAxis))
                            return false;

                        chart.DrawClipLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                        sp = ep;
                    }

                    if (nnp != np)
                    {
                        if (serie.animation.CheckDetailBreak(np, isYAxis))
                            return false;

                        chart.DrawClipPolygon(vh, np, lineWidth, lineColor, serie.clip, true, grid);
                        bool flag = ((isYAxis && nnp.x > np.x && np.x > zeroPos.x) || (!isYAxis && nnp.y > np.y && np.y > zeroPos.y));
                        if (serie.areaStyle.show && flag)
                        {
                            DrawPolygonToZero(vh, np - diff2, np + diff2, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    break;

                case LineType.StepMiddle:

                    middle1 = isYAxis ? new Vector2(lp.x, (lp.y + np.y) / 2) : new Vector2((lp.x + np.x) / 2, lp.y);
                    middle2 = isYAxis ? new Vector2(np.x, (lp.y + np.y) / 2) : new Vector2((lp.x + np.x) / 2, np.y);
                    middleZero = isYAxis ? new Vector3(zeroPos.x, middle1.y) : new Vector3(middle1.x, zeroPos.y);
                    diff1 = (middle1 - lp).normalized * lineWidth;
                    diff2 = (middle2 - middle1).normalized * lineWidth;

                    start = dataIndex == 1 ? lp : lp + diff1;
                    end = nnp != np ? np - diff2 : np;

                    //draw lp to middle1
                    if (Vector3.Distance(lp, middle1) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, start, middle1 - diff1, settings.lineSegmentDistance);
                        sp = linePointList[0];

                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];

                            if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                return false;

                            chart.DrawClipLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }

                        if (serie.animation.CheckDetailBreak(middle1, isYAxis))
                            return false;

                        chart.DrawClipPolygon(vh, middle1, lineWidth, lineColor, serie.clip, true, grid);
                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle1, middle2 - middle1) <= 0)
                        {
                            DrawPolygonToZero(vh, middle1 - diff1, middle1 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1)
                            chart.DrawClipPolygon(vh, lp, lineWidth, lineColor, serie.clip, true, grid);

                        chart.DrawClipLine(vh, lp + diff1, middle1 + diff1, lineWidth, lineColor, serie.clip, grid);
                    }

                    //draw middle1 to middle2
                    if (Vector3.Distance(middle1, middle2) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle1 + diff2, middle2 - diff2, settings.lineSegmentDistance);
                        sp = linePointList[0];

                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            chart.DrawClipLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            sp = ep;
                        }

                        chart.DrawClipPolygon(vh, middle2, lineWidth, lineColor, serie.clip, true, grid);

                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle2, middle2 - middle1) >= 0)
                        {
                            DrawPolygonToZero(vh, middle2 - diff1, middle2 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        chart.DrawClipLine(vh, middle1 + diff2, middle2 + diff2, lineWidth, lineColor, serie.clip, grid);
                    }
                    //draw middle2 to np
                    if (Vector3.Distance(middle2, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle2 + diff1, np - diff1, settings.lineSegmentDistance);
                        sp = linePointList[0];

                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];

                            if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                return false;

                            chart.DrawClipLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }

                        if (serie.animation.CheckDetailBreak(np, isYAxis))
                            return false;

                        chart.DrawClipPolygon(vh, np, lineWidth, lineColor, serie.clip, true, grid);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, np - diff1, np + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        chart.DrawClipLine(vh, middle1 + diff1, middle1 + diff1, lineWidth, lineColor, serie.clip, grid);
                    }
                    break;

                case LineType.StepEnd:

                    middle = isYAxis ? new Vector3(lp.x, np.y) : new Vector3(np.x, lp.y);
                    middleZero = isYAxis ? new Vector3(zeroPos.x, middle.y) : new Vector3(middle.x, zeroPos.y);
                    diff1 = (middle - lp).normalized * lineWidth;
                    diff2 = (np - middle).normalized * lineWidth;
                    start = dataIndex == 1 ? lp : lp + diff1;
                    end = nnp != np ? np - diff2 : np;

                    if (Vector3.Distance(lp, middle) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, start, middle - diff1, settings.lineSegmentDistance);
                        sp = linePointList[0];

                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];

                            if (serie.animation.CheckDetailBreak(ep, isYAxis))
                                return false;

                            chart.DrawClipLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);

                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }

                        if (serie.animation.CheckDetailBreak(middle, isYAxis))
                            return false;

                        chart.DrawClipPolygon(vh, middle, lineWidth, lineColor, serie.clip, true, grid);

                        if (serie.areaStyle.show && Vector3.Dot(np - middle, middleZero - middle) <= 0)
                        {
                            DrawPolygonToZero(vh, middle - diff1, middle + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1)
                            chart.DrawClipPolygon(vh, lp, lineWidth, lineColor, serie.clip, true, grid);

                        chart.DrawClipLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor, serie.clip, grid);
                    }

                    if (Vector3.Distance(middle, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle + diff2, end, settings.lineSegmentDistance);
                        sp = linePointList[0];

                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            chart.DrawClipLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            sp = ep;
                        }

                        if (nnp != np)
                            chart.DrawClipPolygon(vh, np, lineWidth, lineColor, serie.clip, true, grid);
                    }
                    else
                    {
                        chart.DrawClipLine(vh, middle + diff2, np + diff2, lineWidth, lineColor, serie.clip, grid);
                    }

                    bool flag2 = ((isYAxis && middle.x > np.x && np.x > zeroPos.x)
                        || (!isYAxis && middle.y > np.y && np.y > zeroPos.y));
                    if (serie.areaStyle.show && flag2)
                    {
                        DrawPolygonToZero(vh, np - diff1, np + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                    }
                    break;
            }
            return true;
        }
    }
}