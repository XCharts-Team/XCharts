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
        bool m_ThemeModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            var defaultWidth = drawRect.width;
            var defaultX = drawRect.x;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Theme = prop.FindPropertyRelative("m_Theme");
            SerializedProperty m_Font = prop.FindPropertyRelative("m_Font");
            SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");
            SerializedProperty m_TextColor = prop.FindPropertyRelative("m_TitleTextColor");
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

            SerializedProperty m_CustomFont = prop.FindPropertyRelative("m_CustomFont");
            SerializedProperty m_CustomBackgroundColor = prop.FindPropertyRelative("m_CustomBackgroundColor");
            SerializedProperty m_CustomTextColor = prop.FindPropertyRelative("m_CustomTitleTextColor");
            SerializedProperty m_CustomSubTextColor = prop.FindPropertyRelative("m_CustomTitleSubTextColor");
            SerializedProperty m_CustomLegendTextColor = prop.FindPropertyRelative("m_CustomLegendTextColor");
            SerializedProperty m_CustomLegendUnableColor = prop.FindPropertyRelative("m_CustomLegendUnableColor");
            SerializedProperty m_CustomAxisTextColor = prop.FindPropertyRelative("m_CustomAxisTextColor");
            SerializedProperty m_CustomAxisLineColor = prop.FindPropertyRelative("m_CustomAxisLineColor");
            SerializedProperty m_CustomAxisSplitLineColor = prop.FindPropertyRelative("m_CustomAxisSplitLineColor");
            SerializedProperty m_CustomTooltipBackgroundColor = prop.FindPropertyRelative("m_CustomTooltipBackgroundColor");
            SerializedProperty m_CustomTooltipFlagAreaColor = prop.FindPropertyRelative("m_CustomTooltipFlagAreaColor");
            SerializedProperty m_CustomTooltipTextColor = prop.FindPropertyRelative("m_CustomTooltipTextColor");
            SerializedProperty m_CustomTooltipLabelColor = prop.FindPropertyRelative("m_CustomTooltipLabelColor");
            SerializedProperty m_CustomTooltipLineColor = prop.FindPropertyRelative("m_CustomTooltipLineColor");
            SerializedProperty m_CustomDataZoomLineColor = prop.FindPropertyRelative("m_CustomDataZoomLineColor");
            SerializedProperty m_CustomDataZoomSelectedColor = prop.FindPropertyRelative("m_CustomDataZoomSelectedColor");
            SerializedProperty m_CustomDataZoomTextColor = prop.FindPropertyRelative("m_CustomDataZoomTextColor");
            SerializedProperty m_CustomColorPalette = prop.FindPropertyRelative("m_CustomColorPalette");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_ThemeModuleToggle, "Theme");
            drawRect.x = EditorGUIUtility.labelWidth - (EditorGUI.indentLevel - 1) * 15 - 2;
            drawRect.width = defaultWidth - EditorGUIUtility.labelWidth - (m_ThemeModuleToggle ? 45 : 0);
            EditorGUI.PropertyField(drawRect, m_Theme, GUIContent.none);
            if (m_ThemeModuleToggle)
            {
                drawRect.x = defaultWidth - 30;
                drawRect.width = 45;
                if (GUI.Button(drawRect, new GUIContent("Reset", "Reset to theme default color")))
                {
                    m_CustomFont.objectReferenceValue = null;
                    m_CustomBackgroundColor.colorValue = Color.clear;
                    m_CustomTextColor.colorValue = Color.clear;
                    m_CustomSubTextColor.colorValue = Color.clear;
                    m_CustomLegendTextColor.colorValue = Color.clear;
                    m_CustomLegendUnableColor.colorValue = Color.clear;
                    m_CustomAxisTextColor.colorValue = Color.clear;
                    m_CustomAxisLineColor.colorValue = Color.clear;
                    m_CustomAxisSplitLineColor.colorValue = Color.clear;
                    m_CustomTooltipBackgroundColor.colorValue = Color.clear;
                    m_CustomTooltipFlagAreaColor.colorValue = Color.clear;
                    m_CustomTooltipTextColor.colorValue = Color.clear;
                    m_CustomTooltipLabelColor.colorValue = Color.clear;
                    m_CustomTooltipLineColor.colorValue = Color.clear;
                    m_CustomDataZoomLineColor.colorValue = Color.clear;
                    m_CustomDataZoomSelectedColor.colorValue = Color.clear;
                    m_CustomDataZoomTextColor.colorValue = Color.clear;
                    for (int i = 0; i < m_CustomColorPalette.arraySize; i++)
                    {
                        m_CustomColorPalette.GetArrayElementAtIndex(i).colorValue = Color.clear;
                    }

                    ThemeInfo defaultThemeInfo = ThemeInfo.Default;
                    switch (m_Theme.enumValueIndex)
                    {
                        case ((int)Theme.Default): defaultThemeInfo = ThemeInfo.Default; break;
                        case ((int)Theme.Light): defaultThemeInfo = ThemeInfo.Light; break;
                        case ((int)Theme.Dark): defaultThemeInfo = ThemeInfo.Dark; break;
                    }
                    m_Font.objectReferenceValue = defaultThemeInfo.font;
                    m_BackgroundColor.colorValue = defaultThemeInfo.backgroundColor;
                    m_TextColor.colorValue = defaultThemeInfo.titleTextColor;
                    m_SubTextColor.colorValue = defaultThemeInfo.titleSubTextColor;
                    m_LegendTextColor.colorValue = defaultThemeInfo.legendTextColor;
                    m_LegendUnableColor.colorValue = defaultThemeInfo.legendUnableColor;
                    m_AxisTextColor.colorValue = defaultThemeInfo.axisTextColor;
                    m_AxisLineColor.colorValue = defaultThemeInfo.axisLineColor;
                    m_AxisSplitLineColor.colorValue = defaultThemeInfo.axisSplitLineColor;
                    m_TooltipBackgroundColor.colorValue = defaultThemeInfo.tooltipBackgroundColor;
                    m_TooltipFlagAreaColor.colorValue = defaultThemeInfo.tooltipFlagAreaColor;
                    m_TooltipTextColor.colorValue = defaultThemeInfo.tooltipTextColor;
                    m_TooltipLabelColor.colorValue = defaultThemeInfo.tooltipLabelColor;
                    m_TooltipLineColor.colorValue = defaultThemeInfo.tooltipLineColor;
                    m_DataZoomLineColor.colorValue = defaultThemeInfo.dataZoomLineColor;
                    m_DataZoomSelectedColor.colorValue = defaultThemeInfo.dataZoomSelectedColor;
                    m_DataZoomTextColor.colorValue = defaultThemeInfo.dataZoomTextColor;
                    for (int i = 0; i < m_ColorPalette.arraySize; i++)
                    {
                        m_ColorPalette.GetArrayElementAtIndex(i).colorValue = defaultThemeInfo.GetColor(i);
                    }
                }

                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.x = defaultX;
                drawRect.width = defaultWidth;

                ++EditorGUI.indentLevel;
                EditorGUI.BeginChangeCheck();
                var font =m_CustomFont.objectReferenceValue != null?m_CustomFont: m_Font;
                EditorGUI.PropertyField(drawRect, font);
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomFont.objectReferenceValue = font.objectReferenceValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                var color = m_CustomBackgroundColor.colorValue != Color.clear ? m_CustomBackgroundColor : m_BackgroundColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Background Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomBackgroundColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomTextColor.colorValue != Color.clear ? m_CustomTextColor : m_TextColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Title Text Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomTextColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomSubTextColor.colorValue != Color.clear ? m_CustomSubTextColor : m_SubTextColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Title SubText Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomSubTextColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomLegendTextColor.colorValue != Color.clear ? m_CustomLegendTextColor : m_LegendTextColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("LegendText Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomLegendTextColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomLegendUnableColor.colorValue != Color.clear ? m_CustomLegendUnableColor : m_LegendUnableColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("LegendUnable Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomLegendUnableColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomAxisTextColor.colorValue != Color.clear ? m_CustomAxisTextColor : m_AxisTextColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("AxisText Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomAxisTextColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomAxisLineColor.colorValue != Color.clear ? m_CustomAxisLineColor : m_AxisLineColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("AxisLine Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomAxisLineColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomAxisSplitLineColor.colorValue != Color.clear ? m_CustomAxisSplitLineColor : m_AxisSplitLineColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("AxisSplitLine Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomAxisSplitLineColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomTooltipBackgroundColor.colorValue != Color.clear ? m_CustomTooltipBackgroundColor : m_TooltipBackgroundColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Tooltip Background Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomTooltipBackgroundColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomTooltipFlagAreaColor.colorValue != Color.clear ? m_CustomTooltipFlagAreaColor : m_TooltipFlagAreaColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Tooltip FlagArea Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomTooltipFlagAreaColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomTooltipTextColor.colorValue != Color.clear ? m_CustomTooltipTextColor : m_TooltipTextColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Tooltip Text Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomTooltipTextColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomTooltipLabelColor.colorValue != Color.clear ? m_CustomTooltipLabelColor : m_TooltipLabelColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Tooltip Label Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomTooltipLabelColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomTooltipLineColor.colorValue != Color.clear ? m_CustomTooltipLineColor : m_TooltipLineColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("Tooltip Line Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomTooltipLineColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomDataZoomLineColor.colorValue != Color.clear ? m_CustomDataZoomLineColor : m_DataZoomLineColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("DataZoom Line Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomDataZoomLineColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomDataZoomSelectedColor.colorValue != Color.clear ? m_CustomDataZoomSelectedColor : m_DataZoomSelectedColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("DataZoom Selected Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomDataZoomSelectedColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                EditorGUI.BeginChangeCheck();
                color = m_CustomDataZoomTextColor.colorValue != Color.clear ? m_CustomDataZoomTextColor : m_DataZoomTextColor;
                EditorGUI.PropertyField(drawRect, color, new GUIContent("DataZoom Text Color"));
                if (EditorGUI.EndChangeCheck())
                {
                    m_CustomDataZoomTextColor.colorValue = color.colorValue;
                }
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                m_ColorPaletteFoldout = EditorGUI.Foldout(drawRect, m_ColorPaletteFoldout, "ColorPalette");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_ColorPaletteFoldout)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < m_ColorPalette.arraySize; i++)
                    {
                        while (i > m_CustomColorPalette.arraySize - 1)
                        {
                            m_CustomColorPalette.InsertArrayElementAtIndex(m_CustomColorPalette.arraySize);
                        }
                        var customElement = m_CustomColorPalette.GetArrayElementAtIndex(i);
                        color = customElement.colorValue != Color.clear ?
                            customElement :
                            m_ColorPalette.GetArrayElementAtIndex(i);
                        EditorGUI.BeginChangeCheck();
                        EditorGUI.PropertyField(drawRect, color);
                        if (EditorGUI.EndChangeCheck())
                        {
                            customElement.colorValue = color.colorValue;
                        }
                        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    }
                    EditorGUI.indentLevel--;
                }

                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (!m_ThemeModuleToggle)
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
            else
            {
                float height = 0;
                int propertyCount = 18;
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
}