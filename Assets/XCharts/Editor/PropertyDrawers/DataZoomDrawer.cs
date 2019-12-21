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
    [CustomPropertyDrawer(typeof(DataZoom), true)]
    public class DataZoomDrawer : PropertyDrawer
    {
        private bool m_DataZoomModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Enable");
            //SerializedProperty m_FilterMode = prop.FindPropertyRelative("m_FilterMode");
            //SerializedProperty m_Orient = prop.FindPropertyRelative("m_Orient");
            SerializedProperty m_SupportInside = prop.FindPropertyRelative("m_SupportInside");
            SerializedProperty m_SupportSlider = prop.FindPropertyRelative("m_SupportSlider");
            //SerializedProperty m_SupportSelect = prop.FindPropertyRelative("m_SupportSelect");
            SerializedProperty m_ShowDataShadow = prop.FindPropertyRelative("m_ShowDataShadow");
            SerializedProperty m_ShowDetail = prop.FindPropertyRelative("m_ShowDetail");
            SerializedProperty m_ZoomLock = prop.FindPropertyRelative("m_ZoomLock");
            // SerializedProperty m_Realtime = prop.FindPropertyRelative("m_Realtime");
            // SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");
            SerializedProperty m_Height = prop.FindPropertyRelative("m_Height");
            SerializedProperty m_Bottom = prop.FindPropertyRelative("m_Bottom");
            SerializedProperty m_RangeMode = prop.FindPropertyRelative("m_RangeMode");
            SerializedProperty m_Start = prop.FindPropertyRelative("m_Start");
            SerializedProperty m_End = prop.FindPropertyRelative("m_End");
            SerializedProperty m_MinShowNum = prop.FindPropertyRelative("m_MinShowNum");
            SerializedProperty m_ScrollSensitivity = prop.FindPropertyRelative("m_ScrollSensitivity");
            SerializedProperty m_FontSize = prop.FindPropertyRelative("m_FontSize");
            SerializedProperty m_FontStyle = prop.FindPropertyRelative("m_FontStyle");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_DataZoomModuleToggle, "DataZoom", show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_DataZoomModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_SupportInside);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SupportSlider);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_SupportSlider.boolValue)
                {
                    ++EditorGUI.indentLevel;
                    EditorGUI.PropertyField(drawRect, m_ShowDataShadow);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_ShowDetail);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_FontSize);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_FontStyle);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    // EditorGUI.PropertyField(drawRect, m_Realtime);
                    // drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_Height);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_Bottom);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    --EditorGUI.indentLevel;
                }
                //EditorGUI.PropertyField(drawRect, m_SupportSelect);
                //drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ZoomLock);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ScrollSensitivity);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_RangeMode);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Start);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_End);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_MinShowNum);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_Start.floatValue < 0) m_Start.floatValue = 0;
                if (m_End.floatValue > 100) m_End.floatValue = 100;
                if (m_MinShowNum.intValue < 0) m_MinShowNum.intValue = 0;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            int num = 1;
            if (m_DataZoomModuleToggle)
            {
                num += 8;
                if (prop.FindPropertyRelative("m_SupportSlider").boolValue) num += 6;

            }
            height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}