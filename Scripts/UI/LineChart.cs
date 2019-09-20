using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/LineChart", 13)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LineChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "LineChart";
            m_Tooltip.type = Tooltip.Type.Line;
            RemoveData();
            AddSerie("serie1", SerieType.Line);
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
                AddData(0, UnityEngine.Random.Range(10, 90));
            }
        }
#endif

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            if (m_YAxises[0].type == Axis.AxisType.Category
                || m_YAxises[1].type == Axis.AxisType.Category)
            {
                DrawLineChart(vh, true);
            }
            else
            {
                DrawLineChart(vh, false);
            }
            DrawLabelBackground(vh);
        }

        private Dictionary<int, List<Serie>> m_StackSeries = new Dictionary<int, List<Serie>>();
        private List<float> m_SeriesCurrHig = new List<float>();
        private HashSet<string> m_SerieNameSet = new HashSet<string>();
        private void DrawLineChart(VertexHelper vh, bool yCategory)
        {
            m_Series.GetStackSeries(ref m_StackSeries);
            int seriesCount = m_StackSeries.Count;
            int serieCount = 0;
            m_SerieNameSet.Clear();
            int serieNameCount = -1;
            for (int j = 0; j < seriesCount; j++)
            {
                var serieList = m_StackSeries[j];
                if (serieList.Count <= 0) continue;
                m_SeriesCurrHig.Clear();
                if (m_SeriesCurrHig.Capacity != serieList[0].dataCount)
                {
                    m_SeriesCurrHig.Capacity = serieList[0].dataCount;
                }
                for (int n = 0; n < serieList.Count; n++)
                {
                    Serie serie = serieList[n];
                    serie.dataPoints.Clear();
                    if (string.IsNullOrEmpty(serie.name)) serieNameCount++;
                    else if (!m_SerieNameSet.Contains(serie.name))
                    {
                        m_SerieNameSet.Add(serie.name);
                        serieNameCount++;
                    }
                    if (yCategory) DrawYLineSerie(vh, serieCount, serie, ref m_SeriesCurrHig);
                    else DrawXLineSerie(vh, serieCount, serie, ref m_SeriesCurrHig);
                    if (serie.show)
                    {
                        serieCount++;
                    }
                }
            }
            DrawLinePoint(vh);
            DrawLineArrow(vh);
            if (yCategory) DrawYTooltipIndicator(vh);
            else DrawXTooltipIndicator(vh);
        }

        private void DrawLinePoint(VertexHelper vh)
        {
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (serie.type != SerieType.Line) continue;
                if (!serie.show || serie.symbol.type == SerieSymbolType.None) continue;
                var count = serie.dataPoints.Count;
                for (int i = 0; i < count; i++)
                {
                    if (serie.lineArrow.show)
                    {
                        if (serie.lineArrow.position == LineArrow.Position.Start && i == 0) continue;
                        if (serie.lineArrow.position == LineArrow.Position.End && i == count - 1) continue;
                    }
                    Vector3 p = serie.dataPoints[i];
                    bool highlight = (m_Tooltip.show && m_Tooltip.IsSelected(i)) || serie.data[i].highlighted || serie.highlighted;
                    float symbolSize = highlight ? serie.symbol.selectedSize : serie.symbol.size;
                    var symbolColor = serie.GetSymbolColor(m_ThemeInfo, n, highlight);
                    symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                    DrawSymbol(vh, serie.symbol.type, symbolSize, serie.lineStyle.width, p, symbolColor);
                }
            }
        }

        private void DrawLineArrow(VertexHelper vh)
        {
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (serie.type != SerieType.Line) continue;
                if (!serie.show || !serie.lineArrow.show) continue;
                if (serie.dataPoints.Count < 2) return;
                Color lineColor = serie.GetLineColor(m_ThemeInfo, n, false);

                switch (serie.lineArrow.position)
                {
                    case LineArrow.Position.End:
                        var dataPoints = serie.GetUpSmoothList(serie.dataCount - 1);
                        if (dataPoints.Count < 2) dataPoints = serie.dataPoints;
                        var startPos = dataPoints[dataPoints.Count - 2];
                        var arrowPos = dataPoints[dataPoints.Count - 1];
                        ChartHelper.DrawArrow(vh, startPos, arrowPos, serie.lineArrow.width,
                            serie.lineArrow.height, serie.lineArrow.offset, serie.lineArrow.dent, lineColor);
                        break;
                    case LineArrow.Position.Start:
                        dataPoints = serie.GetUpSmoothList(1);
                        if (dataPoints.Count < 2) dataPoints = serie.dataPoints;
                        startPos = dataPoints[1];
                        arrowPos = dataPoints[0];
                        ChartHelper.DrawArrow(vh, startPos, arrowPos, serie.lineArrow.width,
                            serie.lineArrow.height, serie.lineArrow.offset, serie.lineArrow.dent, lineColor);
                        break;
                }
            }
        }

        private void DrawXLineSerie(VertexHelper vh, int serieIndex, Serie serie, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            var showData = serie.GetDataList(m_DataZoom);
            if (showData.Count <= 0) return;
            Color lineColor = serie.GetLineColor(m_ThemeInfo, serieIndex, false);
            Color areaColor = serie.GetAreaColor(m_ThemeInfo, serieIndex, false);
            Color areaToColor = serie.GetAreaToColor(m_ThemeInfo, serieIndex, false);
            Vector3 lp = Vector3.zero, np = Vector3.zero, nnp = Vector3.zero;
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            var zeroPos = new Vector3(coordinateX, coordinateY + yAxis.zeroYOffset);
            var lastSerie = m_Series.GetSerie(serie.index - 1);
            var isStack = m_Series.IsStack(serie.stack, SerieType.Line);
            if (!xAxis.show) xAxis = m_XAxises[(serie.axisIndex + 1) % m_XAxises.Count];
            float scaleWid = xAxis.GetDataWidth(coordinateWid, m_DataZoom);
            float startX = coordinateX + (xAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = maxShowDataNumber > 0 ?
                (maxShowDataNumber > showData.Count ? showData.Count : maxShowDataNumber)
                : showData.Count;
            if (seriesHig.Count < minShowDataNumber)
            {
                for (int i = 0; i < minShowDataNumber; i++)
                {
                    seriesHig.Add(0);
                }
            }

            for (int i = minShowDataNumber; i < maxCount; i++)
            {
                if (i >= seriesHig.Count)
                {
                    seriesHig.Add(0);
                }
                float yValue = showData[i].data[1];
                seriesHig[i] += GetDataPoint(xAxis, yAxis, showData, yValue, startX, i, scaleWid, seriesHig[i], ref np);
                serie.dataPoints.Add(np);
                lp = np;
            }
            if (serie.dataPoints.Count <= 0)
            {
                return;
            }
            lp = serie.dataPoints[0];
            int dataCount = serie.dataPoints.Count;
            float currDetailProgress = lp.x;
            float totalDetailProgress = serie.dataPoints[dataCount - 1].x;
            serie.animation.InitProgress(dataCount, currDetailProgress, totalDetailProgress);
            for (int i = 1; i < serie.dataPoints.Count; i++)
            {
                np = serie.dataPoints[i];
                serie.ClearSmoothList(i);
                if (!serie.animation.NeedAnimation(i)) break;
                bool isFinish = true;
                switch (serie.lineType)
                {
                    case LineType.Normal:
                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawNormalLine(vh, serieIndex, serie, xAxis, lp, np, nnp, i, lineColor, areaColor, areaToColor, zeroPos);
                        break;
                    case LineType.Smooth:
                        isFinish = DrawSmoothLine(vh, serieIndex, serie, xAxis, lp, np, i, lineColor, areaColor, areaToColor, isStack, zeroPos);
                        break;
                    case LineType.StepStart:
                    case LineType.StepMiddle:
                    case LineType.StepEnd:
                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawStepLine(vh, serieIndex, serie, xAxis, lp, np, nnp, i, lineColor, areaColor, areaToColor, zeroPos);
                        break;
                    case LineType.Dash:
                        ChartHelper.DrawDashLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.Dot:
                        ChartHelper.DrawDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDot:
                        ChartHelper.DrawDashDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDotDot:
                        ChartHelper.DrawDashDotDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                }
                if (isFinish) serie.animation.SetDataFinish(i);
                lp = np;
            }
            if (!serie.animation.IsFinish())
            {
                float duration = serie.animation.duration > 0 ? (float)serie.animation.duration / 1000 : 1;
                float speed = (totalDetailProgress - dataCount * serie.lineStyle.width * 0.5f) / duration;
                float symbolSpeed = serie.symbol.size / duration;
                serie.animation.CheckProgress(Time.deltaTime * speed);
                serie.animation.CheckSymbol(Time.deltaTime * symbolSpeed, serie.symbol.size);
                RefreshChart();
            }
        }

        private float GetDetailProgress(Serie serie, Axis axis, Vector3 lp, Vector3 np, bool fine)
        {
            var isYAxis = axis is YAxis;
            float progress = 0;
            switch (serie.lineType)
            {
                case LineType.Normal:
                    progress = Vector3.Distance(lp, np) / 3f - 1;
                    break;
                case LineType.Smooth:
                    ChartHelper.GetBezierList(ref bezierPoints, lp, np, fine, lineSmoothStyle);
                    if (bezierPoints.Count > 0) progress = bezierPoints.Count - 1;
                    break;
                case LineType.StepStart:
                    var middle = isYAxis ? new Vector3(np.x, lp.y) : new Vector3(lp.x, np.y);
                    progress = (Vector3.Distance(middle, np)) / 3f - 1;
                    break;
                case LineType.StepMiddle:
                    var middle1 = isYAxis ? new Vector2(lp.x, (lp.y + np.y) / 2) : new Vector2((lp.x + np.x) / 2, lp.y);
                    var middle2 = isYAxis ? new Vector2(np.x, (lp.y + np.y) / 2) : new Vector2((lp.x + np.x) / 2, np.y);
                    progress = (Vector3.Distance(lp, middle1) + Vector3.Distance(middle2, np)) / 3f - 1;
                    break;
                case LineType.StepEnd:
                    middle = isYAxis ? new Vector3(lp.x, np.y) : new Vector3(np.x, lp.y);
                    progress = Vector3.Distance(lp, middle) / 3f - 1;
                    break;
            }
            return progress;
        }

        private float GetDataPoint(Axis xAxis, Axis yAxis, List<SerieData> showData, float yValue, float startX, int i,
            float scaleWid, float serieHig, ref Vector3 np)
        {
            float xDataHig, yDataHig;
            if (xAxis.IsValue())
            {
                float xValue = i > showData.Count - 1 ? 0 : showData[i].data[0];
                float pX = coordinateX + xAxis.axisLine.width;
                float pY = serieHig + coordinateY + xAxis.axisLine.width;
                if ((xAxis.maxValue - xAxis.minValue) <= 0) xDataHig = 0;
                else xDataHig = (xValue - xAxis.minValue) / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                if ((yAxis.maxValue - yAxis.minValue) <= 0) yDataHig = 0;
                else yDataHig = (yValue - yAxis.minValue) / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                np = new Vector3(pX + xDataHig, pY + yDataHig);
            }
            else
            {
                float pX = startX + i * scaleWid;
                float pY = serieHig + coordinateY + yAxis.axisLine.width;
                if ((yAxis.maxValue - yAxis.minValue) <= 0) yDataHig = 0;
                else yDataHig = (yValue - yAxis.minValue) / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                np = new Vector3(pX, pY + yDataHig);
            }
            return yDataHig;
        }

        private void DrawYLineSerie(VertexHelper vh, int serieIndex, Serie serie, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            var showData = serie.GetDataList(m_DataZoom);
            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            Vector3 nnp = Vector3.zero;
            Color lineColor = serie.GetLineColor(m_ThemeInfo, serieIndex, false);
            Color areaColor = serie.GetAreaColor(m_ThemeInfo, serieIndex, false);
            Color areaToColor = serie.GetAreaToColor(m_ThemeInfo, serieIndex, false);
            var xAxis = m_XAxises[serie.axisIndex];
            var yAxis = m_YAxises[serie.axisIndex];
            var zeroPos = new Vector3(coordinateX + xAxis.zeroXOffset, coordinateY);
            var lastSerie = m_Series.GetSerie(serieIndex - 1);
            var isStack = m_Series.IsStack(serie.stack, SerieType.Line);
            if (!yAxis.show) yAxis = m_YAxises[(serie.axisIndex + 1) % m_YAxises.Count];
            float scaleWid = yAxis.GetDataWidth(coordinateHig, m_DataZoom);
            float startY = coordinateY + (yAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = maxShowDataNumber > 0 ?
                (maxShowDataNumber > showData.Count ? showData.Count : maxShowDataNumber)
                : showData.Count;
            if (seriesHig.Count < minShowDataNumber)
            {
                for (int i = 0; i < minShowDataNumber; i++)
                {
                    seriesHig.Add(0);
                }
            }
            var fine = isStack && m_Series.IsAnyGradientSerie(serie.stack);
            for (int i = minShowDataNumber; i < maxCount; i++)
            {
                if (i >= seriesHig.Count)
                {
                    seriesHig.Add(0);
                }
                float value = showData[i].data[1];
                float pY = startY + i * scaleWid;
                float pX = seriesHig[i] + coordinateX + yAxis.axisLine.width;
                float dataHig = (value - xAxis.minValue) / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                np = new Vector3(pX + dataHig, pY);
                serie.dataPoints.Add(np);
                seriesHig[i] += dataHig;
                lp = np;
            }

            lp = serie.dataPoints[0];
            int dataCount = serie.dataPoints.Count;
            float currDetailProgress = lp.y;
            float totalDetailProgress = serie.dataPoints[dataCount - 1].y;
            serie.animation.InitProgress(dataCount, currDetailProgress, totalDetailProgress);
            for (int i = 1; i < serie.dataPoints.Count; i++)
            {
                np = serie.dataPoints[i];
                serie.ClearSmoothList(i);
                if (!serie.animation.NeedAnimation(i)) break;
                bool isFinish = true;
                switch (serie.lineType)
                {
                    case LineType.Normal:
                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawNormalLine(vh, serieIndex, serie, yAxis, lp, np, nnp, i, lineColor, areaColor, areaToColor, zeroPos);
                        break;
                    case LineType.Smooth:
                        isFinish = DrawSmoothLine(vh, serieIndex, serie, yAxis, lp, np, i, lineColor, areaColor, areaToColor, isStack, zeroPos);
                        break;
                    case LineType.StepStart:
                    case LineType.StepMiddle:
                    case LineType.StepEnd:
                        nnp = i < serie.dataPoints.Count - 1 ? serie.dataPoints[i + 1] : np;
                        isFinish = DrawStepLine(vh, serieIndex, serie, yAxis, lp, np, nnp, i, lineColor, areaColor, areaToColor, zeroPos);
                        break;
                    case LineType.Dash:
                        ChartHelper.DrawDashLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.Dot:
                        ChartHelper.DrawDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDot:
                        ChartHelper.DrawDashDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                    case LineType.DashDotDot:
                        ChartHelper.DrawDashDotDotLine(vh, lp, np, serie.lineStyle.width, lineColor);
                        isFinish = true;
                        break;
                }
                if (isFinish) serie.animation.SetDataFinish(i);
                lp = np;
            }
            if (!serie.animation.IsFinish())
            {
                float duration = serie.animation.duration > 0 ? (float)serie.animation.duration / 1000 : 1;
                float speed = (totalDetailProgress - dataCount * serie.lineStyle.width * 0.5f) / duration;
                float symbolSpeed = serie.symbol.size / duration;
                serie.animation.CheckProgress(Time.deltaTime * speed);
                serie.animation.CheckSymbol(Time.deltaTime * symbolSpeed, serie.symbol.size);
                RefreshChart();
            }
        }

        private Vector3 stPos1, stPos2, lastDir, lastDnPos;
        private bool DrawNormalLine(VertexHelper vh, int serieIndex, Serie serie, Axis axis, Vector3 lp,
            Vector3 np, Vector3 nnp, int dataIndex, Color lineColor, Color areaColor, Color areaToColor, Vector3 zeroPos)
        {
            bool isYAxis = axis is YAxis;
            var lastSerie = m_Series.GetLastStackSerie(serie);
            Vector3 dnPos, upPos1, upPos2, dir1v, dir2v;
            bool isDown;
            var dir1 = (np - lp).normalized;
            dir1v = Vector3.Cross(dir1, Vector3.forward).normalized * (isYAxis ? -1 : 1);
            float cut = 0;
            if (np != nnp)
            {
                var dir2 = (nnp - np).normalized;
                var dir3 = (dir1 + dir2).normalized;
                var normal = Vector3.Cross(dir1, dir2);
                isDown = isYAxis ? normal.z >= 0 : normal.z <= 0;
                var angle = (180 - Vector3.Angle(dir1, dir2)) * Mathf.Deg2Rad / 2;
                var diff = serie.lineStyle.width / Mathf.Sin(angle);
                cut = serie.lineStyle.width / Mathf.Tan(angle);
                var dirDp = Vector3.Cross(dir3, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                dir2v = Vector3.Cross(dir2, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                dnPos = np + (isDown ? dirDp : -dirDp) * diff;
                upPos1 = np + (isDown ? -dir1v : dir1v) * serie.lineStyle.width;
                upPos2 = np + (isDown ? -dir2v : dir2v) * serie.lineStyle.width;
                if (dataIndex == 1)
                {
                    stPos1 = lp - dir1v * serie.lineStyle.width;
                    stPos2 = lp + dir1v * serie.lineStyle.width;
                }
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
            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            var dist = Vector3.Distance(lp, np);
            var fine = m_Series.IsAnyGradientSerie(serie.stack);
            var tick = fine ? 3f : 3f; // 3f:30f
            int segment = (int)(dist / tick);
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
                            if ((isYAxis && tp1.y > lastDnPos.y) || (!isYAxis && tp1.x > lastDnPos.x) || dataIndex == 1 || IsValue())
                            {
                                isStart = true;
                                ChartHelper.DrawPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor);
                            }
                        }
                        else
                        {
                            if (i == segment - 1)
                            {
                                if (np != nnp)
                                {
                                    ChartHelper.DrawPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor);
                                    ChartHelper.DrawTriangle(vh, upPos1, upPos2, dnPos, lineColor);
                                }
                                else ChartHelper.DrawPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor);
                            }
                            else
                            {
                                if ((isYAxis && tp2.y < dnPos.y) || (!isYAxis && tp2.x < dnPos.x) || IsValue())
                                {
                                    ChartHelper.DrawLine(vh, start, cp, serie.lineStyle.width, lineColor);
                                }
                                else
                                {
                                    ChartHelper.DrawPolygon(vh, ltp1, upPos1, dnPos, ltp2, lineColor);
                                    ChartHelper.DrawTriangle(vh, upPos1, upPos2, dnPos, lineColor);
                                    i = segment;
                                }
                            }
                        }
                    }

                    if (isYAxis)
                    {
                        if (tp1.y > lastDnPos.y || dataIndex == 1 || IsValue()) smoothPoints.Add(tp1);
                        if (tp2.y < dnPos.y || IsValue()) smoothDownPoints.Add(tp2);
                    }
                    else
                    {
                        if (tp1.x > lastDnPos.x || dataIndex == 1 || IsValue()) smoothPoints.Add(tp1);
                        if (tp2.x < dnPos.x || dataIndex == 1 || IsValue()) smoothDownPoints.Add(tp2);
                    }
                }
                else
                {
                    if (!isBreak)
                    {
                        if (!isStart)
                        {
                            if ((isYAxis && tp2.y > lastDnPos.y) || (!isYAxis && tp2.x > lastDnPos.x) || dataIndex == 1 || IsValue())
                            {
                                isStart = true;
                                ChartHelper.DrawPolygon(vh, stPos1, tp1, tp2, stPos2, lineColor);
                            }
                        }
                        else
                        {
                            if (i == segment - 1)
                            {
                                if (np != nnp)
                                {
                                    ChartHelper.DrawPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor);
                                    ChartHelper.DrawTriangle(vh, dnPos, upPos2, upPos1, lineColor);
                                }
                                else ChartHelper.DrawPolygon(vh, ltp1, upPos1, upPos2, ltp2, lineColor);
                            }
                            else
                            {
                                if ((isYAxis && tp1.y < dnPos.y) || (!isYAxis && tp1.x < dnPos.x) || IsValue())
                                    ChartHelper.DrawLine(vh, start, cp, serie.lineStyle.width, lineColor);
                                else
                                {
                                    ChartHelper.DrawPolygon(vh, ltp1, dnPos, upPos1, ltp2, lineColor);
                                    ChartHelper.DrawTriangle(vh, dnPos, upPos2, upPos1, lineColor);
                                    i = segment;
                                }
                            }
                        }
                    }

                    if (isYAxis)
                    {
                        if (tp1.y < dnPos.y || IsValue()) smoothPoints.Add(tp1);
                        if (tp2.y > lastDnPos.y || dataIndex == 1 || IsValue()) smoothDownPoints.Add(tp2);
                    }
                    else
                    {
                        if (tp1.x < dnPos.x || IsValue()) smoothPoints.Add(tp1);
                        if (tp2.x > lastDnPos.x || dataIndex == 1 || IsValue()) smoothDownPoints.Add(tp2);
                    }
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
                        smoothDownPoints.Add(np != nnp ? upPos1 : upPos2);
                        smoothDownPoints.Add(np != nnp ? upPos2 : upPos1);
                    }
                    else
                    {
                        smoothDownPoints.Add(np != nnp ? upPos1 : upPos2);
                        smoothDownPoints.Add(np != nnp ? upPos2 : upPos1);
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
                    Vector3 aep = isYAxis ? new Vector3(zeroPos.x, zeroPos.y + coordinateHig) : new Vector3(zeroPos.x + coordinateWid, zeroPos.y);
                    var cross = ChartHelper.GetIntersection(points[0], points[points.Count - 1], zeroPos, aep);
                    if (cross == Vector3.zero || smoothDownPoints.Count <= 3)
                    {
                        Vector3 sp = points[0];
                        Vector3 ep;
                        for (int i = 1; i < points.Count; i++)
                        {
                            ep = points[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) break;
                            DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                            sp = ep;
                        }
                    }
                    else
                    {
                        var sp1 = smoothDownPoints[1];
                        var ep1 = smoothDownPoints[smoothDownPoints.Count - 2];
                        var axisUpStart = zeroPos + (isYAxis ? Vector3.right : Vector3.up) * axis.axisLine.width;
                        var axisUpEnd = axisUpStart + (isYAxis ? Vector3.up * coordinateHig : Vector3.right * coordinateWid);
                        var axisDownStart = zeroPos - (isYAxis ? Vector3.right : Vector3.up) * axis.axisLine.width;
                        var axisDownEnd = axisDownStart + (isYAxis ? Vector3.up * coordinateHig : Vector3.right * coordinateWid);
                        var luPos = ChartHelper.GetIntersection(sp1, ep1, axisUpStart, axisUpEnd);
                        var ldPos = ChartHelper.GetIntersection(sp1, ep1, axisDownStart, axisDownEnd);
                        sp1 = smoothPoints[1];
                        ep1 = smoothPoints[smoothPoints.Count - 2];

                        var ruPos = ChartHelper.GetIntersection(sp1, ep1, axisUpStart, axisUpEnd);
                        var rdPos = ChartHelper.GetIntersection(sp1, ep1, axisDownStart, axisDownEnd);
                        Vector3 sp, ep;
                        if (luPos == Vector3.zero || ldPos == Vector3.zero || ruPos == Vector3.zero || rdPos == Vector3.zero)
                        {
                            sp = points[0];
                            for (int i = 1; i < points.Count; i++)
                            {
                                ep = points[i];
                                if (serie.animation.CheckDetailBreak(ep, isYAxis)) break;
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, Vector3.zero);
                                sp = ep;
                            }
                        }
                        else
                        {
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
                                        ChartHelper.DrawTriangle(vh, sp, luPos, tp, areaColor, areaToColor, areaToColor);
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
                                        ChartHelper.DrawTriangle(vh, rdPos, tp, ep, areaToColor, areaToColor, areaColor);
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
                                        ChartHelper.DrawTriangle(vh, sp, rdPos, tp, areaColor, areaToColor, areaToColor);
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
                                        ChartHelper.DrawTriangle(vh, ep, luPos, tp, areaColor, areaToColor, areaToColor);
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
            }
            stPos1 = isDown ? upPos2 : dnPos;
            stPos2 = isDown ? dnPos : upPos2;
            lastDnPos = dnPos;
            return !isBreak;
        }

        private void DrawPolygonToZero(VertexHelper vh, Vector3 sp, Vector3 ep, Axis axis, Vector3 zeroPos,
            Color areaColor, Color areaToColor, Vector3 areaDiff)
        {
            float diff = 0;
            if (axis is YAxis)
            {
                var isLessthan0 = (sp.x < zeroPos.x || ep.x < zeroPos.x);
                diff = isLessthan0 ? -axis.axisLine.width : axis.axisLine.width;
                if (isLessthan0) areaDiff = -areaDiff;
                ChartHelper.DrawPolygon(vh, new Vector3(zeroPos.x + diff, sp.y), new Vector3(zeroPos.x + diff, ep.y), ep + areaDiff, sp + areaDiff, areaToColor, areaColor);
            }
            else
            {
                var isLessthan0 = (sp.y < zeroPos.y || ep.y < zeroPos.y);
                diff = isLessthan0 ? -axis.axisLine.width : axis.axisLine.width;
                if (isLessthan0) areaDiff = -areaDiff;
                if (isLessthan0)
                {
                    ChartHelper.DrawPolygon(vh, ep + areaDiff, sp + areaDiff, new Vector3(sp.x, zeroPos.y + diff), new Vector3(ep.x, zeroPos.y + diff), areaColor, areaToColor);
                }
                else
                {
                    ChartHelper.DrawPolygon(vh, sp + areaDiff, ep + areaDiff, new Vector3(ep.x, zeroPos.y + diff), new Vector3(sp.x, zeroPos.y + diff), areaColor, areaToColor);
                }
            }
        }

        private List<Vector3> bezierPoints = new List<Vector3>();
        private bool DrawSmoothLine(VertexHelper vh, int serieIndex, Serie serie, Axis xAxis, Vector3 lp,
            Vector3 np, int dataIndex, Color lineColor, Color areaColor, Color areaToColor, bool isStack, Vector3 zeroPos)
        {
            bool isYAxis = xAxis is YAxis;
            var lineWidth = serie.lineStyle.width;
            var smoothPoints = serie.GetUpSmoothList(dataIndex);
            var smoothDownPoints = serie.GetDownSmoothList(dataIndex);
            var fine = isStack && m_Series.IsAnyGradientSerie(serie.stack);

            if (isYAxis) ChartHelper.GetBezierListVertical(ref bezierPoints, lp, np, fine, lineSmoothStyle);
            else ChartHelper.GetBezierList(ref bezierPoints, lp, np, fine, lineSmoothStyle);

            Vector3 start, to;
            start = bezierPoints[0];

            var dir = bezierPoints[1] - start;
            var dir1v = Vector3.Cross(dir, Vector3.forward).normalized;
            var startUp = start + (isYAxis ? Vector3.right : Vector3.up) * lineWidth;
            var startDn = start - (isYAxis ? Vector3.right : Vector3.up) * lineWidth;
            Vector3 toUp, toDn, tnp, tlp;
            smoothPoints.Add(startUp);
            smoothDownPoints.Add(startDn);
            bool isFinish = true;
            for (int k = 1; k < bezierPoints.Count; k++)
            {
                to = bezierPoints[k];
                if (serie.animation.CheckDetailBreak(to, isYAxis))
                {
                    isFinish = false;
                    break;
                }
                dir = to - start;
                if (k < bezierPoints.Count - 1)
                {
                    dir1v = Vector3.Cross(dir, Vector3.forward).normalized * (isYAxis ? -1 : 1);
                    var diff = dir1v * lineWidth;
                    toUp = to - diff;
                    toDn = to + diff;
                    if (isYAxis) ChartHelper.DrawPolygon(vh, startDn, toDn, toUp, startUp, lineColor);
                    else ChartHelper.DrawPolygon(vh, startUp, toUp, toDn, startDn, lineColor);
                }
                else
                {
                    toUp = to + (isYAxis ? Vector3.right : Vector3.up) * lineWidth;
                    toDn = to - (isYAxis ? Vector3.right : Vector3.up) * lineWidth;
                    if (isYAxis) ChartHelper.DrawPolygon(vh, toDn, toUp, startUp, startDn, lineColor);
                    else ChartHelper.DrawPolygon(vh, startUp, toUp, toDn, startDn, lineColor);
                }
                smoothPoints.Add(toUp);
                smoothDownPoints.Add(toDn);


                if (serie.areaStyle.show && (serieIndex == 0 || !isStack))
                {
                    if (isYAxis)
                    {
                        if (start.x > zeroPos.x && to.x > zeroPos.x)
                        {
                            tnp = new Vector3(zeroPos.x + xAxis.axisLine.width, toDn.y);
                            tlp = new Vector3(zeroPos.x + xAxis.axisLine.width, startDn.y);
                            ChartHelper.DrawPolygon(vh, startDn, toDn, tnp, tlp, areaColor, areaToColor);
                        }
                        else if (start.x < zeroPos.x && to.x < zeroPos.x)
                        {
                            tnp = new Vector3(zeroPos.x - xAxis.axisLine.width, toUp.y);
                            tlp = new Vector3(zeroPos.x - xAxis.axisLine.width, startUp.y);
                            ChartHelper.DrawPolygon(vh, tnp, tlp, startUp, toUp, areaToColor, areaColor);
                        }
                    }
                    else
                    {
                        if (start.y > zeroPos.y && to.y > zeroPos.y)
                        {
                            tnp = new Vector3(toDn.x, zeroPos.y + xAxis.axisLine.width);
                            tlp = new Vector3(startDn.x, zeroPos.y + xAxis.axisLine.width);
                            ChartHelper.DrawPolygon(vh, startDn, toDn, tnp, tlp, areaColor, areaToColor);
                        }
                        else if (start.y < zeroPos.y && to.y < zeroPos.y)
                        {
                            tnp = new Vector3(toUp.x, zeroPos.y - xAxis.axisLine.width);
                            tlp = new Vector3(startUp.x, zeroPos.y - xAxis.axisLine.width);
                            ChartHelper.DrawPolygon(vh, tlp, tnp, toUp, startUp, areaToColor, areaColor);
                        }
                    }
                }
                start = to;
                startUp = toUp;
                startDn = toDn;
            }

            if (serie.areaStyle.show)
            {
                var lastSerie = m_Series.GetLastStackSerie(serie);
                if (lastSerie != null)
                {
                    var lastSmoothPoints = lastSerie.GetUpSmoothList(dataIndex);
                    DrawStackArea(vh, serie, xAxis, smoothDownPoints, lastSmoothPoints, lineColor, areaColor, areaToColor);
                }
            }
            return isFinish;
        }

        private void DrawStackArea(VertexHelper vh, Serie serie, Axis axis, List<Vector3> smoothPoints,
            List<Vector3> lastSmoothPoints, Color lineColor, Color areaColor, Color areaToColor)
        {
            if (!serie.areaStyle.show || lastSmoothPoints.Count <= 0) return;
            Vector3 start, to;
            var isYAxis = axis is YAxis;

            var lastCount = 1;
            start = smoothPoints[0];
            for (int k = 1; k < smoothPoints.Count; k++)
            {
                to = smoothPoints[k];
                if (serie.animation.CheckDetailBreak(to, isYAxis)) break;
                Vector3 tnp, tlp;
                if (k == smoothPoints.Count - 1)
                {
                    if (k < lastSmoothPoints.Count - 1)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        ChartHelper.DrawTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor);
                        while (lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                            ChartHelper.DrawTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor);
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
                    ChartHelper.DrawTriangle(vh, to, start, tlp, areaColor, areaColor, areaToColor);
                    start = to;
                    continue;
                }
                tnp = lastSmoothPoints[lastCount];
                var diff = isYAxis ? tnp.y - to.y : tnp.x - to.x;
                if (Math.Abs(diff) < 1)
                {
                    tlp = lastSmoothPoints[lastCount - 1];
                    if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                    ChartHelper.DrawPolygon(vh, start, to, tnp, tlp, areaColor, areaToColor);
                    lastCount++;
                }
                else
                {
                    if (diff < 0)
                    {
                        tnp = lastSmoothPoints[lastCount - 1];
                        ChartHelper.DrawTriangle(vh, start, to, tnp, areaColor, areaColor, areaToColor);
                        while (diff < 0 && lastCount < lastSmoothPoints.Count)
                        {
                            tlp = lastSmoothPoints[lastCount];
                            if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                            ChartHelper.DrawTriangle(vh, tnp, to, tlp, areaToColor, areaColor, areaToColor);
                            lastCount++;
                            diff = isYAxis ? tlp.y - to.y : tlp.x - to.x;
                            tnp = tlp;
                        }
                    }
                    else
                    {
                        tlp = lastSmoothPoints[lastCount - 1];
                        if (serie.animation.CheckDetailBreak(tlp, isYAxis)) break;
                        ChartHelper.DrawTriangle(vh, start, to, tlp, areaColor, areaColor, areaToColor);
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
                    ChartHelper.DrawTriangle(vh, p1, start, p2, areaToColor, areaColor, areaToColor);
                }
            }
        }

        private List<Vector3> linePointList = new List<Vector3>();
        private bool DrawStepLine(VertexHelper vh, int serieIndex, Serie serie, Axis axis, Vector3 lp,
            Vector3 np, Vector3 nnp, int dataIndex, Color lineColor, Color areaColor, Color areaToColor, Vector3 zeroPos)
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
                        ChartHelper.GetPointList(ref linePointList, start, middle - diff1, 3f);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            ChartHelper.DrawLine(vh, sp, ep, lineWidth, lineColor);
                            sp = ep;
                        }
                        ChartHelper.DrawPolygon(vh, middle, lineWidth, lineColor);
                    }
                    else
                    {
                        if (dataIndex == 1) ChartHelper.DrawPolygon(vh, lp, lineWidth, lineColor);
                        ChartHelper.DrawLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor);
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

                    ChartHelper.GetPointList(ref linePointList, middle + diff2, end, 3f);
                    sp = linePointList[0];
                    for (int i = 1; i < linePointList.Count; i++)
                    {
                        ep = linePointList[i];
                        if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                        ChartHelper.DrawLine(vh, sp, ep, lineWidth, lineColor);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                        sp = ep;
                    }

                    if (nnp != np)
                    {
                        if (serie.animation.CheckDetailBreak(np, isYAxis)) return false;
                        ChartHelper.DrawPolygon(vh, np, lineWidth, lineColor);
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
                        ChartHelper.GetPointList(ref linePointList, start, middle1 - diff1, 3f);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            ChartHelper.DrawLine(vh, sp, ep, lineWidth, lineColor);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(middle1, isYAxis)) return false;
                        ChartHelper.DrawPolygon(vh, middle1, lineWidth, lineColor);
                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle1, middle2 - middle1) <= 0)
                        {
                            DrawPolygonToZero(vh, middle1 - diff1, middle1 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1) ChartHelper.DrawPolygon(vh, lp, lineWidth, lineColor);
                        ChartHelper.DrawLine(vh, lp + diff1, middle1 + diff1, lineWidth, lineColor);
                    }

                    //draw middle1 to middle2
                    if (Vector3.Distance(middle1, middle2) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle1 + diff2, middle2 - diff2, 3f);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            ChartHelper.DrawLine(vh, sp, ep, lineWidth, lineColor);
                            sp = ep;
                        }
                        ChartHelper.DrawPolygon(vh, middle2, lineWidth, lineColor);
                        if (serie.areaStyle.show && Vector3.Dot(middleZero - middle2, middle2 - middle1) >= 0)
                        {
                            DrawPolygonToZero(vh, middle2 - diff1, middle2 + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        ChartHelper.DrawLine(vh, middle1 + diff2, middle2 + diff2, lineWidth, lineColor);
                    }
                    //draw middle2 to np
                    if (Vector3.Distance(middle2, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle2 + diff1, np - diff1, 3f);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            ChartHelper.DrawLine(vh, sp, ep, lineWidth, lineColor);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(np, isYAxis)) return false;
                        ChartHelper.DrawPolygon(vh, np, lineWidth, lineColor);
                        if (serie.areaStyle.show)
                        {
                            DrawPolygonToZero(vh, np - diff1, np + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        ChartHelper.DrawLine(vh, middle1 + diff1, middle1 + diff1, lineWidth, lineColor);
                    }
                    break;
                case LineType.StepEnd:
                    middle = isYAxis ? new Vector3(lp.x, np.y) : new Vector3(np.x, lp.y);
                    middleZero = isYAxis ? new Vector3(zeroPos.x, middle.y) : new Vector3(middle.x, zeroPos.y);
                    //serie.animation.SetDestDetailProgress(Vector3.Distance(lp, middle) / 3f);
                    diff1 = (middle - lp).normalized * lineWidth;
                    diff2 = (np - middle).normalized * lineWidth;
                    start = dataIndex == 1 ? lp : lp + diff1;
                    end = nnp != np ? np - diff2 : np;

                    if (Vector3.Distance(lp, middle) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, start, middle - diff1, 3f);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            if (serie.animation.CheckDetailBreak(ep, isYAxis)) return false;
                            ChartHelper.DrawLine(vh, sp, ep, lineWidth, lineColor);
                            if (serie.areaStyle.show)
                            {
                                DrawPolygonToZero(vh, sp, ep, axis, zeroPos, areaColor, areaToColor, areaDiff);
                            }
                            sp = ep;
                        }
                        if (serie.animation.CheckDetailBreak(middle, isYAxis)) return false;
                        ChartHelper.DrawPolygon(vh, middle, lineWidth, lineColor);
                        if (serie.areaStyle.show && Vector3.Dot(np - middle, middleZero - middle) <= 0)
                        {
                            DrawPolygonToZero(vh, middle - diff1, middle + diff1, axis, zeroPos, areaColor, areaToColor, areaDiff);
                        }
                    }
                    else
                    {
                        if (dataIndex == 1) ChartHelper.DrawPolygon(vh, lp, lineWidth, lineColor);
                        ChartHelper.DrawLine(vh, lp + diff1, middle + diff1, lineWidth, lineColor);
                    }

                    if (Vector3.Distance(middle, np) > 2 * lineWidth)
                    {
                        ChartHelper.GetPointList(ref linePointList, middle + diff2, end, 3f);
                        sp = linePointList[0];
                        for (int i = 1; i < linePointList.Count; i++)
                        {
                            ep = linePointList[i];
                            ChartHelper.DrawLine(vh, sp, ep, lineWidth, lineColor);
                            sp = ep;
                        }
                        if (nnp != np) ChartHelper.DrawPolygon(vh, np, lineWidth, lineColor);
                    }
                    else
                    {
                        ChartHelper.DrawLine(vh, middle + diff2, np + diff2, lineWidth, lineColor);
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
