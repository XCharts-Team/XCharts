using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(SimplifiedCandlestickHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight, false)]
    [DefaultTooltip(Tooltip.Type.Shadow, Tooltip.Trigger.Axis)]
    [SerieComponent()]
    [SerieDataComponent()]
    [SerieDataExtraField()]
    public class SimplifiedCandlestick : Serie, INeedSerieContainer, ISimplifiedSerie
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<SimplifiedCandlestick>(serieName);
            var lastValue = 50d;
            for (int i = 0; i < 50; i++)
            {
                var open = lastValue;
                var close = open + Random.Range(-20, 20);
                var min = open < close ? open : close;
                var max = open > close ? open : close;
                var lowest = min + Random.Range(-10, -10);
                var heighest = max + Random.Range(10, 10);
                chart.AddData(serie.index, i, open, close, lowest, heighest);
                lastValue = close;
            }
            return serie;
        }

        public static SimplifiedCandlestick ConvertSerie(Serie serie)
        {
            var newSerie = serie.Clone<SimplifiedCandlestick>();
            return newSerie;
        }
    }
}