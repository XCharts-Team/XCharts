/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace XCharts
{
    public static class ChartCached
    {
        private const string NUMERIC_FORMATTER_D = "D";
        private const string NUMERIC_FORMATTER_d = "d";
        private const string NUMERIC_FORMATTER_X = "X";
        private const string NUMERIC_FORMATTER_x = "x";
        private static CultureInfo ci = new CultureInfo("en-us");// "en-us", "zh-cn", "ar-iq", "de-de"
        private static Dictionary<Color, string> s_ColorToStr = new Dictionary<Color, string>(100);
        private static Dictionary<int, string> s_SerieLabelName = new Dictionary<int, string>(1000);
        private static Dictionary<int, string> s_AxisLabelName = new Dictionary<int, string>(1000);
        private static Dictionary<Color, string> s_ColorDotStr = new Dictionary<Color, string>(100);

        private static Dictionary<float, Dictionary<string, string>> s_NumberToStr = new Dictionary<float, Dictionary<string, string>>();
        private static Dictionary<int, Dictionary<string, string>> s_PrecisionToStr = new Dictionary<int, Dictionary<string, string>>();

        public static string FloatToStr(float value, string numericFormatter = "F", int precision = 0)
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

        public static string NumberToStr(float value, string formatter)
        {
            if (!s_NumberToStr.ContainsKey(value))
            {
                s_NumberToStr[value] = new Dictionary<string, string>();
            }
            if (!s_NumberToStr[value].ContainsKey(formatter))
            {
                if (string.IsNullOrEmpty(formatter))
                {
                    if (value - (int)value == 0)
                        s_NumberToStr[value][formatter] = ((int)value).ToString();
                    else
                        s_NumberToStr[value][formatter] = value.ToString();
                }
                else if (formatter.StartsWith(NUMERIC_FORMATTER_D)
                    || formatter.StartsWith(NUMERIC_FORMATTER_d)
                    || formatter.StartsWith(NUMERIC_FORMATTER_X)
                    || formatter.StartsWith(NUMERIC_FORMATTER_x)
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

        internal static string GetSerieLabelName(string prefix, int i, int j)
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

        internal static string GetAxisLabelName(string prefix, bool isYAxis, int axisIndex, int i)
        {
            int key = (isYAxis ? 2 : 1) * 1000000 + (axisIndex + 1) * 100000 + i;
            if (s_AxisLabelName.ContainsKey(key))
            {
                return s_AxisLabelName[key];
            }
            else
            {
                string name = prefix + "_" + axisIndex + "_" + i;
                s_AxisLabelName[key] = name;
                return name;
            }
        }
    }
}