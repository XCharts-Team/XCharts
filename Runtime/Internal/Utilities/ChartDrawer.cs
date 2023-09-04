using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts.Runtime
{
    public static class ChartDrawer
    {
        public static void DrawSymbol(VertexHelper vh, SymbolType type, float symbolSize, float tickness,
            Vector3 pos, Color32 color, Color32 toColor, float gap, float[] cornerRadius,
            Color32 emptyColor, Color32 backgroundColor, Color32 borderColor, float smoothness, Vector3 startPos)
        {
            switch (type)
            {
                case SymbolType.None:
                    break;
                case SymbolType.Circle:
                    if (gap > 0)
                    {
                        UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + gap, backgroundColor, backgroundColor, color, smoothness);
                    }
                    else
                    {
                        if (tickness > 0)
                            UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + tickness, borderColor, borderColor, color, smoothness);
                        else
                            UGL.DrawCricle(vh, pos, symbolSize, color, toColor, smoothness);
                    }
                    break;
                case SymbolType.EmptyCircle:
                    if (gap > 0)
                    {
                        UGL.DrawCricle(vh, pos, symbolSize + gap, backgroundColor, smoothness);
                        UGL.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, color, emptyColor, smoothness);
                    }
                    else
                    {
                        UGL.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, color, emptyColor, smoothness);
                    }
                    break;
                case SymbolType.Rect:
                    if (gap > 0)
                    {
                        UGL.DrawSquare(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawSquare(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        if (tickness > 0)
                        {
                            UGL.DrawRoundRectangle(vh, pos, symbolSize * 2, symbolSize * 2, color, color, 0, cornerRadius, true);
                            UGL.DrawBorder(vh, pos, symbolSize, symbolSize, tickness, borderColor, 0, cornerRadius);
                        }
                        else
                            UGL.DrawRoundRectangle(vh, pos, symbolSize * 2, symbolSize * 2, color, color, 0, cornerRadius, true);
                    }
                    break;
                case SymbolType.EmptyRect:
                    if (gap > 0)
                    {
                        UGL.DrawSquare(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawBorder(vh, pos, symbolSize * 2, symbolSize * 2, tickness, color);
                    }
                    else
                    {
                        UGL.DrawBorder(vh, pos, symbolSize * 2 - tickness * 2, symbolSize * 2 - tickness * 2, tickness, color);
                    }
                    break;
                case SymbolType.Triangle:
                case SymbolType.EmptyTriangle:
                    if (gap > 0)
                    {
                        UGL.DrawEmptyTriangle(vh, pos, symbolSize * 1.4f + gap * 2, gap * 2, backgroundColor);
                    }
                    if (type == SymbolType.EmptyTriangle)
                    {
                        UGL.DrawEmptyTriangle(vh, pos, symbolSize * 1.4f, tickness * 2f, color, emptyColor);
                    }
                    else
                    {
                        UGL.DrawTriangle(vh, pos, symbolSize * 1.4f, color, toColor);
                    }
                    break;
                case SymbolType.Diamond:
                case SymbolType.EmptyDiamond:
                    var xRadius = symbolSize;
                    var yRadius = symbolSize * 1.5f;
                    if (gap > 0)
                    {
                        UGL.DrawEmptyDiamond(vh, pos, xRadius + gap, yRadius + gap, gap, backgroundColor);
                    }
                    if (type == SymbolType.EmptyDiamond)
                    {
                        UGL.DrawEmptyDiamond(vh, pos, xRadius, yRadius, tickness, color, emptyColor);
                    }
                    else
                    {
                        UGL.DrawDiamond(vh, pos, xRadius, yRadius, color, toColor);
                    }
                    break;
                case SymbolType.Arrow:
                case SymbolType.EmptyArrow:
                    var arrowWidth = symbolSize * 2;
                    var arrowHeight = arrowWidth * 1.5f;
                    var arrowOffset = 0;
                    var arrowDent = arrowWidth / 3.3f;
                    if (gap > 0)
                    {
                        arrowWidth = (symbolSize + gap) * 2;
                        arrowHeight = arrowWidth * 1.5f;
                        arrowOffset = 0;
                        arrowDent = arrowWidth / 3.3f;
                        var dir = (pos - startPos).normalized;
                        var sharpPos = pos + gap * dir;
                        UGL.DrawArrow(vh, startPos, sharpPos, arrowWidth, arrowHeight,
                            arrowOffset, arrowDent, backgroundColor);
                    }
                    arrowWidth = symbolSize * 2;
                    arrowHeight = arrowWidth * 1.5f;
                    arrowOffset = 0;
                    arrowDent = arrowWidth / 3.3f;
                    UGL.DrawArrow(vh, startPos, pos, arrowWidth, arrowHeight,
                        arrowOffset, arrowDent, color);
                    if (type == SymbolType.EmptyArrow)
                    {
                        arrowWidth = (symbolSize - tickness) * 2;
                        arrowHeight = arrowWidth * 1.5f;
                        arrowOffset = 0;
                        arrowDent = arrowWidth / 3.3f;
                        var dir = (pos - startPos).normalized;
                        var sharpPos = pos - tickness * dir;
                        UGL.DrawArrow(vh, startPos, sharpPos, arrowWidth, arrowHeight,
                            arrowOffset, arrowDent, backgroundColor);
                    }
                    break;
                case SymbolType.Plus:
                    if (gap > 0)
                    {
                        UGL.DrawPlus(vh, pos, symbolSize + gap, tickness + gap, backgroundColor);
                    }
                    UGL.DrawPlus(vh, pos, symbolSize, tickness, color);
                    break;
                case SymbolType.Minus:
                    if (gap > 0)
                    {
                        UGL.DrawMinus(vh, pos, symbolSize + gap, tickness + gap, backgroundColor);
                    }
                    UGL.DrawMinus(vh, pos, symbolSize, tickness, color);
                    break;
            }
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle lineStyle, Vector3 startPos, Vector3 endPos,
            Color32 defaultColor, float themeWidth, LineStyle.Type themeType)
        {
            var type = lineStyle.GetType(themeType);
            var width = lineStyle.GetWidth(themeWidth);
            var color = lineStyle.GetColor(defaultColor);
            DrawLineStyle(vh, type, width, startPos, endPos, color, color);
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle lineStyle, Vector3 startPos, Vector3 endPos,
            float themeWidth, LineStyle.Type themeType, Color32 defaultColor, Color32 defaultToColor)
        {
            var type = lineStyle.GetType(themeType);
            var width = lineStyle.GetWidth(themeWidth);
            var color = lineStyle.GetColor(defaultColor);
            var toColor = ChartHelper.IsClearColor(defaultToColor) ? color : defaultToColor;
            DrawLineStyle(vh, type, width, startPos, endPos, color, toColor);
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle.Type lineType, float lineWidth,
            Vector3 startPos, Vector3 endPos, Color32 color)
        {
            DrawLineStyle(vh, lineType, lineWidth, startPos, endPos, color, color);
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle.Type lineType, float lineWidth,
            Vector3 startPos, Vector3 endPos, Color32 color, Color32 toColor)
        {
            switch (lineType)
            {
                case LineStyle.Type.Dashed:
                    UGL.DrawDashLine(vh, startPos, endPos, lineWidth, color, toColor);
                    break;
                case LineStyle.Type.Dotted:
                    UGL.DrawDotLine(vh, startPos, endPos, lineWidth, color, toColor);
                    break;
                case LineStyle.Type.Solid:
                    UGL.DrawLine(vh, startPos, endPos, lineWidth, color, toColor);
                    break;
                case LineStyle.Type.DashDot:
                    UGL.DrawDashDotLine(vh, startPos, endPos, lineWidth, color);
                    break;
                case LineStyle.Type.DashDotDot:
                    UGL.DrawDashDotDotLine(vh, startPos, endPos, lineWidth, color);
                    break;
            }
        }
    }
}