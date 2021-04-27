/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    public static class PolarHelper
    {
        public static void UpdatePolarCenter(Polar polar, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (polar.center.Length < 2) return;
            var centerX = polar.center[0] <= 1 ? chartWidth * polar.center[0] : polar.center[0];
            var centerY = polar.center[1] <= 1 ? chartHeight * polar.center[1] : polar.center[1];
            polar.runtimeCenterPos = chartPosition + new Vector3(centerX, centerY);
            if (polar.radius <= 0)
            {
                polar.runtimeRadius = 0;
            }
            else if (polar.radius <= 1)
            {
                polar.runtimeRadius = Mathf.Min(chartWidth, chartHeight) * polar.radius;
            }
            else
            {
                polar.runtimeRadius = polar.radius;
            }
        }
    }
}