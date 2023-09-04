using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/PolarChart", 23)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class PolarChart : BaseChart
    {
        protected override void DefaultChart()
        {
            EnsureChartComponent<PolarCoord>();
            EnsureChartComponent<AngleAxis>();
            EnsureChartComponent<RadiusAxis>();

            var tooltip = EnsureChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.Corss;
            tooltip.trigger = Tooltip.Trigger.Axis;

            RemoveData();
            var serie = Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            serie.SetCoord<PolarCoord>();
            serie.ClearData();
            for (int i = 0; i <= 360; i++)
            {
                var t = i / 180f * Mathf.PI;
                var r = Mathf.Sin(2 * t) * Mathf.Cos(2 * t) * 2;
                AddData(0, Mathf.Abs(r), i);
            }
        }
    }
}