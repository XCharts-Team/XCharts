/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    public partial class CoordinateChart
    {
        protected void DrawHeatmapSerie(VertexHelper vh, int colorIndex, Serie serie)
        {
            if (serie.animation.HasFadeOut()) return;
            var yAxis = m_YAxes[serie.yAxisIndex];
            var xAxis = m_XAxes[serie.xAxisIndex];
            xAxis.boundaryGap = true;
            yAxis.boundaryGap = true;
            var grid = GetSerieGridOrDefault(serie);
            var xCount = xAxis.data.Count;
            var yCount = yAxis.data.Count;
            var xWidth = grid.runtimeWidth / xCount;
            var yWidth = grid.runtimeHeight / yCount;

            var zeroX = grid.runtimeX;
            var zeroY = grid.runtimeY;
            var dataList = serie.GetDataList();
            var rangeMin = visualMap.rangeMin;
            var rangeMax = visualMap.rangeMax;
            var color = m_Theme.GetColor(serie.index);
            var borderWidth = serie.itemStyle.show ? serie.itemStyle.borderWidth : 0;
            var borderColor = serie.itemStyle.opacity > 0 ? serie.itemStyle.borderColor : ChartConst.clearColor32;
            borderColor.a = (byte)(borderColor.a * serie.itemStyle.opacity);
            var borderToColor = serie.itemStyle.opacity > 0 ? serie.itemStyle.borderToColor : ChartConst.clearColor32;
            borderToColor.a = (byte)(borderToColor.a * serie.itemStyle.opacity);
            serie.dataPoints.Clear();
            serie.animation.InitProgress(1, 0, xCount);
            var animationIndex = serie.animation.GetCurrIndex();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            for (int i = 0; i < xCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    var dataIndex = i * yCount + j;
                    if (dataIndex >= dataList.Count) continue;
                    var serieData = dataList[dataIndex];
                    var dimension = VisualMapHelper.GetDimension(visualMap, serieData.data.Count);
                    if (serie.IsIgnoreIndex(dataIndex, dimension))
                    {
                        serie.dataPoints.Add(Vector3.zero);
                        continue;
                    }
                    var value = serieData.GetCurrData(dimension, dataChangeDuration, yAxis.inverse,
                        yAxis.runtimeMinValue, yAxis.runtimeMaxValue);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    var pos = new Vector3(zeroX + (i + (xAxis.boundaryGap ? 0.5f : 0)) * xWidth,
                        zeroY + (j + (yAxis.boundaryGap ? 0.5f : 0)) * yWidth);
                    serie.dataPoints.Add(pos);
                    serieData.canShowLabel = false;
                    if (value == 0) continue;
                    if (visualMap.enable)
                    {
                        if ((value < rangeMin && rangeMin != visualMap.min)
                            || (value > rangeMax && rangeMax != visualMap.max))
                        {
                            continue;
                        }
                        if (!visualMap.IsInSelectedValue(value)) continue;
                        color = visualMap.GetColor(value);
                    }
                    if (animationIndex >= 0 && i > animationIndex) continue;
                    serieData.canShowLabel = true;
                    var emphasis = (tooltip.show
                        && i == (int)tooltip.runtimeXValues[0]
                        && j == (int)tooltip.runtimeYValues[0])
                        || visualMap.runtimeSelectedIndex > 0;
                    var rectWid = xWidth - 2 * borderWidth;
                    var rectHig = yWidth - 2 * borderWidth;
                    UGL.DrawRectangle(vh, pos, rectWid / 2, rectHig / 2, color);
                    if (borderWidth > 0 && !ChartHelper.IsClearColor(borderColor))
                    {
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, borderWidth, borderColor, borderToColor);
                    }
                    if (visualMap.hoverLink && emphasis && serie.emphasis.show
                        && serie.emphasis.itemStyle.borderWidth > 0)
                    {
                        var emphasisBorderWidth = serie.emphasis.itemStyle.borderWidth;
                        var emphasisBorderColor = serie.emphasis.itemStyle.opacity > 0
                            ? serie.emphasis.itemStyle.borderColor : ChartConst.clearColor32;
                        var emphasisBorderToColor = serie.emphasis.itemStyle.opacity > 0
                            ? serie.emphasis.itemStyle.borderToColor : ChartConst.clearColor32;
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, emphasisBorderWidth, emphasisBorderColor,
                            emphasisBorderToColor);
                    }
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(xCount);
                m_IsPlayingAnimation = true;
                RefreshPainter(serie);
            }
            if (dataChanging)
            {
                RefreshPainter(serie);
            }
        }
    }
}