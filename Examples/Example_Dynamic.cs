using System;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    //[ExecuteInEditMode]
    [RequireComponent(typeof(BaseChart))]
    public class Example_Dynamic : MonoBehaviour
    {
        public int maxCacheDataNumber = 100;
        public float initDataTime = 2;
        public bool insertDataToHead = false;

        private BaseChart chart;
        private float updateTime;
        private float initTime;
        private int initCount;
        private int count;
        private bool isInited;
        private DateTime timeNow;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
            var serie = chart.GetSerie(0);
            serie.symbol.show = false;
            serie.maxCache = maxCacheDataNumber;

            var xAxis = chart.EnsureChartComponent<XAxis>();
            xAxis.maxCache = maxCacheDataNumber;
            timeNow = DateTime.Now;
            timeNow = timeNow.AddSeconds(-maxCacheDataNumber);

            serie.insertDataToHead = insertDataToHead;
            xAxis.insertDataToHead = insertDataToHead;
        }

        void Update()
        {
            if (initCount < maxCacheDataNumber)
            {
                int count = (int) (maxCacheDataNumber / initDataTime * Time.deltaTime);
                for (int i = 0; i < count; i++)
                {
                    timeNow = timeNow.AddSeconds(1);
                    var category = timeNow.ToString("hh:mm:ss");
                    var value = UnityEngine.Random.Range(60, 150);
                    chart.AddXAxisData(category);
                    chart.AddData(0, value);
                    initCount++;
                    if (initCount > maxCacheDataNumber) break;
                }
                chart.RefreshChart();
            }
            updateTime += Time.deltaTime;
            if (updateTime >= 1)
            {
                var category = DateTime.Now.ToString("hh:mm:ss");
                var value = UnityEngine.Random.Range(60, 150);
                updateTime = 0;
                count++;
                chart.AddXAxisData(category);
                chart.AddData(0, value);
                chart.RefreshChart();
            }
        }
    }
}