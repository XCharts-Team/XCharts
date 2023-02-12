using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example04_DataZoom : MonoBehaviour
    {
        BaseChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
            if (chart == null) return;
            var dataZoom = chart.GetChartComponent<DataZoom>();
            if (dataZoom == null) return;
            dataZoom.marqueeStyle.onStart = OnMarqueeStart;
            dataZoom.marqueeStyle.onEnd = OnMarqueeEnd;
            dataZoom.marqueeStyle.onGoing = OnMarquee;
        }

        void OnMarqueeStart(DataZoom dataZoom)
        {
            //Debug.Log("OnMarqueeStart:" + dataZoom);
        }

        void OnMarquee(DataZoom dataZoom)
        {
            //Debug.Log("OnMarquee:" + dataZoom);
        }

        void OnMarqueeEnd(DataZoom dataZoom)
        {
            //Debug.Log("OnMarqueeEnd:" + dataZoom);
            var serie = chart.GetSerie(0);
            foreach (var serieData in serie.data)
            {
                if (dataZoom.IsInMarqueeArea(serieData))
                {
                    serieData.EnsureComponent<ItemStyle>().color = Color.red;
                }
                else
                {
                    serieData.EnsureComponent<ItemStyle>().color = Color.clear;
                }
            }
        }
    }
}