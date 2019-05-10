using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI BarChart.
    /// </summary>

    [CustomEditor(typeof(BarChart), false)]
    public class BarChartEditor : CoordinateChartEditor
    {
        protected SerializedProperty m_Bar;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (BarChart)target;
            m_Bar = serializedObject.FindProperty("m_Bar");
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
            if (m_Target == null && target == null)
            {
                return;
            }
            EditorGUILayout.PropertyField(m_Bar, true);
        }
    }
}