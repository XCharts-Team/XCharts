using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(SimplifiedCandlestick))]
    public class SimplifiedCandlestickEditor : SerieEditor<SimplifiedCandlestick>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_XAxisIndex");
            PropertyField("m_YAxisIndex");
            PropertyField("m_BarWidth");
            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}