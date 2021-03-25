/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public static partial class SerieHelper
    {
        internal static Color32 GetItemBackgroundColor(Serie serie, SerieData serieData, ChartTheme theme, int index,
            bool highlight, bool useDefault = true)
        {
            var color = ChartConst.clearColor32;
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, serieData);
                if (itemStyleEmphasis != null && !ChartHelper.IsClearColor(itemStyleEmphasis.backgroundColor))
                {
                    color = itemStyleEmphasis.backgroundColor;
                    ChartHelper.SetColorOpacity(ref color, itemStyleEmphasis.opacity);
                    return color;
                }
            }
            var itemStyle = GetItemStyle(serie, serieData);
            if (!ChartHelper.IsClearColor(itemStyle.backgroundColor))
            {
                color = itemStyle.backgroundColor;
                if (highlight) color = ChartHelper.GetHighlightColor(color);
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
                return color;
            }
            else if (useDefault)
            {
                color = theme.GetColor(index);
                if (highlight) color = ChartHelper.GetHighlightColor(color);
                color.a = 50;
                return color;
            }
            return color;
        }

        internal static Color32 GetItemColor(Serie serie, SerieData serieData, ChartTheme theme, int index, bool highlight)
        {
            if (serie == null) return ChartConst.clearColor32;
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, serieData);
                if (itemStyleEmphasis != null && !ChartHelper.IsClearColor(itemStyleEmphasis.color))
                {
                    var color = itemStyleEmphasis.color;
                    ChartHelper.SetColorOpacity(ref color, itemStyleEmphasis.opacity);
                    return color;
                }
            }
            var itemStyle = GetItemStyle(serie, serieData);
            if (!ChartHelper.IsClearColor(itemStyle.color))
            {
                return itemStyle.GetColor();
            }
            else
            {
                var color = theme.GetColor(index);
                if (highlight) color = ChartHelper.GetHighlightColor(color);
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
                return color;
            }
        }
        internal static Color32 GetItemColor0(Serie serie, SerieData serieData, ChartTheme theme, bool highlight, Color32 defaultColor)
        {
            if (serie == null) return ChartConst.clearColor32;
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, serieData);
                if (itemStyleEmphasis != null && !ChartHelper.IsClearColor(itemStyleEmphasis.color))
                {
                    var color = itemStyleEmphasis.color0;
                    ChartHelper.SetColorOpacity(ref color, itemStyleEmphasis.opacity);
                    return color;
                }
            }
            var itemStyle = GetItemStyle(serie, serieData);
            if (!ChartHelper.IsClearColor(itemStyle.color0))
            {
                return itemStyle.GetColor0();
            }
            else
            {
                var color = defaultColor;
                if (highlight) color = ChartHelper.GetHighlightColor(color);
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
                return color;
            }
        }

        internal static Color32 GetItemToColor(Serie serie, SerieData serieData, ChartTheme theme, int index, bool highlight)
        {
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, serieData);
                if (itemStyleEmphasis != null && !ChartHelper.IsClearColor(itemStyleEmphasis.toColor))
                {
                    return itemStyleEmphasis.GetColor();
                }
            }
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle == null) itemStyle = serieData.itemStyle;
            if (!ChartHelper.IsClearColor(itemStyle.toColor))
            {
                var color = itemStyle.toColor;
                if (highlight) color = ChartHelper.GetHighlightColor(color);
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
                return color;
            }
            if (!ChartHelper.IsClearColor(itemStyle.color))
            {
                var color = itemStyle.color;
                if (highlight) color = ChartHelper.GetHighlightColor(color);
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
                return color;
            }
            else
            {
                var color = theme.GetColor(index);
                if (highlight) color = ChartHelper.GetHighlightColor(color);
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
                return color;
            }
        }

        internal static bool IsDownPoint(Serie serie, int index)
        {
            var dataPoints = serie.dataPoints;
            if (dataPoints.Count < 2) return false;
            else if (index > 0 && index < dataPoints.Count - 1)
            {
                var lp = dataPoints[index - 1];
                var np = dataPoints[index + 1];
                var cp = dataPoints[index];
                var dot = Vector3.Cross(np - lp, cp - np);
                return dot.z < 0;
            }
            else if (index == 0)
            {
                return dataPoints[0].y < dataPoints[1].y;
            }
            else if (index == dataPoints.Count - 1)
            {
                return dataPoints[index].y < dataPoints[index - 1].y;
            }
            else
            {
                return false;
            }
        }

        internal static ItemStyle GetItemStyle(Serie serie, SerieData serieData, bool highlight = false)
        {
            if (highlight)
            {
                var style = GetItemStyleEmphasis(serie, serieData);
                if (style == null) return GetItemStyle(serie, serieData, false);
                else return style;
            }
            else if (serie.IsPerformanceMode()) return serie.itemStyle;
            else if (serieData != null && serieData.enableItemStyle) return serieData.itemStyle;
            else return serie.itemStyle;
        }

        internal static ItemStyle GetItemStyleEmphasis(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData != null && serieData.enableEmphasis && serieData.emphasis.show)
                return serieData.emphasis.itemStyle;
            else if (serie.emphasis.show) return serie.emphasis.itemStyle;
            else return null;
        }

        internal static SerieLabel GetSerieLabel(Serie serie, SerieData serieData, bool highlight = false)
        {
            if (highlight)
            {
                if (!serie.IsPerformanceMode() && serieData.enableEmphasis && serieData.emphasis.show)
                    return serieData.emphasis.label;
                else if (serie.emphasis.show) return serie.emphasis.label;
                else return serie.label;
            }
            else
            {
                if (!serie.IsPerformanceMode() && serieData.enableLabel) return serieData.label;
                else return serie.label;
            }
        }

        internal static SerieSymbol GetSerieSymbol(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData.enableSymbol) return serieData.symbol;
            else return serie.symbol;
        }

        internal static Color32 GetAreaColor(Serie serie, ChartTheme theme, int index, bool highlight)
        {
            var areaStyle = serie.areaStyle;
            var color = !ChartHelper.IsClearColor(areaStyle.color) ? areaStyle.color : theme.GetColor(index);
            if (highlight)
            {
                if (!ChartHelper.IsClearColor(areaStyle.highlightColor)) color = areaStyle.highlightColor;
                else color = ChartHelper.GetHighlightColor(color);
            }
            ChartHelper.SetColorOpacity(ref color, areaStyle.opacity);
            return color;
        }

        internal static Color32 GetAreaToColor(Serie serie, ChartTheme theme, int index, bool highlight)
        {
            var areaStyle = serie.areaStyle;
            if (!ChartHelper.IsClearColor(areaStyle.toColor))
            {
                var color = areaStyle.toColor;
                if (highlight)
                {
                    if (!ChartHelper.IsClearColor(areaStyle.highlightToColor)) color = areaStyle.highlightToColor;
                    else color = ChartHelper.GetHighlightColor(color);
                }
                ChartHelper.SetColorOpacity(ref color, areaStyle.opacity);
                return color;
            }
            else
            {
                return GetAreaColor(serie, theme, index, highlight);
            }
        }

        internal static Color32 GetLineColor(Serie serie, ChartTheme theme, int index, bool highlight)
        {
            Color32 color = ChartConst.clearColor32;
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, null);
                if (itemStyleEmphasis != null && !ChartHelper.IsClearColor(itemStyleEmphasis.color))
                {
                    color = itemStyleEmphasis.color;
                    ChartHelper.SetColorOpacity(ref color, itemStyleEmphasis.opacity);
                    return color;
                }
            }
            if (!ChartHelper.IsClearColor(serie.lineStyle.color)) color = serie.lineStyle.GetColor();
            else if (!ChartHelper.IsClearColor(serie.itemStyle.color)) color = serie.itemStyle.GetColor();
            if (ChartHelper.IsClearColor(color))
            {
                color = theme.GetColor(index);
                ChartHelper.SetColorOpacity(ref color, serie.lineStyle.opacity);
            }
            if (highlight) color = ChartHelper.GetHighlightColor(color);
            return color;
        }

        internal static float GetSymbolBorder(Serie serie, SerieData serieData, ChartTheme theme, bool highlight, bool useLineWidth = true)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null && itemStyle.borderWidth != 0) return itemStyle.borderWidth;
            else return serie.lineStyle.GetWidth(theme.serie.lineWidth);
        }

        internal static float[] GetSymbolCornerRadius(Serie serie, SerieData serieData, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null) return itemStyle.cornerRadius;
            else return null;
        }

        /// <summary>
        /// 更新运行时中心点和半径
        /// </summary>
        /// <param name="chartWidth"></param>
        /// <param name="chartHeight"></param>
        internal static void UpdateCenter(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (serie.center.Length < 2) return;
            var centerX = serie.center[0] <= 1 ? chartWidth * serie.center[0] : serie.center[0];
            var centerY = serie.center[1] <= 1 ? chartHeight * serie.center[1] : serie.center[1];
            serie.runtimeCenterPos = chartPosition + new Vector3(centerX, centerY);
            var minWidth = Mathf.Min(chartWidth, chartHeight);
            serie.runtimeInsideRadius = serie.radius[0] <= 1 ? minWidth * serie.radius[0] : serie.radius[0];
            serie.runtimeOutsideRadius = serie.radius[1] <= 1 ? minWidth * serie.radius[1] : serie.radius[1];
        }

        internal static string GetNumericFormatter(Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.numericFormatter)) return itemStyle.numericFormatter;
            else return string.Empty;
        }

        /// <summary>
        /// 获得指定维数的最大最小值
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        internal static void UpdateMinMaxData(Serie serie, int dimension, int ceilRate = 0, DataZoom dataZoom = null)
        {
            float min = 0, max = 0;
            GetMinMaxData(serie, dimension, out min, out max, dataZoom);
            if (ceilRate < 0)
            {
                serie.runtimeDataMin = min;
                serie.runtimeDataMax = max;
            }
            else
            {
                serie.runtimeDataMin = ChartHelper.GetMinDivisibleValue(min, ceilRate);
                serie.runtimeDataMax = ChartHelper.GetMaxDivisibleValue(max, ceilRate);
            }
        }

        internal static void GetAllMinMaxData(Serie serie, int ceilRate = 0, DataZoom dataZoom = null)
        {
            float min = 0, max = 0;
            GetMinMaxData(serie, out min, out max, dataZoom);
            if (ceilRate < 0)
            {
                serie.runtimeDataMin = min;
                serie.runtimeDataMax = max;
            }
            else
            {
                serie.runtimeDataMin = ChartHelper.GetMinDivisibleValue(min, ceilRate);
                serie.runtimeDataMax = ChartHelper.GetMaxDivisibleValue(max, ceilRate);
            }
        }

        private static List<SerieData> emptyFilter = new List<SerieData>();
        /// <summary>
        /// 根据dataZoom更新数据列表缓存
        /// </summary>
        /// <param name="dataZoom"></param>
        internal static void UpdateFilterData(Serie serie, DataZoom dataZoom)
        {
            if (dataZoom == null || !dataZoom.enable) return;
            if (dataZoom.xAxisIndexs.Contains(serie.xAxisIndex))
            {
                if (dataZoom.IsXAxisIndexValue(serie.xAxisIndex))
                {
                    float min = 0, max = 0;
                    dataZoom.GetXAxisIndexValue(serie.xAxisIndex, out min, out max);
                    UpdateFilterData_XAxisValue(serie, dataZoom, 0, min, max);
                }
                else
                {
                    UpdateFilterData_Category(serie, dataZoom);
                }
            }
            else if (dataZoom.yAxisIndexs.Contains(serie.yAxisIndex))
            {
                if (dataZoom.IsYAxisIndexValue(serie.yAxisIndex))
                {
                    float min = 0, max = 0;
                    dataZoom.GetYAxisIndexValue(serie.yAxisIndex, out min, out max);
                    UpdateFilterData_XAxisValue(serie, dataZoom, 0, min, max);
                }
                else
                {
                    UpdateFilterData_Category(serie, dataZoom);
                }
            }
        }

        private static void UpdateFilterData_XAxisValue(Serie serie, DataZoom dataZoom, int dimension, float min, float max)
        {
            var data = serie.data;
            var startValue = min + (max - min) * dataZoom.start / 100;
            var endValue = min + (max - min) * dataZoom.end / 100;
            if (endValue < startValue) endValue = startValue;

            if (startValue != serie.m_FilterStartValue || endValue != serie.m_FilterEndValue
                || dataZoom.minShowNum != serie.m_FilterMinShow || serie.m_NeedUpdateFilterData)
            {
                serie.m_FilterStartValue = startValue;
                serie.m_FilterEndValue = endValue;
                serie.m_FilterMinShow = dataZoom.minShowNum;
                serie.m_NeedUpdateFilterData = false;

                serie.m_FilterData.Clear();
                foreach (var serieData in data)
                {
                    var value = serieData.GetData(dimension);
                    if (value >= startValue && value <= endValue)
                    {
                        serie.m_FilterData.Add(serieData);
                    }
                }
            }
            else if (endValue == 0)
            {
                serie.m_FilterData = emptyFilter;
            }
        }

        private static void UpdateFilterData_Category(Serie serie, DataZoom dataZoom)
        {
            var data = serie.data;
            var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
            var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
            if (endIndex < startIndex) endIndex = startIndex;

            if (startIndex != serie.m_FilterStart || endIndex != serie.m_FilterEnd
                || dataZoom.minShowNum != serie.m_FilterMinShow || serie.m_NeedUpdateFilterData)
            {
                serie.m_FilterStart = startIndex;
                serie.m_FilterEnd = endIndex;
                serie.m_FilterMinShow = dataZoom.minShowNum;
                serie.m_NeedUpdateFilterData = false;
                var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                if (count < dataZoom.minShowNum)
                {
                    if (dataZoom.minShowNum > data.Count) count = data.Count;
                    else count = dataZoom.minShowNum;
                }
                if (data.Count > 0)
                {
                    if (startIndex + count > data.Count)
                    {
                        int start = endIndex - count;
                        data = data.GetRange(start < 0 ? 0 : start, count);
                    }
                    else serie.m_FilterData = data.GetRange(startIndex, count);
                }
                else
                {
                    serie.m_FilterData = data;
                }
            }
            else if (endIndex == 0)
            {
                serie.m_FilterData = emptyFilter;
            }
        }
    }
}