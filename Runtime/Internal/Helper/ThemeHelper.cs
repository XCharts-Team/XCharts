/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;

namespace XCharts
{
    internal static class ThemeHelper
    {
        public static Color GetBackgroundColor(ThemeInfo themeInfo, Background background)
        {
            if (background.show && background.runtimeActive && background.hideThemeBackgroundColor) return Color.clear;
            else return themeInfo.backgroundColor;
        }
    }
}