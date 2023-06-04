using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/RadarChart", 16)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class RadarChart : BaseChart
    {
        protected override void DefaultChart()
        {
            RemoveData();
            RemoveChartComponents<RadarCoord>();
            AddChartComponent<RadarCoord>();
            Radar.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}