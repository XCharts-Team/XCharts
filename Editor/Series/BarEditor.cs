using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(Bar))]
    public class BarEditor : SerieEditor<Bar>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_Stack");
            if (serie.IsUseCoord<PolarCoord>())
            {
                PropertyField("m_PolarIndex");
            }
            else
            {
                PropertyField("m_XAxisIndex");
                PropertyField("m_YAxisIndex");
            }
            PropertyField("m_BarType");
            PropertyField("m_BarPercentStack");
            PropertyField("m_BarWidth");
            PropertyField("m_BarGap");
            if (serie.barType == BarType.Zebra)
            {
                PropertyField("m_BarZebraWidth");
                PropertyField("m_BarZebraGap");
            }
            PropertyField("m_Clip");
            PropertyFiledMore(() =>
            {
                PropertyFieldLimitMin("m_MinShow", 0);
                PropertyFieldLimitMin("m_MaxShow", 0);
                PropertyFieldLimitMin("m_MaxCache", 0);
                PropertyField("m_Ignore");
                PropertyField("m_IgnoreValue");
                PropertyField("m_IgnoreLineBreak");
                PropertyField("m_ShowAsPositiveNumber");
                PropertyField("m_Large");
                PropertyField("m_LargeThreshold");
                PropertyField("m_PlaceHolder");
            });
            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}