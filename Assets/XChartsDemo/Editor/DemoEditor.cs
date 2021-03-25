
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;

namespace XChartsDemo
{
    /// <summary>
    /// Editor class used to edit UI BaseChart.
    /// </summary>

    [CustomEditor(typeof(Demo), false)]
    internal class DemoEditor : Editor
    {
        protected Demo m_Target;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_LeftWidth;
        protected SerializedProperty m_LeftButtonHeight;
        protected SerializedProperty m_ChartSpacing;
        protected SerializedProperty m_ChartSizeRatio;
        protected SerializedProperty m_ButtonNormalColor;
        protected SerializedProperty m_ButtonSelectedColor;
        protected SerializedProperty m_ButtonHighlightColor;
        protected SerializedProperty m_ChartModule;
        protected virtual void OnEnable()
        {
            m_Target = (Demo)target;
            m_Script = serializedObject.FindProperty("m_Script");
            m_LeftWidth = serializedObject.FindProperty("m_LeftWidth");
            m_ChartSpacing = serializedObject.FindProperty("m_ChartSpacing");
            m_ChartSizeRatio = serializedObject.FindProperty("m_ChartSizeRatio");
            m_LeftButtonHeight = serializedObject.FindProperty("m_LeftButtonHeight");
            m_ButtonNormalColor = serializedObject.FindProperty("m_ButtonNormalColor");
            m_ButtonSelectedColor = serializedObject.FindProperty("m_ButtonSelectedColor");
            m_ButtonHighlightColor = serializedObject.FindProperty("m_ButtonHighlightColor");
            m_ChartModule = serializedObject.FindProperty("m_ChartModule");

        }

        public override void OnInspectorGUI()
        {
            if (m_Target == null && target == null)
            {
                base.OnInspectorGUI();
                return;
            }
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_LeftWidth);
            EditorGUILayout.PropertyField(m_LeftButtonHeight);
            EditorGUILayout.PropertyField(m_ChartSpacing);
            EditorGUILayout.PropertyField(m_ChartSizeRatio);
            EditorGUILayout.PropertyField(m_ButtonNormalColor);
            EditorGUILayout.PropertyField(m_ButtonSelectedColor);
            EditorGUILayout.PropertyField(m_ButtonHighlightColor);

            var size = m_ChartModule.arraySize;
            size = EditorGUILayout.IntField("Chart Module Size", size);
            if (size != m_ChartModule.arraySize)
            {
                while (size > m_ChartModule.arraySize) m_ChartModule.arraySize++;
                while (size < m_ChartModule.arraySize) m_ChartModule.arraySize--;
            }
            for (int i = 0; i < size; i++)
            {
                EditorGUILayout.PropertyField(m_ChartModule.GetArrayElementAtIndex(i));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}