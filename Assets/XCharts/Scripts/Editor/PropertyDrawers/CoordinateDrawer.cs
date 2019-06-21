using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Coordinate), true)]
    public class CoordinateDrawer : PropertyDrawer
    {
        private bool m_CoordinateModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            SerializedProperty m_Left = prop.FindPropertyRelative("m_Left");
            SerializedProperty m_Right = prop.FindPropertyRelative("m_Right");
            SerializedProperty m_Top = prop.FindPropertyRelative("m_Top");
            SerializedProperty m_Bottom = prop.FindPropertyRelative("m_Bottom");
            SerializedProperty m_Tickness = prop.FindPropertyRelative("m_Tickness");
            SerializedProperty m_FontSize = prop.FindPropertyRelative("m_FontSize");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_CoordinateModuleToggle, "Coordinate");
            EditorGUI.LabelField(drawRect, "Coordinate", EditorStyles.boldLabel);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_CoordinateModuleToggle)
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
                EditorGUI.PropertyField(drawRect, m_Tickness);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_FontSize);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_CoordinateModuleToggle)
                return 7 * EditorGUIUtility.singleLineHeight + 6 * EditorGUIUtility.standardVerticalSpacing;
            else
                return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}