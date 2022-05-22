namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(BarHandler), true)]
    [SerieConvert(typeof(Line), typeof(Pie))]
    [RequireChartComponent(typeof(GridCoord))]
    [DefaultAnimation(AnimationType.BottomToTop)]
    [SerieExtraComponent(typeof(LabelStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraField("m_Ignore")]
    public class Bar : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Bar>(serieName);
            for (int i = 0; i < 5; i++)
            {
                chart.AddData(serie.index, UnityEngine.Random.Range(10, 90));
            }
            return serie;
        }

        public static Bar CovertSerie(Serie serie)
        {
            var newSerie = SerieHelper.CloneSerie<Bar>(serie);
            return newSerie;
        }
    }
}