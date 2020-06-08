/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;

namespace XCharts.Examples
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
            }
            var serie = chart.series.GetSerie(0);
            serie.animation.enable = true;
            //自定义每个数据项的渐入延时
            serie.animation.customFadeInDelay = CustomFadeInDelay;
            //自定义每个数据项的渐入时长
            serie.animation.customFadeInDuration = CustomFadeInDuration;
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