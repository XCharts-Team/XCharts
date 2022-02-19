
using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/BarChart", 14)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public partial class BarChart : BaseChart
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            AddChartComponentWhenNoExist<GridCoord>();
            AddChartComponentWhenNoExist<XAxis>();
            AddChartComponentWhenNoExist<YAxis>();

            var tooltip = GetChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.Shadow;
            tooltip.trigger = Tooltip.Trigger.Axis;

            RemoveData();
            Bar.AddDefaultSerie(this, GenerateDefaultSerieName());
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
#endif
    }
}
