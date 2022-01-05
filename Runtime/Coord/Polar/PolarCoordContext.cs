
using System;
using UnityEngine;

namespace XCharts
{
    public class PolarCoordContext : MainComponentContext
    {
        /// <summary>
        /// the center position of polar in container.
        /// 极坐标在容器中的具体中心点。
        /// </summary>
        public Vector3 center { get; internal set; }
        /// <summary>
        /// the true radius of polar.
        /// 极坐标的运行时实际半径。
        /// </summary>
        public float radius { get; internal set; }
    }
}