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

        public GaugeAxisTheme(Theme theme) : base(theme)
        {
            m_LineWidth = XChartsSettings.gaugeAxisLineWidth;
            m_LineLength = 0;
            m_SplitLineWidth = XChartsSettings.gaugeAxisSplitLineWidth;
            m_SplitLineLength = XChartsSettings.gaugeAxisSplitLineLength;
            m_TickWidth = XChartsSettings.gaugeAxisTickWidth;
            m_TickLength = XChartsSettings.gaugeAxisTickLength;
            m_SplitLineColor = Color.white;
            m_TickColor = Color.white;
            switch (theme)
            {
                case Theme.Default:
                    m_BarBackgroundColor = new Color32(200, 200, 200, 255);
                    m_StageColor = new List<StageColor>()
                    {
                        new StageColor(0.2f,new Color32(145,199,174,255)),
                        new StageColor(0.8f,new Color32(99,134,158,255)),
                        new StageColor(1.0f,new Color32(194,53,49,255)),
                    };
                    break;
                case Theme.Light:
                    m_BarBackgroundColor = new Color32(200, 200, 200, 255);
                    m_StageColor = new List<StageColor>()
                    {
                        new StageColor(0.2f,new Color32(145,199,174,255)),
                        new StageColor(0.8f,new Color32(99,134,158,255)),
                        new StageColor(1.0f,new Color32(194,53,49,255)),
                    };
                    break;
                case Theme.Dark:
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