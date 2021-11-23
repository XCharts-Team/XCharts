/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    [SerieEditor(typeof(Gauge))]
    public class GaugeEditor : SerieEditor<Gauge>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_GaugeType");
            PropertyTwoFiled("m_Center");
            PropertyTwoFiled("m_Radius");
            PropertyField("m_Min");
            PropertyField("m_Max");
            PropertyField("m_StartAngle");
            PropertyField("m_EndAngle");
            PropertyFieldLimitMax("m_SplitNumber", 36);
            PropertyField("m_RoundCap");
            PropertyField("m_TitleStyle");
            PropertyField("m_GaugeAxis");
            PropertyField("m_GaugePointer");

            PropertyField("m_ItemStyle");
            PropertyField("m_IconStyle");
            PropertyField("m_Label");
            PropertyField("m_LabelLine");
            PropertyField("m_Emphasis");
            PropertyField("m_Animation");
        }
    }
}