/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
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

        float SymbolSize(List<double> data)
        {
            return (float)(Math.Sqrt(data[2]) / 6e2);
        }

        float SymbolSelectedSize(List<double> data)
        {
            return (float)(Math.Sqrt(data[2]) / 5e2);
        }
    }
}