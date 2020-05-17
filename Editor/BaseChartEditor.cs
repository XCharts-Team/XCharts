/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

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
        protected SerializedProperty m_Settings;
        protected SerializedProperty m_Large;
        protected SerializedProperty m_ChartName;

        protected float m_DefaultLabelWidth;
        protected float m_DefaultFieldWidth;
        private int m_SeriesSize;
        private Vector2 scrollPos;
        private bool m_CheckWarning = false;

        protected virtual void OnEnable()
        {
            m_Target = (BaseChart)target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_ChartName = serializedObject.FindProperty("m_ChartName");
            m_ChartWidth = serializedObject.FindProperty("m_ChartWidth");
            m_ChartHeight = serializedObject.FindProperty("m_ChartHeight");
            m_Theme = serializedObject.FindProperty("m_Theme");
            m_ThemeInfo = serializedObject.FindProperty("m_ThemeInfo");
            m_Title = serializedObject.FindProperty("m_Title");
            m_Legend = serializedObject.FindProperty("m_Legend");
            m_Tooltip = serializedObject.FindProperty("m_Tooltip");
            m_Series = serializedObject.FindProperty("m_Series");

            m_Large = serializedObject.FindProperty("m_Large");
            m_Settings = serializedObject.FindProperty("m_Settings");
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

            CheckWarning();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnStartInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_Script);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_ChartName);
            EditorGUILayout.PropertyField(m_ThemeInfo, true);
            EditorGUILayout.PropertyField(m_Title, true);
            EditorGUILayout.PropertyField(m_Legend, true);
            EditorGUILayout.PropertyField(m_Tooltip, true);
        }

        protected virtual void OnMiddleInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_Series, true);
            EditorGUILayout.PropertyField(m_Settings, true);
        }

        protected virtual void OnEndInspectorGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }

        private void CheckWarning()
        {
            if (m_CheckWarning)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Check warning"))
                {
                    m_CheckWarning = true;
                    m_Target.CheckWarning();
                }
                if (GUILayout.Button("Hide warning"))
                {
                    m_CheckWarning = false;
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField("version:" + XChartsMgr.Instance.nowVersion);
                if (!string.IsNullOrEmpty(m_Target.warningInfo))
                {
                    var infos = m_Target.warningInfo.Split('\n');
                    foreach (var info in infos)
                    {
                        EditorGUILayout.LabelField(info);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("Perfect! No warning!");
                }
            }
            else
            {
                if (GUILayout.Button("Check warning"))
                {
                    m_CheckWarning = true;
                    m_Target.CheckWarning();
                }
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
        }
    }
}