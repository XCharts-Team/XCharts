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
            m_Name = prop.FindPropertyRelative("m_Name");
            m_InsideRadius = prop.FindPropertyRelative("m_InsideRadius");
            m_OutsideRadius = prop.FindPropertyRelative("m_OutsideRadius");
            m_TooltipExtraRadius = prop.FindPropertyRelative("m_TooltipExtraRadius");
            m_Rose = prop.FindPropertyRelative("m_Rose");
            m_Space = prop.FindPropertyRelative("m_Space");
            m_Left = prop.FindPropertyRelative("m_Left");
            m_Right = prop.FindPropertyRelative("m_Right");
            m_Top = prop.FindPropertyRelative("m_Top");
            m_Bottom = prop.FindPropertyRelative("m_Bottom");
            m_Selected = prop.FindPropertyRelative("m_Selected");
            m_SelectedIndex = prop.FindPropertyRelative("m_SelectedIndex");
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
                EditorGUI.PropertyField(drawRect, m_Name);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_InsideRadius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_OutsideRadius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_TooltipExtraRadius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Selected);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SelectedIndex);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SelectedOffset);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Rose);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Space);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Left);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Right);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Top);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Bottom);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_PieModuleToggle)
                return 14 * EditorGUIUtility.singleLineHeight + 13 * EditorGUIUtility.standardVerticalSpacing;
            else
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
    }
}