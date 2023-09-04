using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public struct AnimationStyleContext
    {
        public AnimationType type;
        public bool enableSerieDataAddedAnimation;
        public float currentPathDistance;
        public bool isAllItemAnimationEnd;
    }
}