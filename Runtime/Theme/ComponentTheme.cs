using System;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    [Serializable]
    public class ComponentTheme : ChildComponent
    {
        [SerializeField] protected Font m_Font;
        [SerializeField] protected Color m_TextColor;
        [SerializeField] protected Color m_TextBackgroundColor;
        [SerializeField] protected int m_FontSize = 18;
#if dUI_TextMeshPro
        [SerializeField] protected TMP_FontAsset m_TMPFont;
#endif

        /// <summary>
        /// the font of text.
        /// |字体。
        /// </summary>
        public Font font
        {
            get { return m_Font; }
            set { m_Font = value; SetComponentDirty(); }
        }
        /// <summary>
        /// the color of text.
        /// |文本颜色。
        /// </summary>
        public Color textColor
        {
            get { return m_TextColor; }
            set { if (PropertyUtil.SetColor(ref m_TextColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color of text.
        /// |文本颜色。
        /// </summary>
        public Color textBackgroundColor
        {
            get { return m_TextBackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_TextBackgroundColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the font size of text.
        /// |文本字体大小。
        /// </summary>
        public int fontSize
        {
            get { return m_FontSize; }
            set { if (PropertyUtil.SetStruct(ref m_FontSize, value)) SetComponentDirty(); }
        }

#if dUI_TextMeshPro
        /// <summary>
        /// the font of chart text。
        /// |字体。
        /// </summary>
        public TMP_FontAsset tmpFont
        {
            get { return m_TMPFont; }
            set { m_TMPFont = value; SetComponentDirty(); }
        }
#endif

        public ComponentTheme(ThemeType theme)
        {
            m_FontSize = XCSettings.fontSizeLv3;
            switch (theme)
            {
                case ThemeType.Default:
                    m_TextColor = ColorUtil.GetColor("#514D4D");
                    break;
                case ThemeType.Light:
                    m_TextColor = ColorUtil.GetColor("#514D4D");
                    break;
                case ThemeType.Dark:
                    m_TextColor = ColorUtil.GetColor("#B9B8CE");
                    break;
            }
        }

        public virtual void Copy(ComponentTheme theme)
        {
            m_Font = theme.font;
            m_FontSize = theme.fontSize;
            m_TextColor = theme.textColor;
            m_TextBackgroundColor = theme.textBackgroundColor;
#if dUI_TextMeshPro
            m_TMPFont = theme.tmpFont;
#endif
        }

        public virtual void Reset(ComponentTheme defaultTheme)
        {
            Copy(defaultTheme);
        }
    }
}