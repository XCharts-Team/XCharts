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
        protected SerializedProperty m_ChartWidth;
        protected SerializedProperty m_ChartHeight;
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
        protected SerializedProperty m_LineSmoothStyle;

        protected float m_DefaultLabelWidth;
        protected float m_DefaultFieldWidth;
        private int m_SeriesSize;
        private bool m_BaseModuleToggle = false;

        protected virtual void OnEnable()
        {
            m_Target = (BaseChart)target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_ChartWidth = serializedObject.FindProperty("m_ChartWidth");
            m_ChartHeight = serializedObject.FindProperty("m_ChartHeight");
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
            m_LineSmoothStyle = serializedObject.FindProperty("m_LineSmoothStyle");
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
            EditorGUILayout.PropertyField(m_ChartWidth);
            EditorGUILayout.PropertyField(m_ChartHeight);
            EditorGUILayout.PropertyField(m_ThemeInfo, true);
            EditorGUILayout.PropertyField(m_Title, true);
            EditorGUILayout.PropertyField(m_Legend, true);
            EditorGUILayout.PropertyField(m_Tooltip, true);
        }

        protected virtual void OnMiddleInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_Series, true);
            m_BaseModuleToggle = EditorGUILayout.Foldout(m_BaseModuleToggle,
                new GUIContent("Base", "基础配置"),
                ChartEditorHelper.foldoutStyle);
            if (m_BaseModuleToggle)
            {
                EditorGUI.indentLevel++;
                var largeTip = "Whether to enable the optimization of large-scale graph. \n是否启用大规模线图的优化，在数据图形特别多的时候（>=5k）可以开启。";
                EditorGUILayout.PropertyField(m_Large, new GUIContent("Large", largeTip));
                EditorGUILayout.PropertyField(m_MinShowDataNumber, true);
                EditorGUILayout.PropertyField(m_MaxShowDataNumber, true);
                EditorGUILayout.PropertyField(m_MaxCacheDataNumber, true);
                EditorGUILayout.PropertyField(m_LineSmoothStyle,true);
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