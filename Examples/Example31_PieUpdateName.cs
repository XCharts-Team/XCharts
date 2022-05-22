using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example31_PieUpdateName : MonoBehaviour
    {
        PieChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<PieChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<PieChart>();
            }
            var serieIndex = 0;
            var serie = chart.GetSerie(serieIndex);
            if (serie == null) return;
            serie.AddExtraComponent<LabelStyle>();
            serie.label.show = true;
            serie.label.position = LabelStyle.Position.Outside;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ClearAndAddData();
                //UpdateDataName();
                //UpdateDataName();
            }
        }

        void UpdateDataName()
        {
            var serieIndex = 0;
            var serie = chart.GetSerie(serieIndex);
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
            var serie = chart.GetSerie(serieIndex);
            if (serie == null) return;
            for (int i = 0; i < serie.dataCount; i++)
            {
                chart.UpdateDataName(serieIndex, i, "piename");
            }
        }

        void ClearAndAddData()
        {
            var serieIndex = 0;
            var serie = chart.GetSerie(serieIndex);
            if (serie == null) return;
            int count = serie.dataCount;
            serie.ClearData();
            for (int i = 0; i < count; i++)
            {
                chart.AddData(0, Random.Range(0, 100), "pie" + i);
            }
        }
    }
}