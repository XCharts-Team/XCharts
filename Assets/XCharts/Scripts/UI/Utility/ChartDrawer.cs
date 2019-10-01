
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public static class ChartDrawer
    {
        public static float CRICLE_SMOOTHNESS = 2f;
        private static UIVertex[] vertex = new UIVertex[4];

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
            float dashLen = 15f, float blankLen = 7f)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dashLen + blankLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            for (int i = 1; i <= segment; i++)
            {
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankLen;
                DrawLine(vh, sp, dashep, size, color);
                sp = np;
            }
            DrawLine(vh, sp, p2, size, color);
        }
        public static void DrawDotLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color,
            float dotLen = 5f, float blankLen = 5f)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dotLen + blankLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            for (int i = 1; i <= segment; i++)
            {
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankLen;
                DrawLine(vh, sp, dashep, size, color);
                sp = np;
            }
            DrawLine(vh, sp, p2, size, color);
        }

        public static void DrawDashDotLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color32 color,
            float dashLen = 15f, float blankDotLen = 15f)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dashLen + blankDotLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            for (int i = 1; i <= segment; i++)
            {
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankDotLen;
                DrawLine(vh, sp, dashep, size, color);
                var dotsp = dashep + (blankDotLen - 2 * size) / 2 * dir;
                var dotep = dotsp + 2 * size * dir;
                DrawLine(vh, dotsp, dotep, size, color);
                sp = np;
            }
            DrawLine(vh, sp, p2, size, color);
        }

        public static void DrawDashDotDotLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size,
            Color32 color, float dashLen = 15f, float blankDotLen = 20f)
        {
            float dist = Vector3.Distance(p1, p2);
            if (dist < 0.1f) return;
            int segment = Mathf.CeilToInt(dist / (dashLen + blankDotLen));
            Vector3 dir = (p2 - p1).normalized;
            Vector3 sp = p1, np;
            for (int i = 1; i <= segment; i++)
            {
                np = p1 + dir * dist * i / segment;
                var dashep = np - dir * blankDotLen;
                DrawLine(vh, sp, dashep, size, color);
                var dotsp = dashep + (blankDotLen / 2 - 2 * size) / 2 * dir;
                var dotep = dotsp + 2 * size * dir;
                DrawLine(vh, dotsp, dotep, size, color);
                var dotsp2 = dashep + blankDotLen / 2 * dir;
                dotsp2 = dotsp2 + (blankDotLen / 4 - 2 * size) / 2 * dir;
                var dotep2 = dotsp2 + 2 * size * dir;
                DrawLine(vh, dotsp2, dotep2, size, color);
                sp = np;
            }
            DrawLine(vh, sp, p2, size, color);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p, float size, Color32 color)
        {
            Vector3 p1 = new Vector3(p.x - size, p.y - size);
            Vector3 p2 = new Vector3(p.x + size, p.y - size);
            Vector3 p3 = new Vector3(p.x + size, p.y + size);
            Vector3 p4 = new Vector3(p.x - size, p.y + size);
            DrawPolygon(vh, p1, p2, p3, p4, color, color);
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
            int segments = 0)
        {
            if (segments <= 0)
            {
                segments = (int)((2 * Mathf.PI * radius) / CRICLE_SMOOTHNESS);
            }
            if (segments < 3) segments = 3;
            DrawSector(vh, p, radius, color, 0, 360, segments);
        }

        public static void DrawCicleNotFill(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color32 color, int segments = 0)
        {
            if (segments <= 0)
            {
                segments = (int)((2 * Mathf.PI * radius) / CRICLE_SMOOTHNESS);
            }
            float startDegree = 0, toDegree = 360;
            Vector3 p2, p3;
            float startAngle = startDegree * Mathf.Deg2Rad;
            float angle = (toDegree - startDegree) * Mathf.Deg2Rad / segments;
            p2 = new Vector3(p.x + radius * Mathf.Sin(startAngle), p.y + radius * Mathf.Cos(startAngle));
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = startAngle + i * angle;
                p3 = new Vector3(p.x + radius * Mathf.Sin(currAngle), p.y + radius * Mathf.Cos(currAngle));
                DrawLine(vh, p2, p3, tickness, color);
                p2 = p3;
            }
        }

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color32 color,
            float startDegree, float toDegree, int segments = 0)
        {
            if (segments <= 0)
            {
                segments = (int)((2 * Mathf.PI * radius) / CRICLE_SMOOTHNESS);
            }
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
            float startDegree, float toDegree, Color32 color, int segments = 0)
        {
            if (insideRadius <= 0)
            {
                DrawSector(vh, p, outsideRadius, color, startDegree, toDegree, segments);
                return;
            }
            if (segments <= 0)
            {
                segments = (int)((2 * Mathf.PI * outsideRadius) / CRICLE_SMOOTHNESS);
            }
            Vector3 p1, p2, p3, p4;
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
                DrawPolygon(vh, p1, p2, p3, p4, color);
                p1 = p4;
                p2 = p3;
            }
        }

    }
}