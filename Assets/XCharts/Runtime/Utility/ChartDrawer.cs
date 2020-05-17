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
        private static readonly Vector2 zeroVector2 = Vector2.zero;
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
                vertex[j].uv0 = zeroVector2;
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
                vertex[j].uv0 = zeroVector2;
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
                    DrawSector(vh, roundLt, brLt, color, color, 270, 360, 1);
                    ltIn = roundLt + brLt * Vector3.left;
                    ltIn2 = roundLt + brLt * Vector3.up;
                }
                if (brRt > 0)
                {
                    roundRt = new Vector3(center.x + halfWid - brRt, center.y + halfHig - brRt);
                    DrawSector(vh, roundRt, brRt, color, color, 0, 90, 1);
                    rtIn = roundRt + brRt * Vector3.up;
                    rtIn2 = roundRt + brRt * Vector3.right;
                }
                if (brRb > 0)
                {
                    roundRb = new Vector3(center.x + halfWid - brRb, center.y - halfHig + brRb);
                    DrawSector(vh, roundRb, brRb, color, color, 90, 180, 1);
                    rbIn = roundRb + brRb * Vector3.right;
                    rbIn2 = roundRb + brRb * Vector3.down;
                }
                if (brLb > 0)
                {
                    roundLb = new Vector3(center.x - halfWid + brLb, center.y - halfHig + brLb);
                    DrawSector(vh, roundLb, brLb, color, color, 180, 270, 1);
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
            if (borderWidth == 0 || ChartHelper.IsClearColor(color)) return;
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
                    DrawDoughnut(vh, tempCenter, brLt, brLt + borderWidth, color, Color.clear, 270, 360);
                    ltIn = tempCenter + brLt * Vector3.left;
                    ltOt = tempCenter + (brLt + borderWidth) * Vector3.left;
                    ltIn2 = tempCenter + brLt * Vector3.up;
                    ltOt2 = tempCenter + (brLt + borderWidth) * Vector3.up;
                }
                if (brRt > 0)
                {
                    tempCenter = new Vector3(center.x + halfWid - brRt, center.y + halfHig - brRt);
                    DrawDoughnut(vh, tempCenter, brRt, brRt + borderWidth, color, Color.clear, 0, 90);
                    rtIn = tempCenter + brRt * Vector3.up;
                    rtOt = tempCenter + (brRt + borderWidth) * Vector3.up;
                    rtIn2 = tempCenter + brRt * Vector3.right;
                    rtOt2 = tempCenter + (brRt + borderWidth) * Vector3.right;
                }
                if (brRb > 0)
                {
                    tempCenter = new Vector3(center.x + halfWid - brRb, center.y - halfHig + brRb);
                    DrawDoughnut(vh, tempCenter, brRb, brRb + borderWidth, color, Color.clear, 90, 180);
                    rbIn = tempCenter + brRb * Vector3.right;
                    rbOt = tempCenter + (brRb + borderWidth) * Vector3.right;
                    rbIn2 = tempCenter + brRb * Vector3.down;
                    rbOt2 = tempCenter + (brRb + borderWidth) * Vector3.down;
                }
                if (brLb > 0)
                {
                    tempCenter = new Vector3(center.x - halfWid + brLb, center.y - halfHig + brLb);
                    DrawDoughnut(vh, tempCenter, brLb, brLb + borderWidth, color, Color.clear, 180, 270);
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
            v1.uv0 = zeroVector2;
            UIVertex v2 = new UIVertex();
            v2.position = p2;
            v2.color = color2;
            v2.uv0 = zeroVector2;
            UIVertex v3 = new UIVertex();
            v3.position = p3;
            v3.color = color3;
            v3.uv0 = zeroVector2;
            int startIndex = vh.currentVertCount;
            vh.AddVert(v1);
            vh.AddVert(v2);
            vh.AddVert(v3);
            vh.AddTriangle(startIndex, startIndex + 1, startIndex + 2);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float smoothness = 2f)
        {
            DrawCricle(vh, p, radius, color, color, 0, Color.clear, smoothness);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 p, float radius, Color32 color,
           Color32 toColor, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, toColor, 0, 360, 0, Color.clear, smoothness);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 p, float radius, Color32 color,
            Color32 toColor, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, toColor, 0, 360, borderWidth, borderColor, smoothness);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawCricle(vh, p, radius, color, color, borderWidth, borderColor, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, Color32 emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, radius - tickness, radius, color, color, emptyColor, 0, 360, 0, Color.clear, 0, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, Color32 emptyColor, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, radius - tickness, radius, color, color, emptyColor, 0, 360, borderWidth,
                borderColor, 0, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, Color32 toColor, Color32 emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, radius - tickness, radius, color, toColor, emptyColor, 0, 360, 0,
                Color.clear, 0, smoothness);
        }

        public static void DrawEmptyCricle(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, Color32 toColor, Color32 emptyColor, float borderWidth, Color32 borderColor,
            float smoothness = 2f)
        {
            DrawDoughnut(vh, p, radius - tickness, radius, color, toColor, emptyColor, 0, 360, borderWidth,
                borderColor, 0, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color,
                    float startDegree, float toDegree, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, color, startDegree, toDegree, 0, Color.clear, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color, Color32 toColor,
                    float startDegree, float toDegree, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, toColor, startDegree, toDegree, 0, Color.clear, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float startDegree, float toDegree, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, color, startDegree, toDegree, borderWidth, borderColor, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color, Color32 toColor,
            float startDegree, float toDegree, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawSector(vh, p, radius, color, toColor, startDegree, toDegree, borderWidth, borderColor, 0, smoothness);
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color, Color32 toColor,
            float startDegree, float toDegree, float borderWidth, Color32 borderColor, float space,
            float smoothness = 2f)
        {
            radius -= borderWidth;
            smoothness = (smoothness < 0 ? 2f : smoothness);
            int segments = (int)((2 * Mathf.PI * radius) * (Mathf.Abs(toDegree - startDegree) / 360) / smoothness);
            float startAngle = startDegree * Mathf.Deg2Rad;
            float toAngle = toDegree * Mathf.Deg2Rad;
            float realStartAngle = startAngle;
            float realToAngle = toAngle;
            float halfAngle = (toAngle - startAngle) / 2;
            float borderAngle = 0;
            float spaceAngle = 0;

            var p2 = p + radius * GetDire(startAngle);
            var p3 = Vector3.zero;
            var spaceCenter = p;
            var realCenter = p;
            var needBorder = borderWidth != 0;
            var needSpace = space != 0;
            var lastPos = Vector3.zero;
            var middleDire = GetDire(startAngle + halfAngle);
            if (needBorder || needSpace)
            {
                float spaceDiff = 0f;
                float borderDiff = 0f;
                if (needSpace)
                {
                    spaceDiff = space / Mathf.Sin(halfAngle);
                    spaceCenter = p + spaceDiff * middleDire;
                    realCenter = spaceCenter;
                    spaceAngle = 2 * Mathf.Asin(space / (2 * radius));
                    realStartAngle = startAngle + spaceAngle;
                    realToAngle = toAngle - spaceAngle;
                    if (realToAngle < realStartAngle) realToAngle = realStartAngle;
                    p2 = GetPos(p, radius, realStartAngle);
                }
                if (needBorder)
                {
                    borderDiff = borderWidth / Mathf.Sin(halfAngle);
                    realCenter += borderDiff * middleDire;
                    borderAngle = 2 * Mathf.Asin(borderWidth / (2 * radius));
                    realStartAngle = realStartAngle + borderAngle;
                    realToAngle = realToAngle - borderAngle;
                    if (realToAngle < realStartAngle)
                    {
                        realToAngle = realStartAngle;
                        p2 = GetPos(p, radius, realStartAngle);
                    }
                    else
                    {
                        var borderX1 = GetPos(p, radius, realStartAngle);
                        DrawPolygon(vh, realCenter, spaceCenter, p2, borderX1, borderColor);
                        p2 = borderX1;

                        var borderX2 = GetPos(p, radius, realToAngle);
                        var pEnd = GetPos(p, radius, toAngle - spaceAngle);
                        DrawPolygon(vh, realCenter, borderX2, pEnd, spaceCenter, borderColor);
                    }
                }
            }
            float segmentAngle = (realToAngle - realStartAngle) / segments;
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = realStartAngle + i * segmentAngle;
                p3 = p + radius * GetDire(currAngle);
                DrawTriangle(vh, realCenter, p2, p3, toColor, color, color);
                p2 = p3;
            }
            if (needBorder || needSpace)
            {
                if (realToAngle > realStartAngle)
                {
                    var borderX2 = p + radius * GetDire(realToAngle);
                    DrawTriangle(vh, realCenter, p2, borderX2, toColor, color, color);
                    if (needBorder)
                    {
                        var realStartDegree = (realStartAngle - borderAngle) * Mathf.Rad2Deg;
                        var realToDegree = (realToAngle + borderAngle) * Mathf.Rad2Deg;
                        DrawDoughnut(vh, p, radius, radius + borderWidth, borderColor, Color.clear, realStartDegree,
                            realToDegree, smoothness);
                    }
                }
            }
        }

        private static Vector3 GetPos(Vector3 center, float radius, float angle, bool isDegree = false)
        {
            angle = isDegree ? angle * Mathf.Deg2Rad : angle;
            return new Vector3(center.x + radius * Mathf.Sin(angle), center.y + radius * Mathf.Cos(angle));
        }

        private static Vector3 GetDire(float angle, bool isDegree = false)
        {
            angle = isDegree ? angle * Mathf.Deg2Rad : angle;
            return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
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
                    ChartDrawer.DrawSector(vh, pos, width, color, angle, angle + 180, 0, Color.clear);
                else
                    ChartDrawer.DrawSector(vh, pos, width, color, angle, angle - 180, 0, Color.clear);
            }
            else
            {
                if (clockwise)
                    ChartDrawer.DrawSector(vh, pos, width, color, angle + 180, angle + 360, 0, Color.clear);
                else
                    ChartDrawer.DrawSector(vh, pos, width, color, angle - 180, angle - 360, 0, Color.clear);
            }
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, insideRadius, outsideRadius, color, color, emptyColor, 0, 360, 0, Color.clear,
                0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color emptyColor, float startDegree,
            float toDegree, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, insideRadius, outsideRadius, color, color, emptyColor, startDegree, toDegree,
                0, Color.clear, 0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color emptyColor, float startDegree,
            float toDegree, float borderWidth, Color32 borderColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, insideRadius, outsideRadius, color, color, emptyColor, startDegree, toDegree,
                borderWidth, borderColor, 0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color32 toColor, Color emptyColor, float smoothness = 2f)
        {
            DrawDoughnut(vh, p, insideRadius, outsideRadius, color, toColor, emptyColor, 0, 360, 0, Color.clear,
                0, smoothness);
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            Color32 color, Color32 toColor, Color emptyColor, float startDegree, float toDegree, float borderWidth,
            Color32 borderColor, float space, float smoothness, bool roundCap = false, bool clockwise = true)
        {
            if (toDegree - startDegree == 0) return;
            if (insideRadius <= 0)
            {
                DrawSector(vh, p, outsideRadius, color, toColor, startDegree, toDegree, borderWidth, borderColor,
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
            float startAngle = startDegree * Mathf.Deg2Rad;
            float toAngle = toDegree * Mathf.Deg2Rad;

            float realStartOutAngle = startAngle;
            float realToOutAngle = toAngle;
            float realStartInAngle = startAngle;
            float realToInAngle = toAngle;
            float halfAngle = (toAngle - startAngle) / 2;
            float borderAngle = 0, borderInAngle = 0, borderHalfAngle = 0;
            float spaceAngle = 0, spaceInAngle = 0, spaceHalfAngle = 0;

            var spaceCenter = p;
            var realCenter = p;
            var startDire = new Vector3(Mathf.Sin(startAngle), Mathf.Cos(startAngle)).normalized;
            var toDire = new Vector3(Mathf.Sin(toAngle), Mathf.Cos(toAngle)).normalized;
            var middleDire = new Vector3(Mathf.Sin(startAngle + halfAngle), Mathf.Cos(startAngle + halfAngle)).normalized;
            p1 = p + insideRadius * startDire;
            p2 = p + outsideRadius * startDire;
            e1 = p + insideRadius * toDire;
            e2 = p + outsideRadius * toDire;
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
                    spaceCenter = p + Mathf.Abs(spaceDiff) * middleDire;
                    realCenter = spaceCenter;
                    spaceAngle = 2 * Mathf.Asin(space / (2 * outsideRadius));
                    spaceInAngle = 2 * Mathf.Asin(space / (2 * insideRadius));
                    spaceHalfAngle = 2 * Mathf.Asin(space / (2 * (insideRadius + (outsideRadius - insideRadius) / 2)));
                    if (clockwise)
                    {
                        p1 = GetPos(p, insideRadius, startAngle + spaceInAngle, false);
                        e1 = GetPos(p, insideRadius, toAngle - spaceInAngle, false);
                        realStartOutAngle = startAngle + spaceAngle;
                        realToOutAngle = toAngle - spaceAngle;
                        realStartInAngle = startAngle + spaceInAngle;
                        realToInAngle = toAngle - spaceInAngle;
                    }
                    else
                    {
                        p1 = GetPos(p, insideRadius, startAngle - spaceInAngle, false);
                        e1 = GetPos(p, insideRadius, toAngle + spaceInAngle, false);
                        realStartOutAngle = startAngle - spaceAngle;
                        realToOutAngle = toAngle + spaceAngle;
                        realStartInAngle = startAngle - spaceInAngle;
                        realToOutAngle = toAngle + spaceInAngle;
                    }
                    p2 = GetPos(p, outsideRadius, realStartOutAngle, false);
                    e2 = GetPos(p, outsideRadius, realToOutAngle, false);
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
                        var newp1 = GetPos(p, insideRadius, startAngle + spaceInAngle + borderInAngle, false);
                        var newp2 = GetPos(p, outsideRadius, realStartOutAngle, false);
                        if (!roundCap) DrawPolygon(vh, newp2, newp1, p1, p2, borderColor);
                        p1 = newp1;
                        p2 = newp2;
                        if (toAngle - spaceInAngle - 2 * borderInAngle > realStartOutAngle)
                        {
                            var newe1 = GetPos(p, insideRadius, toAngle - spaceInAngle - borderInAngle, false);
                            var newe2 = GetPos(p, outsideRadius, realToOutAngle, false);
                            if (!roundCap) DrawPolygon(vh, newe2, e2, e1, newe1, borderColor);
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
                        var newp1 = GetPos(p, insideRadius, startAngle - spaceInAngle - borderInAngle, false);
                        var newp2 = GetPos(p, outsideRadius, realStartOutAngle, false);
                        if (!roundCap) DrawPolygon(vh, newp2, newp1, p1, p2, borderColor);
                        p1 = newp1;
                        p2 = newp2;
                        if (toAngle + spaceInAngle + 2 * borderInAngle < realStartOutAngle)
                        {
                            var newe1 = GetPos(p, insideRadius, toAngle + spaceInAngle + borderInAngle, false);
                            var newe2 = GetPos(p, outsideRadius, realToOutAngle, false);
                            if (!roundCap) DrawPolygon(vh, newe2, e2, e1, newe1, borderColor);
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
                var roundCenter = p + roundAngleRadius * GetDire(realStartOutAngle);
                var sectorStartDegree = clockwise ? roundTotalDegree + 180 : roundTotalDegree;
                var sectorToDegree = clockwise ? roundTotalDegree + 360 : roundTotalDegree + 180;
                DrawSector(vh, roundCenter, roundRadius, color, sectorStartDegree, sectorToDegree, smoothness / 2);
                if (needBorder)
                {
                    DrawDoughnut(vh, roundCenter, roundRadius, roundRadius + borderWidth, borderColor, Color.clear,
                        sectorStartDegree, sectorToDegree, smoothness / 2);
                }
                p1 = GetPos(p, insideRadius, realStartOutAngle);
                p2 = GetPos(p, outsideRadius, realStartOutAngle);

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
                roundCenter = p + roundAngleRadius * GetDire(realToOutAngle);
                sectorStartDegree = clockwise ? roundTotalDegree : roundTotalDegree + 180;
                sectorToDegree = clockwise ? roundTotalDegree + 180 : roundTotalDegree + 360;
                DrawSector(vh, roundCenter, roundRadius, color, sectorStartDegree, sectorToDegree, smoothness / 2);
                if (needBorder)
                {
                    DrawDoughnut(vh, roundCenter, roundRadius, roundRadius + borderWidth, borderColor, Color.clear,
                        sectorStartDegree, sectorToDegree, smoothness / 2);
                }
                e1 = GetPos(p, insideRadius, realToOutAngle);
                e2 = GetPos(p, outsideRadius, realToOutAngle);
            }
            float segmentAngle = (realToInAngle - realStartInAngle) / segments;
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = realStartInAngle + i * segmentAngle;
                p3 = new Vector3(p.x + outsideRadius * Mathf.Sin(currAngle),
                    p.y + outsideRadius * Mathf.Cos(currAngle));
                p4 = new Vector3(p.x + insideRadius * Mathf.Sin(currAngle),
                    p.y + insideRadius * Mathf.Cos(currAngle));
                if (!ChartHelper.IsClearColor(emptyColor)) DrawTriangle(vh, p, p1, p4, emptyColor);
                DrawPolygon(vh, p2, p3, p4, p1, color, toColor);
                p1 = p4;
                p2 = p3;
            }
            if (needBorder || needSpace || roundCap)
            {
                if (clockwise)
                {
                    var isInAngleFixed = toAngle - spaceInAngle - 2 * borderInAngle > realStartOutAngle;
                    if (isInAngleFixed) DrawPolygon(vh, p2, e2, e1, p1, color, toColor);
                    else DrawTriangle(vh, p2, e2, p1, color, color, toColor);
                    if (needBorder)
                    {
                        var realStartDegree = (realStartOutAngle - (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        var realToDegree = (realToOutAngle + (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        if (realToDegree < realStartOutAngle) realToDegree = realStartOutAngle;
                        var inStartDegree = roundCap ? realStartDegree : (startAngle + spaceInAngle) * Mathf.Rad2Deg;
                        var inToDegree = roundCap ? realToDegree : (toAngle - spaceInAngle) * Mathf.Rad2Deg;
                        if (inToDegree < inStartDegree) inToDegree = inStartDegree;
                        if (isInAngleFixed) DrawDoughnut(vh, p, insideRadius - borderWidth, insideRadius, borderColor, Color.clear,
                              inStartDegree, inToDegree, smoothness);
                        DrawDoughnut(vh, p, outsideRadius, outsideRadius + borderWidth, borderColor, Color.clear,
                            realStartDegree, realToDegree, smoothness);
                    }
                }
                else
                {
                    var isInAngleFixed = toAngle + spaceInAngle + 2 * borderInAngle < realStartOutAngle;
                    if (isInAngleFixed) DrawPolygon(vh, p2, e2, e1, p1, color, toColor);
                    else DrawTriangle(vh, p2, e2, p1, color, color, toColor);
                    if (needBorder)
                    {
                        var realStartDegree = (realStartOutAngle + (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        var realToDegree = (realToOutAngle - (roundCap ? 0 : borderAngle)) * Mathf.Rad2Deg;
                        var inStartDegree = roundCap ? realStartDegree : (startAngle - spaceInAngle) * Mathf.Rad2Deg;
                        var inToDegree = roundCap ? realToDegree : (toAngle + spaceInAngle) * Mathf.Rad2Deg;
                        if (inToDegree > inStartDegree) inToDegree = inStartDegree;
                        if (isInAngleFixed) DrawDoughnut(vh, p, insideRadius - borderWidth, insideRadius, borderColor, Color.clear,
                              inStartDegree, inToDegree, smoothness);
                        DrawDoughnut(vh, p, outsideRadius, outsideRadius + borderWidth, borderColor, Color.clear,
                            realStartDegree, realToDegree, smoothness);
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

        public static void DrawSymbol(VertexHelper vh, SerieSymbolType type, float symbolSize,
           float tickness, Vector3 pos, Color color, Color toColor, float gap, float[] cornerRadius,
           Color backgroundColor, float smoothness)
        {
            switch (type)
            {
                case SerieSymbolType.None:
                    break;
                case SerieSymbolType.Circle:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawDoughnut(vh, pos, symbolSize, symbolSize + gap, backgroundColor, color, toColor, smoothness);
                    }
                    else
                    {
                        ChartDrawer.DrawCricle(vh, pos, symbolSize, color, toColor, smoothness);
                    }
                    break;
                case SerieSymbolType.EmptyCircle:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawCricle(vh, pos, symbolSize + gap, backgroundColor, smoothness);
                        ChartDrawer.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, toColor, backgroundColor, smoothness);
                    }
                    else
                    {
                        ChartDrawer.DrawEmptyCricle(vh, pos, symbolSize, tickness, color, toColor, backgroundColor, smoothness);
                    }
                    break;
                case SerieSymbolType.Rect:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawPolygon(vh, pos, symbolSize + gap, backgroundColor);
                        ChartDrawer.DrawPolygon(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        //ChartDrawer.DrawPolygon(vh, pos, symbolSize, color, toColor);
                        ChartDrawer.DrawRoundRectangle(vh, pos, symbolSize, symbolSize, color, 0, cornerRadius);
                    }
                    break;
                case SerieSymbolType.Triangle:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawTriangle(vh, pos, symbolSize + gap, backgroundColor);
                        ChartDrawer.DrawTriangle(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        ChartDrawer.DrawTriangle(vh, pos, symbolSize, color, toColor);
                    }
                    break;
                case SerieSymbolType.Diamond:
                    if (gap > 0)
                    {
                        ChartDrawer.DrawDiamond(vh, pos, symbolSize + gap, backgroundColor);
                        ChartDrawer.DrawDiamond(vh, pos, symbolSize, color, toColor);
                    }
                    else
                    {
                        ChartDrawer.DrawDiamond(vh, pos, symbolSize, color, toColor);
                    }
                    break;
            }
        }

        public static void DrawLineStyle(VertexHelper vh, LineStyle lineStyle,
            Vector3 startPos, Vector3 endPos, Color color)
        {
            var type = lineStyle.type;
            var width = lineStyle.width;
            switch (type)
            {
                case LineStyle.Type.Dashed:
                    ChartDrawer.DrawDashLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.Dotted:
                    ChartDrawer.DrawDotLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.Solid:
                    ChartDrawer.DrawLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.DashDot:
                    ChartDrawer.DrawDashDotLine(vh, startPos, endPos, width, color);
                    break;
                case LineStyle.Type.DashDotDot:
                    ChartDrawer.DrawDashDotDotLine(vh, startPos, endPos, width, color);
                    break;
            }
        }
    }
}