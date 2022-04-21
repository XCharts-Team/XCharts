/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace XCharts
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
        /// 获得系列指定维数的最大最小值。
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
        /// 获得系列所有数据的最大最小值。
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
                    else count = serie.showDataDimension > serieData.data.Count
                        ? serieData.data.Count
                        : serie.showDataDimension;
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
        /// 系列指定维数的数据是否全部为0。
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
            serie.runtimeCenterPos = chartPosition + new Vector3(centerX, centerY);
            var minWidth = Mathf.Min(chartWidth, chartHeight);
            serie.runtimeInsideRadius = serie.radius[0] <= 1 ? minWidth * serie.radius[0] : serie.radius[0];
            serie.runtimeOutsideRadius = serie.radius[1] <= 1 ? minWidth * serie.radius[1] : serie.radius[1];
        }

        public static void UpdateRect(Serie serie, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (serie.left != 0 || serie.right != 0 || serie.top != 0 || serie.bottom != 0)
            {
                var runtimeLeft = serie.left <= 1 ? serie.left * chartWidth : serie.left;
                var runtimeBottom = serie.bottom <= 1 ? serie.bottom * chartHeight : serie.bottom;
                var runtimeTop = serie.top <= 1 ? serie.top * chartHeight : serie.top;
                var runtimeRight = serie.right <= 1 ? serie.right * chartWidth : serie.right;

                serie.runtimeX = chartPosition.x + runtimeLeft;
                serie.runtimeY = chartPosition.y + runtimeBottom;
                serie.runtimeWidth = chartWidth - runtimeLeft - runtimeRight;
                serie.runtimeHeight = chartHeight - runtimeTop - runtimeBottom;
                serie.runtimeCenterPos = new Vector3(serie.runtimeX + serie.runtimeWidth / 2,
                    serie.runtimeY + serie.runtimeHeight / 2);
                serie.runtimeRect = new Rect(serie.runtimeX, serie.runtimeY, serie.runtimeWidth, serie.runtimeHeight);
            }
            else
            {
                serie.runtimeX = chartPosition.x;
                serie.runtimeY = chartPosition.y;
                serie.runtimeWidth = chartWidth;
                serie.runtimeHeight = chartHeight;
                serie.runtimeCenterPos = chartPosition + new Vector3(chartWidth / 2, chartHeight / 2);
                serie.runtimeRect = new Rect(serie.runtimeX, serie.runtimeY, serie.runtimeWidth, serie.runtimeHeight);
            }
        }

        public static Color32 GetItemBackgroundColor(Serie serie, SerieData serieData, ChartTheme theme, int index,
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

        public static Color32 GetItemColor(Serie serie, SerieData serieData, ChartTheme theme, int index, bool highlight)
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
        public static Color32 GetItemColor0(Serie serie, SerieData serieData, ChartTheme theme, bool highlight, Color32 defaultColor)
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

        public static Color32 GetItemToColor(Serie serie, SerieData serieData, ChartTheme theme, int index, bool highlight)
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

        public static bool IsDownPoint(Serie serie, int index)
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

        public static ItemStyle GetItemStyle(Serie serie, SerieData serieData, bool highlight = false)
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

        public static ItemStyle GetItemStyleEmphasis(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData != null && serieData.enableEmphasis && serieData.emphasis.show)
                return serieData.emphasis.itemStyle;
            else if (serie.emphasis.show) return serie.emphasis.itemStyle;
            else return null;
        }

        public static SerieLabel GetSerieLabel(Serie serie, SerieData serieData, bool highlight = false)
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

        public static SerieLabel GetSerieEmphasisLabel(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData.enableEmphasis && serieData.emphasis.show)
                return serieData.emphasis.label;
            else if (serie.emphasis.show) return serie.emphasis.label;
            else return null;
        }

        public static IconStyle GetIconStyle(Serie serie, SerieData serieData)
        {
            if (serieData.enableIconStyle) return serieData.iconStyle;
            else return serie.iconStyle;
        }

        public static SerieSymbol GetSerieSymbol(Serie serie, SerieData serieData)
        {
            if (!serie.IsPerformanceMode() && serieData.enableSymbol) return serieData.symbol;
            else return serie.symbol;
        }

        public static Color32 GetAreaColor(Serie serie, ChartTheme theme, int index, bool highlight)
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

        public static Color32 GetAreaToColor(Serie serie, ChartTheme theme, int index, bool highlight)
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

        public static Color32 GetLineColor(Serie serie, ChartTheme theme, int index, bool highlight)
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

        public static float GetSymbolBorder(Serie serie, SerieData serieData, ChartTheme theme, bool highlight, bool useLineWidth = true)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null && itemStyle.borderWidth != 0) return itemStyle.borderWidth;
            else return serie.lineStyle.GetWidth(theme.serie.lineWidth);
        }

        public static float[] GetSymbolCornerRadius(Serie serie, SerieData serieData, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null) return itemStyle.cornerRadius;
            else return null;
        }

        public static string GetNumericFormatter(Serie serie, SerieData serieData)
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
        public static void UpdateMinMaxData(Serie serie, int dimension, int ceilRate = 0, DataZoom dataZoom = null)
        {
            double min = 0, max = 0;
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

        public static void GetAllMinMaxData(Serie serie, int ceilRate = 0, DataZoom dataZoom = null)
        {
            double min = 0, max = 0;
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
            var range = Mathf.RoundToInt(data.Count * (dataZoom.end - dataZoom.start) / 100);
            if (range <= 0) range = 1;
            int start = 0, end = 0;
            if (dataZoom.runtimeInvert)
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
            if (start != serie.m_FilterStart || end != serie.m_FilterEnd
                || dataZoom.minShowNum != serie.m_FilterMinShow || serie.m_NeedUpdateFilterData)
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
                    serie.m_FilterData = data.GetRange(start, range);
                }
                else
                {
                    serie.m_FilterData = data;
                }
            }
            else if (end == 0)
            {
                serie.m_FilterData = emptyFilter;
            }
        }

        public static void UpdateSerieRuntimeFilterData(Serie serie, bool filterInvisible = true)
        {
            serie.runtimeSortedData.Clear();
            foreach (var serieData in serie.data)
            {
                if (!filterInvisible || (filterInvisible && serieData.show))
                    serie.runtimeSortedData.Add(serieData);
            }
            switch (serie.dataSortType)
            {
                case SerieDataSortType.Ascending:
                    serie.runtimeSortedData.Sort(delegate (SerieData data1, SerieData data2)
                    {
                        var value1 = data1.GetData(1);
                        var value2 = data2.GetData(1);
                        if (value1 == value2) return 0;
                        else if (value1 > value2) return 1;
                        else return -1;
                    });
                    break;
                case SerieDataSortType.Descending:
                    serie.runtimeSortedData.Sort(delegate (SerieData data1, SerieData data2)
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
    }
}