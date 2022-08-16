using System;
using System.Collections.Generic;
using System.Text;
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

namespace XCharts.Runtime
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
            if (gameObject == null) return;
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
            if (transform == null) return;
            if (active) transform.localScale = Vector3.one;
            else transform.localScale = Vector3.zero;
        }
        public static void HideAllObject(GameObject obj, string match = null)
        {
            if (obj == null) return;
            HideAllObject(obj.transform, match);
        }

        public static void HideAllObject(Transform parent, string match = null)
        {
            if (parent == null) return;
            ActiveAllObject(parent, false, match);
        }

        public static void ActiveAllObject(Transform parent, bool active, string match = null)
        {
            if (parent == null) return;
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
                    GameObject.DestroyImmediate(go.gameObject, true);
                }
            }
        }

        public static void DestoryGameObject(Transform parent, string childName)
        {
            if (parent == null) return;
            var go = parent.Find(childName);
            if (go != null)
            {
                GameObject.DestroyImmediate(go.gameObject, true);
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
                    GameObject.DestroyImmediate(go.gameObject, true);
                }
            }
        }

        public static void DestoryGameObject(GameObject go)
        {
            if (go != null) GameObject.DestroyImmediate(go, true);
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
                    GameObject.DestroyImmediate(component as GameObject, true);
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
                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
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
                obj.transform.localRotation = Quaternion.Euler(0, 0, 0);
                obj.layer = parent.gameObject.layer;
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

        public static ChartText AddTextObject(string objectName, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, TextStyle textStyle, ComponentTheme theme, Color autoColor,
            TextAnchor autoAlignment, ChartText chartText = null)
        {
            GameObject txtObj = AddObject(objectName, parent, anchorMin, anchorMax, pivot, sizeDelta);
            txtObj.transform.localEulerAngles = new Vector3(0, 0, textStyle.rotate);
            txtObj.layer = parent.gameObject.layer;
            if (chartText == null)
                chartText = new ChartText();
#if dUI_TextMeshPro
            RemoveComponent<Text>(txtObj);
            chartText.tmpText = GetOrAddComponent<TextMeshProUGUI>(txtObj);
            chartText.tmpText.font = textStyle.tmpFont == null ? theme.tmpFont : textStyle.tmpFont;
            chartText.tmpText.fontStyle = textStyle.tmpFontStyle;
            chartText.tmpText.richText = true;
            chartText.tmpText.raycastTarget = false;
            chartText.tmpText.enableWordWrapping = textStyle.autoWrap;
#else
            chartText.text = GetOrAddComponent<Text>(txtObj);
            chartText.text.font = textStyle.font == null ? theme.font : textStyle.font;
            chartText.text.fontStyle = textStyle.fontStyle;
            chartText.text.horizontalOverflow = textStyle.autoWrap ? HorizontalWrapMode.Wrap : HorizontalWrapMode.Overflow;
            chartText.text.verticalOverflow = VerticalWrapMode.Overflow;
            chartText.text.supportRichText = true;
            chartText.text.raycastTarget = false;
#endif
            if (textStyle.autoColor && autoColor != Color.clear)
                chartText.SetColor(autoColor);
            else
                chartText.SetColor(textStyle.GetColor(theme.textColor));

            chartText.SetAlignment(textStyle.autoAlign ? autoAlignment : textStyle.alignment);
            chartText.SetFontSize(textStyle.GetFontSize(theme));
            chartText.SetText("Text");
            chartText.SetLineSpacing(textStyle.lineSpacing);
            chartText.SetActive(textStyle.show);

            RectTransform rect = GetOrAddComponent<RectTransform>(txtObj);
            rect.localPosition = Vector3.zero;
            rect.sizeDelta = sizeDelta;
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = pivot;
            return chartText;
        }

        internal static Painter AddPainterObject(string name, Transform parent, Vector2 anchorMin, Vector2 anchorMax,
            Vector2 pivot, Vector2 sizeDelta, HideFlags hideFlags, int siblingIndex)
        {
            var painterObj = ChartHelper.AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            painterObj.hideFlags = hideFlags;
            painterObj.transform.SetSiblingIndex(siblingIndex);
            return ChartHelper.GetOrAddComponent<Painter>(painterObj);
        }

        public static Image AddIcon(string name, Transform parent, IconStyle iconStyle)
        {
            return AddIcon(name, parent, iconStyle.width, iconStyle.height, iconStyle.sprite, iconStyle.type);
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
            if (img.raycastTarget != false)
                img.raycastTarget = false;
            if (img.type != type)
                img.type = type;
            if (sprite != null && img.sprite != sprite)
            {
                img.sprite = sprite;
                if (width == 0 || height == 0)
                {
                    img.SetNativeSize();
                }
            }
            return img;
        }

        public static void SetBackground(Image background, ImageStyle imageStyle)
        {
            if (background == null) return;
            if (imageStyle.show)
            {
                background.sprite = imageStyle.sprite;
                background.color = imageStyle.color;
                background.type = imageStyle.type;
            }
            else
            {
                background.sprite = null;
                background.color = Color.clear;
            }
        }

        public static ChartLabel AddAxisLabelObject(int total, int index, string name, Transform parent,
            Vector2 sizeDelta, Axis axis, ComponentTheme theme,
            string content, Color autoColor, TextAnchor autoAlignment = TextAnchor.MiddleCenter)
        {
            var textStyle = axis.axisLabel.textStyle;
            var label = AddChartLabel(name, parent, axis.axisLabel, theme, content, autoColor, autoAlignment);
            var labelShow = axis.IsNeedShowLabel(index, total);
            label.UpdateIcon(axis.axisLabel.icon, axis.GetIcon(index));
            label.text.SetActive(labelShow);
            return label;
        }

        public static ChartLabel AddChartLabel(string name, Transform parent, LabelStyle labelStyle,
            ComponentTheme theme, string content, Color autoColor, TextAnchor autoAlignment = TextAnchor.MiddleCenter)
        {
            Vector2 anchorMin, anchorMax, pivot;
            var sizeDelta = new Vector2(labelStyle.width, labelStyle.height);
            var textStyle = labelStyle.textStyle;
            var alignment = textStyle.GetAlignment(autoAlignment);
            UpdateAnchorAndPivotByTextAlignment(alignment, out anchorMin, out anchorMax, out pivot);
            var labelObj = AddObject(name, parent, anchorMin, anchorMax, pivot, sizeDelta);
            var label = GetOrAddComponent<ChartLabel>(labelObj);
            label.text = AddTextObject("Text", label.gameObject.transform, anchorMin, anchorMax, pivot,
                sizeDelta, textStyle, theme, autoColor, autoAlignment, label.text);
            label.icon = ChartHelper.AddIcon("Icon", label.gameObject.transform, labelStyle.icon);
            label.SetSize(labelStyle.width, labelStyle.height);
            label.SetTextPadding(labelStyle.textPadding);
            label.SetText(content);
            label.UpdateIcon(labelStyle.icon);
            if (labelStyle.background.show)
            {
                label.color = (!labelStyle.background.autoColor || autoColor == Color.clear) ?
                    labelStyle.background.color : autoColor;
                label.sprite = labelStyle.background.sprite;
                label.type = labelStyle.background.type;
            }
            else
            {
                label.color = Color.clear;
                label.sprite = null;
            }
            label.transform.localEulerAngles = new Vector3(0, 0, labelStyle.rotate);
            label.transform.localPosition = labelStyle.offset;
            return label;
        }

        private static void UpdateAnchorAndPivotByTextAlignment(TextAnchor alignment, out Vector2 anchorMin, out Vector2 anchorMax,
            out Vector2 pivot)
        {
            switch (alignment)
            {
                case TextAnchor.LowerLeft:
                    anchorMin = new Vector2(0f, 0f);
                    anchorMax = new Vector2(0f, 0f);
                    pivot = new Vector2(0f, 0f);
                    break;
                case TextAnchor.UpperLeft:
                    anchorMin = new Vector2(0f, 1f);
                    anchorMax = new Vector2(0f, 1f);
                    pivot = new Vector2(0f, 1f);
                    break;
                case TextAnchor.MiddleLeft:
                    anchorMin = new Vector2(0f, 0.5f);
                    anchorMax = new Vector2(0f, 0.5f);
                    pivot = new Vector2(0f, 0.5f);
                    break;
                case TextAnchor.LowerRight:
                    anchorMin = new Vector2(1f, 0f);
                    anchorMax = new Vector2(1f, 0f);
                    pivot = new Vector2(1f, 0f);
                    break;
                case TextAnchor.UpperRight:
                    anchorMin = new Vector2(1f, 1f);
                    anchorMax = new Vector2(1f, 1f);
                    pivot = new Vector2(1f, 1f);
                    break;
                case TextAnchor.MiddleRight:
                    anchorMin = new Vector2(1, 0.5f);
                    anchorMax = new Vector2(1, 0.5f);
                    pivot = new Vector2(1, 0.5f);
                    break;
                case TextAnchor.LowerCenter:
                    anchorMin = new Vector2(0.5f, 0f);
                    anchorMax = new Vector2(0.5f, 0f);
                    pivot = new Vector2(0.5f, 0f);
                    break;
                case TextAnchor.UpperCenter:
                    anchorMin = new Vector2(0.5f, 1f);
                    anchorMax = new Vector2(0.5f, 1f);
                    pivot = new Vector2(0.5f, 1f);
                    break;
                case TextAnchor.MiddleCenter:
                    anchorMin = new Vector2(0.5f, 0.5f);
                    anchorMax = new Vector2(0.5f, 0.5f);
                    pivot = new Vector2(0.5f, 0.5f);
                    break;
                default:
                    anchorMin = new Vector2(0.5f, 0.5f);
                    anchorMax = new Vector2(0.5f, 0.5f);
                    pivot = new Vector2(0.5f, 0.5f);
                    break;
            }
        }

        internal static ChartLabel AddTooltipIndicatorLabel(Tooltip tooltip, string name, Transform parent,
            ThemeStyle theme, TextAnchor alignment)
        {
            var label = ChartHelper.AddChartLabel(name, parent, tooltip.indicatorLabelStyle, theme.tooltip,
                "", Color.clear, alignment);
            label.SetActive(tooltip.show && tooltip.indicatorLabelStyle.show);
            return label;
        }

        public static void GetPointList(ref List<Vector3> posList, Vector3 sp, Vector3 ep, float k = 30f)
        {
            Vector3 dir = (ep - sp).normalized;
            float dist = Vector3.Distance(sp, ep);
            int segment = (int) (dist / k);
            posList.Clear();
            posList.Add(sp);
            for (int i = 1; i < segment; i++)
            {
                posList.Add(sp + dir * dist * i / segment);
            }
            posList.Add(ep);
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

        public static bool IsEquals(double d1, double d2)
        {
            return Math.Abs(d1 - d2) < 0.000001d;
        }

        public static bool IsEquals(float d1, float d2)
        {
            return Math.Abs(d1 - d2) < 0.000001f;
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
            return (Color32) color;
        }

        public static double GetMaxDivisibleValue(double max, double ceilRate)
        {
            if (max == 0) return 0;
            if (max > -1 && max < 1)
            {
                int count = 1;
                int intvalue = (int) (max * Mathf.Pow(10, count));
                while (intvalue == 0 && count < 12)
                {
                    count++;
                    intvalue = (int) (max * Mathf.Pow(10, count));
                }
                var pow = Mathf.Pow(10, count);
                if (max > 0) return (int) ((max * pow + 1)) / pow;
                else return (int) ((max * pow - 1)) / pow;
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
                var mmm = mm - Mathf.Pow(10, n) / 2;
                if (max < 0) return -Math.Ceiling(mmm > -max ? mmm : mm);
                else return Math.Ceiling(mmm > max ? mmm : mm);
            }
            else
            {
                var mod = max % ceilRate;
                int rate = (int) (max / ceilRate);
                return mod == 0 ? max : (max < 0 ? rate : rate + 1) * ceilRate;
            }
        }

        public static double GetMinDivisibleValue(double min, double ceilRate)
        {
            if (min == 0) return 0;
            if (min > -1 && min < 1)
            {
                int count = 1;
                int intvalue = (int) (min * Mathf.Pow(10, count));
                while (intvalue == 0 && count < 12)
                {
                    count++;
                    intvalue = (int) (min * Mathf.Pow(10, count));
                }
                var pow = Mathf.Pow(10, count);
                if (min > 0) return (int) ((min * pow + 1)) / pow;
                else return (int) ((min * pow - 1)) / pow;
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
                int rate = (int) (min / ceilRate);
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
                    max = Math.Exp(splitNumber);
                }
                else
                {
                    max = Math.Pow(logBase, splitNumber);
                }
                splitNumber++;
            }
            return max;
        }

        public static double GetMinLogValue(double value, float logBase, bool isLogBaseE, out int splitNumber)
        {
            splitNumber = 0;
            if (value > 1) return 1;
            double min = 1;
            while (min > value)
            {
                if (isLogBaseE)
                {
                    min = Math.Exp(-splitNumber);
                }
                else
                {
                    min = Math.Pow(logBase, -splitNumber);
                }
                splitNumber++;
            }
            return min;
        }

        public static int GetFloatAccuracy(double value)
        {
            if (value > 1 || value < -1) return 0;
            int count = 1;
            int intvalue = (int) (value * Mathf.Pow(10, count));
            while (intvalue == 0 && count < 38)
            {
                count++;
                intvalue = (int) (value * Mathf.Pow(10, count));
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

        public static Vector3 GetLastValue(List<Vector3> list)
        {
            if (list.Count <= 0) return Vector3.zero;
            else return list[list.Count - 1];
        }

        public static void SetColorOpacity(ref Color32 color, float opacity)
        {
            if (color.a != 0 && opacity != 1)
            {
                color.a = (byte) (color.a * opacity);
            }
        }

        public static Color32 GetHighlightColor(Color32 color, float rate = 0.8f)
        {
            var newColor = color;
            newColor.r = (byte) (color.r * rate);
            newColor.g = (byte) (color.g * rate);
            newColor.b = (byte) (color.b * rate);
            return newColor;
        }

        public static Color32 GetBlurColor(Color32 color, float a = 0.3f)
        {
            var newColor = color;
            newColor.a = (byte) (a * 255);
            return newColor;
        }

        public static Color32 GetSelectColor(Color32 color, float rate = 0.8f)
        {
            var newColor = color;
            newColor.r = (byte) (color.r * rate);
            newColor.g = (byte) (color.g * rate);
            newColor.b = (byte) (color.b * rate);
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

        public static bool IsColorAlphaZero(Color color)
        {
            return !ChartHelper.IsClearColor(color) && color.a == 0;
        }

        public static float GetActualValue(float valueOrRate, float total, float maxRate = 1.5f)
        {
            if (valueOrRate >= -maxRate && valueOrRate <= maxRate) return valueOrRate * total;
            else return valueOrRate;
        }
    }
}