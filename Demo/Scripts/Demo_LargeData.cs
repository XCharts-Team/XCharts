using UnityEngine;
using XCharts;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(CoordinateChart))]
public class Demo_LargeData : MonoBehaviour
{
    public int maxCacheDataNumber = 3000;
    public float initDataTime = 5;

    private CoordinateChart chart;
    private float initTime;
    private int initCount = 0;
    private System.DateTime timeNow;

    void Awake()
    {
        chart = gameObject.GetComponentInChildren<CoordinateChart>();
        timeNow = System.DateTime.Now;
        chart.ClearAxisData();
        chart.series.ClearData();
        chart.maxCacheDataNumber = maxCacheDataNumber;
        timeNow = timeNow.AddSeconds(-maxCacheDataNumber);
    }

    void Update()
    {
        if (initCount < maxCacheDataNumber)
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
        }
    }
}
