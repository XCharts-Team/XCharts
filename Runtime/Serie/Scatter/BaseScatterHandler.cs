using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal class BaseScatterHandler<T> : SerieHandler<T> where T : BaseScatter
    {
        private GridCoord m_Grid;

        public override void Update()
        {
            UpdateSerieContext();
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter,
            ref List<SerieParams> paramList, ref string title)
        {
            dataIndex = serie.context.pointerItemDataIndex;
            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            title = serie.serieName;

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.category = category;
            param.dimension = 1;
            param.dataCount = serie.dataCount;
            param.serieData = serieData;
            param.color = SerieHelper.GetItemColor(serie, serieData, chart.theme, serie.context.colorIndex, false);
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            param.columns.Clear();

            param.columns.Add(param.marker);
            if (!string.IsNullOrEmpty(serieData.name))
                param.columns.Add(serieData.name);
            param.columns.Add(ChartCached.NumberToStr(serieData.GetData(1), param.numericFormatter));

            paramList.Add(param);
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (serie.IsUseCoord<SingleAxisCoord>())
            {
                DrawSingAxisScatterSerie(vh, serie);
            }
            else if (serie.IsUseCoord<GridCoord>())
            {
                DrawScatterSerie(vh, serie);
            }
        }

        private void UpdateSerieContext()
        {
            var needCheck = m_LegendEnter || (chart.isPointerInChart && (m_Grid == null || m_Grid.IsPointerEnter()));

            var needHideAll = false;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag == needCheck)
                    return;
                needHideAll = true;
            }
            m_LastCheckContextFlag = needCheck;
            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = false;
            var themeSymbolSize = chart.theme.serie.scatterSymbolSize;
            var themeSymbolSelectedSize = chart.theme.serie.scatterSymbolSelectedSize;
            var needInteract = false;
            for (int i = serie.dataCount - 1; i >= 0; i--)
            {
                var serieData = serie.data[i];
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                var symbolSize = symbol.GetSize(serieData.data, themeSymbolSize);
                var symbolSelectedSize = symbol.GetSelectedSize(serieData.data, themeSymbolSelectedSize);
                if (m_LegendEnter ||
                    (!needHideAll && Vector3.Distance(serieData.context.position, chart.pointerPos) <= symbolSize))
                {
                    serie.context.pointerItemDataIndex = i;
                    serie.context.pointerEnter = true;
                    serieData.context.highlight = true;
                    serieData.interact.SetValue(ref needInteract, symbolSelectedSize);
                }
                else
                {
                    serieData.context.highlight = false;
                    serieData.interact.SetValue(ref needInteract, symbolSize);
                }
            }
            if (needInteract)
            {
                chart.RefreshPainter(serie);
            }
        }

        protected virtual void DrawScatterSerie(VertexHelper vh, BaseScatter serie)
        {
            if (serie.animation.HasFadeOut())
                return;

            if (!serie.show)
                return;

            XAxis xAxis;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex))
                return;

            YAxis yAxis;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex))
                return;

            if (!chart.TryGetChartComponent<GridCoord>(out m_Grid, xAxis.gridIndex))
                return;

            DataZoom xDataZoom;
            DataZoom yDataZoom;
            chart.GetDataZoomOfSerie(serie, out xDataZoom, out yDataZoom);

            var theme = chart.theme;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > serie.dataCount ? serie.dataCount : serie.maxShow) :
                serie.dataCount;
            serie.animation.InitProgress(0, 1);
            var rate = serie.animation.GetCurrRate();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            var interacting = false;
            var dataList = serie.GetDataList(xDataZoom);
            var isEffectScatter = serie is EffectScatter;
            var colorIndex = serie.context.colorIndex;

            serie.containerIndex = m_Grid.index;
            serie.containterInstanceId = m_Grid.instanceId;

            foreach (var serieData in dataList)
            {
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount))
                    continue;

                var highlight = serie.highlight || serieData.context.highlight;
                var color = SerieHelper.GetItemColor(serie, serieData, theme, colorIndex, highlight);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, theme, colorIndex, highlight);
                var emptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, theme, colorIndex, highlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, theme, highlight);
                var borderColor = SerieHelper.GetSymbolBorderColor(serie, serieData, theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                double xValue = serieData.GetCurrData(0, dataChangeDuration, xAxis.inverse);
                double yValue = serieData.GetCurrData(1, dataChangeDuration, yAxis.inverse);

                if (serieData.IsDataChanged())
                    dataChanging = true;

                float pX = m_Grid.context.x + xAxis.axisLine.GetWidth(theme.axis.lineWidth);
                float pY = m_Grid.context.y + yAxis.axisLine.GetWidth(theme.axis.lineWidth);
                float xDataHig = GetDataHig(xAxis, xValue, m_Grid.context.width);
                float yDataHig = GetDataHig(yAxis, yValue, m_Grid.context.height);
                var pos = new Vector3(pX + xDataHig, pY + yDataHig);

                if (!m_Grid.Contains(pos))
                    continue;

                serie.context.dataPoints.Add(pos);
                serieData.context.position = pos;
                var datas = serieData.data;
                var symbolSize = serie.highlight || serieData.context.highlight ?
                    theme.serie.scatterSymbolSelectedSize :
                    theme.serie.scatterSymbolSize;
                if (!serieData.interact.TryGetValue(ref symbolSize, ref interacting))
                {
                    symbolSize = highlight ?
                        symbol.GetSelectedSize(serieData.data, symbolSize) :
                        symbol.GetSize(serieData.data, symbolSize);
                    serieData.interact.SetValue(ref interacting, symbolSize);
                }

                symbolSize *= rate;

                if (isEffectScatter)
                {
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (byte) (255 * (symbolSize - nowSize) / symbolSize);
                        chart.DrawSymbol(vh, symbol.type, nowSize, symbolBorder, pos,
                            color, toColor, emptyColor, borderColor, symbol.gap, cornerRadius);
                    }
                    chart.RefreshPainter(serie);
                }
                else
                {
                    if (symbolSize > 100) symbolSize = 100;
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos,
                        color, toColor, emptyColor, borderColor, symbol.gap, cornerRadius);
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(1);
                chart.RefreshPainter(serie);
            }
            if (dataChanging || interacting)
            {
                chart.RefreshPainter(serie);
            }
        }

        protected virtual void DrawSingAxisScatterSerie(VertexHelper vh, BaseScatter serie)
        {
            if (serie.animation.HasFadeOut())
                return;

            if (!serie.show)
                return;

            var axis = chart.GetChartComponent<SingleAxis>(serie.singleAxisIndex);
            if (axis == null)
                return;

            DataZoom xDataZoom;
            DataZoom yDataZoom;
            chart.GetDataZoomOfSerie(serie, out xDataZoom, out yDataZoom);

            var theme = chart.theme;
            int maxCount = serie.maxShow > 0 ?
                (serie.maxShow > serie.dataCount ? serie.dataCount : serie.maxShow) :
                serie.dataCount;
            serie.animation.InitProgress(0, 1);

            var rate = serie.animation.GetCurrRate();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var dataChanging = false;
            var dataList = serie.GetDataList(xDataZoom);
            var isEffectScatter = serie is EffectScatter;
            var colorIndex = serie.context.colorIndex;

            serie.containerIndex = axis.index;
            serie.containterInstanceId = axis.instanceId;

            foreach (var serieData in dataList)
            {
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount))
                    continue;

                var highlight = serie.highlight || serieData.context.highlight;
                var color = SerieHelper.GetItemColor(serie, serieData, theme, colorIndex, highlight);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, theme, colorIndex, highlight);
                var emptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, theme, colorIndex, highlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, theme, highlight);
                var borderColor = SerieHelper.GetSymbolBorderColor(serie, serieData, theme, highlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, highlight);
                var xValue = serieData.GetCurrData(0, dataChangeDuration, axis.inverse);

                if (serieData.IsDataChanged())
                    dataChanging = true;

                var pos = Vector3.zero;
                if (axis.orient == Orient.Horizonal)
                {
                    var xDataHig = GetDataHig(axis, xValue, axis.context.width);
                    var yDataHig = axis.context.height / 2;
                    pos = new Vector3(axis.context.x + xDataHig, axis.context.y + yDataHig);
                }
                else
                {
                    var yDataHig = GetDataHig(axis, xValue, axis.context.width);
                    var xDataHig = axis.context.height / 2;
                    pos = new Vector3(axis.context.x + xDataHig, axis.context.y + yDataHig);
                }
                serie.context.dataPoints.Add(pos);
                serieData.context.position = pos;

                var datas = serieData.data;
                var symbolSize = 0f;
                if (serie.highlight || serieData.context.highlight)
                    symbolSize = symbol.GetSelectedSize(datas, theme.serie.scatterSymbolSelectedSize);
                else
                    symbolSize = symbol.GetSize(datas, theme.serie.scatterSymbolSize);
                symbolSize *= rate;

                if (isEffectScatter)
                {
                    if (symbolSize > 100) symbolSize = 100;
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (byte) (255 * (symbolSize - nowSize) / symbolSize);
                        chart.DrawSymbol(vh, symbol.type, nowSize, symbolBorder, pos,
                            color, toColor, emptyColor, borderColor, symbol.gap, cornerRadius);
                    }
                    chart.RefreshPainter(serie);
                }
                else
                {
                    if (symbolSize > 100) symbolSize = 100;
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos,
                        color, toColor, emptyColor, borderColor, symbol.gap, cornerRadius);
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(1);
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }

        private static float GetDataHig(Axis axis, double value, float totalWidth)
        {
            if (axis.IsLog())
            {
                int minIndex = axis.GetLogMinIndex();
                float nowIndex = axis.GetLogValue(value);
                return (nowIndex - minIndex) / axis.splitNumber * totalWidth;
            }
            else if (axis.IsCategory())
            {
                if (axis.boundaryGap)
                {
                    float tick = (float) (totalWidth / (axis.context.minMaxRange + 1));
                    return tick / 2 + (float) (value - axis.context.minValue) * tick;
                }
                else
                {
                    return (float) ((value - axis.context.minValue) / axis.context.minMaxRange * totalWidth);
                }
            }
            else
            {
                return (float) ((value - axis.context.minValue) / axis.context.minMaxRange * totalWidth);
            }
        }
    }
}