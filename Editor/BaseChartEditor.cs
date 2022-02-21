/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Collections.Generic;
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
#if UNITY_2019_3_OR_NEWER
        private const float k_IconWidth = 14;
        private const float k_IconGap = 0f;
        private const float k_IconXOffset = 10f;
        private const float k_IconYOffset = -1.5f;
#else
        private const float k_IconWidth = 14;
        private const float k_IconGap = 0f;
        private const float k_IconXOffset = 4f;
        private const float k_IconYOffset = -3f;
#endif

        protected BaseChart m_Chart;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_MultiComponentMode;
        protected SerializedProperty m_EnableTextMeshPro;
        protected SerializedProperty m_ChartWidth;
        protected SerializedProperty m_ChartHeight;
        protected SerializedProperty m_Settings;
        protected SerializedProperty m_Theme;
        protected SerializedProperty m_Background;
        protected SerializedProperty m_Titles;
        protected SerializedProperty m_Legends;
        protected SerializedProperty m_Tooltips;
        protected SerializedProperty m_Vessels;
        protected SerializedProperty m_Radars;
        protected SerializedProperty m_Series;
        protected SerializedProperty m_Large;
        protected SerializedProperty m_ChartName;
        protected SerializedProperty m_DebugMode;

        protected float m_DefaultLabelWidth;
        protected float m_DefaultFieldWidth;
        private int m_SeriesSize;
        private Vector2 scrollPos;
        private bool m_CheckWarning = false;
        private StringBuilder sb = new StringBuilder();

        private bool m_BaseFoldout;
        private bool m_CustomFoldout;
        protected bool m_ShowAllComponent;
        protected Dictionary<string, bool> m_Flodouts = new Dictionary<string, bool>();

        protected virtual void OnEnable()
        {
            if (target == null) return;
            m_Chart = (BaseChart)target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_MultiComponentMode = serializedObject.FindProperty("m_MultiComponentMode");
            m_EnableTextMeshPro = serializedObject.FindProperty("m_EnableTextMeshPro");
            m_ChartName = serializedObject.FindProperty("m_ChartName");
            m_ChartWidth = serializedObject.FindProperty("m_ChartWidth");
            m_ChartHeight = serializedObject.FindProperty("m_ChartHeight");
            m_Theme = serializedObject.FindProperty("m_Theme");
            m_Settings = serializedObject.FindProperty("m_Settings");
            m_Background = serializedObject.FindProperty("m_Background");
            m_Titles = serializedObject.FindProperty("m_Titles");
            m_Legends = serializedObject.FindProperty("m_Legends");
            m_Tooltips = serializedObject.FindProperty("m_Tooltips");
            m_Vessels = serializedObject.FindProperty("m_Vessels");
            m_Series = serializedObject.FindProperty("m_Series");
            m_Radars = serializedObject.FindProperty("m_Radars");

            m_Large = serializedObject.FindProperty("m_Large");
            m_DebugMode = serializedObject.FindProperty("m_DebugMode");
        }

        public override void OnInspectorGUI()
        {
            if (m_Chart == null && target == null)
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
            OnDebugInspectorGUI();

            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnStartInspectorGUI()
        {
            BlockStart();
            EditorGUILayout.BeginHorizontal();
            var version = string.Format("v{0}_{1}", XChartsMgr.version, XChartsMgr.versionDate);
            if (m_EnableTextMeshPro.boolValue)
            {
                version += " TMP";
            }
            EditorGUILayout.LabelField(version);

            if (GUILayout.Button("Github"))
            {
                Application.OpenURL("https://github.com/monitor1394/unity-ugui-XCharts");
            }
            EditorGUILayout.EndHorizontal();
            BlockEnd();

            BlockStart();
            m_BaseFoldout = EditorGUILayout.Foldout(m_BaseFoldout, "Base", true);
            if (m_BaseFoldout)
            {
                EditorGUILayout.PropertyField(m_Script);
                EditorGUILayout.PropertyField(m_ChartName);
                var fileds = m_Chart.GetCustomChartInspectorShowFileds();
                if (fileds != null && fileds.Length > 0)
                {
                    foreach (var filed in fileds)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(filed));
                    }
                }
                if (XChartsMgr.Instance.IsRepeatChartName(m_Chart, m_ChartName.stringValue))
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("chart name is repeated:" + m_ChartName.stringValue, MessageType.Error);
                    EditorGUILayout.EndHorizontal();
                }
            }
            BlockEnd();

            BlockField(m_Theme);
            BlockField(m_Settings);
            BlockField(m_Background);

            m_ShowAllComponent = m_MultiComponentMode.boolValue;
            BlockListField(m_ShowAllComponent, m_Titles);
            BlockListField(m_ShowAllComponent, m_Legends);
            BlockListField(m_ShowAllComponent, m_Tooltips);
            BlockListField(m_ShowAllComponent, SerieType.Liquid, m_Vessels);
            BlockListField(m_ShowAllComponent, SerieType.Radar, m_Radars);
        }

        protected virtual void OnMiddleInspectorGUI()
        {
            BlockField(m_Series);
        }

        protected virtual void OnEndInspectorGUI()
        {
        }

        protected virtual void OnDebugInspectorGUI()
        {
            BlockStart();
            EditorGUILayout.PropertyField(m_DebugMode);
            EditorGUILayout.PropertyField(m_MultiComponentMode);
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            MoreDebugInspector();
            CheckWarning();
            BlockEnd();
        }

        protected virtual void MoreDebugInspector()
        {
        }

        protected void BlockStart()
        {
            if (XChartsSettings.editorBlockEnable) EditorGUILayout.BeginVertical(EditorStyles.helpBox);
        }

        protected void BlockEnd()
        {
            if (XChartsSettings.editorBlockEnable) EditorGUILayout.EndVertical();
        }

        protected void BlockField(params SerializedProperty[] props)
        {
            if (props.Length == 0) return;
            BlockStart();
            foreach (var prop in props)
                EditorGUILayout.PropertyField(prop, true);
            BlockEnd();
        }

        protected void BlockListField(bool all, params SerializedProperty[] props)
        {
            if (props.Length == 0) return;
            BlockStart();
            foreach (var prop in props)
            {
                if (all)
                {
                    var flag = m_Flodouts.ContainsKey(prop.displayName) && m_Flodouts[prop.displayName];
                    m_Flodouts[prop.displayName] = EditorGUILayout.Foldout(flag, prop.displayName, true);
                    if (m_Flodouts[prop.displayName])
                    {
                        EditorGUI.indentLevel++;
                        var currRect = EditorGUILayout.GetControlRect(GUILayout.Height(0));
                        currRect.y -= EditorGUIUtility.singleLineHeight;
                        var rect1 = new Rect(currRect.width + k_IconXOffset,
                            currRect.y + k_IconYOffset,
                            k_IconWidth, EditorGUIUtility.singleLineHeight);
                        if (GUI.Button(rect1, ChartEditorHelper.Styles.iconAdd, ChartEditorHelper.Styles.invisibleButton))
                        {
                            prop.InsertArrayElementAtIndex(prop.arraySize > 0 ? prop.arraySize - 1 : 0);
                            var chart = prop.GetArrayElementAtIndex(0).serializedObject.targetObject as BaseChart;
                            serializedObject.ApplyModifiedProperties();
                            chart.RemoveChartObject();
                            chart.RefreshAllComponent();
                        }
                        for (int i = 0; i < prop.arraySize; i++)
                        {
                            EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), true);
                            currRect = EditorGUILayout.GetControlRect(GUILayout.Height(0));
                            currRect.y -= EditorGUI.GetPropertyHeight(prop.GetArrayElementAtIndex(i));
                            rect1 = new Rect(currRect.width + k_IconXOffset,
                                currRect.y + k_IconYOffset,
                                k_IconWidth, EditorGUIUtility.singleLineHeight);
                            var oldColor = GUI.contentColor;
                            GUI.contentColor = Color.black;
                            if (GUI.Button(rect1, ChartEditorHelper.Styles.iconRemove, ChartEditorHelper.Styles.invisibleButton))
                            {
                                if (prop.arraySize == 1)
                                {
                                    if (EditorUtility.DisplayDialog("Delete component", "Confirm to delete last component?", "Sure", "Cancel"))
                                    {
                                        var chart = prop.GetArrayElementAtIndex(0).serializedObject.targetObject as BaseChart;
                                        prop.DeleteArrayElementAtIndex(i);
                                        serializedObject.ApplyModifiedProperties();
                                        chart.RemoveChartObject();
                                        chart.RefreshAllComponent();
                                    }
                                }
                                else if (i < prop.arraySize && i >= 0)
                                {
                                    var chart = prop.GetArrayElementAtIndex(0).serializedObject.targetObject as BaseChart;
                                    prop.DeleteArrayElementAtIndex(i);
                                    serializedObject.ApplyModifiedProperties();
                                    chart.RemoveChartObject();
                                    chart.RefreshAllComponent();
                                }
                            }
                            var rect2 = new Rect(currRect.width + k_IconXOffset - k_IconWidth - k_IconGap,
                                 currRect.y + k_IconYOffset,
                                k_IconWidth, EditorGUIUtility.singleLineHeight);
                            if (GUI.Button(rect2, ChartEditorHelper.Styles.iconDown, ChartEditorHelper.Styles.invisibleButton))
                            {
                                if (i < prop.arraySize - 1) prop.MoveArrayElement(i, i + 1);
                            }
                            var rect3 = new Rect(currRect.width + k_IconXOffset - 2 * (k_IconWidth + k_IconGap),
                                currRect.y + k_IconYOffset,
                                k_IconWidth, EditorGUIUtility.singleLineHeight);
                            if (GUI.Button(rect3, ChartEditorHelper.Styles.iconUp, ChartEditorHelper.Styles.invisibleButton))
                            {
                                if (i > 0) prop.MoveArrayElement(i, i - 1);
                            }
                            GUI.contentColor = oldColor;
                        }
                        EditorGUI.indentLevel--;
                    }
                }
                else if (prop.arraySize > 0) EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(0), true);
            }
            BlockEnd();
        }

        protected void BlockListField(bool all, SerieType serieType, params SerializedProperty[] props)
        {
            if (!m_Chart.ContainsSerie(serieType)) return;
            BlockListField(all, props);
        }

        private void CheckWarning()
        {
            if (GUILayout.Button("Remove All Chart Object"))
            {
                m_Chart.RemoveChartObject();
            }
            // if (GUILayout.Button("Check XCharts Update"))
            // {
            //     CheckVersionEditor.ShowWindow();
            // }
            if (m_CheckWarning)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Check Warning"))
                {
                    m_CheckWarning = true;
                    m_Chart.CheckWarning();
                }
                if (GUILayout.Button("Hide Warning"))
                {
                    m_CheckWarning = false;
                }
                EditorGUILayout.EndHorizontal();
                sb.Length = 0;
                sb.AppendFormat("v{0}", XChartsMgr.fullVersion);
                if (!string.IsNullOrEmpty(m_Chart.warningInfo))
                {
                    sb.AppendLine();
                    sb.Append(m_Chart.warningInfo);
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
                    m_Chart.CheckWarning();
                }
            }
            EditorGUILayout.Space();
        }
    }
}