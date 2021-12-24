
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public class ChartLabel : Image
    {
        private bool m_AutoHideIconWhenLabelEmpty = false;
        private bool m_LabelAutoSize = true;
        private float m_LabelPaddingLeftRight = 3f;
        private float m_LabelPaddingTopBottom = 3f;

        private ChartText m_LabelText;
        private RectTransform m_LabelRect;
        private RectTransform m_LabelBackgroundRect;
        private RectTransform m_IconRect;
        private RectTransform m_ObjectRect;
        private Vector3 m_IconOffest;
        private Align m_Align = Align.Left;
        private Image m_IconImage;
        private Image m_LabelBackgroundImage;

        public Image icon
        {
            get { return m_IconImage; }
            set { SetIcon(value); }
        }
        public Image labelBackground
        {
            get { return m_LabelBackgroundImage; }
            set { SetLabelBackground(value); }
        }
        public ChartText label
        {
            get { return m_LabelText; }
            set
            {
                m_LabelText = value;
                if (value != null) m_LabelRect = m_LabelText.gameObject.GetComponent<RectTransform>();
            }
        }

        public bool autoHideIconWhenLabelEmpty { set { m_AutoHideIconWhenLabelEmpty = value; } }
        public bool isIconActive { get; private set; }

        protected override void Awake()
        {
            m_ObjectRect = gameObject.GetComponent<RectTransform>();
            raycastTarget = false;
        }

        // protected override void OnPopulateMesh(VertexHelper vh)
        // {
        //     if (m_BackgroundColor != Color.clear || m_BackgroundImage != null)
        //     {

        //     }
        //     else
        //     {
        //         vh.Clear();
        //     }
        // }

        public void SetLabel(GameObject labelObj, bool autoSize, float paddingLeftRight, float paddingTopBottom)
        {
            m_LabelAutoSize = autoSize;
            m_LabelPaddingLeftRight = paddingLeftRight;
            m_LabelPaddingTopBottom = paddingTopBottom;
            m_LabelText = new ChartText(labelObj);
            m_LabelRect = m_LabelText.gameObject.GetComponent<RectTransform>();

            m_Align = Align.Left;
        }

        public void SetLabelBackground(Image image)
        {
            m_LabelBackgroundImage = image;
            if (image != null)
            {
                m_LabelBackgroundRect = m_LabelBackgroundImage.GetComponent<RectTransform>();
            }
        }

        public void SetIcon(Image image)
        {
            m_IconImage = image;
            if (image != null)
            {
                m_IconRect = m_IconImage.GetComponent<RectTransform>();
            }
        }

        public void SetAutoSize(bool flag)
        {
            m_LabelAutoSize = flag;
        }

        public void SetIconSprite(Sprite sprite)
        {
            if (m_IconImage != null) m_IconImage.sprite = sprite;
        }

        public void SetIconSize(float width, float height)
        {
            if (m_IconRect != null) m_IconRect.sizeDelta = new Vector3(width, height);
        }

        public void UpdateIcon(IconStyle iconStyle, Sprite sprite = null)
        {
            if (m_IconImage == null) return;
            SetIconActive(iconStyle.show);
            if (iconStyle.show)
            {
                m_IconImage.sprite = sprite == null ? iconStyle.sprite : sprite;
                m_IconImage.color = iconStyle.color;
                m_IconRect.sizeDelta = new Vector2(iconStyle.width, iconStyle.height);
                m_IconOffest = iconStyle.offset;
                m_Align = iconStyle.align;
                m_AutoHideIconWhenLabelEmpty = iconStyle.autoHideWhenLabelEmpty;
                AdjustIconPos();
                if (iconStyle.layer == IconStyle.Layer.UnderLabel)
                    m_IconRect.SetSiblingIndex(0);
                else
                    m_IconRect.SetSiblingIndex(transform.childCount - 1);
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

        public void SetTextColor(Color color)
        {
            if (m_LabelText != null) m_LabelText.SetColor(color);
        }

        public void SetLabelRotate(float rotate)
        {
            if (m_LabelText != null) m_LabelText.SetLocalEulerAngles(new Vector3(0, 0, rotate));
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public Vector3 GetPosition()
        {
            return transform.localPosition;
        }

        public void SetLabelPosition(Vector3 position)
        {
            if (m_LabelRect) m_LabelRect.localPosition = position;
        }

        public void SetActive(bool flag)
        {
            ChartHelper.SetActive(gameObject, flag);
        }
        public void SetLabelActive(bool flag)
        {
            if (m_LabelText != null) m_LabelText.SetActive(flag);
        }
        public void SetIconActive(bool flag)
        {
            isIconActive = flag;
            if (m_IconImage) ChartHelper.SetActive(m_IconImage, flag);
        }

        public bool SetText(string text)
        {
            if (m_LabelRect == null || m_LabelText == null)
                return false;

            if (text == null)
                text = "";
            if (!m_LabelText.GetText().Equals(text))
            {
                m_LabelText.SetText(text);
                if (m_LabelAutoSize)
                {
                    var newSize = string.IsNullOrEmpty(text) ? Vector2.zero :
                        new Vector2(m_LabelText.GetPreferredWidth() + m_LabelPaddingLeftRight * 2,
                            m_LabelText.GetPreferredHeight() + m_LabelPaddingTopBottom * 2);
                    var sizeChange = newSize.x != m_LabelRect.sizeDelta.x || newSize.y != m_LabelRect.sizeDelta.y;
                    if (sizeChange)
                    {
                        m_LabelRect.sizeDelta = newSize;
                        if (m_LabelBackgroundRect != null)
                            m_LabelBackgroundRect.sizeDelta = newSize;

                        AdjustIconPos();
                    }
                    return sizeChange;
                }
                AdjustIconPos();
                if (m_AutoHideIconWhenLabelEmpty && isIconActive)
                {
                    ChartHelper.SetActive(m_IconImage.gameObject, !string.IsNullOrEmpty(text));
                }
            }
            return false;
        }

        private void AdjustIconPos()
        {
            if (m_IconImage && m_IconRect && m_LabelText != null && m_ObjectRect != null)
            {
                var iconX = 0f;
                switch (m_Align)
                {
                    case Align.Left:
                        switch (m_LabelText.alignment)
                        {
                            case TextAnchor.LowerLeft:
                            case TextAnchor.UpperLeft:
                            case TextAnchor.MiddleLeft:
                                iconX = -m_ObjectRect.sizeDelta.x / 2 - m_IconRect.sizeDelta.x / 2;
                                break;
                            case TextAnchor.LowerRight:
                            case TextAnchor.UpperRight:
                            case TextAnchor.MiddleRight:
                                iconX = m_ObjectRect.sizeDelta.x / 2 - m_LabelText.GetPreferredWidth() - m_IconRect.sizeDelta.x / 2;
                                break;
                            case TextAnchor.LowerCenter:
                            case TextAnchor.UpperCenter:
                            case TextAnchor.MiddleCenter:
                                iconX = -m_LabelText.GetPreferredWidth() / 2 - m_IconRect.sizeDelta.x / 2;
                                break;
                        }
                        break;
                    case Align.Right:
                        switch (m_LabelText.alignment)
                        {
                            case TextAnchor.LowerLeft:
                            case TextAnchor.UpperLeft:
                            case TextAnchor.MiddleLeft:
                                iconX = m_ObjectRect.sizeDelta.x / 2 + m_IconRect.sizeDelta.x / 2;
                                break;
                            case TextAnchor.LowerRight:
                            case TextAnchor.UpperRight:
                            case TextAnchor.MiddleRight:
                                iconX = m_IconRect.sizeDelta.x / 2;
                                break;
                            case TextAnchor.LowerCenter:
                            case TextAnchor.UpperCenter:
                            case TextAnchor.MiddleCenter:
                                iconX = m_LabelText.GetPreferredWidth() / 2 + m_IconRect.sizeDelta.x / 2;
                                break;
                        }
                        break;
                }
                m_IconRect.anchoredPosition = m_IconOffest + new Vector3(iconX, 0);
            }
        }
    }
}
