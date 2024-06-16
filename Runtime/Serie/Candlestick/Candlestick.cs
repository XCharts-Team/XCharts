using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(CandlestickHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight, false)]
    [DefaultTooltip(Tooltip.Type.Shadow, Tooltip.Trigger.Axis)]
    [SerieComponent()]
    [SerieDataComponent(typeof(ItemStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraField()]
    public class Candlestick : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Candlestick>(serieName);
            var lastValue = 50d;
            for (int i = 0; i < 5; i++)
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
    }
}