using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public class VisualMapContext : MainComponentContext
    {
        /// <summary>
        /// 鼠标悬停选中的index
        /// </summary>
        public int pointerIndex { get; set; }
        public double pointerValue { get; set; }
        public bool minDrag { get; internal set; }
        public bool maxDrag { get; internal set; }
        public double min { get; set; }
        public double max { get; set; }

        internal List<Color32> inRangeColors = new List<Color32>();

    }
}