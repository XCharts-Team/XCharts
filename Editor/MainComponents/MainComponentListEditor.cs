using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using XCharts.Runtime;

namespace XCharts.Editor
{
    public sealed class MainComponentListEditor
    {
        public BaseChart chart { get; private set; }
        BaseChartEditor m_BaseEditor;

        //SerializedObject m_SerializedObject;
        List<SerializedProperty> m_ComponentsProperty;
        SerializedProperty m_EnableProperty;

        Dictionary<Type, Type> m_EditorTypes;
        List<MainComponentBaseEditor> m_Editors;

        public MainComponentListEditor(BaseChartEditor editor)
        {
            Assert.IsNotNull(editor);
            m_BaseEditor = editor;
        }

        public void Init(BaseChart chart, SerializedObject serializedObject, List<SerializedProperty> componentProps)
        {
            Assert.IsNotNull(chart);

            this.chart = chart;
            m_ComponentsProperty = componentProps;

            Assert.IsNotNull(m_ComponentsProperty);

            m_Editors = new List<MainComponentBaseEditor>();
            m_EditorTypes = new Dictionary<Type, Type>();

            var editorTypes = RuntimeUtil.GetAllTypesDerivedFrom<MainComponentBaseEditor>()
                .Where(t => t.IsDefined(typeof(ComponentEditorAttribute), false) && !t.IsAbstract);
            foreach (var editorType in editorTypes)
            {
                var attribute = editorType.GetAttribute<ComponentEditorAttribute>();
                m_EditorTypes.Add(attribute.componentType, editorType);
            }

            RefreshEditors();
        }

        public void UpdateComponentsProperty(List<SerializedProperty> componentProps)
        {
            m_ComponentsProperty = componentProps;
            RefreshEditors();
        }

        public void Clear()
        {
            if (m_Editors == null)
                return;

            foreach (var editor in m_Editors)
                editor.OnDisable();

            m_Editors.Clear();
            m_EditorTypes.Clear();
        }

        public void OnGUI()
        {
            if (chart == null)
                return;

            for (int i = 0; i < m_Editors.Count; i++)
            {
                var editor = m_Editors[i];
                string title = editor.GetDisplayTitle();
                int id = i;

                bool displayContent = ChartEditorHelper.DrawHeader(
                    title,
                    editor.baseProperty,
                    editor.showProperty,
                    () => { if (EditorUtility.DisplayDialog("", "Sure reset " + editor.component.GetType().Name + "?", "Yes", "Cancel")) ResetComponentEditor(id); },
                    () => { if (EditorUtility.DisplayDialog("", "Sure remove " + editor.component.GetType().Name + "?", "Yes", "Cancel")) RemoveComponentEditor(id); },
                    () => { Application.OpenURL("https://xcharts-team.github.io/docs/configuration/#" + editor.component.GetType().Name.ToLower()); }
                );
                if (displayContent)
                {
                    editor.OnInternalInspectorGUI();
                }
            }

            if (m_Editors.Count == 0)
            {
                EditorGUILayout.HelpBox("No componnet.", MessageType.Info);
            }
        }

        void RefreshEditors()
        {
            foreach (var editor in m_Editors)
                editor.OnDisable();

            m_Editors.Clear();
            var count = Mathf.Min(chart.components.Count, m_ComponentsProperty.Count);
            for (int i = 0; i < count; i++)
            {
                if (chart.components[i] != null)
                {
                    CreateEditor(chart.components[i], m_ComponentsProperty[i]);
                }
            }
        }

        void CreateEditor(MainComponent component, SerializedProperty property, int index = -1)
        {

            var settingsType = component.GetType();
            Type editorType;

            if (!m_EditorTypes.TryGetValue(settingsType, out editorType))
                editorType = typeof(MainComponentBaseEditor);
            var editor = (MainComponentBaseEditor)Activator.CreateInstance(editorType);
            editor.Init(chart, component, property, m_BaseEditor);

            if (index < 0)
                m_Editors.Add(editor);
            else
                m_Editors[index] = editor;
        }

        public void AddChartComponent(Type type)
        {
            chart.AddChartComponent(type);
            m_ComponentsProperty = m_BaseEditor.RefreshComponent();
            RefreshEditors();
            EditorUtility.SetDirty(chart);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void ResetComponentEditor(int id)
        {
            m_Editors[id].component.Reset();
            EditorUtility.SetDirty(chart);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void RemoveComponentEditor(int id)
        {
            m_Editors[id].OnDisable();
            chart.RemoveChartComponent(m_Editors[id].component);
            m_Editors.RemoveAt(id);
            chart.RebuildChartObject();
            m_ComponentsProperty = m_BaseEditor.RefreshComponent();
            RefreshEditors();
            EditorUtility.SetDirty(chart);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}