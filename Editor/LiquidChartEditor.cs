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
    /// Editor class used to edit UI LiquidChart.
    /// </summary>

    [CustomEditor(typeof(LiquidChart), false)]
    public class LiquidChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Vessels;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (LiquidChart)target;
            m_Vessels = serializedObject.FindProperty("m_Vessels");
        }

        protected override void OnStartInspectorGUI()
        {
            base.OnStartInspectorGUI();
            EditorGUILayout.PropertyField(m_Vessels, true);
        }
    }
}