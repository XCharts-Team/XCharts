using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class RadiusAxisHandler : AxisHandler<RadiusAxis>
    {
        public override void InitComponent()
        {
            InitRadiusAxis(component);
        }

        public override void Update()
        {
            UpdateAxisMinMaxValue(component);
            UpdatePointerValue(component);
        }

        public override void DrawBase(VertexHelper vh)
        {
            DrawRadiusAxis(vh, component);
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

            var angleAxis = ComponentHelper.GetAngleAxis(chart.components, polar.index);
            if (angleAxis == null)
                return;

            var dist = Vector3.Distance(chart.pointerPos, polar.context.center);
            axis.context.pointerValue = axis.context.minValue + (dist / polar.context.radius) * axis.context.minMaxRange;
            axis.context.pointerLabelPosition = GetLabelPosition(polar, axis, angleAxis.context.startAngle, dist);
        }

        private void UpdateAxisMinMaxValue(RadiusAxis axis, bool updateChart = true)
        {
            if (axis.IsCategory() || !axis.show) return;
            double tempMinValue = 0;
            double tempMaxValue = 0;
            SeriesHelper.GetXMinMaxValue(chart.series, null, axis.polarIndex, true, axis.inverse, out tempMinValue,
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

        internal void UpdateAxisLabelText(RadiusAxis axis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(axis.polarIndex);
            if (axis.context.labelObjectList.Count <= 0)
                InitRadiusAxis(axis);
            else
                axis.UpdateLabelText(polar.context.radius, null, false);
        }

        private void InitRadiusAxis(RadiusAxis axis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(axis.index);
            if (polar == null)
                return;

            var angleAxis = ComponentHelper.GetAngleAxis(chart.components, polar.index);
            if (angleAxis == null)
                return;

            PolarHelper.UpdatePolarCenter(polar, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            axis.context.labelObjectList.Clear();
            var radius = polar.context.radius;
            var objName = component.GetType().Name + axis.index;
            var axisObj = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show && axis.axisLabel.show);
            axisObj.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var textStyle = axis.axisLabel.textStyle;
            var splitNumber = AxisHelper.GetScaleNumber(axis, radius, null);
            var totalWidth = 0f;
            var txtHig = textStyle.GetFontSize(chart.theme.axis) + 2;
            for (int i = 0; i < splitNumber; i++)
            {
                var labelWidth = AxisHelper.GetScaleWidth(axis, radius, i + 1, null);
                var inside = axis.axisLabel.inside;
                var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series);
                var labelName = AxisHelper.GetLabelName(axis, radius, i, axis.context.minValue, axis.context.maxValue,
                    null, isPercentStack);
                var label = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform,
                    new Vector2(labelWidth, txtHig), axis, chart.theme.axis, labelName, Color.clear);

                if (i == 0)
                    axis.axisLabel.SetRelatedText(label.text, labelWidth);

                label.text.SetAlignment(textStyle.GetAlignment(TextAnchor.MiddleCenter));
                label.SetText(labelName);
                label.SetPosition(GetLabelPosition(polar, axis, angleAxis.context.startAngle, totalWidth));
                label.SetActive(true);
                label.SetTextActive(true);

                axis.context.labelObjectList.Add(label);

                totalWidth += labelWidth;
            }
        }

        private Vector3 GetLabelPosition(PolarCoord polar, Axis axis, float startAngle, float totalWidth)
        {
            var cenPos = polar.context.center;
            var dire = ChartHelper.GetDire(startAngle, true).normalized;
            var tickLength = axis.axisTick.GetLength(chart.theme.axis.tickLength);
            var tickVector = ChartHelper.GetVertialDire(dire) *
                (tickLength + axis.axisLabel.distance);
            return ChartHelper.GetPos(cenPos, totalWidth, startAngle, true) + tickVector;
        }

        private void DrawRadiusAxis(VertexHelper vh, RadiusAxis radiusAxis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(radiusAxis.polarIndex);
            if (polar == null)
                return;

            var angleAxis = ComponentHelper.GetAngleAxis(chart.components, polar.index);
            if (angleAxis == null)
                return;

            var startAngle = angleAxis.context.startAngle;
            var radius = polar.context.radius;
            var cenPos = polar.context.center;
            var size = AxisHelper.GetScaleNumber(radiusAxis, radius, null);
            var totalWidth = 0f;
            var dire = ChartHelper.GetDire(startAngle, true).normalized;
            var tickWidth = radiusAxis.axisTick.GetWidth(chart.theme.axis.tickWidth);
            var tickLength = radiusAxis.axisTick.GetLength(chart.theme.axis.tickLength);
            var tickVetor = ChartHelper.GetVertialDire(dire) * tickLength;
            for (int i = 0; i <= size; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(radiusAxis, radius, i);
                var pos = ChartHelper.GetPos(cenPos, totalWidth + tickWidth, startAngle, true);
                if (radiusAxis.show && radiusAxis.splitLine.show)
                {
                    var outsideRaidus = totalWidth + radiusAxis.splitLine.GetWidth(chart.theme.axis.splitLineWidth) * 2;
                    var splitLineColor = radiusAxis.splitLine.GetColor(chart.theme.axis.splitLineColor);
                    UGL.DrawDoughnut(vh, cenPos, totalWidth, outsideRaidus, splitLineColor, Color.clear);
                }
                if (radiusAxis.show && radiusAxis.axisTick.show)
                {
                    if ((i == 0 && radiusAxis.axisTick.showStartTick) ||
                        (i == size && radiusAxis.axisTick.showEndTick) ||
                        (i > 0 && i < size))
                    {
                        UGL.DrawLine(vh, pos, pos + tickVetor, tickWidth, chart.theme.axis.lineColor);
                    }
                }
                totalWidth += scaleWidth;
            }
            if (radiusAxis.show && radiusAxis.axisLine.show)
            {
                var lineStartPos = polar.context.center - dire * tickWidth;
                var lineEndPos = polar.context.center + dire * (radius + tickWidth);
                var lineWidth = radiusAxis.axisLine.GetWidth(chart.theme.axis.lineWidth);
                UGL.DrawLine(vh, lineStartPos, lineEndPos, lineWidth, chart.theme.axis.lineColor);
            }
        }
    }
}