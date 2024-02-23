using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieConvert(typeof(Line), typeof(Bar))]
    [SerieHandler(typeof(PieHandler), true)]
    [DefaultAnimation(AnimationType.Clockwise)]
    [SerieComponent(typeof(LabelStyle), typeof(LabelLine), typeof(TitleStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(LabelLine), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraField("m_Ignore", "m_Selected", "m_Radius")]
    public class Pie : Serie
    {
        [SerializeField][Since("v3.8.1")] private bool m_RadiusGradient = false;

        public override SerieColorBy defaultColorBy { get { return SerieColorBy.Data; } }
        public override bool titleJustForSerie { get { return true; } }

        /// <summary>
        /// Whether to use gradient color in pie chart.
        /// || 是否开启半径方向的渐变效果。
        /// </summary>
        public bool radiusGradient
        {
            get { return m_RadiusGradient; }
            set { if (PropertyUtil.SetStruct(ref m_RadiusGradient, value)) { SetVerticesDirty(); } }
        }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Pie>(serieName);
            chart.AddData(serie.index, Random.Range(10, 100), "pie1");
            chart.AddData(serie.index, Random.Range(10, 100), "pie2");
            chart.AddData(serie.index, Random.Range(10, 100), "pie3");
            return serie;
        }

        public static Pie ConvertSerie(Serie serie)
        {
            var newSerie = SerieHelper.CloneSerie<Pie>(serie);
            return newSerie;
        }
    }
}