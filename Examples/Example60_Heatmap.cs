using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example60_Heatmap : MonoBehaviour
    {
        private HeatmapChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<HeatmapChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<HeatmapChart>();
                chart.Init();
            }
            chart.GetChartComponent<Title>().text = "HeatmapChart";
            chart.GetChartComponent<Tooltip>().type = Tooltip.Type.None;

            var grid = chart.GetChartComponent<GridCoord>();
            grid.left = 100;
            grid.right = 60;
            grid.bottom = 60;

            var xAxis = chart.GetChartComponent<XAxis>();
            var yAxis = chart.GetChartComponent<YAxis>();
            //目前只支持Category
            xAxis.type = Axis.AxisType.Category;
            yAxis.type = Axis.AxisType.Category;

            xAxis.boundaryGap = true;
            xAxis.boundaryGap = true;

            xAxis.splitNumber = 10;
            yAxis.splitNumber = 10;

            //清空数据重新添加
            chart.RemoveData();
            var serie = chart.AddSerie<Heatmap>("serie1");

            //设置样式
            serie.itemStyle.show = true;
            serie.itemStyle.borderWidth = 1;
            serie.itemStyle.borderColor = Color.clear;

            //设置高亮样式
            var emphasisStyle = serie.EnsureComponent<EmphasisStyle>();
            emphasisStyle.itemStyle.show = true;
            emphasisStyle.itemStyle.borderWidth = 1;
            emphasisStyle.itemStyle.borderColor = Color.black;

            //设置视觉映射组件
            var visualMap = chart.GetChartComponent<VisualMap>();
            visualMap.max = 10;
            visualMap.range[0] = 0f;
            visualMap.range[1] = 10f;
            visualMap.orient = Orient.Vertical;
            visualMap.calculable = true;
            visualMap.location.align = Location.Align.BottomLeft;
            visualMap.location.bottom = 100;
            visualMap.location.left = 30;

            //清空颜色重新添加

            var heatmapGridWid = 10f;
            int xSplitNumber = (int) (grid.context.width / heatmapGridWid);
            int ySplitNumber = (int) (grid.context.height / heatmapGridWid);
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
            //添加xAxis的数据
            for (int i = 0; i < xSplitNumber; i++)
            {
                chart.AddXAxisData((i + 1).ToString());
            }
            //添加yAxis的数据
            for (int i = 0; i < ySplitNumber; i++)
            {
                chart.AddYAxisData((i + 1).ToString());
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
                    //至少是一个三位数据：（x,y,value）
                    chart.AddData(0, list);
                }
            }
        }
    }
}