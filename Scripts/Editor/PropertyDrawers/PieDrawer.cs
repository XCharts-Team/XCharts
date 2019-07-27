using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Pie), true)]
    public class PieInfoDrawer : PropertyDrawer
    {
        SerializedProperty m_Name;
        SerializedProperty m_InsideRadius;
        SerializedProperty m_OutsideRadius;
        SerializedProperty m_TooltipExtraRadius;
        SerializedProperty m_Rose;
        SerializedProperty m_Space;
        SerializedProperty m_Left;
        SerializedProperty m_Right;
        SerializedProperty m_Top;
        SerializedProperty m_Bottom;
        SerializedProperty m_Selected;
        SerializedProperty m_SelectedIndex;
        SerializedProperty m_SelectedOffset;
        bool m_PieModuleToggle = true;

        private void InitProperty(SerializedProperty prop)
        {
            m_TooltipExtraRadius = prop.FindPropertyRelative("m_TooltipExtraRadius");
            m_SelectedOffset = prop.FindPropertyRelative("m_SelectedOffset");
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            InitProperty(prop);
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_PieModuleToggle, "Pie");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_PieModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_TooltipExtraRadius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SelectedOffset);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_PieModuleToggle)
                return 3 * EditorGUIUtility.singleLineHeight + 2 * EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}