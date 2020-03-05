/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Demo_Test : MonoBehaviour
    {
        private float updateTime = 0;
        BaseChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
            var btnTrans = transform.parent.Find("Button");
            if (btnTrans)
            {
                btnTrans.gameObject.GetComponent<Button>().onClick.AddListener(OnTestBtn);
            }
        }

        void Update()
        {
            // updateTime += Time.deltaTime;
            // if (chart && updateTime > 2)
            // {
            //     updateTime = 0;
            //     var serie = chart.series.GetSerie(0);
            //     serie.animation.dataChangeEnable = true;
            //     var dataCount = serie.dataCount;
            //     if (chart is HeatmapChart)
            //     {
            //         var dimension = serie.GetSerieData(0).data.Count - 1;
            //         for (int i = 0; i < dataCount; i++)
            //         {
            //             chart.UpdateData(0, i, dimension, Random.Range(0, 10));
            //         }
            //     }
            //     else
            //     {
            //         chart.UpdateData(0, Random.Range(0, dataCount), Random.Range(10, 90));
            //     }
            // }
        }

        void OnTestBtn()
        {
            //chart.series.list[0].lineStyle.width =UnityEngine.Random.Range(1, 5);
            chart.series.list[0].lineStyle.width =UnityEngine.Random.Range(1, 5);
        }
    }
}