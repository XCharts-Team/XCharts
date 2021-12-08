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
        Dictionary<string, int> serieNameSet = new Dictionary<string, int>();

        public override void DrawBase(VertexHelper vh)
        {
            serieNameSet.Clear();
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (!serie.show) return;
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    DrawMutipleRadar(vh, serie, serie.index);
                    break;
                case RadarType.Single:
                    DrawSingleRadar(vh, serie, serie.index);
                    break;
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

        private void DrawMutipleRadar(VertexHelper vh, Serie serie, int i)
        {
            if (!serie.show) return;
            var radar = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
            if (radar == null) return;
            var tooltip = chart.GetChartComponent<Tooltip>();
            var startPoint = Vector3.zero;
            var toPoint = Vector3.zero;
            var firstPoint = Vector3.zero;
            var indicatorNum = radar.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var centerPos = radar.context.center;
            var serieNameCount = -1;
            serie.animation.InitProgress(1, 0, 1);
            if (!serie.show || serie.animation.HasFadeOut())
            {
                return;
            }
            var rate = serie.animation.GetCurrRate();
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            SerieHelper.GetAllMinMaxData(serie, radar.ceilRate);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                int key = i * 1000 + j;
                if (!radar.context.dataPositionDict.ContainsKey(key))
                {
                    radar.context.dataPositionDict.Add(i * 1000 + j, new List<Vector3>(serieData.data.Count));
                }
                else
                {
                    radar.context.dataPositionDict[key].Clear();
                }
                string dataName = serieData.name;
                int serieIndex = 0;
                if (string.IsNullOrEmpty(dataName))
                {
                    serieNameCount++;
                    serieIndex = serieNameCount;
                }
                else if (!serieNameSet.ContainsKey(dataName))
                {
                    serieNameSet.Add(dataName, serieNameCount);
                    serieNameCount++;
                    serieIndex = serieNameCount;
                }
                else
                {
                    serieIndex = serieNameSet[dataName];
                }
                if (!serieData.show)
                {
                    continue;
                }
                var isHighlight = IsHighlight(radar, serie, serieData, tooltip, j, 0);
                var areaColor = SerieHelper.GetAreaColor(serie, chart.theme, serieIndex, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, chart.theme, serieIndex, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, chart.theme, serieIndex, isHighlight);
                var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
                int dataCount = radar.indicatorList.Count;
                List<Vector3> pointList = radar.context.dataPositionDict[key];
                for (int n = 0; n < dataCount; n++)
                {
                    if (n >= serieData.data.Count) break;
                    var max = radar.GetIndicatorMax(n);
                    var value = serieData.GetCurrData(n, dataChangeDuration);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    if (max == 0)
                    {
                        max = serie.context.dataMax;
                    }
                    var radius = (float)(max < 0 ? radar.context.dataRadius - radar.context.dataRadius * value / max
                    : radar.context.dataRadius * value / max);
                    var currAngle = (n + (radar.positionType == RadarCoord.PositionType.Between ? 0.5f : 0)) * angle;
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
                    pointList.Add(startPoint);
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
                    for (int m = 0; m < pointList.Count; m++)
                    {
                        var point = pointList[m];
                        isHighlight = IsHighlight(radar, serie, serieData, tooltip, j, m);
                        var symbolSize = isHighlight
                            ? serie.symbol.GetSelectedSize(null, chart.theme.serie.lineSymbolSelectedSize)
                            : serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize);
                        var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                        var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                        var symbolEmptyColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
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
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
        }

        private bool IsHighlight(RadarCoord radar, Serie serie, SerieData serieData, Tooltip tooltip, int dataIndex, int dimension)
        {
            if (serie.highlighted || serieData.context.highlighted) return true;
            if (tooltip == null) return false;
            var selectedSerieIndex = tooltip.runtimeDataIndex[0];
            if (selectedSerieIndex < 0) return false;
            if (chart.GetSerie(selectedSerieIndex).radarIndex != serie.radarIndex) return false;
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    if (radar.isAxisTooltip)
                    {
                        var selectedDimension = tooltip.runtimeDataIndex[2];
                        return selectedDimension == dimension;
                    }
                    else if (tooltip.runtimeDataIndex.Count >= 2)
                    {
                        return tooltip.runtimeDataIndex[0] == serie.index && tooltip.runtimeDataIndex[1] == dataIndex;
                    }
                    else
                    {
                        return false;
                    }
                case RadarType.Single:
                    return tooltip.runtimeDataIndex[1] == dataIndex;
            }
            return false;
        }

        private void DrawSingleRadar(VertexHelper vh, Serie serie, int i)
        {
            var startPoint = Vector3.zero;
            var toPoint = Vector3.zero;
            var firstPoint = Vector3.zero;
            var lastColor = ColorUtil.clearColor32;
            var firstColor = ColorUtil.clearColor32;

            var radar = chart.GetChartComponent<RadarCoord>(serie.radarIndex);
            var tooltip = chart.GetChartComponent<Tooltip>();
            var indicatorNum = radar.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var centerPos = radar.context.center;
            var serieNameCount = -1;
            serie.animation.InitProgress(1, 0, 1);
            if (!serie.show || serie.animation.HasFadeOut())
            {
                return;
            }
            var rate = serie.animation.GetCurrRate();
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            int key = i * 1000;
            if (!radar.context.dataPositionDict.ContainsKey(key))
            {
                radar.context.dataPositionDict.Add(i * 1000, new List<Vector3>(serie.dataCount));
            }
            else
            {
                radar.context.dataPositionDict[key].Clear();
            }
            var pointList = radar.context.dataPositionDict[key];
            var startIndex = GetStartShowIndex(serie);
            var endIndex = GetEndShowIndex(serie);
            SerieHelper.UpdateMinMaxData(serie, 1, radar.ceilRate);
            for (int j = 0; j < serie.data.Count; j++)
            {
                var serieData = serie.data[j];
                serieData.index = j;
                string dataName = serieData.name;
                int serieIndex = 0;
                if (string.IsNullOrEmpty(dataName))
                {
                    serieNameCount++;
                    serieIndex = serieNameCount;
                }
                else if (!serieNameSet.ContainsKey(dataName))
                {
                    serieNameSet.Add(dataName, serieNameCount);
                    serieNameCount++;
                    serieIndex = serieNameCount;
                }
                else
                {
                    serieIndex = serieNameSet[dataName];
                }
                if (!serieData.show)
                {
                    serieData.context.labelPosition = Vector3.zero;
                    continue;
                }
                var isHighlight = IsHighlight(radar, serie, serieData, tooltip, j, 0);
                var areaColor = SerieHelper.GetAreaColor(serie, chart.theme, serieIndex, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, chart.theme, serieIndex, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, chart.theme, serieIndex, isHighlight);
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
                serieData.context.labelPosition = startPoint;
                pointList.Add(startPoint);

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
                    var isHighlight = serie.highlighted || serieData.context.highlighted ||
                    (tooltip.show && tooltip.runtimeDataIndex[0] == i && tooltip.runtimeDataIndex[1] == j);
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

        private bool IsInRadar(Vector2 local)
        {
            foreach (var component in chart.components)
            {
                if (component is RadarCoord)
                {
                    var radar = component as RadarCoord;
                    var dist = Vector2.Distance(radar.context.center, local);
                    if (dist < radar.context.radius + chart.theme.serie.lineSymbolSize)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}