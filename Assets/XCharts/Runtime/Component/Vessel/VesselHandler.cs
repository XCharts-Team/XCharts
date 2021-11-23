
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class VesselHandler : MainComponentHandler<Vessel>
    {
        public override void Update()
        {
            if (chart.isPointerInChart)
            {
                component.context.isPointerEnter = false;
                return;
            }
            var vessel = component;
            vessel.context.isPointerEnter = vessel.show
                && Vector3.Distance(vessel.context.center, chart.pointerPos) <= vessel.context.radius;
        }
    }
}