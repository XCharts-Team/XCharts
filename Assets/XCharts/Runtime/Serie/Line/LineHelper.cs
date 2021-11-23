/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    internal static class LineHelper
    {
        public static int GetDataAverageRate(Serie serie, GridCoord grid, int maxCount, bool isYAxis)
        {
            var sampleDist = serie.sampleDist;
            var rate = 0;
            var width = isYAxis ? grid.context.height : grid.context.width;
            if (sampleDist > 0)
                rate = (int)((maxCount - serie.minShow) / (width / sampleDist));
            if (rate < 1)
                rate = 1;
            return rate;
        }

        public static Vector3 GetNNPos(List<Vector3> dataPoints, int index, Vector3 np, bool ignoreLineBreak)
        {
            int size = dataPoints.Count;
            if (index >= size)
                return np;
            for (int i = index + 1; i < size; i++)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak)
                    return dataPoints[i];
            }
            return np;
        }

        public static Vector3 GetStartPos(List<Vector3> dataPoints, ref int start, bool ignoreLineBreak)
        {
            for (int i = 0; i < dataPoints.Count; i++)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak)
                {
                    start = i;
                    return dataPoints[i];
                }
            }
            return Vector3.zero;
        }

        public static Vector3 GetEndPos(List<Vector3> dataPoints, ref int end, bool ignoreLineBreak)
        {
            for (int i = dataPoints.Count - 1; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak)
                {
                    end = i;
                    return dataPoints[i];
                }
            }
            return Vector3.zero;
        }

        public static Vector3 GetLastPos(List<Vector3> dataPoints, int index, Vector3 pos, bool ignoreLineBreak)
        {
            if (index <= 0)
                return pos;
            for (int i = index - 1; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak)
                    return dataPoints[i];
            }
            return pos;
        }

        public static Vector3 GetLLPos(List<Vector3> dataPoints, int index, Vector3 lp, bool ignoreLineBreak)
        {
            if (index <= 1)
                return lp;
            for (int i = index - 2; i >= 0; i--)
            {
                if (dataPoints[i] != Vector3.zero || ignoreLineBreak)
                    return dataPoints[i];
            }
            return lp;
        }

        public static bool IsInRightOrUp(bool isYAxis, Vector3 lp, Vector3 rp)
        {
            return ChartHelper.IsZeroVector(lp)
                || ((isYAxis && rp.y > lp.y)
                || (!isYAxis && rp.x > lp.x));
        }

        public static bool IsInRightOrUpNotCheckZero(bool isYAxis, Vector3 lp, Vector3 rp)
        {
            return (isYAxis && rp.y > lp.y) || (!isYAxis && rp.x > lp.x);
        }

        public static bool WasTooClose(bool isYAxis, Vector3 lp, Vector3 rp, bool ignore)
        {
            if (ignore)
                return false;

            if (lp == Vector3.zero || rp == Vector3.zero)
                return false;

            if (isYAxis)
                return Mathf.Abs(rp.y - lp.y) < 1f;
            else
                return Mathf.Abs(rp.x - lp.x) < 1f;
        }
    }
}