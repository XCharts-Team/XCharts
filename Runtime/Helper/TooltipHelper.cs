/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Text;
using UnityEngine;

namespace XCharts
{
    public static class TooltipHelper
    {
        private static void InitScatterTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ChartTheme theme)
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
                double xValue, yValue;
                serie.GetXYData(dataIndex, null, out xValue, out yValue);

                sb.Append("<color=#").Append(theme.GetColorStr(serie.index)).Append(">● </color>");
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
            ChartTheme theme)
        {
            if (tooltip.runtimeDataIndex[serie.index] < 0) return;
            string key = serie.data[index].name;
            var serieData = serie.GetSerieData(index);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);

            var value = serieData.GetData(1);
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serie.name))
            {
                sb.Append(serie.name).Append(FormatterHelper.PH_NN);
            }
            sb.Append("<color=#").Append(theme.GetColorStr(index)).Append(">● </color>");
            if (!string.IsNullOrEmpty(key))
                sb.Append(key).Append(": ");
            sb.Append(ChartCached.FloatToStr(value, numericFormatter));
        }

        private static void InitRingTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ChartTheme theme)
        {
            var serieData = serie.GetSerieData(index);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
            var value = serieData.GetFirstData();
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serieData.name))
            {
                sb.Append("<color=#").Append(theme.GetColorStr(index)).Append(">● </color>")
                    .Append(serieData.name).Append(": ").Append(ChartCached.FloatToStr(value, numericFormatter));
            }
            else
            {
                sb.Append(ChartCached.FloatToStr(value, numericFormatter));
            }
        }
        private static void InitGaugeTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ChartTheme theme)
        {
            if (tooltip.runtimeGridIndex >= 0) return;
            if (serie.index != index || serie.type != SerieType.Gauge) return;
            var serieData = serie.GetSerieData(0);
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
            var value = serieData.data[1];
            sb.Length = 0;
            if (!string.IsNullOrEmpty(serie.name))
            {
                sb.Append(serie.name).Append("\n");
            }
            if (!string.IsNullOrEmpty(serieData.name))
            {
                //sb.Append("<color=#").Append(theme.GetColorStr(index)).Append(">● </color>")
                sb.Append(serieData.name).Append(": ").Append(ChartCached.FloatToStr(value, numericFormatter));
            }
            else
            {
                sb.Append(ChartCached.FloatToStr(value, numericFormatter));
            }
        }

        public static void InitRadarTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, Radar radar,
            ChartTheme theme)
        {
            if (radar == null) return;
            if (!serie.show) return;
            if (tooltip.runtimeGridIndex >= 0) return;
            if (serie.radarIndex != radar.index) return;
            var dataIndex = tooltip.runtimeDataIndex[1];
            var serieData = serie.GetSerieData(dataIndex);
            if (!serieData.show) return;
            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    if (radar.isAxisTooltip)
                    {
                        var dimension = tooltip.runtimeDataIndex[2];
                        if (!string.IsNullOrEmpty(serie.name))
                            sb.Append(serie.name).Append("\n");
                        var total = serie.GetDataTotal(dimension);
                        var first = true;
                        for (int i = 0; i < serie.dataCount; i++)
                        {
                            var sd = serie.GetSerieData(i);
                            if (!sd.show) continue;
                            var key = sd.name;
                            var value = sd.GetData(dimension);
                            var itemFormatter = GetItemFormatter(tooltip, serie, sd);
                            numericFormatter = GetItemNumericFormatter(tooltip, serie, sd);
                            if (!first) sb.Append("\n");
                            first = false;
                            sb.Append("<color=#").Append(theme.GetColorStr(i)).Append(">● </color>");
                            if (string.IsNullOrEmpty(itemFormatter))
                            {
                                if (string.IsNullOrEmpty(key)) key = radar.indicatorList[dataIndex].name;
                                if (string.IsNullOrEmpty(key))
                                    sb.AppendFormat("{0}\n", ChartCached.FloatToStr(value, numericFormatter));
                                else
                                    sb.AppendFormat("{0}: {1}\n", key, ChartCached.FloatToStr(value, numericFormatter));
                            }
                            else
                            {
                                string content = itemFormatter;
                                FormatterHelper.ReplaceSerieLabelContent(ref content, numericFormatter, value, total, serie.name,
                                    sd.name, theme.GetColor(i));
                                sb.Append(content);
                            }
                        }
                    }
                    else
                    {
                        if (serie.index != tooltip.runtimeDataIndex[0]) return;
                        sb.Append(serieData.name);
                        for (int i = 0; i < radar.indicatorList.Count; i++)
                        {
                            var key = radar.indicatorList[i].name;
                            var value = serieData.GetData(i);
                            if ((i == 0 && !string.IsNullOrEmpty(serieData.name)) || i > 0) sb.Append(FormatterHelper.PH_NN);
                            sb.AppendFormat("{0}: {1}", key, ChartCached.FloatToStr(value, numericFormatter));
                        }
                    }
                    break;
                case RadarType.Single:
                    var key2 = serieData.name;
                    var value2 = serieData.GetData(1);
                    var total2 = serie.GetDataTotal(1);
                    var itemFormatter2 = GetItemFormatter(tooltip, serie, serieData);
                    if (string.IsNullOrEmpty(itemFormatter2))
                    {
                        if (string.IsNullOrEmpty(key2))
                        {
                            key2 = radar.indicatorList[dataIndex].name;
                        }
                        sb.AppendFormat("{0}: {1}", key2, ChartCached.FloatToStr(value2, numericFormatter));
                    }
                    else
                    {
                        string content = itemFormatter2;
                        FormatterHelper.ReplaceSerieLabelContent(ref content, numericFormatter, value2, total2, serie.name,
                            serieData.name, theme.GetColor(serie.index));
                        sb.Append(content);
                    }
                    break;
            }
        }

        public static void InitCoordinateTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            ChartTheme theme, bool isCartesian, DataZoom dataZoom = null)
        {
            string key = serie.name;
            double xValue, yValue;
            serie.GetXYData(index, dataZoom, out xValue, out yValue);
            var isIngore = serie.IsIgnorePoint(index);
            if (isIngore) return;
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
            else if (!isIngore || (isIngore && tooltip.ignoreDataShow))
            {
                var valueTxt = isIngore ? tooltip.ignoreDataDefaultContent :
                    ChartCached.FloatToStr(yValue, numericFormatter);
                sb.Append("<color=#").Append(theme.GetColorStr(serie.index)).Append(">● </color>");
                if (serie.type == SerieType.Candlestick)
                {
                    sb.Append(key).Append(FormatterHelper.PH_NN);
                    var data = serieData.data;
                    var open = ChartCached.FloatToStr(data[0], numericFormatter);
                    var close = ChartCached.FloatToStr(data[1], numericFormatter);
                    var lowest = ChartCached.FloatToStr(data[2], numericFormatter);
                    var heighest = ChartCached.FloatToStr(data[3], numericFormatter);
                    sb.Append("   open: ").Append(open).Append(FormatterHelper.PH_NN);
                    sb.Append("   close: ").Append(close).Append(FormatterHelper.PH_NN);
                    sb.Append("   lowest: ").Append(lowest).Append(FormatterHelper.PH_NN);
                    sb.Append("   heighest: ").Append(heighest);
                }
                else
                {
                    sb.Append(key).Append(!string.IsNullOrEmpty(key) ? " : " : "");
                    sb.Append(valueTxt);
                }
            }
        }

        private static void InitHeatmapTooltip(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            CoordinateChart chart)
        {
            if (serie.type != SerieType.Heatmap) return;
            var xData = tooltip.runtimeXValues[0];
            var yData = tooltip.runtimeYValues[0];
            if (chart.IsCategory() && (xData < 0 || yData < 0)) return;
            sb.Length = 0;
            var xAxis = chart.GetXAxis(serie.xAxisIndex);
            var yAxis = chart.GetYAxis(serie.yAxisIndex);
            var xCount = xAxis.data.Count;
            var yCount = yAxis.data.Count;
            var visualMap = chart.visualMap;
            if (chart.IsCategory())
            {
                string key = serie.name;
                var serieData = serie.data[(int) xData * yCount + (int) yData];
                var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
                var value = serieData.data[2];
                var color = visualMap.enable ? visualMap.GetColor(value) :
                    chart.theme.GetColor(serie.index);
                sb.Append("\n")
                    .Append(key).Append(!string.IsNullOrEmpty(key) ? "\n" : "")
                    .Append("<color=#").Append(ChartCached.ColorToStr(color)).Append(">● </color>")
                    .Append(xAxis.data[(int) xData]).Append(": ")
                    .Append(ChartCached.FloatToStr(value, numericFormatter));
            }
        }

        private static void InitDefaultContent(ref StringBuilder sb, Tooltip tooltip, Serie serie, int index,
            BaseChart chart, DataZoom dataZoom = null, bool isCartesian = false,
            Radar radar = null)
        {
            switch (serie.type)
            {
                case SerieType.Line:
                case SerieType.Bar:
                case SerieType.Candlestick:
                    InitCoordinateTooltip(ref sb, tooltip, serie, index, chart.theme, isCartesian, dataZoom);
                    break;
                case SerieType.Scatter:
                case SerieType.EffectScatter:
                    InitScatterTooltip(ref sb, tooltip, serie, index, chart.theme);
                    break;
                case SerieType.Radar:
                    InitRadarTooltip(ref sb, tooltip, serie, radar, chart.theme);
                    break;
                case SerieType.Pie:
                    InitPieTooltip(ref sb, tooltip, serie, index, chart.theme);
                    break;
                case SerieType.Ring:
                    InitRingTooltip(ref sb, tooltip, serie, index, chart.theme);
                    break;
                case SerieType.Heatmap:
                    InitHeatmapTooltip(ref sb, tooltip, serie, index, chart as CoordinateChart);
                    break;
                case SerieType.Gauge:
                    InitGaugeTooltip(ref sb, tooltip, serie, index, chart.theme);
                    break;
                case SerieType.Custom:
                    chart.InitCustomSerieTooltip(ref sb, serie, index);
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

        public static string GetPolarFormatterContent(Tooltip tooltip, BaseChart chart, AngleAxis angleAxis)
        {
            if (string.IsNullOrEmpty(tooltip.formatter))
            {
                var sb = ChartHelper.sb;
                sb.Length = 0;
                var title = tooltip.titleFormatter;
                var formatTitle = !string.IsNullOrEmpty(title);
                if ("{i}".Equals(tooltip.titleFormatter))
                {
                    title = string.Empty;
                    formatTitle = false;
                }
                else if (string.IsNullOrEmpty(title))
                {
                    var angle = angleAxis.clockwise ? tooltip.runtimeAngle : 360 - tooltip.runtimeAngle;
                    title = ChartCached.FloatToStr(angle);
                }
                foreach (var serie in chart.series.list)
                {
                    if (serie.show && IsSelectedSerie(tooltip, serie.index))
                    {
                        if (formatTitle)
                        {
                            FormatterHelper.ReplaceContent(ref title, 0, tooltip.numericFormatter, serie, chart, null);
                        }
                        var dataIndexList = tooltip.runtimeSerieIndex[serie.index];

                        for (int i = 0; i < dataIndexList.Count; i++)
                        {
                            var dataIndex = dataIndexList[i];
                            var serieData = serie.GetSerieData(dataIndex);
                            var itemFormatter = GetItemFormatter(tooltip, serie, serieData);
                            var numericFormatter = GetItemNumericFormatter(tooltip, serie, serieData);
                            double xValue, yValue;
                            serie.GetXYData(dataIndex, null, out xValue, out yValue);
                            if (string.IsNullOrEmpty(itemFormatter))
                            {
                                sb.Append("<color=#").Append(chart.theme.GetColorStr(serie.index)).Append(">● </color>");
                                if (!string.IsNullOrEmpty(serie.name))
                                    sb.Append(serie.name).Append(": ");
                                sb.AppendFormat("{0}", ChartCached.FloatToStr(xValue, numericFormatter));
                                if (i != dataIndexList.Count - 1)
                                {
                                    sb.Append(FormatterHelper.PH_NN);
                                }
                            }
                            else
                            {
                                string content = itemFormatter;
                                FormatterHelper.ReplaceContent(ref content, dataIndex, tooltip.numericFormatter, serie, chart, null);
                                var dotColorIndex = serie.type == SerieType.Pie || serie.type == SerieType.Radar ||
                                    serie.type == SerieType.Ring ? dataIndex : serie.index;
                                sb.Append(ChartCached.ColorToDotStr(chart.theme.GetColor(dotColorIndex)));
                                sb.Append(content);
                            }
                        }
                        sb.Append(FormatterHelper.PH_NN);
                    }
                }
                if (string.IsNullOrEmpty(title))
                {
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
                FormatterHelper.ReplaceContent(ref content, 0, tooltip.numericFormatter, null, chart, null);
                return content;
            }
        }

        public static string GetFormatterContent(Tooltip tooltip, int dataIndex, BaseChart chart, DataZoom dataZoom = null,
            bool isCartesian = false, Radar radar = null)
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
                for (int i = 0; i < chart.series.Count; i++)
                {
                    var serie = chart.series.GetSerie(i);
                    if (tooltip.runtimeGridIndex >= 0 && serie.runtimeGridIndex != tooltip.runtimeGridIndex) continue;
                    if (serie.type == SerieType.Scatter || serie.type == SerieType.EffectScatter)
                    {
                        if (serie.show && IsSelectedSerie(tooltip, serie.index))
                        {
                            isScatter = true;
                            var itemFormatter = GetItemFormatter(tooltip, serie, null);
                            if (string.IsNullOrEmpty(itemFormatter))
                            {
                                if (!first) sb.Append(FormatterHelper.PH_NN);
                                InitDefaultContent(ref sb, tooltip, serie, dataIndex, chart, dataZoom, isCartesian, radar);
                                first = false;
                                continue;
                            }
                            var itemTitle = title;
                            if (!string.IsNullOrEmpty(itemTitle))
                            {
                                FormatterHelper.ReplaceContent(ref itemTitle, dataIndex, tooltip.numericFormatter, serie, chart, dataZoom);
                                sb.Append(itemTitle).Append(FormatterHelper.PH_NN);
                            }
                            var dataIndexList = tooltip.runtimeSerieIndex[serie.index];
                            foreach (var tempIndex in dataIndexList)
                            {
                                string content = itemFormatter;
                                var foundDot = FormatterHelper.ReplaceContent(ref content, tempIndex, tooltip.numericFormatter, serie, chart, dataZoom);
                                if (!foundDot)
                                {
                                    sb.Append(ChartCached.ColorToDotStr(chart.theme.GetColor(serie.index)));
                                }
                                sb.Append(content).Append(FormatterHelper.PH_NN);
                            }
                        }
                    }
                    else if (IsNeedTooltipSerie(serie, tooltip))
                    {
                        var itemFormatter = string.Empty;
                        if (serie.type == SerieType.Gauge)
                        {
                            var serieData = serie.GetSerieData(0, dataZoom);
                            if (serieData == null) continue;
                            itemFormatter = GetItemFormatter(tooltip, serie, serieData);
                        }
                        else
                        {
                            var serieData = serie.GetSerieData(dataIndex, dataZoom);
                            if (serieData == null) continue;
                            itemFormatter = GetItemFormatter(tooltip, serie, serieData);
                        }
                        needCategory = needCategory || (serie.type == SerieType.Line || serie.type == SerieType.Bar);
                        if (formatTitle)
                        {
                            FormatterHelper.ReplaceContent(ref title, dataIndex, tooltip.numericFormatter, serie, chart, dataZoom);
                        }
                        if (serie.show)
                        {
                            if (string.IsNullOrEmpty(itemFormatter) || serie.type == SerieType.Radar)
                            {
                                if (!first) sb.Append(FormatterHelper.PH_NN);
                                InitDefaultContent(ref sb, tooltip, serie, dataIndex, chart, dataZoom, isCartesian, radar);
                                first = false;
                                continue;
                            }
                            string content = itemFormatter;
                            FormatterHelper.ReplaceContent(ref content, dataIndex, tooltip.numericFormatter, serie, chart, dataZoom);
                            if (!first) sb.Append(FormatterHelper.PH_NN);
                            var dotColorIndex = serie.type == SerieType.Pie || serie.type == SerieType.Radar ||
                                serie.type == SerieType.Ring ? dataIndex : i;
                            sb.Append(ChartCached.ColorToDotStr(chart.theme.GetColor(dotColorIndex)));
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
                    {
                        var category = (chart as CoordinateChart).GetTooltipCategory(dataIndex, dataZoom);
                        return category + FormatterHelper.PH_NN + FormatterHelper.TrimAndReplaceLine(sb);
                    }
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
                FormatterHelper.ReplaceContent(ref content, dataIndex, tooltip.numericFormatter, null, chart, dataZoom);
                return content;
            }
        }

        public static bool IsNeedTooltipSerie(Serie serie, Tooltip tooltip)
        {
            //if (serie.type == SerieType.Pie || serie.type == SerieType.Radar || serie.type == SerieType.Ring)
            if (serie.type == SerieType.Pie || serie.type == SerieType.Ring)
            {
                if (serie.index < tooltip.runtimeDataIndex.Count)
                    return tooltip.runtimeDataIndex[serie.index] >= 0;
                else
                    return false;
            }
            else if (serie.type == SerieType.Gauge)
            {
                return serie.index == tooltip.runtimeDataIndex[0];
            }
            else
            {
                return true;
            }
        }

        public static bool IsSelectedSerie(Tooltip tooltip, int serieIndex)
        {
            if (tooltip.runtimeSerieIndex.ContainsKey(serieIndex))
            {
                return tooltip.runtimeSerieIndex[serieIndex].Count > 0;
            }
            return false;
        }

        public static string GetItemFormatter(Tooltip tooltip, Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.tooltipFormatter)) return itemStyle.tooltipFormatter;
            else return tooltip.itemFormatter;
        }

        public static string GetItemNumericFormatter(Tooltip tooltip, Serie serie, SerieData serieData)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!string.IsNullOrEmpty(itemStyle.numericFormatter)) return itemStyle.numericFormatter;
            else return tooltip.numericFormatter;
        }

        public static Color32 GetLineColor(Tooltip tooltip, ChartTheme theme)
        {
            var lineStyle = tooltip.lineStyle;
            if (!ChartHelper.IsClearColor(lineStyle.color))
            {
                return lineStyle.GetColor();
            }
            else
            {
                var color = theme.tooltip.lineColor;
                ChartHelper.SetColorOpacity(ref color, lineStyle.opacity);
                return color;
            }
        }

        public static Color GetTexColor(Tooltip tooltip, ComponentTheme theme)
        {
            if (!ChartHelper.IsClearColor(tooltip.textStyle.color))
            {
                return tooltip.textStyle.color;
            }
            else
            {
                return theme.textColor;
            }
        }

        public static Color GetTexBackgroundColor(Tooltip tooltip, ComponentTheme theme)
        {
            if (!ChartHelper.IsClearColor(tooltip.textStyle.backgroundColor))
            {
                return tooltip.textStyle.backgroundColor;
            }
            else
            {
                return theme.textBackgroundColor;
            }
        }
    }
}