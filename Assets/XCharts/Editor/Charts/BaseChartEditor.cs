
using System;
using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XCharts.Editor
{
    [CustomEditor(typeof(BaseChart), true)]
    public class BaseChartEditor : UnityEditor.Editor
    {
        protected BaseChart m_Chart;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_EnableTextMeshPro;
        protected SerializedProperty m_Settings;
        protected SerializedProperty m_Theme;
        protected SerializedProperty m_ChartName;
        protected SerializedProperty m_DebugMode;
        protected SerializedProperty m_DebugInfo;
        protected SerializedProperty m_RaycastTarget;

        protected List<SerializedProperty> m_Components = new List<SerializedProperty>();
        protected List<SerializedProperty> m_Series = new List<SerializedProperty>();

        private bool m_BaseFoldout;
        private bool m_CheckWarning = false;
        private int m_LastComponentCount = 0;
        private int m_LastSerieCount = 0;
        private StringBuilder sb = new StringBuilder();
        MainComponentListEditor m_ComponentList;
        SerieListEditor m_SerieList;

        protected virtual void OnEnable()
        {
            if (target == null) return;
            m_Chart = (BaseChart)target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_EnableTextMeshPro = serializedObject.FindProperty("m_EnableTextMeshPro");
            m_ChartName = serializedObject.FindProperty("m_ChartName");
            m_Theme = serializedObject.FindProperty("m_Theme");
            m_Settings = serializedObject.FindProperty("m_Settings");
            m_DebugMode = serializedObject.FindProperty("m_DebugMode");
            m_DebugInfo = serializedObject.FindProperty("m_DebugInfo");
            m_RaycastTarget = serializedObject.FindProperty("m_RaycastTarget");

            RefreshComponent();
            m_ComponentList = new MainComponentListEditor(this);
            m_ComponentList.Init(m_Chart, serializedObject, m_Components);

            RefreshSeries();
            m_SerieList = new SerieListEditor(this);
            m_SerieList.Init(m_Chart, serializedObject, m_Series);
        }

        public List<SerializedProperty> RefreshComponent()
        {
            m_Components.Clear();
            serializedObject.Update();
            foreach (var kv in m_Chart.typeListForComponent)
            {
                InitComponent(kv.Value.Name);
            }
            return m_Components;
        }

        public List<SerializedProperty> RefreshSeries()
        {
            m_Series.Clear();
            serializedObject.Update();
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
            serializedObject.Update();
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
            EditorGUILayout.Space();

            OnDebugInspectorGUI();
            EditorGUILayout.Space();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnStartInspectorGUI()
        {
            var version = string.Format("v{0}_{1}", XChartsMgr.version, XChartsMgr.versionDate);
            if (m_EnableTextMeshPro.boolValue)
            {
                version += " TMP";
            }
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(version);
            EditorGUILayout.Space();
            serializedObject.Update();

            m_BaseFoldout = ChartEditorHelper.DrawHeader("Base", m_BaseFoldout, false, null, null);
            if (m_BaseFoldout)
            {
                EditorGUILayout.PropertyField(m_Script);
                EditorGUILayout.PropertyField(m_ChartName);
                EditorGUILayout.PropertyField(m_RaycastTarget);
            }
            EditorGUILayout.PropertyField(m_Theme);
            EditorGUILayout.PropertyField(m_Settings);

            m_ComponentList.OnGUI();

            m_SerieList.OnGUI();
        }

        protected virtual void OnDebugInspectorGUI()
        {
            AddSerie();
            AddComponent();
            CheckWarning();
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.PropertyField(m_DebugMode);
            EditorGUILayout.PropertyField(m_DebugInfo, true);
            EditorGUILayout.EndVertical();
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
        }

        private void AddComponent()
        {
            if (GUILayout.Button("Add Component"))
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
            if (GUILayout.Button("Add Serie"))
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
                if (GUILayout.Button("Covert XY Axis"))
                    m_Chart.CovertXYAxis(0);
            }
            if (GUILayout.Button("Remove All Chart Object"))
            {
                m_Chart.RemoveChartObject();
            }
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
        }
    }
}