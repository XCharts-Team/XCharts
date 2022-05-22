using UnityEngine;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class EffectScatterHandler : BaseScatterHandler<EffectScatter>
    {
        private float m_EffectScatterSpeed = 15;

        public override void Update()
        {
            base.Update();
            var symbolSize = serie.symbol.GetSize(null, chart.theme.serie.scatterSymbolSize);
            for (int i = 0; i < serie.symbol.animationSize.Count; ++i)
            {
                serie.symbol.animationSize[i] += m_EffectScatterSpeed * Time.deltaTime;
                if (serie.symbol.animationSize[i] > symbolSize)
                {
                    serie.symbol.animationSize[i] = i * 5;
                }
                chart.RefreshPainter(serie);
            }
        }
    }
}