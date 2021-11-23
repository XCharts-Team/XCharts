/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XCharts
{
    public interface IUpdateRuntimeData
    {
        void UpdateRuntimeData(float chartX, float chartY, float chartWidth, float chartHeight);
    }
}