/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    [SerieEditor(typeof(Pie))]
    public class PieEditor : SerieEditor<Pie>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_RoseType");
            PropertyField("m_Space");
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
            PropertyField("m_IconStyle");
            PropertyField("m_Label");
            PropertyField("m_LabelLine");
            PropertyField("m_Emphasis");
            PropertyField("m_Animation");
        }
    }
}