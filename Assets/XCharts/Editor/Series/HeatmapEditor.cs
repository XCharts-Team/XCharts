

namespace XCharts.Editor
{
    [SerieEditor(typeof(Heatmap))]
    public class HeatmapEditor : SerieEditor<Heatmap>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_Ignore");
            PropertyField("m_IgnoreValue");

            PropertyField("m_ItemStyle");
            PropertyField("m_IconStyle");
            PropertyField("m_Label");
            PropertyField("m_LabelLine");
            PropertyField("m_Emphasis");
            PropertyField("m_Animation");
        }
    }
}