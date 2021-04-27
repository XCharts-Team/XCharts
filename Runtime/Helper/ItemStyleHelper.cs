/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    public static class ItemStyleHelper
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