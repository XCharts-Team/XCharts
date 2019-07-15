using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/BarChart", 14)]
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

            public static Bar defaultBar{
                get{
                    return new Bar(){
                        m_InSameBar = false,
                        m_BarWidth = 0.6f,
                        m_Space = 10
                    };
                }
            }
        }

        [SerializeField] private Bar m_Bar = Bar.defaultBar;

        public Bar bar { get { return m_Bar; } }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Bar = Bar.defaultBar;
            m_Title.text = "BarChart";
            RemoveData();
            AddSerie("serie1", SerieType.Line);
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
                AddData(0, Random.Range(10, 90));
            }
        }
#endif

        private void DrawYBarSerie(VertexHelper vh, int serieIndex, int stackCount,
            Serie serie, Color color, ref Dictionary<int, float> seriesHig)
        {
            if (!m_Legend.IsActive(serie.name)) return;
            var xAxis = m_XAxises[serie.axisIndex];
            var yAxis = m_YAxises[serie.axisIndex];
            if (!yAxis.show) yAxis = m_YAxises[(serie.axisIndex + 1) % m_YAxises.Count];
            float scaleWid = yAxis.GetDataWidth(coordinateHig, m_DataZoom);
            float barWid = m_Bar.barWidth > 1 ? m_Bar.barWidth : scaleWid * m_Bar.barWidth;
            float offset = m_Bar.inSameBar ?
                (scaleWid - barWid - m_Bar.space * (stackCount - 1)) / 2 :
                (scaleWid - barWid * stackCount - m_Bar.space * (stackCount - 1)) / 2;
            int maxCount = maxShowDataNumber > 0 ?
                (maxShowDataNumber > serie.yData.Count ? serie.yData.Count : maxShowDataNumber)
                : serie.yData.Count;
            for (int i = minShowDataNumber; i < maxCount; i++)
            {
                if (!seriesHig.ContainsKey(i))
                {
                    seriesHig[i] = 0;
                }
                float value = serie.yData[i];
                float pX = seriesHig[i] + coordinateX + xAxis.zeroXOffset + m_Coordinate.tickness;
                float pY = coordinateY + +i * scaleWid;
                if (!yAxis.boundaryGap) pY -= scaleWid / 2;
                float barHig = (xAxis.minValue > 0 ? value - xAxis.minValue : value)
                    / (xAxis.maxValue - xAxis.minValue) * coordinateWid;
                float space = m_Bar.inSameBar ? offset :
                    offset + serieIndex * (barWid + m_Bar.space);
                seriesHig[i] += barHig;
                Vector3 p1 = new Vector3(pX, pY + space + barWid);
                Vector3 p2 = new Vector3(pX + barHig, pY + space + barWid);
                Vector3 p3 = new Vector3(pX + barHig, pY + space);
                Vector3 p4 = new Vector3(pX, pY + space);
                if (serie.show)
                {
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                }
            }
        }

        private void DrawXBarSerie(VertexHelper vh, int serieIndex, int stackCount,
            Serie serie, Color color, ref Dictionary<int, float> seriesHig)
        {
            if (!m_Legend.IsActive(serie.name)) return;
            List<float> showData = serie.GetYDataList(m_DataZoom);
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            if (!xAxis.show) xAxis = m_XAxises[(serie.axisIndex + 1) % m_XAxises.Count];
            float scaleWid = xAxis.GetDataWidth(coordinateWid, m_DataZoom);
            float barWid = m_Bar.barWidth > 1 ? m_Bar.barWidth : scaleWid * m_Bar.barWidth;
            float offset = m_Bar.inSameBar ?
                (scaleWid - barWid - m_Bar.space * (stackCount - 1)) / 2 :
                (scaleWid - barWid * stackCount - m_Bar.space * (stackCount - 1)) / 2;
            int maxCount = maxShowDataNumber > 0 ?
                (maxShowDataNumber > showData.Count ? showData.Count : maxShowDataNumber)
                : showData.Count;
            for (int i = minShowDataNumber; i < maxCount; i++)
            {
                if (!seriesHig.ContainsKey(i))
                {
                    seriesHig[i] = 0;
                }
                float value = showData[i];
                float pX = coordinateX + i * scaleWid;
                float zeroY = coordinateY + yAxis.zeroYOffset;
                if (!xAxis.boundaryGap) pX -= scaleWid / 2;
                float pY = seriesHig[i] + zeroY + m_Coordinate.tickness;
                float barHig = (yAxis.minValue > 0 ? value - yAxis.minValue : value)
                    / (yAxis.maxValue - yAxis.minValue) * coordinateHig;
                seriesHig[i] += barHig;
                float space = m_Bar.inSameBar ? offset :
                    offset + serieIndex * (barWid + m_Bar.space);
                Vector3 p1 = new Vector3(pX + space, pY);
                Vector3 p2 = new Vector3(pX + space, pY + barHig);
                Vector3 p3 = new Vector3(pX + space + barWid, pY + barHig);
                Vector3 p4 = new Vector3(pX + space + barWid, pY);
                if (serie.show)
                {
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                }
            }
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            if (m_YAxises[0].type == Axis.AxisType.Category)
            {
                var stackSeries = m_Series.GetStackSeries();
                int seriesCount = stackSeries.Count;
                int serieCount = 0;
                for (int j = 0; j < seriesCount; j++)
                {
                    var seriesHig = new Dictionary<int, float>();
                    var serieList = stackSeries[j];
                    for (int n = 0; n < serieList.Count; n++)
                    {
                        Serie serie = serieList[n];
                        Color color = m_ThemeInfo.GetColor(serieCount);
                        DrawYBarSerie(vh, j, seriesCount, serie, color, ref seriesHig);
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
                        for (int i = 0; i < m_YAxises.Count; i++)
                        {
                            var yAxis = m_YAxises[i];
                            if (!yAxis.show) continue;
                            Vector3 sp = new Vector2(m_Tooltip.pointerPos.x, coordinateY);
                            Vector3 ep = new Vector2(m_Tooltip.pointerPos.x, coordinateY + coordinateHig);
                            DrawSplitLine(vh, false, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);
                            float splitWidth = yAxis.GetSplitWidth(coordinateHig, m_DataZoom);
                            float pY = coordinateY + (m_Tooltip.yValues[i] - 1) * splitWidth +
                                (yAxis.boundaryGap ? splitWidth / 2 : 0);
                            sp = new Vector2(coordinateX, pY);
                            ep = new Vector2(coordinateX + coordinateWid, pY);
                            DrawSplitLine(vh, true, Axis.SplitLineType.Solid, sp, ep, m_ThemeInfo.tooltipLineColor);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < m_YAxises.Count; i++)
                        {
                            var yAxis = m_YAxises[i];
                            if (!yAxis.show) continue;
                            float splitWidth = yAxis.GetSplitWidth(coordinateHig, m_DataZoom);
                            float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                            float pX = coordinateX + coordinateWid;
                            float pY = coordinateY + splitWidth * (m_Tooltip.yValues[i] - 1) -
                                (yAxis.boundaryGap ? 0 : splitWidth / 2);
                            Vector3 p1 = new Vector3(coordinateX, pY);
                            Vector3 p2 = new Vector3(coordinateX, pY + tooltipSplitWid);
                            Vector3 p3 = new Vector3(pX, pY + tooltipSplitWid);
                            Vector3 p4 = new Vector3(pX, pY);
                            ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, m_ThemeInfo.tooltipFlagAreaColor);
                        }
                    }
                }
            }
            else
            {
                var stackSeries = m_Series.GetStackSeries();
                int seriesCount = stackSeries.Count;
                int serieCount = 0;
                for (int j = 0; j < seriesCount; j++)
                {
                    var seriesHig = new Dictionary<int, float>();
                    var serieList = stackSeries[j];
                    for (int n = 0; n < serieList.Count; n++)
                    {
                        Serie serie = serieList[n];
                        Color color = m_ThemeInfo.GetColor(serieCount);
                        DrawXBarSerie(vh, j, seriesCount, serie, color, ref seriesHig);
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
                        for (int i = 0; i < m_XAxises.Count; i++)
                        {
                            var xAxis = m_XAxises[i];
                            if (!xAxis.show) continue;
                            Vector3 sp = new Vector2(coordinateX, m_Tooltip.pointerPos.y);
                            Vector3 ep = new Vector2(coordinateX + coordinateWid, m_Tooltip.pointerPos.y);
                            DrawSplitLine(vh, true, Axis.SplitLineType.Dashed, sp, ep, m_ThemeInfo.tooltipLineColor);

                            float splitWidth = xAxis.GetSplitWidth(coordinateWid, m_DataZoom);
                            float px = coordinateX + (m_Tooltip.xValues[i] - 1) * splitWidth
                                + (xAxis.boundaryGap ? splitWidth / 2 : 0);
                            sp = new Vector2(px, coordinateY);
                            ep = new Vector2(px, coordinateY + coordinateHig);
                            DrawSplitLine(vh, false, Axis.SplitLineType.Solid, sp, ep, m_ThemeInfo.tooltipLineColor);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < m_XAxises.Count; i++)
                        {
                            var xAxis = m_XAxises[i];
                            if (!xAxis.show) continue;
                            float splitWidth = xAxis.GetSplitWidth(coordinateWid, m_DataZoom);
                            float tooltipSplitWid = splitWidth < 1 ? 1 : splitWidth;
                            float pX = coordinateX + splitWidth * (m_Tooltip.xValues[i] - 1) -
                                (xAxis.boundaryGap ? 0 : splitWidth / 2);
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
}
