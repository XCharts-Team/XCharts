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
        CoordinateChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<CoordinateChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<CoordinateChart>();
            }
        }

        void Update()
        {
            updateTime += Time.deltaTime;
            if (updateTime > 2)
            {
                updateTime = 0;
                chart.UpdateData(0, Random.Range(0, 5), Random.Range(10, 90));
            }
        }
    }
}