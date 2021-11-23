/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    [SerieEditor(typeof(Radar))]
    public class RadarEditor : SerieEditor<Radar>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_RadarType");
            PropertyField("m_RadarIndex");
            PropertyField("m_Symbol");
            PropertyField("m_LineStyle");
            PropertyField("m_AreaStyle");

            PropertyField("m_ItemStyle");
            PropertyField("m_IconStyle");
            PropertyField("m_Label");
            PropertyField("m_LabelLine");
            PropertyField("m_Emphasis");
            PropertyField("m_Animation");
        }
    }
}