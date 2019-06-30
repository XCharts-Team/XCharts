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
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Type = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_FilterMode = prop.FindPropertyRelative("m_FilterMode");
            //SerializedProperty m_Orient = prop.FindPropertyRelative("m_Orient");
            SerializedProperty m_ShowDataShadow = prop.FindPropertyRelative("m_ShowDataShadow");
            SerializedProperty m_ShowDetail = prop.FindPropertyRelative("m_ShowDetail");
            SerializedProperty m_ZoomLock = prop.FindPropertyRelative("m_ZoomLock");
            SerializedProperty m_Realtime = prop.FindPropertyRelative("m_Realtime");
            SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");
            SerializedProperty m_Height = prop.FindPropertyRelative("m_Height");
            SerializedProperty m_Bottom = prop.FindPropertyRelative("m_Bottom");
            SerializedProperty m_RangeMode = prop.FindPropertyRelative("m_RangeMode");
            SerializedProperty m_Start = prop.FindPropertyRelative("m_Start");
            SerializedProperty m_End = prop.FindPropertyRelative("m_End");
            SerializedProperty m_ScrollSensitivity = prop.FindPropertyRelative("m_ScrollSensitivity");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_DataZoomModuleToggle, "DataZoom", show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_DataZoomModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FilterMode);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                //EditorGUI.PropertyField(drawRect, m_Orient);
                //drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ShowDataShadow);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ShowDetail);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ZoomLock);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Realtime);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ScrollSensitivity);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BackgroundColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Height);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Bottom);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_RangeMode);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Start);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_End);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (m_DataZoomModuleToggle)
            {
                height += 13 * EditorGUIUtility.singleLineHeight + 12 * EditorGUIUtility.standardVerticalSpacing;

            }
            height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}