using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [AddComponentMenu("XCharts/LineChart", 13)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LineChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "LineChart";
            m_Tooltip.type = Tooltip.Type.Line;
            RemoveData();
            AddSerie(SerieType.Line, "serie1");
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
                AddData(0, UnityEngine.Random.Range(10, 90));
            }
        }
#endif
    }
}
