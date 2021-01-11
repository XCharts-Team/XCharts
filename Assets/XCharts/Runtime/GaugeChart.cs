using System;
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/GaugeChart", 19)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class GaugeChart : BaseChart
    {
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            RemoveData();
            title.text = "GuageChart";
            var serie = AddSerie(SerieType.Gauge, "serie1");
            serie.min = 0;
            serie.max = 100;
            serie.startAngle = -125;
            serie.endAngle = 125;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.5f;
            serie.radius[0] = 80;
            serie.splitNumber = 5;
            serie.animation.dataChangeEnable = true;
            serie.titleStyle.show = true;
            serie.titleStyle.textStyle.offset = new Vector2(0, 20);
            serie.label.show = true;
            serie.label.offset = new Vector3(0, -30);
            serie.itemStyle.show = true;
            serie.gaugeAxis.axisLabel.show = true;
            serie.gaugeAxis.axisLabel.margin = 18;
            AddData(0, UnityEngine.Random.Range(10, 90), "title");
        }
#endif
    }
}
