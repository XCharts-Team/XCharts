/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    public static class ThemeHelper
    {
        public static Color32 GetBackgroundColor(ChartTheme theme, Background background)
        {
            if (background.show && background.hideThemeBackgroundColor) return ChartConst.clearColor32;
            else return theme.backgroundColor;
        }
    }
}