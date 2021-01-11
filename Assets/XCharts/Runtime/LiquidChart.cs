/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    /// <summary>
    /// 水位图
    /// </summary>
    [AddComponentMenu("XCharts/LiquidChart", 22)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LiquidChart : BaseChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            title.text = "LiquidChart";
            RemoveData();
            RemoveVessel();
            AddVessel(Vessel.defaultVessel);
            SerieTemplate.AddDefaultLiquidSerie(this, "serie1");
        }
#endif
    }
}
