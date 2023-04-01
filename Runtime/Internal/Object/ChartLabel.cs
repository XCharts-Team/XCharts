using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    public class ChartLabel : Image
    {
        [SerializeField] private ChartText m_LabelText;

        private bool m_HideIconIfTextEmpty = false;
        private bool m_AutoSize = true;
        private float m_PaddingLeft = 0;
        private float m_PaddingRight = 0;
        private float m_PaddingTop = 0;
        private float m_PaddingBottom = 0;
        private float m_Width = 0;
        private float m_Height = 0;
        private RectTransform m_TextRect;
        private RectTransform m_IconRect;
        private RectTransform m_ObjectRect;
        private Vector3 m_IconOffest;
        private Align m_Align = Align.Left;
        private Image m_IconImage;
        private bool m_Active = true;

        public Image icon
        {
            get { return m_IconImage; }
            set { SetIcon(value); }
        }
        public ChartText text
        {
            get { return m_LabelText; }
            set
            {
                m_LabelText = value;
                if (value != null) m_TextRect = m_LabelText.gameObject.GetComponent<RectTransform>();
            }
        }

        public bool hideIconIfTextEmpty { set { m_HideIconIfTextEmpty = value; } }
        public bool isIconActive { get; private set; }
        public bool isAnimationEnd { get; internal set; }
        public Rect rect { get; set; }

        internal RectTransform objectRect
        {
            get
            {
                if (m_ObjectRect == null)
                    m_ObjectRect = gameObject.GetComponent<RectTransform>();
                return m_ObjectRect;
            }
        }

        protected override void Awake()
        {
            raycastTarget = false;
        }

        public void SetTextPadding(TextPadding padding)
        {
            m_PaddingLeft = padding.left;
            m_PaddingRight = padding.right;
            m_PaddingTop = padding.top;
            m_PaddingBottom = padding.bottom;
            UpdatePadding();
        }
        public void SetPadding(float[] padding)
        {
            if (padding.Length >= 4)
            {
                m_PaddingLeft = padding[3];
                m_PaddingRight = padding[1];
                m_PaddingTop = padding[0];
                m_PaddingBottom = padding[2];
            }
            else if (padding.Length >= 2)
            {
                m_PaddingLeft = padding[1];
                m_PaddingRight = padding[1];
                m_PaddingTop = padding[0];
                m_PaddingBottom = padding[0];
            }
            else if (padding.Length == 1)
            {
                m_PaddingLeft = padding[0];
                m_PaddingRight = padding[0];
                m_PaddingTop = padding[0];
                m_PaddingBottom = padding[0];
            }
            UpdatePadding();
        }

        public void SetIcon(Image image)
        {
            m_IconImage = image;
            if (image != null)
            {
                m_IconRect = m_IconImage.GetComponent<RectTransform>();
            }
        }

        public float GetWidth()
        {
            return m_Width;
        }

        public float GetHeight()
        {
            return m_Height;
        }

        public void SetSize(float width, float height)
        {
            this.m_Width = width;
            this.m_Height = height;
            m_AutoSize = width == 0 && height == 0;
            objectRect.sizeDelta = new Vector2(width, height);
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
            if (m_IconImage == null || iconStyle == null)
                return;

            SetIconActive(iconStyle.show);
            if (iconStyle.show)
            {
                m_IconImage.sprite = sprite == null ? iconStyle.sprite : sprite;
                m_IconImage.color = iconStyle.color;
                m_IconImage.type = iconStyle.type;
                m_IconRect.sizeDelta = new Vector2(iconStyle.width, iconStyle.height);
                m_IconOffest = iconStyle.offset;
                m_Align = iconStyle.align;
                m_HideIconIfTextEmpty = iconStyle.autoHideWhenLabelEmpty;
                AdjustIconPos();
                if (iconStyle.layer == IconStyle.Layer.UnderText)
                    m_IconRect.SetSiblingIndex(0);
                else
                    m_IconRect.SetSiblingIndex(transform.childCount - 1);
            }
        }

        public float GetTextWidth()
        {
            if (m_TextRect) return m_TextRect.sizeDelta.x;
            else return 0;
        }

        public float GetTextHeight()
        {
            if (m_TextRect) return m_TextRect.sizeDelta.y;
            return 0;
        }

        public void SetTextColor(Color color)
        {
            if (m_LabelText != null) m_LabelText.SetColor(color);
        }

        public void SetRotate(float rotate)
        {
            transform.localEulerAngles = new Vector3(0, 0, rotate);
        }

        public void SetTextRotate(float rotate)
        {
            if (m_LabelText != null) m_LabelText.SetLocalEulerAngles(new Vector3(0, 0, rotate));
        }

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }

        public void SetRectPosition(Vector3 position)
        {
            objectRect.anchoredPosition3D = position;
        }

        public Vector3 GetPosition()
        {
            return transform.localPosition;
        }

        public override bool IsActive()
        {
            return m_Active;
        }

        public void SetActive(bool flag)
        {
            m_Active = flag;
            ChartHelper.SetActive(gameObject, flag);
        }

        public void SetTextActive(bool flag)
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
            if (m_TextRect == null || m_LabelText == null)
                return false;

            if (text == null)
                text = "";
            if (!m_LabelText.GetText().Equals(text))
            {
                m_LabelText.SetText(text);
                if (m_AutoSize)
                {
                    var newSize = string.IsNullOrEmpty(text) ? Vector2.zero :
                        new Vector2(m_LabelText.GetPreferredWidth(),
                            m_LabelText.GetPreferredHeight());
                    var sizeChange = newSize.x != m_TextRect.sizeDelta.x || newSize.y != m_TextRect.sizeDelta.y;
                    this.m_Width = newSize.x;
                    this.m_Height = newSize.y;
                    if (sizeChange)
                    {
                        m_TextRect.sizeDelta = newSize;
                        UpdateSize();
                        UpdatePadding();
                        AdjustIconPos();
                    }
                    return sizeChange;
                }
                AdjustIconPos();
                if (m_HideIconIfTextEmpty && isIconActive)
                {
                    ChartHelper.SetActive(m_IconImage.gameObject, !string.IsNullOrEmpty(text));
                }
            }
            return false;
        }

        private void UpdateSize()
        {
            if (m_AutoSize)
            {
                var sizeDelta = m_TextRect.sizeDelta;
                m_Width = sizeDelta.x + m_PaddingLeft + m_PaddingRight;
                m_Height = sizeDelta.y + m_PaddingTop + m_PaddingBottom;
                objectRect.sizeDelta = new Vector2(m_Width, m_Height);
            }
        }

        private void UpdatePadding()
        {
            if (m_TextRect == null) return;
            switch (text.alignment)
            {
                case TextAnchor.LowerLeft:
                    m_TextRect.anchoredPosition = new Vector2(m_PaddingLeft, m_PaddingBottom);
                    break;
                case TextAnchor.UpperLeft:
                    m_TextRect.anchoredPosition = new Vector2(m_PaddingLeft, -m_PaddingTop);
                    break;
                case TextAnchor.MiddleLeft:
                    m_TextRect.anchoredPosition = new Vector2(m_PaddingLeft, m_Height / 2 - m_PaddingTop - m_TextRect.sizeDelta.y / 2);
                    break;
                case TextAnchor.LowerRight:
                    m_TextRect.anchoredPosition = new Vector2(-m_PaddingRight, m_PaddingBottom);
                    break;
                case TextAnchor.UpperRight:
                    m_TextRect.anchoredPosition = new Vector2(-m_PaddingRight, -m_PaddingTop);
                    break;
                case TextAnchor.MiddleRight:
                    m_TextRect.anchoredPosition = new Vector2(-m_PaddingRight, m_Height / 2 - m_PaddingTop - m_TextRect.sizeDelta.y / 2);
                    break;
                case TextAnchor.LowerCenter:
                    m_TextRect.anchoredPosition = new Vector2(-(m_Width / 2 - m_PaddingLeft - m_TextRect.sizeDelta.x / 2), m_PaddingBottom);
                    break;
                case TextAnchor.UpperCenter:
                    m_TextRect.anchoredPosition = new Vector2(-(m_Width / 2 - m_PaddingLeft - m_TextRect.sizeDelta.x / 2), -m_PaddingTop);
                    break;
                case TextAnchor.MiddleCenter:
                    m_TextRect.anchoredPosition = new Vector2(-(m_Width / 2 - m_PaddingLeft - m_TextRect.sizeDelta.x / 2), m_Height / 2 - m_PaddingTop - m_TextRect.sizeDelta.y / 2);
                    break;
                default:
                    break;
            }
        }

        private void AdjustIconPos()
        {
            if (m_IconImage && m_IconRect && m_LabelText != null && m_TextRect != null)
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
                                iconX = -m_TextRect.sizeDelta.x / 2 - m_IconRect.sizeDelta.x / 2;
                                break;
                            case TextAnchor.LowerRight:
                            case TextAnchor.UpperRight:
                            case TextAnchor.MiddleRight:
                                iconX = m_TextRect.sizeDelta.x / 2 - m_LabelText.GetPreferredWidth() - m_IconRect.sizeDelta.x / 2;
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
                                iconX = m_TextRect.sizeDelta.x / 2 + m_IconRect.sizeDelta.x / 2;
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