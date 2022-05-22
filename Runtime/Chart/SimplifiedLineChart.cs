using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/SimplifiedLineChart", 26)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class SimplifiedLineChart : BaseChart
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
            SimplifiedLine.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < GetSerie(0).dataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
    }
}