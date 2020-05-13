/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
            m_Title.text = "HeatmapChart";
            m_Tooltip.type = Tooltip.Type.None;
            m_Grid.left = 100;
            m_Grid.right = 60;
            m_Grid.bottom = 60;

            m_XAxises[0].type = Axis.AxisType.Category;
            m_XAxises[0].boundaryGap = false;
            m_YAxises[0].type = Axis.AxisType.Category;
            m_YAxises[0].boundaryGap = false;
            m_XAxises[0].splitNumber = 10;
            m_YAxises[0].splitNumber = 10;
            RemoveData();
            var serie = AddSerie(SerieType.Heatmap, "serie1");
            var heatmapGridWid = 10f;
            int xSplitNumber = (int)(m_CoordinateWidth / heatmapGridWid);
            int ySplitNumber = (int)(m_CoordinateHeight / heatmapGridWid);
            serie.itemStyle.show = true;
            serie.itemStyle.borderWidth = 1;
            serie.itemStyle.borderColor = Color.clear;
            serie.emphasis.show = true;
            serie.emphasis.itemStyle.show = true;
            serie.emphasis.itemStyle.borderWidth = 1;
            serie.emphasis.itemStyle.borderColor = Color.black;

            m_VisualMap.enable = true;
            m_VisualMap.max = 10;
            m_VisualMap.range[0] = 0f;
            m_VisualMap.range[1] = 10f;
            m_VisualMap.orient = Orient.Vertical;
            m_VisualMap.calculable = true;
            m_VisualMap.location.align = Location.Align.BottomLeft;
            m_VisualMap.location.bottom = 100;
            m_VisualMap.location.left = 30;
            var colors = new List<string>{"#313695", "#4575b4", "#74add1", "#abd9e9", "#e0f3f8", "#ffffbf",
                "#fee090", "#fdae61", "#f46d43", "#d73027", "#a50026"};
            m_VisualMap.inRange.Clear();
            foreach (var str in colors)
            {
                m_VisualMap.inRange.Add(ThemeInfo.GetColor(str));
            }
            for (int i = 0; i < xSplitNumber; i++)
            {
                m_XAxises[0].data.Add((i + 1).ToString());
            }
            for (int i = 0; i < ySplitNumber; i++)
            {
                m_YAxises[0].data.Add((i + 1).ToString());
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
                    AddData(0, list);
                }
            }
        }
#endif

        protected override void UpdateTooltip()
        {
            var xData = m_Tooltip.runtimeXValues[0];
            var yData = m_Tooltip.runtimeYValues[0];
            if (IsCategory() && (xData < 0 || yData < 0)) return;
            sb.Length = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                var xAxis = m_XAxises[serie.axisIndex];
                var yAxis = m_YAxises[serie.axisIndex];
                var xCount = xAxis.data.Count;
                var yCount = yAxis.data.Count;
                if (serie.show && serie.type == SerieType.Heatmap)
                {
                    if (IsCategory())
                    {
                        string key = serie.name;
                        var serieData = serie.data[(int)xData * yCount + (int)yData];
                        var value = serieData.data[2];
                        var color = m_VisualMap.enable ? m_VisualMap.GetColor(value) :
                            (Color)m_ThemeInfo.GetColor(serie.index);
                        sb.Append("\n")
                            .Append(key).Append(!string.IsNullOrEmpty(key) ? "\n" : "")
                            .Append("<color=#").Append(ChartCached.ColorToStr(color)).Append(">● </color>")
                            .Append(xAxis.data[(int)xData]).Append(": ")
                            .Append(ChartCached.FloatToStr(value, string.Empty));
                    }
                }
            }
            TooltipHelper.SetContentAndPosition(tooltip, sb.ToString().Trim(), chartRect);
            m_Tooltip.SetActive(true);

            for (int i = 0; i < m_XAxises.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_XAxises[i]);
            }
            for (int i = 0; i < m_YAxises.Count; i++)
            {
                UpdateAxisTooltipLabel(i, m_YAxises[i]);
            }
        }
    }
}
