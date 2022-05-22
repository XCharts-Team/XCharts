using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public class GridCoordContext : MainComponentContext
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public Vector3 position;
        public Vector3 center;
        public float left;
        public float right;
        public float bottom;
        public float top;
        public bool isPointerEnter;
        public List<ChartLabel> endLabelList = new List<ChartLabel>();
    }
}