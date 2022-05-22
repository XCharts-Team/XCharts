using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    public partial class BaseChart
    {
        public void DrawClipPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 color, bool clip, GridCoord grid)
        {
            DrawClipPolygon(vh, p1, p2, p3, p4, color, color, clip, grid);
        }

        public void DrawClipPolygon(VertexHelper vh, Vector3 p, float radius, Color32 color,
            bool clip, bool vertical, GridCoord grid)
        {
            if (!IsInChart(p)) return;
            if (!clip || (clip && (grid.Contains(p))))
                UGL.DrawSquare(vh, p, radius, color);
        }

        public void DrawClipPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 startColor, Color32 toColor, bool clip, GridCoord grid)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            ClampInChart(ref p3);
            ClampInChart(ref p4);
            if (clip)
            {
                p1 = ClampInGrid(grid, p1);
                p2 = ClampInGrid(grid, p2);
                p3 = ClampInGrid(grid, p3);
                p4 = ClampInGrid(grid, p4);
            }
            if (!clip || (clip && (grid.Contains(p1) && grid.Contains(p2) && grid.Contains(p3) &&
                    grid.Contains(p4))))
                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, startColor, toColor);
        }

        public void DrawClipPolygon(VertexHelper vh, ref Vector3 p1, ref Vector3 p2, ref Vector3 p3, ref Vector3 p4,
            Color32 startColor, Color32 toColor, bool clip, GridCoord grid)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            ClampInChart(ref p3);
            ClampInChart(ref p4);
            if (clip)
            {
                p1 = ClampInGrid(grid, p1);
                p2 = ClampInGrid(grid, p2);
                p3 = ClampInGrid(grid, p3);
                p4 = ClampInGrid(grid, p4);
            }
            if (!clip ||
                (clip && (grid.Contains(p1) && grid.Contains(p2) && grid.Contains(p3) &&
                    grid.Contains(p4))))
                UGL.DrawQuadrilateral(vh, p1, p2, p3, p4, startColor, toColor);
        }

        public void DrawClipTriangle(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Color32 color,
            bool clip, GridCoord grid)
        {
            DrawClipTriangle(vh, p1, p2, p3, color, color, color, clip, grid);
        }

        public void DrawClipTriangle(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Color32 color,
            Color32 color2, Color32 color3, bool clip, GridCoord grid)
        {
            if (!IsInChart(p1) || !IsInChart(p2) || !IsInChart(p3)) return;
            if (!clip || (clip && (grid.Contains(p1) || grid.Contains(p2) || grid.Contains(p3))))
                UGL.DrawTriangle(vh, p1, p2, p3, color, color2, color3);
        }

        public void DrawClipLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color,
            bool clip, GridCoord grid)
        {
            if (!IsInChart(p1) || !IsInChart(p2)) return;
            if (!clip || (clip && (grid.Contains(p1) || grid.Contains(p2))))
                UGL.DrawLine(vh, p1, p2, size, color);
        }

        public void DrawClipSymbol(VertexHelper vh, SymbolType type, float symbolSize, float tickness,
            Vector3 pos, Color32 color, Color32 toColor, Color32 emptyColor, Color32 borderColor, float gap,
            bool clip, float[] cornerRadius, GridCoord grid, Vector3 startPos)
        {
            if (!IsInChart(pos)) return;
            if (!clip || (clip && (grid.Contains(pos))))
                DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, emptyColor, borderColor,
                    gap, cornerRadius, startPos);
        }

        public void DrawClipZebraLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, float zebraWidth,
            float zebraGap, Color32 color, Color32 toColor, bool clip, GridCoord grid, float maxDistance)
        {
            ClampInChart(ref p1);
            ClampInChart(ref p2);
            UGL.DrawZebraLine(vh, p1, p2, size, zebraWidth, zebraGap, color, toColor, maxDistance);
        }

        public void DrawSymbol(VertexHelper vh, SymbolType type, float symbolSize, float tickness,
            Vector3 pos, Color32 color, Color32 toColor, Color32 emptyColor, Color32 borderColor,
            float gap, float[] cornerRadius)
        {
            DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, emptyColor, borderColor,
                gap, cornerRadius, Vector3.zero);
        }

        public void DrawSymbol(VertexHelper vh, SymbolType type, float symbolSize, float tickness,
            Vector3 pos, Color32 color, Color32 toColor, Color32 emptyColor, Color32 borderColor,
            float gap, float[] cornerRadius, Vector3 startPos)
        {
            var backgroundColor = GetChartBackgroundColor();
            if (ChartHelper.IsClearColor(emptyColor))
                emptyColor = backgroundColor;
            var smoothness = settings.cicleSmoothness;
            ChartDrawer.DrawSymbol(vh, type, symbolSize, tickness, pos, color, toColor, gap,
                cornerRadius, emptyColor, backgroundColor, borderColor, smoothness, startPos);
        }

        public Color32 GetXLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, GridCoord grid)
        {
            if (ChartHelper.IsValueEqualsColor(areaColor, areaToColor)) return areaColor;
            return Color32.Lerp(areaToColor, areaColor, (pos.y - grid.context.y) / grid.context.height);
        }

        public Color32 GetYLerpColor(Color32 areaColor, Color32 areaToColor, Vector3 pos, GridCoord grid)
        {
            if (ChartHelper.IsValueEqualsColor(areaColor, areaToColor)) return areaColor;
            return Color32.Lerp(areaToColor, areaColor, (pos.x - grid.context.x) / grid.context.width);
        }
    }
}