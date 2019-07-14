using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(AxisLine), true)]
    public class AxisLineDrawer : PropertyDrawer
    {
        private bool m_AxisLineToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_OnZero = prop.FindPropertyRelative("m_OnZero");
            SerializedProperty m_Symbol = prop.FindPropertyRelative("m_Symbol");
            SerializedProperty m_SymbolWidth = prop.FindPropertyRelative("m_SymbolWidth");
            SerializedProperty m_SymbolHeight = prop.FindPropertyRelative("m_SymbolHeight");
            SerializedProperty m_SymbolOffset = prop.FindPropertyRelative("m_SymbolOffset");
            SerializedProperty m_SymbolDent = prop.FindPropertyRelative("m_SymbolDent");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_AxisLineToggle, "Axis Line", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (m_AxisLineToggle)
            {
                ++EditorGUI.indentLevel;
                EditorGUI.PropertyField(drawRect, m_OnZero);
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
            if (m_AxisLineToggle)
            {
                height += 6 * EditorGUIUtility.singleLineHeight + 7 * EditorGUIUtility.standardVerticalSpacing;
            }
            return height;
        }
    }
}