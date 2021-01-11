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
            chart.title.show = true;
            chart.title.text = "Line Simple";

            chart.tooltip.show = true;
            chart.legend.show = false;

            chart.xAxes[0].show = true;
            chart.xAxes[1].show = false;
            chart.yAxes[0].show = true;
            chart.yAxes[1].show = false;
            chart.xAxes[0].type = Axis.AxisType.Category;
            chart.yAxes[0].type = Axis.AxisType.Value;

            chart.xAxes[0].splitNumber = 10;
            chart.xAxes[0].boundaryGap = true;

            chart.RemoveData();
            chart.AddSerie(SerieType.Line);
            chart.AddSerie(SerieType.Line);
            for (int i = 0; i < 2000; i++)
            {
                chart.AddXAxisData("x" + i);
                chart.AddData(0, Random.Range(10, 20));
                chart.AddData(1, Random.Range(10, 20));
            }
        }
    }
}