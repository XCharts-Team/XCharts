/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    [SerieEditor(typeof(Scatter))]
    public class ScatterEditor : SerieEditor<Scatter>
    {
        public override void OnCustomInspectorGUI()
        {
            if (serie.IsUseCoord<SingleAxisCoord>())
            {
                PropertyField("m_SingleAxisIndex");
            }
            else
            {
                PropertyField("m_XAxisIndex");
                PropertyField("m_YAxisIndex");
            }
            PropertyField("m_Clip");
            PropertyField("m_Symbol");

            PropertyField("m_ItemStyle");
            PropertyField("m_IconStyle");
            PropertyField("m_Label");
            PropertyField("m_LabelLine");
            PropertyField("m_Emphasis");
            PropertyField("m_Animation");
        }
    }
}