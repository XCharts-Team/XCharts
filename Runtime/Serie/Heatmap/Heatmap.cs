using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// The mapping type of heatmap.
    /// |热力图类型。通过颜色映射划分。
    /// </summary>
    public enum HeatmapType
    {
        /// <summary>
        /// Data mapping type.By default, the second dimension data is used as the color map.
        /// |数据映射型。默认用第2维数据作为颜色映射。要求数据至少有3个维度数据。
        /// </summary>
        Data,
        /// <summary>
        /// Number mapping type.The number of occurrences of a statistic in a divided grid, as a color map.
        /// |个数映射型。统计数据在划分的格子中出现的次数，作为颜色映射。要求数据至少有2个维度数据。
        /// </summary>
        Count
    }

    [System.Serializable]
    [SerieHandler(typeof(HeatmapHandler), true)]
    [DefaultAnimation(AnimationType.LeftToRight)]
    [RequireChartComponent(typeof(VisualMap))]
    [CoordOptions(typeof(GridCoord), typeof(PolarCoord))]
    [SerieExtraComponent(typeof(LabelStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraComponent(typeof(ItemStyle), typeof(LabelStyle), typeof(EmphasisStyle), typeof(BlurStyle), typeof(SelectStyle))]
    [SerieDataExtraField()]
    public class Heatmap : Serie, INeedSerieContainer
    {
        [SerializeField][Since("v3.3.0")] private HeatmapType m_HeatmapType = HeatmapType.Data;

        /// <summary>
        /// The mapping type of heatmap.
        /// |热力图类型。通过颜色映射划分。
        /// </summary>
        public HeatmapType heatmapType
        {
            get { return m_HeatmapType; }
            set { if (PropertyUtil.SetStruct(ref m_HeatmapType, value)) { SetVerticesDirty(); } }
        }
        public int containerIndex { get; internal set; }
        public int containterInstanceId { get; internal set; }

        public static Serie AddDefaultSerie(BaseChart chart, string serieName)
        {
            var serie = chart.AddSerie<Heatmap>(serieName);
            serie.itemStyle.show = true;
            serie.itemStyle.borderWidth = 1;
            serie.itemStyle.borderColor = Color.clear;
            return serie;
        }
    }
}