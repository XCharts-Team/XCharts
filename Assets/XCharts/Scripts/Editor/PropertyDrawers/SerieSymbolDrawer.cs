using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(SerieSymbol), true)]
    public class SerieSymbolDrawer : PropertyDrawer
    {

        private List<bool> m_SerieModuleToggle = new List<bool>();
        private List<bool> m_DataFoldout = new List<bool>();
        private int m_DataSize = 0;
        private bool m_ShowJsonDataArea = false;
        private string m_JsonDataAreaText;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Type = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_SizeType = prop.FindPropertyRelative("m_SizeType");
            SerializedProperty m_Size = prop.FindPropertyRelative("m_Size");
            SerializedProperty m_SelectedSize = prop.FindPropertyRelative("m_SelectedSize");
            SerializedProperty m_DataIndex = prop.FindPropertyRelative("m_DataIndex");
            SerializedProperty m_DataScale = prop.FindPropertyRelative("m_DataScale");
            SerializedProperty m_SelectedDataScale = prop.FindPropertyRelative("m_SelectedDataScale");

            SerializedProperty m_SizeCallback = prop.FindPropertyRelative("m_SizeCallback");
            SerializedProperty m_SelectedSizeCallback = prop.FindPropertyRelative("m_SelectedSizeCallback");

            EditorGUI.PropertyField(drawRect, m_Type, new GUIContent("Symbol Type"));
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_SizeType, new GUIContent("Symbol SizeType"));
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            SerieSymbolSizeType sizeType = (SerieSymbolSizeType)m_SizeType.enumValueIndex;
            switch (sizeType)
            {
                case SerieSymbolSizeType.Custom:
                    EditorGUI.PropertyField(drawRect, m_Size, new GUIContent("Symbol Size"));
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_SelectedSize, new GUIContent("Symbol SelectedSize"));
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    break;
                case SerieSymbolSizeType.FromData:
                    EditorGUI.PropertyField(drawRect, m_DataIndex, new GUIContent("Symbol DataIndex"));
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_DataScale, new GUIContent("Symbol DataScale"));
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(drawRect, m_SelectedDataScale, new GUIContent("Symbol SelectedDataScale"));
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    break;
                case SerieSymbolSizeType.Callback:
                    break;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            SerializedProperty m_SizeType = prop.FindPropertyRelative("m_SizeType");
            SerieSymbolSizeType sizeType = (SerieSymbolSizeType)m_SizeType.enumValueIndex;
            switch (sizeType)
            {
                case SerieSymbolSizeType.Custom:
                    return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
                case SerieSymbolSizeType.FromData:
                    return 5 * EditorGUIUtility.singleLineHeight + 4 * EditorGUIUtility.standardVerticalSpacing;
                case SerieSymbolSizeType.Callback:
                    return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
            }
            return 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
        }
    }
}