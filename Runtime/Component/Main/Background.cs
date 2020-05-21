using System.Net.Mime;
/******************************************/
/*                                        */
/*     Copyright (c) 2018 monitor1394     */
/*     https://github.com/monitor1394     */
/*                                        */
/******************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// 背景组件。
    /// 由于框架的局限性，背景组件在chart受上层布局控制时不适用。因为背景组件节点和chart节点是同一级的。
    /// 自动布局下的一种解决方案是，可以将chart节点再包一层parent。
    /// 要处理这个问题底层框架要大改了，目前暂时不打算改。
    /// 背景组件的开启需要通过接口来开启：BaseChart.EnableBackground(bool flag)
    /// </summary>
    [Serializable]
    public class Background : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Sprite m_Image;
        [SerializeField] private Image.Type m_ImageType;
        [SerializeField] private float m_Left;
        [SerializeField] private float m_Right;
        [SerializeField] private float m_Top;
        [SerializeField] private float m_Bottom;
        [SerializeField] private Color m_ImageColor = Color.white;
        [SerializeField] private bool m_HideThemeBackgroundColor = true;

        /// <summary>
        /// 是否启用背景组件。注意背景组件在chart受上层布局控制时不适用。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            internal set { if (PropertyUtility.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 背景图。
        /// </summary>
        public Sprite image
        {
            get { return m_Image; }
            set { if (PropertyUtility.SetClass(ref m_Image, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// 背景图填充类型。
        /// </summary>
        public Image.Type imageType
        {
            get { return m_ImageType; }
            set { if (PropertyUtility.SetStruct(ref m_ImageType, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// Distance between background component and the left side of the container.
        /// background 组件离容器左侧的距离。
        /// </summary>
        public float left
        {
            get { return m_Left; }
            set { if (PropertyUtility.SetStruct(ref m_Left, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Distance between background component and the right side of the container.
        /// background 组件离容器右侧的距离。
        /// </summary>
        public float right
        {
            get { return m_Right; }
            set { if (PropertyUtility.SetStruct(ref m_Right, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Distance between background component and the top side of the container.
        /// background 组件离容器上侧的距离。
        /// </summary>
        public float top
        {
            get { return m_Top; }
            set { if (PropertyUtility.SetStruct(ref m_Top, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// Distance between background component and the bottom side of the container.
        /// background 组件离容器下侧的距离。
        /// </summary>
        public float bottom
        {
            get { return m_Bottom; }
            set { if (PropertyUtility.SetStruct(ref m_Bottom, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// 背景图颜色。
        /// </summary>
        public Color imageColor
        {
            get { return m_ImageColor; }
            set { if (PropertyUtility.SetColor(ref m_ImageColor, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// 当background组件开启时，是否隐藏主题中设置的背景色。
        /// </summary>
        public bool hideThemeBackgroundColor
        {
            get { return m_HideThemeBackgroundColor; }
            set { if (PropertyUtility.SetStruct(ref m_HideThemeBackgroundColor, value)) SetVerticesDirty(); }
        }

        public static Background defaultBackground
        {
            get
            {
                var background = new Background
                {
                    m_Show = false,
                    m_Image = null,
                    m_ImageType = Image.Type.Sliced,
                    m_Left = 0,
                    m_Right = 0,
                    m_Top = 0,
                    m_Bottom = 0,
                    m_ImageColor = Color.white,
                    m_HideThemeBackgroundColor = true,
                };
                return background;
            }
        }
    }
}