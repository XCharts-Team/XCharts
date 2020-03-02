/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Title), true)]
    public class TitleDrawer : PropertyDrawer
    {
        private bool m_TitleModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty text = prop.FindPropertyRelative("m_Text");
            SerializedProperty m_TextStyle = prop.FindPropertyRelative("m_TextStyle");
            SerializedProperty subText = prop.FindPropertyRelative("m_SubText");
            SerializedProperty m_SubTextStyle = prop.FindPropertyRelative("m_SubTextStyle");
            SerializedProperty m_ItemGap = prop.FindPropertyRelative("m_ItemGap");
            SerializedProperty location = prop.FindPropertyRelative("m_Location");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_TitleModuleToggle, "Title", show);
            ++EditorGUI.indentLevel;
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_TitleModuleToggle)
            {
                EditorGUI.PropertyField(drawRect, text);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, subText);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ItemGap, new GUIContent("Item Gap"));
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, location);
                drawRect.y += EditorGUI.GetPropertyHeight(location);
                EditorGUI.PropertyField(drawRect, m_TextStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_TextStyle);
                EditorGUI.PropertyField(drawRect, m_SubTextStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_SubTextStyle);
            }
            --EditorGUI.indentLevel;
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (m_TitleModuleToggle)
            {
                height += 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_TextStyle"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_SubTextStyle"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Location"));
            }
            height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}