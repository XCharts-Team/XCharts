/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(SerieSymbol), true)]
    public class SerieSymbolDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_SerieSymbolToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Type = prop.FindPropertyRelative("m_Type");
            SerializedProperty m_SizeType = prop.FindPropertyRelative("m_SizeType");
            SerializedProperty m_Size = prop.FindPropertyRelative("m_Size");
            SerializedProperty m_SelectedSize = prop.FindPropertyRelative("m_SelectedSize");
            SerializedProperty m_DataIndex = prop.FindPropertyRelative("m_DataIndex");
            SerializedProperty m_DataScale = prop.FindPropertyRelative("m_DataScale");
            SerializedProperty m_SelectedDataScale = prop.FindPropertyRelative("m_SelectedDataScale");
            SerializedProperty m_StartIndex = prop.FindPropertyRelative("m_StartIndex");
            SerializedProperty m_Interval = prop.FindPropertyRelative("m_Interval");
            SerializedProperty m_ForceShowLast = prop.FindPropertyRelative("m_ForceShowLast");
            SerializedProperty m_Gap = prop.FindPropertyRelative("m_Gap");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_SerieSymbolToggle, prop, null, m_Show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_SerieSymbolToggle, prop))
            {
                ++EditorGUI.indentLevel;

                EditorGUI.PropertyField(drawRect, m_Type);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Gap);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SizeType);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                SerieSymbolSizeType sizeType = (SerieSymbolSizeType)m_SizeType.enumValueIndex;
                switch (sizeType)
                {
                    case SerieSymbolSizeType.Custom:
                        EditorGUI.PropertyField(drawRect, m_Size);
                        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(drawRect, m_SelectedSize);
                        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        break;
                    case SerieSymbolSizeType.FromData:
                        EditorGUI.PropertyField(drawRect, m_DataIndex);
                        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(drawRect, m_DataScale);
                        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        EditorGUI.PropertyField(drawRect, m_SelectedDataScale);
                        drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                        break;
                    case SerieSymbolSizeType.Callback:
                        break;
                }
                EditorGUI.PropertyField(drawRect, m_StartIndex);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Interval);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ForceShowLast);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            if (ChartEditorHelper.IsToggle(m_SerieSymbolToggle, prop))
            {
                SerializedProperty m_SizeType = prop.FindPropertyRelative("m_SizeType");
                SerieSymbolSizeType sizeType = (SerieSymbolSizeType)m_SizeType.enumValueIndex;
                var hig = 9 * EditorGUIUtility.singleLineHeight + 9 * EditorGUIUtility.standardVerticalSpacing;
                switch (sizeType)
                {
                    case SerieSymbolSizeType.Custom:
                        break;
                    case SerieSymbolSizeType.FromData:
                        hig += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
                        break;
                    case SerieSymbolSizeType.Callback:
                        break;
                }
                return hig;
            }
            else
            {
                return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            }
        }
    }
}