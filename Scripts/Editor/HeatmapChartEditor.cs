using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI HeatmapChart.
    /// </summary>

    [CustomEditor(typeof(HeatmapChart), false)]
    public class HeatmapChartEditor : CoordinateChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (HeatmapChart)target;
        }

        protected override void OnEndInspectorGUI()
        {
            base.OnEndInspectorGUI();
        }
    }
}