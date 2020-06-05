/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.EventSystems;

namespace XCharts.Examples
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example02_ChartEvent : MonoBehaviour
    {
        BaseChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();
            if (chart == null)
            {
                chart = gameObject.AddComponent<LineChart>();
            }
            chart.onPointerEnter = OnPointerEnter;
            chart.onPointerExit = OnPointerExit;
            chart.onPointerDown = OnPointerDown;
            chart.onPointerUp = OnPointerUp;
            chart.onPointerClick = OnPointerClick;
            chart.onScroll = OnScroll;
        }

        void OnPointerEnter(BaseGraph chart, PointerEventData eventData)
        {
            //Debug.LogError("enter:" + chart);
        }

        void OnPointerExit(BaseGraph chart, PointerEventData eventData)
        {
            //Debug.LogError("exit:" + chart);
        }

        void OnPointerDown(BaseGraph chart, PointerEventData eventData)
        {
            //Debug.LogError("down:" + chart);
        }

        void OnPointerUp(BaseGraph chart, PointerEventData eventData)
        {
            //Debug.LogError("up:" + chart);
        }

        void OnPointerClick(BaseGraph chart, PointerEventData eventData)
        {
            //Debug.LogError("click:" + chart);
        }

        void OnScroll(BaseGraph chart, PointerEventData eventData)
        {
            //Debug.LogError("scroll:" + chart);
        }
    }
}