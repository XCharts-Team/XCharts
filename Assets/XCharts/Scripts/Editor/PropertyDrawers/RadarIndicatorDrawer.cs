using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Radar.Indicator), true)]
    public class RadarIndicatorDrawer : PropertyDrawer
    {
        SerializedProperty m_Name;
        SerializedProperty m_Max;

        private bool m_RadarModuleToggle = false;

        private void InitProperty(SerializedProperty prop)
        {
            m_Name = prop.FindPropertyRelative("m_Name");
            m_Max = prop.FindPropertyRelative("m_Max");
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            InitProperty(prop);
            Rect drawRect = pos;
            float defaultLabelWidth = EditorGUIUtility.labelWidth;
            float defaultFieldWidth = EditorGUIUtility.fieldWidth;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            m_RadarModuleToggle = EditorGUI.Foldout(drawRect, m_RadarModuleToggle, "Indicator");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_RadarModuleToggle)
            {
                ++EditorGUI.indentLevel;

                EditorGUI.PropertyField(drawRect, m_Name);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Max);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            int propNum = 1;
            if (m_RadarModuleToggle)
            {
                propNum += 2;
                return propNum * EditorGUIUtility.singleLineHeight + (propNum - 1) * EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}