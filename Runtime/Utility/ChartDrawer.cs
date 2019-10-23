/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public static class ChartDrawer
    {
        private static UIVertex[] vertex = new UIVertex[4];
        private static List<Vector3> s_CurvesPosList = new List<Vector3>();

        public static void DrawArrow(VertexHelper vh, Vector3 startPos, Vector3 arrowPos, float width,
           float height, float offset, float dent, Color32 color)
        {
            var dir = (arrowPos - startPos).normalized;

            var sharpPos = arrowPos + (offset + height / 2) * dir;
            var middle = sharpPos + (dent - height) * dir;
            var diff = Vector3.Cross(dir, Vector3.forward).normalized * width / 2;
            var left = sharpPos - height * dir + diff;
            var right = sharpPos - height * dir - diff;
            DrawTriangle(vh, middle, sharpPos, left, color);
            DrawTriangle(vh, middle, sharpPos, right, color);
        }

        public static void DrawLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color)
        {
            if (p1 == p2) return;
            Vector3 v = Vector3.Cross(p2 - p1, Vector3.forward).normalized * size;
            vertex[0].position = p1 - v;
            vertex[1].position = p2 - v;
            vertex[2].position = p2 + v;
            vertex[3].position = p1 + v;

            for (int j = 0; j < 4; j++)
            {
                vertex[j].color = color;
                vertex[j].uv0 = Vector2.zero;
            }
            vh.AddUIVertexQuad(vertex);
        }

        public static void DrawDashLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color,
         float dashLen = 15f, float blankLen = 7f, List<Vector3> posList = null)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dashLen + blankLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankLen;
                DrawLine(vh, sp, dashep, size, color);
                sp = np;
            }
            if (posList != null) posList.Add(p2);
            DrawLine(vh, sp, p2, size, color);
        }
        public static void DrawDotLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color,
            float dotLen = 5f, float blankLen = 5f, List<Vector3> posList = null)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dotLen + blankLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankLen;
                DrawLine(vh, sp, dashep, size, color);
                sp = np;
            }
            if (posList != null) posList.Add(p2);
            DrawLine(vh, sp, p2, size, color);
        }

        public static void DrawDashDotLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color,
            float dashLen = 15f, float blankDotLen = 15f, List<Vector3> posList = null)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dashLen + blankDotLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankDotLen;
                DrawLine(vh, sp, dashep, size, color);
                if (posList != null) posList.Add(dashep);
                var dotsp = dashep + (blankDotLen - 2 * size) / 2 * dir;
                var dotep = dotsp + 2 * size * dir;
                DrawLine(vh, dotsp, dotep, size, color);
                if (posList != null) posList.Add(dotsp);
                sp = np;
            }
            if (posList != null) posList.Add(p2);
            DrawLine(vh, sp, p2, size, color);
        }

        public static void DrawDashDotDotLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size,
            Color32 color, float dashLen = 15f, float blankDotLen = 20f, List<Vector3> posList = null)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dashLen + blankDotLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            if (posList != null) posList.Clear();
            for (int i = 1; i <= segment; i++)
            {
                if (posList != null) posList.Add(sp);
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankDotLen;
                DrawLine(vh, sp, dashep, size, color);
                if (posList != null) posList.Add(dashep);
                var dotsp = dashep + (blankDotLen / 2 - 2 * size) / 2 * dir;
                var dotep = dotsp + 2 * size * dir;
                DrawLine(vh, dotsp, dotep, size, color);
                if (posList != null) posList.Add(dotep);
                var dotsp2 = dashep + blankDotLen / 2 * dir;
                dotsp2 = dotsp2 + (blankDotLen / 4 - 2 * size) / 2 * dir;
                var dotep2 = dotsp2 + 2 * size * dir;
                DrawLine(vh, dotsp2, dotep2, size, color);
                if (posList != null) posList.Add(dotep2);
                sp = np;
            }
            if (posList != null) posList.Add(p2);
            DrawLine(vh, sp, p2, size, color);
        }

        public static void DrawZebraLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size,
            float zebraWidth, float zebraGap, Color32 color)
        {
            DrawDotLine(vh, p1, p2, size, color, zebraWidth, zebraGap);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p, float radius, Color32 color,
            bool vertical = true)
        {
            Vector3 p1, p2, p3, p4;
            if (vertical)
            {
                p1 = new Vector3(p.x + radius, p.y - radius);
                p2 = new Vector3(p.x - radius, p.y - radius);
                p3 = new Vector3(p.x - radius, p.y + radius);
                p4 = new Vector3(p.x + radius, p.y + radius);
            }
            else
            {
                p1 = new Vector3(p.x - radius, p.y - radius);
                p2 = new Vector3(p.x - radius, p.y + radius);
                p3 = new Vector3(p.x + radius, p.y + radius);
                p4 = new Vector3(p.x + radius, p.y - radius);
            }
            DrawPolygon(vh, p1, p2, p3, p4, color, color);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, float radius, Color32 color)
        {
            DrawPolygon(vh, p1, p2, radius, color, color);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, float radius, Color32 color, Color32 toColor)
        {
            var dir = (p2 - p1).normalized;
            var dirv = Vector3.Cross(dir, Vector3.forward).normalized;

            var p3 = p1 + dirv * radius;
            var p4 = p1 - dirv * radius;
            var p5 = p2 - dirv * radius;
            var p6 = p2 + dirv * radius;
            DrawPolygon(vh, p3, p4, p5, p6, color, toColor);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p, float xRadius, float yRadius,
                Color32 color, bool vertical = true)
        {
            DrawPolygon(vh, p, xRadius, yRadius, color, color, vertical);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p, float xRadius, float yRadius,
            Color32 color, Color toColor, bool vertical = true)
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

            DrawPolygon(vh, p1, p2, p3, p4, color, toColor);
        }



        public static void DrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 color)
        {
            DrawPolygon(vh, p1, p2, p3, p4, color, color);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color32 startColor, Color32 toColor)
        {
            vertex[0].position = p1;
            vertex[1].position = p2;
            vertex[2].position = p3;
            vertex[3].position = p4;
            for (int j = 0; j < 4; j++)
            {
                vertex[j].color = j >= 2 ? toColor : startColor;
                vertex[j].uv0 = Vector2.zero;
            }
            vh.AddUIVertexQuad(vertex);
        }

        public static void DrawBorder(VertexHelper vh, Vector3 p, float rectWidth, float rectHeight,
            float borderWidth, Color32 color)
        {
            var halfWid = rectWidth / 2;
            var halfHig = rectHeight / 2;
            var p1In = new Vector3(p.x - halfWid, p.y - halfHig);
            var p1Ot = new Vector3(p.x - halfWid - borderWidth, p.y - halfHig - borderWidth);
            var p2In = new Vector3(p.x - halfWid, p.y + halfHig);
            var p2Ot = new Vector3(p.x - halfWid - borderWidth, p.y + halfHig + borderWidth);
            var p3In = new Vector3(p.x + halfWid, p.y + halfHig);
            var p3Ot = new Vector3(p.x + halfWid + borderWidth, p.y + halfHig + borderWidth);
            var p4In = new Vector3(p.x + halfWid, p.y - halfHig);
            var p4Ot = new Vector3(p.x + halfWid + borderWidth, p.y - halfHig - borderWidth);
            DrawPolygon(vh, p1In, p1Ot, p2Ot, p2In, color);
            DrawPolygon(vh, p2In, p2Ot, p3Ot, p3In, color);
            DrawPolygon(vh, p3In, p3Ot, p4Ot, p4In, color);
            DrawPolygon(vh, p4In, p4Ot, p1Ot, p1In, color);
        }

        public static void DrawTriangle(VertexHelper vh, Vector3 p1,
            Vector3 p2, Vector3 p3, Color32 color)
        {
            DrawTriangle(vh, p1, p2, p3, color, color, color);
        }

        public static void DrawTriangle(VertexHelper vh, Vector3 p1,
           Vector3 p2, Vector3 p3, Color32 color, Color32 color2, Color32 color3)
        {
            UIVertex v1 = new UIVertex();
            v1.position = p1;
            v1.color = color;
            v1.uv0 = Vector3.zero;
            UIVertex v2 = new UIVertex();
            v2.position = p2;
            v2.color = color2;
            v2.uv0 = Vector3.zero;
            UIVertex v3 = new UIVertex();
            v3.position = p3;
            v3.color = color3;
            v3.uv0 = Vector3.zero;
            int startIndex = vh.currentVertCount;
            vh.AddVert(v1);
            vh.AddVert(v2);
            vh.AddVert(v3);
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, 0, 360, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, Color emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, radius - tickness, radius, color, emptyColor, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float startDegree, float toDegree, float smoothness = 2f)
        {
            int segments = (int)((2 * Mathf.PI * radius) / (smoothness < 0 ? 2f : smoothness));
            Vector3 p2, p3;
            float startAngle = startDegree * Mathf.Deg2Rad;
            float angle = (toDegree - startDegree) * Mathf.Deg2Rad / segments;
            p2 = new Vector3(p.x + radius * Mathf.Sin(startAngle), p.y + radius * Mathf.Cos(startAngle));
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = startAngle + i * angle;
                p3 = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                    p.y + radius * Mathf.Cos(currAngle));
                DrawTriangle(vh, p, p2, p3, color);
                p2 = p3;
            }
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color emptyColor, float smoothness = 2f, float startDegree = 0, float toDegree = 360)
        {
            if (insideRadius <= 0)
            {
                DrawSector(vh, p, outsideRadius, color, startDegree, toDegree, smoothness);
                return;
            }
            Vector3 p1, p2, p3, p4;
            int segments = (int)((2 * Mathf.PI * outsideRadius) / (smoothness < 0 ? 2f : smoothness));
            float startAngle = startDegree * Mathf.Deg2Rad;
            float angle = (toDegree - startDegree) * Mathf.Deg2Rad / segments;
            p1 = new Vector3(p.x + insideRadius * Mathf.Sin(startAngle),
                p.y + insideRadius * Mathf.Cos(startAngle));
            p2 = new Vector3(p.x + outsideRadius * Mathf.Sin(startAngle),
                p.y + outsideRadius * Mathf.Cos(startAngle));
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = startAngle + i * angle;
                p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                    p.y + outsideRadius * Mathf.Cos(currAngle));
                p4 = new Vector3(p.x + insideRadius * Mathf.Sin(currAngle),
                    p.y + insideRadius * Mathf.Cos(currAngle));
                if (emptyColor != Color.clear) DrawTriangle(vh, p, p1, p4, emptyColor);
                DrawPolygon(vh, p1, p2, p3, p4, color);
                p1 = p4;
                p2 = p3;
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
            float lineWidth, Color lineColor, float smoothness)
        {
            var dist = Vector3.Distance(sp, ep);
            var segment = (int)(dist / (smoothness <= 0 ? 2f : smoothness));
            ChartHelper.GetBezierList2(ref s_CurvesPosList, sp, ep, segment, cp1, cp2);
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
                    DrawPolygon(vh, startUp, toUp, toDn, startDn, lineColor);
                    startUp = toUp;
                    startDn = toDn;
                    start = to;
                }
            }
        }
    }
}