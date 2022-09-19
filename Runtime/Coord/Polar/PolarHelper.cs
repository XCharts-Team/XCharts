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
            var minWidth = Mathf.Min(chartWidth, chartHeight);

            polar.context.center = chartPosition + new Vector3(centerX, centerY);
            polar.context.insideRadius = polar.context.outsideRadius = 0;
            if (polar.radius.Length >= 2)
            {
                polar.context.insideRadius = ChartHelper.GetActualValue(polar.radius[0], minWidth, 1);
                polar.context.outsideRadius = ChartHelper.GetActualValue(polar.radius[1], minWidth, 1);
            }
            else if (polar.radius.Length >= 1)
            {
                polar.context.outsideRadius = ChartHelper.GetActualValue(polar.radius[0], minWidth, 1);
            }
            polar.context.radius = polar.context.outsideRadius - polar.context.insideRadius;
        }

        public static Vector3 UpdatePolarAngleAndPos(PolarCoord polar, AngleAxis angleAxis, RadiusAxis radiusAxis, SerieData serieData)
        {
            var value = serieData.GetData(0);
            var angle = angleAxis.GetValueAngle(serieData.GetData(1));
            var radius = polar.context.insideRadius + radiusAxis.GetValueLength(value, polar.context.radius);

            angle = (angle + 360) % 360;
            serieData.context.angle = angle;
            serieData.context.position = ChartHelper.GetPos(polar.context.center, radius, angle, true);

            return serieData.context.position;
        }
    }
}