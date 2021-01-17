/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    public static class VisualMapHelper
    {
        public static void AutoSetLineMinMax(VisualMap visualMap, Serie serie, XAxis xAxis, YAxis yAxis)
        {
            if (!IsNeedGradient(visualMap) || !visualMap.autoMinMax) return;
            float min = 0;
            float max = 0;
            switch (visualMap.direction)
            {
                case VisualMap.Direction.Default:
                case VisualMap.Direction.X:
                    min = xAxis.IsCategory() ? 0 : xAxis.runtimeMinValue;
                    max = xAxis.IsCategory() ? serie.dataCount : xAxis.runtimeMaxValue;
                    SetMinMax(visualMap, min, max);
                    break;
                case VisualMap.Direction.Y:
                    min = yAxis.IsCategory() ? 0 : yAxis.runtimeMinValue;
                    max = yAxis.IsCategory() ? serie.dataCount : yAxis.runtimeMaxValue;
                    SetMinMax(visualMap, min, max);
                    break;
            }
        }

        public static void SetMinMax(VisualMap visualMap, float min, float max)
        {
            if (visualMap.enable && (visualMap.min != min || visualMap.max != max))
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
            switch (visualMap.direction)
            {
                case VisualMap.Direction.Default:
                case VisualMap.Direction.X:
                    startColor = visualMap.IsPiecewise() ? visualMap.GetColor(xValue) : visualMap.GetColor(xValue - 1);
                    toColor = visualMap.IsPiecewise() ? startColor : visualMap.GetColor(xValue);
                    break;
                case VisualMap.Direction.Y:
                    startColor = visualMap.IsPiecewise() ? visualMap.GetColor(yValue) : visualMap.GetColor(yValue - 1);
                    toColor = visualMap.IsPiecewise() ? startColor : visualMap.GetColor(yValue);
                    break;
            }
        }

        internal static Color32 GetLineGradientColor(VisualMap visualMap, Vector3 pos, CoordinateChart chart, Axis axis, Color32 defaultColor)
        {
            float value = 0;
            switch (visualMap.direction)
            {
                case VisualMap.Direction.Default:
                case VisualMap.Direction.X:
                    var min = axis.runtimeMinValue;
                    var max = axis.runtimeMaxValue;
                    var grid = chart.GetAxisGridOrDefault(axis);
                    value = min + (pos.x - grid.runtimeX) / grid.runtimeWidth * (max - min);
                    break;
                case VisualMap.Direction.Y:
                    if (axis is YAxis)
                    {
                        var yAxis = chart.xAxes[axis.index];
                        min = yAxis.runtimeMinValue;
                        max = yAxis.runtimeMaxValue;
                    }
                    else
                    {
                        var yAxis = chart.yAxes[axis.index];
                        min = yAxis.runtimeMinValue;
                        max = yAxis.runtimeMaxValue;
                    }
                    grid = chart.GetAxisGridOrDefault(axis);
                    value = min + (pos.y - grid.runtimeY) / grid.runtimeHeight * (max - min);
                    break;
            }
            var color = visualMap.GetColor(value);
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }

        internal static Color32 GetItemStyleGradientColor(ItemStyle itemStyle, Vector3 pos, CoordinateChart chart, Axis axis, Color32 defaultColor)
        {
            var min = axis.runtimeMinValue;
            var max = axis.runtimeMaxValue;
            var grid = chart.GetAxisGridOrDefault(axis);
            var value = min + (pos.x - grid.runtimeX) / grid.runtimeWidth * (max - min);
            var rate = (value - min) / (max - min);
            var color = itemStyle.GetGradientColor(rate, defaultColor);
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }

        internal static Color32 GetLineStyleGradientColor(LineStyle lineStyle, Vector3 pos, CoordinateChart chart, Axis axis, Color32 defaultColor)
        {
            var min = axis.runtimeMinValue;
            var max = axis.runtimeMaxValue;
            var grid = chart.GetAxisGridOrDefault(axis);
            var value = min + (pos.x - grid.runtimeX) / grid.runtimeWidth * (max - min);
            var rate = (value - min) / (max - min);
            var color = lineStyle.GetGradientColor(rate, defaultColor);
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }

        public static bool IsNeedGradient(VisualMap visualMap)
        {
            if (!visualMap.enable || visualMap.inRange.Count <= 0) return false;
            return true;
        }

        public static int GetDimension(VisualMap visualMap, int serieDataCount)
        {
            var dimension = visualMap.enable && visualMap.dimension >= 0 ? visualMap.dimension : serieDataCount - 1;
            if (dimension > serieDataCount - 1) dimension = serieDataCount - 1;
            return dimension;
        }
    }
}