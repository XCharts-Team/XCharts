using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public class ParallelCoordContext : MainComponentContext
    {
        public float x;
        public float y;
        public float width;
        public float height;
        public Vector3 position;
        public float left;
        public float right;
        public float bottom;
        public float top;
        public bool runtimeIsPointerEnter;
        internal List<ParallelAxis> parallelAxes = new List<ParallelAxis>();
    }
}