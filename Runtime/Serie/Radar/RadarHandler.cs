using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class RadarHandler : SerieHandler<Radar>
    {
        private RadarCoord m_RadarCoord;
        public override void Update()
        {
            base.Update();
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (!serie.show) return;
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    DrawMutipleRadar(vh);
                    break;
                case RadarType.Single:
                    DrawSingleRadar(vh);
                    break;
            }
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        {
            if (!serie.context.pointerEnter)
                return;
            dataIndex = serie.context.pointerItemDataIndex;
            if (dataIndex < 0)
                return;

            if (serie.radarType == RadarType.Single)
            {
                var colorIndex1 = serie.colorByData ? dataIndex : serie.context.colorIndex;
                UpdateItemSerieParams(ref paramList, ref title, dataIndex, category,
                    marker, itemFormatter, numericFormatter, ignoreDataDefaultContent, 1, colorIndex1);
                return;
            }

            var radar = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
            if (radar == null)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            Color32 color, toColor;
            var colorIndex = serie.colorByData ? chart.GetLegendRealShowNameIndex(serieData.legendName) : serie.context.colorIndex;
            SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, SerieState.Normal);
            title = serieData.name;
            for (int i = 0; i < serieData.data.Count; i++)
            {
                var indicator = radar.GetIndicator(i);
                if (indicator == null) continue;

                var param = new SerieParams();
                param.serieName = serie.serieName;
                param.serieIndex = serie.index;
                param.dimension = i;
                param.serieData = serieData;
                param.dataCount = serie.dataCount;
                param.value = serieData.GetData(i);
                param.total = indicator.max;
                param.color = color;
                param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
                param.itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
                param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
                param.columns.Clear();

                param.columns.Add(param.marker);
                param.columns.Add(indicator.name);
                param.columns.Add(ChartCached.NumberToStr(serieData.GetData(i), param.numericFormatter));

                paramList.Add(param);
            }
        }

        public override void UpdateSerieContext()
        {
            var needCheck = m_LegendEnter ||
                (chart.isPointerInChart && (m_RadarCoord != null && m_RadarCoord.IsPointerEnter()));
            var needInteract = false;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerItemDataDimension = -1;
                    serie.context.pointerEnter = false;
                    foreach (var serieData in serie.data)
                    {
                        serieData.context.highlight = false;
                        serieData.interact.Reset();
                    }
                    chart.RefreshPainter(serie);
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            serie.highlight = false;
            serie.context.pointerEnter = false;
            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerItemDataDimension = -1;
            var areaStyle = serie.areaStyle;
            var themeSymbolSize = chart.theme.serie.lineSymbolSize;
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        var serieData = serie.data[i];
                        var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                        var symbolSize = symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                        if (m_LegendEnter)
                        {
                            serieData.context.highlight = true;
                            serieData.interact.SetValue(ref needInteract, symbolSize, serieData.context.highlight);
                        }
                        else
                        {
                            serieData.context.highlight = false;
                            for (int n = 0; n < serieData.context.dataPoints.Count; n++)
                            {
                                var pos = serieData.context.dataPoints[n];
                                if (Vector3.Distance(chart.pointerPos, pos) < symbolSize * 2)
                                {
                                    serie.highlight = true;
                                    serie.context.pointerEnter = true;
                                    serie.context.pointerItemDataIndex = i;
                                    serie.context.pointerItemDataDimension = n;
                                    serieData.context.highlight = true;
                                    break;
                                }
                            }
                            if (!serieData.context.highlight && areaStyle != null)
                            {
                                var center = m_RadarCoord.context.center;
                                var dataPoints = serieData.context.dataPoints;
                                for (int n = 0; n < dataPoints.Count; n++)
                                {
                                    var p1 = dataPoints[n];
                                    var p2 = n >= dataPoints.Count - 1 ? dataPoints[0] : dataPoints[n + 1];
                                    if (UGLHelper.IsPointInTriangle(p1, center, p2, chart.pointerPos))
                                    {
                                        serie.highlight = true;
                                        serie.context.pointerEnter = true;
                                        serie.context.pointerItemDataIndex = i;
                                        serie.context.pointerItemDataDimension = n;
                                        serieData.context.highlight = true;
                                        break;
                                    }
                                }
                            }
                            serieData.interact.SetValue(ref needInteract, symbolSize, serieData.context.highlight);
                        }
                    }
                    break;
                case RadarType.Single:
                    needInteract = false;
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        var serieData = serie.data[i];
                        var size = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme, themeSymbolSize);
                        if (Vector3.Distance(chart.pointerPos, serieData.context.position) < size * 2)
                        {
                            serie.context.pointerEnter = true;
                            serie.context.pointerItemDataIndex = i;
                            serie.context.pointerItemDataDimension = 1;
                            serieData.context.highlight = true;
                            needInteract = true;
                        }
                        else
                        {
                            serieData.context.highlight = false;
                        }
                    }
                    if (!serie.context.pointerEnter && areaStyle != null)
                    {
                        var center = m_RadarCoord.context.center;
                        var dataPoints = serie.data;
                        for (int n = 0; n < dataPoints.Count; n++)
                        {
                            var p1 = dataPoints[n];
                            var p2 = n >= dataPoints.Count - 1 ? dataPoints[0] : dataPoints[n + 1];
                            if (UGLHelper.IsPointInTriangle(p1.context.position, center, p2.context.position, chart.pointerPos))
                            {
                                serie.context.pointerEnter = true;
                                serie.context.pointerItemDataIndex = n;
                                serie.context.pointerItemDataDimension = 1;
                                p1.context.highlight = true;
                                needInteract = true;
                                break;
                            }
                        }
                    }
                    break;
            }
            if (needInteract)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void DrawMutipleRadar(VertexHelper vh)
        {
            if (!serie.show) return;
            m_RadarCoord = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
            if (m_RadarCoord == null) return;

            serie.containerIndex = m_RadarCoord.index;
            serie.containterInstanceId = m_RadarCoord.instanceId;

            var startPoint = Vector3.zero;
            var toPoint = Vector3.zero;
            var firstPoint = Vector3.zero;
            var indicatorNum = m_RadarCoord.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var centerPos = m_RadarCoord.context.center;
            serie.animation.InitProgress(0, 1);
            if (!serie.show || serie.animation.HasFadeOut())
            {
                return;
            }
            var rate = serie.animation.GetCurrRate();
            var dataChanging = false;
            var interacting = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var unscaledTime = serie.animation.unscaledTime;
            SerieHelper.GetAllMinMaxData(serie, m_RadarCoord.ceilRate);
            Color32 areaColor, areaToColor;
            var startAngle = m_RadarCoord.startAngle * Mathf.PI / 180;
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                string dataName = serieData.name;
                if (!serieData.show)
                {
                    continue;
                }
                var serieState = SerieHelper.GetSerieState(serie, serieData, true);
                var lineStyle = SerieHelper.GetLineStyle(serie, serieData);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData, serieState);

                var colorIndex = serie.colorByData ? chart.GetLegendRealShowNameIndex(serieData.legendName) : serie.context.colorIndex;
                var showArea = SerieHelper.GetAreaColor(out areaColor, out areaToColor, serie, serieData, chart.theme, colorIndex);
                var lineColor = SerieHelper.GetLineColor(serie, serieData, chart.theme, colorIndex);
                var lineWidth = lineStyle.GetWidth(chart.theme.serie.lineWidth);
                int dataCount = m_RadarCoord.indicatorList.Count;
                serieData.context.dataPoints.Clear();
                for (int n = 0; n < dataCount; n++)
                {
                    if (n >= serieData.data.Count) break;
                    var min = m_RadarCoord.GetIndicatorMin(n);
                    var max = m_RadarCoord.GetIndicatorMax(n);
                    var value = serieData.GetCurrData(n, dataChangeDuration, unscaledTime);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    if (max == 0)
                    {
                        if (serie.data.Count > 1)
                        {
                            SerieHelper.GetMinMaxData(serie, n, out min, out max);
                            min = ChartHelper.GetMinDivisibleValue(min, 0);
                            max = ChartHelper.GetMaxDivisibleValue(max, 0);
                            if (min > 0) min = 0;
                        }
                        else
                        {
                            max = serie.context.dataMax;
                        }
                    }
                    var radius = (float)(m_RadarCoord.context.dataRadius * (value - min) / (max - min));
                    var currAngle = startAngle + (n + (m_RadarCoord.positionType == RadarCoord.PositionType.Between ? 0.5f : 0)) * angle;
                    radius *= rate;
                    if (n == 0)
                    {
                        startPoint = new Vector3(centerPos.x + radius * Mathf.Sin(currAngle),
                            centerPos.y + radius * Mathf.Cos(currAngle));
                        firstPoint = startPoint;
                    }
                    else
                    {
                        toPoint = new Vector3(centerPos.x + radius * Mathf.Sin(currAngle),
                            centerPos.y + radius * Mathf.Cos(currAngle));
                        if (showArea && !serie.smooth)
                        {
                            UGL.DrawTriangle(vh, startPoint, toPoint, centerPos, areaColor, areaColor, areaToColor);
                        }
                        if (lineStyle.show && !serie.smooth)
                        {
                            ChartDrawer.DrawLineStyle(vh, lineStyle.type, lineWidth, startPoint, toPoint, lineColor);
                        }
                        startPoint = toPoint;
                    }
                    serieData.context.dataPoints.Add(startPoint);
                }
                if (showArea && !serie.smooth)
                {
                    UGL.DrawTriangle(vh, startPoint, firstPoint, centerPos, areaColor, areaColor, areaToColor);
                }
                if (lineStyle.show && !serie.smooth)
                {
                    ChartDrawer.DrawLineStyle(vh, lineStyle.type, lineWidth, startPoint, firstPoint, lineColor);
                }

                if (serie.smooth)
                {
                    UGL.DrawCurves(vh, serieData.context.dataPoints, lineWidth, lineColor,
                        chart.settings.lineSmoothStyle,
                        chart.settings.lineSmoothness,
                        UGL.Direction.Random,
                        float.NaN, true);
                }

                if (symbol.show && symbol.type != SymbolType.None)
                {
                    float symbolBorder = 0f;
                    float[] cornerRadius = null;
                    Color32 symbolColor, symbolToColor, symbolEmptyColor, borderColor;
                    for (int m = 0; m < serieData.context.dataPoints.Count; m++)
                    {
                        var point = serieData.context.dataPoints[m];
                        var symbolSize = 0f;
                        if (!serieData.interact.TryGetValue(ref symbolSize, ref interacting))
                        {
                            symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme, chart.theme.serie.lineSymbolSize, serieState);
                            serieData.interact.SetValue(ref interacting, symbolSize);
                            symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                        }
                        SerieHelper.GetItemColor(out symbolColor, out symbolToColor, out symbolEmptyColor, serie, serieData, chart.theme, colorIndex, serieState);
                        SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, serieData, chart.theme, serieState);
                        chart.DrawSymbol(vh, symbol.type, symbolSize, symbolBorder, point, symbolColor,
                            symbolToColor, symbolEmptyColor, borderColor, symbol.gap, cornerRadius);
                    }
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

        private void DrawSingleRadar(VertexHelper vh)
        {
            m_RadarCoord = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
            if (m_RadarCoord == null)
                return;

            var indicatorNum = m_RadarCoord.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var centerPos = m_RadarCoord.context.center;
            serie.animation.InitProgress(0, 1);
            if (!serie.show || serie.animation.HasFadeOut())
            {
                return;
            }
            var startPoint = Vector3.zero;
            var toPoint = Vector3.zero;
            var firstPoint = Vector3.zero;
            var lastColor = ColorUtil.clearColor32;
            var firstColor = ColorUtil.clearColor32;

            var rate = serie.animation.GetCurrRate();
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var unscaledTime = serie.animation.unscaledTime;
            var startIndex = GetStartShowIndex(serie);
            var endIndex = GetEndShowIndex(serie);
            var startAngle = m_RadarCoord.startAngle * Mathf.PI / 180;
            SerieHelper.UpdateMinMaxData(serie, 1, m_RadarCoord.ceilRate);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                string dataName = serieData.name;

                if (!serieData.show)
                {
                    serieData.context.labelPosition = Vector3.zero;
                    continue;
                }
                var lineStyle = SerieHelper.GetLineStyle(serie, serieData);
                Color32 areaColor, areaToColor;
                var colorIndex = serie.colorByData ? j : serie.context.colorIndex;
                var showArea = SerieHelper.GetAreaColor(out areaColor, out areaToColor, serie, serieData, chart.theme, colorIndex - 1);
                var lineColor = SerieHelper.GetLineColor(serie, serieData, chart.theme, colorIndex);
                int dataCount = m_RadarCoord.indicatorList.Count;
                var index = serieData.index;
                var p = m_RadarCoord.context.center;
                var max = m_RadarCoord.GetIndicatorMax(index);
                var value = serieData.GetCurrData(1, dataChangeDuration, unscaledTime);
                if (serieData.IsDataChanged()) dataChanging = true;
                if (max == 0)
                {
                    max = serie.context.dataMax;
                }
                if (!m_RadarCoord.IsInIndicatorRange(j, serieData.GetData(1)))
                {
                    lineColor = m_RadarCoord.outRangeColor;
                }
                var radius = (float)(max < 0 ? m_RadarCoord.context.dataRadius - m_RadarCoord.context.dataRadius * value / max :
                    m_RadarCoord.context.dataRadius * value / max);
                var currAngle = startAngle + (index + (m_RadarCoord.positionType == RadarCoord.PositionType.Between ? 0.5f : 0)) * angle;
                radius *= rate;
                if (index == startIndex)
                {
                    startPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                        p.y + radius * Mathf.Cos(currAngle));
                    firstPoint = startPoint;
                    lastColor = lineColor;
                    firstColor = lineColor;
                }
                else
                {
                    toPoint = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                        p.y + radius * Mathf.Cos(currAngle));
                    if (showArea && !serie.smooth)
                    {
                        UGL.DrawTriangle(vh, startPoint, toPoint, p, areaColor, areaColor, areaToColor);
                    }
                    if (lineStyle.show && !serie.smooth)
                    {
                        if (m_RadarCoord.connectCenter)
                            ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, centerPos,
                                chart.theme.serie.lineWidth, LineStyle.Type.Solid, lastColor, lastColor);
                        ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, toPoint, chart.theme.serie.lineWidth,
                            LineStyle.Type.Solid, m_RadarCoord.lineGradient ? lastColor : lineColor, lineColor);
                    }
                    startPoint = toPoint;
                    lastColor = lineColor;
                }
                serie.context.dataPoints.Add(startPoint);
                serie.context.dataIndexs.Add(serieData.index);
                serieData.context.position = startPoint;
                serieData.context.labelPosition = startPoint;

                if (showArea && j == endIndex && !serie.smooth)
                {
                    SerieHelper.GetAreaColor(out areaColor, out areaToColor, serie, serieData, chart.theme, colorIndex);
                    UGL.DrawTriangle(vh, startPoint, firstPoint, centerPos, areaColor, areaColor, areaToColor);
                }
                if (lineStyle.show && j == endIndex && !serie.smooth)
                {
                    if (m_RadarCoord.connectCenter)
                        ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, centerPos,
                            chart.theme.serie.lineWidth, LineStyle.Type.Solid, lastColor, lastColor);
                    ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, firstPoint, chart.theme.serie.lineWidth,
                        LineStyle.Type.Solid, lineColor, m_RadarCoord.lineGradient ? firstColor : lineColor);
                }
            }
            if (serie.smooth)
            {
                var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
                var lineColor = SerieHelper.GetLineColor(serie, null, chart.theme, serie.context.colorIndex);
                UGL.DrawCurves(vh, serie.context.dataPoints, lineWidth, lineColor,
                    chart.settings.lineSmoothStyle,
                    chart.settings.lineSmoothness,
                    UGL.Direction.Random,
                    float.NaN, true);
            }
            if (serie.symbol.show && serie.symbol.type != SymbolType.None)
            {
                float symbolBorder = 0f;
                float[] cornerRadius = null;
                Color32 symbolColor, symbolToColor, symbolEmptyColor, borderColor;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (!serieData.show) continue;
                    var state = SerieHelper.GetSerieState(serie, serieData);
                    var symbolSize = SerieHelper.GetSysmbolSize(serie, serieData, chart.theme, chart.theme.serie.lineSymbolSize, state);
                    var colorIndex = serie.colorByData ? serieData.index : serie.context.colorIndex;
                    SerieHelper.GetItemColor(out symbolColor, out symbolToColor, out symbolEmptyColor, serie, serieData, chart.theme, colorIndex, state);
                    SerieHelper.GetSymbolInfo(out borderColor, out symbolBorder, out cornerRadius, serie, serieData, chart.theme, state);
                    if (!m_RadarCoord.IsInIndicatorRange(j, serieData.GetData(1)))
                    {
                        symbolColor = m_RadarCoord.outRangeColor;
                        symbolToColor = m_RadarCoord.outRangeColor;
                    }
                    chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, serieData.context.labelPosition, symbolColor,
                        symbolToColor, symbolEmptyColor, borderColor, serie.symbol.gap, cornerRadius);
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

        private int GetStartShowIndex(Serie serie)
        {
            for (int i = 0; i < serie.dataCount; i++)
            {
                if (serie.data[i].show) return i;
            }
            return 0;
        }
        private int GetEndShowIndex(Serie serie)
        {
            for (int i = serie.dataCount - 1; i >= 0; i--)
            {
                if (serie.data[i].show) return i;
            }
            return 0;
        }
    }
}