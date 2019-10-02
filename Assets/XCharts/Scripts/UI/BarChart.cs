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

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "BarChart";
            m_Tooltip.type = Tooltip.Type.Shadow;
            RemoveData();
            AddSerie(SerieType.Bar, "serie1");
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
                AddData(0, Random.Range(10, 90));
            }
        }
#endif

        protected HashSet<string> m_SerieNameSet = new HashSet<string>();
        protected Dictionary<int, List<Serie>> m_StackSeries = new Dictionary<int, List<Serie>>();
        protected List<float> m_SeriesCurrHig = new List<float>();
        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            if (!m_CheckMinMaxValue) return;
            bool yCategory = m_YAxises[0].IsCategory() || m_YAxises[1].IsCategory();
            m_Series.GetStackSeries(ref m_StackSeries);
            int seriesCount = m_StackSeries.Count;
            int serieNameCount = -1;
            m_SerieNameSet.Clear();
            m_BarLastOffset = 0;
            for (int j = 0; j < seriesCount; j++)
            {
                var serieList = m_StackSeries[j];
                m_SeriesCurrHig.Clear();
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
                    switch (serie.type)
                    {
                        case SerieType.Line:
                            if (yCategory) DrawYLineSerie(vh, j, serie, ref m_SeriesCurrHig);
                            else DrawXLineSerie(vh, j, serie, ref m_SeriesCurrHig);
                            break;
                        case SerieType.Bar:
                            if (yCategory) DrawYBarSerie(vh, j, serie, serieNameCount, ref m_SeriesCurrHig);
                            else DrawXBarSerie(vh, j, serie, serieNameCount, ref m_SeriesCurrHig);
                            break;
                    }
                }
            }
            DrawLabelBackground(vh);
            DrawLinePoint(vh);
            DrawLineArrow(vh);
            if (yCategory) DrawYTooltipIndicator(vh);
            else DrawXTooltipIndicator(vh);
        }

    }
}
