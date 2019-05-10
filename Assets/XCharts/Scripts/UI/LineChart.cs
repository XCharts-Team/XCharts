using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public class LineChart : CoordinateChart
    {
        [SerializeField] private Line m_Line = Line.defaultLine;

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
            int seriesCount = m_Series.Count;
            float max = GetMaxValue();
            float scaleWid = m_XAxis.GetDataWidth(coordinateWid);
            for (int j = 0; j < seriesCount; j++)
            {
                if (!m_Legend.IsShowSeries(j)) continue;
                Serie serie = m_Series.series[j];
                Color32 color = m_ThemeInfo.GetColor(j);
                Vector3 lp = Vector3.zero;
                Vector3 np = Vector3.zero;
                float startX = zeroX + (m_XAxis.boundaryGap ? scaleWid / 2 : 0);

                int maxCount = maxShowDataNumber > 0 ?
                            (maxShowDataNumber > serie.data.Count ? serie.data.Count : maxShowDataNumber)
                            : serie.data.Count;
                for (int i = minShowDataNumber; i < maxCount; i++)
                {
                    float value = serie.data[i];

                    np = new Vector3(startX + i * scaleWid, zeroY + value * coordinateHig / max);
                    if (i > 0)
                    {
                        if (m_Line.smooth)
                        {
                            var list = ChartHelper.GetBezierList(lp, np, m_Line.smoothStyle);
                            Vector3 start, to;
                            start = list[0];
                            for (int k = 1; k < list.Length; k++)
                            {
                                to = list[k];
                                ChartHelper.DrawLine(vh, start, to, m_Line.tickness, color);
                                start = to;
                            }
                        }
                        else
                        {
                            ChartHelper.DrawLine(vh, lp, np, m_Line.tickness, color);
                            if (m_Line.area)
                            {
                                ChartHelper.DrawPolygon(vh, lp, np, new Vector3(np.x, zeroY),
                                    new Vector3(lp.x, zeroY), color);
                            }
                        }

                    }
                    lp = np;
                }
                // draw point
                if (m_Line.point)
                {
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        float value = serie.data[i];

                        Vector3 p = new Vector3(startX + i * scaleWid,
                            zeroY + value * coordinateHig / max);
                        float pointWid = m_Line.pointWidth;
                        if (m_Tooltip.show && i == m_Tooltip.dataIndex - 1)
                        {
                            pointWid = pointWid * 1.8f;
                        }
                        if (m_Theme == Theme.Dark)
                        {

                            ChartHelper.DrawCricle(vh, p, pointWid, color,
                                (int)m_Line.pointWidth * 5);
                        }
                        else
                        {
                            ChartHelper.DrawCricle(vh, p, pointWid, Color.white);
                            ChartHelper.DrawDoughnut(vh, p, pointWid - m_Line.tickness,
                                pointWid, 0, 360, color);
                        }
                    }
                }
            }
            //draw tooltip line
            if (m_Tooltip.show && m_Tooltip.dataIndex > 0)
            {
                float splitWidth = m_XAxis.GetSplitWidth(coordinateWid);
                float px = zeroX + (m_Tooltip.dataIndex - 1) * splitWidth + (m_XAxis.boundaryGap ? splitWidth / 2 : 0);
                Vector2 sp = new Vector2(px, zeroY);
                Vector2 ep = new Vector2(px, zeroY + coordinateHig);
                ChartHelper.DrawLine(vh, sp, ep, m_Coordinate.tickness, m_ThemeInfo.tooltipFlagAreaColor);
            }
        }
    }
}
