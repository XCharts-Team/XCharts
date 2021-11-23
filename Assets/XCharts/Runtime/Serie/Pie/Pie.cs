/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(PieHandler), true)]
    public class Pie : Serie
    {
        public static void AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Pie>(serieName);
            chart.AddData(serie.index, 70, "pie1");
            chart.AddData(serie.index, 20, "pie2");
            chart.AddData(serie.index, 10, "pie3");
        }

        public override bool useDataNameForColor { get { return true; } }
    }
}