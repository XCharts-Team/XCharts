using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace XCharts.Runtime
{
    public static partial class SerieHelper
    {
        public static double GetMinData(Serie serie, int dimension = 1, DataZoom dataZoom = null)
        {
            double min = double.MaxValue;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (value < min && !serie.IsIgnoreValue(value)) min = value;
                }
            }
            return min == double.MaxValue ? 0 : min;
        }
        public static SerieData GetMinSerieData(Serie serie, int dimension = 1, DataZoom dataZoom = null)
        {
            double min = double.MaxValue;
            SerieData minData = null;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (value < min && !serie.IsIgnoreValue(value))
                    {
                        min = value;
                        minData = serieData;
                    }
                }
            }
            return minData;
        }
        public static double GetMaxData(Serie serie, int dimension = 1, DataZoom dataZoom = null)
        {
            double max = double.MinValue;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (value > max && !serie.IsIgnoreValue(value)) max = value;
                }
            }
            return max == double.MinValue ? 0 : max;
        }
        public static SerieData GetMaxSerieData(Serie serie, int dimension = 1, DataZoom dataZoom = null)
        {
            double max = double.MinValue;
            SerieData maxData = null;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (value > max && !serie.IsIgnoreValue(value))
                    {
                        max = value;
                        maxData = serieData;
                    }
                }
            }
            return maxData;
        }

        public static double GetAverageData(Serie serie, int dimension = 1, DataZoom dataZoom = null)
        {
            double total = 0;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (!serie.IsIgnoreValue(value))
                        total += value;
                }
            }
            return total != 0 ? total / dataList.Count : 0;
        }

        private static List<double> s_TempList = new List<double>();
        public static double GetMedianData(Serie serie, int dimension = 1, DataZoom dataZoom = null)
        {
            s_TempList.Clear();
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (!serie.IsIgnoreValue(value))
                        s_TempList.Add(value);
                }
            }
            s_TempList.Sort();
            var n = s_TempList.Count;
            if (n % 2 == 0) return (s_TempList[n / 2] + s_TempList[n / 2 - 1]) / 2;
            else return s_TempList[n / 2];
        }

        /// <summary>
        /// Gets the maximum and minimum values of the specified dimension of a serie.
        /// |获得系列指定维数的最大最小值。
        /// </summary>
        /// <param name="serie">指定系列</param>
        /// <param name="dimension">指定维数</param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <param name="dataZoom">缩放组件，默认null</param>
        public static void GetMinMaxData(Serie serie, int dimension, out double min, out double max,
            DataZoom dataZoom = null)
        {
            max = double.MinValue;
            min = double.MaxValue;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show && serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (!serie.IsIgnoreValue(value))
                    {
                        if (value > max) max = value;
                        if (value < min) min = value;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the maximum and minimum values of all data in the serie.
        /// |获得系列所有数据的最大最小值。
        /// </summary>
        /// <param name="serie"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="dataZoom"></param>
        public static void GetMinMaxData(Serie serie, out double min, out double max, DataZoom dataZoom = null, int dimension = 0)
        {
            max = double.MinValue;
            min = double.MaxValue;
            var dataList = serie.GetDataList(dataZoom);
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.show)
                {
                    var count = 0;
                    if (dimension > 0) count = dimension;
                    else count = serie.showDataDimension > serieData.data.Count ?
                        serieData.data.Count :
                        serie.showDataDimension;
                    for (int j = 0; j < count; j++)
                    {
                        var value = serieData.data[j];
                        if (!serie.IsIgnoreValue(value))
                        {
                            if (value > max) max = value;
                            if (value < min) min = value;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Whether the data for the specified dimension of serie are all 0.
        /// |系列指定维数的数据是否全部为0。
        /// </summary>
        /// <param name="serie">系列</param>
        /// <param name="dimension">指定维数</param>
        /// <returns></returns>
        public static bool IsAllZeroValue(Serie serie, int dimension = 1)
        {
            foreach (var serieData in serie.data)
            {
                if (serieData.GetData(dimension) != 0) return false;
            }
            return true;
        }

        /// <summary>
        /// 更新运行时中心点和半径
        /// </summary>
        /// <param name="chartWidth"></param>
        /// <param name="chartHeight"></param>
        public static void UpdateCenter(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (serie.center.Length < 2) return;
            var centerX = serie.center[0] <= 1 ? chartWidth * serie.center[0] : serie.center[0];
            var centerY = serie.center[1] <= 1 ? chartHeight * serie.center[1] : serie.center[1];
            serie.context.center = chartPosition + new Vector3(centerX, centerY);
            var minWidth = Mathf.Min(chartWidth, chartHeight);
            serie.context.insideRadius = serie.radius[0] <= 1 ? minWidth * serie.radius[0] : serie.radius[0];
            serie.context.outsideRadius = serie.radius[1] <= 1 ? minWidth * serie.radius[1] : serie.radius[1];
        }

        public static void UpdateRect(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (serie.left != 0 || serie.right != 0 || serie.top != 0 || serie.bottom != 0)
            {
                var runtimeLeft = serie.left <= 1 ? serie.left * chartWidth : serie.left;
                var runtimeBottom = serie.bottom <= 1 ? serie.bottom * chartHeight : serie.bottom;
                var runtimeTop = serie.top <= 1 ? serie.top * chartHeight : serie.top;
                var runtimeRight = serie.right <= 1 ? serie.right * chartWidth : serie.right;

                serie.context.x = chartPosition.x + runtimeLeft;
                serie.context.y = chartPosition.y + runtimeBottom;
                serie.context.width = chartWidth - runtimeLeft - runtimeRight;
                serie.context.height = chartHeight - runtimeTop - runtimeBottom;
                serie.context.center = new Vector3(serie.context.x + serie.context.width / 2,
                    serie.context.y + serie.context.height / 2);
                serie.context.rect = new Rect(serie.context.x, serie.context.y, serie.context.width, serie.context.height);
            }
            else
            {
                serie.context.x = chartPosition.x;
                serie.context.y = chartPosition.y;
                serie.context.width = chartWidth;
                serie.context.height = chartHeight;
                serie.context.center = chartPosition + new Vector3(chartWidth / 2, chartHeight / 2);
                serie.context.rect = new Rect(serie.context.x, serie.context.y, serie.context.width, serie.context.height);
            }
        }

        public static Color32 GetItemBackgroundColor(Serie serie, SerieData serieData, ThemeStyle theme, int index,
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

        public static Color32 GetItemColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight, bool opacity = true)
        {
            if (serie == null)
                return ChartConst.clearColor32;

            ItemStyle itemStyle = null;
            if (highlight)
                itemStyle = GetItemStyleEmphasis(serie, serieData);
            if (itemStyle == null)
                itemStyle = GetItemStyle(serie, serieData);

            var color = ChartHelper.IsClearColor(itemStyle.color) ?
                theme.GetColor(index) :
                itemStyle.color;

            if (highlight)
                color = ChartHelper.GetHighlightColor(color);
            if (opacity)
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
            return color;
        }
        public static Color32 GetItemColor0(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight, Color32 defaultColor)
        {
            if (serie == null)
                return ChartConst.clearColor32;

            ItemStyle itemStyle = null;
            if (highlight)
                itemStyle = GetItemStyleEmphasis(serie, serieData);
            if (itemStyle == null)
                itemStyle = GetItemStyle(serie, serieData);

            var color = ChartHelper.IsClearColor(itemStyle.color0) ?
                defaultColor :
                itemStyle.color0;

            if (highlight)
                color = ChartHelper.GetHighlightColor(color);

            ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
            return color;
        }

        public static Color32 GetItemToColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight, bool opacity = true)
        {
            if (serie == null)
                return ChartConst.clearColor32;

            ItemStyle itemStyle = null;
            if (highlight)
                itemStyle = GetItemStyleEmphasis(serie, serieData);
            if (itemStyle == null)
                itemStyle = GetItemStyle(serie, serieData);

            var color = itemStyle.toColor;
            if (ChartHelper.IsClearColor(color))
            {
                color = ChartHelper.IsClearColor(itemStyle.color) ?
                    theme.GetColor(index) :
                    itemStyle.color;
            }

            if (highlight)
                color = ChartHelper.GetHighlightColor(color);

            if (opacity)
                ChartHelper.SetColorOpacity(ref color, itemStyle.opacity);
            return color;
        }

        public static bool IsDownPoint(Serie serie, int index)
        {
            var dataPoints = serie.context.dataPoints;
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

        public static ItemStyle GetItemStyle(Serie serie, SerieData serieData, bool highlight = false)
        {
            if (highlight)
            {
                var style = GetItemStyleEmphasis(serie, serieData);
                if (style == null) return GetItemStyle(serie, serieData, false);
                else return style;
            }
            else if (serie.IsPerformanceMode()) return serie.itemStyle;
            else if (serieData != null && serieData.itemStyle != null) return serieData.itemStyle;
            else return serie.itemStyle;
        }

        public static ItemStyle GetItemStyleEmphasis(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData != null && serieData.emphasisItemStyle != null && serieData.emphasisItemStyle.show)
                return serieData.emphasisItemStyle;
            else if (serie.emphasisItemStyle != null && serie.emphasisItemStyle.show) return serie.emphasisItemStyle;
            else return null;
        }

        public static LabelStyle GetSerieLabel(Serie serie, SerieData serieData, bool highlight = false)
        {
            if (serieData == null) return serie.label;
            if (highlight)
            {
                if (!serie.IsPerformanceMode() && serieData.emphasisLabel != null && serieData.emphasisLabel.show)
                    return serieData.emphasisLabel;
                else if (serie.emphasisLabel != null && serie.emphasisLabel.show) return serie.emphasisLabel;
                else return serie.label;
            }
            else
            {
                if (!serie.IsPerformanceMode() && serieData.labelStyle != null) return serieData.labelStyle;
                else return serie.label;
            }
        }

        public static LabelStyle GetSerieEmphasisLabel(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData.emphasisLabel != null && serieData.emphasisLabel.show)
                return serieData.emphasisLabel;
            else if (serie.emphasisLabel != null && serie.emphasisLabel.show) return serie.emphasisLabel;
            else return null;
        }

        public static LabelLine GetSerieLabelLine(Serie serie, SerieData serieData, bool highlight = false)
        {
            if (highlight)
            {
                if (!serie.IsPerformanceMode() && serieData.emphasisLabelLine != null && serieData.emphasisLabelLine.show)
                    return serieData.emphasisLabelLine;
                else if (serie.emphasisLabelLine != null && serie.emphasisLabelLine.show) return serie.emphasisLabelLine;
                else return serie.labelLine;
            }
            else
            {
                if (!serie.IsPerformanceMode() && serieData.labelLine != null) return serieData.labelLine;
                else return serie.labelLine;
            }
        }

        public static SerieSymbol GetSerieSymbol(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData.symbol != null) return serieData.symbol;
            else return serie.symbol;
        }

        public static LineStyle GetLineStyle(Serie serie, SerieData serieData)
        {
            if (serieData != null && serieData.lineStyle != null) return serieData.lineStyle;
            else return serie.lineStyle;
        }

        public static AreaStyle GetAreaStyle(Serie serie, SerieData serieData)
        {
            if (serieData != null && serieData.areaStyle != null) return serieData.areaStyle;
            else return serie.areaStyle;
        }

        public static TitleStyle GetTitleStyle(Serie serie, SerieData serieData)
        {
            if (serieData != null && serieData.titleStyle != null) return serieData.titleStyle;
            else return serie.titleStyle;
        }

        public static Color32 GetAreaColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight)
        {
            Color32 color = ChartConst.clearColor32;
            var areaStyle = GetAreaStyle(serie, serieData);
            if (areaStyle == null || !areaStyle.show)
                return color;
            if (!ChartHelper.IsClearColor(areaStyle.color)) color = areaStyle.color;
            else if (!ChartHelper.IsClearColor(serie.itemStyle.color)) color = serie.itemStyle.color;
            else color = theme.GetColor(index);
            ChartHelper.SetColorOpacity(ref color, areaStyle.opacity);
            if (highlight)
            {
                if (!ChartHelper.IsClearColor(areaStyle.highlightColor))
                    color = areaStyle.highlightColor;
                else
                    color = ChartHelper.GetHighlightColor(color);
            }
            return color;
        }

        public static Color32 GetAreaToColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight)
        {
            Color32 color = ChartConst.clearColor32;
            var areaStyle = GetAreaStyle(serie, serieData);
            if (areaStyle == null || !areaStyle.show)
                return color;
            if (!ChartHelper.IsClearColor(areaStyle.toColor)) color = areaStyle.toColor;
            else if (!ChartHelper.IsClearColor(serie.itemStyle.toColor)) color = serie.itemStyle.toColor;
            else color = theme.GetColor(index);
            ChartHelper.SetColorOpacity(ref color, areaStyle.opacity);
            if (highlight)
            {
                if (!ChartHelper.IsClearColor(areaStyle.highlightToColor))
                    color = areaStyle.highlightToColor;
                else
                    color = ChartHelper.GetHighlightColor(color);
            }
            return color;
        }

        public static Color32 GetLineColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, bool highlight)
        {
            Color32 color = ChartConst.clearColor32;
            var lineStyle = GetLineStyle(serie, serieData);
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
            if (!ChartHelper.IsClearColor(lineStyle.color)) color = lineStyle.color;
            else if (!ChartHelper.IsClearColor(serie.itemStyle.color)) color = serie.itemStyle.GetColor();
            if (ChartHelper.IsClearColor(color)) color = theme.GetColor(index);
            ChartHelper.SetColorOpacity(ref color, lineStyle.opacity);
            if (highlight) color = ChartHelper.GetHighlightColor(color);
            return color;
        }

        public static float GetSymbolBorder(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null && itemStyle.borderWidth != 0) return itemStyle.borderWidth;
            else return serie.lineStyle.GetWidth(theme.serie.lineWidth) * 2;
        }

        public static Color32 GetSymbolBorderColor(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null && !ChartHelper.IsClearColor(itemStyle.borderColor)) return itemStyle.borderColor;
            else return serie.itemStyle.borderColor;
        }

        public static float GetSymbolBorder(Serie serie, SerieData serieData, ThemeStyle theme, bool highlight, float defaultWidth)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null && itemStyle.borderWidth != 0) return itemStyle.borderWidth;
            else return defaultWidth;
        }

        public static float[] GetSymbolCornerRadius(Serie serie, SerieData serieData, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null) return itemStyle.cornerRadius;
            else return null;
        }

        public static string GetNumericFormatter(Serie serie, SerieData serieData, string defaultFormatter = null)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.numericFormatter)) return itemStyle.numericFormatter;
            else return defaultFormatter;
        }

        public static string GetItemFormatter(Serie serie, SerieData serieData, string defaultFormatter = null)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.itemFormatter)) return itemStyle.itemFormatter;
            else return defaultFormatter;
        }

        public static string GetItemMarker(Serie serie, SerieData serieData, string defaultMarker = null)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.itemMarker)) return itemStyle.itemMarker;
            else return defaultMarker;
        }

        /// <summary>
        /// 获得指定维数的最大最小值
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public static void UpdateMinMaxData(Serie serie, int dimension, double ceilRate = 0, DataZoom dataZoom = null)
        {
            double min = 0, max = 0;
            GetMinMaxData(serie, dimension, out min, out max, dataZoom);
            if (ceilRate < 0)
            {
                serie.context.dataMin = min;
                serie.context.dataMax = max;
            }
            else
            {
                serie.context.dataMin = ChartHelper.GetMinDivisibleValue(min, ceilRate);
                serie.context.dataMax = ChartHelper.GetMaxDivisibleValue(max, ceilRate);
            }
        }

        public static void GetAllMinMaxData(Serie serie, double ceilRate = 0, DataZoom dataZoom = null)
        {
            double min = 0, max = 0;
            GetMinMaxData(serie, out min, out max, dataZoom);
            if (ceilRate < 0)
            {
                serie.context.dataMin = min;
                serie.context.dataMax = max;
            }
            else
            {
                serie.context.dataMin = ChartHelper.GetMinDivisibleValue(min, ceilRate);
                serie.context.dataMax = ChartHelper.GetMaxDivisibleValue(max, ceilRate);
            }
        }

        private static List<SerieData> emptyFilter = new List<SerieData>();
        /// <summary>
        /// 根据dataZoom更新数据列表缓存
        /// </summary>
        /// <param name="dataZoom"></param>
        public static void UpdateFilterData(Serie serie, DataZoom dataZoom)
        {
            if (dataZoom == null || !dataZoom.enable) return;
            if (dataZoom.IsContainsXAxis(serie.xAxisIndex))
            {
                if (dataZoom.IsXAxisIndexValue(serie.xAxisIndex))
                {
                    double min = 0, max = 0;
                    dataZoom.GetXAxisIndexValue(serie.xAxisIndex, out min, out max);
                    UpdateFilterData_XAxisValue(serie, dataZoom, 0, min, max);
                }
                else
                {
                    UpdateFilterData_Category(serie, dataZoom);
                }
            }
            else if (dataZoom.IsContainsYAxis(serie.yAxisIndex))
            {
                if (dataZoom.IsYAxisIndexValue(serie.yAxisIndex))
                {
                    double min = 0, max = 0;
                    dataZoom.GetYAxisIndexValue(serie.yAxisIndex, out min, out max);
                    UpdateFilterData_XAxisValue(serie, dataZoom, 0, min, max);
                }
                else
                {
                    UpdateFilterData_Category(serie, dataZoom);
                }
            }
        }

        private static void UpdateFilterData_XAxisValue(Serie serie, DataZoom dataZoom, int dimension, double min, double max)
        {
            var data = serie.data;
            var startValue = min + (max - min) * dataZoom.start / 100;
            var endValue = min + (max - min) * dataZoom.end / 100;
            if (endValue < startValue) endValue = startValue;

            if (startValue != serie.m_FilterStartValue || endValue != serie.m_FilterEndValue ||
                dataZoom.minShowNum != serie.m_FilterMinShow || serie.m_NeedUpdateFilterData)
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
            var range = Mathf.RoundToInt(data.Count * (dataZoom.end - dataZoom.start) / 100);
            if (range <= 0) range = 1;
            int start = 0, end = 0;
            if (dataZoom.context.invert)
            {
                end = Mathf.CeilToInt(data.Count * dataZoom.end / 100);
                start = end - range;
                if (start < 0) start = 0;
            }
            else
            {
                start = Mathf.FloorToInt(data.Count * dataZoom.start / 100);
                end = start + range;
                if (end > data.Count) end = data.Count;
            }
            if (start != serie.m_FilterStart || end != serie.m_FilterEnd ||
                dataZoom.minShowNum != serie.m_FilterMinShow || serie.m_NeedUpdateFilterData)
            {
                serie.m_FilterStart = start;
                serie.m_FilterEnd = end;
                serie.m_FilterMinShow = dataZoom.minShowNum;
                serie.m_NeedUpdateFilterData = false;
                if (data.Count > 0)
                {
                    if (range < dataZoom.minShowNum)
                    {
                        if (dataZoom.minShowNum > data.Count) range = data.Count;
                        else range = dataZoom.minShowNum;
                    }
                    if (range > data.Count - start - 1)
                        start = data.Count - range - 1;
                    if (start >= 0)
                    {
                        serie.context.dataZoomStartIndex = start;
                        serie.m_FilterData = data.GetRange(start, range);
                    }
                    else
                    {
                        serie.context.dataZoomStartIndex = 0;
                        serie.m_FilterData = data;
                    }
                }
                else
                {
                    serie.context.dataZoomStartIndex = 0;
                    serie.m_FilterData = data;
                }
            }
            else if (end == 0)
            {
                serie.context.dataZoomStartIndex = 0;
                serie.m_FilterData = emptyFilter;
            }
        }

        public static void UpdateSerieRuntimeFilterData(Serie serie, bool filterInvisible = true)
        {
            serie.context.sortedData.Clear();
            foreach (var serieData in serie.data)
            {
                if (!filterInvisible || (filterInvisible && serieData.show))
                    serie.context.sortedData.Add(serieData);
            }
            switch (serie.dataSortType)
            {
                case SerieDataSortType.Ascending:
                    serie.context.sortedData.Sort(delegate(SerieData data1, SerieData data2)
                    {
                        var value1 = data1.GetData(1);
                        var value2 = data2.GetData(1);
                        if (value1 == value2) return 0;
                        else if (value1 > value2) return 1;
                        else return -1;
                    });
                    break;
                case SerieDataSortType.Descending:
                    serie.context.sortedData.Sort(delegate(SerieData data1, SerieData data2)
                    {
                        var value1 = data1.GetData(1);
                        var value2 = data2.GetData(1);
                        if (value1 == value2) return 0;
                        else if (value1 > value2) return -1;
                        else return 1;
                    });
                    break;
                case SerieDataSortType.None:
                    break;
            }
        }

        public static T CloneSerie<T>(Serie serie) where T : Serie
        {
            var newSerie = Activator.CreateInstance<T>();
            SerieHelper.CopySerie(serie, newSerie);
            return newSerie;
        }

        public static void CopySerie(Serie oldSerie, Serie newSerie)
        {
            var fields = typeof(Serie).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.IsDefined(typeof(SerializeField), false))
                {
                    var filedValue = field.GetValue(oldSerie);
                    if (filedValue == null) continue;
                    var filedType = filedValue.GetType();
                    if (filedType.IsClass)
                        field.SetValue(newSerie, ReflectionUtil.DeepCloneSerializeField(filedValue));
                }
            }
        }
    }
}