using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Grid), true)]
    public class GridDrawer : PropertyDrawer
    {
        private bool m_GridModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_Show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Left = prop.FindPropertyRelative("m_Left");
            SerializedProperty m_Right = prop.FindPropertyRelative("m_Right");
            SerializedProperty m_Top = prop.FindPropertyRelative("m_Top");
            SerializedProperty m_Bottom = prop.FindPropertyRelative("m_Bottom");
            SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_GridModuleToggle, "Grid",m_Show);
            EditorGUI.LabelField(drawRect, "Grid", EditorStyles.boldLabel);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_GridModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Left);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Right);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Top);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Bottom);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BackgroundColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_GridModuleToggle)
                return 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
            else
                return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}