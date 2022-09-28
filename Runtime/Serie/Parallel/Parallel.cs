using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(ParallelHandler), true)]
    [RequireChartComponent(typeof(ParallelCoord))]
    [SerieExtraComponent(typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraComponent(typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraField()]
    public class Parallel : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Parallel>(serieName);
            serie.lineStyle.width = 0.8f;
            serie.lineStyle.opacity = 0.6f;

            for (int i = 0; i < 100; i++)
            {
                var data = new List<double>()
                {
                    Random.Range(0f, 50f),
                    Random.Range(0f, 100f),
                    Random.Range(0f, 1000f),
                    Random.Range(0, 5),
                };
                serie.AddData(data, "data" + i);
            }
            chart.RefreshChart();
            return serie;
        }
    }
}