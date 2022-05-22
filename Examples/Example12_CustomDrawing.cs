using UnityEngine;
using UnityEngine.UI;
using XCharts.Runtime;
using XUGL;

namespace XCharts.Example
{
    [DisallowMultipleComponent]
    [ExecuteInEditMode]
    public class Example12_CustomDrawing : MonoBehaviour
    {
        LineChart chart;
        void Awake()
        {
            chart = gameObject.GetComponent<LineChart>();
            if (chart == null) return;

            chart.onDraw = delegate(VertexHelper vh) { };
            // or
            chart.onDrawBeforeSerie = delegate(VertexHelper vh, Serie serie) { };
            // or
            chart.onDrawAfterSerie = delegate(VertexHelper vh, Serie serie)
            {
                if (serie.index != 0) return;
                var dataPoints = serie.context.dataPoints;
                if (dataPoints.Count > 0)
                {
                    var pos = dataPoints[3];
                    var grid = chart.GetChartComponent<GridCoord>();
                    var zeroPos = new Vector3(grid.context.x, grid.context.y);
                    var startPos = new Vector3(pos.x, zeroPos.y);
                    var endPos = new Vector3(pos.x, zeroPos.y + grid.context.height);
                    UGL.DrawLine(vh, startPos, endPos, chart.theme.serie.lineWidth, Color.blue);
                    UGL.DrawCricle(vh, pos, 5, Color.blue);
                }
            };
            // or
            chart.onDrawTop = delegate(VertexHelper vh) { };
        }
    }
}