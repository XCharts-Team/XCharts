using System;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class VisualMapHelper
    {
        public static void AutoSetLineMinMax(VisualMap visualMap, Serie serie, bool isY, Axis axis, Axis relativedAxis)
        {
            if (!IsNeedGradient(visualMap) || !visualMap.autoMinMax)
                return;

            double min = 0;
            double max = 0;
            var xAxis = isY ? relativedAxis : axis;
            var yAxis = isY ? axis : relativedAxis;
            if (visualMap.dimension == 0)
            {
                min = xAxis.IsCategory() ? 0 : xAxis.context.minValue;
                max = xAxis.IsCategory() ? serie.dataCount - 1 : xAxis.context.maxValue;
                SetMinMax(visualMap, min, max);
            }
            else
            {
                min = yAxis.IsCategory() ? 0 : yAxis.context.minValue;
                max = yAxis.IsCategory() ? serie.dataCount - 1 : yAxis.context.maxValue;
                SetMinMax(visualMap, min, max);
            }
        }

        public static void SetMinMax(VisualMap visualMap, double min, double max)
        {
            if ((visualMap.min != min || visualMap.max != max))
            {
                if (max >= min)
                {
                    visualMap.min = min;
                    visualMap.max = max;
                }
                else
                {
                    throw new Exception("SetMinMax:max < min:" + min + "," + max);
                }
            }
        }

        public static void GetLineGradientColor(VisualMap visualMap, float xValue, float yValue,
            out Color32 startColor, out Color32 toColor)
        {
            startColor = ChartConst.clearColor32;
            toColor = ChartConst.clearColor32;
            if (visualMap.dimension == 0)
            {
                startColor = visualMap.IsPiecewise() ? visualMap.GetColor(xValue) : visualMap.GetColor(xValue - 1);
                toColor = visualMap.IsPiecewise() ? startColor : visualMap.GetColor(xValue);
            }
            else
            {
                startColor = visualMap.IsPiecewise() ? visualMap.GetColor(yValue) : visualMap.GetColor(yValue - 1);
                toColor = visualMap.IsPiecewise() ? startColor : visualMap.GetColor(yValue);
            }
        }

        public static Color32 GetLineGradientColor(VisualMap visualMap, Vector3 pos, GridCoord grid, Axis axis,
            Axis relativedAxis, Color32 defaultColor)
        {
            double value = 0;
            double min = 0;
            double max = 0;

            if (visualMap.dimension == 0)
            {
                min = axis.context.minValue;
                max = axis.context.maxValue;
                if (axis.IsCategory() && axis.boundaryGap)
                {
                    float startX = grid.context.x + axis.context.scaleWidth / 2;
                    value = (min + (pos.x - startX) / (grid.context.width - axis.context.scaleWidth) * (max - min));
                    if (visualMap.IsPiecewise())
                        value = (int) value;
                }
                else
                {
                    value = min + (pos.x - grid.context.x) / grid.context.width * (max - min);
                }
            }
            else
            {
                min = relativedAxis.context.minValue;
                max = relativedAxis.context.maxValue;
                if (relativedAxis.IsCategory() && relativedAxis.boundaryGap)
                {
                    float startY = grid.context.y + relativedAxis.context.scaleWidth / 2;
                    value = (min + (pos.y - startY) / (grid.context.height - relativedAxis.context.scaleWidth) * (max - min));
                    if (visualMap.IsPiecewise())
                        value = (int) value;
                }
                else
                {
                    value = min + (pos.y - grid.context.y) / grid.context.height * (max - min);
                }
            }

            var color = visualMap.GetColor(value);
            if (ChartHelper.IsClearColor(color))
            {
                return defaultColor;
            }
            else
            {
                if (color.a != 0)
                    color.a = defaultColor.a;

                return color;
            }
        }

        public static Color32 GetItemStyleGradientColor(ItemStyle itemStyle, Vector3 pos, BaseChart chart,
            Axis axis, Color32 defaultColor)
        {
            var min = axis.context.minValue;
            var max = axis.context.maxValue;
            var grid = chart.GetChartComponent<GridCoord>(axis.gridIndex);
            var value = min + (pos.x - grid.context.x) / grid.context.width * (max - min);
            var rate = (value - min) / (max - min);
            var color = itemStyle.GetGradientColor((float) rate, defaultColor);

            if (ChartHelper.IsClearColor(color))
                return defaultColor;
            else
                return color;
        }

        public static Color32 GetLineStyleGradientColor(LineStyle lineStyle, Vector3 pos, GridCoord grid,
            Axis axis, Color32 defaultColor)
        {
            var min = axis.context.minValue;
            var max = axis.context.maxValue;
            var value = min + (pos.x - grid.context.x) / grid.context.width * (max - min);
            var rate = (value - min) / (max - min);
            var color = lineStyle.GetGradientColor((float) rate, defaultColor);

            if (ChartHelper.IsClearColor(color))
                return defaultColor;
            else
                return color;
        }

        public static bool IsNeedGradient(VisualMap visualMap)
        {
            if (visualMap == null)
                return false;
            if (!visualMap.show || (!visualMap.workOnLine && !visualMap.workOnArea))
                return false;
            if (visualMap.inRange.Count <= 0)
                return false;
            return true;
        }
        public static bool IsNeedLineGradient(VisualMap visualMap)
        {
            if (visualMap == null)
                return false;
            if (!visualMap.show || !visualMap.workOnLine)
                return false;
            if (visualMap.inRange.Count <= 0)
                return false;
            return true;
        }
        public static bool IsNeedAreaGradient(VisualMap visualMap)
        {
            if (visualMap == null)
                return false;
            if (!visualMap.show || !visualMap.workOnArea)
                return false;
            if (visualMap.inRange.Count <= 0)
                return false;
            return true;
        }

        public static int GetDimension(VisualMap visualMap, int serieDataCount)
        {
            var dimension = visualMap != null && visualMap.dimension >= 0 ?
                visualMap.dimension : serieDataCount - 1;

            if (dimension > serieDataCount - 1)
                dimension = serieDataCount - 1;

            return dimension;
        }
    }
}