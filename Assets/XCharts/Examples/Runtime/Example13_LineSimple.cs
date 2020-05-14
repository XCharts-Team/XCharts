/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
            }
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

            chart.xAxises[0].splitNumber = 10;
            chart.xAxises[0].boundaryGap = true;

            chart.RemoveData();
            chart.AddSerie(SerieType.Line);
            chart.AddSerie(SerieType.Line);
            for (int i = 0; i < 10; i++)
            {
                chart.AddXAxisData("x" + i);
                chart.AddData(0, Random.Range(10, 20));
                chart.AddData(1, Random.Range(10, 20));
            }
        }
    }
}