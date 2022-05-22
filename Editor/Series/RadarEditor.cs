using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(Radar))]
    public class RadarEditor : SerieEditor<Radar>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_RadarType");
            PropertyField("m_RadarIndex");

            PropertyField("m_Symbol");
            PropertyField("m_LineStyle");
            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}