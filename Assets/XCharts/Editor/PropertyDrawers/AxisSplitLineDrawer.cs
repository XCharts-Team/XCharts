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
    [CustomPropertyDrawer(typeof(AxisSplitLine), true)]
    public class AxisSplitLineDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_AxisSplitLineToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Interval = prop.FindPropertyRelative("m_Interval");
            SerializedProperty m_LineStyle = prop.FindPropertyRelative("m_LineStyle");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AxisSplitLineToggle, prop, "Split Line", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_AxisSplitLineToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Interval);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LineStyle);
                drawRect.y += EditorGUI.GetPropertyHeight(m_LineStyle);
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_AxisSplitLineToggle, prop))
            {
                height += 2 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_LineStyle"));
            }
            else
            {
                height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}