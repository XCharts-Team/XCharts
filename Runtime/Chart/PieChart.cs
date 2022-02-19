
using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/PieChart", 15)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class PieChart : BaseChart
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            var legend = GetOrAddChartComponent<Legend>();
            legend.show = true;

            RemoveData();
            Pie.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
#endif
    }
}
