using System;
using UnityEngine;

namespace XCharts.Runtime
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
        { }
    }
}