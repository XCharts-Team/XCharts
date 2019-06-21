using UnityEngine;
using XCharts;

[DisallowMultipleComponent]
[ExecuteInEditMode]
[RequireComponent(typeof(PieChart))]
public class Demo_PieChart : MonoBehaviour
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
                chart.AddData("time" + count, Random.Range(10, 100));
            }
            else
            {
                int index = count % 5;
                chart.UpdateData("time" + index, Random.Range(10, 100));
            }
            count++;
        }
    }
}