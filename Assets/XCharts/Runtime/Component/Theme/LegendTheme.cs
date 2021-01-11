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
    [Serializable]
    public class LegendTheme : ComponentTheme
    {
        [SerializeField] protected Color m_UnableColor;

        /// <summary>
        /// the color of text.
        /// 文本颜色。
        /// </summary>
        public Color unableColor
        {
            get { return m_UnableColor; }
            set { if (PropertyUtil.SetColor(ref m_UnableColor, value)) SetComponentDirty(); }
        }

        public void Copy(LegendTheme theme)
        {
            base.Copy(theme);
            m_UnableColor = theme.unableColor;
        }

        public LegendTheme(Theme theme) : base(theme)
        {
            m_UnableColor = ColorUtil.GetColor("#cccccc");
            
        }
    }
}