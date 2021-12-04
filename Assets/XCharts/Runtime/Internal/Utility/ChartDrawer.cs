/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using XUGL;

namespace XCharts
{
    public static class ChartDrawer
    {

        public static void DrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
           float tickness, Vector3 pos, Color32 color, Color32 toColor, float gap, float[] cornerRadius,
           Color32 centerFillColor, Color32 backgroundColor, float smoothness, Vector3 startPos)
        {
            switch (type)
            {
                case SerieSymbolType.None:
                    break;
                case SerieSymbolType.Circle:
                    if (gap > 0)
                    {
                        UGL.DrawDoughnut(vh, pos, symbolSize, symbolSize + gap, backgroundColor, backgroundColor, color, smoothness);
                    }
                    else
                    {
                        UGL.DrawCricle(vh, pos, symbolSize, color, toColor, smoothness);
                    }
                    break;
                case SerieSymbolType.EmptyCircle:
                    if (gap > 0)
                    {
                        UGL.DrawCricle(vh, pos, symbolSize + gap, backgroundColor, smoothness);
                        UGL.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, color, centerFillColor, smoothness);
                    }
                    else
                    {
                        UGL.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, color, centerFillColor, smoothness);
                    }
                    break;
                case SerieSymbolType.Rect:
                    if (gap > 0)
                    {
                        UGL.DrawSquare(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawSquare(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        UGL.DrawRoundRectangle(vh, pos, symbolSize, symbolSize, color, color, 0, cornerRadius, true);
                    }
                    break;
                case SerieSymbolType.EmptyRect:
                    if (gap > 0)
                    {
                        UGL.DrawSquare(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawBorder(vh, pos, symbolSize / 2, symbolSize / 2, tickness, color);
                    }
                    else
                    {
                        UGL.DrawBorder(vh, pos, symbolSize / 2, symbolSize / 2, tickness, color);
                    }
                    break;
                case SerieSymbolType.Triangle:
                    if (gap > 0)
                    {
                        UGL.DrawTriangle(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawTriangle(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        UGL.DrawTriangle(vh, pos, symbolSize, color, toColor);
                    }
                    break;
                case SerieSymbolType.EmptyTriangle:
                    if (gap > 0)
                    {
                        UGL.DrawTriangle(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawTriangle(vh, pos, symbolSize, color, toColor);
                        UGL.DrawTriangle(vh, pos, symbolSize - tickness, centerFillColor, centerFillColor);
                    }
                    else
                    {
                        UGL.DrawTriangle(vh, pos, symbolSize, color, toColor);
                        UGL.DrawTriangle(vh, pos, symbolSize - tickness, centerFillColor, centerFillColor);
                    }
                    break;
                case SerieSymbolType.Diamond:
                    if (gap > 0)
                    {
                        UGL.DrawDiamond(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawDiamond(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        UGL.DrawDiamond(vh, pos, symbolSize, color, toColor);
                    }
                    break;
                case SerieSymbolType.EmptyDiamond:
                    if (gap > 0)
                    {
                        UGL.DrawDiamond(vh, pos, symbolSize + gap, backgroundColor);
                        UGL.DrawDiamond(vh, pos, symbolSize, color, toColor);
                        UGL.DrawDiamond(vh, pos, symbolSize - tickness, centerFillColor, centerFillColor);
                    }
                    else
                    {
                        UGL.DrawDiamond(vh, pos, symbolSize, color, toColor);
                        UGL.DrawDiamond(vh, pos, symbolSize - tickness, centerFillColor, centerFillColor);
                    }
                    break;
                case SerieSymbolType.Arrow:
                    var arrowWidth = symbolSize * 2;
                    var arrowHeight = arrowWidth * 1.5f;
                    var arrowOffset = 0;
                    var arrowDent = arrowWidth / 3.3f;
                    UGL.DrawArrow(vh, startPos, pos, arrowWidth, arrowHeight,
                            arrowOffset, arrowDent, color);
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