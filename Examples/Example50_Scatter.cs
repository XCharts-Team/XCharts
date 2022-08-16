using System;
using System.Collections.Generic;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
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
            foreach (var serie in chart.series)
            {
                serie.symbol.sizeFunction = SymbolSize;
            }
        }

        float SymbolSize(List<double> data)
        {
            return (float) (Math.Sqrt(data[2]) / 6e2);
        }
    }
}