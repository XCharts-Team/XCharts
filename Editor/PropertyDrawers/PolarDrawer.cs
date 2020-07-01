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
    [CustomPropertyDrawer(typeof(Polar), true)]
    public class PolarDrawer : PropertyDrawer
    {
        private bool m_PolarModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Center = prop.FindPropertyRelative("m_Center");
            SerializedProperty m_Radius = prop.FindPropertyRelative("m_Radius");
            SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_PolarModuleToggle, "Polar", show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_PolarModuleToggle)
            {
                EditorGUI.indentLevel++;
                ChartEditorHelper.MakeTwoField(ref drawRect, pos.width, m_Center, "Center");
                EditorGUI.PropertyField(drawRect, m_Radius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BackgroundColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_PolarModuleToggle)
                return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}