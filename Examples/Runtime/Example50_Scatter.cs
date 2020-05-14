/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example50_Scatter : MonoBehaviour
    {
        private ScatterChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<ScatterChart>();
            if (chart == null) return;
            chart.series.SetSerieSymbolSizeCallback(SymbolSize, SymbolSelectedSize);
        }

        float SymbolSize(List<float> data)
        {
            //return Mathf.Clamp(data[1] * 10,1,100);
            return (float)(Mathf.Sqrt(data[2]) / 6e2);
        }

        float SymbolSelectedSize(List<float> data)
        {
            //return Mathf.Clamp(data[1] * 10,1,100);
            return (float)(Mathf.Sqrt(data[2]) / 5e2);
        }
    }
}