using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using xchart;

public class Demo : MonoBehaviour
{
    private LineChart lineChart;
    private float time;

    void Awake () {
        lineChart = transform.Find("line_chart").GetComponent<LineChart>();
    }

	void Update () {
        time += Time.deltaTime;
        if (time >= 1)
        {
            time = 0;
            lineChart.AddPoint("fps", Random.Range(24.0f, 60.0f));
            lineChart.AddPoint("rtt", Random.Range(15, 30));
            lineChart.AddPoint("ping", Random.Range(0, 100));
        }
    }
}
