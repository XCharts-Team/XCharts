using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Legend), true)]
    public class LegendDrawer : PropertyDrawer
    {
        private bool m_DataFoldout = false;
        private int m_DataSize = 0;
        private bool m_ShowJsonDataArea = false;
        private string m_JsonDataAreaText;
        private bool m_LegendModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_SelectedMode = prop.FindPropertyRelative("m_SelectedMode");
            SerializedProperty orient = prop.FindPropertyRelative("m_Orient");
            SerializedProperty location = prop.FindPropertyRelative("m_Location");
            SerializedProperty itemWidth = prop.FindPropertyRelative("m_ItemWidth");
            SerializedProperty itemHeight = prop.FindPropertyRelative("m_ItemHeight");
            SerializedProperty itemGap = prop.FindPropertyRelative("m_ItemGap");
            SerializedProperty itemFontSize = prop.FindPropertyRelative("m_ItemFontSize");
            SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_LegendModuleToggle, "Legend", show);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_LegendModuleToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, itemWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, itemHeight);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, itemGap);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, itemFontSize);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SelectedMode);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, orient);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, location);
                drawRect.y += EditorGUI.GetPropertyHeight(location);
                drawRect.width = EditorGUIUtility.labelWidth + 10;
                m_DataFoldout = EditorGUI.Foldout(drawRect, m_DataFoldout, "Data");
                ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop,pos.width);
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
            if (m_LegendModuleToggle)
            {
                SerializedProperty location = prop.FindPropertyRelative("m_Location");
                height += 6 * EditorGUIUtility.singleLineHeight + 5 * EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(location);
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_DataFoldout)
                {
                    SerializedProperty m_Data = prop.FindPropertyRelative("m_Data");
                    int num = m_Data.arraySize + 1;
                    height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
                    height += EditorGUIUtility.standardVerticalSpacing;
                }
            }
            if (m_ShowJsonDataArea)
            {
                height += EditorGUIUtility.singleLineHeight * 4 + EditorGUIUtility.standardVerticalSpacing;
            }
            height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}