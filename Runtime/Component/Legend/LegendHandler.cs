using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class LegendHandler : MainComponentHandler<Legend>
    {
        private static readonly string s_LegendObjectName = "legend";
        private static readonly char[] s_NameSplit = new char[] { '_' };

        public override void InitComponent()
        {
            InitLegend(component);
        }

        public override void CheckComponent(System.Text.StringBuilder sb)
        {
            var legend = component;
            if (ChartHelper.IsColorAlphaZero(legend.labelStyle.textStyle.color))
                sb.AppendFormat("warning:legend{0}->textStyle->color alpha is 0\n", legend.index);
            var serieNameList = SeriesHelper.GetLegalSerieNameList(chart.series);
            if (serieNameList.Count == 0)
                sb.AppendFormat("warning:legend{0} need serie.serieName or serieData.name not empty\n", legend.index);
            foreach (var category in legend.data)
            {
                if (!serieNameList.Contains(category))
                {
                    sb.AppendFormat("warning:legend{0} [{1}] is invalid, must be one of serie.serieName or serieData.name\n",
                        legend.index, category);
                }
            }
        }
        public override void DrawTop(VertexHelper vh)
        {
            DrawLegend(vh);
        }

        public override void OnSerieDataUpdate(int serieIndex)
        {
#pragma warning disable 0618
            if (FormatterHelper.NeedFormat(component.formatter) || FormatterHelper.NeedFormat(component.labelStyle.formatter))
                component.refreshComponent();
#pragma warning restore 0618
        }

        private void InitLegend(Legend legend)
        {
            legend.painter = null;
            legend.refreshComponent = delegate ()
            {
                legend.OnChanged();
                var legendObject = ChartHelper.AddObject(s_LegendObjectName + legend.index, chart.transform, chart.chartMinAnchor,
                    chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
                legend.gameObject = legendObject;
                legendObject.hideFlags = chart.chartHideFlags;
                //ChartHelper.DestoryGameObjectByMatch(legendObject.transform, "_");
                SeriesHelper.UpdateSerieNameList(chart, ref chart.m_LegendRealShowName);
                legend.context.background = ChartHelper.AddIcon("background", legendObject.transform, 0, 0);
                legend.context.background.transform.SetSiblingIndex(0);
                ChartHelper.SetBackground(legend.context.background, legend.background);
                List<string> datas;
                if (legend.show && legend.data.Count > 0)
                {
                    datas = new List<string>();
                    foreach (var data in legend.data)
                    {
                        if (chart.m_LegendRealShowName.Contains(data) || chart.IsSerieName(data))
                            datas.Add(data);
                    }
                }
                else
                {
                    datas = chart.m_LegendRealShowName;
                }
                int totalLegend = 0;
                for (int i = 0; i < datas.Count; i++)
                {
                    if (!SeriesHelper.IsLegalLegendName(datas[i])) continue;
                    totalLegend++;
                }
                legend.RemoveButton();
                ChartHelper.HideAllObject(legendObject);
                if (!legend.show) return;
                for (int i = 0; i < datas.Count; i++)
                {
                    if (!SeriesHelper.IsLegalLegendName(datas[i])) continue;
                    string legendName = datas[i];
                    var legendContent = GetFormatterContent(legend, i, datas[i]);
                    var readIndex = chart.m_LegendRealShowName.IndexOf(datas[i]);
                    var active = chart.IsActiveByLegend(datas[i]);
                    var bgColor = LegendHelper.GetIconColor(chart, legend, readIndex, datas[i], active);
                    bgColor.a = legend.itemOpacity;
                    var item = LegendHelper.AddLegendItem(chart, legend, i, datas[i], legendObject.transform, chart.theme,
                        legendContent, bgColor, active, readIndex);
                    legend.SetButton(legendName, item, totalLegend);
                    ChartHelper.ClearEventListener(item.button.gameObject);
                    ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerDown, (data) =>
                    {
                        if (data.selectedObject == null || legend.selectedMode == Legend.SelectedMode.None) return;
                        var temp = data.selectedObject.name.Split(s_NameSplit, 2);
                        string selectedName = temp[1];
                        int clickedIndex = int.Parse(temp[0]);
                        if (legend.selectedMode == Legend.SelectedMode.Multiple)
                        {
                            OnLegendButtonClick(legend, clickedIndex, selectedName, !chart.IsActiveByLegend(selectedName));
                        }
                        else
                        {
                            var btnList = legend.context.buttonList.Values.ToArray();
                            if (btnList.Length == 1)
                            {
                                OnLegendButtonClick(legend, 0, selectedName, !chart.IsActiveByLegend(selectedName));
                            }
                            else
                            {
                                for (int n = 0; n < btnList.Length; n++)
                                {
                                    temp = btnList[n].name.Split(s_NameSplit, 2);
                                    selectedName = btnList[n].legendName;
                                    var index = btnList[n].index;
                                    OnLegendButtonClick(legend, n, selectedName, index == clickedIndex ? true : false);
                                }
                            }
                        }
                    });
                    ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerEnter, (data) =>
                    {
                        if (item.button == null) return;
                        var temp = item.button.name.Split(s_NameSplit, 2);
                        string selectedName = temp[1];
                        int index = int.Parse(temp[0]);
                        OnLegendButtonEnter(legend, index, selectedName);
                    });
                    ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerExit, (data) =>
                    {
                        if (item.button == null) return;
                        var temp = item.button.name.Split(s_NameSplit, 2);
                        string selectedName = temp[1];
                        int index = int.Parse(temp[0]);
                        OnLegendButtonExit(legend, index, selectedName);
                    });
                }
                LegendHelper.ResetItemPosition(legend, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            };
            legend.refreshComponent();
        }

        private string GetFormatterContent(Legend legend, int dataIndex, string category)
        {
#pragma warning disable 0618
            if (string.IsNullOrEmpty(legend.formatter) || string.IsNullOrEmpty(legend.labelStyle.formatter))
                return category;
            else
            {
                var formatter = string.IsNullOrEmpty(legend.labelStyle.formatter) ? legend.formatter : legend.labelStyle.formatter;
                var content = formatter.Replace("{name}", category);
                content = content.Replace("{value}", category);
                var serie = chart.GetSerie(0);
                FormatterHelper.ReplaceContent(ref content, dataIndex, legend.labelStyle.numericFormatter, serie, chart, category);
                return content;
            }
#pragma warning restore 0618
        }

        private void OnLegendButtonClick(Legend legend, int index, string legendName, bool show)
        {
            chart.OnLegendButtonClick(index, legendName, show);
            if (chart.onLegendClick != null)
                chart.onLegendClick(legend, index, legendName, show);
        }

        private void OnLegendButtonEnter(Legend legend, int index, string legendName)
        {
            chart.OnLegendButtonEnter(index, legendName);
            if (chart.onLegendEnter != null)
                chart.onLegendEnter(legend, index, legendName);
        }

        private void OnLegendButtonExit(Legend legend, int index, string legendName)
        {
            chart.OnLegendButtonExit(index, legendName);
            if (chart.onLegendExit != null)
                chart.onLegendExit(legend, index, legendName);
        }

        private void DrawLegend(VertexHelper vh)
        {
            if (chart.series.Count == 0) return;
            var legend = component;
            if (!legend.show) return;
            if (legend.iconType == Legend.Type.Custom) return;
            foreach (var kv in legend.context.buttonList)
            {
                var item = kv.Value;
                var rect = item.GetIconRect();
                var radius = Mathf.Min(rect.width, rect.height) / 2;
                var color = item.GetIconColor();
                var iconType = legend.iconType;
                if (legend.iconType == Legend.Type.Auto)
                {
                    var serie = chart.GetSerie(item.legendName);
                    if (serie != null)
                    {
                        if (serie is Line || serie is SimplifiedLine)
                        {
                            var sp = new Vector3(rect.center.x - rect.width / 2, rect.center.y);
                            var ep = new Vector3(rect.center.x + rect.width / 2, rect.center.y);
                            UGL.DrawLine(vh, sp, ep, chart.settings.legendIconLineWidth, color);
                            if (!serie.symbol.show) continue;
                            switch (serie.symbol.type)
                            {
                                case SymbolType.None:
                                    continue;
                                case SymbolType.Circle:
                                    iconType = Legend.Type.Circle;
                                    break;
                                case SymbolType.Diamond:
                                    iconType = Legend.Type.Diamond;
                                    break;
                                case SymbolType.EmptyCircle:
                                    iconType = Legend.Type.EmptyCircle;
                                    break;
                                case SymbolType.Rect:
                                    iconType = Legend.Type.Rect;
                                    break;
                                case SymbolType.Triangle:
                                    iconType = Legend.Type.Triangle;
                                    break;
                            }
                        }
                        else
                        {
                            iconType = Legend.Type.Rect;
                        }
                    }
                    else
                    {
                        iconType = Legend.Type.Rect;
                    }
                }
                switch (iconType)
                {
                    case Legend.Type.Rect:
                        var cornerRadius = chart.settings.legendIconCornerRadius;
                        UGL.DrawRoundRectangle(vh, rect.center, rect.width, rect.height, color, color,
                            0, cornerRadius, false, 0.5f);
                        break;
                    case Legend.Type.Circle:
                        UGL.DrawCricle(vh, rect.center, radius, color);
                        break;
                    case Legend.Type.Diamond:
                        UGL.DrawDiamond(vh, rect.center, radius, color);
                        break;
                    case Legend.Type.EmptyCircle:
                        var backgroundColor = chart.GetChartBackgroundColor();
                        UGL.DrawEmptyCricle(vh, rect.center, radius, 2 * chart.settings.legendIconLineWidth,
                            color, color, backgroundColor, 1f);
                        break;
                    case Legend.Type.Triangle:
                        UGL.DrawTriangle(vh, rect.center, 1.2f * radius, color);
                        break;
                    case Legend.Type.Candlestick:
                        UGL.DrawRoundRectangle(vh, rect.center, rect.width / 2, rect.height / 2, color, color,
                            0, null, false, 0.5f);
                        UGL.DrawLine(vh, new Vector3(rect.center.x, rect.center.y - rect.height / 2),
                            new Vector3(rect.center.x, rect.center.y + rect.height / 2), 1, color);
                        break;
                }
            }
        }
    }
}
