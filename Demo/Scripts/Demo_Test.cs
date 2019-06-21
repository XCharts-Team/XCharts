using UnityEngine;
using XCharts;

[DisallowMultipleComponent]
[ExecuteInEditMode]
//[RequireComponent(typeof(CoordinateChart))]
public class Demo_Test : MonoBehaviour
{
    private CoordinateChart chart;
    private float time;
    private int count;

    void Awake()
    {
        chart = gameObject.GetComponentInChildren<CoordinateChart>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;
            count++;
            chart.UpdateData(0, Random.Range(60, 150));
            chart.AddXAxisData("time" + count);
            chart.AddData(0, Random.Range(60, 150));
        }
    }
}
