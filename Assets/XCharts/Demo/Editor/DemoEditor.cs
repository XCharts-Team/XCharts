using UnityEditor;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI BaseChart.
    /// </summary>

    [CustomEditor(typeof(Demo), false)]
    public class DemoEditor : Editor
    {
        protected Demo m_Target;
        protected SerializedProperty m_Script;
        protected SerializedProperty m_ButtonNormalColor;
        protected SerializedProperty m_ButtonSelectedColor;
        protected SerializedProperty m_ButtonHighlightColor;
        protected SerializedProperty m_ChartModule;
        protected virtual void OnEnable()
        {
            m_Target = (Demo)target;
            m_Script = serializedObject.FindProperty("m_Script");
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
            EditorGUILayout.PropertyField(m_ButtonNormalColor);
            EditorGUILayout.PropertyField(m_ButtonSelectedColor);
            EditorGUILayout.PropertyField(m_ButtonHighlightColor);

            var size = m_ChartModule.arraySize;
            size = EditorGUILayout.IntField("Chart Module Size", size);
            if (size != m_ChartModule.arraySize)
            {
                while (size > m_ChartModule.arraySize)
                    m_ChartModule.InsertArrayElementAtIndex(m_ChartModule.arraySize);
                while (size < m_ChartModule.arraySize)
                    m_ChartModule.DeleteArrayElementAtIndex(m_ChartModule.arraySize - 1);
            }
            for (int i = 0; i < size; i++)
            {
                EditorGUILayout.PropertyField(m_ChartModule.GetArrayElementAtIndex(i));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}