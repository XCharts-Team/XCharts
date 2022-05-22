using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    [RequireComponent(typeof(PieChart))]
    public class Example_PieChart : MonoBehaviour
    {
        private PieChart chart;
        private float time;
        private int count = 0;

        private void Awake()
        {
            chart = transform.GetComponent<PieChart>();
            chart.ClearData();
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                time = 0;
                if (count < 5)
                {
                    chart.AddData(0, Random.Range(10, 100), "time" + count);
                }
                else
                {
                    int index = count % 5;
                    chart.UpdateData(0, Random.Range(10, 100), index);
                }
                count++;
            }
        }
    }
}