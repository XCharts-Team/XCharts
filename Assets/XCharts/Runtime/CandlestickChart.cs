
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/CandlestickChart", 23)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class CandlestickChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            title.text = "CandlestickChart";
            tooltip.type = Tooltip.Type.Corss;

            RemoveData();
            var defaultDataCount = SerieTemplate.AddDefaultCandlestickSerie(this, "serie1");
            for (int i = 0; i < defaultDataCount; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
#endif
    }
}
