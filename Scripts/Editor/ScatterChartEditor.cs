using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI ScatterChart.
    /// </summary>

    [CustomEditor(typeof(ScatterChart), false)]
    public class ScatterChartEditor : CoordinateChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (ScatterChart)target;
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
        }
    }
}