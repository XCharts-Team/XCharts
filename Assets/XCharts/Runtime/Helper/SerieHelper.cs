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
        internal static Color GetItemBackgroundColor(Serie serie, SerieData serieData, ThemeInfo theme, int index, bool highlight, bool useDefault = true)
        {
            var itemStyle = GetItemStyle(serie, serieData);
            var color = Color.clear;
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, serieData);
                if (itemStyleEmphasis != null && itemStyleEmphasis.backgroundColor != Color.clear)
                {
                    color = itemStyleEmphasis.backgroundColor;
                    color.a *= itemStyleEmphasis.opacity;
                    return color;
                }
            }
            if (itemStyle.backgroundColor != Color.clear)
            {
                color = itemStyle.backgroundColor;
                if (highlight) color *= color;
                color.a *= itemStyle.opacity;
                return color;
            }
            else if (useDefault)
            {
                color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a = 0.2f;
                return color;
            }
            return Color.clear;
        }

        internal static Color GetItemColor(Serie serie, SerieData serieData, ThemeInfo theme, int index, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData);
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, serieData);
                if (itemStyleEmphasis != null && itemStyleEmphasis.color != Color.clear)
                {
                    var color = itemStyleEmphasis.color;
                    color.a *= itemStyleEmphasis.opacity;
                    return color;
                }
            }
            if (itemStyle.color != Color.clear)
            {
                var color = itemStyle.color;
                if (highlight) color *= color;
                color.a *= itemStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a *= itemStyle.opacity;
                return color;
            }
        }

        internal static Color GetItemToColor(Serie serie, SerieData serieData, ThemeInfo theme, int index, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, serieData);
                if (itemStyleEmphasis != null && itemStyleEmphasis.toColor != Color.clear)
                {
                    var color = itemStyleEmphasis.toColor;
                    color.a *= itemStyleEmphasis.opacity;
                    return color;
                }
            }
            if (itemStyle == null) itemStyle = serieData.itemStyle;
            if (itemStyle.toColor != Color.clear)
            {
                var color = itemStyle.toColor;
                if (highlight) color *= color;
                color.a *= itemStyle.opacity;
                return color;
            }
            if (itemStyle.color != Color.clear)
            {
                var color = itemStyle.color;
                if (highlight) color *= color;
                color.a *= itemStyle.opacity;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a *= itemStyle.opacity;
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

        public static ItemStyle GetItemStyle(Serie serie, SerieData serieData, bool highlight = false)
        {
            if (highlight)
            {
                var style = GetItemStyleEmphasis(serie, serieData);
                if (style == null) return GetItemStyle(serie, serieData, false);
                else return style;
            }
            else if (serieData != null && serieData.enableItemStyle) return serieData.itemStyle;
            else return serie.itemStyle;
        }

        public static ItemStyle GetItemStyleEmphasis(Serie serie, SerieData serieData)
        {
            if (serieData != null && serieData.enableEmphasis && serieData.emphasis.show)
                return serieData.emphasis.itemStyle;
            else if (serie.emphasis.show) return serie.emphasis.itemStyle;
            else return null;
        }

        public static SerieLabel GetSerieLabel(Serie serie, SerieData serieData, bool highlight = false)
        {
            if (highlight)
            {
                if (serieData.enableEmphasis && serieData.emphasis.show) return serieData.emphasis.label;
                else if (serie.emphasis.show) return serie.emphasis.label;
                else return serie.label;
            }
            else
            {
                if (serieData.enableLabel) return serieData.label;
                else return serie.label;
            }
        }

        public static Color GetAreaColor(Serie serie, ThemeInfo theme, int index, bool highlight)
        {
            var areaStyle = serie.areaStyle;
            var color = areaStyle.color != Color.clear ? areaStyle.color : (Color)theme.GetColor(index);
            if (highlight)
            {
                if (areaStyle.highlightColor != Color.clear) color = areaStyle.highlightColor;
                else color *= color;
            }
            color.a *= areaStyle.opacity;
            return color;
        }

        public static Color GetAreaToColor(Serie serie, ThemeInfo theme, int index, bool highlight)
        {
            var areaStyle = serie.areaStyle;
            if (areaStyle.toColor != Color.clear)
            {
                var color = areaStyle.toColor;
                if (highlight)
                {
                    if (areaStyle.highlightToColor != Color.clear) color = areaStyle.highlightToColor;
                    else color *= color;
                }
                color.a *= areaStyle.opacity;
                return color;
            }
            else
            {
                return GetAreaColor(serie, theme, index, highlight);
            }
        }

        public static Color GetLineColor(Serie serie, ThemeInfo theme, int index, bool highlight)
        {
            var color = Color.clear;
            if (highlight)
            {
                var itemStyleEmphasis = GetItemStyleEmphasis(serie, null);
                if (itemStyleEmphasis != null && itemStyleEmphasis.color != Color.clear)
                {
                    color = itemStyleEmphasis.color;
                    color.a *= itemStyleEmphasis.opacity;
                    return color;
                }
            }
            if (serie.lineStyle.color != Color.clear) color = serie.lineStyle.GetColor();
            else if (serie.itemStyle.color != Color.clear) color = serie.itemStyle.GetColor();
            if (color == Color.clear)
            {
                color = (Color)theme.GetColor(index);
                color.a = serie.lineStyle.opacity;
            }
            if (highlight) color *= color;
            return color;
        }

        public static float GetSymbolBorder(Serie serie, SerieData serieData, bool highlight, bool useLineWidth = true)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null && itemStyle.borderWidth != 0) return itemStyle.borderWidth;
            else if (serie.lineStyle.width != 0 && useLineWidth) return serie.lineStyle.width;
            else return 0;
        }

        public static float[] GetSymbolCornerRadius(Serie serie, SerieData serieData, bool highlight)
        {
            var itemStyle = GetItemStyle(serie, serieData, highlight);
            if (itemStyle != null) return itemStyle.cornerRadius;
            else return null;
        }
    }
}