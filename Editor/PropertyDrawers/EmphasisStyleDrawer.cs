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
    [CustomPropertyDrawer(typeof(Emphasis), true)]
    public class EmphasisDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_EmphasisToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Label = prop.FindPropertyRelative("m_Label");
            SerializedProperty m_ItemStyle = prop.FindPropertyRelative("m_ItemStyle");
            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_EmphasisToggle, prop, "Emphasis", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_EmphasisToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Label);
                drawRect.y += EditorGUI.GetPropertyHeight(m_Label);
                EditorGUI.PropertyField(drawRect, m_ItemStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_ItemStyle);
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_EmphasisToggle, prop))
            {
                height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Label"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_ItemStyle"));
            }
            else
            {
                height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}