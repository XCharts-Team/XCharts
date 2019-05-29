using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public class LineChart : CoordinateChart
    {
        [SerializeField] private Line m_Line = Line.defaultLine;

        public Line line { get { return line; } }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void Reset()
        {
            base.Reset();
            m_Line = Line.defaultLine;
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);

            if (m_XAxis.type == Axis.AxisType.Category)
            {
                DrawXCategory(vh);
            }
            else
            {
                DrawYCategory(vh);
            }
        }

        private void DrawXCategory(VertexHelper vh)
        {
            var stackSeries = m_Series.GetStackSeries();
            int seriesCount = stackSeries.Count;
            float scaleWid = m_XAxis.GetDataWidth(coordinateWid);
            int serieCount = 0;
            List<Vector3> points = new List<Vector3>();
            List<Vector3> smoothPoints = new List<Vector3>();
            List<Color> colorList = new List<Color>();
            int dataCount = 0;
            for (int j = 0; j < seriesCount; j++)
            {
                var seriesCurrHig = new Dictionary<int, float>();
                var serieList = stackSeries[j];

                for (int n = 0; n < serieList.Count; n++)
                {
                    Serie serie = serieList[n];
                    if (!IsActive(serie.name)) continue;
                    List<Vector3> lastPoints = new List<Vector3>();
                    List<Vector3> lastSmoothPoints = new List<Vector3>();

                    Color color = m_ThemeInfo.GetColor(serieCount);
                    Vector3 lp = Vector3.zero;
                    Vector3 np = Vector3.zero;
                    float startX = zeroX + (m_XAxis.boundaryGap ? scaleWid / 2 : 0);
                    int maxCount = maxShowDataNumber > 0 ?
                        (maxShowDataNumber > serie.data.Count ? serie.data.Count : maxShowDataNumber)
                        : serie.data.Count;
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
                    for (int i = minShowDataNumber; i < maxCount; i++)
                    {
                        if (!seriesCurrHig.ContainsKey(i))
                        {
                            seriesCurrHig[i] = 0;
                        }
                        float value = serie.data[i];
                        float pX = startX + i * scaleWid;
                        float pY = seriesCurrHig[i] + zeroY + m_Coordinate.tickness;
                        float dataHig = value / (maxValue - minValue) * coordinateHig;

                        np = new Vector3(pX, pY + dataHig);

                        if (i > 0)
                        {
                            if (m_Line.smooth)
                            {
                                var list = ChartHelper.GetBezierList(lp, np, m_Line.smoothStyle);
                                Vector3 start, to;
                                start = list[0];
                                for (int k = 1; k < list.Length; k++)
                                {
                                    smoothPoints.Add(list[k]);
                                    to = list[k];
                                    ChartHelper.DrawLine(vh, start, to, m_Line.tickness, color);

                                    if (m_Line.area)
                                    {
                                        Vector3 alp = new Vector3(start.x, start.y - m_Line.tickness);
                                        Vector3 anp = new Vector3(to.x, to.y - m_Line.tickness);
                                        Vector3 tnp = serieCount > 0 ?
                                            (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                            new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 1].x, 
                                                lastSmoothPoints[lastSmoothPoints.Count - 1].y + m_Line.tickness) :
                                            new Vector3(lastSmoothPoints[smoothPointCount].x, 
                                                lastSmoothPoints[smoothPointCount].y + m_Line.tickness)) :
                                            new Vector3(to.x, zeroY + m_Coordinate.tickness);
                                        Vector3 tlp = serieCount > 0 ?
                                            (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                            new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 2].x,
                                                lastSmoothPoints[lastSmoothPoints.Count - 2].y + m_Line.tickness) :
                                            new Vector3(lastSmoothPoints[smoothPointCount - 1].x,
                                                lastSmoothPoints[smoothPointCount - 1].y + m_Line.tickness)) :
                                            new Vector3(start.x, zeroY + m_Coordinate.tickness);
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
                                    var cross = ChartHelper.GetIntersection(lp, np, new Vector3(zeroX, zeroY),
                                        new Vector3(zeroX + coordinateWid, zeroY));
                                    if (cross == Vector3.zero)
                                    {
                                        Vector3 tnp = serieCount > 0 ?
                                            new Vector3(lastPoints[i].x, lastPoints[i].y + m_Line.tickness) :
                                            new Vector3(np.x, zeroY + m_Coordinate.tickness);
                                        Vector3 tlp = serieCount > 0 ?
                                            new Vector3(lastPoints[i - 1].x, lastPoints[i - 1].y + m_Line.tickness) :
                                            new Vector3(lp.x, zeroY + m_Coordinate.tickness);
                                        ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                                    }
                                    else
                                    {
                                        Vector3 cross1 = new Vector3(cross.x, cross.y + (alp.y > zeroY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                        Vector3 cross2 = new Vector3(cross.x, cross.y + (anp.y > zeroY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                        Vector3 xp1 = new Vector3(alp.x, zeroY + (alp.y > zeroY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                        Vector3 xp2 = new Vector3(anp.x, zeroY + (anp.y > zeroY ? m_Coordinate.tickness : -m_Coordinate.tickness));
                                        ChartHelper.DrawTriangle(vh, alp, cross1, xp1, areaColor);
                                        ChartHelper.DrawTriangle(vh, anp, cross2, xp2, areaColor);
                                    }
                                }
                            }
                        }
                        if (m_Line.point)
                        {
                            points.Add(np);
                            colorList.Add(color);
                        }
                        seriesCurrHig[i] += dataHig;
                        lp = np;
                    }
                    if (serie.show)
                    {
                        serieCount++;
                    }
                }
                // draw point
                if (m_Line.point)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        Vector3 p = points[i];
                        float pointWid = m_Line.pointWidth;
                        if (m_Tooltip.show && i % dataCount == m_Tooltip.dataIndex - 1)
                        {
                            pointWid = pointWid * 1.8f;
                        }
                        if (m_Theme == Theme.Dark)
                        {

                            ChartHelper.DrawCricle(vh, p, pointWid, colorList[i],
                                (int)m_Line.pointWidth * 5);
                        }
                        else
                        {
                            ChartHelper.DrawCricle(vh, p, pointWid, Color.white);
                            ChartHelper.DrawDoughnut(vh, p, pointWid - m_Line.tickness,
                                pointWid, 0, 360, colorList[i]);
                        }
                    }
                }
            }

            //draw tooltip line
            if (m_Tooltip.show && m_Tooltip.dataIndex > 0)
            {
                float splitWidth = m_XAxis.GetSplitWidth(coordinateWid);
                float px = zeroX + (m_Tooltip.dataIndex - 1) * splitWidth
                    + (m_XAxis.boundaryGap ? splitWidth / 2 : 0);
                Vector2 sp = new Vector2(px, coordinateY);
                Vector2 ep = new Vector2(px, coordinateY + coordinateHig);
                ChartHelper.DrawLine(vh, sp, ep, m_Coordinate.tickness, m_ThemeInfo.tooltipLineColor);
                if (m_Tooltip.crossLabel)
                {
                    sp = new Vector2(zeroX, m_Tooltip.pointerPos.y);
                    ep = new Vector2(zeroX + coordinateWid, m_Tooltip.pointerPos.y);
                    DrawSplitLine(vh, true, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);
                }
            }
        }

        private void DrawYCategory(VertexHelper vh)
        {
            var stackSeries = m_Series.GetStackSeries();
            int seriesCount = stackSeries.Count;
            float scaleWid = m_YAxis.GetDataWidth(coordinateHig);
            int serieCount = 0;
            List<Vector3> points = new List<Vector3>();
            List<Vector3> smoothPoints = new List<Vector3>();
            List<Color> colorList = new List<Color>();
            int dataCount = 0;
            for (int j = 0; j < seriesCount; j++)
            {
                var seriesCurrHig = new Dictionary<int, float>();
                var serieList = stackSeries[j];

                for (int n = 0; n < serieList.Count; n++)
                {
                    Serie serie = serieList[n];
                    if (!IsActive(serie.name)) continue;
                    List<Vector3> lastPoints = new List<Vector3>();
                    List<Vector3> lastSmoothPoints = new List<Vector3>();

                    Color color = m_ThemeInfo.GetColor(serieCount);
                    Vector3 lp = Vector3.zero;
                    Vector3 np = Vector3.zero;
                    float startY = zeroY + (m_YAxis.boundaryGap ? scaleWid / 2 : 0);
                    int maxCount = maxShowDataNumber > 0 ?
                        (maxShowDataNumber > serie.data.Count ? serie.data.Count : maxShowDataNumber)
                        : serie.data.Count;
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
                    for (int i = minShowDataNumber; i < maxCount; i++)
                    {
                        if (!seriesCurrHig.ContainsKey(i))
                        {
                            seriesCurrHig[i] = 0;
                        }
                        float value = serie.data[i];
                        float pY = startY + i * scaleWid;
                        float pX = seriesCurrHig[i] + zeroX + m_Coordinate.tickness;
                        float dataHig = value / (maxValue - minValue) * coordinateWid;
                        np = new Vector3(pX + dataHig, pY);

                        if (i > 0)
                        {
                            if (m_Line.smooth)
                            {
                                var list = ChartHelper.GetBezierListVertical(lp, np, m_Line.smoothStyle);
                                Vector3 start, to;
                                start = list[0];
                                for (int k = 1; k < list.Length; k++)
                                {
                                    smoothPoints.Add(list[k]);
                                    to = list[k];
                                    ChartHelper.DrawLine(vh, start, to, m_Line.tickness, color);

                                    if (m_Line.area)
                                    {
                                        Vector3 alp = new Vector3(start.x, start.y - m_Line.tickness);
                                        Vector3 anp = new Vector3(to.x, to.y - m_Line.tickness);
                                        Vector3 tnp = serieCount > 0 ?
                                            (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                            new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 1].x,
                                                lastSmoothPoints[lastSmoothPoints.Count - 1].y + m_Line.tickness) :
                                            new Vector3(lastSmoothPoints[smoothPointCount].x,
                                                lastSmoothPoints[smoothPointCount].y + m_Line.tickness)) :
                                            new Vector3(zeroX + m_Coordinate.tickness, to.y);
                                        Vector3 tlp = serieCount > 0 ?
                                            (smoothPointCount > lastSmoothPoints.Count - 1 ?
                                            new Vector3(lastSmoothPoints[lastSmoothPoints.Count - 2].x,
                                                lastSmoothPoints[lastSmoothPoints.Count - 2].y + m_Line.tickness) :
                                            new Vector3(lastSmoothPoints[smoothPointCount - 1].x,
                                                lastSmoothPoints[smoothPointCount - 1].y + m_Line.tickness)) :
                                            new Vector3(zeroX + m_Coordinate.tickness, start.y);
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
                                    var cross = ChartHelper.GetIntersection(lp, np, new Vector3(zeroX, zeroY),
                                        new Vector3(zeroX, zeroY + coordinateHig));
                                    if (cross == Vector3.zero)
                                    {
                                        Vector3 tnp = serieCount > 0 ?
                                            new Vector3(lastPoints[i].x, lastPoints[i].y + m_Line.tickness) :
                                            new Vector3(zeroX + m_Coordinate.tickness, np.y);
                                        Vector3 tlp = serieCount > 0 ?
                                            new Vector3(lastPoints[i - 1].x, lastPoints[i - 1].y + m_Line.tickness) :
                                            new Vector3(zeroX + m_Coordinate.tickness, lp.y);
                                        ChartHelper.DrawPolygon(vh, alp, anp, tnp, tlp, areaColor);
                                    }
                                    else
                                    {
                                        Vector3 cross1 = new Vector3(cross.x + (alp.x > zeroX ? m_Coordinate.tickness : -m_Coordinate.tickness), cross.y);
                                        Vector3 cross2 = new Vector3(cross.x + (anp.x > zeroX ? m_Coordinate.tickness : -m_Coordinate.tickness), cross.y);
                                        Vector3 xp1 = new Vector3(zeroX + (alp.x > zeroX ? m_Coordinate.tickness : -m_Coordinate.tickness), alp.y);
                                        Vector3 xp2 = new Vector3(zeroX + (anp.x > zeroX ? m_Coordinate.tickness : -m_Coordinate.tickness), anp.y);
                                        ChartHelper.DrawTriangle(vh, alp, cross1, xp1, areaColor);
                                        ChartHelper.DrawTriangle(vh, anp, cross2, xp2, areaColor);
                                    }
                                }
                            }
                        }
                        if (m_Line.point)
                        {
                            points.Add(np);
                            colorList.Add(color);
                        }
                        seriesCurrHig[i] += dataHig;
                        lp = np;
                    }
                    if (serie.show)
                    {
                        serieCount++;
                    }
                }
                // draw point
                if (m_Line.point)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        Vector3 p = points[i];
                        float pointWid = m_Line.pointWidth;
                        if (m_Tooltip.show && i % dataCount == m_Tooltip.dataIndex - 1)
                        {
                            pointWid = pointWid * 1.8f;
                        }
                        if (m_Theme == Theme.Dark)
                        {

                            ChartHelper.DrawCricle(vh, p, pointWid, colorList[i],
                                (int)m_Line.pointWidth * 5);
                        }
                        else
                        {
                            ChartHelper.DrawCricle(vh, p, pointWid, Color.white);
                            ChartHelper.DrawDoughnut(vh, p, pointWid - m_Line.tickness,
                                pointWid, 0, 360, colorList[i]);
                        }
                    }
                }
            }
            //draw tooltip line
            if (m_Tooltip.show && m_Tooltip.dataIndex > 0)
            {
                float splitWidth = m_YAxis.GetSplitWidth(coordinateHig);
                float pY = zeroY + (m_Tooltip.dataIndex - 1) * splitWidth + (m_YAxis.boundaryGap ? splitWidth / 2 : 0);
                Vector2 sp = new Vector2(coordinateX, pY);
                Vector2 ep = new Vector2(coordinateX + coordinateWid, pY);
                ChartHelper.DrawLine(vh, sp, ep, m_Coordinate.tickness, m_ThemeInfo.tooltipFlagAreaColor);
                if (m_Tooltip.crossLabel)
                {
                    sp = new Vector2(m_Tooltip.pointerPos.x,zeroY);
                    ep = new Vector2(m_Tooltip.pointerPos.x,zeroY + coordinateHig);
                    DrawSplitLine(vh, false, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);
                }
            }
        }
    }
}
