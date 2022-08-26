using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(CandlestickHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight)]
    [SerieExtraComponent()]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraField()]
    public class Candlestick : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Candlestick>(serieName);
            var defaultDataCount = 5;
            for (int i = 0; i < defaultDataCount; i++)
            {
                var open = Random.Range(20, 60);
                var close = Random.Range(40, 90);
                var lowest = Random.Range(0, 50);
                var heighest = Random.Range(50, 100);
                chart.AddData(serie.index, i, open, close, lowest, heighest);
            }
            return serie;
        }
    }
}