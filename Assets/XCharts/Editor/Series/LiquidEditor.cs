

namespace XCharts.Editor
{
    [SerieEditor(typeof(Liquid))]
    public class LiquidEditor : SerieEditor<Liquid>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_VesselIndex");
            PropertyField("m_Min");
            PropertyField("m_Max");
            PropertyField("m_WaveLength");
            PropertyField("m_WaveHeight");
            PropertyField("m_WaveSpeed");
            PropertyField("m_WaveOffset");

            PropertyField("m_ItemStyle");
            PropertyField("m_IconStyle");
            PropertyField("m_Label");
            PropertyField("m_LabelLine");
            PropertyField("m_Emphasis");
            PropertyField("m_Animation");
        }
    }
}