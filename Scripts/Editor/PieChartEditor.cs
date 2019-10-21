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
    /// Editor class used to edit UI PieChart.
    /// </summary>

    [CustomEditor(typeof(PieChart), false)]
    public class PieChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Pie;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (PieChart)target;
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
        }
    }
}