/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/


using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example80_Polar : MonoBehaviour
    {
        private PolarChart chart;
        private float updateTime;

        void Awake()
        {
            chart = gameObject.GetComponent<PolarChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<PolarChart>();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddData();
            }
        }

        void AddData()
        {
            chart.RemoveData();
            chart.tooltip.type = Tooltip.Type.Corss;
            chart.angleAxis.type = Axis.AxisType.Value;
            chart.angleAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            chart.angleAxis.min = 0;
            chart.angleAxis.max = 360;
            chart.angleAxis.startAngle = Random.Range(0, 90);
            chart.AddSerie(SerieType.Line, "line1");

            var rate = Random.Range(1, 4);
            for (int i = 0; i <= 360; i++)
            {
                var t = i / 180f * Mathf.PI;
                var r = Mathf.Sin(2 * t) * Mathf.Cos(2 * t) * rate;
                chart.AddData(0, Mathf.Abs(r), i);
            }
        }
    }
}