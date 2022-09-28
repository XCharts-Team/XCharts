using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    [System.Serializable]
    [RequireChartComponent(typeof(ParallelCoord))]
    [ComponentHandler(typeof(ParallelAxisHander), true)]
    public class ParallelAxis : Axis
    {
        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Type = AxisType.Value;
            m_Min = 0;
            m_Max = 0;
            m_SplitNumber = 0;
            m_BoundaryGap = true;
            m_Position = AxisPosition.Bottom;
            m_Offset = 0;
            m_Data = new List<string>() { "x1", "x2", "x3", "x4", "x5" };
            m_Icons = new List<Sprite>(5);
            splitLine.show = false;
            splitLine.lineStyle.type = LineStyle.Type.None;
            axisLabel.textLimit.enable = true;
            axisName.labelStyle.offset = new Vector3(0, 25, 0);
        }

    }
}