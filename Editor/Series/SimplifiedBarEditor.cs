using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(SimplifiedBar))]
    public class SimplifiedBarEditor : SerieEditor<SimplifiedBar>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_XAxisIndex");
            PropertyField("m_YAxisIndex");
            PropertyField("m_BarWidth");
            PropertyField("m_BarGap");
            PropertyField("m_Clip");
            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}