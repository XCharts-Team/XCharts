using System;
using UnityEngine;
using XCharts;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(CoordinateChart))]
public class Demo_Dynamic : MonoBehaviour
{
    public int maxCacheDataNumber = 100;
    public float initDataTime = 2;

    private CoordinateChart chart;
    private float updateTime;
    private float initTime;
    private int initCount;
    private int count;
    private bool isInited;
    private DateTime timeNow;

    void Awake()
    {
        chart = gameObject.GetComponentInChildren<CoordinateChart>();
        chart.ClearAxisData();
        chart.series.ClearData();
        chart.maxCacheDataNumber = maxCacheDataNumber;
        timeNow = DateTime.Now;
        timeNow = timeNow.AddSeconds(-maxCacheDataNumber);
    }

    void Update()
    {
        if (initCount< maxCacheDataNumber)
        {
            int count = (int)(maxCacheDataNumber / initDataTime * Time.deltaTime);
            for (int i = 0; i < count; i++)
            {
                timeNow = timeNow.AddSeconds(1);
                chart.AddXAxisData(timeNow.ToString("hh:mm:ss"));
                chart.AddData(0, UnityEngine.Random.Range(60, 150));
                initCount++;
                if (initCount > maxCacheDataNumber) break;
            }
            chart.RefreshChart();
        }
        updateTime += Time.deltaTime;
        if (updateTime >= 1)
        {
            updateTime = 0;
            count++;
            chart.AddXAxisData(DateTime.Now.ToString("hh:mm:ss"));
            chart.AddData(0, UnityEngine.Random.Range(60, 150));
            chart.RefreshChart();
        }
    }
}
