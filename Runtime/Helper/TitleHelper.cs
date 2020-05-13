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
    internal static class TitleHelper
    {
        public static Font GetTextFont(Title title, ThemeInfo themeInfo)
        {
            return (title.textStyle.font != null) ? title.textStyle.font : themeInfo.font;
        }

        public static Color GetTextColor(Title title, ThemeInfo themeInfo)
        {
            return !ChartHelper.IsClearColor(title.textStyle.color) ? title.textStyle.color : (Color)themeInfo.titleTextColor;
        }

        public static Font GetSubTextFont(Title title, ThemeInfo themeInfo)
        {
            return (title.subTextStyle.font != null) ? title.subTextStyle.font : themeInfo.font;
        }

        public static Color GetSubTextColor(Title title, ThemeInfo themeInfo)
        {
            return !ChartHelper.IsClearColor(title.subTextStyle.color) ? title.subTextStyle.color : (Color)themeInfo.titleSubTextColor;
        }
    }
}