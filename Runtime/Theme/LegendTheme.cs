using System;
using UnityEngine;
using UnityEngine.Serialization;
#if dUI_TextMeshPro
using TMPro;
#endif

namespace XCharts.Runtime
{
    [Serializable]
    public class LegendTheme : ComponentTheme
    {
        [SerializeField][FormerlySerializedAs("m_UnableColor")] protected Color m_InactiveColor;

        /// <summary>
        /// the color of text.
        /// ||文本颜色。
        /// </summary>
        [Obsolete("Use inactiveColor instead.", true)]
        public Color unableColor
        {
            get { return m_InactiveColor; }
            set { if (PropertyUtil.SetColor(ref m_InactiveColor, value)) SetComponentDirty(); }
        }
        /// <summary>
        /// the color when the component is inactive.
        /// ||非激活状态时的颜色。
        /// </summary>
        public Color inactiveColor
        {
            get { return m_InactiveColor; }
            set { if (PropertyUtil.SetColor(ref m_InactiveColor, value)) SetComponentDirty(); }
        }

        public void Copy(LegendTheme theme)
        {
            base.Copy(theme);
            m_InactiveColor = theme.inactiveColor;
        }

        public LegendTheme(ThemeType theme) : base(theme)
        {
            m_InactiveColor = ColorUtil.GetColor("#cccccc");

        }
    }
}