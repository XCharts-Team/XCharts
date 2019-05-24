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
        chart.xAxis.ClearData();
        chart.series.ClearData();
        chart.maxCacheDataNumber = maxCacheDataNumber;
        timeNow = timeNow.AddSeconds(-maxCacheDataNumber);
    }

    void Update()
    {
        if (initCount< maxCacheDataNumber)
        {
            int count = (int)(maxCacheDataNumber / initDataTime * Time.deltaTime);
            for(int i = 0; i < count; i++)
            {
                timeNow = timeNow.AddSeconds(1);
                chart.AddXAxisData(timeNow.ToString("hh:mm:ss"));
                chart.AddData(0, UnityEngine.Random.Range(60, 150));
                initCount++;
                if (initCount > maxCacheDataNumber) break;
            }
            chart.RefreshChart();
        }
    }

    void GenerateData(int count, CoordinateChart chart)
    {
        var baseValue = Random.Range(0, 1000);
        var time = new System.DateTime(2011, 1, 1);
        var smallBaseValue = 0;

        chart.xAxis.ClearData();
        for (var i = 0; i < count; i++)
        {
            chart.AddXAxisData(time.ToString("hh:mm:ss"));

            smallBaseValue = i % 30 == 0
                ? Random.Range(0, 700)
                : (smallBaseValue + Random.Range(0, 500) - 250);
            baseValue += Random.Range(0, 20) - 10;
            float value = Mathf.Max(
                0,
                Mathf.Round(baseValue + smallBaseValue) + 3000
            );
            //var index = i % 100;
            //var value = (Mathf.Sin(index / 5) * (index / 5 - 10) + index / 6) * 5;
            value = Mathf.Abs(value);
            chart.AddData(0, value);
            time = time.AddSeconds(1);
        }
    }
}
