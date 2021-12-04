/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public partial class CoordinateChart
    {
        protected void DrawScatterSerie(VertexHelper vh, int colorIndex, Serie serie)
        {
            if (serie.animation.HasFadeOut()) return;
            if (!serie.show) return;
            DataZoom xDataZoom, yDataZoom;
            DataZoomHelper.GetSerieRelatedDataZoom(serie, dataZooms, out xDataZoom, out yDataZoom);
            var yAxis = m_YAxes[serie.yAxisIndex];
            var xAxis = m_XAxes[serie.xAxisIndex];
            var grid = GetSerieGridOrDefault(serie);
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > serie.dataCount ? serie.dataCount : serie.maxShow)
                : serie.dataCount;
            serie.animation.InitProgress(1, 0, 1);
            var rate = serie.animation.GetCurrRate();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            var dataList = serie.GetDataList(xDataZoom);
            foreach (var serieData in dataList)
            {
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount)) continue;
                var highlight = serie.highlighted || serieData.highlighted;
                var color = SerieHelper.GetItemColor(serie, serieData, m_Theme, colorIndex, highlight);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, m_Theme, colorIndex, highlight);
                var backgroundColor = SerieHelper.GetItemBackgroundColor(serie, serieData, m_Theme, colorIndex, highlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, m_Theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                double xValue = serieData.GetCurrData(0, dataChangeDuration, xAxis.inverse);
                double yValue = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse);
                if (serieData.IsDataChanged()) dataChanging = true;
                float pX = grid.runtimeX + xAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                float pY = grid.runtimeY + yAxis.axisLine.GetWidth(m_Theme.axis.lineWidth);
                float xDataHig = GetDataHig(xAxis, xValue, grid.runtimeWidth);
                float yDataHig = GetDataHig(yAxis, yValue, grid.runtimeHeight);
                var pos = new Vector3(pX + xDataHig, pY + yDataHig);
                if (!IsInGrid(grid, pos)) continue;
                serie.dataPoints.Add(pos);
                serieData.runtimePosition = pos;
                var datas = serieData.data;
                float symbolSize = 0;
                if (serie.highlighted || serieData.highlighted)
                {
                    symbolSize = symbol.GetSelectedSize(datas, m_Theme.serie.scatterSymbolSelectedSize);
                }
                else
                {
                    symbolSize = symbol.GetSize(datas, m_Theme.serie.scatterSymbolSize);
                }
                symbolSize *= rate;
                if (symbolSize > 100) symbolSize = 100;
                if (serie.type == SerieType.EffectScatter)
                {
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (byte)(255 * (symbolSize - nowSize) / symbolSize);
                        DrawSymbol(vh, symbol.type, nowSize, symbolBorder, pos, color, toColor, backgroundColor, symbol.gap, cornerRadius);
                    }
                    RefreshPainter(serie);
                }
                else
                {
                    DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos, color, toColor, backgroundColor, symbol.gap, cornerRadius);
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(1);
                m_IsPlayingAnimation = true;
                RefreshPainter(serie);
            }
            if (dataChanging)
            {
                RefreshPainter(serie);
            }
        }

        private float GetDataHig(Axis axis, double value, float totalWidth)
        {
            if (axis.IsLog())
            {
                int minIndex = axis.runtimeMinLogIndex;
                float nowIndex = axis.GetLogValue(value);
                return (nowIndex - minIndex) / axis.splitNumber * totalWidth;
            }
            else
            {
                return (float)((value - axis.runtimeMinValue) / axis.runtimeMinMaxRange * totalWidth);
            }
        }
    }
}