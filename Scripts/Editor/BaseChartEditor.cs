using UnityEditor;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI BaseChart.
    /// </summary>

    [CustomEditor(typeof(BaseChart), false)]
    public class BaseChartEditor : Editor
    {
        protected BaseChart m_Target;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_Theme;
        protected SerializedProperty m_ThemeInfo;
        protected SerializedProperty m_Title;
        protected SerializedProperty m_Legend;
        protected SerializedProperty m_Tooltip;
        protected SerializedProperty m_Series;

        protected SerializedProperty m_Large;
        protected SerializedProperty m_MinShowDataNumber;
        protected SerializedProperty m_MaxShowDataNumber;
        protected SerializedProperty m_MaxCacheDataNumber;

        protected float m_DefaultLabelWidth;
        protected float m_DefaultFieldWidth;

        private int m_SeriesSize;


        private bool m_ThemeModuleToggle = false;
        private bool m_BaseModuleToggle = false;



        protected virtual void OnEnable()
        {
            m_Target = (BaseChart)target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_Theme = serializedObject.FindProperty("m_Theme");
            m_ThemeInfo = serializedObject.FindProperty("m_ThemeInfo");
            m_Title = serializedObject.FindProperty("m_Title");
            m_Legend = serializedObject.FindProperty("m_Legend");
            m_Tooltip = serializedObject.FindProperty("m_Tooltip");
            m_Series = serializedObject.FindProperty("m_Series");

            m_Large = serializedObject.FindProperty("m_Large");
            m_MinShowDataNumber = serializedObject.FindProperty("m_MinShowDataNumber");
            m_MaxShowDataNumber = serializedObject.FindProperty("m_MaxShowDataNumber");
            m_MaxCacheDataNumber = serializedObject.FindProperty("m_MaxCacheDataNumber");
        }

        public override void OnInspectorGUI()
        {
            if (m_Target == null && target == null)
            {
                base.OnInspectorGUI();
                return;
            }
            serializedObject.Update();
            m_DefaultLabelWidth = EditorGUIUtility.labelWidth;
            m_DefaultFieldWidth = EditorGUIUtility.fieldWidth;

            OnStartInspectorGUI();
            OnMiddleInspectorGUI();
            OnEndInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnStartInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_Script);
            EditorGUILayout.BeginHorizontal();
            EditorGUIUtility.fieldWidth = EditorGUIUtility.labelWidth - 5;
            m_ThemeModuleToggle = EditorGUILayout.Foldout(m_ThemeModuleToggle, "Theme",
                ChartEditorHelper.foldoutStyle);
            EditorGUILayout.PropertyField(m_Theme, GUIContent.none);
            EditorGUILayout.EndHorizontal();
            EditorGUIUtility.labelWidth = m_DefaultLabelWidth;
            EditorGUIUtility.fieldWidth = m_DefaultFieldWidth;
            if (m_ThemeModuleToggle)
            {
                EditorGUILayout.PropertyField(m_ThemeInfo, true);
            }
            EditorGUILayout.PropertyField(m_Title, true);
            EditorGUILayout.PropertyField(m_Legend, true);
            EditorGUILayout.PropertyField(m_Tooltip, true);
        }

        protected virtual void OnMiddleInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_Series, true);
            m_BaseModuleToggle = EditorGUILayout.Foldout(m_BaseModuleToggle, "Base",
                ChartEditorHelper.foldoutStyle);
            if (m_BaseModuleToggle)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(m_Large, true);
                EditorGUILayout.PropertyField(m_MinShowDataNumber, true);
                EditorGUILayout.PropertyField(m_MaxShowDataNumber, true);
                EditorGUILayout.PropertyField(m_MaxCacheDataNumber, true);
                if (m_MinShowDataNumber.intValue < 0) m_MinShowDataNumber.intValue = 0;
                if (m_MaxShowDataNumber.intValue < 0) m_MaxShowDataNumber.intValue = 0;
                if (m_MaxCacheDataNumber.intValue < 0) m_MaxCacheDataNumber.intValue = 0;
                EditorGUI.indentLevel--;
            }
        }

        protected virtual void OnEndInspectorGUI()
        {
        }
    }
}