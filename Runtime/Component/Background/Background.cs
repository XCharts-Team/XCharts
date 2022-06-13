using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    /// <summary>
    /// Background component.
    /// |
    /// 背景组件。
    /// </summary>
    [Serializable]
    [DisallowMultipleComponent]
    [ComponentHandler(typeof(BackgroundHandler), false)]
    public class Background : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Sprite m_Image;
        [SerializeField] private Image.Type m_ImageType;
        [SerializeField] private Color m_ImageColor = Color.white;
        [SerializeField] private bool m_AutoColor = true;

        /// <summary>
        /// Whether to enable the background component.
        /// |是否启用背景组件。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            internal set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the image of background.
        /// |背景图。
        /// </summary>
        public Sprite image
        {
            get { return m_Image; }
            set { if (PropertyUtil.SetClass(ref m_Image, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the fill type of background image.
        /// |背景图填充类型。
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
        /// Whether to use theme background color for component color when the background component is on.
        /// |当background组件开启时，是否自动使用主题背景色作为backgrounnd组件的颜色。当设置为false时，用imageColor作为颜色。
        /// </summary>
        public bool autoColor
        {
            get { return m_AutoColor; }
            set { if (PropertyUtil.SetStruct(ref m_AutoColor, value)) SetVerticesDirty(); }
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