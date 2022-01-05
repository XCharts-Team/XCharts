
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Angle axis of Polar Coordinate.
    /// 极坐标系的角度轴。
    /// </summary>
    [System.Serializable]
    [ComponentHandler(typeof(AngleAxisHandler), true)]
    public class AngleAxis : Axis
    {
        [SerializeField] private float m_StartAngle = 90;

        /// <summary>
        /// Starting angle of axis. 90 degrees by default, standing for top position of center. 
        /// 0 degree stands for right position of center.
        /// 起始刻度的角度，默认为 90 度，即圆心的正上方。0 度为圆心的正右方。
        /// </summary>
        public float startAngle
        {
            get { return m_StartAngle; }
            set { if (PropertyUtil.SetStruct(ref m_StartAngle, value)) SetAllDirty(); }
        }

        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Type = AxisType.Value;
            m_SplitNumber = 12;
            m_StartAngle = 90;
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