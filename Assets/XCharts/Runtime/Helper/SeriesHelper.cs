/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public static class SeriesHelper
    {
        public static bool IsNeedLabelUpdate(Series series)
        {
            foreach (var serie in series.list)
            {
                if (serie.label.vertsDirty) return true;
            }
            return false;
        }

        public static bool IsLabelDirty(Series series)
        {
            if (series.labelDirty) return true;
            foreach (var serie in series.list)
            {
                if (serie.label.componentDirty) return true;
            }
            return false;
        }

        public static bool IsNameDirty(Series series)
        {
            foreach (var serie in series.list)
            {
                if (serie.nameDirty) return true;
            }
            return false;
        }

        public static void ClearNameDirty(Series series)
        {
            foreach (var serie in series.list)
            {
                serie.ClearNameDirty();
            }
        }

        public static bool IsLegalLegendName(string name)
        {
            int numName = -1;
            if (int.TryParse(name, out numName))
            {
                if (numName >= 0 && numName < 100) return false;
            }
            return true;
        }

        public static List<string> GetLegalSerieNameList(Series series)
        {
            var list = new List<string>();
            for (int n = 0; n < series.list.Count; n++)
            {
                var serie = series.GetSerie(n);
                switch (serie.type)
                {
                    case SerieType.Pie:
                    case SerieType.Radar:
                    case SerieType.Ring:
                        for (int i = 0; i < serie.data.Count; i++)
                        {
                            var dataName = serie.data[i].name;
                            if (!string.IsNullOrEmpty(dataName) && IsLegalLegendName(dataName) && !list.Contains(dataName))
                                list.Add(dataName);
                        }
                        break;
                    default:
                        if (!string.IsNullOrEmpty(serie.name) && !list.Contains(serie.name) && IsLegalLegendName(serie.name))
                            list.Add(serie.name);
                        break;
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
            for (int n = 0; n < chart.series.list.Count; n++)
            {
                var serie = chart.series.GetSerie(n);
                if (serie.type == SerieType.Pie
                    || serie.type == SerieType.Radar
                    || serie.type == SerieType.Ring
                    || (serie.type == SerieType.Custom && chart.GetCustomSerieDataNameForColor()))
                {
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        if (serie.type == SerieType.Pie && serie.IsIgnoreValue(serie.data[i])) continue;
                        if (string.IsNullOrEmpty(serie.data[i].name))
                            serieNameList.Add(ChartCached.IntToStr(i));
                        else if (!serieNameList.Contains(serie.data[i].name))
                            serieNameList.Add(serie.data[i].name);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(serie.name))
                        serieNameList.Add(ChartCached.IntToStr(n));
                    else if (!serieNameList.Contains(serie.name))
                        serieNameList.Add(serie.name);
                }
            }
        }

        public static Color GetNameColor(BaseChart chart, int index, string name)
        {
            Serie destSerie = null;
            SerieData destSerieData = null;
            var series = chart.series;
            for (int n = 0; n < series.list.Count; n++)
            {
                var serie = series.GetSerie(n);
                if (serie.type == SerieType.Pie || serie.type == SerieType.Radar || serie.type == SerieType.Ring
                    || (serie.type == SerieType.Custom && chart.GetCustomSerieDataNameForColor()))
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
                else
                {
                    if (name.Equals(serie.name))
                    {
                        destSerie = serie;
                        destSerieData = null;
                        break;
                    }
                }
            }
            return SerieHelper.GetItemColor(destSerie, destSerieData, chart.theme, index, false);
        }

        /// <summary>
        /// 同堆叠的serie是否有渐变色的。
        /// </summary>
        /// <param name="stack"></param>
        /// <returns></returns>
        public static bool IsAnyGradientSerie(Series series, string stack)
        {
            if (string.IsNullOrEmpty(stack)) return false;
            foreach (var serie in series.list)
            {
                if (serie.show && serie.areaStyle.show && stack.Equals(serie.stack))
                {
                    if (!ChartHelper.IsValueEqualsColor(serie.areaStyle.color, serie.areaStyle.toColor)
                    && !ChartHelper.IsClearColor(serie.areaStyle.toColor))
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否有需裁剪的serie。
        /// </summary>
        /// <returns></returns>
        public static bool IsAnyClipSerie(Series series)
        {
            foreach (var serie in series.list)
            {
                if (serie.clip) return true;
            }
            return false;
        }

        public static bool ContainsSerie(Series series, SerieType type)
        {
            foreach (var serie in series.list)
            {
                if (serie.type == type) return true;
            }
            return false;
        }

        public static bool IsAnyUpdateAnimationSerie(Series series)
        {
            foreach (var serie in series.list)
            {
                if (serie.animation.enable && serie.animation.dataChangeEnable)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 获得上一个同堆叠且显示的serie。
        /// </summary>
        /// <param name="serie"></param>
        /// <returns></returns>
        public static Serie GetLastStackSerie(Series series, Serie serie)
        {
            if (serie == null || string.IsNullOrEmpty(serie.stack)) return null;
            for (int i = serie.index - 1; i >= 0; i--)
            {
                var temp = series.list[i];
                if (temp.show && serie.stack.Equals(temp.stack)) return temp;
            }
            return null;
        }

        /// <summary>
        /// 获得上一个同堆叠且显示的serie。
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Serie GetLastStackSerie(Series series, int index)
        {
            var serie = series.GetSerie(index);
            return GetLastStackSerie(series, serie);
        }

        public static Serie GetSerieByVesselIndex(Series series, int vesselIndex)
        {
            foreach (var serie in series.list)
            {
                if (serie.vesselIndex == vesselIndex) return serie;
            }
            return null;
        }

        private static HashSet<string> _setForStack = new HashSet<string>();
        /// <summary>
        /// 是否由数据堆叠
        /// </summary>
        /// <returns></returns>
        public static bool IsStack(Series series)
        {
            _setForStack.Clear();
            foreach (var serie in series.list)
            {
                if (string.IsNullOrEmpty(serie.stack)) continue;
                if (_setForStack.Contains(serie.stack)) return true;
                else
                {
                    _setForStack.Add(serie.stack);
                }
            }
            return false;
        }

        /// <summary>
        /// 是否堆叠
        /// </summary>
        /// <param name="stackName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStack(Series series, string stackName, SerieType type)
        {
            if (string.IsNullOrEmpty(stackName)) return false;
            int count = 0;
            foreach (var serie in series.list)
            {
                if (serie.show && (serie.type == type))
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
        public static bool IsPercentStack(Series series, SerieType type)
        {
            int count = 0;
            bool isPercentStack = false;
            foreach (var serie in series.list)
            {
                if (serie.show && serie.type == type)
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
        public static bool IsPercentStack(Series series, string stackName, SerieType type)
        {
            if (string.IsNullOrEmpty(stackName)) return false;
            int count = 0;
            bool isPercentStack = false;
            foreach (var serie in series.list)
            {
                if (serie.show && serie.type == type)
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
        public static void GetStackSeries(Series series, ref Dictionary<int, List<Serie>> stackSeries)
        {
            int count = 0;
            var serieCount = series.list.Count;
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
                var serie = series.GetSerie(i);
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

        public static void UpdateStackDataList(Series series, Serie currSerie, DataZoom dataZoom, List<List<SerieData>> dataList)
        {
            dataList.Clear();
            for (int i = 0; i <= currSerie.index; i++)
            {
                var serie = series.list[i];
                if (serie.type == currSerie.type && ChartHelper.IsValueEqualsString(serie.stack, currSerie.stack))
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
        /// <param name="minVaule"></param>
        /// <param name="maxValue"></param>
        public static void GetXMinMaxValue(Series series, DataZoom dataZoom, int axisIndex, bool isValueAxis,
            bool inverse, out double minVaule, out double maxValue, bool isPolar = false)
        {
            GetMinMaxValue(series, dataZoom, axisIndex, isValueAxis, inverse, false, out minVaule, out maxValue, isPolar);
        }

        /// <summary>
        /// 获得维度Y的最大最小值
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <param name="axisIndex"></param>
        /// <param name="minVaule"></param>
        /// <param name="maxValue"></param>
        public static void GetYMinMaxValue(Series series, DataZoom dataZoom, int axisIndex, bool isValueAxis,
            bool inverse, out double minVaule, out double maxValue, bool isPolar = false)
        {
            GetMinMaxValue(series, dataZoom, axisIndex, isValueAxis, inverse, true, out minVaule, out maxValue, isPolar);
        }

        private static Dictionary<int, List<Serie>> _stackSeriesForMinMax = new Dictionary<int, List<Serie>>();
        private static Dictionary<int, double> _serieTotalValueForMinMax = new Dictionary<int, double>();
        public static void GetMinMaxValue(Series series, DataZoom dataZoom, int axisIndex, bool isValueAxis,
            bool inverse, bool yValue, out double minVaule, out double maxValue, bool isPolar = false)
        {
            double min = double.MaxValue;
            double max = double.MinValue;
            var isPercentStack = SeriesHelper.IsPercentStack(series, SerieType.Bar);
            if (!SeriesHelper.IsStack(series) || (isValueAxis && !yValue))
            {
                for (int i = 0; i < series.list.Count; i++)
                {
                    var serie = series.GetSerie(i);
                    if ((isPolar && serie.polarIndex != axisIndex)
                        || (!isPolar && serie.yAxisIndex != axisIndex)
                        || !series.IsActive(i)) continue;
                    if (isPercentStack && SeriesHelper.IsPercentStack(series, serie.name, SerieType.Bar))
                    {
                        if (100 > max) max = 100;
                        if (0 < min) min = 0;
                    }
                    else
                    {
                        var showData = serie.GetDataList(dataZoom);
                        foreach (var data in showData)
                        {

                            if (serie.type == SerieType.Candlestick)
                            {
                                var dataMin = data.GetMinData(inverse);
                                var dataMax = data.GetMaxData(inverse);
                                if (dataMax > max) max = dataMax;
                                if (dataMin < min) min = dataMin;
                            }
                            else
                            {
                                var currData = data.GetData(yValue ? 1 : 0, inverse);
                                if (!serie.IsIgnoreValue(currData))
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
                        if ((isPolar && serie.polarIndex != axisIndex)
                        || (!isPolar && serie.yAxisIndex != axisIndex)
                        || !series.IsActive(i)) continue;
                        var showData = serie.GetDataList(dataZoom);
                        if (SeriesHelper.IsPercentStack(series, serie.stack, SerieType.Bar))
                        {
                            for (int j = 0; j < showData.Count; j++)
                            {
                                _serieTotalValueForMinMax[j] = 100;
                            }
                        }
                        else
                        {
                            for (int j = 0; j < showData.Count; j++)
                            {
                                if (!_serieTotalValueForMinMax.ContainsKey(j))
                                    _serieTotalValueForMinMax[j] = 0;
                                double currData = 0;
                                if (serie.type == SerieType.Candlestick)
                                {
                                    currData = showData[j].GetMaxData(false);
                                }
                                else
                                {
                                    currData = yValue ? showData[j].GetData(1) : showData[j].GetData(0);
                                }
                                if (inverse) currData = -currData;
                                if (!serie.IsIgnoreValue(currData))
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
                minVaule = 0;
                maxValue = 0;
            }
            else
            {
                minVaule = min > 1 ? Math.Floor(min) : min;
                maxValue = max > 1 ? Math.Ceiling(max) : max;
            }
        }

        public static int GetMaxSerieDataCount(Series series)
        {
            int max = 0;
            foreach (var serie in series.list)
            {
                if (serie.dataCount > max) max = serie.dataCount;
            }
            return max;
        }
    }
}