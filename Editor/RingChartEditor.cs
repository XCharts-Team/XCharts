/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI RingChart.
    /// </summary>

    [CustomEditor(typeof(RingChart), false)]
    public class RingChartEditor : BaseChartEditor
    {
        protected SerializedProperty m_Radar;
        protected SerializedProperty m_Radars;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_Target = (RingChart)target;
        }
    }
}