/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Text;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    public static class MathUtil
    {
        public static double Abs(double d)
        {
            return d > 0 ? d : -d;
        }

        public static double Clamp(double d, double min, double max)
        {
            if (d >= min && d <= max) return d;
            else if (d < min) return min;
            else return max;
        }

        public static bool Approximately(double a, double b)
        {
            return Math.Abs(b - a) < Math.Max(0.000001f * Math.Max(Math.Abs(a), Math.Abs(b)), Mathf.Epsilon * 8);
        }

        public static double Clamp01(double value)
        {
            if (value < 0F)
                return 0F;
            else if (value > 1F)
                return 1F;
            else
                return value;
        }

        public static double Lerp(double a, double b, double t)
        {
            return a + (b - a) * Clamp01(t);
        }
    }
}