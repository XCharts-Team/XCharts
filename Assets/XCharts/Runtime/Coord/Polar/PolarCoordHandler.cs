/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class PolarCoordHandler : MainComponentHandler<PolarCoord>
    {
        public override void Update()
        {
            PolarHelper.UpdatePolarCenter(component, chart.chartPosition, chart.chartWidth, chart.chartHeight);
        }

        public override void DrawBase(VertexHelper vh)
        {
            DrawPolar(vh, component);
        }


        private void DrawPolar(VertexHelper vh, PolarCoord polar)
        {
            PolarHelper.UpdatePolarCenter(polar, chart.chartPosition, chart.chartWidth, chart.chartHeight);
            if (!ChartHelper.IsClearColor(polar.backgroundColor))
            {
                UGL.DrawCricle(vh, polar.context.center, polar.context.radius, polar.backgroundColor);
            }
        }
    }
}