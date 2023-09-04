using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/CandlestickChart", 23)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class CandlestickChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<GridCoord>();
            EnsureChartComponent<XAxis>();
            EnsureChartComponent<YAxis>();

            RemoveData();
            var serie = Candlestick.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < serie.dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}