
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(RingHandler), true)]
    [SerieExtraComponent(typeof(LabelStyle), typeof(TitleStyle), typeof(Emphasis))]
    public class Ring : Serie
    {
        public override bool useDataNameForColor { get { return true; } }
        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Ring>(serieName);
            serie.roundCap = true;
            serie.radius = new float[] { 0.3f, 0.35f };

            serie.AddExtraComponent<LabelStyle>();
            serie.label.show = true;
            serie.label.position = LabelStyle.Position.Center;
            serie.label.formatter = "{d:f0}%";
            serie.label.textStyle.fontSize = 28;

            serie.AddExtraComponent<TitleStyle>();
            serie.titleStyle.show = false;
            serie.titleStyle.textStyle.offset = new Vector2(0, 30);

            var value = Random.Range(30, 90);
            var max = 100;
            chart.AddData(serie.index, value, max, "data1");
            return serie;
        }
    }
}