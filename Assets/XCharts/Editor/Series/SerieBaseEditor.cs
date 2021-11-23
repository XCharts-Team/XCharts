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
    public class SerieBaseEditor
    {
        internal BaseChart chart { get; private set; }
        internal Serie serie { get; private set; }

        //Editor m_Inspector;
        internal SerializedProperty baseProperty;
        internal SerializedProperty showProperty;
        protected Dictionary<string, Type> m_CoordOptionsDic;
        protected List<string> m_CoordOptionsNames;

        internal void Init(BaseChart chart, Serie target, SerializedProperty property, Editor inspector)
        {
            this.chart = chart;
            this.serie = target;
            this.baseProperty = property;
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
            var title = string.Format("serie {0}: {1}", serie.index, serie.GetType().Name);
            return ObjectNames.NicifyVariableName(title);
        }

        protected SerializedProperty FindProperty(string path)
        {
            return baseProperty.FindPropertyRelative(path);
        }

        protected void PropertyField(string path)
        {
            Assert.IsNotNull(path);
            var property = FindProperty(path);
            Assert.IsNotNull(property, "Can't find:" + path);
            var title = ChartEditorHelper.GetContent(property.displayName);
            PropertyField(property, title);
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
            //TODO:
            PropertyField(relativePropName);
        }
        protected void PropertyFieldLimitMin(string relativePropName, double value)
        {
            //TODO:
            PropertyField(relativePropName);
        }
        protected void PropertyFieldLimitMax(string relativePropName, double value)
        {
            //TODO:
            PropertyField(relativePropName);
        }
    }
}