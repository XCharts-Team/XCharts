using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(EffectScatterHandler), true)]
    [CoordOptions(typeof(GridCoord), typeof(SingleAxisCoord))]
    [SerieExtraComponent(typeof(LabelStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraField("m_Radius")]
    public class EffectScatter : BaseScatter
    {
        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<EffectScatter>(serieName);
            serie.symbol.show = true;
            serie.symbol.type = SymbolType.Circle;
            serie.itemStyle.opacity = 0.8f;
            serie.clip = false;
            for (int i = 0; i < 10; i++)
            {
                chart.AddData(serie.index, Random.Range(10, 100), Random.Range(10, 100));
            }
            return serie;
        }
    }
}