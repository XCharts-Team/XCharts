using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Parallel Coordinates is a common way of visualizing high-dimensional geometry and analyzing multivariate data.
    /// || 平行坐标系，通过绘制垂直于坐标轴的平行线来显示数据的一种可视化图表。
    /// </summary>
    [AddComponentMenu("XCharts/ParallelChart", 25)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    [HelpURL("https://xcharts-team.github.io/docs/configuration")]
    public class ParallelChart : BaseChart
    {
        protected override void DefaultChart()
        {
            RemoveData();
            AddChartComponent<ParallelCoord>();

            for (int i = 0; i < 3; i++)
            {
                var valueAxis = AddChartComponent<ParallelAxis>();
                valueAxis.type = Axis.AxisType.Value;
            }
            var categoryAxis = AddChartComponent<ParallelAxis>();
            categoryAxis.type = Axis.AxisType.Category;
            categoryAxis.position = Axis.AxisPosition.Right;
            categoryAxis.data = new List<string>() { "x1", "x2", "x3", "x4", "x5" };

            Parallel.AddDefaultSerie(this, GenerateDefaultSerieName());
        }
    }
}