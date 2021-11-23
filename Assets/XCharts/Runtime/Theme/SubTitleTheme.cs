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