using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(BaseChart))]
    public class Example_LargeData : MonoBehaviour
    {
        public int maxCacheDataNumber = 1000;
        public float initDataTime = 5;

        private BaseChart chart;
        private float initTime;
        private int initCount = 0;
        private System.DateTime timeNow;

        void Awake()
        {
            chart = gameObject.GetComponentInChildren<BaseChart>();
            timeNow = System.DateTime.Now;
            chart.ClearData();
            chart.SetMaxCache(maxCacheDataNumber);
            chart.GetChartComponent<Title>().text = maxCacheDataNumber + "数据";
        }

        private double lastValue = 0d;

        private void Update()
        {
            if (initCount < maxCacheDataNumber)
            {
                for (int i = 0; i < 20; i++)
                {
                    initCount++;
                    if (initCount > maxCacheDataNumber) break;
                    chart.GetChartComponent<Title>().text = initCount + "数据";

                    timeNow = timeNow.AddSeconds(1);
                    if (lastValue < 20)
                        lastValue += UnityEngine.Random.Range(0, 5);
                    else
                        lastValue += UnityEngine.Random.Range(-5f, 5f);
                    chart.AddData(0, lastValue);

                    chart.AddXAxisData(timeNow.ToString("hh:mm:ss"));
                }
            }
        }
    }
}