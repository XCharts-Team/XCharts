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
        LineChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<LineChart>();
            }

            var buttom = transform.parent.gameObject.GetComponentInChildren<Button>();
            buttom.onClick.AddListener(AddData);
        }

        void AddData()
        {
            chart.series.list[0].ClearData();
            chart.series.list[1].ClearData();
            for (int i = 0; i < 5; i++)
            {
                chart.AddData(0, Random.Range(20, 100));
                chart.AddData(1, Random.Range(1, 10));
            }
        }
    }
}