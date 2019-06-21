using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(ThemeInfo), true)]
    public class ThemeInfoDrawer : PropertyDrawer
    {
        ReorderableList m_ColorPaletteList;
        bool m_ColorPaletteFoldout;

        private void InitReorderableList(SerializedProperty prop)
        {
            if (m_ColorPaletteList == null)
            {
                SerializedProperty colorPalette = prop.FindPropertyRelative("m_ColorPalette");
                m_ColorPaletteList = new ReorderableList(colorPalette.serializedObject, colorPalette, false, false, true, true);
                m_ColorPaletteList.elementHeight = EditorGUIUtility.singleLineHeight;
                m_ColorPaletteList.drawHeaderCallback += delegate (Rect rect)
                {
                    EditorGUI.LabelField(rect, colorPalette.displayName);
                };

                m_ColorPaletteList.drawElementCallback = delegate (Rect rect, int index, bool isActive, bool isFocused)
                {
                    EditorGUI.PropertyField(rect, colorPalette.GetArrayElementAtIndex(index), true);
                };
            }
        }

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Font = prop.FindPropertyRelative("m_Font");
            SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");
            SerializedProperty m_TextColor = prop.FindPropertyRelative("m_TextColor");
            SerializedProperty m_SubTextColor = prop.FindPropertyRelative("m_TitleSubTextColor");
            SerializedProperty m_LegendTextColor = prop.FindPropertyRelative("m_LegendTextColor");
            SerializedProperty m_LegendUnableColor = prop.FindPropertyRelative("m_LegendUnableColor");
            SerializedProperty m_AxisTextColor = prop.FindPropertyRelative("m_AxisTextColor");
            SerializedProperty m_AxisLineColor = prop.FindPropertyRelative("m_AxisLineColor");
            SerializedProperty m_AxisSplitLineColor = prop.FindPropertyRelative("m_AxisSplitLineColor");
            SerializedProperty m_TooltipBackgroundColor = prop.FindPropertyRelative("m_TooltipBackgroundColor");
            SerializedProperty m_TooltipFlagAreaColor = prop.FindPropertyRelative("m_TooltipFlagAreaColor");
            SerializedProperty m_TooltipTextColor = prop.FindPropertyRelative("m_TooltipTextColor");
            SerializedProperty m_TooltipLabelColor = prop.FindPropertyRelative("m_TooltipLabelColor");
            SerializedProperty m_TooltipLineColor = prop.FindPropertyRelative("m_TooltipLineColor");
            SerializedProperty m_DataZoomLineColor = prop.FindPropertyRelative("m_DataZoomLineColor");
            SerializedProperty m_DataZoomSelectedColor = prop.FindPropertyRelative("m_DataZoomSelectedColor");
            SerializedProperty m_DataZoomTextColor = prop.FindPropertyRelative("m_DataZoomTextColor");
            SerializedProperty m_ColorPalette = prop.FindPropertyRelative("m_ColorPalette");

            ++EditorGUI.indentLevel;
            EditorGUI.PropertyField(drawRect, m_Font);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_BackgroundColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_TextColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_SubTextColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_LegendTextColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_LegendUnableColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_AxisTextColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_AxisLineColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_AxisSplitLineColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_TooltipBackgroundColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_TooltipFlagAreaColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_TooltipTextColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_TooltipLabelColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_TooltipLineColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_DataZoomLineColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_DataZoomSelectedColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            EditorGUI.PropertyField(drawRect, m_DataZoomTextColor);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            m_ColorPaletteFoldout = EditorGUI.Foldout(drawRect, m_ColorPaletteFoldout, "ColorPalette");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_ColorPaletteFoldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < m_ColorPalette.arraySize; i++)
                {
                    SerializedProperty element = m_ColorPalette.GetArrayElementAtIndex(i);
                    EditorGUI.PropertyField(drawRect, element);
                    drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                }
                EditorGUI.indentLevel--;
            }

            --EditorGUI.indentLevel;
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            int propertyCount = 17;
            if (m_ColorPaletteFoldout)
            {
                SerializedProperty m_ColorPalette = prop.FindPropertyRelative("m_ColorPalette");
                propertyCount += m_ColorPalette.arraySize + 1;
            }
            else
            {
                propertyCount += 1;
            }
            height += propertyCount * EditorGUIUtility.singleLineHeight + (propertyCount - 1) * EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}