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
    [CustomPropertyDrawer(typeof(VisualMap), true)]
    public class VisualMapDrawer : PropertyDrawer
    {
        private bool m_VisualMapModuleToggle = false;
        private bool m_InRangeFoldout;
        private bool m_OutOfRangeFoldout;
        private int m_InRangeSize;
        private int m_OutOfRangeSize;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Enable = prop.FindPropertyRelative("m_Enable");
            SerializedProperty m_Type = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_Show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_SelectedMode = prop.FindPropertyRelative("m_SelectedMode");
            SerializedProperty m_Min = prop.FindPropertyRelative("m_Min");
            SerializedProperty m_Max = prop.FindPropertyRelative("m_Max");
            SerializedProperty m_Range = prop.FindPropertyRelative("m_Range");
            SerializedProperty m_Text = prop.FindPropertyRelative("m_Text");
            // SerializedProperty m_TextGap = prop.FindPropertyRelative("m_TextGap");
            SerializedProperty m_SplitNumber = prop.FindPropertyRelative("m_SplitNumber");
            SerializedProperty m_Calculable = prop.FindPropertyRelative("m_Calculable");
            SerializedProperty m_ItemWidth = prop.FindPropertyRelative("m_ItemWidth");
            SerializedProperty m_ItemHeight = prop.FindPropertyRelative("m_ItemHeight");
            SerializedProperty m_BorderWidth = prop.FindPropertyRelative("m_BorderWidth");
            SerializedProperty m_Dimension = prop.FindPropertyRelative("m_Dimension");
            SerializedProperty m_HoverLink = prop.FindPropertyRelative("m_HoverLink");
            SerializedProperty m_Orient = prop.FindPropertyRelative("m_Orient");
            SerializedProperty m_Location = prop.FindPropertyRelative("m_Location");
            SerializedProperty m_InRange = prop.FindPropertyRelative("m_InRange");
            // SerializedProperty m_OutOfRange = prop.FindPropertyRelative("m_OutOfRange");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_VisualMapModuleToggle, "Visual Map", m_Enable);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_VisualMapModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(drawRect, m_Min);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Max);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SplitNumber);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Dimension);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_HoverLink);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                m_InRangeFoldout = EditorGUI.Foldout(drawRect, m_InRangeFoldout, "InRange");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_InRangeFoldout)
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref m_InRangeSize, m_InRange);
                }

                // drawRect.width = pos.width;
                // m_OutOfRangeFoldout = EditorGUI.Foldout(drawRect, m_OutOfRangeFoldout, "OutOfRange");
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // if (m_OutOfRangeFoldout)
                // {
                //     ChartEditorHelper.MakeList(ref drawRect, ref m_OutOfRangeSize, m_OutOfRange);
                // }
                // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(drawRect, m_Show);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_Show.boolValue)
                {
                    EditorGUI.PropertyField(drawRect, m_SelectedMode);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    ChartEditorHelper.MakeTwoField(ref drawRect, pos.width, m_Range, "Range");
                    ChartEditorHelper.MakeTwoField(ref drawRect, pos.width, m_Text, "Text");
                    ChartEditorHelper.MakeTwoField(ref drawRect, pos.width, m_Text, "TextGap");
                    EditorGUI.PropertyField(drawRect, m_Calculable);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_ItemWidth);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_ItemHeight);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_BorderWidth);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_Orient);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_Location);
                    drawRect.y += EditorGUI.GetPropertyHeight(m_Location);
                }
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            int num = 1;
            if (m_VisualMapModuleToggle)
            {
                num += 8;
                height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;

                if (m_InRangeFoldout)
                {
                    SerializedProperty m_InRange = prop.FindPropertyRelative("m_InRange");
                    int size = m_InRange.arraySize + 1;
                    height += size * EditorGUIUtility.singleLineHeight + (size - 1) * EditorGUIUtility.standardVerticalSpacing;
                    height += EditorGUIUtility.standardVerticalSpacing;
                }
                // if (m_OutOfRangeFoldout)
                // {
                //     SerializedProperty m_OutOfRange = prop.FindPropertyRelative("m_OutOfRange");
                //     int size = m_OutOfRange.arraySize + 1;
                //     height += size * EditorGUIUtility.singleLineHeight + (size - 1) * EditorGUIUtility.standardVerticalSpacing;
                //     height += EditorGUIUtility.standardVerticalSpacing;
                // }
                if (prop.FindPropertyRelative("m_Show").boolValue)
                {
                    height += 9 * EditorGUIUtility.singleLineHeight + 8 * EditorGUIUtility.standardVerticalSpacing;
                    height += EditorGUI.GetPropertyHeight(prop.FindPropertyRelative("m_Location"));
                }
            }
            else
            {
                height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
            }

            return height;
        }
    }
}