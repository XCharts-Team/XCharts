/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    [System.Serializable]
    [SerieHandler(typeof(RadarHandler), true)]
    [RequireChartComponent(typeof(RadarCoord))]
    public class Radar : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }

        public override bool useDataNameForColor { get { return true; } }
        public static void AddDefaultSerie(BaseChart chart, string serieName)
        {
            chart.AddChartComponentWhenNoExist<RadarCoord>();
            var serie = chart.AddSerie<Radar>(serieName);
            serie.symbol.show = true;
            serie.symbol.type = SymbolType.Circle;
            serie.showDataName = true;
            List<double> data = new List<double>();
            for (int i = 0; i < 5; i++)
            {
                data.Add(Random.Range(20, 90));
            }
            chart.AddData(serie.index, data, "legendName");
        }
    }
}