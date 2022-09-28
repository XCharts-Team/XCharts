using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(Parallel))]
    public class ParallelEditor : SerieEditor<Parallel>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_ColorBy");
            PropertyField("m_ParallelIndex");
            PropertyField("m_LineType");
            PropertyField("m_LineStyle");
            PropertyField("m_Animation");
        }
    }
}