/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public partial class PolarChart
    {
        /// <summary>
        /// 极坐标。
        /// </summary>
        public Polar polar { get { return m_Polar; } }
        /// <summary>
        /// Angle axis of Polar Coordinate.
        /// 极坐标系的角度轴。
        /// </summary>
        public AngleAxis angleAxis { get { return m_AngleAxis; } }
        /// <summary>
        /// Radial axis of polar coordinate.
        /// 极坐标系的径向轴。
        /// </summary>
        public RadiusAxis radiusAxis { get { return m_RadiusAxis; } }
    }
}