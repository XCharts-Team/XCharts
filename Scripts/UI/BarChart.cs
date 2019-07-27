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

            public static Bar defaultBar
            {
                get
                {
                    return new Bar()
                    {
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
            m_Tooltip.type = Tooltip.Type.Shadow;
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
            Serie serie, Color color, ref List<float> seriesHig)
        {
            if (!IsActive(serie.name)) return;
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
                if ((m_Tooltip.show && m_Tooltip.IsSelectedDataIndex(i))
                    || serie.data[i].highlighted
                    || serie.highlighted)
                {
                    color *= 1.05f;
                }
                if (serie.show)
                {
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                }
            }
        }

        private void DrawXBarSerie(VertexHelper vh, int serieIndex, int stackCount,
            Serie serie, Color color, ref List<float> seriesHig)
        {
            if (!IsActive(serie.name)) return;
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
                if ((m_Tooltip.show && m_Tooltip.IsSelectedDataIndex(i))
                    || serie.data[i].highlighted
                    || serie.highlighted)
                {
                    color *= 1.05f;
                }
                if (serie.show)
                {
                    ChartHelper.DrawPolygon(vh, p1, p2, p3, p4, color);
                }
            }
        }

        private HashSet<string> serieNameSet = new HashSet<string>();
        private Dictionary<int, List<Serie>> stackSeries = new Dictionary<int, List<Serie>>();
        private List<float> seriesCurrHig = new List<float>();
        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            bool yCategory = m_YAxises[0].IsCategory() || m_YAxises[1].IsCategory();
            m_Series.GetStackSeries(ref stackSeries);
            int seriesCount = stackSeries.Count;
            int serieNameCount = -1;
            serieNameSet.Clear();
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
                    if (yCategory) DrawYBarSerie(vh, j, seriesCount, serie, color, ref seriesCurrHig);
                    else DrawXBarSerie(vh, j, seriesCount, serie, color, ref seriesCurrHig);

                }
            }
            if (yCategory) DrawYTooltipIndicator(vh);
            else DrawXTooltipIndicator(vh);
        }
    }
}
