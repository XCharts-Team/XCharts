using UnityEngine;
using XCharts;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class Demo10_LineSimple : MonoBehaviour
{
    //void Awake()
    void Start()
    {
        var chart = gameObject.GetComponent<LineChart>();
        if (chart == null) return;

        chart.title.show = true;
        chart.title.text = "Line Simple";

        chart.tooltip.show = true;
        chart.legend.show = false;

        chart.xAxises[0].show = true;
        chart.xAxises[1].show = false;
        chart.yAxises[0].show = true;
        chart.yAxises[1].show = false;

        chart.xAxises[0].type = Axis.AxisType.Category;
        chart.yAxises[0].type = Axis.AxisType.Value;

        int dataCount = 10;
        chart.xAxises[0].splitNumber = dataCount;
        chart.xAxises[0].boundaryGap = true;

        chart.RemoveData();
        chart.AddSerie("test", SerieType.Line);
        for (int i = 0; i < dataCount; i++)
        {
            chart.AddXAxisData("x" + i);
            chart.AddData(0, Random.Range(10, 20));
        }
    }
}
