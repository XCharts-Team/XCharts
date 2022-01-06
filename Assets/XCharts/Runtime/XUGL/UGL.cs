/******************************************/
/*                                        */
/*     Copyright (c) 2020 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XUGL
{
    /// <summary>
    /// UGUI Graphics Library.
    /// UGUI 图形库
    /// </summary>
    public static class UGL
    {
        private static readonly Color32 s_ClearColor32 = new Color32(0, 0, 0, 0);
        private static readonly Vector2 s_ZeroVector2 = Vector2.zero;
        private static UIVertex[] s_Vertex = new UIVertex[4];
        private static List<Vector3> s_CurvesPosList = new List<Vector3>();

        /// <summary>
        /// Draw a arrow. 画箭头
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起始位置</param>
        /// <param name="arrowPoint">箭头位置</param>
        /// <param name="width">箭头宽</param>
        /// <param name="height">箭头长</param>
        /// <param name="offset">相对箭头位置的偏移</param>
        /// <param name="dent">箭头凹度</param>
        /// <param name="color">颜色</param>
        public static void DrawArrow(VertexHelper vh, Vector3 startPoint, Vector3 arrowPoint, float width,
           float height, float offset, float dent, Color32 color)
        {
            var dir = (arrowPoint - startPoint).normalized;
            var sharpPos = arrowPoint + (offset + height / 4) * dir;
            var middle = sharpPos + (dent - height) * dir;
            var diff = Vector3.Cross(dir, Vector3.forward).normalized * width / 2;
            var left = sharpPos - height * dir + diff;
            var right = sharpPos - height * dir - diff;
            DrawTriangle(vh, middle, sharpPos, left, color);
            DrawTriangle(vh, middle, sharpPos, right, color);
        }

        /// <summary>
        /// Draw a line. 画直线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="width">线宽</param>
        /// <param name="color">颜色</param>
        public static void DrawLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width, Color32 color)
        {
            DrawLine(vh, startPoint, endPoint, width, color, color);
        }

        /// <summary>
        /// Draw a line. 画直线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="width">线宽</param>
        /// <param name="color">颜色</param>
        /// <param name="toColor">渐变颜色</param>
        public static void DrawLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width, Color32 color, Color32 toColor)
        {
            if (startPoint == endPoint) return;
            Vector3 v = Vector3.Cross(endPoint - startPoint, Vector3.forward).normalized * width;
            s_Vertex[0].position = startPoint - v;
            s_Vertex[1].position = endPoint - v;
            s_Vertex[2].position = endPoint + v;
            s_Vertex[3].position = startPoint + v;

            for (int j = 0; j < 4; j++)
            {
                s_Vertex[j].color = j == 0 || j == 3 ? color : toColor;
                s_Vertex[j].uv0 = s_ZeroVector2;
            }
            vh.AddUIVertexQuad(s_Vertex);
        }

        /// <summary>
        /// Draw a line defined by three points. 画一条由3个点确定的折线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起始点</param>
        /// <param name="middlePoint">中间转折点</param>
        /// <param name="endPoint">终点</param>
        /// <param name="width">线宽</param>
        /// <param name="color">颜色</param>
        public static void DrawLine(VertexHelper vh, Vector3 startPoint, Vector3 middlePoint, Vector3 endPoint,
            float width, Color32 color)
        {
            var dir1 = (middlePoint - startPoint).normalized;
            var dir2 = (endPoint - middlePoint).normalized;
            var dir1v = Vector3.Cross(dir1, Vector3.forward).normalized;
            var dir2v = Vector3.Cross(dir2, Vector3.forward).normalized;
            var dir3 = (dir1 + dir2).normalized;
            var isDown = Vector3.Cross(dir1, dir2).z <= 0;
            var angle = (180 - Vector3.Angle(dir1, dir2)) * Mathf.Deg2Rad / 2;
            var diff = width / Mathf.Sin(angle);
            var dirDp = Vector3.Cross(dir3, Vector3.forward).normalized;
            var dnPos = middlePoint + (isDown ? dirDp : -dirDp) * diff;
            var upPos1 = middlePoint + (isDown ? -dir1v : dir1v) * width;
            var upPos2 = middlePoint + (isDown ? -dir2v : dir2v) * width;
            var startUp = startPoint - dir1v * width;
            var startDn = startPoint + dir1v * width;
            var endUp = endPoint - dir2v * width;
            var endDn = endPoint + dir2v * width;
            if (isDown)
            {
                DrawQuadrilateral(vh, startDn, startUp, upPos1, dnPos, color);
                DrawQuadrilateral(vh, dnPos, upPos2, endUp, endDn, color);
                DrawTriangle(vh, dnPos, upPos1, upPos2, color);
            }
            else
            {
                DrawQuadrilateral(vh, startDn, startUp, dnPos, upPos1, color);
                DrawQuadrilateral(vh, upPos2, dnPos, endUp, endDn, color);
                DrawTriangle(vh, dnPos, upPos1, upPos2, color);
            }
        }
        /// <summary>
        /// Draw a dash line. 画虚线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="width">线宽</param>
        /// <param name="color">起始颜色</param>
        /// <param name="toColor">结束颜色</param>
        /// <param name="lineLength">实线部分长度，默认为线宽的12倍</param>
        /// <param name="gapLength">间隙部分长度，默认为线宽的3倍</param>
        /// <param name="posList">可选，输出的关键点</param>
        public static void DrawDashLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width,
            Color32 color, Color32 toColor, float lineLength = 0f, float gapLength = 0f, List<Vector3> posList = null)
        {
            float dist = Vector3.Distance(startPoint, endPoint);
            if (dist < 0.1f) return;
            if (lineLength == 0) lineLength = 12 * width;
            if (gapLength == 0) gapLength = 3 * width;
            int segment = Mathf.CeilToInt(dist / (lineLength + gapLength));
            Vector3 dir = (endPoint - startPoint).normalized;
            Vector3 sp = startPoint, np;
            var isGradient = !color.Equals(toColor);
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = startPoint + dir * dist * i / segment;
                var dashep = np - dir * gapLength;
                DrawLine(vh, sp, dashep, width, isGradient ? Color32.Lerp(color, toColor, i * 1.0f / segment) : color);
                sp = np;
            }
            if (posList != null) posList.Add(endPoint);
            DrawLine(vh, sp, endPoint, width, toColor);
        }

        /// <summary>
        /// Draw a dot line. 画点线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="width">线宽</param>
        /// <param name="color">起始颜色</param>
        /// <param name="toColor">结束颜色</param>
        /// <param name="lineLength">实线部分长度，默认为线宽的3倍</param>
        /// <param name="gapLength">间隙部分长度，默认为线宽的3倍</param>
        /// <param name="posList">可选，输出的关键点</param>
        public static void DrawDotLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width,
            Color32 color, Color32 toColor, float lineLength = 0f, float gapLength = 0f, List<Vector3> posList = null)
        {
            var dist = Vector3.Distance(startPoint, endPoint);
            if (dist < 0.1f) return;
            if (lineLength == 0) lineLength = 3 * width;
            if (gapLength == 0) gapLength = 3 * width;
            var segment = Mathf.CeilToInt(dist / (lineLength + gapLength));
            var dir = (endPoint - startPoint).normalized;
            var sp = startPoint;
            var np = Vector3.zero;
            var isGradient = !color.Equals(toColor);
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = startPoint + dir * dist * i / segment;
                var dashep = np - dir * gapLength;
                DrawLine(vh, sp, dashep, width, isGradient ? Color32.Lerp(color, toColor, i * 1.0f / segment) : color);
                sp = np;
            }
            if (posList != null) posList.Add(endPoint);
            DrawLine(vh, sp, endPoint, width, toColor);
        }

        /// <summary>
        /// Draw a dash-dot line. 画点划线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="width">线宽</param>
        /// <param name="color">颜色</param>
        /// <param name="dashLength">划线长，默认15倍线宽</param>
        /// <param name="dotLength">点线长，默认3倍线宽</param>
        /// <param name="gapLength">间隙长，默认5倍线宽</param>
        /// <param name="posList">可选，输出的关键点</param>
        public static void DrawDashDotLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width,
            Color32 color, float dashLength = 0f, float dotLength = 0, float gapLength = 0f,
            List<Vector3> posList = null)
        {
            float dist = Vector3.Distance(startPoint, endPoint);
            if (dist < 0.1f) return;
            if (dashLength == 0) dashLength = 15 * width;
            if (dotLength == 0) dotLength = 3 * width;
            if (gapLength == 0) gapLength = 5 * width;
            int segment = Mathf.CeilToInt(dist / (dashLength + 2 * gapLength + dotLength));
            Vector3 dir = (endPoint - startPoint).normalized;
            Vector3 sp = startPoint, np;
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = startPoint + dir * dist * i / segment;
                var dashep = np - dir * (2 * gapLength + dotLength);
                DrawLine(vh, sp, dashep, width, color);
                if (posList != null) posList.Add(dashep);
                var dotsp = dashep + gapLength * dir;
                var dotep = dotsp + dotLength * dir;
                DrawLine(vh, dotsp, dotep, width, color);
                if (posList != null) posList.Add(dotsp);
                sp = np;
            }
            if (posList != null) posList.Add(endPoint);
            DrawLine(vh, sp, endPoint, width, color);
        }

        /// <summary>
        /// Draw a dash-dot-dot line. 画双点划线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="width">线宽</param>
        /// <param name="color">颜色</param>
        /// <param name="dashLength">折线长，默认15倍线宽</param>
        /// <param name="dotLength">点线长，默认3倍线宽</param>
        /// <param name="gapLength">间隙长，默认5倍线宽</param>
        /// <param name="posList">可选，输出的关键点</param>
        public static void DrawDashDotDotLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width,
            Color32 color, float dashLength = 0f, float dotLength = 0f, float gapLength = 0f,
            List<Vector3> posList = null)
        {
            float dist = Vector3.Distance(startPoint, endPoint);
            if (dist < 0.1f) return;
            if (dashLength == 0) dashLength = 15 * width;
            if (dotLength == 0) dotLength = 3 * width;
            if (gapLength == 0) gapLength = 5 * width;
            int segment = Mathf.CeilToInt(dist / (dashLength + 3 * gapLength + 2 * dotLength));
            Vector3 dir = (endPoint - startPoint).normalized;
            Vector3 sp = startPoint, np;
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = startPoint + dir * dist * i / segment;
                var dashep = np - dir * (3 * gapLength + 2 * dotLength);
                DrawLine(vh, sp, dashep, width, color);
                if (posList != null) posList.Add(dashep);
                var dotsp = dashep + gapLength * dir;
                var dotep = dotsp + dotLength * dir;
                DrawLine(vh, dotsp, dotep, width, color);
                if (posList != null) posList.Add(dotep);
                var dotsp2 = dotep + gapLength * dir;
                var dotep2 = dotsp2 + dotLength * dir;
                DrawLine(vh, dotsp2, dotep2, width, color);
                if (posList != null) posList.Add(dotep2);
                sp = np;
            }
            if (posList != null) posList.Add(endPoint);
            DrawLine(vh, sp, endPoint, width, color);
        }

        /// <summary>
        /// Draw a zebar-line. 画斑马线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="startPoint">起始点</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="width">线宽</param>
        /// <param name="zebraWidth">斑马条纹宽</param>
        /// <param name="zebraGap">间隙宽</param>
        /// <param name="color">起始颜色</param>
        /// <param name="toColor">结束颜色</param>
        public static void DrawZebraLine(VertexHelper vh, Vector3 startPoint, Vector3 endPoint, float width,
            float zebraWidth, float zebraGap, Color32 color, Color32 toColor, float maxDistance)
        {
            var dist = Vector3.Distance(startPoint, endPoint);
            if (dist < 0.1f) return;
            if (zebraWidth == 0) zebraWidth = 3 * width;
            if (zebraGap == 0) zebraGap = 3 * width;
            var allSegment = Mathf.CeilToInt(maxDistance / (zebraWidth + zebraGap));
            var segment = Mathf.CeilToInt(dist / maxDistance * allSegment);
            var dir = (endPoint - startPoint).normalized;
            var sp = startPoint;
            var np = Vector3.zero;
            var isGradient = !color.Equals(toColor);
            zebraWidth = (maxDistance - zebraGap * (allSegment - 1)) / allSegment;
            for (int i = 1; i <= segment; i++)
            {
                np = sp + dir * zebraWidth;
                DrawLine(vh, sp, np, width, isGradient ? Color32.Lerp(color, toColor, i * 1.0f / allSegment) : color);
                sp = np + dir * zebraGap;
            }
        }

        /// <summary>
        /// Draw a diamond. 画菱形（钻石形状）
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="center">中心点</param>
        /// <param name="size">尺寸</param>
        /// <param name="color">颜色</param>
        public static void DrawDiamond(VertexHelper vh, Vector3 center, float size, Color32 color)
        {
            DrawDiamond(vh, center, size, color, color);
        }

        /// <summary>
        /// Draw a diamond. 画菱形（钻石形状）
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="center">中心点</param>
        /// <param name="size">尺寸</param>
        /// <param name="color">渐变色1</param>
        /// <param name="toColor">渐变色2</param>
        public static void DrawDiamond(VertexHelper vh, Vector3 center, float size, Color32 color, Color32 toColor)
        {
            var p1 = new Vector2(center.x - size, center.y);
            var p2 = new Vector2(center.x, center.y + size);
            var p3 = new Vector2(center.x + size, center.y);
            var p4 = new Vector2(center.x, center.y - size);
            DrawTriangle(vh, p4, p1, p2, color, color, toColor);
            DrawTriangle(vh, p3, p4, p2, color, color, toColor);
        }

        /// <summary>
        /// Draw a square. 画正方形
        /// </summary>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="color">颜色</param>
        public static void DrawSquare(VertexHelper vh, Vector3 center, float radius, Color32 color)
        {
            DrawSquare(vh, center, radius, color, color, true);
        }

        /// <summary>
        /// Draw a square. 画带渐变的正方形
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="color">渐变色1</param>
        /// <param name="toColor">渐变色2</param>
        /// <param name="vertical">渐变是否为垂直方向</param>
        public static void DrawSquare(VertexHelper vh, Vector3 center, float radius, Color32 color,
            Color32 toColor, bool vertical = true)
        {
            Vector3 p1, p2, p3, p4;
            if (vertical)
            {
                p1 = new Vector3(center.x + radius, center.y - radius);
                p2 = new Vector3(center.x - radius, center.y - radius);
                p3 = new Vector3(center.x - radius, center.y + radius);
                p4 = new Vector3(center.x + radius, center.y + radius);
            }
            else
            {
                p1 = new Vector3(center.x - radius, center.y - radius);
                p2 = new Vector3(center.x - radius, center.y + radius);
                p3 = new Vector3(center.x + radius, center.y + radius);
                p4 = new Vector3(center.x + radius, center.y - radius);
            }
            DrawQuadrilateral(vh, p1, p2, p3, p4, color, toColor);
        }

        /// <summary>
        /// Draw a rectangle. 画带长方形
        /// </summary>
        /// <param name="p1">起始点</param>
        /// <param name="p2">结束点</param>
        /// <param name="radius">半径</param>
        /// <param name="color">颜色</param>
        public static void DrawRectangle(VertexHelper vh, Vector3 p1, Vector3 p2, float radius, Color32 color)
        {
            DrawRectangle(vh, p1, p2, radius, color, color);
        }

        /// <summary>
        /// Draw a rectangle. 画带渐变的长方形
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="p1">起始点</param>
        /// <param name="p2">结束点</param>
        /// <param name="radius">半径</param>
        /// <param name="color">渐变色1</param>
        /// <param name="toColor">渐变色2</param>
        public static void DrawRectangle(VertexHelper vh, Vector3 p1, Vector3 p2, float radius, Color32 color,
            Color32 toColor)
        {
            var dir = (p2 - p1).normalized;
            var dirv = Vector3.Cross(dir, Vector3.forward).normalized;

            var p3 = p1 + dirv * radius;
            var p4 = p1 - dirv * radius;
            var p5 = p2 - dirv * radius;
            var p6 = p2 + dirv * radius;
            DrawQuadrilateral(vh, p3, p4, p5, p6, color, toColor);
        }

        /// <summary>
        /// Draw a rectangle. 画长方形
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="p">中心点</param>
        /// <param name="xRadius">x宽</param>
        /// <param name="yRadius">y宽</param>
        /// <param name="color">颜色</param>
        /// <param name="vertical">是否垂直方向</param>
        public static void DrawRectangle(VertexHelper vh, Vector3 p, float xRadius, float yRadius,
                Color32 color, bool vertical = true)
        {
            DrawRectangle(vh, p, xRadius, yRadius, color, color, vertical);
        }

        /// <summary>
        /// Draw a rectangle. 画带渐变的长方形
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="p">中心点</param>
        /// <param name="xRadius">x宽</param>
        /// <param name="yRadius">y宽</param>
        /// <param name="color">渐变色1</param>
        /// <param name="toColor">渐变色2</param>
        /// <param name="vertical">是否垂直方向</param>
        public static void DrawRectangle(VertexHelper vh, Vector3 p, float xRadius, float yRadius,
            Color32 color, Color32 toColor, bool vertical = true)
        {
            Vector3 p1, p2, p3, p4;
            if (vertical)
            {
                p1 = new Vector3(p.x + xRadius, p.y - yRadius);
                p2 = new Vector3(p.x - xRadius, p.y - yRadius);
                p3 = new Vector3(p.x - xRadius, p.y + yRadius);
                p4 = new Vector3(p.x + xRadius, p.y + yRadius);
            }
            else
            {
                p1 = new Vector3(p.x - xRadius, p.y - yRadius);
                p2 = new Vector3(p.x - xRadius, p.y + yRadius);
                p3 = new Vector3(p.x + xRadius, p.y + yRadius);
                p4 = new Vector3(p.x + xRadius, p.y - yRadius);
            }

            DrawQuadrilateral(vh, p1, p2, p3, p4, color, toColor);
        }

        public static void DrawRectangle(VertexHelper vh, Rect rect, Color32 color)
        {
            DrawRectangle(vh, rect, 0, color);
        }
        public static void DrawRectangle(VertexHelper vh, Rect rect, float border, Color32 color)
        {
            DrawRectangle(vh, rect, border, color, color);
        }

        public static void DrawRectangle(VertexHelper vh, Rect rect, float border, Color32 color, Color32 toColor)
        {
            DrawRectangle(vh, rect.center, rect.width / 2 - border, rect.height / 2 - border, color, toColor, true);
        }

        /// <summary>
        ///  Draw a quadrilateral. 画任意的四边形
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="color"></param>
        public static void DrawQuadrilateral(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 color)
        {
            DrawQuadrilateral(vh, p1, p2, p3, p4, color, color);
        }

        /// <summary>
        /// Draw a quadrilateral. 画任意带渐变的四边形
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <param name="startColor"></param>
        /// <param name="toColor"></param>
        public static void DrawQuadrilateral(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 startColor, Color32 toColor)
        {
            s_Vertex[0].position = p1;
            s_Vertex[1].position = p2;
            s_Vertex[2].position = p3;
            s_Vertex[3].position = p4;
            for (int j = 0; j < 4; j++)
            {
                s_Vertex[j].color = j >= 2 ? toColor : startColor;
                s_Vertex[j].uv0 = s_ZeroVector2;
            }
            vh.AddUIVertexQuad(s_Vertex);
        }

        private static void InitCornerRadius(float[] cornerRadius, float width, float height, bool horizontal,
            bool invert, ref float brLt, ref float brRt, ref float brRb, ref float brLb, ref bool needRound)
        {
            if (cornerRadius == null) return;
            if (invert)
            {
                if (horizontal)
                {
                    brLt = cornerRadius.Length > 0 ? cornerRadius[1] : 0;
                    brRt = cornerRadius.Length > 1 ? cornerRadius[0] : 0;
                    brRb = cornerRadius.Length > 2 ? cornerRadius[3] : 0;
                    brLb = cornerRadius.Length > 3 ? cornerRadius[2] : 0;
                }
                else
                {
                    brLt = cornerRadius.Length > 0 ? cornerRadius[3] : 0;
                    brRt = cornerRadius.Length > 1 ? cornerRadius[2] : 0;
                    brRb = cornerRadius.Length > 2 ? cornerRadius[1] : 0;
                    brLb = cornerRadius.Length > 3 ? cornerRadius[0] : 0;
                }
            }
            else
            {
                brLt = cornerRadius.Length > 0 ? cornerRadius[0] : 0;
                brRt = cornerRadius.Length > 1 ? cornerRadius[1] : 0;
                brRb = cornerRadius.Length > 2 ? cornerRadius[2] : 0;
                brLb = cornerRadius.Length > 3 ? cornerRadius[3] : 0;
            }

            needRound = brLb != 0 || brRt != 0 || brRb != 0 || brLb != 0;
            if (needRound)
            {
                var min = Mathf.Min(width, height);
                if (brLt == 1 && brRt == 1 && brRb == 1 && brLb == 1)
                {
                    brLt = brRt = brRb = brLb = min / 2;
                    return;
                }
                if (brLt > 0 && brLt <= 1) brLt = brLt * min;
                if (brRt > 0 && brRt <= 1) brRt = brRt * min;
                if (brRb > 0 && brRb <= 1) brRb = brRb * min;
                if (brLb > 0 && brLb <= 1) brLb = brLb * min;
                if (horizontal)
                {
                    if (brLb + brLt >= height)
                    {
                        var total = brLb + brLt;
                        brLb = height * (brLb / total);
                        brLt = height * (brLt / total);
                    }
                    if (brRt + brRb >= height)
                    {
                        var total = brRt + brRb;
                        brRt = height * (brRt / total);
                        brRb = height * (brRb / total);
                    }
                    if (brLt + brRt >= width)
                    {
                        var total = brLt + brRt;
                        brLt = width * (brLt / total);
                        brRt = width * (brRt / total);
                    }
                    if (brRb + brLb >= width)
                    {
                        var total = brRb + brLb;
                        brRb = width * (brRb / total);
                        brLb = width * (brLb / total);
                    }
                }
                else
                {
                    if (brLt + brRt >= width)
                    {
                        var total = brLt + brRt;
                        brLt = width * (brLt / total);
                        brRt = width * (brRt / total);
                    }
                    if (brRb + brLb >= width)
                    {
                        var total = brRb + brLb;
                        brRb = width * (brRb / total);
                        brLb = width * (brLb / total);
                    }
                    if (brLb + brLt >= height)
                    {
                        var total = brLb + brLt;
                        brLb = height * (brLb / total);
                        brLt = height * (brLt / total);
                    }
                    if (brRt + brRb >= height)
                    {
                        var total = brRt + brRb;
                        brRt = height * (brRt / total);
                        brRb = height * (brRb / total);
                    }
                }
            }
        }

        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="center"></param>
        /// <param name="rectWidth"></param>
        /// <param name="rectHeight"></param>
        /// <param name="color"></param>
        /// <param name="toColor"></param>
        /// <param name="rotate"></param>
        /// <param name="cornerRadius"></param>
        /// <param name="isYAxis"></param>
        /// <param name="smoothness"></param>
        /// <param name="invertCorner"></param>
        public static void DrawRoundRectangle(VertexHelper vh, Vector3 center, float rectWidth, float rectHeight,
            Color32 color, Color32 toColor, float rotate = 0, float[] cornerRadius = null, bool isYAxis = false,
            float smoothness = 2, bool invertCorner = false)
        {
            var isGradient = !UGLHelper.IsValueEqualsColor(color, toColor);
            var halfWid = rectWidth / 2;
            var halfHig = rectHeight / 2;
            float brLt = 0, brRt = 0, brRb = 0, brLb = 0;
            bool needRound = false;
            InitCornerRadius(cornerRadius, rectWidth, rectHeight, isYAxis, invertCorner, ref brLt, ref brRt, ref brRb,
                ref brLb, ref needRound);
            var tempCenter = Vector3.zero;
            var lbIn = new Vector3(center.x - halfWid, center.y - halfHig);
            var ltIn = new Vector3(center.x - halfWid, center.y + halfHig);
            var rtIn = new Vector3(center.x + halfWid, center.y + halfHig);
            var rbIn = new Vector3(center.x + halfWid, center.y - halfHig);
            if (needRound)
            {
                var lbIn2 = lbIn;
                var ltIn2 = ltIn;
                var rtIn2 = rtIn;
                var rbIn2 = rbIn;
                var roundLb = lbIn;
                var roundLt = ltIn;
                var roundRt = rtIn;
                var roundRb = rbIn;
                if (brLt > 0)
                {
                    roundLt = new Vector3(center.x - halfWid + brLt, center.y + halfHig - brLt);
                    ltIn = roundLt + brLt * Vector3.left;
                    ltIn2 = roundLt + brLt * Vector3.up;
                }
                if (brRt > 0)
                {
                    roundRt = new Vector3(center.x + halfWid - brRt, center.y + halfHig - brRt);
                    rtIn = roundRt + brRt * Vector3.up;
                    rtIn2 = roundRt + brRt * Vector3.right;
                }
                if (brRb > 0)
                {
                    roundRb = new Vector3(center.x + halfWid - brRb, center.y - halfHig + brRb);
                    rbIn = roundRb + brRb * Vector3.right;
                    rbIn2 = roundRb + brRb * Vector3.down;
                }
                if (brLb > 0)
                {
                    roundLb = new Vector3(center.x - halfWid + brLb, center.y - halfHig + brLb);
                    lbIn = roundLb + brLb * Vector3.left;
                    lbIn2 = roundLb + brLb * Vector3.down;
                }

                if (isYAxis)
                {
                    var maxLeft = Mathf.Max(brLt, brLb);
                    var maxRight = Mathf.Max(brRt, brRb);
                    var ltInRight = ltIn + maxLeft * Vector3.right;
                    var lbInRight = lbIn + maxLeft * Vector3.right;
                    var rtIn2Left = rtIn2 + maxRight * Vector3.left;
                    var rbInLeft = rbIn + maxRight * Vector3.left;

                    var roundLbRight = roundLb + (maxLeft - brLb) * Vector3.right;
                    var lbIn2Right = lbIn2 + (maxLeft - brLb) * Vector3.right;
                    if (roundLbRight.x > roundRb.x) roundLbRight.x = roundRb.x;
                    if (lbIn2Right.x > roundRb.x) lbIn2Right.x = roundRb.x;

                    var ltIn2Right = ltIn2 + (maxLeft - brLt) * Vector3.right;
                    var roundLtRight = roundLt + (maxLeft - brLt) * Vector3.right;
                    if (ltIn2Right.x > roundRt.x) ltIn2Right.x = roundRt.x;
                    if (roundLtRight.x > roundRt.x) roundLtRight.x = roundRt.x;

                    var roundRtLeft = roundRt + (maxRight - brRt) * Vector3.left;
                    var rtInLeft = rtIn + (maxRight - brRt) * Vector3.left;
                    if (roundRtLeft.x < roundLt.x) roundRtLeft.x = roundLt.x;
                    if (rtInLeft.x < roundLt.x) rtInLeft.x = roundLt.x;

                    var rbIn2Left = rbIn2 + (maxRight - brRb) * Vector3.left;
                    var roundRbLeft = roundRb + (maxRight - brRb) * Vector3.left;
                    if (rbIn2Left.x < roundLb.x) rbIn2Left.x = roundLb.x;
                    if (roundRbLeft.x < roundLb.x) roundRbLeft.x = roundLb.x;
                    if (!isGradient)
                    {
                        DrawSector(vh, roundLt, brLt, color, color, 270, 360, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRt, brRt, toColor, toColor, 0, 90, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRb, brRb, toColor, toColor, 90, 180, 1, isYAxis, smoothness);
                        DrawSector(vh, roundLb, brLb, color, color, 180, 270, 1, isYAxis, smoothness);

                        DrawQuadrilateral(vh, ltIn, ltInRight, lbInRight, lbIn, color, color);
                        DrawQuadrilateral(vh, lbIn2, roundLb, roundLbRight, lbIn2Right, color, color);
                        DrawQuadrilateral(vh, roundLt, ltIn2, ltIn2Right, roundLtRight, color, color);

                        DrawQuadrilateral(vh, rbInLeft, rtIn2Left, rtIn2, rbIn, toColor, toColor);
                        DrawQuadrilateral(vh, roundRtLeft, rtInLeft, rtIn, roundRt, toColor, toColor);
                        DrawQuadrilateral(vh, rbIn2Left, roundRbLeft, roundRb, rbIn2, toColor, toColor);

                        var clt = new Vector3(center.x - halfWid + maxLeft, center.y + halfHig);
                        var crt = new Vector3(center.x + halfWid - maxRight, center.y + halfHig);
                        var crb = new Vector3(center.x + halfWid - maxRight, center.y - halfHig);
                        var clb = new Vector3(center.x - halfWid + maxLeft, center.y - halfHig);
                        if (crt.x > clt.x)
                        {
                            DrawQuadrilateral(vh, clb, clt, crt, crb, color, toColor);
                        }
                    }
                    else
                    {
                        var tempLeftColor = Color32.Lerp(color, toColor, maxLeft / rectWidth);
                        var upLeftColor = Color32.Lerp(color, tempLeftColor, brLt / maxLeft);
                        var downLeftColor = Color32.Lerp(color, tempLeftColor, brLb / maxLeft);

                        var tempRightColor = Color32.Lerp(color, toColor, (rectWidth - maxRight) / rectWidth);
                        var upRightColor = Color32.Lerp(tempRightColor, toColor, (maxRight - brRt) / maxRight);
                        var downRightColor = Color32.Lerp(tempRightColor, toColor, (maxRight - brRb) / maxRight);

                        DrawSector(vh, roundLt, brLt, color, upLeftColor, 270, 360, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRt, brRt, upRightColor, toColor, 0, 90, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRb, brRb, downRightColor, toColor, 90, 180, 1, isYAxis, smoothness);
                        DrawSector(vh, roundLb, brLb, color, downLeftColor, 180, 270, 1, isYAxis, smoothness);

                        DrawQuadrilateral(vh, lbIn, ltIn, ltInRight, lbInRight, color, tempLeftColor);
                        DrawQuadrilateral(vh, lbIn2, roundLb, roundLbRight, lbIn2Right, downLeftColor,
                            roundLbRight.x == roundRb.x ? downRightColor : tempLeftColor);
                        DrawQuadrilateral(vh, roundLt, ltIn2, ltIn2Right, roundLtRight, upLeftColor,
                            ltIn2Right.x == roundRt.x ? upRightColor : tempLeftColor);

                        DrawQuadrilateral(vh, rbInLeft, rtIn2Left, rtIn2, rbIn, tempRightColor, toColor);
                        DrawQuadrilateral(vh, roundRtLeft, rtInLeft, rtIn, roundRt,
                            roundRtLeft.x == roundLt.x ? upLeftColor : tempRightColor, upRightColor);
                        DrawQuadrilateral(vh, rbIn2Left, roundRbLeft, roundRb, rbIn2,
                            rbIn2Left.x == roundLb.x ? downLeftColor : tempRightColor, downRightColor);

                        var clt = new Vector3(center.x - halfWid + maxLeft, center.y + halfHig);
                        var crt = new Vector3(center.x + halfWid - maxRight, center.y + halfHig);
                        var crb = new Vector3(center.x + halfWid - maxRight, center.y - halfHig);
                        var clb = new Vector3(center.x - halfWid + maxLeft, center.y - halfHig);
                        if (crt.x > clt.x)
                        {
                            DrawQuadrilateral(vh, clb, clt, crt, crb, tempLeftColor, tempRightColor);
                        }
                    }
                }
                else
                {
                    var maxup = Mathf.Max(brLt, brRt);
                    var maxdown = Mathf.Max(brLb, brRb);
                    var clt = new Vector3(center.x - halfWid, center.y + halfHig - maxup);
                    var crt = new Vector3(center.x + halfWid, center.y + halfHig - maxup);
                    var crb = new Vector3(center.x + halfWid, center.y - halfHig + maxdown);
                    var clb = new Vector3(center.x - halfWid, center.y - halfHig + maxdown);
                    var lbIn2Up = lbIn2 + maxdown * Vector3.up;
                    var rbIn2Up = rbIn2 + maxdown * Vector3.up;
                    var rtInDown = rtIn + maxup * Vector3.down;
                    var ltIn2Down = ltIn2 + maxup * Vector3.down;

                    var roundLtDown = roundLt + (maxup - brLt) * Vector3.down;
                    var ltInDown = ltIn + (maxup - brLt) * Vector3.down;
                    if (roundLtDown.y < roundLb.y) roundLtDown.y = roundLb.y;
                    if (ltInDown.y < roundLb.y) ltInDown.y = roundLb.y;

                    var rtIn2Down = rtIn2 + (maxup - brRt) * Vector3.down;
                    var roundRtDown = roundRt + (maxup - brRt) * Vector3.down;
                    if (rtIn2Down.y < roundRb.y) rtIn2Down.y = roundRb.y;
                    if (roundRtDown.y < roundRb.y) roundRtDown.y = roundRb.y;

                    var lbInUp = lbIn + (maxdown - brLb) * Vector3.up;
                    var roundLbUp = roundLb + (maxdown - brLb) * Vector3.up;
                    if (lbInUp.y > roundLt.y) lbInUp.y = roundLt.y;
                    if (roundLbUp.y > roundLt.y) roundLbUp.y = roundLt.y;

                    var roundRbUp = roundRb + (maxdown - brRb) * Vector3.up;
                    var rbInUp = rbIn + (maxdown - brRb) * Vector3.up;
                    if (roundRbUp.y > roundRt.y) roundRbUp.y = roundRt.y;
                    if (rbInUp.y > roundRt.y) rbInUp.y = roundRt.y;

                    if (!isGradient)
                    {
                        DrawSector(vh, roundLt, brLt, toColor, toColor, 270, 360, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRt, brRt, toColor, toColor, 0, 90, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRb, brRb, color, color, 90, 180, 1, isYAxis, smoothness);
                        DrawSector(vh, roundLb, brLb, color, color, 180, 270, 1, isYAxis, smoothness);

                        DrawQuadrilateral(vh, ltIn2, rtIn, rtInDown, ltIn2Down, toColor, toColor);
                        DrawQuadrilateral(vh, ltIn, roundLt, roundLtDown, ltInDown, toColor, toColor);
                        DrawQuadrilateral(vh, roundRt, rtIn2, rtIn2Down, roundRtDown, toColor, toColor);

                        DrawQuadrilateral(vh, lbIn2, lbIn2Up, rbIn2Up, rbIn2, color, color);
                        DrawQuadrilateral(vh, lbIn, lbInUp, roundLbUp, roundLb, color, color);
                        DrawQuadrilateral(vh, roundRb, roundRbUp, rbInUp, rbIn, color, color);
                        if (clt.y > clb.y)
                        {
                            DrawQuadrilateral(vh, clt, crt, crb, clb, toColor, color);
                        }
                    }
                    else
                    {
                        var tempUpColor = Color32.Lerp(color, toColor, (rectHeight - maxup) / rectHeight);
                        var leftUpColor = Color32.Lerp(tempUpColor, toColor, (maxup - brLt) / maxup);
                        var rightUpColor = Color32.Lerp(tempUpColor, toColor, (maxup - brRt) / maxup);
                        var tempDownColor = Color32.Lerp(color, toColor, maxdown / rectHeight);
                        var leftDownColor = Color32.Lerp(color, tempDownColor, brLb / maxdown);
                        var rightDownColor = Color32.Lerp(color, tempDownColor, brRb / maxdown);

                        DrawSector(vh, roundLt, brLt, leftUpColor, toColor, 270, 360, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRt, brRt, rightUpColor, toColor, 0, 90, 1, isYAxis, smoothness);
                        DrawSector(vh, roundRb, brRb, rightDownColor, color, 90, 180, 1, isYAxis, smoothness);
                        DrawSector(vh, roundLb, brLb, leftDownColor, color, 180, 270, 1, isYAxis, smoothness);

                        DrawQuadrilateral(vh, ltIn2, rtIn, rtInDown, ltIn2Down, toColor, tempUpColor);
                        DrawQuadrilateral(vh, ltIn, roundLt, roundLtDown, ltInDown, leftUpColor,
                            roundLtDown.y == roundLb.y ? leftDownColor : tempUpColor);
                        DrawQuadrilateral(vh, roundRt, rtIn2, rtIn2Down, roundRtDown, rightUpColor,
                            rtIn2Down.y == roundRb.y ? rightDownColor : tempUpColor);

                        DrawQuadrilateral(vh, rbIn2, lbIn2, lbIn2Up, rbIn2Up, color, tempDownColor);
                        DrawQuadrilateral(vh, roundLb, lbIn, lbInUp, roundLbUp, leftDownColor,
                            lbInUp.y == roundLt.y ? leftUpColor : tempDownColor);
                        DrawQuadrilateral(vh, rbIn, roundRb, roundRbUp, rbInUp, rightDownColor,
                            roundRbUp.y == roundRt.y ? rightUpColor : tempDownColor);
                        if (clt.y > clb.y)
                        {
                            DrawQuadrilateral(vh, clt, crt, crb, clb, tempUpColor, tempDownColor);
                        }
                    }
                }
            }
            else
            {
                DrawQuadrilateral(vh, lbIn, ltIn, rtIn, rbIn, toColor, color);
            }
        }

        /// <summary>
        /// 绘制（圆角）边框
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="center"></param>
        /// <param name="rectWidth"></param>
        /// <param name="rectHeight"></param>
        /// <param name="borderWidth"></param>
        /// <param name="color"></param>
        /// <param name="rotate"></param>
        /// <param name="cornerRadius"></param>
        /// <param name="invertCorner"></param>
        public static void DrawBorder(VertexHelper vh, Vector3 center, float rectWidth, float rectHeight,
            float borderWidth, Color32 color, float rotate = 0, float[] cornerRadius = null,
            bool horizontal = false, float smoothness = 1f, bool invertCorner = false)
        {
            DrawBorder(vh, center, rectWidth, rectHeight, borderWidth, color, s_ClearColor32, rotate,
                cornerRadius, horizontal, smoothness, invertCorner);
        }

        /// <summary>
        /// 绘制（圆角）边框
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="rect"></param>
        /// <param name="borderWidth"></param>
        /// <param name="color"></param>
        /// <param name="rotate"></param>
        /// <param name="cornerRadius"></param>
        /// <param name="horizontal"></param>
        /// <param name="smoothness"></param>
        /// <param name="invertCorner"></param>
        public static void DrawBorder(VertexHelper vh, Rect rect,
            float borderWidth, Color32 color, float rotate = 0, float[] cornerRadius = null,
            bool horizontal = false, float smoothness = 1f, bool invertCorner = false)
        {
            DrawBorder(vh, rect.center, rect.width, rect.height, borderWidth, color, s_ClearColor32, rotate,
                cornerRadius, horizontal, smoothness, invertCorner);
        }

        /// <summary>
        /// 绘制（圆角）边框
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="center"></param>
        /// <param name="rectWidth"></param>
        /// <param name="rectHeight"></param>
        /// <param name="borderWidth"></param>
        /// <param name="color"></param>
        /// <param name="toColor"></param>
        /// <param name="rotate"></param>
        /// <param name="cornerRadius"></param>
        /// <param name="horizontal"></param>
        /// <param name="smoothness"></param>
        /// <param name="invertCorner"></param>
        public static void DrawBorder(VertexHelper vh, Vector3 center, float rectWidth, float rectHeight,
            float borderWidth, Color32 color, Color32 toColor, float rotate = 0, float[] cornerRadius = null,
            bool horizontal = false, float smoothness = 1f, bool invertCorner = false)
        {
            if (borderWidth == 0 || UGLHelper.IsClearColor(color)) return;
            var halfWid = rectWidth / 2;
            var halfHig = rectHeight / 2;
            var lbIn = new Vector3(center.x - halfWid, center.y - halfHig);
            var lbOt = new Vector3(center.x - halfWid - borderWidth, center.y - halfHig - borderWidth);
            var ltIn = new Vector3(center.x - halfWid, center.y + halfHig);
            var ltOt = new Vector3(center.x - halfWid - borderWidth, center.y + halfHig + borderWidth);
            var rtIn = new Vector3(center.x + halfWid, center.y + halfHig);
            var rtOt = new Vector3(center.x + halfWid + borderWidth, center.y + halfHig + borderWidth);
            var rbIn = new Vector3(center.x + halfWid, center.y - halfHig);
            var rbOt = new Vector3(center.x + halfWid + borderWidth, center.y - halfHig - borderWidth);
            float brLt = 0, brRt = 0, brRb = 0, brLb = 0;
            bool needRound = false;
            InitCornerRadius(cornerRadius, rectWidth, rectHeight, horizontal, invertCorner, ref brLt, ref brRt, ref brRb,
                ref brLb, ref needRound);
            var tempCenter = Vector3.zero;
            if (UGLHelper.IsClearColor(toColor))
            {
                toColor = color;
            }
            if (needRound)
            {
                var lbIn2 = lbIn;
                var lbOt2 = lbOt;
                var ltIn2 = ltIn;
                var ltOt2 = ltOt;
                var rtIn2 = rtIn;
                var rtOt2 = rtOt;
                var rbIn2 = rbIn;
                var rbOt2 = rbOt;
                if (brLt > 0)
                {
                    tempCenter = new Vector3(center.x - halfWid + brLt, center.y + halfHig - brLt);
                    DrawDoughnut(vh, tempCenter, brLt, brLt + borderWidth, horizontal ? color : toColor, s_ClearColor32,
                        270, 360, smoothness);
                    ltIn = tempCenter + brLt * Vector3.left;
                    ltOt = tempCenter + (brLt + borderWidth) * Vector3.left;
                    ltIn2 = tempCenter + brLt * Vector3.up;
                    ltOt2 = tempCenter + (brLt + borderWidth) * Vector3.up;
                }
                if (brRt > 0)
                {
                    tempCenter = new Vector3(center.x + halfWid - brRt, center.y + halfHig - brRt);
                    DrawDoughnut(vh, tempCenter, brRt, brRt + borderWidth, toColor, s_ClearColor32, 0, 90, smoothness);
                    rtIn = tempCenter + brRt * Vector3.up;
                    rtOt = tempCenter + (brRt + borderWidth) * Vector3.up;
                    rtIn2 = tempCenter + brRt * Vector3.right;
                    rtOt2 = tempCenter + (brRt + borderWidth) * Vector3.right;
                }
                if (brRb > 0)
                {
                    tempCenter = new Vector3(center.x + halfWid - brRb, center.y - halfHig + brRb);
                    DrawDoughnut(vh, tempCenter, brRb, brRb + borderWidth, horizontal ? toColor : color, s_ClearColor32,
                        90, 180, smoothness);
                    rbIn = tempCenter + brRb * Vector3.right;
                    rbOt = tempCenter + (brRb + borderWidth) * Vector3.right;
                    rbIn2 = tempCenter + brRb * Vector3.down;
                    rbOt2 = tempCenter + (brRb + borderWidth) * Vector3.down;
                }
                if (brLb > 0)
                {
                    tempCenter = new Vector3(center.x - halfWid + brLb, center.y - halfHig + brLb);
                    DrawDoughnut(vh, tempCenter, brLb, brLb + borderWidth, color, s_ClearColor32, 180, 270, smoothness);
                    lbIn = tempCenter + brLb * Vector3.left;
                    lbOt = tempCenter + (brLb + borderWidth) * Vector3.left;
                    lbIn2 = tempCenter + brLb * Vector3.down;
                    lbOt2 = tempCenter + (brLb + borderWidth) * Vector3.down;
                }
                if (horizontal)
                {
                    DrawQuadrilateral(vh, lbIn, lbOt, ltOt, ltIn, color, color);
                    DrawQuadrilateral(vh, ltIn2, ltOt2, rtOt, rtIn, color, toColor);
                    DrawQuadrilateral(vh, rtIn2, rtOt2, rbOt, rbIn, toColor, toColor);
                    DrawQuadrilateral(vh, rbIn2, rbOt2, lbOt2, lbIn2, toColor, color);
                }
                else
                {
                    DrawQuadrilateral(vh, lbIn, lbOt, ltOt, ltIn, color, toColor);
                    DrawQuadrilateral(vh, ltIn2, ltOt2, rtOt, rtIn, toColor, toColor);
                    DrawQuadrilateral(vh, rtIn2, rtOt2, rbOt, rbIn, toColor, color);
                    DrawQuadrilateral(vh, rbIn2, rbOt2, lbOt2, lbIn2, color, color);
                }
            }
            else
            {
                if (rotate > 0)
                {
                    lbIn = UGLHelper.RotateRound(lbIn, center, Vector3.forward, rotate);
                    lbOt = UGLHelper.RotateRound(lbOt, center, Vector3.forward, rotate);
                    ltIn = UGLHelper.RotateRound(ltIn, center, Vector3.forward, rotate);
                    ltOt = UGLHelper.RotateRound(ltOt, center, Vector3.forward, rotate);
                    rtIn = UGLHelper.RotateRound(rtIn, center, Vector3.forward, rotate);
                    rtOt = UGLHelper.RotateRound(rtOt, center, Vector3.forward, rotate);
                    rbIn = UGLHelper.RotateRound(rbIn, center, Vector3.forward, rotate);
                    rbOt = UGLHelper.RotateRound(rbOt, center, Vector3.forward, rotate);
                }
                if (horizontal)
                {
                    DrawQuadrilateral(vh, lbIn, lbOt, ltOt, ltIn, color, color);
                    DrawQuadrilateral(vh, ltIn, ltOt, rtOt, rtIn, color, toColor);
                    DrawQuadrilateral(vh, rtIn, rtOt, rbOt, rbIn, toColor, toColor);
                    DrawQuadrilateral(vh, rbIn, rbOt, lbOt, lbIn, toColor, color);
                }
                else
                {
                    DrawQuadrilateral(vh, lbIn, lbOt, ltOt, ltIn, color, toColor);
                    DrawQuadrilateral(vh, ltIn, ltOt, rtOt, rtIn, toColor, toColor);
                    DrawQuadrilateral(vh, rtIn, rtOt, rbOt, rbIn, toColor, color);
                    DrawQuadrilateral(vh, rbIn, rbOt, lbOt, lbIn, color, color);
                }
            }
        }

        public static void DrawTriangle(VertexHelper vh, Vector3 p1,
            Vector3 p2, Vector3 p3, Color32 color)
        {
            DrawTriangle(vh, p1, p2, p3, color, color, color);
        }

        public static void DrawTriangle(VertexHelper vh, Vector3 pos, float size, Color32 color)
        {
            DrawTriangle(vh, pos, size, color, color);
        }

        public static void DrawTriangle(VertexHelper vh, Vector3 pos, float size, Color32 color, Color32 toColor)
        {
            var x = size * Mathf.Cos(30 * Mathf.PI / 180);
            var y = size * Mathf.Sin(30 * Mathf.PI / 180);
            var p1 = new Vector2(pos.x - x, pos.y - y);
            var p2 = new Vector2(pos.x, pos.y + size);
            var p3 = new Vector2(pos.x + x, pos.y - y);
            DrawTriangle(vh, p1, p2, p3, color, toColor, color);
        }

        public static void DrawTriangle(VertexHelper vh, Vector3 p1,
           Vector3 p2, Vector3 p3, Color32 color, Color32 color2, Color32 color3)
        {
            UIVertex v1 = new UIVertex();
            v1.position = p1;
            v1.color = color;
            v1.uv0 = s_ZeroVector2;
            UIVertex v2 = new UIVertex();
            v2.position = p2;
            v2.color = color2;
            v2.uv0 = s_ZeroVector2;
            UIVertex v3 = new UIVertex();
            v3.position = p3;
            v3.color = color3;
            v3.uv0 = s_ZeroVector2;
            int startIndex = vh.currentVertCount;
            vh.AddVert(v1);
            vh.AddVert(v2);
            vh.AddVert(v3);
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 center, float radius, Color32 color,
            float smoothness = 2f)
        {
            DrawCricle(vh, center, radius, color, color, 0, s_ClearColor32, smoothness);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 center, float radius, Color32 color,
           Color32 toColor, float smoothness = 2f)
        {
            DrawSector(vh, center, radius, color, toColor, 0, 360, 0, s_ClearColor32, smoothness);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 center, float radius, Color32 color,
            Color32 toColor, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawSector(vh, center, radius, color, toColor, 0, 360, borderWidth, borderColor, smoothness);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 center, float radius, Color32 color,
            float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawCricle(vh, center, radius, color, color, borderWidth, borderColor, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 center, float radius, float tickness,
            Color32 color, Color32 emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, center, radius - tickness, radius, color, color, emptyColor, 0, 360, 0, s_ClearColor32,
                0, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 center, float radius, float tickness,
            Color32 color, Color32 emptyColor, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, center, radius - tickness, radius, color, color, emptyColor, 0, 360, borderWidth,
                borderColor, 0, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 center, float radius, float tickness,
            Color32 color, Color32 toColor, Color32 emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, center, radius - tickness, radius, color, toColor, emptyColor, 0, 360, 0,
                s_ClearColor32, 0, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 center, float radius, float tickness,
            Color32 color, Color32 toColor, Color32 emptyColor, float borderWidth, Color32 borderColor,
            float smoothness = 2f)
        {
            DrawDoughnut(vh, center, radius - tickness, radius, color, toColor, emptyColor, 0, 360, borderWidth,
                borderColor, 0, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 center, float radius, Color32 color,
                    float startDegree, float toDegree, float smoothness = 2f)
        {
            DrawSector(vh, center, radius, color, color, startDegree, toDegree, 0, s_ClearColor32, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 center, float radius, Color32 color, Color32 toColor,
                    float startDegree, float toDegree, int gradientType = 0, bool isYAxis = false, float smoothness = 2f)
        {
            DrawSector(vh, center, radius, color, toColor, startDegree, toDegree, 0, s_ClearColor32, 0, smoothness,
                gradientType, isYAxis);
        }

        public static void DrawSector(VertexHelper vh, Vector3 center, float radius, Color32 color,
            float startDegree, float toDegree, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawSector(vh, center, radius, color, color, startDegree, toDegree, borderWidth, borderColor, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 center, float radius, Color32 color, Color32 toColor,
            float startDegree, float toDegree, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawSector(vh, center, radius, color, toColor, startDegree, toDegree, borderWidth, borderColor, 0, smoothness);
        }

        /// <summary>
        /// 绘制扇形（可带边框、有空白边距、3种类型渐变）
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="center">中心点</param>
        /// <param name="radius">半径</param>
        /// <param name="color">颜色</param>
        /// <param name="toColor">渐变颜色</param>
        /// <param name="startDegree">开始角度</param>
        /// <param name="toDegree">结束角度</param>
        /// <param name="borderWidth">边框宽度</param>
        /// <param name="borderColor">边框颜色</param>
        /// <param name="space">边距</param>
        /// <param name="smoothness">光滑度</param>
        /// <param name="gradientType">渐变类型，0:向圆形渐变，1:水平或垂直渐变，2:开始角度向结束角度渐变</param>
        /// <param name="isYAxis">水平渐变还是垂直渐变，gradientType为1时生效</param>
        public static void DrawSector(VertexHelper vh, Vector3 center, float radius, Color32 color, Color32 toColor,
            float startDegree, float toDegree, float borderWidth, Color32 borderColor, float space,
            float smoothness, int gradientType = 0, bool isYAxis = false)
        {
            if (radius == 0) return;
            if (space > 0 && Mathf.Abs(toDegree - startDegree) >= 360) space = 0;
            radius -= borderWidth;
            smoothness = (smoothness < 0 ? 2f : smoothness);
            int segments = (int)((2 * Mathf.PI * radius) * (Mathf.Abs(toDegree - startDegree) / 360) / smoothness);
            if (segments < 1) segments = 1;
            float startAngle = startDegree * Mathf.Deg2Rad;
            float toAngle = toDegree * Mathf.Deg2Rad;
            float realStartAngle = startAngle;
            float realToAngle = toAngle;
            float halfAngle = (toAngle - startAngle) / 2;
            float borderAngle = 0;
            float spaceAngle = 0;

            var p2 = center + radius * UGLHelper.GetDire(startAngle);
            var p3 = Vector3.zero;
            var p4 = Vector3.zero;
            var spaceCenter = center;
            var realCenter = center;
            var lastP4 = center;
            var lastColor = color;
            var needBorder = borderWidth != 0;
            var needSpace = space != 0;
            var borderLineWidth = needSpace ? borderWidth : borderWidth / 2;
            var lastPos = Vector3.zero;
            var middleDire = UGLHelper.GetDire(startAngle + halfAngle);
            if (needBorder || needSpace)
            {
                float spaceDiff = 0f;
                float borderDiff = 0f;
                if (needSpace)
                {
                    spaceDiff = space / Mathf.Sin(halfAngle);
                    spaceCenter = center + spaceDiff * middleDire;
                    realCenter = spaceCenter;
                    spaceAngle = 2 * Mathf.Asin(space / (2 * radius));
                    realStartAngle = startAngle + spaceAngle;
                    realToAngle = toAngle - spaceAngle;
                    if (realToAngle < realStartAngle) realToAngle = realStartAngle;
                    p2 = UGLHelper.GetPos(center, radius, realStartAngle);
                }
                if (needBorder)
                {
                    borderDiff = borderLineWidth / Mathf.Sin(halfAngle);
                    realCenter += borderDiff * middleDire;
                    borderAngle = 2 * Mathf.Asin(borderLineWidth / (2 * radius));
                    realStartAngle = realStartAngle + borderAngle;
                    realToAngle = realToAngle - borderAngle;
                    if (realToAngle < realStartAngle)
                    {
                        realToAngle = realStartAngle;
                        p2 = UGLHelper.GetPos(center, radius, realStartAngle);
                    }
                    else
                    {
                        var borderX1 = UGLHelper.GetPos(center, radius, realStartAngle);
                        DrawQuadrilateral(vh, realCenter, spaceCenter, p2, borderX1, borderColor);
                        p2 = borderX1;

                        var borderX2 = UGLHelper.GetPos(center, radius, realToAngle);
                        var pEnd = UGLHelper.GetPos(center, radius, toAngle - spaceAngle);
                        DrawQuadrilateral(vh, realCenter, borderX2, pEnd, spaceCenter, borderColor);
                    }
                }
            }
            float segmentAngle = (realToAngle - realStartAngle) / segments;
            bool isLeft = startDegree >= 180;
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = realStartAngle + i * segmentAngle;
                p3 = center + radius * UGLHelper.GetDire(currAngle);
                if (gradientType == 1)
                {
                    if (isYAxis)
                    {
                        p4 = new Vector3(p3.x, realCenter.y);
                        var dist = p4.x - realCenter.x;
                        var tcolor = Color32.Lerp(color, toColor, dist >= 0
                            ? dist / radius
                            : Mathf.Min(radius + dist, radius) / radius);
                        if (isLeft && (i == segments || i == 0)) tcolor = toColor;
                        DrawQuadrilateral(vh, lastP4, p2, p3, p4, lastColor, tcolor);
                        lastP4 = p4;
                        lastColor = tcolor;
                    }
                    else
                    {
                        p4 = new Vector3(realCenter.x, p3.y);
                        var tcolor = Color32.Lerp(color, toColor, Mathf.Abs(p4.y - realCenter.y) / radius);
                        DrawQuadrilateral(vh, lastP4, p2, p3, p4, lastColor, tcolor);
                        lastP4 = p4;
                        lastColor = tcolor;
                    }
                }
                else if (gradientType == 2)
                {
                    var tcolor = Color32.Lerp(color, toColor, i / segments);
                    DrawQuadrilateral(vh, realCenter, p2, p3, realCenter, lastColor, tcolor);
                    lastColor = tcolor;
                }
                else
                {
                    DrawTriangle(vh, realCenter, p2, p3, toColor, color, color);
                }
                p2 = p3;

            }
            if (needBorder || needSpace)
            {
                if (realToAngle > realStartAngle)
                {
                    var borderX2 = center + radius * UGLHelper.GetDire(realToAngle);
                    DrawTriangle(vh, realCenter, p2, borderX2, toColor, color, color);
                    if (needBorder)
                    {
                        var realStartDegree = (realStartAngle - borderAngle) * Mathf.Rad2Deg;
                        var realToDegree = (realToAngle + borderAngle) * Mathf.Rad2Deg;
                        DrawDoughnut(vh, center, radius, radius + borderWidth, borderColor, s_ClearColor32,
                            realStartDegree, realToDegree, smoothness);
                    }
                }
            }
        }

        public static void DrawRoundCap(VertexHelper vh, Vector3 center, float width, float radius, float angle,
            bool clockwise, Color32 color, bool end)
        {
            var px = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            var py = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            var pos = new Vector3(px, py) + center;
            if (end)
            {
                if (clockwise)
                    DrawSector(vh, pos, width, color, angle, angle + 180, 0, s_ClearColor32);
                else
                    DrawSector(vh, pos, width, color, angle, angle - 180, 0, s_ClearColor32);
            }
            else
            {
                if (clockwise)
                    DrawSector(vh, pos, width, color, angle + 180, angle + 360, 0, s_ClearColor32);
                else
                    DrawSector(vh, pos, width, color, angle - 180, angle - 360, 0, s_ClearColor32);
            }
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 center, float insideRadius, float outsideRadius,
            Color32 color, Color32 emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, center, insideRadius, outsideRadius, color, color, emptyColor, 0, 360, 0,
                s_ClearColor32, 0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 center, float insideRadius, float outsideRadius,
            Color32 color, Color32 emptyColor, float startDegree,
            float toDegree, float smoothness = 1f)
        {
            DrawDoughnut(vh, center, insideRadius, outsideRadius, color, color, emptyColor, startDegree, toDegree,
                0, s_ClearColor32, 0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 center, float insideRadius, float outsideRadius,
            Color32 color, Color32 emptyColor, float startDegree,
            float toDegree, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, center, insideRadius, outsideRadius, color, color, emptyColor, startDegree, toDegree,
                borderWidth, borderColor, 0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 center, float insideRadius, float outsideRadius,
            Color32 color, Color32 toColor, Color32 emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, center, insideRadius, outsideRadius, color, toColor, emptyColor, 0, 360, 0,
                s_ClearColor32, 0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 center, float insideRadius, float outsideRadius,
            Color32 color, Color32 toColor, Color32 emptyColor, float startDegree, float toDegree, float borderWidth,
            Color32 borderColor, float space, float smoothness, bool roundCap = false, bool clockwise = true)
        {
            if (toDegree - startDegree == 0) return;
            if (space > 0 && Mathf.Abs(toDegree - startDegree) >= 360) space = 0;
            if (insideRadius <= 0)
            {
                DrawSector(vh, center, outsideRadius, color, toColor, startDegree, toDegree, borderWidth, borderColor,
                    space, smoothness);
                return;
            }
            outsideRadius -= borderWidth;
            insideRadius += borderWidth;
            smoothness = smoothness < 0 ? 2f : smoothness;
            Vector3 p1, p2, p3, p4, e1, e2;
            var needBorder = borderWidth != 0;
            var needSpace = space != 0;
            var diffAngle = Mathf.Abs(toDegree - startDegree) * Mathf.Deg2Rad;

            int segments = (int)((2 * Mathf.PI * outsideRadius) * (diffAngle * Mathf.Rad2Deg / 360) / smoothness);
            if (segments < 1) segments = 1;
            float startAngle = startDegree * Mathf.Deg2Rad;
            float toAngle = toDegree * Mathf.Deg2Rad;

            float realStartOutAngle = startAngle;
            float realToOutAngle = toAngle;
            float realStartInAngle = startAngle;
            float realToInAngle = toAngle;
            float halfAngle = (toAngle - startAngle) / 2;
            float borderAngle = 0, borderInAngle = 0, borderHalfAngle = 0;
            float spaceAngle = 0, spaceInAngle = 0, spaceHalfAngle = 0;

            var spaceCenter = center;
            var realCenter = center;
            var startDire = new Vector3(Mathf.Sin(startAngle), Mathf.Cos(startAngle)).normalized;
            var toDire = new Vector3(Mathf.Sin(toAngle), Mathf.Cos(toAngle)).normalized;
            var middleDire = new Vector3(Mathf.Sin(startAngle + halfAngle), Mathf.Cos(startAngle + halfAngle)).normalized;
            p1 = center + insideRadius * startDire;
            p2 = center + outsideRadius * startDire;
            e1 = center + insideRadius * toDire;
            e2 = center + outsideRadius * toDire;
            if (roundCap)
            {
                var roundRadius = (outsideRadius - insideRadius) / 2;
                var roundAngleRadius = insideRadius + roundRadius;
                var roundAngle = Mathf.Atan(roundRadius / roundAngleRadius);
                if (diffAngle < 2 * roundAngle)
                {
                    roundCap = false;
                }
            }
            if (needBorder || needSpace)
            {
                if (needSpace)
                {
                    var spaceDiff = space / Mathf.Sin(halfAngle);
                    spaceCenter = center + Mathf.Abs(spaceDiff) * middleDire;
                    realCenter = spaceCenter;
                    spaceAngle = 2 * Mathf.Asin(space / (2 * outsideRadius));
                    spaceInAngle = 2 * Mathf.Asin(space / (2 * insideRadius));
                    spaceHalfAngle = 2 * Mathf.Asin(space / (2 * (insideRadius + (outsideRadius - insideRadius) / 2)));
                    if (clockwise)
                    {
                        p1 = UGLHelper.GetPos(center, insideRadius, startAngle + spaceInAngle, false);
                        e1 = UGLHelper.GetPos(center, insideRadius, toAngle - spaceInAngle, false);
                        realStartOutAngle = startAngle + spaceAngle;
                        realToOutAngle = toAngle - spaceAngle;
                        realStartInAngle = startAngle + spaceInAngle;
                        realToInAngle = toAngle - spaceInAngle;
                    }
                    else
                    {
                        p1 = UGLHelper.GetPos(center, insideRadius, startAngle - spaceInAngle, false);
                        e1 = UGLHelper.GetPos(center, insideRadius, toAngle + spaceInAngle, false);
                        realStartOutAngle = startAngle - spaceAngle;
                        realToOutAngle = toAngle + spaceAngle;
                        realStartInAngle = startAngle - spaceInAngle;
                        realToOutAngle = toAngle + spaceInAngle;
                    }
                    p2 = UGLHelper.GetPos(center, outsideRadius, realStartOutAngle, false);
                    e2 = UGLHelper.GetPos(center, outsideRadius, realToOutAngle, false);
                }
                if (needBorder)
                {
                    var borderDiff = borderWidth / Mathf.Sin(halfAngle);
                    realCenter += Mathf.Abs(borderDiff) * middleDire;
                    borderAngle = 2 * Mathf.Asin(borderWidth / (2 * outsideRadius));
                    borderInAngle = 2 * Mathf.Asin(borderWidth / (2 * insideRadius));
                    borderHalfAngle = 2 * Mathf.Asin(borderWidth / (2 * (insideRadius + (outsideRadius - insideRadius) / 2)));
                    if (clockwise)
                    {
                        realStartOutAngle = realStartOutAngle + borderAngle;
                        realToOutAngle = realToOutAngle - borderAngle;
                        realStartInAngle = startAngle + spaceInAngle + borderInAngle;
                        realToInAngle = toAngle - spaceInAngle - borderInAngle;
                        var newp1 = UGLHelper.GetPos(center, insideRadius, startAngle + spaceInAngle + borderInAngle, false);
                        var newp2 = UGLHelper.GetPos(center, outsideRadius, realStartOutAngle, false);
                        if (!roundCap) DrawQuadrilateral(vh, newp2, newp1, p1, p2, borderColor);
                        p1 = newp1;
                        p2 = newp2;
                        if (toAngle - spaceInAngle - 2 * borderInAngle > realStartOutAngle)
                        {
                            var newe1 = UGLHelper.GetPos(center, insideRadius, toAngle - spaceInAngle - borderInAngle, false);
                            var newe2 = UGLHelper.GetPos(center, outsideRadius, realToOutAngle, false);
                            if (!roundCap) DrawQuadrilateral(vh, newe2, e2, e1, newe1, borderColor);
                            e1 = newe1;
                            e2 = newe2;
                        }
                    }
                    else
                    {
                        realStartOutAngle = realStartOutAngle - borderAngle;
                        realToOutAngle = realToOutAngle + borderAngle;
                        realStartInAngle = startAngle - spaceInAngle - borderInAngle;
                        realToInAngle = toAngle + spaceInAngle + borderInAngle;
                        var newp1 = UGLHelper.GetPos(center, insideRadius, startAngle - spaceInAngle - borderInAngle, false);
                        var newp2 = UGLHelper.GetPos(center, outsideRadius, realStartOutAngle, false);
                        if (!roundCap) DrawQuadrilateral(vh, newp2, newp1, p1, p2, borderColor);
                        p1 = newp1;
                        p2 = newp2;
                        if (toAngle + spaceInAngle + 2 * borderInAngle < realStartOutAngle)
                        {
                            var newe1 = UGLHelper.GetPos(center, insideRadius, toAngle + spaceInAngle + borderInAngle, false);
                            var newe2 = UGLHelper.GetPos(center, outsideRadius, realToOutAngle, false);
                            if (!roundCap) DrawQuadrilateral(vh, newe2, e2, e1, newe1, borderColor);
                            e1 = newe1;
                            e2 = newe2;
                        }
                    }
                }
            }
            if (roundCap)
            {
                var roundRadius = (outsideRadius - insideRadius) / 2;
                var roundAngleRadius = insideRadius + roundRadius;
                var roundAngle = Mathf.Atan(roundRadius / roundAngleRadius);
                if (clockwise)
                {
                    realStartOutAngle = startAngle + 2 * spaceHalfAngle + borderHalfAngle + roundAngle;
                    realStartInAngle = startAngle + 2 * spaceHalfAngle + borderHalfAngle + roundAngle;
                }
                else
                {
                    realStartOutAngle = startAngle - 2 * spaceHalfAngle - borderHalfAngle - roundAngle;
                    realStartInAngle = startAngle - 2 * spaceHalfAngle - borderHalfAngle - roundAngle;
                }
                var roundTotalDegree = realStartOutAngle * Mathf.Rad2Deg;
                var roundCenter = center + roundAngleRadius * UGLHelper.GetDire(realStartOutAngle);
                var sectorStartDegree = clockwise ? roundTotalDegree + 180 : roundTotalDegree;
                var sectorToDegree = clockwise ? roundTotalDegree + 360 : roundTotalDegree + 180;
                DrawSector(vh, roundCenter, roundRadius, color, sectorStartDegree, sectorToDegree, smoothness / 2);
                if (needBorder)
                {
                    DrawDoughnut(vh, roundCenter, roundRadius, roundRadius + borderWidth, borderColor,
                        s_ClearColor32, sectorStartDegree, sectorToDegree, smoothness / 2);
                }
                p1 = UGLHelper.GetPos(center, insideRadius, realStartOutAngle);
                p2 = UGLHelper.GetPos(center, outsideRadius, realStartOutAngle);

                if (clockwise)
                {
                    realToOutAngle = toAngle - 2 * spaceHalfAngle - borderHalfAngle - roundAngle;
                    realToInAngle = toAngle - 2 * spaceHalfAngle - borderHalfAngle - roundAngle;
                    if (realToOutAngle < realStartOutAngle) realToOutAngle = realStartOutAngle;
                }
                else
                {
                    realToOutAngle = toAngle + 2 * spaceHalfAngle + borderHalfAngle + roundAngle;
                    realToInAngle = toAngle + 2 * spaceHalfAngle + borderHalfAngle + roundAngle;
                    if (realToOutAngle > realStartOutAngle) realToOutAngle = realStartOutAngle;
                }
                roundTotalDegree = realToOutAngle * Mathf.Rad2Deg;
                roundCenter = center + roundAngleRadius * UGLHelper.GetDire(realToOutAngle);
                sectorStartDegree = clockwise ? roundTotalDegree : roundTotalDegree + 180;
                sectorToDegree = clockwise ? roundTotalDegree + 180 : roundTotalDegree + 360;
                DrawSector(vh, roundCenter, roundRadius, toColor, sectorStartDegree, sectorToDegree, smoothness / 2);
                if (needBorder)
                {
                    DrawDoughnut(vh, roundCenter, roundRadius, roundRadius + borderWidth, borderColor,
                        s_ClearColor32, sectorStartDegree, sectorToDegree, smoothness / 2);
                }
                e1 = UGLHelper.GetPos(center, insideRadius, realToOutAngle);
                e2 = UGLHelper.GetPos(center, outsideRadius, realToOutAngle);
            }
            var segmentAngle = (realToInAngle - realStartInAngle) / segments;
            var isGradient = !UGLHelper.IsValueEqualsColor(color, toColor);
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = realStartInAngle + i * segmentAngle;
                p3 = new Vector3(center.x + outsideRadius * Mathf.Sin(currAngle),
                    center.y + outsideRadius * Mathf.Cos(currAngle));
                p4 = new Vector3(center.x + insideRadius * Mathf.Sin(currAngle),
                    center.y + insideRadius * Mathf.Cos(currAngle));
                if (!UGLHelper.IsClearColor(emptyColor)) DrawTriangle(vh, center, p1, p4, emptyColor);
                if (isGradient)
                {
                    var tcolor = Color32.Lerp(color, toColor, i * 1.0f / segments);
                    DrawQuadrilateral(vh, p2, p3, p4, p1, tcolor, tcolor);
                }
                else
                {
                    DrawQuadrilateral(vh, p2, p3, p4, p1, color, color);
                }
                p1 = p4;
                p2 = p3;
            }
            if (needBorder || needSpace || roundCap)
            {
                if (clockwise)
                {
                    var isInAngleFixed = toAngle - spaceInAngle - 2 * borderInAngle > realStartOutAngle;
                    if (isInAngleFixed) DrawQuadrilateral(vh, p2, e2, e1, p1, color, toColor);
                    else DrawTriangle(vh, p2, e2, p1, color, color, toColor);
                    if (needBorder)
                    {
                        var realStartDegree = (realStartOutAngle - (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        var realToDegree = (realToOutAngle + (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        if (realToDegree < realStartOutAngle) realToDegree = realStartOutAngle;
                        var inStartDegree = roundCap ? realStartDegree : (startAngle + spaceInAngle) * Mathf.Rad2Deg;
                        var inToDegree = roundCap ? realToDegree : (toAngle - spaceInAngle) * Mathf.Rad2Deg;
                        if (inToDegree < inStartDegree) inToDegree = inStartDegree;
                        if (isInAngleFixed) DrawDoughnut(vh, center, insideRadius - borderWidth, insideRadius, borderColor,
                            s_ClearColor32, inStartDegree, inToDegree, smoothness);
                        DrawDoughnut(vh, center, outsideRadius, outsideRadius + borderWidth, borderColor, s_ClearColor32,
                            realStartDegree, realToDegree, smoothness);
                    }
                }
                else
                {
                    var isInAngleFixed = toAngle + spaceInAngle + 2 * borderInAngle < realStartOutAngle;
                    if (isInAngleFixed) DrawQuadrilateral(vh, p2, e2, e1, p1, color, toColor);
                    else DrawTriangle(vh, p2, e2, p1, color, color, toColor);
                    if (needBorder)
                    {
                        var realStartDegree = (realStartOutAngle + (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        var realToDegree = (realToOutAngle - (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        var inStartDegree = roundCap ? realStartDegree : (startAngle - spaceInAngle) * Mathf.Rad2Deg;
                        var inToDegree = roundCap ? realToDegree : (toAngle + spaceInAngle) * Mathf.Rad2Deg;
                        if (inToDegree > inStartDegree) inToDegree = inStartDegree;
                        if (isInAngleFixed)
                        {
                            DrawDoughnut(vh, center, insideRadius - borderWidth, insideRadius, borderColor,
                                s_ClearColor32, inStartDegree, inToDegree, smoothness);
                        }
                        DrawDoughnut(vh, center, outsideRadius, outsideRadius + borderWidth, borderColor,
                            s_ClearColor32, realStartDegree, realToDegree, smoothness);
                    }
                }
            }
        }

        /// <summary>
        /// 画贝塞尔曲线
        /// </summary>
        /// <param name="vh"></param>
        /// <param name="sp">起始点</param>
        /// <param name="ep">结束点</param>
        /// <param name="cp1">控制点1</param>
        /// <param name="cp2">控制点2</param>
        /// <param name="lineWidth">曲线宽</param>
        /// <param name="lineColor">曲线颜色</param>
        public static void DrawCurves(VertexHelper vh, Vector3 sp, Vector3 ep, Vector3 cp1, Vector3 cp2,
            float lineWidth, Color32 lineColor, float smoothness)
        {
            var dist = Vector3.Distance(sp, ep);
            var segment = (int)(dist / (smoothness <= 0 ? 2f : smoothness));
            UGLHelper.GetBezierList2(ref s_CurvesPosList, sp, ep, segment, cp1, cp2);
            if (s_CurvesPosList.Count > 1)
            {
                var start = s_CurvesPosList[0];
                var to = Vector3.zero;
                var dir = s_CurvesPosList[1] - start;
                var diff = Vector3.Cross(dir, Vector3.forward).normalized * lineWidth;
                var startUp = start - diff;
                var startDn = start + diff;
                for (int i = 1; i < s_CurvesPosList.Count; i++)
                {
                    to = s_CurvesPosList[i];
                    diff = Vector3.Cross(to - start, Vector3.forward).normalized * lineWidth;
                    var toUp = to - diff;
                    var toDn = to + diff;
                    DrawQuadrilateral(vh, startUp, toUp, toDn, startDn, lineColor);
                    startUp = toUp;
                    startDn = toDn;
                    start = to;
                }
            }
        }
    }
}
