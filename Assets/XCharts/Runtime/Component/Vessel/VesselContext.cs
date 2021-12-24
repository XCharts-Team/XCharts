
using UnityEngine;

namespace XCharts
{
    public class VesselContext : MainComponentContext
    {
        /// <summary>
        /// the runtime center position of vessel.
        /// 运行时中心点。
        /// </summary>
        public Vector3 center { get; internal set; }
        /// <summary>
        /// the runtime radius of vessel.
        /// 运行时半径。
        /// </summary>
        public float radius { get; internal set; }
        /// <summary>
        /// The actual radius after deducting shapeWidth and gap.
        /// 运行时内半径。扣除厚度和间隙后的实际半径。
        /// </summary>
        public float innerRadius { get; internal set; }
        public float width { get; set; }
        public float height { get; set; }
        public bool isPointerEnter { get; set; }
    }
}