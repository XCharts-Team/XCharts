
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using XCharts;
using System;
using System.Collections.Generic;

namespace XChartsDemo
{
    [DisallowMultipleComponent]
    //[ExecuteInEditMode]
    public class Demo_Line12 : MonoBehaviour
    {
        public int dataCount = 20000;
        void Awake()
        {
            // var chart = GetComponent<BaseChart>();
            // if (chart == null) return;
            // chart.RemoveData();
            // var serie = chart.AddSerie<Line>("Fake Data");
            // serie.sampleDist = 2f;
            // serie.symbol.show = false;
            // serie.itemStyle.color = new Color32(255, 70, 131, 255);
            // serie.areaStyle.show = true;
            // serie.areaStyle.color = new Color32(255, 158, 68, 255);
            // serie.areaStyle.toColor = new Color32(255, 70, 131, 255);

            // var baseDate = new DateTime(1968, 9, 3);
            // var data = new List<double>();
            // data.Add(UnityEngine.Random.Range(0f, 1f) * 300);
            // for (int i = 1; i < dataCount; i++)
            // {
            //     baseDate = baseDate.AddHours(24);
            //     chart.AddXAxisData(baseDate.ToString("yyyy/MM/dd"));
            //     double value = (int)((UnityEngine.Random.Range(0f, 1f) - 0.5f) * 20 + data[i - 1]);
            //     data.Add(value);
            //     chart.AddData(0, value);
            // }
            // chart.RefreshChart();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
            }
        }
    }
}