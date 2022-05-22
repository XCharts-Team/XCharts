using System;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class PolarCoordHandler : MainComponentHandler<PolarCoord>
    {
        public override void Update()
        {
            PolarHelper.UpdatePolarCenter(component, chart.chartPosition, chart.chartWidth, chart.chartHeight);

            if (chart.isPointerInChart)
                component.context.isPointerEnter = component.Contains(chart.pointerPos);
            else
                component.context.isPointerEnter = false;
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