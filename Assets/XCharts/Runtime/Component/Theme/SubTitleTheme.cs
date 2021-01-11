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
        public SubTitleTheme(Theme theme) : base(theme)
        {
            m_FontSize = XChartsSettings.fontSizeLv2;
            switch (theme)
            {
                case Theme.Default:
                    m_TextColor = ColorUtil.GetColor("#969696");
                    break;
                case Theme.Light:
                    m_TextColor = ColorUtil.GetColor("#969696");
                    break;
                case Theme.Dark:
                    m_TextColor = ColorUtil.GetColor("#B9B8CE");
                    break;
            }
        }
    }
}