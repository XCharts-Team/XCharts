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
        private bool m_VisualMapMinDrag;
        private bool m_VisualMapMaxDrag;

        protected void CheckVisualMap()
        {
            if (visualMap == null || !visualMap.enable || !visualMap.show) return;
            Vector2 local;
            if (canvas == null) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                Input.mousePosition, canvas.worldCamera, out local))
            {
                if (visualMap.runtimeSelectedIndex >= 0)
                {
                    visualMap.runtimeSelectedIndex = -1;
                    RefreshChart();
                }
                return;
            }
            if (local.x < chartX || local.x > chartX + chartWidth ||
                local.y < chartY || local.y > chartY + chartHeight ||
                !visualMap.IsInRangeRect(local, chartRect))
            {
                if (visualMap.runtimeSelectedIndex >= 0)
                {
                    visualMap.runtimeSelectedIndex = -1;
                    RefreshChart();
                }
                return;
            }
            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            var halfHig = visualMap.itemHeight / 2;
            var centerPos = chartPosition + visualMap.location.GetPosition(chartWidth, chartHeight);
            var selectedIndex = -1;
            var value = 0f;
            switch (visualMap.orient)
            {
                case Orient.Horizonal:
                    pos1 = centerPos + Vector3.left * halfHig;
                    pos2 = centerPos + Vector3.right * halfHig;
                    value = visualMap.min + (local.x - pos1.x) / (pos2.x - pos1.x) * (visualMap.max - visualMap.min);
                    selectedIndex = visualMap.GetIndex(value);
                    break;
                case Orient.Vertical:
                    pos1 = centerPos + Vector3.down * halfHig;
                    pos2 = centerPos + Vector3.up * halfHig;
                    value = visualMap.min + (local.y - pos1.y) / (pos2.y - pos1.y) * (visualMap.max - visualMap.min);
                    selectedIndex = visualMap.GetIndex(value);
                    break;
            }
            visualMap.runtimeSelectedValue = value;
            visualMap.runtimeSelectedIndex = selectedIndex;
            RefreshChart();
        }

        protected void OnDragVisualMapStart()
        {
            if (!visualMap.enable || !visualMap.show || !visualMap.calculable) return;
            var inMinRect = visualMap.IsInRangeMinRect(pointerPos, chartRect, m_Theme.visualMap.triangeLen);
            var inMaxRect = visualMap.IsInRangeMaxRect(pointerPos, chartRect, m_Theme.visualMap.triangeLen);
            if (inMinRect || inMaxRect)
            {
                if (inMinRect)
                {
                    m_VisualMapMinDrag = true;
                }
                else
                {
                    m_VisualMapMaxDrag = true;
                }
            }
        }

        protected void OnDragVisualMap()
        {
            if (!visualMap.enable || !visualMap.show || !visualMap.calculable) return;
            if (!m_VisualMapMinDrag && !m_VisualMapMaxDrag) return;

            var value = visualMap.GetValue(pointerPos, chartRect);
            if (m_VisualMapMinDrag)
            {
                visualMap.rangeMin = value;
            }
            else
            {
                visualMap.rangeMax = value;
            }
            RefreshChart();
        }

        protected void OnDragVisualMapEnd()
        {
            if (!visualMap.enable || !visualMap.show || !visualMap.calculable) return;
            if (m_VisualMapMinDrag || m_VisualMapMaxDrag)
            {
                RefreshChart();
                m_VisualMapMinDrag = false;
                m_VisualMapMaxDrag = false;
            }
        }

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
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, borderWidth, borderColor);
                    }
                    if (visualMap.hoverLink && emphasis && serie.emphasis.show 
                        && serie.emphasis.itemStyle.borderWidth > 0)
                    {
                        var emphasisBorderWidth = serie.emphasis.itemStyle.borderWidth;
                        var emphasisBorderColor = serie.emphasis.itemStyle.opacity > 0 
                            ? serie.emphasis.itemStyle.borderColor : ChartConst.clearColor32;
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, emphasisBorderWidth, emphasisBorderColor);
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

        protected void DrawVisualMap(VertexHelper vh)
        {
            if (!visualMap.enable || !visualMap.show) return;
            var centerPos = chartPosition + visualMap.location.GetPosition(chartWidth, chartHeight);

            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            var dir = Vector3.zero;
            var halfWid = visualMap.itemWidth / 2;
            var halfHig = visualMap.itemHeight / 2;
            var xRadius = 0f;
            var yRadius = 0f;
            var splitNum = visualMap.runtimeInRange.Count;
            var splitWid = visualMap.itemHeight / (splitNum - 1);
            var isVertical = false;
            var colors = visualMap.runtimeInRange;
            var triangeLen = m_Theme.visualMap.triangeLen;
            switch (visualMap.orient)
            {
                case Orient.Horizonal:
                    pos1 = centerPos + Vector3.left * halfHig;
                    pos2 = centerPos + Vector3.right * halfHig;
                    dir = Vector3.right;
                    xRadius = splitWid / 2;
                    yRadius = halfWid;
                    isVertical = false;
                    if (visualMap.calculable)
                    {
                        var p0 = pos1 + Vector3.right * visualMap.runtimeRangeMinHeight;
                        var p1 = p0 + Vector3.up * halfWid;
                        var p2 = p0 + Vector3.up * (halfWid + triangeLen);
                        var p3 = p2 + Vector3.left * triangeLen;
                        var color = visualMap.GetColor(visualMap.rangeMin);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                        p0 = pos1 + Vector3.right * visualMap.runtimeRangeMaxHeight;
                        p1 = p0 + Vector3.up * halfWid;
                        p2 = p0 + Vector3.up * (halfWid + triangeLen);
                        p3 = p2 + Vector3.right * triangeLen;
                        color = visualMap.GetColor(visualMap.rangeMax);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                    }
                    break;
                case Orient.Vertical:
                    pos1 = centerPos + Vector3.down * halfHig;
                    pos2 = centerPos + Vector3.up * halfHig;
                    dir = Vector3.up;
                    xRadius = halfWid;
                    yRadius = splitWid / 2;
                    isVertical = true;
                    if (visualMap.calculable)
                    {
                        var p0 = pos1 + Vector3.up * visualMap.runtimeRangeMinHeight;
                        var p1 = p0 + Vector3.right * halfWid;
                        var p2 = p0 + Vector3.right * (halfWid + triangeLen);
                        var p3 = p2 + Vector3.down * triangeLen;
                        var color = visualMap.GetColor(visualMap.rangeMin);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                        p0 = pos1 + Vector3.up * visualMap.runtimeRangeMaxHeight;
                        p1 = p0 + Vector3.right * halfWid;
                        p2 = p0 + Vector3.right * (halfWid + triangeLen);
                        p3 = p2 + Vector3.up * triangeLen;
                        color = visualMap.GetColor(visualMap.rangeMax);
                        UGL.DrawTriangle(vh, p1, p2, p3, color);
                    }
                    break;
            }
            if (visualMap.calculable && (visualMap.rangeMin > visualMap.min
                || visualMap.rangeMax < visualMap.max))
            {
                var rangeMin = visualMap.rangeMin;
                var rangeMax = visualMap.rangeMax;
                var diff = (visualMap.max - visualMap.min) / (splitNum - 1);
                for (int i = 1; i < splitNum; i++)
                {
                    var splitMin = visualMap.min + (i - 1) * diff;
                    var splitMax = splitMin + diff;
                    if (rangeMin > splitMax || rangeMax < splitMin)
                    {
                        continue;
                    }
                    else if (rangeMin <= splitMin && rangeMax >= splitMax)
                    {
                        var splitPos = pos1 + dir * (i - 1 + 0.5f) * splitWid;
                        var startColor = colors[i - 1];
                        var toColor = visualMap.IsPiecewise() ? startColor : colors[i];
                        UGL.DrawRectangle(vh, splitPos, xRadius, yRadius, startColor, toColor, isVertical);
                    }
                    else if (rangeMin > splitMin && rangeMax >= splitMax)
                    {
                        var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                        var splitMaxPos = pos1 + dir * i * splitWid;
                        var splitPos = p0 + (splitMaxPos - p0) / 2;
                        var startColor = visualMap.GetColor(visualMap.rangeMin);
                        var toColor = visualMap.IsPiecewise() ? startColor : colors[i];
                        var yRadius1 = Vector3.Distance(p0, splitMaxPos) / 2;
                        if (visualMap.orient == Orient.Vertical)
                            UGL.DrawRectangle(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            UGL.DrawRectangle(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                    else if (rangeMax < splitMax && rangeMin <= splitMin)
                    {
                        var p0 = pos1 + dir * visualMap.runtimeRangeMaxHeight;
                        var splitMinPos = pos1 + dir * (i - 1) * splitWid;
                        var splitPos = splitMinPos + (p0 - splitMinPos) / 2;
                        var startColor = colors[i - 1];
                        var toColor = visualMap.IsPiecewise() ? startColor : visualMap.GetColor(visualMap.rangeMax);
                        var yRadius1 = Vector3.Distance(p0, splitMinPos) / 2;
                        if (visualMap.orient == Orient.Vertical)
                            UGL.DrawRectangle(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            UGL.DrawRectangle(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                    else
                    {
                        var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                        var p1 = pos1 + dir * visualMap.runtimeRangeMaxHeight;
                        var splitPos = (p0 + p1) / 2;
                        var startColor = visualMap.GetColor(visualMap.rangeMin);
                        var toColor = visualMap.GetColor(visualMap.rangeMax);
                        var yRadius1 = Vector3.Distance(p0, p1) / 2;
                        if (visualMap.orient == Orient.Vertical)
                            UGL.DrawRectangle(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            UGL.DrawRectangle(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                }
            }
            else
            {
                for (int i = 1; i < splitNum; i++)
                {
                    var splitPos = pos1 + dir * (i - 1 + 0.5f) * splitWid;
                    var startColor = colors[i - 1];
                    var toColor = visualMap.IsPiecewise() ? startColor : colors[i];
                    UGL.DrawRectangle(vh, splitPos, xRadius, yRadius, startColor, toColor, isVertical);
                }
            }

            if (visualMap.rangeMin > visualMap.min)
            {
                var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                UGL.DrawRectangle(vh, pos1, p0, visualMap.itemWidth / 2, m_Theme.visualMap.backgroundColor);
            }
            if (visualMap.rangeMax < visualMap.max)
            {
                var p1 = pos1 + dir * visualMap.runtimeRangeMaxHeight;
                UGL.DrawRectangle(vh, p1, pos2, visualMap.itemWidth / 2, m_Theme.visualMap.backgroundColor);
            }

            if (visualMap.hoverLink)
            {
                if (visualMap.runtimeSelectedIndex >= 0)
                {
                    var p0 = pos1 + dir * visualMap.runtimeRangeMinHeight;
                    var p1 = pos1 + dir * visualMap.runtimeRangeMaxHeight;

                    if (visualMap.orient == Orient.Vertical)
                    {
                        var p2 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y + (triangeLen / 2), p0.y, p1.y));
                        var p3 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y - (triangeLen / 2), p0.y, p1.y));
                        var p4 = new Vector3(centerPos.x + halfWid + triangeLen / 2, pointerPos.y);
                        UGL.DrawTriangle(vh, p2, p3, p4, colors[visualMap.runtimeSelectedIndex]);
                    }
                    else
                    {
                        var p2 = new Vector3(Mathf.Clamp(pointerPos.x + (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                        var p3 = new Vector3(Mathf.Clamp(pointerPos.x - (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                        var p4 = new Vector3(pointerPos.x, centerPos.y + halfWid + triangeLen / 2);
                        UGL.DrawTriangle(vh, p2, p3, p4, colors[visualMap.runtimeSelectedIndex]);
                    }
                }
                else if (tooltip.show && tooltip.runtimeXValues[0] >= 0 && tooltip.runtimeYValues[0] >= 0)
                {
                    // var p0 = pos1 + dir * m_VisualMap.rangeMinHeight;
                    // var p1 = pos1 + dir * m_VisualMap.rangeMaxHeight;
                    // if (m_VisualMap.orient == Orient.Vertical)
                    // {
                    //     var p2 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y + (triangeLen / 2), p0.y, p1.y));
                    //     var p3 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y - (triangeLen / 2), p0.y, p1.y));
                    //     var p4 = new Vector3(centerPos.x + halfWid + triangeLen / 2, pointerPos.y);
                    //     UGL.DrawTriangle(vh, p2, p3, p4, colors[m_VisualMap.rtSelectedIndex]);
                    // }
                    // else
                    // {
                    //     var p2 = new Vector3(Mathf.Clamp(pointerPos.x + (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                    //     var p3 = new Vector3(Mathf.Clamp(pointerPos.x - (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                    //     var p4 = new Vector3(pointerPos.x, centerPos.y + halfWid + triangeLen / 2);
                    //     UGL.DrawTriangle(vh, p2, p3, p4, colors[m_VisualMap.rtSelectedIndex]);
                    // }
                }
            }
        }
    }
}