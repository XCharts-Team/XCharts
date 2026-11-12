using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using XCharts.Runtime;

namespace XCharts.Editor
{
    public class UIComponentEditor : UnityEditor.Editor
    {
        class Styles
        {
            public static readonly GUIContent btnAddComponent = new GUIContent("Add Main Component", "");
            public static readonly GUIContent btnRebuildChartObject = new GUIContent("Rebuild Object", "");
            public static readonly GUIContent btnSaveAsImage = new GUIContent("Save As Image", "");
            public static readonly GUIContent btnImportJsonData = new GUIContent("Import Json", "");
            public static readonly GUIContent btnExportJsonData = new GUIContent("Export Json", "");
            public static readonly GUIContent btnCheckWarning = new GUIContent("Check Warning", "");
            public static readonly GUIContent btnHideWarning = new GUIContent("Hide Warning", "");
        }
        public UIComponent m_UIComponent;
        private bool m_ExportPending;

        public static T AddUIComponent<T>(string chartName) where T : UIComponent
        {
            return XChartsEditor.AddGraph<T>(chartName);
        }

        protected Dictionary<string, SerializedProperty> m_Properties = new Dictionary<string, SerializedProperty>();

        protected virtual void OnEnable()
        {
            m_Properties.Clear();
            m_UIComponent = (UIComponent) target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            PropertyField("m_Script");

            OnStartInspectorGUI();
            OnDebugInspectorGUI();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void OnStartInspectorGUI() { }

        protected virtual void OnDebugInspectorGUI()
        {
            EditorGUILayout.Space();
            PropertyField("m_DebugModel");
            OnDebugStartInspectorGUI();
            if (GUILayout.Button(Styles.btnRebuildChartObject))
            {
                m_UIComponent.RebuildChartObject();
            }
            if (GUILayout.Button(Styles.btnSaveAsImage))
            {
                m_UIComponent.SaveAsImage("png", "", 4f);
            }
            if (GUILayout.Button(Styles.btnImportJsonData))
            {
                UIComponentJsonImportWindow.ShowWindow(m_UIComponent);
            }
            if (GUILayout.Button(Styles.btnExportJsonData))
            {
                RequestExportJsonData();
            }
            OnDebugEndInspectorGUI();
        }

        protected virtual void OnDebugStartInspectorGUI() { }
        protected virtual void OnDebugEndInspectorGUI() { }

        protected void PropertyField(string name)
        {
            if (!m_Properties.ContainsKey(name))
            {
                var prop = serializedObject.FindProperty(name);
                if (prop == null)
                {
                    Debug.LogError("Property " + name + " not found!");
                    return;
                }
                m_Properties.Add(name, prop);
            }
            EditorGUILayout.PropertyField(m_Properties[name]);
        }

        protected void PropertyField(SerializedProperty property)
        {
            Assert.IsNotNull(property);
            var title = ChartEditorHelper.GetContent(property.displayName);
            PropertyField(property, title);
        }

        protected void PropertyField(SerializedProperty property, GUIContent title)
        {
            EditorGUILayout.PropertyField(property, title);
        }

        private void RequestExportJsonData()
        {
            if (m_ExportPending) return;
            m_ExportPending = true;
            var target = m_UIComponent;
            EditorApplication.delayCall += delegate ()
            {
                m_ExportPending = false;
                ExportJsonData(target);
            };
            GUIUtility.ExitGUI();
        }

        private static void ExportJsonData(UIComponent target)
        {
            if (target == null) return;
            var json = target.ExportToJson(true);
            var defaultName = target.gameObject.name + ".json";
            var path = EditorUtility.SaveFilePanel("Save UI Component JSON", "", defaultName, "json");
            if (string.IsNullOrEmpty(path)) return;
            try
            {
                System.IO.File.WriteAllText(path, json);
                Debug.Log("[XCharts] UI JSON exported to: " + path);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("[XCharts] Failed to save UI JSON: " + ex.Message);
            }
        }

        protected void PropertyListField(string relativePropName, bool showOrder = true, params HeaderMenuInfo[] menus)
        {
            var m_DrawRect = GUILayoutUtility.GetRect(1f, 17f);
            var height = 0f;
            var prop = FindProperty(relativePropName);
            prop.isExpanded = ChartEditorHelper.MakeListWithFoldout(ref m_DrawRect, ref height,
                prop, prop.isExpanded, showOrder, true, menus);
            if (prop.isExpanded)
            {
                GUILayoutUtility.GetRect(1f, height - 17);
            }
        }

        protected void PropertyTwoFiled(string relativePropName)
        {
            var m_DrawRect = GUILayoutUtility.GetRect(1f, 17f);
            var prop = FindProperty(relativePropName);
            ChartEditorHelper.MakeTwoField(ref m_DrawRect, m_DrawRect.width, prop, prop.displayName);
        }

        protected SerializedProperty FindProperty(string path)
        {
            if (!m_Properties.ContainsKey(path))
            {
                m_Properties.Add(path, serializedObject.FindProperty(path));
            }
            return m_Properties[path];
        }
    }
}