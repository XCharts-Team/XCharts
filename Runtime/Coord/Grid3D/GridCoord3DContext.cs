using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public class GridCoord3DContext : MainComponentContext
    {
        public float x;
        public float y;
        public Rect maxRect = new Rect(0, 0, 0, 0);
        public bool isPointerEnter;
        public List<ChartLabel> endLabelList = new List<ChartLabel>();
        //public Vector3 position = Vector3.zero;
        public Vector3 pointA = Vector3.zero;
        public Vector3 pointB = Vector3.zero;
        public Vector3 pointC = Vector3.zero;
        public Vector3 pointD = Vector3.zero;
        public Vector3 pointE = Vector3.zero;
        public Vector3 pointF = Vector3.zero;
        public Vector3 pointG = Vector3.zero;
        public Vector3 pointH = Vector3.zero;
    }
}