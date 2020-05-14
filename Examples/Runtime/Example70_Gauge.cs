using System.Runtime.InteropServices;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/


using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example70_Gauge : MonoBehaviour
    {
        private GaugeChart chart;
        private float updateTime;

        void Awake()
        {
            chart = gameObject.GetComponent<GaugeChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<GaugeChart>();
            }
            chart.title.text = "GaugeChart";
            chart.RemoveData();

            var serie = chart.AddSerie(SerieType.Gauge, "速度");
            serie.min = 0;
            serie.max = 220;
            serie.startAngle = -125;
            serie.endAngle = 125;
            serie.center[0] = 0.5f;
            serie.center[1] = 0.5f;
            serie.radius[0] = 80;
            serie.splitNumber = 5;
            serie.animation.dataChangeEnable = true;
            serie.roundCap = true;

            serie.titleStyle.show = true;
            serie.titleStyle.textStyle.offset = new Vector2(0, 20);

            serie.label.show = true;
            serie.label.offset = new Vector3(0, -20);

            serie.gaugeAxis.show = true;
            serie.gaugeAxis.axisLine.width = 15;

            serie.gaugePointer.show = true;
            serie.gaugePointer.width = 15;

            var value = UnityEngine.Random.Range(serie.min, serie.max);
            chart.AddData(0, value, "km/h");
        }

        void Update()
        {
            updateTime += Time.deltaTime;
            if (updateTime > 2)
            {
                updateTime = 0;
                var value = UnityEngine.Random.Range(0, 220);
                chart.UpdateData(0, 0, value);
            }
        }
    }
}