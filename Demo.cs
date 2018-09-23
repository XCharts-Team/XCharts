using UnityEngine;
using xcharts;

public class Demo : MonoBehaviour
{
    private LineChart lineChart;
    private float time;
    private int count;

    void Awake()
    {
        lineChart = transform.Find("xchart/line_chart").GetComponent<LineChart>();
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;
            count++;
            lineChart.AddXAxisCategory("key" + count);
            lineChart.AddData("line1", "key"+count, Random.Range(24.0f, 60.0f));
            lineChart.AddData("line2", "key"+count, Random.Range(24.0f, 60.0f));
        }
    }
}
