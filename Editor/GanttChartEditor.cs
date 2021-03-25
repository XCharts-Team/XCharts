/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI GanttChart.
    /// </summary>
    [CustomEditor(typeof(GanttChart), false)]
    public class GanttChartEditor : CoordinateChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if(target == null) return;
            m_Chart = (GanttChart)target;
        }
    }
}