using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(RingHandler), true)]
    [SerieExtraComponent(typeof(LabelStyle), typeof(LabelLine), typeof(TitleStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(LabelLine), typeof(TitleStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraField()]
    public class Ring : Serie
    {
        public override SerieColorBy defaultColorBy { get { return SerieColorBy.Data; } }
        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Ring>(serieName);
            serie.roundCap = true;
            serie.gap = 10;
            serie.radius = new float[] { 0.3f, 0.35f };

            var label = serie.AddExtraComponent<LabelStyle>();
            label.show = true;
            label.position = LabelStyle.Position.Center;
            label.formatter = "{d:f0}%";
            label.textStyle.autoColor = true;
            label.textStyle.fontSize = 28;

            var titleStyle = serie.AddExtraComponent<TitleStyle>();
            titleStyle.show = false;
            titleStyle.offset = new Vector2(0, 30);

            var value = Random.Range(30, 90);
            var max = 100;
            chart.AddData(serie.index, value, max, "data1");
            return serie;
        }

        public override double GetDataTotal(int dimension, SerieData serieData = null)
        {
            if (serieData == null || serieData.data.Count <= 1)
                return base.GetDataTotal(dimension, serieData);
            return serieData.GetData(1);
        }
    }
}