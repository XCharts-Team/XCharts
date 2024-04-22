using System.Text;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    [UnityEngine.Scripting.Preserve]
    internal sealed class GridCoord3DHandler : MainComponentHandler<GridCoord3D>
    {
        public override void InitComponent()
        {
            var grid = component;
            grid.painter = chart.painter;
            grid.refreshComponent = delegate()
            {
                grid.UpdateRuntimeData(chart);
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
                component.context.isPointerEnter = component.Contains(chart.pointerPos);
            }
            else
            {
                component.context.isPointerEnter = false;
            }
        }

        public override void DrawBase(VertexHelper vh)
        {
            if (!SeriesHelper.IsAnyClipSerie(chart.series))
            {
                DrawCoord(vh, component);
            }
        }
        public override void DrawUpper(VertexHelper vh)
        {
            if (SeriesHelper.IsAnyClipSerie(chart.series))
            {
                DrawCoord(vh, component);
            }
        }

        private void DrawCoord(VertexHelper vh, GridCoord3D grid)
        {
            if (!grid.show) return;
            if (grid.showBorder)
            {
                var borderWidth = chart.theme.axis.lineWidth * 2;
                var borderColor = chart.theme.axis.lineColor;
                UGL.DrawBorder(vh, grid.context.maxRect, borderWidth, borderColor);
                // UGL.DrawBorder(vh, grid.context.center, grid.context.width - borderWidth,
                //     grid.context.height - borderWidth, borderWidth, borderColor);
            }
        }
    }
}