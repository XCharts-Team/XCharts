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
        [SerializeField] private Line m_Line = Line.defaultLine;

        public Line line { get { return m_Line; } }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Line = Line.defaultLine;
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
        }

        private Dictionary<int, List<Serie>> stackSeries = new Dictionary<int, List<Serie>>();
        private List<float> seriesCurrHig = new List<float>();
        private HashSet<string> serieNameSet = new HashSet<string>();
        private void DrawLineChart(VertexHelper vh, bool yCategory)
        {
            m_Series.GetStackSeries(ref stackSeries);
            int seriesCount = stackSeries.Count;
            int serieCount = 0;
            serieNameSet.Clear();
            int serieNameCount = -1;
            for (int j = 0; j < seriesCount; j++)
            {
                var serieList = stackSeries[j];
                if (serieList.Count <= 0) continue;
                seriesCurrHig.Clear();
                if (seriesCurrHig.Capacity != serieList[0].dataCount)
                {
                    seriesCurrHig.Capacity = serieList[0].dataCount;
                }
                for (int n = 0; n < serieList.Count; n++)
                {
                    Serie serie = serieList[n];
                    serie.dataPoints.Clear();
                    if (string.IsNullOrEmpty(serie.name)) serieNameCount++;
                    else if (!serieNameSet.Contains(serie.name))
                    {
                        serieNameSet.Add(serie.name);
                        serieNameCount++;
                    }
                    Color color = m_ThemeInfo.GetColor(serieNameCount);
                    if (yCategory)
                        DrawYLineSerie(vh, serieCount, color, serie, ref seriesCurrHig);
                    else
                        DrawXLineSerie(vh, serieCount, color, serie, ref seriesCurrHig);
                    if (serie.show)
                    {
                        serieCount++;
                    }
                }
            }
            DrawLinePoint(vh);
            if (yCategory) DrawYTooltipIndicator(vh);
            else DrawXTooltipIndicator(vh);
        }

        private void DrawLinePoint(VertexHelper vh)
        {
            for (int n = 0; n < m_Series.Count; n++)
            {
                var serie = m_Series.GetSerie(n);
                if (!serie.show) continue;
                var color = m_ThemeInfo.GetColor(serie.index);
                float symbolSize = serie.symbol.size;
                for (int i = 0; i < serie.dataPoints.Count; i++)
                {
                    Vector3 p = serie.dataPoints[i];
                    if ((m_Tooltip.show && m_Tooltip.IsSelected(i))
                        || serie.data[i].highlighted
                        || serie.highlighted)
                    {
                        if (IsValue())
                        {
                            if (m_Series.IsHighlight(serie.index))
                            {
                                symbolSize = serie.symbol.selectedSize;
                            }
                        }
                        else
                        {
                            symbolSize = serie.symbol.selectedSize;
                        }
                    }
                    DrawSymbol(vh, serie.symbol.type, symbolSize, m_Line.tickness, p, color);
                }
            }

        }

        // List<Vector3> lastPoints = new List<Vector3>();
        // List<Vector3> lastSmoothPoints = new List<Vector3>();
        // List<Vector3> smoothPoints = new List<Vector3>();
        List<Vector3> smoothSegmentPoints = new List<Vector3>();
        private void DrawXLineSerie(VertexHelper vh, int serieIndex, Color lineColor, Serie serie, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            var showData = serie.GetDataList(m_DataZoom);
            Color areaColor = new Color(lineColor.r, lineColor.g, lineColor.b, lineColor.a * 0.75f);
            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            var lastSerie = m_Series.GetSerie(serie.index - 1);
            var isStack = m_Series.IsStack(serie.stack);
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
                float yDataHig;
                if (xAxis.IsValue())
                {
                    float xValue = i > showData.Count - 1 ? 0 : showData[i].data[0];
                    float pX = coordinateX + xAxis.axisLine.width;
                    float pY = seriesHig[i] + coordinateY + xAxis.axisLine.width;
                    float xDataHig = (xValue - xAxis.minValue) / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                    yDataHig = (yValue - yAxis.minValue) / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                    np = new Vector3(pX + xDataHig, pY + yDataHig);
                }
                else
                {
                    float pX = startX + i * scaleWid;
                    float pY = seriesHig[i] + coordinateY + yAxis.axisLine.width;
                    yDataHig = (yValue - yAxis.minValue) / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                    np = new Vector3(pX, pY + yDataHig);
                }

                if (i > 0)
                {
                    if (m_Line.step)
                    {
                        Vector2 middle1, middle2;
                        switch (m_Line.stepTpe)
                        {
                            case Line.StepType.Start:
                                middle1 = new Vector2(lp.x, np.y + m_Line.tickness);
                                middle2 = new Vector2(lp.x - m_Line.tickness, np.y);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, lineColor);
                                if (m_Line.area)
                                {
                                    ChartHelper.DrawPolygon(vh, new Vector2(middle1.x, coordinateY), middle1, np,
                                        new Vector2(np.x, coordinateY), areaColor);
                                }
                                break;
                            case Line.StepType.Middle:
                                middle1 = new Vector2((lp.x + np.x) / 2 + m_Line.tickness, lp.y);
                                middle2 = new Vector2((lp.x + np.x) / 2 - m_Line.tickness, np.y);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, new Vector2(middle1.x - m_Line.tickness, middle1.y),
                                    new Vector2(middle2.x + m_Line.tickness, middle2.y), m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, lineColor);
                                if (m_Line.area)
                                {
                                    ChartHelper.DrawPolygon(vh, new Vector2(lp.x, coordinateY), lp, middle1,
                                        new Vector2(middle1.x, coordinateY), areaColor);
                                    ChartHelper.DrawPolygon(vh, new Vector2(middle2.x + 2 * m_Line.tickness, coordinateY),
                                        new Vector2(middle2.x + 2 * m_Line.tickness, middle2.y), np,
                                        new Vector2(np.x, coordinateY), areaColor);
                                }
                                break;
                            case Line.StepType.End:
                                middle1 = new Vector2(np.x + m_Line.tickness, lp.y);
                                middle2 = new Vector2(np.x, lp.y);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, lineColor);
                                if (m_Line.area)
                                {
                                    ChartHelper.DrawPolygon(vh, new Vector2(lp.x, coordinateY), lp,
                                        new Vector2(middle1.x - m_Line.tickness, middle1.y),
                                        new Vector2(middle1.x - m_Line.tickness, coordinateY), areaColor);
                                }
                                break;
                        }
                    }
                    else if (m_Line.smooth)
                    {
                        DrawSmoothAreaPoints(vh, serieIndex, serie, xAxis, lp, np, i, lineColor, areaColor, isStack);
                    }
                    else
                    {
                        ChartHelper.DrawLine(vh, lp, np, m_Line.tickness, lineColor);
                        if (m_Line.area)
                        {
                            Vector3 alp = new Vector3(lp.x, lp.y - m_Line.tickness);
                            Vector3 anp = new Vector3(np.x, np.y - m_Line.tickness);
                            var cross = ChartHelper.GetIntersection(lp, np, new Vector3(coordinateX, coordinateY),
                                new Vector3(coordinateX + coordinateWid, coordinateY));
                            if (cross == Vector3.zero)
                            {
                                Vector3 tnp = serieIndex > 0 ?
                                    new Vector3(lastSerie.dataPoints[i].x, lastSerie.dataPoints[i].y + m_Line.tickness) :
                                    new Vector3(np.x, coordinateY + xAxis.axisLine.width);
                                Vector3 tlp = serieIndex > 0 ?
                                    new Vector3(lastSerie.dataPoints[i - 1].x, lastSerie.dataPoints[i - 1].y + m_Line.tickness) :
                                    new Vector3(lp.x, coordinateY + xAxis.axisLine.width);
                                ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                            }
                            else
                            {
                                Vector3 cross1 = new Vector3(cross.x, cross.y + (alp.y > coordinateY ? xAxis.axisLine.width : -xAxis.axisLine.width));
                                Vector3 cross2 = new Vector3(cross.x, cross.y + (anp.y > coordinateY ? xAxis.axisLine.width : -xAxis.axisLine.width));
                                Vector3 xp1 = new Vector3(alp.x, coordinateY + (alp.y > coordinateY ? xAxis.axisLine.width : -xAxis.axisLine.width));
                                Vector3 xp2 = new Vector3(anp.x, coordinateY + (anp.y > coordinateY ? xAxis.axisLine.width : -xAxis.axisLine.width));
                                ChartHelper.DrawTriangle(vh, alp, cross1, xp1, areaColor);
                                ChartHelper.DrawTriangle(vh, anp, cross2, xp2, areaColor);
                            }
                        }
                    }
                }
                serie.dataPoints.Add(np);
                seriesHig[i] += yDataHig;
                lp = np;
            }
        }

        private void DrawSmoothAreaPoints(VertexHelper vh, int serieIndex, Serie serie, Axis xAxis, Vector3 lp,
            Vector3 np, int dataIndex, Color lineColor, Color areaColor, bool isStack)
        {
            bool isYAxis = xAxis is YAxis;
            if (isYAxis) ChartHelper.GetBezierListVertical(ref smoothSegmentPoints, lp, np, m_Line.smoothStyle);
            else ChartHelper.GetBezierList(ref smoothSegmentPoints, lp, np, m_Line.smoothStyle);
            Vector3 start, to;
            start = smoothSegmentPoints[0];

            var smoothPoints = serie.GetSmoothList(dataIndex, smoothSegmentPoints.Count);
            smoothPoints.Clear();
            var lastSerie = m_Series.GetSerie(serie.index - 1);
            var lastSmoothPoints = lastSerie != null ? lastSerie.GetSmoothList(dataIndex, smoothSegmentPoints.Count) : new List<Vector3>();
            smoothPoints.Add(start);
            var lastCount = 1;
            for (int k = 1; k < smoothSegmentPoints.Count; k++)
            {
                smoothPoints.Add(smoothSegmentPoints[k]);
                to = smoothSegmentPoints[k];
                ChartHelper.DrawLine(vh, start, to, m_Line.tickness, lineColor);
                if (m_Line.area)
                {
                    Vector3 alp, anp, tnp, tlp;
                    if (isYAxis)
                    {
                        alp = new Vector3(start.x - m_Line.tickness, start.y);
                        anp = new Vector3(to.x - m_Line.tickness, to.y);
                    }
                    else
                    {
                        alp = new Vector3(start.x, start.y - m_Line.tickness);
                        anp = new Vector3(to.x, to.y - m_Line.tickness);
                    }
                    if (serieIndex > 0 && isStack)
                    {
                        if (k == smoothSegmentPoints.Count - 1)
                        {
                            if (k < lastSmoothPoints.Count - 1)
                            {
                                tnp = lastSmoothPoints[lastCount - 1];
                                if (isYAxis) tnp.x += m_Line.tickness;
                                else tnp.y += m_Line.tickness;
                                ChartHelper.DrawTriangle(vh, alp, anp, tnp, areaColor);
                                while (lastCount < lastSmoothPoints.Count)
                                {
                                    tlp = lastSmoothPoints[lastCount];
                                    if (isYAxis) tlp.x += m_Line.tickness;
                                    else tlp.y += m_Line.tickness;
                                    ChartHelper.DrawTriangle(vh, tnp, anp, tlp, areaColor);
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
                            if (isYAxis) tlp.x += m_Line.tickness;
                            else tlp.y += m_Line.tickness;
                            ChartHelper.DrawTriangle(vh, anp, alp, tlp, areaColor);
                            start = to;
                            continue;
                        }
                        if (isYAxis) tnp = new Vector3(lastSmoothPoints[lastCount].x + m_Line.tickness, lastSmoothPoints[lastCount].y);
                        else tnp = new Vector3(lastSmoothPoints[lastCount].x, lastSmoothPoints[lastCount].y + m_Line.tickness);

                        var diff = isYAxis ? tnp.y - to.y : tnp.x - to.x;
                        if (Math.Abs(diff) < 1)
                        {
                            if (isYAxis) tlp = new Vector3(lastSmoothPoints[lastCount - 1].x + m_Line.tickness, lastSmoothPoints[lastCount - 1].y);
                            else tlp = new Vector3(lastSmoothPoints[lastCount - 1].x, lastSmoothPoints[lastCount - 1].y + m_Line.tickness);
                            ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                            lastCount++;
                        }
                        else
                        {
                            if (diff < 0)
                            {
                                if (isYAxis) tnp = new Vector3(lastSmoothPoints[lastCount - 1].x + m_Line.tickness, lastSmoothPoints[lastCount - 1].y);
                                else tnp = new Vector3(lastSmoothPoints[lastCount - 1].x, lastSmoothPoints[lastCount - 1].y + m_Line.tickness);
                                ChartHelper.DrawTriangle(vh, alp, anp, tnp, areaColor);
                                while (diff < 0 && lastCount < lastSmoothPoints.Count)
                                {
                                    if (isYAxis) tlp = new Vector3(lastSmoothPoints[lastCount].x + m_Line.tickness, lastSmoothPoints[lastCount].y);
                                    else tlp = new Vector3(lastSmoothPoints[lastCount].x, lastSmoothPoints[lastCount].y + m_Line.tickness);
                                    ChartHelper.DrawTriangle(vh, tnp, anp, tlp, areaColor);
                                    lastCount++;
                                    diff = isYAxis ? tlp.y - to.y : tlp.x - to.x;
                                    tnp = tlp;
                                }

                            }
                            else
                            {
                                if (isYAxis) tlp = new Vector3(lastSmoothPoints[lastCount - 1].x + m_Line.tickness, lastSmoothPoints[lastCount - 1].y);
                                else tlp = new Vector3(lastSmoothPoints[lastCount - 1].x, lastSmoothPoints[lastCount - 1].y + m_Line.tickness);
                                ChartHelper.DrawTriangle(vh, alp, anp, tlp, areaColor);
                            }
                        }

                    }
                    else
                    {
                        if (isYAxis)
                        {
                            tnp = new Vector3(coordinateX + xAxis.axisLine.width, to.y);
                            tlp = new Vector3(coordinateX + xAxis.axisLine.width, start.y);
                        }
                        else
                        {
                            tnp = new Vector3(to.x, coordinateY + xAxis.axisLine.width);
                            tlp = new Vector3(start.x, coordinateY + xAxis.axisLine.width);
                        }
                        ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                    }
                }
                start = to;
            }
        }

        private void DrawYLineSerie(VertexHelper vh, int serieIndex, Color lineColor, Serie serie, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            var showData = serie.GetDataList(m_DataZoom);
            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            Color areaColor = new Color(lineColor.r, lineColor.g, lineColor.b, lineColor.a * 0.75f);
            var xAxis = m_XAxises[serie.axisIndex];
            var yAxis = m_YAxises[serie.axisIndex];
            var lastSerie = m_Series.GetSerie(serieIndex - 1);
            var isStack = m_Series.IsStack(serie.stack);
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
                if (i > 0)
                {
                    if (m_Line.step)
                    {
                        Vector2 middle1, middle2;
                        switch (m_Line.stepTpe)
                        {
                            case Line.StepType.Start:
                                middle1 = new Vector2(np.x, lp.y);
                                middle2 = new Vector2(np.x, lp.y - m_Line.tickness);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, lineColor);
                                if (m_Line.area)
                                {
                                    ChartHelper.DrawPolygon(vh, new Vector2(coordinateX, middle1.y), middle1, np,
                                        new Vector2(coordinateX, np.y), areaColor);
                                }
                                break;
                            case Line.StepType.Middle:
                                middle1 = new Vector2(lp.x, (lp.y + np.y) / 2 + m_Line.tickness);
                                middle2 = new Vector2(np.x, (lp.y + np.y) / 2 - m_Line.tickness);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, new Vector2(middle1.x, middle1.y - m_Line.tickness),
                                    new Vector2(middle2.x, middle2.y + m_Line.tickness), m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, lineColor);
                                if (m_Line.area)
                                {
                                    ChartHelper.DrawPolygon(vh, new Vector2(coordinateX, lp.y), lp, middle1,
                                        new Vector2(coordinateX, middle1.y), areaColor);
                                    ChartHelper.DrawPolygon(vh, new Vector2(coordinateX, middle2.y + 2 * m_Line.tickness),
                                        new Vector2(middle2.x, middle2.y + 2 * m_Line.tickness), np,
                                        new Vector2(coordinateX, np.y), areaColor);
                                }
                                break;
                            case Line.StepType.End:
                                middle1 = new Vector2(np.x, lp.y);
                                middle2 = new Vector2(np.x, lp.y - m_Line.tickness);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, lineColor);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, lineColor);
                                if (m_Line.area)
                                {
                                    ChartHelper.DrawPolygon(vh, new Vector2(coordinateX, lp.y), middle1,
                                        new Vector2(np.x, np.y),
                                        new Vector2(coordinateX, np.y), areaColor);
                                }
                                break;
                        }
                    }
                    else if (m_Line.smooth)
                    {
                        DrawSmoothAreaPoints(vh, serieIndex, serie, yAxis, lp, np, i, lineColor, areaColor, isStack);
                    }
                    else
                    {
                        ChartHelper.DrawLine(vh, lp, np, m_Line.tickness, lineColor);
                        if (m_Line.area)
                        {
                            Vector3 alp = new Vector3(lp.x, lp.y);
                            Vector3 anp = new Vector3(np.x, np.y);
                            var cross = ChartHelper.GetIntersection(lp, np, new Vector3(coordinateX, coordinateY),
                                new Vector3(coordinateX, coordinateY + coordinateHig));
                            if (cross == Vector3.zero)
                            {
                                Vector3 tnp = serieIndex > 0 ?
                                    new Vector3(lastSerie.dataPoints[i].x + yAxis.axisLine.width, lastSerie.dataPoints[i].y) :
                                    new Vector3(coordinateX + yAxis.axisLine.width, np.y);
                                Vector3 tlp = serieIndex > 0 ?
                                    new Vector3(lastSerie.dataPoints[i - 1].x + yAxis.axisLine.width, lastSerie.dataPoints[i - 1].y) :
                                    new Vector3(coordinateX + yAxis.axisLine.width, lp.y);
                                ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                            }
                            else
                            {
                                Vector3 cross1 = new Vector3(cross.x + (alp.x > coordinateX ? yAxis.axisLine.width : -yAxis.axisLine.width), cross.y);
                                Vector3 cross2 = new Vector3(cross.x + (anp.x > coordinateX ? yAxis.axisLine.width : -yAxis.axisLine.width), cross.y);
                                Vector3 xp1 = new Vector3(coordinateX + (alp.x > coordinateX ? yAxis.axisLine.width : -yAxis.axisLine.width), alp.y);
                                Vector3 xp2 = new Vector3(coordinateX + (anp.x > coordinateX ? yAxis.axisLine.width : -yAxis.axisLine.width), anp.y);
                                ChartHelper.DrawTriangle(vh, alp, cross1, xp1, areaColor);
                                ChartHelper.DrawTriangle(vh, anp, cross2, xp2, areaColor);
                            }
                        }
                    }
                }
                serie.dataPoints.Add(np);
                seriesHig[i] += dataHig;
                lp = np;
            }
        }
    }
}
