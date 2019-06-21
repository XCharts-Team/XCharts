using UnityEditor;
using UnityEngine;

namespace XCharts
{
    [CustomPropertyDrawer(typeof(Series), true)]
    public class SeriesDrawer : PropertyDrawer
    {
        private int m_DataSize = 0;
        private bool m_ShowJsonDataArea = false;
        private string m_JsonDataAreaText;
        private bool m_SeriesModuleToggle = false;

        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            Rect drawRect = pos;
            drawRect.height = EditorGUIUtility.singleLineHeight;
            SerializedProperty m_Series = prop.FindPropertyRelative("m_Series");

            drawRect.width = EditorGUIUtility.labelWidth + 10;
            ChartEditorHelper.MakeFoldout(ref drawRect, ref m_SeriesModuleToggle, "Series");
            drawRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            //ChartEditorHelper.MakeJsonData(ref drawRect, ref m_ShowJsonDataArea, ref m_JsonDataAreaText, prop);
            drawRect.width = pos.width;
            if (m_SeriesModuleToggle)
            {
                ChartEditorHelper.MakeList(ref drawRect, ref m_DataSize, m_Series);
            }
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
                    height += EditorGUI.GetPropertyHeight(m_Data.GetArrayElementAtIndex(i));
                }
            }
            if (m_ShowJsonDataArea)
            {
                height += EditorGUIUtility.singleLineHeight * 3 + EditorGUIUtility.standardVerticalSpacing;
            }
            height += 1 * EditorGUIUtility.singleLineHeight + 1 * EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}