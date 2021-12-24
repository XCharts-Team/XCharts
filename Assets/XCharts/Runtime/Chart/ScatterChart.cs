
using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/ScatterChart", 17)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class ScatterChart : BaseChart
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            AddChartComponentWhenNoExist<GridCoord>();
            
            var tooltip = GetOrAddChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.None;
            tooltip.trigger = Tooltip.Trigger.Item;

            var xAxis = GetOrAddChartComponent<XAxis>();
            xAxis.type =  Axis.AxisType.Value;
            xAxis.boundaryGap = false;

            var yAxis = GetOrAddChartComponent<YAxis>();
            yAxis.type =  Axis.AxisType.Value;
            yAxis.boundaryGap = false;

            RemoveData();
            Scatter.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
#endif
    }
}
