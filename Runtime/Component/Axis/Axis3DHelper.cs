using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    public static class Axis3DHelper
    {
        public static Vector3 Get3DGridPosition(GridCoord3D grid, XAxis3D xAxis, YAxis3D yAxis, ZAxis3D zAxis, double xValue, double yValue, double zValue)
        {
            var x = xAxis.GetDistance(xValue);
            var y = yAxis.GetDistance(yValue);
            var z = zAxis.GetDistance(zValue);

            var dest = grid.context.pointA;
            dest += xAxis.context.dire * x;
            dest += yAxis.context.dire * y;
            dest += zAxis.context.dire * z;
            return dest;
        }

        public static Vector3 Get3DGridPosition(GridCoord3D grid, XAxis3D xAxis, YAxis3D yAxis, double xValue, double yValue)
        {
            var x = xAxis.GetDistance(xValue);
            var y = yAxis.GetDistance(yValue);

            var dest = grid.context.pointA;
            dest += xAxis.context.dire * x;
            dest += yAxis.context.dire * y;
            return dest;
        }

        internal static void DrawAxisTick(VertexHelper vh, Axis axis, AxisTheme theme, DataZoom dataZoom,
         Vector3 start, Vector3 end, Vector3 relativedDire)
        {
            var tickLength = axis.axisTick.GetLength(theme.tickLength);
            var axisLength = Vector3.Distance(start, end);
            var axisDire = (end - start).normalized;

            if (axis.position == Axis.AxisPosition.Right)
            {
                relativedDire = -relativedDire;
            }

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
                var current = start;
                for (int i = 0; i < size; i++)
                {
                    var scaleWidth = AxisHelper.GetScaleWidth(axis, axisLength, i + 1, dataZoom);
                    var hideTick = (i == 0 && (!axis.axisTick.showStartTick || axis.axisTick.alignWithLabel)) ||
                        (i == size - 1 && !axis.axisTick.showEndTick);
                    if (axis.axisTick.show && !hideTick)
                    {
                        UGL.DrawLine(vh, current, current + relativedDire * tickLength, tickWidth, tickColor);
                    }
                    current += axisDire * scaleWidth;
                }
            }
            if (axis.show && axis.axisLine.show && axis.axisLine.showArrow)
            {

            }
        }

        public static void DrawAxisSplit(VertexHelper vh, Axis axis, AxisTheme theme, DataZoom dataZoom,
            Vector3 start, Vector3 end, Axis relativedAxis)
        {
            if (relativedAxis == null) return;
            var axisLength = Vector3.Distance(start, end);
            var axisDire = (end - start).normalized;
            var splitLength = relativedAxis.context.length;
            var relativeDire = relativedAxis.context.dire;
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

            var current = start;
            for (int i = 0; i < size; i++)
            {
                var scaleWidth = AxisHelper.GetScaleWidth(axis, axisLength, axis.IsTime() ? i : i + 1, dataZoom);
                if (axis.boundaryGap && axis.axisTick.alignWithLabel)
                    current -= axisDire * scaleWidth / 2;

                if (axis.splitArea.show && i <= size - 1)
                {
                    var p1 = current;
                    var p2 = current + relativeDire * splitLength;
                    var p3 = p2 + axisDire * scaleWidth;
                    var p4 = p1 + axisDire * scaleWidth;
                    UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, axis.splitArea.GetColor(i, theme));
                }
                if (axis.splitLine.show)
                {
                    if (axis.splitLine.NeedShow(i, size))
                    {
                        if (relativedAxis == null || !relativedAxis.axisLine.show 
                            || (Vector3.Distance(current, relativedAxis.context.start) > 0.5f && Vector3.Distance(current, relativedAxis.context.end) > 0.5f))
                        {
                            ChartDrawer.DrawLineStyle(vh,
                                lineType,
                                lineWidth,
                                current,
                                current + relativeDire * splitLength,
                                lineColor);
                        }
                    }
                }
                current += axisDire * scaleWidth;
            }
        }

        public static Vector3 GetLabelPosition(int i, Axis axis, Axis relativedAxis, AxisTheme theme, float scaleWid)
        {
            var axisStart = axis.context.start;
            var axisEnd = axis.context.end;
            var axisDire = axis.context.dire;
            var relativedDire = relativedAxis != null ? relativedAxis.context.dire : Vector3.zero;
            var axisLength = Vector3.Distance(axisStart, axisEnd);
            var inside = axis.axisLabel.inside;
            var fontSize = axis.axisLabel.textStyle.GetFontSize(theme);
            var current = axis.offset;

            if (axis.position == Axis.AxisPosition.Right)
            {
                relativedDire = -relativedDire;
            }

            if (axis.IsTime() || axis.IsValue())
            {
                scaleWid = axis.context.minMaxRange != 0 ?
                    axis.GetDistance(axis.GetLabelValue(i), axisLength) :
                    0;
            }

            return axisStart + axisDire * scaleWid + axis.axisLabel.offset - relativedDire * (axis.axisLabel.distance + fontSize / 2);
        }


    }
}