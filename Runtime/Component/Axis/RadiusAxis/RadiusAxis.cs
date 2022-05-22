using System.Collections.Generic;

namespace XCharts.Runtime
{
    /// <summary>
    /// Radial axis of polar coordinate.
    /// |极坐标系的径向轴。
    /// </summary>
    [System.Serializable]
    [RequireChartComponent(typeof(PolarCoord))]
    [ComponentHandler(typeof(RadiusAxisHandler), true)]
    public class RadiusAxis : Axis
    {
        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Type = AxisType.Value;
            m_Min = 0;
            m_Max = 0;
            m_SplitNumber = 5;
            m_BoundaryGap = false;
            m_Data = new List<string>(5);
            splitLine.show = true;
            splitLine.lineStyle.type = LineStyle.Type.Solid;
            axisLabel.textLimit.enable = false;
        }
    }
}