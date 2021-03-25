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
    /// Editor class used to edit UI GaugeChart.
    /// </summary>
    [CustomEditor(typeof(GaugeChart), false)]
    public class GaugeChartEditor : BaseChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if(target == null) return;
            m_Chart = (GaugeChart)target;
        }
    }
}