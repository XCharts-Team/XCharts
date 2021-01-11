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
    public class Example11_AddSinCurve : MonoBehaviour
    {
        private float time;
        public int angle;
        private LineChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<LineChart>();
            }
            chart.title.show = true;
            chart.title.text = "Sin Curve";

            chart.tooltip.show = true;
            chart.legend.show = false;

            chart.xAxes[0].show = true;
            chart.xAxes[1].show = false;
            chart.yAxes[0].show = true;
            chart.yAxes[1].show = false;

            chart.xAxes[0].type = Axis.AxisType.Value;
            chart.yAxes[0].type = Axis.AxisType.Value;

            chart.xAxes[0].boundaryGap = false;
            chart.xAxes[0].maxCache = 0;
            chart.series.list[0].maxCache = 0;

            chart.RemoveData();

            var serie = chart.AddSerie(SerieType.Line);
            serie.symbol.show = false;
            serie.lineType = LineType.Normal;
            for (angle = 0; angle < 1080; angle++)
            {
                float xvalue = Mathf.PI / 180 * angle;
                float yvalue = Mathf.Sin(xvalue);
                chart.AddData(0, xvalue, yvalue);
            }
        }

        void Update()
        {
            if (angle > 3000) return;
            angle++;
            float xvalue = Mathf.PI / 180 * angle;
            float yvalue = Mathf.Sin(xvalue);
            chart.AddData(0, xvalue, yvalue);
        }
    }
}