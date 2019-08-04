using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI RadarChart.
    /// </summary>

    [CustomEditor(typeof(RadarChart), false)]
    public class RadarChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Radar;
        protected SerializedProperty m_Radars;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (RadarChart)target;
            m_Radars = serializedObject.FindProperty("m_Radars");
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
            EditorGUILayout.PropertyField(m_Radars, true);
        }
    }
}