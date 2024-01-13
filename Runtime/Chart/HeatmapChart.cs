using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Heat map mainly use colors to represent values, which must be used along with visualMap component.
    /// It can be used in either rectangular coordinate or geographic coordinate. But the behaviour on them are quite different. Rectangular coordinate must have two categories to use it.
    /// ||热力图主要通过颜色去表现数值的大小，必须要配合 visualMap 组件使用。
    /// 可以应用在直角坐标系以及地理坐标系上，这两个坐标系上的表现形式相差很大，直角坐标系上必须要使用两个类目轴。
    /// </summary>
    [AddComponentMenu("XCharts/HeatmapChart", 18)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class HeatmapChart : BaseChart
    {
        protected override void DefaultChart()
        {
            var grid = EnsureChartComponent<GridCoord>();
            grid.UpdateRuntimeData(this);
            grid.left = 0.12f;

            var heatmapGridWid = 18f;
            int xSplitNumber = (int)(grid.context.width / heatmapGridWid);
            int ySplitNumber = (int)(grid.context.height / heatmapGridWid);

            var xAxis = EnsureChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Category;
            xAxis.splitLine.show = false;
            xAxis.boundaryGap = true;
            xAxis.splitNumber = xSplitNumber / 2;

            var yAxis = EnsureChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Category;
            yAxis.splitLine.show = false;
            yAxis.boundaryGap = true;
            yAxis.splitNumber = ySplitNumber;
            RemoveData();

            Heatmap.AddDefaultSerie(this, GenerateDefaultSerieName());

            var visualMap = EnsureChartComponent<VisualMap>();
            visualMap.autoMinMax = true;
            visualMap.orient = Orient.Vertical;
            visualMap.calculable = true;
            visualMap.location.align = Location.Align.BottomLeft;
            visualMap.location.bottom = 100;
            visualMap.location.left = 30;
            var colors = new List<string>
            {
                "#313695",
                "#4575b4",
                "#74add1",
                "#abd9e9",
                "#e0f3f8",
                "#ffffbf",
                "#fee090",
                "#fdae61",
                "#f46d43",
                "#d73027",
                "#a50026"
            };
            visualMap.AddColors(colors);
            for (int i = 0; i < xSplitNumber; i++)
            {
                xAxis.data.Add((i + 1).ToString());
            }
            for (int i = 0; i < ySplitNumber; i++)
            {
                yAxis.data.Add((i + 1).ToString());
            }
            for (int i = 0; i < xSplitNumber; i++)
            {
                for (int j = 0; j < ySplitNumber; j++)
                {
                    var value = Random.Range(0, 150);
                    var list = new List<double> { i, j, value };
                    AddData(0, list);
                }
            }
        }

        /// <summary>
        /// default count heatmap chart.
        /// || 默认计数热力图。
        /// </summary>
        public void DefaultCountHeatmapChart()
        {
            CheckChartInit();

            var serie = GetSerie<Heatmap>(0);
            serie.heatmapType = HeatmapType.Count;
            var xAxis = GetChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Value;
            xAxis.splitNumber = 4;

            var yAxis = GetChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Value;
            yAxis.splitNumber = 2;

            serie.ClearData();
            for (int i = 0; i < 100; i++)
            {
                var x = UnityEngine.Random.Range(0, 100);
                var y = UnityEngine.Random.Range(0, 100);
                AddData(0, x, y);
            }
        }
    }
}