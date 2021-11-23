/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class RadiusAxisHandler : MainComponentHandler<RadiusAxis>
    {
        public override void InitComponent()
        {
            InitRadiusAxis(component);
        }

        public override void Update()
        {
            UpdateAxisMinMaxValue(component);
        }

        public override void DrawBase(VertexHelper vh)
        {
            DrawRadiusAxis(vh, component);
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
                chart.m_IsPlayingAnimation = true;
                var needCheck = !chart.m_IsPlayingAnimation && axis.context.lastCheckInverse == axis.inverse;
                axis.UpdateMinMaxValue(tempMinValue, tempMaxValue, needCheck);
                axis.context.xOffset = 0;
                axis.context.yOffset = 0;
                axis.context.lastCheckInverse = axis.inverse;

                if (updateChart)
                {
                    UpdateAxisLabelText(axis);
                    chart.RefreshChart();
                }
            }
            if (axis.IsValueChanging(500) && !chart.m_IsPlayingAnimation)
            {
                UpdateAxisLabelText(axis);
                chart.RefreshChart();
            }
        }

        internal void UpdateAxisLabelText(RadiusAxis axis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(axis.polarIndex);
            axis.UpdateLabelText(polar.context.radius, null, false, 500);
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
            var objName = "axis_radius" + axis.index;
            var axisObj = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show && axis.axisLabel.show);
            axisObj.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var textStyle = axis.axisLabel.textStyle;
            var splitNumber = AxisHelper.GetSplitNumber(axis, radius, null);
            var totalWidth = 0f;
            var startAngle = angleAxis.startAngle;
            var cenPos = polar.context.center;
            var txtHig = textStyle.GetFontSize(chart.theme.axis) + 2;
            var dire = ChartHelper.GetDire(startAngle, true).normalized;
            var tickWidth = axis.axisTick.GetLength(chart.theme.axis.tickWidth);
            var tickVector = ChartHelper.GetVertialDire(dire)
                * (tickWidth + axis.axisLabel.margin);
            for (int i = 0; i <= splitNumber; i++)
            {
                var labelWidth = AxisHelper.GetScaleWidth(axis, radius, i, null);
                var inside = axis.axisLabel.inside;
                var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series);
                var labelName = AxisHelper.GetLabelName(axis, radius, i, axis.context.minValue, axis.context.maxValue,
                    null, isPercentStack);
                var label = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(labelWidth, txtHig), axis, chart.theme.axis,
                    labelName);

                if (i == 0)
                    axis.axisLabel.SetRelatedText(label.label, labelWidth);

                label.label.SetAlignment(textStyle.GetAlignment(TextAnchor.MiddleCenter));
                label.SetText(labelName);
                label.SetPosition(ChartHelper.GetPos(cenPos, totalWidth, startAngle, true) + tickVector);
                label.SetActive(true);

                axis.context.labelObjectList.Add(label);

                totalWidth += labelWidth;
            }
        }

        private void DrawRadiusAxis(VertexHelper vh, RadiusAxis radiusAxis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(radiusAxis.polarIndex);
            if (polar == null)
                return;

            var angleAxis = ComponentHelper.GetAngleAxis(chart.components, polar.index);
            if (angleAxis == null)
                return;

            var startAngle = angleAxis.startAngle;
            var radius = polar.context.radius;
            var cenPos = polar.context.center;
            var size = AxisHelper.GetScaleNumber(radiusAxis, radius, null);
            var totalWidth = 0f;
            var dire = ChartHelper.GetDire(startAngle, true).normalized;
            var tickWidth = radiusAxis.axisTick.GetLength(chart.theme.axis.tickWidth);
            var tickLength = radiusAxis.axisTick.GetLength(chart.theme.axis.tickLength);
            var tickVetor = ChartHelper.GetVertialDire(dire) * tickLength;
            for (int i = 0; i < size - 1; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(radiusAxis, radius, i);
                var pos = ChartHelper.GetPos(cenPos, totalWidth, startAngle, true);
                if (radiusAxis.show && radiusAxis.splitLine.show)
                {
                    var outsideRaidus = totalWidth + radiusAxis.splitLine.GetWidth(chart.theme.axis.splitLineWidth) * 2;
                    var splitLineColor = radiusAxis.splitLine.GetColor(chart.theme.axis.splitLineColor);
                    UGL.DrawDoughnut(vh, cenPos, totalWidth, outsideRaidus, splitLineColor, Color.clear);
                }
                if (radiusAxis.show && radiusAxis.axisTick.show)
                {
                    UGL.DrawLine(vh, pos, pos + tickVetor, tickWidth, chart.theme.axis.lineColor);
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