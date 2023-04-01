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
            public static readonly GUIContent btnCheckWarning = new GUIContent("Check Warning", "");
            public static readonly GUIContent btnHideWarning = new GUIContent("Hide Warning", "");
        }
        public UIComponent m_UIComponent;

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
                m_UIComponent.SaveAsImage();
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