using System;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class MathUtil
    {
        // pre-compute the minimum threshold to avoid repeated Mathf.Epsilon * 8 calculation
        private static readonly double k_MinApproxThreshold = Mathf.Epsilon * 8;

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
            var diff = b - a;
            if (diff < 0) diff = -diff;
            var absA = a < 0 ? -a : a;
            var absB = b < 0 ? -b : b;
            var maxAbs = absA > absB ? absA : absB;
            var threshold = 0.000001f * maxAbs;
            if (threshold < k_MinApproxThreshold) threshold = k_MinApproxThreshold;
            return diff < threshold;
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

        public static bool IsInteger(double value)
        {
            if (value == 0) return true;
            if (value >= -1 && value <= 1) return false;
            return Math.Abs(value % 1) <= (Double.Epsilon * 100);
        }

        public static int GetPrecision(double value)
        {
            if (IsInteger(value)) return 0;
            int count = 1;
            double intvalue = value * Mathf.Pow(10, count);
            while (!IsInteger(intvalue) && count < 38)
            {
                count++;
                intvalue = value * Mathf.Pow(10, count);
            }
            return count;
        }
    }
}