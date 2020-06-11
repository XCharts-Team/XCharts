/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;
using UnityEngine;
using System.Text;

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
        protected SerializedProperty m_Background;
        protected SerializedProperty m_Title;
        protected SerializedProperty m_Legend;
        protected SerializedProperty m_Tooltip;
        protected SerializedProperty m_Series;
        protected SerializedProperty m_Settings;
        protected SerializedProperty m_Large;
        protected SerializedProperty m_ChartName;
        protected SerializedProperty m_DebugMode;

        protected float m_DefaultLabelWidth;
        protected float m_DefaultFieldWidth;
        private int m_SeriesSize;
        private Vector2 scrollPos;
        private bool m_CheckWarning = false;
        private StringBuilder sb = new StringBuilder();

        protected virtual void OnEnable()
        {
            m_Target = (BaseChart)target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_ChartName = serializedObject.FindProperty("m_ChartName");
            m_ChartWidth = serializedObject.FindProperty("m_ChartWidth");
            m_ChartHeight = serializedObject.FindProperty("m_ChartHeight");
            m_Theme = serializedObject.FindProperty("m_Theme");
            m_ThemeInfo = serializedObject.FindProperty("m_ThemeInfo");
            m_Background = serializedObject.FindProperty("m_Background");
            m_Title = serializedObject.FindProperty("m_Title");
            m_Legend = serializedObject.FindProperty("m_Legend");
            m_Tooltip = serializedObject.FindProperty("m_Tooltip");
            m_Series = serializedObject.FindProperty("m_Series");

            m_Large = serializedObject.FindProperty("m_Large");
            m_Settings = serializedObject.FindProperty("m_Settings");
            m_DebugMode = serializedObject.FindProperty("m_DebugMode");
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

            EditorGUILayout.PropertyField(m_ChartName);
            EditorGUILayout.PropertyField(m_ThemeInfo, true);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_Background, true);

            var m_Show = m_Background.FindPropertyRelative("m_Show");
            if (m_Show.boolValue && !m_Target.CanShowBackgroundComponent())
            {
                var msg = "The background component cannot be activated because chart is controlled by LayoutGroup,"
                + " or its parent have more than one child.";
                EditorGUILayout.HelpBox(msg, MessageType.Error);
            }
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
            EditorGUILayout.PropertyField(m_DebugMode);
        }

        private void CheckWarning()
        {
            if (GUILayout.Button("Check XCharts Update "))
            {
                CheckVersionEditor.ShowWindow();
            }
            if (m_CheckWarning)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Check Warning"))
                {
                    m_CheckWarning = true;
                    m_Target.CheckWarning();
                }
                if (GUILayout.Button("Hide Warning"))
                {
                    m_CheckWarning = false;
                }
                EditorGUILayout.EndHorizontal();
                sb.Length = 0;
                sb.AppendFormat("version:{0}", XChartsMgr.Instance.nowVersion);
                if (!string.IsNullOrEmpty(m_Target.warningInfo))
                {
                    sb.AppendLine();
                    sb.Append(m_Target.warningInfo);
                }
                else
                {
                    sb.AppendLine();
                    sb.Append("Perfect! No warning!");
                }
                EditorGUILayout.HelpBox(sb.ToString(), MessageType.Warning);
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