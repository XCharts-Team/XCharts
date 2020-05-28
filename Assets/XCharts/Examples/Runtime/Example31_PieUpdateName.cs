/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example31_PieUpdateName : MonoBehaviour
    {
        PieChart chart;
        int count = 0;

        void Awake()
        {
            chart = gameObject.GetComponent<PieChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<PieChart>();
            }
            var serieIndex = 0;
            var serie = chart.series.GetSerie(serieIndex);
            if (serie == null) return;
            serie.label.show = true;
            serie.label.position = SerieLabel.Position.Outside;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (count % 2 == 0) ResetSameName();
                else UpdateDataName();
                count++;
            }
        }

        void UpdateDataName()
        {
            var serieIndex = 0;
            var serie = chart.series.GetSerie(serieIndex);
            if (serie == null) return;
            for (int i = 0; i < serie.dataCount; i++)
            {
                var value = Random.Range(10, 100);
                chart.UpdateData(serieIndex, i, value);
                chart.UpdateDataName(serieIndex, i, "value=" + value);
            }
        }

        void ResetSameName()
        {
            var serieIndex = 0;
            var serie = chart.series.GetSerie(serieIndex);
            if (serie == null) return;
            for (int i = 0; i < serie.dataCount; i++)
            {
                chart.UpdateDataName(serieIndex, i, "piename");
            }
            chart.themeInfo.SetAllDirty();
        }
    }
}