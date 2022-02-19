
using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    internal static class VesselHelper
    {
        public static Color32 GetColor(Vessel vessel, Serie serie, ThemeStyle theme, List<string> legendRealShowName)
        {
            if (serie != null && vessel.autoColor)
            {
                var colorIndex = legendRealShowName.IndexOf(serie.serieName);
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
            vessel.context.center = chartPosition + new Vector3(centerX, centerY);
            vessel.context.radius = ChartHelper.GetRuntimeRelativeOrAbsoluteValue(vessel.radius, checkWidth);
            vessel.context.innerRadius = vessel.context.radius - vessel.shapeWidth - vessel.gap;
            vessel.context.width = ChartHelper.GetRuntimeRelativeOrAbsoluteValue(vessel.width, checkWidth) - 2 * vessel.gap;
            vessel.context.height = ChartHelper.GetRuntimeRelativeOrAbsoluteValue(vessel.height, chartHeight) - 2 * vessel.gap;
        }
    }
}