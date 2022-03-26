using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(Pie))]
    public class PieEditor : SerieEditor<Pie>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_RoseType");
            PropertyField("m_Gap");
            PropertyTwoFiled("m_Center");
            PropertyTwoFiled("m_Radius");

            PropertyFiledMore(() =>
            {
                PropertyField("m_MinAngle");
                PropertyField("m_RoundCap");
                PropertyField("m_Ignore");
                PropertyField("m_IgnoreValue");
                PropertyField("m_AvoidLabelOverlap");
            });

            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}