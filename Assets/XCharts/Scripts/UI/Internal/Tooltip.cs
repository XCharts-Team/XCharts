using System.Xml;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    public class Tooltip
    {
        [SerializeField] private bool m_Show;
        [SerializeField] private bool m_CrossLabel;

        [NonSerialized] private GameObject m_GameObject;
        [NonSerialized] private GameObject m_Content;
        [NonSerialized] private Text m_ContentText;
        [NonSerialized] private RectTransform m_ContentRect;

        public bool show { get { return m_Show; } set { m_Show = value; SetActive(value); } }
        public bool crossLabel { get { return m_CrossLabel; } set { m_CrossLabel = value; } }

        public int dataIndex { get; set; }

        public float[] xValues { get; set; }
        public float[] yValues { get; set; }
        public int lastDataIndex { get; set; }
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
                    m_CrossLabel = false,
                    xValues = new float[2],
                    yValues = new float[2],
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

        public void UpdateToTop(){
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
            dataIndex = 0;
            xValues[0] = xValues[1] = 0;
            yValues[0] = yValues[1] = 0;
        }

        public void SetActive(bool flag)
        {
            lastDataIndex = 0;
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
    }
}