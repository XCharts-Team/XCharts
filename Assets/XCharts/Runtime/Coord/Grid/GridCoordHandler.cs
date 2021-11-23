/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class GridCoordHandler : MainComponentHandler<GridCoord>
    {
        public override void InitComponent()
        {
            var grid = component;
            grid.painter = chart.painter;
            grid.refreshComponent = delegate ()
            {
                grid.UpdateRuntimeData(chart.chartX, chart.chartY, chart.chartWidth, chart.chartHeight);
                chart.OnCoordinateChanged();
            };
            grid.refreshComponent();
        }

        public override void CheckComponent(StringBuilder sb)
        {
            var grid = component;
            if (grid.left >= chart.chartWidth)
                sb.Append("warning:grid->left > chartWidth\n");
            if (grid.right >= chart.chartWidth)
                sb.Append("warning:grid->right > chartWidth\n");
            if (grid.top >= chart.chartHeight)
                sb.Append("warning:grid->top > chartHeight\n");
            if (grid.bottom >= chart.chartHeight)
                sb.Append("warning:grid->bottom > chartHeight\n");
            if (grid.left + grid.right >= chart.chartWidth)
                sb.Append("warning:grid.left + grid.right > chartWidth\n");
            if (grid.top + grid.bottom >= chart.chartHeight)
                sb.Append("warning:grid.top + grid.bottom > chartHeight\n");
        }

        public override void Update()
        {
            if (chart.isPointerInChart)
            {
                component.context.runtimeIsPointerEnter = component.Contains(chart.pointerPos);
            }
            else
            {
                component.context.runtimeIsPointerEnter = false;
            }
        }

        public override void DrawBase(VertexHelper vh)
        {
            if (!SeriesHelper.IsAnyClipSerie(chart.series))
            {
                DrawCoord(vh);
            }
        }
        public override void DrawTop(VertexHelper vh)
        {
            if (SeriesHelper.IsAnyClipSerie(chart.series))
            {
                DrawCoord(vh);
            }
        }

        private void DrawCoord(VertexHelper vh)
        {
            var grid = component;
            if (grid.show && !ChartHelper.IsClearColor(grid.backgroundColor))
            {
                var p1 = new Vector2(grid.context.x, grid.context.y);
                var p2 = new Vector2(grid.context.x, grid.context.y + grid.context.height);
                var p3 = new Vector2(grid.context.x + grid.context.width, grid.context.y + grid.context.height);
                var p4 = new Vector2(grid.context.x + grid.context.width, grid.context.y);
                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, grid.backgroundColor);
            }
        }
    }
}