using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public class LegendItem
    {
        private int m_Index;
        private string m_Name;
        private string m_LegendName;
        private GameObject m_GameObject;
        private Button m_Button;
        private Image m_Icon;
        private ChartText m_Text;
        private Image m_Background;
        private Image m_TextBackground;
        private RectTransform m_Rect;
        private RectTransform m_IconRect;
        private RectTransform m_TextRect;
        private RectTransform m_TextBackgroundRect;
        private float m_Gap = 0f;
        private float m_LabelPaddingLeftRight = 0f;
        private float m_LabelPaddingTopBottom = 0f;
        private bool m_LabelAutoSize = true;

        public int index { get { return m_Index; } set { m_Index = value; } }
        public string name { get { return m_Name; } set { m_Name = value; } }
        public string legendName { get { return m_LegendName; } set { m_LegendName = value; } }
        public GameObject gameObject { get { return m_GameObject; } }
        public Button button { get { return m_Button; } }
        public float width
        {
            get
            {
                if (m_IconRect && m_TextBackgroundRect)
                {
                    return m_IconRect.sizeDelta.x + m_Gap + m_TextBackgroundRect.sizeDelta.x;
                }
                else
                {
                    return 0;
                }
            }
        }

        public float height
        {
            get
            {
                if (m_IconRect && m_TextBackgroundRect)
                {
                    return Mathf.Max(m_IconRect.sizeDelta.y, m_TextBackgroundRect.sizeDelta.y);
                }
                else
                {
                    return m_Text.GetPreferredHeight();
                }
            }
        }

        public void SetObject(GameObject obj)
        {
            m_GameObject = obj;
            m_Button = obj.GetComponent<Button>();
            m_Rect = obj.GetComponent<RectTransform>();
            m_Icon = obj.transform.Find("icon").gameObject.GetComponent<Image>();
            m_Background = obj.GetComponent<Image>();
            m_TextBackground = obj.transform.Find("content").gameObject.GetComponent<Image>();
            m_Text = new ChartText(obj);
            m_IconRect = m_Icon.gameObject.GetComponent<RectTransform>();
            m_TextRect = m_Text.gameObject.GetComponent<RectTransform>();
            m_TextBackgroundRect = m_TextBackground.gameObject.GetComponent<RectTransform>();
        }

        public void SetButton(Button button)
        {
            m_Button = button;
        }

        public void SetIcon(Image icon)
        {
            m_Icon = icon;
        }

        public void SetText(ChartText text)
        {
            m_Text = text;
        }

        public void SetTextBackground(Image image)
        {
            m_TextBackground = image;
        }

        public void SetIconSize(float width, float height)
        {
            if (m_IconRect)
            {
                m_IconRect.sizeDelta = new Vector2(width, height);
            }
        }

        public Rect GetIconRect()
        {
            if (m_GameObject && m_IconRect)
            {
                var pos = m_GameObject.transform.localPosition;
                var sizeDelta = m_IconRect.sizeDelta;
                var y = pos.y - (m_Rect.sizeDelta.y - sizeDelta.y) / 2 - sizeDelta.y;
                return new Rect(pos.x, y, m_IconRect.sizeDelta.x, m_IconRect.sizeDelta.y);
            }
            else
            {
                return Rect.zero;
            }
        }

        public Color GetIconColor()
        {
            if (m_Icon) return m_Icon.color;
            else return Color.clear;
        }

        public void SetIconColor(Color color)
        {
            if (m_Icon)
            {
                m_Icon.color = color;
            }
        }

        public void SetIconImage(Sprite image)
        {
            if (m_Icon)
            {
                m_Icon.sprite = image;
            }
        }

        public void SetIconActive(bool active)
        {
            if (m_Icon)
            {
                m_Icon.gameObject.SetActive(active);
            }
        }

        public void SetContentColor(Color color)
        {
            if (m_Text != null)
            {
                m_Text.SetColor(color);
            }
        }

        public void SetContentBackgroundColor(Color color)
        {
            if (m_TextBackground)
            {
                m_TextBackground.color = color;
            }
        }

        public void SetContentPosition(Vector3 offset)
        {
            m_Gap = offset.x;
            if (m_TextBackgroundRect)
            {
                var posX = m_IconRect.sizeDelta.x + offset.x;
                m_TextBackgroundRect.anchoredPosition3D = new Vector3(posX, offset.y, 0);
            }
        }

        public bool SetContent(string content)
        {
            if (m_Text == null) return false;
            if (!m_Text.GetText().Equals(content))
            {
                m_Text.SetText(content);
                if (m_LabelAutoSize)
                {
                    var newSize = string.IsNullOrEmpty(content) ? Vector2.zero :
                        new Vector2(m_Text.GetPreferredWidth(), m_Text.GetPreferredHeight());
                    var sizeChange = newSize.x != m_TextRect.sizeDelta.x || newSize.y != m_TextRect.sizeDelta.y;
                    if (sizeChange)
                    {
                        m_TextRect.sizeDelta = newSize;
                        m_TextRect.anchoredPosition3D = new Vector3(m_LabelPaddingLeftRight, 0);
                        m_TextBackgroundRect.sizeDelta = new Vector2(m_Text.GetPreferredWidth() + m_LabelPaddingLeftRight * 2,
                            m_Text.GetPreferredHeight() + m_LabelPaddingTopBottom * 2 - 4);

                    }
                    m_Rect.sizeDelta = new Vector3(width, height);
                    return sizeChange;
                }
            }
            m_Rect.sizeDelta = new Vector3(width, height);
            return false;
        }

        public void SetPosition(Vector3 position)
        {
            if (m_GameObject)
            {
                m_GameObject.transform.localPosition = position;
            }
        }

        public void SetActive(bool active)
        {
            if (m_GameObject)
            {
                m_GameObject.SetActive(active);
            }
        }

        public void SetBackground(ImageStyle imageStyle)
        {
            ChartHelper.SetBackground(m_Background, imageStyle);
        }
    }
}