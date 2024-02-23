using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Radar chart is mainly used to show multi-variable data, such as the analysis of a football player's varied attributes. It relies radar component.
    /// || 雷达图主要用于显示多变量的数据，例如足球运动员的各项属性分析。依赖雷达组件。
    /// </summary>
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

        /// <summary>
        /// default circle radar chart.
        /// || 默认圆形雷达图。
        /// </summary>
        public void DefaultCircleRadarChart()
        {
            CheckChartInit();
            var radarCoord = GetChartComponent<RadarCoord>();
            radarCoord.shape = RadarCoord.Shape.Circle;
        }
    }
}