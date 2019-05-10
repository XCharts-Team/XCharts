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
            m_Pie = serializedObject.FindProperty("m_Pie");
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
            EditorGUILayout.PropertyField(m_Pie, true);
        }
    }
}