/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public partial class CoordinateChart
    {
        protected void DrawLinePoint(VertexHelper vh)
        {
            var clip = SeriesHelper.IsAnyClipSerie(m_Series);
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (!serie.show || serie.IsPerformanceMode()) continue;
                if (serie.type != SerieType.Line) continue;
                var count = serie.dataPoints.Count;
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
                    bool highlight = (m_Tooltip.show && m_Tooltip.IsSelected(i))
                        || serie.data[i].highlighted || serie.highlighted;
                    float symbolSize = highlight ? symbol.selectedSize : symbol.size;
                    var symbolColor = SerieHelper.GetItemColor(serie, serieData, m_ThemeInfo, n, highlight);
                    var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, m_ThemeInfo, n, highlight);
                    var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, highlight);
                    var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                    CheckClipAndDrawSymbol(vh, symbol.type, symbolSize, symbolBorder, serie.dataPoints[i], symbolColor,
                        symbolToColor, symbol.gap, clip, cornerRadius);
                }
            }
        }

        protected void DrawLineArrow(VertexHelper vh)
        {
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (serie.type != SerieType.Line) continue;
                if (!serie.show || !serie.lineArrow.show) continue;
                if (serie.dataPoints.Count < 2) return;
                Color lineColor = SerieHelper.GetLineColor(serie, m_ThemeInfo, n, false);
                Vector3 startPos, arrowPos;
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
                        ChartDrawer.DrawArrow(vh, startPos, arrowPos, serie.lineArrow.width,
                             serie.lineArrow.height, serie.lineArrow.offset, serie.lineArrow.dent, lineColor);
                        break;
                    case LineArrow.Position.Start:
                        dataPoints = serie.GetUpSmoothList(1);
                        if (dataPoints.Count < 2) dataPoints = serie.dataPoints;
                        startPos = dataPoints[1];
                        arrowPos = dataPoints[0];
                        ChartDrawer.DrawArrow(vh, startPos, arrowPos, serie.lineArrow.width,
                            serie.lineArrow.height, serie.lineArrow.offset, serie.lineArrow.dent, lineColor);
                        break;
                }
            }
        }

        protected void DrawXLineSerie(VertexHelper vh, Serie serie, int colorIndex, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            if (serie.animation.HasFadeOut()) return;
            var showData = serie.GetDataList(m_DataZoom);
            if (showData.Count <= 0) return;
            Color lineColor = SerieHelper.GetLineColor(serie, m_ThemeInfo, colorIndex, serie.highlighted);
            Color srcAreaColor = SerieHelper.GetAreaColor(serie, m_ThemeInfo, colorIndex, false);
            Color srcAreaToColor = SerieHelper.GetAreaToColor(serie, m_ThemeInfo, colorIndex, false);
            Color highlightAreaColor = SerieHelper.GetAreaColor(serie, m_ThemeInfo, colorIndex, true);
            Color highlightAreaToColor = SerieHelper.GetAreaToColor(serie, m_ThemeInfo, colorIndex, true);
            Color areaColor, areaToColor;
            Vector3 lp = Vector3.zero, np = Vector3.zero, llp = Vector3.zero, nnp = Vector3.zero;
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            var zeroPos = new Vector3(m_CoordinateX, m_CoordinateY + yAxis.runtimeZeroYOffset);
            var isStack = SeriesHelper.IsStack(m_Series, serie.stack, SerieType.Line);
            if (!xAxis.show) xAxis = m_XAxises[(serie.axisIndex + 1) % m_XAxises.Count];
            float scaleWid = AxisHelper.GetDataWidth(xAxis, m_CoordinateWidth, showData.Count, m_DataZoom);
            float startX = m_CoordinateX + (xAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            int i;
            if (seriesHig.Count < serie.minShow)
            {
                for (i = 0; i < serie.minShow; i++)
                {
                    seriesHig.Add(0);
                }
            }
            int rate = 1;
            var sampleDist = serie.sampleDist;
            if (sampleDist > 0) rate = (int)((maxCount - serie.minShow) / (m_CoordinateWidth / sampleDist));
            if (rate < 1) rate = 1;
            var includeLastData = false;
            var totalAverage = serie.sampleAverage > 0 ? serie.sampleAverage :
                DataAverage(ref showData, serie.sampleType, serie.minShow, maxCount, rate);
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            for (i = serie.minShow; i < maxCount; i += rate)
            {
                if (i == maxCount - 1) includeLastData = true;
                if (i >= seriesHig.Count)
                {
                    for (int j = 0; j < rate; j++) seriesHig.Add(0);
                }
                if (serie.IsIgnoreValue(showData[i].GetData(1)))
                {
                    serie.dataPoints.Add(Vector3.zero);
                }
                else
                {
                    float yValue = SampleValue(ref showData, serie.sampleType, rate, serie.minShow, maxCount, totalAverage,
                        i, dataChangeDuration, ref dataChanging, yAxis.inverse);
                    seriesHig[i] += GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, seriesHig[i], ref np,
                        dataChangeDuration);
                    serie.dataPoints.Add(np);
                }
            }
            if (dataChanging)
            {
                RefreshChart();
            }
            if (!includeLastData)
            {
                i = maxCount - 1;
                seriesHig.Add(0);
                if (serie.IsIgnoreValue(showData[i].GetData(1)))
                {
                    serie.dataPoints.Add(Vector3.zero);
                }
                else
                {
                    float yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse);
                    seriesHig[i] += GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, seriesHig[i], ref np,
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
            var startPos = GetStartPos(serie.dataPoints, ref startIndex);
            var endPos = GetEndPos(serie.dataPoints, ref endIndex);
            lp = startPos;
            stPos1 = stPos2 = lastDir = lastDnPos = Vector3.zero;
            smoothStartPosUp = smoothStartPosDn = Vector3.zero;
            float currDetailProgress = lp.x;
            float totalDetailProgress = endPos.x;
            serie.animation.InitProgress(serie.dataPoints.Count, currDetailProgress, totalDetailProgress);
            serie.animation.SetDataFinish(startIndex);

            Vector3 firstLastPos = Vector3.zero, lastNextPos = Vector3.zero;
            if (serie.minShow > 0 && serie.minShow < showData.Count)
            {
                i = serie.minShow - 1;
                if (serie.IsIgnoreValue(showData[i].GetData(1)))
                {
                    serie.dataPoints.Add(Vector3.zero);
                }
                else
                {
                    float yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse);
                    GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, 0, ref firstLastPos, dataChangeDuration);
                }
            }
            else
            {
                firstLastPos = lp;
            }
            if (serie.maxShow > 0 && serie.maxShow < showData.Count)
            {
                i = serie.maxShow;
                if (serie.IsIgnoreValue(showData[i].GetData(1)))
                {
                    serie.dataPoints.Add(Vector3.zero);
                }
                else
                {
                    float yValue = showData[i].GetCurrData(1, dataChangeDuration, yAxis.inverse);
                    GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, 0, ref lastNextPos, dataChangeDuration);
                }
            }
            else
            {
                lastNextPos = endPos;
            }
            for (i = startIndex + 1; i < serie.dataPoints.Count; i++)
            {
                np = serie.dataPoints[i];
                serie.ClearSmoothList(i);
                if (np == Vector3.zero)
                {
                    serie.animation.SetDataFinish(i);
                    continue;
                }
                if (!serie.animation.NeedAnimation(i)) break;
                bool isFinish = true;
                if (serie.areaStyle.tooltipHighlight && m_Tooltip.show && i <= m_Tooltip.runtimeDataIndex[0])
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
                        lp = GetLastPos(serie.dataPoints, i, np);
                        nnp = GetNNPos(serie.dataPoints, i, np);
                        isFinish = DrawNormalLine(vh, serie, xAxis, lp, np, nnp, i, lineColor,
                            areaColor, areaToColor, zeroPos, startIndex);
                        break;
                    case LineType.Smooth:
                    case LineType.SmoothDash:
                        llp = GetLLPos(serie.dataPoints, i, firstLastPos);
                        nnp = GetNNPos(serie.dataPoints, i, lastNextPos);
                        isFinish = DrawSmoothLine(vh, serie, xAxis, lp, np, llp, nnp, i,
                            lineColor, areaColor, areaToColor, isStack, zeroPos, startIndex);
                        break;
                    case LineType.StepStart:
                    case LineType.StepMiddle:
                    case LineType.StepEnd:
                        nnp = GetNNPos(serie.dataPoints, i, np);
                        isFinish = DrawStepLine(vh, serie, xAxis, lp, np, nnp, i, lineColor,
                            areaColor, areaToColor, zeroPos);
                        break;
                    case LineType.Dash:
                    case LineType.Dot:
                    case LineType.DashDot:
                    case LineType.DashDotDot:
                        DrawOtherLine(vh, serie, xAxis, lp, np, i, lineColor, areaColor, areaToColor, zeroPos);
                        break;
                }
                if (isFinish) serie.animation.SetDataFinish(i);
                if (np != Vector3.zero) lp = np;
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(totalDetailProgress);
                serie.animation.CheckSymbol(serie.symbol.size);
                m_IsPlayingAnimation = true;
                RefreshChart();
            }
        }

        private Vector3 GetNNPos(List<Vector3> dataPoints, int index, Vector3 np)
        {
            int size = dataPoints.Count;
            if (index >= size) return np;
            for (int i = index + 1; i < size; i++)
            {
                if (dataPoints[i] != Vector3.zero) return dataPoints[i];
            }
            return np;
        }

        private Vector3 GetStartPos(List<Vector3> dataPoints, ref int start)
        {
            for (int i = 0; i < dataPoints.Count; i++)
            {
                if (dataPoints[i] != Vector3.zero)
                {
                    start = i;
                    return dataPoints[i];
                }
            }
            return Vector3.zero;
        }

        private Vector3 GetEndPos(List<Vector3> dataPoints, ref int end)
        {
            for (int i = dataPoints.Count - 1; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero)
                {
                    end = i;
                    return dataPoints[i];
                }
            }
            return Vector3.zero;
        }

        private Vector3 GetLastPos(List<Vector3> dataPoints, int index, Vector3 pos)
        {
            if (index <= 0) return pos;
            for (int i = index - 1; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero) return dataPoints[i];
            }
            return pos;
        }

        private Vector3 GetLLPos(List<Vector3> dataPoints, int index, Vector3 lp)
        {
            if (index <= 1) return lp;
            for (int i = index - 2; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero) return dataPoints[i];
            }
            return lp;
        }

        private float DataAverage(ref List<SerieData> showData, SampleType sampleType, int minCount, int maxCount, int rate)
        {
            var totalAverage = 0f;
            if (rate > 1 && sampleType == SampleType.Peak)
            {
                var total = 0f;
                for (int i = minCount; i < maxCount; i++)
                {
                    total += showData[i].data[1];
                }
                totalAverage = total / (maxCount - minCount);
            }
            return totalAverage;
        }

        private float SampleValue(ref List<SerieData> showData, SampleType sampleType, int rate,
            int minCount, int maxCount, float totalAverage, int index, float dataChangeDuration,
            ref bool dataChanging, bool inverse)
        {
            if (rate <= 1 || index == minCount)
            {
                if (showData[index].IsDataChanged()) dataChanging = true;
                return showData[index].GetCurrData(1, dataChangeDuration, inverse);
            }
            switch (sampleType)
            {
                case SampleType.Sum:
                case SampleType.Average:
                    float total = 0;
                    for (int i = index; i > index - rate; i--)
                    {
                        total += showData[i].GetCurrData(1, dataChangeDuration, inverse);
                        if (showData[i].IsDataChanged()) dataChanging = true;
                    }
                    if (sampleType == SampleType.Average) return total / rate;
                    else return total;
                case SampleType.Max:
                    float max = float.MinValue;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataChangeDuration, inverse);
                        if (value > max) max = value;
                        if (showData[i].IsDataChanged()) dataChanging = true;
                    }
                    return max;
                case SampleType.Min:
                    float min = float.MaxValue;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataChangeDuration, inverse);
                        if (value < min) min = value;
                        if (showData[i].IsDataChanged()) dataChanging = true;
                    }
                    return min;
                case SampleType.Peak:
                    max = float.MinValue;
                    min = float.MaxValue;
                    total = 0;
                    for (int i = index; i > index - rate; i--)
                    {
                        var value = showData[i].GetCurrData(1, dataChangeDuration, inverse);
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
            return showData[index].GetCurrData(1, dataChangeDuration, inverse);
        }

        private float GetDataPoint(Axis xAxis, Axis yAxis, List<SerieData> showData, float yValue, float startX, int i,
            float scaleWid, float serieHig, ref Vector3 np, float duration, bool isIngoreValue = false)
        {
            if (isIngoreValue)
            {
                np = Vector3.zero;
                return 0;
            }
            float xDataHig, yDataHig;
            float xMinValue = xAxis.GetCurrMinValue(duration);
            float xMaxValue = xAxis.GetCurrMaxValue(duration);
            float yMinValue = yAxis.GetCurrMinValue(duration);
            float yMaxValue = yAxis.GetCurrMaxValue(duration);
            if (xAxis.IsValue() || xAxis.IsLog())
            {
                float xValue = i > showData.Count - 1 ? 0 : showData[i].GetData(0, xAxis.inverse);
                float pX = m_CoordinateX + xAxis.axisLine.width;
                float pY = serieHig + m_CoordinateY + xAxis.axisLine.width;
                if (xAxis.IsLog())
                {
                    int minIndex = xAxis.runtimeMinLogIndex;
                    float nowIndex = xAxis.GetLogValue(xValue);
                    xDataHig = (nowIndex - minIndex) / (xAxis.splitNumber - 1) * m_CoordinateWidth;
                }
                else
                {
                    if ((xMaxValue - xMinValue) <= 0) xDataHig = 0;
                    else xDataHig = (xValue - xMinValue) / (xMaxValue - xMinValue) * m_CoordinateWidth;
                }
                if (yAxis.IsLog())
                {
                    int minIndex = yAxis.runtimeMinLogIndex;
                    float nowIndex = yAxis.GetLogValue(yValue);
                    yDataHig = (nowIndex - minIndex) / (yAxis.splitNumber - 1) * m_CoordinateHeight;
                }
                else
                {
                    if ((yMaxValue - yMinValue) <= 0) yDataHig = 0;
                    else yDataHig = (yValue - yMinValue) / (yMaxValue - yMinValue) * m_CoordinateHeight;
                }
                np = new Vector3(pX + xDataHig, pY + yDataHig);
            }
            else
            {
                float pX = startX + i * scaleWid;
                float pY = serieHig + m_CoordinateY + yAxis.axisLine.width;
                if (yAxis.IsLog())
                {
                    int minIndex = yAxis.runtimeMinLogIndex;
                    float nowIndex = yAxis.GetLogValue(yValue);
                    yDataHig = (nowIndex - minIndex) / (yAxis.splitNumber - 1) * m_CoordinateHeight;
                }
                else
                {
                    if ((yMaxValue - yMinValue) <= 0) yDataHig = 0;
                    else yDataHig = (yValue - yMinValue) / (yMaxValue - yMinValue) * m_CoordinateHeight;
                }
                np = new Vector3(pX, pY + yDataHig);
            }
            return yDataHig;
        }

        protected void DrawYLineSerie(VertexHelper vh, Serie serie, int colorIndex, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            if (serie.animation.HasFadeOut()) return;
            var showData = serie.GetDataList(m_DataZoom);
            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            Vector3 llp = Vector3.zero;
            Vector3 nnp = Vector3.zero;
            Color lineColor = SerieHelper.GetLineColor(serie, m_ThemeInfo, colorIndex, serie.highlighted);
            Color srcAreaColor = SerieHelper.GetAreaColor(serie, m_ThemeInfo, colorIndex, false);
            Color srcAreaToColor = SerieHelper.GetAreaToColor(serie, m_ThemeInfo, colorIndex, false);
            Color highlightAreaColor = SerieHelper.GetAreaColor(serie, m_ThemeInfo, colorIndex, true);
            Color highlightAreaToColor = SerieHelper.GetAreaToColor(serie, m_ThemeInfo, colorIndex, true);
            Color areaColor, areaToColor;
            var xAxis = m_XAxises[serie.axisIndex];
            var yAxis = m_YAxises[serie.axisIndex];
            var zeroPos = new Vector3(m_CoordinateX + xAxis.runtimeZeroXOffset, m_CoordinateY);
            var isStack = SeriesHelper.IsStack(m_Series, serie.stack, SerieType.Line);
            if (!yAxis.show) yAxis = m_YAxises[(serie.axisIndex + 1) % m_YAxises.Count];
            float scaleWid = AxisHelper.GetDataWidth(yAxis, m_CoordinateHeight, showData.Count, m_DataZoom);
            float startY = m_CoordinateY + (yAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            int i = 0;
            if (seriesHig.Count < serie.minShow)
            {
                for (i = 0; i < serie.minShow; i++)
                {
                    seriesHig.Add(0);
                }
            }
            int rate = 1;
            var sampleDist = serie.sampleDist;
            if (sampleDist > 0) rate = (int)((maxCount - serie.minShow) / (m_CoordinateWidth / sampleDist));
            if (rate < 1) rate = 1;
            var dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            float xMinValue = xAxis.GetCurrMinValue(dataChangeDuration);
            float xMaxValue = xAxis.GetCurrMaxValue(dataChangeDuration);
            for (i = serie.minShow; i < maxCount; i += rate)
            {
                if (i >= seriesHig.Count)
                {
                    for (int j = 0; j < rate; j++) seriesHig.Add(0);
                }
                float value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse);
                float pY = startY + i * scaleWid;
                float pX = seriesHig[i] + m_CoordinateX + yAxis.axisLine.width;
                float dataHig = 0;
                if (xAxis.IsLog())
                {
                    int minIndex = xAxis.runtimeMinLogIndex;
                    float nowIndex = xAxis.GetLogValue(value);
                    dataHig = (nowIndex - minIndex) / (xAxis.splitNumber - 1) * m_CoordinateWidth;
                }
                else
                {
                    dataHig = (value - xMinValue) / (xMaxValue - xMinValue) * m_CoordinateWidth;
                }
                np = new Vector3(pX + dataHig, pY);
                serie.dataPoints.Add(np);
                seriesHig[i] += dataHig;
                if (showData[i].IsDataChanged()) dataChanging = true;
            }
            if (dataChanging)
            {
                RefreshChart();
            }
            if (maxCount % rate != 0)
            {
                i = maxCount - 1;
                seriesHig.Add(0);
                float value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse);
                float pY = startY + i * scaleWid;
                float pX = seriesHig[i] + m_CoordinateX + yAxis.axisLine.width;
                float dataHig = 0;
                if (xAxis.IsLog())
                {
                    int minIndex = xAxis.runtimeMinLogIndex;
                    float nowIndex = xAxis.GetLogValue(value);
                    dataHig = (nowIndex - minIndex) / (xAxis.splitNumber - 1) * m_CoordinateWidth;
                }
                else
                {
                    dataHig = (value - xMinValue) / (xMaxValue - xMinValue) * m_CoordinateWidth;
                }
                np = new Vector3(pX + dataHig, pY);
                serie.dataPoints.Add(np);
                seriesHig[i] += dataHig;
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
                if (serie.areaStyle.tooltipHighlight && m_Tooltip.show && i < m_Tooltip.runtimeDataIndex[0])
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
                            areaColor, areaToColor, zeroPos);
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
                        ChartDrawer.DrawDashLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.Dot:
                        ChartDrawer.DrawDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDot:
                        ChartDrawer.DrawDashDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDotDot:
                        ChartDrawer.DrawDashDotDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                }
                if (isFinish) serie.animation.SetDataFinish(i);
                lp = np;
            }
            if (!serie.animation.IsFinish())
            {
                float total = totalDetailProgress - dataCount * serie.lineStyle.width * 0.5f;
                serie.animation.CheckProgress(total);
                serie.animation.CheckSymbol(serie.symbol.size);
                m_IsPlayingAnimation = true;
                RefreshChart();
            }
        }

        private Vector3 stPos1, stPos2, lastDir, lastDnPos;
        private bool lastIsDown;
        private bool DrawNormalLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp,
            Vector3 np, Vector3 nnp, int dataIndex, Color lineColor, Color areaColor, Color areaToColor,
            Vector3 zeroPos, int startIndex = 0)
        {
            var isSecond = dataIndex == startIndex + 1;
            var isTheLastPos = np == nnp;
            bool isYAxis = axis is YAxis;
            var lineWidth = serie.lineStyle.width;
            var ySmall = Mathf.Abs(lp.y - np.y) < lineWidth * 3;
            var xSmall = Mathf.Abs(lp.x - np.x) < lineWidth * 3;
            if ((isYAxis && ySmall) || (!isYAxis && xSmall))
            {
                if (serie.animation.CheckDetailBreak(np, isYAxis)) return false;
                CheckClipAndDrawLine(vh, lp, np, serie.lineStyle.width, lineColor, serie.clip);
                if (serie.areaStyle.show)
                {
                    DrawPolygonToZero(vh, lp, np, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                }
                return true;
            }

            var lastSerie = SeriesHelper.GetLastStackSerie(m_Series, serie);
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
                var diff = serie.lineStyle.width / Mathf.Sin(angle);
                var dirDp = Vector3.Cross(dir3, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                dir2v = Vector3.Cross(dir2, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                dnPos = np + (isDown ? dirDp : -dirDp) * diff;
                upPos1 = np + (isDown ? -dir1v : dir1v) * serie.lineStyle.width;
                upPos2 = np + (isDown ? -dir2v : dir2v) * serie.lineStyle.width;
                lastDir = dir1;
            }
            else
            {
                isDown = Vector3.Cross(dir1, lastDir).z <= 0;
                if (isYAxis) isDown = !isDown;
                dir1v = Vector3.Cross(dir1, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                upPos1 = np - dir1v * serie.lineStyle.width;
                upPos2 = np + dir1v * serie.lineStyle.width;
                dnPos = isDown ? upPos2 : upPos1;
            }
            if (isSecond)
            {
                stPos1 = lp - dir1v * serie.lineStyle.width;
                stPos2 = lp + dir1v * serie.lineStyle.width;
            }
            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            var dist = Vector3.Distance(lp, np);
            int segment = (int)(dist / m_Settings.lineSegmentDistance);
            if (segment <= 3) segment = (int)(dist / lineWidth);
            smoothPoints.Clear();
            smoothDownPoints.Clear();
            smoothPoints.Add(stPos1);
            smoothDownPoints.Add(stPos2);
            var start = lp;
            Vector3 ltp1 = Vector3.zero, ltp2 = Vector3.zero;
            bool isBreak = false;
            bool isStart = false;
            for (int i = 1; i < segment; i++)
            {
                var cp = lp + dir1 * (dist * i / segment);
                if (serie.animation.CheckDetailBreak(cp, isYAxis)) isBreak = true;
                var tp1 = cp - dir1v * serie.lineStyle.width;
                var tp2 = cp + dir1v * serie.lineStyle.width;
                if (isDown)
                {
                    if (!isBreak)
                    {
                        if (!isStart)
                        {
                            if (isSecond || IsValue() ||
                                (lastIsDown && ((isYAxis && tp2.y > lastDnPos.y) || (!isYAxis && tp2.x > lastDnPos.x))) ||
                                (!lastIsDown && ((isYAxis && tp1.y > lastDnPos.y) || (!isYAxis && tp1.x > lastDnPos.x))))
                            {
                                isStart = true;
                                CheckClipAndDrawPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor, serie.clip);
                            }
                        }
                        else
                        {
                            if (i == segment - 1)
                            {
                                if (np != nnp)
                                {
                                    CheckClipAndDrawPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor, serie.clip);
                                    CheckClipAndDrawTriangle(vh, upPos1, upPos2, dnPos, lineColor, serie.clip);
                                }
                                else CheckClipAndDrawPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor, serie.clip);
                            }
                            else
                            {
                                if ((isYAxis && tp2.y < dnPos.y) || (!isYAxis && tp2.x < dnPos.x) || IsValue())
                                {
                                    CheckClipAndDrawLine(vh, start, cp, serie.lineStyle.width, lineColor, serie.clip);
                                }
                                else
                                {
                                    CheckClipAndDrawPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor, serie.clip);
                                    CheckClipAndDrawTriangle(vh, upPos1, upPos2, dnPos, lineColor, serie.clip);
                                    i = segment;
                                }
                            }
                        }
                    }
                    if (IsValue() || (IsInRightOrUp(isYAxis, tp1, dnPos) && IsInRightOrUp(isYAxis, lastDnPos, tp1)))
                        smoothPoints.Add(tp1);
                    if (IsValue() || (IsInRightOrUp(isYAxis, tp2, dnPos) && IsInRightOrUp(isYAxis, stPos2, tp2)))
                        smoothDownPoints.Add(tp2);
                }
                else
                {
                    if (!isBreak)
                    {
                        if (!isStart)
                        {
                            if (isSecond || IsValue() ||
                                (lastIsDown && ((isYAxis && tp2.y > lastDnPos.y) || (!isYAxis && tp2.x > lastDnPos.x))) ||
                                (!lastIsDown && ((isYAxis && tp1.y > lastDnPos.y) || (!isYAxis && tp1.x > lastDnPos.x))))
                            {
                                isStart = true;
                                if (stPos2 != Vector3.zero)
                                {
                                    CheckClipAndDrawPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor, serie.clip);
                                }
                            }
                        }
                        else
                        {
                            if (i == segment - 1)
                            {
                                if (np != nnp)
                                {
                                    CheckClipAndDrawPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor, serie.clip);
                                    CheckClipAndDrawTriangle(vh, dnPos, upPos2, upPos1, lineColor, serie.clip);
                                }
                                else CheckClipAndDrawPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor, serie.clip);
                            }
                            else
                            {
                                if ((isYAxis && tp1.y < dnPos.y) || (!isYAxis && tp1.x < dnPos.x) || IsValue())
                                {
                                    CheckClipAndDrawLine(vh, start, cp, serie.lineStyle.width, lineColor, serie.clip);
                                }
                                else
                                {
                                    CheckClipAndDrawPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor, serie.clip);
                                    CheckClipAndDrawTriangle(vh, dnPos, upPos2, upPos1, lineColor, serie.clip);
                                    i = segment;
                                }
                            }
                        }
                    }
                    if (IsValue() || (IsInRightOrUp(isYAxis, tp1, dnPos) && IsInRightOrUp(isYAxis, lastDnPos, tp1)))
                        smoothPoints.Add(tp1);
                    if (IsValue() || IsInRightOrUp(isYAxis, lastDnPos, tp2))
                        smoothDownPoints.Add(tp2);
                }
                start = cp;
                ltp1 = tp1;
                ltp2 = tp2;
            }
            if (!isBreak)
            {
                if (isDown)
                {
                    smoothPoints.Add(upPos1);
                    smoothPoints.Add(upPos2);
                    smoothDownPoints.Add(dnPos);
                }
                else
                {
                    smoothPoints.Add(dnPos);
                    if (isYAxis)
                    {
                        smoothDownPoints.Add(isTheLastPos ? upPos1 : upPos2);
                        smoothDownPoints.Add(isTheLastPos ? upPos2 : upPos1);
                    }
                    else
                    {
                        if (isTheLastPos)
                        {
                            smoothDownPoints.Add(upPos2);
                            if (IsInRightOrUp(isYAxis, upPos2, upPos1))
                                smoothDownPoints.Add(upPos1);
                        }
                        else
                        {
                            smoothDownPoints.Add(upPos1);
                            smoothDownPoints.Add(upPos2);
                        }
                    }
                }
            }
            if (serie.areaStyle.show)
            {
                if (lastSerie != null)
                {
                    var lastSmoothPoints = lastSerie.GetUpSmoothList(dataIndex);
                    DrawStackArea(vh, serie, axis, smoothDownPoints, lastSmoothPoints, lineColor, areaColor, areaToColor);
                }
                else
                {
                    var points = ((isYAxis && lp.x < zeroPos.x) || (!isYAxis && lp.y < zeroPos.y)) ? smoothPoints : smoothDownPoints;
                    Vector3 aep = isYAxis ? new Vector3(zeroPos.x, zeroPos.y + m_CoordinateHeight) : new Vector3(zeroPos.x + m_CoordinateWidth, zeroPos.y);
                    var sindex = 0;
                    var eindex = 0;
                    var sp = GetStartPos(points, ref sindex);
                    var ep = GetEndPos(points, ref eindex);
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
                        var axisUpStart = zeroPos + (isYAxis ? Vector3.right : Vector3.up) * axis.axisLine.width;
                        var axisUpEnd = axisUpStart + (isYAxis ? Vector3.up * m_CoordinateHeight : Vector3.right * m_CoordinateWidth);
                        var axisDownStart = zeroPos - (isYAxis ? Vector3.right : Vector3.up) * axis.axisLine.width;
                        var axisDownEnd = axisDownStart + (isYAxis ? Vector3.up * m_CoordinateHeight : Vector3.right * m_CoordinateWidth);
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
                                    CheckClipAndDrawTriangle(vh, sp, luPos, tp, areaColor, areaToColor, areaToColor, serie.clip);
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
                                    CheckClipAndDrawTriangle(vh, rdPos, tp, ep, areaToColor, areaToColor, areaColor, serie.clip);
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
                                    CheckClipAndDrawTriangle(vh, sp, rdPos, tp, areaColor, areaToColor, areaToColor, serie.clip);
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
                                    CheckClipAndDrawTriangle(vh, ep, luPos, tp, areaColor, areaToColor, areaToColor, serie.clip);
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

        private bool IsInRightOrUp(bool isYAxis, bool isLastDown, Vector3 dnPos, Vector3 checkPos)
        {
            if ((isLastDown && ((isYAxis && checkPos.y <= dnPos.y) || (!isYAxis && checkPos.x <= dnPos.x))) ||
                (!isLastDown && ((isYAxis && checkPos.y <= dnPos.y) || (!isYAxis && checkPos.x <= dnPos.x))))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsInRightOrUp(bool isYAxis, Vector3 lp, Vector3 rp)
        {
            return lp == Vector3.zero || ((isYAxis && rp.y > lp.y) || (!isYAxis && rp.x > lp.x));
        }

        private void DrawPolygonToZero(VertexHelper vh, Vector3 sp, Vector3 ep, Axis axis, Vector3 zeroPos,
            Color areaColor, Color areaToColor, Vector3 areaDiff, bool clip = false)
        {
            float diff = 0;
            if (axis is YAxis)
            {
                var isLessthan0 = (sp.x < zeroPos.x || ep.x < zeroPos.x);
                diff = isLessthan0 ? -axis.axisLine.width : axis.axisLine.width;
                areaColor = GetYLerpColor(areaColor, areaToColor, sp);
                if (isLessthan0) areaDiff = -areaDiff;
                CheckClipAndDrawPolygon(vh, new Vector3(zeroPos.x + diff, sp.y), new Vector3(zeroPos.x + diff, ep.y),
                    ep + areaDiff, sp + areaDiff, areaToColor, areaColor, clip);
            }
            else
            {
                var isLessthan0 = (sp.y < zeroPos.y || ep.y < zeroPos.y);
                diff = isLessthan0 ? -axis.axisLine.width : axis.axisLine.width;
                areaColor = GetXLerpColor(areaColor, areaToColor, sp);
                if (isLessthan0) areaDiff = -areaDiff;
                if (isLessthan0)
                {
                    CheckClipAndDrawPolygon(vh, ep + areaDiff, sp + areaDiff, new Vector3(sp.x, zeroPos.y + diff),
                        new Vector3(ep.x, zeroPos.y + diff), areaColor, areaToColor, clip);
                }
                else
                {
                    CheckClipAndDrawPolygon(vh, sp + areaDiff, ep + areaDiff, new Vector3(ep.x, zeroPos.y + diff),
                         new Vector3(sp.x, zeroPos.y + diff), areaColor, areaToColor, clip);
                }
            }
        }

        private List<Vector3> posList = new List<Vector3>();
        private bool DrawOtherLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp,
            Vector3 np, int dataIndex, Color lineColor, Color areaColor,
            Color areaToColor, Vector3 zeroPos)
        {
            //lp = ClampInChart(lp);
            //np = ClampInChart(np);
            bool isYAxis = axis is YAxis;
            var lineWidth = serie.lineStyle.width;
            posList.Clear();
            switch (serie.lineType)
            {
                case LineType.Dash:
                    ChartDrawer.DrawDashLine(vh, lp, np, lineWidth, lineColor, 15, 7, posList);
                    break;
                case LineType.Dot:
                    ChartDrawer.DrawDotLine(vh, lp, np, lineWidth, lineColor, 5, 5, posList);
                    break;
                case LineType.DashDot:
                    ChartDrawer.DrawDashDotLine(vh, lp, np, lineWidth, lineColor, 15, 15, posList);
                    break;
                case LineType.DashDotDot:
                    ChartDrawer.DrawDashDotDotLine(vh, lp, np, lineWidth, lineColor, 15, 20, posList);
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
            Vector3 np, Vector3 llp, Vector3 nnp, int dataIndex, Color lineColor, Color areaColor,
            Color areaToColor, bool isStack, Vector3 zeroPos, int startIndex = 0)
        {
            bool isYAxis = xAxis is YAxis;
            var lineWidth = serie.lineStyle.width;
            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            if (isYAxis) ChartHelper.GetBezierListVertical(ref bezierPoints, lp, np, m_Settings.lineSmoothness, m_Settings.lineSmoothStyle);
            else ChartHelper.GetBezierList(ref bezierPoints, lp, np, llp, nnp, m_Settings.lineSmoothness, m_Settings.lineSmoothStyle);

            Vector3 start, to;
            if (serie.lineType == LineType.SmoothDash)
            {
                for (int i = 0; i < bezierPoints.Count - 2; i += 2)
                {
                    start = bezierPoints[i];
                    to = bezierPoints[i + 1];
                    CheckClipAndDrawLine(vh, start, to, lineWidth, lineColor, serie.clip);
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
                    CheckClipAndDrawTriangle(vh, smoothStartPosUp, startUp, lp, lineColor, serie.clip);
                    CheckClipAndDrawTriangle(vh, smoothStartPosDn, startDn, lp, lineColor, serie.clip);
                    smoothPoints.Add(smoothStartPosUp);
                    smoothDownPoints.Add(smoothStartPosDn);
                }
            }
            else
            {
                smoothPoints.Add(startUp);
                smoothDownPoints.Add(startDn);
            }
            var sourAreaColor = areaColor;
            var lastAreaDownEndPos = GetLastSmoothPos(serie.GetDownSmoothList(dataIndex - 1), isYAxis);
            var lastAreaUpEndPos = GetLastSmoothPos(serie.GetUpSmoothList(dataIndex - 1), isYAxis);
            for (int k = 1; k < bezierPoints.Count; k++)
            {
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
                if (isYAxis) CheckClipAndDrawPolygon(vh, startDn, toDn, toUp, startUp, lineColor, serie.clip);
                else CheckClipAndDrawPolygon(vh, startUp, toUp, toDn, startDn, lineColor, serie.clip);
                smoothPoints.Add(toUp);
                smoothDownPoints.Add(toDn);
                if (k == bezierPoints.Count - 1)
                {
                    smoothStartPosUp = toUp;
                    smoothStartPosDn = toDn;
                }
                if (serie.areaStyle.show && (serie.index == 0 || !isStack))
                {
                    var isAllLessthen0 = IsAllLessthen0(isYAxis, zeroPos, start, to);
                    areaColor = isYAxis ? GetYLerpColor(sourAreaColor, areaToColor, start) : GetXLerpColor(sourAreaColor, areaToColor, start);
                    if (startAreaDn == Vector3.zero)
                    {
                        if (IsInRightOrUp(isYAxis, lastAreaDownEndPos, startDn) && IsInRightOrUp(isYAxis, lastAreaDownEndPos, toDn))
                        {
                            startAreaDn = startDn;
                            if (lastAreaDownEndPos != Vector3.zero && !isAllLessthen0)
                            {
                                DrawPolygonToZero(vh, lastAreaDownEndPos, startAreaDn, xAxis, zeroPos, areaColor, areaToColor, Vector3.zero);
                            }
                        }
                    }
                    if (startAreaUp == Vector3.zero)
                    {
                        if (IsInRightOrUp(isYAxis, lastAreaUpEndPos, startUp) && IsInRightOrUp(isYAxis, lastAreaUpEndPos, toUp))
                        {
                            startAreaUp = startUp;
                            if (lastAreaUpEndPos != Vector3.zero && isAllLessthen0)
                            {
                                DrawPolygonToZero(vh, lastAreaUpEndPos, startAreaUp, xAxis, zeroPos, areaColor, areaToColor, Vector3.zero);
                            }
                        }
                    }
                    if (startAreaDn != Vector3.zero)
                    {
                        if (!isAllLessthen0 && IsInRightOrUp(isYAxis, startAreaDn, toDn))
                        {
                            DrawPolygonToZero(vh, startAreaDn, toDn, xAxis, zeroPos, areaColor, areaToColor, Vector3.zero);
                        }
                        startAreaDn = toDn;
                    }
                    if (startAreaUp != Vector3.zero)
                    {
                        if (isAllLessthen0 && IsInRightOrUp(isYAxis, startAreaUp, toUp))
                        {
                            DrawPolygonToZero(vh, startAreaUp, toUp, xAxis, zeroPos, areaColor, areaToColor, Vector3.zero);
                        }
                        startAreaUp = toUp;
                    }
                }
                start = to;
                startUp = toUp;
                startDn = toDn;
            }

            if (serie.areaStyle.show)
            {
                var lastSerie = SeriesHelper.GetLastStackSerie(m_Series, serie);
                if (lastSerie != null)
                {
                    var lastSmoothPoints = lastSerie.GetUpSmoothList(dataIndex);
                    DrawStackArea(vh, serie, xAxis, smoothDownPoints, lastSmoothPoints, lineColor, areaColor, areaToColor);
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
            List<Vector3> lastSmoothPoints, Color lineColor, Color areaColor, Color areaToColor)
        {
            if (!serie.areaStyle.show || lastSmoothPoints.Count <= 0) return;
            Vector3 start, to;
            var isYAxis = axis is YAxis;

            var lastCount = 1;
            start = smoothPoints[0];
            var sourAreaColor = areaColor;
            for (int k = 1; k < smoothPoints.Count; k++)
            {
                to = smoothPoints[k];
                if (!IsInRightOrUp(isYAxis, start, to)) continue;
                if (serie.animation.CheckDetailBreak(to, isYAxis)) break;
                Vector3 tnp, tlp;
                if (isYAxis) areaColor = GetYLerpColor(sourAreaColor, areaToColor, to);
                else areaColor = GetXLerpColor(sourAreaColor, areaToColor, to);
                if (k == smoothPoints.Count - 1)
                {
                    if (k < lastSmoothPoints.Count - 1)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        CheckClipAndDrawTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor, serie.clip);
                        while (lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                            CheckClipAndDrawTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor, serie.clip);
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
                    CheckClipAndDrawTriangle(vh, to, start, tlp, areaColor, areaColor, areaToColor, serie.clip);
                    start = to;
                    continue;
                }
                tnp = lastSmoothPoints[lastCount];
                var diff = isYAxis ? tnp.y - to.y : tnp.x - to.x;
                if (Math.Abs(diff) < 1)
                {
                    tlp = lastSmoothPoints[lastCount - 1];
                    if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                    CheckClipAndDrawPolygon(vh, start, to, tnp, tlp, areaColor, areaToColor, serie.clip);
                    lastCount++;
                }
                else
                {
                    if (diff < 0)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        CheckClipAndDrawTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor, serie.clip);
                        while (diff < 0 && lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                            CheckClipAndDrawTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor, serie.clip);
                            lastCount++;
                            diff = isYAxis ? tlp.y - to.y : tlp.x - to.x;
                            tnp = tlp;
                        }
                    }
                    else
                    {
                        tlp = lastSmoothPoints[lastCount - 1];
                        if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                        CheckClipAndDrawTriangle(vh, start, to, tlp, areaColor, areaColor, areaToColor, serie.clip);
                    }
                }
                start = to;
            }
            if (lastCount < lastSmoothPoints.Count)
            {
                var p1 = lastSmoothPoints[lastCount - 1];
                var p2 = lastSmoothPoints[lastSmoothPoints.Count - 1];
                if (!serie.animation.CheckDetailBreak(p1, isYAxis))
                {
                    CheckClipAndDrawTriangle(vh, p1, start, p2, areaToColor, areaColor, areaToColor, serie.clip);
                }
            }
        }

        private List<Vector3> linePointList = new List<Vector3>();
        private bool DrawStepLine(VertexHelper vh, Serie serie, Axis axis, Vector3 lp, Vector3 np,
            Vector3 nnp, int dataIndex, Color lineColor, Color areaColor, Color areaToColor, Vector3 zeroPos)
        {
            bool isYAxis = axis is YAxis;
            float lineWidth = serie.lineStyle.width;
            Vector3 start, end, middle, middleZero, middle1, middle2;
            Vector3 sp, ep, diff1, diff2;
            var areaDiff = isYAxis ? Vector3.left * lineWidth : Vector3.down * lineWidth;
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
                        ChartHelper.GetPointList(ref linePointList, start, middle - diff1, m_Settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip);
                            sp = ep;
                        }
                        CheckClipAndDrawPolygon(vh, middle, lineWidth, lineColor, serie.clip);
                    }
                    else
                    {
                        if (dataIndex == 1) CheckClipAndDrawPolygon(vh, lp, lineWidth, lineColor, serie.clip);
                        CheckClipAndDrawLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor, serie.clip);
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

                    ChartHelper.GetPointList(ref linePointList, middle + diff2, end, m_Settings.lineSegmentDistance);
                    sp = linePointList[0];
                    for (int i = 1; i < linePointList.Count; i++)
                    {
                        ep = linePointList[i];
                        if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                        CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                        sp = ep;
                    }

                    if (nnp != np)
                    {
                        if (serie.animation.CheckDetailBreak(np, isYAxis)) return false;
                        CheckClipAndDrawPolygon(vh, np, lineWidth, lineColor, serie.clip);
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
                        ChartHelper.GetPointList(ref linePointList, start, middle1 - diff1, m_Settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(middle1, isYAxis)) return false;
                        CheckClipAndDrawPolygon(vh, middle1, lineWidth, lineColor, serie.clip);
                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle1, middle2 - middle1) <= 0)
                        {
                            DrawPolygonToZero(vh, middle1 - diff1, middle1 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1) CheckClipAndDrawPolygon(vh, lp, lineWidth, lineColor, serie.clip);
                        CheckClipAndDrawLine(vh, lp + diff1, middle1 + diff1, lineWidth, lineColor, serie.clip);
                    }

                    //draw middle1 to middle2
                    if (Vector3.Distance(middle1, middle2) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle1 + diff2, middle2 - diff2, m_Settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip);
                            sp = ep;
                        }
                        CheckClipAndDrawPolygon(vh, middle2, lineWidth, lineColor, serie.clip);
                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle2, middle2 - middle1) >= 0)
                        {
                            DrawPolygonToZero(vh, middle2 - diff1, middle2 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        CheckClipAndDrawLine(vh, middle1 + diff2, middle2 + diff2, lineWidth, lineColor, serie.clip);
                    }
                    //draw middle2 to np
                    if (Vector3.Distance(middle2, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle2 + diff1, np - diff1, m_Settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(np, isYAxis)) return false;
                        CheckClipAndDrawPolygon(vh, np, lineWidth, lineColor, serie.clip);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, np - diff1, np + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        CheckClipAndDrawLine(vh, middle1 + diff1, middle1 + diff1, lineWidth, lineColor, serie.clip);
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
                        ChartHelper.GetPointList(ref linePointList, start, middle - diff1, m_Settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(middle, isYAxis)) return false;
                        CheckClipAndDrawPolygon(vh, middle, lineWidth, lineColor, serie.clip);
                        if (serie.areaStyle.show && Vector3.Dot(np - middle, middleZero - middle) <= 0)
                        {
                            DrawPolygonToZero(vh, middle - diff1, middle + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1) CheckClipAndDrawPolygon(vh, lp, lineWidth, lineColor, serie.clip);
                        CheckClipAndDrawLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor, serie.clip);
                    }

                    if (Vector3.Distance(middle, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle + diff2, end, m_Settings.lineSegmentDistance);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            CheckClipAndDrawLine(vh, sp, ep, lineWidth, lineColor, serie.clip);
                            sp = ep;
                        }
                        if (nnp != np) CheckClipAndDrawPolygon(vh, np, lineWidth, lineColor, serie.clip);
                    }
                    else
                    {
                        CheckClipAndDrawLine(vh, middle + diff2, np + diff2, lineWidth, lineColor, serie.clip);
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