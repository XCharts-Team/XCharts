/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(HeatmapHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight)]
    public class Heatmap : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
        public static void AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Heatmap>(serieName);
            serie.itemStyle.show = true;
            serie.itemStyle.borderWidth = 1;
            serie.itemStyle.borderColor = Color.clear;
            serie.emphasis.show = true;
            serie.emphasis.itemStyle.show = true;
            serie.emphasis.itemStyle.borderWidth = 1;
            serie.emphasis.itemStyle.borderColor = Color.black;
        }
    }
}