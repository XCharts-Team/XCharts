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
    [CustomPropertyDrawer(typeof(GaugeAxis), true)]
    public class GaugeAxisDrawer : PropertyDrawer
    {
        private bool m_DataFoldout = false;
        private int m_DataSize = 0;
        private Dictionary<string, bool> m_AxisLineToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_AxisLine = prop.FindPropertyRelative("m_AxisLine");
            SerializedProperty m_SplitLine = prop.FindPropertyRelative("m_SplitLine");
            SerializedProperty m_AxisTick = prop.FindPropertyRelative("m_AxisTick");
            SerializedProperty m_AxisLabel = prop.FindPropertyRelative("m_AxisLabel");
            SerializedProperty m_AxisLabelText = prop.FindPropertyRelative("m_AxisLabelText");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AxisLineToggle, prop, "Gauge Axis", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_AxisLineToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_AxisLine);
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisLine);
                EditorGUI.PropertyField(drawRect, m_SplitLine);
                drawRect.y += EditorGUI.GetPropertyHeight(m_SplitLine);
                EditorGUI.PropertyField(drawRect, m_AxisTick);
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisTick);
                EditorGUI.PropertyField(drawRect, m_AxisLabel);
                drawRect.y += EditorGUI.GetPropertyHeight(m_AxisLabel);
                drawRect.width = EditorGUIUtility.labelWidth + 10;
                m_DataFoldout = EditorGUI.Foldout(drawRect, m_DataFoldout, "Axis Label Text");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.width = pos.width;
                if (m_DataFoldout)
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref m_DataSize, m_AxisLabelText);
                }
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_AxisLineToggle, prop))
            {
                height += 2 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_AxisLine"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_SplitLine"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_AxisTick"));
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_AxisLabel"));
                if (m_DataFoldout)
                {
                    SerializedProperty m_Data = prop.FindPropertyRelative("m_AxisLabelText");
                    int num = m_Data.arraySize + 1;
                    height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
                    height += EditorGUIUtility.standardVerticalSpacing;
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