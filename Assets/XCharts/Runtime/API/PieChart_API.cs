/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace XCharts
{
    public partial class PieChart
    {
        /// <summary>
        /// the callback function of click pie area.
        /// 点击饼图区域回调。参数：PointerEventData，SerieIndex，SerieDataIndex
        /// </summary>
        public Action<PointerEventData, int, int> onPointerClickPie { set { m_OnPointerClickPie = value; m_ForceOpenRaycastTarget = true; } }
    }
}