/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Text;
using UnityEngine;

namespace XCharts
{
    internal static class TooltipHelper
    {
        private static void InitScatterTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ThemeInfo themeInfo)
        {
            if (!tooltip.runtimeSerieIndex.ContainsKey(serie.index)) return;
            var dataIndexList = tooltip.runtimeSerieIndex[serie.index];
            if (!string.IsNullOrEmpty(serie.name))
            {
                sb.Append(serie.name).Append(FormatterHelper.PH_NN);
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
            if (tooltip.runtimeDataIndex[serie.index] < 0) return;
            string key = serie.data[index].name;
            var serieData = serie.GetSerieData(index);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);

            float value = serieData.GetData(1);
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serie.name))
            {
                sb.Append(serie.name).Append(FormatterHelper.PH_NN);
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
                        if ((i == 0 && !string.IsNullOrEmpty(serieData.name)) || i > 0) sb.Append(FormatterHelper.PH_NN);
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
                var titleIsIgnroe = false;
                var needCategory = false;
                var first = true;
                var isScatter = false;
                sb.Length = 0;
                if ("{i}".Equals(tooltip.titleFormatter))
                {
                    title = string.Empty;
                    formatTitle = false;
                    titleIsIgnroe = true;
                }
                for (int i = 0; i < series.Count; i++)
                {
                    var serie = series.GetSerie(i);
                    if (serie.type == SerieType.Scatter || serie.type == SerieType.EffectScatter)
                    {
                        if (serie.show && IsSelectedSerie(tooltip, serie.index))
                        {
                            isScatter = true;
                            var itemFormatter = GetItemFormatter(tooltip, serie, null);
                            if (string.IsNullOrEmpty(itemFormatter))
                            {
                                if (!first) sb.Append(FormatterHelper.PH_NN);
                                InitDefaultContent(ref sb, tooltip, serie, dataIndex, category, themeInfo, dataZoom);
                                first = false;
                                continue;
                            }
                            var itemTitle = title;
                            if (!string.IsNullOrEmpty(itemTitle))
                            {
                                FormatterHelper.ReplaceContent(ref itemTitle, dataIndex, tooltip.numericFormatter, serie, series, themeInfo, category, dataZoom);
                                sb.Append(itemTitle).Append(FormatterHelper.PH_NN);
                            }
                            var dataIndexList = tooltip.runtimeSerieIndex[serie.index];
                            foreach (var tempIndex in dataIndexList)
                            {
                                string content = itemFormatter;
                                var foundDot = FormatterHelper.ReplaceContent(ref content, tempIndex, tooltip.numericFormatter, serie, series, themeInfo, category, dataZoom);
                                if (!foundDot)
                                {
                                    sb.Append(ChartCached.ColorToDotStr(themeInfo.GetColor(serie.index)));
                                }
                                sb.Append(content).Append(FormatterHelper.PH_NN);
                            }
                        }
                    }
                    else if (IsNeedTooltipSerie(serie, tooltip))
                    {
                        var serieData = serie.GetSerieData(dataIndex, dataZoom);
                        if (serieData == null) continue;
                        var itemFormatter = GetItemFormatter(tooltip, serie, serieData);
                        needCategory = needCategory || (serie.type == SerieType.Line || serie.type == SerieType.Bar);
                        if (formatTitle)
                        {
                            FormatterHelper.ReplaceContent(ref title, dataIndex, tooltip.numericFormatter, serie, series, themeInfo, category, dataZoom);
                        }
                        if (serie.show)
                        {
                            if (string.IsNullOrEmpty(itemFormatter))
                            {
                                if (!first) sb.Append(FormatterHelper.PH_NN);
                                InitDefaultContent(ref sb, tooltip, serie, dataIndex, category, themeInfo, dataZoom);
                                first = false;
                                continue;
                            }
                            string content = itemFormatter;
                            FormatterHelper.ReplaceContent(ref content, dataIndex, tooltip.numericFormatter, serie, series, themeInfo, category, dataZoom);
                            if (!first) sb.Append(FormatterHelper.PH_NN);
                            var dotColorIndex = serie.type == SerieType.Pie || serie.type == SerieType.Radar || serie.type == SerieType.Ring ? dataIndex : i;
                            sb.Append(ChartCached.ColorToDotStr(themeInfo.GetColor(dotColorIndex)));
                            sb.Append(content);
                            first = false;
                        }
                    }
                }
                if (isScatter)
                {
                    return FormatterHelper.TrimAndReplaceLine(sb);
                }
                else if (string.IsNullOrEmpty(title))
                {
                    if (needCategory && !titleIsIgnroe)
                        return category + FormatterHelper.PH_NN + FormatterHelper.TrimAndReplaceLine(sb);
                    else
                        return FormatterHelper.TrimAndReplaceLine(sb);
                }
                else
                {
                    title = FormatterHelper.TrimAndReplaceLine(title);
                    return title + FormatterHelper.PH_NN + FormatterHelper.TrimAndReplaceLine(sb);
                }
            }
            else
            {
                string content = tooltip.formatter;
                FormatterHelper.ReplaceContent(ref content, dataIndex, tooltip.numericFormatter, null, series, themeInfo, category, dataZoom);
                return content;
            }
        }

        private static bool IsNeedTooltipSerie(Serie serie, Tooltip tooltip)
        {
            if (serie.type == SerieType.Pie || serie.type == SerieType.Radar || serie.type == SerieType.Ring)
            {
                return tooltip.runtimeDataIndex[serie.index] >= 0;
            }
            else
            {
                return true;
            }
        }

        private static bool IsSelectedSerie(Tooltip tooltip, int serieIndex)
        {
            if (tooltip.runtimeSerieIndex.ContainsKey(serieIndex))
            {
                return tooltip.runtimeSerieIndex[serieIndex].Count > 0;
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