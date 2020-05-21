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
    [CustomPropertyDrawer(typeof(Background), true)]
    public class BackgroundDrawer : PropertyDrawer
    {
        private bool m_BackgroundModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_Show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Image = prop.FindPropertyRelative("m_Image");
            SerializedProperty m_ImageType = prop.FindPropertyRelative("m_ImageType");
            // SerializedProperty m_Left = prop.FindPropertyRelative("m_Left");
            // SerializedProperty m_Right = prop.FindPropertyRelative("m_Right");
            // SerializedProperty m_Top = prop.FindPropertyRelative("m_Top");
            // SerializedProperty m_Bottom = prop.FindPropertyRelative("m_Bottom");
            SerializedProperty m_ImageColor = prop.FindPropertyRelative("m_ImageColor");
            SerializedProperty m_HideThemeBackgroundColor = prop.FindPropertyRelative("m_HideThemeBackgroundColor");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_BackgroundModuleToggle, "Background", m_Show);
            EditorGUI.LabelField(drawRect, "Background", EditorStyles.boldLabel);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_BackgroundModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Image);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ImageType);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // EditorGUI.PropertyField(drawRect, m_Left);
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // EditorGUI.PropertyField(drawRect, m_Right);
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // EditorGUI.PropertyField(drawRect, m_Top);
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // EditorGUI.PropertyField(drawRect, m_Bottom);
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ImageColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_HideThemeBackgroundColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_BackgroundModuleToggle)
                return 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
            else
                return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}