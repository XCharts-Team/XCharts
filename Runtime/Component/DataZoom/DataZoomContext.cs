using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public class DataZoomContext : MainComponentContext
    {
        public float x { get; internal set; }
        public float y { get; internal set; }
        public float width { get; internal set; }
        public float height { get; internal set; }
        public bool isDrag { get; internal set; }
        public bool isCoordinateDrag { get; internal set; }
        public bool isStartDrag { get; internal set; }
        public bool isEndDrag { get; internal set; }
        /// <summary>
        /// 运行时实际范围的开始值
        /// </summary>
        public double startValue { get; set; }
        /// <summary>
        /// 运行时实际范围的结束值
        /// </summary>
        public double endValue { get; set; }
        public bool invert { get; set; }

        public bool isMarqueeDrag { get; set; }
        public Vector3 marqueeStartPos { get; set; }
        public Vector3 marqueeEndPos { get; set; }
        public Rect marqueeRect { get; set; }
    }
}