/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    [SerieEditor(typeof(Parallel))]
    public class ParallelEditor : SerieEditor<Parallel>
    {
        public override void OnCustomInspectorGUI()
        {
            PropertyField("m_ParallelIndex");
            PropertyField("m_LineType");
            PropertyField("m_LineStyle");
            PropertyField("m_Animation");
        }
    }
}