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
    /// Editor class used to edit UI PolarChart.
    /// </summary>

    [CustomEditor(typeof(PolarChart), false)]
    public class PolarChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Polar;
        protected SerializedProperty m_RadiusAxis;
        protected SerializedProperty m_AngleAxis;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (PolarChart)target;
            m_Polar = serializedObject.FindProperty("m_Polar");
            m_RadiusAxis = serializedObject.FindProperty("m_RadiusAxis");
            m_AngleAxis = serializedObject.FindProperty("m_AngleAxis");
        }

        protected override void OnStartInspectorGUI()
        {
            base.OnStartInspectorGUI();
            EditorGUILayout.PropertyField(m_Polar, true);
            EditorGUILayout.PropertyField(m_RadiusAxis, true);
            EditorGUILayout.PropertyField(m_AngleAxis, true);
        }
    }
}