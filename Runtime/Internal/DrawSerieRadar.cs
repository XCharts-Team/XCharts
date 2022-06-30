/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XUGL;
using System.Collections.Generic;

namespace XCharts
{
    internal class DrawSerieRadar : IDrawSerie
    {
        public BaseChart chart;
        private const string INDICATOR_TEXT = "indicator";
        private bool m_IsEnterLegendButtom;
        Dictionary<string, int> serieNameSet = new Dictionary<string, int>();

        public DrawSerieRadar(BaseChart chart)
        {
            this.chart = chart;
        }

        public void InitComponent()
        {
            InitRadars();
        }

        public void CheckComponent()
        {
        }

        public void Update()
        {
        }

        public void DrawBase(VertexHelper vh)
        {
            serieNameSet.Clear();
            for (int i = 0; i < chart.radars.Count; i++)
            {
                var radar = chart.radars[i];
                if (!radar.show) continue;
                radar.index = i;
                radar.UpdateRadarCenter(chart.chartPosition, chart.chartWidth, chart.chartHeight);
                if (radar.shape == Radar.Shape.Circle)
                {
                    DrawCricleRadar(vh, radar);
                }
                else
                {
                    DrawRadar(vh, radar);
                }
            }
        }

        public void DrawSerie(VertexHelper vh, Serie serie)
        {
            if (serie.type != SerieType.Radar) return;
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

        public void RefreshLabel()
        {
            for (int i = 0; i < chart.series.Count; i++)
            {
                var serie = chart.series.GetSerie(i);
                if (serie.type != SerieType.Radar) continue;
                if (!serie.show && serie.radarType != RadarType.Single) continue;
                var radar = chart.GetRadar(serie.radarIndex);
                if (radar == null) continue;
                var center = radar.runtimeCenterPos;
                for (int n = 0; n < serie.dataCount; n++)
                {
                    var serieData = serie.data[n];
                    if (serieData.labelObject == null) continue;
                    var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                    var iconStyle = SerieHelper.GetIconStyle(serie, serieData);
                    var labelPos = serieData.labelPosition;
                    if (serieLabel.margin != 0)
                    {
                        labelPos += serieLabel.margin * (labelPos - center).normalized;
                    }
                    serieData.labelObject.SetPosition(labelPos);
                    serieData.labelObject.UpdateIcon(iconStyle);
                    if (serie.show && serieLabel.show && serieData.canShowLabel)
                    {
                        var value = serieData.GetCurrData(1);
                        var max = radar.GetIndicatorMax(n);
                        SerieLabelHelper.ResetLabel(serieData.labelObject.label, serieLabel, chart.theme, i);
                        serieData.SetLabelActive(serieData.labelPosition != Vector3.zero);
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

        public bool CheckTootipArea(Vector2 local)
        {
            if (!chart.series.Contains(SerieType.Radar)) return false;
            if (m_IsEnterLegendButtom) return false;
            if (!IsInRadar(local)) return false;
            bool highlight = false;
            chart.tooltip.ClearValue();
            for (int i = 0; i < chart.series.Count; i++)
            {
                var serie = chart.series.GetSerie(i);
                if (!serie.show || serie.type != SerieType.Radar) continue;
                var radar = chart.radars[serie.radarIndex];
                var dist = Vector2.Distance(radar.runtimeCenterPos, local);
                if (dist > radar.runtimeRadius + serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize))
                {
                    continue;
                }
                switch (serie.radarType)
                {
                    case RadarType.Multiple:
                        for (int n = 0; n < serie.data.Count; n++)
                        {
                            var posKey = i * 1000 + n;
                            if (radar.runtimeDataPosList.ContainsKey(posKey))
                            {
                                var posList = radar.runtimeDataPosList[posKey];
                                var symbolSize = serie.symbol.GetSize(serie.data[n].data, chart.theme.serie.lineSymbolSize);
                                for (int k = 0; k < posList.Count; k++)
                                {
                                    if (Vector2.Distance(posList[k], local) <= symbolSize * 1.3f)
                                    {
                                        chart.tooltip.runtimeDataIndex[0] = i;
                                        chart.tooltip.runtimeDataIndex[1] = n;
                                        if (chart.tooltip.runtimeDataIndex.Count >= 3)
                                            chart.tooltip.runtimeDataIndex[2] = k;
                                        else
                                            chart.tooltip.runtimeDataIndex.Add(k);
                                        highlight = true;
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    case RadarType.Single:
                        for (int n = 0; n < serie.data.Count; n++)
                        {
                            var serieData = serie.data[n];
                            var symbolSize = serie.symbol.GetSize(serie.data[n].data, chart.theme.serie.lineSymbolSize);
                            if (Vector2.Distance(serieData.labelPosition, local) <= symbolSize * 1.3f)
                            {
                                chart.tooltip.runtimeDataIndex[0] = i;
                                chart.tooltip.runtimeDataIndex[1] = n;
                                highlight = true;
                                break;
                            }
                        }
                        break;
                }

            }

            if (!highlight)
            {
                if (chart.tooltip.IsActive())
                {
                    chart.tooltip.SetActive(false);
                    chart.RefreshChart();
                }
            }
            else
            {
                chart.tooltip.UpdateContentPos(local + chart.tooltip.offset);
                UpdateTooltip();
                chart.RefreshChart();
            }
            return highlight;
        }

        public bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Radar)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Radar)) return false;
            LegendHelper.CheckDataShow(chart.series, legendName, show);
            chart.UpdateLegendColor(legendName, show);
            chart.RefreshChart();
            return true;
        }

        public bool OnLegendButtonEnter(int index, string legendName)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Radar)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Radar)) return false;
            m_IsEnterLegendButtom = true;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, true);
            chart.RefreshChart();
            return true;
        }

        public bool OnLegendButtonExit(int index, string legendName)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Radar)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Radar)) return false;
            m_IsEnterLegendButtom = false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, false);
            chart.RefreshChart();
            return true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
        }

        private void InitRadars()
        {
            for (int n = 0; n < chart.radars.Count; n++)
            {
                Radar radar = chart.radars[n];
                radar.index = n;
                InitRadar(radar);
            }
        }

        private void InitRadar(Radar radar)
        {
            float txtWid = 100;
            float txtHig = 20;
            radar.painter = chart.GetPainter(radar.index);
            radar.refreshComponent = delegate ()
            {
                ChartHelper.HideAllObject(chart.transform, INDICATOR_TEXT + "_" + radar.index);
                radar.UpdateRadarCenter(chart.chartPosition, chart.chartWidth, chart.chartHeight);
                for (int i = 0; i < radar.indicatorList.Count; i++)
                {
                    var indicator = radar.indicatorList[i];
                    var pos = radar.GetIndicatorPosition(i);
                    var textStyle = indicator.textStyle;
                    var objName = INDICATOR_TEXT + "_" + radar.index + "_" + i;
                    var txt = ChartHelper.AddTextObject(objName, chart.transform, new Vector2(0.5f, 0.5f),
                        new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(txtWid, txtHig),
                        textStyle, chart.theme.radar);
                    txt.gameObject.hideFlags = chart.chartHideFlags;
                    txt.SetAlignment(textStyle.GetAlignment(TextAnchor.MiddleCenter));
                    txt.SetText(radar.indicatorList[i].name);
                    txt.SetActive(radar.indicator);
                    var offset = new Vector3(textStyle.offset.x, textStyle.offset.y);
                    AxisHelper.AdjustCircleLabelPos(txt, pos, radar.runtimeCenterPos, txtHig, offset);
                }
                if(chart.tooltip.gameObject != null)
                    chart.tooltip.gameObject.transform.SetSiblingIndex(chart.transform.childCount-1);
                chart.RefreshBasePainter();
            };
            radar.refreshComponent.Invoke();
        }

        private void DrawMutipleRadar(VertexHelper vh, Serie serie, int i)
        {
            if (!serie.show) return;
            var radar = chart.GetRadar(serie.radarIndex);
            if (radar == null) return;
            var startPoint = Vector3.zero;
            var toPoint = Vector3.zero;
            var firstPoint = Vector3.zero;
            var indicatorNum = radar.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var centerPos = radar.runtimeCenterPos;
            var serieNameCount = -1;
            serie.animation.InitProgress(1, 0, 1);
            if (!chart.IsActive(i) || serie.animation.HasFadeOut())
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
                if (!radar.runtimeDataPosList.ContainsKey(key))
                {
                    radar.runtimeDataPosList.Add(i * 1000 + j, new List<Vector3>(serieData.data.Count));
                }
                else
                {
                    radar.runtimeDataPosList[key].Clear();
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
                    serieNameCount++;
                    serieNameSet.Add(dataName, serieNameCount);
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
                var isHighlight = IsHighlight(radar, serie, serieData, j, 0);
                var areaColor = SerieHelper.GetAreaColor(serie, chart.theme, serieIndex, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, chart.theme, serieIndex, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, chart.theme, serieIndex, isHighlight);
                var lineWidth = serie.lineStyle.GetWidth(chart.theme.serie.lineWidth);
                int dataCount = radar.indicatorList.Count;
                List<Vector3> pointList = radar.runtimeDataPosList[key];
                for (int n = 0; n < dataCount; n++)
                {
                    if (n >= serieData.data.Count) break;
                    var max = radar.GetIndicatorMax(n);
                    var value = serieData.GetCurrData(n, dataChangeDuration);
                    if (serieData.IsDataChanged()) dataChanging = true;
                    if (max == 0)
                    {
                        max = serie.runtimeDataMax;
                    }
                    var radius = (float)(max < 0 ? radar.runtimeDataRadius - radar.runtimeDataRadius * value / max
                    : radar.runtimeDataRadius * value / max);
                    var currAngle = (n + (radar.positionType == Radar.PositionType.Between ? 0.5f : 0)) * angle;
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
                if (serie.symbol.show && serie.symbol.type != SerieSymbolType.None)
                {
                    for (int m = 0; m < pointList.Count; m++)
                    {
                        var point = pointList[m];
                        isHighlight = IsHighlight(radar, serie, serieData, j, m);
                        var symbolSize = isHighlight
                            ? serie.symbol.GetSelectedSize(null, chart.theme.serie.lineSymbolSelectedSize)
                            : serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize);
                        var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                        var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                        var backgroundColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
                        var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                        var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                        chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, point, symbolColor,
                           symbolToColor, backgroundColor, serie.symbol.gap, cornerRadius);
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

        private bool IsHighlight(Radar radar, Serie serie, SerieData serieData, int dataIndex, int dimension)
        {
            if (serie.highlighted || serieData.highlighted) return true;
            if (!chart.tooltip.show) return false;
            var selectedSerieIndex = chart.tooltip.runtimeDataIndex[0];
            if (selectedSerieIndex < 0) return false;
            if (chart.series.GetSerie(selectedSerieIndex).radarIndex != serie.radarIndex) return false;
            switch (serie.radarType)
            {
                case RadarType.Multiple:
                    if (radar.isAxisTooltip)
                    {
                        var selectedDimension = chart.tooltip.runtimeDataIndex[2];
                        return selectedDimension == dimension;
                    }
                    else if (chart.tooltip.runtimeDataIndex.Count >= 2)
                    {
                        return chart.tooltip.runtimeDataIndex[0] == serie.index && chart.tooltip.runtimeDataIndex[1] == dataIndex;
                    }
                    else
                    {
                        return false;
                    }
                case RadarType.Single:
                    return chart.tooltip.runtimeDataIndex[1] == dataIndex;
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

            var radar = chart.radars[serie.radarIndex];
            var indicatorNum = radar.indicatorList.Count;
            var angle = 2 * Mathf.PI / indicatorNum;
            var centerPos = radar.runtimeCenterPos;
            var serieNameCount = -1;
            serie.animation.InitProgress(1, 0, 1);
            if (!chart.IsActive(i) || serie.animation.HasFadeOut())
            {
                return;
            }
            var rate = serie.animation.GetCurrRate();
            var dataChanging = false;
            var dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            int key = i * 1000;
            if (!radar.runtimeDataPosList.ContainsKey(key))
            {
                radar.runtimeDataPosList.Add(i * 1000, new List<Vector3>(serie.dataCount));
            }
            else
            {
                radar.runtimeDataPosList[key].Clear();
            }
            var pointList = radar.runtimeDataPosList[key];
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
                    serieNameCount++;
                    serieNameSet.Add(dataName, serieNameCount);
                    serieIndex = serieNameCount;
                }
                else
                {
                    serieIndex = serieNameSet[dataName];
                }
                if (!serieData.show)
                {
                    serieData.labelPosition = Vector3.zero;
                    continue;
                }
                var isHighlight = IsHighlight(radar, serie, serieData, j, 0);
                var areaColor = SerieHelper.GetAreaColor(serie, chart.theme, serieIndex, isHighlight);
                var areaToColor = SerieHelper.GetAreaToColor(serie, chart.theme, serieIndex, isHighlight);
                var lineColor = SerieHelper.GetLineColor(serie, chart.theme, serieIndex, isHighlight);
                int dataCount = radar.indicatorList.Count;
                var index = serieData.index;
                var p = radar.runtimeCenterPos;
                var max = radar.GetIndicatorMax(index);
                var value = serieData.GetCurrData(1, dataChangeDuration);
                if (serieData.IsDataChanged()) dataChanging = true;
                if (max == 0)
                    max = serie.runtimeDataMax;
                if (!radar.IsInIndicatorRange(j, serieData.GetData(1)))
                {
                    lineColor = radar.outRangeColor;
                }
                var radius = (float)(max < 0 ? radar.runtimeDataRadius - radar.runtimeDataRadius * value / max
                : radar.runtimeDataRadius * value / max);
                var currAngle = (index + (radar.positionType == Radar.PositionType.Between ? 0.5f : 0)) * angle;
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
                serieData.labelPosition = startPoint;
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
            if (serie.symbol.show && serie.symbol.type != SerieSymbolType.None)
            {
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (!serieData.show) continue;
                    var isHighlight = serie.highlighted || serieData.highlighted ||
                    (chart.tooltip.show && chart.tooltip.runtimeDataIndex[0] == i && chart.tooltip.runtimeDataIndex[1] == j);
                    var serieIndex = serieData.index;
                    var symbolSize = isHighlight
                        ? serie.symbol.GetSelectedSize(serieData.data, chart.theme.serie.lineSymbolSelectedSize)
                        : serie.symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                    var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                    var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                    var backgroundColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
                    var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                    var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                    if (!radar.IsInIndicatorRange(j, serieData.GetData(1)))
                    {
                        symbolColor = radar.outRangeColor;
                        symbolToColor = radar.outRangeColor;
                    }
                    chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, serieData.labelPosition, symbolColor,
                           symbolToColor, backgroundColor, serie.symbol.gap, cornerRadius);
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
            if (serie.symbol.show && serie.symbol.type != SerieSymbolType.None)
            {
                var symbolSize = isHighlight
                    ? serie.symbol.GetSelectedSize(serieData.data, chart.theme.serie.lineSymbolSelectedSize)
                    : serie.symbol.GetSize(serieData.data, chart.theme.serie.lineSymbolSize);
                var symbolColor = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                var symbolToColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieIndex, isHighlight);
                var backgroundColor = SerieHelper.GetItemBackgroundColor(serie, serieData, chart.theme, serieIndex, isHighlight, false);
                var symbolBorder = SerieHelper.GetSymbolBorder(serie, serieData, chart.theme, isHighlight);
                var cornerRadius = SerieHelper.GetSymbolCornerRadius(serie, serieData, isHighlight);
                foreach (var point in pointList)
                {
                    chart.DrawSymbol(vh, serie.symbol.type, symbolSize, symbolBorder, point, symbolColor,
                       symbolToColor, backgroundColor, serie.symbol.gap, cornerRadius);
                }
            }
        }

        private void DrawRadar(VertexHelper vh, Radar radar)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = radar.runtimeRadius / radar.splitNumber;
            int indicatorNum = radar.indicatorList.Count;
            Vector3 p1, p2, p3, p4;
            Vector3 p = radar.runtimeCenterPos;
            float angle = 2 * Mathf.PI / indicatorNum;
            var lineColor = radar.axisLine.GetColor(chart.theme.radar.lineColor);
            var lineWidth = radar.axisLine.GetWidth(chart.theme.radar.lineWidth);
            var lineType = radar.axisLine.GetType(chart.theme.radar.lineType);
            var splitLineColor = radar.splitLine.GetColor(chart.theme.radar.splitLineColor);
            var splitLineWidth = radar.splitLine.GetWidth(chart.theme.radar.splitLineWidth);
            var splitLineType = radar.splitLine.GetType(chart.theme.radar.splitLineType);
            for (int i = 0; i < radar.splitNumber; i++)
            {
                var color = radar.splitArea.GetColor(i, chart.theme.radar);
                outsideRadius = insideRadius + block;
                p1 = new Vector3(p.x + insideRadius * Mathf.Sin(0), p.y + insideRadius * Mathf.Cos(0));
                p2 = new Vector3(p.x + outsideRadius * Mathf.Sin(0), p.y + outsideRadius * Mathf.Cos(0));
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = j * angle;
                    p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    p4 = new Vector3(p.x + insideRadius * Mathf.Sin(currAngle),
                        p.y + insideRadius * Mathf.Cos(currAngle));
                    if (radar.splitArea.show)
                    {
                        UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, color);
                    }
                    if (radar.splitLine.NeedShow(i))
                    {
                        ChartDrawer.DrawLineStyle(vh, splitLineType, splitLineWidth, p2, p3, splitLineColor);
                    }
                    p1 = p4;
                    p2 = p3;
                }
                insideRadius = outsideRadius;
            }
            if (radar.axisLine.show)
            {
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = j * angle;
                    p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, p, p3, lineColor);
                }
            }
        }

        private void DrawCricleRadar(VertexHelper vh, Radar radar)
        {
            float insideRadius = 0, outsideRadius = 0;
            float block = radar.runtimeRadius / radar.splitNumber;
            int indicatorNum = radar.indicatorList.Count;
            Vector3 p = radar.runtimeCenterPos;
            Vector3 p1;
            float angle = 2 * Mathf.PI / indicatorNum;
            var lineColor = radar.axisLine.GetColor(chart.theme.radar.lineColor);
            var lineWidth = radar.axisLine.GetWidth(chart.theme.radar.lineWidth);
            var lineType = radar.axisLine.GetType(chart.theme.radar.lineType);
            var splitLineColor = radar.splitLine.GetColor(chart.theme.radar.splitLineColor);
            var splitLineWidth = radar.splitLine.GetWidth(chart.theme.radar.splitLineWidth);
            for (int i = 0; i < radar.splitNumber; i++)
            {
                var color = radar.splitArea.GetColor(i, chart.theme.radiusAxis);
                outsideRadius = insideRadius + block;
                if (radar.splitArea.show)
                {
                    UGL.DrawDoughnut(vh, p, insideRadius, outsideRadius, color, Color.clear,
                          0, 360, chart.settings.cicleSmoothness);
                }
                if (radar.splitLine.show)
                {
                    UGL.DrawEmptyCricle(vh, p, outsideRadius, splitLineWidth, splitLineColor,
                        Color.clear, chart.settings.cicleSmoothness);
                }
                insideRadius = outsideRadius;
            }
            if (radar.axisLine.show)
            {
                for (int j = 0; j <= indicatorNum; j++)
                {
                    float currAngle = j * angle;
                    p1 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                        p.y + outsideRadius * Mathf.Cos(currAngle));
                    ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, p, p1, lineColor);
                }
            }
        }

        private bool IsInRadar(Vector2 local)
        {
            foreach (var radar in chart.radars)
            {
                var dist = Vector2.Distance(radar.runtimeCenterPos, local);
                if (dist < radar.runtimeRadius + chart.theme.serie.lineSymbolSize)
                {
                    return true;
                }
            }
            return false;
        }

        protected void UpdateTooltip()
        {
            int serieIndex = chart.tooltip.runtimeDataIndex[0];
            if (serieIndex < 0)
            {
                if (chart.tooltip.IsActive())
                {
                    chart.tooltip.SetActive(false);
                    chart.RefreshChart();
                }
                return;
            }
            chart.tooltip.SetActive(true);
            var serie = chart.series.GetSerie(serieIndex);
            var radar = chart.radars[serie.radarIndex];
            var dataIndex = chart.tooltip.runtimeDataIndex[1];
            var content = TooltipHelper.GetFormatterContent(chart.tooltip, dataIndex, chart, null, false, radar);
            TooltipHelper.SetContentAndPosition(chart.tooltip, content, chart.chartRect);
        }

        private bool IsAnyRadarDirty()
        {
            foreach (var radar in chart.radars)
            {
                if (radar.anyDirty) return true;
            }
            return false;
        }
    }
}