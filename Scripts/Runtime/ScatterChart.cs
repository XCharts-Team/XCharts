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
    }
}
