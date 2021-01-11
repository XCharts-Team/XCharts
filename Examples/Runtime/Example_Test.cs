/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example_Test : MonoBehaviour
    {
        LineChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<LineChart>();
            var btnTrans = transform.parent.Find("Button");
            if (btnTrans)
            {
                btnTrans.gameObject.GetComponent<Button>().onClick.AddListener(OnTestBtn);
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //AddData();
                OnTestBtn();
            }
        }

        void OnTestBtn()
        {
            int index = Random.Range(0, chart.series.Count);
            var serie = chart.series.GetSerie(index);
            chart.UpdateData(index, Random.Range(0, serie.dataCount), Random.Range(50, 100));
        }

        void AddData()
        {
            chart.ClearData();
            int count = Random.Range(5, 100);
            for (int i = 0; i < count; i++)
            {
                (chart as CoordinateChart).AddXAxisData("x" + i);
                if (Random.Range(1, 3) == 2)
                    chart.AddData(0, Random.Range(-110, 200));
                else
                    chart.AddData(0, Random.Range(-100, 100));
            }
        }
    }
}