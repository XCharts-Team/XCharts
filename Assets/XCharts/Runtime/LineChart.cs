
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/LineChart", 13)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LineChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            title.text = "LineChart";
            tooltip.type = Tooltip.Type.Line;

            visualMap.enable = false;
            visualMap.show = false;
            visualMap.autoMinMax = true;
            visualMap.inRange.Clear();
            visualMap.inRange.Add(Color.blue);
            visualMap.inRange.Add(Color.red);

            RemoveData();
            SerieTemplate.AddDefaultLineSerie(this, "serie1");
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
            }
        }
#endif
    }
}
