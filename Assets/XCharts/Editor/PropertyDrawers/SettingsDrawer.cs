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
    [CustomPropertyDrawer(typeof(Settings), true)]
    public class SettingsDrawer : PropertyDrawer
    {
        private bool m_SettingsModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_LineSmoothStyle = prop.FindPropertyRelative("m_LineSmoothStyle");
            SerializedProperty m_LineSmoothness = prop.FindPropertyRelative("m_LineSmoothness");
            SerializedProperty m_LineSegmentDistance = prop.FindPropertyRelative("m_LineSegmentDistance");
            SerializedProperty m_CicleSmoothness = prop.FindPropertyRelative("m_CicleSmoothness");
            SerializedProperty m_VisualMapTriangeLen = prop.FindPropertyRelative("m_VisualMapTriangeLen");
            SerializedProperty m_PieTooltipExtraRadius = prop.FindPropertyRelative("m_PieTooltipExtraRadius");
            SerializedProperty m_PieSelectedOffset = prop.FindPropertyRelative("m_PieSelectedOffset");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_SettingsModuleToggle, "Settings");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_SettingsModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_LineSmoothStyle);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LineSmoothness);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LineSegmentDistance);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_CicleSmoothness);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_VisualMapTriangeLen);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_PieTooltipExtraRadius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_PieSelectedOffset);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            int num = 1;
            if (m_SettingsModuleToggle)
            {
                num = 8;
            }
            return num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}