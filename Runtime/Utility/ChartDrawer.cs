using System.Linq;
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

        public static void DrawDiamond(VertexHelper vh, Vector3 pos, float size, Color32 color)
        {
            DrawDiamond(vh, pos, size, color, color);
        }

        public static void DrawDiamond(VertexHelper vh, Vector3 pos, float size, Color32 color, Color32 toColor)
        {
            var p1 = new Vector2(pos.x - size, pos.y);
            var p2 = new Vector2(pos.x, pos.y + size);
            var p3 = new Vector2(pos.x + size, pos.y);
            var p4 = new Vector2(pos.x, pos.y - size);
            DrawTriangle(vh, p4, p1, p2, color, color, toColor);
            DrawTriangle(vh, p3, p4, p2, color, color, toColor);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p, float radius, Color32 color,
            bool vertical = true)
        {
            DrawPolygon(vh, p, radius, color, color, vertical);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p, float radius, Color32 color, Color32 toColor,
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
            DrawPolygon(vh, p1, p2, p3, p4, color, toColor);
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

        private static void InitCornerRadius(float[] cornerRadius, float width, float height, ref float brLt,
        ref float brRt, ref float brRb, ref float brLb, ref bool needRound)
        {
            brLt = cornerRadius != null && cornerRadius.Length > 0 ? cornerRadius[0] : 0;
            brRt = cornerRadius != null && cornerRadius.Length > 1 ? cornerRadius[1] : 0;
            brRb = cornerRadius != null && cornerRadius.Length > 2 ? cornerRadius[2] : 0;
            brLb = cornerRadius != null && cornerRadius.Length > 3 ? cornerRadius[3] : 0;
            needRound = brLb != 0 || brRt != 0 || brRb != 0 || brLb != 0;
            if (needRound)
            {
                if (brLt + brRt > width)
                {
                    var total = brLt + brRt;
                    brLt = width * (brLt / total);
                    brRt = width * (brRt / total);
                }
                if (brRt + brRb > height)
                {
                    var total = brRt + brRb;
                    brRt = height * (brRt / total);
                    brRb = height * (brRb / total);
                }
                if (brRb + brLb > width)
                {
                    var total = brRb + brLb;
                    brRb = width * (brRb / total);
                    brLb = width * (brLb / total);
                }
                if (brLb + brLt > height)
                {
                    var total = brLb + brLt;
                    brLb = height * (brLb / total);
                    brLt = height * (brLt / total);
                }
                if (brLt + brRb > height)
                {
                    var total = brLt + brRb;
                    brLt = height * (brLt / total);
                    brRb = height * (brRb / total);
                }
                if (brRt + brLb > height)
                {
                    var total = brRt + brRt;
                    brRt = height * (brRt / total);
                    brLb = height * (brLb / total);
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
        /// <param name="rotate"></param>
        /// <param name="cornerRadius"></param>
        public static void DrawRoundRectangle(VertexHelper vh, Vector3 center, float rectWidth, float rectHeight,
            Color32 color, float rotate = 0, float[] cornerRadius = null)
        {
            var halfWid = rectWidth / 2;
            var halfHig = rectHeight / 2;
            float brLt = 0, brRt = 0, brRb = 0, brLb = 0;
            bool needRound = false;
            InitCornerRadius(cornerRadius, rectWidth, rectHeight, ref brLt, ref brRt, ref brRb, ref brLb, ref needRound);
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
                    DrawSector(vh, roundLt, brLt, color, color, 270, 360);
                    ltIn = roundLt + brLt * Vector3.left;
                    ltIn2 = roundLt + brLt * Vector3.up;
                }
                if (brRt > 0)
                {
                    roundRt = new Vector3(center.x + halfWid - brRt, center.y + halfHig - brRt);
                    DrawSector(vh, roundRt, brRt, color, color, 0, 90);
                    rtIn = roundRt + brRt * Vector3.up;
                    rtIn2 = roundRt + brRt * Vector3.right;
                }
                if (brRb > 0)
                {
                    roundRb = new Vector3(center.x + halfWid - brRb, center.y - halfHig + brRb);
                    DrawSector(vh, roundRb, brRb, color, color, 90, 180);
                    rbIn = roundRb + brRb * Vector3.right;
                    rbIn2 = roundRb + brRb * Vector3.down;
                }
                if (brLb > 0)
                {
                    roundLb = new Vector3(center.x - halfWid + brLb, center.y - halfHig + brLb);
                    DrawSector(vh, roundLb, brLb, color, color, 180, 270);
                    lbIn = roundLb + brLb * Vector3.left;
                    lbIn2 = roundLb + brLb * Vector3.down;
                }
                var maxup = Mathf.Max(brLt, brRt);
                DrawPolygon(vh, ltIn2, rtIn, rtIn + maxup * Vector3.down, ltIn2 + maxup * Vector3.down, color);
                DrawPolygon(vh, ltIn, roundLt, roundLt + (maxup - brLt) * Vector3.down, ltIn + (maxup - brLt) * Vector3.down, color);
                DrawPolygon(vh, roundRt, rtIn2, rtIn2 + (maxup - brRt) * Vector3.down, roundRt + (maxup - brRt) * Vector3.down, color);
                var maxdown = Mathf.Max(brLb, brRb);
                DrawPolygon(vh, lbIn2, lbIn2 + maxdown * Vector3.up, rbIn2 + maxdown * Vector3.up, rbIn2, color);
                DrawPolygon(vh, lbIn, lbIn + (maxdown - brLb) * Vector3.up, roundLb + (maxdown - brLb) * Vector3.up, roundLb, color);
                DrawPolygon(vh, roundRb, roundRb + (maxdown - brRb) * Vector3.up, rbIn2 + (maxdown - brRb) * Vector3.up, rbIn2, color);
                var clt = new Vector3(center.x - halfWid, center.y + halfHig - maxup);
                var crt = new Vector3(center.x + halfWid, center.y + halfHig - maxup);
                var crb = new Vector3(center.x + halfWid, center.y - halfHig + maxdown);
                var clb = new Vector3(center.x - halfWid, center.y - halfHig + maxdown);
                if (clt.y > clb.y)
                    DrawPolygon(vh, clt, crt, crb, clb, color);
            }
            else
            {
                DrawPolygon(vh, lbIn, ltIn, rtIn, rbIn, color);
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
        public static void DrawBorder(VertexHelper vh, Vector3 center, float rectWidth, float rectHeight,
            float borderWidth, Color32 color, float rotate = 0, float[] cornerRadius = null)
        {
            if (borderWidth == 0 || color == Color.clear) return;
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
            InitCornerRadius(cornerRadius, rectWidth, rectHeight, ref brLt, ref brRt, ref brRb, ref brLb, ref needRound);
            var tempCenter = Vector3.zero;
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
                    DrawDoughnut(vh, tempCenter, brLt, brLt + borderWidth, color, Color.clear, 2, 270, 360);
                    ltIn = tempCenter + brLt * Vector3.left;
                    ltOt = tempCenter + (brLt + borderWidth) * Vector3.left;
                    ltIn2 = tempCenter + brLt * Vector3.up;
                    ltOt2 = tempCenter + (brLt + borderWidth) * Vector3.up;
                }
                if (brRt > 0)
                {
                    tempCenter = new Vector3(center.x + halfWid - brRt, center.y + halfHig - brRt);
                    DrawDoughnut(vh, tempCenter, brRt, brRt + borderWidth, color, Color.clear, 2, 0, 90);
                    rtIn = tempCenter + brRt * Vector3.up;
                    rtOt = tempCenter + (brRt + borderWidth) * Vector3.up;
                    rtIn2 = tempCenter + brRt * Vector3.right;
                    rtOt2 = tempCenter + (brRt + borderWidth) * Vector3.right;
                }
                if (brRb > 0)
                {
                    tempCenter = new Vector3(center.x + halfWid - brRb, center.y - halfHig + brRb);
                    DrawDoughnut(vh, tempCenter, brRb, brRb + borderWidth, color, Color.clear, 2, 90, 180);
                    rbIn = tempCenter + brRb * Vector3.right;
                    rbOt = tempCenter + (brRb + borderWidth) * Vector3.right;
                    rbIn2 = tempCenter + brRb * Vector3.down;
                    rbOt2 = tempCenter + (brRb + borderWidth) * Vector3.down;
                }
                if (brLb > 0)
                {
                    tempCenter = new Vector3(center.x - halfWid + brLb, center.y - halfHig + brLb);
                    DrawDoughnut(vh, tempCenter, brLb, brLb + borderWidth, color, Color.clear, 2, 180, 270);
                    lbIn = tempCenter + brLb * Vector3.left;
                    lbOt = tempCenter + (brLb + borderWidth) * Vector3.left;
                    lbIn2 = tempCenter + brLb * Vector3.down;
                    lbOt2 = tempCenter + (brLb + borderWidth) * Vector3.down;
                }
                DrawPolygon(vh, lbIn, lbOt, ltOt, ltIn, color);
                DrawPolygon(vh, ltIn2, ltOt2, rtOt, rtIn, color);
                DrawPolygon(vh, rtIn2, rtOt2, rbOt, rbIn, color);
                DrawPolygon(vh, rbIn2, rbOt2, lbOt2, lbIn2, color);
            }
            else
            {
                if (rotate > 0)
                {
                    lbIn = ChartHelper.RotateRound(lbIn, center, Vector3.forward, rotate);
                    lbOt = ChartHelper.RotateRound(lbOt, center, Vector3.forward, rotate);
                    ltIn = ChartHelper.RotateRound(ltIn, center, Vector3.forward, rotate);
                    ltOt = ChartHelper.RotateRound(ltOt, center, Vector3.forward, rotate);
                    rtIn = ChartHelper.RotateRound(rtIn, center, Vector3.forward, rotate);
                    rtOt = ChartHelper.RotateRound(rtOt, center, Vector3.forward, rotate);
                    rbIn = ChartHelper.RotateRound(rbIn, center, Vector3.forward, rotate);
                    rbOt = ChartHelper.RotateRound(rbOt, center, Vector3.forward, rotate);
                }
                DrawPolygon(vh, lbIn, lbOt, ltOt, ltIn, color);
                DrawPolygon(vh, ltIn, ltOt, rtOt, rtIn, color);
                DrawPolygon(vh, rtIn, rtOt, rbOt, rbIn, color);
                DrawPolygon(vh, rbIn, rbOt, lbOt, lbIn, color);
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
            ChartDrawer.DrawTriangle(vh, p1, p2, p3, color, toColor, color);
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
            Color32 toColor, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, toColor, 0, 360, smoothness);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float smoothness = 2f)
        {
            DrawCricle(vh, p, radius, color, color, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, Color emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, radius - tickness, radius, color, color, emptyColor, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, Color32 toColor, Color emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, radius - tickness, radius, color, toColor, emptyColor, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float startDegree, float toDegree, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, color, startDegree, toDegree, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color,
            Color32 toColor, float startDegree, float toDegree, float smoothness = 2f)
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
                DrawTriangle(vh, p, p2, p3, toColor, color, color);
                p2 = p3;
            }
        }

        public static void DrawRoundCap(VertexHelper vh, Vector3 center, float width, float radius, float angle,
            bool clockwise, Color color, bool end)
        {
            var px = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
            var py = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            var pos = new Vector3(px, py) + center;
            if (end)
            {
                if (clockwise)
                    ChartDrawer.DrawSector(vh, pos, width, color, angle, angle + 180);
                else
                    ChartDrawer.DrawSector(vh, pos, width, color, angle, angle - 180);
            }
            else
            {
                if (clockwise)
                    ChartDrawer.DrawSector(vh, pos, width, color, angle + 180, angle + 360);
                else
                    ChartDrawer.DrawSector(vh, pos, width, color, angle - 180, angle - 360);
            }
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color emptyColor, float smoothness = 2f, float startDegree = 0, float toDegree = 360)
        {
            DrawDoughnut(vh, p, insideRadius, outsideRadius, color, color, emptyColor, smoothness,
                startDegree, toDegree);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color32 toColor, Color emptyColor, float smoothness = 2f, float startDegree = 0,
            float toDegree = 360)
        {
            if (insideRadius <= 0)
            {
                DrawSector(vh, p, outsideRadius, color, toColor, startDegree, toDegree, smoothness);
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
                //DrawPolygon(vh, p1, p2, p3, p4, color,Color.blue);
                DrawPolygon(vh, p2, p3, p4, p1, color, toColor);
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