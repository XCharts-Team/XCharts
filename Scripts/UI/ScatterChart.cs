using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/ScatterChart", 17)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class ScatterChart : CoordinateChart
    {
        private float m_EffectScatterSpeed = 15;
        private float m_EffectScatterSize;
        private float m_EffectScatterAplha;

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "ScatterChart";
            m_Tooltip.type = Tooltip.Type.None;
            m_XAxises[0].type = Axis.AxisType.Value;
            m_XAxises[0].boundaryGap = false;
            m_YAxises[1].type = Axis.AxisType.Value;
            m_XAxises[1].boundaryGap = false;
            RemoveData();
            AddSerie(SerieType.Scatter, "serie1");
            for (int i = 0; i < 10; i++)
            {
                AddData(0, Random.Range(10, 100), Random.Range(10, 100));
            }
        }
#endif

        protected override void Update()
        {
            base.Update();
            bool hasEffectScatter = false;
            foreach (var serie in m_Series.list)
            {
                if (serie.type == SerieType.EffectScatter)
                {
                    hasEffectScatter = true;
                    for (int i = 0; i < serie.symbol.animationSize.Count; ++i)
                    {
                        serie.symbol.animationSize[i] += m_EffectScatterSpeed * Time.deltaTime;
                        if (serie.symbol.animationSize[i] > serie.symbol.size)
                        {
                            serie.symbol.animationSize[i] = i * 5;
                        }
                    }
                }
            }
            if (hasEffectScatter)
            {
                RefreshChart();
            }
        }

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
                        case SerieType.Scatter:
                        case SerieType.EffectScatter:
                            DrawScatterSerie(vh, serieNameCount, serie);
                            if (vh.currentVertCount > 60000)
                            {
                                m_Large++;
                                RefreshChart();
                                return;
                            }
                            break;
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
