/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

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
        private static readonly string s_DefaultAxisY = "axis_y";
        private static readonly string s_DefaultAxisX = "axis_x";
        private static CultureInfo ci = new CultureInfo("en-us");// "en-us", "zh-cn", "ar-iq", "de-de"
        private static Dictionary<Color, string> s_ColorToStr = new Dictionary<Color, string>(100);
        private static Dictionary<int, string> s_SerieLabelName = new Dictionary<int, string>(1000);
        private static Dictionary<Color, string> s_ColorDotStr = new Dictionary<Color, string>(100);
        private static Dictionary<int, string> s_XAxisName = new Dictionary<int, string>();
        private static Dictionary<int, string> s_YAxisName = new Dictionary<int, string>();
        private static Dictionary<string, string> s_AxisLabel = new Dictionary<string, string>();


        private static Dictionary<double, Dictionary<string, string>> s_NumberToStr = new Dictionary<double, Dictionary<string, string>>();
        private static Dictionary<int, Dictionary<string, string>> s_PrecisionToStr = new Dictionary<int, Dictionary<string, string>>();

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

        internal static string GetXAxisName(int axisIndex, int index = -1)
        {
            if (index >= 0)
            {
                int key = (axisIndex + 1) * 10000 + index;
                if (!s_XAxisName.ContainsKey(key))
                {
                    s_XAxisName[key] = axisIndex > 0 ? s_DefaultAxisX + axisIndex + index : s_DefaultAxisX + index;
                }
                return s_XAxisName[key];
            }
            else if (!s_XAxisName.ContainsKey(axisIndex))
            {
                s_XAxisName[axisIndex] = axisIndex > 0 ? s_DefaultAxisX + axisIndex : s_DefaultAxisX;
            }
            return s_XAxisName[axisIndex];
        }

        internal static string GetYAxisName(int axisIndex, int index = -1)
        {
            if (index >= 0)
            {
                int key = (axisIndex + 1) * 10000 + index;
                if (!s_YAxisName.ContainsKey(key))
                {
                    s_YAxisName[key] = axisIndex > 0 ? s_DefaultAxisY + axisIndex + index : s_DefaultAxisY + index;
                }
                return s_YAxisName[key];
            }
            else if (!s_YAxisName.ContainsKey(axisIndex))
            {
                s_YAxisName[axisIndex] = axisIndex > 0 ? s_DefaultAxisY + axisIndex : s_DefaultAxisY;
            }
            return s_YAxisName[axisIndex];
        }

        internal static string GetAxisTooltipLabel(string axisName)
        {
            if (!s_AxisLabel.ContainsKey(axisName))
            {
                s_AxisLabel[axisName] = axisName + "_label";
            }
            return s_AxisLabel[axisName];
        }
    }
}