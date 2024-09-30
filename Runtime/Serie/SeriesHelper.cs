using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class SeriesHelper
    {

        public static bool IsLegalLegendName(string name)
        {
            int numName = -1;
            if (int.TryParse(name, out numName))
            {
                if (numName >= 0 && numName < 100) return false;
            }
            return true;
        }

        public static List<string> GetLegalSerieNameList(List<Serie> series)
        {
            var list = new List<string>();
            for (int n = 0; n < series.Count; n++)
            {
                var serie = series[n];
                if (serie.placeHolder) continue;
                if (serie.colorByData)
                {
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        var dataName = serie.data[i].name;
                        if (!string.IsNullOrEmpty(dataName) && IsLegalLegendName(dataName) && !list.Contains(dataName))
                            list.Add(dataName);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(serie.serieName) && !list.Contains(serie.serieName) && IsLegalLegendName(serie.serieName))
                        list.Add(serie.serieName);
                }
            }
            return list;
        }

        /// <summary>
        /// 获得所有系列名，不包含空名字。
        /// </summary>
        /// <returns></returns>
        public static void UpdateSerieNameList(BaseChart chart, ref List<string> serieNameList)
        {
            serieNameList.Clear();
            for (int n = 0; n < chart.series.Count; n++)
            {
                var serie = chart.series[n];
                if (serie.placeHolder) continue;
                if (serie.colorByData)
                {
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        var serieData = serie.data[i];
                        if (serie is Pie && serie.IsIgnoreValue(serieData)) continue;
                        if (string.IsNullOrEmpty(serieData.name))
                            serieNameList.Add(ChartCached.IntToStr(i));
                        else if (!serieNameList.Contains(serieData.name))
                            serieNameList.Add(serieData.name);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(serie.serieName))
                        serieNameList.Add(ChartCached.IntToStr(n));
                    else if (!serieNameList.Contains(serie.serieName))
                        serieNameList.Add(serie.serieName);
                }
            }
        }

        public static Color GetNameColor(BaseChart chart, int index, string name)
        {
            Serie destSerie = null;
            SerieData destSerieData = null;
            var series = chart.series;
            for (int n = 0; n < series.Count; n++)
            {
                var serie = series[n];
                if (serie.placeHolder) continue;
                if (serie.colorByData)
                {
                    bool found = false;
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        if (name.Equals(serie.data[i].name))
                        {
                            destSerie = serie;
                            destSerieData = serie.data[i];
                            found = true;
                            break;
                        }
                    }
                    if (found) break;
                }
                if (name.Equals(serie.serieName))
                {
                    destSerie = serie;
                    destSerieData = null;
                    break;
                }
            }
            var itemStyle = SerieHelper.GetItemStyle(destSerie, destSerieData, SerieState.Normal);
            if (ChartHelper.IsClearColor(itemStyle.markColor))
            {
                Color32 color, toColor;
                SerieHelper.GetItemColor(out color, out toColor, destSerie, destSerieData, chart.theme, index, SerieState.Normal);
                return color;
            }
            else
            {
                return itemStyle.markColor;
            }
        }

        /// <summary>
        /// 是否有需裁剪的serie。
        /// </summary>
        /// <returns></returns>
        public static bool IsAnyClipSerie(List<Serie> series)
        {
            foreach (var serie in series)
            {
                if (serie.clip) return true;
            }
            return false;
        }

        /// <summary>
        /// check if series has any serie which is color by data.
        /// || 是否有任何一个系列是按数据颜色的。
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        public static bool IsAnyColorByDataSerie(List<Serie> series)
        {
            foreach (var serie in series)
            {
                if (serie.defaultColorBy == SerieColorBy.Data) return true;
            }
            return false;
        }

        /// <summary>
        /// 获得上一个同堆叠且显示的serie。
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public static Serie GetLastStackSerie(List<Serie> series, Serie serie)
        {
            if (serie == null || string.IsNullOrEmpty(serie.stack)) return null;
            for (int i = serie.index - 1; i >= 0; i--)
            {
                var temp = series[i];
                if (temp.show && serie.stack.Equals(temp.stack)) return temp;
            }
            return null;
        }

        private static HashSet<string> _setForStack = new HashSet<string>();
        /// <summary>
        /// 是否由数据堆叠
        /// </summary>
        /// <returns></returns>
        public static bool IsStack(List<Serie> series)
        {
            _setForStack.Clear();
            foreach (var serie in series)
            {
                if (string.IsNullOrEmpty(serie.stack)) continue;
                if (_setForStack.Contains(serie.stack)) return true;
                _setForStack.Add(serie.stack);
            }
            return false;
        }

        /// <summary>
        /// 是否堆叠
        /// </summary>
        /// <param name="stackName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStack<T>(List<Serie> series, string stackName) where T : Serie
        {
            if (string.IsNullOrEmpty(stackName)) return false;
            int count = 0;
            foreach (var serie in series)
            {
                if (serie.show && serie is T)
                {
                    if (stackName.Equals(serie.stack)) count++;
                    if (count >= 2) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否时百分比堆叠
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPercentStack<T>(List<Serie> series) where T : Serie
        {
            int count = 0;
            bool isPercentStack = false;
            foreach (var serie in series)
            {
                if (serie.show && serie is T)
                {
                    if (!string.IsNullOrEmpty(serie.stack))
                    {
                        count++;
                        if (serie.barPercentStack) isPercentStack = true;
                    }
                    if (count >= 2 && isPercentStack) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否时百分比堆叠
        /// </summary>
        /// <param name="stackName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPercentStack<T>(List<Serie> series, string stackName) where T : Serie
        {
            if (string.IsNullOrEmpty(stackName)) return false;
            int count = 0;
            bool isPercentStack = false;
            foreach (var serie in series)
            {
                if (serie.show && serie is T)
                {
                    if (stackName.Equals(serie.stack))
                    {
                        count++;
                        if (serie.barPercentStack) isPercentStack = true;
                    }
                    if (count >= 2 && isPercentStack) return true;
                }
            }
            return false;
        }

        private static Dictionary<string, int> sets = new Dictionary<string, int>();
        /// <summary>
        /// 获得堆叠系列列表
        /// </summary>
        /// <param name="Dictionary<int"></param>
        /// <param name="stackSeries"></param>
        public static void GetStackSeries(List<Serie> series, ref Dictionary<int, List<Serie>> stackSeries)
        {
            int count = 0;
            var serieCount = series.Count;
            sets.Clear();
            if (stackSeries == null)
            {
                stackSeries = new Dictionary<int, List<Serie>>(serieCount);
            }
            else
            {
                foreach (var kv in stackSeries)
                {
                    kv.Value.Clear();
                }
            }
            for (int i = 0; i < serieCount; i++)
            {
                var serie = series[i];
                serie.index = i;
                if (string.IsNullOrEmpty(serie.stack))
                {
                    if (!stackSeries.ContainsKey(count))
                        stackSeries[count] = new List<Serie>(serieCount);
                    stackSeries[count].Add(serie);
                    count++;
                }
                else
                {
                    if (!sets.ContainsKey(serie.stack))
                    {
                        sets.Add(serie.stack, count);
                        if (!stackSeries.ContainsKey(count))
                            stackSeries[count] = new List<Serie>(serieCount);
                        stackSeries[count].Add(serie);
                        count++;
                    }
                    else
                    {
                        int stackIndex = sets[serie.stack];
                        stackSeries[stackIndex].Add(serie);
                    }
                }
            }
        }

        public static void UpdateStackDataList(List<Serie> series, Serie currSerie, DataZoom dataZoom, List<List<SerieData>> dataList)
        {
            dataList.Clear();
            for (int i = 0; i <= currSerie.index; i++)
            {
                var serie = series[i];
                if (serie.show && serie.GetType() == currSerie.GetType() && ChartHelper.IsValueEqualsString(serie.stack, currSerie.stack))
                {
                    dataList.Add(serie.GetDataList(dataZoom));
                }
            }
        }

        /// <summary>
        /// 获得维度X的最大最小值
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <param name="axisIndex"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public static void GetXMinMaxValue(BaseChart chart, int axisIndex, bool inverse, out double minValue,
            out double maxValue, bool isPolar = false, bool filterByDataZoom = true, bool needAnimation = false)
        {
            GetMinMaxValue(chart, axisIndex, inverse, 0, out minValue, out maxValue, isPolar, filterByDataZoom, needAnimation);
        }

        /// <summary>
        /// 获得维度Y的最大最小值
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <param name="axisIndex"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public static void GetYMinMaxValue(BaseChart chart, int axisIndex, bool inverse, out double minValue,
            out double maxValue, bool isPolar = false, bool filterByDataZoom = true, bool needAnimation = false)
        {
            GetMinMaxValue(chart, axisIndex, inverse, 1, out minValue, out maxValue, isPolar, filterByDataZoom, needAnimation);
        }

        /// <summary>
        /// 获得维度Z的最大最小值
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <param name="axisIndex"></param>
        /// <param name="minValue"></param>
        /// <param name="maxValue"></param>
        public static void GetZMinMaxValue(BaseChart chart, int axisIndex, bool inverse, out double minValue,
            out double maxValue, bool isPolar = false, bool filterByDataZoom = true, bool needAnimation = false)
        {
            GetMinMaxValue(chart, axisIndex, inverse, 2, out minValue, out maxValue, isPolar, filterByDataZoom, needAnimation);
        }

        private static Dictionary<int, List<Serie>> _stackSeriesForMinMax = new Dictionary<int, List<Serie>>();
        private static Dictionary<int, double> _serieTotalValueForMinMax = new Dictionary<int, double>();
        public static void GetMinMaxValue(BaseChart chart, int axisIndex,
            bool inverse, int dimension, out double minValue, out double maxValue, bool isPolar = false,
            bool filterByDataZoom = true, bool needAnimation = false)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            var series = chart.series;
            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(series);
            if (!SeriesHelper.IsStack(series))
            {
                for (int i = 0; i < series.Count; i++)
                {
                    var serie = series[i];
                    if ((isPolar && serie.polarIndex != axisIndex) ||
                        (!isPolar && serie.yAxisIndex != axisIndex) ||
                        !serie.show) continue;
                    var updateDuration = needAnimation ? serie.animation.GetChangeDuration() : 0;
                    var dataAddDuration = needAnimation ? serie.animation.GetAdditionDuration() : 0;
                    var unscaledTime = serie.animation.unscaledTime;
                    if (isPercentStack && SeriesHelper.IsPercentStack<Bar>(series, serie.serieName))
                    {
                        if (100 > max) max = 100;
                        if (0 < min) min = 0;
                    }
                    else
                    {
                        var showData = serie.GetDataList(filterByDataZoom ? chart.GetXDataZoomOfSerie(serie) : null);
                        if (serie is Candlestick || serie is SimplifiedCandlestick)
                        {
                            foreach (var data in showData)
                            {
                                double dataMin, dataMax;
                                data.GetMinMaxData(1, inverse, out dataMin, out dataMax);
                                if (dataMax > max) max = dataMax;
                                if (dataMin < min) min = dataMin;
                            }
                        }
                        else
                        {
                            var performanceMode = serie.IsPerformanceMode();
                            foreach (var data in showData)
                            {
                                var currData = performanceMode ? data.GetData(dimension, inverse) :
                                    data.GetCurrData(dimension, dataAddDuration, updateDuration, unscaledTime, inverse);
                                if (!serie.IsIgnoreValue(data, currData))
                                {
                                    if (currData > max) max = currData;
                                    if (currData < min) min = currData;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                SeriesHelper.GetStackSeries(series, ref _stackSeriesForMinMax);
                foreach (var ss in _stackSeriesForMinMax)
                {
                    _serieTotalValueForMinMax.Clear();
                    for (int i = 0; i < ss.Value.Count; i++)
                    {
                        var serie = ss.Value[i];
                        if ((isPolar && serie.polarIndex != axisIndex) ||
                            (!isPolar && serie.yAxisIndex != axisIndex) ||
                            !serie.show) continue;
                        var showData = serie.GetDataList(filterByDataZoom ? chart.GetXDataZoomOfSerie(serie) : null);
                        if (SeriesHelper.IsPercentStack<Bar>(series, serie.stack))
                        {
                            for (int j = 0; j < showData.Count; j++)
                            {
                                _serieTotalValueForMinMax[j] = 100;
                            }
                        }
                        else
                        {
                            var updateDuration = needAnimation ? serie.animation.GetChangeDuration() : 0;
                            var dataAddDuration = needAnimation ? serie.animation.GetAdditionDuration() : 0;
                            var unscaledTime = serie.animation.unscaledTime;
                            for (int j = 0; j < showData.Count; j++)
                            {
                                if (!_serieTotalValueForMinMax.ContainsKey(j))
                                    _serieTotalValueForMinMax[j] = 0;
                                double currData = 0;
                                if (serie is Candlestick)
                                {
                                    currData = showData[j].GetMaxData(false);
                                }
                                else
                                {
                                    currData = showData[j].GetCurrData(dimension, dataAddDuration, updateDuration, unscaledTime, inverse);
                                }
                                if (!serie.IsIgnoreValue(showData[j], currData))
                                    _serieTotalValueForMinMax[j] = _serieTotalValueForMinMax[j] + currData;
                            }
                        }
                    }
                    double tmax = double.MinValue;
                    double tmin = double.MaxValue;
                    foreach (var tt in _serieTotalValueForMinMax)
                    {
                        if (tt.Value > tmax) tmax = tt.Value;
                        if (tt.Value < tmin) tmin = tt.Value;
                    }
                    if (tmax > max) max = tmax;
                    if (tmin < min) min = tmin;
                }
            }
            if (max == double.MinValue && min == double.MaxValue)
            {
                minValue = 0;
                maxValue = 0;
            }
            else if (min == 0 && max == 0)
            {
                minValue = 0;
                maxValue = 1;
            }
            else
            {
                minValue = min;
                maxValue = max;
            }
        }

        public static int GetMaxSerieDataCount(List<Serie> series)
        {
            int max = 0;
            foreach (var serie in series)
            {
                if (serie.dataCount > max) max = serie.dataCount;
            }
            return max;
        }

        public static float GetMinAnimationDuration(List<Serie> series)
        {
            float min = float.MaxValue;
            foreach (var serie in series)
            {
                var changeAnimation = serie.animation.change.duration;
                var additionAnimation = serie.animation.addition.duration;
                if (changeAnimation != 0 && changeAnimation < min) min = changeAnimation;
                if (additionAnimation != 0 && additionAnimation < min) min = additionAnimation;
            }
            return min;
        }
    }
}