/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
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

            var color = chart.theme.GetColor(dataIndex);
            title = serieData.name;
            for (int i = 0; i < serieData.data.Count; i++)
            {
                var indicator = radar.GetIndicator(i);

                var param = new SerieParams();
                param.serieName = serie.serieName;
                param.serieIndex = serie.index;
                param.dimension = i;
                param.serieData = serieData;
                param.value = serieData.GetData(i);
                param.total = indicator.max;
                param.color = color;
                param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
                param.itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
                param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
                param.columns.Clear();

                param.columns.Add(param.marker);
                param.columns.Add(indicator == null ? string.Empty : indicator.name);
                param.columns.Add(ChartCached.NumberToStr(serieData.GetData(i), param.numericFormatter));

                paramList.Add(param);
            }
        }

        public override void RefreshLabelInternal()
        {
            for (int i = 0; i < chart.series.Count; i++)
            {
                var serie = chart.GetSerie(i);
                if (!(serie is Radar)) continue;
                if (!serie.show && serie.radarType != RadarType.Single) continue;
                var radar = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
                if (radar == null) continue;
                var center = radar.context.center;
                for (int n = 0; n < serie.dataCount; n++)
                {
                    var serieData = serie.data[n];
                    if (serieData.labelObject == null) continue;
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                    var iconStyle = SerieHelper.GetIconStyle(serie, serieData);
                    var labelPos = serieData.context.labelPosition;
                    if (serieLabel.margin != 0)
                    {
                        labelPos += serieLabel.margin * (labelPos - center).normalized;
                    }
                    serieData.labelObject.SetPosition(labelPos);
                    serieData.labelObject.UpdateIcon(iconStyle);
                    if (serie.show && serieLabel.show && serieData.context.canShowLabel)
                    {
                        var value = serieData.GetCurrData(1);
                        var max = radar.GetIndicatorMax(n);
                        SerieLabelHelper.ResetLabel(serieData.labelObject.label, serieLabel, chart.theme, i);
                        serieData.SetLabelActive(serieData.context.labelPosition != Vector3.zero);
                        serieData.labelObject.SetLabelPosition(serieLabel.offset);
                        var content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, max,
                            serieLabel, Color.clear);
                        if (serieData.labelObject.SetText(content))
                        {
                            chart.RefreshPainter(serie);
                        }
                    }
                    else
                    {
                        serieData.SetLabelActive(false);
                    }
                }
            }
        }

        public override bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (!chart.HasSerie<Radar>()) return false;
            if (!LegendHelper.IsSerieLegend<Radar>(chart, legendName)) return false;
            LegendHelper.CheckDataShow(chart.series, legendName, show);
            chart.UpdateLegendColor(legendName, show);
            chart.RefreshChart();
            return true;
        }

        public override bool OnLegendButtonEnter(int index, string legendName)
        {
            if (!chart.HasSerie<Radar>()) return false;
            if (!LegendHelper.IsSerieLegend<Radar>(chart, legendName)) return false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, true);
            chart.RefreshChart();
            return true;
        }

        public override bool OnLegendButtonExit(int index, string legendName)
        {
            if (!chart.HasSerie<Radar>()) return false;
            if (!LegendHelper.IsSerieLegend<Radar>(chart, legendName)) return false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, false);
            chart.RefreshChart();
            return true;
        }

        private void UpdateSerieContext()
        {
            var needCheck = serie.context.isLegendEnter || (chart.isPointerInChart && m_RadarCoord.IsPointerEnter());
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
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    for (int i = 0; i < serie.data.Count; i++)
                    {
                        var serieData = serie.data[i];
                        serieData.index = i;
                        var symbol = SerieHelper.GetSerieSymbol(serie, serieData);
                        var symbolSize = symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                        if (needHideAll || serie.context.isLegendEnter)
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
                var isHighlight = serieData.context.highlight;
                var areaColor = SerieHelper.GetAreaColor(serie, chart.theme, j, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, chart.theme, j, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, chart.theme, j, isHighlight);
                var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
                int dataCount = m_RadarCoord.indicatorList.Count;
                serieData.context.dataPoints.Clear();
                for (int n = 0; n < dataCount; n++)
                {
                    if (n >= serieData.data.Count) break;
                    var max = m_RadarCoord.GetIndicatorMax(n);
                    var value = serieData.GetCurrData(n, dataChangeDuration);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    if (max == 0)
                    {
                        max = serie.context.dataMax;
                    }
                    var radius = (float)(max < 0 ? m_RadarCoord.context.dataRadius - m_RadarCoord.context.dataRadius * value / max
                    : m_RadarCoord.context.dataRadius * value / max);
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
                        if (serie.areaStyle.show)
                        {
                            UGL.DrawTriangle(vh, startPoint, toPoint, centerPos, areaColor, areaColor, areaToColor);
                        }
                        if (serie.lineStyle.show)
                        {
                            ChartDrawer.DrawLineStyle(vh, serie.lineStyle.type, lineWidth, startPoint, toPoint, lineColor);
                        }
                        startPoint = toPoint;
                    }
                    serieData.context.dataPoints.Add(startPoint);
                }
                if (serie.areaStyle.show)
                {
                    UGL.DrawTriangle(vh, startPoint, firstPoint, centerPos, areaColor, areaColor, areaToColor);
                }
                if (serie.lineStyle.show)
                {
                    ChartDrawer.DrawLineStyle(vh, serie.lineStyle.type, lineWidth, startPoint, firstPoint, lineColor);
                }
                if (serie.symbol.show && serie.symbol.type != SymbolType.None)
                {
                    for (int m = 0; m < serieData.context.dataPoints.Count; m++)
                    {
                        var point = serieData.context.dataPoints[m];
                        var symbolSize = isHighlight
                            ? serie.symbol.GetSelectedSize(null, chart.theme.serie.lineSymbolSelectedSize)
                            : serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize);
                        if (!serieData.interact.TryGetValue(ref symbolSize, ref interacting))
                        {
                            symbolSize = isHighlight
                                ? serie.symbol.GetSelectedSize(serieData.data, symbolSize)
                                : serie.symbol.GetSize(serieData.data, symbolSize);
                            serieData.interact.SetValue(ref interacting, symbolSize);
                            symbolSize = serie.animation.GetSysmbolSize(symbolSize);
                        }
                        var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, j, isHighlight);
                        var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, j, isHighlight);
                        var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, j, isHighlight, false);
                        var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                        var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                        chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, point, symbolColor,
                           symbolToColor, symbolEmptyColor, serie.symbol.gap, cornerRadius);
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
                var isHighlight = serie.context.pointerEnter;
                var areaColor = SerieHelper.GetAreaColor(serie, chart.theme, j, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, chart.theme, j, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, chart.theme, j, isHighlight);
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
                var radius = (float)(max < 0 ? radar.context.dataRadius - radar.context.dataRadius * value / max
                : radar.context.dataRadius * value / max);
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
                    if (serie.areaStyle.show)
                    {
                        UGL.DrawTriangle(vh, startPoint, toPoint, p, areaColor, areaColor, areaToColor);
                    }
                    if (serie.lineStyle.show)
                    {
                        if (radar.connectCenter)
                            ChartDrawer.DrawLineStyle(vh, serie.lineStyle, startPoint, centerPos,
                                chart.theme.serie.lineWidth, LineStyle.Type.Solid, lastColor, lastColor);
                        ChartDrawer.DrawLineStyle(vh, serie.lineStyle, startPoint, toPoint, chart.theme.serie.lineWidth,
                            LineStyle.Type.Solid, radar.lineGradient ? lastColor : lineColor, lineColor);
                    }
                    startPoint = toPoint;
                    lastColor = lineColor;
                }
                serieData.context.position = startPoint;
                serieData.context.labelPosition = startPoint;

                if (serie.areaStyle.show && j == endIndex)
                {
                    UGL.DrawTriangle(vh, startPoint, firstPoint, centerPos, areaColor, areaColor, areaToColor);
                }
                if (serie.lineStyle.show && j == endIndex)
                {
                    if (radar.connectCenter)
                        ChartDrawer.DrawLineStyle(vh, serie.lineStyle, startPoint, centerPos,
                             chart.theme.serie.lineWidth, LineStyle.Type.Solid, lastColor, lastColor);
                    ChartDrawer.DrawLineStyle(vh, serie.lineStyle, startPoint, firstPoint, chart.theme.serie.lineWidth,
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
                    var symbolSize = isHighlight
                        ? serie.symbol.GetSelectedSize(serieData.data, chart.theme.serie.lineSymbolSelectedSize)
                        : serie.symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                    var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                    var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                    var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
                    var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                    var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                    if (!radar.IsInIndicatorRange(j, serieData.GetData(1)))
                    {
                        symbolColor = radar.outRangeColor;
                        symbolToColor = radar.outRangeColor;
                    }
                    chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, serieData.context.labelPosition, symbolColor,
                           symbolToColor, symbolEmptyColor, serie.symbol.gap, cornerRadius);
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
                var symbolSize = isHighlight
                    ? serie.symbol.GetSelectedSize(serieData.data, chart.theme.serie.lineSymbolSelectedSize)
                    : serie.symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                foreach (var point in pointList)
                {
                    chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, point, symbolColor,
                       symbolToColor, symbolEmptyColor, serie.symbol.gap, cornerRadius);
                }
            }
        }
    }
}