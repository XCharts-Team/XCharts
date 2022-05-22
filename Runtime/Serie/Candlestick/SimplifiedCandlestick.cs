using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(SimplifiedCandlestickHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight)]
    [SerieExtraComponent()]
    [SerieDataExtraComponent()]
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
                lastValue += UnityEngine.Random.Range(-10, 20);
                var open = lastValue + Random.Range(-10, 5);
                var close = lastValue + Random.Range(-5, 10);
                var lowest = lastValue + Random.Range(-15, -10);
                var heighest = lastValue + Random.Range(10, 20);
                chart.AddData(serie.index, open, close, lowest, heighest);
            }
            return serie;
        }

        public static SimplifiedCandlestick CovertSerie(Serie serie)
        {
            var newSerie = serie.Clone<SimplifiedCandlestick>();
            return newSerie;
        }
    }
}