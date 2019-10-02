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
            AddSerie(SerieType.Line, "serie1");
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
            if (!m_CheckMinMaxValue) return;
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
        private void DrawLineChart(VertexHelper vh, bool yCategory)
        {
            m_BarLastOffset = 0;
            m_Series.GetStackSeries(ref m_StackSeries);
            int seriesCount = m_StackSeries.Count;
            int serieCount = 0;
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
                    switch (serie.type)
                    {
                        case SerieType.Line:
                            if (yCategory) DrawYLineSerie(vh, serieCount, serie, ref m_SeriesCurrHig);
                            else DrawXLineSerie(vh, serieCount, serie, ref m_SeriesCurrHig);
                            break;
                        case SerieType.Bar:
                            if (yCategory) DrawYBarSerie(vh, serieCount, serie, serieCount, ref m_SeriesCurrHig);
                            else DrawXBarSerie(vh, serieCount, serie, serieCount, ref m_SeriesCurrHig);
                            break;
                    }

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
    }
}
