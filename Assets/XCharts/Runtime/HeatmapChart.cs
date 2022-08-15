/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/HeatmapChart", 18)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class HeatmapChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            title.text = "HeatmapChart";
            tooltip.type = Tooltip.Type.None;
            grid.left = 100;
            grid.right = 60;
            grid.bottom = 60;

            m_XAxes[0].type = Axis.AxisType.Category;
            m_XAxes[0].boundaryGap = true;
            m_YAxes[0].type = Axis.AxisType.Category;
            m_YAxes[0].boundaryGap = true;
            m_XAxes[0].splitNumber = 10;
            m_YAxes[0].splitNumber = 10;
            RemoveData();
            var heatmapGridWid = 10f;
            int xSplitNumber = (int) (grid.runtimeWidth / heatmapGridWid);
            int ySplitNumber = (int) (grid.runtimeHeight / heatmapGridWid);

            SerieTemplate.AddDefaultHeatmapSerie(this, "serie1");

            visualMap.enable = true;
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
            visualMap.inRange.Clear();
            foreach (var str in colors)
            {
                visualMap.inRange.Add(ChartTheme.GetColor(str));
            }
            for (int i = 0; i < xSplitNumber; i++)
            {
                m_XAxes[0].data.Add((i + 1).ToString());
            }
            for (int i = 0; i < ySplitNumber; i++)
            {
                m_YAxes[0].data.Add((i + 1).ToString());
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
#endif

        protected override void UpdateTooltip()
        {
            var dataIndex = GetDataIndex();
            if (dataIndex < 0) return;
            var content = TooltipHelper.GetFormatterContent(tooltip, dataIndex, this);
            TooltipHelper.SetContentAndPosition(tooltip, content, chartRect);
            tooltip.SetActive(true);
            for (int i = 0; i < m_XAxes.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_XAxes[i]);
            }
            for (int i = 0; i < m_YAxes.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_YAxes[i]);
            }
        }

        private int GetDataIndex()
        {
            var xData = tooltip.runtimeXValues[0];
            var yData = tooltip.runtimeYValues[0];
            if (IsCategory() && (xData < 0 || yData < 0)) return -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                var xAxis = m_XAxes[serie.xAxisIndex];
                var yAxis = m_YAxes[serie.yAxisIndex];
                var xCount = xAxis.data.Count;
                var yCount = yAxis.data.Count;
                if (serie.show && serie.type == SerieType.Heatmap)
                {
                    if (IsCategory())
                    {
                        return (int) xData * yCount + (int) yData;
                    }
                }
            }
            return -1;
        }
    }
}