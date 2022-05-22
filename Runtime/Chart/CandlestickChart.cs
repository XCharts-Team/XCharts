using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/CandlestickChart", 23)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class CandlestickChart : BaseChart
    {
        protected override void DefaultChart()
        {
            AddChartComponentWhenNoExist<GridCoord>();
            AddChartComponentWhenNoExist<XAxis>();
            AddChartComponentWhenNoExist<YAxis>();

            var tooltip = GetOrAddChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.Shadow;
            tooltip.trigger = Tooltip.Trigger.Axis;

            RemoveData();
            var serie = Candlestick.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < serie.dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}