using System.Collections.Generic;
using UnityEngine;
using XCharts;

[DisallowMultipleComponent]
[ExecuteInEditMode]
public class Demo30_Pie : MonoBehaviour
{
    private PieChart chart;

    void Awake()
    {
        chart = gameObject.GetComponent<PieChart>();
        if (chart == null) return;
        
        var serie = chart.series.GetSerie(0);
        var serieData = serie.GetSerieData(0);
        serieData.radius = 100;
    }
}
