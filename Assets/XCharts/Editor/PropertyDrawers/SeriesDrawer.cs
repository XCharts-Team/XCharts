/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Series), true)]
    public class SeriesDrawer : PropertyDrawer
    {
        private bool m_SeriesModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Series = prop.FindPropertyRelative("m_Series");
            m_SeriesModuleToggle = ChartEditorHelper.MakeListWithFoldout(ref drawRect,
                m_Series, m_SeriesModuleToggle, true, true);
        }

        public override float GetPropertyHeight(SerializedProperty prop, GUIContent label)
        {
            float height = 0;
            if (m_SeriesModuleToggle)
            {
                SerializedProperty m_Data = prop.FindPropertyRelative("m_Series");
                height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
                for (int i = 0; i < m_Data.arraySize; i++)
                {
                    height += EditorGUI.GetPropertyHeight(m_Data.GetArrayElementAtIndex(i)) + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}