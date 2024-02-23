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
            base.Update();
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
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
            param.color = chart.GetMarkColor(serie, serieData);
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

        public override void UpdateSerieContext()
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
            var needInteract = false;
            for (int i = serie.dataCount - 1; i >= 0; i--)
            {
                var serieData = serie.data[i];
                var symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize);
                if (m_LegendEnter ||
                    (!needHideAll && Vector3.Distance(serieData.context.position, chart.pointerPos) <= symbolSize))
                {
                    serie.context.pointerItemDataIndex = i;
                    serie.context.pointerEnter = true;
                    serieData.context.highlight = true;
                }
                else
                {
                    serieData.context.highlight = false;
                }
                var state = SerieHelper.GetSerieState(serie, serieData, true);
                symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, themeSymbolSize, state);
                serieData.interact.SetValue(ref needInteract, symbolSize);
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
            var dataChangeDuration = serie.animation.GetChangeDuration();
            var interactDuration = serie.animation.GetInteractionDuration();
            var isFadeOut = serie.animation.IsFadeOut();
            var unscaledTime = serie.animation.unscaledTime;
            var dataChanging = false;
            var interacting = false;
            var dataList = serie.GetDataList(xDataZoom);
            var isEffectScatter = serie is EffectScatter;
            var colorIndex = serie.context.colorIndex;

            serie.containerIndex = m_Grid.index;
            serie.containterInstanceId = m_Grid.instanceId;

            float symbolBorder = 0f;
            float[] cornerRadius = null;
            Color32 color, toColor, emptyColor, borderColor;
            foreach (var serieData in dataList)
            {
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount))
                    continue;

                var state = SerieHelper.GetSerieState(serie, serieData, true);

                SerieHelper.GetItemColor(out color, out toColor, out emptyColor, serie, serieData, chart.theme, colorIndex, state);
                SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, serieData, chart.theme, state);
                double xValue = serieData.GetCurrData(0, 0, isFadeOut ? 0 : dataChangeDuration, unscaledTime, xAxis.inverse);
                double yValue = serieData.GetCurrData(1, 0, isFadeOut ? 0 : dataChangeDuration, unscaledTime, yAxis.inverse);

                if (serieData.IsDataChanged())
                    dataChanging = true;

                float xDataHig = GetDataHig(xAxis, xValue, m_Grid.context.width);
                float yDataHig = GetDataHig(yAxis, yValue, m_Grid.context.height);
                var pos = new Vector3(m_Grid.context.x + xDataHig, m_Grid.context.y + yDataHig);

                if (!m_Grid.Contains(pos))
                    continue;

                serie.context.dataPoints.Add(pos);
                serie.context.dataIndexs.Add(serieData.index);
                serieData.context.position = pos;
                var datas = serieData.data;
                var symbolSize = 0f;
                if (isFadeOut || !serieData.interact.TryGetValue(ref symbolSize, ref interacting, interactDuration))
                {
                    symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme.serie.scatterSymbolSize, state);
                    if (!isFadeOut)
                    {
                        serieData.interact.SetValue(ref interacting, symbolSize, true);
                        serieData.interact.TryGetValue(ref symbolSize, ref interacting, interactDuration);
                    }
                }
                symbolSize *= rate;

                if (isEffectScatter)
                {
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (byte)(255 * (symbolSize - nowSize) / symbolSize);
                        chart.DrawSymbol(vh, symbol.type, nowSize, symbolBorder, pos,
                            color, toColor, emptyColor, borderColor, symbol.gap, cornerRadius);
                    }
                    chart.RefreshPainter(serie);
                }
                else
                {
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
            var dataChangeDuration = serie.animation.GetChangeDuration();
            var unscaledTime = serie.animation.unscaledTime;
            var dataChanging = false;
            var dataList = serie.GetDataList(xDataZoom);
            var isEffectScatter = serie is EffectScatter;
            var colorIndex = serie.context.colorIndex;

            serie.containerIndex = axis.index;
            serie.containterInstanceId = axis.instanceId;

            float symbolBorder = 0f;
            float[] cornerRadius = null;
            Color32 color, toColor, emptyColor, borderColor;
            foreach (var serieData in dataList)
            {
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                if (!symbol.ShowSymbol(serieData.index, maxCount))
                    continue;

                var state = SerieHelper.GetSerieState(serie, serieData, true);
                SerieHelper.GetItemColor(out color, out toColor, out emptyColor, serie, serieData, chart.theme, colorIndex, state);
                SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, serieData, chart.theme, state);

                if (serieData.IsDataChanged())
                    dataChanging = true;

                var pos = Vector3.zero;
                var xValue = serieData.GetCurrData(0, 0, dataChangeDuration, unscaledTime, axis.inverse);

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
                serie.context.dataIndexs.Add(serieData.index);
                serieData.context.position = pos;

                var datas = serieData.data;
                var symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme.serie.scatterSymbolSize, state);
                symbolSize *= rate;

                if (isEffectScatter)
                {
                    if (symbolSize > 100) symbolSize = 100;
                    for (int count = 0; count < symbol.animationSize.Count; count++)
                    {
                        var nowSize = symbol.animationSize[count];
                        color.a = (byte)(255 * (symbolSize - nowSize) / symbolSize);
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
                var minIndex = axis.GetLogMinIndex();
                var nowIndex = axis.GetLogValue(value);
                return (float)((nowIndex - minIndex) / axis.splitNumber * totalWidth);
            }
            else if (axis.IsCategory())
            {
                if (axis.boundaryGap)
                {
                    float tick = (float)(totalWidth / (axis.context.minMaxRange + 1));
                    return tick / 2 + (float)(value - axis.context.minValue) * tick;
                }
                else
                {
                    return (float)((value - axis.context.minValue) / axis.context.minMaxRange * totalWidth);
                }
            }
            else
            {
                return (float)((value - axis.context.minValue) / axis.context.minMaxRange * totalWidth);
            }
        }
    }
}