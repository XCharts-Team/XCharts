/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using System.Collections.Generic;
using System;

namespace XCharts
{
    public partial class BaseChart
    {
        public virtual void InitAxisRuntimeData(Axis axis)
        {
        }

        public virtual void GetSeriesMinMaxValue(Axis axis, int axisIndex, out double tempMinValue, out double tempMaxValue)
        {
            if (IsAllAxisValue())
            {
                if (axis is XAxis)
                {
                    SeriesHelper.GetXMinMaxValue(m_Series, null, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue);
                }
                else
                {
                    SeriesHelper.GetYMinMaxValue(m_Series, null, axisIndex, true, axis.inverse, out tempMinValue, out tempMaxValue);
                }
            }
            else
            {
                SeriesHelper.GetYMinMaxValue(m_Series, null, axisIndex, false, axis.inverse, out tempMinValue, out tempMaxValue);
            }
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
        }
    }
}
