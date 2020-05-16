/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;


namespace XCharts.Examples
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

            chart.onCustomDraw = delegate (VertexHelper vh)
            {
                var dataPoints = chart.series.list[0].dataPoints;
                if (dataPoints.Count > 0)
                {
                    var pos = dataPoints[3];
                    var zeroPos = new Vector3(chart.coordinateX, chart.coordinateY);
                    var startPos = new Vector3(pos.x, zeroPos.y);
                    var endPos = new Vector3(pos.x, zeroPos.y + chart.coordinateHeight);
                    ChartDrawer.DrawLine(vh, startPos, endPos, 1, Color.blue);
                    ChartDrawer.DrawCricle(vh, pos, 5, Color.blue);
                }
            };
        }
    }
}