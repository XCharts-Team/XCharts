using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// Angle axis of Polar Coordinate.
    /// |极坐标系的角度轴。
    /// </summary>
    [System.Serializable]
    [RequireChartComponent(typeof(PolarCoord))]
    [ComponentHandler(typeof(AngleAxisHandler), true)]
    public class AngleAxis : Axis
    {
        [SerializeField] private float m_StartAngle = 0;

        /// <summary>
        /// Starting angle of axis. 0 degrees by default, standing for right position of center.
        /// |起始刻度的角度，默认为 0 度，即圆心的正右方。
        /// </summary>
        public float startAngle
        {
            get { return m_StartAngle; }
            set { if (PropertyUtil.SetStruct(ref m_StartAngle, value)) SetAllDirty(); }
        }

        public float GetValueAngle(float value)
        {
            return (value + context.startAngle + 360) % 360;
        }

        public float GetValueAngle(double value)
        {
            return (float) (value + context.startAngle + 360) % 360;
        }

        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Type = AxisType.Value;
            m_SplitNumber = 12;
            m_StartAngle = 0;
            m_BoundaryGap = false;
            m_Data = new List<string>(12);
            splitLine.show = true;
            splitLine.lineStyle.type = LineStyle.Type.Solid;
            axisLabel.textLimit.enable = false;
            minMaxType = AxisMinMaxType.Custom;
            min = 0;
            max = 360;
        }
    }
}