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
    [CustomPropertyDrawer(typeof(Vessel), true)]
    public class VesselDrawer : PropertyDrawer
    {
        private bool m_VesselModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Shape = prop.FindPropertyRelative("m_Shape");
            SerializedProperty m_Center = prop.FindPropertyRelative("m_Center");
            SerializedProperty m_Radius = prop.FindPropertyRelative("m_Radius");
            SerializedProperty m_ShapeWidth = prop.FindPropertyRelative("m_ShapeWidth");
            SerializedProperty m_Gap = prop.FindPropertyRelative("m_Gap");
            SerializedProperty m_Smoothness = prop.FindPropertyRelative("m_Smoothness");
            SerializedProperty m_AutoColor = prop.FindPropertyRelative("m_AutoColor");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");
            SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");

            int index = ChartEditorHelper.GetIndexFromPath(prop);
            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_VesselModuleToggle, "Vessel " + index, show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_VesselModuleToggle)
            {
                EditorGUI.indentLevel++;
                EditorGUI.PropertyField(drawRect, m_Shape);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ShapeWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Gap);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                ChartEditorHelper.MakeTwoField(ref drawRect, pos.width, m_Center, "Center");
                EditorGUI.PropertyField(drawRect, m_Radius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BackgroundColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Color);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_AutoColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Smoothness);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.indentLevel--;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_VesselModuleToggle)
                return 10 * EditorGUIUtility.singleLineHeight + 9 * EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}