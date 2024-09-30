using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class RingHandler : SerieHandler<Ring>
    {
        public override int defaultDimension { get { return 0; } }

        public override void Update()
        {
            base.Update();
        }

        public override void UpdateSerieContext()
        {
            var needCheck = chart.isPointerInChart || m_LegendEnter;
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
                serie.context.pointerEnter = false;
                serie.context.pointerItemDataIndex = -1;
                var ringIndex = GetRingIndex(chart.pointerPos);
                foreach (var serieData in serie.data)
                {
                    if (!needInteract && ringIndex == serieData.index)
                    {
                        serie.context.pointerEnter = true;
                        serie.context.pointerItemDataIndex = ringIndex;
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

        public override void UpdateTooltipSerieParams(int dataIndex, bool showCategory, string category,
            string marker, string itemFormatter, string numericFormatter, string ignoreDataDefaultContent,
            ref List<SerieParams> paramList, ref string title)
        {
            if (dataIndex < 0)
                dataIndex = serie.context.pointerItemDataIndex;

            if (dataIndex < 0)
                return;

            var serieData = serie.GetSerieData(dataIndex);
            if (serieData == null)
                return;
            Color32 color, toColor;
            SerieHelper.GetItemColor(out color, out toColor, serie, serieData, chart.theme, dataIndex);

            var param = serie.context.param;
            param.serieName = serie.serieName;
            param.serieIndex = serie.index;
            param.category = category;
            param.dimension = defaultDimension;
            param.serieData = serieData;
            param.dataCount = serie.dataCount;
            param.value = serieData.GetData(0);
            param.total = serieData.GetData(1);
            param.color = color;
            param.marker = SerieHelper.GetItemMarker(serie, serieData, marker);
            param.itemFormatter = SerieHelper.GetItemFormatter(serie, serieData, itemFormatter);
            param.numericFormatter = SerieHelper.GetNumericFormatter(serie, serieData, numericFormatter);
            param.columns.Clear();

            param.columns.Add(param.marker);
            param.columns.Add(serieData.name);
            param.columns.Add(ChartCached.NumberToStr(param.value, param.numericFormatter));

            paramList.Add(param);
        }

        private Vector3 GetLabelLineEndPosition(Serie serie, SerieData serieData, LabelLine labelLine)
        {
            if (labelLine == null || !labelLine.show)
                return serieData.context.labelLinePosition;
            var isRight = !serie.clockwise;
            var dire = isRight ? Vector3.right : Vector3.left;
            var rad = Mathf.Deg2Rad * (isRight ? labelLine.lineAngle : 180 - labelLine.lineAngle);
            var lineLength1 = ChartHelper.GetActualValue(labelLine.lineLength1, serie.context.outsideRadius);
            var lineLength2 = ChartHelper.GetActualValue(labelLine.lineLength2, serie.context.outsideRadius);
            var pos1 = serieData.context.labelLinePosition;
            var pos2 = pos1 + new Vector3(Mathf.Cos(rad) * lineLength1, Mathf.Sin(rad) * lineLength1);
            var pos5 = labelLine.lineType == LabelLine.LineType.HorizontalLine
                ? pos1 + dire * (lineLength1 + lineLength2) + labelLine.GetEndSymbolOffset()
                : pos2 + dire * lineLength2 + labelLine.GetEndSymbolOffset();
            if (labelLine.lineEndX != 0)
            {
                pos5.x = labelLine.lineEndX;
            }
            return pos5;
        }

        public override void DrawSerie(VertexHelper vh)
        {
            if (!serie.show || serie.animation.HasFadeOut()) return;
            UpdateRuntimeData();
            var data = serie.data;
            serie.animation.InitProgress(serie.startAngle, serie.startAngle + 360);
            var ringWidth = serie.context.outsideRadius - serie.context.insideRadius;
            var dataChanging = false;
            for (int j = 0; j < data.Count; j++)
            {
                var serieData = data[j];
                if (!serieData.show) continue;
                if (serieData.IsDataChanged()) dataChanging = true;
                var outsideRadius = serie.context.outsideRadius - j * (ringWidth + serie.gap);
                if (outsideRadius < 0) continue;
                var value = serieData.GetCurrData(0, serie.animation, false, false);
                var max = serieData.GetLastData();
                var degree = (float)(360 * value / max);
                var startDegree = GetStartAngle(serie);
                var toDegree = GetToAngle(serie, degree);
                var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
                var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                Color32 itemColor, itemToColor;
                SerieHelper.GetItemColor(out itemColor, out itemToColor, serie, serieData, chart.theme, colorIndex);

                var insideRadius = outsideRadius - ringWidth;
                var borderWidth = itemStyle.borderWidth;
                var borderColor = itemStyle.borderColor;
                var roundCap = serie.roundCap && insideRadius > 0;
                DrawBackground(vh, serie, serieData, j, insideRadius, outsideRadius);
                UGL.DrawDoughnut(vh, serie.context.center, insideRadius, outsideRadius, itemColor, itemToColor,
                    Color.clear, startDegree, toDegree, borderWidth, borderColor, 0, chart.settings.cicleSmoothness,
                    roundCap, serie.clockwise, serie.radiusGradient);
                DrawCenter(vh, serie, serieData, insideRadius, j == data.Count - 1);
            }

            for (int j = 0; j < data.Count; j++)
            {
                var serieData = data[j];
                if (!serieData.show) continue;
                var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
                var colorIndex = chart.GetLegendRealShowNameIndex(serieData.legendName);
                Color32 itemColor, itemToColor;
                SerieHelper.GetItemColor(out itemColor, out itemToColor, serie, serieData, chart.theme, colorIndex);
                if (SerieLabelHelper.CanShowLabel(serie, serieData, serieLabel, 0))
                {
                    DrawRingLabelLine(vh, serie, serieData, itemColor);
                }
            }
            if (!serie.animation.IsFinish())
            {
                serie.animation.CheckProgress(360);
                chart.RefreshChart();
            }
            if (dataChanging)
            {
                chart.RefreshChart();
            }
        }

        private void UpdateRuntimeData()
        {
            var data = serie.data;
            SerieHelper.UpdateCenter(serie, chart);
            var ringWidth = serie.context.outsideRadius - serie.context.insideRadius;
            for (int j = 0; j < data.Count; j++)
            {
                var serieData = data[j];
                if (!serieData.show) continue;
                var outsideRadius = serie.context.outsideRadius - j * (ringWidth + serie.gap);
                if (outsideRadius < 0) continue;
                var value = serieData.GetCurrData(0, serie.animation, false, false);
                var max = serieData.GetLastData();
                var degree = (float)(360 * value / max);
                var startDegree = GetStartAngle(serie);
                var toDegree = GetToAngle(serie, degree);
                var insideRadius = outsideRadius - ringWidth;
                var halfAngle = startDegree + (toDegree - startDegree) / 2;
                var halfRadius = (outsideRadius + insideRadius) / 2;
                serieData.context.startAngle = startDegree;
                serieData.context.toAngle = toDegree;
                serieData.context.insideRadius = insideRadius;
                serieData.context.outsideRadius = serieData.radius > 0 ? serieData.radius : outsideRadius;
                serieData.context.position = ChartHelper.GetPosition(serie.context.center, halfAngle, halfRadius);
                UpdateLabelPosition(serieData);
            }
            AvoidLabelOverlap();
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

        public override void OnPointerDown(PointerEventData eventData) { }

        private float GetStartAngle(Serie serie)
        {
            return serie.clockwise ? serie.startAngle : 360 - serie.startAngle;
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

        private void DrawCenter(VertexHelper vh, Serie serie, SerieData serieData, float insideRadius, bool last)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (!ChartHelper.IsClearColor(itemStyle.centerColor) && last)
            {
                var radius = insideRadius - itemStyle.centerGap;
                var smoothness = chart.settings.cicleSmoothness;
                UGL.DrawCricle(vh, serie.context.center, radius, itemStyle.centerColor, smoothness);
            }
        }

        private void DrawBackground(VertexHelper vh, Serie serie, SerieData serieData, int index, float insideRadius, float outsideRadius)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            var backgroundColor = itemStyle.backgroundColor;
            if (ChartHelper.IsClearColor(backgroundColor))
            {
                backgroundColor = chart.theme.GetColor(index);
                backgroundColor.a = 50;
            }
            if (itemStyle.backgroundWidth != 0)
            {
                var centerRadius = (outsideRadius + insideRadius) / 2;
                var inradius = centerRadius - itemStyle.backgroundWidth / 2;
                var outradius = centerRadius + itemStyle.backgroundWidth / 2;
                UGL.DrawDoughnut(vh, serie.context.center, inradius,
                    outradius, backgroundColor, Color.clear, chart.settings.cicleSmoothness);
            }
            else
            {
                UGL.DrawDoughnut(vh, serie.context.center, insideRadius,
                    outsideRadius, backgroundColor, Color.clear, chart.settings.cicleSmoothness);
            }
        }

        private void DrawBorder(VertexHelper vh, Serie serie, SerieData serieData, float insideRadius, float outsideRadius)
        {
            var itemStyle = SerieHelper.GetItemStyle(serie, serieData);
            if (itemStyle.show && itemStyle.borderWidth > 0 && !ChartHelper.IsClearColor(itemStyle.borderColor))
            {
                UGL.DrawDoughnut(vh, serie.context.center, outsideRadius,
                    outsideRadius + itemStyle.borderWidth, itemStyle.borderColor,
                    Color.clear, chart.settings.cicleSmoothness);
                UGL.DrawDoughnut(vh, serie.context.center, insideRadius,
                    insideRadius + itemStyle.borderWidth, itemStyle.borderColor,
                    Color.clear, chart.settings.cicleSmoothness);
            }
        }

        private int GetRingIndex(Vector2 local)
        {
            var dist = Vector2.Distance(local, serie.context.center);
            if (dist > serie.context.outsideRadius) return -1;
            Vector2 dir = local - new Vector2(serie.context.center.x, serie.context.center.y);
            float angle = VectorAngle(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (dist >= serieData.context.insideRadius &&
                    dist <= serieData.context.outsideRadius &&
                    IsInAngle(serieData, angle, serie.clockwise))
                {
                    return i;
                }
            }
            return -1;
        }

        private bool IsInAngle(SerieData serieData, float angle, bool clockwise)
        {
            if (clockwise)
                return angle >= serieData.context.startAngle && angle <= serieData.context.toAngle;
            else
                return angle >= serieData.context.toAngle && angle <= serieData.context.startAngle;
        }

        private float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
        }

        private void UpdateLabelPosition(SerieData serieData)
        {
            if (serieData.labelObject == null) return;
            var label = SerieHelper.GetSerieLabel(serie, serieData);
            var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
            var centerRadius = (serieData.context.outsideRadius + serieData.context.insideRadius) / 2;
            var startAngle = serieData.context.startAngle;
            var toAngle = serieData.context.toAngle;
            switch (label.position)
            {
                case LabelStyle.Position.Bottom:
                case LabelStyle.Position.Start:
                    var px1 = Mathf.Sin(startAngle * Mathf.Deg2Rad) * centerRadius;
                    var py1 = Mathf.Cos(startAngle * Mathf.Deg2Rad) * centerRadius;
                    var xDiff = serie.clockwise ? -label.distance : label.distance;

                    if (labelLine != null && labelLine.show)
                    {
                        serieData.context.labelLinePosition = serie.context.center + new Vector3(px1, py1) + labelLine.GetStartSymbolOffset();
                        serieData.context.labelPosition = GetLabelLineEndPosition(serie, serieData, labelLine) + new Vector3(xDiff, 0);
                    }
                    else
                    {
                        serieData.context.labelLinePosition = serie.context.center + new Vector3(px1 + xDiff, py1);
                        serieData.context.labelPosition = serieData.context.labelLinePosition;
                    }
                    break;
                case LabelStyle.Position.Top:
                case LabelStyle.Position.End:
                case LabelStyle.Position.Outside:
                    startAngle += serie.clockwise ? -label.distance : label.distance;
                    toAngle += serie.clockwise ? label.distance : -label.distance;
                    var px2 = Mathf.Sin(toAngle * Mathf.Deg2Rad) * centerRadius;
                    var py2 = Mathf.Cos(toAngle * Mathf.Deg2Rad) * centerRadius;

                    if (labelLine != null && labelLine.show)
                    {
                        serieData.context.labelLinePosition = serie.context.center + new Vector3(px2, py2) + labelLine.GetStartSymbolOffset();
                        serieData.context.labelPosition = GetLabelLineEndPosition(serie, serieData, labelLine);
                    }
                    else
                    {
                        serieData.context.labelLinePosition = serie.context.center + new Vector3(px2, py2);
                        serieData.context.labelPosition = serieData.context.labelLinePosition;
                    }
                    break;
                default: //LabelStyle.Position.Center
                    serieData.context.labelLinePosition = serie.context.center + label.offset;
                    serieData.context.labelPosition = serieData.context.labelLinePosition;
                    break;
            }
        }

        private void AvoidLabelOverlap()
        {
            if (!serie.avoidLabelOverlap) return;
            serie.context.sortedData.Clear();
            foreach (var serieData in serie.data)
            {
                serie.context.sortedData.Add(serieData);
            }
            serie.context.sortedData.Sort(delegate (SerieData a, SerieData b)
            {
                if (a == null || b == null) return 0;
                return a.context.labelPosition.y.CompareTo(b.context.labelPosition.y);
            });
            var startY = serie.context.sortedData[0].context.labelPosition.y;
            for (int i = 1; i < serie.context.sortedData.Count; i++)
            {
                var serieData = serie.context.sortedData[i];
                var fontSize = serieData.labelObject.GetHeight();
                if (serieData.context.labelPosition.y - startY < fontSize)
                {
                    serieData.context.labelPosition.y = startY + fontSize;
                }
                startY = serieData.context.labelPosition.y;
            }
        }

        private void DrawRingLabelLine(VertexHelper vh, Serie serie, SerieData serieData, Color32 defaltColor)
        {
            var serieLabel = SerieHelper.GetSerieLabel(serie, serieData);
            var labelLine = SerieHelper.GetSerieLabelLine(serie, serieData);
            if (serieLabel != null && serieLabel.show &&
                labelLine != null && labelLine.show)
            {
                var color = ChartHelper.IsClearColor(labelLine.lineColor) ?
                    ChartHelper.GetHighlightColor(defaltColor, 0.9f) :
                    labelLine.lineColor;
                var isRight = !serie.clockwise;
                var rad = Mathf.Deg2Rad * (isRight ? labelLine.lineAngle : 180 - labelLine.lineAngle);
                var lineLength1 = ChartHelper.GetActualValue(labelLine.lineLength1, serie.context.outsideRadius);
                var pos1 = serieData.context.labelLinePosition;
                var pos2 = pos1 + new Vector3(Mathf.Cos(rad) * lineLength1, Mathf.Sin(rad) * lineLength1);
                var pos5 = serieData.context.labelPosition;
                switch (labelLine.lineType)
                {
                    case LabelLine.LineType.BrokenLine:
                        UGL.DrawLine(vh, pos1, pos2, pos5, labelLine.lineWidth, color);
                        break;
                    case LabelLine.LineType.Curves:
                        UGL.DrawCurves(vh, pos1, pos5, pos1, pos2, labelLine.lineWidth, color,
                            chart.settings.lineSmoothness, UGL.Direction.XAxis);
                        break;
                    case LabelLine.LineType.HorizontalLine:
                        UGL.DrawLine(vh, pos1, pos5, labelLine.lineWidth, color);
                        break;
                }
                DrawLabelLineSymbol(vh, labelLine, pos1, pos5, color);
            }
        }
    }
}