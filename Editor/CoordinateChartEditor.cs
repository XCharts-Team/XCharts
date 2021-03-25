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
    /// <summary>
    /// Editor class used to edit UI CoordinateChart.
    /// </summary>
    [CustomEditor(typeof(CoordinateChart), false)]
    public class CoordinateChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Grids;
        protected SerializedProperty m_MultipleXAxis;
        protected SerializedProperty m_XAxes;
        protected SerializedProperty m_MultipleYAxis;
        protected SerializedProperty m_YAxes;
        protected SerializedProperty m_DataZooms;
        protected SerializedProperty m_VisualMaps;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(target == null) return;
            m_Chart = (CoordinateChart)target;
            m_Grids = serializedObject.FindProperty("m_Grids");
            m_XAxes = serializedObject.FindProperty("m_XAxes");
            m_YAxes = serializedObject.FindProperty("m_YAxes");
            m_DataZooms = serializedObject.FindProperty("m_DataZooms");
            m_VisualMaps = serializedObject.FindProperty("m_VisualMaps");
        }

        protected override void OnStartInspectorGUI()
        {
            base.OnStartInspectorGUI();
            BlockListField(m_ShowAllComponent, m_DataZooms);
            BlockListField(m_ShowAllComponent, m_VisualMaps);
            BlockListField(m_ShowAllComponent, m_Grids);
            BlockListField(m_ShowAllComponent, m_XAxes);
            BlockListField(m_ShowAllComponent, m_YAxes);
        }

        protected override void MoreDebugInspector()
        {
            base.MoreDebugInspector();
            CovertXYAxis();
        }

        private void CovertXYAxis()
        {
            if (GUILayout.Button("Covert XY Axis"))
            {
                (m_Chart as CoordinateChart).CovertXYAxis(0);
            }
        }
    }
}