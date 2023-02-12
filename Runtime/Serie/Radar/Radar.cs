using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [SerieHandler(typeof(RadarHandler), true)]
    [RequireChartComponent(typeof(RadarCoord))]
    [SerieComponent(typeof(LabelStyle), typeof(AreaStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(AreaStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraField()]
    public class Radar : Serie, INeedSerieContainer
    {
        [SerializeField][Since("v3.2.0")] private bool m_Smooth = false;

        /// <summary>
        /// Whether use smooth curve.
        /// |是否平滑曲线。平滑曲线时不支持区域填充颜色。
        /// </summary>
        public bool smooth
        {
            get { return m_Smooth; }
            set { if (PropertyUtil.SetStruct(ref m_Smooth, value)) { SetVerticesDirty(); } }
        }

        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }
        public override SerieColorBy defaultColorBy { get { return radarType == RadarType.Multiple?SerieColorBy.Data : SerieColorBy.Serie; } }
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