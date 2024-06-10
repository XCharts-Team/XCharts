using UnityEngine;
using System.Collections.Generic;

namespace XCharts.Runtime
{
    /// <summary>
    /// Polar coordinates are usually used in a circular layout.
    /// || 极坐标系，可以用于散点图和折线图。
    /// </summary>
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
            var radiusAxis = EnsureChartComponent<RadiusAxis>();
            radiusAxis.axisLabel.show = false;

            var tooltip = EnsureChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.Cross;
            tooltip.trigger = Tooltip.Trigger.Axis;

            RemoveData();
            var serie = Line.AddDefaultSerie(this, GenerateDefaultSerieName());
            serie.SetCoord<PolarCoord>();
            serie.ClearData();
            serie.symbol.show = false;
            for (int i = 0; i <= 360; i++)
            {
                var t = i / 180f * Mathf.PI;
                var r = Mathf.Sin(2 * t) * Mathf.Cos(2 * t) * 2;
                AddData(0, Mathf.Abs(r), i);
            }
        }

        /// <summary>
        /// default radial bar polar chart.
        /// || 默认径向柱状极坐标图。
        /// </summary>
        public void DefaultRadialBarPolarChart()
        {
            CheckChartInit();
            RemoveData();

            var polarCoord = GetChartComponent<PolarCoord>();
            polarCoord.radius[0] = 20;

            var categorys = new string[] { "a", "b", "c", "d" };
            var radiusAxis = GetChartComponent<RadiusAxis>();
            radiusAxis.splitNumber = 4;

            var angleAxis = GetChartComponent<AngleAxis>();
            angleAxis.type = Axis.AxisType.Category;
            angleAxis.startAngle = 75;
            angleAxis.boundaryGap = true;
            angleAxis.splitLine.show = false;

            foreach (var category in categorys)
                angleAxis.AddData(category);

            var serie = AddSerie<Bar>(GenerateDefaultSerieName());
            serie.SetCoord<PolarCoord>();
            serie.ClearData();
            serie.symbol.show = false;
            for (int i = 0; i < categorys.Length; i++)
            {
                var x = UnityEngine.Random.Range(0f, 4f);
                var y = i;
                AddData(0, x, y, categorys[i]);
            }
        }

        /// <summary>
        /// default tangential bar polar chart.
        /// || 默认切向柱状极坐标图。
        /// </summary>
        public void DefaultTangentialBarPolarChart()
        {
            CheckChartInit();
            RemoveData();

            var polarCoord = GetChartComponent<PolarCoord>();
            polarCoord.radius[0] = 20;

            var categorys = new string[] { "a", "b", "c", "d" };
            var radiusAxis = GetChartComponent<RadiusAxis>();
            radiusAxis.type = Axis.AxisType.Category;
            radiusAxis.splitNumber = 4;
            radiusAxis.boundaryGap = true;

            var angleAxis = GetChartComponent<AngleAxis>();
            angleAxis.type = Axis.AxisType.Value;
            radiusAxis.splitNumber = 12;
            angleAxis.startAngle = 75;
            angleAxis.max = 4;

            foreach (var category in categorys)
                radiusAxis.AddData(category);

            var serie = AddSerie<Bar>(GenerateDefaultSerieName());
            serie.SetCoord<PolarCoord>();
            serie.ClearData();
            serie.symbol.show = false;
            for (int i = 0; i < categorys.Length; i++)
            {
                var x = UnityEngine.Random.Range(0f, 4f);
                var y = i;
                AddData(0, y, x, categorys[i]);
            }
        }

        /// <summary>
        /// default heatmap polar chart.
        /// || 默认极坐标色块图。 
        /// </summary>
        public void DefaultHeatmapPolarChart()
        {
            CheckChartInit();
            RemoveData();

            var visualMap = EnsureChartComponent<VisualMap>();
            var colors = new List<string> { "#BAE7FF", "#1890FF", "#1028ff" };
            visualMap.AddColors(colors);
            visualMap.autoMinMax = true;

            var polarCoord = GetChartComponent<PolarCoord>();
            polarCoord.radius[0] = 20;

            var categorys = new string[] { "a", "b", "c", "d" };
            var radiusAxis = GetChartComponent<RadiusAxis>();
            radiusAxis.type = Axis.AxisType.Category;
            radiusAxis.splitNumber = 4;
            radiusAxis.boundaryGap = true;

            var angleAxis = GetChartComponent<AngleAxis>();
            angleAxis.type = Axis.AxisType.Category;
            angleAxis.boundaryGap = true;
            angleAxis.splitNumber = 24;
            angleAxis.startAngle = 75;
            angleAxis.max = 4;

            foreach (var category in categorys)
                radiusAxis.AddData(category);

            for (int i = 0; i < 24; i++)
            {
                angleAxis.AddData(i + "h");
            }

            var serie = AddSerie<Heatmap>(GenerateDefaultSerieName());
            serie.SetCoord<PolarCoord>();
            serie.ClearData();
            serie.symbol.show = false;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 24; y++)
                {
                    AddData(0, x, y, UnityEngine.Random.Range(0f, 4f));
                }
            }
        }
    }
}