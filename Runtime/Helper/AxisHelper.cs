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
    internal static class AxisHelper
    {
        public static float GetTickWidth(Axis axis)
        {
            return axis.axisTick.width != 0 ? axis.axisTick.width : axis.axisLine.width;
        }
    }
}