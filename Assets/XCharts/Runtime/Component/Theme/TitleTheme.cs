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
        public TitleTheme(Theme theme) : base(theme)
        {
            m_FontSize = XChartsSettings.fontSizeLv1;
            switch (theme)
            {
                case Theme.Default:
                    m_TextColor = ColorUtil.GetColor("#514D4D");
                    break;
                case Theme.Light:
                    break;
                case Theme.Dark:
                    m_TextColor = ColorUtil.GetColor("#EEF1FA");
                    break;
            }
        }
    }
}