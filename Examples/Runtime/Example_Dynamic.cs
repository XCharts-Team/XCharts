/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(CoordinateChart))]
    public class Example_Dynamic : MonoBehaviour
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
            chart.RemoveData();
            var serie = chart.AddSerie(SerieType.Line);
            serie.symbol.show = false;
            serie.maxCache = maxCacheDataNumber;
            chart.xAxises[0].maxCache = maxCacheDataNumber;
            timeNow = DateTime.Now;
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
                    string category = timeNow.ToString("hh:mm:ss");
                    float value = UnityEngine.Random.Range(60, 150);
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
                updateTime = 0;
                count++;
                string category = DateTime.Now.ToString("hh:mm:ss");
                float value = UnityEngine.Random.Range(60, 150);
                chart.AddXAxisData(category);
                chart.AddData(0, value);
                chart.RefreshChart();
            }
        }
    }
}