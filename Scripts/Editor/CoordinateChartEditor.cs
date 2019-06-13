using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI CoordinateChart.
    /// </summary>

    [CustomEditor(typeof(CoordinateChart), false)]
    public class CoordinateChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Coordinate;
        protected SerializedProperty m_XAxis;
        protected SerializedProperty m_YAxis;
        protected SerializedProperty m_DataZoom;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (CoordinateChart)target;
            m_Coordinate = serializedObject.FindProperty("m_Coordinate");
            m_XAxis = serializedObject.FindProperty("m_XAxis");
            m_YAxis = serializedObject.FindProperty("m_YAxis");
            m_DataZoom = serializedObject.FindProperty("m_DataZoom");
        }

        protected override void OnStartInspectorGUI()
        {
            base.OnStartInspectorGUI();
            EditorGUILayout.PropertyField(m_DataZoom);
            EditorGUILayout.PropertyField(m_Coordinate);
            EditorGUILayout.PropertyField(m_XAxis);
            EditorGUILayout.PropertyField(m_YAxis);
        }
    }
}