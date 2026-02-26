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
            bool isX = false, isY = false, isZ = false;
            tempMinValue = 0;
            tempMaxValue = 0;
            if (axis is XAxis3D)
                isX = true;
            else if (axis is ZAxis3D)
            {
                isZ = true;
            }
            else if (axis is YAxis3D)
            {
                isY = true;
            }
            else if (IsAllAxisValue())
            {
                var mainAxis = GetMainAxis();
                if (mainAxis == null)
                {
                    if (axis is XAxis)
                    {
                        isX = true;
                    }
                    else
                    {
                        isY = true;
                    }
                }
                else
                {
                    if (axis == mainAxis)
                    {
                        isX = true;
                    }
                    else
                    {
                        isY = true;
                    }
                }
            }
            else
            {
                isY = true;
            }
            if (isX)
            {
                SeriesHelper.GetXMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
            }
            else if (isY)
            {
                SeriesHelper.GetYMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
            }
            else if(isZ)
            {
                SeriesHelper.GetZMinMaxValue(this, axisIndex, axis.inverse, out tempMinValue, out tempMaxValue, false, false, needAnimationData);
            }
            AxisHelper.AdjustMinMaxValue(axis, ref tempMinValue, ref tempMaxValue, true);
        }
    }
}