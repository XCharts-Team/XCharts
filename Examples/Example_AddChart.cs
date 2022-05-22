using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    //[ExecuteInEditMode]
    public class Example_AddChart : MonoBehaviour
    {
        BaseChart chart;
        void Awake()
        {
            //AddChart();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddChart();
            }
        }

        void AddChart()
        {
            chart = gameObject.GetComponent<BaseChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<LineChart>();
                chart.Init();
                chart.SetSize(1200, 600);
            }
            var title = chart.GetOrAddChartComponent<Title>();
            title.text = "Simple LineChart";
            title.subText = "normal line";

            var tooltip = chart.GetOrAddChartComponent<Tooltip>();
            tooltip.show = true;

            var legend = chart.GetOrAddChartComponent<Legend>();
            legend.show = false;

            var xAxis = chart.GetOrAddChartComponent<XAxis>();
            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;
            xAxis.type = Axis.AxisType.Category;

            var yAxis = chart.GetOrAddChartComponent<YAxis>();
            yAxis.type = Axis.AxisType.Value;

            chart.RemoveData();
            chart.AddSerie<Line>("line");

            for (int i = 0; i < 5; i++)
            {
                chart.AddXAxisData("x" + i);
                chart.AddData(0, Random.Range(10, 20));
            }
        }

        void ModifyComponent()
        {

        }
    }
}