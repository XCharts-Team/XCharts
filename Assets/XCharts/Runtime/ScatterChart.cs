/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;

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
            var serie = AddSerie(SerieType.Scatter, "serie1");
            serie.symbol.show = true;
            serie.symbol.type = SerieSymbolType.Circle;
            serie.itemStyle.opacity = 0.8f;
            serie.clip = false;
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

        protected override void CheckTootipArea(Vector2 local)
        {
            base.CheckTootipArea(local);
            m_Tooltip.ClearSerieDataIndex();
            bool selected = false;
            foreach (var serie in m_Series.list)
            {
                if (!serie.show) continue;
                if (serie.type != SerieType.Scatter && serie.type != SerieType.EffectScatter) continue;
                bool refresh = false;
                var dataCount = serie.data.Count;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                    if (!symbol.ShowSymbol(j, dataCount)) continue;
                    var dist = Vector3.Distance(local, serieData.runtimePosition);
                    if (dist <= symbol.size)
                    {
                        serieData.selected = true;
                        m_Tooltip.AddSerieDataIndex(serie.index, j);
                        selected = true;
                    }
                    else
                    {
                        serieData.selected = false;
                    }
                }
                if (refresh) RefreshChart();
            }
            if (selected)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                UpdateTooltip();
            }
            else if (m_Tooltip.IsActive())
            {
                m_Tooltip.SetActive(false);
                RefreshChart();
            }
        }

        protected override void UpdateTooltip()
        {
            base.UpdateTooltip();
            if (m_Tooltip.isAnySerieDataIndex())
            {
                var content = TooltipHelper.GetFormatterContent(m_Tooltip, 0, m_Series, m_ThemeInfo);
                TooltipHelper.SetContentAndPosition(tooltip, content, chartRect);
                m_Tooltip.SetActive(true);
            }
            else
            {
                m_Tooltip.SetActive(false);
            }
        }
    }
}
