using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(HeatmapHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight)]
    [RequireChartComponent(typeof(VisualMap))]
    [SerieExtraComponent(typeof(LabelStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
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

            var emphasis = serie.AddExtraComponent<EmphasisStyle>();
            emphasis.show = true;
            emphasis.itemStyle.show = true;
            emphasis.itemStyle.borderWidth = 1;
            emphasis.itemStyle.borderColor = Color.black;
            return serie;
        }
    }
}