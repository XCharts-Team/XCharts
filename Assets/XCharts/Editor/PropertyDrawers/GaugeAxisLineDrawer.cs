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
    [CustomPropertyDrawer(typeof(GaugeAxis.AxisLine), true)]
    public class GaugeAxisLineDrawer : PropertyDrawer
    {
        private bool m_DataFoldout = false;
        private int m_DataSize = 0;
        private Dictionary<string, bool> m_Toggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Width = prop.FindPropertyRelative("m_Width");
            SerializedProperty m_Opacity = prop.FindPropertyRelative("m_Opacity");
            SerializedProperty m_BarBackgroundColor = prop.FindPropertyRelative("m_BarBackgroundColor");
            SerializedProperty m_BarColor = prop.FindPropertyRelative("m_BarColor");
            SerializedProperty m_StageColor = prop.FindPropertyRelative("m_StageColor");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_Toggle, prop, "Axis Line", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_Toggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Width);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Opacity);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BarColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BarBackgroundColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                m_DataFoldout = EditorGUI.Foldout(drawRect, m_DataFoldout, "Stage Color");
                drawRect.width = pos.width;
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_DataFoldout)
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref m_DataSize, m_StageColor);
                }
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (ChartEditorHelper.IsToggle(m_Toggle, prop))
            {
                float height = 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
                if (m_DataFoldout)
                {
                    var arraySize = prop.FindPropertyRelative("m_StageColor").arraySize + 1;
                    height += arraySize * EditorGUIUtility.singleLineHeight + (arraySize) * EditorGUIUtility.standardVerticalSpacing;
                    height += 2 * EditorGUIUtility.standardVerticalSpacing;
                    return height;
                }
                return height;
            }
            else
            {
                return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}