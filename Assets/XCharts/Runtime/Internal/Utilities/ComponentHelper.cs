/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/


using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public static class ComponentHelper
    {
        public static AngleAxis GetAngleAxis(List<MainComponent> components, int polarIndex)
        {
            foreach (var component in components)
            {
                if (component is AngleAxis)
                {
                    var axis = component as AngleAxis;
                    if (axis.polarIndex == polarIndex) return axis;
                }
            }
            return null;
        }
        public static RadiusAxis GetRadiusAxis(List<MainComponent> components, int polarIndex)
        {
            foreach (var component in components)
            {
                if (component is RadiusAxis)
                {
                    var axis = component as RadiusAxis;
                    if (axis.polarIndex == polarIndex) return axis;
                }
            }
            return null;
        }

        public static YAxis GetYAxisRelatedWitchXAxis(List<MainComponent> components, XAxis axis)
        {
            foreach (var component in components)
            {
                if (component is YAxis)
                {
                    var yAxis = component as YAxis;
                    if (yAxis.gridIndex == axis.gridIndex) return yAxis;
                }
            }
            return null;
        }

        public static XAxis GetXAxisRelatedWithYAxis(List<MainComponent> components, YAxis axis)
        {
            foreach (var component in components)
            {
                if (component is XAxis)
                {
                    var xAxis = component as XAxis;
                    if (xAxis.gridIndex == axis.gridIndex) return xAxis;
                }
            }
            return null;
        }

        public static float GetXAxisOnZeroOffset(List<MainComponent> components, XAxis axis)
        {
            if (!axis.axisLine.onZero) return 0;
            foreach (var component in components)
            {
                if (component is YAxis)
                {
                    var yAxis = component as YAxis;
                    if (yAxis.IsValue() && yAxis.gridIndex == axis.gridIndex) return yAxis.context.offset;
                }
            }
            return 0;
        }

        public static float GetYAxisOnZeroOffset(List<MainComponent> components, YAxis axis)
        {
            if (!axis.axisLine.onZero) return 0;
            foreach (var component in components)
            {
                if (component is XAxis)
                {
                    var xAxis = component as XAxis;
                    if (xAxis.IsValue() && xAxis.gridIndex == axis.gridIndex) return xAxis.context.offset;
                }
            }
            return 0;
        }

        public static bool IsAnyCategoryOfYAxis(List<MainComponent> components)
        {
            foreach (var component in components)
            {
                if (component is YAxis)
                {
                    var yAxis = component as YAxis;
                    if (yAxis.type == Axis.AxisType.Category)
                        return true;
                }
            }
            return false;
        }
    }
}