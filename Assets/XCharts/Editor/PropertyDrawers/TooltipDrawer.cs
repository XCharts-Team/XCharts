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
    [CustomPropertyDrawer(typeof(Tooltip), true)]
    public class TooltipDrawer : PropertyDrawer
    {
        private bool m_TooltipModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty type = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_Formatter = prop.FindPropertyRelative("m_Formatter");
            SerializedProperty m_TitleFormatter = prop.FindPropertyRelative("m_TitleFormatter");
            SerializedProperty m_ItemFormatter = prop.FindPropertyRelative("m_ItemFormatter");
            SerializedProperty m_FixedWidth = prop.FindPropertyRelative("m_FixedWidth");
            SerializedProperty m_FixedHeight = prop.FindPropertyRelative("m_FixedHeight");
            SerializedProperty m_MinWidth = prop.FindPropertyRelative("m_MinWidth");
            SerializedProperty m_MinHeight = prop.FindPropertyRelative("m_MinHeight");
            SerializedProperty m_NumericFormatter = prop.FindPropertyRelative("m_NumericFormatter");
            SerializedProperty m_PaddingLeftRight = prop.FindPropertyRelative("m_PaddingLeftRight");
            SerializedProperty m_PaddingTopBottom = prop.FindPropertyRelative("m_PaddingTopBottom");
            SerializedProperty m_BackgroundImage = prop.FindPropertyRelative("m_BackgroundImage");
            SerializedProperty m_IgnoreDataDefaultContent = prop.FindPropertyRelative("m_IgnoreDataDefaultContent");
            SerializedProperty m_LineStyle = prop.FindPropertyRelative("m_LineStyle");
            SerializedProperty m_TextStyle = prop.FindPropertyRelative("m_TextStyle");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_TooltipModuleToggle, "Tooltip", show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_TooltipModuleToggle)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(drawRect, type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Formatter);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_TitleFormatter);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ItemFormatter);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_NumericFormatter);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FixedWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FixedHeight);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_MinWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_MinHeight);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_PaddingLeftRight);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_PaddingTopBottom);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BackgroundImage);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_IgnoreDataDefaultContent);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LineStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_LineStyle);
                EditorGUI.PropertyField(drawRect, m_TextStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_TextStyle);
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_TooltipModuleToggle)
                return 14 * EditorGUIUtility.singleLineHeight + 13 * EditorGUIUtility.standardVerticalSpacing +
                    EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_LineStyle")) +
                    EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_TextStyle"));
            else
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}