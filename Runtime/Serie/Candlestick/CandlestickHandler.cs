using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class CandlestickHandler : SerieHandler<Candlestick>
    {
        private GridCoord m_SerieGrid;
        public override void DrawSerie(VertexHelper vh)
        {
            DrawCandlestickSerie(vh, serie);
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        {
            if (dataIndex < 0)
                dataIndex = serie.context.pointerItemDataIndex;

            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            title = category;

            var color = chart.GetMarkColor(serie, serieData);
            var newMarker = SerieHelper.GetItemMarker(serie, serieData, marker);
            var newItemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            var newNumericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            var isEmptyItemFormatter = string.IsNullOrEmpty(newItemFormatter);

            if (isEmptyItemFormatter)
            {
                var param = serie.context.param;
                param.serieName = serie.serieName;
                param.serieIndex = serie.index;
                param.category = category;
                param.dimension = 1;
                param.serieData = serieData;
                param.dataCount = serie.dataCount;
                param.value = 0;
                param.total = 0;
                param.color = color;
                param.marker = newMarker;
                param.itemFormatter = newItemFormatter;
                param.numericFormatter = newNumericFormatter;
                param.columns.Clear();

                param.columns.Add(param.marker);
                param.columns.Add(serie.serieName);
                param.columns.Add(string.Empty);

                paramList.Add(param);
                for (int i = 1; i < 5; i++)
                {
                    param = new SerieParams();
                    param.serieName = serie.serieName;
                    param.serieIndex = serie.index;
                    param.dimension = i;
                    param.serieData = serieData;
                    param.dataCount = serie.dataCount;
                    param.value = serieData.GetData(i);
                    param.total = SerieHelper.GetMaxData(serie, i);
                    param.color = color;
                    param.marker = newMarker;
                    param.itemFormatter = newItemFormatter;
                    param.numericFormatter = newNumericFormatter;
                    param.isSecondaryMark = true;
                    param.columns.Clear();

                    param.columns.Add(param.marker);
                    param.columns.Add(XCSettings.lang.GetCandlestickDimensionName(i - 1));
                    param.columns.Add(ChartCached.NumberToStr(param.value, param.numericFormatter));

                    paramList.Add(param);
                }
            }
            else
            {
                newItemFormatter = newItemFormatter.Replace("\\n", "\n");
                var temp = newItemFormatter.Split('\n');
                foreach (var str in temp)
                {
                    var param = new SerieParams();
                    param.serieName = serie.serieName;
                    param.serieIndex = serie.index;
                    param.category = category;
                    param.serieData = serieData;
                    param.dataCount = serie.dataCount;
                    param.value = 0;
                    param.total = 0;
                    param.color = color;
                    param.marker = newMarker;
                    param.itemFormatter = str;
                    param.numericFormatter = newNumericFormatter;
                    param.isSecondaryMark = false;
                    param.columns.Clear();
                    paramList.Add(param);
                }
            }
        }

        public override void UpdateSerieContext()
        {
            if (m_SerieGrid == null)
                return;

            var needCheck = (chart.isPointerInChart && m_SerieGrid.IsPointerEnter() && !serie.placeHolder) || m_LegendEnter;
            var needInteract = false;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    Color32 color1, toColor1;
                    foreach (var serieData in serie.data)
                    {
                        serieData.context.highlight = false;
                        var state = SerieHelper.GetSerieState(serie, serieData, true);
                        SerieHelper.GetItemColor(out color1, out toColor1, serie, serieData, chart.theme, state);
                        serieData.interact.SetColor(ref needInteract, color1, toColor1);
                    }
                    chart.RefreshPainter(serie);
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            Color32 color, toColor;
            if (m_LegendEnter)
            {
                serie.context.pointerEnter = true;
                foreach (var serieData in serie.data)
                {
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme);
                    serieData.interact.SetColor(ref needInteract, color, toColor);
                }
            }
            else
            {
                serie.context.pointerItemDataIndex = -1;
                serie.context.pointerEnter = false;
                foreach (var serieData in serie.data)
                {
                    if (serie.context.pointerAxisDataIndexs.Contains(serieData.index) ||
                        serieData.context.rect.Contains(chart.pointerPos))
                    {
                        serie.context.pointerItemDataIndex = serieData.index;
                        serie.context.pointerEnter = true;
                        serieData.context.highlight = true;
                    }
                    else
                    {
                        serieData.context.highlight = false;
                    }
                    var state = SerieHelper.GetSerieState(serie, serieData, true);
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, state);
                    serieData.interact.SetColor(ref needInteract, color, toColor);
                }
            }
            if (needInteract)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void DrawCandlestickSerie(VertexHelper vh, Candlestick serie)
        {
            if (!serie.show) return;
            if (serie.animation.HasFadeOut()) return;
            XAxis xAxis;
            YAxis yAxis;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex)) return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex)) return;
            if (!chart.TryGetChartComponent<GridCoord>(out m_SerieGrid, xAxis.gridIndex)) return;
            var theme = chart.theme;
            var dataZoom = chart.GetDataZoomOfAxis(xAxis);
            var showData = serie.GetDataList(dataZoom);
            float categoryWidth = AxisHelper.GetDataWidth(xAxis, m_SerieGrid.context.width, showData.Count, dataZoom);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float gap = (categoryWidth - barWidth) / 2;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow) :
                showData.Count;

            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetChangeDuration();
            var dataAddDuration = serie.animation.GetAdditionDuration();
            var unscaledTime = serie.animation.unscaledTime;
            double yMinValue = yAxis.context.minValue;
            double yMaxValue = yAxis.context.maxValue;
            var isYAxis = false;
            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;
            var intensive = m_SerieGrid.context.width / (maxCount - serie.minShow) < 0.6f;
            for (int i = serie.minShow; i < maxCount; i++)
            {
                var serieData = showData[i];
                if (!serieData.show || serie.IsIgnoreValue(serieData))
                {
                    serie.context.dataPoints.Add(Vector3.zero);
                    serie.context.dataIndexs.Add(serieData.index);
                    continue;
                }
                var state = SerieHelper.GetSerieState(serie, serieData);
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, state);
                var startDataIndex = serieData.data.Count > 4 ? 1 : 0;
                var open = serieData.GetCurrData(startDataIndex, dataAddDuration, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue, unscaledTime);
                var close = serieData.GetCurrData(startDataIndex + 1, dataAddDuration, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue, unscaledTime);
                var lowest = serieData.GetCurrData(startDataIndex + 2, dataAddDuration, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue, unscaledTime);
                var heighest = serieData.GetCurrData(startDataIndex + 3, dataAddDuration, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue, unscaledTime);
                var isRise = yAxis.inverse ? close < open : close > open;
                var borderWidth = open == 0 ? 0f :
                    (itemStyle.borderWidth == 0 ? theme.serie.candlestickBorderWidth :
                        itemStyle.borderWidth);
                if (serieData.IsDataChanged()) dataChanging = true;
                float pX = xAxis.IsCategory() ? m_SerieGrid.context.x + i * categoryWidth : AxisHelper.GetAxisValuePosition(m_SerieGrid, xAxis, categoryWidth, serieData.GetData(0));
                float zeroY = m_SerieGrid.context.y + yAxis.context.offset;
                if (!xAxis.boundaryGap) pX -= categoryWidth / 2;
                float pY = zeroY;
                var barHig = 0f;
                double valueTotal = yMaxValue - yMinValue;
                var minCut = yMinValue > 0 ? yMinValue : 0;
                if (valueTotal != 0)
                {
                    barHig = (float)((close - open) / valueTotal * m_SerieGrid.context.height);
                    pY += (float)((open - minCut) / valueTotal * m_SerieGrid.context.height);
                }
                serieData.context.stackHeight = barHig;
                float currHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, barHig);
                Vector3 plb, plt, prt, prb, top;

                var offset = 2 * borderWidth;
                if (isRise)
                {
                    plb = new Vector3(pX + gap + offset, pY + offset);
                    plt = new Vector3(pX + gap + offset, pY + currHig - offset);
                    prt = new Vector3(pX + gap + barWidth - offset, pY + currHig - offset);
                    prb = new Vector3(pX + gap + barWidth - offset, pY + offset);
                    top = new Vector3(pX + gap + barWidth / 2, pY + currHig - offset);
                }
                else
                {
                    plb = new Vector3(pX + gap + offset, pY - offset);
                    plt = new Vector3(pX + gap + offset, pY + currHig + offset);
                    prt = new Vector3(pX + gap + barWidth - offset, pY + currHig + offset);
                    prb = new Vector3(pX + gap + barWidth - offset, pY - offset);
                    top = new Vector3(pX + gap + barWidth / 2, pY + currHig + offset);
                }
                if (serie.clip)
                {
                    plb = chart.ClampInGrid(m_SerieGrid, plb);
                    plt = chart.ClampInGrid(m_SerieGrid, plt);
                    prt = chart.ClampInGrid(m_SerieGrid, prt);
                    prb = chart.ClampInGrid(m_SerieGrid, prb);
                    top = chart.ClampInGrid(m_SerieGrid, top);
                }
                serie.context.dataPoints.Add(top);
                serie.context.dataIndexs.Add(serieData.index);
                var areaColor = isRise ?
                    itemStyle.GetColor(theme.serie.candlestickColor) :
                    itemStyle.GetColor0(theme.serie.candlestickColor0);
                var borderColor = isRise ?
                    itemStyle.GetBorderColor(theme.serie.candlestickBorderColor) :
                    itemStyle.GetBorderColor0(theme.serie.candlestickBorderColor0);
                var itemWidth = Mathf.Abs(prt.x - plb.x);
                var itemHeight = Mathf.Abs(plt.y - prb.y);
                var center = new Vector3((plb.x + prt.x) / 2, (plt.y + prb.y) / 2);
                var lowPos = new Vector3(center.x, zeroY + (float)((lowest - minCut) / valueTotal * m_SerieGrid.context.height));
                var heighPos = new Vector3(center.x, zeroY + (float)((heighest - minCut) / valueTotal * m_SerieGrid.context.height));
                var openCenterPos = new Vector3(center.x, prb.y);
                var closeCenterPos = new Vector3(center.x, prt.y);

                var rectMinX = Mathf.Min(plb.x, prb.x, plt.x, prt.x);
                var rectMaxX = Mathf.Max(plb.x, prb.x, plt.x, prt.x);
                var rectMinY = Mathf.Min(plb.y, prb.y, plt.y, prt.y, lowPos.y, heighPos.y);
                var rectMaxY = Mathf.Max(plb.y, prb.y, plt.y, prt.y, lowPos.y, heighPos.y);
                serieData.context.rect = new Rect(rectMinX, rectMinY, rectMaxX - rectMinX, rectMaxY - rectMinY);
                UGL.DrawRectangle(vh, serieData.context.rect, Color.yellow);
                if (intensive)
                {
                    UGL.DrawLine(vh, lowPos, heighPos, borderWidth, borderColor);
                }
                else
                {
                    if (barWidth > 2f * borderWidth)
                    {
                        if (itemWidth > 0 && itemHeight > 0)
                        {
                            if (itemStyle.IsNeedCorner())
                            {
                                UGL.DrawRoundRectangle(vh, center, itemWidth, itemHeight, areaColor, areaColor, 0,
                                    itemStyle.cornerRadius, isYAxis, 0.5f);
                            }
                            else
                            {
                                chart.DrawClipPolygon(vh, ref prb, ref plb, ref plt, ref prt, areaColor, areaColor,
                                    serie.clip, m_SerieGrid);
                            }
                            UGL.DrawBorder(vh, center, itemWidth, itemHeight, 2 * borderWidth, borderColor, 0,
                                itemStyle.cornerRadius, isYAxis, 0.5f);
                        }
                    }
                    else
                    {
                        UGL.DrawLine(vh, openCenterPos, closeCenterPos, Mathf.Max(borderWidth, barWidth / 2), borderColor);
                    }
                    if (isRise)
                    {
                        UGL.DrawLine(vh, openCenterPos, lowPos, borderWidth, borderColor);
                        UGL.DrawLine(vh, closeCenterPos, heighPos, borderWidth, borderColor);
                    }
                    else
                    {
                        UGL.DrawLine(vh, closeCenterPos, lowPos, borderWidth, borderColor);
                        UGL.DrawLine(vh, openCenterPos, heighPos, borderWidth, borderColor);
                    }
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }
    }
}