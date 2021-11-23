/************************************************/
/*                                              */
/*     Copyright (c) 2018 - 2021 monitor1394    */
/*     https://github.com/monitor1394           */
/*                                              */
/************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;

namespace XCharts
{
    [Serializable]
    public class GaugeAxisTheme : BaseAxisTheme
    {
        [SerializeField] private Color32 m_BarBackgroundColor;
        [SerializeField]
        private List<StageColor> m_StageColor = new List<StageColor>()
        {
            new StageColor(0.2f,new Color32(145,199,174,255)),
            new StageColor(0.8f,new Color32(99,134,158,255)),
            new StageColor(1.0f,new Color32(194,53,49,255)),
        };
        /// <summary>
        /// 进度条背景颜色。
        /// </summary>
        public Color32 barBackgroundColor { get { return m_BarBackgroundColor; } set { m_BarBackgroundColor = value; } }
        /// <summary>
        /// 阶段颜色。
        /// </summary>
        public List<StageColor> stageColor { get { return m_StageColor; } set { m_StageColor = value; } }

        public GaugeAxisTheme(ThemeType theme) : base(theme)
        {
            m_LineWidth = XCSettings.gaugeAxisLineWidth;
            m_LineLength = 0;
            m_SplitLineWidth = XCSettings.gaugeAxisSplitLineWidth;
            m_SplitLineLength = XCSettings.gaugeAxisSplitLineLength;
            m_TickWidth = XCSettings.gaugeAxisTickWidth;
            m_TickLength = XCSettings.gaugeAxisTickLength;
            m_SplitLineColor = Color.white;
            m_TickColor = Color.white;
            switch (theme)
            {
                case ThemeType.Default:
                    m_BarBackgroundColor = new Color32(200, 200, 200, 255);
                    m_StageColor = new List<StageColor>()
                    {
                        new StageColor(0.2f,new Color32(145,199,174,255)),
                        new StageColor(0.8f,new Color32(99,134,158,255)),
                        new StageColor(1.0f,new Color32(194,53,49,255)),
                    };
                    break;
                case ThemeType.Light:
                    m_BarBackgroundColor = new Color32(200, 200, 200, 255);
                    m_StageColor = new List<StageColor>()
                    {
                        new StageColor(0.2f,new Color32(145,199,174,255)),
                        new StageColor(0.8f,new Color32(99,134,158,255)),
                        new StageColor(1.0f,new Color32(194,53,49,255)),
                    };
                    break;
                case ThemeType.Dark:
                    m_BarBackgroundColor = new Color32(200, 200, 200, 255);
                    m_StageColor = new List<StageColor>()
                    {
                        new StageColor(0.2f,new Color32(145,199,174,255)),
                        new StageColor(0.8f,new Color32(99,134,158,255)),
                        new StageColor(1.0f,new Color32(194,53,49,255)),
                    };
                    break;
            }
        }

        public void Copy(GaugeAxisTheme theme)
        {
            base.Copy(theme);
            m_BarBackgroundColor = theme.barBackgroundColor;
            ChartHelper.CopyList(m_StageColor, theme.stageColor);
        }
    }
}