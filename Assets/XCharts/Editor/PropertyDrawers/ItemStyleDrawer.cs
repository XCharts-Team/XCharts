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
    [CustomPropertyDrawer(typeof(ItemStyle), true)]
    public class ItemStyleDrawer : PropertyDrawer
    {
        private int m_CornerRadius = 0;
        private Dictionary<string, bool> m_ItemStyleToggle = new Dictionary<string, bool>();
        private Dictionary<string, bool> m_CornerRadiusToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");
            SerializedProperty m_ToColor = prop.FindPropertyRelative("m_ToColor");
            SerializedProperty m_BackgroundColor = prop.FindPropertyRelative("m_BackgroundColor");
            SerializedProperty m_BackgroundWidth = prop.FindPropertyRelative("m_BackgroundWidth");
            SerializedProperty m_CenterColor = prop.FindPropertyRelative("m_CenterColor");
            SerializedProperty m_CenterGap = prop.FindPropertyRelative("m_CenterGap");
            SerializedProperty m_BorderType = prop.FindPropertyRelative("m_BorderType");
            SerializedProperty m_BorderWidth = prop.FindPropertyRelative("m_BorderWidth");
            SerializedProperty m_BorderColor = prop.FindPropertyRelative("m_BorderColor");
            SerializedProperty m_Opacity = prop.FindPropertyRelative("m_Opacity");
            SerializedProperty m_TooltipFormatter = prop.FindPropertyRelative("m_TooltipFormatter");
            SerializedProperty m_NumericFormatter = prop.FindPropertyRelative("m_NumericFormatter");
            SerializedProperty m_CornerRadius = prop.FindPropertyRelative("m_CornerRadius");
            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_ItemStyleToggle, prop, "Item Style", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_ItemStyleToggle, prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_Color);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_ToColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BackgroundColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BackgroundWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_CenterColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_CenterGap);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BorderType);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BorderWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_BorderColor);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Opacity);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_TooltipFormatter);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_NumericFormatter);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                ChartEditorHelper.MakeFoldout(ref drawRect, ref m_CornerRadiusToggle, m_CornerRadius, "Corner Radius", null, false);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (ChartEditorHelper.IsToggle(m_CornerRadiusToggle, m_CornerRadius))
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref this.m_CornerRadius, m_CornerRadius, false, false);
                }
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_ItemStyleToggle, prop))
            {
                height += 14 * EditorGUIUtility.singleLineHeight + 13 * EditorGUIUtility.standardVerticalSpacing;
                var m_CornerRadius = prop.FindPropertyRelative("m_CornerRadius");
                if (ChartEditorHelper.IsToggle(m_CornerRadiusToggle, m_CornerRadius))
                {
                    height += 4 * EditorGUIUtility.singleLineHeight + 3 * EditorGUIUtility.standardVerticalSpacing;
                }
            }
            else
            {
                height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}