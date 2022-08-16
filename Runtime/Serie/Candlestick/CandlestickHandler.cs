using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class CandlestickHandler : SerieHandler<Candlestick>
    {
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

            var color = chart.GetItemColor(serie, serieData);
            var newMarker = SerieHelper.GetItemMarker(serie, serieData, marker);
            var newItemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            var newNumericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);

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
            for (int i = 0; i < 4; i++)
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
                param.columns.Clear();

                param.columns.Add(param.marker);
                param.columns.Add(XCSettings.lang.GetCandlestickDimensionName(i));
                param.columns.Add(ChartCached.NumberToStr(param.value, param.numericFormatter));

                paramList.Add(param);
            }
        }

        private void DrawCandlestickSerie(VertexHelper vh, Candlestick serie)
        {
            if (!serie.show) return;
            if (serie.animation.HasFadeOut()) return;
            XAxis xAxis;
            YAxis yAxis;
            GridCoord grid;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex)) return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex)) return;
            if (!chart.TryGetChartComponent<GridCoord>(out grid, xAxis.gridIndex)) return;
            var theme = chart.theme;
            var dataZoom = chart.GetDataZoomOfAxis(xAxis);
            var showData = serie.GetDataList(dataZoom);
            float categoryWidth = AxisHelper.GetDataWidth(xAxis, grid.context.width, showData.Count, dataZoom);
            float barWidth = serie.GetBarWidth(categoryWidth);
            float gap = (categoryWidth - barWidth) / 2;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > showData.Count ? showData.Count : serie.maxShow) :
                showData.Count;

            bool dataChanging = false;
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            double yMinValue = yAxis.context.minValue;
            double yMaxValue = yAxis.context.maxValue;
            var isYAxis = false;
            serie.containerIndex = grid.index;
            serie.containterInstanceId = grid.instanceId;
            for (int i = serie.minShow; i < maxCount; i++)
            {
                var serieData = showData[i];
                if (serie.IsIgnoreValue(serieData))
                {
                    serie.context.dataPoints.Add(Vector3.zero);
                    serie.context.dataIndexs.Add(serieData.index);
                    continue;
                }
                var state = SerieHelper.GetSerieState(serie, serieData);
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, state);
                var open = serieData.GetCurrData(0, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var close = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var lowest = serieData.GetCurrData(2, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var heighest = serieData.GetCurrData(3, dataChangeDuration, yAxis.inverse, yMinValue, yMaxValue);
                var isRise = yAxis.inverse ? close<open : close> open;
                var borderWidth = open == 0 ? 0f :
                    (itemStyle.runtimeBorderWidth == 0 ? theme.serie.candlestickBorderWidth :
                        itemStyle.runtimeBorderWidth);
                if (serieData.IsDataChanged()) dataChanging = true;
                float pX = grid.context.x + i * categoryWidth;
                float zeroY = grid.context.y + yAxis.context.offset;
                if (!xAxis.boundaryGap) pX -= categoryWidth / 2;
                float pY = zeroY;
                var barHig = 0f;
                double valueTotal = yMaxValue - yMinValue;
                var minCut = (yMinValue > 0 ? yMinValue : 0);
                if (valueTotal != 0)
                {
                    barHig = (float) ((close - open) / valueTotal * grid.context.height);
                    pY += (float) ((open - minCut) / valueTotal * grid.context.height);
                }
                serieData.context.stackHeight = barHig;
                float currHig = AnimationStyleHelper.CheckDataAnimation(chart, serie, i, barHig);
                Vector3 plb, plt, prt, prb, top;

                plb = new Vector3(pX + gap + borderWidth, pY + borderWidth);
                plt = new Vector3(pX + gap + borderWidth, pY + currHig - borderWidth);
                prt = new Vector3(pX + gap + barWidth - borderWidth, pY + currHig - borderWidth);
                prb = new Vector3(pX + gap + barWidth - borderWidth, pY + borderWidth);
                top = new Vector3(pX + gap + barWidth / 2, pY + currHig - borderWidth);
                if (serie.clip)
                {
                    plb = chart.ClampInGrid(grid, plb);
                    plt = chart.ClampInGrid(grid, plt);
                    prt = chart.ClampInGrid(grid, prt);
                    prb = chart.ClampInGrid(grid, prb);
                    top = chart.ClampInGrid(grid, top);
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
                var lowPos = new Vector3(center.x, zeroY + (float) ((lowest - minCut) / valueTotal * grid.context.height));
                var heighPos = new Vector3(center.x, zeroY + (float) ((heighest - minCut) / valueTotal * grid.context.height));
                var openCenterPos = new Vector3(center.x, prb.y);
                var closeCenterPos = new Vector3(center.x, prt.y);
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
                                serie.clip, grid);
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