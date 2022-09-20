using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(Heatmap))]
    public class HeatmapEditor : SerieEditor<Heatmap>
    {
        public override void OnCustomInspectorGUI()
        {
            if (serie.IsUseCoord<PolarCoord>())
            {
                PropertyField("m_PolarIndex");
            }
            else
            {
                PropertyField("m_XAxisIndex");
                PropertyField("m_YAxisIndex");
            }
            PropertyField("m_HeatmapType");
            PropertyField("m_Ignore");
            PropertyField("m_IgnoreValue");

            PropertyField("m_Symbol");
            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}