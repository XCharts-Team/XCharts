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
            this.component = (T)component;
        }

        protected virtual Vector3 GetLabelPosition(float scaleWid, int i)
        {
            return Vector3.zero;
        }

        internal virtual float GetAxisLineXOrY()
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
                        double xValue;
                        if (axis.IsLog())
                        {
                            var logBase = axis.logBase;
                            var minLog = Math.Log(axis.context.minValue, logBase);
                            var maxLog = Math.Log(axis.context.maxValue, logBase);
                            var logRange = maxLog - minLog;
                            var pointerLog = minLog + logRange * (chart.pointerPos.x - grid.context.x - axis.context.offset) / grid.context.width;
                            xValue = Math.Pow(logBase, pointerLog);
                        }
                        else
                        {
                            var xRate = axis.context.minMaxRange / grid.context.width;
                            xValue = xRate * (chart.pointerPos.x - grid.context.x - axis.context.offset);
                            if (axis.context.minValue > 0)
                                xValue += axis.context.minValue;
                        }
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

            double tempMinValue;
            double tempMaxValue;
            axis.context.needAnimation = Application.isPlaying && axis.animation.show;
            chart.GetSeriesMinMaxValue(axis, axisIndex, out tempMinValue, out tempMaxValue);

            var dataZoom = chart.GetDataZoomOfAxis(axis);
            if (dataZoom != null && dataZoom.enable)
            {
                if (axis is XAxis)
                    dataZoom.SetXAxisIndexValueInfo(axisIndex, ref tempMinValue, ref tempMaxValue);
                else
                    dataZoom.SetYAxisIndexValueInfo(axisIndex, ref tempMinValue, ref tempMaxValue);
            }

            if (tempMinValue != axis.context.destMinValue ||
                tempMaxValue != axis.context.destMaxValue ||
                m_LastInterval != axis.interval ||
                m_LastSplitNumber != axis.splitNumber)
            {
                m_LastSplitNumber = axis.splitNumber;
                m_LastInterval = axis.interval;
                axis.UpdateMinMaxValue(tempMinValue, tempMaxValue, axis.context.needAnimation);
                axis.context.offset = 0;
                axis.context.lastCheckInverse = axis.inverse;
                UpdateAxisTickValueList(axis);

                if (tempMinValue != 0 || tempMaxValue != 0)
                {
                    var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
                    if (grid != null && axis is XAxis && axis.IsValue())
                    {
                        axis.UpdateZeroOffset(grid.context.width);
                    }
                    if (grid != null && axis is YAxis && axis.IsValue())
                    {
                        axis.UpdateZeroOffset(grid.context.height);
                    }
                }

                if (updateChart)
                {
                    UpdateAxisLabelText(axis);
                    chart.RefreshChart();
                }
            }

            if (axis.context.needAnimation && (axis.context.minValue != axis.context.destMinValue || axis.context.maxValue != axis.context.destMaxValue))
            {
                var duration = axis.animation.duration == 0
                    ? SeriesHelper.GetMinAnimationDuration(chart.series) / 1000f
                    : axis.animation.duration / 1000f;
                var deltaTime = axis.animation.unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                var minDiff = axis.context.destMinValue - axis.context.lastMinValue;
                var maxDiff = axis.context.destMaxValue - axis.context.lastMaxValue;
                var minDelta = minDiff / duration * deltaTime;
                var maxDelta = maxDiff / duration * deltaTime;
                axis.context.minValue += minDelta;
                axis.context.maxValue += maxDelta;
                if ((minDiff > 0 && axis.context.minValue > axis.context.destMinValue)
                    || (minDiff < 0 && axis.context.minValue < axis.context.destMinValue))
                {
                    axis.context.minValue = axis.context.destMinValue;
                    axis.context.lastMinValue = axis.context.destMinValue;
                }
                if ((maxDiff > 0 && axis.context.maxValue > axis.context.destMaxValue)
                    || (maxDiff < 0 && axis.context.maxValue < axis.context.destMaxValue))
                {
                    axis.context.maxValue = axis.context.destMaxValue;
                    axis.context.lastMaxValue = axis.context.destMaxValue;
                }
                axis.context.minMaxRange = axis.context.maxValue - axis.context.minValue;
                UpdateAxisTickValueList(axis);
                UpdateAxisLabelText(axis);
                chart.RefreshChart();
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
                axis.context.tickValue = DateTimeUtil.UpdateTimeAxisDateTimeList(axis.context.labelValueList,
                    (int)axis.context.minValue, (int)axis.context.maxValue, axis.splitNumber);

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
                        if (range / 4 % each == 0)
                            tick = range / 4;
                        else if (range / tick > 8)
                            tick = 2 * each;
                        else if (range / tick < 4)
                            tick = each / 2;
                    }
                }
                var value = 0d;
                axis.context.tickValue = tick;
                if (Mathf.Approximately((float)(axis.context.minValue % tick), 0))
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
            if (max > 1 && max < 10) return 1;
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
                        label.SetTextActive(axis.IsNeedShowLabel(i) && dist > label.text.GetPreferredWidth());
                    }
                    else if (i == axis.context.labelValueList.Count - 1)
                    {
                        var dist = pos.x - GetLabelPosition(0, i - 1).x;
                        label.SetTextActive(axis.IsNeedShowLabel(i) && dist > label.text.GetPreferredWidth());
                    }
                }
                else
                {
                    if (i == 0)
                    {
                        var dist = GetLabelPosition(0, 1).y - pos.y;
                        label.SetTextActive(axis.IsNeedShowLabel(i) && dist > label.text.GetPreferredHeight());
                    }
                    else if (i == axis.context.labelValueList.Count - 1)
                    {
                        var dist = pos.y - GetLabelPosition(0, i - 1).y;
                        label.SetTextActive(axis.IsNeedShowLabel(i) && dist > label.text.GetPreferredHeight());
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
            axis.context.aligment = defaultAlignment;
            for (int i = 0; i < splitNumber; i++)
            {
                var labelWidth = AxisHelper.GetScaleWidth(axis, axisLength, i + 1, dataZoom);
                var labelName = AxisHelper.GetLabelName(axis, axisLength, i,
                    axis.context.destMinValue,
                    axis.context.destMaxValue,
                    dataZoom, isPercentStack);

                var label = ChartHelper.AddAxisLabelObject(splitNumber, i,
                    ChartCached.GetAxisLabelName(i),
                    axisObj.transform,
                    new Vector2(textWidth, textHeight),
                    axis, chart.theme.axis, labelName,
                    Color.clear,
                    defaultAlignment,
                    chart.theme.GetColor(i));

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
                    var posY = !axis.axisName.onZero && grid != null ? grid.context.y : GetAxisLineXOrY() + offset.y;
                    switch (axis.axisName.labelStyle.position)
                    {
                        case LabelStyle.Position.Start:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleRight);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Top ?
                                new Vector2(zeroPos.x - offset.x, axisStartY + relativedLength + offset.y + axis.offset) :
                                new Vector2(zeroPos.x - offset.x, posY + offset.y));
                            break;

                        case LabelStyle.Position.Middle:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Top ?
                                new Vector2(axisStartX + axisLength / 2 + offset.x, axisStartY + relativedLength - offset.y + axis.offset) :
                                new Vector2(axisStartX + axisLength / 2 + offset.x, posY + offset.y));
                            break;

                        default:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleLeft);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Top ?
                                new Vector2(axisStartX + axisLength + offset.x, axisStartY + relativedLength + offset.y + axis.offset) :
                                new Vector2(axisStartX + axisLength + offset.x, posY + offset.y));
                            break;
                    }
                }
                else
                {
                    var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
                    var posX = !axis.axisName.onZero && grid != null ? grid.context.x : GetAxisLineXOrY() + offset.x;
                    switch (axis.axisName.labelStyle.position)
                    {
                        case LabelStyle.Position.Start:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Right ?
                                new Vector2(axisStartX + relativedLength + offset.x + axis.offset, axisStartY - offset.y) :
                                new Vector2(posX + offset.x, axisStartY - offset.y));
                            break;

                        case LabelStyle.Position.Middle:

                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Right ?
                                new Vector2(axisStartX + relativedLength - offset.x + axis.offset, axisStartY + axisLength / 2 + offset.y) :
                                new Vector2(posX + offset.x, axisStartY + axisLength / 2 + offset.y));
                            break;

                        default:
                            //LabelStyle.Position
                            label = ChartHelper.AddChartLabel(s_DefaultAxisName, axisObj.transform, axis.axisName.labelStyle,
                                chart.theme.axis, axis.axisName.name, autoColor, TextAnchor.MiddleCenter);
                            label.SetActive(axis.axisName.labelStyle.show);
                            label.SetPosition(axis.position == Axis.AxisPosition.Right ?
                                new Vector2(axisStartX + relativedLength + offset.x + axis.offset, axisStartY + axisLength + offset.y) :
                                new Vector2(posX + offset.x, axisStartY + axisLength + offset.y));
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
                if (axis.IsTime())
                {
                    size += 1;
                    if (!ChartHelper.IsEquals(axis.GetLastLabelValue(), axis.context.maxValue))
                        size += 1;
                }
                var tickWidth = axis.axisTick.GetWidth(theme.tickWidth);
                var tickColor = axis.axisTick.GetColor(theme.tickColor);
                var current = orient == Orient.Horizonal ? startX : startY;
                var maxAxisXY = current + axisLength;
                var lastTickX = current;
                var lastTickY = current;
                var minorTickSplitNumber = axis.minorTick.splitNumber <= 0 ? 5 : axis.minorTick.splitNumber;
                var minorTickDistance = axis.GetValueLength(axis.context.tickValue / minorTickSplitNumber, axisLength);
                var minorTickColor = axis.minorTick.GetColor(theme.tickColor);
                var minorTickWidth = axis.minorTick.GetWidth(theme.tickWidth);
                var minorTickLength = axis.minorTick.GetLength(theme.tickLength * 0.6f);
                var minorStartIndex = axis.IsTime() ? 0 : 1;
                var isLogAxis = axis.IsLog();
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(axis, axisLength, i + 1, dataZoom);
                    var hideTick = (i == 0 && (!axis.axisTick.showStartTick || axis.axisTick.alignWithLabel)) ||
                        (i == size - 1 && !axis.axisTick.showEndTick);
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
                            var mY = 0f;
                            if ((axis.axisTick.inside && axis.IsBottom()) ||
                                (!axis.axisTick.inside && axis.IsTop()))
                            {
                                sY = startY + lineWidth;
                                eY = sY + tickLength;
                                mY = sY + minorTickLength;
                            }
                            else
                            {
                                sY = startY - lineWidth;
                                eY = sY - tickLength;
                                mY = sY - minorTickLength;
                            }
                            if (!hideTick)
                                UGL.DrawLine(vh, new Vector3(pX, sY), new Vector3(pX, eY), tickWidth, tickColor);
                            if (axis.minorTick.show && i >= minorStartIndex && (minorTickDistance > 0 || isLogAxis))
                            {
                                if (isLogAxis)
                                {
                                    var count = 0;
                                    var logRange = (axis.logBase - 1f);
                                    minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                    var tickTotal = lastTickX + minorTickDistance;
                                    while (tickTotal < current && count < minorTickSplitNumber - 1)
                                    {
                                        UGL.DrawLine(vh, new Vector3(tickTotal, sY), new Vector3(tickTotal, mY), minorTickWidth, minorTickColor);
                                        count++;
                                        minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                        tickTotal = lastTickX + minorTickDistance;
                                    }
                                }
                                else if (lastTickX <= axis.context.zeroX || (i == minorStartIndex && pX > axis.context.zeroX))
                                {
                                    var tickTotal = pX - minorTickDistance;
                                    while (tickTotal > lastTickX)
                                    {
                                        UGL.DrawLine(vh, new Vector3(tickTotal, sY), new Vector3(tickTotal, mY), minorTickWidth, minorTickColor);
                                        tickTotal -= minorTickDistance;
                                    }
                                }
                                else
                                {
                                    var tickTotal = lastTickX + minorTickDistance;
                                    while (tickTotal < pX)
                                    {
                                        UGL.DrawLine(vh, new Vector3(tickTotal, sY), new Vector3(tickTotal, mY), minorTickWidth, minorTickColor);
                                        tickTotal += minorTickDistance;
                                    }
                                }
                                if (i == size - 1)
                                {
                                    var tickTotal = pX + minorTickDistance;
                                    while (tickTotal < maxAxisXY)
                                    {
                                        UGL.DrawLine(vh, new Vector3(tickTotal, sY), new Vector3(tickTotal, mY), minorTickWidth, minorTickColor);
                                        tickTotal += minorTickDistance;
                                    }
                                }
                            }
                            lastTickX = pX;
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
                            var mX = 0f;
                            if ((axis.axisTick.inside && axis.IsLeft()) ||
                                (!axis.axisTick.inside && axis.IsRight()))
                            {
                                sX = startX + lineWidth;
                                eX = sX + tickLength;
                                mX = sX + minorTickLength;
                            }
                            else
                            {
                                sX = startX - lineWidth;
                                eX = sX - tickLength;
                                mX = sX - minorTickLength;
                            }
                            if (!hideTick)
                                UGL.DrawLine(vh, new Vector3(sX, pY), new Vector3(eX, pY), tickWidth, tickColor);
                            if (axis.minorTick.show && i >= minorStartIndex && (minorTickDistance > 0 || isLogAxis))
                            {
                                if (isLogAxis)
                                {
                                    var count = 0;
                                    var logRange = (axis.logBase - 1f);
                                    minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                    var tickTotal = lastTickY + minorTickDistance;
                                    while (tickTotal < current && count < minorTickSplitNumber - 1)
                                    {
                                        UGL.DrawLine(vh, new Vector3(sX, tickTotal), new Vector3(mX, tickTotal), minorTickWidth, minorTickColor);
                                        count++;
                                        minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                        tickTotal = lastTickY + minorTickDistance;
                                    }
                                }
                                else if (lastTickY <= axis.context.zeroY || (i == minorStartIndex && pY > axis.context.zeroY))
                                {
                                    var tickTotal = pY - minorTickDistance;
                                    while (tickTotal > lastTickY)
                                    {

                                        UGL.DrawLine(vh, new Vector3(sX, tickTotal), new Vector3(mX, tickTotal), minorTickWidth, minorTickColor);
                                        tickTotal -= minorTickDistance;
                                    }
                                }
                                else
                                {
                                    var tickTotal = lastTickY + minorTickDistance;
                                    while (tickTotal < pY)
                                    {

                                        UGL.DrawLine(vh, new Vector3(sX, tickTotal), new Vector3(mX, tickTotal), minorTickWidth, minorTickColor);
                                        tickTotal += minorTickDistance;
                                    }
                                }
                                if (i == size - 1)
                                {
                                    var tickTotal = pY + minorTickDistance;
                                    while (tickTotal < maxAxisXY)
                                    {
                                        UGL.DrawLine(vh, new Vector3(sX, tickTotal), new Vector3(mX, tickTotal), minorTickWidth, minorTickColor);
                                        tickTotal += minorTickDistance;
                                    }
                                }
                            }
                            lastTickY = pY;
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
            Orient orient, float startX, float startY, float axisLength, float splitLength,
            Axis relativedAxis = null)
        {
            Axis axis = component;
            var axisLineWidth = axis.axisLine.GetWidth(theme.lineWidth);
            splitLength -= axisLineWidth;
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

            var current = orient == Orient.Horizonal ? startX : startY;
            var maxAxisXY = current + axisLength;
            var lastSplitX = 0f;
            var lastSplitY = 0f;
            var minorTickSplitNumber = axis.minorTick.splitNumber <= 0 ? 5 : axis.minorTick.splitNumber;
            var minorTickDistance = axis.GetValueLength(axis.context.tickValue / minorTickSplitNumber, axisLength);
            var minorSplitLineColor = axis.minorSplitLine.GetColor(theme.minorSplitLineColor);
            var minorLineWidth = axis.minorSplitLine.GetWidth(theme.lineWidth);
            var minorLineType = axis.minorSplitLine.GetType(theme.splitLineType);
            var minorStartIndex = axis.IsTime() ? 0 : 1;
            var isLogAxis = axis.IsLog();
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
                    if (axis.splitLine.NeedShow(i, size))
                    {
                        if (orient == Orient.Horizonal)
                        {
                            if (relativedAxis == null || !relativedAxis.axisLine.show || !MathUtil.Approximately(current, relativedAxis.context.x))
                            {
                                ChartDrawer.DrawLineStyle(vh,
                                    lineType,
                                    lineWidth,
                                    new Vector3(current, startY),
                                    new Vector3(current, startY + splitLength),
                                    lineColor);
                            }
                            if (axis.minorSplitLine.show && i >= minorStartIndex && (minorTickDistance > 0 || isLogAxis))
                            {
                                if (isLogAxis)
                                {
                                    var count = 0;
                                    var logRange = axis.logBase - 1f;
                                    minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                    var tickTotal = lastSplitX + minorTickDistance;
                                    while (tickTotal < current && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(tickTotal, startY),
                                            new Vector3(tickTotal, startY + splitLength),
                                            minorSplitLineColor);
                                        count++;
                                        minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                        tickTotal = lastSplitX + minorTickDistance;
                                    }
                                }
                                else if (lastSplitX <= axis.context.zeroX || (i == minorStartIndex && current > axis.context.zeroX))
                                {
                                    var tickTotal = current - minorTickDistance;
                                    var count = 0;
                                    while (tickTotal > lastSplitX && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(tickTotal, startY),
                                            new Vector3(tickTotal, startY + splitLength),
                                            minorSplitLineColor);
                                        count++;
                                        tickTotal -= minorTickDistance;
                                    }
                                }
                                else
                                {
                                    var tickTotal = lastSplitX + minorTickDistance;
                                    var count = 0;
                                    while (tickTotal < current && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(tickTotal, startY),
                                            new Vector3(tickTotal, startY + splitLength),
                                            minorSplitLineColor);
                                        count++;
                                        tickTotal += minorTickDistance;
                                    }
                                }
                                if (i == size - 1)
                                {
                                    var tickTotal = current + minorTickDistance;
                                    var count = 0;
                                    while (tickTotal < maxAxisXY && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(tickTotal, startY),
                                            new Vector3(tickTotal, startY + splitLength),
                                            minorSplitLineColor);
                                        count++;
                                        tickTotal += minorTickDistance;
                                    }
                                }
                            }
                            lastSplitX = current;
                        }
                        else
                        {
                            if (relativedAxis == null || !relativedAxis.axisLine.show || !MathUtil.Approximately(current, relativedAxis.context.y))
                            {
                                ChartDrawer.DrawLineStyle(vh,
                                    lineType,
                                    lineWidth,
                                    new Vector3(startX, current),
                                    new Vector3(startX + splitLength, current),
                                    lineColor);
                            }
                            if (axis.minorSplitLine.show && i >= minorStartIndex && (minorTickDistance > 0 || isLogAxis))
                            {
                                if (isLogAxis)
                                {
                                    var count = 0;
                                    var logRange = (axis.logBase - 1f);
                                    minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                    var tickTotal = lastSplitY + minorTickDistance;
                                    while (tickTotal < current && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(startX, tickTotal),
                                            new Vector3(startX + splitLength, tickTotal),
                                            minorSplitLineColor);
                                        count++;
                                        minorTickDistance = scaleWidth * axis.GetLogValue(1 + (count + 1) * logRange / minorTickSplitNumber);
                                        tickTotal = lastSplitY + minorTickDistance;
                                    }
                                }
                                else if (lastSplitY <= axis.context.zeroY || (i == minorStartIndex && current > axis.context.zeroY))
                                {
                                    var tickTotal = current - minorTickDistance;
                                    var count = 0;
                                    while (tickTotal > lastSplitY && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(startX, tickTotal),
                                            new Vector3(startX + splitLength, tickTotal),
                                            minorSplitLineColor);
                                        count++;
                                        tickTotal -= minorTickDistance;
                                    }
                                }
                                else
                                {
                                    var tickTotal = lastSplitY + minorTickDistance;
                                    var count = 0;
                                    while (tickTotal < current && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(startX, tickTotal),
                                            new Vector3(startX + splitLength, tickTotal),
                                            minorSplitLineColor);
                                        count++;
                                        tickTotal += minorTickDistance;
                                    }
                                }
                                if (i == size - 1)
                                {
                                    var tickTotal = current + minorTickDistance;
                                    var count = 0;
                                    while (tickTotal < maxAxisXY && count < minorTickSplitNumber - 1)
                                    {
                                        ChartDrawer.DrawLineStyle(vh,
                                            minorLineType,
                                            minorLineWidth,
                                            new Vector3(startX, tickTotal),
                                            new Vector3(startX + splitLength, tickTotal),
                                            minorSplitLineColor);
                                        count++;
                                        tickTotal += minorTickDistance;
                                    }
                                }
                            }
                            lastSplitY = current;
                        }
                    }
                }
                current += scaleWidth;
            }
        }
    }
}