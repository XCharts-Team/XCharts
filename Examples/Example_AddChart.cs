using UnityEngine;
using XCharts.Runtime;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif

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
            var title = chart.EnsureChartComponent<Title>();
            title.text = "Simple LineChart";
            title.subText = "normal line";

            var tooltip = chart.EnsureChartComponent<Tooltip>();
            tooltip.show = true;

            var legend = chart.EnsureChartComponent<Legend>();
            legend.show = false;

            var xAxis = chart.EnsureChartComponent<XAxis>();
            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;
            xAxis.type = Axis.AxisType.Category;

            var yAxis = chart.EnsureChartComponent<YAxis>();
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