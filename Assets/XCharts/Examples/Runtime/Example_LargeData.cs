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
    [RequireComponent(typeof(CoordinateChart))]
    public class Example_LargeData : MonoBehaviour
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
            chart.SetMaxCache(maxCacheDataNumber);
            chart.title.text = maxCacheDataNumber + "数据";
        }

        private void Update()
        {
            if (initCount < maxCacheDataNumber)
            {
                for (int i = 0; i < 10; i++)
                {
                    initCount++;
                    if (initCount > maxCacheDataNumber) break;
                    chart.title.text = initCount + "数据";
                    timeNow = timeNow.AddSeconds(1);
                    float xvalue = Mathf.PI / 180 * initCount;
                    float yvalue = Mathf.Sin(xvalue);

                    chart.AddData(0, 15 + yvalue * 2);
                    chart.AddXAxisData(timeNow.ToString("hh:mm:ss"));
                }
            }
        }
    }
}