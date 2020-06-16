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
    public class LabelObject : ChartObject
    {
        private bool m_LabelAutoSize = true;
        private float m_LabelPaddingLeftRight = 3f;
        private float m_LabelPaddingTopBottom = 3f;
        private Text m_LabelText;
        private RectTransform m_LabelRect;
        private RectTransform m_IconRect;
        private RectTransform m_ObjectRect;

        private Image m_IconImage;

        public GameObject gameObject { get { return m_GameObject; } }
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
            m_ObjectRect = labelObj.GetComponent<RectTransform>();
        }

        public void SetIcon(Image image)
        {
            m_IconImage = image;
            if (image != null)
            {
                m_IconRect = m_IconImage.GetComponent<RectTransform>();
            }
        }

        public void SetIconSprite(Sprite sprite)
        {
            if (m_IconImage != null) m_IconImage.sprite = sprite;
        }

        public void SetIconSize(float width, float height)
        {
            if (m_IconRect != null) m_IconRect.sizeDelta = new Vector3(width, height);
        }

        public void UpdateIcon(IconStyle iconStyle)
        {
            if (m_IconImage == null) return;
            if (iconStyle.show)
            {
                ChartHelper.SetActive(m_IconImage.gameObject, true);
                m_IconImage.sprite = iconStyle.sprite;
                m_IconImage.color = iconStyle.color;
                m_IconRect.sizeDelta = new Vector2(iconStyle.width, iconStyle.height);
                m_IconImage.transform.localPosition = iconStyle.offset;
                if (iconStyle.layer == IconStyle.Layer.UnderLabel)
                    m_IconRect.SetSiblingIndex(0);
                else
                    m_IconRect.SetSiblingIndex(m_GameObject.transform.childCount - 1);
            }
            else
            {
                ChartHelper.SetActive(m_IconImage.gameObject, false);
            }
        }

        public float GetLabelWidth()
        {
            if (m_LabelRect) return m_LabelRect.sizeDelta.x;
            else return 0;
        }

        public float GetLabelHeight()
        {
            if (m_LabelRect) return m_LabelRect.sizeDelta.y;
            return 0;
        }

        public void SetLabelColor(Color color)
        {
            if (m_LabelText) m_LabelText.color = color;
        }

        public void SetLabelRotate(float rotate)
        {
            if (m_LabelText) m_LabelText.transform.localEulerAngles = new Vector3(0, 0, rotate);
        }

        public void SetPosition(Vector3 position)
        {
            if (m_GameObject != null)
            {
                m_GameObject.transform.localPosition = position;
            }
        }

        public void SetLabelPosition(Vector3 position)
        {
            if (m_LabelRect) m_LabelRect.localPosition = position;
        }

        public void SetActive(bool flag)
        {
            if (m_GameObject) ChartHelper.SetActive(m_GameObject, flag);
        }
        public void SetLabelActive(bool flag)
        {
            if (m_LabelText) ChartHelper.SetActive(m_LabelText, flag);
        }
        public void SetIconActive(bool flag)
        {
            if (m_IconImage) ChartHelper.SetActive(m_IconImage, flag);
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
                    if (sizeChange)
                    {
                        m_LabelRect.sizeDelta = newSize;
                        m_ObjectRect.sizeDelta = newSize;
                    }
                    return sizeChange;
                }
            }
            return false;
        }
    }
}
