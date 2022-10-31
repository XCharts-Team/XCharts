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
            if (IsAllAxisValue())
            {
                if (axis is XAxis)
                {
                    SeriesHelper.GetXMinMaxValue(this, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue, false, false);
                }
                else
                {
                    SeriesHelper.GetYMinMaxValue(this, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue);
                }
            }
            else
            {
                SeriesHelper.GetYMinMaxValue(this, axisIndex, false, axis.inverse, out tempMinValue, out tempMaxValue);
            }
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
        }
    }
}