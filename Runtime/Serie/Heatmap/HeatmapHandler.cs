using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed partial class HeatmapHandler : SerieHandler<Heatmap>
    {
        private GridCoord m_SerieGrid;
        private Dictionary<int, int> m_CountDict = new Dictionary<int, int>();

        public override int defaultDimension { get { return 2; } }

        public static int GetGridKey(int x, int y)
        {
            return x * 100000 + y;
        }

        public static void GetGridXYByKey(int key, out int x, out int y)
        {
            x = key / 100000;
            y = key % 100000;
        }

        public override void Update()
        {
            base.Update();
            if (serie.IsUseCoord<GridCoord>())
                UpdateSerieContext();
            else if (serie.IsUseCoord<PolarCoord>())
                UpdateSeriePolarContext();
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (serie.heatmapType == HeatmapType.Count)
                DrawCountHeatmapSerie(vh, serie);
            else
            {
                if (serie.IsUseCoord<PolarCoord>())
                {
                    DrawPolarHeatmap(vh, serie);
                }
                else if (serie.IsUseCoord<GridCoord>())
                {
                    DrawDataHeatmapSerie(vh, serie);
                }
            }
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        {
            dataIndex = serie.context.pointerItemDataIndex;
            if (serie.heatmapType == HeatmapType.Count)
            {
                int value;
                if (!m_CountDict.TryGetValue(dataIndex, out value)) return;
                var visualMap = chart.GetVisualMapOfSerie(serie);
                var dimension = VisualMapHelper.GetDimension(visualMap, defaultDimension);

                title = serie.serieName;

                var param = serie.context.param;
                param.serieName = serie.serieName;
                param.serieIndex = serie.index;
                param.dimension = dimension;
                param.dataCount = serie.dataCount;
                param.serieData = null;
                param.color = visualMap.GetColor(value);
                param.marker = SerieHelper.GetItemMarker(serie, null, marker);
                param.itemFormatter = SerieHelper.GetItemFormatter(serie, null, itemFormatter);
                param.numericFormatter = SerieHelper.GetNumericFormatter(serie, null, numericFormatter);
                param.columns.Clear();

                param.columns.Add(param.marker);
                param.columns.Add("count");
                param.columns.Add(ChartCached.NumberToStr(value, param.numericFormatter));

                paramList.Add(param);
            }
            else
            {
                if (dataIndex < 0)
                    return;

                var serieData = serie.GetSerieData(dataIndex);
                if (serieData == null)
                    return;
                var visualMap = chart.GetVisualMapOfSerie(serie);
                var dimension = VisualMapHelper.GetDimension(visualMap, defaultDimension);

                if (string.IsNullOrEmpty(category))
                {
                    var xAxis = chart.GetChartComponent<XAxis>(serie.xAxisIndex);
                    if (xAxis != null)
                        category = xAxis.GetData((int) serieData.GetData(0));
                }
                title = serie.serieName;

                var param = serie.context.param;
                param.serieName = serie.serieName;
                param.serieIndex = serie.index;
                param.dimension = dimension;
                param.dataCount = serie.dataCount;
                param.serieData = serieData;
                param.color = serieData.context.color;
                param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
                param.itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
                param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
                param.columns.Clear();

                param.columns.Add(param.marker);
                param.columns.Add(category);
                param.columns.Add(ChartCached.NumberToStr(serieData.GetData(dimension), param.numericFormatter));

                paramList.Add(param);
            }
        }

        private void UpdateSerieContext()
        {
            if (m_SerieGrid == null)
                return;

            var needCheck = (chart.isPointerInChart && m_SerieGrid.IsPointerEnter()) || m_LegendEnter;
            var needInteract = false;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    foreach (var serieData in serie.data)
                    {
                        serieData.context.highlight = false;
                    }
                    chart.RefreshPainter(serie);
                }
                return;
            }
            if (serie.heatmapType == HeatmapType.Count)
                return;
            m_LastCheckContextFlag = needCheck;
            if (m_LegendEnter)
            {
                serie.context.pointerEnter = true;
                foreach (var serieData in serie.data)
                {
                    serieData.context.highlight = true;
                }
            }
            else
            {
                serie.context.pointerItemDataIndex = -1;
                serie.context.pointerEnter = false;
                foreach (var serieData in serie.data)
                {
                    if (!needInteract && serieData.context.rect.Contains(chart.pointerPos))
                    {
                        serie.context.pointerItemDataIndex = serieData.index;
                        serie.context.pointerEnter = true;
                        serieData.context.highlight = true;
                        needInteract = true;
                    }
                    else
                    {
                        serieData.context.highlight = false;
                    }
                }
            }
            if (needInteract)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void DrawDataHeatmapSerie(VertexHelper vh, Heatmap serie)
        {
            if (!serie.show || serie.animation.HasFadeOut()) return;
            XAxis xAxis;
            YAxis yAxis;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex)) return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex)) return;
            m_SerieGrid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
            xAxis.boundaryGap = true;
            yAxis.boundaryGap = true;
            var visualMap = chart.GetVisualMapOfSerie(serie);
            var emphasisStyle = serie.emphasisStyle;
            var xCount = AxisHelper.GetTotalSplitGridNum(xAxis);
            var yCount = AxisHelper.GetTotalSplitGridNum(yAxis);
            var xWidth = m_SerieGrid.context.width / xCount;
            var yWidth = m_SerieGrid.context.height / yCount;

            var zeroX = m_SerieGrid.context.x;
            var zeroY = m_SerieGrid.context.y;
            var borderWidth = serie.itemStyle.show ? serie.itemStyle.borderWidth : 0;
            var splitWid = xWidth - 2 * borderWidth;
            var splitHig = yWidth - 2 * borderWidth;
            var defaultSymbolSize = Mathf.Min(splitWid, splitHig) * 0.25f;

            serie.animation.InitProgress(0, xCount);
            var animationIndex = serie.animation.GetCurrIndex();
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var unscaledTime = serie.animation.unscaledTime;
            var dataChanging = false;
            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;

            var dimension = VisualMapHelper.GetDimension(visualMap, defaultDimension);
            if (visualMap.autoMinMax)
            {
                double maxValue, minValue;
                SerieHelper.GetMinMaxData(serie, dimension, out minValue, out maxValue);
                VisualMapHelper.SetMinMax(visualMap, minValue, maxValue);
            }
            var rangeMin = visualMap.rangeMin;
            var rangeMax = visualMap.rangeMax;
            var color = chart.theme.GetColor(serie.index);
            float symbolBorder = 0f;
            float[] cornerRadius = null;
            Color32 borderColor;
            for (int n = 0; n < serie.dataCount; n++)
            {
                var serieData = serie.data[n];
                var xValue = serieData.GetData(0);
                var yValue = serieData.GetData(1);
                var i = AxisHelper.GetAxisValueSplitIndex(xAxis, xValue, xCount);
                var j = AxisHelper.GetAxisValueSplitIndex(yAxis, yValue, yCount);

                if (serie.IsIgnoreValue(serieData, dimension))
                {
                    serie.context.dataPoints.Add(Vector3.zero);
                    serie.context.dataIndexs.Add(serieData.index);
                    continue;
                }
                var state = SerieHelper.GetSerieState(serie, serieData, true);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData, state);
                var isRectSymbol = symbol.type == SymbolType.Rect;
                SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, serieData, chart.theme, state);
                var value = serieData.GetCurrData(dimension, dataChangeDuration, yAxis.inverse,
                    yAxis.context.minValue, yAxis.context.maxValue, unscaledTime);
                if (serieData.IsDataChanged()) dataChanging = true;
                var pos = new Vector3(zeroX + (i + 0.5f) * xWidth,
                    zeroY + (j + 0.5f) * yWidth);
                serie.context.dataPoints.Add(pos);
                serie.context.dataIndexs.Add(serieData.index);
                serieData.context.position = pos;
                serieData.context.canShowLabel = false;

                if ((value < rangeMin && rangeMin != visualMap.min) ||
                    (value > rangeMax && rangeMax != visualMap.max))
                {
                    continue;
                }
                if (!visualMap.IsInSelectedValue(value)) continue;
                if (animationIndex >= 0 && i > animationIndex) continue;
                color = visualMap.GetColor(value);
                if (serieData.context.highlight)
                    color = ChartHelper.GetHighlightColor(color);

                serieData.context.canShowLabel = true;
                serieData.context.color = color;

                var highlight = (serieData.context.highlight) ||
                    visualMap.context.pointerIndex > 0;
                var rectWid = 0f;
                var rectHig = 0f;
                if (isRectSymbol)
                {
                    if (symbol.size == 0 && symbol.sizeType == SymbolSizeType.Custom)
                    {
                        rectWid = splitWid;
                        rectHig = splitHig;
                    }
                    else
                    {
                        var symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme, defaultSymbolSize, state);
                        rectWid = symbolSize;
                        rectHig = symbolSize;
                    }
                    serieData.context.rect = new Rect(pos.x - rectWid / 2, pos.y - rectHig / 2, rectWid, rectHig);
                    UGL.DrawRectangle(vh, serieData.context.rect, color);

                    if (borderWidth > 0 && !ChartHelper.IsClearColor(borderColor))
                    {
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, borderWidth, borderColor, borderColor);
                    }
                }
                else
                {
                    var symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme, defaultSymbolSize, state);
                    var emptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serie.context.colorIndex, state);
                    serieData.context.rect = new Rect(pos.x - symbolSize / 2, pos.y - symbolSize / 2, symbolSize, symbolSize);
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos,
                        color, color, emptyColor, borderColor, symbol.gap, cornerRadius);
                }

                if (visualMap.hoverLink && highlight && emphasisStyle != null &&
                    emphasisStyle.itemStyle.borderWidth > 0)
                {
                    var emphasisItemStyle = emphasisStyle.itemStyle;
                    var emphasisBorderWidth = emphasisItemStyle.borderWidth;
                    var emphasisBorderColor = emphasisItemStyle.opacity > 0 ?
                        emphasisItemStyle.borderColor : ChartConst.clearColor32;
                    var emphasisBorderToColor = emphasisItemStyle.opacity > 0 ?
                        emphasisItemStyle.borderToColor : ChartConst.clearColor32;
                    UGL.DrawBorder(vh, pos, rectWid, rectHig, emphasisBorderWidth, emphasisBorderColor,
                        emphasisBorderToColor);
                }

            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(xCount);
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void DrawCountHeatmapSerie(VertexHelper vh, Heatmap serie)
        {
            if (!serie.show || serie.animation.HasFadeOut()) return;
            XAxis xAxis;
            YAxis yAxis;
            if (!chart.TryGetChartComponent<XAxis>(out xAxis, serie.xAxisIndex)) return;
            if (!chart.TryGetChartComponent<YAxis>(out yAxis, serie.yAxisIndex)) return;
            m_SerieGrid = chart.GetChartComponent<GridCoord>(xAxis.gridIndex);
            xAxis.boundaryGap = true;
            yAxis.boundaryGap = true;
            var visualMap = chart.GetVisualMapOfSerie(serie);
            var emphasisStyle = serie.emphasisStyle;
            var xCount = AxisHelper.GetTotalSplitGridNum(xAxis);
            var yCount = AxisHelper.GetTotalSplitGridNum(yAxis);
            var xWidth = m_SerieGrid.context.width / xCount;
            var yWidth = m_SerieGrid.context.height / yCount;

            var zeroX = m_SerieGrid.context.x;
            var zeroY = m_SerieGrid.context.y;
            var borderWidth = serie.itemStyle.show ? serie.itemStyle.borderWidth : 0;
            var splitWid = xWidth - 2 * borderWidth;
            var splitHig = yWidth - 2 * borderWidth;
            var defaultSymbolSize = Mathf.Min(splitWid, splitHig) * 0.25f;

            serie.animation.InitProgress(0, xCount);
            var animationIndex = serie.animation.GetCurrIndex();
            var dataChanging = false;
            serie.containerIndex = m_SerieGrid.index;
            serie.containterInstanceId = m_SerieGrid.instanceId;

            m_CountDict.Clear();
            double minCount = 0, maxCount = 0;
            foreach (var serieData in serie.data)
            {
                var xValue = serieData.GetData(0);
                var yValue = serieData.GetData(1);
                var i = AxisHelper.GetAxisValueSplitIndex(xAxis, xValue, xCount);
                var j = AxisHelper.GetAxisValueSplitIndex(yAxis, yValue, yCount);
                var key = GetGridKey(i, j);
                var count = 0;

                if (!m_CountDict.TryGetValue(key, out count))
                    count = 1;
                else
                    count++;
                if (count > maxCount)
                    maxCount = count;
                m_CountDict[key] = count;
            }

            if (visualMap.autoMinMax)
            {
                VisualMapHelper.SetMinMax(visualMap, minCount, maxCount);
            }
            var rangeMin = visualMap.rangeMin;
            var rangeMax = visualMap.rangeMax;

            int highlightX = -1;
            int highlightY = -1;
            if (serie.context.pointerItemDataIndex > 0)
            {
                if (m_CountDict.ContainsKey(serie.context.pointerItemDataIndex))
                {
                    GetGridXYByKey(serie.context.pointerItemDataIndex, out highlightX, out highlightY);
                }
            }
            var state = SerieHelper.GetSerieState(serie, null, true);
            var symbol = SerieHelper.GetSerieSymbol(serie, null, state);
            var symbolSize = SerieHelper.GetSysmbolSize(serie, null, chart.theme, defaultSymbolSize, state);
            var isRectSymbol = symbol.type == SymbolType.Rect;
            float symbolBorder = 0f;
            float[] cornerRadius = null;
            Color32 color, toColor, emptyColor, borderColor;
            SerieHelper.GetItemColor(out color, out toColor, out emptyColor, serie, null, chart.theme, serie.context.colorIndex, state);
            SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, null, chart.theme, state);
            foreach (var kv in m_CountDict)
            {
                int i, j;
                GetGridXYByKey(kv.Key, out i, out j);
                var value = kv.Value;

                if (serie.IsIgnoreValue(value))
                {
                    continue;
                }

                if ((value < rangeMin && rangeMin != visualMap.min) ||
                    (value > rangeMax && rangeMax != visualMap.max))
                {
                    continue;
                }
                if (!visualMap.IsInSelectedValue(value))
                    continue;
                if (animationIndex >= 0 && i > animationIndex)
                    continue;

                var highlight = i == highlightX && j == highlightY;

                color = visualMap.GetColor(value);
                if (highlight)
                    color = ChartHelper.GetHighlightColor(color);

                var pos = new Vector3(zeroX + (i + 0.5f) * xWidth,
                    zeroY + (j + 0.5f) * yWidth);

                var rectWid = 0f;
                var rectHig = 0f;
                if (isRectSymbol)
                {
                    if (symbol.size == 0 && symbol.sizeType == SymbolSizeType.Custom)
                    {
                        rectWid = splitWid;
                        rectHig = splitHig;
                    }
                    else
                    {
                        rectWid = symbolSize;
                        rectHig = symbolSize;
                    }
                    var rect = new Rect(pos.x - rectWid / 2, pos.y - rectHig / 2, rectWid, rectHig);
                    UGL.DrawRectangle(vh, rect, color);

                    if (borderWidth > 0 && !ChartHelper.IsClearColor(borderColor))
                    {
                        UGL.DrawBorder(vh, pos, rectWid, rectHig, borderWidth, borderColor, borderColor);
                    }
                }
                else
                {
                    chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, pos,
                        color, color, emptyColor, borderColor, symbol.gap, cornerRadius);
                }

                if (visualMap.hoverLink && highlight && emphasisStyle != null &&
                    emphasisStyle.itemStyle.borderWidth > 0)
                {
                    var emphasisItemStyle = emphasisStyle.itemStyle;
                    var emphasisBorderWidth = emphasisItemStyle.borderWidth;
                    var emphasisBorderColor = emphasisItemStyle.opacity > 0 ?
                        emphasisItemStyle.borderColor : ChartConst.clearColor32;
                    var emphasisBorderToColor = emphasisItemStyle.opacity > 0 ?
                        emphasisItemStyle.borderToColor : ChartConst.clearColor32;
                    UGL.DrawBorder(vh, pos, rectWid, rectHig, emphasisBorderWidth, emphasisBorderColor,
                        emphasisBorderToColor);
                }

            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(xCount);
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }
    }
}