/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Examples
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
            }
            chart.title.text = "HeatmapChart";
            chart.tooltip.type = Tooltip.Type.None;
            chart.grid.left = 100;
            chart.grid.right = 60;
            chart.grid.bottom = 60;
            //目前只支持Category
            chart.xAxises[0].type = Axis.AxisType.Category;
            chart.yAxises[0].type = Axis.AxisType.Category;

            chart.xAxises[0].boundaryGap = true;
            chart.xAxises[0].boundaryGap = true;

            chart.xAxises[0].splitNumber = 10;
            chart.yAxises[0].splitNumber = 10;

            //清空数据重新添加
            chart.RemoveData();
            var serie = chart.AddSerie(SerieType.Heatmap, "serie1");

            //设置样式
            serie.itemStyle.show = true;
            serie.itemStyle.borderWidth = 1;
            serie.itemStyle.borderColor = Color.clear;

            //设置高亮样式
            serie.emphasis.show = true;
            serie.emphasis.itemStyle.show = true;
            serie.emphasis.itemStyle.borderWidth = 1;
            serie.emphasis.itemStyle.borderColor = Color.black;

            //设置视觉映射组件
            chart.visualMap.enable = true;
            chart.visualMap.max = 10;
            chart.visualMap.range[0] = 0f;
            chart.visualMap.range[1] = 10f;
            chart.visualMap.orient = Orient.Vertical;
            chart.visualMap.calculable = true;
            chart.visualMap.location.align = Location.Align.BottomLeft;
            chart.visualMap.location.bottom = 100;
            chart.visualMap.location.left = 30;

            //清空颜色重新添加
            chart.visualMap.inRange.Clear();

            var heatmapGridWid = 10f;
            int xSplitNumber = (int)(chart.coordinateWidth / heatmapGridWid);
            int ySplitNumber = (int)(chart.coordinateHeight / heatmapGridWid);
            var colors = new List<string>{"#313695", "#4575b4", "#74add1", "#abd9e9", "#e0f3f8", "#ffffbf",
                "#fee090", "#fdae61", "#f46d43", "#d73027", "#a50026"};
            foreach (var str in colors)
            {
                chart.visualMap.inRange.Add(ThemeInfo.GetColor(str));
            }
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
                    var list = new List<float> { i, j, value };
                    //至少是一个三位数据：（x,y,value）
                    chart.AddData(0, list);
                }
            }
        }
    }
}