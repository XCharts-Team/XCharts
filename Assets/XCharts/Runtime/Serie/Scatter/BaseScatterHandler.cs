/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal class BaseScatterHandler<T> : SerieHandler<T> where T : BaseScatter
    {
        public override void Update()
        {
            UpdateSerieContext();
        }

        public override bool SetDefaultTooltipContent(Tooltip tooltip, StringBuilder sb)
        {
            var dataIndex = serie.context.pointerItemDataIndex;
            if (dataIndex < 0)
                return false;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return false;

            double xValue, yValue;
            serie.GetXYData(dataIndex, null, out xValue, out yValue);

            var key = serie.serieName;
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);
            var numericFormatter = TooltipHelper.GetItemNumericFormatter(tooltip, serie, serieData);

            if (!string.IsNullOrEmpty(key))
                sb.Append(key).Append("\n");

            sb.Append("<color=#")
                .Append(chart.theme.GetColorStr(colorIndex))
                .Append(">● </color>");

            sb.Append(ChartCached.FloatToStr(xValue, numericFormatter))
                .Append(", ")
                .Append(ChartCached.FloatToStr(yValue, numericFormatter));

            return true;
        }

        public override void DrawSerie(VertexHelper vh)
        {
            var colorIndex = chart.GetLegendRealShowNameIndex(serie.legendName);

            if (serie.IsUseCoord<SingleAxisCoord>())
            {
                DrawSingAxisScatterSerie(vh, colorIndex, serie);
            }
            else if (serie.IsUseCoord<GridCoord>())
            {
                DrawScatterSerie(vh, colorIndex, serie);
            }
        }

        private void UpdateSerieContext()
        {
            if (serie.IsUseCoord<GridCoord>())
            {
                var grid = chart.GetChartComponent<GridCoord>(serie.containerIndex);
                if (grid == null)
                    return;

                if (!grid.IsPointerEnter())
                    return;
            }

            var lastDataIndex = serie.context.pointerItemDataIndex;
            var maxCount = serie.maxShow > 0 ?
                (serie.maxShow > serie.dataCount ? serie.dataCount : serie.maxShow)
                : serie.dataCount;
            var themeSymbolSize = chart.theme.serie.scatterSymbolSize;

            if (lastDataIndex >= 0)
            {
                var serieData = serie.GetSerieData(lastDataIndex);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);
                if (Vector3.Distance(serieData.runtimePosition, chart.pointerPos) <= symbolSize)
                {
                    serieData.highlighted = true;
                    serie.context.pointerItemDataIndex = lastDataIndex;
                    return;
                }
            }
            serie.context.pointerEnter = false;
            serie.context.pointerItemDataIndex = -1;
            if (lastDataIndex >= 0)
            {
                serie.GetSerieData(lastDataIndex).highlighted = false;
                chart.RefreshPainter(serie);
            }
            for (int i = serie.dataCount - 1; i >= 0; i--)
            {
                var serieData = serie.data[i];

                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount))
                    continue;

                var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);
                if (Vector3.Distance(serieData.runtimePosition, chart.pointerPos) <= symbolSize)
                {
                    serieData.highlighted = true;
                    serie.context.pointerItemDataIndex = i;
                    serie.context.pointerEnter = true;
                    chart.RefreshPainter(serie);
                    break;
                }
            }
        }

        protected virtual void DrawScatterSerie(VertexHelper vh, int colorIndex, BaseScatter serie)
        {
            if (serie.animation.HasFadeOut())
                return;

            if (!serie.show)
                return;

            XAxis xAxis;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex))
                return;

            YAxis yAxis;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex))
                return;

            GridCoord grid;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex))
                return;

            DataZoom xDataZoom;
            DataZoom yDataZoom;
            chart.GetDataZoomOfSerie(serie, out xDataZoom, out yDataZoom);

            var theme = chart.theme;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > serie.dataCount ? serie.dataCount : serie.maxShow)
                : serie.dataCount;
            serie.animation.InitProgress(1, 0, 1);
            var rate = serie.animation.GetCurrRate();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            var dataList = serie.GetDataList(xDataZoom);
            var isEffectScatter = serie is EffectScatter;

            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;

            foreach (var serieData in dataList)
            {
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount))
                    continue;

                var highlight = serie.highlighted || serieData.highlighted;
                var color = SerieHelper.GetItemColor(serie, serieData, theme, colorIndex, highlight);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, theme, colorIndex, highlight);
                var emptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, theme, colorIndex, highlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                double xValue = serieData.GetCurrData(0, dataChangeDuration, xAxis.inverse);
                double yValue = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse);

                if (serieData.IsDataChanged())
                    dataChanging = true;

                float pX = grid.context.x + xAxis.axisLine.GetWidth(theme.axis.lineWidth);
                float pY = grid.context.y + yAxis.axisLine.GetWidth(theme.axis.lineWidth);
                float xDataHig = GetDataHig(xAxis, xValue, grid.context.width);
                float yDataHig = GetDataHig(yAxis, yValue, grid.context.height);
                var pos = new Vector3(pX + xDataHig, pY + yDataHig);

                if (!grid.Contains(pos))
                    continue;

                serie.dataPoints.Add(pos);
                serieData.runtimePosition = pos;
                var datas = serieData.data;
                float symbolSize = 0;

                if (serie.highlighted || serieData.highlighted)
                {
                    symbolSize = symbol.GetSelectedSize(datas, theme.serie.scatterSymbolSelectedSize);
                }
                else
                {
                    symbolSize = symbol.GetSize(datas, theme.serie.scatterSymbolSize);
                }
                symbolSize *= rate;

                if (isEffectScatter)
                {
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (byte)(255 * (symbolSize - nowSize) / symbolSize);
                        chart.DrawSymbol(vh, symbol.type, nowSize, symbolBorder, pos, color, toColor, emptyColor, symbol.gap, cornerRadius);
                    }
                    chart.RefreshPainter(serie);
                }
                else
                {
                    if (symbolSize > 100) symbolSize = 100;
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos, color, toColor, emptyColor, symbol.gap, cornerRadius);
                }

            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(1);
                chart.m_IsPlayingAnimation = true;
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }

        protected virtual void DrawSingAxisScatterSerie(VertexHelper vh, int colorIndex, BaseScatter serie)
        {
            if (serie.animation.HasFadeOut())
                return;

            if (!serie.show)
                return;

            var axis = chart.GetChartComponent<SingleAxis>(serie.singleAxisIndex);
            if (axis == null)
                return;

            DataZoom xDataZoom;
            DataZoom yDataZoom;
            chart.GetDataZoomOfSerie(serie, out xDataZoom, out yDataZoom);

            var theme = chart.theme;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > serie.dataCount ? serie.dataCount : serie.maxShow)
                : serie.dataCount;
            serie.animation.InitProgress(1, 0, 1);

            var rate = serie.animation.GetCurrRate();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            var dataList = serie.GetDataList(xDataZoom);
            var isEffectScatter = serie is EffectScatter;

            serie.containerIndex = axis.index;
            serie.containterInstanceId = axis.instanceId;

            foreach (var serieData in dataList)
            {
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount))
                    continue;

                var highlight = serie.highlighted || serieData.highlighted;
                var color = SerieHelper.GetItemColor(serie, serieData, theme, colorIndex, highlight);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, theme, colorIndex, highlight);
                var emptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, theme, colorIndex, highlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                var xValue = serieData.GetCurrData(0, dataChangeDuration, axis.inverse);

                if (serieData.IsDataChanged())
                    dataChanging = true;

                var pos = Vector3.zero;
                if (axis.orient == Orient.Horizonal)
                {
                    var xDataHig = GetDataHig(axis, xValue, axis.context.width);
                    var yDataHig = axis.context.height / 2;
                    pos = new Vector3(axis.context.x + xDataHig, axis.context.y + yDataHig);
                }
                else
                {
                    var yDataHig = GetDataHig(axis, xValue, axis.context.width);
                    var xDataHig = axis.context.height / 2;
                    pos = new Vector3(axis.context.x + xDataHig, axis.context.y + yDataHig);
                }
                serie.dataPoints.Add(pos);
                serieData.runtimePosition = pos;

                var datas = serieData.data;
                var symbolSize = 0f;
                if (serie.highlighted || serieData.highlighted)
                    symbolSize = symbol.GetSelectedSize(datas, theme.serie.scatterSymbolSelectedSize);
                else
                    symbolSize = symbol.GetSize(datas, theme.serie.scatterSymbolSize);
                symbolSize *= rate;

                if (isEffectScatter)
                {
                    if (symbolSize > 100) symbolSize = 100;
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (byte)(255 * (symbolSize - nowSize) / symbolSize);
                        chart.DrawSymbol(vh, symbol.type, nowSize, symbolBorder, pos,
                            color, toColor, emptyColor, symbol.gap, cornerRadius);
                    }
                    chart.RefreshPainter(serie);
                }
                else
                {
                    if (symbolSize > 100) symbolSize = 100;
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos,
                        color, toColor, emptyColor, symbol.gap, cornerRadius);
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(1);
                chart.m_IsPlayingAnimation = true;
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }

        private static float GetDataHig(Axis axis, double value, float totalWidth)
        {
            if (axis.IsLog())
            {
                int minIndex = axis.GetLogMinIndex();
                float nowIndex = axis.GetLogValue(value);
                return (nowIndex - minIndex) / axis.splitNumber * totalWidth;
            }
            else if (axis.IsCategory())
            {
                if (axis.boundaryGap)
                {
                    float tick = (float)(totalWidth / (axis.context.minMaxRange + 1));
                    return tick / 2 + (float)(value - axis.context.minValue) * tick;
                }
                else
                {
                    return (float)((value - axis.context.minValue) / axis.context.minMaxRange * totalWidth);
                }
            }
            else
            {
                return (float)((value - axis.context.minValue) / axis.context.minMaxRange * totalWidth);
            }
        }
    }
}