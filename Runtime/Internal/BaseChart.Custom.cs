using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public partial class BaseChart
    {
        public virtual void InitAxisRuntimeData(Axis axis) { }

        public virtual void GetSeriesMinMaxValue(Axis axis, int axisIndex, out double tempMinValue, out double tempMaxValue)
        {
            var needAnimationData = !axis.context.needAnimation;
            if (axis is XAxis3D)
            {
                SeriesHelper.GetXMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
            }
            else if (axis is ZAxis3D)
            {
                SeriesHelper.GetZMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
            }
            else if (axis is YAxis3D)
            {
                SeriesHelper.GetYMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
            }
            else if (IsAllAxisValue())
            {
                if (axis is XAxis)
                {
                    SeriesHelper.GetXMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
                }
                else
                {
                    SeriesHelper.GetYMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
                }
            }
            else
            {
                SeriesHelper.GetYMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
            }
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
        }
    }
}