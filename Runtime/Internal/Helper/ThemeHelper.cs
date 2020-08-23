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
        public static Color32 GetBackgroundColor(ThemeInfo themeInfo, Background background)
        {
            if (background.show && background.runtimeActive && background.hideThemeBackgroundColor) return ChartConst.clearColor32;
            else return themeInfo.backgroundColor;
        }
    }
}