/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using System.Linq;

namespace XCharts
{
    internal static class TooltipHelper
    {
        private const string PH_NN = "\n";
        private static Regex s_Regex = new Regex(@"{([a-d|.]\d*)(:\d+(-\d+)?)?(:[c-g|x|p|r]\d*)?}", RegexOptions.IgnoreCase);
        private static Regex s_RegexSub = new Regex(@"(\w?-?\d+)|(\w)|(\.)", RegexOptions.IgnoreCase);
        private static Regex s_RegexN = new Regex(@"^\d+", RegexOptions.IgnoreCase);
        private static Regex s_RegexN_N = new Regex(@"\d+-\d+", RegexOptions.IgnoreCase);
        private static Regex s_RegexFn = new Regex(@"[c-g|x|p|r]\d*", RegexOptions.IgnoreCase);
        private static Regex s_RegexNewLine = new Regex(@"[\\|/]+n", RegexOptions.IgnoreCase);

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
                                ReplaceContent(ref itemTitle, dataIndex, tooltip, serie, null, themeInfo, category, dataZoom, isCartesian);
                                sb.Append(itemTitle).Append(PH_NN);
                            }
                            var dataIndexList = tooltip.runtimeSerieDataIndex[serie.index];
                            foreach (var tempIndex in dataIndexList)
                            {
                                string content = itemFormatter;
                                var foundDot = ReplaceContent(ref content, tempIndex, tooltip, serie, null, themeInfo, category, dataZoom, isCartesian);
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
                        needCategory = needCategory || (serie.type == SerieType.Line || serie.type == SerieType.Bar);
                        if (formatTitle)
                        {
                            ReplaceContent(ref title, dataIndex, tooltip, null, series, themeInfo, category, dataZoom, isCartesian);
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
                            ReplaceContent(ref content, dataIndex, tooltip, serie, null, themeInfo, category, dataZoom, isCartesian);
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
                    title = s_RegexNewLine.Replace(title, PH_NN);
                    return title + PH_NN + TrimAndReplaceLine(sb);
                }
            }
            else
            {
                string content = tooltip.formatter;
                ReplaceContent(ref content, dataIndex, tooltip, null, series, themeInfo, category, dataZoom, isCartesian);
                return content;
            }
        }

        public static bool ReplaceContent(ref string content, int dataIndex, Tooltip tooltip, Serie serie, Series series,
            ThemeInfo themeInfo, string category = null, DataZoom dataZoom = null, bool isCartesian = false)
        {
            var foundDot = false;
            var mc = s_Regex.Matches(content);
            foreach (var m in mc)
            {
                var old = m.ToString();
                var args = s_RegexSub.Matches(m.ToString());
                var argsCount = args.Count;
                if (argsCount <= 0) continue;
                int targetIndex = 0;
                char p = GetSerieIndex(args[0].ToString(), ref targetIndex);
                if (serie == null)
                {
                    if (targetIndex == -1) continue;
                    serie = series.GetSerie(targetIndex);
                    if (serie == null) continue;
                }
                else
                {
                    targetIndex = serie.index;
                }
                if (p == '.')
                {
                    var bIndex = targetIndex;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexN.IsMatch(args1Str)) bIndex = int.Parse(args1Str);
                    }
                    content = content.Replace(old, ChartCached.ColorToDotStr(themeInfo.GetColor(bIndex)));
                    foundDot = true;
                }
                else if (p == 'a' || p == 'A')
                {
                    if (argsCount == 1)
                    {
                        content = content.Replace(old, serie.name);
                    }
                }
                else if (p == 'b' || p == 'B')
                {
                    var bIndex = dataIndex;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexN.IsMatch(args1Str)) bIndex = int.Parse(args1Str);
                    }
                    var needCategory = serie.type == SerieType.Line || serie.type == SerieType.Bar;
                    if (needCategory)
                    {
                        content = content.Replace(old, category);
                    }
                    else
                    {
                        var serieData = serie.GetSerieData(bIndex, dataZoom);
                        content = content.Replace(old, serieData.name);
                    }
                }
                else if (p == 'c' || p == 'C' || p == 'd' || p == 'D')
                {
                    var isPercent = p == 'd' || p == 'D';
                    var bIndex = dataIndex;
                    var dimensionIndex = -1;
                    var numericFormatter = string.Empty;
                    if (argsCount >= 2)
                    {
                        var args1Str = args[1].ToString();
                        if (s_RegexFn.IsMatch(args1Str))
                        {
                            numericFormatter = args1Str;
                        }
                        else if (s_RegexN_N.IsMatch(args1Str))
                        {
                            var temp = args1Str.Split('-');
                            bIndex = int.Parse(temp[0]);
                            dimensionIndex = int.Parse(temp[1]);
                        }
                        else if (s_RegexN.IsMatch(args1Str))
                        {
                            dimensionIndex = int.Parse(args1Str);
                        }
                        else
                        {
                            Debug.LogError("unmatch:" + args1Str);
                            continue;
                        }
                    }
                    if (argsCount >= 3)
                    {
                        numericFormatter = args[2].ToString();
                    }
                    if (dimensionIndex == -1) dimensionIndex = 1;
                    if (numericFormatter == string.Empty)
                    {
                        numericFormatter = GetItemNumericFormatter(tooltip, serie, serie.GetSerieData(bIndex));
                    }
                    var value = serie.GetData(bIndex, dimensionIndex, dataZoom);
                    if (isPercent)
                    {
                        var percent = value / serie.yTotal * 100;
                        content = content.Replace(old, ChartCached.FloatToStr(percent, numericFormatter));
                    }
                    else
                    {
                        content = content.Replace(old, ChartCached.FloatToStr(value, numericFormatter));
                    }
                }
            }
            content = s_RegexNewLine.Replace(content, PH_NN);
            return foundDot;
        }

        private static char GetSerieIndex(string strType, ref int index)
        {
            index = 0;
            if (strType.Length > 1)
            {
                if (!int.TryParse(strType.Substring(1), out index))
                {
                    index = -1;
                }
            }
            return strType.ElementAt(0);
        }

        private static string TrimAndReplaceLine(StringBuilder sb)
        {
            return s_RegexNewLine.Replace(sb.ToString().Trim(), PH_NN);
        }

        private static bool IsSelectedSerie(Tooltip tooltip, int serieIndex)
        {
            if (tooltip.runtimeSerieDataIndex.ContainsKey(serieIndex))
            {
                return tooltip.runtimeSerieDataIndex[serieIndex].Count > 0;
            }
            return false;
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
            if (!ChartHelper.IsClearColor(lineStyle.color))
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