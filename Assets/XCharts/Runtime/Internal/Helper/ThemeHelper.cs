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
        public static Color GetBackgroundColor(ThemeInfo themeInfo, Background background, bool m_IsControlledByLayout)
        {
            if (!m_IsControlledByLayout && background.show && background.hideThemeBackgroundColor) return Color.clear;
            else return themeInfo.backgroundColor;
        }
    }
}