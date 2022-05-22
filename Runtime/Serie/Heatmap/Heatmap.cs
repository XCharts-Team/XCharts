using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(HeatmapHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight)]
    [RequireChartComponent(typeof(VisualMap))]
    [SerieExtraComponent(typeof(LabelStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraField()]
    public class Heatmap : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Heatmap>(serieName);
            serie.itemStyle.show = true;
            serie.itemStyle.borderWidth = 1;
            serie.itemStyle.borderColor = Color.clear;

            var emphasis = serie.AddExtraComponent<EmphasisItemStyle>();
            emphasis.show = true;
            emphasis.borderWidth = 1;
            emphasis.borderColor = Color.black;
            return serie;
        }
    }
}