using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example80_Polar : MonoBehaviour
    {
        private BaseChart chart;
        private float updateTime;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<BaseChart>();
            }
            chart.GetOrAddChartComponent<PolarCoord>();
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
            chart.GetChartComponent<Tooltip>().type = Tooltip.Type.Corss;
            var angleAxis = chart.GetChartComponent<AngleAxis>();
            angleAxis.type = Axis.AxisType.Value;
            angleAxis.minMaxType = Axis.AxisMinMaxType.Custom;
            angleAxis.min = 0;
            angleAxis.max = 360;
            angleAxis.startAngle = Random.Range(0, 90);
            chart.AddSerie<Line>("line1");

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