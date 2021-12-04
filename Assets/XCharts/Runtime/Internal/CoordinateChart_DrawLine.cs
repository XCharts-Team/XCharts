using System.Runtime.InteropServices.ComTypes;
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    public partial class CoordinateChart
    {
        protected void DrawLinePoint(VertexHelper vh, Serie serie)
        {
            if (!serie.show || serie.IsPerformanceMode()) return;
            if (serie.type != SerieType.Line) return;
            var count = serie.dataPoints.Count;
            var clip = SeriesHelper.IsAnyClipSerie(m_Series);
            var grid = GetSerieGridOrDefault(serie);
            for (int i = 0; i < count; i++)
            {
                var serieData = serie.GetSerieData(i);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.show || !symbol.ShowSymbol(i, count)) continue;
                if (serie.lineArrow.show)
                {
                    if (serie.lineArrow.position == LineArrow.Position.Start && i == 0) continue;
                    if (serie.lineArrow.position == LineArrow.Position.End && i == count - 1) continue;
                }
                if (ChartHelper.IsIngore(serie.dataPoints[i])) continue;
                bool highlight = (tooltip.show && tooltip.IsSelected(i))
                    || serie.data[i].highlighted || serie.highlighted;
                float symbolSize = highlight
                    ? symbol.GetSelectedSize(serie.data[i].data, m_Theme.serie.lineSymbolSelectedSize)
                    : symbol.GetSize(serie.data[i].data, m_Theme.serie.lineSymbolSize);
                var symbolColor = SerieHelper.GetItemColor(serie, serieData, m_Theme, serie.index, highlight);
                var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, m_Theme, serie.index, highlight);
                var backgroundColor = SerieHelper.GetItemBackgroundColor(serie, serieData, m_Theme, serie.index, highlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, m_Theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                Internal_CheckClipAndDrawSymbol(vh, symbol.type, symbolSize, symbolBorder, serie.dataPoints[i], symbolColor,
                    symbolToColor, backgroundColor, symbol.gap, clip, cornerRadius, grid,
                    i > 0 ? serie.dataPoints[i - 1] : grid.runtimePosition);
            }
        }

        protected void DrawLineArrow(VertexHelper vh, Serie serie)
        {
            if (serie.type != SerieType.Line) return;
            if (!serie.show || !serie.lineArrow.show) return;
            if (serie.dataPoints.Count < 2) return;
            Color32 lineColor = SerieHelper.GetLineColor(serie, m_Theme, serie.index, false);
            Vector3 startPos, arrowPos;
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

        protected void DrawXLineSerie(VertexHelper vh, Serie serie, int colorIndex)
        {
            if (!IsActive(serie.index)) return;
            if (serie.animation.HasFadeOut()) return;
            var yAxis = GetSerieYAxisOrDefault(serie);
            var xAxis = GetSerieXAxisOrDefault(serie);
            var grid = GetSerieGridOrDefault(serie);
            var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
            var showData = serie.GetDataList(dataZoom);
            if (showData.Count <= 0) return;
            Color32 lineColor = SerieHelper.GetLineColor(serie, m_Theme, colorIndex, serie.highlighted);
            Color32 srcAreaColor = SerieHelper.GetAreaColor(serie, m_Theme, colorIndex, false);
            Color32 srcAreaToColor = SerieHelper.GetAreaToColor(serie, m_Theme, colorIndex, false);
            Color32 highlightAreaColor = SerieHelper.GetAreaColor(serie, m_Theme, colorIndex, true);
            Color32 highlightAreaToColor = SerieHelper.GetAreaToColor(serie, m_Theme, colorIndex, true);
            Color32 areaColor, areaToColor;
            Vector3 lp = Vector3.zero, np = Vector3.zero, llp = Vector3.zero, nnp = Vector3.zero;
            var zeroPos = new Vector3(grid.runtimeX, grid.runtimeY + yAxis.runtimeZeroYOffset);
            var isStack = SeriesHelper.IsStack(m_Series, serie.stack, SerieType.Line);
            m_StackSerieData.Clear();
            if (isStack) SeriesHelper.UpdateStackDataList(m_Series, serie, dataZoom, m_StackSerieData);
            float scaleWid = AxisHelper.GetDataWidth(xAxis, grid.runtimeWidth, showData.Count, dataZoom);
            xAxis.runtimeScaleWidth = scaleWid;
            float startX = grid.runtimeX + (xAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            int i;
            int rate = 1;
            var sampleDist = serie.sampleDist;
            if (sampleDist > 0) rate = (int)((maxCount - serie.minShow) / (grid.runtimeWidth / sampleDist));
            if (rate < 1) rate = 1;
            var includeLastData = false;
            var totalAverage = serie.sampleAverage > 0 ? serie.sampleAverage :
                DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            for (i = serie.minShow; i < maxCount; i += rate)
            {
                if (i == maxCount - 1) includeLastData = true;
                if (serie.IsIgnoreValue(showData[i]))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    showData[i].runtimeStackHig = 0;
                }
                else
                {
                    double yValue = SampleValue(ref showData, serie.sampleType, rate, serie.minShow, maxCount, totalAverage,
                        i, dataChangeDuration, ref dataChanging, yAxis);
                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, isStack,
                        ref np, dataChangeDuration);
                    serie.dataPoints.Add(np);
                }
            }
            if (dataChanging)
            {
                RefreshPainter(serie);
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
                    double yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse, yAxis.runtimeMinValue, yAxis.runtimeMaxValue);
                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, isStack, ref np,
                        dataChangeDuration);
                    serie.dataPoints.Add(np);
                }
            }
            if (serie.dataPoints.Count <= 0)
            {
                return;
            }
            var startIndex = 0;
            var endIndex = serie.dataPoints.Count;
            var startPos = GetStartPos(serie.dataPoints, ref startIndex, serie.ignoreLineBreak);
            var endPos = GetEndPos(serie.dataPoints, ref endIndex, serie.ignoreLineBreak);
            lp = startPos;
            stPos1 = stPos2 = lastDir = lastDnPos = Vector3.zero;
            smoothStartPosUp = smoothStartPosDn = Vector3.zero;

            Vector3 firstLastPos = Vector3.zero, lastNextPos = Vector3.zero;
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
                    double yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse, yAxis.runtimeMinValue, yAxis.runtimeMaxValue);
                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, isStack, ref firstLastPos, dataChangeDuration);
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
                    double yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse, yAxis.runtimeMinValue, yAxis.runtimeMaxValue);
                    showData[i].runtimeStackHig = GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, isStack, ref lastNextPos, dataChangeDuration);
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
                if (!serie.animation.NeedAnimation(i)) break;
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
                        lp = GetLastPos(serie.dataPoints, i, np, serie.ignoreLineBreak);
                        nnp = GetNNPos(serie.dataPoints, i, np, serie.ignoreLineBreak);
                        if (lp == Vector3.zero && serie.ignoreLineBreak) isIgnoreBreak = true;
                        isFinish = DrawNormalLine(vh, serie, xAxis, lp, np, nnp, i,
                            isIgnoreBreak ? ColorUtil.clearColor32 : lineColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaToColor,
                            zeroPos, startIndex);
                        break;
                    case LineType.Smooth:
                    case LineType.SmoothDash:
                        llp = GetLLPos(serie.dataPoints, i, firstLastPos, serie.ignoreLineBreak);
                        nnp = GetNNPos(serie.dataPoints, i, lastNextPos, serie.ignoreLineBreak);
                        if (lp == Vector3.zero && serie.ignoreLineBreak) isIgnoreBreak = true;
                        isFinish = DrawSmoothLine(vh, serie, xAxis, lp, np, llp, nnp, i,
                            isIgnoreBreak ? ColorUtil.clearColor32 : lineColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaColor,
                            isIgnoreBreak ? ColorUtil.clearColor32 : areaToColor,
                             isStack, zeroPos, startIndex);
                        break;
                    case LineType.StepStart:
                    case LineType.StepMiddle:
                    case LineType.StepEnd:
                        nnp = GetNNPos(serie.dataPoints, i, np, serie.ignoreLineBreak);
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
                if (isFinish) serie.animation.SetDataFinish(i);
                if (np != Vector3.zero || serie.ignoreLineBreak)
                {
                    lp = np;
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress - currDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, m_Theme.serie.lineSymbolSize));
                m_IsPlayingAnimation = true;
                RefreshPainter(serie);
            }
        }

        private Vector3 GetNNPos(List<Vector3> dataPoints, int index, Vector3 np, bool ignoreLineBreak)
        {
            int size = dataPoints.Count;
            if (index >= size) return np;
            for (int i = index + 1; i < size; i++)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak) return dataPoints[i];
            }
            return np;
        }

        private Vector3 GetStartPos(List<Vector3> dataPoints, ref int start, bool ignoreLineBreak)
        {
            for (int i = 0; i < dataPoints.Count; i++)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak)
                {
                    start = i;
                    return dataPoints[i];
                }
            }
            return Vector3.zero;
        }

        private Vector3 GetEndPos(List<Vector3> dataPoints, ref int end, bool ignoreLineBreak)
        {
            for (int i = dataPoints.Count - 1; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak)
                {
                    end = i;
                    return dataPoints[i];
                }
            }
            return Vector3.zero;
        }

        private Vector3 GetLastPos(List<Vector3> dataPoints, int index, Vector3 pos, bool ignoreLineBreak)
        {
            if (index <= 0) return pos;
            for (int i = index - 1; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak) return dataPoints[i];
            }
            return pos;
        }

        private Vector3 GetLLPos(List<Vector3> dataPoints, int index, Vector3 lp, bool ignoreLineBreak)
        {
            if (index <= 1) return lp;
            for (int i = index - 2; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak) return dataPoints[i];
            }
            return lp;
        }

        internal double DataAverage(ref List<SerieData> showData, SampleType sampleType, int minCount, int maxCount, int rate)
        {
            double totalAverage = 0;
            if (rate > 1 && sampleType == SampleType.Peak)
            {
                double total = 0;
                for (int i = minCount; i < maxCount; i++)
                {
                    total += showData[i].data[1];
                }
                totalAverage = total / (maxCount - minCount);
            }
            return totalAverage;
        }

        internal double SampleValue(ref List<SerieData> showData, SampleType sampleType, int rate,
            int minCount, int maxCount, double totalAverage, int index, float dataChangeDuration,
            ref bool dataChanging, Axis axis)
        {
            var inverse = axis.inverse;
            double minValue = axis.runtimeMinValue;
            double MaxValue = axis.runtimeMaxValue;
            if (rate <= 1 || index == minCount)
            {
                if (showData[index].IsDataChanged()) dataChanging = true;
                return showData[index].GetCurrData(1, dataChangeDuration, inverse, minValue, MaxValue);
            }
            switch (sampleType)
            {
                case SampleType.Sum:
                case SampleType.Average:
                    double total = 0;
                    for (int i = index; i > index - rate; i--)
                    {
                        total += showData[i].GetCurrData(1, dataChangeDuration, inverse, minValue, MaxValue);
                        if (showData[i].IsDataChanged()) dataChanging = true;
                    }
                    if (sampleType == SampleType.Average) return total / rate;
                    else return total;
                case SampleType.Max:
                    double max = double.MinValue;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataChangeDuration, inverse, minValue, MaxValue);
                        if (value > max) max = value;
                        if (showData[i].IsDataChanged()) dataChanging = true;
                    }
                    return max;
                case SampleType.Min:
                    double min = double.MaxValue;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataChangeDuration, inverse, minValue, MaxValue);
                        if (value < min) min = value;
                        if (showData[i].IsDataChanged()) dataChanging = true;
                    }
                    return min;
                case SampleType.Peak:
                    max = double.MinValue;
                    min = double.MaxValue;
                    total = 0;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataChangeDuration, inverse, minValue, MaxValue);
                        total += value;
                        if (value < min) min = value;
                        if (value > max) max = value;
                        if (showData[i].IsDataChanged()) dataChanging = true;
                    }
                    var average = total / rate;
                    if (average >= totalAverage) return max;
                    else return min;
            }
            if (showData[index].IsDataChanged()) dataChanging = true;
            return showData[index].GetCurrData(1, dataChangeDuration, inverse, minValue, MaxValue);
        }

        private float GetDataPoint(Axis xAxis, Axis yAxis, List<SerieData> showData, double yValue, float startX, int i,
            float scaleWid, bool isStack, ref Vector3 np, float duration, bool isIngoreValue = false)
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
            if (xAxis.IsValue() || xAxis.IsLog())
            {
                var grid = GetAxisGridOrDefault(xAxis);
                var axisLineWidth = xAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                double xValue = i > showData.Count - 1 ? 0 : showData[i].GetData(0, xAxis.inverse);
                float pX = grid.runtimeX + axisLineWidth;
                float pY = grid.runtimeY + axisLineWidth;
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pY += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }
                if (xAxis.IsLog())
                {
                    int minIndex = xAxis.runtimeMinLogIndex;
                    float nowIndex = xAxis.GetLogValue(xValue);
                    xDataHig = (nowIndex - minIndex) / xAxis.splitNumber * grid.runtimeWidth;
                }
                else
                {
                    if ((xMaxValue - xMinValue) <= 0) xDataHig = 0;
                    else xDataHig = (float)((xValue - xMinValue) / (xMaxValue - xMinValue)) * grid.runtimeWidth;
                }
                if (yAxis.IsLog())
                {
                    int minIndex = yAxis.runtimeMinLogIndex;
                    float nowIndex = yAxis.GetLogValue(yValue);
                    yDataHig = (nowIndex - minIndex) / yAxis.splitNumber * grid.runtimeHeight;
                }
                else
                {
                    double valueTotal = yMaxValue - yMinValue;
                    if (valueTotal <= 0) yDataHig = 0;
                    else yDataHig = (float)((yValue - yMinValue) / valueTotal) * grid.runtimeHeight;
                }
                np = new Vector3(pX + xDataHig, pY + yDataHig);
            }
            else
            {
                var grid = GetAxisGridOrDefault(yAxis);
                var axisLineWidth = yAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                float pX = startX + i * scaleWid;
                float pY = grid.runtimeY + axisLineWidth;
                if (isStack)
                {
                    for (int n = 0; n < m_StackSerieData.Count - 1; n++)
                    {
                        pY += m_StackSerieData[n][i].runtimeStackHig;
                    }
                }
                if (yAxis.IsLog())
                {
                    int minIndex = yAxis.runtimeMinLogIndex;
                    float nowIndex = yAxis.GetLogValue(yValue);
                    yDataHig = (nowIndex - minIndex) / yAxis.splitNumber * grid.runtimeHeight;
                }
                else
                {
                    double valueTotal = yMaxValue - yMinValue;
                    if (valueTotal <= 0) yDataHig = 0;
                    else yDataHig = (float)((yValue - yMinValue) / valueTotal * grid.runtimeHeight);
                }
                np = new Vector3(pX, pY + yDataHig);
            }
            return yDataHig;
        }

        List<List<SerieData>> m_StackSerieData = new List<List<SerieData>>();
        protected void DrawYLineSerie(VertexHelper vh, Serie serie, int colorIndex)
        {
            if (!IsActive(serie.index)) return;
            if (serie.animation.HasFadeOut()) return;
            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            Vector3 llp = Vector3.zero;
            Vector3 nnp = Vector3.zero;
            var lineColor = SerieHelper.GetLineColor(serie, m_Theme, colorIndex, serie.highlighted);
            var srcAreaColor = SerieHelper.GetAreaColor(serie, m_Theme, colorIndex, false);
            var srcAreaToColor = SerieHelper.GetAreaToColor(serie, m_Theme, colorIndex, false);
            var highlightAreaColor = SerieHelper.GetAreaColor(serie, m_Theme, colorIndex, true);
            var highlightAreaToColor = SerieHelper.GetAreaToColor(serie, m_Theme, colorIndex, true);
            Color32 areaColor, areaToColor;
            var xAxis = m_XAxes[serie.xAxisIndex];
            var yAxis = m_YAxes[serie.yAxisIndex];
            var grid = GetSerieGridOrDefault(serie);
            var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(yAxis, dataZooms);
            var showData = serie.GetDataList(dataZoom);
            var zeroPos = new Vector3(grid.runtimeX + xAxis.runtimeZeroXOffset, grid.runtimeY);
            var isStack = SeriesHelper.IsStack(m_Series, serie.stack, SerieType.Line);
            m_StackSerieData.Clear();
            if (isStack) SeriesHelper.UpdateStackDataList(m_Series, serie, dataZoom, m_StackSerieData);
            if (!yAxis.show) yAxis = m_YAxes[(serie.yAxisIndex + 1) % m_YAxes.Count];
            float scaleWid = AxisHelper.GetDataWidth(yAxis, grid.runtimeHeight, showData.Count, dataZoom);
            float startY = grid.runtimeY + (yAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            int i = 0;
            int rate = 1;
            var sampleDist = serie.sampleDist;
            if (sampleDist > 0) rate = (int)((maxCount - serie.minShow) / (grid.runtimeWidth / sampleDist));
            if (rate < 1) rate = 1;
            var dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double xMinValue = xAxis.GetCurrMinValue(dataChangeDuration);
            double xMaxValue = xAxis.GetCurrMaxValue(dataChangeDuration);
            for (i = serie.minShow; i < maxCount; i += rate)
            {
                double value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse, xAxis.runtimeMinValue, xAxis.runtimeMaxValue);
                float pY = startY + i * scaleWid;
                float pX = grid.runtimeX + yAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
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
                    int minIndex = xAxis.runtimeMinLogIndex;
                    float nowIndex = xAxis.GetLogValue(value);
                    dataHig = (nowIndex - minIndex) / (xAxis.splitNumber - 1) * grid.runtimeWidth;
                }
                else
                {
                    dataHig = (float)((value - xMinValue) / (xMaxValue - xMinValue) * grid.runtimeWidth);
                }
                showData[i].runtimeStackHig = dataHig;
                np = new Vector3(pX + dataHig, pY);
                serie.dataPoints.Add(np);
                if (showData[i].IsDataChanged()) dataChanging = true;
            }
            if (dataChanging)
            {
                RefreshPainter(serie);
            }
            if (maxCount % rate != 0)
            {
                i = maxCount - 1;
                double value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse, xAxis.runtimeMinValue, xAxis.runtimeMaxValue);
                float pY = startY + i * scaleWid;
                float pX = grid.runtimeX + yAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
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
                    int minIndex = xAxis.runtimeMinLogIndex;
                    float nowIndex = xAxis.GetLogValue(value);
                    dataHig = (nowIndex - minIndex) / xAxis.splitNumber * grid.runtimeWidth;
                }
                else
                {
                    dataHig = (float)((value - xMinValue) / (xMaxValue - xMinValue)) * grid.runtimeWidth;
                }
                showData[i].runtimeStackHig = dataHig;
                np = new Vector3(pX + dataHig, pY);
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
                if (!serie.animation.NeedAnimation(i)) break;
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
                        isFinish = DrawNormalLine(vh, serie, yAxis, lp, np, nnp, i, lineColor,
                            areaColor, areaToColor, zeroPos, 0);
                        break;
                    case LineType.Smooth:
                    case LineType.SmoothDash:
                        llp = i > 1 ? serie.dataPoints[i - 2] : lp;
                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawSmoothLine(vh, serie, yAxis, lp, np, llp, nnp, i,
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
                        UGL.DrawDashLine(vh, lp, np, serie.lineStyle.GetWidth(m_Theme.serie.lineWidth), lineColor, lineColor);
                        isFinish = true;
                        break;
                    case LineType.Dot:
                        UGL.DrawDotLine(vh, lp, np, serie.lineStyle.GetWidth(m_Theme.serie.lineWidth), lineColor, lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDot:
                        UGL.DrawDashDotLine(vh, lp, np, serie.lineStyle.GetWidth(m_Theme.serie.lineWidth), lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDotDot:
                        UGL.DrawDashDotDotLine(vh, lp, np, serie.lineStyle.GetWidth(m_Theme.serie.lineWidth), lineColor);
                        isFinish = true;
                        break;
                }
                if (isFinish) serie.animation.SetDataFinish(i);
                lp = np;
            }
            if (!serie.animation.IsFinish())
            {
                float total = totalDetailProgress - currDetailProgress - dataCount * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth) * 0.5f;
                serie.animation.CheckProgress(total);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, m_Theme.serie.lineSymbolSize));
                m_IsPlayingAnimation = true;
                RefreshPainter(serie);
            }
        }

        private double GetStackValue(List<List<SerieData>> stackDataList, int dataIndex, float dataChangeDuration, Axis xAxis)
        {
            double value = 0;
            foreach (var dataList in stackDataList)
            {
                value += dataList[dataIndex].GetCurrData(1, dataChangeDuration, xAxis.inverse, xAxis.runtimeMinValue, xAxis.runtimeMaxValue);
            }
            return value;
        }

        private Vector3 stPos1, stPos2, lastDir, lastDnPos;
        private bool lastIsDown;
        private bool DrawNormalLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp, Vector3 np, Vector3 nnp,
            int dataIndex, Color32 lineColor, Color32 areaColor, Color32 areaToColor,
            Vector3 zeroPos, int startIndex)
        {
            var defaultLineColor = lineColor;
            var isSecond = dataIndex == startIndex + 1;
            var isTheLastPos = np == nnp;
            bool isYAxis = axis is YAxis;
            var isTurnBack = IsInRightOrUp(isYAxis, np, lp);
            var lineWidth = serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
            var grid = GetSerieGridOrDefault(serie);

            Vector3 dnPos, upPos1, upPos2, dir1v, dir2v;
            bool isDown;
            var dir1 = (np - lp).normalized;
            dir1v = Vector3.Cross(dir1, Vector3.forward).normalized * (isYAxis ? -1 : 1);
            if (np != nnp)
            {
                var dir2 = (nnp - np).normalized;
                var dir3 = (dir1 + dir2).normalized;
                var normal = Vector3.Cross(dir1, dir2);
                isDown = isYAxis ? normal.z >= 0 : normal.z <= 0;
                var angle = (180 - Vector3.Angle(dir1, dir2)) * Mathf.Deg2Rad / 2;
                var diff = serie.lineStyle.GetWidth(m_Theme.serie.lineWidth) / Mathf.Sin(angle);
                var dirDp = Vector3.Cross(dir3, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                dir2v = Vector3.Cross(dir2, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                dnPos = np + (isDown ? dirDp : -dirDp) * diff;
                upPos1 = np + (isDown ? -dir1v : dir1v) * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
                upPos2 = np + (isDown ? -dir2v : dir2v) * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
                lastDir = dir1;
                if (isDown)
                {
                    if (isYAxis && dnPos.x < lp.x && dnPos.x < nnp.x) dnPos.x = lp.x;
                    if (!isYAxis && dnPos.y < lp.y && dnPos.y < nnp.y) dnPos.y = lp.y;
                }
                else
                {
                    if (isYAxis && dnPos.x > lp.x && dnPos.x > nnp.x) dnPos.x = lp.x;
                    if (!isYAxis && dnPos.y > lp.y && dnPos.y > nnp.y) dnPos.y = lp.y;
                }
            }
            else
            {
                isDown = Vector3.Cross(dir1, lastDir).z <= 0;
                if (isYAxis) isDown = !isDown;
                dir1v = Vector3.Cross(dir1, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                upPos1 = np - dir1v * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
                upPos2 = np + dir1v * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
                dnPos = isDown ? upPos2 : upPos1;

            }
            if (isSecond)
            {
                stPos1 = lp - dir1v * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
                stPos2 = lp + dir1v * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
            }
            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            var dist = Vector3.Distance(lp, np);
            var lastSmoothPoint = Vector3.zero;
            var lastSmoothDownPoint = Vector3.zero;
            int segment = (int)(dist / settings.lineSegmentDistance);
            if (segment <= 3) segment = (int)(dist / lineWidth);
            if (segment < 2) segment = 2;
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
            Vector3 ltp1 = stPos1, ltp2 = stPos2;
            bool isBreak = false;
            bool isStart = false;
            bool isShort = false;
            for (int i = 1; i < segment; i++)
            {
                var isEndPos = i == segment - 1;
                var cp = lp + dir1 * (dist * i / segment);
                if (serie.animation.CheckDetailBreak(cp, isYAxis)) isBreak = true;
                var tp1 = cp - dir1v * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
                var tp2 = cp + dir1v * serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
                CheckLineGradientColor(cp, serie.lineStyle, axis, defaultLineColor, ref lineColor);
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
                                Internal_CheckClipAndDrawPolygon(vh, stPos1, upPos1, upPos2, stPos2, lineColor, serie.clip, grid);
                                Internal_CheckClipAndDrawTriangle(vh, stPos2, upPos2, dnPos, lineColor, serie.clip, grid);
                                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, stPos1, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, upPos1, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, dnPos, isEndPos);
                            }
                            else if (isSecond || isTurnBack ||
                                (lastIsDown && IsInRightOrUp(isYAxis, lastDnPos, tp2)) ||
                                (!lastIsDown && IsInRightOrUp(isYAxis, lastDnPos, tp1)))
                            {
                                isStart = true;
                                if (stPos1 != Vector3.zero && stPos2 != Vector3.zero)
                                    Internal_CheckClipAndDrawPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor, serie.clip, grid);
                            }
                        }
                        else
                        {
                            if (isEndPos)
                            {
                                if (np != nnp)
                                {
                                    Internal_CheckClipAndDrawPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor, serie.clip, grid);
                                    Internal_CheckClipAndDrawTriangle(vh, upPos1, upPos2, dnPos, lineColor, serie.clip, grid);
                                }
                                else
                                {
                                    Internal_CheckClipAndDrawPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor, serie.clip, grid);
                                }
                            }
                            else
                            {
                                if (IsInRightOrUp(isYAxis, tp2, dnPos) || isTurnBack)
                                {
                                    Internal_CheckClipAndDrawLine(vh, start, cp, serie.lineStyle.GetWidth(m_Theme.serie.lineWidth), lineColor, serie.clip, grid);
                                }
                                else
                                {
                                    Internal_CheckClipAndDrawPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor, serie.clip, grid);
                                    Internal_CheckClipAndDrawTriangle(vh, upPos1, upPos2, dnPos, lineColor, serie.clip, grid);
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
                                    Internal_CheckClipAndDrawPolygon(vh, stPos1, dnPos, upPos2, stPos2, lineColor, serie.clip, grid);
                                else
                                {
                                    Internal_CheckClipAndDrawPolygon(vh, stPos1, dnPos, upPos1, stPos2, lineColor, serie.clip, grid);
                                    Internal_CheckClipAndDrawTriangle(vh, dnPos, upPos1, upPos2, lineColor, serie.clip, grid);
                                }
                                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, dnPos, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, stPos2, isEndPos);
                                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, upPos2, isEndPos);
                            }
                            else if (isSecond || isTurnBack ||
                                (lastIsDown && IsInRightOrUp(isYAxis, lastDnPos, tp2)) ||
                                (!lastIsDown && IsInRightOrUp(isYAxis, lastDnPos, tp1)))
                            {
                                isStart = true;
                                if (stPos2 != Vector3.zero)
                                {
                                    Internal_CheckClipAndDrawPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor, serie.clip, grid);
                                }
                            }
                        }
                        else
                        {
                            if (isEndPos)
                            {
                                if (np != nnp)
                                {
                                    Internal_CheckClipAndDrawPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor, serie.clip, grid);
                                    Internal_CheckClipAndDrawTriangle(vh, dnPos, upPos2, upPos1, lineColor, serie.clip, grid);
                                }
                                else Internal_CheckClipAndDrawPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor, serie.clip, grid);
                            }
                            else
                            {
                                if (IsInRightOrUp(isYAxis, tp1, dnPos) || isTurnBack)
                                {
                                    Internal_CheckClipAndDrawLine(vh, start, cp, serie.lineStyle.GetWidth(m_Theme.serie.lineWidth), lineColor, serie.clip, grid);
                                }
                                else
                                {
                                    Internal_CheckClipAndDrawPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor, serie.clip, grid);
                                    Internal_CheckClipAndDrawTriangle(vh, dnPos, upPos2, upPos1, lineColor, serie.clip, grid);
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
                var lastSerie = SeriesHelper.GetLastStackSerie(m_Series, serie);
                if (lastSerie != null)
                {
                    var lastSmoothPoints = lastSerie.GetUpSmoothList(dataIndex);
                    DrawStackArea(vh, serie, axis, smoothDownPoints, lastSmoothPoints, areaColor, areaToColor);
                }
                else
                {
                    var points = ((isYAxis && lp.x < zeroPos.x) || (!isYAxis && lp.y < zeroPos.y)) ? smoothPoints : smoothDownPoints;
                    Vector3 aep = isYAxis ? new Vector3(zeroPos.x, zeroPos.y + grid.runtimeHeight) : new Vector3(zeroPos.x + grid.runtimeWidth, zeroPos.y);
                    var sindex = 0;
                    var eindex = 0;
                    var sp = GetStartPos(points, ref sindex, serie.ignoreLineBreak);
                    var ep = GetEndPos(points, ref eindex, serie.ignoreLineBreak);
                    var cross = ChartHelper.GetIntersection(lp, np, zeroPos, aep);
                    if (cross == Vector3.zero || smoothDownPoints.Count <= 3)
                    {
                        sp = points[sindex];
                        for (int i = sindex + 1; i <= eindex; i++)
                        {
                            ep = points[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) break;
                            DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                            sp = ep;
                        }
                    }
                    else
                    {
                        var sp1 = smoothDownPoints[0];
                        var ep1 = smoothDownPoints[smoothDownPoints.Count - 1];
                        var axisLineWidth = axis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                        var axisUpStart = zeroPos + (isYAxis ? Vector3.right : Vector3.up) * axisLineWidth;
                        var axisUpEnd = axisUpStart + (isYAxis ? Vector3.up * grid.runtimeHeight : Vector3.right * grid.runtimeWidth);
                        var axisDownStart = zeroPos - (isYAxis ? Vector3.right : Vector3.up) * axisLineWidth;
                        var axisDownEnd = axisDownStart + (isYAxis ? Vector3.up * grid.runtimeHeight : Vector3.right * grid.runtimeWidth);
                        var luPos = ChartHelper.GetIntersection(sp1, ep1, axisUpStart, axisUpEnd);
                        sp1 = smoothPoints[0];
                        ep1 = smoothPoints[smoothPoints.Count - 2];
                        var rdPos = ChartHelper.GetIntersection(sp1, ep1, axisDownStart, axisDownEnd);
                        if ((isYAxis && lp.x >= zeroPos.x) || (!isYAxis && lp.y >= zeroPos.y))
                        {
                            sp = smoothDownPoints[0];
                            for (int i = 1; i < smoothDownPoints.Count; i++)
                            {
                                ep = smoothDownPoints[i];
                                if (serie.animation.CheckDetailBreak(ep, isYAxis)) break;
                                if (luPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }

                                if ((isYAxis && ep.y > luPos.y) || (!isYAxis && ep.x > luPos.x))
                                {
                                    var tp = isYAxis ? new Vector3(luPos.x, sp.y) : new Vector3(sp.x, luPos.y);
                                    Internal_CheckClipAndDrawTriangle(vh, sp, luPos, tp, areaColor, areaToColor, areaToColor, serie.clip, grid);
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
                                if (serie.animation.CheckDetailBreak(ep, isYAxis)) break;
                                if ((isYAxis && ep.y <= rdPos.y) || (!isYAxis && ep.x <= rdPos.x)) continue;
                                if (rdPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }
                                if (!first)
                                {
                                    first = true;
                                    var tp = isYAxis ? new Vector3(rdPos.x, ep.y) : new Vector3(ep.x, rdPos.y);
                                    Internal_CheckClipAndDrawTriangle(vh, rdPos, tp, ep, areaToColor, areaToColor, areaColor, serie.clip, grid);
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
                                if (serie.animation.CheckDetailBreak(ep, isYAxis)) break;
                                if (rdPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }

                                if ((isYAxis && ep.y > rdPos.y) || (!isYAxis && ep.x > rdPos.x))
                                {
                                    var tp = isYAxis ? new Vector3(rdPos.x, sp.y) : new Vector3(sp.x, rdPos.y);
                                    Internal_CheckClipAndDrawTriangle(vh, sp, rdPos, tp, areaColor, areaToColor, areaToColor, serie.clip, grid);
                                    break;
                                }
                                if (rdPos != Vector3.zero) DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                                sp = ep;
                            }
                            sp = smoothDownPoints[0];
                            bool first = false;
                            for (int i = 1; i < smoothDownPoints.Count; i++)
                            {
                                ep = smoothDownPoints[i];
                                if (serie.animation.CheckDetailBreak(ep, isYAxis)) break;
                                if ((isYAxis && ep.y < luPos.y) || (!isYAxis && ep.x < luPos.x)) continue;
                                if (luPos == Vector3.zero)
                                {
                                    sp = ep;
                                    continue;
                                }
                                if (!first)
                                {
                                    first = true;
                                    var tp = isYAxis ? new Vector3(luPos.x, ep.y) : new Vector3(ep.x, luPos.y);
                                    Internal_CheckClipAndDrawTriangle(vh, ep, luPos, tp, areaColor, areaToColor, areaToColor, serie.clip, grid);
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

        private bool TryAddToList(bool isTurnBack, bool isYAxis, List<Vector3> list, Vector3 lastPos, Vector3 pos, bool ignoreClose = false)
        {
            if (ChartHelper.IsZeroVector(pos)) return false;
            if (isTurnBack)
            {
                list.Add(pos);
                return true;
            }
            else if (!ChartHelper.IsZeroVector(lastPos) && IsInRightOrUpNotCheckZero(isYAxis, pos, lastPos))
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
                if (IsInRightOrUpNotCheckZero(isYAxis, end, pos) && (!ignoreClose || !WasTooClose(isYAxis, end, pos, ignoreClose)))
                {
                    list.Add(pos);
                    return true;
                }
            }
            return false;
        }

        private void CheckLineGradientColor(Vector3 cp, LineStyle lineStyle, Axis axis, Color32 defaultLineColor, ref Color32 lineColor)
        {
            if (VisualMapHelper.IsNeedGradient(visualMap))
                lineColor = VisualMapHelper.GetLineGradientColor(visualMap, cp, this, axis, defaultLineColor);
            else if (lineStyle.IsNeedGradient())
                lineColor = VisualMapHelper.GetLineStyleGradientColor(lineStyle, cp, this, axis, defaultLineColor);
        }

        private bool IsInRightOrUp(bool isYAxis, Vector3 lp, Vector3 rp)
        {
            return ChartHelper.IsZeroVector(lp) || ((isYAxis && rp.y > lp.y) || (!isYAxis && rp.x > lp.x));
        }

        private bool IsInRightOrUpNotCheckZero(bool isYAxis, Vector3 lp, Vector3 rp)
        {
            return (isYAxis && rp.y > lp.y) || (!isYAxis && rp.x > lp.x);
        }

        private bool WasTooClose(bool isYAxis, Vector3 lp, Vector3 rp, bool ignore)
        {
            if (ignore) return false;
            if (lp == Vector3.zero || rp == Vector3.zero) return false;
            if (isYAxis) return Mathf.Abs(rp.y - lp.y) < 1f;
            else return Mathf.Abs(rp.x - lp.x) < 1f;
        }

        private void DrawPolygonToZero(VertexHelper vh, Vector3 sp, Vector3 ep, Axis axis, Vector3 zeroPos,
            Color32 areaColor, Color32 areaToColor, Vector3 areaDiff, bool clip = false)
        {
            float diff = 0;
            var grid = GetAxisGridOrDefault(axis);
            var lineWidth = axis.axisLine.GetWidth(m_Theme.axis.lineWidth);
            if (axis is YAxis)
            {
                var isLessthan0 = (sp.x < zeroPos.x || ep.x < zeroPos.x);
                diff = isLessthan0 ? -lineWidth : lineWidth;
                areaColor = GetYLerpColor(areaColor, areaToColor, sp, grid);
                if (isLessthan0) areaDiff = -areaDiff;
                Internal_CheckClipAndDrawPolygon(vh, new Vector3(zeroPos.x + diff, sp.y), new Vector3(zeroPos.x + diff, ep.y),
                    ep + areaDiff, sp + areaDiff, areaToColor, areaColor, clip, grid);
            }
            else
            {
                var isLessthan0 = (sp.y < zeroPos.y || ep.y < zeroPos.y);
                diff = isLessthan0 ? -lineWidth : lineWidth;
                areaColor = GetXLerpColor(areaColor, areaToColor, sp, grid);
                if (isLessthan0) areaDiff = -areaDiff;
                if (isLessthan0)
                {
                    Internal_CheckClipAndDrawPolygon(vh, ep + areaDiff, sp + areaDiff, new Vector3(sp.x, zeroPos.y + diff),
                        new Vector3(ep.x, zeroPos.y + diff), areaColor, areaToColor, clip, grid);
                }
                else
                {
                    Internal_CheckClipAndDrawPolygon(vh, sp + areaDiff, ep + areaDiff, new Vector3(ep.x, zeroPos.y + diff),
                         new Vector3(sp.x, zeroPos.y + diff), areaColor, areaToColor, clip, grid);
                }
            }
        }

        private List<Vector3> posList = new List<Vector3>();
        private bool DrawOtherLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp,
            Vector3 np, int dataIndex, Color32 lineColor, Color32 areaColor,
            Color32 areaToColor, Vector3 zeroPos)
        {
            //lp = ClampInChart(lp);
            //np = ClampInChart(np);
            bool isYAxis = axis is YAxis;
            var lineWidth = serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
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
        private bool DrawSmoothLine(VertexHelper vh, Serie serie, Axis xAxis, Vector3 lp,
            Vector3 np, Vector3 llp, Vector3 nnp, int dataIndex, Color32 lineColor, Color32 areaColor,
            Color32 areaToColor, bool isStack, Vector3 zeroPos, int startIndex = 0)
        {
            var defaultLineColor = lineColor;
            bool isYAxis = xAxis is YAxis;
            var isTurnBack = IsInRightOrUp(isYAxis, np, lp);
            var lineWidth = serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            var lastSmoothPoint = Vector3.zero;
            var lastSmoothDownPoint = Vector3.zero;
            var grid = GetSerieGridOrDefault(serie);
            if (dataIndex > startIndex)
            {
                lastSmoothPoint = ChartHelper.GetLastPoint(serie.GetUpSmoothList(dataIndex - 1));
                lastSmoothDownPoint = ChartHelper.GetLastPoint(serie.GetDownSmoothList(dataIndex - 1));
                TryAddToList(isTurnBack, isYAxis, smoothPoints, lastSmoothPoint, lastSmoothPoint, true);
                TryAddToList(isTurnBack, isYAxis, smoothDownPoints, lastSmoothDownPoint, lastSmoothDownPoint, true);
            }
            if (isYAxis) ChartHelper.GetBezierListVertical(ref bezierPoints, lp, np, settings.lineSmoothness, settings.lineSmoothStyle);
            else ChartHelper.GetBezierList(ref bezierPoints, lp, np, llp, nnp, settings.lineSmoothness, settings.lineSmoothStyle);

            Vector3 start, to;
            if (serie.lineType == LineType.SmoothDash)
            {
                for (int i = 0; i < bezierPoints.Count - 2; i += 2)
                {
                    start = bezierPoints[i];
                    to = bezierPoints[i + 1];
                    CheckLineGradientColor(start, serie.lineStyle, xAxis, defaultLineColor, ref lineColor);
                    Internal_CheckClipAndDrawLine(vh, start, to, lineWidth, lineColor, serie.clip, grid);
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
                        CheckLineGradientColor(lp, serie.lineStyle, xAxis, defaultLineColor, ref lineColor);
                        Internal_CheckClipAndDrawTriangle(vh, smoothStartPosUp, startUp, lp, lineColor, serie.clip, grid);
                        Internal_CheckClipAndDrawTriangle(vh, smoothStartPosDn, startDn, lp, lineColor, serie.clip, grid);
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
                CheckLineGradientColor(to, serie.lineStyle, xAxis, defaultLineColor, ref lineColor);
                if (isYAxis) Internal_CheckClipAndDrawPolygon(vh, startDn, toDn, toUp, startUp, lineColor, serie.clip, grid);
                else Internal_CheckClipAndDrawPolygon(vh, startUp, toUp, toDn, startDn, lineColor, serie.clip, grid);
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
                        if (IsInRightOrUp(!isYAxis, zeroPos, to))
                        {
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
                        if (!IsInRightOrUp(!isYAxis, zeroPos, to))
                        {
                            DrawPolygonToZero(vh, to, start, xAxis, zeroPos, areaColor, areaToColor, Vector3.zero);
                        }
                        start = to;
                    }
                }
            }

            if (serie.areaStyle.show)
            {
                var lastSerie = SeriesHelper.GetLastStackSerie(m_Series, serie);
                if (lastSerie != null)
                {
                    var lastSmoothPoints = lastSerie.GetUpSmoothList(dataIndex);
                    DrawStackArea(vh, serie, xAxis, smoothDownPoints, lastSmoothPoints, areaColor, areaToColor);
                }
            }
            return isFinish;
        }

        private bool IsAllLessthen0(bool isYAxis, Vector3 zeroPos, Vector3 start, Vector3 to)
        {
            if (isYAxis) return start.x < zeroPos.x && to.x < zeroPos.x;
            else return start.y < zeroPos.y && to.y < zeroPos.y;
        }

        private Vector3 GetLastSmoothPos(List<Vector3> list, bool isYAxis)
        {
            var count = list.Count;
            if (count <= 0) return Vector3.zero;
            var pos = list[count - 1];
            for (int i = count - 2; i > count - 4 && i > 0; i--)
            {
                if (isYAxis)
                {
                    if (list[i].y > pos.y) pos = list[i];
                }
                else
                {
                    if (list[i].x > pos.x) pos = list[i];
                }
            }
            return pos;
        }

        private void DrawStackArea(VertexHelper vh, Serie serie, Axis axis, List<Vector3> smoothPoints,
            List<Vector3> lastSmoothPoints, Color32 areaColor, Color32 areaToColor)
        {
            if (!serie.areaStyle.show || lastSmoothPoints.Count <= 0) return;
            Vector3 start, to;
            var isYAxis = axis is YAxis;

            var lastCount = 1;
            start = smoothPoints[0];
            var sourAreaColor = areaColor;
            var grid = GetAxisGridOrDefault(axis);
            for (int k = 1; k < smoothPoints.Count; k++)
            {
                to = smoothPoints[k];
                if (!IsInRightOrUp(isYAxis, start, to)) continue;
                if (serie.animation.CheckDetailBreak(to, isYAxis)) break;
                Vector3 tnp, tlp;
                if (isYAxis) areaColor = GetYLerpColor(sourAreaColor, areaToColor, to, grid);
                else areaColor = GetXLerpColor(sourAreaColor, areaToColor, to, grid);
                if (k == smoothPoints.Count - 1)
                {
                    if (k < lastSmoothPoints.Count - 1)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        Internal_CheckClipAndDrawTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor, serie.clip, grid);
                        while (lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                            Internal_CheckClipAndDrawTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor, serie.clip, grid);
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
                    if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                    Internal_CheckClipAndDrawTriangle(vh, to, start, tlp, areaColor, areaColor, areaToColor, serie.clip, grid);
                    start = to;
                    continue;
                }
                tnp = lastSmoothPoints[lastCount];
                var diff = isYAxis ? tnp.y - to.y : tnp.x - to.x;
                if (Math.Abs(diff) < 1)
                {
                    tlp = lastSmoothPoints[lastCount - 1];
                    if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                    Internal_CheckClipAndDrawPolygon(vh, start, to, tnp, tlp, areaColor, areaToColor, serie.clip, grid);
                    lastCount++;
                }
                else
                {
                    if (diff < 0)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        Internal_CheckClipAndDrawTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor, serie.clip, grid);
                        while (diff < 0 && lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                            Internal_CheckClipAndDrawTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor, serie.clip, grid);
                            lastCount++;
                            diff = isYAxis ? tlp.y - to.y : tlp.x - to.x;
                            tnp = tlp;
                        }
                    }
                    else
                    {
                        tlp = lastSmoothPoints[lastCount - 1];
                        if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                        Internal_CheckClipAndDrawTriangle(vh, start, to, tlp, areaColor, areaColor, areaToColor, serie.clip, grid);
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
                    Internal_CheckClipAndDrawTriangle(vh, p1, start, p2, areaToColor, areaColor, areaToColor, serie.clip, grid);
                }
            }
        }

        private List<Vector3> linePointList = new List<Vector3>();
        private bool DrawStepLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp, Vector3 np,
            Vector3 nnp, int dataIndex, Color32 lineColor, Color32 areaColor, Color32 areaToColor, Vector3 zeroPos)
        {
            bool isYAxis = axis is YAxis;
            float lineWidth = serie.lineStyle.GetWidth(m_Theme.serie.lineWidth);
            Vector3 start, end, middle, middleZero, middle1, middle2;
            Vector3 sp, ep, diff1, diff2;
            var areaDiff = isYAxis ? Vector3.left * lineWidth : Vector3.down * lineWidth;
            var grid = GetAxisGridOrDefault(axis);
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
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            Internal_CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            sp = ep;
                        }
                        Internal_CheckClipAndDrawPolygon(vh, middle, lineWidth, lineColor, serie.clip, true, grid);
                    }
                    else
                    {
                        if (dataIndex == 1) Internal_CheckClipAndDrawPolygon(vh, lp, lineWidth, lineColor, serie.clip, true, grid);
                        Internal_CheckClipAndDrawLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor, serie.clip, grid);
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
                        if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                        Internal_CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                        sp = ep;
                    }

                    if (nnp != np)
                    {
                        if (serie.animation.CheckDetailBreak(np, isYAxis)) return false;
                        Internal_CheckClipAndDrawPolygon(vh, np, lineWidth, lineColor, serie.clip, true, grid);
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
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            Internal_CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(middle1, isYAxis)) return false;
                        Internal_CheckClipAndDrawPolygon(vh, middle1, lineWidth, lineColor, serie.clip, true, grid);
                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle1, middle2 - middle1) <= 0)
                        {
                            DrawPolygonToZero(vh, middle1 - diff1, middle1 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1) Internal_CheckClipAndDrawPolygon(vh, lp, lineWidth, lineColor, serie.clip, true, grid);
                        Internal_CheckClipAndDrawLine(vh, lp + diff1, middle1 + diff1, lineWidth, lineColor, serie.clip, grid);
                    }

                    //draw middle1 to middle2
                    if (Vector3.Distance(middle1, middle2) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle1 + diff2, middle2 - diff2, settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            Internal_CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            sp = ep;
                        }
                        Internal_CheckClipAndDrawPolygon(vh, middle2, lineWidth, lineColor, serie.clip, true, grid);
                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle2, middle2 - middle1) >= 0)
                        {
                            DrawPolygonToZero(vh, middle2 - diff1, middle2 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        Internal_CheckClipAndDrawLine(vh, middle1 + diff2, middle2 + diff2, lineWidth, lineColor, serie.clip, grid);
                    }
                    //draw middle2 to np
                    if (Vector3.Distance(middle2, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle2 + diff1, np - diff1, settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            Internal_CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(np, isYAxis)) return false;
                        Internal_CheckClipAndDrawPolygon(vh, np, lineWidth, lineColor, serie.clip, true, grid);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, np - diff1, np + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        Internal_CheckClipAndDrawLine(vh, middle1 + diff1, middle1 + diff1, lineWidth, lineColor, serie.clip, grid);
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
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            Internal_CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(middle, isYAxis)) return false;
                        Internal_CheckClipAndDrawPolygon(vh, middle, lineWidth, lineColor, serie.clip, true, grid);
                        if (serie.areaStyle.show && Vector3.Dot(np - middle, middleZero - middle) <= 0)
                        {
                            DrawPolygonToZero(vh, middle - diff1, middle + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1) Internal_CheckClipAndDrawPolygon(vh, lp, lineWidth, lineColor, serie.clip, true, grid);
                        Internal_CheckClipAndDrawLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor, serie.clip, grid);
                    }

                    if (Vector3.Distance(middle, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle + diff2, end, settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            Internal_CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip, grid);
                            sp = ep;
                        }
                        if (nnp != np) Internal_CheckClipAndDrawPolygon(vh, np, lineWidth, lineColor, serie.clip, true, grid);
                    }
                    else
                    {
                        Internal_CheckClipAndDrawLine(vh, middle + diff2, np + diff2, lineWidth, lineColor, serie.clip, grid);
                    }
                    bool flag2 = ((isYAxis && middle.x > np.x && np.x > zeroPos.x) || (!isYAxis && middle.y > np.y && np.y > zeroPos.y));
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