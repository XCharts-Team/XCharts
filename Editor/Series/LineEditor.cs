using XCharts.Runtime;

namespace XCharts.Editor
{
    [SerieEditor(typeof(Line))]
    public class LineEditor : SerieEditor<Line>
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
            PropertyField("m_LineType");
            //PropertyField("m_Clip");
            PropertyFiledMore(() =>
            {
                PropertyFieldLimitMin("m_MinShow", 0);
                PropertyFieldLimitMin("m_MaxShow", 0);
                PropertyFieldLimitMin("m_MaxCache", 0);
                PropertyField("m_SampleDist");
                PropertyField("m_SampleType");
                PropertyField("m_SampleAverage");
                PropertyField("m_Ignore");
                PropertyField("m_IgnoreValue");
                PropertyField("m_IgnoreLineBreak");
                PropertyField("m_ShowAsPositiveNumber");
                PropertyField("m_Large");
                PropertyField("m_LargeThreshold");
            });
            PropertyField("m_Symbol");
            PropertyField("m_LineStyle");
            PropertyField("m_ItemStyle");
            PropertyField("m_Animation");
        }
    }
}