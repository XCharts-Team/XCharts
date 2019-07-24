using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Radar), true)]
    public class RadarDrawer : PropertyDrawer
    {
        SerializedProperty m_Cricle;
        SerializedProperty m_Area;
        SerializedProperty m_Radius;
        SerializedProperty m_SplitNumber;
        SerializedProperty m_Left;
        SerializedProperty m_Right;
        SerializedProperty m_Top;
        SerializedProperty m_Bottom;
        SerializedProperty m_LineTickness;
        SerializedProperty m_LinePointSize;
        SerializedProperty m_BackgroundColorList;
        SerializedProperty m_LineColor;
        SerializedProperty m_AreaAlpha;
        SerializedProperty m_Indicator;
        SerializedProperty m_IndicatorList;

        private bool m_RadarModuleToggle = false;
        private bool m_IndicatorToggle = false;
        private bool m_IndicatorJsonAreaToggle = false;
        private string m_IndicatorJsonAreaText;
        private int m_IndicatorSize;
        private bool m_BackgroundColorToggle = false;
        private int m_BackgroundColorSize;

        private void InitProperty(SerializedProperty prop)
        {
            m_Cricle = prop.FindPropertyRelative("m_Cricle");
            m_Area = prop.FindPropertyRelative("m_Area");
            m_Radius = prop.FindPropertyRelative("m_Radius");
            m_SplitNumber = prop.FindPropertyRelative("m_SplitNumber");
            m_Left = prop.FindPropertyRelative("m_Left");
            m_Right = prop.FindPropertyRelative("m_Right");
            m_Top = prop.FindPropertyRelative("m_Top");
            m_Bottom = prop.FindPropertyRelative("m_Bottom");
            m_LineTickness = prop.FindPropertyRelative("m_LineTickness");
            m_LinePointSize = prop.FindPropertyRelative("m_LinePointSize");
            m_LineColor = prop.FindPropertyRelative("m_LineColor");
            m_AreaAlpha = prop.FindPropertyRelative("m_AreaAlpha");
            m_BackgroundColorList = prop.FindPropertyRelative("m_BackgroundColorList");
            m_Indicator = prop.FindPropertyRelative("m_Indicator");
            m_IndicatorList = prop.FindPropertyRelative("m_IndicatorList");
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            InitProperty(prop);
            Rect drawRect = pos;
            float defaultLabelWidth = EditorGUIUtility.labelWidth;
            float defaultFieldWidth = EditorGUIUtility.fieldWidth;
            drawRect.height = EditorGUIUtility.singleLineHeight;

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_RadarModuleToggle, "Radar");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_RadarModuleToggle)
            {
                ++EditorGUI.indentLevel;

                EditorGUIUtility.fieldWidth = 10;

                EditorGUIUtility.labelWidth = 50;
                drawRect.width = 60;
                EditorGUI.PropertyField(drawRect, m_Cricle);

                EditorGUIUtility.labelWidth = 45;
                drawRect.x += 60;
                EditorGUI.PropertyField(drawRect, m_Area);

                EditorGUIUtility.labelWidth = 70;
                drawRect.x += 55;
                drawRect.width = 80;
                EditorGUI.PropertyField(drawRect, m_Indicator);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.x = pos.x;
                drawRect.width = pos.width;
                EditorGUIUtility.labelWidth = defaultLabelWidth;
                EditorGUIUtility.fieldWidth = defaultFieldWidth;
                EditorGUI.PropertyField(drawRect, m_Radius);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SplitNumber);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Left);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Right);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Top);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Bottom);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LineTickness);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LinePointSize);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_AreaAlpha);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_LineColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                m_BackgroundColorToggle = EditorGUI.Foldout(drawRect, m_BackgroundColorToggle, "BackgroundColors");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.width = pos.width;
                if (m_BackgroundColorToggle)
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref m_BackgroundColorSize, m_BackgroundColorList);
                }
                drawRect.width = EditorGUIUtility.labelWidth + 10;
                m_IndicatorToggle = EditorGUI.Foldout(drawRect, m_IndicatorToggle, "Indicators");
                ChartEditorHelper.MakeJsonData(ref drawRect, ref m_IndicatorJsonAreaToggle, ref m_IndicatorJsonAreaText, prop,pos.width);
                drawRect.width = pos.width;
                if (m_IndicatorToggle)
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref m_IndicatorSize, m_IndicatorList);
                }
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            int propNum = 1;
            if (m_RadarModuleToggle)
            {
                propNum += 13;

                if (m_BackgroundColorToggle)
                {
                    m_BackgroundColorList = prop.FindPropertyRelative("m_BackgroundColorList");
                    propNum += 2;
                    propNum += m_BackgroundColorList.arraySize;
                }
                if (m_IndicatorJsonAreaToggle) propNum += 4;


                float height = propNum * EditorGUIUtility.singleLineHeight + (propNum - 1) * EditorGUIUtility.standardVerticalSpacing;

                if (m_IndicatorToggle)
                {
                    m_IndicatorList = prop.FindPropertyRelative("m_IndicatorList");
                    height += EditorGUIUtility.singleLineHeight * 2 + EditorGUIUtility.standardVerticalSpacing;

                    for (int i = 0; i < m_IndicatorSize; i++)
                    {
                        height += EditorGUI.GetPropertyHeight(m_IndicatorList.GetArrayElementAtIndex(i));
                    }
                }
                return height;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}