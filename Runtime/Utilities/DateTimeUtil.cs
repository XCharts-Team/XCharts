using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class DateTimeUtil
    {
#if UNITY_2018_3_OR_NEWER
        private static readonly DateTime k_LocalDateTime1970 = TimeZoneInfo.ConvertTimeFromUtc(new DateTime(1970, 1, 1), TimeZoneInfo.Local);     
#else
        private static readonly DateTime k_LocalDateTime1970 = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
#endif
        private static readonly DateTime k_DateTime1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static readonly int ONE_SECOND = 1;
        public static readonly int ONE_MINUTE = ONE_SECOND * 60;
        public static readonly int ONE_HOUR = ONE_MINUTE * 60;
        public static readonly int ONE_DAY = ONE_HOUR * 24;
        public static readonly int ONE_MONTH = ONE_DAY * 30;
        public static readonly int ONE_YEAR = ONE_DAY * 365;
        public static readonly int MIN_TIME_SPLIT_NUMBER = 4;

        private static string s_YearDateFormatter = "yyyy";
        //private static string s_MonthDateFormatter = "MM";
        //private static string s_DayDateFormatter = "dd";
        //private static string s_HourDateFormatter = "HH:mm";
        //private static string s_MinuteDateFormatter = "mm:ss";
        private static string s_SecondDateFormatter = "HH:mm:ss";
        //private static string s_FullDateFormatter = "yyyy-MM-dd HH:mm:ss";
        private static Regex s_DateOrTimeRegex = new Regex(@"^(date|time)\s*[:\s]+(.*)", RegexOptions.IgnoreCase);

        public static bool IsDateOrTimeRegex(string regex)
        {
            return regex.StartsWith("date") || regex.StartsWith("time");
        }

        public static bool IsDateOrTimeRegex(string regex, ref bool date, ref string formatter)
        {
            if (IsDateOrTimeRegex(regex))
            {
                if (regex == "date" || regex == "time")
                {
                    date = regex == "date";
                    formatter = "";
                    return true;
                }
                var mc = s_DateOrTimeRegex.Matches(regex);
                date = mc[0].Groups[1].Value == "date";
                formatter = mc[0].Groups[2].Value;
                return true;
            }
            return false;
        }

        public static double GetTimestamp()
        {
            return (DateTime.Now - k_LocalDateTime1970).TotalSeconds;
        }

        public static double GetTimestamp(DateTime time, bool local = false)
        {
            if (local)
            {
                return (time - k_LocalDateTime1970).TotalSeconds;
            }
            else
            {
                return (time - k_DateTime1970).TotalSeconds;
            }
        }

        public static double GetTimestamp(string dateTime, bool local = false)
        {
            try
            {

                return GetTimestamp(DateTime.Parse(dateTime), local);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static DateTime GetDateTime(double timestamp, bool local = false)
        {
            var dateTime = local ? k_LocalDateTime1970.AddSeconds(timestamp) : k_DateTime1970.AddSeconds(timestamp);
            return dateTime;
        }

        public static string GetDefaultDateTimeString(double timestamp, double range = 0, bool local = false)
        {
            var dateString = String.Empty;
            var dateTime = GetDateTime(timestamp, local);
            if (range <= 0 || range >= DateTimeUtil.ONE_DAY)
            {
                dateString = dateTime.ToString("yyyy-MM-dd");
            }
            else
            {
                dateString = dateTime.ToString(s_SecondDateFormatter);
            }
            return dateString;
        }

        internal static string GetDateTimeFormatString(DateTime dateTime, double range)
        {
            var dateString = String.Empty;
            if (range >= DateTimeUtil.ONE_YEAR * DateTimeUtil.MIN_TIME_SPLIT_NUMBER)
            {
                dateString = dateTime.ToString(s_YearDateFormatter);
            }
            else if (range >= DateTimeUtil.ONE_MONTH * DateTimeUtil.MIN_TIME_SPLIT_NUMBER)
            {
                dateString = dateTime.Month == 1 ?
                    dateTime.ToString(s_YearDateFormatter) :
                    XCSettings.lang.GetMonthAbbr(dateTime.Month);
            }
            else if (range >= DateTimeUtil.ONE_DAY * DateTimeUtil.MIN_TIME_SPLIT_NUMBER)
            {
                dateString = dateTime.Day == 1 ?
                    XCSettings.lang.GetMonthAbbr(dateTime.Month) :
                    XCSettings.lang.GetDay(dateTime.Day);
            }
            else if (range >= DateTimeUtil.ONE_HOUR * DateTimeUtil.MIN_TIME_SPLIT_NUMBER)
            {
                dateString = dateTime.ToString(s_SecondDateFormatter);
            }
            else if (range >= DateTimeUtil.ONE_MINUTE * DateTimeUtil.MIN_TIME_SPLIT_NUMBER)
            {
                dateString = dateTime.ToString(s_SecondDateFormatter);
            }
            else
            {
                dateString = dateTime.ToString(s_SecondDateFormatter);
            }
            return dateString;
        }

        /// <summary>
        /// 根据给定的最大最小时间戳范围，计算合适的Tick值
        /// </summary>
        /// <param name="list"></param>
        /// <param name="minTimestamp"></param>
        /// <param name="maxTimestamp"></param>
        /// <param name="splitNumber"></param>
        internal static float UpdateTimeAxisDateTimeList(List<double> list, double minTimestamp, double maxTimestamp, int splitNumber, double ceilRate)
        {
            var range = maxTimestamp - minTimestamp;
            if (range <= 0)
            {
                list.Clear();
                return 0;
            }
            var dtMin = GetDateTime(minTimestamp);
            var dtMax = GetDateTime(maxTimestamp);
            int tick;
            if (ceilRate != 0)
            {
                var tickSecond = (int)ceilRate;
                tick = GetTickSecond(range, 0, tickSecond);
                var let = minTimestamp % tickSecond;
                var defaultTimestamp = let == 0 ? minTimestamp : minTimestamp - let + tickSecond;
                var startTimestamp = (int)GetFirstMaxValue(list, minTimestamp, defaultTimestamp);
                while (startTimestamp > minTimestamp)
                {
                    startTimestamp -= tick;
                }
                if (startTimestamp < minTimestamp)
                {
                    startTimestamp += tick;
                }
                list.Clear();
                AddTickTimestamp(list, startTimestamp, maxTimestamp, tick);
            }
            else
            {
                if (range >= ONE_YEAR * MIN_TIME_SPLIT_NUMBER)
                {
                    var num = splitNumber <= 0 ? GetSplitNumber(range, ONE_YEAR) : (int)Math.Max(range / (splitNumber * ONE_YEAR), 1);
                    var dtStart = GetDateTime(GetFirstMaxValue(list, minTimestamp));
                    dtStart = new DateTime(dtStart.Year, dtStart.Month, 1);
                    while (dtStart > dtMin)
                    {
                        dtStart = dtStart.AddYears(-num);
                    }
                    if (dtStart < dtMin)
                    {
                        dtStart = dtStart.AddYears(num);
                    }
                    tick = num * 365 * 24 * 3600;
                    list.Clear();
                    while (dtStart.Ticks < dtMax.Ticks)
                    {
                        list.Add(DateTimeUtil.GetTimestamp(dtStart));
                        dtStart = dtStart.AddYears(num);
                    }
                }
                else if (range >= ONE_MONTH * MIN_TIME_SPLIT_NUMBER)
                {
                    var num = splitNumber <= 0 ? GetSplitNumber(range, ONE_MONTH) : (int)Math.Max(range / (splitNumber * ONE_MONTH), 1);
                    var dtStart = GetDateTime(GetFirstMaxValue(list, minTimestamp));
                    dtStart = new DateTime(dtStart.Year, dtStart.Month, 1);
                    while (dtStart > dtMin)
                    {
                        dtStart = dtStart.AddMonths(-num);
                    }
                    if (dtStart < dtMin)
                    {
                        dtStart = dtStart.AddMonths(num);
                    }
                    tick = num * 30 * 24 * 3600;
                    list.Clear();
                    while (dtStart.Ticks < dtMax.Ticks)
                    {
                        list.Add(DateTimeUtil.GetTimestamp(dtStart));
                        dtStart = dtStart.AddMonths(num);
                    }
                }
                else
                {
                    int tickSecond;
                    if (range >= ONE_DAY * MIN_TIME_SPLIT_NUMBER)
                    {
                        tickSecond = ONE_DAY;
                    }
                    else if (range >= ONE_HOUR * MIN_TIME_SPLIT_NUMBER)
                    {
                        tickSecond = ONE_HOUR;
                    }
                    else if (range >= ONE_MINUTE * MIN_TIME_SPLIT_NUMBER)
                    {
                        tickSecond = ONE_MINUTE;
                    }
                    else
                    {
                        tickSecond = ONE_SECOND;
                    }
                    tick = GetTickSecond(range, splitNumber, tickSecond);
                    var let = minTimestamp % tickSecond;
                    var defaultTimestamp = let == 0 ? minTimestamp : minTimestamp - let + tickSecond;
                    var startTimestamp = (int)GetFirstMaxValue(list, minTimestamp, defaultTimestamp);
                    while (startTimestamp > minTimestamp)
                    {
                        startTimestamp -= tick;
                    }
                    if (startTimestamp < minTimestamp)
                    {
                        startTimestamp += tick;
                    }
                    list.Clear();
                    AddTickTimestamp(list, startTimestamp, maxTimestamp, tick);
                }
            }
            return tick;
        }

        private static double GetFirstMaxValue(List<double> list, double minTimestamp, double defaultTimestamp = 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] >= minTimestamp)
                {
                    return list[i];
                }
            }
            return defaultTimestamp == 0 ? minTimestamp : defaultTimestamp;
        }

        private static int GetSplitNumber(double range, int tickSecond)
        {
            var num = 1;
            while (range / (num * tickSecond) > 8)
            {
                num++;
            }
            return num;
        }

        private static int GetTickSecond(double range, int splitNumber, int tickSecond)
        {
            var num = 0;
            if (splitNumber > 0)
            {
                num = (int)Math.Max(range / (splitNumber * tickSecond), 1);
            }
            else
            {
                num = 1;
                var tick = tickSecond;
                while (range / tick > 8)
                {
                    num++;
                    tick = num * tickSecond;
                }
            }
            return num * tickSecond;
        }

        private static void AddTickTimestamp(List<double> list, double startTimestamp, double maxTimestamp, int tickSecond)
        {
            while (startTimestamp <= maxTimestamp)
            {
                list.Add(startTimestamp);
                startTimestamp += tickSecond;
            }
        }
    }
}