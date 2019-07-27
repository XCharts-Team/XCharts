using System.Collections.Generic;
using System.Xml;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    public class Tooltip
    {
        /// <summary>
        /// Type of triggering.
        /// </summary>
        public enum Trigger
        {
            /// <summary>
            /// Triggered by axes, which is mainly used for charts that have category axes, 
            /// like bar charts or line charts.
            /// </summary>
            Axis,
            /// <summary>
            /// Triggered by data item, which is mainly used for charts that don't have a 
            /// category axis like scatter charts or pie charts.
            /// </summary>
            Item
        }

        /// <summary>
        /// Indicator type.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// line indicator.
            /// </summary>
            Line,
            /// <summary>
            /// shadow crosshair indicator.
            /// </summary>
            Shadow,
            /// <summary>
            /// no indicator displayed.
            /// </summary>
            None,
            /// <summary>
            /// crosshair indicator, which is actually the shortcut of enable two axisPointers of two orthometric axes.
            /// </summary>
            Corss
        }

        [SerializeField] private bool m_Show;
        [SerializeField] private Type m_Type;
        [SerializeField] private Trigger m_Trigger;

        [NonSerialized] private GameObject m_GameObject;
        [NonSerialized] private GameObject m_Content;
        [NonSerialized] private Text m_ContentText;
        [NonSerialized] private RectTransform m_ContentRect;

        public bool show { get { return m_Show; } set { m_Show = value; SetActive(value); } }
        public Type type { get { return m_Type; } set { m_Type = value; } }
        public Trigger trigger { get { return m_Trigger; } set { m_Trigger = value; } }

        /// <summary>
        /// The data index currently indicated by Tooltip.
        /// </summary>
        public List<int> dataIndex { get; set; }
        public List<int> lastDataIndex { get; set; }
        public float[] xValues { get; set; }
        public float[] yValues { get; set; }

        public Vector2 pointerPos { get; set; }
        public float width { get { return m_ContentRect.sizeDelta.x; } }
        public float height { get { return m_ContentRect.sizeDelta.y; } }
        public bool isInited { get { return m_GameObject != null; } }
        public GameObject gameObject { get { return m_GameObject; } }

        public static Tooltip defaultTooltip
        {
            get
            {
                var tooltip = new Tooltip
                {
                    m_Show = true,
                    xValues = new float[2],
                    yValues = new float[2],
                    dataIndex = new List<int>() { -1, -1 },
                    lastDataIndex = new List<int>() { -1, -1 }
                };
                return tooltip;
            }
        }

        public void SetObj(GameObject obj)
        {
            m_GameObject = obj;
            m_GameObject.SetActive(false);
        }

        public void SetContentObj(GameObject content)
        {
            m_Content = content;
            m_ContentRect = m_Content.GetComponent<RectTransform>();
            m_ContentText = m_Content.GetComponentInChildren<Text>();
        }

        public void UpdateToTop()
        {
            int count = m_GameObject.transform.parent.childCount;
            m_GameObject.GetComponent<RectTransform>().SetSiblingIndex(count - 1);
        }

        public void SetContentBackgroundColor(Color color)
        {
            m_Content.GetComponent<Image>().color = color;
        }

        public void SetContentTextColor(Color color)
        {
            if (m_ContentText)
            {
                m_ContentText.color = color;
            }
        }

        public void UpdateContentText(string txt)
        {
            if (m_ContentText)
            {
                m_ContentText.text = txt;
                m_ContentRect.sizeDelta = new Vector2(m_ContentText.preferredWidth + 8,
                    m_ContentText.preferredHeight + 8);
            }
        }

        public void ClearValue()
        {
            dataIndex[0] = dataIndex[1] = -1;
            xValues[0] = xValues[1] = -1;
            yValues[0] = yValues[1] = -1;
        }

        public bool IsActive()
        {
            return m_GameObject != null && m_GameObject.activeInHierarchy;
        }

        public void SetActive(bool flag)
        {
            lastDataIndex[0] = lastDataIndex[1] = -1;
            if (m_GameObject && m_GameObject.activeInHierarchy != flag)
                m_GameObject.SetActive(flag);
        }

        public void UpdateContentPos(Vector2 pos)
        {
            if (m_Content)
                m_Content.transform.localPosition = pos;
        }

        public Vector3 GetContentPos()
        {
            if (m_Content)
                return m_Content.transform.localPosition;
            else
                return Vector3.zero;
        }

        public bool IsDataIndexChanged()
        {
            return dataIndex[0] != lastDataIndex[0] ||
                dataIndex[1] != lastDataIndex[1];
        }

        public void UpdateLastDataIndex()
        {
            lastDataIndex[0] = dataIndex[0];
            lastDataIndex[1] = dataIndex[1];
        }

        public bool IsSelected()
        {
            return dataIndex[0] >= 0 || dataIndex[1] >= 0;
        }

        public bool IsSelectedDataIndex(int index)
        {
            return dataIndex[0] == index || dataIndex[1] == index;
        }
    }
}