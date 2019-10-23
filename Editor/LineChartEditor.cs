/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI LineChart.
    /// </summary>

    [CustomEditor(typeof(LineChart), false)]
    public class LineChartEditor : CoordinateChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (LineChart)target;
        }
    }
}