using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class ChartCached
    {
        private const string NUMERIC_FORMATTER_D = "D";
        private const string NUMERIC_FORMATTER_d = "d";
        private const string NUMERIC_FORMATTER_X = "X";
        private const string NUMERIC_FORMATTER_x = "x";
        private static readonly string s_DefaultAxis = "axis_";
        private static CultureInfo ci = GetDefaultCultureInfo(); // "en-us", "zh-cn", "ar-iq", "de-de"
        private static Dictionary<Color, string> s_ColorToStr = new Dictionary<Color, string>(100);
        private static Dictionary<int, string> s_SerieLabelName = new Dictionary<int, string>(1000);
        private static Dictionary<Color, string> s_ColorDotStr = new Dictionary<Color, string>(100);
        private static Dictionary<Type, Dictionary<int, string>> s_ComponentObjectName = new Dictionary<Type, Dictionary<int, string>>();
        private static Dictionary<int, string> s_AxisLabelName = new Dictionary<int, string>();
        private static Dictionary<Type, string> s_TypeName = new Dictionary<Type, string>();

        private static Dictionary<double, Dictionary<string, string>> s_NumberToStr = new Dictionary<double, Dictionary<string, string>>();
        private static Dictionary<int, Dictionary<string, string>> s_PrecisionToStr = new Dictionary<int, Dictionary<string, string>>();
        private static Dictionary<string, Dictionary<int, string>> s_StringIntDict = new Dictionary<string, Dictionary<int, string>>();
        private static Dictionary<double, DateTime> s_TimestampToDateTimeDict = new Dictionary<double, DateTime>();
        private static Dictionary<double, TimeSpan> s_NumberToTimeSpanDict = new Dictionary<double, TimeSpan>();

        private static CultureInfo GetDefaultCultureInfo()
        {
            try
            {
                return new CultureInfo("en-us");
            }
            catch (Exception)
            {
                return CultureInfo.InvariantCulture;
            }
        }

        public static string FloatToStr(double value, string numericFormatter = "F", int precision = 0)
        {
            if (precision > 0 && numericFormatter.Length == 1)
            {
                if (!s_PrecisionToStr.ContainsKey(precision))
                {
                    s_PrecisionToStr[precision] = new Dictionary<string, string>();
                }
                if (!s_PrecisionToStr[precision].ContainsKey(numericFormatter))
                {
                    s_PrecisionToStr[precision][numericFormatter] = numericFormatter + precision;
                }
                return NumberToStr(value, s_PrecisionToStr[precision][numericFormatter]);
            }
            else
            {
                return NumberToStr(value, numericFormatter);
            }
        }

        public static string NumberToStr(double value, string formatter)
        {
            if (!s_NumberToStr.ContainsKey(value))
            {
                s_NumberToStr[value] = new Dictionary<string, string>();
            }
            if (!s_NumberToStr[value].ContainsKey(formatter))
            {
                bool isDateFormatter = false;
                string newFormatter = null;
                if (string.IsNullOrEmpty(formatter))
                {
                    s_NumberToStr[value][formatter] = value.ToString();
                }
                else if (DateTimeUtil.IsDateOrTimeRegex(formatter,ref isDateFormatter, ref newFormatter))
                {
                    if(isDateFormatter)
                        s_NumberToStr[value][formatter] = NumberToDateStr(value, newFormatter);
                    else
                        s_NumberToStr[value][formatter] = NumberToTimeStr(value, newFormatter);                    
                }
                else if (formatter.StartsWith(NUMERIC_FORMATTER_D) ||
                    formatter.StartsWith(NUMERIC_FORMATTER_d) ||
                    formatter.StartsWith(NUMERIC_FORMATTER_X) ||
                    formatter.StartsWith(NUMERIC_FORMATTER_x)
                )
                {
                    s_NumberToStr[value][formatter] = ((int)value).ToString(formatter, ci);
                }
                else
                {
                    s_NumberToStr[value][formatter] = value.ToString(formatter, ci);
                }
            }
            return s_NumberToStr[value][formatter];
        }

        public static string IntToStr(int value, string numericFormatter = "")
        {
            return NumberToStr(value, numericFormatter);
        }

        public static string NumberToDateStr(double timestamp, string formatter)
        {
            var dt = NumberToDateTime(timestamp);
            try
            {
                return dt.ToString(formatter, ci);
            }
            catch (Exception)
            {
                XLog.LogError("Not support DateTime format: " + formatter);
                return timestamp.ToString();
            }
        }

        public static string NumberToTimeStr(double timestamp, string formatter)
        {
            try
            {
            var ts = NumberToTimeSpan(timestamp);
#if UNITY_2018_3_OR_NEWER
                return ts.ToString(formatter, ci);
#else
                return ts.ToString();
#endif
            }
            catch (Exception)
            {
                XLog.LogError("Not support TimeSpan format: " + formatter);
                return timestamp.ToString();
            }                    
        }

        public static DateTime NumberToDateTime(double timestamp)
        {
            if (!s_TimestampToDateTimeDict.ContainsKey(timestamp))
            {
                s_TimestampToDateTimeDict[timestamp] = DateTimeUtil.GetDateTime(timestamp);
            }
            return s_TimestampToDateTimeDict[timestamp];
        }

        public static TimeSpan NumberToTimeSpan(double timestamp)
        {
            if(!s_NumberToTimeSpanDict.ContainsKey(timestamp))
            {
                s_NumberToTimeSpanDict[timestamp] = TimeSpan.FromSeconds(timestamp);
            }
            return s_NumberToTimeSpanDict[timestamp];
        }

        public static string ColorToStr(Color color)
        {
            if (s_ColorToStr.ContainsKey(color))
            {
                return s_ColorToStr[color];
            }
            else
            {
                s_ColorToStr[color] = ColorUtility.ToHtmlStringRGBA(color);
                return s_ColorToStr[color];
            }
        }

        public static string ColorToDotStr(Color color)
        {
            if (!s_ColorDotStr.ContainsKey(color))
            {
                s_ColorDotStr[color] = "<color=#" + ColorToStr(color) + ">● </color>";
            }
            return s_ColorDotStr[color];
        }

        public static string GetSerieLabelName(string prefix, int i, int j)
        {
            int key = i * 10000000 + j;
            if (s_SerieLabelName.ContainsKey(key))
            {
                return s_SerieLabelName[key];
            }
            else
            {
                string name = prefix + "_" + i + "_" + j;
                s_SerieLabelName[key] = name;
                return name;
            }
        }

        public static string GetString(string prefix, int suffix)
        {
            if (!s_StringIntDict.ContainsKey(prefix))
            {
                s_StringIntDict[prefix] = new Dictionary<int, string>();
            }
            if (!s_StringIntDict[prefix].ContainsKey(suffix))
            {
                s_StringIntDict[prefix][suffix] = prefix + suffix;
            }
            return s_StringIntDict[prefix][suffix];
        }

        internal static string GetComponentObjectName(MainComponent component)
        {
            Dictionary<int, string> dict;
            var type = component.GetType();
            if (s_ComponentObjectName.TryGetValue(type, out dict))
            {
                string name;
                if (!dict.TryGetValue(component.index, out name))
                {
                    name = GetTypeName(type) + component.index;
                    dict[component.index] = name;
                }
                return name;
            }
            else
            {
                var name = GetTypeName(type) + component.index;
                dict = new Dictionary<int, string>();
                dict.Add(component.index, name);
                s_ComponentObjectName[type] = dict;
                return name;
            }
        }

        internal static string GetAxisLabelName(int index)
        {
            string name;
            if (!s_AxisLabelName.TryGetValue(index, out name))
            {
                name = s_DefaultAxis + index;
                s_AxisLabelName[index] = name;
                return name;
            }
            else
            {
                return name;
            }
        }

        internal static string GetTypeName<T>()
        {
            return GetTypeName(typeof(T));
        }

        internal static string GetTypeName(Type type)
        {
            if (s_TypeName.ContainsKey(type)) return s_TypeName[type];
            else
            {
                var name = type.Name;
                s_TypeName[type] = name;
                return name;
            }
        }
    }
}