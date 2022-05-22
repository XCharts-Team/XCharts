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
            UpdateSerieContext();
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
            string marker, string itemFormatter, string numericFormatter,
            ref List<SerieParams> paramList, ref string title)
        {
            if (!serie.context.pointerEnter)
                return;
            dataIndex = serie.context.pointerItemDataIndex;
            if (dataIndex < 0)
                return;

            if (serie.radarType == RadarType.Single)
            {
                UpdateItemSerieParams(ref paramList, ref title, dataIndex, category,
                    marker, itemFormatter, numericFormatter);
                return;
            }

            var radar = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
            if (radar == null)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;

            var color = SerieHelper.GetItemColor(serie, serieData, chart.theme, dataIndex, false);;
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

        private void UpdateSerieContext()
        {
            var needCheck = m_LegendEnter ||
                (chart.isPointerInChart && (m_RadarCoord != null && m_RadarCoord.IsPointerEnter()));
            var needInteract = false;
            var needHideAll = false;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag == needCheck)
                    return;
                needHideAll = true;
            }
            m_LastCheckContextFlag = needCheck;
            serie.context.pointerEnter = false;
            serie.context.pointerItemDataIndex = -1;
            var areaStyle = serie.areaStyle;
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        var serieData = serie.data[i];
                        serieData.index = i;
                        var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                        var symbolSize = symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                        if (needHideAll || m_LegendEnter)
                        {
                            serieData.context.highlight = needHideAll ? false : true;
                            serieData.interact.SetValue(ref needInteract, symbolSize, serieData.context.highlight);
                        }
                        else
                        {
                            serieData.context.highlight = false;
                            foreach (var pos in serieData.context.dataPoints)
                            {
                                if (Vector3.Distance(chart.pointerPos, pos) < symbolSize * 2)
                                {
                                    serie.context.pointerEnter = true;
                                    serie.context.pointerItemDataIndex = i;
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
                                        serie.context.pointerEnter = true;
                                        serie.context.pointerItemDataIndex = i;
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
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        var serieData = serie.data[i];
                        serieData.index = i;
                        if (Vector3.Distance(chart.pointerPos, serieData.context.position) < serie.symbol.size * 2)
                        {
                            serie.context.pointerEnter = true;
                            serie.context.pointerItemDataIndex = i;
                            return;
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
                                p1.context.highlight = true;
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
            SerieHelper.GetAllMinMaxData(serie, m_RadarCoord.ceilRate);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                string dataName = serieData.name;
                if (!serieData.show)
                {
                    continue;
                }
                var lineStyle = SerieHelper.GetLineStyle(serie, serieData);
                var areaStyle = SerieHelper.GetAreaStyle(serie, serieData);
                var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                var isHighlight = serieData.context.highlight;
                var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                var areaColor = SerieHelper.GetAreaColor(serie, serieData, chart.theme, colorIndex, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, serieData, chart.theme, colorIndex, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, serieData, chart.theme, colorIndex, isHighlight);
                var lineWidth = lineStyle.GetWidth(chart.theme.serie.lineWidth);
                int dataCount = m_RadarCoord.indicatorList.Count;
                serieData.context.dataPoints.Clear();
                for (int n = 0; n < dataCount; n++)
                {
                    if (n >= serieData.data.Count) break;
                    var min = m_RadarCoord.GetIndicatorMin(n);
                    var max = m_RadarCoord.GetIndicatorMax(n);
                    var value = serieData.GetCurrData(n, dataChangeDuration);
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
                    var radius = (float) (m_RadarCoord.context.dataRadius * (value - min) / (max - min));
                    var currAngle = (n + (m_RadarCoord.positionType == RadarCoord.PositionType.Between ? 0.5f : 0)) * angle;
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
                        if (areaStyle != null && areaStyle.show)
                        {
                            UGL.DrawTriangle(vh, startPoint, toPoint, centerPos, areaColor, areaColor, areaToColor);
                        }
                        if (lineStyle.show)
                        {
                            ChartDrawer.DrawLineStyle(vh, lineStyle.type, lineWidth, startPoint, toPoint, lineColor);
                        }
                        startPoint = toPoint;
                    }
                    serieData.context.dataPoints.Add(startPoint);
                }
                if (areaStyle != null && areaStyle.show)
                {
                    UGL.DrawTriangle(vh, startPoint, firstPoint, centerPos, areaColor, areaColor, areaToColor);
                }
                if (lineStyle.show)
                {
                    ChartDrawer.DrawLineStyle(vh, lineStyle.type, lineWidth, startPoint, firstPoint, lineColor);
                }
                if (symbol.show && symbol.type != SymbolType.None)
                {
                    for (int m = 0; m < serieData.context.dataPoints.Count; m++)
                    {
                        var point = serieData.context.dataPoints[m];
                        var symbolSize = isHighlight ?
                            symbol.GetSelectedSize(null, chart.theme.serie.lineSymbolSelectedSize) :
                            symbol.GetSize(null, chart.theme.serie.lineSymbolSize);
                        if (!serieData.interact.TryGetValue(ref symbolSize, ref interacting))
                        {
                            symbolSize = isHighlight ?
                                symbol.GetSelectedSize(serieData.data, symbolSize) :
                                symbol.GetSize(serieData.data, symbolSize);
                            serieData.interact.SetValue(ref interacting, symbolSize);
                            symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                        }
                        var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, j, isHighlight);
                        var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, j, isHighlight);
                        var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, j, isHighlight, false);
                        var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                        var borderColor = SerieHelper.GetSymbolBorderColor(serie, serieData, chart.theme, isHighlight);
                        var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
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
            var radar = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
            if (radar == null)
                return;

            var indicatorNum = radar.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var centerPos = radar.context.center;
            serie.animation.InitProgress(0, 1);
            serie.context.dataPoints.Clear();
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
            var startIndex = GetStartShowIndex(serie);
            var endIndex = GetEndShowIndex(serie);
            SerieHelper.UpdateMinMaxData(serie, 1, radar.ceilRate);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                serieData.index = j;
                string dataName = serieData.name;

                if (!serieData.show)
                {
                    serieData.context.labelPosition = Vector3.zero;
                    continue;
                }
                var lineStyle = SerieHelper.GetLineStyle(serie, serieData);
                var areaStyle = SerieHelper.GetAreaStyle(serie, serieData);
                var isHighlight = serie.context.pointerEnter;
                var areaColor = SerieHelper.GetAreaColor(serie, serieData, chart.theme, j, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, serieData, chart.theme, j, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, serieData, chart.theme, j, isHighlight);
                int dataCount = radar.indicatorList.Count;
                var index = serieData.index;
                var p = radar.context.center;
                var max = radar.GetIndicatorMax(index);
                var value = serieData.GetCurrData(1, dataChangeDuration);
                if (serieData.IsDataChanged()) dataChanging = true;
                if (max == 0)
                {
                    max = serie.context.dataMax;
                }
                if (!radar.IsInIndicatorRange(j, serieData.GetData(1)))
                {
                    lineColor = radar.outRangeColor;
                }
                var radius = (float) (max < 0 ? radar.context.dataRadius - radar.context.dataRadius * value / max :
                    radar.context.dataRadius * value / max);
                var currAngle = (index + (radar.positionType == RadarCoord.PositionType.Between ? 0.5f : 0)) * angle;
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
                    if (areaStyle != null && areaStyle.show)
                    {
                        UGL.DrawTriangle(vh, startPoint, toPoint, p, areaColor, areaColor, areaToColor);
                    }
                    if (lineStyle.show)
                    {
                        if (radar.connectCenter)
                            ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, centerPos,
                                chart.theme.serie.lineWidth, LineStyle.Type.Solid, lastColor, lastColor);
                        ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, toPoint, chart.theme.serie.lineWidth,
                            LineStyle.Type.Solid, radar.lineGradient ? lastColor : lineColor, lineColor);
                    }
                    startPoint = toPoint;
                    lastColor = lineColor;
                }
                serieData.context.position = startPoint;
                serieData.context.labelPosition = startPoint;

                if (areaStyle != null && areaStyle.show && j == endIndex)
                {
                    UGL.DrawTriangle(vh, startPoint, firstPoint, centerPos, areaColor, areaColor, areaToColor);
                }
                if (lineStyle.show && j == endIndex)
                {
                    if (radar.connectCenter)
                        ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, centerPos,
                            chart.theme.serie.lineWidth, LineStyle.Type.Solid, lastColor, lastColor);
                    ChartDrawer.DrawLineStyle(vh, lineStyle, startPoint, firstPoint, chart.theme.serie.lineWidth,
                        LineStyle.Type.Solid, lineColor, radar.lineGradient ? firstColor : lineColor);
                }
            }
            if (serie.symbol.show && serie.symbol.type != SymbolType.None)
            {
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (!serieData.show) continue;
                    var isHighlight = serie.highlight || serieData.context.highlight || serie.context.pointerEnter;
                    var serieIndex = serieData.index;
                    var symbolSize = isHighlight ?
                        serie.symbol.GetSelectedSize(serieData.data, chart.theme.serie.lineSymbolSelectedSize) :
                        serie.symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                    var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                    var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                    var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
                    var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                    var borderColor = SerieHelper.GetSymbolBorderColor(serie, serieData, chart.theme, isHighlight);
                    var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                    if (!radar.IsInIndicatorRange(j, serieData.GetData(1)))
                    {
                        symbolColor = radar.outRangeColor;
                        symbolToColor = radar.outRangeColor;
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

        private void DrawRadarSymbol(VertexHelper vh, Serie serie, SerieData serieData, int serieIndex, bool isHighlight,
            List<Vector3> pointList)
        {
            if (serie.symbol.show && serie.symbol.type != SymbolType.None)
            {
                var symbolSize = isHighlight ?
                    serie.symbol.GetSelectedSize(serieData.data, chart.theme.serie.lineSymbolSelectedSize) :
                    serie.symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                var borderColor = SerieHelper.GetSymbolBorderColor(serie, serieData, chart.theme, isHighlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                foreach (var point in pointList)
                {
                    chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, point, symbolColor,
                        symbolToColor, symbolEmptyColor, borderColor, serie.symbol.gap, cornerRadius);
                }
            }
        }
    }
}