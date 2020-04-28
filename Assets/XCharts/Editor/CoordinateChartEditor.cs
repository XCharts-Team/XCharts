/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI CoordinateChart.
    /// </summary>

    [CustomEditor(typeof(CoordinateChart), false)]
    public class CoordinateChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Grid;
        protected SerializedProperty m_MultipleXAxis;
        protected SerializedProperty m_XAxises;
        protected SerializedProperty m_MultipleYAxis;
        protected SerializedProperty m_YAxises;
        protected SerializedProperty m_DataZoom;
        protected SerializedProperty m_VisualMap;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (CoordinateChart)target;
            m_Grid = serializedObject.FindProperty("m_Grid");
            m_XAxises = serializedObject.FindProperty("m_XAxises");
            m_YAxises = serializedObject.FindProperty("m_YAxises");
            m_DataZoom = serializedObject.FindProperty("m_DataZoom");
            m_VisualMap = serializedObject.FindProperty("m_VisualMap");
        }

        protected override void OnStartInspectorGUI()
        {
            base.OnStartInspectorGUI();
            if (m_Target is LineChart || m_Target is BarChart)
            {
                EditorGUILayout.PropertyField(m_DataZoom);
            }
            if (m_Target is HeatmapChart)
            {
                EditorGUILayout.PropertyField(m_VisualMap);
            }
            EditorGUILayout.PropertyField(m_Grid);
            for (int i = 0; i < m_XAxises.arraySize; i++)
            {
                SerializedProperty axis = m_XAxises.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(axis);
            }
            for (int i = 0; i < m_YAxises.arraySize; i++)
            {
                SerializedProperty axis = m_YAxises.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(axis);
            }
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
            CovertXYAxis();
        }

        private void CovertXYAxis()
        {
            if (GUILayout.Button("Covert XY Axis"))
            {
                (m_Target as CoordinateChart).CovertXYAxis(0);
            }
        }
    }
}