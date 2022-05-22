using System;

namespace XCharts.Runtime
{
    [Serializable]
    public class SubTitleTheme : ComponentTheme
    {
        public SubTitleTheme(ThemeType theme) : base(theme)
        {
            m_FontSize = XCSettings.fontSizeLv2;
            switch (theme)
            {
                case ThemeType.Default:
                    m_TextColor = ColorUtil.GetColor("#969696");
                    break;
                case ThemeType.Light:
                    m_TextColor = ColorUtil.GetColor("#969696");
                    break;
                case ThemeType.Dark:
                    m_TextColor = ColorUtil.GetColor("#B9B8CE");
                    break;
            }
        }
    }
}