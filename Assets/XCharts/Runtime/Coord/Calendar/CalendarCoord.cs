/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;

namespace XCharts
{
    [Serializable]
    [ComponentHandler(typeof(CalendarCoordHandler), true)]
    public class CalendarCoord : CoordSystem, IUpdateRuntimeData, ISerieContainer
    {
        public bool IsPointerEnter()
        {
            return false;
        }

        public void UpdateRuntimeData(float chartX, float chartY, float chartWidth, float chartHeight)
        {
        }
    }
}