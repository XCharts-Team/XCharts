/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public class LabelObject
    {
        private GameObject m_GameObject;
        private bool m_LabelAutoSize = true;
        private float m_LabelPaddingLeftRight = 3f;
        private float m_LabelPaddingTopBottom = 3f;
        private Text m_LabelText;
        private RectTransform m_LabelRect;
        private Image m_IconImage;
        // private RectTransform m_IconRect;

        public Image icon { get { return m_IconImage; } }
        public Text label { get { return m_LabelText; } }

        public LabelObject()
        {
        }

        public void SetLabel(GameObject labelObj, bool autoSize, float paddingLeftRight, float paddingTopBottom)
        {
            m_GameObject = labelObj;
            m_LabelAutoSize = autoSize;
            m_LabelPaddingLeftRight = paddingLeftRight;
            m_LabelPaddingTopBottom = paddingTopBottom;
            m_LabelText = labelObj.GetComponentInChildren<Text>();
            m_LabelRect = m_LabelText.GetComponent<RectTransform>();
        }

        public void SetIcon(Image image)
        {
            m_IconImage = image;
            if (image != null)
            {
                // m_IconRect = m_IconImage.GetComponent<RectTransform>();
            }
        }

        public void SetIconSprite(Sprite sprite)
        {
            if (m_IconImage != null) m_IconImage.sprite = sprite;
        }

        public void SetIconSize(float width, float height)
        {
            if (m_LabelRect != null) m_LabelRect.sizeDelta = new Vector3(width, height);
        }

        public void SetIconActive(bool flag)
        {
            ChartHelper.SetActive(m_IconImage, flag);
        }

        public void SetPosition(Vector3 position)
        {
            if (m_GameObject != null)
            {
                m_GameObject.transform.localPosition = position;
            }
        }

        public void SetActive(bool flag)
        {
            ChartHelper.SetActive(m_GameObject, flag);
        }

        public bool SetText(string text)
        {
            if (m_LabelText && !m_LabelText.text.Equals(text))
            {
                m_LabelText.text = text;
                if (m_LabelAutoSize)
                {
                    var newSize = string.IsNullOrEmpty(text) ? Vector2.zero :
                        new Vector2(m_LabelText.preferredWidth + m_LabelPaddingLeftRight * 2,
                                        m_LabelText.preferredHeight + m_LabelPaddingTopBottom * 2);
                    var sizeChange = newSize.x != m_LabelRect.sizeDelta.x || newSize.y != m_LabelRect.sizeDelta.y;
                    if (sizeChange) m_LabelRect.sizeDelta = newSize;
                    return sizeChange;
                }
            }
            return false;
        }
    }
}
