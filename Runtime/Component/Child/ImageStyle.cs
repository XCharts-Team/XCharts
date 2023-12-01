using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [System.Serializable]
    public class ImageStyle : ChildComponent, ISerieComponent, ISerieDataComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Sprite m_Sprite;
        [SerializeField] private Image.Type m_Type;
        [SerializeField] private bool m_AutoColor;
        [SerializeField] private Color m_Color = Color.clear;
        [SerializeField] private float m_Width = 0;
        [SerializeField] private float m_Height = 0;

        public void Reset()
        {
            m_Show = false;
            m_Type = Image.Type.Simple;
            m_Sprite = null;
            m_AutoColor = false;
            m_Color = Color.white;
            m_Width = 0;
            m_Height = 0;
        }

        /// <summary>
        /// Whether the data icon is show.
        /// ||是否显示图标。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// The image of icon.
        /// ||图标的图片。
        /// </summary>
        public Sprite sprite { get { return m_Sprite; } set { m_Sprite = value; } }
        /// <summary>
        /// How to display the image.
        /// ||图片的显示类型。
        /// </summary>
        public Image.Type type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// 是否自动颜色。
        /// </summary>
        public bool autoColor { get { return m_AutoColor; } set { m_AutoColor = value; } }
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
        public ImageStyle Clone()
        {
            var imageStyle = new ImageStyle();
            imageStyle.type = type;
            imageStyle.sprite = sprite;
            imageStyle.autoColor = autoColor;
            imageStyle.color = color;
            imageStyle.width = width;
            imageStyle.height = height;
            return imageStyle;
        }

        public void Copy(ImageStyle imageStyle)
        {
            type = imageStyle.type;
            sprite = imageStyle.sprite;
            autoColor = imageStyle.autoColor;
            color = imageStyle.color;
            width = imageStyle.width;
            height = imageStyle.height;
        }
    }
}