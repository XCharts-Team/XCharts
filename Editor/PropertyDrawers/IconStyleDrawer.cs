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
    [CustomPropertyDrawer(typeof(IconStyle), true)]
    public class IconStyleDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_IconStyleToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Layer = prop.FindPropertyRelative("m_Layer");
            SerializedProperty m_Sprite = prop.FindPropertyRelative("m_Sprite");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");
            SerializedProperty m_Width = prop.FindPropertyRelative("m_Width");
            SerializedProperty m_Height = prop.FindPropertyRelative("m_Height");
            SerializedProperty m_Offset = prop.FindPropertyRelative("m_Offset");
            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_IconStyleToggle, prop, null, m_Show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_IconStyleToggle, prop))
            {
                ++EditorGUI.indentLevel;

                EditorGUI.PropertyField(drawRect, m_Layer);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Sprite);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Color);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Width);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Height);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Offset);
                drawRect.y += EditorGUI.GetPropertyHeight(m_Offset);
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (ChartEditorHelper.IsToggle(m_IconStyleToggle, prop))
            {
                var hight = 6 * EditorGUIUtility.singleLineHeight + 6 * EditorGUIUtility.standardVerticalSpacing;
                hight += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Offset"));
                hight += EditorGUIUtility.standardVerticalSpacing;
                return hight;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}