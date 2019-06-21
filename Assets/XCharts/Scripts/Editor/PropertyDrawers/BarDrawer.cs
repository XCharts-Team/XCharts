using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(BarChart.Bar), true)]
    public class BarDrawer : PropertyDrawer
    {
        SerializedProperty m_InSameBar;
        SerializedProperty m_BarWidth;
        SerializedProperty m_Space;
        bool m_BarModuleToggle = true;

        private void InitProperty(SerializedProperty prop)
        {
            m_InSameBar = prop.FindPropertyRelative("m_InSameBar");
            m_BarWidth = prop.FindPropertyRelative("m_BarWidth");
            m_Space = prop.FindPropertyRelative("m_Space");
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            InitProperty(prop);
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_BarModuleToggle, "Bar");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            if (m_BarModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_InSameBar);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BarWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Space);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (m_BarModuleToggle)
                return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
            else
                return 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}