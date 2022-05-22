using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using XCharts.Runtime;

namespace XCharts.Editor
{
    public class SerieBaseEditor
    {
        internal BaseChart chart { get; private set; }
        internal Serie serie { get; private set; }

        //Editor m_Inspector;
        internal SerializedProperty baseProperty;
        internal SerializedProperty showProperty;
        internal List<HeaderMenuInfo> menus = new List<HeaderMenuInfo>();
        internal List<HeaderMenuInfo> serieDataMenus = new List<HeaderMenuInfo>();
        protected Dictionary<string, Type> m_CoordOptionsDic;
        protected List<string> m_CoordOptionsNames;
        private string m_DisplayName;

        internal void Init(BaseChart chart, Serie target, SerializedProperty property, UnityEditor.Editor inspector)
        {
            this.chart = chart;
            this.serie = target;
            this.baseProperty = property;
            m_DisplayName = string.Format("Serie {0}: {1}", serie.index, serie.GetType().Name);
            //m_Inspector = inspector;
            showProperty = baseProperty.FindPropertyRelative("m_Show");
            if (showProperty == null)
                showProperty = baseProperty.FindPropertyRelative("m_Enable");
            OnEnable();

            if (serie.GetType().IsDefined(typeof(CoordOptionsAttribute), false))
            {
                var attribute = serie.GetType().GetAttribute<CoordOptionsAttribute>();
                m_CoordOptionsDic = new Dictionary<string, Type>();
                m_CoordOptionsNames = new List<string>();
                if (attribute.type0 != null)
                {
                    m_CoordOptionsDic[attribute.type0.Name] = attribute.type0;
                    m_CoordOptionsNames.Add(attribute.type0.Name);
                }
                if (attribute.type1 != null)
                {
                    m_CoordOptionsDic[attribute.type1.Name] = attribute.type1;
                    m_CoordOptionsNames.Add(attribute.type1.Name);
                }
                if (attribute.type2 != null)
                {
                    m_CoordOptionsDic[attribute.type2.Name] = attribute.type2;
                    m_CoordOptionsNames.Add(attribute.type2.Name);
                }
                if (attribute.type3 != null)
                {
                    m_CoordOptionsDic[attribute.type3.Name] = attribute.type3;
                    m_CoordOptionsNames.Add(attribute.type3.Name);
                }
            }
        }

        public virtual void OnEnable()
        { }

        public virtual void OnDisable()
        { }

        internal void OnInternalInspectorGUI()
        {
            OnInspectorGUI();
            EditorGUILayout.Space();
        }

        public virtual void OnInspectorGUI()
        { }

        protected virtual void DrawExtendeds()
        { }

        public virtual string GetDisplayTitle()
        {
            // var title = string.Format("serie {0}: {1}", serie.index, serie.GetType().Name);
            // return ObjectNames.NicifyVariableName(title);
            return m_DisplayName;
        }

        internal SerializedProperty FindProperty(string path)
        {
            return baseProperty.FindPropertyRelative(path);
        }

        protected SerializedProperty PropertyField(string path)
        {
            Assert.IsNotNull(path);
            var property = FindProperty(path);
            Assert.IsNotNull(property, "Can't find:" + path);
            var title = ChartEditorHelper.GetContent(property.displayName);
            PropertyField(property, title);
            return property;
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

        protected void PropertyListField(string relativePropName, bool showOrder = true)
        {
            //TODO:
            PropertyField(relativePropName);
        }

        protected void PropertyTwoFiled(string relativePropName)
        {
            var m_DrawRect = GUILayoutUtility.GetRect(1f, 17f);
            var prop = FindProperty(relativePropName);
            ChartEditorHelper.MakeTwoField(ref m_DrawRect, m_DrawRect.width, prop, prop.displayName);
        }
        protected void PropertyFieldLimitMin(string relativePropName, double min)
        {
            var prop = PropertyField(relativePropName);
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Float:
                    if (prop.floatValue < min)
                        prop.floatValue = (float) min;
                    break;
                case SerializedPropertyType.Integer:
                    if (prop.intValue < min)
                        prop.intValue = (int) min;
                    break;
            }

        }
        protected void PropertyFieldLimitMax(string relativePropName, int max)
        {
            var prop = PropertyField(relativePropName);
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Float:
                    if (prop.floatValue > max)
                        prop.floatValue = (float) max;
                    break;
                case SerializedPropertyType.Integer:
                    if (prop.intValue > max)
                        prop.intValue = (int) max;
                    break;
            }
        }
    }
}