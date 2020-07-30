
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;

namespace XCharts
{
    [AddComponentMenu("XCharts/LineChart", 13)]
    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [DisallowMultipleComponent]
    public class LineChart : CoordinateChart
    {

#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            m_Title.text = "LineChart";
            m_Tooltip.type = Tooltip.Type.Line;

            m_VisualMap.enable = false;
            m_VisualMap.show = false;
            m_VisualMap.autoMinMax = true;
            m_VisualMap.direction = VisualMap.Direction.Y;
            m_VisualMap.inRange.Clear();
            m_VisualMap.inRange.Add(Color.blue);
            m_VisualMap.inRange.Add(Color.red);

            RemoveData();
            var serie = AddSerie(SerieType.Line, "serie1");
            serie.symbol.show = true;
            for (int i = 0; i < 5; i++)
            {
                AddXAxisData("x" + (i + 1));
                AddData(0, UnityEngine.Random.Range(10, 90));
            }
        }
#endif
    }
}
