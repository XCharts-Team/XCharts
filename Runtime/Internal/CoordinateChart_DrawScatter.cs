/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > serie.dataCount ? serie.dataCount : serie.maxShow)
                : serie.dataCount;
            serie.animation.InitProgress(1, 0, 1);
            var rate = serie.animation.GetCurrRate();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            for (int n = serie.minShow; n < maxCount; n++)
            {
                var serieData = serie.GetDataList(m_DataZoom)[n];
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(n, maxCount)) continue;
                var highlight = serie.highlighted || serieData.highlighted;
                var color = SerieHelper.GetItemColor(serie, serieData, m_ThemeInfo, colorIndex, highlight);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, m_ThemeInfo, colorIndex, highlight);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                float xValue = serieData.GetCurrData(0, dataChangeDuration, xAxis.inverse);
                float yValue = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse);
                if (serieData.IsDataChanged()) dataChanging = true;
                float pX = m_CoordinateX + xAxis.axisLine.width;
                float pY = m_CoordinateY + yAxis.axisLine.width;
                float xDataHig = (xValue - xAxis.runtimeMinValue) / (xAxis.runtimeMaxValue - xAxis.runtimeMinValue) * m_CoordinateWidth;
                float yDataHig = (yValue - yAxis.runtimeMinValue) / (yAxis.runtimeMaxValue - yAxis.runtimeMinValue) * m_CoordinateHeight;
                var pos = new Vector3(pX + xDataHig, pY + yDataHig);
                serie.dataPoints.Add(pos);
                serieData.runtimePosition = pos;
                var datas = serie.data[n].data;
                float symbolSize = 0;
                if (serie.highlighted || serieData.highlighted)
                {
                    symbolSize = symbol.GetSelectedSize(datas);
                }
                else
                {
                    symbolSize = symbol.GetSize(datas);
                }
                symbolSize *= rate;
                if (symbolSize > 100) symbolSize = 100;
                if (serie.type == SerieType.EffectScatter)
                {
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (symbolSize - nowSize) / symbolSize;
                        DrawSymbol(vh, symbol.type, nowSize, symbolBorder, pos, color, toColor, symbol.gap, cornerRadius);
                    }
                    RefreshChart();
                }
                else
                {
                    DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos, color, toColor, symbol.gap, cornerRadius);
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(1);
                m_IsPlayingAnimation = true;
                RefreshChart();
            }
            if (dataChanging)
            {
                RefreshChart();
            }
        }
    }
}