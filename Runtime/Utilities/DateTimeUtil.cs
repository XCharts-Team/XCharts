using System;
using System.Collections.Generic;

namespace XCharts.Runtime
{
    public static class DateTimeUtil
    {
        //private static readonly DateTime k_DateTime1970 = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
        private static readonly DateTime k_DateTime1970 = new DateTime(1970, 1, 1);
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
        private static string s_HourDateFormatter = "HH:mm";
        private static string s_MinuteDateFormatter = "HH:mm";
        private static string s_SecondDateFormatter = "HH:mm:ss";
        //private static string s_DateFormatter = "yyyy-MM-dd HH:mm:ss";

        public static int GetTimestamp()
        {
            return (int) (DateTime.Now - k_DateTime1970).TotalSeconds;
        }

        public static int GetTimestamp(DateTime time)
        {
            return (int) (time - k_DateTime1970).TotalSeconds;
        }

        public static DateTime GetDateTime(int timestamp)
        {
            long span = ((long) timestamp) * 10000000;
            return k_DateTime1970.Add(new TimeSpan(span));
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
                dateString = dateTime.ToString(s_HourDateFormatter);
            }
            else if (range >= DateTimeUtil.ONE_MINUTE * DateTimeUtil.MIN_TIME_SPLIT_NUMBER)
            {
                dateString = dateTime.ToString(s_MinuteDateFormatter);
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
        internal static void UpdateTimeAxisDateTimeList(List<double> list, int minTimestamp, int maxTimestamp, int splitNumber)
        {
            list.Clear();
            var range = maxTimestamp - minTimestamp;
            if (range <= 0) return;
            if (splitNumber <= 0) splitNumber = 1;
            var dtMin = DateTimeUtil.GetDateTime(minTimestamp);
            var dtMax = DateTimeUtil.GetDateTime(maxTimestamp);
            if (range >= ONE_YEAR * MIN_TIME_SPLIT_NUMBER)
            {
                var num = Math.Max(range / (splitNumber * ONE_YEAR), 1);
                var dtStart = new DateTime(dtMin.Year + 1, 1, 1);
                while (dtStart.Ticks < dtMax.Ticks)
                {
                    list.Add(DateTimeUtil.GetTimestamp(dtStart));
                    dtStart = dtStart.AddYears(num);
                }
            }
            else if (range >= ONE_MONTH * MIN_TIME_SPLIT_NUMBER)
            {
                var num = Math.Max(range / (splitNumber * ONE_MONTH), 1);
                var dtStart = new DateTime(dtMin.Year, dtMin.Month, 1).AddMonths(1);
                while (dtStart.Ticks < dtMax.Ticks)
                {
                    list.Add(DateTimeUtil.GetTimestamp(dtStart));
                    dtStart = dtStart.AddMonths(num);
                }
            }
            else if (range >= ONE_DAY * MIN_TIME_SPLIT_NUMBER)
            {
                var tick = GetTickSecond(range, splitNumber, ONE_DAY);
                var startTimestamp = (minTimestamp - minTimestamp % tick) + tick;
                AddTickTimestamp(list, startTimestamp, maxTimestamp, tick);
            }
            else if (range >= ONE_HOUR * MIN_TIME_SPLIT_NUMBER)
            {
                var tick = GetTickSecond(range, splitNumber, ONE_HOUR);
                var startTimestamp = (minTimestamp - minTimestamp % tick) + tick;
                AddTickTimestamp(list, startTimestamp, maxTimestamp, tick);
            }
            else if (range >= ONE_MINUTE * MIN_TIME_SPLIT_NUMBER)
            {
                var tick = GetTickSecond(range, splitNumber, ONE_MINUTE);
                var startTimestamp = (minTimestamp - minTimestamp % tick) + tick;
                AddTickTimestamp(list, startTimestamp, maxTimestamp, tick);
            }
            else
            {
                var tick = GetTickSecond(range, splitNumber, ONE_SECOND);
                var startTimestamp = (minTimestamp - minTimestamp % tick) + tick;
                AddTickTimestamp(list, startTimestamp, maxTimestamp, tick);
            }
        }

        private static int GetTickSecond(int range, int splitNumber, int tickSecond)
        {
            var num = 0;
            if (splitNumber > 0)
            {
                num = Math.Max(range / (splitNumber * tickSecond), 1);
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

        private static void AddTickTimestamp(List<double> list, int startTimestamp, int maxTimestamp, int tickSecond)
        {
            while (startTimestamp < maxTimestamp)
            {
                list.Add(startTimestamp);
                startTimestamp += tickSecond;
            }
        }
    }
}