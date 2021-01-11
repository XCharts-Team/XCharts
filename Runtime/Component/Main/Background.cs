using System.Net.Mime;
/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    /// <summary>
    /// Background component.
    /// Due to the limitations of the framework, there are two limitations to the use of background component:
    /// 1: The parent node of chart cannot have a layout control class component.
    /// 2: The parent node of Chart can only have one child node of the current chart.
    /// 
    /// 背景组件。
    /// 由于框架的局限性，背景组件使用有以下两个限制：
    /// 1：chart的父节点不能有布局控制类组件。
    /// 2：chart的父节点只能有当前chart一个子节点。
    /// 背景组件的开启需要通过接口来开启：BaseChart.EnableBackground(bool flag)
    /// </summary>
    [Serializable]
    public class Background : MainComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Sprite m_Image;
        [SerializeField] private Image.Type m_ImageType;
        [SerializeField] private Color m_ImageColor = Color.white;
        [SerializeField] private bool m_HideThemeBackgroundColor = true;

        /// <summary>
        /// Whether to enable the background component. However, 
        /// the ability to activate the background component is subject to other conditions.
        /// 是否启用背景组件。但能否激活背景组件还要受其他条件限制。
        /// </summary>
        public bool show
        {
            get { return m_Show; }
            internal set { if (PropertyUtil.SetStruct(ref m_Show, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the image of background.
        /// 背景图。
        /// </summary>
        public Sprite image
        {
            get { return m_Image; }
            set { if (PropertyUtil.SetClass(ref m_Image, value)) SetComponentDirty(); }
        }

        /// <summary>
        /// the fill type of background image.
        /// 背景图填充类型。
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
        /// Whether to hide the background color set in the theme when the background component is on.
        /// 当background组件开启时，是否隐藏主题中设置的背景色。
        /// </summary>
        public bool hideThemeBackgroundColor
        {
            get { return m_HideThemeBackgroundColor; }
            set { if (PropertyUtil.SetStruct(ref m_HideThemeBackgroundColor, value)) SetVerticesDirty(); }
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
                    m_ImageColor = Color.white,
                    m_HideThemeBackgroundColor = true,
                };
                return background;
            }
        }
    }
}