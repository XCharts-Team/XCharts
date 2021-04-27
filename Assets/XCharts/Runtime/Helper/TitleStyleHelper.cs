/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/
using UnityEngine;
using UnityEngine.UI;

namespace XCharts
{
    public static class TitleStyleHelper
    {
        public static void CheckTitle(Serie serie, ref bool m_ReinitTitle, ref bool m_UpdateTitleText)
        {
            if (serie.titleStyle.show)
            {
                if (serie.titleStyle.IsInited())
                {
                    serie.titleStyle.UpdatePosition(serie.runtimeCenterPos);
                    m_UpdateTitleText = true;
                }
                else
                {
                    m_ReinitTitle = true;
                }
            }
        }

        public static void UpdateTitleText(Series series)
        {
            foreach (var serie in series.list) UpdateTitleText(serie);
        }

        public static void UpdateTitleText(Serie serie)
        {
            if (serie.titleStyle.show)
            {
                serie.titleStyle.SetText(serie.name);
            }
        }
    }
}