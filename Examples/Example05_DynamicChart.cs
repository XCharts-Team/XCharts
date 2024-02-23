using UnityEngine;
using XCharts.Runtime;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example05_DynamicChart : MonoBehaviour
    {
        BaseChart chart;

        void Awake() { }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                AddPieChart("Dynamic PieChart");
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                AddLineChart("Dynamic LineChart");
            }
        }

        GameObject CreateChartObject(string chartName)
        {
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            var chartObject = new GameObject();
            chartObject.name = chartName;
            chartObject.transform.SetParent(transform);
            chartObject.transform.localScale = Vector3.one;
            chartObject.transform.localPosition = Vector3.zero;
            return chartObject;
        }

        void AddPieChart(string chartName)
        {
            var chartObject = CreateChartObject(chartName);
            var chart = chartObject.AddComponent<PieChart>();
            chart.SetSize(580, 300);

            chart.EnsureChartComponent<Title>().show = true;
            chart.EnsureChartComponent<Title>().text = chartName;

            chart.EnsureChartComponent<Tooltip>().show = true;
            chart.EnsureChartComponent<Legend>().show = true;

            chart.RemoveData();
            chart.AddSerie<Pie>();

            for (int i = 0; i < 3; i++)
            {
                chart.AddData(0, Random.Range(10, 20), "pie" + (i + 1));
            }
        }

        void AddLineChart(string chartName)
        {
            var chartObject = CreateChartObject(chartName);
            var chart = chartObject.AddComponent<PieChart>();
            chart.SetSize(580, 300);

            chart.EnsureChartComponent<Title>().show = true;
            chart.EnsureChartComponent<Title>().text = chartName;

            chart.EnsureChartComponent<Legend>().show = false;

            var tooltip = chart.EnsureChartComponent<Tooltip>();
            tooltip.trigger = Tooltip.Trigger.Axis;

            var xAxis = chart.EnsureChartComponent<XAxis>();
            var yAxis = chart.EnsureChartComponent<YAxis>();
            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;
            xAxis.show = true;
            yAxis.show = true;
            xAxis.type = Axis.AxisType.Category;
            yAxis.type = Axis.AxisType.Value;

            chart.RemoveData();
            chart.AddSerie<Line>();

            for (int i = 0; i < 10; i++)
            {
                chart.AddXAxisData("x" + (i + 1));
                chart.AddData(0, Random.Range(10, 100));
            }
        }
    }
}