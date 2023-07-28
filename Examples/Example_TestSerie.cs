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
        public int maxCache = 100;
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
        }
    }
}