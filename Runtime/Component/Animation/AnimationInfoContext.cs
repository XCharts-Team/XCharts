using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public sealed class AnimationInfoContext
    {
        public bool init;
        public bool start;
        public bool pause;
        public bool end;
        public float startTime;
        public float currProgress;
        public float destProgress;
        public float totalProgress;
        public float sizeProgress;
        public int currPointIndex;
        public int destPointIndex;
        public Dictionary<int, float> itemCurrProgress = new Dictionary<int, float>();
        public Dictionary<int, float> itemDestProgress = new Dictionary<int, float>();
    }
}