using System.Collections;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example21_BarRace : MonoBehaviour
    {
        private BarChart chart;
        private float lastTime;

        void Awake()
        {
            chart = gameObject.GetComponent<BarChart>();
            chart.ClearData();
            for (int i = 0; i < 5; i++)
            {
                chart.AddYAxisData("y" + i);
                chart.AddData(0, Random.Range(0, 200));
            }
        }

        void Update()
        {
            if (Time.time - lastTime >= 3f)
            {
                lastTime = Time.time;
                UpdateData();
            }
        }

        void UpdateData()
        {
            var serie = chart.GetSerie(0);

            for (int i = 0; i < serie.dataCount; i++)
            {
                if (Random.Range(0, 1f) > 0.9f)
                    chart.UpdateData(0, i, chart.GetData(0, i) + Mathf.Round(Random.Range(0, 2000)));
                else
                    chart.UpdateData(0, i, chart.GetData(0, i) + Mathf.Round(Random.Range(0, 200)));
            }
        }
    }
}