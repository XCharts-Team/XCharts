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
    /// Editor class used to edit UI PieChart.
    /// </summary>
    [CustomEditor(typeof(PieChart), false)]
    public class PieChartEditor : BaseChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if(target == null) return;
            m_Chart = (PieChart)target;
        }
    }
}