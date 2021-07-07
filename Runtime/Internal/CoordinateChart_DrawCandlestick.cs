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
        protected void DrawCandlestickSerie(VertexHelper vh, int colorIndex, Serie serie)
        {
            if (!IsActive(serie.index)) return;
            if (serie.animation.HasFadeOut()) return;
            var yAxis = m_YAxes[serie.yAxisIndex];
            var xAxis = m_XAxes[serie.xAxisIndex];
            var grid = GetSerieGridOrDefault(serie);
            var dataZoom = DataZoomHelper.GetAxisRelatedDataZoom(xAxis, dataZooms);
            var showData = serie.GetDataList(dataZoom);
            float categoryWidth = AxisHelper.GetDataWidth(xAxis, grid.runtimeWidth, showData.Count, dataZoom);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float space = (categoryWidth - barWidth) / 2;
            int maxCount = serie.maxShow > 0
                ? (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;

            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double yMinValue = yAxis.GetCurrMinValue(dataChangeDuration);
            double yMaxValue = yAxis.GetCurrMaxValue(dataChangeDuration);
            var isAllBarEnd = true;
            var isYAxis = false;
            for (int i = serie.minShow; i < maxCount; i++)
            {
                var serieData = showData[i];
                if (serie.IsIgnoreValue(serieData))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    continue;
                }
                var highlight = (tooltip.show && tooltip.IsSelected(i))
                    || serie.data[i].highlighted
                    || serie.highlighted;
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, highlight);
                var open = serieData.GetCurrData(0, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var close = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var lowest = serieData.GetCurrData(2, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var heighest = serieData.GetCurrData(3, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var isRise = yAxis.inverse ? close < open : close > open;
                var borderWidth = open == 0 ? 0f
                    : (itemStyle.runtimeBorderWidth == 0 ? m_Theme.serie.candlestickBorderWidth
                    : itemStyle.runtimeBorderWidth);
                if (serieData.IsDataChanged()) dataChanging = true;
                float pX = grid.runtimeX + i * categoryWidth;
                float zeroY = grid.runtimeY + yAxis.runtimeZeroYOffset;
                if (!xAxis.boundaryGap) pX -= categoryWidth / 2;
                float pY = zeroY;
                var barHig = 0f;
                double valueTotal = yMaxValue - yMinValue;
                var minCut = (yMinValue > 0 ? yMinValue : 0);
                if (valueTotal != 0)
                {
                    barHig = (float)((close - open) / valueTotal * grid.runtimeHeight);
                    pY += (float)((open - minCut) / valueTotal * grid.runtimeHeight);
                }
                serieData.runtimeStackHig = barHig;
                var isBarEnd = false;
                float currHig = Internal_CheckBarAnimation(serie, i, barHig, out isBarEnd);
                if (!isBarEnd) isAllBarEnd = false;
                Vector3 plb, plt, prt, prb, top;

                plb = new Vector3(pX + space + borderWidth, pY + borderWidth);
                plt = new Vector3(pX + space + borderWidth, pY + currHig - borderWidth);
                prt = new Vector3(pX + space + barWidth - borderWidth, pY + currHig - borderWidth);
                prb = new Vector3(pX + space + barWidth - borderWidth, pY + borderWidth);
                top = new Vector3(pX + space + barWidth / 2, pY + currHig - borderWidth);
                if (serie.clip)
                {
                    plb = ClampInGrid(grid, plb);
                    plt = ClampInGrid(grid, plt);
                    prt = ClampInGrid(grid, prt);
                    prb = ClampInGrid(grid, prb);
                    top = ClampInGrid(grid, top);
                }
                serie.dataPoints.Add(top);
                var areaColor = isRise
                    ? itemStyle.GetColor(m_Theme.serie.candlestickColor)
                    : itemStyle.GetColor0(m_Theme.serie.candlestickColor0);
                var borderColor = isRise
                    ? itemStyle.GetBorderColor(m_Theme.serie.candlestickBorderColor)
                    : itemStyle.GetBorderColor0(m_Theme.serie.candlestickBorderColor0);
                var itemWidth = Mathf.Abs(prt.x - plb.x);
                var itemHeight = Mathf.Abs(plt.y - prb.y);
                var center = new Vector3((plb.x + prt.x) / 2, (plt.y + prb.y) / 2);
                var lowPos = new Vector3(center.x, zeroY + (float)((lowest - minCut) / valueTotal * grid.runtimeHeight));
                var heighPos = new Vector3(center.x, zeroY + (float)((heighest - minCut) / valueTotal * grid.runtimeHeight));
                var openCenterPos = new Vector3(center.x, prb.y);
                var closeCenterPos = new Vector3(center.x, prt.y);
                if (barWidth > 2f * borderWidth)
                {
                    if (itemWidth > 0 && itemHeight > 0)
                    {
                        if (ItemStyleHelper.IsNeedCorner(itemStyle))
                        {
                            UGL.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaColor, 0,
                                itemStyle.cornerRadius, isYAxis, 0.5f);
                        }
                        else
                        {
                            Internal_CheckClipAndDrawPolygon(vh, ref prb, ref plb, ref plt, ref prt, areaColor, areaColor,
                                serie.clip, grid);
                        }
                        UGL.DrawBorder(vh, center, itemWidth, itemHeight, 2 * borderWidth, borderColor, 0,
                          itemStyle.cornerRadius, isYAxis, 0.5f);
                    }
                }
                else
                {
                    UGL.DrawLine(vh, openCenterPos, closeCenterPos, Mathf.Max(borderWidth, barWidth / 2), borderColor);
                }
                if (isRise)
                {
                    UGL.DrawLine(vh, openCenterPos, lowPos, borderWidth, borderColor);
                    UGL.DrawLine(vh, closeCenterPos, heighPos, borderWidth, borderColor);
                }
                else
                {
                    UGL.DrawLine(vh, closeCenterPos, lowPos, borderWidth, borderColor);
                    UGL.DrawLine(vh, openCenterPos, heighPos, borderWidth, borderColor);
                }
            }
            if (isAllBarEnd)
            {
                serie.animation.AllBarEnd();
            }
            if (dataChanging)
            {
                RefreshPainter(serie);
            }
        }
    }
}