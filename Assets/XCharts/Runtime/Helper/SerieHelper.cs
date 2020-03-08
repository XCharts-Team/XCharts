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
    }
}