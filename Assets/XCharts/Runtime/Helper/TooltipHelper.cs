using System;
using System.Collections.Generic;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    internal static class TooltipHelper
    {
        private const string PH_A = "{a}";
        private const string PH_B = "{b}";
        private const string PH_C = "{c}";
        private const string PH_D = "{d}";
        private const string PH_I = "{.}";
        private const string PH_Y = "{j}";
        private const string PH_ON = "\\n";
        private const string PH_NN = "\n";
        private const string PH_NN_BBB = "\n";
        private const string PH_BR = "<br/>";
        private static Dictionary<string, Dictionary<int, string>> s_PHDic = new Dictionary<string, Dictionary<int, string>>();
        private static Dictionary<int, string> s_PHCCDic = new Dictionary<int, string>();
        private static Dictionary<int, Dictionary<int, string>> s_PHSerieCCDic = new Dictionary<int, Dictionary<int, string>>();

        private static void InitScatterTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ThemeInfo themeInfo)
        {
            if (!tooltip.runtimeSerieDataIndex.ContainsKey(serie.index)) return;
            var dataIndexList = tooltip.runtimeSerieDataIndex[serie.index];
            if (!string.IsNullOrEmpty(serie.name))
            {
                sb.Append(serie.name).Append(PH_NN);
            }
            for (int i = 0; i < dataIndexList.Count; i++)
            {
                var dataIndex = dataIndexList[i];
                var serieData = serie.GetSerieData(dataIndex);
                var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
                float xValue, yValue;
                serie.GetXYData(dataIndex, null, out xValue, out yValue);

                sb.Append("<color=#").Append(themeInfo.GetColorStr(serie.index)).Append(">● </color>");
                if (!string.IsNullOrEmpty(serieData.name))
                    sb.Append(serieData.name).Append(": ");
                sb.AppendFormat("({0},{1})", ChartCached.FloatToStr(xValue, numericFormatter),
                ChartCached.FloatToStr(yValue, numericFormatter));
                if (i != dataIndexList.Count - 1)
                {
                    sb.Append("\n");
                }
            }
        }


        private static void InitPieTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ThemeInfo themeInfo)
        {
            string key = serie.data[index].name;
            var serieData = serie.GetSerieData(index);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);

            float value = serieData.GetData(1);
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serie.name))
            {
                sb.Append(serie.name).Append(PH_NN);
            }
            sb.Append("<color=#").Append(themeInfo.GetColorStr(index)).Append(">● </color>");
            if (!string.IsNullOrEmpty(key))
                sb.Append(key).Append(": ");
            sb.Append(ChartCached.FloatToStr(value, numericFormatter));
        }

        private static void InitRingTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ThemeInfo themeInfo)
        {
            var serieData = serie.GetSerieData(index);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
            float value = serieData.GetFirstData();
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serieData.name))
            {
                sb.Append("<color=#").Append(themeInfo.GetColorStr(index)).Append(">● </color>")
                .Append(serieData.name).Append(": ").Append(ChartCached.FloatToStr(value, numericFormatter));
            }
            else
            {
                sb.Append(ChartCached.FloatToStr(value, numericFormatter));
            }
        }

        public static void InitRadarTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, Radar radar,
            ThemeInfo themeInfo)
        {
            var dataIndex = tooltip.runtimeDataIndex[1];
            var serieData = serie.GetSerieData(dataIndex);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    sb.Append(serieData.name);
                    for (int i = 0; i < radar.indicatorList.Count; i++)
                    {
                        string key = radar.indicatorList[i].name;
                        float value = serieData.GetData(i);
                        if ((i == 0 && !string.IsNullOrEmpty(serieData.name)) || i > 0) sb.Append(PH_NN);
                        sb.AppendFormat("{0}: {1}", key, ChartCached.FloatToStr(value, numericFormatter));
                    }
                    break;
                case RadarType.Single:
                    string key2 = serieData.name;
                    float value2 = serieData.GetData(1);
                    if (string.IsNullOrEmpty(key2))
                    {
                        key2 = radar.indicatorList[dataIndex].name;
                    }
                    sb.AppendFormat("{0}: {1}", key2, ChartCached.FloatToStr(value2, numericFormatter));
                    break;
            }
        }

        private static void InitCoordinateTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ThemeInfo themeInfo, bool isCartesian, DataZoom dataZoom = null)
        {
            string key = serie.name;
            float xValue, yValue;
            serie.GetXYData(index, dataZoom, out xValue, out yValue);
            var isIngore = serie.IsIgnorePoint(index);
            var serieData = serie.GetSerieData(index, dataZoom);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
            if (isCartesian)
            {
                if (serieData != null && serieData.highlighted)
                {
                    sb.Append(key).Append(!string.IsNullOrEmpty(key) ? " : " : "");
                    sb.Append("[").Append(ChartCached.FloatToStr(xValue, numericFormatter)).Append(",")
                        .Append(ChartCached.FloatToStr(yValue, numericFormatter)).Append("]");
                }
            }
            else
            {
                var valueTxt = isIngore ? tooltip.ignoreDataDefaultContent :
                    ChartCached.FloatToStr(yValue, numericFormatter);
                sb.Append("<color=#").Append(themeInfo.GetColorStr(serie.index)).Append(">● </color>")
                .Append(key).Append(!string.IsNullOrEmpty(key) ? " : " : "")
                .Append(valueTxt);
            }
        }

        private static void InitDefaultContent(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            string category, ThemeInfo themeInfo = null, DataZoom dataZoom = null, bool isCartesian = false)
        {
            switch (serie.type)
            {
                case SerieType.Line:
                case SerieType.Bar:
                    InitCoordinateTooltip(ref sb, tooltip, serie, index, themeInfo, isCartesian, dataZoom);
                    break;
                case SerieType.Scatter:
                case SerieType.EffectScatter:
                    InitScatterTooltip(ref sb, tooltip, serie, index, themeInfo);
                    break;
                case SerieType.Radar:
                    break;
                case SerieType.Pie:
                    InitPieTooltip(ref sb, tooltip, serie, index, themeInfo);
                    break;
                case SerieType.Ring:
                    InitRingTooltip(ref sb, tooltip, serie, index, themeInfo);
                    break;
                case SerieType.Heatmap:
                    break;
                case SerieType.Gauge:
                    break;
            }
        }

        public static void SetContentAndPosition(Tooltip tooltip, string content, Rect chartRect)
        {
            tooltip.UpdateContentText(content);
            var pos = tooltip.GetContentPos();
            if (pos.x + tooltip.runtimeWidth > chartRect.x + chartRect.width)
            {
                pos.x = chartRect.x + chartRect.width - tooltip.runtimeWidth;
            }
            if (pos.y - tooltip.runtimeHeight < chartRect.y)
            {
                pos.y = chartRect.y + tooltip.runtimeHeight;
            }
            tooltip.UpdateContentPos(pos);
        }

        public static string GetFormatterContent(Tooltip tooltip, int dataIndex, Series series, ThemeInfo themeInfo,
            string category = null, DataZoom dataZoom = null, bool isCartesian = false)
        {
            if (string.IsNullOrEmpty(tooltip.formatter))
            {
                var sb = ChartHelper.sb;
                var title = tooltip.titleFormatter;
                var formatTitle = !string.IsNullOrEmpty(title);
                var needCategory = false;
                var first = true;
                var isScatter = false;
                sb.Length = 0;
                for (int i = 0; i < series.Count; i++)
                {
                    var serie = series.GetSerie(i);
                    if (serie.type == SerieType.Scatter || serie.type == SerieType.EffectScatter)
                    {
                        if (serie.show && IsSelectedSerie(tooltip, serie.index))
                        {
                            isScatter = true;
                            var itemFormatter = GetItemFormatter(tooltip, serie, null);
                            var numericFormatter = GetItemNumericFormatter(tooltip, serie, null);
                            if (string.IsNullOrEmpty(itemFormatter))
                            {
                                if (!first) sb.Append(PH_NN);
                                InitDefaultContent(ref sb, tooltip, serie, dataIndex, category, themeInfo, dataZoom, isCartesian);
                                first = false;
                                continue;
                            }
                            var itemTitle = title;
                            if (!string.IsNullOrEmpty(itemTitle))
                            {
                                Replace(ref itemTitle, PH_A, i, serie.name, true);
                                sb.Append(itemTitle).Append(PH_NN);
                            }
                            var dataIndexList = tooltip.runtimeSerieDataIndex[serie.index];
                            foreach (var tempIndex in dataIndexList)
                            {
                                var foundDot = false;
                                var serieData = serie.GetSerieData(tempIndex);
                                string content = itemFormatter;
                                Replace(ref content, PH_A, i, serie.name, true);
                                Replace(ref content, PH_B, i, needCategory ? category : serieData.name, true);
                                if (itemFormatter.IndexOf(PH_I) >= 0)
                                {
                                    foundDot = true;
                                    Replace(ref content, PH_I, i, ChartCached.ColorToDotStr(themeInfo.GetColor(serie.index)), true);
                                }
                                for (int n = 0; n < serieData.data.Count; n++)
                                {
                                    var valueStr = ChartCached.FloatToStr(serieData.GetData(n), numericFormatter);
                                    Replace(ref content, GetPHCC(n), i, valueStr, true);
                                }
                                if (!foundDot)
                                {
                                    sb.Append(ChartCached.ColorToDotStr(themeInfo.GetColor(serie.index)));
                                }
                                sb.Append(content).Append(PH_NN);
                            }
                        }
                    }
                    else
                    {
                        var serieData = serie.GetSerieData(dataIndex, dataZoom);
                        if (serieData == null) continue;
                        var itemFormatter = GetItemFormatter(tooltip, serie, serieData);
                        var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
                        var percent = serieData.GetData(1) / serie.yTotal * 100;
                        needCategory = needCategory || (serie.type == SerieType.Line || serie.type == SerieType.Bar);
                        if (formatTitle)
                        {
                            var valueStr = ChartCached.FloatToStr(serieData.GetData(1), numericFormatter);
                            Replace(ref title, PH_A, i, serie.name, true);
                            Replace(ref title, PH_B, i, needCategory ? category : serieData.name, true);
                            Replace(ref title, PH_C, i, valueStr, true);
                            Replace(ref title, PH_D, i, ChartCached.FloatToStr(percent, string.Empty, 1), true);
                        }
                        if (serie.show)
                        {
                            if (string.IsNullOrEmpty(itemFormatter))
                            {
                                if (!first) sb.Append(PH_NN);
                                InitDefaultContent(ref sb, tooltip, serie, dataIndex, category, themeInfo, dataZoom, isCartesian);
                                first = false;
                                continue;
                            }
                            string content = itemFormatter;
                            var valueStr = ChartCached.FloatToStr(serieData.GetData(1), numericFormatter);
                            Replace(ref content, PH_A, i, serie.name, true);
                            Replace(ref content, PH_B, i, needCategory ? category : serieData.name, true);
                            Replace(ref content, PH_C, i, valueStr, true);
                            Replace(ref content, PH_D, i, ChartCached.FloatToStr(percent, string.Empty, 1), true);
                            for (int n = 0; n < serieData.data.Count; n++)
                            {
                                valueStr = ChartCached.FloatToStr(serieData.GetData(n), numericFormatter);
                                Replace(ref content, GetPHCC(n), i, valueStr, true);
                            }
                            if (!first) sb.Append(PH_NN);
                            sb.Append(ChartCached.ColorToDotStr(themeInfo.GetColor(i)));
                            sb.Append(content);
                            first = false;
                        }
                    }
                }
                if (isScatter)
                {
                    return TrimAndReplaceLine(sb);
                }
                else if (string.IsNullOrEmpty(title))
                {
                    if (needCategory) return category + PH_NN + TrimAndReplaceLine(sb);
                    else return TrimAndReplaceLine(sb);
                }
                else
                {
                    title = title.Replace(PH_ON, PH_NN);
                    title = title.Replace(PH_BR, PH_NN);
                    return title + PH_NN + TrimAndReplaceLine(sb);
                }
            }
            else
            {
                string content = tooltip.formatter;
                for (int i = 0; i < series.Count; i++)
                {
                    var serie = series.GetSerie(i);
                    if (serie.show)
                    {
                        var needCategory = serie.type == SerieType.Line || serie.type == SerieType.Bar;
                        var serieData = serie.GetSerieData(dataIndex, dataZoom);
                        var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
                        var percent = serieData.GetData(1) / serie.yTotal * 100;
                        Replace(ref content, PH_A, i, serie.name);
                        Replace(ref content, PH_B, i, needCategory ? category : serieData.name);
                        Replace(ref content, PH_C, i, ChartCached.FloatToStr(serieData.GetData(1), numericFormatter));
                        Replace(ref content, PH_D, i, ChartCached.FloatToStr(percent, string.Empty, 1));
                        Replace(ref content, PH_I, i, ChartCached.ColorToDotStr(themeInfo.GetColor(i)));
                        for (int n = 0; n < serieData.data.Count; n++)
                        {
                            var valueStr = ChartCached.FloatToStr(serieData.GetData(n), numericFormatter);
                            if (i == 0) Replace(ref content, GetPHCC(n), i, valueStr, true);
                            Replace(ref content, GetPHCC(i, n), i, valueStr, true);
                        }
                    }
                }
                content = content.Replace(PH_ON, PH_NN);
                content = content.Replace(PH_BR, PH_NN);
                return content;
            }
        }

        private static string TrimAndReplaceLine(StringBuilder sb)
        {
            return sb.ToString().Trim().Replace(PH_ON, PH_NN_BBB).Replace(PH_BR, PH_NN_BBB);
        }

        private static bool IsSelectedSerie(Tooltip tooltip, int serieIndex)
        {
            if (tooltip.runtimeSerieDataIndex.ContainsKey(serieIndex))
            {
                return tooltip.runtimeSerieDataIndex[serieIndex].Count > 0;
            }
            return false;
        }

        private static void Replace(ref string content, string placeHolder, int index, string newStr, bool all = false)
        {
            if ((all || index == 0) && content.IndexOf(placeHolder) >= 0)
            {
                content = content.Replace(placeHolder, newStr);
            }
            if (!s_PHDic.ContainsKey(placeHolder))
            {
                s_PHDic[placeHolder] = new Dictionary<int, string>();
            }
            if (!s_PHDic[placeHolder].ContainsKey(index))
            {
                s_PHDic[placeHolder][index] = placeHolder.Insert(2, index.ToString());
            }
            var holder = s_PHDic[placeHolder][index];
            if (content.IndexOf(holder) >= 0)
            {
                content = content.Replace(holder, newStr);
            }
        }

        private static string GetPHCC(int index)
        {
            if (!s_PHCCDic.ContainsKey(index))
            {
                s_PHCCDic[index] = "{c:" + index + "}";
            }
            return s_PHCCDic[index];
        }

        private static string GetPHCC(int serieIndex, int dataIndex)
        {
            if (!s_PHSerieCCDic.ContainsKey(serieIndex))
            {
                s_PHSerieCCDic[serieIndex] = new Dictionary<int, string>();
            }
            if (!s_PHSerieCCDic[serieIndex].ContainsKey(dataIndex))
            {
                s_PHSerieCCDic[serieIndex][dataIndex] = "{c" + serieIndex + ":" + dataIndex + "}";
            }
            return s_PHSerieCCDic[serieIndex][dataIndex];
        }

        private static string GetItemFormatter(Tooltip tooltip, Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.tooltipFormatter)) return itemStyle.tooltipFormatter;
            else return tooltip.itemFormatter;
        }

        private static string GetItemNumericFormatter(Tooltip tooltip, Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.numericFormatter)) return itemStyle.numericFormatter;
            else return tooltip.numericFormatter;
        }

        public static Color GetLineColor(Tooltip tooltip, ThemeInfo theme)
        {
            var lineStyle = tooltip.lineStyle;
            if (lineStyle.color != Color.clear)
            {
                var color = lineStyle.color;
                color.a *= lineStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.tooltipLineColor;
                color.a *= lineStyle.opacity;
                return color;
            }
        }
    }
}