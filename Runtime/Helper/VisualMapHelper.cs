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
            double min = 0;
            double max = 0;
            if (visualMap.dimension == 0)
            {
                min = xAxis.IsCategory() ? 0 : xAxis.runtimeMinValue;
                max = xAxis.IsCategory() ? serie.dataCount - 1 : xAxis.runtimeMaxValue;
                SetMinMax(visualMap, min, max);
            }
            else
            {
                min = yAxis.IsCategory() ? 0 : yAxis.runtimeMinValue;
                max = yAxis.IsCategory() ? serie.dataCount - 1 : yAxis.runtimeMaxValue;
                SetMinMax(visualMap, min, max);
            }
        }

        public static void SetMinMax(VisualMap visualMap, double min, double max)
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

        public static Color32 GetLineGradientColor(VisualMap visualMap, Vector3 pos, CoordinateChart chart, Axis axis,
            Color32 defaultColor)
        {
            double value = 0;
            double min = 0;
            double max = 0;
            if (visualMap.dimension == 0)
            {
                min = axis.runtimeMinValue;
                max = axis.runtimeMaxValue;
                var grid = chart.GetAxisGridOrDefault(axis);
                if (axis.IsCategory() && axis.boundaryGap)
                {
                    float startX = grid.runtimeX + axis.runtimeScaleWidth / 2;
                    value = (int)(min + (pos.x - startX) / (grid.runtimeWidth - axis.runtimeScaleWidth) * (max - min));
                }
                else
                {
                    value = min + (pos.x - grid.runtimeX) / grid.runtimeWidth * (max - min);
                }
            }
            else
            {
                Axis yAxis;
                if (axis is YAxis)
                {
                    yAxis = chart.xAxes[axis.index];
                    min = yAxis.runtimeMinValue;
                    max = yAxis.runtimeMaxValue;
                }
                else
                {
                    yAxis = chart.yAxes[axis.index];
                    min = yAxis.runtimeMinValue;
                    max = yAxis.runtimeMaxValue;
                }
                var grid = chart.GetAxisGridOrDefault(axis);
                if (yAxis.IsCategory() && yAxis.boundaryGap)
                {
                    float startY = grid.runtimeY + yAxis.runtimeScaleWidth / 2;
                    value = (int)(min + (pos.y - startY) / (grid.runtimeHeight - yAxis.runtimeScaleWidth) * (max - min));
                }
                else
                {
                    value = min + (pos.y - grid.runtimeY) / grid.runtimeHeight * (max - min);
                }
            }
            var color = visualMap.GetColor(value);
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }

        public static Color32 GetItemStyleGradientColor(ItemStyle itemStyle, Vector3 pos, CoordinateChart chart,
            Axis axis, Color32 defaultColor)
        {
            var min = axis.runtimeMinValue;
            var max = axis.runtimeMaxValue;
            var grid = chart.GetAxisGridOrDefault(axis);
            var value = min + (pos.x - grid.runtimeX) / grid.runtimeWidth * (max - min);
            var rate = (value - min) / (max - min);
            var color = itemStyle.GetGradientColor((float)rate, defaultColor);
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }

        public static Color32 GetLineStyleGradientColor(LineStyle lineStyle, Vector3 pos, CoordinateChart chart,
            Axis axis, Color32 defaultColor)
        {
            var min = axis.runtimeMinValue;
            var max = axis.runtimeMaxValue;
            var grid = chart.GetAxisGridOrDefault(axis);
            var value = min + (pos.x - grid.runtimeX) / grid.runtimeWidth * (max - min);
            var rate = (value - min) / (max - min);
            var color = lineStyle.GetGradientColor((float)rate, defaultColor);
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }

        public static bool IsNeedGradient(VisualMap visualMap)
        {
            if (!visualMap.enable || (visualMap.inRange.Count <= 0 && visualMap.pieces.Count <= 0)) return false;
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