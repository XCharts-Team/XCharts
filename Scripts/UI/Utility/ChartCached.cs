using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public static class ChartCached
    {
        private static Dictionary<float, string> s_ValueToF1Str = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToF2Str = new Dictionary<float, string>(1000);
        private static Dictionary<float, string> s_ValueToStr = new Dictionary<float, string>(1000);
        private static Dictionary<int, string> s_IntToStr = new Dictionary<int, string>(1000);

        public static string FloatToStr(float value, int f = 0)
        {
            Dictionary<float, string> valueDic;
            if (f == 1) valueDic = s_ValueToF1Str;
            else if (f == 2) valueDic = s_ValueToF2Str;
            else valueDic = s_ValueToStr;
            if (valueDic.ContainsKey(value))
            {
                return valueDic[value];
            }
            else
            {
                if (f == 1) valueDic[value] = value.ToString("f1");
                else if (f == 2) valueDic[value] = value.ToString("f2");
                else valueDic[value] = value.ToString();
                return valueDic[value];
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
    }
}