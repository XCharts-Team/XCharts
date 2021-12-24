
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(RingHandler), true)]
    public class Ring : Serie
    {
        public override bool useDataNameForColor { get { return true; } }
        public static void AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Ring>(serieName);
            serie.roundCap = true;
            serie.radius = new float[] { 0.3f, 0.35f };
            serie.titleStyle.show = false;
            serie.titleStyle.textStyle.offset = new Vector2(0, 30);
            serie.label.show = true;
            serie.label.position = LabelStyle.Position.Center;
            serie.label.formatter = "{d:f0}%";
            serie.label.textStyle.fontSize = 28;
            var value = Random.Range(30, 90);
            var max = 100;
            chart.AddData(serie.index, value, max, "data1");
        }
    }
}