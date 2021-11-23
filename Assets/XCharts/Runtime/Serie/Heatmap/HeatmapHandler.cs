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
    [UnityEngine.Scripting.Preserve]
    internal sealed class HeatmapHandler : SerieHandler<Heatmap>
    {
        public override void Update()
        {
            base.Update();
            UpdateSerieContext();
        }

        public override void DrawSerie(VertexHelper vh)
        {
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);
            DrawHeatmapSerie(vh, colorIndex, serie);
        }

        private void UpdateSerieContext()
        {
            if (!chart.isPointerInChart) return;
            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex)) return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex)) return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex)) return;
            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;
            if (!grid.IsPointerEnter()) return;
            foreach (var serieData in serie.data)
            {
                if (serieData.runtimeRect.Contains(chart.pointerPos))
                {
                    serie.context.pointerItemDataIndex = serieData.index;
                    serie.context.pointerEnter = true;
                    serieData.highlighted = true;
                    chart.RefreshTopPainter();
                }
                else
                {
                    serieData.highlighted = false;
                }
            }
        }

        private void DrawHeatmapSerie(VertexHelper vh, int colorIndex, Heatmap serie)
        {
            if (serie.animation.HasFadeOut()) return;
            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex)) return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex)) return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex)) return;
            xAxis.boundaryGap = true;
            yAxis.boundaryGap = true;
            var visualMap = chart.GetVisualMapOfSerie(serie);
            var xCount = xAxis.data.Count;
            var yCount = yAxis.data.Count;
            var xWidth = grid.context.width / xCount;
            var yWidth = grid.context.height / yCount;

            var zeroX = grid.context.x;
            var zeroY = grid.context.y;
            var dataList = serie.GetDataList();
            var rangeMin = visualMap.rangeMin;
            var rangeMax = visualMap.rangeMax;
            var color = chart.theme.GetColor(serie.index);
            var borderWidth = serie.itemStyle.show ? serie.itemStyle.borderWidth : 0;
            var rectWid = xWidth - 2 * borderWidth;
            var rectHig = yWidth - 2 * borderWidth;
            var borderColor = serie.itemStyle.opacity > 0 ? serie.itemStyle.borderColor : ChartConst.clearColor32;
            borderColor.a = (byte)(borderColor.a * serie.itemStyle.opacity);
            var borderToColor = serie.itemStyle.opacity > 0 ? serie.itemStyle.borderToColor : ChartConst.clearColor32;
            borderToColor.a = (byte)(borderToColor.a * serie.itemStyle.opacity);
            serie.dataPoints.Clear();
            serie.animation.InitProgress(1, 0, xCount);
            var animationIndex = serie.animation.GetCurrIndex();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;
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
                        yAxis.context.minValue, yAxis.context.maxValue);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    var pos = new Vector3(zeroX + (i + (xAxis.boundaryGap ? 0.5f : 0)) * xWidth,
                        zeroY + (j + (yAxis.boundaryGap ? 0.5f : 0)) * yWidth);
                    serie.dataPoints.Add(pos);
                    serieData.canShowLabel = false;
                    serieData.runtimeRect = new Rect(pos.x - rectWid / 2, pos.y - rectHig / 2, rectWid, rectHig);
                    if (value == 0) continue;
                    if ((value < rangeMin && rangeMin != visualMap.min)
                        || (value > rangeMax && rangeMax != visualMap.max))
                    {
                        continue;
                    }
                    if (!visualMap.IsInSelectedValue(value)) continue;
                    color = visualMap.GetColor(value);
                    if (animationIndex >= 0 && i > animationIndex) continue;
                    serieData.canShowLabel = true;
                    var emphasis = (serieData.highlighted)
                        || visualMap.context.pointerIndex > 0;

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
                chart.m_IsPlayingAnimation = true;
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }
    }
}