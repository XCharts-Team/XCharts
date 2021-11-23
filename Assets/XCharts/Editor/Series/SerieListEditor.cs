/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace XCharts
{
    public sealed class SerieListEditor
    {
        public BaseChart chart { get; private set; }
        BaseChartEditor m_BaseEditor;

        SerializedObject m_SerializedObject;
        List<SerializedProperty> m_SeriesProperty;
        SerializedProperty m_EnableProperty;

        Dictionary<Type, Type> m_EditorTypes;
        List<SerieBaseEditor> m_Editors;


        public SerieListEditor(BaseChartEditor editor)
        {
            Assert.IsNotNull(editor);
            m_BaseEditor = editor;
        }

        public void Init(BaseChart chart, SerializedObject serializedObject, List<SerializedProperty> componentProps)
        {
            Assert.IsNotNull(chart);
            Assert.IsNotNull(serializedObject);

            this.chart = chart;
            m_SerializedObject = serializedObject;
            m_SeriesProperty = componentProps;

            m_Editors = new List<SerieBaseEditor>();
            m_EditorTypes = new Dictionary<Type, Type>();

            var editorTypes = RuntimeUtil.GetAllTypesDerivedFrom<SerieBaseEditor>()
                .Where(t => t.IsDefined(typeof(SerieEditorAttribute), false) && !t.IsAbstract);
            foreach (var editorType in editorTypes)
            {
                var attribute = editorType.GetAttribute<SerieEditorAttribute>();
                m_EditorTypes.Add(attribute.serieType, editorType);
            }

            RefreshEditors();
        }

        public void UpdateSeriesProperty(List<SerializedProperty> componentProps)
        {
            m_SeriesProperty = componentProps;
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
                var menus = new List<HeaderMenuInfo>();
                menus.Add(new HeaderMenuInfo("Remove", () => { RemoveSerieEditor(id); }));
                if (i < m_Editors.Count - 1)
                    menus.Add(new HeaderMenuInfo("Move Down", () =>
                    {
                        if (chart.MoveDownSerie(id))
                        {
                            m_SeriesProperty = m_BaseEditor.RefreshSeries();
                            RefreshEditors();
                        }
                    }));
                if (i > 0)
                    menus.Add(new HeaderMenuInfo("Move Up", () =>
                    {
                        if (chart.MoveUpSerie(id))
                        {
                            m_SeriesProperty = m_BaseEditor.RefreshSeries();
                            RefreshEditors();
                        }
                    }));
                bool displayContent = ChartEditorHelper.DrawHeader(
                    title,
                    editor.baseProperty,
                    editor.showProperty,
                    menus.ToArray());
                if (displayContent)
                {
                    editor.OnInternalInspectorGUI();
                }
            }
            if (m_Editors.Count > 0)
            {
                //EditorGUILayout.Space();
            }
            else
            {
                EditorGUILayout.HelpBox("No serie.", MessageType.Info);
            }
            //EditorGUILayout.Space();
        }

        void RefreshEditors()
        {
            foreach (var editor in m_Editors)
                editor.OnDisable();

            m_Editors.Clear();

            for (int i = 0; i < chart.series.Count; i++)
            {
                if (chart.series[i] != null)
                    CreateEditor(chart.series[i], m_SeriesProperty[i]);
            }
        }

        void CreateEditor(Serie serie, SerializedProperty property, int index = -1)
        {

            var settingsType = serie.GetType();
            Type editorType;

            if (!m_EditorTypes.TryGetValue(settingsType, out editorType))
                editorType = typeof(SerieBaseEditor);
            var editor = (SerieBaseEditor)Activator.CreateInstance(editorType);
            editor.Init(chart, serie, property, m_BaseEditor);

            if (index < 0)
                m_Editors.Add(editor);
            else
                m_Editors[index] = editor;
        }

        public void AddSerie(Type type)
        {
            m_SerializedObject.Update();
            var serieName = chart.GenerateDefaultSerieName();
            type.InvokeMember("AddDefaultSerie",
                BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.Public, null, null,
                new object[] { chart, serieName });
            m_SerializedObject.Update();
            m_SerializedObject.ApplyModifiedProperties();
            m_SeriesProperty = m_BaseEditor.RefreshSeries();
            RefreshEditors();
            EditorUtility.SetDirty(chart);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private void RemoveSerieEditor(int id)
        {
            m_Editors[id].OnDisable();
            chart.RemoveSerie(m_Editors[id].serie);
            m_Editors.RemoveAt(id);
            m_SerializedObject.Update();
            m_SerializedObject.ApplyModifiedProperties();
            m_SeriesProperty = m_BaseEditor.RefreshSeries();
            RefreshEditors();
            EditorUtility.SetDirty(chart);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}