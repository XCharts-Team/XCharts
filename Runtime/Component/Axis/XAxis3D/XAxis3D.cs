using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    /// <summary>
    /// The x axis in cartesian(rectangular) coordinate.
    /// ||直角坐标系 grid 中的 x 轴。
    /// </summary>
    [Since("v3.11.0")]
    [System.Serializable]
    [RequireChartComponent(typeof(GridCoord3D))]
    [ComponentHandler(typeof(XAxis3DHander), true)]
    public class XAxis3D : Axis
    {
        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Type = AxisType.Category;
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
            axisName.name = "X";
            axisName.labelStyle.position = LabelStyle.Position.Middle;
        }
    }
}