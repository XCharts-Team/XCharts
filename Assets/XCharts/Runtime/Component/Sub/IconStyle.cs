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
    /// <summary>
    /// 系列数据项的图标
    /// </summary>
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
        [SerializeField] private Sprite m_Sprite;
        [SerializeField] private Color m_Color = Color.white;
        [SerializeField] private float m_Width = 40;
        [SerializeField] private float m_Height = 40;
        [SerializeField] private Vector3 m_Offset;

        /// <summary>
        /// Whether the data icon is show.
        /// 是否显示图标。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; UpdateIcon(); } }
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

        public Image image { get; private set; }
        public RectTransform rect { get; private set; }

        public void SetImage(Image image)
        {
            this.image = image;
            if (image)
            {
                rect = image.GetComponent<RectTransform>();
                if (m_Layer == Layer.UnderLabel)
                    rect.SetSiblingIndex(0);
                else
                    rect.SetSiblingIndex(image.transform.childCount - 1);
                UpdateIcon();
            }
        }

        public void SetActive(bool flag)
        {
            if (image)
            {
                ChartHelper.SetActive(image.gameObject, flag);
            }
        }

        public void UpdateIcon()
        {
            if (image == null) return;
            if (show)
            {
                ChartHelper.SetActive(image.gameObject, true);
                image.sprite = m_Sprite;
                image.color = m_Color;
                rect.sizeDelta = new Vector2(m_Width, m_Height);
                image.transform.localPosition = m_Offset;
            }
            else
            {
                ChartHelper.SetActive(image.gameObject, false);
            }
        }
    }
}
