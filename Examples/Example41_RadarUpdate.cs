using UnityEngine;
using XCharts.Runtime;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example41_RadarUpdate : MonoBehaviour
    {
        RadarChart chart;
        int count = 0;
        double max = 0;

        void Awake()
        {
            chart = gameObject.GetComponent<RadarChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<RadarChart>();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                UpdateData();
                count++;
            }
            UpdateMax();
        }

        void UpdateData()
        {
            var serieIndex = 0;
            var serie = chart.GetSerie(serieIndex);
            if (serie == null) return;
            if (serie.radarType == RadarType.Multiple)
            {
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var serieData = serie.GetSerieData(i);
                    for (int j = 0; j < serieData.data.Count; j++)
                    {
                        var value = Random.Range(10, 100);
                        chart.UpdateData(serieIndex, i, j, value);
                    }
                }
            }
            else
            {
                for (int i = 0; i < serie.dataCount; i++)
                {
                    var value = Random.Range(10, 100);
                    chart.UpdateData(serieIndex, i, value);
                }
            }
            chart.GetChartComponent<Title>().subText = "max:" + serie.context.dataMax;
        }

        void UpdateMax()
        {
            var serieIndex = 0;
            var serie = chart.GetSerie(serieIndex);
            if (serie == null) return;
            if (serie.context.dataMax != max)
            {
                chart.GetChartComponent<Title>().subText = "max:" + serie.context.dataMax;
                max = serie.context.dataMax;
            }
        }
    }
}