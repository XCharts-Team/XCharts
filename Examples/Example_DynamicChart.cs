using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example_DynamicChart : MonoBehaviour
    {
        BaseChart chart;

        void Awake() { }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
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
            chart.Init();
            chart.SetSize(580, 300);

            chart.GetOrAddChartComponent<Title>().show = true;
            chart.GetOrAddChartComponent<Title>().text = chartName;

            chart.GetOrAddChartComponent<Tooltip>().show = true;
            chart.GetOrAddChartComponent<Legend>().show = true;

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
            chart.Init();
            chart.SetSize(580, 300);

            chart.GetOrAddChartComponent<Title>().show = true;
            chart.GetOrAddChartComponent<Title>().text = chartName;

            chart.GetOrAddChartComponent<Legend>().show = false;

            var tooltip = chart.GetOrAddChartComponent<Tooltip>();
            tooltip.trigger = Tooltip.Trigger.Axis;

            var xAxis = chart.GetOrAddChartComponent<XAxis>();
            var yAxis = chart.GetOrAddChartComponent<YAxis>();
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