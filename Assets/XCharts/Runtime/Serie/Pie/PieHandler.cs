/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class PieHandler : SerieHandler<Pie>
    {
        public override void Update()
        {
            UpdateSerieContext();
        }

        public override void DrawSerie(VertexHelper vh)
        {
            UpdateRuntimeData(serie);
            DrawPieLabelLine(vh, serie);
            DrawPie(vh, serie);
        }

        public override bool SetDefaultTooltipContent(Tooltip tooltip, StringBuilder sb)
        {
            if (!serie.context.pointerEnter || serie.context.pointerItemDataIndex < 0) return false;
            var serieData = serie.GetSerieData(serie.context.pointerItemDataIndex);
            if (serieData == null) return false;
            var key = serieData.name;
            var numericFormatter = TooltipHelper.GetItemNumericFormatter(tooltip, serie, serieData);
            var value = serieData.GetData(1);
            if (!string.IsNullOrEmpty(serie.serieName))
            {
                sb.Append(serie.serieName).Append(FormatterHelper.PH_NN);
            }
            sb.Append("<color=#").Append(chart.theme.GetColorStr(serie.context.pointerItemDataIndex)).Append(">● </color>");
            if (!string.IsNullOrEmpty(key))
                sb.Append(key).Append(": ");
            sb.Append(ChartCached.FloatToStr(value, numericFormatter));
            return true;
        }

        public override void RefreshLabelInternal()
        {
            var data = serie.data;
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                if (!serieData.context.canShowLabel || serie.IsIgnoreValue(serieData))
                {
                    serieData.SetLabelActive(false);
                    continue;
                }
                if (!serieData.show) continue;

                var colorIndex = chart.GetLegendRealShowNameIndex(serieData.name);
                Color color = chart.theme.GetColor(colorIndex);
                DrawPieLabel(serie, n, serieData, color);
            }
        }

        public override bool OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (!chart.HasSerie<Pie>()) return false;
            if (!LegendHelper.IsSerieLegend<Pie>(chart, legendName)) return false;
            LegendHelper.CheckDataShow(chart.series, legendName, show);
            chart.UpdateLegendColor(legendName, show);
            chart.RefreshChart();
            return true;
        }

        public override bool OnLegendButtonEnter(int index, string legendName)
        {
            if (!chart.HasSerie<Pie>()) return false;
            if (!LegendHelper.IsSerieLegend<Pie>(chart, legendName)) return false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, true);
            chart.RefreshChart();
            return true;
        }

        public override bool OnLegendButtonExit(int index, string legendName)
        {
            if (!chart.HasSerie<Pie>()) return false;
            if (!LegendHelper.IsSerieLegend<Pie>(chart, legendName)) return false;
            LegendHelper.CheckDataHighlighted(chart.series, legendName, false);
            chart.RefreshChart();
            return true;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (!chart.HasSerie<Pie>()) return;
            if (chart.pointerPos == Vector2.zero) return;
            var refresh = false;
            for (int i = 0; i < chart.series.Count; i++)
            {
                var serie = chart.GetSerie(i);
                if (!(serie is Pie)) continue;
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

        private void UpdateSerieContext()
        {
            if (!chart.isPointerInChart) return;
            var lastPointerEnter = serie.context.pointerEnter;
            serie.context.pointerEnter = PointerIsInPieSerie(serie, chart.pointerPos);
            if (serie.context.pointerEnter)
            {
                var lastDataIndex = serie.context.pointerItemDataIndex;
                var dataIndex = GetPiePosIndex(serie, chart.pointerPos);
                if (dataIndex >= 0)
                {
                    if (lastDataIndex >= 0)
                        serie.GetSerieData(lastDataIndex).context.highlight = false;
                    if (lastDataIndex != dataIndex)
                        chart.RefreshPainter(serie);
                    serie.GetSerieData(dataIndex).context.highlight = true;
                    serie.context.pointerItemDataIndex = dataIndex;
                }
                else
                {
                    if (lastDataIndex >= 0)
                        serie.GetSerieData(lastDataIndex).context.highlight = false;
                    serie.context.pointerItemDataIndex = -1;
                }
            }
            else
            {
                if (lastPointerEnter)
                {
                    foreach (var serieData in serie.data)
                        serieData.context.highlight = false;
                }
                serie.context.pointerItemDataIndex = -1;
            }
        }

        private void UpdateRuntimeData(Serie serie)
        {
            var data = serie.data;
            serie.context.dataMax = serie.yMax;
            var runtimePieDataTotal = serie.yTotal;

            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            float totalDegree = 0;
            float startDegree = 0;
            float zeroReplaceValue = 0;
            int showdataCount = 0;
            foreach (var sd in serie.data)
            {
                if (sd.show && serie.pieRoseType == RoseType.Area) showdataCount++;
                sd.context.canShowLabel = false;
            }
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            bool isAllZeroValue = SerieHelper.IsAllZeroValue(serie, 1);
            var dataTotalFilterMinAngle = runtimePieDataTotal;
            if (isAllZeroValue)
            {
                totalDegree = 360;
                zeroReplaceValue = totalDegree / data.Count;
                serie.context.dataMax = zeroReplaceValue;
                runtimePieDataTotal = 360;
                dataTotalFilterMinAngle = 360;
            }
            else
            {
                dataTotalFilterMinAngle = GetTotalAngle(serie, runtimePieDataTotal, ref totalDegree);
            }
            serie.animation.InitProgress(data.Count, 0, 360);
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                serieData.index = n;
                var value = isAllZeroValue ? zeroReplaceValue : serieData.GetCurrData(1, dataChangeDuration);
                serieData.context.startAngle = startDegree;
                serieData.context.toAngle = startDegree;
                serieData.context.halfAngle = startDegree;
                serieData.context.currentAngle = startDegree;
                if (!serieData.show)
                {
                    continue;
                }
                float degree = serie.pieRoseType == RoseType.Area
                    ? (totalDegree / showdataCount)
                    : (float)(totalDegree * value / dataTotalFilterMinAngle);
                if (serie.minAngle > 0 && degree < serie.minAngle) degree = serie.minAngle;
                serieData.context.toAngle = startDegree + degree;
                if (serieData.radius > 0)
                    serieData.context.outsideRadius = serieData.radius;
                else
                    serieData.context.outsideRadius = serie.pieRoseType > 0 ?
                    serie.context.insideRadius + (float)((serie.context.outsideRadius - serie.context.insideRadius) * value / serie.context.dataMax) :
                    serie.context.outsideRadius;
                if (serieData.context.highlight)
                {
                    serieData.context.outsideRadius += chart.theme.serie.pieTooltipExtraRadius;
                }
                var offset = 0f;
                if (serie.pieClickOffset && serieData.selected)
                {
                    offset += chart.theme.serie.pieSelectedOffset;
                }
                if (serie.animation.CheckDetailBreak(serieData.context.toAngle))
                {
                    serieData.context.currentAngle = serie.animation.GetCurrDetail();
                }
                else
                {
                    serieData.context.currentAngle = serieData.context.toAngle;
                }
                var halfDegree = (serieData.context.toAngle - startDegree) / 2;
                serieData.context.halfAngle = startDegree + halfDegree;
                serieData.context.offsetCenter = serie.context.center;
                serieData.context.insideRadius = serie.context.insideRadius;
                if (offset > 0)
                {
                    var currRad = serieData.context.halfAngle * Mathf.Deg2Rad;
                    var currSin = Mathf.Sin(currRad);
                    var currCos = Mathf.Cos(currRad);
                    serieData.context.offsetRadius = 0;
                    serieData.context.insideRadius -= serieData.context.offsetRadius;
                    serieData.context.outsideRadius -= serieData.context.offsetRadius;
                    if (serie.pieClickOffset && serieData.selected)
                    {
                        serieData.context.offsetRadius += chart.theme.serie.pieSelectedOffset;
                        if (serieData.context.insideRadius > 0)
                        {
                            serieData.context.insideRadius += chart.theme.serie.pieSelectedOffset;
                        }
                        serieData.context.outsideRadius += chart.theme.serie.pieSelectedOffset;
                    }
                    serieData.context.offsetCenter = new Vector3(
                        serie.context.center.x + serieData.context.offsetRadius * currSin,
                        serie.context.center.y + serieData.context.offsetRadius * currCos);
                }
                serieData.context.canShowLabel = serieData.context.currentAngle >= serieData.context.halfAngle;
                startDegree = serieData.context.toAngle;
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
                UGL.DrawCricle(vh, serie.context.center, radius, itemStyle.centerColor, chart.settings.cicleSmoothness);
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
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData, serieData.context.highlight);
                if (serieData.IsDataChanged()) dataChanging = true;
                var serieNameCount = chart.m_LegendRealShowName.IndexOf(serieData.legendName);
                var color = SerieHelper.GetItemColor(serie, serieData, chart.theme, serieNameCount,
                    serieData.context.highlight);
                var toColor = SerieHelper.GetItemToColor(serie, serieData, chart.theme, serieNameCount,
                    serieData.context.highlight);
                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;

                if (serie.pieClickOffset && serieData.selected)
                {
                    var drawEndDegree = serieData.context.currentAngle;
                    var needRoundCap = serie.roundCap && serieData.context.insideRadius > 0;
                    UGL.DrawDoughnut(vh, serieData.context.offsetCenter, serieData.context.insideRadius,
                        serieData.context.outsideRadius, color, toColor, Color.clear, serieData.context.startAngle,
                        drawEndDegree, borderWidth, borderColor, serie.pieSpace / 2, chart.settings.cicleSmoothness,
                        needRoundCap, true);
                }
                else
                {
                    var drawEndDegree = serieData.context.currentAngle;
                    var needRoundCap = serie.roundCap && serieData.context.insideRadius > 0;
                    UGL.DrawDoughnut(vh, serie.context.center, serieData.context.insideRadius,
                        serieData.context.outsideRadius, color, toColor, Color.clear, serieData.context.startAngle,
                        drawEndDegree, borderWidth, borderColor, serie.pieSpace / 2, chart.settings.cicleSmoothness,
                        needRoundCap, true);
                    DrawPieCenter(vh, serie, itemStyle, serieData.context.insideRadius);
                }
                if (!serie.animation.CheckDetailBreak(serieData.context.toAngle)) serie.animation.SetDataFinish(n);
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
        }

        private bool IsAnyPieClickOffset()
        {
            foreach (var serie in chart.series)
            {
                if (serie is Pie && serie.pieClickOffset) return true;
            }
            return false;
        }

        private bool IsAnyPieDataHighlight()
        {
            foreach (var serie in chart.series)
            {
                if (serie is Pie)
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.context.highlight) return true;
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

        private void DrawPieLabelLine(VertexHelper vh, Serie serie, SerieData serieData, Color color)
        {
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
            if (serieLabel.show
                && serieLabel.position == LabelStyle.Position.Outside
                && labelLine.show)
            {
                var insideRadius = serieData.context.insideRadius;
                var outSideRadius = serieData.context.outsideRadius;
                var center = serie.context.center;
                var currAngle = serieData.context.halfAngle;
                if (!ChartHelper.IsClearColor(labelLine.lineColor)) color = labelLine.lineColor;
                else if (labelLine.lineType == LabelLine.LineType.HorizontalLine) color *= color;
                float currSin = Mathf.Sin(currAngle * Mathf.Deg2Rad);
                float currCos = Mathf.Cos(currAngle * Mathf.Deg2Rad);
                var radius1 = labelLine.lineType == LabelLine.LineType.HorizontalLine ?
                    serie.context.outsideRadius : outSideRadius;
                var radius2 = serie.context.outsideRadius + labelLine.lineLength1;
                var radius3 = insideRadius + (outSideRadius - insideRadius) / 2;
                if (radius1 < serie.context.insideRadius) radius1 = serie.context.insideRadius;
                radius1 -= 0.1f;
                var pos0 = new Vector3(center.x + radius3 * currSin, center.y + radius3 * currCos);
                var pos1 = new Vector3(center.x + radius1 * currSin, center.y + radius1 * currCos);
                var pos2 = serieData.context.labelPosition;
                if (pos2.x == 0)
                {
                    pos2 = new Vector3(center.x + radius2 * currSin, center.y + radius2 * currCos);
                }
                Vector3 pos4, pos6;
                var horizontalLineCircleRadius = labelLine.lineWidth * 4f;
                var lineCircleDiff = horizontalLineCircleRadius - 0.3f;
                if (currAngle < 90)
                {
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += labelLine.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 180)
                {
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += labelLine.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 270)
                {
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += labelLine.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                else
                {
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += labelLine.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                var pos5X = currAngle > 180 ? pos2.x - labelLine.lineLength2 : pos2.x + labelLine.lineLength2;
                var pos5 = new Vector3(pos5X, pos2.y);
                switch (labelLine.lineType)
                {
                    case LabelLine.LineType.BrokenLine:
                        UGL.DrawLine(vh, pos1, pos2, pos5, labelLine.lineWidth, color);
                        break;
                    case LabelLine.LineType.Curves:
                        UGL.DrawCurves(vh, pos1, pos5, pos1, pos2, labelLine.lineWidth, color,
                            chart.settings.lineSmoothness);
                        break;
                    case LabelLine.LineType.HorizontalLine:
                        UGL.DrawCricle(vh, pos0, horizontalLineCircleRadius, color);
                        UGL.DrawLine(vh, pos6, pos4, labelLine.lineWidth, color);
                        break;
                }
            }
        }

        private void DrawPieLabel(Serie serie, int dataIndex, SerieData serieData, Color serieColor)
        {
            if (serieData.labelObject == null) return;
            var currAngle = serieData.context.halfAngle;
            var isHighlight = (serieData.context.highlight && serie.emphasis.label.show);
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
            var iconStyle = SerieHelper.GetIconStyle(serie, serieData);
            var showLabel = ((serieLabel.show || isHighlight) && serieData.context.canShowLabel);
            if (showLabel)
            {
                serieData.SetLabelActive(showLabel);
                float rotate = 0;
                bool isInsidePosition = serieLabel.position == LabelStyle.Position.Inside;
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
                serieData.labelObject.SetPosition(SerieLabelHelper.GetRealLabelPosition(serieData, serieLabel, labelLine));
                if (showLabel) serieData.labelObject.SetLabelPosition(serieLabel.offset);
                else serieData.SetLabelActive(false);
            }
            else
            {
                serieData.SetLabelActive(false);
            }
            serieData.labelObject.UpdateIcon(iconStyle);
        }

        private int GetPiePosIndex(Serie serie, Vector2 local)
        {
            if (!(serie is Pie)) return -1;
            var dist = Vector2.Distance(local, serie.context.center);
            var maxRadius = serie.context.outsideRadius + 3 * chart.theme.serie.pieSelectedOffset;
            if (dist < serie.context.insideRadius || dist > maxRadius) return -1;
            Vector2 dir = local - new Vector2(serie.context.center.x, serie.context.center.y);
            float angle = ChartHelper.GetAngle360(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (angle >= serieData.context.startAngle && angle <= serieData.context.toAngle)
                {
                    var ndist = !serieData.selected ? dist :
                         Vector2.Distance(local, serieData.context.offsetCenter);
                    if (ndist >= serieData.context.insideRadius && ndist <= serieData.context.outsideRadius)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }

        private bool PointerIsInPieSerie(Serie serie, Vector2 local)
        {
            if (!(serie is Pie)) return false;
            var dist = Vector2.Distance(local, serie.context.center);
            if (dist >= serie.context.insideRadius && dist <= serie.context.outsideRadius) return true;
            return false;
        }
    }
}