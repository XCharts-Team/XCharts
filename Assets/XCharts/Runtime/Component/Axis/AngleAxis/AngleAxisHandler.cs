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
    internal sealed class AngleAxisHandler : MainComponentHandler<AngleAxis>
    {
        public override void InitComponent()
        {
            InitAngleAxis(component);
        }

        public override void Update()
        {
            component.startAngle = 90 - component.startAngle;
            UpdateAxisMinMaxValue(component);
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
            SeriesHelper.GetYMinMaxValue(chart.series, null, axis.polarIndex, true, axis.inverse, out tempMinValue,
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

        internal void UpdateAxisLabelText(AngleAxis axis)
        {
            var runtimeWidth = 360;
            axis.UpdateLabelText(runtimeWidth, null, false, 500);
        }

        private void InitAngleAxis(AngleAxis axis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(axis.polarIndex);
            if (polar == null) return;
            PolarHelper.UpdatePolarCenter(polar, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            var radius = polar.context.radius;
            axis.context.labelObjectList.Clear();

            string objName = "axis_angle" + axis.index;
            var axisObj = ChartHelper.AddObject(objName, chart.transform, chart.chartMinAnchor,
                chart.chartMaxAnchor, chart.chartPivot, chart.chartSizeDelta);
            axisObj.transform.localPosition = Vector3.zero;
            axisObj.SetActive(axis.show);
            axisObj.hideFlags = chart.chartHideFlags;
            ChartHelper.HideAllObject(axisObj);
            var splitNumber = AxisHelper.GetSplitNumber(axis, radius, null);
            var totalAngle = axis.startAngle;
            var total = 360;
            var cenPos = polar.context.center;
            var txtHig = axis.axisLabel.textStyle.GetFontSize(chart.theme.axis) + 2;
            var margin = axis.axisLabel.margin;
            var isCategory = axis.IsCategory();
            var isPercentStack = SeriesHelper.IsPercentStack<Bar>(chart.series);
            for (int i = 0; i < splitNumber; i++)
            {
                float scaleAngle = AxisHelper.GetScaleWidth(axis, total, i, null);
                bool inside = axis.axisLabel.inside;
                var labelName = AxisHelper.GetLabelName(axis, total, i, axis.context.minValue, axis.context.maxValue,
                    null, isPercentStack);
                var label = ChartHelper.AddAxisLabelObject(splitNumber, i, objName + i, axisObj.transform, new Vector2(0.5f, 0.5f),
                    new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(scaleAngle, txtHig), axis,
                    chart.theme.axis, labelName);
                label.label.SetAlignment(axis.axisLabel.textStyle.GetAlignment(TextAnchor.MiddleCenter));
                var pos = ChartHelper.GetPos(cenPos, radius + margin,
                    isCategory ? (totalAngle + scaleAngle / 2) : totalAngle, true);
                AxisHelper.AdjustCircleLabelPos(label, pos, cenPos, txtHig, Vector3.zero);
                if (i == 0) axis.axisLabel.SetRelatedText(label.label, scaleAngle);
                axis.context.labelObjectList.Add(label);

                totalAngle += scaleAngle;
            }
        }

        private void DrawAngleAxis(VertexHelper vh, AngleAxis angleAxis)
        {
            var polar = chart.GetChartComponent<PolarCoord>(angleAxis.polarIndex);
            var radius = polar.context.radius;
            var cenPos = polar.context.center;
            var total = 360;
            var size = AxisHelper.GetScaleNumber(angleAxis, total, null);
            var currAngle = angleAxis.startAngle;
            var tickWidth = angleAxis.axisTick.GetWidth(chart.theme.axis.tickWidth);
            var tickLength = angleAxis.axisTick.GetLength(chart.theme.axis.tickLength);
            for (int i = 0; i < size; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(angleAxis, total, i);
                var pos = ChartHelper.GetPos(cenPos, radius, currAngle, true);
                if (angleAxis.show && angleAxis.splitLine.show)
                {
                    var splitLineColor = angleAxis.splitLine.GetColor(chart.theme.axis.splitLineColor);
                    var lineWidth = angleAxis.splitLine.GetWidth(chart.theme.axis.splitLineWidth);
                    UGL.DrawLine(vh, cenPos, pos, lineWidth, splitLineColor);
                }
                if (angleAxis.show && angleAxis.axisTick.show)
                {
                    var tickY = radius + tickLength;
                    var tickPos = ChartHelper.GetPos(cenPos, tickY, currAngle, true);
                    UGL.DrawLine(vh, pos, tickPos, tickWidth, chart.theme.axis.lineColor);
                }
                currAngle += scaleWidth;
            }
            if (angleAxis.show && angleAxis.axisLine.show)
            {
                var lineWidth = angleAxis.axisLine.GetWidth(chart.theme.axis.lineWidth);
                var outsideRaidus = radius + lineWidth * 2;
                UGL.DrawDoughnut(vh, cenPos, radius, outsideRaidus, chart.theme.axis.lineColor, Color.clear);
            }
        }
    }
}