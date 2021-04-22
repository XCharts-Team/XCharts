/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    public static class DateTimeUtil
    {
        private static readonly DateTime k_DateTime1970 = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);

        public static int GetTimestamp()
        {
            return (int)(DateTime.Now - k_DateTime1970).TotalSeconds;
        }

        public static int GetTimestamp(DateTime time)
        {
            return (int)(time - k_DateTime1970).TotalSeconds;
        }

        public static DateTime GetDateTime(int timestamp)
        {
            long span = ((long)timestamp) * 10000000;
            return k_DateTime1970.Add(new TimeSpan(span));
        }
    }
}