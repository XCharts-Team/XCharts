using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
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
                chart.Init();
            }
            chart.EnsureChartComponent<Title>().show = true;
            chart.EnsureChartComponent<Title>().text = "Sin Curve";

            chart.EnsureChartComponent<Tooltip>().show = true;
            chart.EnsureChartComponent<Legend>().show = false;

            var xAxis = chart.EnsureChartComponent<XAxis>();
            var yAxis = chart.EnsureChartComponent<YAxis>();

            xAxis.show = true;
            yAxis.show = true;

            xAxis.type = Axis.AxisType.Value;
            yAxis.type = Axis.AxisType.Value;

            xAxis.boundaryGap = false;
            xAxis.maxCache = 0;
            chart.series[0].maxCache = 0;

            chart.RemoveData();

            var serie = chart.AddSerie<Line>();
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