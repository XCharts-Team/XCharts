/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System.Text;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XCharts
{
    public static class ChartHelper
    {
        private static StringBuilder s_Builder = new StringBuilder();

        public static string Cancat(string str1, string str2)
        {
            s_Builder.Length = 0;
            s_Builder.Append(str1).Append(str2);
            return s_Builder.ToString();
        }

        public static string Cancat(string str1, int i)
        {
            s_Builder.Length = 0;
            s_Builder.Append(str1).Append(ChartCached.IntToStr(i));
            return s_Builder.ToString();
        }

        public static void SetActive(GameObject gameObject, bool active)
        {
            SetActive(gameObject.transform, active);
        }

        /// <summary>
        /// 通过设置scale实现是否显示，优化性能，减少GC
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="active"></param>   
        public static void SetActive(Transform transform, bool active)
        {
            if (active) transform.localScale = Vector3.one;
            else transform.localScale = Vector3.zero;
        }
        public static void HideAllObject(GameObject obj, string match = null)
        {
            HideAllObject(obj.transform, match);
        }

        public static void HideAllObject(Transform parent, string match = null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (match == null)
                    SetActive(parent.GetChild(i), false);
                //parent.GetChild(i).gameObject.SetActive(false);
                else
                {
                    var go = parent.GetChild(i);
                    if (go.name.StartsWith(match))
                    {
                        SetActive(go, false);
                        //go.gameObject.SetActive(false);
                    }
                }
            }
        }

        public static void DestroyAllChildren(Transform parent)
        {
            if (parent == null) return;
#if UNITY_EDITOR && UNITY_2018_3_OR_NEWER
            if (PrefabUtility.IsPartOfAnyPrefab(parent.gameObject))
            {
                return;
            }
#endif
            while (parent.childCount > 0)
            {
                var go = parent.GetChild(0);
                if (go != null)
                {
                    GameObject.DestroyImmediate(go.gameObject);
                }
            }
        }

        public static string GetFullName(Transform transform)
        {
            string name = transform.name;
            Transform obj = transform;
            while (obj.transform.parent)
            {
                name += "/" + obj.transform.parent.name;
                obj = obj.transform.parent;
            }
            return name;
        }

        public static T GetOrAddComponent<T>(Transform transform) where T : Component
        {
            return GetOrAddComponent<T>(transform.gameObject);
        }

        public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() == null)
            {
                gameObject.AddComponent<T>();
            }
            return gameObject.GetComponent<T>();
        }

        public static GameObject AddObject(string name, Transform parent, Vector2 anchorMin,
            Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta)
        {
            GameObject obj;
            if (parent.Find(name))
            {
                obj = parent.Find(name).gameObject;
                obj.SetActive(true);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
            }
            else
            {
                obj = new GameObject();
                obj.name = name;
                obj.transform.SetParent(parent);
                obj.transform.localScale = Vector3.one;
                obj.transform.localPosition = Vector3.zero;
            }
            RectTransform rect = GetOrAddComponent<RectTransform>(obj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            return obj;
        }

        public static Text AddTextObject(string name, Transform parent, Font font, Color color,
            TextAnchor anchor, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta,
            int fontSize = 14, float rotate = 0, FontStyle fontStyle = FontStyle.Normal)
        {
            GameObject txtObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            Text txt = GetOrAddComponent<Text>(txtObj);
            txt.font = font;
            txt.fontSize = fontSize;
            txt.fontStyle = fontStyle;
            txt.text = "Text";
            txt.alignment = anchor;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
            txt.color = color;
            txtObj.transform.localEulerAngles = new Vector3(0, 0, rotate);

            RectTransform rect = GetOrAddComponent<RectTransform>(txtObj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            return txtObj.GetComponent<Text>();
        }

        public static Button AddButtonObject(string name, Transform parent, Font font, int fontSize,
            Color color, TextAnchor anchor, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta)
        {
            GameObject btnObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            GetOrAddComponent<Image>(btnObj);
            GetOrAddComponent<Button>(btnObj);
            Text txt = AddTextObject("Text", btnObj.transform, font, color, TextAnchor.MiddleCenter,
                    new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f),
                    sizeDelta, fontSize);
            txt.rectTransform.offsetMin = Vector2.zero;
            txt.rectTransform.offsetMax = Vector2.zero;
            return btnObj.GetComponent<Button>();
        }

        internal static GameObject AddTooltipContent(string name, Transform parent, Font font, int fontSize, FontStyle fontStyle)
        {
            var anchorMax = new Vector2(0, 1);
            var anchorMin = new Vector2(0, 1);
            var pivot = new Vector2(0, 1);
            var sizeDelta = new Vector2(100, 100);
            GameObject tooltipObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var img = GetOrAddComponent<Image>(tooltipObj);
            img.color = Color.black;
            Text txt = AddTextObject("Text", tooltipObj.transform, font, Color.white, TextAnchor.UpperLeft,
                    anchorMin, anchorMax, pivot, sizeDelta, fontSize, 0, fontStyle);
            txt.text = "Text";
            txt.transform.localPosition = new Vector2(3, -3);
            tooltipObj.transform.localPosition = Vector3.zero;
            return tooltipObj;
        }

        public static GameObject AddIcon(string name, Transform parent, float width, float height)
        {
            var anchorMax = new Vector2(0.5f, 0.5f);
            var anchorMin = new Vector2(0.5f, 0.5f);
            var pivot = new Vector2(0.5f, 0.5f);
            var sizeDelta = new Vector2(width, height);
            GameObject iconObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            GetOrAddComponent<Image>(iconObj);
            return iconObj;
        }

        internal static GameObject AddSerieLabel(string name, Transform parent, Font font, Color textColor, Color backgroundColor,
            int fontSize, FontStyle fontStyle, float rotate, float width, float height)
        {
            var anchorMin = new Vector2(0.5f, 0.5f);
            var anchorMax = new Vector2(0.5f, 0.5f);
            var pivot = new Vector2(0.5f, 0.5f);
            var sizeDelta = (width != 0 && height != 0) ? new Vector2(width, height) : new Vector2(50, fontSize + 2);
            GameObject labelObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            //var img = GetOrAddComponent<Image>(labelObj);
            //img.color = backgroundColor;
            labelObj.transform.localEulerAngles = new Vector3(0, 0, rotate);
            Text txt = AddTextObject("Text", labelObj.transform, font, textColor, TextAnchor.MiddleCenter,
                    anchorMin, anchorMax, pivot, sizeDelta, fontSize, 0, fontStyle);
            txt.text = "Text";
            txt.transform.localPosition = new Vector2(0, 0);
            txt.transform.localEulerAngles = Vector3.zero;
            labelObj.transform.localPosition = Vector3.zero;
            return labelObj;
        }

        internal static GameObject AddTooltipLabel(string name, Transform parent, Font font, Vector2 pivot)
        {
            var anchorMax = new Vector2(0, 0);
            var anchorMin = new Vector2(0, 0);
            var sizeDelta = new Vector2(100, 50);
            GameObject labelObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            labelObj.transform.localPosition = Vector3.zero;
            var img = GetOrAddComponent<Image>(labelObj);
            img.color = Color.black;
            Text txt = AddTextObject("Text", labelObj.transform, font, Color.white, TextAnchor.MiddleCenter,
                    new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 1), sizeDelta, 16);
            txt.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            txt.GetComponent<RectTransform>().offsetMax = Vector2.zero;
            txt.text = "Text";
            return labelObj;
        }


        public static void GetPointList(ref List<Vector3> posList, Vector3 sp, Vector3 ep, float k = 30f)
        {
            Vector3 dir = (ep - sp).normalized;
            float dist = Vector3.Distance(sp, ep);
            int segment = (int)(dist / k);
            posList.Clear();
            posList.Add(sp);
            for (int i = 1; i < segment; i++)
            {
                posList.Add(sp + dir * dist * i / segment);
            }
            posList.Add(ep);
        }

        public static void GetBezierList(ref List<Vector3> posList, Vector3 sp, Vector3 ep,
            Vector3 lsp, Vector3 nep, float smoothness = 2f, float k = 2.0f)
        {
            float dist = Mathf.Abs(sp.x - ep.x);
            Vector3 cp1, cp2;
            var dir = (ep - sp).normalized;
            var diff = dist / k;
            if (lsp == sp)
            {
                cp1 = sp + dist / k * dir * 1;
                cp1.y = sp.y;
                cp1 = sp;
            }
            else
            {
                cp1 = sp + (ep - lsp).normalized * diff;
            }
            if (nep == ep) cp2 = ep;
            else cp2 = ep - (nep - sp).normalized * diff;
            dist = Vector3.Distance(sp, ep);
            int segment = (int)(dist / (smoothness <= 0 ? 2f : smoothness));
            if (segment < 1) segment = (int)(dist / 0.5f);
            if (segment < 4) segment = 4;
            GetBezierList2(ref posList, sp, ep, segment, cp1, cp2);
        }

        public static void GetBezierListVertical(ref List<Vector3> posList, Vector3 sp, Vector3 ep, float smoothness = 2f, float k = 2.0f)
        {
            Vector3 dir = (ep - sp).normalized;
            float dist = Vector3.Distance(sp, ep);
            Vector3 cp1 = sp + dist / k * dir * 1;
            Vector3 cp2 = sp + dist / k * dir * (k - 1);
            cp1.x = sp.x;
            cp2.x = ep.x;
            int segment = (int)(dist / (smoothness <= 0 ? 2f : smoothness));
            GetBezierList2(ref posList, sp, ep, segment, cp1, cp2);
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

        public static void GetBezierList2(ref List<Vector3> posList, Vector3 sp, Vector3 ep, int segment, Vector3 cp,
            Vector3 cp2)
        {
            posList.Clear();
            if (posList.Capacity < segment + 1)
            {
                posList.Capacity = segment + 1;
            }
            for (int i = 0; i < segment; i++)
            {
                posList.Add((GetBezier2(i / (float)segment, sp, cp, cp2, ep)));
            }
            posList.Add(ep);
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

        public static bool IsValueEqualsColor(Color32 color1, Color32 color2)
        {
            return color1.a == color2.a &&
                color1.b == color2.b &&
                color1.g == color2.g &&
                color1.r == color2.r;
        }

        public static bool IsValueEqualsList<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null || list2 == null) return false;
            if (list1.Count != list2.Count) return false;
            for (int i = 0; i < list1.Count; i++)
            {
                if (!list1[i].Equals(list2[i])) return false;
            }
            return true;
        }

        public static List<float> ParseFloatFromString(string jsonData)
        {
            List<float> list = new List<float>();
            if (string.IsNullOrEmpty(jsonData)) return list;
            int startIndex = jsonData.IndexOf("[");
            int endIndex = jsonData.IndexOf("]");
            string temp = jsonData.Substring(startIndex + 1, endIndex - startIndex - 1);
            if (temp.IndexOf("],") > -1 || temp.IndexOf("] ,") > -1)
            {
                string[] datas = temp.Split(new string[] { "],", "] ," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    temp = datas[i];
                }
                return list;
            }
            else
            {
                string[] datas = temp.Split(',');
                for (int i = 0; i < datas.Length; i++)
                {
                    list.Add(float.Parse(datas[i].Trim()));
                }
                return list;
            }

        }

        public static List<string> ParseStringFromString(string jsonData)
        {
            List<string> list = new List<string>();
            if (string.IsNullOrEmpty(jsonData)) return list;
            string pattern = "[\"'](.*?)[\"']";
            if (Regex.IsMatch(jsonData, pattern))
            {
                MatchCollection m = Regex.Matches(jsonData, pattern);
                foreach (Match match in m)
                {
                    list.Add(match.Groups[1].Value);
                }
            }
            return list;
        }

        public static Color32 GetColor(string hexColorStr)
        {
            Color color;
            ColorUtility.TryParseHtmlString(hexColorStr, out color);
            return (Color32)color;
        }

        public static float GetMaxDivisibleValue(float max)
        {
            if (max == 0) return 0;
            if (max > -1 && max < 1)
            {
                int count = 1;
                int intvalue = (int)(max * Mathf.Pow(10, count));
                while (intvalue == 0 && count < 12)
                {
                    count++;
                    intvalue = (int)(max * Mathf.Pow(10, count));
                }
                if (max > 0) return 1 / Mathf.Pow(10, count - 1);
                else return -1 / Mathf.Pow(10, count);
            }
            int bigger = Mathf.CeilToInt(Mathf.Abs(max));
            int n = 1;
            while (bigger / (Mathf.Pow(10, n)) > 10)
            {
                n++;
            }
            float mm = bigger;
            if (mm > 10)
            {
                mm = bigger - bigger % (Mathf.Pow(10, n));
                mm += max > 0 ? Mathf.Pow(10, n) : -Mathf.Pow(10, n);
            }
            if (max < 0) return -Mathf.CeilToInt(mm);
            else return Mathf.CeilToInt(mm);
        }

        public static float GetMinDivisibleValue(float min)
        {
            if (min == 0) return 0;
            if (min > -1 && min < 1)
            {
                int count = 1;
                int intvalue = (int)(min * Mathf.Pow(10, count));
                while (intvalue == 0 && count < 12)
                {
                    count++;
                    intvalue = (int)(min * Mathf.Pow(10, count));
                }
                if (min > 0) return 1 / Mathf.Pow(10, count);
                else return -1 / Mathf.Pow(10, count - 1);
            }
            int bigger = Mathf.FloorToInt(Mathf.Abs(min));
            int n = 1;
            while (bigger / (Mathf.Pow(10, n)) > 10)
            {
                n++;
            }
            float mm = bigger;
            if (mm > 10)
            {
                mm = bigger - bigger % (Mathf.Pow(10, n));
                mm += min < 0 ? Mathf.Pow(10, n) : -Mathf.Pow(10, n);
            }
            if (min < 0) return -Mathf.FloorToInt(mm);
            else return Mathf.FloorToInt(mm);
        }

        public static int GetFloatAccuracy(float value)
        {
            if (value > 1 || value < -1) return 0;
            int count = 1;
            int intvalue = (int)(value * Mathf.Pow(10, count));
            while (intvalue == 0 && count < 12)
            {
                count++;
                intvalue = (int)(value * Mathf.Pow(10, count));
            }
            return count;
        }

        public static void AddEventListener(GameObject obj, EventTriggerType type,
            UnityEngine.Events.UnityAction<BaseEventData> call)
        {
            EventTrigger trigger = GetOrAddComponent<EventTrigger>(obj.gameObject);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback = new EventTrigger.TriggerEvent();
            entry.callback.AddListener(call);
            trigger.triggers.Add(entry);
        }

        public static void ClearEventListener(GameObject obj)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            if (trigger != null)
            {
                trigger.triggers.Clear();
            }
        }


        //获取两直线交点
        public static Vector3 GetIntersection(Vector3 lineAStart, Vector3 lineAEnd, Vector3 lineBStart,
            Vector3 lineBEnd)
        {
            float x1 = lineAStart.x, y1 = lineAStart.y;
            float x2 = lineAEnd.x, y2 = lineAEnd.y;
            float x3 = lineBStart.x, y3 = lineBStart.y;
            float x4 = lineBEnd.x, y4 = lineBEnd.y;
            if (x1 == x2 && x3 == x4 && x1 == x3)
            {
                return Vector3.zero;
            }
            if (y1 == y2 && y3 == y4 && y1 == y3)
            {
                return Vector3.zero;
            }
            if (x1 == x2 && x3 == x4)
            {
                return Vector3.zero;
            }
            if (y1 == y2 && y3 == y4)
            {
                return Vector3.zero;
            }
            float x, y;
            if (x1 == x2)
            {
                float m2 = (y4 - y3) / (x4 - x3);
                float c2 = -m2 * x3 + y3;
                x = x1;
                y = c2 + m2 * x1;
            }
            else if (x3 == x4)
            {
                float m1 = (y2 - y1) / (x2 - x1);
                float c1 = -m1 * x1 + y1;
                x = x3;
                y = c1 + m1 * x3;
            }
            else
            {
                float m1 = (y2 - y1) / (x2 - x1);
                float c1 = -m1 * x1 + y1;
                float m2 = (y4 - y3) / (x4 - x3);
                float c2 = -m2 * x3 + y3;
                x = (c1 - c2) / (m2 - m1);
                y = c2 + m2 * x;
            }

            if (IsInsideLine(lineAStart, lineAEnd, x, y) &&
                IsInsideLine(lineBStart, lineBEnd, x, y))
            {
                return new Vector3(x, y, 0);
            }
            return Vector3.zero;
        }

        private static bool IsInsideLine(Vector3 start, Vector3 end, float x, float y)
        {
            return ((x >= start.x && x <= end.x)
                || (x >= end.x && x <= start.x))
                && ((y >= start.y && y <= end.y)
                    || (y >= end.y && y <= start.y));
        }

        public static Vector3 RotateRound(Vector3 position, Vector3 center, Vector3 axis, float angle)
        {
            Vector3 point = Quaternion.AngleAxis(angle, axis) * (position - center);
            Vector3 resultVec3 = center + point;
            return resultVec3;
        }
    }
}