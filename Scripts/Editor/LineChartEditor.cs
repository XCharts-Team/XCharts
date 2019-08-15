using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI LineChart.
    /// </summary>

    [CustomEditor(typeof(LineChart), false)]
    public class LineChartEditor : CoordinateChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (LineChart)target;
        }
    }
}