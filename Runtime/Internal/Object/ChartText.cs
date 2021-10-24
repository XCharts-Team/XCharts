/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts
{
    public class ChartText
    {
        private Text m_Text;
        private TextGenerationSettings m_RelatedTextSettings;
        public Text text
        {
            get { return m_Text; }
            set
            {
                m_Text = value;
                if (value != null)
                {
                    m_RelatedTextSettings = m_Text.GetGenerationSettings(Vector2.zero);
                }
            }
        }
#if dUI_TextMeshPro
        private TextMeshProUGUI m_TMPText;
        public TextMeshProUGUI tmpText { get { return m_TMPText; } set { m_TMPText = value; } }
#endif
        public GameObject gameObject
        {
            get
            {
#if dUI_TextMeshPro
                if (m_TMPText != null) return m_TMPText.gameObject;
#else
                if (m_Text != null) return m_Text.gameObject;
#endif
                return null;
            }
        }

        public TextAnchor alignment
        {
            get
            {
#if dUI_TextMeshPro
            if (m_TMPText == null) return TextAnchor.MiddleCenter;
            switch (m_TMPText.alignment)
            {
                case TextAlignmentOptions.Bottom: return TextAnchor.LowerCenter; 
                case TextAlignmentOptions.BottomLeft: return TextAnchor.LowerLeft; 
                case TextAlignmentOptions.BottomRight: return TextAnchor.LowerRight;
                case TextAlignmentOptions.Center: return TextAnchor.MiddleCenter;
                case TextAlignmentOptions.Left: return TextAnchor.MiddleLeft;
                case TextAlignmentOptions.Right: return TextAnchor.MiddleRight;
                case TextAlignmentOptions.Top: return TextAnchor.UpperCenter;
                case TextAlignmentOptions.TopLeft: return TextAnchor.UpperLeft;
                case TextAlignmentOptions.TopRight: return TextAnchor.UpperRight;
                default: return TextAnchor.MiddleCenter;
            }
#else
                if (m_Text != null) return m_Text.alignment;
                else return TextAnchor.MiddleCenter;
#endif
            }
            set
            {
                SetAlignment(alignment);
            }
        }

        public ChartText()
        {
        }

        public ChartText(GameObject textParent)
        {
#if dUI_TextMeshPro
            m_TMPText = textParent.GetComponentInChildren<TextMeshProUGUI>();
            if (m_TMPText == null)
            {
                Debug.LogError("can't find TextMeshProUGUI component:" + textParent);
            }
#else
            m_Text = textParent.GetComponentInChildren<Text>();
            if (m_Text == null)
            {
                Debug.LogError("can't find Text component:" + textParent);
            }
#endif
        }

        public void SetFontSize(float fontSize)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) m_TMPText.fontSize = fontSize;
#else               
            if (m_Text != null) m_Text.fontSize = (int)fontSize;
#endif           
        }

        public void SetText(string text)
        {
            if (text == null) text = string.Empty;
            else text = text.Replace("\\n", "\n");
#if dUI_TextMeshPro
            if(m_TMPText != null) m_TMPText.text = text;
#else
            if (m_Text != null) m_Text.text = text;
#endif
        }

        public string GetText()
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) return m_TMPText.text;
#else               
            if (m_Text != null) return m_Text.text;
#endif
            return string.Empty;
        }

        public void SetColor(Color color)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) m_TMPText.color = color;
#else              
            if (m_Text != null) m_Text.color = color;
#endif
        }

        public void SetLineSpacing(float lineSpacing)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) m_TMPText.lineSpacing = lineSpacing;
#else              
            if (m_Text != null) m_Text.lineSpacing = lineSpacing;
#endif
        }

        public void SetActive(bool flag)
        {
#if dUI_TextMeshPro
            //m_TMPText.gameObject.SetActive(flag);
            if (m_TMPText != null) ChartHelper.SetActive(m_TMPText.gameObject, flag);
#else
            //m_Text.gameObject.SetActive(flag);
            if (m_Text != null) ChartHelper.SetActive(m_Text.gameObject, flag);
#endif
        }

        public void SetLocalPosition(Vector3 position)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) m_TMPText.transform.localPosition = position;
#else
            if (m_Text != null) m_Text.transform.localPosition = position;
#endif
        }
        public void SetSizeDelta(Vector2 sizeDelta)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) m_TMPText.GetComponent<RectTransform>().sizeDelta = sizeDelta;
#else
            if (m_Text != null) m_Text.GetComponent<RectTransform>().sizeDelta = sizeDelta;
#endif
        }

        public void SetLocalEulerAngles(Vector3 position)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) m_TMPText.transform.localEulerAngles = position;
#else
            if (m_Text != null) m_Text.transform.localEulerAngles = position;
#endif
        }

        public void SetAlignment(TextAnchor alignment)
        {
#if dUI_TextMeshPro
            if (m_TMPText == null) return;
            switch (alignment)
            {
                case TextAnchor.LowerCenter: m_TMPText.alignment = TextAlignmentOptions.Bottom; break;
                case TextAnchor.LowerLeft: m_TMPText.alignment = TextAlignmentOptions.BottomLeft; break;
                case TextAnchor.LowerRight: m_TMPText.alignment = TextAlignmentOptions.BottomRight; break;
                case TextAnchor.MiddleCenter: m_TMPText.alignment = TextAlignmentOptions.Center; break;
                case TextAnchor.MiddleLeft: m_TMPText.alignment = TextAlignmentOptions.Left; break;
                case TextAnchor.MiddleRight: m_TMPText.alignment = TextAlignmentOptions.Right; break;
                case TextAnchor.UpperCenter: m_TMPText.alignment = TextAlignmentOptions.Top; break;
                case TextAnchor.UpperLeft: m_TMPText.alignment = TextAlignmentOptions.TopLeft; break;
                case TextAnchor.UpperRight: m_TMPText.alignment = TextAlignmentOptions.TopRight; break;
            }
#else
            if (m_Text != null) m_Text.alignment = alignment;
#endif              
        }

        public void SetFont(Font font)
        {
            if (m_Text) m_Text.font = font;
        }

        public void SetFontStyle(FontStyle fontStyle)
        {
#if dUI_TextMeshPro
            if (m_TMPText == null) return;
            switch (fontStyle)
            {
                case FontStyle.Normal: m_TMPText.fontStyle = FontStyles.Normal; break;
                case FontStyle.Bold: m_TMPText.fontStyle = FontStyles.Bold; break;
                case FontStyle.BoldAndItalic: m_TMPText.fontStyle = FontStyles.Bold | FontStyles.Italic; break;
                case FontStyle.Italic: m_TMPText.fontStyle = FontStyles.Italic; break;
            }
#else
            if (m_Text != null) m_Text.fontStyle = fontStyle;
#endif              
        }

        public void SetFontAndSizeAndStyle(TextStyle textStyle, ComponentTheme theme)
        {
#if dUI_TextMeshPro
            if (m_TMPText == null) return;
            m_TMPText.font = textStyle.tmpFont == null ? theme.tmpFont : textStyle.tmpFont;
            m_TMPText.fontSize = textStyle.fontSize == 0 ? theme.fontSize : textStyle.fontSize;
            m_TMPText.fontStyle = textStyle.tmpFontStyle;
#else
            if (m_Text != null)
            {
                m_Text.font = textStyle.font == null ? theme.font : textStyle.font;
                m_Text.fontSize = textStyle.fontSize == 0 ? theme.fontSize : textStyle.fontSize;
                m_Text.fontStyle = textStyle.fontStyle;
            }
#endif
        }

        public float GetPreferredWidth(string content)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) return 0; // TODO:
#else
            if (m_Text != null)
            {
                var tg = m_Text.cachedTextGeneratorForLayout;
                var setting = m_Text.GetGenerationSettings(Vector2.zero);
                return tg.GetPreferredWidth(content, setting) / m_Text.pixelsPerUnit;
            }
#endif
            return 0;
        }

        public float GetPreferredWidth()
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) return m_TMPText.preferredWidth;
#else
            if (m_Text != null) return m_Text.preferredWidth;
#endif
            return 0;
        }
        public float GetPreferredHeight()
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) return m_TMPText.preferredHeight;
#else
            if (m_Text != null) return m_Text.preferredHeight;
#endif
            return 0;
        }

        public string GetPreferredText(string content, string suffix, float maxWidth)
        {
#if dUI_TextMeshPro
            if (m_TMPText != null) return content; // TODO:
#else
            if (m_Text != null)
            {
                var sourWid = GetPreferredWidth(content);
                if (sourWid < maxWidth) return content;
                var suffixWid = GetPreferredWidth(suffix);
                var textWid = maxWidth - 1.3f * suffixWid;
                for (int i = content.Length; i > 0; i--)
                {
                    var temp = content.Substring(0, i);
                    if (GetPreferredWidth(temp) < textWid)
                    {
                        return temp + suffix;
                    }
                }
            }
#endif
            return string.Empty;
        }

#if dUI_TextMeshPro

        public void SetFont(TMP_FontAsset font)
        {
            if (m_TMPText != null) m_TMPText.font = font;
        }
#endif
    }
}