using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// A simplified candlestick chart is a simplified mode of a bar chart that provides better performance by simplifying components and configurations.
    /// || 简化K线图是K线图的简化模式，通过简化组件和配置，拥有更好的性能。
    /// </summary>
    [AddComponentMenu("XCharts/SimplifiedCandlestickChart", 28)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class SimplifiedCandlestickChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();
            EnsureChartComponent<XAxis>();
            EnsureChartComponent<YAxis>();

            RemoveData();
            SimplifiedCandlestick.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < GetSerie(0).dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}