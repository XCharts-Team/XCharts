using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    public static class ChartUtils
    {
        private static float CRICLE_SMOOTHNESS = 1f;
        public static Text AddTextObject(string name, Transform parent, Font font, Color color,
            TextAnchor anchor, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta,
            int fontSize = 14)
        {
            GameObject txtObj;
            if (parent.Find(name))
            {
                txtObj = parent.Find(name).gameObject;
                txtObj.SetActive(true);
                txtObj.transform.localPosition = Vector3.zero;
            }
            else
            {
                txtObj = new GameObject();
                txtObj.name = name;
                txtObj.transform.parent = parent;
                txtObj.transform.localScale = Vector3.one;
                txtObj.transform.localPosition = Vector3.zero;
                txtObj.AddComponent<Text>();
            }
            Text txt = txtObj.GetComponent<Text>();
            txt.font = font;
            txt.fontSize = fontSize;
            txt.text = "Text";
            txt.alignment = anchor;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
            txt.color = color;

            txtObj.GetComponent<Text>().alignment = anchor;
            RectTransform rect = txtObj.GetComponent<RectTransform>();
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            return txtObj.GetComponent<Text>();
        }

        public static Button AddButtonObject(string name, Transform parent, Font font, Color color,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta)
        {
            GameObject btnObj;
            if (parent.Find(name))
            {
                btnObj = parent.Find(name).gameObject;
                btnObj.SetActive(true);
            }
            else
            {
                btnObj = new GameObject();
                btnObj.name = name;
                btnObj.transform.parent = parent;
                btnObj.transform.localPosition = Vector3.zero;
                btnObj.transform.localScale = Vector3.one;
                btnObj.AddComponent<Image>();
                btnObj.AddComponent<Button>();

                Text txt = AddTextObject("Text", btnObj.transform, font, color, TextAnchor.MiddleCenter,
                    Vector2.zero, Vector2.zero, Vector2.zero, sizeDelta);
                txt.text = "Text";
            }
            RectTransform rect = btnObj.GetComponent<RectTransform>();
            if (rect == null)
            {
                rect = btnObj.AddComponent<RectTransform>();
            }
            btnObj.GetComponentInChildren<Text>().color = color;
            rect.anchorMax = anchorMax;
            rect.anchorMin = anchorMin;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            return btnObj.GetComponent<Button>();
        }

        public static GameObject AddTooltipObject(string name, Transform parent, Font font)
        {
            GameObject tooltipObj;
            if (parent.Find(name))
            {
                tooltipObj = parent.Find(name).gameObject;
                tooltipObj.SetActive(true);
            }
            else
            {
                tooltipObj = new GameObject();
                tooltipObj.name = name;
                tooltipObj.transform.parent = parent;
                tooltipObj.transform.localPosition = Vector3.zero;
                tooltipObj.transform.localScale = Vector3.one;
                tooltipObj.AddComponent<Image>();
                Text txt = AddTextObject("Text", tooltipObj.transform, font, Color.white, TextAnchor.UpperLeft,
                    new Vector2(0, 1), new Vector2(0, 1), new Vector2(0, 1), new Vector2(100, 100));
                txt.text = "Text";
            }
            RectTransform rect = tooltipObj.GetComponent<RectTransform>();
            if (rect == null)
            {
                rect = tooltipObj.AddComponent<RectTransform>();
            }
            tooltipObj.GetComponent<Image>().color = Color.black;
            rect.anchorMax = new Vector2(0, 1);
            rect.anchorMin = new Vector2(0, 1);
            rect.pivot = new Vector2(0, 1);
            rect.sizeDelta = new Vector2(100, 100);
            tooltipObj.GetComponentInChildren<Text>().transform.localPosition = new Vector2(3, -3);
            return tooltipObj;
        }

        public static void DrawLine(VertexHelper vh, Vector3 p1, Vector3 p2, float size, Color color)
        {
            Vector3 v = Vector3.Cross(p2 - p1, Vector3.forward).normalized * size;

            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = p1 + v;
            vertex[1].position = p2 + v;
            vertex[2].position = p2 - v;
            vertex[3].position = p1 - v;
            for (int j = 0; j < 4; j++)
            {
                vertex[j].color = color;
                vertex[j].uv0 = Vector2.zero;
            }
            vh.AddUIVertexQuad(vertex);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p, float size, Color color)
        {
            Vector3 p1 = new Vector3(p.x - size, p.y - size);
            Vector3 p2 = new Vector3(p.x + size, p.y - size);
            Vector3 p3 = new Vector3(p.x + size, p.y + size);
            Vector3 p4 = new Vector3(p.x - size, p.y + size);
            DrawPolygon(vh, p1, p2, p3, p4, color, color);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color color)
        {
            DrawPolygon(vh, p1, p2, p3, p4, color, color);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color startColor, Color toColor)
        {
            UIVertex[] vertex = new UIVertex[4];
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

        public static void DrawTriangle(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3,
            Color color)
        {
            List<UIVertex> vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
            DrawTriangle(vh, vertexs, p1, p2, p3, color);
        }

        public static void DrawTriangle(VertexHelper vh, List<UIVertex> vertexs, Vector3 p1,
            Vector3 p2, Vector3 p3, Color color)
        {
            UIVertex v1 = new UIVertex();
            v1.position = p1;
            v1.color = color;
            v1.uv0 = Vector3.zero;
            vertexs.Add(v1);
            UIVertex v2 = new UIVertex();
            v2.position = p2;
            v2.color = color;
            v2.uv0 = Vector3.zero;
            vertexs.Add(v2);
            UIVertex v3 = new UIVertex();
            v3.position = p3;
            v3.color = color;
            v3.uv0 = Vector3.zero;
            vertexs.Add(v3);
            vh.AddUIVertexTriangleStream(vertexs);
        }

        public static void DrawCricle(VertexHelper vh, Vector3 p, float radius, Color color,
            int segments = 0, bool fill = true)
        {
            if (segments <= 0)
            {
                segments = (int)((2 * Mathf.PI * radius) / CRICLE_SMOOTHNESS);
            }
            DrawSector(vh, p, radius, color, 0, 360, segments);
        }

        public static void DrawCicleNotFill(VertexHelper vh, Vector3 p, float radius, float tickness,
            Color color, int segments = 0)
        {
            if (segments <= 0)
            {
                segments = (int)((2 * Mathf.PI * radius) / CRICLE_SMOOTHNESS);
            }
            float startDegree = 0, toDegree = 360;
            List<UIVertex> vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
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

        public static void DrawSector(VertexHelper vh, Vector3 p, float radius, Color color,
            float startDegree, float toDegree, int segments = 0)
        {
            if (segments <= 0)
            {
                segments = (int)((2 * Mathf.PI * radius) / CRICLE_SMOOTHNESS);
            }
            List<UIVertex> vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
            Vector3 p2, p3;
            float startAngle = startDegree * Mathf.Deg2Rad;
            float angle = (toDegree - startDegree) * Mathf.Deg2Rad / segments;
            p2 = new Vector3(p.x + radius * Mathf.Sin(startAngle), p.y + radius * Mathf.Cos(startAngle));
            for (int i = 0; i <= segments; i++)
            {
                float currAngle = startAngle + i * angle;
                p3 = new Vector3(p.x + radius * Mathf.Sin(currAngle),
                    p.y + radius * Mathf.Cos(currAngle));
                DrawTriangle(vh, vertexs, p, p2, p3, color);
                p2 = p3;
            }
        }

        public static void DrawDoughnut(VertexHelper vh, Vector3 p, float insideRadius, float outsideRadius,
            float startDegree, float toDegree, Color color, int segments = 0)
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


        public static List<Vector3> GetBezierList(Vector3 sp, Vector3 ep, float k = 2.0f)
        {
            Vector3 dir = (ep - sp).normalized;
            float dist = Vector3.Distance(sp, ep);
            Vector3 cp1 = sp + dist / k * dir * 1;
            Vector3 cp2 = sp + dist / k * dir * (k - 1);
            cp1.y = sp.y;
            cp2.y = ep.y;
            int segment = (int)(dist / 0.1f);
            return GetBezierList2(sp, ep, segment, cp1, cp2);
        }

        public static List<Vector3> GetBezierList(Vector3 sp, Vector3 ep, int segment, Vector3 cp)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < segment; i++)
            {
                list.Add(GetBezier(i / (float)segment, sp, cp, ep));
            }
            list.Add(ep);
            return list;
        }

        public static List<Vector3> GetBezierList2(Vector3 sp, Vector3 ep, int segment, Vector3 cp,
            Vector3 cp2)
        {
            List<Vector3> list = new List<Vector3>();
            for (int i = 0; i < segment; i++)
            {
                list.Add(GetBezier2(i / (float)segment, sp, cp, cp2, ep));
            }
            list.Add(ep);
            return list;
        }

        public static Vector3 GetBezier(float t, Vector3 sp, Vector3 cp, Vector3 ep)
        {
            Vector3 aa = sp + (cp - sp) * t;
            Vector3 bb = cp + (ep - cp) * t;
            return aa + (bb - aa) * t;
        }

        public static Vector3 GetBezier2(float t, Vector3 sp, Vector3 p1, Vector3 p2, Vector3 ep)
        {
            t = Mathf.Clamp01(t);
            var oneMinusT = 1f - t;
            return oneMinusT * oneMinusT * oneMinusT * sp +
                3f * oneMinusT * oneMinusT * t * p1 +
                3f * oneMinusT * t * t * p2 +
                t * t * t * ep;
        }

        public static List<Vector3> GetBezierN(List<Vector3> arrayToCurve, float smoothness = 1.0f)
        {
            List<Vector3> points;
            List<Vector3> curvedPoints;
            int pointsLength = 0;
            int curvedLength = 0;

            if (smoothness < 1.0f) smoothness = 1.0f;

            pointsLength = arrayToCurve.Count;

            curvedLength = (pointsLength * Mathf.RoundToInt(smoothness)) - 1;
            curvedPoints = new List<Vector3>(curvedLength);

            float t = 0.0f;
            for (int pointInTimeOnCurve = 0; pointInTimeOnCurve < curvedLength + 1; pointInTimeOnCurve++)
            {
                t = Mathf.InverseLerp(0, curvedLength, pointInTimeOnCurve);

                points = new List<Vector3>(arrayToCurve);

                for (int j = pointsLength - 1; j > 0; j--)
                {
                    for (int i = 0; i < j; i++)
                    {
                        points[i] = (1 - t) * points[i] + t * points[i + 1];
                    }
                }

                curvedPoints.Add(points[0]);
            }

            return curvedPoints;
        }
    }
}