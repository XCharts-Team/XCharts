using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [AddComponentMenu("XCharts/HeatmapChart", 18)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class HeatmapChart : BaseChart
    {
        protected override void DefaultChart()
        {
            var tooltip = GetChartComponent<Tooltip>();
            tooltip.type = Tooltip.Type.None;
            tooltip.trigger = Tooltip.Trigger.Axis;

            var grid = GetOrAddChartComponent<GridCoord>();
            grid.left = 0.12f;

            var xAxis = GetOrAddChartComponent<XAxis>();
            xAxis.type = Axis.AxisType.Category;
            xAxis.boundaryGap = true;
            xAxis.splitNumber = 10;

            var yAxis = GetOrAddChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Category;
            yAxis.boundaryGap = true;
            yAxis.splitNumber = 10;
            RemoveData();

            var heatmapGridWid = 10f;
            int xSplitNumber = (int) (grid.context.width / heatmapGridWid);
            int ySplitNumber = (int) (grid.context.height / heatmapGridWid);

            Heatmap.AddDefaultSerie(this, GenerateDefaultSerieName());

            var visualMap = GetOrAddChartComponent<VisualMap>();
            visualMap.max = 10;
            visualMap.range[0] = 0f;
            visualMap.range[1] = 10f;
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
                    var value = 0f;
                    var rate = Random.Range(0, 101);
                    if (rate > 70) value = Random.Range(8f, 10f);
                    else value = Random.Range(1f, 8f);
                    var list = new List<double> { i, j, value };
                    AddData(0, list);
                }
            }
        }
    }
}