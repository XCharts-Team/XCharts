using UnityEngine;
#if INPUT_SYSTEM_ENABLED
using Input = XCharts.Runtime.InputHelper;
#endif
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example_Test : MonoBehaviour
    {
        BaseChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                AddData();
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                chart.AnimationReset();
                chart.AnimationFadeIn();
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                chart.UpdateData(0, 2, 99);
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                chart.UpdateData(0, 2, 22);
            }
        }

        void AddData()
        {
            chart.AnimationReset();
            chart.AnimationFadeOut();
        }
    }
}