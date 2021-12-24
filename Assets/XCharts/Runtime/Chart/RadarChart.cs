
using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/RadarChart", 16)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class RadarChart : BaseChart
    {
        protected override void InitComponent()
        {
            base.InitComponent();
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            RemoveData();
            RemoveChartComponents<RadarCoord>();
            AddChartComponent<RadarCoord>();
            Radar.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
#endif
    }
}
