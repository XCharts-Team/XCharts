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
    [CustomPropertyDrawer(typeof(TextStyle), true)]
    public class TextStyleDrawer : PropertyDrawer
    {
        //private Dictionary<string, bool> m_TextStyleToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Rotate = prop.FindPropertyRelative("m_Rotate");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");
            SerializedProperty m_FontSize = prop.FindPropertyRelative("m_FontSize");
            SerializedProperty m_FontStyle = prop.FindPropertyRelative("m_FontStyle");
            SerializedProperty m_Offset = prop.FindPropertyRelative("m_Offset");
            // ChartEditorHelper.MakeFoldout(ref drawRect, ref m_TextStyleToggle, prop, "Text Style");
            // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            // if (ChartEditorHelper.IsToggle(m_TextStyleToggle, prop))
            // {
            //     ++EditorGUI.indentLevel;
            EditorGUI.PropertyField(drawRect, m_Rotate);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_Offset);
            drawRect.y += EditorGUI.GetPropertyHeight(m_Offset);
            EditorGUI.PropertyField(drawRect, m_Color);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_FontSize);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_FontStyle);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //     --EditorGUI.indentLevel;
            // }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            // float height = 0;
            // if (ChartEditorHelper.IsToggle(m_TextStyleToggle, prop))
            // {
            //     height += 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
            // }
            // else
            // {
            //     height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            // }
            float height = 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
            height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Offset"));
            return height;
        }
    }
}