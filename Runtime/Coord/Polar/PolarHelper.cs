using UnityEngine;

namespace XCharts.Runtime
{
    internal static class PolarHelper
    {
        public static void UpdatePolarCenter(PolarCoord polar, Vector3 chartPosition, float chartWidth, float chartHeight)
        {
            if (polar.center.Length < 2) return;
            var centerX = polar.center[0] <= 1 ? chartWidth * polar.center[0] : polar.center[0];
            var centerY = polar.center[1] <= 1 ? chartHeight * polar.center[1] : polar.center[1];
            polar.context.center = chartPosition + new Vector3(centerX, centerY);
            if (polar.radius <= 0)
            {
                polar.context.radius = 0;
            }
            else if (polar.radius <= 1)
            {
                polar.context.radius = Mathf.Min(chartWidth, chartHeight) * polar.radius;
            }
            else
            {
                polar.context.radius = polar.radius;
            }
        }
    }
}