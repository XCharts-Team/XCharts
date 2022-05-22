using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/SimplifiedBarChart", 27)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class SimplifiedBarChart : BaseChart
    {
        protected override void DefaultChart()
        {
            AddChartComponentWhenNoExist<GridCoord>();
            AddChartComponentWhenNoExist<XAxis>();
            AddChartComponentWhenNoExist<YAxis>();

            var tooltip = GetChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.Line;
            tooltip.trigger = Tooltip.Trigger.Axis;

            RemoveData();
            SimplifiedBar.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < GetSerie(0).dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}