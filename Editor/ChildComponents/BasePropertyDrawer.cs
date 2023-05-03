using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts.Editor
{
    public delegate void DelegateMenuAction(Vector2 postion);
    public class BasePropertyDrawer : PropertyDrawer
    {
        protected int m_Index;
        protected int m_DataSize;
        protected float m_DefaultWidth;
        protected string m_DisplayName;
        protected string m_KeyName;
        protected Rect m_DrawRect;
        protected Dictionary<string, float> m_Heights = new Dictionary<string, float>();
        protected Dictionary<string, bool> m_PropToggles = new Dictionary<string, bool>();
        protected Dictionary<string, bool> m_DataToggles = new Dictionary<string, bool>();

        public virtual string ClassName { get { return ""; } }
        public virtual List<string> IngorePropertys { get { return new List<string> { }; } }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            m_DrawRect = pos;
            m_DrawRect.height = EditorGUIUtility.singleLineHeight;
            m_DefaultWidth = pos.width;
            var list = prop.displayName.Split(' ');
            if (list.Length > 0)
            {
                if (!int.TryParse(list[list.Length - 1], out m_Index))
                {
                    m_Index = 0;
                    m_DisplayName = prop.displayName;
                    m_KeyName = prop.propertyPath + "_" + m_Index;
                }
                else
                {
                    m_DisplayName = ClassName + " " + m_Index;
                    m_KeyName = prop.propertyPath + "_" + m_Index;
                }
            }
            else
            {
                m_DisplayName = prop.displayName;
            }
            if (!m_PropToggles.ContainsKey(m_KeyName))
            {
                m_PropToggles.Add(m_KeyName, false);
            }
            if (!m_DataToggles.ContainsKey(m_KeyName))
            {
                m_DataToggles.Add(m_KeyName, false);
            }
            if (!m_Heights.ContainsKey(m_KeyName))
            {
                m_Heights.Add(m_KeyName, 0);
            }
            else
            {
                m_Heights[m_KeyName] = 0;
            }
        }

        private string GetKeyName(SerializedProperty prop)
        {
            var index = 0;
            var list = prop.displayName.Split(' ');
            if (list.Length > 0)
            {
                int.TryParse(list[list.Length - 1], out index);
            }
            return prop.propertyPath + "_" + index;
        }

        protected void AddHelpBox(string message, MessageType type = MessageType.Warning, int line = 2)
        {
            var offset = EditorGUI.indentLevel * ChartEditorHelper.INDENT_WIDTH;
            EditorGUI.HelpBox(new Rect(m_DrawRect.x + offset, m_DrawRect.y, m_DrawRect.width - offset, EditorGUIUtility.singleLineHeight * line), message, type);
            for (int i = 0; i < line; i++)
                AddSingleLineHeight();
        }

        protected void AddSingleLineHeight()
        {
            m_Heights[m_KeyName] += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            m_DrawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        protected void AddHeight(float height)
        {
            m_Heights[m_KeyName] += height;
            m_DrawRect.y += height;
        }

        protected void PropertyListField(SerializedProperty prop, string relativePropName, bool showOrder = true)
        {
            if (IngorePropertys.Contains(relativePropName)) return;
            var height = m_Heights[m_KeyName];
            var toggleKeyName = m_KeyName + relativePropName;
            m_DataToggles[toggleKeyName] = ChartEditorHelper.MakeListWithFoldout(ref m_DrawRect, ref height,
                prop.FindPropertyRelative(relativePropName),
                m_DataToggles.ContainsKey(toggleKeyName) && m_DataToggles[toggleKeyName], showOrder, true);
            m_Heights[m_KeyName] = height;
        }

        protected void PropertyField(SerializedProperty prop, string relativePropName)
        {
            if (IngorePropertys.Contains(relativePropName)) return;
            if (!ChartEditorHelper.PropertyField(ref m_DrawRect, m_Heights, m_KeyName, prop, relativePropName))
            {
                Debug.LogError("PropertyField ERROR:" + prop.displayName + ", " + relativePropName);
            }
        }

        protected void PropertyFieldLimitMin(SerializedProperty prop, string relativePropName, float minValue)
        {
            if (IngorePropertys.Contains(relativePropName)) return;
            if (!ChartEditorHelper.PropertyFieldWithMinValue(ref m_DrawRect, m_Heights, m_KeyName, prop,
                    relativePropName, minValue))
            {
                Debug.LogError("PropertyField ERROR:" + prop.displayName + ", " + relativePropName);
            }
        }
        protected void PropertyFieldLimitMax(SerializedProperty prop, string relativePropName, float maxValue)
        {
            if (IngorePropertys.Contains(relativePropName)) return;
            if (!ChartEditorHelper.PropertyFieldWithMaxValue(ref m_DrawRect, m_Heights, m_KeyName, prop,
                    relativePropName, maxValue))
            {
                Debug.LogError("PropertyField ERROR:" + prop.displayName + ", " + relativePropName);
            }
        }

        protected void PropertyField(SerializedProperty prop, SerializedProperty relativeProp)
        {
            if (!ChartEditorHelper.PropertyField(ref m_DrawRect, m_Heights, m_KeyName, relativeProp))
            {
                Debug.LogError("PropertyField ERROR:" + prop.displayName + ", " + relativeProp);
            }
        }

        protected void PropertyTwoFiled(SerializedProperty prop, string relativeListProp, string labelName = null)
        {
            PropertyTwoFiled(prop, prop.FindPropertyRelative(relativeListProp), labelName);
        }
        protected void PropertyTwoFiled(SerializedProperty prop, SerializedProperty relativeListProp,
            string labelName = null)
        {
            if (string.IsNullOrEmpty(labelName))
            {
                labelName = relativeListProp.displayName;
            }
            ChartEditorHelper.MakeTwoField(ref m_DrawRect, m_DefaultWidth, relativeListProp, labelName);
            m_Heights[m_KeyName] += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        protected bool MakeFoldout(SerializedProperty prop, string relativePropName)
        {
            if (string.IsNullOrEmpty(relativePropName))
            {
                return ChartEditorHelper.MakeFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName,
                    m_DisplayName, null);
            }
            else
            {
                var relativeProp = prop.FindPropertyRelative(relativePropName);
                return ChartEditorHelper.MakeFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName,
                    m_DisplayName, relativeProp);
            }
        }
        protected bool MakeComponentFoldout(SerializedProperty prop, string relativePropName, bool relativePropEnable,
            params HeaderMenuInfo[] menus)
        {
            if (string.IsNullOrEmpty(relativePropName))
            {
                return ChartEditorHelper.MakeComponentFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName,
                    m_DisplayName, null, null, relativePropEnable, menus);
            }
            else
            {
                var relativeProp = prop.FindPropertyRelative(relativePropName);
                return ChartEditorHelper.MakeComponentFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName,
                    m_DisplayName, relativeProp, null, relativePropEnable, menus);
            }
        }

        protected bool MakeComponentFoldout(SerializedProperty prop, string relativePropName, string relativePropName2,
            bool relativePropEnable, params HeaderMenuInfo[] menus)
        {
            if (string.IsNullOrEmpty(relativePropName))
            {
                return ChartEditorHelper.MakeComponentFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName,
                    m_DisplayName, null, null, relativePropEnable, menus);
            }
            else
            {
                var relativeProp = prop.FindPropertyRelative(relativePropName);
                var relativeProp2 = prop.FindPropertyRelative(relativePropName2);
                return ChartEditorHelper.MakeComponentFoldout(ref m_DrawRect, m_Heights, m_PropToggles, m_KeyName,
                    m_DisplayName, relativeProp, relativeProp2, relativePropEnable, menus);
            }
        }

        protected virtual void DrawExtendeds(SerializedProperty prop) { }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            var key = GetKeyName(prop);
            if (m_Heights.ContainsKey(key)) return m_Heights[key] + GetExtendedHeight();
            else return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        protected virtual float GetExtendedHeight()
        {
            return 0;
        }
    }
}