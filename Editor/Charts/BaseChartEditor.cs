using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;
using XCharts.Runtime;

namespace XCharts.Editor
{
    [CustomEditor(typeof(BaseChart), true)]
    public class BaseChartEditor : UnityEditor.Editor
    {
        class Styles
        {
            public static readonly GUIContent btnAddSerie = new GUIContent("Add Serie", "");
            public static readonly GUIContent btnAddComponent = new GUIContent("Add Main Component", "");
            public static readonly GUIContent btnCovertXYAxis = new GUIContent("Covert XY Axis", "");
            public static readonly GUIContent btnRebuildChartObject = new GUIContent("Rebuild Chart Object", "");
            public static readonly GUIContent btnCheckWarning = new GUIContent("Check Warning", "");
            public static readonly GUIContent btnHideWarning = new GUIContent("Hide Warning", "");
        }
        protected BaseChart m_Chart;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_EnableTextMeshPro;
        protected SerializedProperty m_Settings;
        protected SerializedProperty m_Theme;
        protected SerializedProperty m_ChartName;
        protected SerializedProperty m_DebugInfo;
        protected SerializedProperty m_RaycastTarget;

        protected List<SerializedProperty> m_Components = new List<SerializedProperty>();
        protected List<SerializedProperty> m_Series = new List<SerializedProperty>();

        private bool m_BaseFoldout;

        private bool m_CheckWarning = false;
        private int m_LastComponentCount = 0;
        private int m_LastSerieCount = 0;
        private string m_VersionString = "";
        private StringBuilder sb = new StringBuilder();
        MainComponentListEditor m_ComponentList;
        SerieListEditor m_SerieList;

        protected virtual void OnEnable()
        {
            if (target == null) return;
            m_Chart = (BaseChart) target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_EnableTextMeshPro = serializedObject.FindProperty("m_EnableTextMeshPro");
            m_ChartName = serializedObject.FindProperty("m_ChartName");
            m_Theme = serializedObject.FindProperty("m_Theme");
            m_Settings = serializedObject.FindProperty("m_Settings");
            m_DebugInfo = serializedObject.FindProperty("m_DebugInfo");
            m_RaycastTarget = serializedObject.FindProperty("m_RaycastTarget");

            RefreshComponent();
            m_ComponentList = new MainComponentListEditor(this);
            m_ComponentList.Init(m_Chart, serializedObject, m_Components);

            RefreshSeries();
            m_SerieList = new SerieListEditor(this);
            m_SerieList.Init(m_Chart, serializedObject, m_Series);

            m_VersionString = "v" + XChartsMgr.fullVersion;
            if (m_EnableTextMeshPro.boolValue)
                m_VersionString += "-tmp";
        }

        public List<SerializedProperty> RefreshComponent()
        {
            m_Components.Clear();
            serializedObject.UpdateIfRequiredOrScript();
            foreach (var kv in m_Chart.typeListForComponent)
            {
                InitComponent(kv.Value.Name);
            }
            return m_Components;
        }

        public List<SerializedProperty> RefreshSeries()
        {
            m_Series.Clear();
            serializedObject.UpdateIfRequiredOrScript();
            foreach (var kv in m_Chart.typeListForSerie)
            {
                InitSerie(kv.Value.Name);
            }
            return m_Series;
        }

        public override void OnInspectorGUI()
        {
            if (m_Chart == null && target == null)
            {
                base.OnInspectorGUI();
                return;
            }
            serializedObject.UpdateIfRequiredOrScript();
            if (m_LastComponentCount != m_Chart.components.Count)
            {
                m_LastComponentCount = m_Chart.components.Count;
                RefreshComponent();
                m_ComponentList.UpdateComponentsProperty(m_Components);

            }
            if (m_LastSerieCount != m_Chart.series.Count)
            {
                m_LastSerieCount = m_Chart.series.Count;
                RefreshSeries();
                m_SerieList.UpdateSeriesProperty(m_Series);
            }
            OnStartInspectorGUI();
            OnDebugInspectorGUI();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnStartInspectorGUI()
        {
            ShowVersion();
            m_BaseFoldout = ChartEditorHelper.DrawHeader("Base", m_BaseFoldout, false, null, null);
            if (m_BaseFoldout)
            {
                EditorGUILayout.PropertyField(m_Script);
                EditorGUILayout.PropertyField(m_ChartName);
                EditorGUILayout.PropertyField(m_RaycastTarget);
                if (XChartsMgr.IsRepeatChartName(m_Chart, m_ChartName.stringValue))
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("chart name is repeated: " + m_ChartName.stringValue, MessageType.Error);
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.PropertyField(m_Theme);
            EditorGUILayout.PropertyField(m_Settings);
            m_ComponentList.OnGUI();
            m_SerieList.OnGUI();
        }

        protected virtual void OnDebugInspectorGUI()
        {
            EditorGUILayout.PropertyField(m_DebugInfo, true);
            EditorGUILayout.Space();
            AddSerie();
            AddComponent();
            CheckWarning();
        }

        protected void PropertyComponnetList(SerializedProperty prop)
        {
            for (int i = 0; i < prop.arraySize; i++)
            {
                EditorGUILayout.PropertyField(prop.GetArrayElementAtIndex(i), true);
            }
        }

        private void InitComponent(string propName)
        {
            var prop = serializedObject.FindProperty(propName);
            for (int i = 0; i < prop.arraySize; i++)
            {
                m_Components.Add(prop.GetArrayElementAtIndex(i));
            }
            m_Components.Sort((a, b) => { return a.propertyPath.CompareTo(b.propertyPath); });
        }

        private void InitSerie(string propName)
        {
            var prop = serializedObject.FindProperty(propName);
            for (int i = 0; i < prop.arraySize; i++)
            {
                m_Series.Add(prop.GetArrayElementAtIndex(i));
            }
            m_Series.Sort(delegate(SerializedProperty a, SerializedProperty b)
            {
                var index1 = a.FindPropertyRelative("m_Index").intValue;
                var index2 = b.FindPropertyRelative("m_Index").intValue;
                return index1.CompareTo(index2);
            });
        }

        private void ShowVersion()
        {
            EditorGUILayout.HelpBox(m_VersionString, MessageType.None);
        }

        private void AddComponent()
        {
            if (GUILayout.Button(Styles.btnAddComponent))
            {
                var menu = new GenericMenu();
                foreach (var type in GetMainComponentTypeNames())
                {
                    var title = ChartEditorHelper.GetContent(type.Name);
                    bool exists = !m_Chart.CanAddChartComponent(type);
                    if (!exists)
                        menu.AddItem(title, false, () =>
                        {
                            m_ComponentList.AddChartComponent(type);
                        });
                    else
                    {
                        menu.AddDisabledItem(title);
                    }
                }

                menu.ShowAsContext();
            }
        }
        private void AddSerie()
        {
            if (GUILayout.Button(Styles.btnAddSerie))
            {
                var menu = new GenericMenu();
                foreach (var type in GetSerieTypeNames())
                {
                    var title = ChartEditorHelper.GetContent(type.Name);
                    if (m_Chart.CanAddSerie(type))
                    {
                        menu.AddItem(title, false, () =>
                        {
                            m_SerieList.AddSerie(type);
                        });
                    }
                    else
                    {
                        menu.AddDisabledItem(title);
                    }
                }
                menu.ShowAsContext();
            }
        }

        private List<Type> GetMainComponentTypeNames()
        {
            var list = new List<Type>();
            var typeMap = RuntimeUtil.GetAllTypesDerivedFrom<MainComponent>();
            foreach (var kvp in typeMap)
            {
                var type = kvp;
                if (RuntimeUtil.HasSubclass(type)) continue;

                if (type.IsDefined(typeof(ComponentHandlerAttribute), false))
                {
                    var attribute = type.GetAttribute<ComponentHandlerAttribute>();
                    if (attribute != null && attribute.handler != null)
                        list.Add(type);
                }
                else
                {
                    list.Add(type);
                }
            }
            list.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            return list;
        }
        private List<Type> GetSerieTypeNames()
        {
            var list = new List<Type>();
            var typeMap = RuntimeUtil.GetAllTypesDerivedFrom<Serie>();
            foreach (var kvp in typeMap)
            {
                var type = kvp;
                if (type.IsDefined(typeof(SerieHandlerAttribute), false))
                    list.Add(type);
            }
            list.Sort((a, b) => { return a.Name.CompareTo(b.Name); });
            return list;
        }

        private void CheckWarning()
        {
            if (m_Chart.HasChartComponent<XAxis>() && m_Chart.HasChartComponent<YAxis>())
            {
                if (GUILayout.Button(Styles.btnCovertXYAxis))
                    m_Chart.CovertXYAxis(0);
            }
            if (GUILayout.Button(Styles.btnRebuildChartObject))
            {
                m_Chart.RebuildChartObject();
            }
            if (m_CheckWarning)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button(Styles.btnCheckWarning))
                {
                    m_CheckWarning = true;
                    m_Chart.CheckWarning();
                }
                if (GUILayout.Button(Styles.btnHideWarning))
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
                if (GUILayout.Button(Styles.btnCheckWarning))
                {
                    m_CheckWarning = true;
                    m_Chart.CheckWarning();
                }

            }
        }
    }
}