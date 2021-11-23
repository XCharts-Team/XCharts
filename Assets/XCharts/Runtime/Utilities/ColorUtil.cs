/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    public static class ColorUtil
    {
        public static readonly Color32 clearColor32 = new Color32(0, 0, 0, 0);
        public static readonly Vector2 zeroVector2 = Vector2.zero;
        /// <summary>
        /// Convert the html string to color. 
        /// 将字符串颜色值转成Color。
        /// </summary>
        /// <param name="hexColorStr"></param>
        /// <returns></returns>
        public static Color32 GetColor(string hexColorStr)
        {
            Color color;
            ColorUtility.TryParseHtmlString(hexColorStr, out color);
            return (Color32)color;
        }
    }
}