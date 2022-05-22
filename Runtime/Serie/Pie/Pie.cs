namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieConvert(typeof(Line), typeof(Bar))]
    [SerieHandler(typeof(PieHandler), true)]
    [DefaultAnimation(AnimationType.Clockwise)]
    [SerieExtraComponent(typeof(LabelStyle), typeof(LabelLine), typeof(TitleStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle), typeof(EmphasisLabelLine))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(LabelLine), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle), typeof(EmphasisLabelLine))]
    [SerieDataExtraField("m_Ignore", "m_Selected", "m_Radius")]
    public class Pie : Serie
    {
        public override bool useDataNameForColor { get { return true; } }
        public override bool titleJustForSerie { get { return true; } }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Pie>(serieName);
            chart.AddData(serie.index, 70, "pie1");
            chart.AddData(serie.index, 20, "pie2");
            chart.AddData(serie.index, 10, "pie3");
            return serie;
        }

        public static Pie CovertSerie(Serie serie)
        {
            var newSerie = SerieHelper.CloneSerie<Pie>(serie);
            return newSerie;
        }
    }
}