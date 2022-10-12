using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class PieHandler : SerieHandler<Pie>
    {
        public override void Update()
        {
            base.Update();
            UpdateSerieContext();
        }

        public override void DrawBase(VertexHelper vh)
        {
            UpdateRuntimeData(serie);
            DrawPieLabelLine(vh, serie, false);
        }

        public override void DrawSerie(VertexHelper vh)
        {
            UpdateRuntimeData(serie);
            DrawPie(vh, serie);
            chart.RefreshBasePainter();
        }

        public override void DrawUpper(VertexHelper vh)
        {
            DrawPieLabelLine(vh, serie, true);
        }

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        {
            UpdateItemSerieParams(ref paramList, ref title, dataIndex, category,
                marker, itemFormatter, numericFormatter, ignoreDataDefaultContent);
        }

        public override Vector3 GetSerieDataLabelPosition(SerieData serieData, LabelStyle label)
        {
            var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
            return SerieLabelHelper.GetRealLabelPosition(serie, serieData, label, labelLine);
        }

        public override Vector3 GetSerieDataTitlePosition(SerieData serieData, TitleStyle titleStyle)
        {
            return serie.context.center;
        }

        public override void OnLegendButtonClick(int index, string legendName, bool show)
        {
            if (!serie.IsLegendName(legendName))
                return;
            LegendHelper.CheckDataShow(serie, legendName, show);
            chart.UpdateLegendColor(legendName, show);
            chart.RefreshPainter(serie);
        }

        public override void OnLegendButtonEnter(int index, string legendName)
        {
            if (!serie.IsLegendName(legendName))
                return;
            LegendHelper.CheckDataHighlighted(serie, legendName, true);
            chart.RefreshPainter(serie);
        }

        public override void OnLegendButtonExit(int index, string legendName)
        {
            if (!serie.IsLegendName(legendName))
                return;
            LegendHelper.CheckDataHighlighted(serie, legendName, false);
            chart.RefreshPainter(serie);
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
                        if (j == index) serie.data[j].context.selected = !serie.data[j].context.selected;
                        else serie.data[j].context.selected = false;
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
            var needCheck = m_LegendEnter || (chart.isPointerInChart && PointerIsInPieSerie(serie, chart.pointerPos));
            var needInteract = false;
            Color32 color, toColor;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck)
                {
                    m_LastCheckContextFlag = needCheck;
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    foreach (var serieData in serie.data)
                    {
                        var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                        SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, SerieState.Normal);
                        serieData.context.highlight = false;
                        serieData.interact.SetValueAndColor(ref needInteract, serieData.context.outsideRadius, color, toColor);
                    }
                    if (chart.onPointerEnterPie != null)
                    {
                        chart.onPointerEnterPie(serie.index, serie.context.pointerItemDataIndex);
                    }
                    if (needInteract)
                    {
                        chart.RefreshPainter(serie);
                    }
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            var lastPointerItemDataIndex = serie.context.pointerItemDataIndex;
            var dataIndex = GetPiePosIndex(serie, chart.pointerPos);
            serie.context.pointerItemDataIndex = -1;
            for (int i = 0; i < serie.dataCount; i++)
            {
                var serieData = serie.data[i];
                if (dataIndex == i || (m_LegendEnter && m_LegendEnterIndex == i))
                {
                    serie.context.pointerItemDataIndex = i;
                    serieData.context.highlight = true;

                    var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, SerieState.Emphasis);
                    var value = serieData.context.outsideRadius + chart.theme.serie.pieTooltipExtraRadius;
                    serieData.interact.SetValueAndColor(ref needInteract, value, color, toColor);
                }
                else
                {
                    serieData.context.highlight = false;
                    var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, SerieState.Normal);
                    serieData.interact.SetValueAndColor(ref needInteract, serieData.context.outsideRadius, color, toColor);
                }
            }
            if (lastPointerItemDataIndex != serie.context.pointerItemDataIndex)
            {
                needInteract = true;
                if (chart.onPointerEnterPie != null)
                {
                    chart.onPointerEnterPie(serie.index, serie.context.pointerItemDataIndex);
                }
            }
            if (needInteract)
            {
                chart.RefreshPainter(serie);
            }
        }

        private void UpdateRuntimeData(Serie serie)
        {
            var data = serie.data;
            serie.context.dataMax = serie.yMax;
            serie.context.startAngle = GetStartAngle(serie);
            var runtimePieDataTotal = serie.yTotal;

            SerieHelper.UpdateCenter(serie, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            float startDegree = serie.context.startAngle;
            float totalDegree = 0;
            float zeroReplaceValue = 0;
            int showdataCount = 0;
            foreach (var sd in serie.data)
            {
                if (sd.show && serie.pieRoseType == RoseType.Area) showdataCount++;
                sd.context.canShowLabel = false;
            }
            float dataChangeDuration = serie.animation.GetUpdateAnimationDuration();
            var unscaledTime = serie.animation.unscaledTime;
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
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                var value = isAllZeroValue ? zeroReplaceValue : serieData.GetCurrData(1, dataChangeDuration, unscaledTime);
                serieData.context.startAngle = startDegree;
                serieData.context.toAngle = startDegree;
                serieData.context.halfAngle = startDegree;
                serieData.context.currentAngle = startDegree;
                if (!serieData.show)
                {
                    continue;
                }
                float degree = serie.pieRoseType == RoseType.Area ?
                    (totalDegree / showdataCount) :
                    (float) (totalDegree * value / dataTotalFilterMinAngle);
                if (serie.minAngle > 0 && degree < serie.minAngle) degree = serie.minAngle;
                serieData.context.toAngle = startDegree + degree;
                if (serieData.radius > 0)
                    serieData.context.outsideRadius = ChartHelper.GetActualValue(serieData.radius, Mathf.Min(chart.chartWidth, chart.chartHeight));
                else
                    serieData.context.outsideRadius = serie.pieRoseType > 0 ?
                    serie.context.insideRadius + (float) ((serie.context.outsideRadius - serie.context.insideRadius) * value / serie.context.dataMax) :
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
            totalAngle = serie.context.startAngle + 360f;
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
            if (!serie.show || serie.animation.HasFadeOut())
            {
                return;
            }
            var dataChanging = false;
            var interacting = false;
            var color = ColorUtil.clearColor32;
            var toColor = ColorUtil.clearColor32;
            var data = serie.data;
            serie.animation.InitProgress(0, 360);
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                if (!serieData.show)
                {
                    continue;
                }
                if (serieData.IsDataChanged())
                    dataChanging = true;

                var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
                var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                var outsideRadius = 0f;

                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;

                var progress = AnimationStyleHelper.CheckDataAnimation(chart, serie, n, 1);
                var insideRadius = serieData.context.insideRadius * progress;

                //if (!serieData.interact.TryGetValueAndColor(ref outsideRadius, ref color, ref toColor, ref interacting))
                {
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex);
                    outsideRadius = serieData.context.outsideRadius * progress;
                    serieData.interact.SetValueAndColor(ref interacting, outsideRadius, color, toColor);
                }

                if (serie.pieClickOffset && serieData.selected)
                {
                    var drawEndDegree = serieData.context.currentAngle;
                    var needRoundCap = serie.roundCap && insideRadius > 0;
                    UGL.DrawDoughnut(vh, serieData.context.offsetCenter, insideRadius,
                        outsideRadius, color, toColor, Color.clear, serieData.context.startAngle,
                        drawEndDegree, borderWidth, borderColor, serie.gap / 2, chart.settings.cicleSmoothness,
                        needRoundCap, true);
                }
                else
                {
                    var drawEndDegree = serieData.context.currentAngle;
                    var needRoundCap = serie.roundCap && insideRadius > 0;
                    UGL.DrawDoughnut(vh, serie.context.center, insideRadius,
                        outsideRadius, color, toColor, Color.clear, serieData.context.startAngle,
                        drawEndDegree, borderWidth, borderColor, serie.gap / 2, chart.settings.cicleSmoothness,
                        needRoundCap, true);
                    DrawPieCenter(vh, serie, itemStyle, insideRadius);
                }

                if (serie.animation.CheckDetailBreak(serieData.context.toAngle))
                    break;
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
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
                if (serie is Pie && serie.pieClickOffset)
                    return true;
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
                        if (serieData.context.highlight)
                            return true;
                    }
                }
            }
            return false;
        }

        private void DrawPieLabelLine(VertexHelper vh, Serie serie, bool drawHightlight)
        {
            foreach (var serieData in serie.data)
            {
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                if (drawHightlight && !serieData.context.highlight) continue;
                if (!drawHightlight && serieData.context.highlight) continue;
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
            if (serieLabel != null && serieLabel.show &&
                labelLine != null && labelLine.show &&
                (serieLabel.IsDefaultPosition(LabelStyle.Position.Outside)))
            {
                var insideRadius = serieData.context.insideRadius;
                var outSideRadius = serieData.context.outsideRadius;
                var center = serie.context.center;
                var currAngle = serieData.context.halfAngle;

                if (!ChartHelper.IsClearColor(labelLine.lineColor))
                    color = labelLine.lineColor;
                else if (labelLine.lineType == LabelLine.LineType.HorizontalLine)
                    color *= color;

                float currSin = Mathf.Sin(currAngle * Mathf.Deg2Rad);
                float currCos = Mathf.Cos(currAngle * Mathf.Deg2Rad);
                var radius1 = labelLine.lineType == LabelLine.LineType.HorizontalLine ?
                    serie.context.outsideRadius : outSideRadius;
                var radius3 = insideRadius + (outSideRadius - insideRadius) / 2;
                if (radius1 < serie.context.insideRadius) radius1 = serie.context.insideRadius;
                radius1 -= 0.1f;
                var pos0 = new Vector3(center.x + radius3 * currSin, center.y + radius3 * currCos);
                var pos1 = new Vector3(center.x + radius1 * currSin, center.y + radius1 * currCos);
                var pos2 = serieData.context.labelPosition;
                Vector3 pos4, pos6;
                var horizontalLineCircleRadius = labelLine.lineWidth * 4f;
                var lineCircleDiff = horizontalLineCircleRadius - 0.3f;
                var startAngle = serie.context.startAngle;
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
                var pos5X = (currAngle - startAngle) % 360 > 180 ?
                    pos2.x - labelLine.lineLength2 : pos2.x + labelLine.lineLength2;
                var pos5 = new Vector3(pos5X, pos2.y);
                var angle = Vector3.Angle(pos1 - center, pos2 - pos1);
                if (angle > 15)
                {
                    UGL.DrawLine(vh, pos1, pos5, labelLine.lineWidth, color);
                }
                else
                {
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
        }

        private int GetPiePosIndex(Serie serie, Vector2 local)
        {
            if (!(serie is Pie))
                return -1;

            var dist = Vector2.Distance(local, serie.context.center);
            var maxRadius = serie.context.outsideRadius + 3 * chart.theme.serie.pieSelectedOffset;
            if (dist < serie.context.insideRadius || dist > maxRadius)
                return -1;

            var dir = local - new Vector2(serie.context.center.x, serie.context.center.y);
            var angle = ChartHelper.GetAngle360(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (angle >= serieData.context.startAngle && angle <= serieData.context.toAngle)
                {
                    var ndist = serieData.selected ?
                        Vector2.Distance(local, serieData.context.offsetCenter) :
                        dist;
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
            if (!(serie is Pie))
                return false;

            var dist = Vector2.Distance(local, serie.context.center);
            if (dist >= serie.context.insideRadius && dist <= serie.context.outsideRadius)
                return true;

            return false;
        }

        private float GetStartAngle(Serie serie)
        {
            return serie.clockwise ? (serie.startAngle + 360) % 360 : 360 - serie.startAngle;
        }

        private float GetToAngle(Serie serie, float angle)
        {
            var toAngle = angle + serie.startAngle;
            if (!serie.clockwise)
            {
                toAngle = 360 - angle - serie.startAngle;
            }
            if (!serie.animation.IsFinish())
            {
                var currAngle = serie.animation.GetCurrDetail();
                if (serie.clockwise)
                {
                    toAngle = toAngle > currAngle ? currAngle : toAngle;
                }
                else
                {
                    toAngle = toAngle < 360 - currAngle ? 360 - currAngle : toAngle;
                }
            }
            return toAngle;
        }
    }
}