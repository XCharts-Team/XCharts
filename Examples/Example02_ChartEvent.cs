using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using XCharts.Runtime;
using XUGL;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(BaseChart))]
    public class Example02_ChartEvent : MonoBehaviour
    {
        BaseChart chart;

        void Awake()
        {
            chart = gameObject.GetComponent<BaseChart>();

            chart.onPointerEnter = OnPointerEnter;
            chart.onPointerExit = OnPointerExit;
            chart.onPointerDown = OnPointerDown;
            chart.onPointerUp = OnPointerUp;
            chart.onPointerClick = OnPointerClick;
            chart.onScroll = OnScroll;

            chart.onSerieClick = OnSerieClick;
            chart.onSerieEnter = OnSerieEnter;
            chart.onSerieExit = OnSerieExit;

            chart.onDraw = OnDraw;
            chart.onDrawBeforeSerie = OnDrawBeforeSerie;
            chart.onDrawAfterSerie = OnDrawAfterSerie;
            chart.onDrawTop = OnDrawTop;
        }

        void OnPointerEnter(PointerEventData eventData, BaseGraph chart)
        {
            Debug.Log("enter:" + chart);
        }

        void OnPointerExit(PointerEventData eventData, BaseGraph chart)
        {
            Debug.Log("exit:" + chart);
        }

        void OnPointerDown(PointerEventData eventData, BaseGraph chart)
        {
            Debug.Log("down:" + chart);
        }

        void OnPointerUp(PointerEventData eventData, BaseGraph chart)
        {
            Debug.Log("up:" + chart);
        }

        void OnPointerClick(PointerEventData eventData, BaseGraph chart)
        {
            Debug.Log("click:" + chart);
        }

        void OnScroll(PointerEventData eventData, BaseGraph chart)
        {
            Debug.Log("scroll:" + chart);
        }

        void OnSerieClick(SerieEventData data)
        {
            Debug.Log("OnSerieClick: " + data.serieIndex + " " + data.dataIndex + " " + data.dimension + " " + data.value);
        }

        void OnSerieEnter(SerieEventData data)
        {
            Debug.Log("OnSerieEnter: " + data.serieIndex + " " + data.dataIndex + " " + data.dimension + " " + data.value);
        }

        void OnSerieExit(SerieEventData data)
        {
            Debug.Log("OnSerieExit: " + data.serieIndex + " " + data.dataIndex + " " + data.dimension + " " + data.value);
        }

        void OnDraw(VertexHelper vh)
        {
            //Debug.Log("OnDraw");
        }

        void OnDrawBeforeSerie(VertexHelper vh, Serie serie)
        {
            //Debug.Log("OnDrawBeforeSerie: " + serie.index);
        }

        void OnDrawAfterSerie(VertexHelper vh, Serie serie)
        {
            //Debug.Log("OnDrawAfterSerie: " + serie.index);
            if (serie.index != 0) return;
            var dataPoints = serie.context.dataPoints;
            if (dataPoints.Count > 4)
            {
                var pos = dataPoints[3];
                var grid = chart.GetChartComponent<GridCoord>();
                var zeroPos = new Vector3(grid.context.x, grid.context.y);
                var startPos = new Vector3(pos.x, zeroPos.y);
                var endPos = new Vector3(pos.x, zeroPos.y + grid.context.height);
                UGL.DrawLine(vh, startPos, endPos, chart.theme.serie.lineWidth, Color.blue);
                UGL.DrawCricle(vh, pos, 5, Color.blue);
            }
        }

        void OnDrawTop(VertexHelper vh)
        {
            //Debug.Log("OnDrawTop");
        }
    }
}