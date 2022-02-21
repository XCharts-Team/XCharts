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
    public static class CheckHelper
    {
        private static bool IsColorAlphaZero(Color color)
        {
            return !ChartHelper.IsClearColor(color) && color.a == 0;
        }

        public static string CheckChart(BaseGraph chart)
        {
            if (chart == null)
                return string.Empty;
            if (chart is BaseChart)
                return CheckChart((BaseChart)chart);
            else
                return string.Empty;
        }

        public static string CheckChart(BaseChart chart)
        {
            if (chart == null)
                return string.Empty;
            var sb = ChartHelper.sb;
            sb.Length = 0;
            CheckName(chart, sb);
            CheckSize(chart, sb);
            CheckTheme(chart, sb);
            CheckTitle(chart, sb);
            CheckLegend(chart, sb);
            CheckGrid(chart, sb);
            CheckSerie(chart, sb);
            return sb.ToString();
        }

        private static void CheckName(BaseChart chart, StringBuilder sb)
        {
            if (string.IsNullOrEmpty(chart.chartName)) return;
            if (XChartsMgr.Instance.IsRepeatChartName(chart))
            {
                var info = XChartsMgr.Instance.GetRepeatChartNameInfo(chart, chart.chartName);
                sb.AppendFormat("warning:chart name is repeated: {0}\n{1}", chart.chartName, info);
            }
        }

        private static void CheckSize(BaseChart chart, StringBuilder sb)
        {
            if (chart.chartWidth == 0 || chart.chartHeight == 0)
            {
                sb.Append("warning:chart width or height is 0\n");
            }
        }

        private static void CheckTheme(BaseChart chart, StringBuilder sb)
        {
            var theme = chart.theme;
            theme.CheckWarning(sb);
        }

        private static void CheckTitle(BaseChart chart, StringBuilder sb)
        {
            foreach (var title in chart.titles)
            {
                if (!title.show) return;
                if (string.IsNullOrEmpty(title.text)) sb.AppendFormat("warning:title{0}->text is null\n", title.index);
                if (IsColorAlphaZero(title.textStyle.color))
                    sb.AppendFormat("warning:title{0}->textStyle->color alpha is 0\n", title.index);
                if (IsColorAlphaZero(title.subTextStyle.color))
                    sb.AppendFormat("warning:title{0}->subTextStyle->color alpha is 0\n", title.index);
            }
        }

        private static void CheckLegend(BaseChart chart, StringBuilder sb)
        {
            foreach (var legend in chart.legends)
            {
                if (!legend.show) return;
                if (IsColorAlphaZero(legend.textStyle.color))
                    sb.AppendFormat("warning:legend{0}->textStyle->color alpha is 0\n", legend.index);
                var serieNameList = SeriesHelper.GetLegalSerieNameList(chart.series);
                if (serieNameList.Count == 0)
                    sb.AppendFormat("warning:legend{0} need serie.name or serieData.name not empty\n", legend.index);
                foreach (var category in legend.data)
                {
                    if (!serieNameList.Contains(category))
                    {
                        sb.AppendFormat("warning:legend{0} [{1}] is invalid, must be one of serie.name or serieData.name\n",
                            legend.index, category);
                    }
                }
            }
        }

        private static void CheckGrid(BaseChart chart, StringBuilder sb)
        {
            if (chart is CoordinateChart)
            {
                foreach (var grid in (chart as CoordinateChart).grids)
                {
                    if (grid.left >= chart.chartWidth)
                        sb.Append("warning:grid->left > chartWidth\n");
                    if (grid.right >= chart.chartWidth)
                        sb.Append("warning:grid->right > chartWidth\n");
                    if (grid.top >= chart.chartHeight)
                        sb.Append("warning:grid->top > chartHeight\n");
                    if (grid.bottom >= chart.chartHeight)
                        sb.Append("warning:grid->bottom > chartHeight\n");
                    if (grid.left + grid.right >= chart.chartWidth)
                        sb.Append("warning:grid.left + grid.right > chartWidth\n");
                    if (grid.top + grid.bottom >= chart.chartHeight)
                        sb.Append("warning:grid.top + grid.bottom > chartHeight\n");
                }
            }
        }

        private static void CheckSerie(BaseChart chart, StringBuilder sb)
        {
            var allDataIsEmpty = true;
            var allDataIsZero = true;
            var allSerieIsHide = true;
            foreach (var serie in chart.series.list)
            {
                if (serie.show) allSerieIsHide = false;
                if (serie.dataCount > 0)
                {
                    allDataIsEmpty = false;
                    for (int i = 0; i < serie.dataCount; i++)
                    {
                        var serieData = serie.GetSerieData(i);
                        for (int j = 1; j < serieData.data.Count; j++)
                        {
                            if (serieData.GetData(j) != 0)
                            {
                                allDataIsZero = false;
                                break;
                            }
                        }
                    }
                    var dataCount = serie.GetSerieData(0).data.Count;
                    if (serie.showDataDimension > 1 && serie.showDataDimension != dataCount)
                    {
                        sb.AppendFormat("warning:serie {0} serieData.data.count[{1}] not match showDataDimension[{2}]\n", serie.index, dataCount, serie.showDataDimension);
                    }
                }
                else
                {
                    sb.AppendFormat("warning:serie {0} no data\n", serie.index);
                }
                if (IsColorAlphaZero(serie.itemStyle.color))
                    sb.AppendFormat("warning:serie {0} itemStyle->color alpha is 0\n", serie.index);
                if (serie.itemStyle.opacity == 0)
                    sb.AppendFormat("warning:serie {0} itemStyle->opacity is 0\n", serie.index);
                if (serie.itemStyle.borderWidth != 0 && IsColorAlphaZero(serie.itemStyle.borderColor))
                    sb.AppendFormat("warning:serie {0} itemStyle->borderColor alpha is 0\n", serie.index);
                switch (serie.type)
                {
                    case SerieType.Line:
                        if (serie.lineStyle.opacity == 0)
                            sb.AppendFormat("warning:serie {0} lineStyle->opacity is 0\n", serie.index);
                        if (IsColorAlphaZero(serie.lineStyle.color))
                            sb.AppendFormat("warning:serie {0} lineStyle->color alpha is 0\n", serie.index);
                        break;
                    case SerieType.Bar:
                        if (serie.barWidth == 0)
                            sb.AppendFormat("warning:serie {0} barWidth is 0\n", serie.index);
                        break;
                    case SerieType.Pie:
                        if (serie.radius.Length >= 2 && serie.radius[1] == 0)
                            sb.AppendFormat("warning:serie {0} radius[1] is 0\n", serie.index);
                        break;
                    case SerieType.Scatter:
                    case SerieType.EffectScatter:
                        if (!serie.symbol.show)
                            sb.AppendFormat("warning:serie {0} symbol type is None\n", serie.index);
                        break;
                }
            }
            if (allDataIsEmpty) sb.Append("warning:all serie data is empty\n");
            if (!allDataIsEmpty && allDataIsZero) sb.Append("warning:all serie data is 0\n");
            if (allSerieIsHide) sb.Append("warning:all serie is hide\n");
        }
    }
}