using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/BarChart", 13)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class BarChart : CoordinateChart
    {
        [System.Serializable]
        public class Bar
        {
            [SerializeField] private bool m_InSameBar;
            [SerializeField] private float m_BarWidth = 0.7f;
            [SerializeField] private float m_Space;

            public bool inSameBar { get { return m_InSameBar; } set { m_InSameBar = value; } }
            public float barWidth { get { return m_BarWidth; } set { m_BarWidth = value; } }
            public float space { get { return m_Space; } set { m_Space = value; } }
        }

        [SerializeField] private Bar m_Bar = new Bar();

        public Bar bar { get { return m_Bar; } }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            if (m_YAxis.type == Axis.AxisType.Category)
            {
                var stackSeries = m_Series.GetStackSeries();
                int seriesCount = stackSeries.Count;
                float scaleWid = m_YAxis.GetDataWidth(coordinateHig, m_DataZoom);
                float barWid = m_Bar.barWidth > 1 ? m_Bar.barWidth : scaleWid * m_Bar.barWidth;
                float offset = m_Bar.inSameBar ?
                    (scaleWid - barWid - m_Bar.space * (seriesCount - 1)) / 2 :
                    (scaleWid - barWid * seriesCount - m_Bar.space * (seriesCount - 1)) / 2;
                int serieCount = 0;
                for (int j = 0; j < seriesCount; j++)
                {
                    var seriesCurrHig = new Dictionary<int, float>();
                    var serieList = stackSeries[j];
                    for (int n = 0; n < serieList.Count; n++)
                    {
                        Serie serie = serieList[n];
                        if (!m_Legend.IsActive(serie.name)) continue;
                        Color color = m_ThemeInfo.GetColor(serieCount);
                        int maxCount = maxShowDataNumber > 0 ?
                            (maxShowDataNumber > serie.data.Count ? serie.data.Count : maxShowDataNumber)
                            : serie.data.Count;
                        for (int i = minShowDataNumber; i < maxCount; i++)
                        {
                            if (!seriesCurrHig.ContainsKey(i))
                            {
                                seriesCurrHig[i] = 0;
                            }
                            float value = serie.data[i];
                            float pX = seriesCurrHig[i] + zeroX + m_Coordinate.tickness;
                            float pY = coordinateY + i * scaleWid;
                            if (!m_YAxis.boundaryGap) pY -= scaleWid / 2;
                            float barHig = (minValue > 0 ? value - minValue : value)
                                / (maxValue - minValue) * coordinateWid;
                            float space = m_Bar.inSameBar ? offset :
                                offset + j * (barWid + m_Bar.space);
                            seriesCurrHig[i] += barHig;
                            Vector3 p1 = new Vector3(pX, pY + space + barWid);
                            Vector3 p2 = new Vector3(pX + barHig, pY + space + barWid);
                            Vector3 p3 = new Vector3(pX + barHig, pY + space);
                            Vector3 p4 = new Vector3(pX, pY + space);
                            if (serie.show)
                            {
                                ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                            }
                        }
                        if (serie.show)
                        {
                            serieCount++;
                        }
                    }
                }
                if (m_Tooltip.show && m_Tooltip.dataIndex > 0)
                {
                    if (m_Tooltip.crossLabel)
                    {
                        Vector3 sp = new Vector2(m_Tooltip.pointerPos.x, zeroY);
                        Vector3 ep = new Vector2(m_Tooltip.pointerPos.x, zeroY + coordinateHig);
                        DrawSplitLine(vh, false, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);
                        float splitWidth = m_YAxis.GetSplitWidth(coordinateHig, m_DataZoom);
                        float pY = zeroY + (m_Tooltip.dataIndex - 1) * splitWidth +
                            (m_YAxis.boundaryGap ? splitWidth / 2 : 0);
                        sp = new Vector2(coordinateX, pY);
                        ep = new Vector2(coordinateX + coordinateWid, pY);
                        DrawSplitLine(vh, true, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);
                    }
                    else
                    {
                        float tooltipSplitWid = scaleWid < 1 ? 1 : scaleWid;
                        float pX = coordinateX + coordinateWid;
                        float pY = coordinateY + scaleWid * (m_Tooltip.dataIndex - 1) -
                            (m_YAxis.boundaryGap ? 0 : scaleWid / 2);
                        Vector3 p1 = new Vector3(coordinateX, pY);
                        Vector3 p2 = new Vector3(coordinateX, pY + tooltipSplitWid);
                        Vector3 p3 = new Vector3(pX, pY + tooltipSplitWid);
                        Vector3 p4 = new Vector3(pX, pY);
                        ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.tooltipFlagAreaColor);
                    }
                }
            }
            else
            {
                var stackSeries = m_Series.GetStackSeries();
                int seriesCount = stackSeries.Count;
                float scaleWid = m_XAxis.GetDataWidth(coordinateWid, m_DataZoom);
                float barWid = m_Bar.barWidth > 1 ? m_Bar.barWidth : scaleWid * m_Bar.barWidth;
                float offset = m_Bar.inSameBar ?
                    (scaleWid - barWid - m_Bar.space * (seriesCount - 1)) / 2 :
                    (scaleWid - barWid * seriesCount - m_Bar.space * (seriesCount - 1)) / 2;
                int serieCount = 0;
                for (int j = 0; j < seriesCount; j++)
                {
                    var seriesCurrHig = new Dictionary<int, float>();
                    var serieList = stackSeries[j];
                    for (int n = 0; n < serieList.Count; n++)
                    {
                        Serie serie = serieList[n];
                        if (!m_Legend.IsActive(serie.name)) continue;
                        Color color = m_ThemeInfo.GetColor(serieCount);
                        List<float> showData = serie.GetData(m_DataZoom);
                        int maxCount = maxShowDataNumber > 0 ?
                            (maxShowDataNumber > showData.Count ? showData.Count : maxShowDataNumber)
                            : showData.Count;
                        for (int i = minShowDataNumber; i < maxCount; i++)
                        {
                            if (!seriesCurrHig.ContainsKey(i))
                            {
                                seriesCurrHig[i] = 0;
                            }
                            float value = showData[i];
                            float pX = zeroX + i * scaleWid;
                            if (!m_XAxis.boundaryGap) pX -= scaleWid / 2;
                            float pY = seriesCurrHig[i] + zeroY + m_Coordinate.tickness;
                            float barHig = (minValue > 0 ? value - minValue : value)
                                / (maxValue - minValue) * coordinateHig;
                            seriesCurrHig[i] += barHig;
                            float space = m_Bar.inSameBar ? offset :
                                offset + j * (barWid + m_Bar.space);
                            Vector3 p1 = new Vector3(pX + space, pY);
                            Vector3 p2 = new Vector3(pX + space, pY + barHig);
                            Vector3 p3 = new Vector3(pX + space + barWid, pY + barHig);
                            Vector3 p4 = new Vector3(pX + space + barWid, pY);
                            if (serie.show)
                            {
                                ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                            }
                        }
                        if (serie.show)
                        {
                            serieCount++;
                        }
                    }
                }
                if (m_Tooltip.show && m_Tooltip.dataIndex > 0)
                {
                    if (m_Tooltip.crossLabel)
                    {
                        Vector3 sp = new Vector2(zeroX, m_Tooltip.pointerPos.y);
                        Vector3 ep = new Vector2(zeroX + coordinateWid, m_Tooltip.pointerPos.y);
                        DrawSplitLine(vh, true, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);

                        float splitWidth = m_XAxis.GetSplitWidth(coordinateWid, m_DataZoom);
                        float px = zeroX + (m_Tooltip.dataIndex - 1) * splitWidth
                            + (m_XAxis.boundaryGap ? splitWidth / 2 : 0);
                        sp = new Vector2(px, coordinateY);
                        ep = new Vector2(px, coordinateY + coordinateHig);
                        DrawSplitLine(vh, false, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);
                    }
                    else
                    {
                        float tooltipSplitWid = scaleWid < 1 ? 1 : scaleWid;
                        float pX = coordinateX + scaleWid * (m_Tooltip.dataIndex - 1) -
                            (m_XAxis.boundaryGap ? 0 : scaleWid / 2);
                        float pY = coordinateY + coordinateHig;
                        Vector3 p1 = new Vector3(pX, coordinateY);
                        Vector3 p2 = new Vector3(pX, pY);
                        Vector3 p3 = new Vector3(pX + tooltipSplitWid, pY);
                        Vector3 p4 = new Vector3(pX + tooltipSplitWid, coordinateY);
                        ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.tooltipFlagAreaColor);
                    }
                }
            }
        }
    }
}
