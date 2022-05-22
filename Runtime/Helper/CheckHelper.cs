using System.Text;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class CheckHelper
    {
        private static bool IsColorAlphaZero(Color color)
        {
            return !ChartHelper.IsClearColor(color) && color.a == 0;
        }

        public static string CheckChart(BaseGraph chart)
        {
            if (chart is BaseChart) return CheckChart((BaseChart) chart);
            else return string.Empty;
        }

        public static string CheckChart(BaseChart chart)
        {
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
            var list = XChartsMgr.GetCharts(chart.chartName);
            if (list.Count > 1)
            {
                sb.AppendFormat("warning:chart name is repeated: {0}\n", chart.chartName);
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
            // foreach (var title in chart.titles)
            // {
            //     if (!title.show) return;
            //     if (string.IsNullOrEmpty(title.text)) sb.AppendFormat("warning:title{0}->text is null\n", title.index);
            //     if (IsColorAlphaZero(title.textStyle.color))
            //         sb.AppendFormat("warning:title{0}->textStyle->color alpha is 0\n", title.index);
            //     if (IsColorAlphaZero(title.subTextStyle.color))
            //         sb.AppendFormat("warning:title{0}->subTextStyle->color alpha is 0\n", title.index);
            // }
        }

        private static void CheckLegend(BaseChart chart, StringBuilder sb)
        { }

        private static void CheckGrid(BaseChart chart, StringBuilder sb)
        { }

        private static void CheckSerie(BaseChart chart, StringBuilder sb)
        {
            var allDataIsEmpty = true;
            var allDataIsZero = true;
            var allSerieIsHide = true;
            foreach (var serie in chart.series)
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
                if (serie is Line)
                {
                    if (serie.lineStyle.opacity == 0)
                        sb.AppendFormat("warning:serie {0} lineStyle->opacity is 0\n", serie.index);
                    if (IsColorAlphaZero(serie.lineStyle.color))
                        sb.AppendFormat("warning:serie {0} lineStyle->color alpha is 0\n", serie.index);
                }
                else if (serie is Pie)
                {
                    if (serie.radius.Length >= 2 && serie.radius[1] == 0)
                        sb.AppendFormat("warning:serie {0} radius[1] is 0\n", serie.index);
                }
                else if (serie is Scatter || serie is EffectScatter)
                {
                    if (!serie.symbol.show)
                        sb.AppendFormat("warning:serie {0} symbol type is None\n", serie.index);
                }
            }
            if (allDataIsEmpty) sb.Append("warning:all serie data is empty\n");
            if (!allDataIsEmpty && allDataIsZero) sb.Append("warning:all serie data is 0\n");
            if (allSerieIsHide) sb.Append("warning:all serie is hide\n");
        }
    }
}