using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/BarChart", 14)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class BarChart : BaseChart
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
            Bar.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}