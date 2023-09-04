using System;
using UnityEngine;

namespace XCharts.Runtime
{
    [Serializable]
    [SerieHandler(typeof(SimplifiedBarHandler), true)]
    [SerieConvert(typeof(SimplifiedLine), typeof(Bar))]
    [CoordOptions(typeof(GridCoord))]
    [DefaultAnimation(AnimationType.LeftToRight, false)]
    [DefaultTooltip(Tooltip.Type.Shadow, Tooltip.Trigger.Axis)]
    [SerieComponent()]
    [SerieDataComponent()]
    [SerieDataExtraField()]
    public class SimplifiedBar : Serie, INeedSerieContainer, ISimplifiedSerie
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<SimplifiedBar>(serieName);
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

        public static SimplifiedBar ConvertSerie(Serie serie)
        {
            var newSerie = serie.Clone<SimplifiedBar>();
            return newSerie;
        }
    }
}