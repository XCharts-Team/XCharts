using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(RadarHandler), true)]
    [RequireChartComponent(typeof(RadarCoord))]
    [SerieExtraComponent(typeof(LabelStyle), typeof(AreaStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(AreaStyle), typeof(EmphasisItemStyle), typeof(EmphasisLabelStyle))]
    [SerieDataExtraField()]
    public class Radar : Serie, INeedSerieContainer
    {
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
        public override bool useDataNameForColor { get { return true; } }
        public override bool multiDimensionLabel { get { return radarType == RadarType.Multiple; } }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
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
            return serie;
        }
    }
}