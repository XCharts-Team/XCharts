/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    internal static class SerieHelper
    {
        internal static Color GetItemBackgroundColor(Serie serie, ThemeInfo theme, int index, bool highlight)
        {
            if (serie.itemStyle.backgroundColor != Color.clear)
            {
                var color = serie.itemStyle.backgroundColor;
                if (highlight) color *= color;
                color.a *= serie.itemStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a = 0.2f;
                return color;
            }
        }

        internal static Color GetItemColor(Serie serie, ThemeInfo theme, int index, bool highlight)
        {
            if (serie.itemStyle.color != Color.clear)
            {
                var color = serie.itemStyle.color;
                if (highlight) color *= color;
                color.a *= serie.itemStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a *= serie.itemStyle.opacity;
                return color;
            }
        }

        public static bool IsDownPoint(Serie serie, int index)
        {
            var dataPoints = serie.dataPoints;
            if (dataPoints.Count < 2) return false;
            else if (index > 0 && index < dataPoints.Count - 1)
            {
                var lp = dataPoints[index - 1];
                var np = dataPoints[index + 1];
                var cp = dataPoints[index];
                var dot = Vector3.Cross(np - lp, cp - np);
                return dot.z < 0;
            }
            else if (index == 0)
            {
                return dataPoints[0].y < dataPoints[1].y;
            }
            else if (index == dataPoints.Count - 1)
            {
                return dataPoints[index].y < dataPoints[index - 1].y;
            }
            else
            {
                return false;
            }
        }
    }
}