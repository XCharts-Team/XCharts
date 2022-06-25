using System;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    /// <summary>
    /// Settings related to text.
    /// |文本的相关设置。
    /// </summary>
    [Serializable]
    public class TextStyle : ChildComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Font m_Font;
        [SerializeField] private bool m_AutoWrap = false;
        [SerializeField] private bool m_AutoAlign = true;
        [SerializeField] private float m_Rotate = 0;
        [SerializeField] private bool m_AutoColor = false;
        [SerializeField] private Color m_Color = Color.clear;
        [SerializeField] private int m_FontSize = 0;
        [SerializeField] private FontStyle m_FontStyle = FontStyle.Normal;
        [SerializeField] private float m_LineSpacing = 1f;
        [SerializeField] private TextAnchor m_Alignment = TextAnchor.MiddleCenter;
#if dUI_TextMeshPro
        [SerializeField] private TMP_FontAsset m_TMPFont;
        [SerializeField] private FontStyles m_TMPFontStyle = FontStyles.Normal;
        [SerializeField] private TextAlignmentOptions m_TMPAlignment = TextAlignmentOptions.Left;
        [SerializeField][Since("v3.1.0")] private TMP_SpriteAsset m_TMPSpriteAsset;
#endif
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Rotation of text.
        /// |文本的旋转。
        /// [default: `0f`]
        /// </summary>
        public float rotate
        {
            get { return m_Rotate; }
            set { if (PropertyUtil.SetStruct(ref m_Rotate, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 是否开启自动颜色。当开启时，会自动设置颜色。
        /// </summary>
        public bool autoColor
        {
            get { return m_AutoColor; }
            set { if (PropertyUtil.SetStruct(ref m_AutoColor, value)) SetAllDirty(); }
        }
        /// <summary>
        /// the color of text.
        /// |文本的颜色。
        /// [default: `Color.clear`]
        /// </summary>
        public Color color
        {
            get { return m_Color; }
            set { if (PropertyUtil.SetColor(ref m_Color, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the font of text. When `null`, the theme's font is used by default.
        /// |文本字体。
        /// [default: null]
        /// </summary>
        public Font font
        {
            get { return m_Font; }
            set { if (PropertyUtil.SetClass(ref m_Font, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// font size.
        /// |文本字体大小。
        /// [default: 18]
        /// </summary>
        public int fontSize
        {
            get { return m_FontSize; }
            set { if (PropertyUtil.SetStruct(ref m_FontSize, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// font style.
        /// |文本字体的风格。
        /// [default: FontStyle.Normal]
        /// </summary>
        public FontStyle fontStyle
        {
            get { return m_FontStyle; }
            set { if (PropertyUtil.SetStruct(ref m_FontStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// text line spacing.
        /// |行间距。
        /// [default: 1f]
        /// </summary>
        public float lineSpacing
        {
            get { return m_LineSpacing; }
            set { if (PropertyUtil.SetStruct(ref m_LineSpacing, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 是否自动换行。
        /// </summary>
        public bool autoWrap
        {
            get { return m_AutoWrap; }
            set { if (PropertyUtil.SetStruct(ref m_AutoWrap, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 文本是否让系统自动选对齐方式。为false时才会用alignment。
        /// </summary>
        public bool autoAlign
        {
            get { return m_AutoAlign; }
            set { if (PropertyUtil.SetStruct(ref m_AutoAlign, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 对齐方式。
        /// </summary>
        public TextAnchor alignment
        {
            get { return m_Alignment; }
            set { if (PropertyUtil.SetStruct(ref m_Alignment, value)) SetComponentDirty(); }
        }
#if dUI_TextMeshPro
        /// <summary>
        /// the font of textmeshpro.
        /// |TextMeshPro字体。
        /// </summary>
        public TMP_FontAsset tmpFont
        {
            get { return m_TMPFont; }
            set { if (PropertyUtil.SetClass(ref m_TMPFont, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the font style of TextMeshPro.
        /// |TextMeshPro字体类型。
        /// </summary>
        public FontStyles tmpFontStyle
        {
            get { return m_TMPFontStyle; }
            set { if (PropertyUtil.SetStruct(ref m_TMPFontStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the text alignment of TextMeshPro.
        /// |TextMeshPro字体对齐方式。
        /// </summary>
        public TextAlignmentOptions tmpAlignment
        {
            get { return m_TMPAlignment; }
            set { if (PropertyUtil.SetStruct(ref m_TMPAlignment, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the sprite asset of TextMeshPro.
        /// |TextMeshPro的Sprite Asset。
        /// </summary>
        public TMP_SpriteAsset tmpSpriteAsset
        {
            get { return m_TMPSpriteAsset; }
            set { if (PropertyUtil.SetClass(ref m_TMPSpriteAsset, value)) SetComponentDirty(); }
        }
#endif

        public TextStyle() { }

        public TextStyle(int fontSize)
        {
            this.fontSize = fontSize;
        }

        public TextStyle(int fontSize, FontStyle fontStyle)
        {
            this.fontSize = fontSize;
            this.fontStyle = fontStyle;
        }

        public TextStyle(int fontSize, FontStyle fontStyle, Color color)
        {
            this.fontSize = fontSize;
            this.fontStyle = fontStyle;
            this.color = color;
        }

        public TextStyle(int fontSize, FontStyle fontStyle, Color color, int rorate)
        {
            this.fontSize = fontSize;
            this.fontStyle = fontStyle;
            this.color = color;
            this.rotate = rotate;
        }

        public void Copy(TextStyle textStyle)
        {
            font = textStyle.font;
            rotate = textStyle.rotate;
            color = textStyle.color;
            fontSize = textStyle.fontSize;
            fontStyle = textStyle.fontStyle;
            lineSpacing = textStyle.lineSpacing;
            alignment = textStyle.alignment;
            autoWrap = textStyle.autoWrap;
            autoAlign = textStyle.autoAlign;
#if dUI_TextMeshPro
            m_TMPFont = textStyle.tmpFont;
            m_TMPFontStyle = textStyle.tmpFontStyle;
            m_TMPSpriteAsset = textStyle.tmpSpriteAsset;
#endif
        }

        public void UpdateAlignmentByLocation(Location location)
        {
#if dUI_TextMeshPro
            m_TMPAlignment = location.runtimeTMPTextAlignment;
#else
            m_Alignment = location.runtimeTextAlignment;
#endif
        }

        public Color GetColor(Color defaultColor)
        {
            if (ChartHelper.IsClearColor(color))
                return defaultColor;
            else
                return color;
        }

        public int GetFontSize(ComponentTheme defaultTheme)
        {
            if (fontSize == 0)
                return defaultTheme.fontSize;
            else
                return fontSize;
        }

        public TextAnchor GetAlignment(TextAnchor defaultAlignment)
        {
            return m_AutoAlign ? defaultAlignment : alignment;
        }
    }
}