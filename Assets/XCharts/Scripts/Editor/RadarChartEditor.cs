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
        protected bool m_RadarModuleToggle = false;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (RadarChart)target;
            m_Radar = serializedObject.FindProperty("m_Radar");
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
            EditorGUILayout.PropertyField(m_Radar, true);
        }
    }
}