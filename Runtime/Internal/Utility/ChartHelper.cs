/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System.Text;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if dUI_TextMeshPro
using TMPro;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace XCharts
{
    public static class ChartHelper
    {
        private static StringBuilder s_Builder = new StringBuilder();
        private static Vector3 s_DefaultIngoreDataVector3 = Vector3.zero;

        public static StringBuilder sb { get { return s_Builder; } }
        public static Vector3 ignoreVector3 { get { return s_DefaultIngoreDataVector3; } }

        public static bool IsIngore(Vector3 pos)
        {
            return pos == s_DefaultIngoreDataVector3;
        }
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

        public static void SetActive(Image image, bool active)
        {
            if (image == null) return;
            SetActive(image.gameObject, active);
        }

        public static void SetActive(Text text, bool active)
        {
            if (text == null) return;
            SetActive(text.gameObject, active);
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
            ActiveAllObject(parent, false, match);
        }

        public static void ActiveAllObject(Transform parent, bool active, string match = null)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                if (match == null)
                    SetActive(parent.GetChild(i), active);
                else
                {
                    var go = parent.GetChild(i);
                    if (go.name.StartsWith(match))
                    {
                        SetActive(go, active);
                    }
                }
            }
        }

        public static void DestroyAllChildren(Transform parent)
        {
            if (parent == null) return;
            var childCount = parent.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                var go = parent.GetChild(i);
                if (go != null)
                {
                    GameObject.DestroyImmediate(go.gameObject);
                }
            }
        }

        public static void DestoryGameObject(Transform parent, string childName)
        {
            if (parent == null) return;
            var go = parent.Find(childName);
            if (go != null)
            {
                GameObject.DestroyImmediate(go.gameObject);
            }
        }
        public static void DestoryGameObjectByMatch(Transform parent, string match)
        {
            if (parent == null) return;
            var childCount = parent.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                var go = parent.GetChild(i);
                if (go != null && go.name.StartsWith(match))
                {
                    GameObject.DestroyImmediate(go.gameObject);
                }
            }
        }

        public static void DestoryGameObject(GameObject go)
        {
            if (go != null) GameObject.DestroyImmediate(go);
        }

        public static string GetFullName(Transform transform)
        {
            string name = transform.name;
            Transform obj = transform;
            while (obj.transform.parent)
            {
                name = obj.transform.parent.name + "/" + name;
                obj = obj.transform.parent;
            }
            return name;
        }

        public static void RemoveComponent<T>(GameObject gameObject)
        {
            var component = gameObject.GetComponent<T>();
            if (component != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    GameObject.DestroyImmediate(component as GameObject);
                else
                    GameObject.Destroy(component as GameObject);
#else
                GameObject.Destroy(component as GameObject);
#endif
            }
        }
        public static T GetOrAddComponent<T>(Transform transform) where T : Component
        {
            return GetOrAddComponent<T>(transform.gameObject);
        }

        public static T GetOrAddComponent<T>(GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() == null)
            {
                return gameObject.AddComponent<T>();
            }
            else
            {
                return gameObject.GetComponent<T>();
            }
        }

        public static GameObject AddObject(string name, Transform parent, Vector2 anchorMin,
            Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta, int replaceIndex = -1)
        {
            GameObject obj;
            if (parent.Find(name))
            {
                obj = parent.Find(name).gameObject;
                SetActive(obj, true);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
            }
            else if (replaceIndex >= 0 && replaceIndex < parent.childCount)
            {
                obj = parent.GetChild(replaceIndex).gameObject;
                if (!obj.name.Equals(name)) obj.name = name;
                SetActive(obj, true);
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
            rect.anchoredPosition3D = Vector3.zero;
            return obj;
        }

        public static void UpdateRectTransform(GameObject obj, Vector2 anchorMin,
            Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta)
        {
            if (obj == null) return;
            RectTransform rect = GetOrAddComponent<RectTransform>(obj);
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
        }

        public static ChartText AddTextObject(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, TextStyle textStyle, ComponentTheme theme)
        {
            GameObject txtObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            txtObj.transform.localEulerAngles = new Vector3(0, 0, textStyle.rotate);
            var chartText = new ChartText();
#if dUI_TextMeshPro
            RemoveComponent<Text>(txtObj);
            chartText.tmpText = GetOrAddComponent<TextMeshProUGUI>(txtObj);
            chartText.tmpText.font = textStyle.tmpFont == null ? theme.tmpFont : textStyle.tmpFont;
            chartText.tmpText.fontStyle = textStyle.tmpFontStyle;
            chartText.tmpText.alignment = textStyle.tmpAlignment;
            chartText.tmpText.richText = true;
            chartText.tmpText.raycastTarget = false;
            chartText.tmpText.enableWordWrapping = textStyle.autoWrap;
#else
            chartText.text = GetOrAddComponent<Text>(txtObj);
            chartText.text.font = textStyle.font == null ? theme.font : textStyle.font;
            chartText.text.fontStyle = textStyle.fontStyle;
            chartText.text.alignment = textStyle.alignment;
            chartText.text.horizontalOverflow = textStyle.autoWrap ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            chartText.text.verticalOverflow = VerticalWrapMode.Overflow;
            chartText.text.supportRichText = true;
            chartText.text.raycastTarget = false;
#endif
            chartText.SetColor(textStyle.GetColor(theme.textColor));
            chartText.SetFontSize(textStyle.GetFontSize(theme));
            chartText.SetText("Text");
            chartText.SetLineSpacing(textStyle.lineSpacing);

            RectTransform rect = GetOrAddComponent<RectTransform>(txtObj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            return chartText;
        }

        public static Text AddTextObject(string name, Transform parent, Font font, Color color,
            TextAnchor anchor, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta,
            int fontSize = 14, float rotate = 0, FontStyle fontStyle = FontStyle.Normal, float lineSpacing = 1)
        {
            GameObject txtObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var txt = GetOrAddComponent<Text>(txtObj);
            txt.font = font;
            txt.fontSize = fontSize;
            txt.fontStyle = fontStyle;
            txt.text = "Text";
            txt.alignment = anchor;
            txt.horizontalOverflow = HorizontalWrapMode.Overflow;
            txt.verticalOverflow = VerticalWrapMode.Overflow;
            txt.color = color;
            txt.lineSpacing = lineSpacing;
            txt.raycastTarget = false;
            txtObj.transform.localEulerAngles = new Vector3(0, 0, rotate);

            RectTransform rect = GetOrAddComponent<RectTransform>(txtObj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            return txt;
        }

#if dUI_TextMeshPro
        public static TextMeshProUGUI AddTMPTextObject(string name, Transform parent, TMP_FontAsset font, Color color,
           TextAlignmentOptions anchor, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta,
           int fontSize = 14, float rotate = 0, FontStyles fontStyle = FontStyles.Normal, float lineSpacing = 1)
        {
            GameObject txtObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var txt = GetOrAddComponent<TextMeshProUGUI>(txtObj);
            txt.font = font;
            txt.fontSize = fontSize;
            txt.fontStyle = fontStyle;
            txt.text = "Text";
            txt.alignment = anchor;
            txt.color = color;
            txt.lineSpacing = lineSpacing;
            txt.raycastTarget = false;
            txt.enableWordWrapping = false;
            txtObj.transform.localEulerAngles = new Vector3(0, 0, rotate);

            RectTransform rect = GetOrAddComponent<RectTransform>(txtObj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            return txt;
        }
#endif

        public static Button AddButtonObject(string name, Transform parent, Font font, int fontSize,
            Color color, TextAnchor anchor, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, float lineSpacing)
        {
            GameObject btnObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            GetOrAddComponent<Image>(btnObj);
            GetOrAddComponent<Button>(btnObj);
            Text txt = AddTextObject("Text", btnObj.transform, font, color, TextAnchor.MiddleCenter,
                    new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f),
                    sizeDelta, fontSize, lineSpacing);
            txt.rectTransform.offsetMin = Vector2.zero;
            txt.rectTransform.offsetMax = Vector2.zero;
            return btnObj.GetComponent<Button>();
        }

#if dUI_TextMeshPro
        public static Button AddTMPButtonObject(string name, Transform parent, TMP_FontAsset font, int fontSize,
            Color color, TextAlignmentOptions anchor, Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot,
            Vector2 sizeDelta, float lineSpacing)
        {
            GameObject btnObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            GetOrAddComponent<Image>(btnObj);
            GetOrAddComponent<Button>(btnObj);
            var txt = AddTMPTextObject("Text", btnObj.transform, font, color, anchor,
                    new Vector2(0, 0), new Vector2(1, 1), new Vector2(0.5f, 0.5f),
                    sizeDelta, fontSize, lineSpacing);
            txt.rectTransform.offsetMin = Vector2.zero;
            txt.rectTransform.offsetMax = Vector2.zero;
            return btnObj.GetComponent<Button>();
        }
#endif

        internal static GameObject AddTooltipContent(string name, Transform parent, TextStyle textStyle,
            ChartTheme theme)
        {
            var anchorMax = new Vector2(0, 1);
            var anchorMin = new Vector2(0, 1);
            var pivot = new Vector2(0, 1);
            var sizeDelta = new Vector2(100, 100);
            GameObject tooltipObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var img = GetOrAddComponent<Image>(tooltipObj);
            img.color = Color.black;
            var txt = AddTextObject("Text", tooltipObj.transform, anchorMin, anchorMax, pivot, sizeDelta,
                textStyle, theme.tooltip);
            txt.SetAlignment(textStyle.GetAlignment(TextAnchor.UpperLeft));
            txt.SetText("Text");
            txt.SetLocalPosition(new Vector2(3, -3));
            tooltipObj.transform.localPosition = Vector3.zero;
            return tooltipObj;
        }

        internal static Painter AddPainterObject(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, HideFlags hideFlags, int siblingIndex)
        {
            var painterObj = ChartHelper.AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            painterObj.hideFlags = hideFlags;
            painterObj.transform.SetSiblingIndex(siblingIndex);
            return ChartHelper.GetOrAddComponent<Painter>(painterObj);
        }

        public static Image AddIcon(string name, Transform parent, float width, float height, Sprite sprite = null,
            Image.Type type = Image.Type.Simple)
        {
            var anchorMax = new Vector2(0.5f, 0.5f);
            var anchorMin = new Vector2(0.5f, 0.5f);
            var pivot = new Vector2(0.5f, 0.5f);
            var sizeDelta = new Vector2(width, height);
            GameObject iconObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var img = GetOrAddComponent<Image>(iconObj);
            img.raycastTarget = false;
            img.type = type;
            if (sprite != null)
            {
                img.sprite = sprite;
                if (width == 0 || height == 0)
                {
                    img.SetNativeSize();
                }
            }
            return img;
        }

        public static ChartLabel AddAxisLabelObject(int total, int index, string name, Transform parent,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta, Axis axis, ComponentTheme theme,
            string content)
        {
            var textStyle = axis.axisLabel.textStyle;
            var iconStyle = axis.iconStyle;
            var label = new ChartLabel();
            label.gameObject = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);

            // TODO: 为了兼容旧版本，这里后面版本可以去掉
            var oldText = label.gameObject.GetComponent<Text>();
            if (oldText != null)
            {
                GameObject.DestroyImmediate(oldText);
            }
            var labelShow = axis.axisLabel.show && (axis.axisLabel.interval == 0 || index % (axis.axisLabel.interval + 1) == 0);
            if (labelShow)
            {
                if (!axis.axisLabel.showStartLabel && index == 0) labelShow = false;
                else if (!axis.axisLabel.showEndLabel && index == total - 1) labelShow = false;
            }
            label.label = AddTextObject("Text", label.gameObject.transform, anchorMin, anchorMax, pivot, sizeDelta, textStyle, theme);
            label.icon = ChartHelper.AddIcon("Icon", label.gameObject.transform, iconStyle.width, iconStyle.height);
            label.SetAutoSize(false);
            label.UpdateIcon(iconStyle, axis.GetIcon(index));
            label.label.SetActive(labelShow);
            label.SetText(content);
            return label;
        }

        internal static GameObject AddSerieLabel(string name, Transform parent, float width, float height,
            Color color, TextStyle textStyle, ChartTheme theme)
        {
            var anchorMin = new Vector2(0.5f, 0.5f);
            var anchorMax = new Vector2(0.5f, 0.5f);
            var pivot = new Vector2(0.5f, 0.5f);
            var sizeDelta = (width != 0 && height != 0)
                ? new Vector2(width, height)
                : new Vector2(50, textStyle.GetFontSize(theme.common) + 2);
            var labelObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var txt = AddTextObject("Text", labelObj.transform, anchorMin, anchorMax, pivot, sizeDelta, textStyle,
                theme.common);
            txt.SetColor(color);
            txt.SetAlignment(textStyle.alignment);
            txt.SetText("Text");
            txt.SetLocalPosition(new Vector2(0, 0));
            txt.SetLocalEulerAngles(Vector3.zero);
            labelObj.transform.localPosition = Vector3.zero;
            labelObj.transform.localEulerAngles = new Vector3(0, 0, textStyle.rotate);
            return labelObj;
        }

        internal static GameObject AddTooltipLabel(string name, Transform parent, ChartTheme theme, Vector2 pivot)
        {
            var anchorMax = new Vector2(0, 0);
            var anchorMin = new Vector2(0, 0);
            var sizeDelta = new Vector2(100, 50);
            return AddTooltipLabel(name, parent, theme, pivot, anchorMin, anchorMax, sizeDelta);
        }

        internal static GameObject AddTooltipLabel(string name, Transform parent, ChartTheme theme, Vector2 pivot,
            Vector2 anchorMin, Vector2 anchorMax, Vector2 sizeDelta)
        {
            GameObject labelObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            labelObj.transform.localPosition = Vector3.zero;
            var img = GetOrAddComponent<Image>(labelObj);
            img.color = Color.black;
#if dUI_TextMeshPro
            var alignment = TextAlignmentOptions.Center;
            var txt = AddTMPTextObject("Text", labelObj.transform, theme.tmpFont, Color.white, alignment,
                    new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 1), sizeDelta, 16);
#else
            var txt = AddTextObject("Text", labelObj.transform, theme.font, Color.white, TextAnchor.MiddleCenter,
                    new Vector2(0, 0), new Vector2(1, 1), new Vector2(1, 1), sizeDelta, 16);
#endif
            txt.GetComponent<RectTransform>().offsetMin = Vector2.zero;
            txt.GetComponent<RectTransform>().offsetMax = Vector2.zero;
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
            if (posList.Count < 2)
            {
                posList.Clear();
                posList.Add(sp);
                posList.Add(ep);
            }
        }

        public static void GetBezierListVertical(ref List<Vector3> posList, Vector3 sp, Vector3 ep,
            float smoothness = 2f, float k = 2.0f)
        {
            Vector3 dir = (ep - sp).normalized;
            float dist = Vector3.Distance(sp, ep);
            Vector3 cp1 = sp + dist / k * dir * 1;
            Vector3 cp2 = sp + dist / k * dir * (k - 1);
            cp1.x = sp.x;
            cp2.x = ep.x;
            int segment = (int)(dist / (smoothness <= 0 ? 2f : smoothness));
            GetBezierList2(ref posList, sp, ep, segment, cp1, cp2);
            if (posList.Count < 2)
            {
                posList.Clear();
                posList.Add(sp);
                posList.Add(ep);
            }
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

        public static bool IsValueEqualsColor(Color color1, Color color2)
        {
            return color1.a == color2.a &&
                color1.b == color2.b &&
                color1.g == color2.g &&
                color1.r == color2.r;
        }

        public static bool IsValueEqualsString(string str1, string str2)
        {
            if (str1 == null && str2 == null) return true;
            else if (str1 != null && str2 != null) return str1.Equals(str2);
            else return false;
        }

        public static bool IsValueEqualsVector2(Vector2 v1, Vector2 v2)
        {
            return v1.x == v2.x && v1.y == v2.y;
        }

        public static bool IsValueEqualsVector3(Vector3 v1, Vector3 v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        public static bool IsValueEqualsList<T>(List<T> list1, List<T> list2)
        {
            if (list1 == null || list2 == null) return false;
            if (list1.Count != list2.Count) return false;
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] == null && list2[i] == null) { }
                else
                {
                    if (list1[i] != null)
                    {
                        if (!list1[i].Equals(list2[i])) return false;
                    }
                    else
                    {
                        if (!list2[i].Equals(list1[i])) return false;
                    }
                }
            }
            return true;
        }

        public static bool IsClearColor(Color32 color)
        {
            return color.a == 0 && color.b == 0 && color.g == 0 && color.r == 0;
        }

        public static bool IsClearColor(Color color)
        {
            return color.a == 0 && color.b == 0 && color.g == 0 && color.r == 0;
        }

        public static bool IsZeroVector(Vector3 pos)
        {
            return pos.x == 0 && pos.y == 0 && pos.z == 0;
        }

        public static bool CopyList<T>(List<T> toList, List<T> fromList)
        {
            if (toList == null || fromList == null) return false;
            toList.Clear();
            foreach (var item in fromList) toList.Add(item);
            return true;
        }
        public static bool CopyArray<T>(T[] toList, T[] fromList)
        {
            if (toList == null || fromList == null) return false;
            if (toList.Length != fromList.Length)
            {
                toList = new T[fromList.Length];
            }
            for (int i = 0; i < fromList.Length; i++) toList[i] = fromList[i];
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

        public static double GetMaxDivisibleValue(double max, int ceilRate)
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
            if (ceilRate == 0)
            {
                var bigger = Math.Ceiling(Math.Abs(max));
                int n = 1;
                while (bigger / (Mathf.Pow(10, n)) > 10)
                {
                    n++;
                }
                double mm = bigger;
                if (mm > 10 && n < 38)
                {
                    mm = bigger - bigger % (Mathf.Pow(10, n));
                    mm += max > 0 ? Mathf.Pow(10, n) : -Mathf.Pow(10, n);
                }
                if (max < 0) return -Math.Ceiling(mm);
                else return Math.Ceiling(mm);
            }
            else
            {
                var mod = max % ceilRate;
                int rate = (int)(max / ceilRate);
                return mod == 0 ? max : (max < 0 ? rate : rate + 1) * ceilRate;
            }
        }

        public static double GetMinDivisibleValue(double min, int ceilRate)
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
            if (ceilRate == 0)
            {
                var bigger = Math.Floor(Math.Abs(min));
                int n = 1;
                while (bigger / (Mathf.Pow(10, n)) > 10)
                {
                    n++;
                }
                double mm = bigger;
                if (mm > 10 && n < 38)
                {
                    mm = bigger - bigger % (Mathf.Pow(10, n));
                    mm += min < 0 ? Mathf.Pow(10, n) : -Mathf.Pow(10, n);
                }
                if (min < 0) return -Math.Floor(mm);
                else return Math.Floor(mm);
            }
            else
            {
                var mod = min % ceilRate;
                int rate = (int)(min / ceilRate);
                return mod == 0 ? min : (min < 0 ? rate - 1 : rate) * ceilRate;
            }
        }

        public static double GetMaxLogValue(double value, float logBase, bool isLogBaseE, out int splitNumber)
        {
            splitNumber = 0;
            if (value <= 0) return 0;
            double max = 0;
            while (max < value)
            {
                if (isLogBaseE)
                {
                    max = Mathf.Exp(splitNumber);
                }
                else
                {
                    max = Mathf.Pow(logBase, splitNumber);
                }
                splitNumber++;
            }
            return max;
        }

        public static float GetMinLogValue(double value, float logBase, bool isLogBaseE, out int splitNumber)
        {
            splitNumber = 0;
            if (value > 1) return 1;
            float min = 1;
            while (min > value)
            {
                if (isLogBaseE)
                {
                    min = Mathf.Exp(-splitNumber);
                }
                else
                {
                    min = Mathf.Pow(logBase, -splitNumber);
                }
                splitNumber++;
            }
            return min;
        }

        public static int GetFloatAccuracy(double value)
        {
            if (value > 1 || value < -1) return 0;
            int count = 1;
            int intvalue = (int)(value * Mathf.Pow(10, count));
            while (intvalue == 0 && count < 38)
            {
                count++;
                intvalue = (int)(value * Mathf.Pow(10, count));
            }
            if (count == 38 && (value == 0 || value == 1)) return 1;
            else return count;
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

        public static Vector3 GetPosition(Vector3 center, float angle, float radius)
        {
            var rad = angle * Mathf.Deg2Rad;
            var px = Mathf.Sin(rad) * radius;
            var py = Mathf.Cos(rad) * radius;
            return center + new Vector3(px, py);
        }

        /// <summary>
        /// 获得0-360的角度（12点钟方向为0度）
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static float GetAngle360(Vector2 from, Vector2 to)
        {
            float angle;

            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);
            angle = cross.z > 0 ? -angle : angle;
            angle = (angle + 360) % 360;
            return angle;
        }

        public static Vector3 GetPos(Vector3 center, float radius, float angle, bool isDegree = false)
        {
            angle = isDegree ? angle * Mathf.Deg2Rad : angle;
            return new Vector3(center.x + radius * Mathf.Sin(angle), center.y + radius * Mathf.Cos(angle));
        }

        public static Vector3 GetDire(float angle, bool isDegree = false)
        {
            angle = isDegree ? angle * Mathf.Deg2Rad : angle;
            return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle));
        }

        public static Vector3 GetVertialDire(Vector3 dire)
        {
            if (dire.x == 0)
            {
                return new Vector3(-1, 0, 0);
            }
            if (dire.y == 0)
            {
                return new Vector3(0, -1, 0);
            }
            else
            {
                return new Vector3(-dire.y / dire.x, 1, 0).normalized;
            }
        }

        public static float GetRuntimeRelativeOrAbsoluteValue(float check, float total)
        {
            if (check <= 0)
            {
                return 0;
            }
            else if (check <= 1)
            {
                return total * check;
            }
            else
            {
                return check;
            }
        }

        public static Vector3 GetLastPoint(List<Vector3> list)
        {
            if (list.Count <= 0) return Vector3.zero;
            else return list[list.Count - 1];
        }

        public static void SetColorOpacity(ref Color32 color, float opacity)
        {
            if (color.a != 0 && opacity != 1)
            {
                color.a = (byte)(color.a * opacity);
            }
        }

        public static Color32 GetHighlightColor(Color32 color, float rate = 0.8f)
        {
            var newColor = color;
            newColor.r = (byte)(color.r * rate);
            newColor.g = (byte)(color.g * rate);
            newColor.b = (byte)(color.b * rate);
            return newColor;
        }

        public static bool IsPointInQuadrilateral(Vector3 P, Vector3 A, Vector3 B, Vector3 C, Vector3 D)
        {
            Vector3 v0 = Vector3.Cross(A - D, P - D);
            Vector3 v1 = Vector3.Cross(B - A, P - A);
            Vector3 v2 = Vector3.Cross(C - B, P - B);
            Vector3 v3 = Vector3.Cross(D - C, P - C);
            if (Vector3.Dot(v0, v1) < 0 || Vector3.Dot(v0, v2) < 0 || Vector3.Dot(v0, v3) < 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool IsInRect(Vector3 pos, float xMin, float xMax, float yMin, float yMax)
        {
            return pos.x >= xMin && pos.x <= xMax && pos.y <= yMax && pos.y >= yMin;
        }
    }
}