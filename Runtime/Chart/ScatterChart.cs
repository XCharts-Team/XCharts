using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/ScatterChart", 17)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class ScatterChart : BaseChart
    {
        protected override void DefaultChart()
        {
            AddChartComponentWhenNoExist<GridCoord>();

            var tooltip = EnsureChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.None;
            tooltip.trigger = Tooltip.Trigger.Item;

            var xAxis = EnsureChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Value;
            xAxis.boundaryGap = false;

            var yAxis = EnsureChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Value;
            yAxis.boundaryGap = false;

            RemoveData();
            Scatter.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}