
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
                SeriesHelper.UpdateSerieNameList(chart, ref chart.m_LegendRealShowName);
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
                    string legendName = legend.GetFormatterContent(datas[i]);
                    var readIndex = chart.m_LegendRealShowName.IndexOf(datas[i]);
                    var active = chart.IsActiveByLegend(datas[i]);
                    var bgColor = LegendHelper.GetIconColor(chart, legend, readIndex, datas[i], active);
                    var item = LegendHelper.AddLegendItem(legend, i, datas[i], legendObject.transform, chart.theme,
                        legendName, bgColor, active, readIndex);
                    legend.SetButton(legendName, item, totalLegend);
                    ChartHelper.ClearEventListener(item.button.gameObject);
                    ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerDown, (data) =>
                    {
                        if (data.selectedObject == null || legend.selectedMode == Legend.SelectedMode.None) return;
                        var temp = data.selectedObject.name.Split('_');
                        string selectedName = temp[1];
                        int clickedIndex = int.Parse(temp[0]);
                        if (legend.selectedMode == Legend.SelectedMode.Multiple)
                        {
                            chart.OnLegendButtonClick(clickedIndex, selectedName, !chart.IsActiveByLegend(selectedName));
                        }
                        else
                        {
                            var btnList = legend.context.buttonList.Values.ToArray();
                            if (btnList.Length == 1)
                            {
                                chart.OnLegendButtonClick(0, selectedName, !chart.IsActiveByLegend(selectedName));
                            }
                            else
                            {
                                for (int n = 0; n < btnList.Length; n++)
                                {
                                    temp = btnList[n].name.Split('_');
                                    selectedName = btnList[n].legendName;
                                    var index = btnList[n].index;
                                    chart.OnLegendButtonClick(n, selectedName, index == clickedIndex ? true : false);
                                }
                            }
                        }
                    });
                    ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerEnter, (data) =>
                    {
                        if (item.button == null) return;
                        var temp = item.button.name.Split('_');
                        string selectedName = temp[1];
                        int index = int.Parse(temp[0]);
                        chart.OnLegendButtonEnter(index, selectedName);
                    });
                    ChartHelper.AddEventListener(item.button.gameObject, EventTriggerType.PointerExit, (data) =>
                    {
                        if (item.button == null) return;
                        var temp = item.button.name.Split('_');
                        string selectedName = temp[1];
                        int index = int.Parse(temp[0]);
                        chart.OnLegendButtonExit(index, selectedName);
                    });
                }
                LegendHelper.ResetItemPosition(legend, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            };
            legend.refreshComponent();
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
                    if (serie != null && serie is Line)
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
                }
            }
        }
    }
}