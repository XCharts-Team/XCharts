using System.Collections.Generic;
using UnityEngine;

namespace XCharts.Runtime
{
    public static class SerieLabelPool
    {
        private static readonly Stack<GameObject> m_Stack = new Stack<GameObject>(200);
        private static Dictionary<int, bool> m_ReleaseDic = new Dictionary<int, bool>(1000);

        public static GameObject Get(string name, Transform parent, LabelStyle label, Color color,
            float iconWidth, float iconHeight, ThemeStyle theme)
        {
            GameObject element;
            if (m_Stack.Count == 0 || !Application.isPlaying)
            {
                element = CreateSerieLabel(name, parent, label, color, iconWidth, iconHeight, theme);
            }
            else
            {
                element = m_Stack.Pop();
                if (element == null)
                {
                    element = CreateSerieLabel(name, parent, label, color, iconWidth, iconHeight, theme);
                }
                m_ReleaseDic.Remove(element.GetInstanceID());
                element.name = name;
                element.transform.SetParent(parent);
                var text = new ChartText(element);
                text.SetColor(color);
                text.SetFontAndSizeAndStyle(label.textStyle, theme.common);
                ChartHelper.SetActive(element, true);
            }
            element.transform.localEulerAngles = new Vector3(0, 0, label.rotate);
            return element;
        }

        public static void Release(GameObject element)
        {
            if (element == null) return;
            ChartHelper.SetActive(element, false);
            if (!Application.isPlaying) return;
            if (!m_ReleaseDic.ContainsKey(element.GetInstanceID()))
            {
                m_Stack.Push(element);
                m_ReleaseDic.Add(element.GetInstanceID(), true);
            }
        }

        public static void ReleaseAll(Transform parent)
        {
            int count = parent.childCount;
            for (int i = 0; i < count; i++)
            {
                Release(parent.GetChild(i).gameObject);
            }
        }

        public static void ClearAll()
        {
            m_Stack.Clear();
            m_ReleaseDic.Clear();
        }

        private static GameObject CreateSerieLabel(string name, Transform parent, LabelStyle labelStyle, Color color,
            float iconWidth, float iconHeight, ThemeStyle theme)
        {
            var label = ChartHelper.AddChartLabel(name, parent, labelStyle, theme.common,
                "", color, TextAnchor.MiddleCenter);
            label.SetActive(labelStyle.show);
            return label.gameObject;
        }
    }
}