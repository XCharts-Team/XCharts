using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(Ring))]
    public class RingEditor : SerieEditor<Ring>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_GridIndex");
            PropertyTwoFiled("m_Center");
            PropertyTwoFiled("m_Radius");
            PropertyField("m_StartAngle");
            PropertyField("m_Gap");
            PropertyField("m_MaxCache");
            PropertyField("m_RoundCap");
            PropertyField("m_Clockwise");
            PropertyField("m_AvoidLabelOverlap");
            PropertyField("m_RadiusGradient");

            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}