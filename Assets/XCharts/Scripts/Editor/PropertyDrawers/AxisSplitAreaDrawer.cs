using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(AxisSplitArea), true)]
    public class AxisSplitAreaDrawer : PropertyDrawer
    {
        private bool m_ColorFoldout = false;
        private int m_ColorSize = 0;
        private Dictionary<string, bool> m_SplitAreaToggle = new Dictionary<string, bool>();

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty show = prop.FindPropertyRelative("m_Show");
            SerializedProperty m_Color = prop.FindPropertyRelative("m_Color");

            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_SplitAreaToggle, prop, "Split Area", show, false);
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            if (ChartEditorHelper.IsToggle(m_SplitAreaToggle, prop))
            {
                ++EditorGUI.indentLevel;
                m_ColorFoldout = EditorGUI.Foldout(drawRect, m_ColorFoldout, "Color");
                drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                drawRect.width = pos.width;
                if (m_ColorFoldout)
                {
                    ChartEditorHelper.MakeList(ref drawRect, ref m_ColorSize, m_Color);
                }
                --EditorGUI.indentLevel;
            }
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (ChartEditorHelper.IsToggle(m_SplitAreaToggle, prop))
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                if (m_ColorFoldout)
                {
                    SerializedProperty m_Data = prop.FindPropertyRelative("m_Color");
                    int num = m_Data.arraySize + 1;
                    height += num * EditorGUIUtility.singleLineHeight + (num - 1) * EditorGUIUtility.standardVerticalSpacing;
                    height += EditorGUIUtility.standardVerticalSpacing;
                }
            }
            return height;
        }
    }
}