using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [Serializable]
    [SerieHandler(typeof(SimplifiedLineHandler), true)]
    [SerieConvert(typeof(SimplifiedBar), typeof(Line))]
    [CoordOptions(typeof(GridCoord))]
    [DefaultAnimation(AnimationType.LeftToRight)]
    [SerieExtraComponent(typeof(AreaStyle))]
    [SerieDataExtraComponent()]
    [SerieDataExtraField()]
    public class SimplifiedLine : Serie, INeedSerieContainer, ISimplifiedSerie
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<SimplifiedLine>(serieName);
            serie.symbol.show = false;
            var lastValue = 0d;
            for (int i = 0; i < 50; i++)
            {
                if (i < 20)
                    lastValue += UnityEngine.Random.Range(0, 5);
                else
                    lastValue += UnityEngine.Random.Range(-3, 5);
                chart.AddData(serie.index, lastValue);
            }
            return serie;
        }

        public static SimplifiedLine CovertSerie(Serie serie)
        {
            var newSerie = serie.Clone<SimplifiedLine>();
            return newSerie;
        }
    }
}