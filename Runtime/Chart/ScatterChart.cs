using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Scatter chart is mainly used to show the relationship between two data dimensions.
    /// || 散点图主要用于展现两个数据维度之间的关系。
    /// </summary>
    [AddComponentMenu("XCharts/ScatterChart", 17)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class ScatterChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();

            var xAxis = EnsureChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Value;
            xAxis.boundaryGap = false;

            var yAxis = EnsureChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Value;
            yAxis.boundaryGap = false;

            RemoveData();
            Scatter.AddDefaultSerie(this, GenerateDefaultSerieName());
        }

        /// <summary>
        /// default bubble chart.
        /// || 默认气泡图。
        /// </summary>
        public void DefaultBubbleChart()
        {
            CheckChartInit();
            var serie = GetSerie(0);
            serie.itemStyle.borderWidth = 2f;
            serie.itemStyle.borderColor = theme.GetColor(0);
            serie.itemStyle.opacity = 0.35f;
            serie.symbol.sizeType = SymbolSizeType.FromData;
            serie.symbol.dataScale = 0.3f;
            serie.symbol.maxSize = 30f;
        }
    }
}