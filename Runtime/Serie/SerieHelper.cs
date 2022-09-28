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
            if (min == double.MaxValue && max == double.MinValue)
            {
                min = 0;
                max = 0;
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
            if (min == double.MaxValue && max == double.MinValue)
            {
                min = 0;
                max = 0;
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

        public static SerieState GetSerieState(Serie serie)
        {
            if (serie.highlight) return SerieState.Emphasis;
            return serie.state;
        }

        public static SerieState GetSerieState(SerieData serieData)
        {
            if (serieData.context.highlight) return SerieState.Emphasis;
            return serieData.state;
        }

        public static SerieState GetSerieState(Serie serie, SerieData serieData, bool defaultSerieState = false)
        {
            if (serieData == null) return GetSerieState(serie);
            if (serieData.context.highlight) return SerieState.Emphasis;
            if (serieData.state == SerieState.Auto) return defaultSerieState?serie.state : GetSerieState(serie);
            return serieData.state;
        }

        public static Color32 GetItemBackgroundColor(Serie serie, SerieData serieData, ThemeStyle theme, int index,
            SerieState state = SerieState.Auto, bool useDefault = false)
        {
            var color = ChartConst.clearColor32;
            var stateStyle = GetStateStyle(serie, serieData, state);
            if (stateStyle == null)
                color = GetItemStyle(serie, serieData, SerieState.Normal).backgroundColor;
            else
                color = stateStyle.itemStyle.backgroundColor;
            if (useDefault && ChartHelper.IsClearColor(color))
            {
                color = theme.GetColor(index);
                color.a = 50;
            }
            return color;
        }

        public static void GetItemColor(out Color32 color, out Color32 toColor,
            Serie serie, SerieData serieData, ThemeStyle theme, SerieState state = SerieState.Auto)
        {
            var colorIndex = serieData != null && serie.colorByData? serieData.index : serie.context.colorIndex;
            GetItemColor(out color, out toColor, serie, serieData, theme, colorIndex, state, true);
        }

        public static void GetItemColor(out Color32 color, out Color32 toColor,
            Serie serie, SerieData serieData, ThemeStyle theme, int index, SerieState state = SerieState.Auto, bool opacity = true)
        {
            color = ColorUtil.clearColor32;
            toColor = ColorUtil.clearColor32;
            if (serie == null) return;
            if (state == SerieState.Auto) state = GetSerieState(serie, serieData);
            var stateStyle = GetStateStyle(serie, serieData, state);
            if (stateStyle == null)
            {
                var style = GetItemStyle(serie, serieData, SerieState.Normal);
                GetColor(ref color, style.color, style.color, style.opacity, theme, index, opacity);
                GetColor(ref toColor, style.toColor, color, style.opacity, theme, index, opacity);
                switch (state)
                {
                    case SerieState.Emphasis:
                        color = ChartHelper.GetHighlightColor(color);
                        toColor = ChartHelper.GetHighlightColor(toColor);
                        break;
                    case SerieState.Blur:
                        color = ChartHelper.GetBlurColor(color);
                        toColor = ChartHelper.GetBlurColor(toColor);
                        break;
                    case SerieState.Select:
                        color = ChartHelper.GetSelectColor(color);
                        toColor = ChartHelper.GetSelectColor(toColor);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                GetColor(ref color, stateStyle.itemStyle.color, stateStyle.itemStyle.color, stateStyle.itemStyle.opacity, theme, index, opacity);
                GetColor(ref toColor, stateStyle.itemStyle.toColor, color, stateStyle.itemStyle.opacity, theme, index, opacity);
            }
        }

        public static void GetItemColor(out Color32 color, out Color32 toColor, out Color32 backgroundColor,
            Serie serie, SerieData serieData, ThemeStyle theme, int index, SerieState state = SerieState.Auto, bool opacity = true)
        {
            color = ColorUtil.clearColor32;
            toColor = ColorUtil.clearColor32;
            backgroundColor = ColorUtil.clearColor32;
            if (serie == null) return;
            if (state == SerieState.Auto) state = GetSerieState(serie, serieData);
            var stateStyle = GetStateStyle(serie, serieData, state);
            if (stateStyle == null)
            {
                var style = GetItemStyle(serie, serieData, SerieState.Normal);
                GetColor(ref color, style.color, style.color, style.opacity, theme, index, opacity);
                GetColor(ref toColor, style.toColor, color, style.opacity, theme, index, opacity);
                backgroundColor = style.backgroundColor;
                switch (state)
                {
                    case SerieState.Emphasis:
                        color = ChartHelper.GetHighlightColor(color);
                        toColor = ChartHelper.GetHighlightColor(toColor);
                        break;
                    case SerieState.Blur:
                        color = ChartHelper.GetBlurColor(color);
                        toColor = ChartHelper.GetBlurColor(toColor);
                        break;
                    case SerieState.Select:
                        color = ChartHelper.GetSelectColor(color);
                        toColor = ChartHelper.GetSelectColor(toColor);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                backgroundColor = stateStyle.itemStyle.backgroundColor;
                GetColor(ref color, stateStyle.itemStyle.color, stateStyle.itemStyle.color, stateStyle.itemStyle.opacity, theme, index, opacity);
                GetColor(ref toColor, stateStyle.itemStyle.toColor, color, stateStyle.itemStyle.opacity, theme, index, opacity);
            }
        }

        public static Color32 GetItemColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, SerieState state = SerieState.Auto, bool opacity = true)
        {
            var color = ColorUtil.clearColor32;
            if (serie == null) return color;
            if (state == SerieState.Auto) state = GetSerieState(serie, serieData);
            var stateStyle = GetStateStyle(serie, serieData, state);
            if (stateStyle == null || !stateStyle.itemStyle.show)
            {
                var style = GetItemStyle(serie, serieData);
                GetColor(ref color, style.color, style.color, style.opacity, theme, index, opacity);
                switch (state)
                {
                    case SerieState.Emphasis:
                        color = ChartHelper.GetHighlightColor(color);
                        break;
                    case SerieState.Blur:
                        color = ChartHelper.GetBlurColor(color);
                        break;
                    case SerieState.Select:
                        color = ChartHelper.GetSelectColor(color);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                GetColor(ref color, stateStyle.itemStyle.color, stateStyle.itemStyle.color, stateStyle.itemStyle.opacity, theme, index, opacity);
            }
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

        public static ItemStyle GetItemStyle(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)
        {
            if (state == SerieState.Auto) state = GetSerieState(serie, serieData);
            if (state == SerieState.Normal)
            {
                return serieData != null && serieData.itemStyle != null? serieData.itemStyle : serie.itemStyle;
            }
            else
            {
                var stateStyle = GetStateStyle(serie, serieData, state);
                return stateStyle == null || !stateStyle.show ? serie.itemStyle : stateStyle.itemStyle;
            }
        }

        public static LabelStyle GetSerieLabel(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)
        {
            if (state == SerieState.Auto) state = GetSerieState(serie, serieData);
            if (state == SerieState.Normal)
            {
                return serieData != null && serieData.labelStyle != null? serieData.labelStyle : serie.label;
            }
            else
            {
                var stateStyle = GetStateStyle(serie, serieData, state);
                return stateStyle == null || !stateStyle.show ? serie.label : stateStyle.label;
            }
        }

        public static LabelLine GetSerieLabelLine(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)
        {
            if (state == SerieState.Auto) state = GetSerieState(serie, serieData);
            if (state == SerieState.Normal)
            {
                return serieData != null && serieData.labelLine != null? serieData.labelLine : serie.labelLine;
            }
            else
            {
                var stateStyle = GetStateStyle(serie, serieData, state);
                return stateStyle == null || !stateStyle.show ? serie.labelLine : stateStyle.labelLine;
            }
        }

        public static SerieSymbol GetSerieSymbol(Serie serie, SerieData serieData, SerieState state = SerieState.Auto)
        {
            if (state == SerieState.Auto) state = GetSerieState(serie, serieData);
            if (state == SerieState.Normal)
            {
                return serieData != null && serieData.symbol != null? serieData.symbol : serie.symbol;
            }
            else
            {
                var stateStyle = GetStateStyle(serie, serieData, state);
                return stateStyle == null || !stateStyle.show ? serie.symbol : stateStyle.symbol;
            }
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

        public static EmphasisStyle GetEmphasisStyle(Serie serie, SerieData serieData)
        {
            if (serieData != null && serieData.emphasisStyle != null) return serieData.emphasisStyle;
            else return serie.emphasisStyle;
        }

        public static BlurStyle GetBlurStyle(Serie serie, SerieData serieData)
        {
            if (serieData != null && serieData.blurStyle != null) return serieData.blurStyle;
            else return serie.blurStyle;
        }
        public static SelectStyle GetSelectStyle(Serie serie, SerieData serieData)
        {
            if (serieData != null && serieData.selectStyle != null) return serieData.selectStyle;
            else return serie.selectStyle;
        }

        public static StateStyle GetStateStyle(Serie serie, SerieData serieData, SerieState state)
        {
            switch (state)
            {
                case SerieState.Emphasis:
                    return GetEmphasisStyle(serie, serieData);
                case SerieState.Blur:
                    return GetBlurStyle(serie, serieData);
                case SerieState.Select:
                    return GetSelectStyle(serie, serieData);
                default:
                    return null;
            }
        }

        public static bool GetAreaColor(out Color32 color, out Color32 toColor,
            Serie serie, SerieData serieData, ThemeStyle theme, int index)
        {
            bool fill;
            return GetAreaColor(out color, out toColor, out fill, serie, serieData, theme, index);
        }

        public static bool GetAreaColor(out Color32 color, out Color32 toColor, out bool innerFill,
            Serie serie, SerieData serieData, ThemeStyle theme, int index)
        {
            color = ChartConst.clearColor32;
            toColor = ChartConst.clearColor32;
            innerFill = false;
            var state = GetSerieState(serie, serieData);
            var stateStyle = GetStateStyle(serie, serieData, state);
            if (stateStyle == null)
            {
                var areaStyle = GetAreaStyle(serie, serieData);
                if (areaStyle == null || !areaStyle.show) return false;
                innerFill = areaStyle.innerFill;
                GetColor(ref color, areaStyle.color, serie.itemStyle.color, areaStyle.opacity, theme, index);
                GetColor(ref toColor, areaStyle.toColor, color, areaStyle.opacity, theme, index);
                switch (state)
                {
                    case SerieState.Emphasis:
                        color = ChartHelper.GetHighlightColor(color);
                        toColor = ChartHelper.GetHighlightColor(toColor);
                        break;
                    case SerieState.Blur:
                        color = ChartHelper.GetBlurColor(color);
                        toColor = ChartHelper.GetBlurColor(toColor);
                        break;
                    case SerieState.Select:
                        color = ChartHelper.GetSelectColor(color);
                        toColor = ChartHelper.GetSelectColor(toColor);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (stateStyle.areaStyle.show)
                {
                    innerFill = stateStyle.areaStyle.innerFill;
                    GetColor(ref color, stateStyle.areaStyle.color, stateStyle.itemStyle.color, stateStyle.areaStyle.opacity, theme, index);
                    GetColor(ref color, stateStyle.areaStyle.toColor, color, stateStyle.areaStyle.opacity, theme, index);
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        public static Color32 GetLineColor(Serie serie, SerieData serieData, ThemeStyle theme, int index, SerieState state = SerieState.Auto)
        {
            Color32 color = ChartConst.clearColor32;
            if (state == SerieState.Auto)
                state = GetSerieState(serie, serieData);
            var stateStyle = GetStateStyle(serie, serieData, state);
            if (stateStyle == null)
            {
                var lineStyle = GetLineStyle(serie, serieData);
                GetColor(ref color, lineStyle.color, serie.itemStyle.color, lineStyle.opacity, theme, index);
                switch (state)
                {
                    case SerieState.Emphasis:
                        return ChartHelper.GetHighlightColor(color);
                    case SerieState.Blur:
                        return ChartHelper.GetBlurColor(color);
                    case SerieState.Select:
                        return ChartHelper.GetSelectColor(color);
                    default:
                        return color;
                }
            }
            else
            {
                GetColor(ref color, stateStyle.lineStyle.color, stateStyle.itemStyle.color, stateStyle.lineStyle.opacity, theme, index);
                return color;
            }
        }

        private static void GetColor(ref Color32 color, Color32 checkColor, Color32 itemColor,
            float opacity, ThemeStyle theme, int colorIndex, bool setOpacity = true)
        {
            if (!ChartHelper.IsClearColor(checkColor)) color = checkColor;
            else if (!ChartHelper.IsClearColor(itemColor)) color = itemColor;
            if (ChartHelper.IsClearColor(color) && colorIndex >= 0) color = theme.GetColor(colorIndex);
            if (setOpacity) ChartHelper.SetColorOpacity(ref color, opacity);
        }

        public static void GetSymbolInfo(out Color32 borderColor, out float border, out float[] cornerRadius,
            Serie serie, SerieData serieData, ThemeStyle theme, SerieState state = SerieState.Auto)
        {
            borderColor = ChartConst.clearColor32;
            if (state == SerieState.Auto)
                state = GetSerieState(serie, serieData);
            var stateStyle = GetStateStyle(serie, serieData, state);
            if (stateStyle == null)
            {
                var itemStyle = GetItemStyle(serie, serieData, SerieState.Normal);
                border = itemStyle.borderWidth != 0 ? itemStyle.borderWidth : serie.lineStyle.GetWidth(theme.serie.lineWidth);
                cornerRadius = itemStyle.cornerRadius;
                GetColor(ref borderColor, itemStyle.borderColor, itemStyle.borderColor, 1, theme, -1);
                switch (state)
                {
                    case SerieState.Emphasis:
                        borderColor = ChartHelper.GetHighlightColor(borderColor);
                        break;
                    case SerieState.Blur:
                        borderColor = ChartHelper.GetBlurColor(borderColor);
                        break;
                    case SerieState.Select:
                        borderColor = ChartHelper.GetSelectColor(borderColor);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var itemStyle = stateStyle.itemStyle;
                border = itemStyle.borderWidth != 0 ? itemStyle.borderWidth : stateStyle.lineStyle.GetWidth(theme.serie.lineWidth);
                cornerRadius = itemStyle.cornerRadius;
                GetColor(ref borderColor, stateStyle.itemStyle.borderColor, ColorUtil.clearColor32, 1, theme, -1);
            }
        }

        public static float GetSysmbolSize(Serie serie, SerieData serieData, ThemeStyle theme, float defaultSize, SerieState state = SerieState.Auto)
        {
            if (state == SerieState.Auto)
                state = GetSerieState(serie, serieData);
            var stateStyle = GetStateStyle(serie, serieData, state);
            var size = 0f;
            if (stateStyle == null)
            {
                var symbol = GetSerieSymbol(serie, serieData, SerieState.Normal);
                size = symbol.GetSize(serieData == null? null : serieData.data, defaultSize);
                switch (state)
                {
                    case SerieState.Emphasis:
                    case SerieState.Select:
                        size *= theme.serie.selectedRate;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                var symbol = stateStyle.symbol;
                size = symbol.GetSize(serieData == null? null : serieData.data, defaultSize);
            }
            return size;
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
            var startValue = min;
            var endValue = max;
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
                end = Mathf.RoundToInt(data.Count * dataZoom.end / 100);
                start = end - range;
                if (start < 0) start = 0;
            }
            else
            {
                start = Mathf.RoundToInt(data.Count * dataZoom.start / 100);
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
                    if (range > data.Count - start)
                        start = data.Count - range;
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