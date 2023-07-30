using UnityEngine;
using XCharts.Runtime;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
namespace XCharts.Example
{
    [DisallowMultipleComponent]
    public class Example_TestSerie : MonoBehaviour
    {
        BaseChart chart;
        int timestamp;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                chart.GetSerie(0).radius[1] = Random.Range(50, 80);
                chart.SetAllDirty();
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                chart.GetSerie(0).lineStyle.width = Random.Range(1, 5);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                chart.GetSerie(0).symbol.size = Random.Range(1, 10);
            }
        }
    }
}