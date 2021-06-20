/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    [System.Serializable]
    public class IconStyle : SubComponent
    {
        public enum Layer
        {
            UnderLabel,
            AboveLabel
        }
        [SerializeField] private bool m_Show;
        [SerializeField] private Layer m_Layer;
        [SerializeField] private Align m_Align = Align.Left;
        [SerializeField] private Sprite m_Sprite;
        [SerializeField] private Color m_Color = Color.white;
        [SerializeField] private float m_Width = 20;
        [SerializeField] private float m_Height = 20;
        [SerializeField] private Vector3 m_Offset;
        [SerializeField] private bool m_AutoHideWhenLabelEmpty = false;

        public void Reset()
        {
            m_Show = false;
            m_Layer = Layer.UnderLabel;
            m_Sprite = null;
            m_Color = Color.white;
            m_Width = 20;
            m_Height = 20;
            m_Offset = Vector3.zero;
            m_AutoHideWhenLabelEmpty = false;
        }
        /// <summary>
        /// Whether the data icon is show.
        /// 是否显示图标。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// 显示在上层还是在下层。
        /// </summary>
        public Layer layer { get { return m_Layer; } set { m_Layer = value; } }
        /// <summary>
        /// The image of icon.
        /// 图标的图片。
        /// </summary>
        public Sprite sprite { get { return m_Sprite; } set { m_Sprite = value; } }
        /// <summary>
        /// 图标颜色。
        /// </summary>
        public Color color { get { return m_Color; } set { m_Color = value; } }
        /// <summary>
        /// 图标宽。
        /// </summary>
        public float width { get { return m_Width; } set { m_Width = value; } }
        /// <summary>
        /// 图标高。
        /// </summary>
        public float height { get { return m_Height; } set { m_Height = value; } }
        /// <summary>
        /// 图标偏移。
        /// </summary>
        public Vector3 offset { get { return m_Offset; } set { m_Offset = value; } }
        /// <summary>
        /// 水平方向对齐方式。
        /// </summary>
        public Align align { get { return m_Align; } set { m_Align = value; } }
        /// <summary>
        /// 当label内容为空时是否自动隐藏图标
        /// </summary>
        public bool autoHideWhenLabelEmpty { get { return m_AutoHideWhenLabelEmpty; } set { m_AutoHideWhenLabelEmpty = value; } }
        public IconStyle Clone()
        {
            var iconStyle = new IconStyle();
            iconStyle.show = show;
            iconStyle.layer = layer;
            iconStyle.sprite = sprite;
            iconStyle.color = color;
            iconStyle.width = width;
            iconStyle.height = height;
            iconStyle.offset = offset;
            iconStyle.align = align;
            iconStyle.autoHideWhenLabelEmpty = autoHideWhenLabelEmpty;
            return iconStyle;
        }

        public void Copy(IconStyle iconStyle)
        {
            show = iconStyle.show;
            layer = iconStyle.layer;
            sprite = iconStyle.sprite;
            color = iconStyle.color;
            width = iconStyle.width;
            height = iconStyle.height;
            offset = iconStyle.offset;
            align = iconStyle.align;
            autoHideWhenLabelEmpty = iconStyle.autoHideWhenLabelEmpty;
        }
    }
}
