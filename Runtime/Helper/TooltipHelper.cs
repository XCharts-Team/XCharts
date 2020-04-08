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

        private static void InitPieTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ThemeInfo themeInfo)
        {
            string key = serie.data[index].name;

            float value = serie.data[index].data[1];
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serie.name))
            {
                sb.Append(serie.name).Append("\n");
            }
            sb.Append("<color=#").Append(themeInfo.GetColorStr(index)).Append(">● </color>");
            if (!string.IsNullOrEmpty(key))
                sb.Append(key).Append(": ");
            sb.Append(ChartCached.FloatToStr(value, 0, tooltip.forceENotation));
        }

        private static void InitRingTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ThemeInfo themeInfo)
        {
            var serieData = serie.GetSerieData(index);
            float value = serieData.GetFirstData();
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serieData.name))
            {
                sb.Append("<color=#").Append(themeInfo.GetColorStr(index)).Append(">● </color>")
                .Append(serieData.name).Append(": ").Append(ChartCached.FloatToStr(value, 0, tooltip.forceENotation));
            }
            else
            {
                sb.Append(ChartCached.FloatToStr(value, 0, tooltip.forceENotation));
            }
        }

        public static void InitRadarTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, Radar radar,
            ThemeInfo themeInfo)
        {
            var dataIndex = tooltip.runtimeDataIndex[1];
            var serieData = serie.GetSerieData(dataIndex);
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    sb.Append(serieData.name);
                    for (int i = 0; i < radar.indicatorList.Count; i++)
                    {
                        string key = radar.indicatorList[i].name;
                        float value = serieData.GetData(i);
                        if ((i == 0 && !string.IsNullOrEmpty(serieData.name)) || i > 0) sb.Append("\n");
                        sb.AppendFormat("{0}: {1}", key, ChartCached.FloatToStr(value, 0, tooltip.forceENotation));
                    }
                    break;
                case RadarType.Single:
                    string key2 = serieData.name;
                    float value2 = serieData.GetData(1);
                    if (string.IsNullOrEmpty(key2))
                    {
                        key2 = radar.indicatorList[dataIndex].name;
                    }
                    sb.AppendFormat("{0}: {1}", key2, ChartCached.FloatToStr(value2, 0, tooltip.forceENotation));
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
            if (isCartesian)
            {
                var serieData = serie.GetSerieData(index, dataZoom);
                if (serieData != null && serieData.highlighted)
                {
                    sb.Append(key).Append(!string.IsNullOrEmpty(key) ? " : " : "");
                    sb.Append("[").Append(ChartCached.FloatToStr(xValue, 0, tooltip.forceENotation)).Append(",")
                        .Append(ChartCached.FloatToStr(yValue, 0, tooltip.forceENotation)).Append("]\n");
                }
            }
            else
            {
                var valueTxt = isIngore ? tooltip.ignoreDataDefaultContent :
                    ChartCached.FloatToStr(yValue, 0, tooltip.forceENotation);
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
                case SerieType.Scatter:
                case SerieType.EffectScatter:
                    InitCoordinateTooltip(ref sb, tooltip, serie, index, themeInfo, isCartesian, dataZoom);
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
                sb.Length = 0;
                for (int i = 0; i < series.Count; i++)
                {
                    var serie = series.GetSerie(i);
                    if (!serie.show) continue;
                    var serieData = serie.GetSerieData(dataIndex, dataZoom);
                    if (serieData == null) continue;
                    var itemFormatter = GetItemFormatter(tooltip, serie, serieData);
                    var percent = serieData.GetData(1) / serie.yTotal * 100;
                    needCategory = needCategory || (serie.type == SerieType.Line || serie.type == SerieType.Bar);
                    if (serie.show)
                    {
                        if (string.IsNullOrEmpty(itemFormatter))
                        {
                            if (!first) sb.Append("\n");
                            InitDefaultContent(ref sb, tooltip, serie, dataIndex, category, themeInfo, dataZoom, isCartesian);
                            first = false;
                            continue;
                        }
                        string content = itemFormatter;
                        content = content.Replace("{a}", serie.name);
                        content = content.Replace("{b}", needCategory ? category : serieData.name);
                        content = content.Replace("{c}", ChartCached.FloatToStr(serieData.GetData(1), 0, tooltip.forceENotation));
                        content = content.Replace("{d}", ChartCached.FloatToStr(percent, 1));
                        if (!first) sb.Append("\n");
                        sb.Append("<color=#").Append(themeInfo.GetColorStr(i)).Append(">● </color>");
                        sb.Append(content);
                        first = false;
                    }
                    if (formatTitle)
                    {
                        if (i == 0)
                        {
                            title = title.Replace("{a}", serie.name);
                            title = title.Replace("{b}", needCategory ? category : serieData.name);
                            title = title.Replace("{c}", ChartCached.FloatToStr(serieData.GetData(1), 0, tooltip.forceENotation));
                            title = title.Replace("{d}", ChartCached.FloatToStr(percent, 1));
                        }
                        title = title.Replace("{a" + i + "}", serie.name);
                        title = title.Replace("{b" + i + "}", needCategory ? category : serieData.name);
                        title = title.Replace("{c" + i + "}", ChartCached.FloatToStr(serieData.GetData(1), 0, tooltip.forceENotation));
                        title = title.Replace("{d" + i + "}", ChartCached.FloatToStr(percent, 1));
                    }
                }
                if (string.IsNullOrEmpty(title))
                {
                    if (needCategory) return category + "\n" + sb.ToString();
                    else return sb.ToString();
                }
                else
                {
                    title = title.Replace("\\n", "\n");
                    title = title.Replace("<br/>", "\n");
                    return title + "\n" + sb.ToString();
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
                        var percent = serieData.GetData(1) / serie.yTotal * 100;
                        if (i == 0)
                        {
                            content = content.Replace("{a}", serie.name);
                            content = content.Replace("{b}", needCategory ? category : serieData.name);
                            content = content.Replace("{c}", ChartCached.FloatToStr(serieData.GetData(1), 0, tooltip.forceENotation));
                            content = content.Replace("{d}", ChartCached.FloatToStr(percent, 1));
                        }
                        content = content.Replace("{a" + i + "}", serie.name);
                        content = content.Replace("{b" + i + "}", needCategory ? category : serieData.name);
                        content = content.Replace("{c" + i + "}", ChartCached.FloatToStr(serieData.GetData(1), 0, tooltip.forceENotation));
                        content = content.Replace("{d" + i + "}", ChartCached.FloatToStr(percent, 1));
                    }
                }
                content = content.Replace("\\n", "\n");
                content = content.Replace("<br/>", "\n");
                return content;
            }
        }

        private static string GetItemFormatter(Tooltip tooltip, Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.tooltipFormatter)) return itemStyle.tooltipFormatter;
            else return tooltip.itemFormatter;
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