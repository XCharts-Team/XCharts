/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(TextLimit), true)]
    public class TextLimitDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_TextLimitToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Enable = prop.FindPropertyRelative("m_Enable");
            SerializedProperty m_MaxWidth = prop.FindPropertyRelative("m_MaxWidth");
            SerializedProperty m_Gap = prop.FindPropertyRelative("m_Gap");
            SerializedProperty m_LimitSuffix = prop.FindPropertyRelative("m_Suffix");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_TextLimitToggle, prop, "Text Limit", m_Enable, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_TextLimitToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_MaxWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Gap);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LimitSuffix);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_TextLimitToggle, prop))
            {
                height += 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}