using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Tooltip), true)]
    public class TooltipDrawer : PropertyDrawer
    {
        private bool m_TooltipModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty type = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_Formatter = prop.FindPropertyRelative("m_Formatter");
            SerializedProperty m_FixedWidth = prop.FindPropertyRelative("m_FixedWidth");
            SerializedProperty m_FixedHeight = prop.FindPropertyRelative("m_FixedHeight");
            SerializedProperty m_MinWidth = prop.FindPropertyRelative("m_MinWidth");
            SerializedProperty m_MinHeight = prop.FindPropertyRelative("m_MinHeight");
            SerializedProperty m_FontSize = prop.FindPropertyRelative("m_FontSize");
            SerializedProperty m_FontStyle = prop.FindPropertyRelative("m_FontStyle");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_TooltipModuleToggle, "Tooltip", show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_TooltipModuleToggle)
            {
                EditorGUI.PropertyField(drawRect, type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Formatter);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FixedWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FixedHeight);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_MinWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_MinHeight);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FontSize);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FontStyle);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_TooltipModuleToggle)
                return 9 * EditorGUIUtility.singleLineHeight + 8 * EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}