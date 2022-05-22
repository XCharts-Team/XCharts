using System.Collections.Generic;

namespace XCharts.Runtime
{
    /// <summary>
    /// The x axis in cartesian(rectangular) coordinate.
    /// |直角坐标系 grid 中的 y 轴。
    /// </summary>
    [System.Serializable]
    [RequireChartComponent(typeof(GridCoord), typeof(XAxis))]
    [ComponentHandler(typeof(YAxisHander), true)]
    public class YAxis : Axis
    {
        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Type = AxisType.Value;
            m_Min = 0;
            m_Max = 0;
            m_SplitNumber = 0;
            m_BoundaryGap = false;
            m_Position = AxisPosition.Left;
            m_Data = new List<string>(5);
            splitLine.show = true;
            splitLine.lineStyle.type = LineStyle.Type.None;
            axisLabel.textLimit.enable = false;
            axisTick.showStartTick = true;
        }
    }
}