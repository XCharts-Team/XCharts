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
    internal static class SerieLabelPool
    {
        private static readonly Stack<GameObject> m_Stack = new Stack<GameObject>(200);

        public static GameObject Get(string name, Transform parent, SerieLabel label, Font font, Color color, SerieData serieData)
        {
            GameObject element;
            if (m_Stack.Count == 0 || !Application.isPlaying)
            {
                element = ChartHelper.AddSerieLabel(name, parent, font,
                        color, label.backgroundColor, label.fontSize, label.fontStyle, label.rotate,
                        label.backgroundWidth, label.backgroundHeight);
                ChartHelper.AddIcon("Icon", element.transform, serieData.iconStyle.width, serieData.iconStyle.height);
            }
            else
            {
                element = m_Stack.Pop();
                element.name = name;
                element.transform.SetParent(parent);
                element.transform.localEulerAngles = new Vector3(0, 0, label.rotate);
                var text = element.GetComponentInChildren<Text>();
                text.color = color;
                text.font = font;
                text.fontSize =label.fontSize;
                text.fontStyle = label.fontStyle;
                ChartHelper.SetActive(element, true);
            }
            return element;
        }

        public static void Release(GameObject element)
        {
            ChartHelper.SetActive(element, false);
            //if (m_Stack.Count > 0 && ReferenceEquals(m_Stack.Peek(), element))
            //    Debug.LogError("Internal error. Trying to destroy object that is already released to pool." + element.name);
            if (Application.isPlaying)
                m_Stack.Push(element);
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
        }
    }
}
