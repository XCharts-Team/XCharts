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
    /// Editor class used to edit UI GaugeChart.
    /// </summary>

    [CustomEditor(typeof(GaugeChart), false)]
    public class GaugeChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Radar;
        protected SerializedProperty m_Radars;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (GaugeChart)target;
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
        }
    }
}