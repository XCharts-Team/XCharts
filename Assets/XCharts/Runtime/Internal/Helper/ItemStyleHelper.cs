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
    internal static class ItemStyleHelper
    {
        public static bool IsNeedCorner(ItemStyle itemStyle)
        {
            if (itemStyle.cornerRadius == null) return false;
            foreach (var value in itemStyle.cornerRadius)
            {
                if (value != 0) return true;
            }
            return false;
        }
    }
}