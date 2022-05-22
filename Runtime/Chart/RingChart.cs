using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/RingChart", 20)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class RingChart : BaseChart
    {
        protected override void DefaultChart()
        {
            GetChartComponent<Tooltip>().type = Tooltip.Type.Line;
            RemoveData();
            Ring.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}