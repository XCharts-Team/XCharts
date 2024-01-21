using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example03_ChartAnimation : MonoBehaviour
    {
        BaseChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<BarChart>();
                chart.Init();
            }
            var serie = chart.GetSerie(0);
            serie.animation.enable = true;
            //自定义每个数据项的渐入延时
            serie.animation.fadeIn.delayFunction = CustomFadeInDelay;
            //自定义每个数据项的渐入时长
            serie.animation.fadeIn.durationFunction = CustomFadeInDuration;
        }

        float CustomFadeInDelay(int dataIndex)
        {
            return dataIndex * 1000;
        }

        float CustomFadeInDuration(int dataIndex)
        {
            return dataIndex * 1000 + 1000;
        }
    }
}