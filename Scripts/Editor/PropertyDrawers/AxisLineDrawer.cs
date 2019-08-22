using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(AxisLine), true)]
    public class AxisLineDrawer : PropertyDrawer
    {
        private Dictionary<string, bool> m_AxisLineToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_OnZero = prop.FindPropertyRelative("m_OnZero");
            SerializedProperty m_Width = prop.FindPropertyRelative("m_Width");
            SerializedProperty m_Symbol = prop.FindPropertyRelative("m_Symbol");
            SerializedProperty m_SymbolWidth = prop.FindPropertyRelative("m_SymbolWidth");
            SerializedProperty m_SymbolHeight = prop.FindPropertyRelative("m_SymbolHeight");
            SerializedProperty m_SymbolOffset = prop.FindPropertyRelative("m_SymbolOffset");
            SerializedProperty m_SymbolDent = prop.FindPropertyRelative("m_SymbolDent");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AxisLineToggle, prop, "Axis Line", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_AxisLineToggle,prop))
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_OnZero);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Width);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_Symbol);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SymbolWidth);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SymbolHeight);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SymbolOffset);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(drawRect, m_SymbolDent);
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_AxisLineToggle,prop))
            {
                height += 7 * EditorGUIUtility.singleLineHeight + 6 * EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}