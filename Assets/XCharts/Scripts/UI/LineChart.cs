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
                AddData(0, Random.Range(10, 90));
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
        private List<Vector3> points = new List<Vector3>();
        private List<int> pointSerieIndex = new List<int>();
        private void DrawLineChart(VertexHelper vh, bool yCategory)
        {
            m_Series.GetStackSeries(ref stackSeries);
            int seriesCount = stackSeries.Count;
            int serieCount = 0;
            int dataCount = 0;
            serieNameSet.Clear();
            points.Clear();
            pointSerieIndex.Clear();
            int serieNameCount = -1;
            for (int j = 0; j < seriesCount; j++)
            {
                var serieList = stackSeries[j];
                seriesCurrHig.Clear();
                if (seriesCurrHig.Capacity != serieList[0].dataCount)
                {
                    seriesCurrHig.Capacity = serieList[0].dataCount;
                }
                for (int n = 0; n < serieList.Count; n++)
                {
                    Serie serie = serieList[n];
                    if (string.IsNullOrEmpty(serie.name)) serieNameCount++;
                    else if (!serieNameSet.Contains(serie.name))
                    {
                        serieNameSet.Add(serie.name);
                        serieNameCount++;
                    }
                    Color color = m_ThemeInfo.GetColor(serieNameCount);
                    if (yCategory)
                        DrawYLineSerie(vh, serieCount, color, serie, ref dataCount, ref points, ref pointSerieIndex, ref seriesCurrHig);
                    else
                        DrawXLineSerie(vh, serieCount, color, serie, ref dataCount, ref points, ref pointSerieIndex, ref seriesCurrHig);
                    if (serie.show)
                    {
                        serieCount++;
                    }
                }
                DrawLinePoint(vh, dataCount, points, pointSerieIndex);

            }
            if (yCategory) DrawYTooltipIndicator(vh);
            else DrawXTooltipIndicator(vh);
        }

        private void DrawLinePoint(VertexHelper vh, int dataCount, List<Vector3> points, List<int> pointSerieIndex)
        {
            for (int i = 0; i < points.Count; i++)
            {
                Vector3 p = points[i];
                var dataIndex = i % dataCount;
                var serie = m_Series.GetSerie(pointSerieIndex[i]);
                float symbolSize = serie.symbol.size;
                if ((m_Tooltip.show && m_Tooltip.IsSelectedDataIndex(dataIndex))
                    || serie.data[dataIndex].highlighted
                    || serie.highlighted)
                {
                    if (IsCartesian())
                    {
                        if (m_Series.IsTooltipSelected(serie.index))
                        {
                            symbolSize = serie.symbol.selectedSize;
                        }
                    }
                    else
                    {
                        symbolSize = serie.symbol.selectedSize;
                    }
                }
                var color = m_ThemeInfo.GetColor(serie.index);
                DrawSymbol(vh, serie.symbol.type, symbolSize, m_Line.tickness, p, color);
            }
        }

        List<Vector3> lastPoints = new List<Vector3>();
        List<Vector3> lastSmoothPoints = new List<Vector3>();
        List<Vector3> smoothPoints = new List<Vector3>();
        List<Vector3> smoothSegmentPoints = new List<Vector3>();
        private void DrawXLineSerie(VertexHelper vh, int serieIndex, Color color, Serie serie, ref int dataCount,
            ref List<Vector3> points, ref List<int> pointSerieIndexs, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            lastPoints.Clear();
            lastSmoothPoints.Clear();
            smoothPoints.Clear();
            List<float> yData = serie.GetYDataList(m_DataZoom);
            List<float> xData = serie.GetXDataList(m_DataZoom);

            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            if (!xAxis.show) xAxis = m_XAxises[(serie.axisIndex + 1) % m_XAxises.Count];
            float scaleWid = xAxis.GetDataWidth(coordinateWid, m_DataZoom);
            float startX = coordinateX + (xAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = maxShowDataNumber > 0 ?
                (maxShowDataNumber > yData.Count ? yData.Count : maxShowDataNumber)
                : yData.Count;
            dataCount = (maxCount - minShowDataNumber);
            if (m_Line.area && points.Count > 0)
            {
                if (!m_Line.smooth && points.Count > 0)
                {
                    for (int m = points.Count - dataCount; m < points.Count; m++)
                    {
                        lastPoints.Add(points[m]);
                    }
                }
                else if (m_Line.smooth && smoothPoints.Count > 0)
                {
                    for (int m = 0; m < smoothPoints.Count; m++)
                    {
                        lastSmoothPoints.Add(smoothPoints[m]);
                    }
                    smoothPoints.Clear();
                }
            }
            int smoothPointCount = 1;
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
                float yValue = yData[i];
                float yDataHig;
                if (xAxis.IsValue())
                {
                    float xValue = i > xData.Count - 1 ? 0 : xData[i];
                    float pX = coordinateX + m_Coordinate.tickness;
                    float pY = seriesHig[i] + coordinateY + m_Coordinate.tickness;
                    float xDataHig = (xValue - xAxis.minValue) / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                    yDataHig = (yValue - yAxis.minValue) / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                    np = new Vector3(pX + xDataHig, pY + yDataHig);
                }
                else
                {
                    float pX = startX + i * scaleWid;
                    float pY = seriesHig[i] + coordinateY + m_Coordinate.tickness;
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
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, color);
                                if (m_Line.area)
                                {
                                    Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                                    ChartHelper.DrawPolygon(vh, new Vector2(middle1.x, coordinateY), middle1, np,
                                        new Vector2(np.x, coordinateY), areaColor);
                                }
                                break;
                            case Line.StepType.Middle:
                                middle1 = new Vector2((lp.x + np.x) / 2 + m_Line.tickness, lp.y);
                                middle2 = new Vector2((lp.x + np.x) / 2 - m_Line.tickness, np.y);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, new Vector2(middle1.x - m_Line.tickness, middle1.y),
                                    new Vector2(middle2.x + m_Line.tickness, middle2.y), m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, color);
                                if (m_Line.area)
                                {
                                    Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
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
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, color);
                                if (m_Line.area)
                                {
                                    Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                                    ChartHelper.DrawPolygon(vh, new Vector2(lp.x, coordinateY), lp,
                                        new Vector2(middle1.x - m_Line.tickness, middle1.y),
                                        new Vector2(middle1.x - m_Line.tickness, coordinateY), areaColor);
                                }
                                break;
                        }
                    }
                    else if (m_Line.smooth)
                    {
                        if (xAxis.IsValue()) ChartHelper.GetBezierListVertical(ref smoothSegmentPoints, lp, np, m_Line.smoothStyle);
                        else ChartHelper.GetBezierList(ref smoothSegmentPoints, lp, np, m_Line.smoothStyle);
                        Vector3 start, to;
                        start = smoothSegmentPoints[0];
                        for (int k = 1; k < smoothSegmentPoints.Count; k++)
                        {
                            smoothPoints.Add(smoothSegmentPoints[k]);
                            to = smoothSegmentPoints[k];
                            ChartHelper.DrawLine(vh, start, to, m_Line.tickness, color);

                            if (m_Line.area)
                            {
                                Vector3 alp = new Vector3(start.x, start.y - m_Line.tickness);
                                Vector3 anp = new Vector3(to.x, to.y - m_Line.tickness);
                                Vector3 tnp = serieIndex > 0 ?
                                    (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                    new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 1].x,
                                        lastSmoothPoints[lastSmoothPoints.Count - 1].y + m_Line.tickness) :
                                    new Vector3(lastSmoothPoints[smoothPointCount].x,
                                        lastSmoothPoints[smoothPointCount].y + m_Line.tickness)) :
                                    new Vector3(to.x, coordinateY + m_Coordinate.tickness);
                                Vector3 tlp = serieIndex > 0 ?
                                    (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                    new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 2].x,
                                        lastSmoothPoints[lastSmoothPoints.Count - 2].y + m_Line.tickness) :
                                    new Vector3(lastSmoothPoints[smoothPointCount - 1].x,
                                        lastSmoothPoints[smoothPointCount - 1].y + m_Line.tickness)) :
                                    new Vector3(start.x, coordinateY + m_Coordinate.tickness);
                                Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                                ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                            }
                            smoothPointCount++;
                            start = to;
                        }
                    }
                    else
                    {
                        ChartHelper.DrawLine(vh, lp, np, m_Line.tickness, color);
                        if (m_Line.area)
                        {
                            Vector3 alp = new Vector3(lp.x, lp.y - m_Line.tickness);
                            Vector3 anp = new Vector3(np.x, np.y - m_Line.tickness);
                            Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                            var cross = ChartHelper.GetIntersection(lp, np, new Vector3(coordinateX, coordinateY),
                                new Vector3(coordinateX + coordinateWid, coordinateY));
                            if (cross == Vector3.zero)
                            {
                                Vector3 tnp = serieIndex > 0 ?
                                    new Vector3(lastPoints[i].x, lastPoints[i].y + m_Line.tickness) :
                                    new Vector3(np.x, coordinateY + m_Coordinate.tickness);
                                Vector3 tlp = serieIndex > 0 ?
                                    new Vector3(lastPoints[i - 1].x, lastPoints[i - 1].y + m_Line.tickness) :
                                    new Vector3(lp.x, coordinateY + m_Coordinate.tickness);
                                ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                            }
                            else
                            {
                                Vector3 cross1 = new Vector3(cross.x, cross.y + (alp.y > coordinateY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                Vector3 cross2 = new Vector3(cross.x, cross.y + (anp.y > coordinateY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                Vector3 xp1 = new Vector3(alp.x, coordinateY + (alp.y > coordinateY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                Vector3 xp2 = new Vector3(anp.x, coordinateY + (anp.y > coordinateY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                ChartHelper.DrawTriangle(vh, alp, cross1, xp1, areaColor);
                                ChartHelper.DrawTriangle(vh, anp, cross2, xp2, areaColor);
                            }
                        }
                    }
                }
                if (serie.symbol.type != SerieSymbolType.None || m_Line.area)
                {
                    points.Add(np);
                    pointSerieIndexs.Add(serie.index);
                }
                seriesHig[i] += yDataHig;
                lp = np;
            }
        }

        private void DrawYLineSerie(VertexHelper vh, int serieIndex, Color color, Serie serie, ref int dataCount,
            ref List<Vector3> points, ref List<int> pointSerieIndexs, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            lastPoints.Clear();
            lastSmoothPoints.Clear();
            smoothPoints.Clear();

            Vector3 lp = Vector3.zero;
            Vector3 np = Vector3.zero;
            var xAxis = m_XAxises[serie.axisIndex];
            var yAxis = m_YAxises[serie.axisIndex];
            if (!yAxis.show) yAxis = m_YAxises[(serie.axisIndex + 1) % m_YAxises.Count];
            float scaleWid = yAxis.GetDataWidth(coordinateHig, m_DataZoom);
            float startY = coordinateY + (yAxis.boundaryGap ? scaleWid / 2 : 0);
            int maxCount = maxShowDataNumber > 0 ?
                (maxShowDataNumber > serie.yData.Count ? serie.yData.Count : maxShowDataNumber)
                : serie.yData.Count;
            dataCount = (maxCount - minShowDataNumber);
            if (m_Line.area && points.Count > 0)
            {
                if (!m_Line.smooth && points.Count > 0)
                {
                    for (int m = points.Count - dataCount; m < points.Count; m++)
                    {
                        lastPoints.Add(points[m]);
                    }
                }
                else if (m_Line.smooth && smoothPoints.Count > 0)
                {
                    for (int m = 0; m < smoothPoints.Count; m++)
                    {
                        lastSmoothPoints.Add(smoothPoints[m]);
                    }
                    smoothPoints.Clear();
                }
            }
            int smoothPointCount = 1;
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
                float value = serie.yData[i];
                float pY = startY + i * scaleWid;
                float pX = seriesHig[i] + coordinateX + m_Coordinate.tickness;
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
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, color);
                                if (m_Line.area)
                                {
                                    Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                                    ChartHelper.DrawPolygon(vh, new Vector2(coordinateX, middle1.y), middle1, np,
                                        new Vector2(coordinateX, np.y), areaColor);
                                }
                                break;
                            case Line.StepType.Middle:
                                middle1 = new Vector2(lp.x, (lp.y + np.y) / 2 + m_Line.tickness);
                                middle2 = new Vector2(np.x, (lp.y + np.y) / 2 - m_Line.tickness);
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, new Vector2(middle1.x, middle1.y - m_Line.tickness),
                                    new Vector2(middle2.x, middle2.y + m_Line.tickness), m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, color);
                                if (m_Line.area)
                                {
                                    Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
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
                                ChartHelper.DrawLine(vh, lp, middle1, m_Line.tickness, color);
                                ChartHelper.DrawLine(vh, middle2, np, m_Line.tickness, color);
                                if (m_Line.area)
                                {
                                    Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                                    ChartHelper.DrawPolygon(vh, new Vector2(coordinateX, lp.y), middle1,
                                        new Vector2(np.x, np.y),
                                        new Vector2(coordinateX, np.y), areaColor);
                                }
                                break;
                        }
                    }
                    else if (m_Line.smooth)
                    {
                        ChartHelper.GetBezierListVertical(ref smoothSegmentPoints, lp, np, m_Line.smoothStyle);
                        Vector3 start, to;
                        start = smoothSegmentPoints[0];
                        for (int k = 1; k < smoothSegmentPoints.Count; k++)
                        {
                            smoothPoints.Add(smoothSegmentPoints[k]);
                            to = smoothSegmentPoints[k];
                            ChartHelper.DrawLine(vh, start, to, m_Line.tickness, color);

                            if (m_Line.area)
                            {
                                Vector3 alp = new Vector3(start.x, start.y - m_Line.tickness);
                                Vector3 anp = new Vector3(to.x, to.y - m_Line.tickness);
                                Vector3 tnp = serieIndex > 0 ?
                                    (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                    new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 1].x,
                                        lastSmoothPoints[lastSmoothPoints.Count - 1].y + m_Line.tickness) :
                                    new Vector3(lastSmoothPoints[smoothPointCount].x,
                                        lastSmoothPoints[smoothPointCount].y + m_Line.tickness)) :
                                    new Vector3(coordinateX + m_Coordinate.tickness, to.y);
                                Vector3 tlp = serieIndex > 0 ?
                                    (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                    new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 2].x,
                                        lastSmoothPoints[lastSmoothPoints.Count - 2].y + m_Line.tickness) :
                                    new Vector3(lastSmoothPoints[smoothPointCount - 1].x,
                                        lastSmoothPoints[smoothPointCount - 1].y + m_Line.tickness)) :
                                    new Vector3(coordinateX + m_Coordinate.tickness, start.y);
                                Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                                ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                            }
                            smoothPointCount++;
                            start = to;
                        }
                    }
                    else
                    {
                        ChartHelper.DrawLine(vh, lp, np, m_Line.tickness, color);
                        if (m_Line.area)
                        {
                            Vector3 alp = new Vector3(lp.x, lp.y);
                            Vector3 anp = new Vector3(np.x, np.y);
                            Color areaColor = new Color(color.r, color.g, color.b, color.a * 0.75f);
                            var cross = ChartHelper.GetIntersection(lp, np, new Vector3(coordinateX, coordinateY),
                                new Vector3(coordinateX, coordinateY + coordinateHig));
                            if (cross == Vector3.zero)
                            {
                                Vector3 tnp = serieIndex > 0 ?
                                    new Vector3(lastPoints[i].x + m_Coordinate.tickness, lastPoints[i].y) :
                                    new Vector3(coordinateX + m_Coordinate.tickness, np.y);
                                Vector3 tlp = serieIndex > 0 ?
                                    new Vector3(lastPoints[i - 1].x + m_Coordinate.tickness, lastPoints[i - 1].y) :
                                    new Vector3(coordinateX + m_Coordinate.tickness, lp.y);
                                ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                            }
                            else
                            {
                                Vector3 cross1 = new Vector3(cross.x + (alp.x > coordinateX ? m_Coordinate.tickness : -m_Coordinate.tickness), cross.y);
                                Vector3 cross2 = new Vector3(cross.x + (anp.x > coordinateX ? m_Coordinate.tickness : -m_Coordinate.tickness), cross.y);
                                Vector3 xp1 = new Vector3(coordinateX + (alp.x > coordinateX ? m_Coordinate.tickness : -m_Coordinate.tickness), alp.y);
                                Vector3 xp2 = new Vector3(coordinateX + (anp.x > coordinateX ? m_Coordinate.tickness : -m_Coordinate.tickness), anp.y);
                                ChartHelper.DrawTriangle(vh, alp, cross1, xp1, areaColor);
                                ChartHelper.DrawTriangle(vh, anp, cross2, xp2, areaColor);
                            }
                        }
                    }
                }
                if (serie.symbol.type != SerieSymbolType.None || m_Line.area)
                {
                    points.Add(np);
                    pointSerieIndexs.Add(serie.index);
                }
                seriesHig[i] += dataHig;
                lp = np;
            }
        }
    }
}
