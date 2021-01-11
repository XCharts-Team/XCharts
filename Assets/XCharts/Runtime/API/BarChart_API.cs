/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace XCharts
{
    public partial class BarChart
    {
        /// <summary>
        /// the callback function of click bar. 
        /// 点击柱形图柱条回调。参数：eventData, dataIndex
        /// </summary>
        public Action<PointerEventData, int> onPointerClickBar { set { m_OnPointerClickBar = value; m_ForceOpenRaycastTarget = true; } }
    }
}