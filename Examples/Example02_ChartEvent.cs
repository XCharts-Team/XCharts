using UnityEngine;
using UnityEngine.EventSystems;
using XCharts.Runtime;

namespace XCharts.Example
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

        void OnPointerEnter(PointerEventData eventData, BaseGraph chart)
        {
            //Debug.LogError("enter:" + chart);
        }

        void OnPointerExit(PointerEventData eventData, BaseGraph chart)
        {
            //Debug.LogError("exit:" + chart);
        }

        void OnPointerDown(PointerEventData eventData, BaseGraph chart)
        {
            //Debug.LogError("down:" + chart);
        }

        void OnPointerUp(PointerEventData eventData, BaseGraph chart)
        {
            //Debug.LogError("up:" + chart);
        }

        void OnPointerClick(PointerEventData eventData, BaseGraph chart)
        {
            //Debug.LogError("click:" + chart);
        }

        void OnScroll(PointerEventData eventData, BaseGraph chart)
        {
            //Debug.LogError("scroll:" + chart);
        }
    }
}