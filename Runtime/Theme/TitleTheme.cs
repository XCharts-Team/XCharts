using System;

namespace XCharts.Runtime
{
    [Serializable]
    public class TitleTheme : ComponentTheme
    {
        public TitleTheme(ThemeType theme) : base(theme)
        {
            m_FontSize = XCSettings.fontSizeLv1;
            switch (theme)
            {
                case ThemeType.Default:
                    m_TextColor = ColorUtil.GetColor("#514D4D");
                    break;
                case ThemeType.Light:
                    break;
                case ThemeType.Dark:
                    m_TextColor = ColorUtil.GetColor("#EEF1FA");
                    break;
            }
        }
    }
}