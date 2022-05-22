using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public class RadarCoordContext : MainComponentContext
    {
        /// <summary>
        /// the center position of radar in container.
        /// |雷达图在容器中的具体中心点。
        /// </summary>
        public Vector3 center { get; internal set; }
        /// <summary>
        /// the true radius of radar.
        /// |雷达图的运行时实际半径。
        /// </summary>
        public float radius { get; internal set; }
        public float dataRadius { get; internal set; }
        public bool isPointerEnter { get; set; }
    }
}