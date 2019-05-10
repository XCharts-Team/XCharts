using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Serie), true)]
    public class SerieDrawer : PropertyDrawer
    {
        private bool m_DataFoldout = false;
        private int m_DataSize = 0;
        private bool m_ShowJsonDataArea = false;
        private string m_JsonDataAreaText;
        private bool m_SerieModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            //SerializedProperty type = prop.FindPropertyRelative("m_Type");
            SerializedProperty name = prop.FindPropertyRelative("m_Name");
            SerializedProperty stack = prop.FindPropertyRelative("m_Stack");
            SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");

            string moduleName = "Serie " + prop.displayName.Split(' ')[1];
            if (!string.IsNullOrEmpty(name.stringValue))
                moduleName += ":" + name.stringValue;
            m_SerieModuleToggle = EditorGUI.Foldout(drawRect, m_SerieModuleToggle, "Serie");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_SerieModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, show);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, name);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, stack);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.width = EditorGUIUtility.labelWidth + 10;
                m_DataFoldout = EditorGUI.Foldout(drawRect, m_DataFoldout, "Data");
                ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop);
                drawRect.width = pos.width;
                if (m_DataFoldout)
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref m_DataSize, m_Data);
                }
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (!m_SerieModuleToggle)
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                height += 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
                if (m_DataFoldout)
                {
                    SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");
                    int num = m_Data.arraySize + 1;
                    if (num > 50) num = 13;
                    height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
                }
                if (m_ShowJsonDataArea)
                {
                    height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
                }
                return height;
            }
        }
    }
}