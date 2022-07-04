using System;
using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;
using XUGL;

namespace XCharts
{
    public abstract class AxisHandler<T> : MainComponentHandler
    where T : Axis
    {
        private static readonly string s_DefaultAxisName = "name";
        private double m_LastInterval = double.MinValue;
        private int m_LastSplitNumber = int.MinValue;
        public T component { get; internal set; }

        internal override void SetComponent(MainComponent component)
        {
            this.component = (T) component;
        }

        protected virtual Vector3 GetLabelPosition(float scaleWid, int i)
        {
            return Vector3.zero;
        }

        protected virtual float GetAxisLineXOrY()
        {
            return 0;
        }

        protected virtual Orient orient { get; set; }

        protected virtual void UpdatePointerValue(Axis axis)
        {
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            if (grid == null)
                return;
            if (!grid.context.isPointerEnter)
            {
                axis.context.pointerValue = double.PositiveInfinity;
            }
            else
            {
                var lastPointerValue = axis.context.pointerValue;
                if (axis.IsCategory())
                {
                    var dataZoom = chart.GetDataZoomOfAxis(axis);
                    var dataCount = chart.series.Count > 0 ? chart.series[0].GetDataList(dataZoom).Count : 0;
                    var local = chart.pointerPos;
                    if (axis is YAxis)
                    {
                        float splitWid = AxisHelper.GetDataWidth(axis, grid.context.height, dataCount, dataZoom);
                        for (int j = 0; j < axis.GetDataCount(dataZoom); j++)
                        {
                            float pY = grid.context.y + j * splitWid;
                            if ((axis.boundaryGap && (local.y > pY && local.y <= pY + splitWid)) ||
                                (!axis.boundaryGap && (local.y > pY - splitWid / 2 && local.y <= pY + splitWid / 2)))
                            {
                                axis.context.pointerValue = j;
                                axis.context.pointerLabelPosition = axis.GetLabelObjectPosition(j);
                                if (j != lastPointerValue)
                                {
                                    if (chart.onAxisPointerValueChanged != null)
                                        chart.onAxisPointerValueChanged(axis, j);
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        float splitWid = AxisHelper.GetDataWidth(axis, grid.context.width, dataCount, dataZoom);
                        for (int j = 0; j < axis.GetDataCount(dataZoom); j++)
                        {
                            float pX = grid.context.x + j * splitWid;
                            if ((axis.boundaryGap && (local.x > pX && local.x <= pX + splitWid)) ||
                                (!axis.boundaryGap && (local.x > pX - splitWid / 2 && local.x <= pX + splitWid / 2)))
                            {
                                axis.context.pointerValue = j;
                                axis.context.pointerLabelPosition = axis.GetLabelObjectPosition(j);
                                if (j != lastPointerValue)
                                {
                                    if (chart.onAxisPointerValueChanged != null)
                                        chart.onAxisPointerValueChanged(axis, j);
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (axis is YAxis)
                    {
                        var yRate = axis.context.minMaxRange / grid.context.height;
                        var yValue = yRate * (chart.pointerPos.y - grid.context.y - axis.context.offset);
                        if (axis.context.minValue > 0)
                            yValue += axis.context.minValue;

                        var labelX = axis.GetLabelObjectPosition(0).x;
                        axis.context.pointerValue = yValue;
                        axis.context.pointerLabelPosition = new Vector3(labelX, chart.pointerPos.y);
                        if (yValue != lastPointerValue)
                        {
                            if (chart.onAxisPointerValueChanged != null)
                                chart.onAxisPointerValueChanged(axis, yValue);
                        }
                    }
                    else
                    {
                        var xRate = axis.context.minMaxRange / grid.context.width;
                        var xValue = xRate * (chart.pointerPos.x - grid.context.x - axis.context.offset);
                        if (axis.context.minValue > 0)
                            xValue += axis.context.minValue;

                        var labelY = axis.GetLabelObjectPosition(0).y;
                        axis.context.pointerValue = xValue;
                        axis.context.pointerLabelPosition = new Vector3(chart.pointerPos.x, labelY);
                        if (xValue != lastPointerValue)
                        {
                            if (chart.onAxisPointerValueChanged != null)
                                chart.onAxisPointerValueChanged(axis, xValue);
                        }
                    }
                }
            }
        }

        internal void UpdateAxisMinMaxValue(int axisIndex, Axis axis, bool updateChart = true)
        {
            if (!axis.show)
                return;

            if (axis.IsCategory())
            {
                axis.context.minValue = 0;
                axis.context.maxValue = SeriesHelper.GetMaxSerieDataCount(chart.series) - 1;
                axis.context.minMaxRange = axis.context.maxValue;
                return;
            }

            double tempMinValue = 0;
            double tempMaxValue = 0;
            chart.GetSeriesMinMaxValue(axis, axisIndex, out tempMinValue, out tempMaxValue);

            if (tempMinValue != axis.context.minValue ||
                tempMaxValue != axis.context.maxValue ||
                m_LastInterval != axis.interval ||
                m_LastSplitNumber != axis.splitNumber)
            {
                m_LastSplitNumber = axis.splitNumber;
                m_LastInterval = axis.interval;

                axis.UpdateMinMaxValue(tempMinValue, tempMaxValue);
                axis.context.offset = 0;
                axis.context.lastCheckInverse = axis.inverse;
                UpdateAxisTickValueList(axis);

                if (tempMinValue != 0 || tempMaxValue != 0)
                {
                    var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
                    if (grid != null && axis is XAxis && axis.IsValue())
                    {
                        axis.context.offset = axis.context.minValue > 0 ?
                            0 :
                            (axis.context.maxValue < 0 ?
                                grid.context.width :
                                (float) (Math.Abs(axis.context.minValue) * (grid.context.width /
                                    (Math.Abs(axis.context.minValue) + Math.Abs(axis.context.maxValue))))
                            );
                        axis.context.x = grid.context.x;
                        axis.context.y = GetAxisLineXOrY();
                        axis.context.zeroY = grid.context.y;
                        axis.context.zeroX = grid.context.x - (float) (axis.context.minValue * grid.context.width / axis.context.minMaxRange);
                    }
                    if (grid != null && axis is YAxis && axis.IsValue())
                    {
                        axis.context.offset = axis.context.minValue > 0 ?
                            0 :
                            (axis.context.maxValue < 0 ?
                                grid.context.height :
                                (float) (Math.Abs(axis.context.minValue) * (grid.context.height /
                                    (Math.Abs(axis.context.minValue) + Math.Abs(axis.context.maxValue))))
                            );
                        axis.context.x = GetAxisLineXOrY();
                        axis.context.y = grid.context.y;
                        axis.context.zeroX = grid.context.x;
                        axis.context.zeroY = grid.context.y - (float) (axis.context.minValue * grid.context.height / axis.context.minMaxRange);
                    }
                }
                var dataZoom = chart.GetDataZoomOfAxis(axis);
                if (dataZoom != null && dataZoom.enable)
                {
                    if (axis is XAxis)
                        dataZoom.SetXAxisIndexValueInfo(axisIndex, tempMinValue, tempMaxValue);
                    else
                        dataZoom.SetYAxisIndexValueInfo(axisIndex, tempMinValue, tempMaxValue);
                }
                if (updateChart)
                {
                    UpdateAxisLabelText(axis);
                    chart.RefreshChart();
                }
            }
        }

        internal virtual void UpdateAxisLabelText(Axis axis)
        {
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            if (grid == null || axis == null)
                return;

            float runtimeWidth = axis is XAxis ? grid.context.width : grid.context.height;
            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series);
            var dataZoom = chart.GetDataZoomOfAxis(axis);

            axis.UpdateLabelText(runtimeWidth, dataZoom, isPercentStack);
        }

        internal void UpdateAxisTickValueList(Axis axis)
        {
            if (axis.IsTime())
            {
                var lastCount = axis.context.labelValueList.Count;
                DateTimeUtil.UpdateTimeAxisDateTimeList(axis.context.labelValueList, (int) axis.context.minValue,
                    (int) axis.context.maxValue, axis.splitNumber);

                if (axis.context.labelValueList.Count != lastCount)
                    axis.SetAllDirty();
            }
            else if (axis.IsValue())
            {
                var list = axis.context.labelValueList;
                var lastCount = list.Count;
                list.Clear();

                var range = axis.context.maxValue - axis.context.minValue;
                if (range <= 0)
                    return;

                double tick = axis.interval;

                if (axis.interval == 0)
                {
                    if (axis.splitNumber > 0)
                    {
                        tick = range / axis.splitNumber;
                    }
                    else
                    {
                        var each = GetTick(range);
                        tick = each;
                        if (range / tick > 8)
                            tick = 2 * each;
                        else if (range / tick < 4)
                            tick = each / 2;
                    }
                }
                var value = 0d;
                if (Mathf.Approximately((float) (axis.context.minValue % tick), 0))
                {
                    value = axis.context.minValue;
                }
                else
                {
                    list.Add(axis.context.minValue);
                    value = Math.Ceiling(axis.context.minValue / tick) * tick;
                }
                var maxSplitNumber = chart.settings.axisMaxSplitNumber;
                while (value <= axis.context.maxValue)
                {
                    list.Add(value);
                    value += tick;

                    if (maxSplitNumber > 0 && list.Count > maxSplitNumber)
                        break;
                }
                if (!ChartHelper.IsEquals(axis.context.maxValue, list[list.Count - 1]))
                {
                    list.Add(axis.context.maxValue);
                }
                if (lastCount != list.Count)
                {
                    axis.SetAllDirty();
                }
            }
        }

        private static double GetTick(double max)
        {
            if (max <= 1) return max / 5;
            var bigger = Math.Ceiling(Math.Abs(max));
            int n = 1;
            while (bigger / (Mathf.Pow(10, n)) > 10)
            {
                n++;
            }
            return Math.Pow(10, n);
        }

        internal void CheckValueLabelActive(Axis axis, int i, ChartLabel label, Vector3 pos)
        {
            if (!axis.show || !axis.axisLabel.show)
            {
                label.SetTextActive(false);
                return;
            }
            if (axis.IsValue())
            {
                if (orient == Orient.Horizonal)
                {
                    if (i == 0)
                    {
                        var dist = GetLabelPosition(0, 1).x - pos.x;
                        label.SetTextActive(dist > label.text.GetPreferredWidth());
                    }
                    else if (i == axis.context.labelValueList.Count - 1)
                    {
                        var dist = pos.x - GetLabelPosition(0, i - 1).x;
                        label.SetTextActive(dist > label.text.GetPreferredWidth());
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        var dist = GetLabelPosition(0, 1).y - pos.y;
                        label.SetTextActive(dist > label.text.GetPreferredHeight());
                    }
                    else if (i == axis.context.labelValueList.Count - 1)
                    {
                        var dist = pos.y - GetLabelPosition(0, i - 1).y;
                        label.SetTextActive(dist > label.text.GetPreferredHeight());
                    }
                }
            }
        }

        protected void InitAxis(Axis relativedAxis, Orient orient,
            float axisStartX, float axisStartY, float axisLength, float relativedLength)
        {
            Axis axis = component;
            chart.InitAxisRuntimeData(axis);

            var objName = ChartCached.GetComponentObjectName(axis);
            var axisObj = ChartHelper.AddObject(objName,
                chart.transform,
                chart.chartMinAnchor,
                chart.chartMaxAnchor,
                chart.chartPivot,
                chart.chartSizeDelta);

            axisObj.SetActive(axis.show);
            axisObj.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(axisObj);

            axis.gameObject = axisObj;
            axis.context.labelObjectList.Clear();

            if (!axis.show)
                return;

            var axisLabelTextStyle = axis.axisLabel.textStyle;
            var dataZoom = chart.GetDataZoomOfAxis(axis);
            var splitNumber = AxisHelper.GetScaleNumber(axis, axisLength, dataZoom);
            var totalWidth = 0f;
            var eachWidth = AxisHelper.GetEachWidth(axis, axisLength, dataZoom);
            var gapWidth = axis.boundaryGap ? eachWidth / 2 : 0;

            var textWidth = axis.axisLabel.width > 0 ?
                axis.axisLabel.width :
                (orient == Orient.Horizonal ?
                    AxisHelper.GetScaleWidth(axis, axisLength, 0, dataZoom) :
                    (axisStartX - chart.chartX)
                );

            var textHeight = axis.axisLabel.height > 0 ?
                axis.axisLabel.height :
                20f;

            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series);
            var inside = axis.axisLabel.inside;
            var defaultAlignment = orient == Orient.Horizonal ? TextAnchor.MiddleCenter :
                ((inside && axis.IsLeft()) || (!inside && axis.IsRight()) ?
                    TextAnchor.MiddleLeft :
                    TextAnchor.MiddleRight);

            if (axis.IsCategory() && axis.boundaryGap)
                splitNumber -= 1;

            for (int i = 0; i < splitNumber; i++)
            {
                var labelWidth = AxisHelper.GetScaleWidth(axis, axisLength, i + 1, dataZoom);
                var labelName = AxisHelper.GetLabelName(axis, axisLength, i,
                    axis.context.minValue,
                    axis.context.maxValue,
                    dataZoom, isPercentStack);

                var label = ChartHelper.AddAxisLabelObject(splitNumber, i,
                    ChartCached.GetAxisLabelName(i),
                    axisObj.transform,
                    new Vector2(textWidth, textHeight),
                    axis, chart.theme.axis, labelName,
                    Color.clear,
                    defaultAlignment);

                if (i == 0)
                    axis.axisLabel.SetRelatedText(label.text, labelWidth);

                var pos = GetLabelPosition(totalWidth + gapWidth, i);
                label.SetPosition(pos);
                CheckValueLabelActive(axis, i, label, pos);

                axis.context.labelObjectList.Add(label);

                totalWidth += labelWidth;
            }
            if (axis.axisName.show)
            {
                ChartLabel label = null;
                var relativedDist = (relativedAxis == null ? 0 : relativedAxis.context.offset);
                var zeroPos = new Vector3(axisStartX, axisStartY + relativedDist);
                var offset = axis.axisName.labelStyle.offset;
                var autoColor = axis.axisLine.GetColor(chart.theme.axis.lineColor);
                if (orient == Orient.Horizonal)
                {
                    var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
                    var posY = !axis.axisName.onZero && grid != null? grid.context.y : GetAxisLineXOrY() + offset.y;
                    switch (axis.axisName.labelStyle.position)
                    {
                        case LabelStyle.Position.Start:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleRight);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Top ?
                                new Vector2(zeroPos.x - offset.x, axisStartY + relativedLength + offset.y + axis.offset) :
                                new Vector2(zeroPos.x - offset.x, posY));
                            break;

                        case LabelStyle.Position.Middle:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Top ?
                                new Vector2(axisStartX + axisLength / 2 + offset.x, axisStartY + relativedLength - offset.y + axis.offset) :
                                new Vector2(axisStartX + axisLength / 2 + offset.x, posY));
                            break;

                        default:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleLeft);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Top ?
                                new Vector2(axisStartX + axisLength + offset.x, axisStartY + relativedLength + offset.y + axis.offset) :
                                new Vector2(axisStartX + axisLength + offset.x, posY));
                            break;
                    }
                }
                else
                {
                    var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
                    var posX = !axis.axisName.onZero && grid != null? grid.context.x : GetAxisLineXOrY() + offset.x;
                    switch (axis.axisName.labelStyle.position)
                    {
                        case LabelStyle.Position.Start:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Right ?
                                new Vector2(axisStartX + relativedLength + offset.x + axis.offset, axisStartY - offset.y) :
                                new Vector2(posX, axisStartY - offset.y));
                            break;

                        case LabelStyle.Position.Middle:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Right ?
                                new Vector2(axisStartX + relativedLength - offset.x + axis.offset, axisStartY + axisLength / 2 + offset.y) :
                                new Vector2(posX, axisStartY + axisLength / 2 + offset.y));
                            break;

                        default:
                            //LabelStyle.Position
                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Right ?
                                new Vector2(axisStartX + relativedLength + offset.x + axis.offset, axisStartY + axisLength + offset.y) :
                                new Vector2(posX, axisStartY + axisLength + offset.y));
                            break;
                    }
                }
            }
        }

        internal static Vector3 GetLabelPosition(int i, Orient orient, Axis axis, Axis relativedAxis, AxisTheme theme,
            float scaleWid, float axisStartX, float axisStartY, float axisLength, float relativedLength)
        {
            var inside = axis.axisLabel.inside;
            var fontSize = axis.axisLabel.textStyle.GetFontSize(theme);
            var current = axis.offset;

            if (axis.IsTime() || axis.IsValue())
            {
                scaleWid = axis.context.minMaxRange != 0 ?
                    axis.GetDistance(axis.GetLabelValue(i), axisLength) :
                    0;
            }

            if (orient == Orient.Horizonal)
            {
                if (axis.axisLabel.onZero && relativedAxis != null)
                    axisStartY += relativedAxis.context.offset;

                if (axis.IsTop())
                    axisStartY += relativedLength;

                if ((inside && axis.IsBottom()) || (!inside && axis.IsTop()))
                    current += axisStartY + axis.axisLabel.distance + fontSize / 2;
                else
                    current += axisStartY - axis.axisLabel.distance - fontSize / 2;

                return new Vector3(axisStartX + scaleWid, current) + axis.axisLabel.offset;
            }
            else
            {
                if (axis.axisLabel.onZero && relativedAxis != null)
                    axisStartX += relativedAxis.context.offset;

                if (axis.IsRight())
                    axisStartX += relativedLength;

                if ((inside && axis.IsLeft()) || (!inside && axis.IsRight()))
                    current += axisStartX + axis.axisLabel.distance;
                else
                    current += axisStartX - axis.axisLabel.distance;

                return new Vector3(current, axisStartY + scaleWid) + axis.axisLabel.offset;
            }
        }

        internal static void DrawAxisLine(VertexHelper vh, Axis axis, AxisTheme theme, Orient orient,
            float startX, float startY, float axisLength)
        {
            var inverse = axis.IsValue() && axis.inverse;
            var offset = AxisHelper.GetAxisLineArrowOffset(axis);

            var lineWidth = axis.axisLine.GetWidth(theme.lineWidth);
            var lineType = axis.axisLine.GetType(theme.lineType);
            var lineColor = axis.axisLine.GetColor(theme.lineColor);

            if (orient == Orient.Horizonal)
            {
                var left = new Vector3(startX - lineWidth - (inverse ? offset : 0), startY);
                var right = new Vector3(startX + axisLength + lineWidth + (!inverse ? offset : 0), startY);
                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, left, right, lineColor);
            }
            else
            {
                var bottom = new Vector3(startX, startY - lineWidth - (inverse ? offset : 0));
                var top = new Vector3(startX, startY + axisLength + lineWidth + (!inverse ? offset : 0));
                ChartDrawer.DrawLineStyle(vh, lineType, lineWidth, bottom, top, lineColor);
            }
        }

        internal static void DrawAxisTick(VertexHelper vh, Axis axis, AxisTheme theme, DataZoom dataZoom,
            Orient orient, float startX, float startY, float axisLength)
        {
            var lineWidth = axis.axisLine.GetWidth(theme.lineWidth);
            var tickLength = axis.axisTick.GetLength(theme.tickLength);

            if (AxisHelper.NeedShowSplit(axis))
            {
                var size = AxisHelper.GetScaleNumber(axis, axisLength, dataZoom);

                var current = orient == Orient.Horizonal ?
                    startX :
                    startY;

                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(axis, axisLength, i + 1, dataZoom);
                    if (i == 0 && (!axis.axisTick.showStartTick || axis.axisTick.alignWithLabel))
                    {
                        current += scaleWidth;
                        continue;
                    }
                    if (i == size - 1 && !axis.axisTick.showEndTick)
                    {
                        current += scaleWidth;
                        continue;
                    }
                    if (axis.axisTick.show)
                    {
                        if (orient == Orient.Horizonal)
                        {
                            float pX = axis.IsTime() ?
                                (startX + axis.GetDistance(axis.GetLabelValue(i), axisLength)) :
                                current;

                            if (axis.boundaryGap && axis.axisTick.alignWithLabel)
                                pX -= scaleWidth / 2;

                            var sY = 0f;
                            var eY = 0f;
                            if ((axis.axisTick.inside && axis.IsBottom()) ||
                                (!axis.axisTick.inside && axis.IsTop()))
                            {
                                sY = startY + lineWidth;
                                eY = sY + tickLength;
                            }
                            else
                            {
                                sY = startY - lineWidth;
                                eY = sY - tickLength;
                            }

                            UGL.DrawLine(vh, new Vector3(pX, sY), new Vector3(pX, eY),
                                axis.axisTick.GetWidth(theme.tickWidth),
                                axis.axisTick.GetColor(theme.tickColor));
                        }
                        else
                        {
                            float pY = axis.IsTime() ?
                                (startY + axis.GetDistance(axis.GetLabelValue(i), axisLength)) :
                                current;

                            if (axis.boundaryGap && axis.axisTick.alignWithLabel)
                                pY -= scaleWidth / 2;

                            var sX = 0f;
                            var eX = 0f;
                            if ((axis.axisTick.inside && axis.IsLeft()) ||
                                (!axis.axisTick.inside && axis.IsRight()))
                            {
                                sX = startX + lineWidth;
                                eX = sX + tickLength;
                            }
                            else
                            {
                                sX = startX - lineWidth;
                                eX = sX - tickLength;
                            }

                            UGL.DrawLine(vh, new Vector3(sX, pY), new Vector3(eX, pY),
                                axis.axisTick.GetWidth(theme.tickWidth),
                                axis.axisTick.GetColor(theme.tickColor));
                        }
                    }
                    current += scaleWidth;
                }
            }
            if (axis.show && axis.axisLine.show && axis.axisLine.showArrow)
            {
                var lineY = startY + axis.offset;
                var inverse = axis.IsValue() && axis.inverse;
                var axisArrow = axis.axisLine.arrow;
                if (orient == Orient.Horizonal)
                {
                    if (inverse)
                    {
                        var startPos = new Vector3(startX + axisLength, lineY);
                        var arrowPos = new Vector3(startX, lineY);
                        UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                            axisArrow.offset, axisArrow.dent,
                            axisArrow.GetColor(axis.axisLine.GetColor(theme.lineColor)));
                    }
                    else
                    {
                        var arrowPosX = startX + axisLength + lineWidth;
                        var startPos = new Vector3(startX, lineY);
                        var arrowPos = new Vector3(arrowPosX, lineY);
                        UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                            axisArrow.offset, axisArrow.dent,
                            axisArrow.GetColor(axis.axisLine.GetColor(theme.lineColor)));
                    }
                }
                else
                {
                    if (inverse)
                    {
                        var startPos = new Vector3(startX, startY + axisLength);
                        var arrowPos = new Vector3(startX, startY);
                        UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                            axisArrow.offset, axisArrow.dent,
                            axisArrow.GetColor(axis.axisLine.GetColor(theme.lineColor)));
                    }
                    else
                    {
                        var startPos = new Vector3(startX, startY);
                        var arrowPos = new Vector3(startX, startY + axisLength + lineWidth);
                        UGL.DrawArrow(vh, startPos, arrowPos, axisArrow.width, axisArrow.height,
                            axisArrow.offset, axisArrow.dent,
                            axisArrow.GetColor(axis.axisLine.GetColor(theme.lineColor)));
                    }
                }
            }
        }

        protected void DrawAxisSplit(VertexHelper vh, AxisTheme theme, DataZoom dataZoom,
            Orient orient, float startX, float startY, float axisLength, float splitLength, Axis relativedAxis = null)
        {
            Axis axis = component;
            var lineColor = axis.splitLine.GetColor(theme.splitLineColor);
            var lineWidth = axis.splitLine.GetWidth(theme.lineWidth);
            var lineType = axis.splitLine.GetType(theme.splitLineType);

            var size = AxisHelper.GetScaleNumber(axis, axisLength, dataZoom);
            if (axis.IsTime())
            {
                size += 1;
                if (!ChartHelper.IsEquals(axis.GetLastLabelValue(), axis.context.maxValue))
                    size += 1;
            }

            var current = orient == Orient.Horizonal ?
                startX :
                startY;
            for (int i = 0; i < size; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(axis, axisLength, axis.IsTime() ? i : i + 1, dataZoom);

                if (axis.boundaryGap && axis.axisTick.alignWithLabel)
                    current -= scaleWidth / 2;

                if (axis.splitArea.show && i <= size - 1)
                {
                    if (orient == Orient.Horizonal)
                    {
                        UGL.DrawQuadrilateral(vh,
                            new Vector2(current, startY),
                            new Vector2(current, startY + splitLength),
                            new Vector2(current + scaleWidth, startY + splitLength),
                            new Vector2(current + scaleWidth, startY),
                            axis.splitArea.GetColor(i, theme));
                    }
                    else
                    {
                        UGL.DrawQuadrilateral(vh,
                            new Vector2(startX, current),
                            new Vector2(startX + splitLength, current),
                            new Vector2(startX + splitLength, current + scaleWidth),
                            new Vector2(startX, current + scaleWidth),
                            axis.splitArea.GetColor(i, theme));
                    }

                }
                if (axis.splitLine.show)
                {
                    if (axis.splitLine.NeedShow(i))
                    {
                        if (orient == Orient.Horizonal)
                        {
                            if (relativedAxis == null || !MathUtil.Approximately(current, GetAxisLineXOrY()))
                                ChartDrawer.DrawLineStyle(vh,
                                    lineType,
                                    lineWidth,
                                    new Vector3(current, startY),
                                    new Vector3(current, startY + splitLength),
                                    lineColor);
                        }
                        else
                        {
                            if (relativedAxis == null || !MathUtil.Approximately(current, GetAxisLineXOrY()))
                                ChartDrawer.DrawLineStyle(vh,
                                    lineType,
                                    lineWidth,
                                    new Vector3(startX, current),
                                    new Vector3(startX + splitLength, current),
                                    lineColor);
                        }
                    }
                }
                current += scaleWidth;
            }
        }
    }
}