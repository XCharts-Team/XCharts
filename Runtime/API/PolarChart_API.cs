/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

namespace XCharts
{
    public partial class BaseChart
    {
        /// <summary>
        /// 极坐标。
        /// </summary>
        public Polar polar { get { return m_Polars.Count > 0 ? m_Polars[0] : null; } }
        /// <summary>
        /// Angle axis of Polar Coordinate.
        /// 极坐标系的角度轴。
        /// </summary>
        public AngleAxis angleAxis { get { return m_AngleAxes.Count > 0 ? m_AngleAxes[0] : null; } }
        /// <summary>
        /// Radial axis of polar coordinate.
        /// 极坐标系的径向轴。
        /// </summary>
        public RadiusAxis radiusAxis { get { return m_RadiusAxes.Count > 0 ? m_RadiusAxes[0] : null; } }
    }
}