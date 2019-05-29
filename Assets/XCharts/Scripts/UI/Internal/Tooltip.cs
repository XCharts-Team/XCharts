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
        [NonSerialized] private GameObject m_LabelX;
        [NonSerialized] private GameObject m_LabelY;
        [NonSerialized] private Text m_ContentText;
        [NonSerialized] private Text m_LabelTextX;
        [NonSerialized] private Text m_LabelTextY;
        [NonSerialized] private RectTransform m_ContentRect;
        [NonSerialized] private RectTransform m_LabelRectX;
        [NonSerialized] private RectTransform m_LabelRectY;

        public bool show { get { return m_Show; }set { m_Show = value; } }
        public bool crossLabel { get { return m_CrossLabel; } set { m_CrossLabel = value; } }

        public int dataIndex { get; set; }
        public int lastDataIndex { get; set; }
        public Vector2 pointerPos { get; set; }
        public float width { get { return m_ContentRect.sizeDelta.x; } }
        public float height { get { return m_ContentRect.sizeDelta.y; } }
        public bool isInited { get { return m_GameObject != null; } }

        public static Tooltip defaultTooltip
        {
            get
            {
                var tooltip = new Tooltip
                {
                    m_Show = true,
                    m_CrossLabel = false
                };
                return tooltip;
            }
        }

        public void SetObj(GameObject obj)
        {
            m_GameObject = obj;
        }

        public void SetContentObj(GameObject content)
        {
            m_Content = content;
            m_ContentRect = m_Content.GetComponent<RectTransform>();
            m_ContentText = m_Content.GetComponentInChildren<Text>();
        }

        public void SetLabelObj(GameObject labelX,GameObject labelY)
        {
            m_LabelX = labelX;
            m_LabelRectX = labelX.GetComponent<RectTransform>();
            m_LabelTextX = labelX.GetComponentInChildren<Text>();
            m_LabelY = labelY;
            m_LabelRectY = labelY.GetComponent<RectTransform>();
            m_LabelTextY = labelY.GetComponentInChildren<Text>();
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

        public void SetLabelBackgroundColor(Color color)
        {
            m_LabelX.GetComponent<Image>().color = color;
            m_LabelY.GetComponent<Image>().color = color;
        }

        public void SetLabelTextColor(Color color)
        {
            m_LabelTextX.color = color;
            m_LabelTextY.color = color;
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

        public void UpdateLabelText(string labelX,string labelY)
        {
            if (m_LabelTextX)
            {
                m_LabelTextX.text = labelX;
                m_LabelRectX.sizeDelta = new Vector2(m_LabelTextX.preferredWidth + 8, 
                    m_LabelTextX.preferredHeight + 8);
            }
            if (m_LabelTextY)
            {
                m_LabelTextY.text = labelY;
                m_LabelRectY.sizeDelta = new Vector2(m_LabelTextY.preferredWidth + 8,
                    m_LabelTextY.preferredHeight + 8);
            }
        }

        public void SetActive(bool flag)
        {
            if(m_GameObject)
                m_GameObject.SetActive(flag);
        }

        public void SetLabelActive(bool flag)
        {
            if (m_LabelX && m_LabelX.activeInHierarchy != flag) m_LabelX.SetActive(flag);
            if (m_LabelY && m_LabelY.activeInHierarchy != flag) m_LabelY.SetActive(flag);
        }

        public void UpdateContentPos(Vector2 pos)
        {
            if(m_Content)
                m_Content.transform.localPosition = pos;
        }

        public void UpdateLabelPos(Vector2 xLabelPos,Vector2 yLabelPos)
        {
            if (m_LabelX)
            {
                m_LabelX.transform.localPosition = xLabelPos;
            }
            if (m_LabelY)
            {
                m_LabelY.transform.localPosition = yLabelPos;
            }
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