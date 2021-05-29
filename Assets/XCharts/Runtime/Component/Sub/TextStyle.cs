/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts
{
    /// <summary>
    /// Settings related to text.
    /// 文本的相关设置。
    /// </summary>
    [Serializable]
    public class TextStyle : SubComponent
    {
        [SerializeField] private Font m_Font;
        [SerializeField] private bool m_AutoWrap = false;
        [SerializeField] private bool m_AutoAlign = true;
        [SerializeField] private float m_Rotate = 0;
        [SerializeField] private Vector2 m_Offset = Vector2.zero;
        [SerializeField] private Color m_Color = Color.clear;
        [SerializeField] private Color m_BackgroundColor = Color.clear;
        [SerializeField] private int m_FontSize = 0;
        [SerializeField] private FontStyle m_FontStyle = FontStyle.Normal;
        [SerializeField] private float m_LineSpacing = 1f;
        [SerializeField] private TextAnchor m_Alignment = TextAnchor.MiddleCenter;
#if dUI_TextMeshPro
        [SerializeField] private TMP_FontAsset m_TMPFont;
        [SerializeField] private FontStyles m_TMPFontStyle = FontStyles.Normal;
        [SerializeField] private TextAlignmentOptions m_TMPAlignment = TextAlignmentOptions.Left;
#endif
        /// <summary>
        /// Rotation of text.
        /// 文本的旋转。
        /// [default: `0f`]
        /// </summary>
        public float rotate
        {
            get { return m_Rotate; }
            set { if (PropertyUtil.SetStruct(ref m_Rotate, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the offset of position.
        /// 坐标偏移。
        /// [Default: `Vector2.zero`]
        /// </summary>
        public Vector2 offset
        {
            get { return m_Offset; }
            set { if (PropertyUtil.SetStruct(ref m_Offset, value)) SetComponentDirty(); }
        }

        public Vector3 offsetv3 { get { return new Vector3(m_Offset.x, m_Offset.y, 0); } }

        /// <summary>
        /// the color of text. 
        /// 文本的颜色。
        /// [default: `Color.clear`]
        /// </summary>
        public Color color
        {
            get { return m_Color; }
            set { if (PropertyUtil.SetColor(ref m_Color, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color of text. 
        /// 文本的背景颜色。
        /// [default: `Color.clear`]
        /// </summary>
        public Color backgroundColor
        {
            get { return m_BackgroundColor; }
            set { if (PropertyUtil.SetColor(ref m_BackgroundColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the font of text. When `null`, the theme's font is used by default.
        /// 文本字体。
        /// [default: null]
        /// </summary>
        public Font font
        {
            get { return m_Font; }
            set { if (PropertyUtil.SetClass(ref m_Font, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// font size.
        /// 文本字体大小。
        /// [default: 18]
        /// </summary>
        public int fontSize
        {
            get { return m_FontSize; }
            set { if (PropertyUtil.SetStruct(ref m_FontSize, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// font style.
        /// 文本字体的风格。
        /// [default: FontStyle.Normal]
        /// </summary>
        public FontStyle fontStyle
        {
            get { return m_FontStyle; }
            set { if (PropertyUtil.SetStruct(ref m_FontStyle, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// text line spacing.
        /// 行间距。
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
        public TMP_FontAsset tmpFont
        {
            get { return m_TMPFont; }
            set { if (PropertyUtil.SetClass(ref m_TMPFont, value)) SetComponentDirty(); }
        }

        public FontStyles tmpFontStyle
        {
            get { return m_TMPFontStyle; }
            set { if (PropertyUtil.SetStruct(ref m_TMPFontStyle, value)) SetComponentDirty(); }
        }
        public TextAlignmentOptions tmpAlignment
        {
            get { return m_TMPAlignment; }
            set { if (PropertyUtil.SetStruct(ref m_TMPAlignment, value)) SetComponentDirty(); }
        }
#endif

        public TextStyle()
        {
        }

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
            offset = textStyle.offset;
            color = textStyle.color;
            backgroundColor = textStyle.backgroundColor;
            fontSize = textStyle.fontSize;
            fontStyle = textStyle.fontStyle;
            lineSpacing = textStyle.lineSpacing;
            alignment = textStyle.alignment;
            autoWrap = textStyle.autoWrap;
            autoAlign = textStyle.autoAlign;
#if dUI_TextMeshPro
            m_TMPFont = textStyle.tmpFont;
            m_TMPAlignment = textStyle.tmpAlignment;
            m_TMPFontStyle = textStyle.tmpFontStyle;
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
            if (ChartHelper.IsClearColor(color)) return defaultColor;
            else return color;
        }

        public int GetFontSize(ComponentTheme defaultTheme)
        {
            if (fontSize == 0) return defaultTheme.fontSize;
            else return fontSize;
        }

        public TextAnchor GetAlignment(TextAnchor systemAlignment)
        {
            return m_AutoAlign ? systemAlignment : alignment;
        }
    }
}