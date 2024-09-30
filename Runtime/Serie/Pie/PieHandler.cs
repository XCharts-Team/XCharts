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
            if (labelLine != null && labelLine.show && serieData.labelObject != null)
            {
                var currAngle = serieData.context.halfAngle - serie.context.startAngle;
                var isLeft = currAngle > 180 || (currAngle == 0 && serieData.context.startAngle > 0);
                var textOffset = serieData.labelObject.text.GetPreferredWidth() / 2;
                return serieData.context.labelPosition + (isLeft ? Vector3.left : Vector3.right) * textOffset;
            }
            else
            {
                return serieData.context.labelPosition;
            }
        }

        public override Vector3 GetSerieDataLabelOffset(SerieData serieData, LabelStyle label)
        {
            var offset = label.GetOffset(serie.context.insideRadius);
            if (label.autoOffset)
            {
                var currAngle = serieData.context.halfAngle - serie.context.startAngle;
                var isLeft = currAngle > 180 || (currAngle == 0 && serieData.context.startAngle > 0);
                if (isLeft)
                    return new Vector3(-offset.x, offset.y, offset.z);
                else
                    return offset;
            }
            else
            {
                return offset;
            }
        }

        public override Vector3 GetSerieDataTitlePosition(SerieData serieData, TitleStyle titleStyle)
        {
            return serie.context.center;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (chart.pointerPos == Vector2.zero) return;
            var dataIndex = GetPiePosIndex(serie, chart.pointerPos);
            var refresh = false;
            if (dataIndex >= 0)
            {
                refresh = true;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    if (j == dataIndex) serie.data[j].context.selected = !serie.data[j].context.selected;
                    else serie.data[j].context.selected = false;
                }
            }
            if (refresh) chart.RefreshChart();
            base.OnPointerDown(eventData);
        }

        public override int GetPointerItemDataIndex()
        {
            return GetPiePosIndex(serie, chart.pointerPos);
        }

        public override void UpdateSerieContext()
        {
            var needCheck = m_LegendEnter || m_LegendExiting || m_ForceUpdateSerieContext || (chart.isPointerInChart && PointerIsInPieSerie(serie, chart.pointerPos));
            var needInteract = false;
            var interactEnable = serie.animation.enable && serie.animation.interaction.enable;
            Color32 color, toColor;
            if (!needCheck)
            {
                if (m_LastCheckContextFlag != needCheck || m_ForceUpdateSerieContext)
                {
                    serie.context.pointerItemDataIndex = -1;
                    serie.context.pointerEnter = false;
                    bool isAllZeroValue1 = SerieHelper.IsAllZeroValue(serie, 1);
                    var zeroReplaceValue1 = isAllZeroValue1 ? 360 / serie.dataCount : 0;
                    foreach (var serieData in serie.data)
                    {
                        serieData.context.highlight = false;
                        if (interactEnable)
                        {
                            var value = isAllZeroValue1 ? zeroReplaceValue1 : serieData.GetCurrData(1, serie.animation);
                            var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                            SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, SerieState.Normal);
                            UpdateSerieDataRadius(serieData, value);
                            serieData.interact.SetValueAndColor(ref needInteract, serieData.context.outsideRadius, color, toColor);
                            serieData.interact.SetPosition(ref needInteract, serieData.context.offsetCenter);
                        }
                    }
                    if (needInteract)
                    {
                        chart.RefreshPainter(serie);
                    }
                    else
                    {
                        m_LastCheckContextFlag = needCheck;
                        m_LegendExiting = false;
                        chart.RefreshPainter(serie);
                    }
                }
                return;
            }
            m_LastCheckContextFlag = needCheck;
            var lastPointerItemDataIndex = serie.context.pointerItemDataIndex;
            var dataIndex = GetPiePosIndex(serie, chart.pointerPos);
            serie.context.pointerItemDataIndex = -1;
            serie.context.pointerEnter = dataIndex >= 0;

            bool isAllZeroValue = SerieHelper.IsAllZeroValue(serie, 1);
            var zeroReplaceValue = isAllZeroValue ? 360 / serie.dataCount : 0;

            for (int i = 0; i < serie.dataCount; i++)
            {
                var serieData = serie.data[i];
                var value = isAllZeroValue ? zeroReplaceValue : serieData.GetCurrData(1, serie.animation);
                var state = SerieState.Normal;
                if (dataIndex == i || (m_LegendEnter && m_LegendEnterIndex == i))
                {
                    serie.context.pointerItemDataIndex = i;
                    serieData.context.highlight = true;
                    state = SerieState.Emphasis;
                }
                else
                {
                    serieData.context.highlight = false;
                }
                if (interactEnable)
                {
                    UpdateSerieDataRadius(serieData, value);
                    var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex, state);
                    serieData.interact.SetValueAndColor(ref needInteract, serieData.context.outsideRadius, color, toColor);
                    serieData.interact.SetPosition(ref needInteract, serieData.context.offsetCenter);
                }
            }
            if (lastPointerItemDataIndex != serie.context.pointerItemDataIndex)
            {
                needInteract = true;
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
            SerieHelper.UpdateCenter(serie, chart);
            float startDegree = serie.context.startAngle;
            float totalDegree = 0;
            float zeroReplaceValue = 0;
            int showdataCount = 0;
            foreach (var sd in serie.data)
            {
                if (sd.show && serie.pieRoseType == RoseType.Area) showdataCount++;
                sd.context.canShowLabel = false;
            }
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
            if (dataTotalFilterMinAngle == 0)
            {
                dataTotalFilterMinAngle = 360;
            }
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                var value = isAllZeroValue ? zeroReplaceValue : serieData.GetCurrData(1, serie.animation);
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
                    (float)(totalDegree * value / dataTotalFilterMinAngle);
                if (serie.minAngle > 0 && degree < serie.minAngle) degree = serie.minAngle;
                serieData.context.toAngle = startDegree + degree;
                var halfDegree = (serieData.context.toAngle - startDegree) / 2;
                serieData.context.halfAngle = startDegree + halfDegree;
                serieData.context.angle = startDegree + halfDegree;
                serieData.context.currentAngle = serie.animation.CheckDetailBreak(serieData.context.toAngle)
                    ? serie.animation.GetCurrDetail() : serieData.context.toAngle;
                serieData.context.insideRadius = serie.context.insideRadius;
                serieData.context.canShowLabel = serieData.context.currentAngle >= serieData.context.halfAngle && !serie.IsMinShowLabelValue(value);
                UpdateSerieDataRadius(serieData, value);
                UpdatePieLabelPosition(serie, serieData);
                startDegree = serieData.context.toAngle;
            }
            AvoidLabelOverlap(serie, chart.theme.common);
        }

        private void UpdateSerieDataRadius(SerieData serieData, double value)
        {
            var minChartWidth = Mathf.Min(chart.chartWidth, chart.chartHeight);
            var minRadius = serie.minRadius > 0 ? ChartHelper.GetActualValue(serie.minRadius, minChartWidth) : 0;
            if (serieData.radius > 0)
            {
                serieData.context.outsideRadius = ChartHelper.GetActualValue(serieData.radius, minChartWidth);
            }
            else
            {
                var minInsideRadius = minRadius > 0 ? minRadius : serie.context.insideRadius;
                serieData.context.outsideRadius = serie.pieRoseType > 0 ?
                minInsideRadius + (float)((serie.context.outsideRadius - minInsideRadius) * value / serie.context.dataMax) :
                serie.context.outsideRadius;
            }
            if (minRadius > 0 && serieData.context.outsideRadius < minRadius)
            {
                serieData.context.outsideRadius = minRadius;
            }
            var offset = 0f;
            var interactOffset = serie.animation.interaction.GetOffset(serie.context.outsideRadius);
            if (serie.pieClickOffset && (serieData.selected || serieData.context.selected))
            {
                offset += interactOffset;
            }
            if (offset > 0)
            {
                serieData.context.outsideRadius += interactOffset;
                var currRad = serieData.context.halfAngle * Mathf.Deg2Rad;
                var currSin = Mathf.Sin(currRad);
                var currCos = Mathf.Cos(currRad);
                serieData.context.offsetRadius = 0;
                if (serie.pieClickOffset && (serieData.selected || serieData.context.selected))
                {
                    serieData.context.offsetRadius += interactOffset;
                    if (serieData.context.insideRadius > 0)
                    {
                        serieData.context.insideRadius += interactOffset;
                    }
                }
                serieData.context.offsetCenter = new Vector3(
                    serie.context.center.x + serieData.context.offsetRadius * currSin,
                    serie.context.center.y + serieData.context.offsetRadius * currCos);
            }
            else
            {
                serieData.context.offsetCenter = serie.context.center;
            }
            if (serieData.context.highlight)
            {
                serieData.context.outsideRadius = serie.animation.GetInteractionRadius(serieData.context.outsideRadius);
            }
            var halfRadius = serie.context.insideRadius + (serieData.context.outsideRadius - serie.context.insideRadius) / 2;
            serieData.context.position = ChartHelper.GetPosition(serie.context.center, serieData.context.halfAngle, halfRadius);
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

        private void DrawPie(VertexHelper vh, Pie serie)
        {
            if (!serie.show || serie.animation.HasFadeOut())
            {
                return;
            }
            var dataChanging = false;
            var interacting = false;
            var color = ColorUtil.clearColor32;
            var toColor = ColorUtil.clearColor32;
            var interactDuration = serie.animation.GetInteractionDuration();
            var interactEnable = serie.animation.enable && serie.animation.interaction.enable
                && !serie.animation.IsFadeIn() && !serie.animation.IsFadeOut();
            var data = serie.data;
            serie.animation.InitProgress(0, 360);
            if (data.Count == 0)
            {
                var itemStyle = SerieHelper.GetItemStyle(serie, null);
                var fillColor = ChartHelper.IsClearColor(itemStyle.backgroundColor) ?
                    (Color32)chart.theme.legend.unableColor : itemStyle.backgroundColor;
                UGL.DrawDoughnut(vh, serie.context.center, serie.context.insideRadius,
                    serie.context.outsideRadius, fillColor, fillColor, Color.clear, 0,
                    360, itemStyle.borderWidth, itemStyle.borderColor, serie.gap / 2, chart.settings.cicleSmoothness,
                    false, true, serie.radiusGradient);
            }
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

                var needOffset = (serie.pieClickOffset && (serieData.selected || serieData.context.selected));
                var offsetCenter = needOffset ? serieData.context.offsetCenter : serie.context.center;

                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;

                var progress = AnimationStyleHelper.CheckDataAnimation(chart, serie, n, 1);
                var insideRadius = serieData.context.insideRadius * progress;

                if (!interactEnable || !serieData.interact.TryGetValueAndColor(
                    ref outsideRadius, ref offsetCenter, ref color, ref toColor, ref interacting, interactDuration))
                {
                    SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, colorIndex);
                    outsideRadius = serieData.context.outsideRadius * progress;
                    if (interactEnable)
                    {
                        serieData.interact.SetValueAndColor(ref interacting, outsideRadius, color, toColor);
                        serieData.interact.SetPosition(ref interacting, offsetCenter);
                    }
                }
                var drawEndDegree = serieData.context.currentAngle;
                var needRoundCap = serie.roundCap && insideRadius > 0;
                UGL.DrawDoughnut(vh, offsetCenter, insideRadius,
                    outsideRadius, color, toColor, Color.clear, serieData.context.startAngle,
                    drawEndDegree, borderWidth, borderColor, serie.gap / 2, chart.settings.cicleSmoothness,
                    needRoundCap, true, serie.radiusGradient);
                DrawPieCenter(vh, serie, itemStyle, insideRadius);

                if (serie.animation.CheckDetailBreak(serieData.context.toAngle))
                    break;
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress();
                serie.animation.CheckSymbol(serie.symbol.GetSize(null, chart.theme.serie.lineSymbolSize));
                chart.RefreshPainter(serie);
            }
            if (dataChanging || interacting)
            {
                chart.RefreshPainter(serie);
            }
        }

        private static void UpdatePieLabelPosition(Serie serie, SerieData serieData)
        {
            if (serieData.labelObject == null) return;
            var startAngle = serie.context.startAngle;
            var currAngle = serieData.context.halfAngle;
            var currRad = currAngle * Mathf.Deg2Rad;
            var offsetRadius = serieData.context.offsetRadius;
            var insideRadius = serieData.context.insideRadius;
            var outsideRadius = serieData.context.outsideRadius;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
            var center = serieData.context.offsetCenter;
            var interact = false;
            serieData.interact.TryGetValueAndColor(ref outsideRadius, ref center, ref interact, serie.animation.GetInteractionDuration());
            var diffAngle = (currAngle - startAngle) % 360;
            var isLeft = diffAngle > 180 || (diffAngle == 0 && serieData.context.startAngle > 0);
            switch (serieLabel.position)
            {
                case LabelStyle.Position.Center:
                    serieData.context.labelPosition = serie.context.center;
                    break;
                case LabelStyle.Position.Inside:
                case LabelStyle.Position.Middle:
                    var labelRadius = offsetRadius + insideRadius + (outsideRadius - insideRadius) / 2 + serieLabel.distance;
                    var labelCenter = new Vector2(center.x + labelRadius * Mathf.Sin(currRad),
                        center.y + labelRadius * Mathf.Cos(currRad));
                    UpdateLabelPosition(serie, serieData, labelLine, labelCenter, isLeft);
                    break;
                default:
                    //LabelStyle.Position.Outside
                    var startPos = new Vector2(center.x + outsideRadius * Mathf.Sin(currRad),
                            center.y + outsideRadius * Mathf.Cos(currRad));
                    UpdateLabelPosition(serie, serieData, labelLine, startPos, isLeft);
                    break;
            }
        }

        private static void UpdateLabelPosition(Serie serie, SerieData serieData, LabelLine labelLine, Vector3 startPosition, bool isLeft)
        {
            serieData.context.labelLinePosition = startPosition;
            if (labelLine == null || !labelLine.show)
            {
                serieData.context.labelPosition = startPosition;
                return;
            }
            var dire = isLeft ? Vector3.left : Vector3.right;
            var rad = Mathf.Deg2Rad * serieData.context.halfAngle;
            var lineLength1 = ChartHelper.GetActualValue(labelLine.lineLength1, serie.context.outsideRadius);
            var lineLength2 = ChartHelper.GetActualValue(labelLine.lineLength2, serie.context.outsideRadius);
            var radius = lineLength1;
            var pos1 = startPosition;
            var pos2 = pos1 + new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius);
            var pos5 = labelLine.lineType == LabelLine.LineType.HorizontalLine
                ? pos1 + dire * (radius + lineLength2) + labelLine.GetEndSymbolOffset()
                : pos2 + dire * lineLength2 + labelLine.GetEndSymbolOffset();
            if (labelLine.lineEndX != 0)
            {
                pos5.x = serie.context.center.x + (isLeft ? -Mathf.Abs(labelLine.lineEndX) : Mathf.Abs(labelLine.lineEndX));
            }
            serieData.context.labelLinePosition2 = pos2;
            serieData.context.labelPosition = pos5;
        }

        private void DrawPieLabelLine(VertexHelper vh, Serie serie, bool isTop)
        {
            foreach (var serieData in serie.data)
            {
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
                if (SerieLabelHelper.CanShowLabel(serie, serieData, serieLabel, 1))
                {
                    int colorIndex = chart.m_LegendRealShowName.IndexOf(serieData.name);
                    if (serieLabel != null && serieLabel.show &&
                        labelLine != null && labelLine.show)
                    {
                        if (serieLabel.position == LabelStyle.Position.Inside || serieLabel.position == LabelStyle.Position.Middle)
                        {
                            if (!isTop) continue;
                        }
                        else
                        {
                            if (isTop && !labelLine.startSymbol.show) continue;
                        }
                        var color = ChartHelper.IsClearColor(labelLine.lineColor) ?
                            chart.theme.GetColor(colorIndex) :
                            labelLine.lineColor;
                        switch (labelLine.lineType)
                        {
                            case LabelLine.LineType.BrokenLine:
                                UGL.DrawLine(vh, serieData.context.labelLinePosition, serieData.context.labelLinePosition2,
                                    serieData.context.labelPosition, labelLine.lineWidth, color);
                                break;
                            case LabelLine.LineType.Curves:
                                if (serieData.context.labelLinePosition2 == serieData.context.labelPosition)
                                {
                                    UGL.DrawCurves(vh, serieData.context.labelLinePosition, serieData.context.labelPosition,
                                    serieData.context.labelLinePosition, (serieData.context.labelLinePosition + serieData.context.labelPosition) * 0.6f,
                                    labelLine.lineWidth, color, chart.settings.lineSmoothness);
                                }
                                else
                                {
                                    UGL.DrawCurves(vh, serieData.context.labelLinePosition, serieData.context.labelPosition,
                                    serieData.context.labelLinePosition, serieData.context.labelLinePosition2,
                                    labelLine.lineWidth, color, chart.settings.lineSmoothness);
                                }
                                break;
                            case LabelLine.LineType.HorizontalLine:
                                UGL.DrawLine(vh, serieData.context.labelLinePosition, serieData.context.labelPosition,
                                    labelLine.lineWidth, color);
                                break;
                        }
                        DrawLabelLineSymbol(vh, labelLine, serieData.context.labelLinePosition, serieData.context.labelPosition, color);
                    }
                }
            }
        }

        private int GetPiePosIndex(Serie serie, Vector2 local)
        {
            if (!(serie is Pie))
                return -1;

            var dist = Vector2.Distance(local, serie.context.center);
            var interactOffset = serie.animation.interaction.GetOffset(serie.context.outsideRadius);
            var maxRadius = serie.context.outsideRadius + 2 * interactOffset;
            if (dist < serie.context.insideRadius || dist > maxRadius)
                return -1;

            var dir = local - new Vector2(serie.context.center.x, serie.context.center.y);
            var angle = ChartHelper.GetAngle360(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (angle >= serieData.context.startAngle && angle <= serieData.context.toAngle)
                {
                    var ndist = (serieData.selected || serieData.context.selected) ?
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

        private void AvoidLabelOverlap(Serie serie, ComponentTheme theme)
        {
            if (!serie.avoidLabelOverlap) return;
            var lastCheckPos = Vector3.zero;
            var lastX = 0f;
            var data = serie.data;
            var splitCount = 0;
            for (int n = 0; n < data.Count; n++)
            {
                var serieData = data[n];
                if (serieData.context.labelPosition.x != 0 && serieData.context.labelPosition.x < serie.context.center.x)
                {
                    splitCount = n;
                    break;
                }
            }
            var limitX = float.MinValue;
            for (int n = 0; n < splitCount; n++)
            {
                CheckSerieDataLabel(serie, data[n], splitCount, false, n == splitCount - 1, theme, ref lastCheckPos, ref lastX, ref limitX);
            }
            lastCheckPos = Vector3.zero;
            limitX = float.MaxValue;
            for (int n = data.Count - 1; n >= splitCount; n--)
            {
                CheckSerieDataLabel(serie, data[n], data.Count - splitCount, true, n == splitCount, theme, ref lastCheckPos, ref lastX, ref limitX);
            }
        }

        private void CheckSerieDataLabel(Serie serie, SerieData serieData, int total, bool isLeft, bool isLastOne, ComponentTheme theme,
            ref Vector3 lastCheckPos, ref float lastX, ref float limitX)
        {
            if (!serieData.context.canShowLabel)
            {
                serieData.SetLabelActive(false);
                return;
            }
            if (!serieData.show) return;
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            if (serieLabel == null) return;
            if (!serieLabel.show) return;
            var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
            var fontSize = serieData.labelObject.GetHeight();
            var lineLength1 = 0f;
            var lineLength2 = 0f;
            if (labelLine != null && labelLine.show)
            {
                lineLength1 = ChartHelper.GetActualValue(labelLine.lineLength1, serie.context.outsideRadius);
                lineLength2 = ChartHelper.GetActualValue(labelLine.lineLength2, serie.context.outsideRadius);
            }
            if (lastCheckPos == Vector3.zero)
            {
                lastCheckPos = serieData.context.labelPosition;
            }
            else if (serieData.context.labelPosition.x != 0)
            {
                if (lastCheckPos.y - serieData.context.labelPosition.y < fontSize)
                {
                    var labelRadius = serie.context.outsideRadius + lineLength1;
                    var y1 = lastCheckPos.y - fontSize;
                    var cy = serie.context.center.y;
                    var diff = Mathf.Abs(y1 - cy);
                    var diffX = labelRadius * labelRadius - diff * diff;
                    diffX = diffX <= 0 ? 0 : diffX;
                    var x1 = serie.context.center.x + Mathf.Sqrt(diffX) * (isLeft ? -1 : 1);
                    var newPos = new Vector3(x1, y1);
                    serieData.context.labelLinePosition2 = newPos;
                    if (isLeft)
                    {
                        if (x1 < limitX)
                        {
                            limitX = x1;
                            serieData.context.labelPosition = new Vector3(newPos.x - lineLength2, newPos.y);
                            lastX = serieData.context.labelPosition.x;
                        }
                        else
                        {
                            serieData.context.labelPosition = new Vector3(lastX, y1);
                            lastX += 2;
                        }
                    }
                    else
                    {
                        if (x1 > limitX)
                        {
                            limitX = x1;
                            serieData.context.labelPosition = new Vector3(newPos.x + lineLength2, newPos.y);
                            lastX = serieData.context.labelPosition.x;
                        }
                        else
                        {
                            serieData.context.labelPosition = new Vector3(lastX, y1);
                            lastX -= 2;
                        }

                    }
                    if (labelLine != null && labelLine.show && labelLine.lineEndX != 0)
                    {
                        serieData.context.labelPosition.x = isLeft ? -Mathf.Abs(labelLine.lineEndX) : Mathf.Abs(labelLine.lineEndX);
                    }
                    if (!isLastOne && serieData.context.labelPosition.y < serieData.context.labelLinePosition.y)
                    {
                        serieData.context.labelLinePosition2 = serieData.context.labelPosition;
                    }
                    else
                    {
                        if (isLeft && serieData.context.labelLinePosition2.x > serieData.context.labelLinePosition.x)
                        {
                            serieData.context.labelLinePosition2.x = serieData.context.labelLinePosition.x;
                        }
                        else if (!isLeft && serieData.context.labelLinePosition2.x < serieData.context.labelLinePosition.x)
                        {
                            serieData.context.labelLinePosition2.x = serieData.context.labelLinePosition.x;
                        }
                    }

                }
                else
                {
                    lastX = serieData.context.labelPosition.x;
                }
                lastCheckPos = serieData.context.labelPosition;
                UpdateLabelPosition(serieData, serieLabel);
            }
        }
    }
}