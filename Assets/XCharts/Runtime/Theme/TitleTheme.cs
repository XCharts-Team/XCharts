/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;

namespace XCharts
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