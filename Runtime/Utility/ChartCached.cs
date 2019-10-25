/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public static class ChartCached
    {
        private static Dictionary<float, string> s_ValueToF1Str = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToE1Str = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToF2Str = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToE2Str = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToFnStr = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToEnStr = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToFStr = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToEStr = new Dictionary<float, string>(1000);
        private static Dictionary<int, string> s_IntToStr = new Dictionary<int, string>(1000);
        private static Dictionary<int, string> s_IntToFn = new Dictionary<int, string>(20);
        private static Dictionary<Color, string> s_ColorToStr = new Dictionary<Color, string>(1000);

        public static string FloatToStr(float value, int f = 0, bool forceE = false)
        {
            Dictionary<float, string> valueDic;
            if (f == 0) valueDic = forceE ? s_ValueToEStr : s_ValueToFStr;
            if (f == 1) valueDic = forceE ? s_ValueToE1Str : s_ValueToF1Str;
            else if (f == 2) valueDic = forceE ? s_ValueToE2Str : s_ValueToF2Str;
            else valueDic = forceE ? s_ValueToEnStr : s_ValueToFnStr;
            if (valueDic.ContainsKey(value))
            {
                return valueDic[value];
            }
            else
            {
                if (f == 0) valueDic[value] = forceE ? value.ToString("E") : value.ToString();
                else if (f == 1) valueDic[value] = value.ToString("f1");
                else if (f == 2) valueDic[value] = value.ToString("f2");
                else valueDic[value] = (f > 3 || forceE) ? value.ToString("E0") : value.ToString(GetFn(f));
                return valueDic[value];
            }
        }

        private static string GetFn(int f)
        {
            if (s_IntToFn.ContainsKey(f))
            {
                return s_IntToFn[f];
            }
            else
            {
                s_IntToFn[f] = "f" + f;
                return s_IntToFn[f];
            }
        }

        public static string IntToStr(int value)
        {
            if (s_IntToStr.ContainsKey(value))
            {
                return s_IntToStr[value];
            }
            else
            {
                s_IntToStr[value] = value.ToString();
                return s_IntToStr[value];
            }
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
    }
}