/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace XCharts
{
    public class MainComponentBaseEditor
    {
        internal BaseChart chart { get; private set; }
        internal MainComponent component { get; private set; }

        //Editor m_Inspector;
        internal SerializedProperty baseProperty;
        internal SerializedProperty showProperty;

        internal void Init(BaseChart chart, MainComponent target, SerializedProperty property, Editor inspector)
        {
            this.chart = chart;
            this.component = target;
            this.baseProperty = property;
            //m_Inspector = inspector;
            showProperty = baseProperty.FindPropertyRelative("m_Show");
            if (showProperty == null)
                showProperty = baseProperty.FindPropertyRelative("m_Enable");
            OnEnable();
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        internal void OnInternalInspectorGUI()
        {
            OnInspectorGUI();
            EditorGUILayout.Space();
        }

        public virtual void OnInspectorGUI()
        {
        }

        protected virtual void DrawExtendeds()
        {
        }

        public virtual string GetDisplayTitle()
        {
            var num = chart.GetChartComponentNum(component.GetType());
            if (num > 1)
                return ObjectNames.NicifyVariableName(component.GetType().Name) + " " + component.index;
            else
                return ObjectNames.NicifyVariableName(component.GetType().Name);
        }

        protected SerializedProperty FindProperty(string path)
        {
            return baseProperty.FindPropertyRelative(path);
        }

        protected void PropertyField(string path)
        {
            var property = FindProperty(path);
            if (property != null)
            {
                var title = ChartEditorHelper.GetContent(property.displayName);
                PropertyField(property, title);
            }
            else
            {
                Debug.LogError("Property not exist:" + baseProperty.propertyPath + "," + path);
            }
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
    }
}