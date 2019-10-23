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
    [CustomPropertyDrawer(typeof(ItemStyle), true)]
    public class ItemStyleDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_ItemStyleToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");
            SerializedProperty m_BorderType = prop.FindPropertyRelative("m_BorderType");
            SerializedProperty m_BorderWidth = prop.FindPropertyRelative("m_BorderWidth");
            SerializedProperty m_BorderColor = prop.FindPropertyRelative("m_BorderColor");
            SerializedProperty m_Opacity = prop.FindPropertyRelative("m_Opacity");
            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_ItemStyleToggle, prop, "Item Style", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_ItemStyleToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Color);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BorderType);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BorderWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BorderColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Opacity);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_ItemStyleToggle, prop))
            {
                height += 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}