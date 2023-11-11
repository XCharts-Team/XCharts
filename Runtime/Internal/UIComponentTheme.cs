using System;
using UnityEngine;
using UnityEngine.UI;

namespace XCharts.Runtime
{
    [Serializable]
    public class UIComponentTheme : ChildComponent
    {
        [SerializeField] private bool m_Show = true;
        [SerializeField] private Theme m_SharedTheme;
        [SerializeField] private bool m_TransparentBackground = false;

        public bool show { get { return m_Show; } }
        /// <summary>
        /// the theme of chart.
        /// ||主题类型。
        /// </summary>
        public ThemeType themeType
        {
            get { return sharedTheme.themeType; }
        }
        /// <summary>
        /// theme name.
        /// ||主题名字。
        /// </summary>
        public string themeName
        {
            get { return sharedTheme.themeName; }
        }
        /// <summary>
        /// the asset of theme.
        /// ||主题配置。
        /// </summary>
        public Theme sharedTheme
        {
            get { return m_SharedTheme; }
            set { m_SharedTheme = value; SetAllDirty(); }
        }
        /// <summary>
        /// the background color of chart.
        /// ||背景颜色。
        /// </summary>
        public Color32 backgroundColor
        {
            get
            {
                if (m_TransparentBackground) return ColorUtil.clearColor32;
                else if (sharedTheme != null) return sharedTheme.backgroundColor;
                else return ColorUtil.clearColor32;
            }
        }
    }
}