/******************************************/
/*                                        */
/*     Copyright (c) 2021 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    public struct AnimationStyleContext
    {
        public AnimationType type;
        internal float currentPathDistance;
        internal bool isAllItemAnimationEnd;
    }
}