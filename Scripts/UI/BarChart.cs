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
            [SerializeField] private float m_BarWidth = 0.7f;
            [SerializeField] private float m_Space;

            public float barWidth { get { return m_BarWidth; } set { m_BarWidth = value; } }
            public float space { get { return m_Space; } set { m_Space = value; } }
        }

        [SerializeField] private Bar m_Bar = new Bar();

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            if (m_YAxis.type == Axis.AxisType.Category)
            {
                var stackSeries = m_Series.GetStackSeries();
                int seriesCount = stackSeries.Count;
                float scaleWid = m_YAxis.GetDataWidth(coordinateHig);
                float barWid = m_Bar.barWidth > 1 ? m_Bar.barWidth : scaleWid * m_Bar.barWidth;
                float offset = (scaleWid - barWid * seriesCount - m_Bar.space * (seriesCount - 1)) / 2;
                int serieCount = 0;
                for (int j = 0; j < seriesCount; j++)
                {
                    var seriesCurrHig = new Dictionary<int, float>();
                    var serieList = stackSeries[j];
                    for (int n = 0; n < serieList.Count; n++)
                    {
                        Serie serie = serieList[n];
                        if (!m_Legend.IsShowSeries(serie.name)) continue;
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
                            float data = serie.data[i];
                            float pX = seriesCurrHig[i] + zeroX + m_Coordinate.tickness;
                            float pY = zeroY + i * scaleWid;
                            if (!m_YAxis.boundaryGap) pY -= scaleWid / 2;
                            float barHig = data / maxValue * coordinateWid;
                            float space = offset + j * (barWid + m_Bar.space);
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
                    float tooltipSplitWid = scaleWid < 1 ? 1 : scaleWid;
                    float pX = zeroX + coordinateWid;
                    float pY = zeroY + scaleWid * (m_Tooltip.dataIndex - 1) - (m_YAxis.boundaryGap ? 0 : scaleWid / 2);
                    Vector3 p1 = new Vector3(zeroX, pY);
                    Vector3 p2 = new Vector3(zeroX, pY + tooltipSplitWid);
                    Vector3 p3 = new Vector3(pX, pY + tooltipSplitWid);
                    Vector3 p4 = new Vector3(pX, pY);
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.tooltipFlagAreaColor);
                }
            }
            else
            {
                var stackSeries = m_Series.GetStackSeries();
                int seriesCount = stackSeries.Count;
                float scaleWid = m_XAxis.GetDataWidth(coordinateWid);
                float barWid = m_Bar.barWidth > 1 ? m_Bar.barWidth : scaleWid * m_Bar.barWidth;
                float offset = (scaleWid - barWid * seriesCount - m_Bar.space * (seriesCount - 1)) / 2;
                int serieCount = 0;
                for (int j = 0; j < seriesCount; j++)
                {
                    var seriesCurrHig = new Dictionary<int, float>();
                    var serieList = stackSeries[j];
                    for (int n = 0; n < serieList.Count; n++)
                    {
                        Serie serie = serieList[n];
                        if (!m_Legend.IsShowSeries(serie.name)) continue;
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
                            float data = serie.data[i];
                            float pX = zeroX + i * scaleWid;
                            if (!m_XAxis.boundaryGap) pX -= scaleWid / 2;
                            float pY = seriesCurrHig[i] + zeroY + m_Coordinate.tickness;
                            float barHig = data / maxValue * coordinateHig;
                            seriesCurrHig[i] += barHig;
                            float space = offset + j * (barWid + m_Bar.space);
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
                    float tooltipSplitWid = scaleWid < 1 ? 1 : scaleWid;
                    float pX = zeroX + scaleWid * (m_Tooltip.dataIndex - 1) - (m_XAxis.boundaryGap?0:scaleWid/2);
                    float pY = zeroY + coordinateHig;
                    Vector3 p1 = new Vector3(pX, zeroY);
                    Vector3 p2 = new Vector3(pX, pY);
                    Vector3 p3 = new Vector3(pX + tooltipSplitWid, pY);
                    Vector3 p4 = new Vector3(pX + tooltipSplitWid, zeroY);
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.tooltipFlagAreaColor);
                }
            }
        }
    }
}
