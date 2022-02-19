
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example13_LineSimple : MonoBehaviour
    {
        void Awake()
        {
            var chart = gameObject.GetComponent<LineChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<LineChart>();
                chart.SetSize(580, 300);//代码动态添加图表需要设置尺寸，或直接操作chart.rectTransform
            }
            chart.GetChartComponent<Title>().show = true;
            chart.GetChartComponent<Title>().text = "Line Simple";

            chart.GetChartComponent<Tooltip>().show = true;
            chart.GetChartComponent<Legend>().show = false;

            var xAxis = chart.GetChartComponent<XAxis>();
            var yAxis = chart.GetChartComponent<YAxis>();
            xAxis.show = true;
            yAxis.show = true;
            xAxis.type = Axis.AxisType.Category;
            yAxis.type = Axis.AxisType.Value;

            xAxis.splitNumber = 10;
            xAxis.boundaryGap = true;

            chart.RemoveData();
            chart.AddSerie<Line>();
            chart.AddSerie<Line>();
            for (int i = 0; i < 2000; i++)
            {
                chart.AddXAxisData("x" + i);
                chart.AddData(0, Random.Range(10, 20));
                chart.AddData(1, Random.Range(10, 20));
            }
        }
    }
}