/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI PolarChart.
    /// </summary>
    [CustomEditor(typeof(PolarChart), false)]
    public class PolarChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Polars;
        protected SerializedProperty m_RadiusAxes;
        protected SerializedProperty m_AngleAxes;

        protected override void OnEnable()
        {
            base.OnEnable();
            if(target == null) return;
            m_Chart = (PolarChart)target;
            m_Polars = serializedObject.FindProperty("m_Polars");
            m_RadiusAxes = serializedObject.FindProperty("m_RadiusAxes");
            m_AngleAxes = serializedObject.FindProperty("m_AngleAxes");
        }

        protected override void OnStartInspectorGUI()
        {
            base.OnStartInspectorGUI();
            var showAll = m_MultiComponentMode.boolValue;
            BlockListField(showAll, m_Polars);
            BlockListField(showAll, m_RadiusAxes);
            BlockListField(showAll, m_AngleAxes);
        }
    }
}