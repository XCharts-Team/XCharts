using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// Background component.
    /// ||背景组件。
    /// </summary>
    [Serializable]
    [DisallowMultipleComponent]
    [ComponentHandler(typeof(BackgroundHandler), false, 0)]
    public class Background : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Sprite m_Image;
        [SerializeField] private Image.Type m_ImageType;
        [SerializeField] private Color m_ImageColor = Color.white;
        [SerializeField][Since("v3.10.0")] private float m_ImageWidth = 0;
        [SerializeField][Since("v3.10.0")] private float m_ImageHeight = 0;
        [SerializeField] private bool m_AutoColor = true;
        [SerializeField][Since("v3.10.0")] private BorderStyle m_BorderStyle = new BorderStyle();

        /// <summary>
        /// Whether to enable the background component.
        /// ||是否启用背景组件。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the image of background.
        /// ||背景图。
        /// </summary>
        public Sprite image
        {
            get { return m_Image; }
            set { if (PropertyUtil.SetClass(ref m_Image, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the fill type of background image.
        /// ||背景图填充类型。
        /// </summary>
        public Image.Type imageType
        {
            get { return m_ImageType; }
            set { if (PropertyUtil.SetStruct(ref m_ImageType, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// 背景图颜色。
        /// </summary>
        public Color imageColor
        {
            get { return m_ImageColor; }
            set { if (PropertyUtil.SetColor(ref m_ImageColor, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the width of background image.
        /// ||背景图宽度。
        /// </summary>
        public float imageWidth
        {
            get { return m_ImageWidth; }
            set { if (PropertyUtil.SetStruct(ref m_ImageWidth, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the height of background image.
        /// ||背景图高度。
        /// </summary>
        public float imageHeight
        {
            get { return m_ImageHeight; }
            set { if (PropertyUtil.SetStruct(ref m_ImageHeight, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// Whether to use theme background color for component color when the background component is on.
        /// ||当background组件开启时，是否自动使用主题背景色作为backgrounnd组件的颜色。当设置为false时，用imageColor作为颜色。
        /// </summary>
        public bool autoColor
        {
            get { return m_AutoColor; }
            set { if (PropertyUtil.SetStruct(ref m_AutoColor, value)) SetVerticesDirty(); }
        }

        /// <summary>
        /// the border style of background.        
        /// ||背景边框样式。
        /// </summary>
        public BorderStyle borderStyle
        {
            get { return m_BorderStyle; }
            set { if (PropertyUtil.SetClass(ref m_BorderStyle, value)) SetComponentDirty(); }
        }

        public override void SetDefaultValue()
        {
            m_Show = true;
            m_Image = null;
            m_ImageType = Image.Type.Sliced;
            m_ImageColor = Color.white;
            m_AutoColor = true;
        }
    }
}