using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Line), true)]
    public class LineDrawer : PropertyDrawer
    {
        SerializedProperty m_Tickness;
        SerializedProperty m_Smooth;
        SerializedProperty m_SmoothStyle;
        SerializedProperty m_Area;
        SerializedProperty m_Step;
        SerializedProperty m_StepType;

        private bool m_LineModuleToggle = false;

        private void InitProperty(SerializedProperty prop)
        {
            m_Tickness = prop.FindPropertyRelative("m_Tickness");
            m_Smooth = prop.FindPropertyRelative("m_Smooth");
            m_SmoothStyle = prop.FindPropertyRelative("m_SmoothStyle");
            m_Area = prop.FindPropertyRelative("m_Area");
            m_Step = prop.FindPropertyRelative("m_Step");
            m_StepType = prop.FindPropertyRelative("m_StepType");
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            InitProperty(prop);
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_LineModuleToggle, "Line");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_LineModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Tickness);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                drawRect.width = EditorGUIUtility.labelWidth + 10;
                EditorGUI.PropertyField(drawRect, m_Smooth);
                if (m_Smooth.boolValue)
                {
                    drawRect.x = EditorGUIUtility.labelWidth + 15;
                    EditorGUI.LabelField(drawRect, "Style");
                    drawRect.x = EditorGUIUtility.labelWidth + 65;
                    float tempWidth = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 70;
                    if (tempWidth < 20) tempWidth = 20;
                    drawRect.width = tempWidth;
                    EditorGUI.PropertyField(drawRect, m_SmoothStyle, GUIContent.none);
                    drawRect.x = pos.x;
                    drawRect.width = pos.width;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                drawRect.width = EditorGUIUtility.labelWidth + 10;
                EditorGUI.PropertyField(drawRect, m_Step);
                if (m_Step.boolValue)
                {
                    drawRect.x = EditorGUIUtility.labelWidth + 15;
                    EditorGUI.LabelField(drawRect, "Type");
                    drawRect.x = EditorGUIUtility.labelWidth + 65;
                    float tempWidth = EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth - 70;
                    if (tempWidth < 20) tempWidth = 20;
                    drawRect.width = tempWidth;
                    EditorGUI.PropertyField(drawRect, m_StepType, GUIContent.none);
                    drawRect.x = pos.x;
                    drawRect.width = pos.width;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.PropertyField(drawRect, m_Area);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (m_LineModuleToggle)
            {
                height = 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
                return height;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}