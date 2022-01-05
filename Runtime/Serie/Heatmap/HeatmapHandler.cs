
using System.Collections.Generic;
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

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter,
            ref List<SerieParams> paramList, ref string title)
        {
            dataIndex = serie.context.pointerItemDataIndex;
            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            title = serie.serieName;

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.dimension = 2;
            param.serieData = serieData;
            param.color = chart.theme.GetColor(serie.index);
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            param.columns.Clear();

            param.columns.Add(param.marker);
            param.columns.Add(category);
            param.columns.Add(ChartCached.NumberToStr(serieData.GetData(2), param.numericFormatter));

            paramList.Add(param);
        }

        private void UpdateSerieContext()
        {
            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;

            if (!chart.isPointerInChart)
                return;

            var grid = chart.GetChartComponent<GridCoord>(serie.containerIndex);
            if (grid == null)
                return;

            if (!grid.IsPointerEnter())
                return;

            foreach (var serieData in serie.data)
            {
                if (serieData.context.rect.Contains(chart.pointerPos))
                {
                    serie.context.pointerItemDataIndex = serieData.index;
                    serie.context.pointerEnter = true;
                    serieData.context.highlight = true;
                    chart.RefreshTopPainter();
                }
                else
                {
                    serieData.context.highlight = false;
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
            var emphasis = serie.emphasis;
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
            serie.context.dataPoints.Clear();
            serie.animation.InitProgress(0, xCount);
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
                    serieData.index = dataIndex;
                    if (serie.IsIgnoreIndex(dataIndex, dimension))
                    {
                        serie.context.dataPoints.Add(Vector3.zero);
                        continue;
                    }
                    var value = serieData.GetCurrData(dimension, dataChangeDuration, yAxis.inverse,
                        yAxis.context.minValue, yAxis.context.maxValue);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    var pos = new Vector3(zeroX + (i + (xAxis.boundaryGap ? 0.5f : 0)) * xWidth,
                        zeroY + (j + (yAxis.boundaryGap ? 0.5f : 0)) * yWidth);
                    serie.context.dataPoints.Add(pos);
                    serieData.context.canShowLabel = false;
                    serieData.context.rect = new Rect(pos.x - rectWid / 2, pos.y - rectHig / 2, rectWid, rectHig);
                    if (value == 0) continue;
                    if ((value < rangeMin && rangeMin != visualMap.min)
                        || (value > rangeMax && rangeMax != visualMap.max))
                    {
                        continue;
                    }
                    if (!visualMap.IsInSelectedValue(value)) continue;
                    color = visualMap.GetColor(value);
                    if (animationIndex >= 0 && i > animationIndex) continue;
                    serieData.context.canShowLabel = true;
                    var highlight = (serieData.context.highlight)
                        || visualMap.context.pointerIndex > 0;

                    UGL.DrawRectangle(vh, pos, rectWid / 2, rectHig / 2, color);
                    if (borderWidth > 0 && !ChartHelper.IsClearColor(borderColor))
                    {
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, borderWidth, borderColor, borderToColor);
                    }
                    if (visualMap.hoverLink && highlight && emphasis != null && emphasis.show
                        && emphasis.itemStyle.borderWidth > 0)
                    {
                        var emphasisBorderWidth = emphasis.itemStyle.borderWidth;
                        var emphasisBorderColor = emphasis.itemStyle.opacity > 0
                            ? emphasis.itemStyle.borderColor : ChartConst.clearColor32;
                        var emphasisBorderToColor = emphasis.itemStyle.opacity > 0
                            ? emphasis.itemStyle.borderToColor : ChartConst.clearColor32;
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, emphasisBorderWidth, emphasisBorderColor,
                            emphasisBorderToColor);
                    }
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(xCount);
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }
    }
}