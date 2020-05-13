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
        private bool m_VisualMapMinDrag;
        private bool m_VisualMapMaxDrag;

        protected void CheckVisualMap()
        {
            if (!m_VisualMap.enable || !m_VisualMap.show) return;
            Vector2 local;
            if (canvas == null) return;

            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                Input.mousePosition, canvas.worldCamera, out local))
            {
                if (m_VisualMap.runtimeSelectedIndex >= 0)
                {
                    m_VisualMap.runtimeSelectedIndex = -1;
                    RefreshChart();
                }
                return;
            }
            if (local.x < chartX || local.x > chartX + chartWidth ||
                local.y < chartY || local.y > chartY + chartHeight ||
                !m_VisualMap.IsInRangeRect(local, chartRect))
            {
                if (m_VisualMap.runtimeSelectedIndex >= 0)
                {
                    m_VisualMap.runtimeSelectedIndex = -1;
                    RefreshChart();
                }
                return;
            }
            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            var halfHig = m_VisualMap.itemHeight / 2;
            var centerPos = chartPosition + m_VisualMap.location.GetPosition(chartWidth, chartHeight);
            var selectedIndex = -1;
            var value = 0f;
            switch (m_VisualMap.orient)
            {
                case Orient.Horizonal:
                    pos1 = centerPos + Vector3.left * halfHig;
                    pos2 = centerPos + Vector3.right * halfHig;
                    value = m_VisualMap.min + (local.x - pos1.x) / (pos2.x - pos1.x) * (m_VisualMap.max - m_VisualMap.min);
                    selectedIndex = m_VisualMap.GetIndex(value);
                    break;
                case Orient.Vertical:
                    pos1 = centerPos + Vector3.down * halfHig;
                    pos2 = centerPos + Vector3.up * halfHig;
                    value = m_VisualMap.min + (local.y - pos1.y) / (pos2.y - pos1.y) * (m_VisualMap.max - m_VisualMap.min);
                    selectedIndex = m_VisualMap.GetIndex(value);
                    break;
            }
            m_VisualMap.runtimeSelectedValue = value;
            m_VisualMap.runtimeSelectedIndex = selectedIndex;
            RefreshChart();
        }

        protected void OnDragVisualMapStart()
        {
            if (!m_VisualMap.enable || !m_VisualMap.show || !m_VisualMap.calculable) return;
            var inMinRect = m_VisualMap.IsInRangeMinRect(pointerPos, chartRect, m_Settings.visualMapTriangeLen);
            var inMaxRect = m_VisualMap.IsInRangeMaxRect(pointerPos, chartRect, m_Settings.visualMapTriangeLen);
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
            if (!m_VisualMap.enable || !m_VisualMap.show || !m_VisualMap.calculable) return;
            if (!m_VisualMapMinDrag && !m_VisualMapMaxDrag) return;

            var value = m_VisualMap.GetValue(pointerPos, chartRect);
            if (m_VisualMapMinDrag)
            {
                m_VisualMap.rangeMin = value;
            }
            else
            {
                m_VisualMap.rangeMax = value;
            }
            RefreshChart();
        }

        protected void OnDragVisualMapEnd()
        {
            if (!m_VisualMap.enable || !m_VisualMap.show || !m_VisualMap.calculable) return;
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
            var yAxis = m_YAxises[serie.axisIndex];
            var xAxis = m_XAxises[serie.axisIndex];
            var xCount = xAxis.data.Count;
            var yCount = yAxis.data.Count;
            var xWidth = m_CoordinateWidth / xCount;
            var yWidth = m_CoordinateHeight / yCount;

            var zeroX = m_CoordinateX;
            var zeroY = m_CoordinateY;
            var dataList = serie.GetDataList();
            var rangeMin = m_VisualMap.rangeMin;
            var rangeMax = m_VisualMap.rangeMax;
            var color = m_ThemeInfo.GetColor(serie.index);
            var borderWidth = serie.itemStyle.show ? serie.itemStyle.borderWidth : 0;
            var borderColor = serie.itemStyle.opacity > 0 ? serie.itemStyle.borderColor : Color.clear;
            borderColor.a *= serie.itemStyle.opacity;
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
                    var dimension = m_VisualMap.enable && m_VisualMap.dimension > 0 ? m_VisualMap.dimension - 1 :
                        serieData.data.Count - 1;
                    if (serie.IsIgnoreIndex(dataIndex, dimension))
                    {
                        serie.dataPoints.Add(Vector3.zero);
                        continue;
                    }
                    var value = serieData.GetCurrData(dimension, dataChangeDuration, yAxis.inverse);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    var pos = new Vector3(zeroX + (i + 0.5f) * xWidth, zeroY + (j + 0.5f) * yWidth);
                    serie.dataPoints.Add(pos);
                    serieData.canShowLabel = false;
                    if (value == 0) continue;
                    if (m_VisualMap.enable)
                    {
                        if ((value < rangeMin && rangeMin != m_VisualMap.min)
                            || (value > rangeMax && rangeMax != m_VisualMap.max))
                        {
                            continue;
                        }
                        if (!m_VisualMap.IsInSelectedValue(value)) continue;
                        color = m_VisualMap.GetColor(value);
                    }
                    if (animationIndex >= 0 && i > animationIndex) continue;
                    serieData.canShowLabel = true;
                    var emphasis = (m_Tooltip.show && i == (int)m_Tooltip.runtimeXValues[0] && j == (int)m_Tooltip.runtimeYValues[0])
                        || m_VisualMap.runtimeSelectedIndex > 0;
                    var rectWid = xWidth - 2 * borderWidth;
                    var rectHig = yWidth - 2 * borderWidth;
                    ChartDrawer.DrawPolygon(vh, pos, rectWid / 2, rectHig / 2, color);
                    if (borderWidth > 0 && !ChartHelper.IsClearColor(borderColor))
                    {
                        ChartDrawer.DrawBorder(vh, pos, rectWid, rectHig, borderWidth, borderColor);
                    }
                    if (m_VisualMap.hoverLink && emphasis && serie.emphasis.show && serie.emphasis.itemStyle.borderWidth > 0)
                    {
                        var emphasisBorderWidth = serie.emphasis.itemStyle.borderWidth;
                        var emphasisBorderColor = serie.emphasis.itemStyle.opacity > 0 ? serie.emphasis.itemStyle.borderColor : Color.clear;
                        ChartDrawer.DrawBorder(vh, pos, rectWid, rectHig, emphasisBorderWidth, emphasisBorderColor);
                    }
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(xCount);
                m_IsPlayingAnimation = true;
                RefreshChart();
            }
            if (dataChanging)
            {
                RefreshChart();
            }
        }

        protected void DrawVisualMap(VertexHelper vh)
        {
            if (!m_VisualMap.enable || !m_VisualMap.show) return;
            var centerPos = chartPosition + m_VisualMap.location.GetPosition(chartWidth, chartHeight);

            var pos1 = Vector3.zero;
            var pos2 = Vector3.zero;
            var dir = Vector3.zero;
            var halfWid = m_VisualMap.itemWidth / 2;
            var halfHig = m_VisualMap.itemHeight / 2;
            var xRadius = 0f;
            var yRadius = 0f;
            var splitNum = m_VisualMap.runtimeInRange.Count;
            var splitWid = m_VisualMap.itemHeight / (splitNum - 1);
            var isVertical = false;
            var colors = m_VisualMap.runtimeInRange;
            var triangeLen = m_Settings.visualMapTriangeLen;
            switch (m_VisualMap.orient)
            {
                case Orient.Horizonal:
                    pos1 = centerPos + Vector3.left * halfHig;
                    pos2 = centerPos + Vector3.right * halfHig;
                    dir = Vector3.right;
                    xRadius = splitWid / 2;
                    yRadius = halfWid;
                    isVertical = false;
                    if (m_VisualMap.calculable)
                    {
                        var p0 = pos1 + Vector3.right * m_VisualMap.runtimeRangeMinHeight;
                        var p1 = p0 + Vector3.up * halfWid;
                        var p2 = p0 + Vector3.up * (halfWid + triangeLen);
                        var p3 = p2 + Vector3.left * triangeLen;
                        var color = m_VisualMap.GetColor(m_VisualMap.rangeMin);
                        ChartDrawer.DrawTriangle(vh, p1, p2, p3, color);
                        p0 = pos1 + Vector3.right * m_VisualMap.runtimeRangeMaxHeight;
                        p1 = p0 + Vector3.up * halfWid;
                        p2 = p0 + Vector3.up * (halfWid + triangeLen);
                        p3 = p2 + Vector3.right * triangeLen;
                        color = m_VisualMap.GetColor(m_VisualMap.rangeMax);
                        ChartDrawer.DrawTriangle(vh, p1, p2, p3, color);
                    }
                    break;
                case Orient.Vertical:
                    pos1 = centerPos + Vector3.down * halfHig;
                    pos2 = centerPos + Vector3.up * halfHig;
                    dir = Vector3.up;
                    xRadius = halfWid;
                    yRadius = splitWid / 2;
                    isVertical = true;
                    if (m_VisualMap.calculable)
                    {
                        var p0 = pos1 + Vector3.up * m_VisualMap.runtimeRangeMinHeight;
                        var p1 = p0 + Vector3.right * halfWid;
                        var p2 = p0 + Vector3.right * (halfWid + triangeLen);
                        var p3 = p2 + Vector3.down * triangeLen;
                        var color = m_VisualMap.GetColor(m_VisualMap.rangeMin);
                        ChartDrawer.DrawTriangle(vh, p1, p2, p3, color);
                        p0 = pos1 + Vector3.up * m_VisualMap.runtimeRangeMaxHeight;
                        p1 = p0 + Vector3.right * halfWid;
                        p2 = p0 + Vector3.right * (halfWid + triangeLen);
                        p3 = p2 + Vector3.up * triangeLen;
                        color = m_VisualMap.GetColor(m_VisualMap.rangeMax);
                        ChartDrawer.DrawTriangle(vh, p1, p2, p3, color);
                    }
                    break;
            }
            if (m_VisualMap.calculable && (m_VisualMap.rangeMin > m_VisualMap.min
                || m_VisualMap.rangeMax < m_VisualMap.max))
            {
                var rangeMin = m_VisualMap.rangeMin;
                var rangeMax = m_VisualMap.rangeMax;
                var diff = (m_VisualMap.max - m_VisualMap.min) / (splitNum - 1);
                for (int i = 1; i < splitNum; i++)
                {
                    var splitMin = m_VisualMap.min + (i - 1) * diff;
                    var splitMax = splitMin + diff;
                    if (rangeMin > splitMax || rangeMax < splitMin)
                    {
                        continue;
                    }
                    else if (rangeMin <= splitMin && rangeMax >= splitMax)
                    {
                        var splitPos = pos1 + dir * (i - 1 + 0.5f) * splitWid;
                        var startColor = colors[i - 1];
                        var toColor = colors[i];
                        ChartDrawer.DrawPolygon(vh, splitPos, xRadius, yRadius, startColor, toColor, isVertical);
                    }
                    else if (rangeMin > splitMin && rangeMax >= splitMax)
                    {
                        var p0 = pos1 + dir * m_VisualMap.runtimeRangeMinHeight;
                        var splitMaxPos = pos1 + dir * i * splitWid;
                        var splitPos = p0 + (splitMaxPos - p0) / 2;
                        var startColor = m_VisualMap.GetColor(m_VisualMap.rangeMin);
                        var toColor = colors[i];
                        var yRadius1 = Vector3.Distance(p0, splitMaxPos) / 2;
                        if (m_VisualMap.orient == Orient.Vertical)
                            ChartDrawer.DrawPolygon(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            ChartDrawer.DrawPolygon(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                    else if (rangeMax < splitMax && rangeMin <= splitMin)
                    {
                        var p0 = pos1 + dir * m_VisualMap.runtimeRangeMaxHeight;
                        var splitMinPos = pos1 + dir * (i - 1) * splitWid;
                        var splitPos = splitMinPos + (p0 - splitMinPos) / 2;
                        var startColor = colors[i - 1];
                        var toColor = m_VisualMap.GetColor(m_VisualMap.rangeMax);
                        var yRadius1 = Vector3.Distance(p0, splitMinPos) / 2;
                        if (m_VisualMap.orient == Orient.Vertical)
                            ChartDrawer.DrawPolygon(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            ChartDrawer.DrawPolygon(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                    else
                    {
                        var p0 = pos1 + dir * m_VisualMap.runtimeRangeMinHeight;
                        var p1 = pos1 + dir * m_VisualMap.runtimeRangeMaxHeight;
                        var splitPos = (p0 + p1) / 2;
                        var startColor = m_VisualMap.GetColor(m_VisualMap.rangeMin);
                        var toColor = m_VisualMap.GetColor(m_VisualMap.rangeMax);
                        var yRadius1 = Vector3.Distance(p0, p1) / 2;
                        if (m_VisualMap.orient == Orient.Vertical)
                            ChartDrawer.DrawPolygon(vh, splitPos, xRadius, yRadius1, startColor, toColor, isVertical);
                        else
                            ChartDrawer.DrawPolygon(vh, splitPos, yRadius1, yRadius, startColor, toColor, isVertical);
                    }
                }
            }
            else
            {
                for (int i = 1; i < splitNum; i++)
                {
                    var splitPos = pos1 + dir * (i - 1 + 0.5f) * splitWid;
                    var startColor = colors[i - 1];
                    var toColor = colors[i];
                    ChartDrawer.DrawPolygon(vh, splitPos, xRadius, yRadius, startColor, toColor, isVertical);
                }
            }

            if (m_VisualMap.rangeMin > m_VisualMap.min)
            {
                var p0 = pos1 + dir * m_VisualMap.runtimeRangeMinHeight;
                ChartDrawer.DrawPolygon(vh, pos1, p0, m_VisualMap.itemWidth / 2, m_ThemeInfo.visualMapBackgroundColor);
            }
            if (m_VisualMap.rangeMax < m_VisualMap.max)
            {
                var p1 = pos1 + dir * m_VisualMap.runtimeRangeMaxHeight;
                ChartDrawer.DrawPolygon(vh, p1, pos2, m_VisualMap.itemWidth / 2, m_ThemeInfo.visualMapBackgroundColor);
            }

            if (m_VisualMap.hoverLink)
            {
                if (m_VisualMap.runtimeSelectedIndex >= 0)
                {
                    var p0 = pos1 + dir * m_VisualMap.runtimeRangeMinHeight;
                    var p1 = pos1 + dir * m_VisualMap.runtimeRangeMaxHeight;

                    if (m_VisualMap.orient == Orient.Vertical)
                    {
                        var p2 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y + (triangeLen / 2), p0.y, p1.y));
                        var p3 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y - (triangeLen / 2), p0.y, p1.y));
                        var p4 = new Vector3(centerPos.x + halfWid + triangeLen / 2, pointerPos.y);
                        ChartDrawer.DrawTriangle(vh, p2, p3, p4, colors[m_VisualMap.runtimeSelectedIndex]);
                    }
                    else
                    {
                        var p2 = new Vector3(Mathf.Clamp(pointerPos.x + (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                        var p3 = new Vector3(Mathf.Clamp(pointerPos.x - (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                        var p4 = new Vector3(pointerPos.x, centerPos.y + halfWid + triangeLen / 2);
                        ChartDrawer.DrawTriangle(vh, p2, p3, p4, colors[m_VisualMap.runtimeSelectedIndex]);
                    }
                }
                else if (m_Tooltip.show && m_Tooltip.runtimeXValues[0] >= 0 && m_Tooltip.runtimeYValues[0] >= 0)
                {
                    // var p0 = pos1 + dir * m_VisualMap.rangeMinHeight;
                    // var p1 = pos1 + dir * m_VisualMap.rangeMaxHeight;
                    // if (m_VisualMap.orient == Orient.Vertical)
                    // {
                    //     var p2 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y + (triangeLen / 2), p0.y, p1.y));
                    //     var p3 = new Vector3(centerPos.x + halfWid, Mathf.Clamp(pointerPos.y - (triangeLen / 2), p0.y, p1.y));
                    //     var p4 = new Vector3(centerPos.x + halfWid + triangeLen / 2, pointerPos.y);
                    //     ChartDrawer.DrawTriangle(vh, p2, p3, p4, colors[m_VisualMap.rtSelectedIndex]);
                    // }
                    // else
                    // {
                    //     var p2 = new Vector3(Mathf.Clamp(pointerPos.x + (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                    //     var p3 = new Vector3(Mathf.Clamp(pointerPos.x - (triangeLen / 2), p0.x, p1.x), centerPos.y + halfWid);
                    //     var p4 = new Vector3(pointerPos.x, centerPos.y + halfWid + triangeLen / 2);
                    //     ChartDrawer.DrawTriangle(vh, p2, p3, p4, colors[m_VisualMap.rtSelectedIndex]);
                    // }
                }
            }
        }
    }
}