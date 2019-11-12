using System.Threading;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;

namespace XCharts
{
    /// <summary>
    /// Settings related to text.
    /// 文本的相关设置。
    /// </summary>
    [Serializable]
    public class TextStyle : SubComponent, IEquatable<TextStyle>
    {
        [SerializeField] private float m_Rotate = 0;
        [SerializeField] private Vector2 m_Offset = Vector2.zero;
        [SerializeField] private Color m_Color = Color.clear;
        [SerializeField] private int m_FontSize = 18;
        [SerializeField] private FontStyle m_FontStyle = FontStyle.Normal;

        /// <summary>
        /// Rotation of text.
        /// 文本的旋转。
        /// </summary>
        public float rotate { get { return m_Rotate; } set { m_Rotate = value; } }
        /// <summary>
        /// the offset of position.
        /// 坐标偏移。
        /// </summary>
        public Vector2 offset { get { return m_Offset; } set { m_Offset = value; } }

        /// <summary>
        /// the color of text. 
        /// 文本的颜色。
        /// </summary>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// font size.
        /// 文本字体大小。
        /// </summary>
        public int fontSize { get { return m_FontSize; } set { m_FontSize = value; } }
        /// <summary>
        /// font style.
        /// 文本字体的风格。
        /// </summary>
        public FontStyle fontStyle { get { return m_FontStyle; } set { m_FontStyle = value; } }

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

        public TextStyle Clone()
        {
            var textStyle = new TextStyle();
            textStyle.rotate = rotate;
            textStyle.color = color;
            textStyle.fontSize = fontSize;
            textStyle.fontStyle = fontStyle;
            textStyle.offset = offset;
            return textStyle;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            else if (obj is TextStyle)
            {
                return Equals((TextStyle)obj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(TextStyle other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            return rotate == other.rotate &&
                fontSize == other.fontSize &&
                fontStyle == other.fontStyle &&
                offset == other.offset &&
                ChartHelper.IsValueEqualsColor(m_Color, other.color);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}