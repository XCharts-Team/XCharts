using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace xcharts
{
    public static class ChartUtils
    {
        public static Text AddTextObject(string name, Transform parent, Font font, TextAnchor anchor,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta,int fontSize = 14)
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

            txtObj.GetComponent<Text>().alignment = anchor;
            RectTransform rect = txtObj.GetComponent<RectTransform>();
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            rect.localPosition = Vector3.zero;
            return txtObj.GetComponent<Text>();
        }

        public static Button AddButtonObject(string name,Transform parent,Font font,
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

                Text txt = AddTextObject("Text", btnObj.transform, font, TextAnchor.MiddleCenter, Vector2.zero,
                    Vector2.zero, Vector2.zero, sizeDelta);
                txt.text = "Text";
            }
            RectTransform rect = btnObj.GetComponent<RectTransform>();
            if (rect == null)
            {
                rect = btnObj.AddComponent<RectTransform>();
            }
            rect.anchorMax = anchorMax;
            rect.anchorMin = anchorMin;
            rect.pivot = pivot;
            rect.sizeDelta = sizeDelta;
            return btnObj.GetComponent<Button>();
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
            DrawPolygon(vh, p1, p2, p3, p4, color);
        }

        public static void DrawPolygon(VertexHelper vh, Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4,
            Color color)
        {
            UIVertex[] vertex = new UIVertex[4];
            vertex[0].position = p1;
            vertex[1].position = p2;
            vertex[2].position = p3;
            vertex[3].position = p4;
            for (int j = 0; j < 4; j++)
            {
                vertex[j].color = color;
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
            int segments)
        {
            List<UIVertex> vertexs = new List<UIVertex>();
            vh.GetUIVertexStream(vertexs);
            float angle = 2 * Mathf.PI / segments;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);
            float cx = radius, cy = 0;

            List<UIVertex> vs = new List<UIVertex>();

            segments--;
            Vector3 p2, p3;
            for (int i = 0; i < segments; i++)
            {
                p2 = new Vector3(p.x + cx, p.y + cy);
                float temp = cx;
                cx = cos * cx - sin * cy;
                cy = sin * temp + cos * cy;
                p3 = new Vector3(p.x + cx, p.y + cy);
                DrawTriangle(vh, vertexs, p, p2, p3, color);
            }
            p2 = new Vector3(p.x + cx, p.y + cy);
            cx = radius;
            cy = 0;
            p3 = new Vector3(p.x + cx, p.y + cy);
            DrawTriangle(vh, vertexs, p, p2, p3, color);
        }

    }
}