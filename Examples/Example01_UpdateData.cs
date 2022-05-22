using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example01_UpdateData : MonoBehaviour
    {
        private float updateTime = 0;
        BaseChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
        }

        void Update()
        {
            updateTime += Time.deltaTime;
            if (chart && updateTime > 2)
            {
                updateTime = 0;
                var serie = chart.GetSerie(0);
                //serie.animation.dataChangeEnable = true;
                var dataCount = serie.dataCount;
                if (chart is RadarChart)
                {
                    var dimension = serie.GetSerieData(0).data.Count - 1;
                    chart.UpdateData(0, 0, Random.Range(0, dimension + 1), Random.Range(0, 100));
                }
                else if (chart is HeatmapChart)
                {
                    var dimension = serie.GetSerieData(0).data.Count - 1;
                    for (int i = 0; i < dataCount; i++)
                    {
                        chart.UpdateData(0, i, dimension, Random.Range(0, 10));
                    }
                }
                else
                {
                    chart.UpdateData(0, Random.Range(0, dataCount), Random.Range(10, 90));
                }
            }
        }
    }
}