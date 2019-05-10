using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    public class Tooltip
    {
        [SerializeField] private bool m_Show;

        [NonSerialized] private GameObject m_GameObject;
        [NonSerialized] private Text m_Text;
        [NonSerialized] private RectTransform m_BackgroudRect;

        public bool show { get { return m_Show; }set { m_Show = value; } }
        public int dataIndex { get; set; }
        public int lastDataIndex { get; set; }
        public float width { get { return m_BackgroudRect.sizeDelta.x; } }
        public float height { get { return m_BackgroudRect.sizeDelta.y; } }

        public static Tooltip defaultTooltip
        {
            get
            {
                var tooltip = new Tooltip
                {
                    m_Show = true
                };
                return tooltip;
            }
        }

        public void SetObj(GameObject obj)
        {
            m_GameObject = obj;
            m_BackgroudRect = m_GameObject.GetComponent<RectTransform>();
            m_Text = m_GameObject.GetComponentInChildren<Text>();
        }

        public void SetBackgroundColor(Color color)
        {
            m_GameObject.GetComponent<Image>().color = color;
        }

        public void SetTextColor(Color color)
        {
            m_Text.color = color;
        }

        public void UpdateTooltipText(string txt)
        {
            m_Text.text = txt;
            m_BackgroudRect.sizeDelta = new Vector2(m_Text.preferredWidth + 8, m_Text.preferredHeight + 8);
        }

        public void SetActive(bool flag)
        {
            if(m_GameObject)
                m_GameObject.SetActive(flag);
        }

        public void UpdatePos(Vector2 pos)
        {
            if(m_GameObject)
                m_GameObject.transform.localPosition = pos;
        }

        public Vector3 GetPos()
        {
            return m_GameObject.transform.localPosition;
        }
    }
}