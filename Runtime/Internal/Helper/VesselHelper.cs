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
    public static class VesselHelper
    {
        public static Color32 GetColor(Vessel vessel, Serie serie, ChartTheme theme, List<string> legendRealShowName)
        {
            if (serie != null && vessel.autoColor)
            {
                var colorIndex = legendRealShowName.IndexOf(serie.name);
                return SerieHelper.GetItemColor(serie, null, theme, colorIndex, false);
            }
            else
            {
                return vessel.color;
            }
        }

        public static void UpdateVesselCenter(Vessel vessel, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (vessel.center.Length < 2) return;
            var centerX = vessel.center[0] <= 1 ? chartWidth * vessel.center[0] : vessel.center[0];
            var centerY = vessel.center[1] <= 1 ? chartHeight * vessel.center[1] : vessel.center[1];
            var checkWidth = Mathf.Min(chartWidth, chartHeight);
            vessel.runtimeCenterPos = chartPosition + new Vector3(centerX, centerY);
            vessel.runtimeRadius = ChartHelper.GetRuntimeRelativeOrAbsoluteValue(vessel.radius, checkWidth);
            vessel.runtimeInnerRadius = vessel.runtimeRadius - vessel.shapeWidth - vessel.gap;
            vessel.runtimeWidth = ChartHelper.GetRuntimeRelativeOrAbsoluteValue(vessel.width, checkWidth) - 2 * vessel.gap;
            vessel.runtimeHeight = ChartHelper.GetRuntimeRelativeOrAbsoluteValue(vessel.height, chartHeight) - 2 * vessel.gap;
        }
    }
}