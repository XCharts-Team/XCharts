using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI LineChart.
    /// </summary>

    [CustomEditor(typeof(LineChart), false)]
    public class LineChartEditor : CoordinateChartEditor
    {
        protected SerializedProperty m_Line;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (LineChart)target;
            m_Line = serializedObject.FindProperty("m_Line");
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
            EditorGUILayout.PropertyField(m_Line, true);
        }
    }
}