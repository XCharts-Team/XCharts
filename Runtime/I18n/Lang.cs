using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Language.
    /// |国际化语言表。
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "XCharts/Export Lang")]
    public class Lang : ScriptableObject
    {
        public string langName = "EN";
        public LangTime time = new LangTime();
        public LangCandlestick candlestick = new LangCandlestick();

        public string GetMonthAbbr(int month)
        {
            if (month < 1 && month > 12) return month.ToString();
            else return time.monthAbbr[month - 1];
        }

        public string GetDay(int day)
        {
            day = day - 1;
            if (day >= 0 && day < time.dayOfMonth.Count - 1)
                return time.dayOfMonth[day];
            else
                return day.ToString();
        }

        public string GetCandlestickDimensionName(int i)
        {
            if (i >= 0 && i < candlestick.dimensionNames.Count)
                return candlestick.dimensionNames[i];
            else
                return string.Empty;
        }
    }

    [Serializable]
    public class LangTime
    {
        public List<string> months = new List<string>()
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };
        public List<string> monthAbbr = new List<string>()
        {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"
        };
        public List<string> dayOfMonth = new List<string>()
        {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30",
            "31"
        };
        public List<string> dayOfWeek = new List<string>()
        {
            "Sunday",
            "Monday",
            "Tuesday",
            "Wednesday",
            "Thursday",
            "Friday",
            "Saturday"
        };
        public List<string> dayOfWeekAbbr = new List<string>()
        {
            "Sun",
            "Mon",
            "Tue",
            "Wed",
            "Thu",
            "Fri",
            "Sat"
        };
    }

    [Serializable]
    public class LangCandlestick
    {
        public List<string> dimensionNames = new List<string>() { "open", "close", "lowest", "highest" };
    }
}