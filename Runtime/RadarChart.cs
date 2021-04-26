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
    [AddComponentMenu("XCharts/RadarChart", 16)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class RadarChart : BaseChart
    {
        protected override void InitComponent()
        {
            base.InitComponent();
            if (m_Radars.Count == 0) m_Radars = new List<Radar>() { Radar.defaultRadar };
        }

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            RemoveData();
            m_Radars.Clear();
            title.text = "RadarChart";
            SerieTemplate.AddDefaultRadarSerie(this, "serie1");
        }
#endif
    }
}
