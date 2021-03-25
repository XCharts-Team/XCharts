/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEditor;

namespace XCharts
{
    /// <summary>
    /// Editor class used to edit UI CandlestickChart.
    /// </summary>
    [CustomEditor(typeof(CandlestickChart), false)]
    public class CandlestickChartEditor : CoordinateChartEditor
    {
        protected override void OnEnable()
        {
            base.OnEnable();
            if(target == null) return;
            m_Chart = (CandlestickChart)target;
        }
    }
}