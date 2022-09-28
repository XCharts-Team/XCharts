using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class AngleAxisHandler : AxisHandler<AngleAxis>
    {
        public override void InitComponent()
        {
            InitAngleAxis(component);
        }

        public override void Update()
        {
            component.context.startAngle = 90 - component.startAngle;
            UpdateAxisMinMaxValue(component);
            UpdatePointerValue(component);
        }

        public override void DrawBase(VertexHelper vh)
        {
            DrawAngleAxis(vh, component);
        }

        private void UpdateAxisMinMaxValue(AngleAxis axis, bool updateChart = true)
        {
            if (axis.IsCategory() || !axis.show) return;
            double tempMinValue = 0;
            double tempMaxValue = 0;
            SeriesHelper.GetYMinMaxValue(chart, axis.polarIndex, true, axis.inverse, out tempMinValue,
                out tempMaxValue, true);
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
            if (tempMinValue != axis.context.minValue || tempMaxValue != axis.context.maxValue)
            {
                axis.UpdateMinMaxValue(tempMinValue, tempMaxValue);
                axis.context.offset = 0;
                axis.context.lastCheckInverse = axis.inverse;
                UpdateAxisTickValueList(axis);

                if (updateChart)
                {
                    UpdateAxisLabelText(axis);
                    chart.RefreshChart();
                }
            }
        }

        internal void UpdateAxisLabelText(AngleAxis axis)
        {
            var runtimeWidth = 360;
            if (axis.context.labelObjectList.Count <= 0)
                InitAngleAxis(axis);
            else
                axis.UpdateLabelText(runtimeWidth, null, false);
        }

        private void InitAngleAxis(AngleAxis axis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(axis.polarIndex);
            if (polar == null) return;
            PolarHelper.UpdatePolarCenter(polar, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            var radius = polar.context.outsideRadius;
            axis.context.labelObjectList.Clear();
            axis.context.startAngle = 90 - axis.startAngle;

            string objName = component.GetType().Name + axis.index;
            var axisObj = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show);
            axisObj.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var splitNumber = AxisHelper.GetSplitNumber(axis, radius, null);
            var totalAngle = axis.context.startAngle;
            var total = 360;
            var cenPos = polar.context.center;
            var txtHig = axis.axisLabel.textStyle.GetFontSize(chart.theme.axis) + 2;
            var margin = axis.axisLabel.distance + axis.axisTick.GetLength(chart.theme.axis.tickLength);
            var isCategory = axis.IsCategory();
            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series);
            for (int i = 0; i < splitNumber; i++)
            {
                float scaleAngle = AxisHelper.GetScaleWidth(axis, total, i + 1, null);
                bool inside = axis.axisLabel.inside;
                var labelName = AxisHelper.GetLabelName(axis, total, i, axis.context.minValue, axis.context.maxValue,
                    null, isPercentStack);
                var label = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform,
                    new Vector2(scaleAngle, txtHig), axis,
                    chart.theme.axis, labelName, Color.clear);
                label.text.SetAlignment(axis.axisLabel.textStyle.GetAlignment(TextAnchor.MiddleCenter));
                var pos = ChartHelper.GetPos(cenPos, radius + margin,
                    isCategory ? (totalAngle + scaleAngle / 2) : totalAngle, true);
                AxisHelper.AdjustCircleLabelPos(label, pos, cenPos, txtHig, Vector3.zero);
                if (i == 0) axis.axisLabel.SetRelatedText(label.text, scaleAngle);
                axis.context.labelObjectList.Add(label);

                totalAngle += scaleAngle;
            }
        }

        private void DrawAngleAxis(VertexHelper vh, AngleAxis angleAxis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(angleAxis.polarIndex);
            var radius = polar.context.outsideRadius;
            var cenPos = polar.context.center;
            var total = 360;
            var size = AxisHelper.GetScaleNumber(angleAxis, total, null);
            var currAngle = angleAxis.context.startAngle;
            var tickWidth = angleAxis.axisTick.GetWidth(chart.theme.axis.tickWidth);
            var tickLength = angleAxis.axisTick.GetLength(chart.theme.axis.tickLength);
            var tickColor = angleAxis.axisTick.GetColor(chart.theme.axis.lineColor);
            var lineColor = angleAxis.axisLine.GetColor(chart.theme.axis.lineColor);
            var splitLineColor = angleAxis.splitLine.GetColor(chart.theme.axis.splitLineColor);
            for (int i = 1; i < size; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(angleAxis, total, i);
                var pos1 = ChartHelper.GetPos(cenPos, polar.context.insideRadius, currAngle, true);
                var pos2 = ChartHelper.GetPos(cenPos, polar.context.outsideRadius, currAngle, true);
                if (angleAxis.show && angleAxis.splitLine.show)
                {
                    if (angleAxis.splitLine.NeedShow(i - 1, size - 1))
                    {
                        var lineWidth = angleAxis.splitLine.GetWidth(chart.theme.axis.splitLineWidth);
                        UGL.DrawLine(vh, pos1, pos2, lineWidth, splitLineColor);
                    }
                }
                if (angleAxis.show && angleAxis.axisTick.show)
                {
                    if ((i == 1 && angleAxis.axisTick.showStartTick) ||
                        (i == size - 1 && angleAxis.axisTick.showEndTick) ||
                        (i > 1 && i < size - 1))
                    {
                        var tickY = radius + tickLength;
                        var tickPos = ChartHelper.GetPos(cenPos, tickY, currAngle, true);
                        UGL.DrawLine(vh, pos2, tickPos, tickWidth, tickColor);
                    }
                }
                currAngle += scaleWidth;
            }
            if (angleAxis.show && angleAxis.axisLine.show)
            {
                var lineWidth = angleAxis.axisLine.GetWidth(chart.theme.axis.lineWidth);
                var outsideRaidus = radius + lineWidth * 2;
                UGL.DrawDoughnut(vh, cenPos, radius, outsideRaidus, lineColor, ColorUtil.clearColor32);
                if (polar.context.insideRadius > 0)
                {
                    radius = polar.context.insideRadius;
                    outsideRaidus = radius + lineWidth * 2;
                    UGL.DrawDoughnut(vh, cenPos, radius, outsideRaidus, lineColor, ColorUtil.clearColor32);
                }
            }
        }

        protected override void UpdatePointerValue(Axis axis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(axis.polarIndex);
            if (polar == null)
                return;

            if (!polar.context.isPointerEnter)
            {
                axis.context.pointerValue = double.PositiveInfinity;
                return;
            }

            var dir = (chart.pointerPos - new Vector2(polar.context.center.x, polar.context.center.y)).normalized;
            var angle = ChartHelper.GetAngle360(Vector2.up, dir);
            axis.context.pointerValue = (angle - component.context.startAngle + 360) % 360;
            axis.context.pointerLabelPosition = polar.context.center + new Vector3(dir.x, dir.y) * (polar.context.outsideRadius + 25);
        }
    }
}