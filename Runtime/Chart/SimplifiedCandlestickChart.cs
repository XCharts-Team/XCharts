using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/SimplifiedCandlestickChart", 28)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class SimplifiedCandlestickChart : BaseChart
    {
        protected override void DefaultChart()
        {
            AddChartComponentWhenNoExist<GridCoord>();
            AddChartComponentWhenNoExist<XAxis>();
            AddChartComponentWhenNoExist<YAxis>();

            var tooltip = GetChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.Shadow;
            tooltip.trigger = Tooltip.Trigger.Axis;

            RemoveData();
            SimplifiedCandlestick.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < GetSerie(0).dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}