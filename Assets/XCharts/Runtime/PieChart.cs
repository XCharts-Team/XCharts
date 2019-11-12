/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/PieChart", 15)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class PieChart : BaseChart
    {
        private bool isDrawPie;
        private bool m_IsEnterLegendButtom;

        protected override void Awake()
        {
            base.Awake();
            raycastTarget = false;
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "PieChart";
            RemoveData();
            AddSerie(SerieType.Pie, "serie1");
            AddData(0, 70, "pie1");
            AddData(0, 20, "pie2");
            AddData(0, 10, "pie3");
        }
#endif

        protected override void Update()
        {
            base.Update();
            if (!isDrawPie) RefreshChart();
        }

        protected override void DrawChart(VertexHelper vh)
        {
            base.DrawChart(vh);
            int serieNameCount = -1;
            bool isClickOffset = false;
            bool isDataHighlight = false;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                serie.index = i;
                var data = serie.data;
                serie.animation.InitProgress(data.Count, 0, 360);
                if (!serie.show)
                {
                    continue;
                }
                bool isFinish = true;
                if (serie.pieClickOffset) isClickOffset = true;
                serie.runtimePieDataMax = serie.yMax;
                serie.runtimePieDataTotal = serie.yTotal;
                UpdatePieCenter(serie);

                float totalDegree = 360;
                float startDegree = 0;
                int showdataCount = 0;
                foreach (var sd in serie.data)
                {
                    if (sd.show && serie.pieRoseType == RoseType.Area) showdataCount++;
                    sd.canShowLabel = false;
                }
                for (int n = 0; n < data.Count; n++)
                {
                    if (!serie.animation.NeedAnimation(n)) break;
                    var serieData = data[n];
                    serieData.index = n;
                    float value = serieData.data[1];
                    serieNameCount = m_LegendRealShowName.IndexOf(serieData.legendName);
                    Color color = m_ThemeInfo.GetColor(serieNameCount);
                    serieData.runtimePieStartAngle = startDegree;
                    serieData.runtimePieToAngle = startDegree;
                    serieData.runtimePieHalfAngle = startDegree;
                    serieData.runtimePieCurrAngle = startDegree;
                    if (!serieData.show)
                    {
                        continue;
                    }
                    float degree = serie.pieRoseType == RoseType.Area ?
                        (totalDegree / showdataCount) : (totalDegree * value / serie.runtimePieDataTotal);
                    serieData.runtimePieToAngle = startDegree + degree;

                    serieData.runtimePieOutsideRadius = serie.pieRoseType > 0 ?
                        serie.runtimePieInsideRadius + (serie.runtimePieOutsideRadius - serie.runtimePieInsideRadius) * value / serie.runtimePieDataMax :
                        serie.runtimePieOutsideRadius;
                    if (serieData.highlighted)
                    {
                        isDataHighlight = true;
                        color *= 1.2f;
                        serieData.runtimePieOutsideRadius += m_Settings.pieTooltipExtraRadius;
                    }
                    var offset = serie.pieSpace;
                    if (serie.pieClickOffset && serieData.selected)
                    {
                        offset += m_Settings.pieSelectedOffset;
                    }
                    var halfDegree = (serieData.runtimePieToAngle - startDegree) / 2;
                    serieData.runtimePieHalfAngle = startDegree + halfDegree;
                    float currRad = serieData.runtimePieHalfAngle * Mathf.Deg2Rad;
                    float currSin = Mathf.Sin(currRad);
                    float currCos = Mathf.Cos(currRad);
                    var center = serie.runtimePieCenterPos;

                    serieData.runtimePieCurrAngle = serieData.runtimePieToAngle;
                    serieData.runtiemPieOffsetCenter = center;
                    serieData.runtimePieInsideRadius = serie.runtimePieInsideRadius;
                    if (serie.animation.CheckDetailBreak(n, serieData.runtimePieToAngle))
                    {
                        isFinish = false;
                        serieData.runtimePieCurrAngle = serie.animation.GetCurrDetail();
                    }
                    if (offset > 0)
                    {
                        serieData.runtimePieOffsetRadius = serie.pieSpace / Mathf.Sin(halfDegree * Mathf.Deg2Rad);
                        serieData.runtimePieInsideRadius -= serieData.runtimePieOffsetRadius;
                        serieData.runtimePieOutsideRadius -= serieData.runtimePieOffsetRadius;
                        if (serie.pieClickOffset && serieData.selected)
                        {
                            serieData.runtimePieOffsetRadius += m_Settings.pieSelectedOffset;
                            if (serieData.runtimePieInsideRadius > 0) serieData.runtimePieInsideRadius += m_Settings.pieSelectedOffset;
                            serieData.runtimePieOutsideRadius += m_Settings.pieSelectedOffset;
                        }

                        serieData.runtiemPieOffsetCenter = new Vector3(center.x + serieData.runtimePieOffsetRadius * currSin,
                            center.y + serieData.runtimePieOffsetRadius * currCos);

                        ChartDrawer.DrawDoughnut(vh, serieData.runtiemPieOffsetCenter, serieData.runtimePieInsideRadius, serieData.runtimePieOutsideRadius,
                            color, m_ThemeInfo.backgroundColor, m_Settings.cicleSmoothness, startDegree, serieData.runtimePieCurrAngle);
                    }
                    else
                    {
                        ChartDrawer.DrawDoughnut(vh, center, serieData.runtimePieInsideRadius, serieData.runtimePieOutsideRadius,
                            color, m_ThemeInfo.backgroundColor, m_Settings.cicleSmoothness, startDegree, serieData.runtimePieCurrAngle);
                    }
                    serieData.canShowLabel = serieData.runtimePieCurrAngle >= serieData.runtimePieHalfAngle;
                    isDrawPie = true;
                    startDegree = serieData.runtimePieToAngle;
                    if (isFinish) serie.animation.SetDataFinish(n);
                    else break;
                }
                if (!serie.animation.IsFinish())
                {
                    float duration = serie.animation.duration > 0 ? (float)serie.animation.duration / 1000 : 1;
                    float speed = 360 / duration;
                    float symbolSpeed = serie.symbol.size / duration;
                    serie.animation.CheckProgress(Time.deltaTime * speed);
                    serie.animation.CheckSymbol(Time.deltaTime * symbolSpeed, serie.symbol.size);
                    RefreshChart();
                }
            }
            DrawLabelLine(vh);
            DrawLabelBackground(vh);
            raycastTarget = isClickOffset && isDataHighlight;
        }

        private void DrawLabelLine(VertexHelper vh)
        {
            foreach (var serie in m_Series.list)
            {
                if (serie.type == SerieType.Pie && serie.label.show)
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.canShowLabel)
                        {
                            int colorIndex = m_LegendRealShowName.IndexOf(serieData.name);
                            Color color = m_ThemeInfo.GetColor(colorIndex);
                            DrawLabelLine(vh, serie, serieData, color);
                        }
                    }
                }
            }
        }

        private void DrawLabelBackground(VertexHelper vh)
        {
            foreach (var serie in m_Series.list)
            {
                if (serie.type == SerieType.Pie && serie.label.show)
                {
                    foreach (var serieData in serie.data)
                    {
                        if (serieData.canShowLabel)
                        {
                            UpdateLabelPostion(serie, serieData);
                            DrawLabelBackground(vh, serie, serieData);
                        }
                    }
                }
            }
        }

        private void DrawLabelLine(VertexHelper vh, Serie serie, SerieData serieData, Color color)
        {
            if (serie.label.show
                && serie.label.position == SerieLabel.Position.Outside
                && serie.label.line)
            {
                var insideRadius = serieData.runtimePieInsideRadius;
                var outSideRadius = serieData.runtimePieOutsideRadius;
                var center = serie.runtimePieCenterPos;
                var currAngle = serieData.runtimePieHalfAngle;
                if (serie.label.lineColor != Color.clear) color = serie.label.lineColor;
                else if (serie.label.lineType == SerieLabel.LineType.HorizontalLine) color *= color;
                float currSin = Mathf.Sin(currAngle * Mathf.Deg2Rad);
                float currCos = Mathf.Cos(currAngle * Mathf.Deg2Rad);
                var radius1 = serie.label.lineType == SerieLabel.LineType.HorizontalLine ?
                    serie.runtimePieOutsideRadius : outSideRadius;
                var radius2 = serie.runtimePieOutsideRadius + serie.label.lineLength1;
                var radius3 = insideRadius + (outSideRadius - insideRadius) / 2;
                if (radius1 < serie.runtimePieInsideRadius) radius1 = serie.runtimePieInsideRadius;
                radius1 -= 0.1f;
                var pos0 = new Vector3(center.x + radius3 * currSin, center.y + radius3 * currCos);
                var pos1 = new Vector3(center.x + radius1 * currSin, center.y + radius1 * currCos);
                var pos2 = new Vector3(center.x + radius2 * currSin, center.y + radius2 * currCos);
                float tx, ty;
                Vector3 pos3, pos4, pos6;
                var horizontalLineCircleRadius = serie.label.lineWidth * 4f;
                var lineCircleDiff = horizontalLineCircleRadius - 0.3f;
                if (currAngle < 90)
                {
                    ty = serie.label.lineWidth * Mathf.Cos((90 - currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Sin((90 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x - tx, pos2.y + ty - serie.label.lineWidth);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 180)
                {
                    ty = serie.label.lineWidth * Mathf.Sin((180 - currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Cos((180 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x - tx, pos2.y - ty + serie.label.lineWidth);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.right * lineCircleDiff;
                    pos4 = pos6 + Vector3.right * r4;
                }
                else if (currAngle < 270)
                {
                    ty = serie.label.lineWidth * Mathf.Sin((180 + currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Cos((180 + currAngle) * Mathf.Deg2Rad);
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x + tx, pos2.y - ty + serie.label.lineWidth);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                else
                {
                    ty = serie.label.lineWidth * Mathf.Cos((90 + currAngle) * Mathf.Deg2Rad);
                    tx = serie.label.lineWidth * Mathf.Sin((90 + currAngle) * Mathf.Deg2Rad);
                    pos3 = new Vector3(pos2.x + tx, pos2.y + ty - serie.label.lineWidth);
                    var currSin1 = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                    var currCos1 = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                    var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos1 * radius3, 2)) - currSin1 * radius3;
                    r4 += serie.label.lineLength1 - lineCircleDiff;
                    pos6 = pos0 + Vector3.left * lineCircleDiff;
                    pos4 = pos6 + Vector3.left * r4;
                }
                var pos5 = new Vector3(currAngle > 180 ? pos3.x - serie.label.lineLength2 : pos3.x + serie.label.lineLength2, pos3.y);
                switch (serie.label.lineType)
                {
                    case SerieLabel.LineType.BrokenLine:
                        ChartDrawer.DrawLine(vh, pos1, pos2, serie.label.lineWidth, color);
                        ChartDrawer.DrawLine(vh, pos3, pos5, serie.label.lineWidth, color);
                        break;
                    case SerieLabel.LineType.Curves:
                        ChartDrawer.DrawCurves(vh, pos1, pos5, pos1, pos2, serie.label.lineWidth, color, m_Settings.lineSmoothness);
                        break;
                    case SerieLabel.LineType.HorizontalLine:
                        ChartDrawer.DrawCricle(vh, pos0, horizontalLineCircleRadius, color);
                        ChartDrawer.DrawLine(vh, pos6, pos4, serie.label.lineWidth, color);
                        break;
                }
            }
        }

        protected override void OnRefreshLabel()
        {
            int serieNameCount = -1;
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.list[i];
                serie.index = i;
                if (!serie.show) continue;
                var data = serie.data;

                int showdataCount = 0;
                if (serie.pieRoseType == RoseType.Area)
                {
                    foreach (var sd in serie.data)
                    {
                        if (sd.show) showdataCount++;
                    }
                }
                for (int n = 0; n < data.Count; n++)
                {
                    var serieData = data[n];
                    if (!serieData.canShowLabel)
                    {
                        serieData.SetLabelActive(false);
                        continue;
                    }
                    if (!serieData.show) continue;
                    serieNameCount = m_LegendRealShowName.IndexOf(serieData.name);
                    Color color = m_ThemeInfo.GetColor(serieNameCount);

                    DrawLabel(serie, n, serieData, color);
                }
            }
        }

        private void DrawLabel(Serie serie, int dataIndex, SerieData serieData, Color serieColor)
        {
            if (serieData.labelText == null) return;
            var currAngle = serieData.runtimePieHalfAngle;
            var isHighlight = (serieData.highlighted && serie.emphasis.label.show);
            var showLabel = ((serie.label.show || isHighlight) && serieData.canShowLabel);
            if (showLabel || serieData.iconStyle.show)
            {
                serieData.SetLabelActive(showLabel);
                float rotate = 0;
                bool isInsidePosition = serie.label.position == SerieLabel.Position.Inside;
                if (serie.label.rotate > 0 && isInsidePosition)
                {
                    if (currAngle > 180) rotate += 270 - currAngle;
                    else rotate += -(currAngle - 90);
                }
                Color color = serieColor;
                if (isHighlight)
                {
                    if (serie.emphasis.label.color != Color.clear) color = serie.emphasis.label.color;
                }
                else if (serie.label.color != Color.clear)
                {
                    color = serie.label.color;
                }
                else
                {
                    color = isInsidePosition ? Color.white : serieColor;
                }
                var fontSize = isHighlight ? serie.emphasis.label.fontSize : serie.label.fontSize;
                var fontStyle = isHighlight ? serie.emphasis.label.fontStyle : serie.label.fontStyle;

                serieData.labelText.color = color;
                serieData.labelText.fontSize = fontSize;
                serieData.labelText.fontStyle = fontStyle;

                serieData.labelRect.transform.localEulerAngles = new Vector3(0, 0, rotate);

                UpdateLabelPostion(serie, serieData);
                if (!string.IsNullOrEmpty(serie.label.formatter))
                {
                    var value = serieData.data[1];
                    var total = serie.yTotal;
                    var content = serie.label.GetFormatterContent(serie.name, serieData.name, value, total);
                    if (serieData.SetLabelText(content)) RefreshChart();
                }
                serieData.SetGameObjectPosition(serieData.labelPosition);
                if (showLabel) serieData.SetLabelPosition(serie.label.offset);
            }
            else
            {
                serieData.SetLabelActive(false);
            }
            serieData.UpdateIcon();
        }

        protected void UpdateLabelPostion(Serie serie, SerieData serieData)
        {
            if (serieData.labelText == null) return;
            var currAngle = serieData.runtimePieHalfAngle;
            var currRad = currAngle * Mathf.Deg2Rad;
            var offsetRadius = serieData.runtimePieOffsetRadius;
            var insideRadius = serieData.runtimePieInsideRadius;
            var outsideRadius = serieData.runtimePieOutsideRadius;
            switch (serie.label.position)
            {
                case SerieLabel.Position.Center:
                    serieData.labelPosition = serie.runtimePieCenterPos;
                    break;
                case SerieLabel.Position.Inside:
                    var labelRadius = offsetRadius + insideRadius + (outsideRadius - insideRadius) / 2;
                    var labelCenter = new Vector2(serie.runtimePieCenterPos.x + labelRadius * Mathf.Sin(currRad),
                        serie.runtimePieCenterPos.y + labelRadius * Mathf.Cos(currRad));
                    serieData.labelPosition = labelCenter;
                    break;
                case SerieLabel.Position.Outside:
                    if (serie.label.lineType == SerieLabel.LineType.HorizontalLine)
                    {
                        var radius1 = serie.runtimePieOutsideRadius;
                        var radius3 = insideRadius + (outsideRadius - insideRadius) / 2;
                        var currSin = Mathf.Sin(currRad);
                        var currCos = Mathf.Cos(currRad);
                        var pos0 = new Vector3(serie.runtimePieCenterPos.x + radius3 * currSin, serie.runtimePieCenterPos.y + radius3 * currCos);
                        if (currAngle > 180)
                        {
                            currSin = Mathf.Sin((360 - currAngle) * Mathf.Deg2Rad);
                            currCos = Mathf.Cos((360 - currAngle) * Mathf.Deg2Rad);
                        }
                        var r4 = Mathf.Sqrt(radius1 * radius1 - Mathf.Pow(currCos * radius3, 2)) - currSin * radius3;
                        r4 += serie.label.lineLength1 + serie.label.lineWidth * 4;
                        r4 += serieData.labelText.preferredWidth / 2;
                        serieData.labelPosition = pos0 + (currAngle > 180 ? Vector3.left : Vector3.right) * r4;
                    }
                    else
                    {
                        labelRadius = serie.runtimePieOutsideRadius + serie.label.lineLength1;
                        labelCenter = new Vector2(serie.runtimePieCenterPos.x + labelRadius * Mathf.Sin(currRad),
                            serie.runtimePieCenterPos.y + labelRadius * Mathf.Cos(currRad));
                        float labelWidth = serieData.labelText.preferredWidth;
                        if (currAngle > 180)
                        {
                            serieData.labelPosition = new Vector2(labelCenter.x - serie.label.lineLength2 - 5 - labelWidth / 2, labelCenter.y);
                        }
                        else
                        {
                            serieData.labelPosition = new Vector2(labelCenter.x + serie.label.lineLength2 + 5 + labelWidth / 2, labelCenter.y);
                        }
                    }
                    break;
            }
        }

        protected override void OnLegendButtonClick(int index, string legendName, bool show)
        {
            bool active = CheckDataShow(legendName, show);
            var bgColor1 = active ? m_ThemeInfo.GetColor(index) : m_ThemeInfo.legendUnableColor;
            m_Legend.UpdateButtonColor(legendName, bgColor1);
            RefreshChart();
        }

        protected override void OnLegendButtonEnter(int index, string legendName)
        {
            m_IsEnterLegendButtom = true;
            CheckDataHighlighted(legendName, true);
            RefreshChart();
        }

        protected override void OnLegendButtonExit(int index, string legendName)
        {
            m_IsEnterLegendButtom = false;
            CheckDataHighlighted(legendName, false);
            RefreshChart();
        }

        private void UpdatePieCenter(Serie serie)
        {
            if (serie.pieCenter.Length < 2) return;
            var centerX = serie.pieCenter[0] <= 1 ? chartWidth * serie.pieCenter[0] : serie.pieCenter[0];
            var centerY = serie.pieCenter[1] <= 1 ? chartHeight * serie.pieCenter[1] : serie.pieCenter[1];
            serie.runtimePieCenterPos = new Vector2(centerX, centerY);
            var minWidth = Mathf.Min(chartWidth, chartHeight);
            serie.runtimePieInsideRadius = serie.pieRadius[0] <= 1 ? minWidth * serie.pieRadius[0] : serie.pieRadius[0];
            serie.runtimePieOutsideRadius = serie.pieRadius[1] <= 1 ? minWidth * serie.pieRadius[1] : serie.pieRadius[1];
        }

        protected override void CheckTootipArea(Vector2 local)
        {
            if (m_IsEnterLegendButtom) return;
            m_Tooltip.runtimeDataIndex.Clear();
            bool selected = false;
            foreach (var serie in m_Series.list)
            {
                int index = GetPosPieIndex(serie, local);
                m_Tooltip.runtimeDataIndex.Add(index);
                if (serie.type != SerieType.Pie) continue;
                bool refresh = false;
                for (int j = 0; j < serie.data.Count; j++)
                {
                    var serieData = serie.data[j];
                    if (serieData.highlighted != (j == index)) refresh = true;
                    serieData.highlighted = j == index;
                }
                if (index >= 0) selected = true;
                if (refresh) RefreshChart();
            }
            if (selected)
            {
                m_Tooltip.UpdateContentPos(new Vector2(local.x + 18, local.y - 25));
                RefreshTooltip();
            }
            else if (m_Tooltip.IsActive())
            {
                m_Tooltip.SetActive(false);
                RefreshChart();
            }
        }

        private int GetPosPieIndex(Serie serie, Vector2 local)
        {
            if (serie.type != SerieType.Pie) return -1;
            var dist = Vector2.Distance(local, serie.runtimePieCenterPos);
            if (dist < serie.runtimePieInsideRadius || dist > serie.runtimePieOutsideRadius) return -1;
            Vector2 dir = local - new Vector2(serie.runtimePieCenterPos.x, serie.runtimePieCenterPos.y);
            float angle = VectorAngle(Vector2.up, dir);
            for (int i = 0; i < serie.data.Count; i++)
            {
                var serieData = serie.data[i];
                if (angle >= serieData.runtimePieStartAngle && angle <= serieData.runtimePieToAngle)
                {
                    return i;
                }
            }
            return -1;
        }

        float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
        }

        StringBuilder sb = new StringBuilder();
        protected override void RefreshTooltip()
        {
            base.RefreshTooltip();
            bool showTooltip = false;
            foreach (var serie in m_Series.list)
            {
                int index = m_Tooltip.runtimeDataIndex[serie.index];
                if (index < 0) continue;
                showTooltip = true;
                if (string.IsNullOrEmpty(tooltip.formatter))
                {
                    string key = serie.data[index].name;
                    if (string.IsNullOrEmpty(key)) key = m_Legend.GetData(index);

                    float value = serie.data[index].data[1];
                    sb.Length = 0;
                    if (!string.IsNullOrEmpty(serie.name))
                    {
                        sb.Append(serie.name).Append("\n");
                    }
                    sb.Append("<color=#").Append(m_ThemeInfo.GetColorStr(index)).Append(">● </color>")
                        .Append(key).Append(": ").Append(ChartCached.FloatToStr(value, 0, m_Tooltip.forceENotation));
                    m_Tooltip.UpdateContentText(sb.ToString());
                }
                else
                {
                    m_Tooltip.UpdateContentText(m_Tooltip.GetFormatterContent(index, m_Series, null));
                }

                var pos = m_Tooltip.GetContentPos();
                if (pos.x + m_Tooltip.runtimeWidth > chartWidth)
                {
                    pos.x = chartWidth - m_Tooltip.runtimeWidth;
                }
                if (pos.y - m_Tooltip.runtimeHeight < 0)
                {
                    pos.y = m_Tooltip.runtimeHeight;
                }
                m_Tooltip.UpdateContentPos(pos);
            }
            m_Tooltip.SetActive(showTooltip);
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            Vector2 local;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform,
                eventData.position, canvas.worldCamera, out local))
            {
                return;
            }
            for (int i = 0; i < m_Series.Count; i++)
            {
                var serie = m_Series.GetSerie(i);
                if (serie.type == SerieType.Pie)
                {
                    var index = GetPosPieIndex(serie, local);
                    if (index >= 0)
                    {
                        for (int j = 0; j < serie.data.Count; j++)
                        {
                            if (j == index) serie.data[j].selected = !serie.data[j].selected;
                            else serie.data[j].selected = false;
                        }
                    }
                }
            }
            RefreshChart();
        }
    }
}
