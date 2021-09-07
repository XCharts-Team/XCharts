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

namespace XCharts
{
    internal class DrawSeriePie : IDrawSerie
    {
        public BaseChart chart;
        protected bool m_IsEnterPieLegendButtom;

        public DrawSeriePie(BaseChart chart)
        {
            this.chart = chart;
        }

        public void InitComponent()
        {
        }

        public void CheckComponent()
        {
        }

        public void Update()
        {
        }

        public void DrawBase(VertexHelper vh)
        {
        }

        public void DrawSerie(VertexHelper vh, Serie serie)
        {
            if (serie.type != SerieType.Pie) return;
            UpdateRuntimeData(serie);
            DrawPieLabelLine(vh, serie);
            DrawPie(vh, serie);
            DrawPieLabelBackground(vh, serie);
        }

        public void RefreshLabel()
        {
            int serieNameCount = -1;
            for (int i = 0; i < chart.series.Count; i++)
            {
                var serie = chart.series.list[i];
                serie.index = i;
                if (!serie.show || serie.type != SerieType.Pie) continue;
                var data = serie.data;
                for (int n = 0; n < data.Count; n++)
                {
                    var serieData = data[n];
                    if (!serieData.canShowLabel || serie.IsIgnoreValue(serieData))
                    {
                        serieData.SetLabelActive(false);
                        continue;
                    }
                    if (!serieData.show) continue;

                    serieNameCount = chart.m_LegendRealShowName.IndexOf(serieData.name);
                    Color color = chart.theme.GetColor(serieNameCount);
                    DrawPieLabel(serie, n, serieData, color);
                }
            }
        }

        public bool CheckTootipArea(Vector2 local)
        {
            if (!chart.series.Contains(SerieType.Pie)) return false;
            bool selected = false;
            chart.tooltip.runtimeDataIndex.Clear();
            foreach (var serie in chart.series.list)
            {
                int index = GetPiePosIndex(serie, local);
                chart.tooltip.runtimeDataIndex.Add(index);
                if (serie.type != SerieType.Pie) continue;
                bool refresh = false;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (serieData.highlighted != (j == index)) refresh = true;
                    serieData.highlighted = j == index;
                }
                if (index >= 0) selected = true;
                if (refresh) chart.RefreshChart();
            }
            if (selected)
            {
                chart.tooltip.UpdateContentPos(local + chart.tooltip.offset);
                UpdatePieTooltip();
            }
            else if (chart.tooltip.IsActive())
            {
                chart.tooltip.SetActive(false);
                chart.RefreshChart();
            }
            return selected;
        }

        public bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Pie)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Pie)) return false;
            LegendHelper.CheckDataShow(chart.series, legendName, show);
            chart.UpdateLegendColor(legendName, show);
            chart.RefreshChart();
            return true;
        }

        public bool OnLegendButtonEnter(int index, string legendName)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Pie)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Pie)) return false;
            m_IsEnterPieLegendButtom = true;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, true);
            chart.RefreshChart();
            return true;
        }

        public bool OnLegendButtonExit(int index, string legendName)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Pie)) return false;
            if (!LegendHelper.IsSerieLegend(chart, legendName, SerieType.Pie)) return false;
            m_IsEnterPieLegendButtom = false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, false);
            chart.RefreshChart();
            return true;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (!SeriesHelper.ContainsSerie(chart.series, SerieType.Pie)) return;
            if (chart.pointerPos == Vector2.zero) return;
            var refresh = false;
            for (int i = 0; i < chart.series.Count; i++)
            {
                var serie = chart.series.GetSerie(i);
                if (serie.type != SerieType.Pie) continue;
                var index = GetPiePosIndex(serie, chart.pointerPos);
                if (index >= 0)
                {
                    refresh = true;
                    for (int j = 0; j < serie.data.Count; j++)
                    {
                        if (j == index) serie.data[j].selected = !serie.data[j].selected;
                        else serie.data[j].selected = false;
                    }
                    if (chart.onPointerClickPie != null)
                    {
                        chart.onPointerClickPie(eventData, i, index);
                    }
                }
            }
            if (refresh) chart.RefreshChart();
        }

        private void UpdateRuntimeData(Serie serie)
        {
            var data = serie.data;
            serie.runtimeDataMax = serie.yMax;
            serie.runtimePieDataTotal = serie.yTotal;

            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            float totalDegree = 0;
            float startDegree = 0;
            float zeroReplaceValue = 0;
            int showdataCount = 0;
            foreach (var sd in serie.data)
            {
                if (sd.show && serie.pieRoseType == RoseType.Area) showdataCount++;
                sd.canShowLabel = false;
            }
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            bool isAllZeroValue = SerieHelper.IsAllZeroValue(serie, 1);
            var dataTotalFilterMinAngle = serie.runtimePieDataTotal;
            if (isAllZeroValue)
            {
                totalDegree = 360;
                zeroReplaceValue = totalDegree / data.Count;
                serie.runtimeDataMax = zeroReplaceValue;
                serie.runtimePieDataTotal = 360;
                dataTotalFilterMinAngle = 360;
            }
            else
            {
                dataTotalFilterMinAngle = GetTotalAngle(serie, serie.runtimePieDataTotal, ref totalDegree);
            }
            serie.animation.InitProgress(data.Count, 0, 360);
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                serieData.index = n;
                var value = isAllZeroValue ? zeroReplaceValue : serieData.GetCurrData(1, dataChangeDuration);
                serieData.runtimePieStartAngle = startDegree;
                serieData.runtimePieToAngle = startDegree;
                serieData.runtimePieHalfAngle = startDegree;
                serieData.runtimePieCurrAngle = startDegree;
                if (!serieData.show)
                {
                    continue;
                }
                float degree = serie.pieRoseType == RoseType.Area
                    ? (totalDegree / showdataCount)
                    : (float)(totalDegree * value / dataTotalFilterMinAngle);
                if (serie.minAngle > 0 && degree < serie.minAngle) degree = serie.minAngle;
                serieData.runtimePieToAngle = startDegree + degree;
                serieData.runtimePieOutsideRadius = serie.pieRoseType > 0 ?
                    serie.runtimeInsideRadius + (float)((serie.runtimeOutsideRadius - serie.runtimeInsideRadius) * value / serie.runtimeDataMax) :
                    serie.runtimeOutsideRadius;
                if (serieData.highlighted)
                {
                    serieData.runtimePieOutsideRadius += chart.theme.serie.pieTooltipExtraRadius;
                }
                var offset = 0f;
                if (serie.pieClickOffset && serieData.selected)
                {
                    offset += chart.theme.serie.pieSelectedOffset;
                }
                if (serie.animation.CheckDetailBreak(serieData.runtimePieToAngle))
                {
                    serieData.runtimePieCurrAngle = serie.animation.GetCurrDetail();
                }
                else
                {
                    serieData.runtimePieCurrAngle = serieData.runtimePieToAngle;
                }
                var halfDegree = (serieData.runtimePieToAngle - startDegree) / 2;
                serieData.runtimePieHalfAngle = startDegree + halfDegree;
                serieData.runtiemPieOffsetCenter = serie.runtimeCenterPos;
                serieData.runtimePieInsideRadius = serie.runtimeInsideRadius;
                if (offset > 0)
                {
                    var currRad = serieData.runtimePieHalfAngle * Mathf.Deg2Rad;
                    var currSin = Mathf.Sin(currRad);
                    var currCos = Mathf.Cos(currRad);
                    serieData.runtimePieOffsetRadius = 0;
                    serieData.runtimePieInsideRadius -= serieData.runtimePieOffsetRadius;
                    serieData.runtimePieOutsideRadius -= serieData.runtimePieOffsetRadius;
                    if (serie.pieClickOffset && serieData.selected)
                    {
                        serieData.runtimePieOffsetRadius += chart.theme.serie.pieSelectedOffset;
                        if (serieData.runtimePieInsideRadius > 0)
                        {
                            serieData.runtimePieInsideRadius += chart.theme.serie.pieSelectedOffset;
                        }
                        serieData.runtimePieOutsideRadius += chart.theme.serie.pieSelectedOffset;
                    }
                    serieData.runtiemPieOffsetCenter = new Vector3(
                        serie.runtimeCenterPos.x + serieData.runtimePieOffsetRadius * currSin,
                        serie.runtimeCenterPos.y + serieData.runtimePieOffsetRadius * currCos);
                }
                serieData.canShowLabel = serieData.runtimePieCurrAngle >= serieData.runtimePieHalfAngle;
                startDegree = serieData.runtimePieToAngle;
                SerieLabelHelper.UpdatePieLabelPosition(serie, serieData);
            }
            SerieLabelHelper.AvoidLabelOverlap(serie, chart.theme.common);
        }

        private double GetTotalAngle(Serie serie, double dataTotal, ref float totalAngle)
        {
            totalAngle = 360f;
            if (serie.minAngle > 0)
            {
                var rate = serie.minAngle / 360;
                var minAngleValue = dataTotal * rate;
                foreach (var serieData in serie.data)
                {
                    var value = serieData.GetData(1);
                    if (value < minAngleValue)
                    {
                        totalAngle -= serie.minAngle;
                        dataTotal -= value;
                    }
                }
                return dataTotal;
            }
            else
            {
                return dataTotal;
            }
        }

        private void DrawPieCenter(VertexHelper vh, Serie serie, ItemStyle itemStyle, float insideRadius)
        {
            if (!ChartHelper.IsClearColor(itemStyle.centerColor))
            {
                var radius = insideRadius - itemStyle.centerGap;
                UGL.DrawCricle(vh, serie.runtimeCenterPos, radius, itemStyle.centerColor, chart.settings.cicleSmoothness);
            }
        }

        private void DrawPie(VertexHelper vh, Serie serie)
        {
            var data = serie.data;
            serie.animation.InitProgress(data.Count, 0, 360);
            if (!serie.show || serie.animation.HasFadeOut())
            {
                return;
            }
            bool dataChanging = false;
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                if (!serieData.show)
                {
                    continue;
                }
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, serieData.highlighted);
                if (serieData.IsDataChanged()) dataChanging = true;
                var serieNameCount = chart.m_LegendRealShowName.IndexOf(serieData.legendName);
                var color = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieNameCount,
                    serieData.highlighted);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieNameCount,
                    serieData.highlighted);
                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;

                if (serie.pieClickOffset && serieData.selected)
                {
                    var drawEndDegree = serieData.runtimePieCurrAngle;
                    var needRoundCap = serie.roundCap && serieData.runtimePieInsideRadius > 0;
                    UGL.DrawDoughnut(vh, serieData.runtiemPieOffsetCenter, serieData.runtimePieInsideRadius,
                        serieData.runtimePieOutsideRadius, color, toColor, Color.clear, serieData.runtimePieStartAngle,
                        drawEndDegree, borderWidth, borderColor, serie.pieSpace / 2, chart.settings.cicleSmoothness,
                        needRoundCap, true);
                }
                else
                {
                    var drawEndDegree = serieData.runtimePieCurrAngle;
                    var needRoundCap = serie.roundCap && serieData.runtimePieInsideRadius > 0;
                    UGL.DrawDoughnut(vh, serie.runtimeCenterPos, serieData.runtimePieInsideRadius,
                        serieData.runtimePieOutsideRadius, color, toColor, Color.clear, serieData.runtimePieStartAngle,
                        drawEndDegree, borderWidth, borderColor, serie.pieSpace / 2, chart.settings.cicleSmoothness,
                        needRoundCap, true);
                    DrawPieCenter(vh, serie, itemStyle, serieData.runtimePieInsideRadius);
                }
                if (!serie.animation.CheckDetailBreak(serieData.runtimePieToAngle)) serie.animation.SetDataFinish(n);
                else break;
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(360);
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize));
                chart.RefreshPainter(serie);
            }
            if (dataChanging)
            {
                chart.RefreshPainter(serie);
            }
            chart.raycastTarget = IsAnyPieClickOffset() || IsAnyPieDataHighlight();
        }

        private bool IsAnyPieClickOffset()
        {
            foreach (var serie in chart.series.list)
            {
                if (serie.type == SerieType.Pie && serie.pieClickOffset) return true;
            }
            return false;
        }

        private bool IsAnyPieDataHighlight()
        {
            foreach (var serie in chart.series.list)
            {
                if (serie.type == SerieType.Pie)
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.highlighted) return true;
                    }
                }
            }
            return false;
        }

        private void DrawPieLabelLine(VertexHelper vh, Serie serie)
        {
            foreach (var serieData in serie.data)
            {
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                if (SerieLabelHelper.CanShowLabel(serie, serieData, serieLabel, 1))
                {
                    int colorIndex = chart.m_LegendRealShowName.IndexOf(serieData.name);
                    Color color = chart.theme.GetColor(colorIndex);
                    DrawPieLabelLine(vh, serie, serieData, color);
                }
            }
        }

        private void DrawPieLabelBackground(VertexHelper vh, Serie serie)
        {
            if (serie.avoidLabelOverlap) return;
            foreach (var serieData in serie.data)
            {
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                if (SerieLabelHelper.CanShowLabel(serie, serieData, serieLabel, 1))
                {
                    SerieLabelHelper.UpdatePieLabelPosition(serie, serieData);
                    chart.DrawLabelBackground(vh, serie, serieData);
                }
            }
        }

        private void DrawPieLabelLine(VertexHelper vh, Serie serie, SerieData serieData, Color color)
        {
            if (serie.animation.enable && serie.animation.HasFadeOut()) return;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (serieLabel.show
                && serieLabel.position == SerieLabel.Position.Outside
                && serieLabel.line)
            {
                var insideRadius = serieData.runtimePieInsideRadius;
                var outSideRadius = serieData.runtimePieOutsideRadius;
                var center = serie.runtimeCenterPos;
                var currAngle = serieData.runtimePieHalfAngle;
                if (!ChartHelper.IsClearColor(serieLabel.lineColor)) color = serieLabel.lineColor;
                else if (serieLabel.lineType == SerieLabel.LineType.HorizontalLine) color *= color;
                float currSin = Mathf.Sin(currAngle * Mathf.Deg2Rad);
                float currCos = Mathf.Cos(currAngle * Mathf.Deg2Rad);
                var radius1 = serieLabel.lineType == SerieLabel.LineType.HorizontalLine ?
                    serie.runtimeOutsideRadius : outSideRadius;
                var radius2 = serie.runtimeOutsideRadius + serieLabel.lineLength1;
                var radius3 = insideRadius + (outSideRadius - insideRadius) / 2;
                if (radius1 < serie.runtimeInsideRadius) radius1 = serie.runtimeInsideRadius;
                radius1 -= 0.1f;
                var pos0 = new Vector3(center.x + radius3 * currSin, center.y + radius3 * currCos);
                var pos1 = new Vector3(center.x + radius1 * currSin, center.y + radius1 * currCos);
                var pos2 = serieData.labelPosition;
                if (pos2.x == 0)
                {
                    pos2 = new Vector3(center.x + radius2 * currSin, center.y + radius2 * currCos);
                }
                Vector3 pos4, pos6;
                var horizontalLineCircleRadius = serieLabel.lineWidth * 4f;
                var lineCircleDiff = horizontalLineCircleRadius - 0.3f;
                if (currAngle < 90)
                {
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += serieLabel.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 180)
                {
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += serieLabel.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 270)
                {
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += serieLabel.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                else
                {
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += serieLabel.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                var pos5X = currAngle > 180 ? pos2.x - serieLabel.lineLength2 : pos2.x + serieLabel.lineLength2;
                var pos5 = new Vector3(pos5X, pos2.y);
                switch (serieLabel.lineType)
                {
                    case SerieLabel.LineType.BrokenLine:
                        UGL.DrawLine(vh, pos1, pos2, pos5, serieLabel.lineWidth, color);
                        break;
                    case SerieLabel.LineType.Curves:
                        UGL.DrawCurves(vh, pos1, pos5, pos1, pos2, serieLabel.lineWidth, color,
                            chart.settings.lineSmoothness);
                        break;
                    case SerieLabel.LineType.HorizontalLine:
                        UGL.DrawCricle(vh, pos0, horizontalLineCircleRadius, color);
                        UGL.DrawLine(vh, pos6, pos4, serieLabel.lineWidth, color);
                        break;
                }
            }
        }

        private void DrawPieLabel(Serie serie, int dataIndex, SerieData serieData, Color serieColor)
        {
            if (serieData.labelObject == null) return;
            if (serie.animation.enable && serie.animation.HasFadeOut()) return;
            var currAngle = serieData.runtimePieHalfAngle;
            var isHighlight = (serieData.highlighted && serie.emphasis.label.show);
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            var iconStyle = SerieHelper.GetIconStyle(serie, serieData);
            var showLabel = ((serieLabel.show || isHighlight) && serieData.canShowLabel);
            if (showLabel)
            {
                serieData.SetLabelActive(showLabel);
                float rotate = 0;
                bool isInsidePosition = serieLabel.position == SerieLabel.Position.Inside;
                if (serieLabel.textStyle.rotate > 0 && isInsidePosition)
                {
                    if (currAngle > 180) rotate += 270 - currAngle;
                    else rotate += -(currAngle - 90);
                }
                Color color = serieColor;
                if (isHighlight)
                {
                    if (!ChartHelper.IsClearColor(serie.emphasis.label.textStyle.color))
                    {
                        color = serie.emphasis.label.textStyle.color;
                    }
                }
                else if (!ChartHelper.IsClearColor(serieLabel.textStyle.color))
                {
                    color = serieLabel.textStyle.color;
                }
                else
                {
                    color = isInsidePosition ? Color.white : serieColor;
                }
                var fontSize = isHighlight
                    ? serie.emphasis.label.textStyle.GetFontSize(chart.theme.common)
                    : serieLabel.textStyle.GetFontSize(chart.theme.common);
                var fontStyle = isHighlight
                    ? serie.emphasis.label.textStyle.fontStyle
                    : serieLabel.textStyle.fontStyle;

                serieData.labelObject.label.SetColor(color);
                serieData.labelObject.label.SetFontSize(fontSize);
                serieData.labelObject.label.SetFontStyle(fontStyle);
                serieData.labelObject.SetLabelRotate(rotate);
                if (!string.IsNullOrEmpty(serieLabel.formatter))
                {
                    var value = serieData.data[1];
                    var total = serie.yTotal;
                    var content = SerieLabelHelper.GetFormatterContent(serie, serieData, value, total,
                        serieLabel, serieColor);
                    if (serieData.labelObject.SetText(content)) chart.RefreshPainter(serie);
                }
                else
                {
                    if (serieData.labelObject.SetText(serieData.name)) chart.RefreshPainter(serie);
                }
                serieData.labelObject.SetPosition(SerieLabelHelper.GetRealLabelPosition(serieData, serieLabel));
                if (showLabel) serieData.labelObject.SetLabelPosition(serieLabel.offset);
                else serieData.SetLabelActive(false);
            }
            else
            {
                serieData.SetLabelActive(false);
            }
            serieData.labelObject.UpdateIcon(iconStyle);
        }

        protected int GetPiePosIndex(Serie serie, Vector2 local)
        {
            if (serie.type != SerieType.Pie) return -1;
            var dist = Vector2.Distance(local, serie.runtimeCenterPos);
            var maxRadius = serie.runtimeOutsideRadius + 3 * chart.theme.serie.pieSelectedOffset;
            if (dist < serie.runtimeInsideRadius || dist > maxRadius) return -1;
            Vector2 dir = local - new Vector2(serie.runtimeCenterPos.x, serie.runtimeCenterPos.y);
            float angle = ChartHelper.GetAngle360(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (angle >= serieData.runtimePieStartAngle && angle <= serieData.runtimePieToAngle)
                {
                    var ndist = !serieData.selected ? dist :
                         Vector2.Distance(local, serieData.runtiemPieOffsetCenter);
                    if (ndist >= serieData.runtimePieInsideRadius && ndist <= serieData.runtimePieOutsideRadius)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private bool PointerIsInPieSerie(Series series, Vector2 local)
        {
            foreach (var serie in series.list)
            {
                if (serie.type != SerieType.Pie) continue;
                var dist = Vector2.Distance(local, serie.runtimeCenterPos);
                if (dist >= serie.runtimeInsideRadius && dist <= serie.runtimeOutsideRadius) return true;
            }
            return false;
        }

        protected void UpdatePieTooltip()
        {
            bool showTooltip = false;
            foreach (var serie in chart.series.list)
            {
                int index = chart.tooltip.runtimeDataIndex[serie.index];
                if (index < 0) continue;
                showTooltip = true;
                var content = TooltipHelper.GetFormatterContent(chart.tooltip, index, chart);
                TooltipHelper.SetContentAndPosition(chart.tooltip, content.TrimStart(), chart.chartRect);
            }
            chart.tooltip.SetActive(showTooltip);
        }
    }
}