/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public partial class CoordinateChart
    {
        protected float m_BarLastOffset = 0;

        protected void DrawYBarSerie(VertexHelper vh, Serie serie, int colorIndex, ref List<float> seriesHig)
        {
            if (!IsActive(serie.name)) return;
            if (serie.animation.HasFadeOut()) return;
            var xAxis = m_XAxises[serie.axisIndex];
            var yAxis = m_YAxises[serie.axisIndex];
            if (!yAxis.show) yAxis = m_YAxises[(serie.axisIndex + 1) % m_YAxises.Count];

            var showData = serie.GetDataList(m_DataZoom);
            float categoryWidth = AxisHelper.GetDataWidth(yAxis, m_CoordinateHeight, showData.Count, m_DataZoom);
            float barGap = GetBarGap();
            float totalBarWidth = GetBarTotalWidth(categoryWidth, barGap);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float offset = (categoryWidth - totalBarWidth) / 2;
            float barGapWidth = barWidth + barWidth * barGap;
            float space = serie.barGap == -1 ? offset : offset + m_BarLastOffset;


            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;
            if (seriesHig.Count < serie.minShow)
            {
                for (int i = 0; i < serie.minShow; i++)
                {
                    seriesHig.Add(0);
                }
            }
            var isPercentStack = SeriesHelper.IsPercentStack(m_Series, serie.stack, SerieType.Bar);
            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            float xMinValue = xAxis.GetCurrMinValue(dataChangeDuration);
            float xMaxValue = xAxis.GetCurrMaxValue(dataChangeDuration);

            for (int i = serie.minShow; i < maxCount; i++)
            {
                if (i >= seriesHig.Count)
                {
                    seriesHig.Add(0);
                }
                var serieData = showData[i];
                if (serie.IsIgnoreValue(serieData.GetData(1)))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    continue;
                }
                var highlight = (m_Tooltip.show && m_Tooltip.IsSelected(i))
                    || serie.data[i].highlighted
                    || serie.highlighted;
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, highlight);

                serieData.canShowLabel = true;
                float value = showData[i].GetCurrData(1, dataChangeDuration, xAxis.inverse);
                float borderWidth = value == 0 ? 0 : itemStyle.runtimeBorderWidth;
                if (showData[i].IsDataChanged()) dataChanging = true;
                float axisLineWidth = (value < 0 ? -1 : 1) * yAxis.axisLine.width;
                float pX = seriesHig[i] + m_CoordinateX + xAxis.runtimeZeroXOffset + axisLineWidth;
                float pY = m_CoordinateY + i * categoryWidth;
                if (!yAxis.boundaryGap) pY -= categoryWidth / 2;

                var barHig = 0f;
                var valueTotal = 0f;
                if (isPercentStack)
                {
                    valueTotal = GetSameStackTotalValue(serie.stack, i);
                    barHig = valueTotal != 0 ? (value / valueTotal * m_CoordinateWidth) : 0;
                    seriesHig[i] += barHig;
                }
                else
                {
                    valueTotal = xMaxValue - xMinValue;
                    if (valueTotal != 0)
                        barHig = (xMinValue > 0 ? value - xMinValue : value)
                            / valueTotal * m_CoordinateWidth;
                    seriesHig[i] += barHig;
                }


                float currHig = CheckAnimation(serie, i, barHig);
                Vector3 plt, prt, prb, plb, top;
                if (value < 0)
                {
                    plt = new Vector3(pX - borderWidth, pY + space + barWidth - borderWidth);
                    prt = new Vector3(pX + currHig + borderWidth - axisLineWidth, pY + space + barWidth - borderWidth);
                    prb = new Vector3(pX + currHig + borderWidth - axisLineWidth, pY + space + borderWidth);
                    plb = new Vector3(pX - borderWidth, pY + space + borderWidth);
                }
                else
                {
                    plt = new Vector3(pX + borderWidth, pY + space + barWidth - borderWidth);
                    prt = new Vector3(pX + currHig - borderWidth - axisLineWidth, pY + space + barWidth - borderWidth);
                    prb = new Vector3(pX + currHig - borderWidth - axisLineWidth, pY + space + borderWidth);
                    plb = new Vector3(pX + borderWidth, pY + space + borderWidth);
                }
                top = new Vector3(pX + currHig - borderWidth, pY + space + barWidth / 2);
                plt = ClampInCoordinate(plt);
                prt = ClampInCoordinate(prt);
                prb = ClampInCoordinate(prb);
                plb = ClampInCoordinate(plb);
                top = ClampInCoordinate(top);
                serie.dataPoints.Add(top);
                if (serie.show)
                {
                    switch (serie.barType)
                    {
                        case BarType.Normal:
                            DrawNormalBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, true);
                            break;
                        case BarType.Zebra:
                            DrawZebraBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, true);
                            break;
                        case BarType.Capsule:
                            DrawCapsuleBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, true);
                            break;
                    }
                }
            }
            if (!SeriesHelper.IsStack(m_Series, serie.stack, SerieType.Bar))
            {
                m_BarLastOffset += barGapWidth;
            }
            if (dataChanging)
            {
                RefreshChart();
            }
        }

        private float CheckAnimation(Serie serie, int dataIndex, float barHig)
        {
            float currHig = serie.animation.CheckBarProgress(dataIndex, barHig, serie.dataCount);
            if (!serie.animation.IsFinish())
            {
                RefreshChart();
                m_IsPlayingAnimation = true;
            }
            return currHig;
        }

        protected void DrawXBarSerie(VertexHelper vh, Serie serie, int colorIndex, ref List<float> seriesHig)
        {
            if (!IsActive(serie.index)) return;
            if (serie.animation.HasFadeOut()) return;
            var showData = serie.GetDataList(m_DataZoom);
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            if (!xAxis.show) xAxis = m_XAxises[(serie.axisIndex + 1) % m_XAxises.Count];

            float categoryWidth = AxisHelper.GetDataWidth(xAxis, m_CoordinateWidth, showData.Count, m_DataZoom);
            float barGap = GetBarGap();
            float totalBarWidth = GetBarTotalWidth(categoryWidth, barGap);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float offset = (categoryWidth - totalBarWidth) / 2;
            float barGapWidth = barWidth + barWidth * barGap;
            float space = serie.barGap == -1 ? offset : offset + m_BarLastOffset;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow)
                : showData.Count;

            if (seriesHig.Count < serie.minShow)
            {
                for (int i = 0; i < serie.minShow; i++)
                {
                    seriesHig.Add(0);
                }
            }

            var isPercentStack = SeriesHelper.IsPercentStack(m_Series, serie.stack, SerieType.Bar);
            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            float yMinValue = yAxis.GetCurrMinValue(dataChangeDuration);
            float yMaxValue = yAxis.GetCurrMaxValue(dataChangeDuration);
            for (int i = serie.minShow; i < maxCount; i++)
            {
                if (i >= seriesHig.Count)
                {
                    seriesHig.Add(0);
                }
                var serieData = showData[i];
                if (serie.IsIgnoreValue(serieData.GetData(1)))
                {
                    serie.dataPoints.Add(Vector3.zero);
                    continue;
                }
                var highlight = (m_Tooltip.show && m_Tooltip.IsSelected(i))
                    || serie.data[i].highlighted
                    || serie.highlighted;
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, highlight);
                float value = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse);
                float borderWidth = value == 0 ? 0 : itemStyle.runtimeBorderWidth;
                if (serieData.IsDataChanged()) dataChanging = true;
                float pX = m_CoordinateX + i * categoryWidth;
                float zeroY = m_CoordinateY + yAxis.runtimeZeroYOffset;
                if (!xAxis.boundaryGap) pX -= categoryWidth / 2;
                float axisLineWidth = (value < 0 ? -1 : 1) * xAxis.axisLine.width;
                float pY = seriesHig[i] + zeroY + axisLineWidth;

                var barHig = 0f;
                var valueTotal = 0f;
                if (isPercentStack)
                {
                    valueTotal = GetSameStackTotalValue(serie.stack, i);
                    barHig = valueTotal != 0 ? (value / valueTotal * m_CoordinateHeight) : 0;
                    seriesHig[i] += barHig;
                }
                else
                {
                    valueTotal = yMaxValue - yMinValue;
                    if (valueTotal != 0)
                        barHig = (yMinValue > 0 ? value - yMinValue : value)
                            / valueTotal * m_CoordinateHeight;
                    seriesHig[i] += barHig;
                }
                float currHig = CheckAnimation(serie, i, barHig);
                Vector3 plb, plt, prt, prb, top;
                if (value < 0)
                {
                    plb = new Vector3(pX + space + borderWidth, pY - borderWidth);
                    plt = new Vector3(pX + space + borderWidth, pY + currHig + borderWidth - axisLineWidth);
                    prt = new Vector3(pX + space + barWidth - borderWidth, pY + currHig + borderWidth - axisLineWidth);
                    prb = new Vector3(pX + space + barWidth - borderWidth, pY - borderWidth);
                }
                else
                {
                    plb = new Vector3(pX + space + borderWidth, pY + borderWidth);
                    plt = new Vector3(pX + space + borderWidth, pY + currHig - borderWidth - axisLineWidth);
                    prt = new Vector3(pX + space + barWidth - borderWidth, pY + currHig - borderWidth - axisLineWidth);
                    prb = new Vector3(pX + space + barWidth - borderWidth, pY + borderWidth);
                }
                top = new Vector3(pX + space + barWidth / 2, pY + currHig - borderWidth);
                if (serie.clip)
                {
                    plb = ClampInCoordinate(plb);
                    plt = ClampInCoordinate(plt);
                    prt = ClampInCoordinate(prt);
                    prb = ClampInCoordinate(prb);
                    top = ClampInCoordinate(top);
                }
                serie.dataPoints.Add(top);
                if (serie.show && currHig != 0)
                {
                    switch (serie.barType)
                    {
                        case BarType.Normal:
                            DrawNormalBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, false);
                            break;
                        case BarType.Zebra:
                            DrawZebraBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                                pX, pY, plb, plt, prt, prb, false);
                            break;
                        case BarType.Capsule:
                            DrawCapsuleBar(vh, serie, serieData, itemStyle, colorIndex, highlight, space, barWidth,
                               pX, pY, plb, plt, prt, prb, false);
                            break;
                    }
                }
            }
            if (dataChanging)
            {
                RefreshChart();
            }
            if (!SeriesHelper.IsStack(m_Series, serie.stack, SerieType.Bar))
            {
                m_BarLastOffset += barGapWidth;
            }
        }

        private void DrawNormalBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
            bool highlight, float space, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis)
        {
            Color areaColor = SerieHelper.GetItemColor(serie, serieData, m_ThemeInfo, colorIndex, highlight);
            Color areaToColor = SerieHelper.GetItemToColor(serie, serieData, m_ThemeInfo, colorIndex, highlight);
            DrawBarBackground(vh, serie, serieData, itemStyle, colorIndex, highlight, pX, pY, space, barWidth, isYAxis);
            var borderWidth = itemStyle.runtimeBorderWidth;
            if (isYAxis)
            {
                if (serie.clip)
                {
                    prb = ClampInCoordinate(prb);
                    plb = ClampInCoordinate(plb);
                    plt = ClampInCoordinate(plt);
                    prt = ClampInCoordinate(prt);
                }
                var borderColor = itemStyle.borderColor;
                var itemWidth = Mathf.Abs(prb.x - plt.x);
                var itemHeight = Mathf.Abs(prt.y - plb.y);
                var center = new Vector3((plt.x + prb.x) / 2, (prt.y + plb.y) / 2);
                if (itemWidth > 0 && itemHeight > 0)
                {
                    if (ItemStyleHelper.IsNeedCorner(itemStyle))
                    {
                        ChartDrawer.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaToColor, 0, itemStyle.cornerRadius, isYAxis);
                    }
                    else
                    {
                        CheckClipAndDrawPolygon(vh, plb, plt, prt, prb, areaColor, areaToColor, serie.clip);
                    }
                    ChartDrawer.DrawBorder(vh, center, itemWidth, itemHeight, borderWidth, borderColor, 0, itemStyle.cornerRadius, isYAxis);
                }
            }
            else
            {
                if (serie.clip)
                {
                    prb = ClampInCoordinate(prb);
                    plb = ClampInCoordinate(plb);
                    plt = ClampInCoordinate(plt);
                    prt = ClampInCoordinate(prt);
                }
                var borderColor = itemStyle.borderColor;
                var itemWidth = Mathf.Abs(prt.x - plb.x);
                var itemHeight = Mathf.Abs(plt.y - prb.y);
                var center = new Vector3((plb.x + prt.x) / 2, (plt.y + prb.y) / 2);
                if (itemWidth > 0 && itemHeight > 0)
                {
                    if (ItemStyleHelper.IsNeedCorner(itemStyle))
                    {
                        ChartDrawer.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaToColor, 0, itemStyle.cornerRadius, isYAxis);
                    }
                    else
                    {
                        CheckClipAndDrawPolygon(vh, ref prb, ref plb, ref plt, ref prt, areaColor, areaToColor, serie.clip);
                    }
                    ChartDrawer.DrawBorder(vh, center, itemWidth, itemHeight, borderWidth, borderColor, 0, itemStyle.cornerRadius, isYAxis);
                }
            }
        }

        private void DrawZebraBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
            bool highlight, float space, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis)
        {
            Color areaColor = SerieHelper.GetItemColor(serie, serieData, m_ThemeInfo, colorIndex, highlight);
            DrawBarBackground(vh, serie, serieData, itemStyle, colorIndex, highlight, pX, pY, space, barWidth, isYAxis);
            if (isYAxis)
            {
                plt = (plb + plt) / 2;
                prt = (prt + prb) / 2;
                CheckClipAndDrawZebraLine(vh, plt, prt, barWidth / 2, serie.barZebraWidth, serie.barZebraGap,
                    areaColor, serie.clip);
            }
            else
            {
                plb = (prb + plb) / 2;
                plt = (plt + prt) / 2;
                CheckClipAndDrawZebraLine(vh, plb, plt, barWidth / 2, serie.barZebraWidth, serie.barZebraGap,
                    areaColor, serie.clip);
            }
        }

        private void DrawCapsuleBar(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
            bool highlight, float space, float barWidth, float pX, float pY, Vector3 plb, Vector3 plt, Vector3 prt,
            Vector3 prb, bool isYAxis)
        {
            Color areaColor = SerieHelper.GetItemColor(serie, serieData, m_ThemeInfo, colorIndex, highlight);
            Color areaToColor = SerieHelper.GetItemToColor(serie, serieData, m_ThemeInfo, colorIndex, highlight);
            DrawBarBackground(vh, serie, serieData, itemStyle, colorIndex, highlight, pX, pY, space, barWidth, isYAxis);
            var borderWidth = itemStyle.runtimeBorderWidth;
            var radius = barWidth / 2 - borderWidth;
            var isGradient = !ChartHelper.IsValueEqualsColor(areaColor, areaToColor);
            if (isYAxis)
            {
                var diff = Vector3.right * radius;
                if (plt.x < prt.x)
                {
                    var pcl = (plt + plb) / 2 + diff;
                    var pcr = (prt + prb) / 2 - diff;
                    if (pcr.x > pcl.x)
                    {
                        if (isGradient)
                        {
                            var barLen = prt.x - plt.x;
                            var rectStartColor = Color.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            CheckClipAndDrawPolygon(vh, plb + diff, plt + diff, prt - diff, prb - diff, rectStartColor, rectEndColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pcl, radius, areaColor, rectStartColor, 180, 360, 1, isYAxis);
                            ChartDrawer.DrawSector(vh, pcr, radius, rectEndColor, areaToColor, 0, 180, 1, isYAxis);
                        }
                        else
                        {
                            CheckClipAndDrawPolygon(vh, plb + diff, plt + diff, prt - diff, prb - diff, areaColor, areaToColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pcl, radius, areaColor, 180, 360);
                            ChartDrawer.DrawSector(vh, pcr, radius, areaToColor, 0, 180);
                        }
                    }
                }
                else if (plt.x > prt.x)
                {
                    var pcl = (plt + plb) / 2 - diff;
                    var pcr = (prt + prb) / 2 + diff;
                    if (pcr.x < pcl.x)
                    {
                        if (isGradient)
                        {
                            var barLen = plt.x - prt.x;
                            var rectStartColor = Color.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            CheckClipAndDrawPolygon(vh, plb - diff, plt - diff, prt + diff, prb + diff, rectStartColor, rectEndColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pcl, radius, rectStartColor, areaColor, 0, 180, 1, isYAxis);
                            ChartDrawer.DrawSector(vh, pcr, radius, areaToColor, rectEndColor, 180, 360, 1, isYAxis);
                        }
                        else
                        {
                            CheckClipAndDrawPolygon(vh, plb - diff, plt - diff, prt + diff, prb + diff, areaColor, areaToColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pcl, radius, areaColor, 0, 180);
                            ChartDrawer.DrawSector(vh, pcr, radius, areaToColor, 180, 360);
                        }
                    }
                }
            }
            else
            {
                var diff = Vector3.up * radius;
                if (plt.y > plb.y)
                {
                    var pct = (plt + prt) / 2 - diff;
                    var pcb = (plb + prb) / 2 + diff;
                    if (pct.y > pcb.y)
                    {
                        if (isGradient)
                        {
                            var barLen = plt.y - plb.y;
                            var rectStartColor = Color.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            CheckClipAndDrawPolygon(vh, prb + diff, plb + diff, plt - diff, prt - diff, rectStartColor, rectEndColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pct, radius, rectEndColor, areaToColor, 270, 450, 1, isYAxis);
                            ChartDrawer.DrawSector(vh, pcb, radius, rectStartColor, areaColor, 90, 270, 1, isYAxis);
                        }
                        else
                        {
                            CheckClipAndDrawPolygon(vh, prb + diff, plb + diff, plt - diff, prt - diff, areaColor, areaToColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pct, radius, areaToColor, 270, 450);
                            ChartDrawer.DrawSector(vh, pcb, radius, areaColor, 90, 270);
                        }
                    }
                }
                else if (plt.y < plb.y)
                {
                    var pct = (plt + prt) / 2 + diff;
                    var pcb = (plb + prb) / 2 - diff;
                    if (pct.y < pcb.y)
                    {
                        if (isGradient)
                        {
                            var barLen = plb.y - plt.y;
                            var rectStartColor = Color.Lerp(areaColor, areaToColor, radius / barLen);
                            var rectEndColor = Color.Lerp(areaColor, areaToColor, (barLen - radius) / barLen);
                            CheckClipAndDrawPolygon(vh, prb - diff, plb - diff, plt + diff, prt + diff, rectStartColor, rectEndColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pct, radius, rectEndColor, areaToColor, 90, 270, 1, isYAxis);
                            ChartDrawer.DrawSector(vh, pcb, radius, rectStartColor, areaColor, 270, 450, 1, isYAxis);
                        }
                        else
                        {
                            CheckClipAndDrawPolygon(vh, prb - diff, plb - diff, plt + diff, prt + diff, areaColor, areaToColor, serie.clip);
                            ChartDrawer.DrawSector(vh, pct, radius, areaToColor, 90, 270);
                            ChartDrawer.DrawSector(vh, pcb, radius, areaColor, 270, 450);
                        }
                    }
                }
            }
        }

        private void DrawBarBackground(VertexHelper vh, Serie serie, SerieData serieData, ItemStyle itemStyle, int colorIndex,
             bool highlight, float pX, float pY, float space, float barWidth, bool isYAxis)
        {
            Color color = SerieHelper.GetItemBackgroundColor(serie, serieData, m_ThemeInfo, colorIndex, highlight, false);
            if (ChartHelper.IsClearColor(color)) return;
            if (isYAxis)
            {
                var axis = m_YAxises[serie.axisIndex];
                var axisWidth = axis.axisLine.width;
                Vector3 plt = new Vector3(m_CoordinateX + axisWidth, pY + space + barWidth);
                Vector3 prt = new Vector3(m_CoordinateX + axisWidth + m_CoordinateWidth, pY + space + barWidth);
                Vector3 prb = new Vector3(m_CoordinateX + axisWidth + m_CoordinateWidth, pY + space);
                Vector3 plb = new Vector3(m_CoordinateX + axisWidth, pY + space);
                if (serie.barType == BarType.Capsule)
                {
                    var radius = barWidth / 2;
                    var diff = Vector3.right * radius;
                    var pcl = (plt + plb) / 2 + diff;
                    var pcr = (prt + prb) / 2 - diff;
                    CheckClipAndDrawPolygon(vh, plb + diff, plt + diff, prt - diff, prb - diff, color, color, serie.clip);
                    ChartDrawer.DrawSector(vh, pcl, radius, color, 180, 360);
                    ChartDrawer.DrawSector(vh, pcr, radius, color, 0, 180);
                    if (itemStyle.NeedShowBorder())
                    {
                        var borderWidth = itemStyle.borderWidth;
                        var borderColor = itemStyle.borderColor;
                        var smoothness = m_Settings.cicleSmoothness;
                        var inRadius = radius - borderWidth;
                        var outRadius = radius;
                        var p1 = plb + diff + Vector3.up * borderWidth / 2;
                        var p2 = prb - diff + Vector3.up * borderWidth / 2;
                        var p3 = plt + diff - Vector3.up * borderWidth / 2;
                        var p4 = prt - diff - Vector3.up * borderWidth / 2;
                        ChartDrawer.DrawLine(vh, p1, p2, borderWidth / 2, borderColor);
                        ChartDrawer.DrawLine(vh, p3, p4, borderWidth / 2, borderColor);
                        ChartDrawer.DrawDoughnut(vh, pcl, inRadius, outRadius, borderColor, Color.clear, 180, 360, smoothness);
                        ChartDrawer.DrawDoughnut(vh, pcr, inRadius, outRadius, borderColor, Color.clear, 0, 180, smoothness);
                    }
                }
                else
                {
                    CheckClipAndDrawPolygon(vh, ref plb, ref plt, ref prt, ref prb, color, color, serie.clip);
                }
            }
            else
            {
                var axis = m_XAxises[serie.axisIndex];
                var axisWidth = axis.axisLine.width;
                Vector3 plb = new Vector3(pX + space, m_CoordinateY + axisWidth);
                Vector3 plt = new Vector3(pX + space, m_CoordinateY + m_CoordinateHeight + axisWidth);
                Vector3 prt = new Vector3(pX + space + barWidth, m_CoordinateY + m_CoordinateHeight + axisWidth);
                Vector3 prb = new Vector3(pX + space + barWidth, m_CoordinateY + axisWidth);
                if (serie.barType == BarType.Capsule)
                {
                    var radius = barWidth / 2;
                    var diff = Vector3.up * radius;
                    var pct = (plt + prt) / 2 - diff;
                    var pcb = (plb + prb) / 2 + diff;
                    CheckClipAndDrawPolygon(vh, prb + diff, plb + diff, plt - diff, prt - diff, color, color, serie.clip);
                    ChartDrawer.DrawSector(vh, pct, radius, color, 270, 450);
                    ChartDrawer.DrawSector(vh, pcb, radius, color, 90, 270);
                    if (itemStyle.NeedShowBorder())
                    {
                        var borderWidth = itemStyle.borderWidth;
                        var borderColor = itemStyle.borderColor;
                        var smoothness = m_Settings.cicleSmoothness;
                        var inRadius = radius - borderWidth;
                        var outRadius = radius;
                        var p1 = plb + diff + Vector3.right * borderWidth / 2;
                        var p2 = plt - diff + Vector3.right * borderWidth / 2;
                        var p3 = prb + diff - Vector3.right * borderWidth / 2;
                        var p4 = prt - diff - Vector3.right * borderWidth / 2;
                        ChartDrawer.DrawLine(vh, p1, p2, borderWidth / 2, borderColor);
                        ChartDrawer.DrawLine(vh, p3, p4, borderWidth / 2, borderColor);
                        ChartDrawer.DrawDoughnut(vh, pct, inRadius, outRadius, borderColor, Color.clear, 270, 450, smoothness);
                        ChartDrawer.DrawDoughnut(vh, pcb, inRadius, outRadius, borderColor, Color.clear, 90, 270, smoothness);
                    }
                }
                else
                {
                    CheckClipAndDrawPolygon(vh, ref prb, ref plb, ref plt, ref prt, color, color, serie.clip);
                }
            }
        }

        private float GetBarGap()
        {
            float gap = 0.3f;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                if (serie.type == SerieType.Bar)
                {
                    if (serie.barGap != 0)
                    {
                        gap = serie.barGap;
                    }
                }
            }
            return gap;
        }

        private float GetSameStackTotalValue(string stack, int dataIndex)
        {
            if (string.IsNullOrEmpty(stack)) return 0;
            float total = 0;
            foreach (var serie in m_Series.list)
            {
                if (serie.type == SerieType.Bar)
                {
                    if (stack.Equals(serie.stack))
                    {
                        total += serie.data[dataIndex].data[1];
                    }
                }
            }
            return total;
        }


        private HashSet<string> barStackSet = new HashSet<string>();
        private float GetBarTotalWidth(float categoryWidth, float gap)
        {
            float total = 0;
            float lastGap = 0;
            barStackSet.Clear();
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                if (serie.type == SerieType.Bar && serie.show)
                {
                    if (!string.IsNullOrEmpty(serie.stack))
                    {
                        if (barStackSet.Contains(serie.stack)) continue;
                        barStackSet.Add(serie.stack);
                    }
                    var width = GetStackBarWidth(categoryWidth, serie);
                    if (gap == -1)
                    {
                        if (width > total) total = width;
                    }
                    else
                    {
                        lastGap = width * gap;
                        total += width;
                        total += lastGap;
                    }
                }
            }
            if (total > 0 && gap != -1) total -= lastGap;
            return total;
        }

        private float GetStackBarWidth(float categoryWidth, Serie now)
        {
            if (string.IsNullOrEmpty(now.stack)) return now.GetBarWidth(categoryWidth);
            float barWidth = 0;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                if (serie.type == SerieType.Bar && serie.show && now.stack.Equals(serie.stack))
                {
                    if (serie.barWidth > barWidth) barWidth = serie.barWidth;
                }
            }
            if (barWidth > 1) return barWidth;
            else return barWidth * categoryWidth;
        }
    }
}